/* Copyright 1998-2022 by Northwoods Software Corporation. */

namespace Demo.Extensions.FreehandDrawing {
  public partial class FreehandDrawing : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitCheckBoxes() {
      resizingCb.Checked += (s, e) => _ToggleResizing();
      resizingCb.Unchecked += (s, e) => _ToggleResizing();
      reshapingCb.Checked += (s, e) => _ToggleReshaping();
      reshapingCb.Unchecked += (s, e) => _ToggleReshaping();
      rotatingCb.Checked += (s, e) => _ToggleRotating();
      rotatingCb.Unchecked += (s, e) => _ToggleRotating();
    }
  }
}
