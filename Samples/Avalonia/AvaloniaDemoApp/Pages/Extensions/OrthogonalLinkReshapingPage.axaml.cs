/* Copyright 1998-2024 by Northwoods Software Corporation. */

using Northwoods.Go;

namespace Demo.Extensions.OrthogonalLinkReshaping {
  public partial class OrthogonalLinkReshaping : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitRadioButtons() {
      orthogonalRb.IsCheckedChanged += (s, e) => {
        if ((bool)orthogonalRb.IsChecked) _UpdateRouting(LinkRouting.Orthogonal);
      };
      avoidsNodesRb.IsCheckedChanged += (s, e) => {
        if ((bool)avoidsNodesRb.IsChecked) _UpdateRouting(LinkRouting.AvoidsNodes);
      };
    }
  }
}
