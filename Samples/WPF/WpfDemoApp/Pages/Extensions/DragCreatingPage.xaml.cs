/* Copyright (c) Northwoods Software Corporation. */

using System.Windows.Controls;

namespace Demo.Extensions.DragCreating; 
public partial class DragCreating : DemoControl {
  // This is a stub for the designer.
  // See the SharedSamples project for sample implementation.

  private void _InitCheckBox() {
    enabledCb.Click += (s, e) => _EnableTool(enabledCb.IsChecked ?? false);
  }
}
