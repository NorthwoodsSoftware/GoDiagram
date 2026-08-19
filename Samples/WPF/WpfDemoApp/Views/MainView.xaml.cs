/* Copyright (c) Northwoods Software Corporation. */

using System.Windows.Controls;

namespace WpfDemoApp.Views; 
public partial class MainView : UserControl {
  // so we can access from CustomLinkCommand in the DescriptionView
  internal static MainView _MainView;

  public MainView() {
    Resources["Samples"] = DemoIndex.Samples;
    Resources["Extensions"] = DemoIndex.Extensions;

    InitializeComponent();

    _MainView = this;
    Northwoods.Go.Wpf.DiagramControl.UseHardwareRendering = true;  // use hardware rendering for better performance, if available
  }

  public static (DemoType, KeyValuePair<string, NavItem>) ProcessInput(string s) {
    s = s.Substring(s.IndexOf(':') + 1);
    // unfortunately, original keys must be iterated to match given string
    // https://stackoverflow.com/questions/1619090/getting-a-keyvaluepair-directly-from-a-dictionary
    if (DemoIndex.Samples.ContainsKey(s)) {
      var kvp = DemoIndex.Samples.First(kvp => DemoIndex.Samples.Comparer.Equals(kvp.Key, s));
      return (DemoType.Sample, kvp);
    } else if (DemoIndex.Extensions.ContainsKey(s)) {
      var kvp = DemoIndex.Extensions.First(kvp => DemoIndex.Extensions.Comparer.Equals(kvp.Key, s));
      return (DemoType.Extension, kvp);
    }
    return (DemoType.Sample, DemoIndex.Samples.First());  // unknown input argument? use first sample
  }

  public static void SelectDemo((DemoType, KeyValuePair<string, NavItem>) openTo) {
    var (type, demo) = openTo;
    switch (type) {
      case DemoType.Sample:
        _MainView.TabControl.SelectedItem = _MainView.SampleTab;
        _MainView.SidebarSamples.SelectedItem = demo;
        break;
      case DemoType.Extension:
        _MainView.TabControl.SelectedItem = _MainView.ExtensionTab;
        _MainView.SidebarExtensions.SelectedItem = demo;
        break;
    }
  }

  private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
    if (e.AddedItems.Count > 0) {
      var kvp = (KeyValuePair<string, NavItem>)e.AddedItems[0];
      _ActivateDemo(kvp.Value);
    }
  }

  private async void _ActivateDemo(NavItem nav) {
    await DescriptionView._InitApiMap();  // ensure API map is initialized before creating any controls

    if (Activator.CreateInstance(nav.ControlType) is not UserControl sample) return;
    DemoContent.Content = sample;
  }
}
