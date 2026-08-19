/* Copyright (c) Northwoods Software Corporation. */

namespace Demo.Extensions.ZoomSlider; 
public partial class ZoomSlider : DemoControl {
  // This is a stub for the designer.
  // See the SharedSamples project for sample implementation.

  private void _InitSlider() {
    slider.ValueChanged += (s, e) => Rescale();
  }

  private void _SetSlider(double value) {
    slider.Value = value;
  }
}
