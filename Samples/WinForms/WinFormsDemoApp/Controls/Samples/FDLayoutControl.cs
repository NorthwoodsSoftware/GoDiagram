/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using System.ComponentModel;
using Northwoods.Go.Layouts;

namespace Demo.Samples.FDLayout {
  [ToolboxItem(false)]
  public partial class FDLayout : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitControls() {
      // TextBoxes
      maxIter.Leave += (s, e) => _Layout();
      epsilon.Leave += (s, e) => _Layout();
      infinity.Leave += (s, e) => _Layout();
      arrangement.Leave += (s, e) => _Layout();
      charge.Leave += (s, e) => _Layout();
      mass.Leave += (s, e) => _Layout();
      stiffness.Leave += (s, e) => _Layout();
      length.Leave += (s, e) => _Layout();
    }
  }
}
