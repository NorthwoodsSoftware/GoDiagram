using System;
using System.Linq;
using Avalonia.Controls;

namespace AvaloniaDemoApp.Views {
  public partial class MainView : UserControl {
    // so we can access from CustomLinkCommand in the DescriptionView
    internal static MainView _MainView;

    public MainView() {
      // Expose the NavItem values (not the dictionary) so the ListBox item templates are strongly
      // typed for compiled bindings. The dictionaries themselves remain available for key lookups.
      Resources["Samples"] = DemoIndex.Samples.Values;
      Resources["Extensions"] = DemoIndex.Extensions.Values;

      InitializeComponent();

      _MainView = this;
    }

    public static (DemoType, NavItem) ProcessInput(string s) {
      s = s.Substring(s.IndexOf(':') + 1);
      // the dictionaries use a case-insensitive comparer, so TryGetValue matches regardless of casing
      if (DemoIndex.Samples.TryGetValue(s, out var sample)) {
        return (DemoType.Sample, sample);
      } else if (DemoIndex.Extensions.TryGetValue(s, out var extension)) {
        return (DemoType.Extension, extension);
      }
      return (DemoType.Sample, DemoIndex.Samples.Values.First());  // unknown input argument? use first sample
    }

    public static void SelectDemo((DemoType, NavItem) openTo) {
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
      var nav = e.AddedItems.OfType<NavItem>().FirstOrDefault();
      if (nav != null) _ActivateDemo(nav);
    }

    private async void _ActivateDemo(NavItem nav) {
      await DescriptionView._InitApiMap();  // ensure API map is initialized before creating any controls

      if (Activator.CreateInstance(nav.ControlType) is not UserControl sample) return;
      DemoContent.Children.Clear();
      DockPanel.SetDock(sample, Dock.Top);
      DemoContent.Children.Add(sample);
    }
  }
}
