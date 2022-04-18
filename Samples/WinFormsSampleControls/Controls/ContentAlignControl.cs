/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.ComponentModel;
using Northwoods.Go;

namespace WinFormsSampleControls.ContentAlign {
  [ToolboxItem(false)]
  public partial class ContentAlignControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public ContentAlignControl() {
      InitializeComponent();

      myDiagram = diagramControl1.Diagram;
      Setup();

      noneBtn.Click += (e, obj) => ChangeContentAlign(Spot.None);
      centerBtn.Click += (e, obj) => ChangeContentAlign(Spot.Center);
      topBtn.Click += (e, obj) => ChangeContentAlign(Spot.Top);
      bottomBtn.Click += (e, obj) => ChangeContentAlign(Spot.Bottom);
      leftBtn.Click += (e, obj) => ChangeContentAlign(Spot.Left);
      rightBtn.Click += (e, obj) => ChangeContentAlign(Spot.Right);

      positionChangeBtn.Click += (e, obj) => ChangePosition(positionX.Text, positionY.Text);
      scaleChangeBtn.Click += (e, obj) => ChangeScale(scale.Text);

      fixedBoundsSetBtn.Click += (e, obj) => ChangeFixedBounds(fixedX.Text, fixedY.Text, fixedW.Text, fixedH.Text);
      paddingSetBtn.Click += (e, obj) => ChangePadding(padT.Text, padR.Text, pabB.Text, padL.Text);

      autoScaleNoneBtn.Click += (e, obj) => ChangeAutoScale(AutoScale.None);
      autoScaleUniformBtn.Click += (e, obj) => ChangeAutoScale(AutoScale.Uniform);
      autoScaleUTFBtn.Click += (e, obj) => ChangeAutoScale(AutoScale.UniformToFill);

      zoomToFitBtn.Click += (e, obj) => myDiagram.CommandHandler.ZoomToFit();

      goWebBrowser1.Html = @"

           <p>
        A Diagram's <a>Diagram.ContentAlignment</a> property determines how parts are positioned when the
        <a>Diagram.ViewportBounds</a> width or height is different than the <a>Diagram.DocumentBounds</a> width or height.
          </p>
";
    }

    private void Setup() {
      // diagram properties
      myDiagram.UndoManager.IsEnabled = true;

      // add random unbound nodes to diagram
      var rand = new Random();
      for (var i = 0; i < 15; i++) {
        myDiagram.Add(
          new Node {
            Position = new Point(rand.NextDouble() * 251, rand.NextDouble() * 251)
          }.Add(
            new Shape {
              Figure = "Circle",
              Fill = Brush.RandomColor(150),
              StrokeWidth = 2,
              DesiredSize = new Size(30, 30)
            }
          )
        );
      }

      // automatically update information in the panel
      myDiagram.DocumentBoundsChanged += UpdateDOM;
      myDiagram.ViewportBoundsChanged += UpdateDOM;
    }

    void UpdateDOM(object _, DiagramEvent e) {
      if (InvokeRequired) {
        Invoke(UpdateDOM, null, e);
      }
      var d = e.Diagram;
      var pos = d.Position;
      positionX.Text = ((int)pos.X).ToString();
      positionY.Text = ((int)pos.Y).ToString();
      scale.Text = d.Scale.ToString();
      var b = d.DocumentBounds;
      docBounds.Text = ToFixed(b.X) + ", " + ToFixed(b.Y) + "  " + ToFixed(b.Width) + " x " + ToFixed(b.Height);;
    }

    string ToFixed(double a) {
      return (Math.Truncate(a * 100) / 100).ToString();
    }

    // occurs when one of the contentAlign radio buttons is clicked
    private void ChangeContentAlign(Spot spot) {
      myDiagram.Commit((d) => {
        d.ContentAlignment = spot;
      });
    }

    private void ChangePosition(string posx, string posy) {
      if (!double.TryParse(posx, out var x) || !double.TryParse(posy, out var y)) return;
      myDiagram.Commit((d) => {
        d.Position = new Point(x, y);
      });
    }

    private void ChangeScale(string scale) {
      if (!double.TryParse(scale, out var s)) return;
      if (s > 0) {
        myDiagram.Commit((d) => {
          d.Scale = s;
        });
      }
    }

    private void ChangeFixedBounds(string fx, string fy, string fw, string fh) {
      if (!double.TryParse(fx, out var x) || !double.TryParse(fy, out var y) || !int.TryParse(fw, out var w) || !int.TryParse(fh, out var h)) return;
      myDiagram.Commit((d) => {
        d.FixedBounds = new Rect(x, y, Math.Max(1, w), Math.Max(1, h));
      });
    }

    private void ChangePadding(string pt, string pr, string pb, string pl) {
      if (!double.TryParse(pt, out var t) || !double.TryParse(pr, out var r) || !double.TryParse(pb, out var b) || !double.TryParse(pl, out var l)) return;
      myDiagram.Commit((d) => {
        d.Padding = new Margin(t, r, b, l);
      });
    }

    private void ChangeAutoScale(AutoScale scaleType) {
      myDiagram.Commit((d) => {
        d.AutoScale = scaleType;
      });
    }
  }
}
