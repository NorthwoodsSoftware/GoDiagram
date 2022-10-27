/* Copyright 1998-2022 by Northwoods Software Corporation. */

namespace Demo.Extensions.TreeMap {
  public partial class TreeMap : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitTextBoxes() {
      minNodes.LostFocus += (e, obj) => _MinNodes = int.Parse(minNodes.Text);
      maxNodes.LostFocus += (e, obj) => _MaxNodes = int.Parse(maxNodes.Text);
      minChildren.LostFocus += (e, obj) => _MinChil = int.Parse(minChildren.Text);
      maxChildren.LostFocus += (e, obj) => _MaxChil = int.Parse(maxChildren.Text);
    }
  }
}
