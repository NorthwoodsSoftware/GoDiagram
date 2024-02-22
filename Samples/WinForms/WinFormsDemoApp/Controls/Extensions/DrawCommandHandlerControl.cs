/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System.ComponentModel;
using Northwoods.Go.Extensions;

namespace Demo.Extensions.DrawCommandHandler {
  [ToolboxItem(false)]
  public partial class DrawCommandHandler : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitRadioButtons() {
      moveRb.CheckedChanged += (s, e) => { if (moveRb.Checked) _SetArrowMode(ArrowBehavior.Move); };
      scrollRb.CheckedChanged += (s, e) => { if (scrollRb.Checked) _SetArrowMode(ArrowBehavior.Scroll); };
      selectRb.CheckedChanged += (s, e) => { if (selectRb.Checked) _SetArrowMode(ArrowBehavior.Select); };
      treeRb.CheckedChanged += (s, e) => { if (treeRb.Checked) _SetArrowMode(ArrowBehavior.Tree); };
    }
  }
}
