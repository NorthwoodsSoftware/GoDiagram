/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using Avalonia.Controls;

namespace Demo.Samples.Navigation {
  public partial class Navigation : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitRadioButtons() {
      unhighlightAllRb.IsCheckedChanged += _RadioChanged;
      linksIntoRb.IsCheckedChanged += _RadioChanged;
      linksOutOfRb.IsCheckedChanged += _RadioChanged;
      linksConnectedRb.IsCheckedChanged += _RadioChanged;
      nodesIntoRb.IsCheckedChanged += _RadioChanged;
      nodesOutOfRb.IsCheckedChanged += _RadioChanged;
      nodesConnectedRb.IsCheckedChanged += _RadioChanged;
      nodesReachableRb.IsCheckedChanged += _RadioChanged;
      containingGroupParentRb.IsCheckedChanged += _RadioChanged;
      containingGroupsAllRb.IsCheckedChanged += _RadioChanged;
      memberNodesChildrenRb.IsCheckedChanged += _RadioChanged;
      memberNodesAllRb.IsCheckedChanged += _RadioChanged;
      memberLinksChildrenRb.IsCheckedChanged += _RadioChanged;
      memberLinksAllRb.IsCheckedChanged += _RadioChanged;
    }

    private void _RadioChanged(object sender, EventArgs e) {
      if (sender is RadioButton rb && rb.IsChecked == true) {
        _SelectedRb = rb.Name;
        _UpdateHighlights(_SelectedRb);
      }
    }
  }
}
