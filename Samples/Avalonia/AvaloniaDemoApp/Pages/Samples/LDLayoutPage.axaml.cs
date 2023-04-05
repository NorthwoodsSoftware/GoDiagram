/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using Northwoods.Go.Layouts;

namespace Demo.Samples.LDLayout {
  public partial class LDLayout : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private bool _GetChecked(Avalonia.Controls.CheckBox cb) {
      return (bool)cb.IsChecked;
    }

    private void _InitControls() {
      // TextBoxes
      layerSpacing.LostFocus += (s, e) => _Layout();
      columnSpacing.LostFocus += (s, e) => _Layout();

      // RadioButtons
      right.IsCheckedChanged += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      down.IsCheckedChanged += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      left.IsCheckedChanged += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      up.IsCheckedChanged += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      depthFirst.IsCheckedChanged += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      greedy.IsCheckedChanged += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      optimalLinkLength.IsCheckedChanged += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      longestPathSource.IsCheckedChanged += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      longestPathSink.IsCheckedChanged += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      depthFirstOut.IsCheckedChanged += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      depthFirstIn.IsCheckedChanged += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      naive.IsCheckedChanged += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      none.IsCheckedChanged += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      less.IsCheckedChanged += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);
      more.IsCheckedChanged += (s, e) => _RadioChanged((Avalonia.Controls.RadioButton)s);

      // CheckBoxes
      median.IsCheckedChanged += (s, e) => _Layout();
      straighten.IsCheckedChanged += (s, e) => _Layout();
      expand.IsCheckedChanged += (s, e) => _Layout();
      setsPortSpots.IsCheckedChanged += (s, e) => _Layout();
    }

    private void _RadioChanged(Avalonia.Controls.RadioButton rb) {
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
  }
}
