/* Copyright 1998-2024 by Northwoods Software Corporation. */

using Northwoods.Go;

namespace Demo.Samples.ContentAlign {
  public partial class ContentAlign : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitRadioButtons() {
      alignNoneRb.IsCheckedChanged += (s, e) => { if ((bool)alignNoneRb.IsChecked) ChangeContentAlign(Spot.None); };
      alignCenterRb.IsCheckedChanged += (s, e) => { if ((bool)alignCenterRb.IsChecked) ChangeContentAlign(Spot.Center); };
      alignLeftRb.IsCheckedChanged += (s, e) => { if ((bool)alignLeftRb.IsChecked) ChangeContentAlign(Spot.Left); };
      alignRightRb.IsCheckedChanged += (s, e) => { if ((bool)alignRightRb.IsChecked) ChangeContentAlign(Spot.Right); };
      alignTopRb.IsCheckedChanged += (s, e) => { if ((bool)alignTopRb.IsChecked) ChangeContentAlign(Spot.Top); };
      alignBottomRb.IsCheckedChanged += (s, e) => { if ((bool)alignBottomRb.IsChecked) ChangeContentAlign(Spot.Bottom); };

      autoScaleNoneRb.IsCheckedChanged += (s, e) => { if ((bool)autoScaleNoneRb.IsChecked) ChangeAutoScale(AutoScale.None); };
      autoScaleUniformRb.IsCheckedChanged += (s, e) => { if ((bool)autoScaleUniformRb.IsChecked) ChangeAutoScale(AutoScale.Uniform); };
      autoScaleUniformToFillRb.IsCheckedChanged += (s, e) => { if ((bool)autoScaleUniformToFillRb.IsChecked) ChangeAutoScale(AutoScale.UniformToFill); };
    }

    private void _UpdateUI(object sender, DiagramEvent e) {
      var control = diagramControl1;
      if (control != null && !control.CheckAccess()) {
        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(
          () => _UpdateUI(sender, e),
          Avalonia.Threading.DispatcherPriority.Send);
        return;
      }
      var d = e.Diagram;
      var pos = d.Position;
      positionXTb.Text = ((int)pos.X).ToString();
      positionYTb.Text = ((int)pos.Y).ToString();
      scaleTb.Text = d.Scale.ToString();
      var b = d.DocumentBounds;
      documentBoundsTb.Text = ToFixed(b.X) + ", " + ToFixed(b.Y) + "  " + ToFixed(b.Width) + " x " + ToFixed(b.Height); ;
    }
  }
}
