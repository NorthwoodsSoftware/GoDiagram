/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;

namespace Demo {
  // Simple derived class to improve commonization of code
  public class DemoControl : System.Windows.Forms.UserControl {
    private bool _FirstPaint = true;

    protected void AfterLoad(Action loadFunc) {
      // Don't call setup until we know the diagrams have their bounds.
      // First Paint ensures all children have finished loading,
      // including docking, anchoring, etc.
      Paint += (s, e) => {
        if (!_FirstPaint) return;
        _FirstPaint = false;
        loadFunc();
      };
    }

    public void ShowDialog(string text) {
      var window = FindForm();
      System.Windows.Forms.MessageBox.Show(window, text);
    }

    protected static decimal ToNumericUpDownValue(double value) {
      return (decimal)value;
    }
  }
}
