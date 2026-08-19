/* Copyright (c) Northwoods Software Corporation. */

using System.Windows;
using WpfDemoApp.Views;

namespace WpfDemoApp;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
  private void Application_Startup(object sender, StartupEventArgs e) {
    // read only the first arg (e.g. the "gowpf:flowchart" launch-protocol URL), which
    // directs the app to open a particular sample or extension
    var window = e.Args.Length > 0
      ? new MainWindow(MainView.ProcessInput(e.Args[0]))
      : new MainWindow();
    window.Show();
  }
}

