/* Copyright (c) Northwoods Software Corporation. */

using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Demo;

public class DemoControl : UserControl {
  protected void AfterLoad(Action loadFunc) {
    // Don't call setup until we know the diagrams have their bounds.
    // Input priority ensures all children have finished loading,
    // since it is lower priority than Loaded.
    Dispatcher.BeginInvoke(loadFunc, DispatcherPriority.Input);
  }

  public static void ShowDialog(string text) {
    MessageBox.Show(text);
  }

  protected static decimal ToNumericUpDownValue(double value) {
    return (decimal)value;
  }
}
