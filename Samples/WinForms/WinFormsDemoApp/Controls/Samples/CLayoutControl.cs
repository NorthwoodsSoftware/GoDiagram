/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System;
using System.ComponentModel;
using Northwoods.Go.Layouts;

namespace Demo.Samples.CLayout {
  [ToolboxItem(false)]
  public partial class CLayout : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private bool _GetChecked(System.Windows.Forms.CheckBox cb) {
      return cb.Checked;
    }

    private void _InitControls() {
      // TextBoxes
      radius.Leave += (s, e) => _Layout();
      aspectRatio.Leave += (s, e) => _Layout();
      startAngle.Leave += (s, e) => _Layout();
      sweepAngle.Leave += (s, e) => _Layout();
      spacing.Leave += (s, e) => _Layout();

      // ComboBoxes
      arrangement.DataSource = Enum.GetNames(typeof(CircularArrangement));
      direction.DataSource = Enum.GetNames(typeof(CircularDirection));
      sorting.DataSource = Enum.GetNames(typeof(CircularSorting));

      arrangement.SelectedItem = "ConstantSpacing";
      direction.SelectedItem = "Clockwise";
      sorting.SelectedItem = "Forwards";

      arrangement.SelectedIndexChanged += (s, e) => _Layout();
      direction.SelectedIndexChanged += (s, e) => _Layout();
      sorting.SelectedIndexChanged += (s, e) => _Layout();

      // RadioButtons
      pythagorean.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      circular.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
    }

    private void _RadioChanged(System.Windows.Forms.RadioButton rb) {
      if (!rb.Checked) return;
      if (rb.Parent == diamFormula) {  // diamFormula radio changed
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
