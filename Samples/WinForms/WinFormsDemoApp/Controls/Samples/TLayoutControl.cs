/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.ComponentModel;
using Northwoods.Go.Layouts;

namespace Demo.Samples.TLayout {
  [ToolboxItem(false)]
  public partial class TLayout : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private bool _GetChecked(System.Windows.Forms.CheckBox cb) {
      return cb.Checked;
    }

    private void _InitControls() {
      // ComboBoxes
      style.DataSource = Enum.GetNames(typeof(TreeStyle));
      layerStyle.DataSource = Enum.GetNames(typeof(TreeLayerStyle));
      align.DataSource = Enum.GetNames(typeof(TreeAlignment));
      sorting.DataSource = Enum.GetNames(typeof(TreeSorting));
      altAlign.DataSource = Enum.GetNames(typeof(TreeAlignment));
      altSorting.DataSource = Enum.GetNames(typeof(TreeSorting));

      style.SelectedItem = "Layered";
      layerStyle.SelectedItem = "Individual";
      align.SelectedItem = "CenterChildren";
      sorting.SelectedItem = "Forwards";
      altAlign.SelectedItem = "CenterChildren";
      altSorting.SelectedItem = "Forwards";

      style.SelectedIndexChanged += (s, e) => _Layout();
      layerStyle.SelectedIndexChanged += (s, e) => _Layout();
      align.SelectedIndexChanged += (s, e) => _Layout();
      sorting.SelectedIndexChanged += (s, e) => _Layout();
      altAlign.SelectedIndexChanged += (s, e) => _Layout();
      altSorting.SelectedIndexChanged += (s, e) => _Layout();

      // NumericUpDowns
      nodeSpacing.ValueChanged += (s, e) => _Layout();
      nodeIndent.ValueChanged += (s, e) => _Layout();
      nodeIndentPastParent.ValueChanged += (s, e) => _Layout();
      layerSpacing.ValueChanged += (s, e) => _Layout();
      layerSpacingParentOverlap.ValueChanged += (s, e) => _Layout();
      breadthLimit.ValueChanged += (s, e) => _Layout();
      rowSpacing.ValueChanged += (s, e) => _Layout();
      rowIndent.ValueChanged += (s, e) => _Layout();
      altNodeSpacing.ValueChanged += (s, e) => _Layout();
      altNodeIndent.ValueChanged += (s, e) => _Layout();
      altNodeIndentPastParent.ValueChanged += (s, e) => _Layout();
      altLayerSpacing.ValueChanged += (s, e) => _Layout();
      altLayerSpacingParentOverlap.ValueChanged += (s, e) => _Layout();
      altBreadthLimit.ValueChanged += (s, e) => _Layout();
      altRowSpacing.ValueChanged += (s, e) => _Layout();
      altRowIndent.ValueChanged += (s, e) => _Layout();

      // CheckBoxes
      setsPortSpot.CheckedChanged += (s, e) => _Layout();
      setsChildPortSpot.CheckedChanged += (s, e) => _Layout();
      altSetsPortSpot.CheckedChanged += (s, e) => _Layout();
      altSetsChildPortSpot.CheckedChanged += (s, e) => _Layout();

      // RadioButtons
      right.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      down.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      left.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      right.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      block.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      none.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      altRight.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      altDown.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      altLeft.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      altRight.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      altBlock.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      altNone.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
    }

    private void _RadioChanged(System.Windows.Forms.RadioButton rb) {
      if (!rb.Checked) return;
      if (rb.Parent == angle) {  // angle radio changed
        switch (rb.Name) {
          case "right": _Angle = 0; break;
          case "down": _Angle = 90; break;
          case "left": _Angle = 180; break;
          case "up": _Angle = 270; break;
          default: _Angle = 0; break;
        }
      } else if (rb.Parent == altAngle) {  // alt angle radio changed
        switch (rb.Name) {
          case "altRight": _AltAngle = 0; break;
          case "altDown": _AltAngle = 90; break;
          case "altLeft": _AltAngle = 180; break;
          case "altUp": _AltAngle = 270; break;
          default: _AltAngle = 0; break;
        }
      } else if (rb.Parent == compaction) {  // compaction radio changed
        _Compaction = rb.Name == "block" ? TreeCompaction.Block : TreeCompaction.None;
      } else if (rb.Parent == altCompaction) {  // alt compaction radio changed
        _AltCompaction = rb.Name == "altBlock" ? TreeCompaction.Block : TreeCompaction.None;
      }
      _Layout();
    }
  }
}
