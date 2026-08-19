/* Copyright (c) Northwoods Software Corporation. */

using System.Collections.ObjectModel;
using System.Windows;

namespace Demo.Samples.UpdateDemo; 
public partial class UpdateDemo : DemoControl {
  // This is a stub for the designer.
  // See the SharedSamples project for sample implementation.

  public static readonly DependencyProperty LogProperty =
    DependencyProperty.Register(nameof(Log), typeof(ObservableCollection<string>), typeof(UpdateDemo));

  public ObservableCollection<string> Log {
    get { return (ObservableCollection<string>)GetValue(LogProperty); }
    set { SetValue(LogProperty, value); }
  }

  private void _AddToLog(string changes) {
    if (Log == null) Log = new ObservableCollection<string>();
    Log.Add(changes);

    modelLog.ScrollIntoView(modelLog.Items.Count - 1);
  }

  private void _ClearLog() {
    Log.Clear();
  }
}
