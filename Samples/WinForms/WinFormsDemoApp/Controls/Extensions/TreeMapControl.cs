/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System.ComponentModel;

namespace Demo.Extensions.TreeMap {
  [ToolboxItem(false)]
  public partial class TreeMap : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitTextBoxes() {
      minNodes.Leave += (e, obj) => _MinNodes = int.Parse(minNodes.Text);
      maxNodes.Leave += (e, obj) => _MaxNodes = int.Parse(maxNodes.Text);
      minChildren.Leave += (e, obj) => _MinChil = int.Parse(minChildren.Text);
      maxChildren.Leave += (e, obj) => _MaxChil = int.Parse(maxChildren.Text);
    }
  }
}
