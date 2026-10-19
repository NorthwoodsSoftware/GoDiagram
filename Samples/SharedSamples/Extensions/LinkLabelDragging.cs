/* Copyright (c) Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;
using Northwoods.Go.Tools.Extensions;

namespace Demo.Extensions.LinkLabelDragging; 
public partial class LinkLabelDragging : DemoControl {
  private Diagram _Diagram;

  public LinkLabelDragging() {
    InitializeComponent();
    _Diagram = diagramControl1.Diagram;

    desc1.MdText = DescriptionReader.Read("Extensions.LinkLabelDragging.md");

    modelJson1.SaveClick = SaveModel;
    modelJson1.LoadClick = LoadModel;
    modelJson1.JsonText = @"{
  ""nodeKeyProperty"": ""Id"",
  ""nodeDataSource"": [
    { ""id"": ""0"", ""loc"": ""120 120"", ""text"": ""Initial"" },
    { ""id"": ""1"", ""loc"": ""330 120"", ""text"": ""First Down"" },
    { ""id"": ""2"", ""loc"": ""226 376"", ""text"": ""First Up"" },
    { ""id"": ""3"", ""loc"": ""60 276"", ""text"": ""Second Down"" },
    { ""id"": ""4"", ""loc"": ""226 226"", ""text"": ""Wait"" }
  ],
  ""linkDataSource"": [
    { ""from"": ""0"", ""to"": ""0"", ""text"": ""up or timer"", ""curviness"": -20 },
    { ""from"": ""0"", ""to"": ""1"", ""text"": ""down"", ""curviness"": 20 },
    { ""from"": ""1"", ""to"": ""0"", ""text"": ""up (moved)\nPOST"", ""curviness"": 20 },
    { ""from"": ""1"", ""to"": ""1"", ""text"": ""down"", ""curviness"": -20 },
    { ""from"": ""1"", ""to"": ""2"", ""text"": ""up (no move)"" },
    { ""from"": ""1"", ""to"": ""4"", ""text"": ""timer"" },
    { ""from"": ""2"", ""to"": ""0"", ""text"": ""timer\nPOST"" },
    { ""from"": ""2"", ""to"": ""3"", ""text"": ""down"" },
    { ""from"": ""3"", ""to"": ""0"", ""text"": ""up\nPOST\n(dblclick\nif no move)"" },
    { ""from"": ""3"", ""to"": ""3"", ""text"": ""down or timer"", ""curviness"": 20 },
    { ""from"": ""4"", ""to"": ""0"", ""text"": ""up\nPOST"" },
    { ""from"": ""4"", ""to"": ""4"", ""text"": ""down"" }
  ]
}";

    Setup();
  }

  private void Setup() {
    // have mouse wheel events zoom in and out instead of scroll up and down
    _Diagram.ToolManager.MouseWheelBehavior = WheelMode.Zoom;
    // support double-click in background creating a new node
    _Diagram.ToolManager.ClickCreatingTool.ArchetypeNodeData = new NodeData {
      Text = "new node"
    };
    // enable undo and redo
    _Diagram.UndoManager.IsEnabled = true;

    // install the LinkLabelDraggingTool as a "mouse move" tool
    _Diagram.ToolManager.MouseMoveTools.Insert(0, new LinkLabelDraggingTool());

    // node template
    _Diagram.NodeTemplate =
      new Node(PanelType.Auto)
        .BindTwoWay("Location", "Loc", Point.Parse, Point.Stringify)
        .Add(new Shape {
          Figure = "RoundedRectangle",
          Parameter1 = 20, // corner has a large radius
          Fill = "orange",
          Stroke = "black",
          PortId = "",
          FromLinkable = true,
          FromLinkableSelfNode = true,
          FromLinkableDuplicates = true,
          ToLinkable = true,
          ToLinkableSelfNode = true,
          ToLinkableDuplicates = true,
          Cursor = "pointer"
        },
        new TextBlock {
          Font = new Font("Arial", 11, Northwoods.Go.FontWeight.Bold),
          Editable = true, // editing text automatically updates model data
          Margin = new Margin(1)
        }.BindTwoWay("Text")
      );

    // event for button to create "next" node
    void AddNodeAndLink(InputEvent e, GraphObject obj) {
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
    }

    // make button for selection adornment template
    var button = Builder.Make<Panel>("Button").Add(
      new Shape {
        Figure = "PlusLine",
        DesiredSize = new Size(6, 6)
      }
    );
    button.Click = AddNodeAndLink;
    button.Alignment = Spot.TopRight;

    // unlike the normal selection adornment, this one includes a Button
    _Diagram.NodeTemplate.SelectionAdornmentTemplate =
      new Adornment(PanelType.Spot).Add(
        new Panel(PanelType.Auto).Add(
          new Shape {
            Fill = (Brush)null,
            Stroke = "blue",
            StrokeWidth = 2
          },
          new Placeholder() // this represents the selected node
        ),
        // the button to create a "next" node, at the top-right corner
        button
      );

    // define paint
    var colorStops = new Dictionary<float, string> {
      { 0, "rgb(240, 240, 240)" },
      { 0.3f, "rgb(240, 240, 240)" },
      { 1, "rgba(240, 240, 240, 0)" }
    };
    var paint = new RadialGradientPaint(colorStops);


    // link template
    _Diagram.LinkTemplate =
      new Link {
        Curve = LinkCurve.Bezier,
        Adjusting = LinkAdjusting.Stretch,
        Reshapable = true
      }
      .Bind(
        new Binding("Points").MakeTwoWay(),
        new Binding("Curviness"))
      .Add(new Shape { // link shape
        StrokeWidth = 1.5
      },
        new Shape { // arrowhead
          ToArrow = "standard",
          Stroke = (Brush)null
        },
        new Panel(PanelType.Auto) {
          Cursor = "move" // visual hint that the user can do something with link label
        }.Add(
          new Shape { // label background which becomes transparent around the edges
            Fill = new Brush(paint),
            Stroke = (Brush)null
          },
          new TextBlock {
            Text = "transition",
            TextAlign = TextAlign.Center,
            Font = new Font("Arial", 10, Northwoods.Go.FontWeight.Bold),
            Stroke = "black",
            Margin = new Margin(4),
            Editable = true // editing the text automatically updates the model data
          }.BindTwoWay("Text"))
        .BindTwoWay("SegmentOffset", Point.Parse, Point.Stringify)
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

public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
public class NodeData : Model.NodeData {
  public string Id { get; set; }
  public string Loc { get; set; }
}
public class LinkData : Model.LinkData {
  public List<Point> Points { get; set; }
  public double? Curviness { get; set; }
  public string SegmentOffset { get; set; }
}
