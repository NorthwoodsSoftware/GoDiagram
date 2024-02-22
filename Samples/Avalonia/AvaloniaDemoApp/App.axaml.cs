using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaDemoApp.Views;

namespace AvaloniaDemoApp {
  public partial class App : Application {
    public override void Initialize() {
      Name = "AvaloniaDemoApp";
      AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted() {
      if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
        var openTo = (DemoType.Sample, DemoIndex.Samples.First());  // open to Org Chart Static by default
        var args = desktop.Args;
        if (args != null && args.Length > 0) {
          openTo = MainView.ProcessInput(args[0]);
        }

        desktop.MainWindow = new MainWindow(openTo);
      }

      base.OnFrameworkInitializationCompleted();
    }
  }
}
