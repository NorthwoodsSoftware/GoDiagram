using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;

namespace Demo {
  public partial class DialogView : Window {
    public DialogView() {
      InitializeComponent();
    }

    public DialogView(string text) {
      Text = text;
      InitializeComponent();
    }

    private void InitializeComponent() {
      AvaloniaXamlLoader.Load(this);
    }

    private void OnOkClick(object sender, Avalonia.Interactivity.RoutedEventArgs e) {
      Close();
    }

    public static void Show(Control caller, string text) {
      var msgbox = new DialogView(text);
      var root = caller.GetVisualRoot();

      if (root != null) msgbox.ShowDialog((Window)root);
      else msgbox.Show();
    }

    public string Text { get; } = "Dialog Text";
  }
}
