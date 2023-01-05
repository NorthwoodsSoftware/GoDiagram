/* Copyright 1998-2023 by Northwoods Software Corporation. */

using Northwoods.Go;

namespace Demo.Extensions.OrthogonalLinkReshaping {
  public partial class OrthogonalLinkReshaping : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitRadioButtons() {
      orthogonalRb.Checked += (s, e) => _UpdateRouting(LinkRouting.Orthogonal);
      avoidsNodesRb.Checked += (s, e) => _UpdateRouting(LinkRouting.AvoidsNodes);
    }
  }
}
