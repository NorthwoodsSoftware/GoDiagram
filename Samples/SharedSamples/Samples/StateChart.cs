/* Copyright (c) Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;

namespace Demo.Samples.StateChart; 
public partial class StateChart : DemoControl {
  private Diagram _Diagram;

  public StateChart() {
    InitializeComponent();
    _Diagram = diagramControl1.Diagram;

    modelJson1.SaveClick = SaveModel;
    modelJson1.LoadClick = LoadModel;

    desc1.MdText = DescriptionReader.Read("Samples.StateChart.md");

    modelJson1.JsonText = @"{
  ""nodeKeyProperty"": ""Id"",
  ""nodeDataSource"": [
    { ""id"": ""-1"", ""loc"": ""155 -138"", ""category"": ""Start"" },
    { ""id"": ""0"", ""loc"": ""190 15"", ""text"": ""Shopping"" },
    { ""id"": ""1"", ""loc"": ""353 32"", ""text"": ""Browse Items"" },
    { ""id"": ""2"", ""loc"": ""353 166"", ""text"": ""Search Items"" },
    { ""id"": ""3"", ""loc"": ""512 12"", ""text"": ""View Item"" },
    { ""id"": ""4"", ""loc"": ""661 17"", ""text"": ""View Cart"" },
    { ""id"": ""5"", ""loc"": ""644 171"", ""text"": ""Update Cart"" },
    { ""id"": ""6"", ""loc"": ""800 96"", ""text"": ""Checkout"" },
    { ""id"": ""-2"", ""loc"": ""757 229"", ""category"": ""End"" }
  ],
  ""linkDataSource"": [
    { ""from"": ""-1"", ""to"": ""0"", ""text"": ""Visit online store"" },
    { ""from"": ""0"", ""to"": ""1"", ""progress"": true, ""text"": ""Browse"" },
    { ""from"": ""0"", ""to"": ""2"", ""progress"": true, ""text"": ""Use search bar"" },
    { ""from"": ""1"", ""to"": ""2"", ""progress"": true, ""text"": ""Use search bar"" },
    { ""from"": ""2"", ""to"": ""3"", ""progress"": true, ""text"": ""Click item"" },
    { ""from"": ""2"", ""to"": ""2"", ""text"": ""Another search"", ""curviness"": 20 },
    { ""from"": ""1"", ""to"": ""3"", ""progress"": true, ""text"": ""Click item"" },
    { ""from"": ""3"", ""to"": ""0"", ""text"": ""Not interested"", ""curviness"": -100 },
    { ""from"": ""3"", ""to"": ""4"", ""progress"": true, ""text"": ""Add to cart"" },
    { ""from"": ""4"", ""to"": ""0"", ""text"": ""More shopping"", ""curviness"": -150 },
    { ""from"": ""4"", ""to"": ""5"", ""text"": ""Update needed"", ""curviness"": -50 },
    { ""from"": ""5"", ""to"": ""4"", ""text"": ""Update made"" },
    { ""from"": ""4"", ""to"": ""6"", ""progress"": true, ""text"": ""Proceed"" },
    { ""from"": ""6"", ""to"": ""5"", ""text"": ""Update needed"" },
    { ""from"": ""6"", ""to"": ""-2"", ""progress"": true, ""text"": ""Purchase made"" }
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

    _Diagram.AnimationManager.InitialAnimationStyle = AnimationStyle.None;
    _Diagram.InitialAnimationStarting += (s, e) => {
      var animation = (e.Subject as AnimationManager).DefaultAnimation;
      animation.Easing = Animation.EaseOutExpo;
      animation.Duration = TimeSpan.FromMilliseconds(900);
      animation.Add(e.Diagram, "Scale", 0.1, 1);
      animation.Add(e.Diagram, "Opacity", 0, 1);
    };
    // mouse wheel zooms instead of scrolling
    _Diagram.ToolManager.MouseWheelBehavior = WheelMode.Zoom;
    // support double-click in background to create a new node
    _Diagram.ToolManager.ClickCreatingTool.ArchetypeNodeData = new NodeData { Text = "new node" };
    _Diagram.UndoManager.IsEnabled = true;
    _Diagram.PositionComputation = (diagram, pt) => {
      return new Point(Math.Floor(pt.X), Math.Floor(pt.Y));
    };

    // node template
    _Diagram.NodeTemplate =
      new Node("Auto") {
        LocationSpot = Spot.Top,
        IsShadowed = true, ShadowBlur = 1,
        ShadowOffset = new Point(0, 1),
        ShadowColor = "rgba(0, 0, 0, .14)"
      }
        .BindTwoWay("Location", "Loc", Point.Parse, Point.Stringify)
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
            Font = new Font("Segoe UI", 11, Northwoods.Go.FontWeight.Bold), Margin = 7, Stroke = "rgba(0, 0, 0, .87)",
            Editable = true // editing the text automatically updates the model data
          }
            .BindTwoWay("Text")
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
    _Diagram.NodeTemplate.SelectionAdornmentTemplate =
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
    _Diagram.NodeTemplateMap.Add("Start",
      new Node("Spot") { DesiredSize = new Size(75, 75) }
        .BindTwoWay("Location", "Loc", Point.Parse, Point.Stringify)
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
            Font = new Font("Segoe UI", 16, Northwoods.Go.FontWeight.Bold),
            Stroke = "whitesmoke"
          }
        )
    );

    _Diagram.NodeTemplateMap.Add("End",
      new Node("Spot") { DesiredSize = new Size(75, 75) }
        .BindTwoWay("Location", "Loc", Point.Parse, Point.Stringify)
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
            Font = new Font("Segoe UI", 16, Northwoods.Go.FontWeight.Bold),
            Stroke = "whitesmoke"
          }
        )
    );

    // link template
    _Diagram.LinkTemplate =
      new Link {
        Curve = LinkCurve.Bezier,
        Adjusting = LinkAdjusting.Stretch,
        Reshapable = true, RelinkableFrom = true, RelinkableTo = true,
        ToShortLength = 3
      }
        .BindTwoWay("Points")
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
                Font = new Font("Segoe UI", 9, Northwoods.Go.FontWeight.Bold),
                Margin = 4,
                Editable = true  // enable in-place editing
              }
                .BindTwoWay("Text")
            )
        );

    // read in the JSON data from the "mySavedModel" element
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
