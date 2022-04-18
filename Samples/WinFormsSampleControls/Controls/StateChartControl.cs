/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;

namespace WinFormsSampleControls.StateChart {
  [ToolboxItem(false)]
  public partial class StateChartControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public StateChartControl() {
      InitializeComponent();

      saveLoadModel1.SaveClick += (e, obj) => SaveModel();
      saveLoadModel1.LoadClick += (e, obj) => LoadModel();

      goWebBrowser1.Html = @"
        <p>
      This sample creates a state chart to story-board an online shopping experience.
        </p>
        <p>
      The text is editable for both the nodes and the links.
      The user can draw as many links from one node to another node as desired,
      and the links can be reshaped or deleted when selected.
      Double-clicking in the background of the diagram creates a new node.
      The mouse wheel is set to zoom in and out instead of scroll.
        </p>
        <p>
      This sample customizes the <a>Part.SelectionAdornmentTemplate</a>
      of the node to a template that contains a button
      The button is positioned to be at the Top-Right corner of the node by
      being in a Spot Panel with its <a>GraphObject.Alignment</a> property set to Spot.TopRight.
        </p>
        <p>
      The Button's <a>GraphObject.Click</a> method creates a new node data,
      adds it to the model, and creates a link from the original node data to the new node data.
      All of this is done inside a transaction, so that it can be undone by the user
      (Ctrl+Z and Ctrl+Y will undo and redo transactions). After the node is created,
      <a>Diagram.ScrollToRect</a> is called in case it is off-screen.
        </p>
";

      saveLoadModel1.ModelJson = @"{
  ""NodeKeyProperty"": ""Id"",
  ""NodeDataSource"": [
    { ""Id"": ""-1"", ""Loc"": ""155 -138"", ""Category"": ""Start"" },
    { ""Id"": ""0"", ""Loc"": ""190 15"", ""Text"": ""Shopping"" },
    { ""Id"": ""1"", ""Loc"": ""353 32"", ""Text"": ""Browse Items"" },
    { ""Id"": ""2"", ""Loc"": ""353 166"", ""Text"": ""Search Items"" },
    { ""Id"": ""3"", ""Loc"": ""512 12"", ""Text"": ""View Item"" },
    { ""Id"": ""4"", ""Loc"": ""661 17"", ""Text"": ""View Cart"" },
    { ""Id"": ""5"", ""Loc"": ""644 171"", ""Text"": ""Update Cart"" },
    { ""Id"": ""6"", ""Loc"": ""800 96"", ""Text"": ""Checkout"" },
    { ""Id"": ""-2"", ""Loc"": ""757 229"", ""Category"": ""End"" }
  ],
  ""LinkDataSource"": [
    { ""From"": ""-1"", ""To"": ""0"", ""Text"": ""Visit online store"" },
    { ""From"": ""0"", ""To"": ""1"", ""Progress"": true, ""Text"": ""Browse"" },
    { ""From"": ""0"", ""To"": ""2"", ""Progress"": true, ""Text"": ""Use search bar"" },
    { ""From"": ""1"", ""To"": ""2"", ""Progress"": true, ""Text"": ""Use search bar"" },
    { ""From"": ""2"", ""To"": ""3"", ""Progress"": true, ""Text"": ""Click item"" },
    { ""From"": ""2"", ""To"": ""2"", ""Text"": ""Another search"", ""Curviness"": 20 },
    { ""From"": ""1"", ""To"": ""3"", ""Progress"": true, ""Text"": ""Click item"" },
    { ""From"": ""3"", ""To"": ""0"", ""Text"": ""Not interested"", ""Curviness"": -100 },
    { ""From"": ""3"", ""To"": ""4"", ""Progress"": true, ""Text"": ""Add to cart"" },
    { ""From"": ""4"", ""To"": ""0"", ""Text"": ""More shopping"", ""Curviness"": -150 },
    { ""From"": ""4"", ""To"": ""5"", ""Text"": ""Update needed"", ""Curviness"": -50 },
    { ""From"": ""5"", ""To"": ""4"", ""Text"": ""Update made"" },
    { ""From"": ""4"", ""To"": ""6"", ""Progress"": true, ""Text"": ""Proceed"" },
    { ""From"": ""6"", ""To"": ""5"", ""Text"": ""Update needed"" },
    { ""From"": ""6"", ""To"": ""-2"", ""Progress"": true, ""Text"": ""Purchase made"" }
  ]
}";

      Setup();
    }

    private void Setup() {
      // some constants that will be reused within templates
      var roundedRectangleParams = new {
        Parameter1 = 2,  // set the rounded corner
        Spot1 = Spot.TopLeft, Spot2 = Spot.BottomRight  // make content go all the way to inside edges of rounded corners
      };

      myDiagram = diagramControl1.Diagram;

      // diagram properties
      // myDiagram.AnimationManager.InitialAnimationStyle = AnimationStyle.None;
      // myDiagram.InitialAnimationStarting = (e) => {} <- TODO
      // mouse wheel zooms instead of scrolling
      myDiagram.ToolManager.MouseWheelBehavior = WheelMode.Zoom;
      // support double-click in background to create a new node
      myDiagram.ToolManager.ClickCreatingTool.ArchetypeNodeData = new NodeData { Text = "new node" };
      myDiagram.UndoManager.IsEnabled = true;
      myDiagram.PositionComputation = (diagram, pt) => {
        return new Point(Math.Floor(pt.X), Math.Floor(pt.Y));
      };

      // node template
      myDiagram.NodeTemplate =
        new Node("Auto") {
          LocationSpot = Spot.Top,
          IsShadowed = true, ShadowBlur = 1,
          ShadowOffset = new Point(0, 1),
          ShadowColor = "rgba(0, 0, 0, .14)"
        }
          .Bind("Location", "Loc", Point.Parse, Point.Stringify)
          .Add(
            // define the node's outer shape, which will surround the TextBlock
            new Shape("RoundedRectangle") {
              Name = "SHAPE", Fill = "#ffffff", StrokeWidth = 0,
              Stroke = null,
              PortId = "",  // this shape is the Node's port, not the whole Node
              FromLinkable = true, FromLinkableDuplicates = true, FromLinkableSelfNode = true,
              ToLinkable = true, ToLinkableDuplicates = true, ToLinkableSelfNode = true,
              Cursor = "pointer"
            }
              .Set(roundedRectangleParams),
            new TextBlock {
              Font = new Font("Segoe UI", 11, FontWeight.Bold), Margin = 7, Stroke = "rgba(0, 0, 0, .87)",
              Editable = true // editing the text automatically updates the model data
            }
              .Bind(new Binding("Text").MakeTwoWay())
          );

      // clicking the button inserts a new node to the right of the selected node,
      // and adds a link to that new node
      Action<InputEvent, GraphObject> addNodeAndLink = (InputEvent e, GraphObject obj) => {
        var adorn = obj.Part as Adornment;
        var fromNode = adorn.AdornedPart;
        if (fromNode == null) return;

        e.Handled = true;
        var diagram = e.Diagram;
        diagram.StartTransaction("Add State");

        // get the node data for which the user clicked the button
        var fromData = fromNode.Data as NodeData;
        Point p = fromNode.Location.Offset(200, 0);
        // create a new "state" data object, positioned to the right of the adorned node
        var toData = new NodeData { Text = "new", Loc = Point.Stringify(p) };
        var model = diagram.Model as Model;
        model.AddNodeData(toData);

        // create a link data from the old node data to the new node data
        var linkdata = new LinkData {
          From = fromData.Id,
          To = toData.Id,
          Text = "transition"
        };
        // add the link data to model
        model.AddLinkData(linkdata);

        // select new node
        var newnode = diagram.FindNodeForData(toData);
        diagram.Select(newnode);

        diagram.CommitTransaction("Add State");

        // if new node is off-screen, scroll to show new node
        if (newnode != null) diagram.ScrollToRect(newnode.ActualBounds);
      };

      // unlike the normal selection Adornment, this one includes a Button
      myDiagram.NodeTemplate.SelectionAdornmentTemplate =
        new Adornment("Spot")
          .Add(
            new Panel("Auto")
              .Add(
                new Shape("RoundedRectangle") {
                  Fill = null, Stroke = "#7986cb", StrokeWidth = 3
                }
                .Set(roundedRectangleParams),
                new Placeholder()
            ),
            // the button to create a "next" node, at the top-right corner
            Builder.Make<Panel>("Button")
              .Set(new {
                Alignment = Spot.TopRight,
                Click = addNodeAndLink
              })
              .Add(new Shape("PlusLine") { Width = 6, Height = 6 })
          );

      // define more node templates
      myDiagram.NodeTemplateMap.Add("Start",
        new Node("Spot") { DesiredSize = new Size(75, 75) }
          .Bind("Location", "Loc", Point.Parse, Point.Stringify)
          .Add(
            new Shape("Circle") {
              Fill = "#52ce60",  // green
              Stroke = null,
              PortId = "",
              FromLinkable = true, FromLinkableDuplicates = true, FromLinkableSelfNode = true,
              ToLinkable = true, ToLinkableDuplicates = true, ToLinkableSelfNode = true,
              Cursor = "pointer"
            },
            new TextBlock("Start") {
              Font = new Font("Segoe UI", 16, FontWeight.Bold),
              Stroke = "whitesmoke"
            }
          )
      );

      myDiagram.NodeTemplateMap.Add("End",
        new Node("Spot") { DesiredSize = new Size(75, 75) }
          .Bind("Location", "Loc", Point.Parse, Point.Stringify)
          .Add(
            new Shape("Circle") {
              Fill = "maroon",
              Stroke = null,
              PortId = "",
              FromLinkable = true, FromLinkableDuplicates = true, FromLinkableSelfNode = true,
              ToLinkable = true, ToLinkableDuplicates = true, ToLinkableSelfNode = true,
              Cursor = "pointer"
            },
            new Shape("Circle") {
              Fill = null, DesiredSize = new Size(65, 65),
              StrokeWidth = 2, Stroke = "whitesmoke"
            },
            new TextBlock("End") {
              Font = new Font("Segoe UI", 16, FontWeight.Bold),
              Stroke = "whitesmoke"
            }
          )
      );

      // link template
      myDiagram.LinkTemplate =
        new Link {
          Curve = LinkCurve.Bezier,
          Adjusting = LinkAdjusting.Stretch,
          Reshapable = true, RelinkableFrom = true, RelinkableTo = true,
          ToShortLength = 3
        }
          .Bind(new Binding("Points").MakeTwoWay())
          .Bind("Curviness")
          .Add(
            new Shape { StrokeWidth = 1.5 }  // the link shape
              .Bind("Stroke", "Progress", (progress, obj) => {
                return (bool)progress ? "#52ce60" /* green */ : "black";
              })
              .Bind("StrokeWidth", "Progress", (progress, obj) => {
                return (bool)progress ? 2.5 : 1.5;
              }),
            new Shape {  // the arrowhead
              ToArrow = "standard", Stroke = null
            }
              .Bind("Fill", "Progress", (progress, obj) => {
                return (bool)progress ? "#52ce60" /* green */ : "black";
              }),
            new Panel("Auto")
              .Add(
                new Shape {  // the label background, which becomes transparent around the edges
                  Fill = new Brush("Radial", new[] { (0, "rgb(245, 245, 245)"), (0.7f, "rgb(245, 245, 245)"), (1, "rgba(245, 245, 245, 0)") }),
                  Stroke = null
                },
                new TextBlock("transition") {  // the label text
                  TextAlign = TextAlign.Center,
                  Font = new Font("Segoe UI", 9, FontWeight.Bold),
                  Margin = 4,
                  Editable = true  // enable in-place editing
                }
                  .Bind(new Binding("Text").MakeTwoWay())
              )
          );

      // read in the JSON data from the "mySavedModel" element
      LoadModel();
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
    public string Id { get; set; }
    public string Loc { get; set; }
  }

  public class LinkData : Model.LinkData {
    public List<Point> Points { get; set; }
    public double? Curviness { get; set; }
    public bool? Progress { get; set; }
  }

}
