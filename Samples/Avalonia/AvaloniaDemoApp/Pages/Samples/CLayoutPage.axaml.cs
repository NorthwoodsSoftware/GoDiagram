/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using Northwoods.Go.Layouts;

namespace Demo.Samples.CLayout {
  public partial class CLayout : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private bool _GetChecked(Avalonia.Controls.CheckBox cb) {
      return (bool)cb.IsChecked;
    }

    private void _InitControls() {
      // TextBoxes
      radius.LostFocus += (s, e) => _Layout();
      aspectRatio.LostFocus += (s, e) => _Layout();
      startAngle.LostFocus += (s, e) => _Layout();
      sweepAngle.LostFocus += (s, e) => _Layout();
      spacing.LostFocus += (s, e) => _Layout();

      // ComboBoxes
      arrangement.Items = Enum.GetNames(typeof(CircularArrangement));
      direction.Items = Enum.GetNames(typeof(CircularDirection));
      sorting.Items = Enum.GetNames(typeof(CircularSorting));

      arrangement.SelectedItem = "ConstantSpacing";
      direction.SelectedItem = "Clockwise";
      sorting.SelectedItem = "Forwards";

      arrangement.SelectionChanged += (s, e) => _Layout();
      direction.SelectionChanged += (s, e) => _Layout();
      sorting.SelectionChanged += (s, e) => _Layout();

      // RadioButtons
      pythagorean.Checked += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      circular.Checked += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
    }

    private void _RadioChanged(Avalonia.Controls.RadioButton rb) {
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
}
