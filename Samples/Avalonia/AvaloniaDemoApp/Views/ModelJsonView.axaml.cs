using System;
using Avalonia;
using Avalonia.Controls;

namespace AvaloniaDemoApp.Views {
  public partial class ModelJsonView : UserControl {
    public ModelJsonView() {
      InitializeComponent();
    }

    public static readonly StyledProperty<bool> CanSaveLoadProperty =
      AvaloniaProperty.Register<ModelJsonView, bool>(nameof(CanSaveLoad), true);

    public bool CanSaveLoad {
      get { return GetValue(CanSaveLoadProperty); }
      set {
        SetValue(CanSaveLoadProperty, value);
      }
    }

    public Action SaveClick { get; set; }

    public Action LoadClick { get; set; }

    public static readonly StyledProperty<string> JsonTextProperty =
      AvaloniaProperty.Register<ModelJsonView, string>(nameof(JsonText), "Model JSON", false, Avalonia.Data.BindingMode.TwoWay);

    public string JsonText {
      get { return GetValue(JsonTextProperty); }
      set {
        SetValue(JsonTextProperty, value);
      }
    }

    private void saveBtn_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e) {
      SaveClick?.Invoke();
    }

    private void loadBtn_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e) {
      LoadClick?.Invoke();
    }
  }
}
