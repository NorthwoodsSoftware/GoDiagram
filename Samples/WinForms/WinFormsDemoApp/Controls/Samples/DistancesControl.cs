/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.ComponentModel;

namespace Demo.Samples.Distances {
  [ToolboxItem(false)]
  public partial class Distances : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private BindingList<string> Paths { get; set; } = new();

    private void _RebuildList() {
      Paths.Clear();
      foreach (var path in myPaths) {
        Paths.Add(path);
      }
    }
  }
}
