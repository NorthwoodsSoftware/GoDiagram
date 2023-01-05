/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System.ComponentModel;
using Northwoods.Go;

namespace Demo.Extensions.VirtualizedPacked {
  [ToolboxItem(false)]
  public partial class VirtualizedPacked : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private bool _MaybeInvoke(Diagram diagram) {
      var control = diagramControl1;
      if (control != null && control.InvokeRequired) {
        control.Invoke(() => _RemoveOffscreen(diagram));
        return true;
      }
      return false;
    }
  }
}
