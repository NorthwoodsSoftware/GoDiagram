using Avalonia.Controls;
using Avalonia.Markup.Xaml;

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

    public static void Show(IControl caller, string text) {
      var msgbox = new DialogView(text);

      if (caller != null) msgbox.ShowDialog((Window)caller.VisualRoot);
      else msgbox.Show();
    }

    public string Text { get; } = "Dialog Text";
  }
}
