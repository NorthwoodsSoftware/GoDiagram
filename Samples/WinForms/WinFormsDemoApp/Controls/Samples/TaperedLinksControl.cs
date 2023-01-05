/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace Demo.Samples.TaperedLinks {
  [ToolboxItem(false)]
  public partial class TaperedLinksControl : DemoControl {
    private Diagram myDiagram;

    public TaperedLinksControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"
        <p>
      This sample demonstrates customization of the <a>Link</a> <a>Shape</a>'s <a>Geometry</a>.
        </p>
        <p>
      The TaperedLink class in this sample inherits from Link and overrides the MakeGeometry method.
      For Bezier-curve Links, this computes a Geometry that is thick at the ""from"" end and thin at the ""to"" end.
      The implementation is very simple and does not account for links that
      are coming out from a node at angles that are not a multiple of 90 degrees.
        </p>
";

    }

    private void Setup() {

      myDiagram = diagramControl1.Diagram;

      myDiagram.Layout = new ForceDirectedLayout();
      myDiagram.UndoManager.IsEnabled = true;

      // this controls whether links overlap each other at each side of the node,
      // or if the links are spread out on each side of the node.
      var SPREADLINKS = true; // must be set before defining templates!

      myDiagram.NodeTemplate = new Node(PanelType.Auto) {
        LocationSpot = Spot.Center
      }.Add(
        new Shape {
          Figure = "RoundedRectangle",
          Parameter1 = 10,
          Fill = "orange", // default fill color
          PortId = "",
          FromLinkable = true,
          FromSpot = SPREADLINKS ? Spot.AllSides : Spot.None,
          ToLinkable = true,
          ToSpot = SPREADLINKS ? Spot.AllSides : Spot.None,
          Cursor = "pointer"
        }.Bind("Fill", "Color"),
        new TextBlock {
          Margin = 10,
          Font = new Font("Segoe UI", 12, FontWeight.Bold)
        }.Bind("Text")
      );

      myDiagram.LinkTemplate = new TaperedLink {
        Curve = LinkCurve.Bezier,
        Routing = SPREADLINKS ? LinkRouting.Normal : LinkRouting.Orthogonal,
        FromEndSegmentLength = SPREADLINKS ? 50 : 1,
        ToEndSegmentLength = SPREADLINKS ? 50 : 1,
        RelinkableFrom = true,
        RelinkableTo = true
      }.Add(
        new Shape {
          Stroke = null,
          StrokeWidth = 0,
        }.Bind("Fill", "Color")
      );

      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = 1, Text = "one", Color = "lightyellow" },
          new NodeData { Key = 2, Text = "two", Color = "brown" },
          new NodeData { Key = 3, Text = "three", Color = "green" },
          new NodeData { Key = 4, Text = "four", Color = "slateblue" },
          new NodeData { Key = 5, Text = "five", Color = "aquamarine" },
          new NodeData { Key = 6, Text = "six", Color = "lightgreen" },
          new NodeData { Key = 7, Text = "seven", Color = "orange" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = 5, To = 6, Color = "orange" },
          new LinkData { From = 1, To = 2, Color = "red" },
          new LinkData { From = 1, To = 3, Color = "blue" },
          new LinkData { From = 1, To = 4, Color = "goldenrod" },
          new LinkData { From = 2, To = 5, Color = "fuchsia" },
          new LinkData { From = 3, To = 5, Color = "green" },
          new LinkData { From = 4, To = 5, Color = "black" },
          new LinkData { From = 6, To = 7, Color = "black" }
        }
      };
    }

    public class TaperedLink : Link {
      public override Geometry MakeGeometry() {
        // maybe use the standard geometry for this route, instead?
        var numpts = PointsCount;
        if (numpts < 4 || ComputeCurve() != LinkCurve.Bezier) {
          return base.MakeGeometry();
        }

        var p0 = GetPoint(0);
        var p1 = GetPoint((numpts > 4) ? 2 : 1);
        var p2 = GetPoint((numpts > 4) ? (numpts - 3) : 2);
        var p3 = GetPoint(numpts - 1);
        var fromHoriz = Math.Abs(p0.Y - p1.Y) < Math.Abs(p0.X - p1.X);
        var toHoriz = Math.Abs(p2.Y - p3.Y) < Math.Abs(p2.X - p3.X);

        var p0x = p0.X + (fromHoriz ? 0 : -4);
        var p0y = p0.Y + (fromHoriz ? -4 : 0);
        var p1x = p1.X + (fromHoriz ? 0 : -3);
        var p1y = p1.Y + (fromHoriz ? -3 : 0);
        var p2x = p2.X + (toHoriz ? 0 : -2);
        var p2y = p2.Y + (toHoriz ? -2 : 0);
        var p3x = p3.X + (toHoriz ? 0 : -1);
        var p3y = p3.Y + (toHoriz ? -1 : 0);

        var fig = new PathFigure(p0x, p0y, true);  // filled
        fig.Add(new PathSegment(SegmentType.Bezier, p3x, p3y, p1x, p1y, p2x, p2y));

        p0x = p0.X + (fromHoriz ? 0 : 4);
        p0y = p0.Y + (fromHoriz ? 4 : 0);
        p1x = p1.X + (fromHoriz ? 0 : 3);
        p1y = p1.Y + (fromHoriz ? 3 : 0);
        p2x = p2.X + (toHoriz ? 0 : 2);
        p2y = p2.Y + (toHoriz ? 2 : 0);
        p3x = p3.X + (toHoriz ? 0 : 1);
        p3y = p3.Y + (toHoriz ? 1 : 0);
        fig.Add(new PathSegment(SegmentType.Line, p3x, p3y));
        fig.Add(new PathSegment(SegmentType.Bezier, p0x, p0y, p2x, p2y, p1x, p1y).Close());

        var geo = new Geometry();
        geo.Add(fig);
        geo.Normalize();
        return geo;
      }
    }

  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }

  public class NodeData : Model.NodeData {
    public Brush Color { get; set; }
  }

  public class LinkData : Model.LinkData {
    public Brush Color { get; set; } = "black";
  }

}
