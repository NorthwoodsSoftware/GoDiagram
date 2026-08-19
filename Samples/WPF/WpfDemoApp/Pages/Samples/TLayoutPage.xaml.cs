/* Copyright (c) Northwoods Software Corporation. */

using System.Windows.Controls;
using Northwoods.Go.Layouts;

namespace Demo.Samples.TLayout; 
public partial class TLayout : DemoControl {
  // This is a stub for the designer.
  // See the SharedSamples project for sample implementation.

  private bool _GetChecked(RadioButton rb) {
    return rb.IsChecked == true;
  }

  private bool _GetChecked(CheckBox cb) {
    return cb.IsChecked == true;
  }

  private void _InitControls() {
    // ComboBoxes
    style.ItemsSource = Enum.GetNames<TreeStyle>();
    layerStyle.ItemsSource = Enum.GetNames<TreeLayerStyle>();
    align.ItemsSource = Enum.GetNames<TreeAlignment>();
    sorting.ItemsSource = Enum.GetNames<TreeSorting>();
    altAlign.ItemsSource = Enum.GetNames<TreeAlignment>();
    altSorting.ItemsSource = Enum.GetNames<TreeSorting>();

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

    // TextBoxes
    nodeSpacing.LostFocus += (s, e) => _Layout();
    nodeIndent.LostFocus += (s, e) => _Layout();
    nodeIndentPastParent.LostFocus += (s, e) => _Layout();
    layerSpacing.LostFocus += (s, e) => _Layout();
    layerSpacingParentOverlap.LostFocus += (s, e) => _Layout();
    breadthLimit.LostFocus += (s, e) => _Layout();
    rowSpacing.LostFocus += (s, e) => _Layout();
    rowIndent.LostFocus += (s, e) => _Layout();
    altNodeSpacing.LostFocus += (s, e) => _Layout();
    altNodeIndent.LostFocus += (s, e) => _Layout();
    altNodeIndentPastParent.LostFocus += (s, e) => _Layout();
    altLayerSpacing.LostFocus += (s, e) => _Layout();
    altLayerSpacingParentOverlap.LostFocus += (s, e) => _Layout();
    altBreadthLimit.LostFocus += (s, e) => _Layout();
    altRowSpacing.LostFocus += (s, e) => _Layout();
    altRowIndent.LostFocus += (s, e) => _Layout();

    // CheckBoxes
    setsPortSpot.Checked += (s, e) => _Layout();
    setsPortSpot.Unchecked += (s, e) => _Layout();
    setsChildPortSpot.Checked += (s, e) => _Layout();
    setsChildPortSpot.Unchecked += (s, e) => _Layout();
    altSetsPortSpot.Checked += (s, e) => _Layout();
    altSetsPortSpot.Unchecked += (s, e) => _Layout();
    altSetsChildPortSpot.Checked += (s, e) => _Layout();
    altSetsChildPortSpot.Unchecked += (s, e) => _Layout();

    // RadioButtons
    right.Checked += (s, e) => _RadioChanged((RadioButton)s);
    down.Checked += (s, e) => _RadioChanged((RadioButton)s);
    left.Checked += (s, e) => _RadioChanged((RadioButton)s);
    up.Checked += (s, e) => _RadioChanged((RadioButton)s);
    block.Checked += (s, e) => _RadioChanged((RadioButton)s);
    none.Checked += (s, e) => _RadioChanged((RadioButton)s);
    altRight.Checked += (s, e) => _RadioChanged((RadioButton)s);
    altDown.Checked += (s, e) => _RadioChanged((RadioButton)s);
    altLeft.Checked += (s, e) => _RadioChanged((RadioButton)s);
    altUp.Checked += (s, e) => _RadioChanged((RadioButton)s);
    altBlock.Checked += (s, e) => _RadioChanged((RadioButton)s);
    altNone.Checked += (s, e) => _RadioChanged((RadioButton)s);
  }

  private void _RadioChanged(RadioButton rb) {
    if (rb.IsChecked != true) return;
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
