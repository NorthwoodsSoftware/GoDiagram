using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go.Tools.Extensions;
using Northwoods.Go.Extensions;
using System.Threading.Tasks;
using Northwoods.Go.WinForms;

namespace WinFormsExtensionControls.PortShifting {
  [ToolboxItem(false)]
  public partial class PortShiftingControl : System.Windows.Forms.UserControl {
    public Diagram myDiagram;
    private Diagram myPalette;
    private Dictionary<string, Part> sharedNodeTemplateMap;
    public PortShiftingControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;
      paletteControl1.AfterRender = SetupPalette;

      saveLoadModel1.SaveClick += (e, obj) => SaveModel();
      saveLoadModel1.LoadClick += (e, obj) => LoadModel();

      goWebBrowser1.Html = @"
           <p>
      This is exactly like the <a href=""LogicCircuit"">Logic Circuit sample</a>
      but also makes use of the PortShiftingTool,
      which is defined in <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/PortShifting/PortShiftingTool.cs"">PortShiftingTool.cs</a>
          </p>
            <p>
      When the user wants to shift the position of a port on a node,
      the user can hold down the Shift key during a mouse - down on a port element.
      Dragging then will move the port within the node.
            </p>
            <p>
      Note how the relative position of the port within the node is maintained as you resize the node.
            </p>
            <p>
      If you want to persist the port's spot, you should add a TwoWay Binding of the <a>GraphObject.Alignment</a>
      property with a property that you define on the node data for each port.
            </p>
            <p>
      This sample does not constrain the position of the port within the node,
      but you could adapt the PortShiftingTool.UpdateAlignment method to do so.
      For example if you wanted, you could keep a port stuck along one edge of the node.
            </p>
";
      saveLoadModel1.ModelJson = @"
        { 
          ""LinkFromPortIdProperty"": ""FromPort"",
          ""LinkToPortIdProperty"": ""ToPort"",
          ""NodeDataSource"": [
            {""Category"":""Input"", ""Key"":""Input1"", ""Loc"":""-150 -80"" },
            {""Category"":""Or"", ""Key"":""Or1"", ""Loc"":""-70 0"" },
            { ""Category"":""Not"", ""Key"":""Not1"", ""Loc"":""10 0"" },
            { ""Category"":""Xor"", ""Key"":""Xor1"", ""Loc"":""100 0"" },
            { ""Category"":""Or"", ""Key"":""Or2"", ""Loc"":""200 0"" },
            { ""Category"":""Output"", ""Key"":""Output1"", ""Loc"":""200 -100"" }
          ],
          ""LinkDataSource"": [
            {""From"":""Input1"", ""FromPort"":""out"", ""To"":""Or1"", ""ToPort"":""in1""},
            { ""From"":""Or1"", ""FromPort"":""out"", ""To"":""Not1"", ""ToPort"":""in""},
            { ""From"":""Not1"", ""FromPort"":""out"", ""To"":""Or1"", ""ToPort"":""in2""},
            { ""From"":""Not1"", ""FromPort"":""out"", ""To"":""Xor1"", ""ToPort"":""in1""},
            { ""From"":""Xor1"", ""FromPort"":""out"", ""To"":""Or2"", ""ToPort"":""in1""},
            { ""From"":""Or2"", ""FromPort"":""out"", ""To"":""Xor1"", ""ToPort"":""in2""},
            { ""From"":""Xor1"", ""FromPort"":""out"", ""To"":""Output1"", ""ToPort"":""""}
          ]}
  
";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;



      // diagram properties
      myDiagram.ToolManager.DraggingTool.IsGridSnapEnabled = true; // dragged nodes will snap to grid of 10x10 cells
      myDiagram.UndoManager.IsEnabled = true;

      // install the PortShiftingTool as a mouse-move tool
      myDiagram.ToolManager.MouseMoveTools.Insert(0, new PortShiftingTool());

      // creates relinkable links that will avoid crossing Nodes when possible
      // and will jump over other links
      myDiagram.LinkTemplate =
        new Link {
          Routing = LinkRouting.AvoidsNodes,
          Curve = LinkCurve.JumpOver,
          Corner = 3,
          RelinkableFrom = true,
          RelinkableTo = true,
          SelectionAdorned = false, // Links are not adorned when selected so that their color remains visible.
          ShadowOffset = new Point(0, 0),
          ShadowBlur = 5,
          ShadowColor = "blue"
        }
        .Bind(new Binding("IsShadowed", "IsSelected").OfElement())
        .Add(new Shape {
          Name = "SHAPE",
          StrokeWidth = 2,
          Stroke = "orangered"
        }
        );

      DefineNodeTemplates();
      myDiagram.NodeTemplateMap = sharedNodeTemplateMap;

      // load initial model
      LoadModel();
      

      // start logic
      Loop(myDiagram);
    }

    private void SetupPalette() {
      myPalette = paletteControl1.Diagram as Palette;
      // palette properties
      myPalette.AllowDragOut = true;
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

    private static void Loop(Diagram d) {
      Task.Delay(250).ContinueWith((t) => {
        UpdateStates(d);
        Loop(d);
      });
    }

    private static void UpdateStates(Diagram d) {
      var oldskip = d.SkipsUndoManager;
      d.SkipsUndoManager = true;
      // do all "input" nodes first
      foreach (var node in d.Nodes) {
        if (node.Category == "Input") {
          DoInput(node);
        }
      }
      // now we can do all other kinds of nodes
      foreach (var node in d.Nodes) {
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
      d.SkipsUndoManager = oldskip;
    }

    // helper predicate
    private static bool LinkIsTrue(Link link) { // assume link has a shape named "SHAPE"
      return (link.FindElement("SHAPE") as Shape).Stroke == "green";
    }

    // helper function for propagating results
    private static void SetOutputLinks(Node node, Brush color) {
      foreach (var link in node.FindLinksOutOf()) {
        (link.FindElement("SHAPE") as Shape).Stroke = color;
      }
    }

    // update nodes by the specific function for its type
    // determine the color of links coming out of this node based on those coming in and node type

    private static void DoInput(Node node) {
      // the output is just the node's Shape.Fill
      SetOutputLinks(node, (node.FindElement("NODESHAPE") as Shape).Fill);
    }

    private static void DoAnd(Node node) {
      var color = node.FindLinksInto().All(LinkIsTrue) ? "green" : "red";
      SetOutputLinks(node, color);
    }

    private static void DoNand(Node node) {
      var color = !node.FindLinksInto().All(LinkIsTrue) ? "green" : "red";
      SetOutputLinks(node, color);
    }

    private static void DoNot(Node node) {
      var color = !node.FindLinksInto().All(LinkIsTrue) ? "green" : "red";
      SetOutputLinks(node, color);
    }

    private static void DoOr(Node node) {
      var color = node.FindLinksInto().Any(LinkIsTrue) ? "green" : "red";
      SetOutputLinks(node, color);
    }

    private static void DoNor(Node node) {
      var color = !node.FindLinksInto().Any(LinkIsTrue) ? "green" : "red";
      SetOutputLinks(node, color);
    }

    private static void DoXor(Node node) {
      var truecount = 0;
      foreach (var link in node.FindLinksInto()) {
        if (LinkIsTrue(link)) {
          truecount++;
        }
      }
      var color = truecount % 2 != 0 ? "green" : "red";
      SetOutputLinks(node, color);
    }

    private static void DoXnor(Node node) {
      var truecount = 0;
      foreach (var link in node.FindLinksInto()) {
        if (LinkIsTrue(link)) {
          truecount++;
        }
      }
      var color = truecount % 2 == 0 ? "green" : "red";
      SetOutputLinks(node, color);
    }

    private static void DoOutput(Node node) {
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

    private void DefineNodeTemplates() {
      if (sharedNodeTemplateMap != null) return;
      // load extra shapes
      Figures.DefineExtraFigures();

      // node template headers
      var sharedToolTip =
        Builder.Make<Adornment>("ToolTip").Add(
          new TextBlock {
            Margin = 2
          }.Bind(
            new Binding("Text", "", (data, obj) => {
              return (data as NodeData).Category;
            })
          )
        ) as Adornment;
      (sharedToolTip.FindElement("Border") as Shape).Figure = "RoundedRectangle";


      // define some common property settings
      Node NodeStyle() {
        return new Node(PanelLayoutSpot.Instance) {
          SelectionAdorned = false,
          ShadowOffset = new Point(0, 4),
          ShadowBlur = 15,
          ShadowColor = "blue",
          Resizable = true,
          ResizeElementName = "NODESHAPE",
          ToolTip = sharedToolTip
        }.Bind(
          new Binding("Location", "Loc", Point.Parse).MakeTwoWay(Point.Stringify),
          new Binding("IsShadowed", "IsSelected").OfElement()
        );
      }

      Shape ShapeStyle(string figIn) {
        return new Shape {
          Figure = figIn,
          Name = "NODESHAPE",
          Fill = "lightgray",
          Stroke = "darkslategray",
          DesiredSize = new Size(40, 40),
          StrokeWidth = 2
        };
      }

      Shape PortStyle(bool input, string figIn, string portIdIn, Spot alignmentIn) {
        return new Shape {
          Figure = figIn,
          DesiredSize = new Size(6, 6),
          Fill = "black",
          FromSpot = Spot.Right,
          FromLinkable = !input,
          ToSpot = Spot.Left,
          ToLinkable = input,
          ToMaxLinks = 1,
          Cursor = "pointer",
          PortId = portIdIn,
          Alignment = alignmentIn
        };
      }

      // input template
      var inputPort = PortStyle(false, "Rectangle", "", new Spot(1, 0.5));
      var inputShape = ShapeStyle("Circle");
      inputShape.Fill = "red";
      var inputTemplate = NodeStyle().Add(
        inputShape,
        inputPort
      );
      // if double-clicked, and input node will change its value, represented by the color
      inputTemplate.DoubleClick = (e, obj) => {
        if (!(obj is Node)) return;
        var node = obj as Node;
        e.Diagram.StartTransaction("Toggle Input");
        var shp = node.FindElement("NODESHAPE") as Shape;
        shp.Fill = (shp.Fill == "green") ? "red" : "green";
        UpdateStates(e.Diagram);
        e.Diagram.CommitTransaction("Toggle Input");
      };

      // output template
      var outputPort = PortStyle(true, "Rectangle", "", new Spot(0, 0.5));
      var outputShape = ShapeStyle("Rectangle");
      outputShape.Fill = "green";
      var outputTemplate = NodeStyle().Add(
        outputShape,
        outputPort
      );

      // and template
      var andPort1 = PortStyle(true, "Rectangle", "in1", new Spot(0, 0.3));
      var andPort2 = PortStyle(true, "Rectangle", "in2", new Spot(0, 0.7));
      var andPort3 = PortStyle(false, "Rectangle", "out", new Spot(1, 0.5));
      var andShape = ShapeStyle("AndGate");
      var andTemplate = NodeStyle().Add(
        andShape,
        andPort1,
        andPort2,
        andPort3
      );

      // or template
      var orPort1 = PortStyle(true, "Rectangle", "in1", new Spot(0.16, 0.3));
      var orPort2 = PortStyle(true, "Rectangle", "in2", new Spot(0.16, 0.7));
      var orPort3 = PortStyle(false, "Rectangle", "out", new Spot(1, 0.5));
      var orShape = ShapeStyle("OrGate");
      var orTemplate = NodeStyle().Add(
        orShape,
        orPort1,
        orPort2,
        orPort3
      );

      // xor template
      var xorPort1 = PortStyle(true, "Rectangle", "in1", new Spot(0.26, 0.3));
      var xorPort2 = PortStyle(true, "Rectangle", "in2", new Spot(0.26, 0.7));
      var xorPort3 = PortStyle(false, "Rectangle", "out", new Spot(1, 0.5));
      var xorShape = ShapeStyle("XorGate");
      var xorTemplate = NodeStyle().Add(
        xorShape,
        xorPort1,
        xorPort2,
        xorPort3
      );

      // nor template
      var norPort1 = PortStyle(true, "Rectangle", "in1", new Spot(0.16, 0.3));
      var norPort2 = PortStyle(true, "Rectangle", "in2", new Spot(0.16, 0.7));
      var norPort3 = PortStyle(false, "Rectangle", "out", new Spot(1, 0.5));
      var norShape = ShapeStyle("NorGate");
      var norTemplate = NodeStyle().Add(
        norShape,
        norPort1,
        norPort2,
        norPort3
      );

      // xnor template
      var xnorPort1 = PortStyle(true, "Rectangle", "in1", new Spot(0.26, 0.3));
      var xnorPort2 = PortStyle(true, "Rectangle", "in2", new Spot(0.26, 0.7));
      var xnorPort3 = PortStyle(false, "Rectangle", "out", new Spot(1, 0.5));
      var xnorShape = ShapeStyle("XorGate");
      var xnorTemplate = NodeStyle().Add(
        xnorShape,
        xnorPort1,
        xnorPort2,
        xnorPort3
      );

      // nand template
      var nandPort1 = PortStyle(true, "Rectangle", "in1", new Spot(0, 0.3));
      var nandPort2 = PortStyle(true, "Rectangle", "in2", new Spot(0, 0.7));
      var nandPort3 = PortStyle(false, "Rectangle", "out", new Spot(1, 0.5));
      var nandShape = ShapeStyle("NandGate");
      var nandTemplate = NodeStyle().Add(
        nandShape,
        nandPort1,
        nandPort2,
        nandPort3
      );

      // not template
      var notPort1 = PortStyle(true, "Rectangle", "in", new Spot(0, 0.5));
      var notPort2 = PortStyle(false, "Rectangle", "out", new Spot(1, 0.5));
      var notShape = ShapeStyle("Inverter");
      var notTemplate = NodeStyle().Add(
        notShape,
        notPort1,
        notPort2
      );

      // Add the templates created above to diagram and palette
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
  }

  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Loc { get; set; }

  }
  public class LinkData : Model.LinkData { }
}
