/* Copyright 1998-2023 by Northwoods Software Corporation. */

using Northwoods.Go.Extensions;

namespace Demo.Extensions.DrawCommandHandler {
  public partial class DrawCommandHandler : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitRadioButtons() {
      moveRb.Checked += (s, e) => _SetArrowMode(ArrowBehavior.Move);
      scrollRb.Checked += (s, e) => _SetArrowMode(ArrowBehavior.Scroll);
      selectRb.Checked += (s, e) => _SetArrowMode(ArrowBehavior.Select);
      treeRb.Checked += (s, e) => _SetArrowMode(ArrowBehavior.Tree);
    }
  }
}
