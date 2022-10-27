using System.Collections.Generic;
using Avalonia.Controls;

namespace AvaloniaDemoApp.Views {
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
    }

    public MainWindow((DemoType, KeyValuePair<string, NavItem>) opento) {
      InitializeComponent();

      MainView._SelectDemo(opento);
    }
  }
}
