/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System;
using System.ComponentModel;

namespace Demo.Samples.UpdateDemo {
  [ToolboxItem(false)]
  public partial class UpdateDemo : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _AddToLog(string changes) {
      modelLog.Items.Add(changes);
      // scrolls down automatically
      var visibleItems = modelLog.ClientSize.Height / modelLog.ItemHeight;
      modelLog.TopIndex = Math.Max(modelLog.Items.Count - visibleItems + 1, 0);
    }

    private void _ClearLog() {
      modelLog.Items.Clear();
    }
  }
}
