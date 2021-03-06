/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Extensions;
using Northwoods.Go.Models;

namespace WinFormsExtensionControls.Dimensioning {
  [ToolboxItem(false)]
  public partial class DimensioningControl : System.Windows.Forms.UserControl {
    private Diagram MyDiagram;

    public DimensioningControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"

   <p>
      This sample makes use of the DimensioningLink class, which inherits from the <a>Link</a> class.
      That class is defined at <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/GraphObjects/DimensioningLink/DimensioningLink.cs"">DimensioningLink.cs</a>.
   </p>
";

    }

    private void Setup() {

      MyDiagram = diagramControl1.Diagram;
      MyDiagram.UndoManager.IsEnabled = true;

      // a simple resizable node
      MyDiagram.NodeTemplate = new Node(PanelLayoutAuto.Instance) {
        LocationSpot = Spot.Center, Resizable = true
      }.Bind(new Binding("Location", "Loc", Point.Parse, Point.Stringify))
      .Add(
       new Shape {
         StrokeWidth = 0, Fill = "lightgray"
       }.Bind("Fill", "Color"),
       new TextBlock {
         Margin = 10
       }.Bind("Text", "Key")
      );

      // A generalized example template using a DimensioningLink.
      // Most usage might not have so many bindings.
      MyDiagram.LinkTemplateMap.Add("Dimensioning",
        new DimensioningLink().Bind(
          new Binding("FromSpot", "FromSpot", Spot.Parse),
          new Binding("ToSpot", "ToSpot", Spot.Parse),
          new Binding("Direction"),
          new Binding("Extension"),
          new Binding("Inset"))
        .Add(
          new Shape { Stroke = "gray" }.Bind("Stroke", "Color"),
          new Shape { FromArrow = "BackwardOpenTriangle", SegmentIndex = 2, Stroke = "gray" }
            .Bind("Stroke", "Color"),
          new Shape { ToArrow = "OpenTriangle", SegmentIndex = -3, Stroke = "gray" }
            .Bind("Stroke", "Color"),
          new TextBlock {
            SegmentIndex = 2,
            SegmentFraction = 0.5,
            SegmentOrientation = Orientation.Upright,
            AlignmentFocus = Spot.Bottom,
            Stroke = "gray",
            Font = new Font("Microsoft Sans Serif", 10)
          }.Bind(
            new Binding("Text", "", showDistance).OfElement(),
            new Binding("Stroke", "Color")
          )
        )
      );

      // Return a string representing the distance between the two points.
      // This is the cartesian distance if this.Direction is NaN;
      // otherwise it is the orthogonal distance along that axis.
      static string showDistance(object _link, object obj) {
        var link = (DimensioningLink)_link;
        var numpts = link.PointsCount;
        if (numpts < 2) return "";
        var p0 = link.GetPoint(0);
        var pn = link.GetPoint(numpts - 1);
        var ang = link.Direction;
        if (double.IsNaN(ang)) return Math.Floor(Math.Sqrt(p0.DistanceSquared(pn))) + "";
        var rad = ang * Math.PI / 180;
        return Math.Floor(Math.Abs(Math.Cos(rad) * (p0.X - pn.X)) +
          Math.Abs(Math.Sin(rad) * (p0.Y - pn.Y))) + "";
      }

      MyDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "Alpha", Loc = "0 50" },
          new NodeData { Key = "Beta", Loc = "150 0" },
          new NodeData { Key = "Gamma", Loc = "100 150" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData {
            From = "Alpha", To = "Beta", Category = "Dimensioning",
            FromSpot = "TopRight", ToSpot = "TopLeft"
          },
          new LinkData {
            From = "Alpha", To = "Beta", Category = "Dimensioning",
            FromSpot = "TopLeft", ToSpot = "TopRight", Extension = 50, Color = "blue"
          },
          new LinkData {
            From = "Alpha", To = "Beta", Category = "Dimensioning",
            FromSpot = "TopLeft", ToSpot = "TopLeft", Direction = 270, Color = "green"
          },
          new LinkData {
            From = "Alpha", To = "Beta", Category = "Dimensioning",
            FromSpot = "BottomRight", ToSpot = "BottomRight", Direction = 90, Color = "purple"
          },
          new LinkData {
            From = "Alpha", To = "Beta", Category = "Dimensioning",
            FromSpot = "Center", ToSpot = "Center", Extension = 50, Direction = double.NaN, Color = "red"
          },
          new LinkData {
            From = "Gamma", To = "Gamma", Category = "Dimensioning",
            FromSpot = "TopLeft", ToSpot = "TopRight", Direction = 0
          },
          new LinkData {
            From = "Gamma", To = "Gamma", Category = "Dimensioning",
            FromSpot = "TopRight", ToSpot = "BottomRight", Direction = 90
          },
          new LinkData {
            From = "Gamma", To = "Gamma", Category = "Dimensioning",
            FromSpot = "BottomRight", ToSpot = "BottomLeft", Direction = 180
          },
          new LinkData {
            From = "Gamma", To = "Gamma", Category = "Dimensioning",
            FromSpot = "BottomLeft", ToSpot = "TopLeft", Direction = 270
          }
        }
      };
    }

  }

  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }

  public class NodeData : Model.NodeData {
    public string Loc { get; set; }
    public string Color { get; set; }
  }

  public class LinkData : Model.LinkData {
    public string FromSpot { get; set; }
    public string ToSpot { get; set; }
    public string Color { get; set; }
    public double Direction { get; set; }
    public float Extension { get; set; } = 30;
    public float Inset { get; set; } = 10;
  }

}
