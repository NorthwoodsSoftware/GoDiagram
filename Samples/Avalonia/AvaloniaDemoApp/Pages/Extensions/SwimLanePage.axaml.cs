/* Copyright 1998-2022 by Northwoods Software Corporation. */

namespace Demo.Extensions.SwimLane {
  public partial class SwimLane : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitRadioButtons() {
      conferencesRb.Checked += (s, e) => _PartitionBy('c');
      divisionsRb.Checked += (s, e) => _PartitionBy('d');
    }
  }
}
