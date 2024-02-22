/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System;
using Northwoods.Go;

namespace Demo.Samples.ContentAlign {
  public partial class ContentAlign : DemoControl {
    private Diagram _Diagram;

    public ContentAlign() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      _InitRadioButtons();

      posBtn.Click += (s, e) => ChangePosition(positionXTb.Text, positionYTb.Text);
      scaleBtn.Click += (s, e) => ChangeScale(scaleTb.Text);
      fixedBoundsBtn.Click += (s, e) => ChangeFixedBounds(fixedBoundsXTb.Text, fixedBoundsYTb.Text, fixedBoundsWidthTb.Text, fixedBoundsHeightTb.Text);
      paddingBtn.Click += (s, e) => ChangePadding(paddingTopTb.Text, paddingRightTb.Text, paddingBottomTb.Text, paddingLeftTb.Text);

      zoomFitBtn.Click += (s, e) => _Diagram.CommandHandler.ZoomToFit();

      desc1.MdText = DescriptionReader.Read("Samples.ContentAlign.md");

      Setup();
    }

    private void Setup() {
      // diagram properties
      _Diagram.UndoManager.IsEnabled = true;

      // add random unbound nodes to diagram
      var rand = new Random();
      for (var i = 0; i < 15; i++) {
        _Diagram.Add(
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
      _Diagram.DocumentBoundsChanged += _UpdateUI;
      _Diagram.ViewportBoundsChanged += _UpdateUI;
    }

    //void UpdateDOM(object _, DiagramEvent e) {
    //  if (InvokeRequired) {
    //    Invoke(UpdateDOM, null, e);
    //  }
    //  var d = e.Diagram;
    //  var pos = d.Position;
    //  positionXTb.Text = ((int)pos.X).ToString();
    //  positionY.Text = ((int)pos.Y).ToString();
    //  scale.Text = d.Scale.ToString();
    //  var b = d.DocumentBounds;
    //  docBounds.Text = ToFixed(b.X) + ", " + ToFixed(b.Y) + "  " + ToFixed(b.Width) + " x " + ToFixed(b.Height);;
    //}

    string ToFixed(double a) {
      return (Math.Truncate(a * 100) / 100).ToString();
    }

    // occurs when one of the contentAlign radio buttons is clicked
    private void ChangeContentAlign(Spot spot) {
      _Diagram.Commit((d) => {
        d.ContentAlignment = spot;
      });
    }

    private void ChangePosition(string posx, string posy) {
      if (!double.TryParse(posx, out var x) || !double.TryParse(posy, out var y)) return;
      _Diagram.Commit((d) => {
        d.Position = new Point(x, y);
      });
    }

    private void ChangeScale(string scale) {
      if (!double.TryParse(scale, out var s)) return;
      if (s > 0) {
        _Diagram.Commit((d) => {
          d.Scale = s;
        });
      }
    }

    private void ChangeFixedBounds(string fx, string fy, string fw, string fh) {
      if (!double.TryParse(fx, out var x) || !double.TryParse(fy, out var y) || !int.TryParse(fw, out var w) || !int.TryParse(fh, out var h)) return;
      _Diagram.Commit((d) => {
        d.FixedBounds = new Rect(x, y, Math.Max(1, w), Math.Max(1, h));
      });
    }

    private void ChangePadding(string pt, string pr, string pb, string pl) {
      if (!double.TryParse(pt, out var t) || !double.TryParse(pr, out var r) || !double.TryParse(pb, out var b) || !double.TryParse(pl, out var l)) return;
      _Diagram.Commit((d) => {
        d.Padding = new Margin(t, r, b, l);
      });
    }

    private void ChangeAutoScale(AutoScale scaleType) {
      _Diagram.Commit((d) => {
        d.AutoScale = scaleType;
      });
    }
  }
}
