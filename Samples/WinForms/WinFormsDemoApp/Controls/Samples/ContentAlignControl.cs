/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.ComponentModel;
using Northwoods.Go;

namespace Demo.Samples.ContentAlign {
  [ToolboxItem(false)]
  public partial class ContentAlign : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private void _InitRadioButtons() {
      alignNoneRb.CheckedChanged += (s, e) => { if (alignNoneRb.Checked) ChangeContentAlign(Spot.None); };
      alignCenterRb.CheckedChanged += (s, e) => { if (alignCenterRb.Checked) ChangeContentAlign(Spot.Center); };
      alignLeftRb.CheckedChanged += (s, e) => { if (alignLeftRb.Checked) ChangeContentAlign(Spot.Left); };
      alignRightRb.CheckedChanged += (s, e) => { if (alignRightRb.Checked) ChangeContentAlign(Spot.Right); };
      alignTopRb.CheckedChanged += (s, e) => { if (alignTopRb.Checked) ChangeContentAlign(Spot.Top); };
      alignBottomRb.CheckedChanged += (s, e) => { if (alignBottomRb.Checked) ChangeContentAlign(Spot.Bottom); };

      autoScaleNoneRb.CheckedChanged += (s, e) => { if (autoScaleNoneRb.Checked) ChangeAutoScale(AutoScale.None); };
      autoScaleUniformRb.CheckedChanged += (s, e) => { if (autoScaleUniformRb.Checked) ChangeAutoScale(AutoScale.Uniform); };
      autoScaleUniformToFillRb.CheckedChanged += (s, e) => { if (autoScaleUniformToFillRb.Checked) ChangeAutoScale(AutoScale.UniformToFill); };
    }

    private void _UpdateUI(object sender, DiagramEvent e) {
      if (InvokeRequired) {
        Invoke(_UpdateUI, null, e);
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
