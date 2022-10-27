/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using Avalonia;

namespace Demo.Extensions.ZoomSlider {
  public partial class ZoomSlider : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitSlider() {
      slider.GetPropertyChangedObservable(Avalonia.Controls.Slider.ValueProperty).Subscribe(o => Rescale());
    }

    private void _SetSlider(double value) {
      slider.Value = value;
    }
  }
}
