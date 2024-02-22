/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;

namespace Demo.Samples.Grafcet {
  public partial class Grafcet : DemoControl {
    private Diagram _Diagram;

    public Grafcet() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      modelJson1.SaveClick = SaveModel;
      modelJson1.LoadClick = LoadModel;

      desc1.MdText = DescriptionReader.Read("Samples.Grafcet.md");

      modelJson1.JsonText = @"{
  ""NodeDataSource"": [
    { ""Key"": 1, ""Category"": ""Start"", ""Loc"": ""300 50"", ""Step"": 1, ""Text"": ""Action 1"" },
    { ""Key"": 2, ""Category"": ""Parallel"", ""Loc"": ""300 100"" },
    { ""Key"": 3, ""Loc"": ""225 125"", ""Step"": 3, ""Text"": ""Action 2"" },
    { ""Key"": 4, ""Loc"": ""325 150"", ""Step"": 4, ""Text"": ""Action 3"" },
    { ""Key"": 5, ""Loc"": ""225 175"", ""Step"": 5, ""Text"": ""Action 4"" },
    { ""Key"": 6, ""Category"": ""Parallel"", ""Loc"": ""300 200"" },
    { ""Key"": 7, ""Loc"": ""300 250"", ""Step"": 7, ""Text"": ""Action 6"" },
    { ""Key"": 11, ""Category"": ""Start"", ""Loc"": ""300 350"", ""Step"": 11, ""Text"": ""Action 1"" },
    { ""Key"": 12, ""Category"": ""Exclusive"", ""Loc"": ""300 400"" },
    { ""Key"": 13, ""Loc"": ""225 450"", ""Step"": 13, ""Text"": ""Action 2"" },
    { ""Key"": 14, ""Loc"": ""325 475"", ""Step"": 14, ""Text"": ""Action 3"" },
    { ""Key"": 15, ""Loc"": ""225 500"", ""Step"": 15, ""Text"": ""Action 4"" },
    { ""Key"": 16, ""Category"": ""Exclusive"", ""Loc"": ""300 550"" },
    { ""Key"": 17, ""Loc"": ""300 600"", ""Step"": 17, ""Text"": ""Action 6"" },
    { ""Key"": 21, ""Loc"": ""500 50"", ""Step"": 21, ""Text"": ""Act 1"" },
    { ""Key"": 22, ""Loc"": ""500 100"", ""Step"": 22, ""Text"": ""Act 2"" },
    { ""Key"": 23, ""Loc"": ""500 150"", ""Step"": 23, ""Text"": ""Act 3"" },
    { ""Key"": 24, ""Loc"": ""500 200"", ""Step"": 24, ""Text"": ""Act 4"" },
    { ""Key"": 31, ""Loc"": ""500 400"", ""Step"": 31, ""Text"": ""Act 1"" },
    { ""Key"": 32, ""Loc"": ""500 450"", ""Step"": 32, ""Text"": ""Act 2"" },
    { ""Key"": 33, ""Loc"": ""500 500"", ""Step"": 33, ""Text"": ""Act 3"" },
    { ""Key"": 34, ""Loc"": ""500 550"", ""Step"": 34, ""Text"": ""Act 4"" }
  ],
  ""LinkDataSource"": [
    { ""From"": 1, ""To"": 2, ""Text"": ""condition 1"" },
    { ""From"": 2, ""To"": 3 },
    { ""From"": 2, ""To"": 4 },
    { ""From"": 3, ""To"": 5, ""Text"": ""condition 2"" },
    { ""From"": 4, ""To"": 6 },
    { ""From"": 5, ""To"": 6 },
    { ""From"": 6, ""To"": 7, ""Text"": ""condition 5"" },
    { ""From"": 11, ""To"": 12, ""Text"": ""condition 1"" },
    { ""From"": 12, ""To"": 13, ""Text"": ""condition 12"" },
    { ""From"": 12, ""To"": 14, ""Text"": ""condition 13"" },
    { ""From"": 13, ""To"": 15, ""Text"": ""condition 2"" },
    { ""From"": 14, ""To"": 16, ""Text"": ""condition 14"" },
    { ""From"": 15, ""To"": 16, ""Text"": ""condition 15"" },
    { ""From"": 16, ""To"": 17, ""Text"": ""condition 5"" },
    { ""From"": 21, ""To"": 22, ""Text"": ""c1"" },
    { ""From"": 22, ""To"": 23, ""Text"": ""c2"" },
    { ""From"": 23, ""To"": 24, ""Text"": ""c3"" },
    { ""From"": 21, ""To"": 24, ""Text"": ""c14"", ""Category"": ""Skip"" },
    { ""From"": 31, ""To"": 32, ""Text"": ""c1"" },
    { ""From"": 32, ""To"": 33, ""Text"": ""c2"" },
    { ""From"": 33, ""To"": 34, ""Text"": ""c3"" },
    { ""From"": 33, ""To"": 32, ""Text"": ""c14"", ""Category"": ""Repeat"" }
  ]
}";

      Setup();
    }

    private void Setup() {
      _Diagram.AllowLink = false;  // linking is only started via buttons, not modelessly
      // see the "StartLink..." functions and CustomLinkingTool defined above
      // double-click in the background creates a new "Start" node
      _Diagram.ToolManager.ClickCreatingTool.ArchetypeNodeData = new NodeData {
        Category = "Start", Step = 1, Text = "Action"
      };
      _Diagram.ToolManager.LinkingTool = new CustomLinkingTool();
      _Diagram.UndoManager.IsEnabled = true;

      // This implements a selection Adornment that is a horizontal bar of command buttons
      // that appear when the user selects a node.
      // Each button has a click function to execute the command, a tooltip for a textual description,
      // and a Binding of "Visible" to hide the button if it cannot be executed for that particular node.

      var commandsAdornment =
        Builder.Make<Adornment>("ContextMenu")
          .Add(
            new Panel("Auto")
              .Add(
                new Shape { Fill = null, Stroke = "deepskyblue", StrokeWidth = 2, ShadowVisible = false },
                new Placeholder()
              ),
            new Panel("Horizontal") { DefaultStretch = Stretch.Vertical }
              .Add(
                Builder.Make<Panel>("Button")
                  .Add(
                    new Shape {
                      GeometryString = "M0 0 L10 0",
                      Fill = null, Stroke = "red", Margin = 3
                    }
                  )
                  .Set(new { Click = new Action<InputEvent, GraphObject>(AddExclusive), ToolTip = MakeToolTip("Add Exclusive") })
                  .Bind(new Binding("Visible", "", CanAddSplit).OfElement()),
                Builder.Make<Panel>("Button")
                  .Add(
                    new Shape {
                      GeometryString = "M0 0 L10 0 M0 3 10 3",
                      Fill = null, Stroke = "red", Margin = 3
                    }
                  )
                  .Set(new { Click = new Action<InputEvent, GraphObject>(AddParallel), ToolTip = MakeToolTip("Add Parallel") })
                  .Bind(new Binding("Visible", "", CanAddSplit).OfElement()),
                Builder.Make<Panel>("Button")
                  .Add(
                    new Shape {
                      GeometryString = "M0 0 L10 0 10 6 0 6z",
                      Fill = "lightyellow", Margin = 3
                    }
                  )
                  .Set(new { Click = new Action<InputEvent, GraphObject>(AddStep), ToolTip = MakeToolTip("Add Step") })
                  .Bind(new Binding("Visible", "", CanAddStep).OfElement()),
                Builder.Make<Panel>("Button")
                  .Add(
                    new Shape {
                      GeometryString = "M0 0 M5 0 L5 10 M3 8 5 10 7 8 M10 0",
                      Fill = null, Margin = 3
                    }
                  )
                  .Set(new { Click = new Action<InputEvent, GraphObject>(StartLinkDown), ToolTip = MakeToolTip("Draw Link Down") })
                  .Bind(new Binding("Visible", "", CanStartLink).OfElement()),
                Builder.Make<Panel>("Button")
                  .Add(
                    new Shape {
                      GeometryString = "M0 0 M3 0 L3 2 7 2 7 6 3 6 3 10 M1 8 3 10 5 8 M10 0",
                      Fill = null, Margin = 3
                    }
                  )
                  .Set(new { Click = new Action<InputEvent, GraphObject>(StartLinkAround), ToolTip = MakeToolTip("Draw Link Skip") })
                  .Bind(new Binding("Visible", "", CanStartLink).OfElement()),
                Builder.Make<Panel>("Button")
                  .Add(
                    new Shape {
                      GeometryString = "M0 0 M3 2 L3 0 7 0 7 10 3 10 3 8 M5 6 7 4 9 6 M10 0",
                      Fill = null, Margin = 3
                    }
                  )
                  .Set(new { Click = new Action<InputEvent, GraphObject>(StartLinkUp), ToolTip = MakeToolTip("Draw Link Repeat") })
                  .Bind(new Binding("Visible", "", CanStartLink).OfElement())
              )
          );


      Adornment MakeToolTip(string str) {  // helper for defining tooltips
        return Builder.Make<Adornment>("ToolTip")
          .Add(new TextBlock(str));
      }

      // commands for adding new nodes
      void AddExclusive(InputEvent e, GraphObject obj) {
        AddSplit((obj.Part as Adornment).AdornedPart as Node, "Exclusive");
      }

      void AddParallel(InputEvent e, GraphObject obj) {
        AddSplit((obj.Part as Adornment).AdornedPart as Node, "Parallel");
      };

      void AddStep(InputEvent e, GraphObject obj) {
        var node = (obj.Part as Adornment).AdornedPart;
        var model = _Diagram.Model as Model;
        model.StartTransaction("add Step");
        var loc = node.Location.Offset(0, 50);
        var nodedata = new NodeData { Loc = Point.Stringify(loc) };
        model.AddNodeData(nodedata);
        var nodekey = model.GetKeyForNodeData(nodedata);
        var linkdata = new LinkData { From = model.GetKeyForNodeData(node.Data as NodeData), To = nodekey, Text = "c" };
        model.AddLinkData(linkdata);
        var newnode = _Diagram.FindNodeForData(nodedata);
        _Diagram.Select(newnode);
        model.CommitTransaction("add Step");
      };

      object CanAddStep(object val, object targetObj) {
        var adorn = val as Adornment;
        var node = adorn.AdornedPart as Node;
        if (node.Category == "" || node.Category == "Start") {
          return node.FindLinksOutOf().Count() == 0;
        } else if (node.Category == "Parallel" || node.Category == "Exclusive") {
          return true;
        }
        return false;
      }

      void AddSplit(Node node, string type) {
        var model = _Diagram.Model as Model;
        model.StartTransaction("add " + type);
        var loc = node.Location.Offset(0, 50);
        var nodedata = new NodeData { Category = type, Loc = Point.Stringify(loc) };
        model.AddNodeData(nodedata);
        var nodekey = model.GetKeyForNodeData(nodedata);
        var linkdata = new LinkData { From = model.GetKeyForNodeData(node.Data as NodeData), To = nodekey };
        model.AddLinkData(linkdata);
        var newnode = _Diagram.FindNodeForData(nodedata);
        _Diagram.Select(newnode);
        model.CommitTransaction("add " + type);
      }

      object CanAddSplit(object val, object targetObj) {
        var adorn = val as Adornment;
        var node = adorn.AdornedPart as Node;
        if (node.Category == "" || node.Category == "Start") {
          return node.FindNodesOutOf().Count() == 0;
        } else if (node.Category == "Parallel" || node.Category == "Exclusive") {
          return false;
        }
        return false;
      }

      // commands for starting drawing new links

      void StartLinkDown(InputEvent e, GraphObject obj) {
        StartLink((obj.Part as Adornment).AdornedPart as Node, "", "c");
      }

      void StartLinkAround(InputEvent e, GraphObject obj) {
        StartLink((obj.Part as Adornment).AdornedPart as Node, "Skip", "s");
      }

      void StartLinkUp(InputEvent e, GraphObject obj) {
        StartLink((obj.Part as Adornment).AdornedPart as Node, "Repeat", "r");
      }

      void StartLink(Node node, string category, string condition) {
        var tool = _Diagram.ToolManager.LinkingTool;
        // to control what kind of link is created,
        // change the LinkingTool.ArchetypeLinKData's category and text
        tool.ArchetypeLinkData = new LinkData { Category = category, Text = condition };
        tool.StartElement = node.Port;
        _Diagram.CurrentTool = tool;
        tool.DoActivate();
      }

      object CanStartLink(object val, object targetObj) {
        var adorn = val as Adornment;
        var node = adorn.AdornedPart;
        return true; // this could be smarter
      }

      // the various kinds of nodes

      // helper function that declares common properties for all nodes
      var commonNodeStyle = new {
        LocationSpot = Spot.Center,
        SelectionAdornmentTemplate = commandsAdornment
      };
      Binding locBind() {
        return new Binding("Location", "Loc", Point.Parse, Point.Stringify);
      }

      _Diagram.NodeTemplateMap["Start"] =
        new Node("Horizontal") {
          LocationElementName = "STEPPANEL", SelectionElementName = "STEPPANEL"
        }
          .Set(commonNodeStyle)
          .Bind(locBind())
          .Add(
            new Panel("Auto") {  // this is the port element, not the whole Node
              Name = "STEPPANEL", PortId = "",
              FromSpot = Spot.Bottom, FromLinkable = true
            }
              .Add(
                new Shape { Fill = "lightgreen" },
                new Panel("Auto") { Margin = 3 }
                  .Add(
                    new Shape { Fill = null, MinSize = new Size(20, 20) },
                    new TextBlock("Start") { Margin = 3, Editable = true }
                      .Bind("Text", "Step", (s, _) => { return s.ToString(); }, (t, _, _) => { return int.Parse(t as string); })
                  )
              ),
            // a connector line between the texts
            new Shape("LineH") { Width = 10, Height = 1 },
            // the boxed, editable text on the side
            new Panel("Auto")
              .Add(
                new Shape { Fill = "white" },
                new TextBlock("Action") { Margin = 3, Editable = true }
                  .Bind(new Binding("Text").MakeTwoWay())
              )
          );

      _Diagram.NodeTemplateMap[""] =
        new Node("Horizontal") {
          LocationElementName = "STEPPANEL", SelectionElementName = "STEPPANEL"
        }
          .Set(commonNodeStyle)
          .Bind(locBind())
          .Add(
            new Panel("Auto") {  // this is the port element, not the whole Node
              Name = "STEPPANEL", PortId = "",
              FromSpot = Spot.Bottom, FromLinkable = true,
              ToSpot = Spot.Top, ToLinkable = true
            }
              .Add(
                new Shape { Fill = "lightyellow", MinSize = new Size(20, 20) },
                new TextBlock("Step") { Margin = 3, Editable = true }
                  .Bind("Text", "Step", (s, _) => { return s.ToString(); }, (t, _, _) => { return int.Parse(t as string); })
              ),
            new Shape("LineH") { Width = 10, Height = 1 },
            new Panel("Auto")
              .Add(
                new Shape { Fill = "white" },
                new TextBlock("Action") { Margin = 3, Editable = true }
                  .Bind(new Binding("Text").MakeTwoWay())
              )
          );

      var resizeAdornment =
        new Adornment("Spot")
          .Add(
            new Placeholder(),
            new Shape { // left resize handle
              Alignment = Spot.Left, Cursor = "col-resize",
              DesiredSize = new Size(6, 6), Fill = "lightblue", Stroke = "dodgerblue"
            },
            new Shape { // right resize handle
              Alignment = Spot.Right, Cursor = "col-resize",
              DesiredSize = new Size(6, 6), Fill = "lightblue", Stroke = "dodgerblue"
            }
          );

      _Diagram.NodeTemplateMap["Parallel"] =
        new Node {  // special resizing: just at the ends
          Resizable = true, ResizeElementName = "SHAPE", ResizeAdornmentTemplate = resizeAdornment,
          FromLinkable = true, ToLinkable = true
        }
          .Set(commonNodeStyle)
          .Bind(locBind())
          .Add(
            new Shape {  // horizontal pair of lines stretched to an initial width of 200
              Name = "SHAPE", GeometryString = "M0 0 L100 0 M0 4 L100 4",
              Fill = "transparent", Stroke = "red", Width = 200
            }
              .Bind("DesiredSize", "Size", Northwoods.Go.Size.Parse, Northwoods.Go.Size.Stringify)
          );

      _Diagram.NodeTemplateMap["Exclusive"] =
        new Node {  // special resizing: just at the ends
          Resizable = true, ResizeElementName = "SHAPE", ResizeAdornmentTemplate = resizeAdornment,
          FromLinkable = true, ToLinkable = true
        }
          .Set(commonNodeStyle)
          .Bind(locBind())
          .Add(
            new Shape {  // horizontal line stretched to an initial width of 200
              Name = "SHAPE", GeometryString = "M0 0 L100 0",
              Fill = "transparent", Stroke = "red", Width = 200
            }
              .Bind("DesiredSize", "Size", Northwoods.Go.Size.Parse, Northwoods.Go.Size.Stringify)
          );

      // various kinds of links

      // replace default link
      _Diagram.LinkTemplateMap[""] =
        new BarLink { Routing = LinkRouting.Orthogonal }
          .Add(
            new Shape { StrokeWidth = 1.5 },
            new Shape("LineH") {  // only visible when there is text
              Width = 20, Height = 1, Visible = false
            }
              .Bind("Visible", "Text", (t, _) => { return t as string != ""; }),
            new TextBlock {  // only visible when there is text
              AlignmentFocus = new Spot(0, 0.5, -12, 0), Editable = true
            }
              .Bind(new Binding("Text").MakeTwoWay())
              .Bind("Visible", "Text", (t, _) => { return t as string != ""; })
          );

      _Diagram.LinkTemplateMap["Skip"] =
        new Link {
          Routing = LinkRouting.AvoidsNodes,
          FromSpot = Spot.Bottom, ToSpot = Spot.Top,
          FromEndSegmentLength = 4, ToEndSegmentLength = 4
        }
          .Add(
            new Shape { StrokeWidth = 1.5 },
            new Shape("LineH") {  // only visible when there is text
              Width = 20, Height = 1, Visible = false
            }
              .Bind("Visible", "Text", (t, _) => { return t as string != ""; }),
            new TextBlock {  // only visible when there is text
              AlignmentFocus = new Spot(1, 0.5, 12, 0), Editable = true
            }
              .Bind(new Binding("Text").MakeTwoWay())
              .Bind("Visible", "Text", (t, _) => { return (t as string) != ""; })
          );

      _Diagram.LinkTemplateMap["Repeat"] =
        new Link {
          Routing = LinkRouting.AvoidsNodes,
          FromSpot = Spot.Bottom, ToSpot = Spot.Top,
          FromEndSegmentLength = 4, ToEndSegmentLength = 4
        }
          .Add(
            new Shape { StrokeWidth = 1.5 },
            new Shape { ToArrow = "OpenTriangle", SegmentIndex = 3, SegmentFraction = 0.75 },
            new Shape { ToArrow = "OpenTriangle", SegmentIndex = 3, SegmentFraction = 0.25 },
            new Shape("LineH") {  // only visible when there is text
              Width = 20, Height = 1, Visible = false
            }
              .Bind("Visible", "Text", (t, _) => { return t as string != ""; }),
            new TextBlock {  // only visible when there is text
              AlignmentFocus = new Spot(1, 0.5, 12, 0), Editable = true
            }
              .Bind(new Binding("Text").MakeTwoWay())
              .Bind("Visible", "Text", (t, _) => { return t as string != ""; })
          );

      LoadModel();
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
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public int Step { get; set; }
    public string Loc { get; set; }
    public string Size { get; set; }
  }

  public class LinkData : Model.LinkData { }

  // This custom LinkingTool just turns on Diagram.AllowLink when it starts,
  // and turns it off again when it stops so that users cannot draw new links modelessly.
  public class CustomLinkingTool : LinkingTool {
    // user-drawn linking is normally disabled
    // but needs to be turned on when using this tool
    public override void DoStart() {
      Diagram.AllowLink = true;
      base.DoStart();
    }

    public override void DoStop() {
      base.DoStop();
      Diagram.AllowLink = false;
    }
  }

  // This custom link class is smart about computing the link point and direction
  // at "Parallel" and "Exclusive" nodes
  public class BarLink : Link {
    public override Point GetLinkPoint(Node node, GraphObject port, Spot spot, bool from, bool ortho, Node othernode, GraphObject otherport) {
      var r = port.GetDocumentBounds();
      var op = otherport.GetDocumentBounds();
      var below = op.CenterY > r.CenterY;
      var y = below ? r.Bottom : r.Top;
      if (node.Category == "Parallel" || node.Category == "Exclusive") {
        if (op.Right < r.Left) return new Point(r.Left, y);
        if (op.Left > r.Right) return new Point(r.Right, y);
        return new Point((Math.Max(r.Left, op.Left) + Math.Min(r.Right, op.Right)) / 2, y);
      } else {
        return new Point(r.CenterX, y);
      }
    }

    public override int GetLinkDirection(Node node, GraphObject port, Point linkpoint, Spot spot, bool from, bool ortho, Node othernode, GraphObject otherport) {
      var p = port.GetDocumentPoint(Spot.Center);
      var op = otherport.GetDocumentPoint(Spot.Center);
      var below = op.Y > p.Y;
      return below ? 90 : 270;
    }
  }
}
