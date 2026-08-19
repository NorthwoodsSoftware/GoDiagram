/* Copyright (c) Northwoods Software Corporation. */

using System.Windows.Controls;
using Northwoods.Go.Layouts;

namespace Demo.Samples.CLayout; 
public partial class CLayout : DemoControl {
  // This is a stub for the designer.
  // See the SharedSamples project for sample implementation.

  private bool _GetChecked(CheckBox cb) {
    return cb.IsChecked == true;
  }

  private void _InitControls() {
    // TextBoxes
    radius.LostFocus += (s, e) => _Layout();
    aspectRatio.LostFocus += (s, e) => _Layout();
    startAngle.LostFocus += (s, e) => _Layout();
    sweepAngle.LostFocus += (s, e) => _Layout();
    spacing.LostFocus += (s, e) => _Layout();

    // ComboBoxes
    arrangement.ItemsSource = Enum.GetNames<CircularArrangement>();
    direction.ItemsSource = Enum.GetNames<CircularDirection>();
    sorting.ItemsSource = Enum.GetNames<CircularSorting>();

    arrangement.SelectedItem = "ConstantSpacing";
    direction.SelectedItem = "Clockwise";
    sorting.SelectedItem = "Forwards";

    arrangement.SelectionChanged += (s, e) => _Layout();
    direction.SelectionChanged += (s, e) => _Layout();
    sorting.SelectionChanged += (s, e) => _Layout();

    // RadioButtons
    pythagorean.Checked += (s, e) => _RadioChanged((RadioButton)s);
    pythagorean.Unchecked += (s, e) => _RadioChanged((RadioButton)s);
    circular.Checked += (s, e) => _RadioChanged((RadioButton)s);
    circular.Unchecked += (s, e) => _RadioChanged((RadioButton)s);
  }

  private void _RadioChanged(RadioButton rb) {
    if (rb.IsChecked != true) return;
    if (rb.GroupName == "diamFormula") {  // diamFormula radio changed
      switch (rb.Name) {
        case "pythagorean": _DiamFormula = CircularNodeDiameterFormula.Pythagorean; break;
        case "circular": _DiamFormula = CircularNodeDiameterFormula.Circular; break;
        default: _DiamFormula = CircularNodeDiameterFormula.Pythagorean; break;
      }
    }
    _Layout();
  }
}
