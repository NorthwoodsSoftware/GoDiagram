/* Copyright 1998-2023 by Northwoods Software Corporation. */

namespace Demo.Extensions.PolygonDrawing {
  public partial class PolygonDrawing : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitCheckBoxes() {
      resizingCb.Checked += (s, e) => _ToggleResizing();
      resizingCb.Unchecked += (s, e) => _ToggleResizing();
      reshapingCb.Checked += (s, e) => _ToggleReshaping();
      reshapingCb.Unchecked += (s, e) => _ToggleReshaping();
      resegmentingCb.Checked += (s, e) => _ToggleResegmenting();
      resegmentingCb.Unchecked += (s, e) => _ToggleResegmenting();
      rotatingCb.Checked += (s, e) => _ToggleRotating();
      rotatingCb.Unchecked += (s, e) => _ToggleRotating();
    }
  }
}
