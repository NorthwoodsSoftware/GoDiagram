/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using Avalonia.Controls;

namespace Demo.Samples.Navigation {
  public partial class Navigation : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitRadioButtons() {
      unhighlightAllRb.Checked += _RadioChanged;
      linksIntoRb.Checked += _RadioChanged;
      linksOutOfRb.Checked += _RadioChanged;
      linksConnectedRb.Checked += _RadioChanged;
      nodesIntoRb.Checked += _RadioChanged;
      nodesOutOfRb.Checked += _RadioChanged;
      nodesConnectedRb.Checked += _RadioChanged;
      nodesReachableRb.Checked += _RadioChanged;
      containingGroupParentRb.Checked += _RadioChanged;
      containingGroupsAllRb.Checked += _RadioChanged;
      memberNodesChildrenRb.Checked += _RadioChanged;
      memberNodesAllRb.Checked += _RadioChanged;
      memberLinksChildrenRb.Checked += _RadioChanged;
      memberLinksAllRb.Checked += _RadioChanged;
    }

    private void _RadioChanged(object sender, EventArgs e) {
      if (sender is RadioButton rb) {
        _SelectedRb = rb.Name;
        _UpdateHighlights(_SelectedRb);
      }
    }
  }
}
