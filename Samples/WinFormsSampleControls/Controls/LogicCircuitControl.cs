using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Extensions;
using Northwoods.Go.Models;
using Northwoods.Go.WinForms;

namespace WinFormsSampleControls.LogicCircuit {
  [ToolboxItem(false)]
  public partial class LogicCircuitControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    private Palette myPalette;

    private Dictionary<string, Part> sharedNodeTemplateMap;

    public LogicCircuitControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;
      paletteControl1.AfterRender = SetupPalette;

      saveLoadModel1.SaveClick += (e, obj) => SaveModel();
      saveLoadModel1.LoadClick += (e, obj) => LoadModel();

      goWebBrowser1.Html = @"
    <p>
    The Logic Circuit sample allows the user to make circuits using gates and wires,
    which are updated whenever a Link is modified and at intervals by a looped setTimeout function.
    </p>
    <p>
    The <b>updateStates</b> function calls a function to update each node according to type,
    which uses the color of the links into the node to determine the color of those exiting it.
    Red means zero or false; green means one or true. Double-clicking an input node will toggle true/false.
    </p>
    <p>
    Mouse over a node to see its category, displayed using a shared <a>Adornment</a> set as the tooltip.
    A <a>Palette</a> to the left of the main diagram allows the user to drag and drop new nodes.
    These nodes can then be linked using ports which are defined on the various node templates.
    Each input port can only have one input link, while output ports can have many output links.
    This is controlled by the <a>GraphObject.toMaxLinks</a> property.
    </p>
";

      saveLoadModel1.ModelJson = @"{
  ""LinkFromPortIdProperty"": ""FromPort"",
  ""LinkToPortIdProperty"": ""ToPort"",
  ""NodeDataSource"": [
    {""Category"":""Input"", ""Key"":""Input1"", ""Loc"":""-150 -80"" },
    {""Category"":""Or"", ""Key"":""Or1"", ""Loc"":""-70 0"" },
    {""Category"":""Not"", ""Key"":""Not1"", ""Loc"":""10 0"" },
    {""Category"":""Xor"", ""Key"":""Xor1"", ""Loc"":""100 0"" },
    {""Category"":""Or"", ""Key"":""Or2"", ""Loc"":""200 0"" },
    {""Category"":""Output"", ""Key"":""Output1"", ""Loc"":""200 -100"" }
  ],
  ""LinkDataSource"": [
    {""From"":""Input1"", ""FromPort"":""out"", ""To"":""Or1"", ""ToPort"":""in1""},
    {""From"":""Or1"", ""FromPort"":""out"", ""To"":""Not1"", ""ToPort"":""in""},
    {""From"":""Not1"", ""FromPort"":""out"", ""To"":""Or1"", ""ToPort"":""in2""},
    {""From"":""Not1"", ""FromPort"":""out"", ""To"":""Xor1"", ""ToPort"":""in1""},
    {""From"":""Xor1"", ""FromPort"":""out"", ""To"":""Or2"", ""ToPort"":""in1""},
    {""From"":""Or2"", ""FromPort"":""out"", ""To"":""Xor1"", ""ToPort"":""in2""},
    {""From"":""Xor1"", ""FromPort"":""out"", ""To"":""Output1"", ""ToPort"":""""}
  ]
}";

    }

    private void DefineNodeTemplates() {
      if (sharedNodeTemplateMap != null) return;  // already defined

      // load extra shapes
      Figures.DefineExtraFigures();

      // node template helpers
      var sharedToolTip =
        Builder.Make<Adornment>("ToolTip")
          .Set(new { Border_Figure = "RoundedRectangle" })
          .Add(
            new TextBlock { Margin = 2 }
              .Bind("Text", "", (data, _) => {
                return (data as NodeData).Category;
              })
          );

      // define some common property settings
      var nodeStyle = new {
        SelectionAdorned = false,
        ShadowOffset = new Point(0, 4),
        ShadowBlur = 15,
        ShadowColor = "blue",
        Resizable = true,
        ResizeElementName = "NODESHAPE",
        ToolTip = sharedToolTip
      };
      Binding[] nodeBind() {
        return new[] {
          new Binding("Location", "Loc", Point.Parse, Point.Stringify),
          new Binding("IsShadowed", "IsSelected").OfElement()
        };
      }

      var shapeStyle = new {
        Name = "NODESHAPE",
        Fill = "lightgray",
        Stroke = "darkslategray",
        DesiredSize = new Size(40, 40),
        StrokeWidth = 2
      };

      object portStyle(bool input) {
        return new {
          DesiredSize = new Size(6, 6),
          Fill = "black",
          FromSpot = Spot.Right,
          FromLinkable = !input,
          ToSpot = Spot.Left,
          ToLinkable = input,
          ToMaxLinks = 1,
          Cursor = "pointer"
        };
      }

      // define templates for each type of node
      var inputTemplate =
        new Node("Spot") {
          // if double-clicked, and input node will change its value, represented by the color
          DoubleClick = (e, obj) => {
            var node = obj as Node;
            e.Diagram.StartTransaction("Toggle Input");
            var shp = node.FindElement("NODESHAPE") as Shape;
            shp.Fill = (shp.Fill == "green") ? "red" : "green";
            UpdateStates();
            e.Diagram.CommitTransaction("Toggle Input");
          }
        }
          .Set(nodeStyle)
          .Bind(nodeBind())
          .Add(
            new Shape("Circle")
              .Set(shapeStyle)
              .Set(new { Fill = "red" }),  // override the default fill (from shapeStyle) to be red
            new Shape("Rectangle") { Name = "outport", PortId = "", Alignment = new Spot(1, 0.5) }  // the only port
              .Set(portStyle(false))
          );

      var outputTemplate =
        new Node("Spot")
          .Set(nodeStyle)
          .Bind(nodeBind())
          .Add(
            new Shape("Rectangle")
              .Set(shapeStyle)
              .Set(new { Fill = "green" }),  // override the default fill (from shapeStyle) to be green
            new Shape("Rectangle") { PortId = "", Alignment = new Spot(0, 0.5) }  // the only port
              .Set(portStyle(true))
          );

      var andTemplate =
        new Node("Spot")
          .Set(nodeStyle)
          .Bind(nodeBind())
          .Add(
            new Shape("AndGate").Set(shapeStyle),
            new Shape("Rectangle") { PortId = "in1", Alignment = new Spot(0, 0.3) }
              .Set(portStyle(true)),
            new Shape("Rectangle") { PortId = "in2", Alignment = new Spot(0, 0.7) }
              .Set(portStyle(true)),
            new Shape("Rectangle") { PortId = "out", Alignment = new Spot(1, 0.5) }
              .Set(portStyle(false))
          );

      var orTemplate =
        new Node("Spot")
          .Set(nodeStyle)
          .Bind(nodeBind())
          .Add(
            new Shape("OrGate").Set(shapeStyle),
            new Shape("Rectangle") { PortId = "in1", Alignment = new Spot(0.16, 0.3) }
              .Set(portStyle(true)),
            new Shape("Rectangle") { PortId = "in2", Alignment = new Spot(0.16, 0.7) }
              .Set(portStyle(true)),
            new Shape("Rectangle") { PortId = "out", Alignment = new Spot(1, 0.5) }
              .Set(portStyle(false))
          );

      var xorTemplate =
        new Node("Spot")
          .Set(nodeStyle)
          .Bind(nodeBind())
          .Add(
            new Shape("XorGate").Set(shapeStyle),
            new Shape("Rectangle") { PortId = "in1", Alignment = new Spot(0.26, 0.3) }
              .Set(portStyle(true)),
            new Shape("Rectangle") { PortId = "in2", Alignment = new Spot(0.26, 0.7) }
              .Set(portStyle(true)),
            new Shape("Rectangle") { PortId = "out", Alignment = new Spot(1, 0.5) }
              .Set(portStyle(false))
          );

      var norTemplate =
        new Node("Spot")
          .Set(nodeStyle)
          .Bind(nodeBind())
          .Add(
            new Shape("NorGate").Set(shapeStyle),
            new Shape("Rectangle") { PortId = "in1", Alignment = new Spot(0.16, 0.3) }
              .Set(portStyle(true)),
            new Shape("Rectangle") { PortId = "in2", Alignment = new Spot(0.16, 0.7) }
              .Set(portStyle(true)),
            new Shape("Rectangle") { PortId = "out", Alignment = new Spot(1, 0.5) }
              .Set(portStyle(false))
          );

      var xnorTemplate =
        new Node("Spot")
          .Set(nodeStyle)
          .Bind(nodeBind())
          .Add(
            new Shape("XnorGate").Set(shapeStyle),
            new Shape("Rectangle") { PortId = "in1", Alignment = new Spot(0.26, 0.3) }
              .Set(portStyle(true)),
            new Shape("Rectangle") { PortId = "in2", Alignment = new Spot(0.26, 0.7) }
              .Set(portStyle(true)),
            new Shape("Rectangle") { PortId = "out", Alignment = new Spot(1, 0.5) }
              .Set(portStyle(false))
          );

      var nandTemplate =
        new Node("Spot")
          .Set(nodeStyle)
          .Bind(nodeBind())
          .Add(
            new Shape("NandGate").Set(shapeStyle),
            new Shape("Rectangle") { PortId = "in1", Alignment = new Spot(0, 0.3) }
              .Set(portStyle(true)),
            new Shape("Rectangle") { PortId = "in2", Alignment = new Spot(0, 0.7) }
              .Set(portStyle(true)),
            new Shape("Rectangle") { PortId = "out", Alignment = new Spot(1, 0.5) }
              .Set(portStyle(false))
          );

      var notTemplate =
        new Node("Spot")
          .Set(nodeStyle)
          .Bind(nodeBind())
          .Add(
            new Shape("Inverter").Set(shapeStyle),
            new Shape("Rectangle") { PortId = "in", Alignment = new Spot(0, 0.5) }
              .Set(portStyle(true)),
            new Shape("Rectangle") { PortId = "out", Alignment = new Spot(1, 0.5) }
              .Set(portStyle(false))
          );

      // Add the templates created above to the shared template map
      sharedNodeTemplateMap = new Dictionary<string, Part> {
        { "Input", inputTemplate },
        { "Output", outputTemplate },
        { "And", andTemplate },
        { "Or", orTemplate },
        { "Xor", xorTemplate },
        { "Not", notTemplate },
        { "Nand", nandTemplate },
        { "Nor", norTemplate },
        { "Xnor", xnorTemplate }
      };
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.ToolManager.DraggingTool.IsGridSnapEnabled = true;  // dragged nodes will snap to grid of 10x10 cells
      myDiagram.UndoManager.IsEnabled = true;

      // creates relinkable links that will avoid crossing Nodes when possible and will jump over other links
      myDiagram.LinkTemplate =
        new Link {
          Routing = LinkRouting.AvoidsNodes,
          Curve = LinkCurve.JumpOver,
          Corner = 3,
          RelinkableFrom = true, RelinkableTo = true,
          SelectionAdorned = false,  // Links are not adorned when selected so that their color remains visible.
          ShadowOffset = new Point(0, 0), ShadowBlur = 5, ShadowColor = "blue"
        }
          .Bind(new Binding("IsShadowed", "IsSelected").OfElement())
          .Add(
            new Shape {
              Name = "SHAPE", StrokeWidth = 2, Stroke = "red"
            }
          );

      DefineNodeTemplates();
      myDiagram.NodeTemplateMap = sharedNodeTemplateMap;

      // load initial model
      LoadModel();

      // start logic
      Loop();
    }

    private void SetupPalette() {
      myPalette = paletteControl1.Diagram as Palette;

      DefineNodeTemplates();
      myPalette.NodeTemplateMap = sharedNodeTemplateMap;

      myPalette.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Category = "Input" },
          new NodeData { Category = "Output" },
          new NodeData { Category = "And" },
          new NodeData { Category = "Or" },
          new NodeData { Category = "Xor" },
          new NodeData { Category = "Not" },
          new NodeData { Category = "Nand" },
          new NodeData { Category = "Nor" },
          new NodeData { Category = "Xnor" }
        }
      };
    }

    private void Loop() {
      Task.Delay(250).ContinueWith((t) => {
        UpdateStates();
        Loop();
      });
    }

    private void UpdateStates() {
      var oldskip = myDiagram.SkipsUndoManager;
      myDiagram.SkipsUndoManager = true;
      // do all "input" nodes first
      foreach (var node in myDiagram.Nodes) {
        if (node.Category == "Input") {
          DoInput(node);
        }
      }
      // now we can do all other kinds of nodes
      foreach (var node in myDiagram.Nodes) {
        switch (node.Category) {
          case "And": DoAnd(node); break;
          case "Or": DoOr(node); break;
          case "Xor": DoXor(node); break;
          case "Not": DoNot(node); break;
          case "Nand": DoNand(node); break;
          case "Nor": DoNor(node); break;
          case "Xnor": DoXnor(node); break;
          case "Output": DoOutput(node); break;
          case "Input": break;  // DoInput already called, above
        }
      }
      myDiagram.SkipsUndoManager = oldskip;
    }

    // helper predicate
    private bool LinkIsTrue(Link link) { // assume link has a shape named "SHAPE"
      return (link.FindElement("SHAPE") as Shape).Stroke == "green";
    }

    // helper function for propagating results
    private void SetOutputLinks(Node node, Brush color) {
      foreach (var link in node.FindLinksOutOf()) {
        (link.FindElement("SHAPE") as Shape).Stroke = color;
      }
    }

    // update nodes by the specific function for its type
    // determine the color of links coming out of this node based on those coming in and node type

    private void DoInput(Node node) {
      // the output is just the node's Shape.Fill
      SetOutputLinks(node, (node.FindElement("NODESHAPE") as Shape).Fill);
    }

    private void DoAnd(Node node) {
      var color = node.FindLinksInto().All(LinkIsTrue) ? "green" : "red";
      SetOutputLinks(node, color);
    }

    private void DoNand(Node node) {
      var color = !node.FindLinksInto().All(LinkIsTrue) ? "green" : "red";
      SetOutputLinks(node, color);
    }

    private void DoNot(Node node) {
      var color = !node.FindLinksInto().All(LinkIsTrue) ? "green" : "red";
      SetOutputLinks(node, color);
    }

    private void DoOr(Node node) {
      var color = node.FindLinksInto().Any(LinkIsTrue) ? "green" : "red";
      SetOutputLinks(node, color);
    }

    private void DoNor(Node node) {
      var color = !node.FindLinksInto().Any(LinkIsTrue) ? "green" : "red";
      SetOutputLinks(node, color);
    }

    private void DoXor(Node node) {
      var truecount = 0;
      foreach (var link in node.FindLinksInto()) {
        if (LinkIsTrue(link)) {
          truecount++;
        }
      }
      var color = truecount % 2 != 0 ? "green" : "red";
      SetOutputLinks(node, color);
    }

    private void DoXnor(Node node) {
      var truecount = 0;
      foreach (var link in node.FindLinksInto()) {
        if (LinkIsTrue(link)) {
          truecount++;
        }
      }
      var color = truecount % 2 == 0 ? "green" : "red";
      SetOutputLinks(node, color);
    }

    private void DoOutput(Node node) {
      // assume there is just one input link
      // we just need to update the node's Shape.Fill
      foreach (var link in node.LinksConnected) {
        (node.FindElement("NODESHAPE") as Shape).Fill = (link.FindElement("SHAPE") as Shape).Stroke;
      }
    }

    private void SaveModel() {
      if (myDiagram == null) return;
      saveLoadModel1.ModelJson = myDiagram.Model.ToJson();
    }

    private void LoadModel() {
      if (myDiagram == null) return;
      myDiagram.Model = Model.FromJson<Model>(saveLoadModel1.ModelJson);
      myDiagram.Model.UndoManager.IsEnabled = true;
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Loc { get; set; }

  }
  public class LinkData : Model.LinkData { }

}
