/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using Northwoods.Go.Layouts;

namespace Demo.Samples.GLayout {
  public partial class GLayout : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private bool _GetChecked(Avalonia.Controls.RadioButton rb) {
      return (bool)rb.IsChecked;
    }

    private void _InitControls() {
      wrapColTb.LostFocus += (s, e) => _Layout();
      wrapWidthTb.LostFocus += (s, e) => _Layout();
      cellSizeTb.LostFocus += (s, e) => _Layout();
      spacingTb.LostFocus += (s, e) => _Layout();

      alignPosRb.Checked += (s, e) => _Layout();
      alignLocRb.Checked += (s, e) => _Layout();
      arrangeLTRRb.Checked += (s, e) => _Layout();
      arrangeRTLRb.Checked += (s, e) => _Layout();

      sortingCb.Items = Enum.GetNames(typeof(GridSorting));
      sortingCb.SelectedItem = "Forwards";
      sortingCb.SelectionChanged += (s, e) => _Layout();
    }
  }
}
