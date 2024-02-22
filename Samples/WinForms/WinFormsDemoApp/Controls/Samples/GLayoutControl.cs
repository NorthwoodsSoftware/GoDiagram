/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System;
using System.ComponentModel;
using Northwoods.Go.Layouts;

namespace Demo.Samples.GLayout {
  [ToolboxItem(false)]
  public partial class GLayout : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private bool _GetChecked(System.Windows.Forms.RadioButton rb) {
      return rb.Checked;
    }

    private void _InitControls() {
      wrapColTb.Leave += (s, e) => _Layout();
      wrapWidthTb.Leave += (s, e) => _Layout();
      cellSizeTb.Leave += (s, e) => _Layout();
      spacingTb.Leave += (s, e) => _Layout();

      alignPosRb.CheckedChanged += (s, e) => _Layout();
      alignLocRb.CheckedChanged += (s, e) => _Layout();
      arrangeLTRRb.CheckedChanged += (s, e) => _Layout();
      arrangeRTLRb.CheckedChanged += (s, e) => _Layout();

      sortingCb.DataSource = Enum.GetNames(typeof(GridSorting));
      sortingCb.SelectedIndexChanged += (s, e) => _Layout();
    }
  }
}
