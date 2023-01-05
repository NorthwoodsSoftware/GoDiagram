/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using Avalonia;

namespace Demo.Samples.Gantt {
  public partial class Gantt : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitSlider() {
      widthSlider.GetPropertyChangedObservable(Avalonia.Controls.Slider.ValueProperty).Subscribe(o => _Rescale(Convert.ToInt32(o.NewValue)));
    }
  }
}
