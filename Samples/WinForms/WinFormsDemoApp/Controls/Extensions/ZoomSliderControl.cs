/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.ComponentModel;

namespace Demo.Extensions.ZoomSlider {
  [ToolboxItem(false)]
  public partial class ZoomSlider : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitSlider() {
      slider.ValueChanged += (s, e) => Rescale();
    }

    private void _SetSlider(double value) {
      slider.Value = (int)Math.Round(value);
    }
  }
}
