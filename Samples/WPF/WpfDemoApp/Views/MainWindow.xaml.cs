/* Copyright (c) Northwoods Software Corporation. */

using System.Windows;

namespace WpfDemoApp.Views;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {
  public MainWindow() {
    InitializeComponent();
    MainView.SelectDemo((DemoType.Sample, DemoIndex.Samples.First()));
  }

  public MainWindow((DemoType, KeyValuePair<string, NavItem>) opento) {
    InitializeComponent();
    MainView.SelectDemo(opento);
  }
}
