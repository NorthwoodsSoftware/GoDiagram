/* Copyright 1998-2024 by Northwoods Software Corporation. */

using Avalonia.Controls;

namespace Demo.Extensions.ZoomSlider {
  public partial class ZoomSlider : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitSlider() {
      slider.PropertyChanged += (s, e) => {
        if (e.Property == Slider.ValueProperty) Rescale();
      };
    }

    private void _SetSlider(double value) {
      slider.Value = value;
    }
  }
}
