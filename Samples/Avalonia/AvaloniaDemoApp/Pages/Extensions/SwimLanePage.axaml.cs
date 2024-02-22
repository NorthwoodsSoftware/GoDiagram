/* Copyright 1998-2024 by Northwoods Software Corporation. */

namespace Demo.Extensions.SwimLane {
  public partial class SwimLane : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitRadioButtons() {
      conferencesRb.IsCheckedChanged += (s, e) => { if ((bool)conferencesRb.IsChecked) _PartitionBy('c'); };
      divisionsRb.IsCheckedChanged += (s, e) => { if ((bool)divisionsRb.IsChecked) _PartitionBy('d'); };
    }
  }
}
