﻿using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;
using Northwoods.Go.Tools.Extensions;

namespace WinFormsExtensionControls.LinkLabelDragging {
  [ToolboxItem(false)]
  public partial class LinkLabelDraggingControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    
    public LinkLabelDraggingControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      saveLoadModel1.SaveClick += (e, obj) => SaveModel();
      saveLoadModel1.LoadClick += (e, obj) => LoadModel();

      goWebBrowser1.Html = @"
   <p>
    This sample is a modification of the <a href="".. / samples / stateChart.html"">State Chart</a> sample
    that makes use of the LinkLabelDraggingTool that is defined in its own file, as <a href = ""LinkLabelDraggingTool.js"">LinkLabelDraggingTool.js</a>.
   </p>
 
   <p>
    Note that after dragging a link label you can move a node connected by that link and the label maintains the same position relative to the link route.
    That relative position is specified by the <a>GraphObject.segmentOffset</a> property.
    This sample also saves any changes to that property by means of a TwoWay <a>Binding</a>.
   </p>

   <p>
    See also the similar <a href = ""LinkLabelOnPathDragging.html"" target = ""_blank"">Link Label On Path Dragging sample</a>,
    where the label is constrained to remain on the path of the link.
   </p>
";

      saveLoadModel1.ModelJson = @"{ 
  ""NodeKeyProperty"": ""Id"",
  ""NodeDataSource"": [
    { ""Id"": ""0"", ""Loc"": ""120 120"", ""Text"": ""Initial"" },
    { ""Id"": ""1"", ""Loc"": ""330 120"", ""Text"": ""First Down"" },
    { ""Id"": ""2"", ""Loc"": ""226 376"", ""Text"": ""First Up"" },
    { ""Id"": ""3"", ""Loc"": ""60 276"", ""Text"": ""Second Down"" },
    { ""Id"": ""4"", ""Loc"": ""226 226"", ""Text"": ""Wait"" }
  ],
  ""LinkDataSource"": [
    { ""From"": ""0"", ""To"": ""0"", ""Text"": ""up or timer"", ""Curviness"": -20 },
    { ""From"": ""0"", ""To"": ""1"", ""Text"": ""down"", ""Curviness"": 20 },
    { ""From"": ""1"", ""To"": ""0"", ""Text"": ""up (moved)\nPOST"", ""Curviness"": 20 },
    { ""From"": ""1"", ""To"": ""1"", ""Text"": ""down"", ""Curviness"": -20 },
    { ""From"": ""1"", ""To"": ""2"", ""Text"": ""up (no move)"" },
    { ""From"": ""1"", ""To"": ""4"", ""Text"": ""timer"" },
    { ""From"": ""2"", ""To"": ""0"", ""Text"": ""timer\nPOST"" },
    { ""From"": ""2"", ""To"": ""3"", ""Text"": ""down"" },
    { ""From"": ""3"", ""To"": ""0"", ""Text"": ""up\nPOST\n(dblclick\nif no move)"" },
    { ""From"": ""3"", ""To"": ""3"", ""Text"": ""down or timer"", ""Curviness"": 20 },
    { ""From"": ""4"", ""To"": ""0"", ""Text"": ""up\nPOST"" },
    { ""From"": ""4"", ""To"": ""4"", ""Text"": ""down"" }
  ]
}";

    }

    private void Setup() {

      myDiagram = diagramControl1.Diagram;

      // have mouse wheel events zoom in and out instead of scroll up and down
      myDiagram.ToolManager.MouseWheelBehavior = WheelMode.Zoom;
      // support double-click in background creating a new node
      myDiagram.ToolManager.ClickCreatingTool.ArchetypeNodeData = new NodeData {
        Text = "new node"
      };
      // enable undo and redo
      myDiagram.UndoManager.IsEnabled = true;

      // install the LinkLabelDraggingTool as a "mouse move" tool
      myDiagram.ToolManager.MouseMoveTools.Insert(0, new LinkLabelDraggingTool());

      // node template
      myDiagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance)
          .Bind(new Binding("Location", "Loc", Point.Parse).MakeTwoWay(Point.Stringify))
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
            Font = "Helvetica, 11px, style=bold", // bold arial??
            Editable = true, // editing text automatically updates model data
            Margin = new Margin(1)
          }.Bind(
            new Binding("Text", "Text").MakeTwoWay()
          )
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
      myDiagram.NodeTemplate.SelectionAdornmentTemplate =
        new Adornment(PanelLayoutSpot.Instance).Add(
          new Panel(PanelLayoutAuto.Instance).Add(
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
        { 1, "rgba(0, 240, 240, 0)" }
      };
      var paint = new RadialGradientPaint(colorStops);


      // link template
      myDiagram.LinkTemplate =
        new Link {
          Curve = LinkCurve.Bezier,
          Adjusting = LinkAdjusting.Stretch,
          Reshapable = true
        }
        .Bind(new Binding("Points").MakeTwoWay(),
          new Binding("Curviness"))
        .Add(new Shape { // link shape
          StrokeWidth = 1.5
        },
          new Shape { // arrowhead
            ToArrow = "standard",
            Stroke = (Brush)null
          },
          new Panel(PanelLayoutAuto.Instance) {
            Cursor = "move" // visual hint that the user can do something with link label
          }.Add(
            new Shape { // label background which becomes transparent around the edges
              Fill = new Brush(paint),
              Stroke = (Brush)null
            },
            new TextBlock {
              Text = "transition",
              TextAlign = TextAlign.Center,
              Font = "Helvetica, 10px, style=bold", // arial??
              Stroke = "black",
              Margin = new Margin(4),
              Editable = true // editing the text automatically updates the model data
            }.Bind(
              new Binding("Text").MakeTwoWay()
            ))
          .Bind(new Binding("SegmentOffset", "SegmentOffset", Point.Parse).MakeTwoWay(Point.Stringify))
        );

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

}