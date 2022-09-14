/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.ComponentModel;
using Northwoods.Go;

namespace Demo.Extensions.OrthogonalLinkReshaping {
  [ToolboxItem(false)]
  public partial class OrthogonalLinkReshaping : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitRadioButtons() {
      orthogonalRb.CheckedChanged += (s, e) => {
        if (orthogonalRb.Checked) _UpdateRouting(LinkRouting.Orthogonal);
      };
      avoidsNodesRb.CheckedChanged += (s, e) => {
        if (avoidsNodesRb.Checked) _UpdateRouting(LinkRouting.AvoidsNodes);
      };
    }
  }
}
