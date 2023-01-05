/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System.ComponentModel;

namespace Demo.Extensions.PolygonDrawing {
  [ToolboxItem(false)]
  public partial class PolygonDrawing : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitCheckBoxes() {
      resizingCb.CheckedChanged += (s, e) => _ToggleResizing();
      reshapingCb.CheckedChanged += (s, e) => _ToggleReshaping();
      resegmentingCb.CheckedChanged += (s, e) => _ToggleResegmenting();
      rotatingCb.CheckedChanged += (s, e) => _ToggleRotating();
    }
  }
}
