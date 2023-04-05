/* Copyright 1998-2023 by Northwoods Software Corporation. */

using Avalonia.Controls;

namespace Demo.Extensions.DragCreating {
  public partial class DragCreating : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitCheckBox() {
      enabledCb.IsCheckedChanged += (s, e) => _EnableTool(enabledCb.IsChecked ?? false);
    }
  }
}
