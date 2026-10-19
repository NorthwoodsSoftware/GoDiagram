/* Copyright (c) Northwoods Software Corporation. */

using System;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace Demo.Samples.FaultTree {
  [ToolboxItem(false)]
  public partial class FaultTreeControl : DemoControl {
    private Diagram MyDiagram;

    public FaultTreeControl() {
      InitializeComponent();

      modelJson1.SaveClick = SaveModel;
      modelJson1.LoadClick = LoadModel;

      goWebBrowser1.Html = @"

  <p>
    <em>Fault trees</em> are used to conduct deductive failure analysis in which an undesired state of a
    system is analyzed using Boolean logic to combine a series of lower-level events.
  </p>
  <p>
    This diagram uses a basic <a>TreeModel</a> and <a>TreeLayout</a> to layout nodes in a tree structure.
    The <a>Diagram.NodeTemplate</a> definition allows for text describing the undesirable states and,
    when necessary, a figure indicating an event/gate.
  </p>
  <p>
    The <b>Visible</b> property on some of the node template's <a>Shape</a>s is set based on
    whether a figure is chosen for the node in the <a>Model.NodeDataSource</a>. The nodes also
    display a <b>TreeExpanderButton</b> allowing for expanding/collapsing of subtrees.
    See the <a href=""intro/buttons.html"">Intro page on Buttons</a> for more GoDiagram button information.
      </p>

      <p>
        Related to deductive failure analysis is root cause analysis, or RCA. See the <a href=""demo/Fishbone"">fishbone layout</a>
          extension page for a diagram format typically used in root cause analysis.
  </p>
";

      modelJson1.JsonText = @"
       {
        ""nodeDataSource"": [
        {""key"":1, ""text"":""No flow to receiver"", ""figure"":""None""},
        {""key"":2, ""text"":""No flow from Component B"", ""parent"":1, ""figure"":""OrGate"", ""choice"":""G02""},
        {""key"":3, ""text"":""No flow into Component B"", ""parent"":2, ""figure"":""AndGate"", ""choice"":""G03""},
        {""key"":4, ""text"":""Component B blocks flow"", ""parent"":2, ""figure"":""Circle"", ""choice"":""B01""},
        {""key"":5, ""text"":""No flow from Component A1"", ""parent"":3, ""figure"":""OrGate"", ""choice"":""G04""},
        {""key"":6, ""text"":""No flow from Component A2"", ""parent"":3, ""figure"":""OrGate"", ""choice"":""G05""},
        {""key"":7, ""text"":""No flow from source1"", ""parent"":5, ""figure"":""Triangle"", ""choice"":""T01""},
        {""key"":8, ""text"":""Component A1 blocks flow"", ""parent"":5, ""figure"":""Circle"", ""fill"":""green"", ""choice"":""B02""},
        {""key"":9, ""text"":""No flow from source2"", ""parent"":6, ""figure"":""Triangle"", ""choice"":""T02""},
        {""key"":10, ""text"":""Component A2 blocks flow"", ""parent"":6, ""figure"":""Circle"", ""choice"":""B03""}
        ]}";

      Setup();
    }

    private string nodeFillConverter(object figure, object _) {
      switch ((string)figure) {
        case "AndGate":
          // right to left so when it's rotated, it goes from top to bottom
          return "#EA8100";
        case "OrGate":
          return "#0058D3";
        case "Circle":
          return "#009620";
        case "Triangle":
          return "#7A0099";
        default:
          return "whitesmoke";
      }
    }

    private void Setup() {

      MyDiagram = diagramControl1.Diagram;

      MyDiagram.AllowCopy = false;
      MyDiagram.AllowDelete = false;
      MyDiagram.ToolManager.DraggingTool.DragsTree = true;
      MyDiagram.Layout = new TreeLayout {
        Angle = 90,
        LayerSpacing = 30
      };
      MyDiagram.UndoManager.IsEnabled = true;

      var KAPPA = 4 * ((Math.Sqrt(2) - 1) / 3);

      Shape.DefineFigureGenerator("OrGate", (shape, w, h) => {
        var geo = new Geometry();
        var radius = .5;
        var cpOffset = KAPPA * radius;
        var centerx = 0;
        var centery = .5;
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Bezier, w, .5 * h, (centerx + cpOffset + cpOffset) * w, (centery - radius) * h,
          .8 * w, (centery - cpOffset) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, h, .8 * w, (centery + cpOffset) * h,
          (centerx + cpOffset + cpOffset) * w, (centery + radius) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, 0, .25 * w, .75 * h, .25 * w, .25 * h).Close());
        geo.Spot1 = new Spot(.2, .25);
        geo.Spot2 = new Spot(.75, .75);
        return geo;
      });

      Shape.DefineFigureGenerator("AndGate", (shape, w, h) => {
        var geo = new Geometry();
        var cpOffset = KAPPA * .5;
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        // The gate body
        fig.Add(new PathSegment(SegmentType.Line, .5 * w, 0));
        fig.Add(new PathSegment(SegmentType.Bezier, w, .5 * h, (.5 + cpOffset) * w, 0,
          w, (.5 - cpOffset) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, .5 * w, h, w, (.5 + cpOffset) * h,
          (.5 + cpOffset) * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h).Close());
        geo.Spot1 = Spot.TopLeft;
        geo.Spot2 = new Spot(.55, 1);
        return geo;
      });

      var treeExpander = Builder.Make<Panel>("TreeExpanderButton");
      treeExpander.Alignment = Spot.Right;
      treeExpander.AlignmentFocus = Spot.Left;

      MyDiagram.NodeTemplate = new Node(PanelType.Spot) { // the default node template
        SelectionElementName = "BODY", LocationSpot = Spot.Center, LocationElementName = "BODY"
        // the main "BODY" consists of a Rectangle surrounding some text
      }.Add(
        new Panel(PanelType.Auto) {
          Name = "BODY", PortId = ""
        }.Add(
          new Shape { Fill = "#770000", Stroke = null },
          new TextBlock {
            Margin = new Margin(2, 10, 1, 10), MaxSize = new Size(100, double.NaN),
            Stroke = "whitesmoke", Font = new Font("Segoe UI", 10, FontWeight.Bold)
          }.Bind("Text")
        ), // end BODY, an Auto Panel
        treeExpander,
        new Shape {
          Figure = "LineV", StrokeWidth = 1.5, Height = 20, Alignment = new Spot(0.5, 1, 0, -1), AlignmentFocus = Spot.Top
        }.Bind("Visible", "Figure", (f, _) => (string)f != "None"),
        new Shape {
          Alignment = new Spot(0.5, 1, 0, 5), AlignmentFocus = Spot.Top, Width = 30, Height = 30,
          Stroke = null
        }.Bind(
          new Binding("Visible", "Figure", (f, _) => (string)f != "None"),
          new Binding("Figure"),
          new Binding("Fill", "Figure", nodeFillConverter),
          new Binding("Angle", "Figure", (f, _) => ((string)f == "OrGate" || (string)f == "AndGate") ? -90 : 0) // ORs and ANDs should point upwards
        ),
        new TextBlock {
          Alignment = new Spot(0.5, 1, 20, 20), AlignmentFocus = Spot.Left,
          Stroke = "black", Font = new Font("Segoe UI", 10, FontWeight.Bold)
        }.Bind(
          new Binding("Visible", "Figure", (f, _) => (string)f != "None"), // if we don't have a figure, don't display text
          new Binding("Text", "Choice")
        )
      );

      MyDiagram.LinkTemplate = new Link {
        Routing = LinkRouting.Orthogonal,
        LayerName = "Background",
        Curviness = 20,
        Corner = 5
      }.Add(
        new Shape { StrokeWidth = 1.5 }
      );

      LoadModel();
    }

    private void SaveModel() {
      if (MyDiagram == null) return;
      modelJson1.JsonText = MyDiagram.Model.ToJson();
    }

    private void LoadModel() {
      if (MyDiagram == null) return;
      MyDiagram.Model = Model.FromJson<Model>(modelJson1.JsonText);
      MyDiagram.Model.UndoManager.IsEnabled = true;
    }

  }

  public class Model : TreeModel<NodeData, int, object> { }

  public class NodeData : Model.NodeData {
    public string Figure { get; set; }
    public string Choice { get; set; }
  }

}
