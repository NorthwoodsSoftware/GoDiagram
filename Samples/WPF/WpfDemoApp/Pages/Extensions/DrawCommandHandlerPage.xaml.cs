/* Copyright (c) Northwoods Software Corporation. */

using Northwoods.Go.Extensions;

namespace Demo.Extensions.DrawCommandHandler; 
public partial class DrawCommandHandler : DemoControl {
  // This is a stub for the designer.
  // See the SharedSamples project for sample implementation.

  private void _InitRadioButtons() {
    moveRb.Click += (s, e) => {
      if ((bool)moveRb.IsChecked) _SetArrowMode(ArrowBehavior.Move);
    };
    scrollRb.Click += (s, e) => {
      if ((bool)scrollRb.IsChecked) _SetArrowMode(ArrowBehavior.Scroll);
    };
    selectRb.Click += (s, e) => {
      if ((bool)selectRb.IsChecked) _SetArrowMode(ArrowBehavior.Select);
    };
    treeRb.Click += (s, e) => {
      if ((bool)treeRb.IsChecked) _SetArrowMode(ArrowBehavior.Tree);
    };
  }
}
