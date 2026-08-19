/* Copyright (c) Northwoods Software Corporation. */

using System.Windows.Controls;
using Northwoods.Go.Layouts;

namespace Demo.Samples.GLayout; 
public partial class GLayout : DemoControl {
  // This is a stub for the designer.
  // See the SharedSamples project for sample implementation.

  private bool _GetChecked(RadioButton rb) {
    return rb.IsChecked == true;
  }

  private void _InitControls() {
    wrapColTb.LostFocus += (s, e) => _Layout();
    wrapWidthTb.LostFocus += (s, e) => _Layout();
    cellSizeTb.LostFocus += (s, e) => _Layout();
    spacingTb.LostFocus += (s, e) => _Layout();

    alignPosRb.Checked += (s, e) => _Layout();
    alignPosRb.Unchecked += (s, e) => _Layout();
    alignLocRb.Checked += (s, e) => _Layout();
    alignLocRb.Unchecked += (s, e) => _Layout();
    arrangeLTRRb.Checked += (s, e) => _Layout();
    arrangeLTRRb.Unchecked += (s, e) => _Layout();
    arrangeRTLRb.Checked += (s, e) => _Layout();
    arrangeRTLRb.Unchecked += (s, e) => _Layout();

    sortingCb.ItemsSource = Enum.GetNames(typeof(GridSorting));
    sortingCb.SelectedItem = "Forwards";
    sortingCb.SelectionChanged += (s, e) => _Layout();
  }
}
