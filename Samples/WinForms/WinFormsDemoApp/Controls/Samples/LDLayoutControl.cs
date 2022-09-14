/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.ComponentModel;
using Northwoods.Go.Layouts;

namespace Demo.Samples.LDLayout {
  [ToolboxItem(false)]
  public partial class LDLayout : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private bool _GetChecked(System.Windows.Forms.CheckBox cb) {
      return cb.Checked;
    }

    private void _InitControls() {
      // TextBoxes
      layerSpacing.LostFocus += (s, e) => _Layout();
      columnSpacing.LostFocus += (s, e) => _Layout();

      // RadioButtons
      right.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      down.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      left.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      up.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      depthFirst.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      greedy.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      optimalLinkLength.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      longestPathSource.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      longestPathSink.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      depthFirstOut.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      depthFirstIn.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      naive.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      none.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      less.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);
      more.CheckedChanged += (s, e) => _RadioChanged((System.Windows.Forms.RadioButton)s);

      // CheckBoxes
      median.CheckedChanged += (s, e) => _Layout();
      straighten.CheckedChanged += (s, e) => _Layout();
      expand.CheckedChanged += (s, e) => _Layout();
      setsPortSpots.CheckedChanged += (s, e) => _Layout();
    }

    private void _RadioChanged(System.Windows.Forms.RadioButton rb) {
      if (!rb.Checked) return;
      if (rb.Parent == direction) {  // direction radio changed
        switch (rb.Name) {
          case "right": _Direction = 0; break;
          case "down": _Direction = 90; break;
          case "left": _Direction = 180; break;
          case "up": _Direction = 270; break;
          default: _Direction = 0; break;
        }
      } else if (rb.Parent == cycleRemove) {  // cycleRemove radio changed
        switch (rb.Name) {
          case "depthFirst": _CycleRemove = LayeredDigraphCycleRemove.DepthFirst; break;
          case "greedy": _CycleRemove = LayeredDigraphCycleRemove.Greedy; break;
          default: _CycleRemove = LayeredDigraphCycleRemove.DepthFirst; break;
        }
      } else if (rb.Parent == layering) {  // layering radio changed
        switch (rb.Name) {
          case "optimalLinkLength": _Layering = LayeredDigraphLayering.OptimalLinkLength; break;
          case "longestPathSource": _Layering = LayeredDigraphLayering.LongestPathSource; break;
          case "longestPathSink": _Layering = LayeredDigraphLayering.LongestPathSink; break;
          default: _Layering = LayeredDigraphLayering.OptimalLinkLength; break;
        }
      } else if (rb.Parent == initialize) {  // initialize radio changed
        switch (rb.Name) {
          case "depthFirstOut": _Init = LayeredDigraphInit.DepthFirstOut; break;
          case "depthFirstIn": _Init = LayeredDigraphInit.DepthFirstIn; break;
          case "naive": _Init = LayeredDigraphInit.Naive; break;
          default: _Init = LayeredDigraphInit.DepthFirstOut; break;
        }
      } else if (rb.Parent == aggressive) {  // aggressive radio changed
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
