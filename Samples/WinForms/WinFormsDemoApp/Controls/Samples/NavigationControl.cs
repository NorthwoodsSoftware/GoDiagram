/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using System.ComponentModel;

namespace Demo.Samples.Navigation {
  [ToolboxItem(false)]
  public partial class Navigation : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitRadioButtons() {
      unhighlightAllRb.CheckedChanged += _RadioChanged;
      linksIntoRb.CheckedChanged += _RadioChanged;
      linksOutOfRb.CheckedChanged += _RadioChanged;
      linksConnectedRb.CheckedChanged += _RadioChanged;
      nodesIntoRb.CheckedChanged += _RadioChanged;
      nodesOutOfRb.CheckedChanged += _RadioChanged;
      nodesConnectedRb.CheckedChanged += _RadioChanged;
      nodesReachableRb.CheckedChanged += _RadioChanged;
      containingGroupParentRb.CheckedChanged += _RadioChanged;
      containingGroupsAllRb.CheckedChanged += _RadioChanged;
      memberNodesChildrenRb.CheckedChanged += _RadioChanged;
      memberNodesAllRb.CheckedChanged += _RadioChanged;
      memberLinksChildrenRb.CheckedChanged += _RadioChanged;
      memberLinksAllRb.CheckedChanged += _RadioChanged;
    }

    private void _RadioChanged(object sender, EventArgs e) {
      if (sender is System.Windows.Forms.RadioButton rb && rb.Checked) {
        _SelectedRb = rb.Name;
        _UpdateHighlights(_SelectedRb);
      }
    }
  }
}
