/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;

namespace Demo.Samples.Network {
  [ToolboxItem(false)]
  public partial class NetworkControl : DemoControl {
    private Diagram myDiagram;
    private Palette myPalette;
    private Overview myOverview;

    private Dictionary<string, Part> sharedNodeTemplateMap;

    public NetworkControl() {
      InitializeComponent();

      myDiagram = diagramControl1.Diagram;
      myPalette = paletteControl1.Diagram as Palette;
      myOverview = overviewControl1.Diagram as Overview;

      modelJson1.SaveClick = SaveModel;
      modelJson1.LoadClick = LoadModel;

      modelJson1.JsonText = @"
    {
      ""NodeDataSource"": [
        { ""Key"":1, ""Text"":""Gen1"", ""Category"":""Generator"", ""Loc"":""300 0""},
        { ""Key"":2, ""Text"":""Bar1"", ""Category"":""HBar"", ""Loc"":""100 100"", ""Size"":""500 4"", ""Fill"":""green""},
        { ""Key"":4, ""Text"":""Cons1"", ""Category"":""Consumer"", ""Loc"":""53 234""},
        { ""Key"":3, ""Text"":""Bar2"", ""Category"":""HBar"", ""Loc"":""0 300"", ""Size"":""600 4"", ""Fill"":""orange""},
        { ""Key"":5, ""Text"":""Conn1"", ""Category"":""Connector"", ""Loc"":""232.5 207.75""},
        { ""Key"":6, ""Text"":""Cons3"", ""Category"":""Consumer"", ""Loc"":""357.5 230.75""},
        { ""Key"":7, ""Text"":""Cons2"", ""Category"":""Consumer"", ""Loc"":""484.5 164.75""}
     ],
      ""LinkDataSource"": [
        { ""From"":1, ""To"":2},
        { ""From"":1, ""To"":3},
        { ""From"":4, ""To"":3},
        { ""From"":5, ""To"":2},
        { ""From"":5, ""To"":3},
        { ""From"":6, ""To"":3},
        { ""From"":7, ""To"":2}
     ]}
    ";

      Setup();
      SetupPalette();
      SetupOverview();
    }

    // Must use sharedNodeTemplate because don't know if palette or diagram will be initialized first
    private void DefineNodeTemplate() {
      if (sharedNodeTemplateMap != null) return;  // already defined

      var KAPPA = 4 * ((Math.Sqrt(2) - 1) / 3);
      Shape.DefineFigureGenerator("ACvoltageSource", (shape, w, h) => {
        var geo = new Geometry();
        var cpOffset = KAPPA * .5;
        var radius = .5;
        var centerx = .5;
        var centery = .5;
        var fig = new PathFigure((centerx - radius) * w, centery * h, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - radius) * h, (centerx - radius) * w, (centery - cpOffset) * h,
          (centerx - cpOffset) * w, (centery - radius) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, centery * h, (centerx + cpOffset) * w, (centery - radius) * h,
          (centerx + radius) * w, (centery - cpOffset) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radius) * h, (centerx + radius) * w, (centery + cpOffset) * h,
          (centerx + cpOffset) * w, (centery + radius) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w, centery * h, (centerx - cpOffset) * w, (centery + radius) * h,
          (centerx - radius) * w, (centery + cpOffset) * h));
        fig.Add(new PathSegment(SegmentType.Move, (centerx - radius + .1) * w, centery * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx + radius - .1) * w, centery * h, centerx * w, (centery - radius) * h,
          centerx * w, (centery + radius) * h));
        return geo;
      });

      var generatorTemplate =
        new Node("Spot") {
            LocationSpot = Spot.Center,
            SelectionElementName = "BODY"
          }
          .Bind("Location", "Loc", Point.Parse, Point.Stringify)
          .Add(
            new Shape("ACVoltageSource") {
                Name = "BODY", Stroke = "white", StrokeWidth = 3, Fill = "transparent", Background = "darkblue",
                Width = 40, Height = 40, Margin = 5,
                PortId = "", FromLinkable = true, Cursor = "pointer", FromSpot = Spot.TopBottomSides, ToSpot = Spot.TopBottomSides
              },
            new TextBlock {
                Alignment = Spot.Right,
                AlignmentFocus = Spot.Left,
                Editable = true
              }
              .Bind(new Binding("Text").MakeTwoWay())
          );

      var connectorTemplate =
        new Node("Spot") {
            LocationSpot = Spot.Center,
            SelectionElementName = "BODY"
          }
          .Bind("Location", "Loc", Point.Parse, Point.Stringify)
          .Add(
            new Shape("Circle") {
                Name = "BODY",
                Stroke = null,
                Fill = new Brush(new LinearGradientPaint(new Dictionary<float, string> {
                  { 0, "lightgray" }, { 1, "gray" }
                }, Spot.Left, Spot.Right)),
                Background = "gray", Width = 20, Height = 20, Margin = 2,
                PortId = "", FromLinkable = true, Cursor = "pointer", FromSpot = Spot.TopBottomSides, ToSpot = Spot.TopBottomSides
              },
            new TextBlock {
                Alignment = Spot.Right,
                AlignmentFocus = Spot.Left,
                Editable = true
              }
              .Bind(new Binding("Text").MakeTwoWay())
          );

      var consumerTemplate =
        new Node("Spot") {
            LocationSpot = Spot.Center, LocationElementName = "BODY",
            SelectionElementName = "BODY"
          }
         .Bind("Location", "Loc", Point.Parse, Point.Stringify)
         .Add(
            new Picture("pc") {
                Name = "BODY", Width = 50, Height = 40, Margin = 2,
                PortId = "", FromLinkable = true, Cursor = "pointer", FromSpot = Spot.TopBottomSides, ToSpot = Spot.TopBottomSides
              },
            new TextBlock {
                Alignment = Spot.Right, AlignmentFocus = Spot.Left, Editable = true
              }
              .Bind(new Binding("Text").MakeTwoWay())
          );

      var hBarTemplate =
        new Node("Spot") {
            LayerName = "Background",
            // special resizing, just at the ends
            Resizable = true, ResizeElementName = "SHAPE",
            ResizeAdornmentTemplate =
              new Adornment("Spot")
                .Add(
                    new Placeholder(),
                    new Shape {  // left resize handle
                        Alignment = Spot.Left, Cursor = "col-resize",
                        DesiredSize = new Size(6, 6), Fill = "lightblue", Stroke = "dodgerblue"
                      },
                    new Shape {  // right resize handle
                        Alignment = Spot.Right, Cursor = "col-resize",
                        DesiredSize = new Size(6, 6), Fill = "lightblue", Stroke = "dodgerblue"
                      }
                )
          }
          .Bind("Location", "Loc", Point.Parse, Point.Stringify)
          .Add(
            new Shape("Rectangle") {
                Name = "SHAPE",
                Fill = "black", Stroke = null, StrokeWidth = 0,
                Width = 1000, Height = 4,
                MinSize = new Size(100, 4),
                MaxSize = new Size(double.PositiveInfinity, 4),
                PortId = "",
                ToLinkable = true
              }
             .Bind("DesiredSize", "Size", Northwoods.Go.Size.Parse, Northwoods.Go.Size.Stringify)
             .Bind("Fill"),
            new TextBlock {
                Alignment = Spot.Right,
                AlignmentFocus = Spot.Left,
                Editable = true
              }
              .Bind(new Binding("Text").MakeTwoWay())
          );

      sharedNodeTemplateMap = new Dictionary<string, Part> {
        { "Generator", generatorTemplate },
        { "Connector", connectorTemplate },
        { "Consumer", consumerTemplate },
        { "HBar", hBarTemplate },
      };
    }

    private void Setup() {
      myDiagram.ToolManager.LinkingTool.Direction = LinkingDirection.ForwardsOnly;
      myDiagram.UndoManager.IsEnabled = true;

      DefineNodeTemplate();
      myDiagram.NodeTemplateMap = sharedNodeTemplateMap;

      myDiagram.LinkTemplate =
        new BarLink {  // subclass defined below
            Routing = LinkRouting.Orthogonal,
            RelinkableFrom = true, RelinkableTo = true,
            ToPortChanged = (link, oldport, newport) => {
              if (newport is Shape newshape) link.Path.Stroke = newshape.Fill;
            }
          }
         .Add(new Shape { StrokeWidth = 2 });

      // start off with a simple diagram
      LoadModel();
    }

    private void SetupPalette() {
      // initialize Palette
      DefineNodeTemplate();
      myPalette.NodeTemplateMap = sharedNodeTemplateMap;
      myPalette.Layout =
        new GridLayout {
          CellSize = new Size(2, 2),
          IsViewportSized = true
        };

      myPalette.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Text = "Generator", Category = "Generator" },
          new NodeData { Text = "Consumer", Category = "Consumer" },
          new NodeData { Text = "Connector", Category = "Connector" },
          new NodeData { Text = "Bar", Category = "HBar", Size = "100 4" }
        }
      };

      // remove cursors on all ports in the Palette
      // make TextBlocks invisible, to reduce size of Nodes
      foreach (var node in myPalette.Nodes) {
        foreach (var port in node.Ports) {
          port.Cursor = "";
        }
        foreach (var tb in node.Elements) {
          if (tb is TextBlock) tb.Visible = false;
        }
      }
    }

    private void SetupOverview() {
      // initialize Overview
      myOverview.Observed = myDiagram;
      myOverview.ContentAlignment = Spot.Center;
    }

    private void SaveModel() {
      if (myDiagram == null) return;
      modelJson1.JsonText = myDiagram.Model.ToJson();
    }

    private void LoadModel() {
      if (myDiagram == null) return;
      myDiagram.Model = Model.FromJson<Model>(modelJson1.JsonText);
      myDiagram.Model.UndoManager.IsEnabled = true;
    }
  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }
  public class NodeData : Model.NodeData {
    public string Size { get; set; }
    public string Loc { get; set; }
  }
  public class LinkData : Model.LinkData { }

  public class BarLink : Link {
    public override Spot ComputeSpot(bool from, GraphObject port = null) {
      if (from && ToNode != null && ToNode.Category == "HBar") return Spot.TopBottomSides;
      if (!from && FromNode != null && FromNode.Category == "HBar") return Spot.TopBottomSides;
      return base.ComputeSpot(from, port);
    }

    public override Point GetLinkPoint(Node node, GraphObject port, Spot spot, bool from, bool ortho, Node othernode, GraphObject otherport) {
      if (!from && node.Category == "HBar") {
        var op = base.GetLinkPoint(othernode, otherport, ComputeSpot(!from), !from, ortho, node, port);
        var r = port.GetDocumentBounds();
        var y = (op.Y > r.CenterY) ? r.Bottom : r.Top;
        if (op.X < r.Left) return new Point(r.Left, y);
        if (op.X > r.Right) return new Point(r.Right, y);
        return new Point(op.X, y);
      } else {
        return base.GetLinkPoint(node, port, spot, from, ortho, othernode, otherport);
      }
    }

    public override int GetLinkDirection(Node node, GraphObject port, Point linkpoint, Spot spot, bool from, bool ortho, Node othernode, GraphObject otherport) {
      if (node.Category == "HBar" || othernode.Category == "HBar") {
        var p = port.GetDocumentPoint(Spot.Center);
        var op = otherport.GetDocumentPoint(Spot.Center);
        var below = op.Y > p.Y;
        return below ? 90 : 270;
      } else {
        return base.GetLinkDirection(node, port, linkpoint, spot, from, ortho, othernode, otherport);
      }
    }
  }

}

