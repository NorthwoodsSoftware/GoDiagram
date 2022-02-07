using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.FaultTree {
  [ToolboxItem(false)]
  public partial class FaultTreeControl : System.Windows.Forms.UserControl {
    private Diagram MyDiagram;
    
    public FaultTreeControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      saveLoadModel1.SaveClick += (e, obj) => SaveModel();
      saveLoadModel1.LoadClick += (e, obj) => LoadModel();

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
        Related to deductive failure analysis is root cause analysis, or RCA. See the <a href=""Fishbone"">fishbone layout</a>
          extension page for a diagram format typically used in root cause analysis.
  </p>    
";

      saveLoadModel1.ModelJson = @"
       {
        ""NodeDataSource"": [
        {""Key"":1, ""Text"":""No flow to receiver"", ""Figure"":""None""},
        {""Key"":2, ""Text"":""No flow from Component B"", ""Parent"":1, ""Figure"":""OrGate"", ""Choice"":""G02""},
        {""Key"":3, ""Text"":""No flow into Component B"", ""Parent"":2, ""Figure"":""AndGate"", ""Choice"":""G03""},
        {""Key"":4, ""Text"":""Component B blocks flow"", ""Parent"":2, ""Figure"":""Circle"", ""Choice"":""B01""},
        {""Key"":5, ""Text"":""No flow from Component A1"", ""Parent"":3, ""Figure"":""OrGate"", ""Choice"":""G04""},
        {""Key"":6, ""Text"":""No flow from Component A2"", ""Parent"":3, ""Figure"":""OrGate"", ""Choice"":""G05""},
        {""Key"":7, ""Text"":""No flow from source1"", ""Parent"":5, ""Figure"":""Triangle"", ""Choice"":""T01""},
        {""Key"":8, ""Text"":""Component A1 blocks flow"", ""Parent"":5, ""Figure"":""Circle"", ""fill"":""green"", ""Choice"":""B02""},
        {""Key"":9, ""Text"":""No flow from source2"", ""Parent"":6, ""Figure"":""Triangle"", ""Choice"":""T02""},
        {""Key"":10, ""Text"":""Component A2 blocks flow"", ""Parent"":6, ""Figure"":""Circle"", ""Choice"":""B03""}
        ]}";

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

      MyDiagram.NodeTemplate = new Node(PanelLayoutSpot.Instance) { // the default node template
        SelectionElementName = "BODY", LocationSpot = Spot.Center, LocationElementName = "BODY"
        // the main "BODY" consists of a Rectangle surrounding some text
      }.Add(
        new Panel(PanelLayoutAuto.Instance) {
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
      saveLoadModel1.ModelJson = MyDiagram.Model.ToJson();
    }

    private void LoadModel() {
      if (MyDiagram == null) return;
      MyDiagram.Model = Model.FromJson<Model>(saveLoadModel1.ModelJson);
      MyDiagram.Model.UndoManager.IsEnabled = true;
    }

  }

  public class Model : TreeModel<NodeData, int, object> { }

  public class NodeData : Model.NodeData {
    public string Figure { get; set; }
    public string Choice { get; set; }
  }

}
