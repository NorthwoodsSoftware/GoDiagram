/* Copyright (c) Northwoods Software Corporation. */

using System.Windows.Threading;
using Northwoods.Go;

namespace Demo.Extensions.VirtualizedPacked; 
public partial class VirtualizedPacked : DemoControl {
  // This is a stub for the designer.
  // See the SharedSamples project for sample implementation.

  private bool _MaybeInvoke(Diagram diagram) {
    var control = diagramControl1;
    if (control != null && !control.CheckAccess()) {
      control.Dispatcher.InvokeAsync(
        () => _RemoveOffscreen(diagram),
        DispatcherPriority.Background);
      return true;
    }
    return false;
  }
}
