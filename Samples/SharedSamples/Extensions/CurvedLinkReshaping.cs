/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;
using Northwoods.Go.Tools.Extensions;

namespace Demo.Extensions.CurvedLinkReshaping {
  public partial class CurvedLinkReshaping : DemoControl {
    private Diagram _Diagram;

    public CurvedLinkReshaping() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      modelJson1.SaveClick = SaveModel;
      modelJson1.LoadClick = LoadModel;

      desc1.MdText = DescriptionReader.Read("Extensions.CurvedLinkReshaping.md");

      modelJson1.JsonText = @"{
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

      Setup();
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

    private void Setup() {
      _Diagram.ToolManager.LinkReshapingTool = new CurvedLinkReshapingTool();
      _Diagram.ToolManager.MouseWheelBehavior = WheelMode.Zoom;
      _Diagram.ToolManager.ClickCreatingTool.ArchetypeNodeData = new NodeData { Text = "new node" };
      _Diagram.UndoManager.IsEnabled = true;

      _Diagram.NodeTemplate =
        new Node(PanelType.Auto)
          .Bind("Location", "Loc", Point.Parse, Point.Stringify)
          .Add(new Shape {
            Parameter1 = 20,
            Figure = "RoundedRectangle",
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
            Editable = true,
            Margin = 0
          }.Bind(
            new Binding("Text", "Text").MakeTwoWay()
          )
        );

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

      _Diagram.NodeSelectionAdornmentTemplate =
        new Adornment(PanelType.Spot).Add(
          new Panel(PanelType.Auto).Add(
            new Shape {
              Fill = (Brush)null,
              Stroke = "blue",
              StrokeWidth = 2
            },
            new Placeholder()
          ),
          button
        );

      // define paint
      var colorStops = new Dictionary<float, string> {
        { 0, "rgb(240, 240, 240)" },
        { 0.3f, "rgb(240, 240, 240)" },
        { 1, "rgba(240, 240, 240, 0)" }
      };
      var paint = new RadialGradientPaint(colorStops);

      // replace default link template
      _Diagram.LinkTemplate =
        new Link { Curve = LinkCurve.Bezier, Reshapable = true }
          .Bind(new Binding("Curviness", "Curviness").MakeTwoWay())
          .Add(new Shape { // link shape
            StrokeWidth = 1.5
          },
          new Shape { // arrowhead
            ToArrow = "standard",
            Stroke = (Brush)null
          },
          new Panel(PanelType.Auto).Add(
            new Shape {
              Fill = new Brush(paint),
              Stroke = (Brush)null
            },
            new TextBlock() {
              Text = "transition",
              TextAlign = TextAlign.Center,
              Font = new Font("Arial", 10),
              Stroke = "black",
              Margin = 4,
              Editable = true
            }.Bind(new Binding("Text", "Text").MakeTwoWay())
          )
        );

      LoadModel();
    }
  }

  // define the model types
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Id { get; set; }
    public string Loc { get; set; }
  }
  public class LinkData : Model.LinkData {
    public double? Curviness { get; set; }
  }
}
