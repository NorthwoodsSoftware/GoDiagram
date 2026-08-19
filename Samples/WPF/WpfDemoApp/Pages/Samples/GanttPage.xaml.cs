/* Copyright (c) Northwoods Software Corporation. */

namespace Demo.Samples.Gantt; 
public partial class Gantt : DemoControl {
  // This is a stub for the designer.
  // See the SharedSamples project for sample implementation.

  private void _InitSlider() {
    widthSlider.ValueChanged += (s, e) => _Rescale(Convert.ToInt32(e.NewValue));
  }
}
