/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System.ComponentModel;

namespace Demo.Extensions.DragCreating {
  [ToolboxItem(false)]
  public partial class DragCreating : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitCheckBox() {
      enabledCb.CheckedChanged += (s, e) => _EnableTool(enabledCb.Checked);
    }
  }
}
