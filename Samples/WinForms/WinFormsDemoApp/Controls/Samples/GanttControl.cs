/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.ComponentModel;

namespace Demo.Samples.Gantt {
  [ToolboxItem(false)]
  public partial class Gantt : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitSlider() {
      widthSlider.ValueChanged += (s, e) => _Rescale(widthSlider.Value);
    }
  }
}
