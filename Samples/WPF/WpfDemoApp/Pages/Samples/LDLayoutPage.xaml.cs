/* Copyright (c) Northwoods Software Corporation. */

using System.Windows.Controls;
using Northwoods.Go.Layouts;

namespace Demo.Samples.LDLayout; 
public partial class LDLayout : DemoControl {
  // This is a stub for the designer.
  // See the SharedSamples project for sample implementation.

  private bool _GetChecked(CheckBox cb) {
    return cb.IsChecked == true;
  }

  private void _InitControls() {
    // TextBoxes
    layerSpacing.LostFocus += (s, e) => _Layout();
    columnSpacing.LostFocus += (s, e) => _Layout();

    // RadioButtons
    right.Checked += (s, e) => _RadioChanged((RadioButton)s);
    down.Checked += (s, e) => _RadioChanged((RadioButton)s);
    left.Checked += (s, e) => _RadioChanged((RadioButton)s);
    up.Checked += (s, e) => _RadioChanged((RadioButton)s);
    depthFirst.Checked += (s, e) => _RadioChanged((RadioButton)s);
    greedy.Checked += (s, e) => _RadioChanged((RadioButton)s);
    optimalLinkLength.Checked += (s, e) => _RadioChanged((RadioButton)s);
    longestPathSource.Checked += (s, e) => _RadioChanged((RadioButton)s);
    longestPathSink.Checked += (s, e) => _RadioChanged((RadioButton)s);
    depthFirstOut.Checked += (s, e) => _RadioChanged((RadioButton)s);
    depthFirstIn.Checked += (s, e) => _RadioChanged((RadioButton)s);
    naive.Checked += (s, e) => _RadioChanged((RadioButton)s);
    none.Checked += (s, e) => _RadioChanged((RadioButton)s);
    less.Checked += (s, e) => _RadioChanged((RadioButton)s);
    more.Checked += (s, e) => _RadioChanged((RadioButton)s);

    // CheckBoxes
    median.Checked += (s, e) => _Layout();
    median.Unchecked += (s, e) => _Layout();
    straighten.Checked += (s, e) => _Layout();
    straighten.Unchecked += (s, e) => _Layout();
    expand.Checked += (s, e) => _Layout();
    expand.Unchecked += (s, e) => _Layout();
    upperLeft.Checked += (s, e) => _AlignChanged();
    upperLeft.Unchecked += (s, e) => _AlignChanged();
    upperRight.Checked += (s, e) => _AlignChanged();
    upperRight.Unchecked += (s, e) => _AlignChanged();
    lowerLeft.Checked += (s, e) => _AlignChanged();
    lowerLeft.Unchecked += (s, e) => _AlignChanged();
    lowerRight.Checked += (s, e) => _AlignChanged();
    lowerRight.Unchecked += (s, e) => _AlignChanged();
    setsPortSpots.Checked += (s, e) => _Layout();
    setsPortSpots.Unchecked += (s, e) => _Layout();
  }

  private void _RadioChanged(RadioButton rb) {
    if (rb.IsChecked != true) return;
    if (rb.GroupName == "direction") {  // direction radio changed
      switch (rb.Name) {
        case "right": _Direction = 0; break;
        case "down": _Direction = 90; break;
        case "left": _Direction = 180; break;
        case "up": _Direction = 270; break;
        default: _Direction = 0; break;
      }
    } else if (rb.GroupName == "cycleRemove") {  // cycleRemove radio changed
      switch (rb.Name) {
        case "depthFirst": _CycleRemove = LayeredDigraphCycleRemove.DepthFirst; break;
        case "greedy": _CycleRemove = LayeredDigraphCycleRemove.Greedy; break;
        default: _CycleRemove = LayeredDigraphCycleRemove.DepthFirst; break;
      }
    } else if (rb.GroupName == "layering") {  // layering radio changed
      switch (rb.Name) {
        case "optimalLinkLength": _Layering = LayeredDigraphLayering.OptimalLinkLength; break;
        case "longestPathSource": _Layering = LayeredDigraphLayering.LongestPathSource; break;
        case "longestPathSink": _Layering = LayeredDigraphLayering.LongestPathSink; break;
        default: _Layering = LayeredDigraphLayering.OptimalLinkLength; break;
      }
    } else if (rb.GroupName == "initialize") {  // initialize radio changed
      switch (rb.Name) {
        case "depthFirstOut": _Init = LayeredDigraphInit.DepthFirstOut; break;
        case "depthFirstIn": _Init = LayeredDigraphInit.DepthFirstIn; break;
        case "naive": _Init = LayeredDigraphInit.Naive; break;
        default: _Init = LayeredDigraphInit.DepthFirstOut; break;
      }
    } else if (rb.GroupName == "aggressive") {  // aggressive radio changed
      switch (rb.Name) {
        case "none": _Aggressive = LayeredDigraphAggressive.None; break;
        case "less": _Aggressive = LayeredDigraphAggressive.Less; break;
        case "more": _Aggressive = LayeredDigraphAggressive.More; break;
        default: _Aggressive = LayeredDigraphAggressive.Less; break;
      }
    }
    _Layout();
  }

  private void _AlignChanged() {
    if (_GetChecked(upperLeft) || _GetChecked(upperRight) || _GetChecked(lowerLeft) || _GetChecked(lowerRight)) {
      _SetPackEnabled(false);
    } else {
      _SetPackEnabled(true);
    }
    _Layout();
  }

  private void _SetPackEnabled(bool value) {
    median.IsEnabled = value;
    straighten.IsEnabled = value;
    expand.IsEnabled = value;
  }
}
