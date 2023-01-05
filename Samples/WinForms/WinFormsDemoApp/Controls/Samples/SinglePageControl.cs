/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;

namespace Demo.Samples.SinglePage {
  [ToolboxItem(false)]
  public partial class SinglePageControl : DemoControl {
    private Diagram myDiagram;

    public SinglePageControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"
        <p>
      This pretends to show a sheet of paper with the diagram on it.
      Both the <a>DraggingTool</a> and the <a>ResizingTool</a> are constrained to keep the nodes within
      the area of the sheet of paper, minus the margins.
      The user can zoom and scroll/pan normally.
      There are several variables, such as <code>pageSize</code>, that govern how the diagram is set up.
        </p>
";

    }

    // some consts
    public static Size pageSize = new Size(612, 792);
    public static Margin pageMargin = new Margin(10);
    public static Margin usableMargin = new Margin(10);
    public static Rect pageBounds = new Rect(-usableMargin.Left, -usableMargin.Top, pageSize.Width, pageSize.Height);
    public static Rect usableArea = pageBounds.Deflate(usableMargin);

    public static Point LimitPoint(Point p) {
      return new Point(Math.Max(usableArea.Left, Math.Min(p.X, usableArea.Right)),
                       Math.Max(usableArea.Top, Math.Min(p.Y, usableArea.Bottom)));
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.FixedBounds = pageBounds.Inflate(pageMargin);
      myDiagram.InitialAutoScale = AutoScale.Uniform;
      myDiagram.AnimationManager.IsInitial = false;
      myDiagram.UndoManager.IsEnabled = true;
      myDiagram.ToolManager.ResizingTool = new SinglePageResizingTool();
      myDiagram.TextEdited += (_, e) => {
        var node = (e.Subject as GraphObject).Part as Node;
        node.EnsureBounds();  // has been resized, compute its new bounds
        var pt = LimitPoint(node.Location);
        node.Location = StayInFixedArea(node, pt, pt);
      };

      // the background Part showing the sheet of paper;
      // it has the fixed bounds of the diagram contents
      myDiagram.Add(
        new Part(PanelType.Grid) {
          LayerName = "Grid",
          Position = pageBounds.Position,
          DesiredSize = pageSize,
          IsShadowed = true,
          Background = "floralwhite"
        }.Add(
          new Shape {
            Figure = "LineV",
            Stroke = "lightgray",
            StrokeWidth = 0.5
          },
          new Shape {
            Figure = "LineH",
            Stroke = "lightgray",
            StrokeWidth = 0.5
          }
        )
      );

      // this function is the Node.DragComputation, to limit the movement of the parts
      // use GRIDPT instead of PT if DraggingTool.IsGridSnapEnabled and movement should snap to grid
      Point StayInFixedArea(Part part, Point pt, Point gridpt) {
        var diagram = part.Diagram;
        if (diagram == null) return pt;
        // compute the document area without padding
        var v = usableArea;
        // get the bounds of the part being dragged
        var b = part.ActualBounds;
        var loc = part.Location;
        // now limit the location appropriately
        var x = Math.Max(v.X, Math.Min(pt.X, v.Right - b.Width)) + (loc.X - b.X);
        var y = Math.Max(v.Y, Math.Min(pt.Y, v.Bottom - b.Height)) + (loc.Y - b.Y);
        return new Point(x, y);
      }

      // node template
      myDiagram.NodeTemplate =
        new Node(PanelType.Auto) {
          Resizable = true,  // but limited by overrides of ResizingTool methods, above
          DragComputation = StayInFixedArea  // this limits the DraggingTool
        }.Add(
          new Shape {
            Fill = "white",
            PortId = "",
            FromLinkable = true,
            ToLinkable = true,
            Cursor = "pointer"
          }.Bind(
            new Binding("Fill", "Color")
          ),
          new TextBlock {
            Margin = 8,
            Editable = true
          }.Bind(
            new Binding("Text").MakeTwoWay()
          )
        );

      myDiagram.LinkTemplate =
        new Link {
          RelinkableFrom = true,
          RelinkableTo = true
        }.Add(
          new Shape(),
          new Shape {
            ToArrow = "OpenTriangle"
          }
        );

      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = 1, Text = "Alpha", Color = "lightblue" },
          new NodeData { Key = 2, Text = "Beta", Color = "orange" },
          new NodeData { Key = 3, Text = "Gamma", Color = "lightgreen" },
          new NodeData { Key = 4, Text = "Delta", Color = "pink" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = 1, To = 2 },
          new LinkData { From = 1, To = 3 },
          new LinkData { From = 2, To = 2 },
          new LinkData { From = 3, To = 4 },
          new LinkData { From = 4, To = 1 }
        }
      };
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Color { get; set; }
  }

  public class LinkData : Model.LinkData { }

  // extend resizing tool
  public class SinglePageResizingTool : ResizingTool {
    public override void DoMouseMove() {
      var e = Diagram.LastInput;
      e.DocumentPoint = SinglePageControl.LimitPoint(e.DocumentPoint);
      e.ViewPoint = Diagram.TransformDocToView(e.DocumentPoint);
      base.DoMouseMove();
    }
    public override void DoMouseUp() {
      var e = Diagram.LastInput;
      e.DocumentPoint = SinglePageControl.LimitPoint(e.DocumentPoint);
      e.ViewPoint = Diagram.TransformDocToView(e.DocumentPoint);
      base.DoMouseUp();
    }
  }

}
