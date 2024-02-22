/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System;
using Northwoods.Go.Layouts;

namespace Demo.Samples.FDLayout {
  public partial class FDLayout : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitControls() {
      // TextBoxes
      maxIter.LostFocus += (s, e) => _Layout();
      epsilon.LostFocus += (s, e) => _Layout();
      infinity.LostFocus += (s, e) => _Layout();
      arrangement.LostFocus += (s, e) => _Layout();
      charge.LostFocus += (s, e) => _Layout();
      mass.LostFocus += (s, e) => _Layout();
      stiffness.LostFocus += (s, e) => _Layout();
      length.LostFocus += (s, e) => _Layout();
    }
  }
}
