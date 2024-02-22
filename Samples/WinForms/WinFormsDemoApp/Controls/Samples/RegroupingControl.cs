/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System.ComponentModel;

namespace Demo.Samples.Regrouping {
  [ToolboxItem(false)]
  public partial class Regrouping : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitSlider() {
      levelSlider.ValueChanged += (s, e) => Reexpand();
    }
  }
}
