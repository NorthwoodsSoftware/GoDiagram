/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System;
using System.Linq;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Northwoods.Go.Avalonia;

namespace Demo {
  // Simple derived class to improve commonization of code
  public class DemoControl : Avalonia.Controls.UserControl {

    protected void AfterLoad(Action loadFunc) {
      // Don't call setup until we know the diagrams have their bounds.
      // Input priority ensures all children have finished loading,
      // since it is lower priority than Loaded.
      Dispatcher.UIThread.Post(loadFunc, DispatcherPriority.Input);
    }

    protected override void OnUnloaded(RoutedEventArgs e) {
      base.OnUnloaded(e);

      // clean up controls on unload since we always recreate them
      foreach (var diaCtrl in this.GetVisualDescendants().OfType<DiagramControl>()) {
        diaCtrl.Cleanup();
      }
    }

    public void ShowDialog(string text) {
      DialogView.Show(this, text);
    }

    protected static decimal ToNumericUpDownValue(double value) {
      return (decimal)value;
    }
  }
}
