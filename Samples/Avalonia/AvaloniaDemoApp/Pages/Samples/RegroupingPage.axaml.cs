/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using Avalonia;

namespace Demo.Samples.Regrouping {
  public partial class Regrouping : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitSlider() {
      levelSlider.GetPropertyChangedObservable(Avalonia.Controls.Slider.ValueProperty).Subscribe(o => Reexpand());
    }
  }
}
