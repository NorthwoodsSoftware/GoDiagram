/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.ComponentModel;
using Northwoods.Go.Tools.Extensions;

namespace Demo.Extensions.SnapLinkReshaping {
  [ToolboxItem(false)]
  public partial class SnapLinkReshaping : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitCheckBoxes() {
      avoidsNodesCb.CheckedChanged += (e, obj) => (_Diagram.ToolManager.LinkReshapingTool as SnapLinkReshapingTool).AvoidsNodes = avoidsNodesCb.Checked;
    }
  }
}
