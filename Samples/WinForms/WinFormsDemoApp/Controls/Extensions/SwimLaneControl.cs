/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System.ComponentModel;

namespace Demo.Extensions.SwimLane {
  [ToolboxItem(false)]
  public partial class SwimLane : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitRadioButtons() {
      conferencesRb.CheckedChanged += (s, e) => { if (conferencesRb.Checked) _PartitionBy('c'); };
      divisionsRb.CheckedChanged += (s, e) => { if (divisionsRb.Checked) _PartitionBy('d'); };
    }
  }
}
