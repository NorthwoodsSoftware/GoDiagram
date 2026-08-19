/* Copyright (c) Northwoods Software Corporation. */

namespace Demo.Samples.Regrouping; 
public partial class Regrouping : DemoControl {
  // This is a stub for the designer.
  // See the SharedSamples project for sample implementation.

  private void _InitSlider() {
    levelSlider.ValueChanged += (s, e) => Reexpand();
  }
}
