/* Copyright 1998-2024 by Northwoods Software Corporation. */

namespace Demo.Extensions.PolygonDrawing {
  public partial class PolygonDrawing : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitCheckBoxes() {
      resizingCb.IsCheckedChanged += (s, e) => _ToggleResizing();
      reshapingCb.IsCheckedChanged += (s, e) => _ToggleReshaping();
      resegmentingCb.IsCheckedChanged += (s, e) => _ToggleResegmenting();
      rotatingCb.IsCheckedChanged += (s, e) => _ToggleRotating();
    }
  }
}
