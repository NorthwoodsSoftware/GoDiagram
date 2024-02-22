/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System;
using Avalonia.Controls;

namespace Demo.Samples.Gantt {
  public partial class Gantt : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitSlider() {
      widthSlider.PropertyChanged += (s, e) => {
        if (e.Property == Slider.ValueProperty) _Rescale(Convert.ToInt32(e.NewValue));
      };
    }
  }
}
