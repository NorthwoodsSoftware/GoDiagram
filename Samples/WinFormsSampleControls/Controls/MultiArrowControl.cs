/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.MultiArrow {
  [ToolboxItem(false)]
  public partial class MultiArrowControl : System.Windows.Forms.UserControl {
    private Diagram MyDiagram;

    public MultiArrowControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"
        <p>
      This sample demonstrates customization of the <a>Link</a> <a>Shape</a>'s <a>Geometry</a>.
        </p>
        <p>
      The MultiArrowLink class in this sample inherits from Link and overrides the <a>Link.MakeGeometry</a> method
      to add arrowheads at the end of each interior segment, assuming that the route is orthogonal.
        </p>
";

    }

    private void Setup() {

      MyDiagram = diagramControl1.Diagram;

      MyDiagram.Layout = new ForceDirectedLayout();
      MyDiagram.UndoManager.IsEnabled = true;

      MyDiagram.NodeTemplate = new Node(PanelLayoutAuto.Instance) {
        LocationSpot = Spot.Center
      }.Add(
        new Shape {
          Figure = "RoundedRectangle",
          Parameter1 = 10,
          Fill = "orange",
          PortId = "",
          FromLinkable = true,
          FromSpot = Spot.AllSides,
          ToLinkable = true,
          ToSpot = Spot.AllSides,
          Cursor = "pointer"
        }.Bind("Fill", "Color"),
        new TextBlock {
          Margin = 10,
          Font = new Font("Segoe UI", 12, FontWeight.Bold)
        }.Bind("Text")
      );

      MyDiagram.LinkTemplate = new MultiArrowLink { // subclass of Link, defined below
        RelinkableFrom = true,
        RelinkableTo = true,
        Reshapable = true,
        Resegmentable = true
      }.Add(
       new Shape {
         IsPanelMain = true
       }.Bind("Fill", "Color")
      // no arrowhead is defined here -- they are hard-coded in MultiArrowLink.makeGeometry
      );

      MyDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = 1, Text = "one", Color = "lightyellow" },
          new NodeData { Key = 2, Text = "two", Color = "brown" },
          new NodeData { Key = 3, Text = "three", Color = "green" },
          new NodeData { Key = 4, Text = "four", Color = "slateblue" },
          new NodeData { Key = 5, Text = "five", Color = "aquamarine" },
          new NodeData { Key = 6, Text = "six", Color = "lightgreen" },
          new NodeData { Key = 7, Text = "seven" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = 5, To = 6, Color = "orange" },
          new LinkData { From = 1, To = 2, Color = "red" },
          new LinkData { From = 1, To = 3, Color = "blue" },
          new LinkData { From = 1, To = 4, Color = "goldenrod" },
          new LinkData { From = 2, To = 5, Color = "fuchsia" },
          new LinkData { From = 3, To = 5, Color = "green" },
          new LinkData { From = 4, To = 5, Color = "black" },
          new LinkData { From = 6, To = 7 }
        }
      };
    }

  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }

  public class NodeData : Model.NodeData {
    public string Color { get; set; }
  }

  public class LinkData : Model.LinkData {
    public string Color { get; set; }
  }

  // Produce a Geometry that includes an arrowhead at the end of each segment.
  // This only works with orthogonal non-Bezier routing.
  public class MultiArrowLink : Link {
    public MultiArrowLink() : base() {
      Routing = LinkRouting.Orthogonal;
    }

    // produce a Geometry from the Link's route
    public override Geometry MakeGeometry() {
      var geo = base.MakeGeometry(); // from the standard behavior

      if (geo.Type != GeometryType.Path || geo.Figures.Count == 0) return geo;
      var mainfig = geo.Figures[0]; // assume there's just one PathFigure
      var mainsegs = mainfig.Segments;

      var arrowLen = 8; // length for each arrowhead
      var arrowWid = 3; // actually half-width of each arrowhead
      var fx = mainfig.StartX;
      var fy = mainfig.StartY;
      for (var i = 0; i < mainsegs.Count; i++) {
        var a = mainsegs[i];
        // assume each arrowhead is a simple triangle
        var ax = a.EndX;
        var ay = a.EndY;
        var bx = ax;
        var by = ay;
        var cx = ax;
        var cy = ay;
        if (fx < ax - arrowLen) {
          bx -= arrowLen; by += arrowWid;
          cx -= arrowLen; cy -= arrowWid;
        } else if (fx > ax + arrowLen) {
          bx += arrowLen; by += arrowWid;
          cx += arrowLen; cy -= arrowWid;
        } else if (fy < ay - arrowLen) {
          bx -= arrowWid; by -= arrowLen;
          cx += arrowWid; cy -= arrowLen;
        } else if (fy > ay + arrowLen) {
          bx -= arrowWid; by += arrowLen;
          cx += arrowWid; cy += arrowLen;
        }
        geo.Add(new PathFigure(ax, ay, true)
          .Add(new PathSegment(SegmentType.Line, bx, by))
          .Add(new PathSegment(SegmentType.Line, cx, cy).Close()));
        fx = ax;
        fy = ay;
      }

      return geo;
    }
  }

}
