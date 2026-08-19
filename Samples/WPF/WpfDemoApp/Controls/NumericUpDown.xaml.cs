/* Copyright (c) Northwoods Software Corporation. */

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Demo;

public partial class NumericUpDown : UserControl {
  private bool _IsUpdatingText = false;

  public NumericUpDown() {
    InitializeComponent();
    UpdateText(Value);
  }

  public static readonly DependencyProperty ValueProperty =
      DependencyProperty.Register(
          nameof(Value),
          typeof(decimal),
          typeof(NumericUpDown),
          new FrameworkPropertyMetadata(
              0m,
              FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
              OnValueChanged,
              CoerceValue));

  public decimal Value {
    get => (decimal)GetValue(ValueProperty);
    set => SetValue(ValueProperty, value);
  }

  public static readonly DependencyProperty MinimumProperty =
      DependencyProperty.Register(
          nameof(Minimum),
          typeof(decimal),
          typeof(NumericUpDown),
          new PropertyMetadata(decimal.MinValue, OnMinimumChanged));

  public decimal Minimum {
    get => (decimal)GetValue(MinimumProperty);
    set => SetValue(MinimumProperty, value);
  }

  public static readonly DependencyProperty MaximumProperty =
      DependencyProperty.Register(
          nameof(Maximum),
          typeof(decimal),
          typeof(NumericUpDown),
          new PropertyMetadata(decimal.MaxValue, OnMaximumChanged));

  public decimal Maximum {
    get => (decimal)GetValue(MaximumProperty);
    set => SetValue(MaximumProperty, value);
  }

  public static readonly DependencyProperty IncrementProperty =
      DependencyProperty.Register(
          nameof(Increment),
          typeof(decimal),
          typeof(NumericUpDown),
          new PropertyMetadata(1m));

  public decimal Increment {
    get => (decimal)GetValue(IncrementProperty);
    set => SetValue(IncrementProperty, value);
  }

  private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
    if (d is NumericUpDown numericUpDown) {
      numericUpDown.UpdateText((decimal)e.NewValue);
    }
  }

  private static object CoerceValue(DependencyObject d, object value) {
    if (d is NumericUpDown numericUpDown && value is decimal decimalValue) {
      if (decimalValue < numericUpDown.Minimum) return numericUpDown.Minimum;
      if (decimalValue > numericUpDown.Maximum) return numericUpDown.Maximum;
    }
    return value;
  }

  private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
    if (d is NumericUpDown numericUpDown) {
      numericUpDown.CoerceValue(ValueProperty);
    }
  }

  private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
    if (d is NumericUpDown numericUpDown) {
      numericUpDown.CoerceValue(ValueProperty);
    }
  }

  private void UpdateText(decimal value) {
    if (TextBoxValue == null) return;

    _IsUpdatingText = true;
    try {
      // Only update text if it differs in numeric value to preserve user typing states like "0."
      if (decimal.TryParse(TextBoxValue.Text, out var currentParsed)) {
        if (currentParsed != value) {
          TextBoxValue.Text = value.ToString("G29");
        }
      } else {
        TextBoxValue.Text = value.ToString("G29");
      }
    } finally {
      _IsUpdatingText = false;
    }
  }

  private void TextBoxValue_TextChanged(object sender, TextChangedEventArgs e) {
    if (_IsUpdatingText) return;

    if (decimal.TryParse(TextBoxValue.Text, out decimal result)) {
      var clamped = Math.Max(Minimum, Math.Min(Maximum, result));
      if (Value != clamped) {
        Value = clamped;
      }
    }
  }

  private void TextBoxValue_PreviewTextInput(object sender, TextCompositionEventArgs e) {
    // Only allow valid numeric chars: digits, minus sign, decimal separator
    e.Handled = e.Text.Any(c => !char.IsDigit(c) && c != '.' && c != '-');
  }

  private void ButtonUp_Click(object sender, RoutedEventArgs e) {
    Value = Math.Min(Maximum, Value + Increment);
  }

  private void ButtonDown_Click(object sender, RoutedEventArgs e) {
    Value = Math.Max(Minimum, Value - Increment);
  }
}
