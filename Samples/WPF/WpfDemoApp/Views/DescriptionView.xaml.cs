/* Copyright (c) Northwoods Software Corporation. */

using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfDemoApp.Views {
  public partial class DescriptionView : UserControl {
    private static readonly HttpClient _HttpClient = new();
    private static Dictionary<string, string> _ApiMap = null;
    private static readonly MatchEvaluator _DocsMatchEvaluator = new(_ReplaceApiLinks);

    public DescriptionView() {
      InitializeComponent();

      markdown.PreviewMouseWheel += (s, e) => {
        if (!e.Handled) {
          e.Handled = true;
          var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta) {
            RoutedEvent = UIElement.MouseWheelEvent,
            Source = s
          };
          var parent = ((Control)s).Parent as UIElement;
          parent?.RaiseEvent(eventArg);
        }
      };
    }

    public static readonly DependencyProperty MdTextProperty =
      DependencyProperty.Register(nameof(MdText), typeof(string), typeof(DescriptionView),
        new PropertyMetadata("[Brush](https://godiagram.com/winforms/api/symbols/Brush.html)"));

    public string MdText {
      get { return (string)GetValue(MdTextProperty); }
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
      mdText = rx.Replace(mdText, @"[$1](https://godiagram.com/winforms/learn/$2)");  // NYI, update to WPF URL when available

      // process [abc](intro/xyz) links
      rx = new Regex(@"\[(.*)\]\(intro/(.*)\)", RegexOptions.Multiline);
      mdText = rx.Replace(mdText, @"[$1](https://godiagram.com/winforms/intro/$2)");  // NYI, update to WPF URL when available

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
        return $"[{str}](https://godiagram.com/winforms/{url})";  // NYI, update to WPF URL when available
      return m.Value;
    }

    /// <summary>
    /// Initialize the API map if it hasn't been already.
    /// </summary>
    /// <returns></returns>
    internal static async Task _InitApiMap() {
      if (_ApiMap == null) {
        _ApiMap = await GetApiMap();
      }
    }

    /// <summary>
    /// This method is responsible for fetching the map of short names to API URLs on godiagram.com.
    /// </summary>
    /// <returns></returns>
    static async Task<Dictionary<string, string>> GetApiMap() {
      var url = "https://godiagram.com/winforms/api/apiMap.js";  // NYI, update to WPF URL when available
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
        MainView.SelectDemo(MainView.ProcessInput(urlTxt.Substring(5)));
      } else {
        Process.Start(new ProcessStartInfo(urlTxt) { UseShellExecute = true, Verb = "open" });
      }
    }
  }
}
