/* Copyright 1998-2023 by Northwoods Software Corporation. */

using Northwoods.Go;

namespace Demo.Samples.ContentAlign {
  public partial class ContentAlign : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitRadioButtons() {
      alignNoneRb.Checked += (s, e) => ChangeContentAlign(Spot.None);
      alignCenterRb.Checked += (s, e) => ChangeContentAlign(Spot.Center);
      alignLeftRb.Checked += (s, e) => ChangeContentAlign(Spot.Left);
      alignRightRb.Checked += (s, e) => ChangeContentAlign(Spot.Right);
      alignTopRb.Checked += (s, e) => ChangeContentAlign(Spot.Top);
      alignBottomRb.Checked += (s, e) => ChangeContentAlign(Spot.Bottom);

      autoScaleNoneRb.Checked += (s, e) => ChangeAutoScale(AutoScale.None);
      autoScaleUniformRb.Checked += (s, e) => ChangeAutoScale(AutoScale.Uniform);
      autoScaleUniformToFillRb.Checked += (s, e) => ChangeAutoScale(AutoScale.UniformToFill);
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
