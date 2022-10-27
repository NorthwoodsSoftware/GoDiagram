/* Copyright 1998-2022 by Northwoods Software Corporation. */

using Northwoods.Go;

namespace Demo.Extensions.VirtualizedPacked {
  public partial class VirtualizedPacked : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private bool _MaybeInvoke(Diagram diagram) {
      var control = diagramControl1;
      if (control != null && !control.CheckAccess()) {
        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(
          () => _RemoveOffscreen(diagram),
          Avalonia.Threading.DispatcherPriority.Background);
        return true;
      }
      return false;
    }
  }
}
