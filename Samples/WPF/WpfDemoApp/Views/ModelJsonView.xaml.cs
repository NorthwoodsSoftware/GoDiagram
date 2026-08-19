/* Copyright (c) Northwoods Software Corporation. */

using System.Windows;
using System.Windows.Controls;

namespace WpfDemoApp.Views; 
public partial class ModelJsonView : UserControl {
  public ModelJsonView() {
    InitializeComponent();
  }

  public static readonly DependencyProperty CanSaveLoadProperty =
    DependencyProperty.Register(nameof(CanSaveLoad), typeof(bool), typeof(ModelJsonView), new PropertyMetadata(true));

  public bool CanSaveLoad {
    get { return (bool)GetValue(CanSaveLoadProperty); }
    set { SetValue(CanSaveLoadProperty, value); }
  }

  public Action SaveClick { get; set; }

  public Action LoadClick { get; set; }

  public static readonly DependencyProperty JsonTextProperty =
    DependencyProperty.Register(nameof(JsonText), typeof(string), typeof(ModelJsonView), new FrameworkPropertyMetadata("Model JSON", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

  public string JsonText {
    get { return (string)GetValue(JsonTextProperty); }
    set { SetValue(JsonTextProperty, value); }
  }

  private void saveBtn_Click(object sender, RoutedEventArgs e) {
    SaveClick?.Invoke();
  }

  private void loadBtn_Click(object sender, RoutedEventArgs e) {
    LoadClick?.Invoke();
  }
}
