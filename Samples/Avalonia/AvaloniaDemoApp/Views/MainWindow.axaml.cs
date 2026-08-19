using Avalonia.Controls;

namespace AvaloniaDemoApp.Views {
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
    }

    public MainWindow((DemoType, NavItem) opento) {
      InitializeComponent();

      MainView.SelectDemo(opento);
    }
  }
}
