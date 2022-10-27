/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;

namespace Demo.Samples.UpdateDemo {
  public partial class UpdateDemo : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    public static readonly StyledProperty<ObservableCollection<string>> LogProperty =
      AvaloniaProperty.Register<UpdateDemo, ObservableCollection<string>>(nameof(Log));

    public ObservableCollection<string> Log {
      get { return GetValue(LogProperty); }
      set {
        SetValue(LogProperty, value);
      }
    }

    private void _AddToLog(string changes) {
      if (Log == null) Log = new ObservableCollection<string>();
      Log.Add(changes);

      modelLog.ScrollIntoView(modelLog.ItemCount - 1);
    }

    private void _ClearLog() {
      Log.Clear();
    }
  }
}
