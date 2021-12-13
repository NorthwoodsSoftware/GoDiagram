using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using WinFormsSharedControls;

namespace WinFormsDemoApp {
  public partial class App : Form {

    public App((DemoType, string) opento) {
      InitializeComponent();

      sampleList.DataSource = new BindingSource(DemoIndex.Samples, null);
      sampleList.ValueMember = "Key";
      sampleList.DisplayMember = "Value";
      sampleList.SelectedValueChanged += SampleList_SelectedValueChanged;
      sampleList.ClearSelected();

      extensionList.DataSource = new BindingSource(DemoIndex.Extensions, null);
      extensionList.ValueMember = "Key";
      extensionList.DisplayMember = "Value";
      extensionList.SelectedValueChanged += ExtensionList_SelectedValueChanged;
      extensionList.ClearSelected();

      _SelectDemo(opento);
    }

    private void _SelectDemo((DemoType, string) opento) {
      var (type, demo) = opento;
      switch (type) {
        case DemoType.Sample:
          tabControl1.SelectedTab = sampleTab;
          sampleList.SelectedValue = demo;
          break;
        case DemoType.Extension:
          tabControl1.SelectedTab = extensionTab;
          extensionList.SelectedValue = demo;
          break;
      }
    }

    private void SampleList_SelectedValueChanged(object sender, EventArgs e) {
      if (sampleList.SelectedValue == null) return;
      if (DemoIndex.Samples.TryGetValue((string)sampleList.SelectedValue, out var sel)) {
        _ActivateDemo(sel);
      }
    }

    private void ExtensionList_SelectedValueChanged(object sender, EventArgs e) {
      if (extensionList.SelectedValue == null) return;
      if (DemoIndex.Extensions.TryGetValue((string)extensionList.SelectedValue, out var sel)) {
        _ActivateDemo(sel);
      }
    }

    private void _InitWebBrowsers() {
      var sample = panel1.Controls[0];
      var webBrowsers = new List<GoWebBrowser>();
      _FindWebBrowsers(sample.Controls, ref webBrowsers);
      foreach (var gwb in webBrowsers) {
        gwb.InitializeAsync();
        gwb.NavigationStarting += _NavigateToUri;
      }
    }

    private async void _ActivateDemo(NavItem nav) {
      await GoWebBrowser._InitApiMap();  // ensure API map is initialized for GoWebBrowser before creating any controls

      if (Activator.CreateInstance(nav.ControlType) is not UserControl demo) return;
      panel1.Controls.Clear();
      demo.Dock = DockStyle.Fill;
      // We don't want a horizontal scrollbar due to the vertical one
      var tlp1 = demo.Controls.Find("tableLayoutPanel1", false).FirstOrDefault();
      if (tlp1 != null) tlp1.Padding = new Padding(0, 0, SystemInformation.VerticalScrollBarWidth, 0);
      panel1.Controls.Add(demo);
      // Initialize any GoWebBrowsers in the main content panel
      _InitWebBrowsers();
    }

    private void _FindWebBrowsers(Control.ControlCollection controls, ref List<GoWebBrowser> webBrowsers) {
      foreach (Control control in controls) {
        if (control.GetType() == typeof(GoWebBrowser)) {
          webBrowsers.Add((GoWebBrowser)control);
          continue;
        }
        if (control.Controls.Count > 0) _FindWebBrowsers(control.Controls, ref webBrowsers);
      }
    }

    private void _NavigateToUri(object sender, CoreWebView2NavigationStartingEventArgs e) {
      if (((GoWebBrowser)sender).Html == null) return;
      if (e.Uri.StartsWith("https://demo/")) {
        _SelectDemo(Program.ProcessInput(e.Uri.Substring(13)));
        e.Cancel = true;
      } else if (e.Uri.StartsWith("http")) {  // web links
        Process.Start("explorer.exe", e.Uri);
        e.Cancel = true;
      }
    }
  }
}
