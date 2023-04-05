using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaDemoApp.Views {
  public partial class DescriptionView : UserControl {
    private static HttpClient _HttpClient = new();
    private static Dictionary<string, string> _ApiMap = null;
    private static MatchEvaluator _DocsMatchEvaluator = new(_ReplaceApiLinks);

    public DescriptionView() {
      InitializeComponent();
    }

    private void InitializeComponent() {
      AvaloniaXamlLoader.Load(this);
    }

    public static readonly StyledProperty<string> MdTextProperty =
      AvaloniaProperty.Register<DescriptionView, string>(nameof(MdText), "Sample Description");

    public string MdText {
      get { return GetValue(MdTextProperty); }
      set {
        var val = PreprocessMd(value);
        SetValue(MdTextProperty, val);
      }
    }

    /// <summary>
    /// Gets the map of API names to URLs.
    /// </summary>
    public static Dictionary<string, string> ApiMap {
      get { return _ApiMap; }
    }

    private static string PreprocessMd(string mdText) {
      if (mdText == null) return "";

      // process [abc] links for API docs
      var rx = new Regex(@"\[(.*?)\](?!\()", RegexOptions.Multiline);
      mdText = rx.Replace(mdText, _DocsMatchEvaluator);

      // process [abc](learn/xyz) links
      rx = new Regex(@"\[(.*)\]\(learn/(.*)\)", RegexOptions.Multiline);
      mdText = rx.Replace(mdText, @"[$1](https://godiagram.com/avalonia/latest/learn/$2)");

      // process [abc](intro/xyz) links
      rx = new Regex(@"\[(.*)\]\(intro/(.*)\)", RegexOptions.Multiline);
      mdText = rx.Replace(mdText, @"[$1](https://godiagram.com/avalonia/latest/intro/$2)");

      // leave [abc](demo/xyz) links alone

      return mdText;
    }

    /// <summary>
    /// Used as a delegate for regex replacement of <a> API references.
    /// </summary>
    /// <param name="m"></param>
    /// <returns></returns>
    private static string _ReplaceApiLinks(Match m) {
      var str = m.Groups[1].Value;
      if (_ApiMap != null && _ApiMap.TryGetValue(str, out var url))
        return $"[{str}](https://godiagram.com/avalonia/latest/{url})";
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
      var url = "https://godiagram.com/avalonia/latest/api/apiMap.js";
      using var response = await _HttpClient.GetAsync(url);
      if (response.IsSuccessStatusCode) {
        using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);
        var text = reader.ReadToEnd();
        text = text.Substring(text.IndexOf('{'));
        return JsonSerializer.Deserialize<Dictionary<string, string>>(text);
      }
      return null;
    }
  }

  public class CustomLinkCommand : ICommand {
    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter) {
      var urlTxt = (string)parameter;
      if (urlTxt.StartsWith("demo/")) {
        MainView._SelectDemo(MainView.ProcessInput(urlTxt.Substring(5)));
      } else {
        Markdown.Avalonia.Utils.DefaultHyperlinkCommand.GoTo(urlTxt);
      }
    }
  }
}
