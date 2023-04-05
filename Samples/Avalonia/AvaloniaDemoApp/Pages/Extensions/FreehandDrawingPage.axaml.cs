/* Copyright 1998-2023 by Northwoods Software Corporation. */

namespace Demo.Extensions.FreehandDrawing {
  public partial class FreehandDrawing : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitCheckBoxes() {
      resizingCb.IsCheckedChanged += (s, e) => _ToggleResizing();
      reshapingCb.IsCheckedChanged += (s, e) => _ToggleReshaping();
      rotatingCb.IsCheckedChanged += (s, e) => _ToggleRotating();
    }
  }
}
