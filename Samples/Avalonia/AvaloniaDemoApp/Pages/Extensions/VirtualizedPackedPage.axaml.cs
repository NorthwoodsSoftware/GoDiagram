/* Copyright (c) Northwoods Software Corporation. */

using Northwoods.Go;

namespace Demo.Extensions.VirtualizedPacked {
  public partial class VirtualizedPacked : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private bool _MaybeInvoke(Diagram diagram) {
      var control = diagramControl1;
      if (control != null && !control.Dispatcher.CheckAccess()) {
        control.Dispatcher.InvokeAsync(
          () => _RemoveOffscreen(diagram),
          Avalonia.Threading.DispatcherPriority.Background);
        return true;
      }
      return false;
    }
  }
}
