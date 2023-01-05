/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

using Markdig;

namespace WinFormsDemoApp {
  [ToolboxItem(true)]
  public class GoWebBrowser : WebView2 {
    private bool _Initialized;
    private string _Html;
    private static HttpClient _HttpClient = new();
    private static Dictionary<string, string> _ApiMap = null;
    private static MatchEvaluator _DocsMatchEvaluator = new(_ReplaceApiLinks);
    private static MatchEvaluator _DocsMdMatchEvaluator = new(_ReplaceMdApiLinks);

    public GoWebBrowser() {
      NavigationCompleted += GoWebBrowser_NavigationCompleted;
    }

    public async void InitializeAsync() {
      var userDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\GoWinForms";
      var env = await CoreWebView2Environment.CreateAsync(null, userDataFolder);
      await EnsureCoreWebView2Async(env);
      _Initialized = true;
      if (_Html != null) NavigateToString(_Html);
    }

    private void GoWebBrowser_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e) {
      if (e.IsSuccess) {
        ((WebView2)sender).ExecuteScriptAsync("var body = document.querySelector('body'); body.style.overflow='scroll'; body.style.font='9pt Segoe UI'; var style=document.createElement('style');style.type='text/css';style.innerHTML='::-webkit-scrollbar{display:none}; font-size: 9pt; line-height: 1.5; letter-spacing: 0;';document.getElementsByTagName('body')[0].appendChild(style)");
      }
    }

    /// <summary>
    /// Gets or sets the Markdown string for this webview.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string MdText {
      get { return _Html; }
      set {
        // preprocess HTML before setting
        var val = PreprocessMd(value);
        _Html = Markdown.ToHtml(val);
        if (_Initialized) NavigateToString(_Html);
      }
    }

    /// <summary>
    /// Gets or sets the HTML string for this webview.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string Html {
      get { return _Html; }
      set {
        // preprocess HTML before setting
        var val = PreprocessHtml(value);
        _Html = val;
        if (_Initialized) NavigateToString(_Html);
      }
    }

    /// <summary>
    /// Gets the map of API names to URLs.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public static Dictionary<string, string> ApiMap {
      get { return _ApiMap; }
    }

    private static string PreprocessMd(string mdText) {
      if (mdText == null) return "";

      // process [abc] links for API docs
      var rx = new Regex(@"\[(.*?)\](?!\()", RegexOptions.Multiline);
      mdText = rx.Replace(mdText, _DocsMdMatchEvaluator);

      // process [abc](learn/xyz) links
      rx = new Regex(@"\[(.*)\]\(learn/(.*)\)", RegexOptions.Multiline);
      mdText = rx.Replace(mdText, @"[$1](https://godiagram.com/winforms/latest/learn/$2)");

      // process [abc](intro/xyz) links
      rx = new Regex(@"\[(.*)\]\(intro/(.*)\)", RegexOptions.Multiline);
      mdText = rx.Replace(mdText, @"[$1](https://godiagram.com/winforms/latest/intro/$2)");

      // process [abc](demo/xyz) links
      rx = new Regex(@"\[(.*)\]\(demo/(.*)\)", RegexOptions.Multiline);
      mdText = rx.Replace(mdText, @"[$1](https://demo/$2)");  // cheat and pretend it's a web link to avoid about:blank#blocked

      return mdText;
    }

    /// <summary>
    /// Process the provided HTML string for proper links.
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    private static string PreprocessHtml(string html) {
      if (html == null) return "";

      // process <a> links for API docs
      var rx = new Regex(@"<a>(.*?)</a>", RegexOptions.Multiline);
      html = rx.Replace(html, _DocsMatchEvaluator);

      // process [] links for API docs
      rx = new Regex(@"\[(.*?)\]", RegexOptions.Multiline);
      html = rx.Replace(html, _DocsMatchEvaluator);

      // process <a href="learn/<page>"> links
      rx = new Regex(@"<a href=""learn/(.*)"">", RegexOptions.Multiline);
      html = rx.Replace(html, @"<a href=""https://godiagram.com/winforms/latest/learn/$1"">");

      // process <a href="intro/<page>"> links
      rx = new Regex(@"<a href=""intro/(.*)"">", RegexOptions.Multiline);
      html = rx.Replace(html, @"<a href=""https://godiagram.com/winforms/latest/intro/$1"">");

      // process <a href="<demo>"> links
      rx = new Regex(@"<a href=""demo/(\w+)"">", RegexOptions.Multiline);
      html = rx.Replace(html, @"<a href=""https://demo/$1"">");  // cheat and pretend it's a web link to avoid about:blank#blocked

      return html;
    }

    /// <summary>
    /// Used as a delegate for regex replacement of [xyz] API references.
    /// </summary>
    /// <param name="m"></param>
    /// <returns></returns>
    private static string _ReplaceMdApiLinks(Match m) {
      var str = m.Groups[1].Value;
      if (_ApiMap != null && _ApiMap.TryGetValue(str, out var url))
        return $"[{str}](https://godiagram.com/winforms/latest/{url})";
      return m.Value;
    }

    /// <summary>
    /// Used as a delegate for regex replacement of <a> API references.
    /// </summary>
    /// <param name="m"></param>
    /// <returns></returns>
    private static string _ReplaceApiLinks(Match m) {
      var str = m.Groups[1].Value;
      if (_ApiMap != null && _ApiMap.TryGetValue(str, out var url))
        return @"<a href=""https://godiagram.com/winforms/latest/" + url + @""">" + str + "</a>";
      return m.Value;
    }

    /// <summary>
    /// Initialize the API map if it hasn't been already.
    /// </summary>
    /// <returns></returns>
    public static async Task _InitApiMap() {
      if (_ApiMap == null) {
        _ApiMap = await GetApiMap();
      }
    }

    /// <summary>
    /// This method is responsible for fetching the map of short names to API URLs on godiagram.com.
    /// </summary>
    /// <returns></returns>
    static async Task<Dictionary<string, string>> GetApiMap() {
      var url = "https://godiagram.com/winforms/latest/api/apiMap.js";
      using var response = await _HttpClient.GetAsync(url);
      if (response.IsSuccessStatusCode) {
        using var stream = await response.Content.ReadAsStreamAsync();
        var reader = new StreamReader(stream);
        var text = reader.ReadToEnd();
        text = text.Substring(text.IndexOf('{'));
        return JsonSerializer.Deserialize<Dictionary<string, string>>(text);
      }
      return null;
    }
  }
}
