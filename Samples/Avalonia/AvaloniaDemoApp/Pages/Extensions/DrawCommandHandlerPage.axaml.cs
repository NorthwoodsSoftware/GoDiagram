/* Copyright 1998-2024 by Northwoods Software Corporation. */

using Northwoods.Go.Extensions;

namespace Demo.Extensions.DrawCommandHandler {
  public partial class DrawCommandHandler : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitRadioButtons() {
      moveRb.IsCheckedChanged += (s, e) => {
        if ((bool)moveRb.IsChecked) _SetArrowMode(ArrowBehavior.Move);
      };
      scrollRb.IsCheckedChanged += (s, e) => {
        if ((bool)scrollRb.IsChecked) _SetArrowMode(ArrowBehavior.Scroll);
      };
      selectRb.IsCheckedChanged += (s, e) => {
        if ((bool)selectRb.IsChecked) _SetArrowMode(ArrowBehavior.Select);
      };
      treeRb.IsCheckedChanged += (s, e) => {
        if ((bool)treeRb.IsChecked) _SetArrowMode(ArrowBehavior.Tree);
      };
    }
  }
}
