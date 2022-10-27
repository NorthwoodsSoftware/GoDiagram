/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using Northwoods.Go.Layouts;

namespace Demo.Samples.TLayout {
  public partial class TLayout : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private bool _GetChecked(Avalonia.Controls.RadioButton rb) {
      return (bool)rb.IsChecked;
    }

    private bool _GetChecked(Avalonia.Controls.CheckBox cb) {
      return (bool)cb.IsChecked;
    }

    private void _InitControls() {
      // ComboBoxes
      style.Items = Enum.GetNames(typeof(TreeStyle));
      layerStyle.Items = Enum.GetNames(typeof(TreeLayerStyle));
      align.Items = Enum.GetNames(typeof(TreeAlignment));
      sorting.Items = Enum.GetNames(typeof(TreeSorting));
      altAlign.Items = Enum.GetNames(typeof(TreeAlignment));
      altSorting.Items = Enum.GetNames(typeof(TreeSorting));

      style.SelectedItem = "Layered";
      layerStyle.SelectedItem = "Individual";
      align.SelectedItem = "CenterChildren";
      sorting.SelectedItem = "Forwards";
      altAlign.SelectedItem = "CenterChildren";
      altSorting.SelectedItem = "Forwards";

      style.SelectionChanged += (s, e) => _Layout();
      layerStyle.SelectionChanged += (s, e) => _Layout();
      align.SelectionChanged += (s, e) => _Layout();
      sorting.SelectionChanged += (s, e) => _Layout();
      altAlign.SelectionChanged += (s, e) => _Layout();
      altSorting.SelectionChanged += (s, e) => _Layout();

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
      setsPortSpot.Checked += (s, e) => _Layout();
      setsChildPortSpot.Checked += (s, e) => _Layout();
      altSetsPortSpot.Checked += (s, e) => _Layout();
      altSetsChildPortSpot.Checked += (s, e) => _Layout();

      // RadioButtons
      right.Checked += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      down.Checked += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      left.Checked += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      up.Checked += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      block.Checked += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      none.Checked += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      altRight.Checked += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      altDown.Checked += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      altLeft.Checked += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      altUp.Checked += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      altBlock.Checked += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      altNone.Checked += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
    }

    private void _RadioChanged(Avalonia.Controls.RadioButton rb) {
      if (rb.GroupName == "angle") {  // angle radio changed
        switch (rb.Name) {
          case "right": _Angle = 0; break;
          case "down": _Angle = 90; break;
          case "left": _Angle = 180; break;
          case "up": _Angle = 270; break;
          default: _Angle = 0; break;
        }
      } else if (rb.GroupName == "altAngle") {  // alt angle radio changed
        switch (rb.Name) {
          case "altRight": _AltAngle = 0; break;
          case "altDown": _AltAngle = 90; break;
          case "altLeft": _AltAngle = 180; break;
          case "altUp": _AltAngle = 270; break;
          default: _AltAngle = 0; break;
        }
      } else if (rb.GroupName == "compaction") {  // compaction radio changed
        _Compaction = rb.Name == "block" ? TreeCompaction.Block : TreeCompaction.None;
      } else if (rb.GroupName == "altCompaction") {  // alt compaction radio changed
        _AltCompaction = rb.Name == "altBlock" ? TreeCompaction.Block : TreeCompaction.None;
      }
      _Layout();
    }
  }
}
