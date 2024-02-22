/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools.Extensions;

namespace Demo.Extensions.PolylineLinking {
  public partial class PolylineLinking : DemoControl {
    private Diagram _Diagram;

    public PolylineLinking() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      modelJson1.SaveClick = SaveModel;
      modelJson1.LoadClick = LoadModel;
      modelJson1.JsonText = @"{
        ""NodeDataSource"": [
          {""Key"":1,""Text"":""Node 1"",""Fill"":""blueviolet"",""Loc"":""100 100""},
          {""Key"":2,""Text"":""Node 2"",""Fill"":""orange"",""Loc"":""400 100""}
        ],
        ""LinkDataSource"": []
}";

      Setup();

      desc1.MdText = DescriptionReader.Read("Extensions.PolylineLinking.md");
    }

    private void Setup() {
      // install custom linking tool, defined in PolylineLinkingTool.cs
      var tool = new PolylineLinkingTool();
      //tool.TemporaryLink.Routing = LinkRouting.Orthogonal; // optional, but need to keep link template in sync below
      _Diagram.ToolManager.LinkingTool = tool;
      _Diagram.UndoManager.IsEnabled = true;

      // node template
      _Diagram.NodeTemplate =
        new Node(PanelType.Spot) {
          LocationSpot = Spot.Center
        }.Bind(new Binding("Location", "Loc", Point.Parse).MakeTwoWay(Point.Stringify))
        .Add(
          new Shape {
            Width = 100,
            Height = 100,
            Fill = "lightgray",
            PortId = "",
            Cursor = "Pointer",
            FromLinkable = true,
            FromLinkableSelfNode = true,
            FromLinkableDuplicates = true, // optional
            ToLinkable = true,
            ToLinkableSelfNode = true,
            ToLinkableDuplicates = true // optional
          }.Bind(
            new Binding("Fill")
          ),
          new Shape {
            Width = 70,
            Height = 70,
            Fill = "transparent",
            Stroke = (Brush)null
          },
          new TextBlock().Bind(
            new Binding("Text")
          )
        );

      // link template
      _Diagram.LinkTemplate =
        new Link {
          Reshapable = true,
          Resegmentable = true,
          //Routing = LinkRouting.Orthogonal, // optional, but need to keep LinkingTool.TemporaryLink in sync above
          Adjusting = LinkAdjusting.Stretch
        }.Bind(new Binding("Points").MakeTwoWay())
        .Add(
          new Shape {
            StrokeWidth = 1.5
          },
          new Shape {
            ToArrow = "OpenTriangle"
          }
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

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Loc { get; set; }
    public string Fill { get; set; }
  }
  public class LinkData : Model.LinkData {
    public List<Point> Points { get; set; }
  }
}
