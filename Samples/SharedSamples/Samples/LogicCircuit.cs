/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Extensions;
using Northwoods.Go.Models;

namespace Demo.Samples.LogicCircuit {
  public partial class LogicCircuit : DemoControl {
    private Diagram _Diagram;
    private Palette _Palette;

    private Dictionary<string, Part> sharedNodeTemplateMap;

    public LogicCircuit() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;
      _Palette = paletteControl1.Diagram as Palette;

      modelJson1.SaveClick = SaveModel;
      modelJson1.LoadClick = LoadModel;

      desc1.MdText = DescriptionReader.Read("Samples.LogicCircuit.md");

      modelJson1.JsonText = @"{
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

      Setup();
      SetupPalette();
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
      void nodeStyle(Node node) {
        node
          .Bind(new[] {
            new Binding("Location", "Loc", Point.Parse, Point.Stringify),
            new Binding("IsShadowed", "IsSelected").OfElement()
          })
          .Set(new {
            SelectionAdorned = false,
            ShadowOffset = new Point(0, 4),
            ShadowBlur = 15,
            ShadowColor = "blue",
            Resizable = true,
            ResizeElementName = "NODESHAPE",
            ToolTip = sharedToolTip
          });
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
          .Apply(nodeStyle)
          .Add(
            new Shape("Circle")
              .Set(shapeStyle)
              .Set(new { Fill = "red" }),  // override the default fill (from shapeStyle) to be red
            new Shape("Rectangle") { Name = "outport", PortId = "", Alignment = new Spot(1, 0.5) }  // the only port
              .Set(portStyle(false))
          );

      var outputTemplate =
        new Node("Spot")
          .Apply(nodeStyle)
          .Add(
            new Shape("Rectangle")
              .Set(shapeStyle)
              .Set(new { Fill = "green" }),  // override the default fill (from shapeStyle) to be green
            new Shape("Rectangle") { PortId = "", Alignment = new Spot(0, 0.5) }  // the only port
              .Set(portStyle(true))
          );

      var andTemplate =
        new Node("Spot")
          .Apply(nodeStyle)
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
          .Apply(nodeStyle)
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
          .Apply(nodeStyle)
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
          .Apply(nodeStyle)
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
          .Apply(nodeStyle)
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
          .Apply(nodeStyle)
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
          .Apply(nodeStyle)
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
      _Diagram.ToolManager.DraggingTool.IsGridSnapEnabled = true;  // dragged nodes will snap to grid of 10x10 cells
      _Diagram.UndoManager.IsEnabled = true;

      // creates relinkable links that will avoid crossing Nodes when possible and will jump over other links
      _Diagram.LinkTemplate =
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
      _Diagram.NodeTemplateMap = sharedNodeTemplateMap;

      // load initial model
      LoadModel();

      // start logic
      Loop();
    }

    private void SetupPalette() {
      DefineNodeTemplates();
      _Palette.NodeTemplateMap = sharedNodeTemplateMap;

      _Palette.Model = new Model {
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

    private async void Loop() {
      await Task.Delay(250);
      UpdateStates();
      Loop();
    }

    private void UpdateStates() {
      var oldskip = _Diagram.SkipsUndoManager;
      _Diagram.SkipsUndoManager = true;
      // do all "input" nodes first
      foreach (var node in _Diagram.Nodes) {
        if (node.Category == "Input") {
          DoInput(node);
        }
      }
      // now we can do all other kinds of nodes
      foreach (var node in _Diagram.Nodes) {
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
      _Diagram.SkipsUndoManager = oldskip;
    }

    // helper predicate
    private bool LinkIsTrue(Link link) { // assume link has a shape named "SHAPE"
      var shape = link.FindElement("SHAPE") as Shape;
      return shape.Stroke == "green";
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
      if (_Diagram == null) return;
      modelJson1.JsonText = _Diagram.Model.ToJson();
    }

    private void LoadModel() {
      if (_Diagram == null) return;
      _Diagram.Model = Model.FromJson<Model>(modelJson1.JsonText);
      _Diagram.Model.UndoManager.IsEnabled = true;
    }
  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Loc { get; set; }

  }
  public class LinkData : Model.LinkData { }
}
