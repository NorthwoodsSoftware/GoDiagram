using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace WinFormsSharedControls {
  [ToolboxItem(true)]
  public class GoWebBrowser : WebView2 {
    private bool _Initialized;
    private string _Html;
    private SideNav _SideNav;

    public GoWebBrowser() {

      NavigationStarting += GoWebBrowser_NavigationStarting;
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
        ((WebView2)sender).ExecuteScriptAsync("var body = document.querySelector('body'); body.style.overflow='scroll'; body.style.font='.825rem Segoe UI'; var style=document.createElement('style');style.type='text/css';style.innerHTML='::-webkit-scrollbar{display:none}; font-size: .875rem; line-height: 1.5; letter-spacing: 0;';document.getElementsByTagName('body')[0].appendChild(style)");
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
    /// Process the provided HTML string for proper links.
    /// NYI: currently pointing to GoJS.
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    private static string PreprocessHtml(string html) {
      if (html == null) return "";
      // process <a> links
      var rx = new Regex(@"<a>(.*)</a>", RegexOptions.Multiline);
      html = rx.Replace(html, @"<a href=""https://gojs.net/api/symbols/$1.html"">$1</a>");
      // process <a href="<relative>"> links
      rx = new Regex(@"<a href=""samples"".*>(.*)</a>");
      // process <a href="https"> links
      rx = new Regex(@"<a href=""http"".*>(.*)</a>");
      return html;
    }

    /// <summary>
    /// Open links in a browser.
    /// </summary>
    /// <param name="e"></param>
    private void GoWebBrowser_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e) {
      if (_Html == null) return;
      
      if (e.Uri.StartsWith("http")) {
        Process.Start("explorer.exe", e.Uri);
        e.Cancel = true;
      } else if (e.Uri.StartsWith("samples/")) {
        _SideNav ??= FindForm().Controls.OfType<SideNav>().FirstOrDefault();
        var sampleName = e.Uri.Substring(8);
        var sideNavItems = _SideNav.DataSource as List<NavItem>;
        foreach (var item in sideNavItems) {
          if (item.Name == sampleName) {
            _SideNav.SelectedItem = item;
            break;
          }
        }
        e.Cancel = true;
      }
    }

  }
}
