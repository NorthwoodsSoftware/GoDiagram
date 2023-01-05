/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools.Extensions;

namespace Demo.Extensions.RotateMultiple {
  public partial class RotateMultiple : DemoControl {
    private Diagram _Diagram;

    public RotateMultiple() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      desc1.MdText = DescriptionReader.Read("Extensions.RotateMultiple.md");
    }

    private void Setup() {
      _Diagram.ToolManager.RotatingTool = new RotateMultipleTool();
      _Diagram.UndoManager.IsEnabled = true;

      _Diagram.NodeTemplate =
        new Node("Auto") { LocationSpot = Spot.Center, Rotatable = true }
          .Bind("Location", "Loc", Point.Parse, Point.Stringify)
          .Bind(new Binding("Angle").MakeTwoWay())
          .Add(
            new Shape("RoundedRectangle") { StrokeWidth = 0 }
              .Bind("Fill", "Color"),
            new TextBlock { Margin = 8 }
              .Bind("Text", "Key")
          );

      // use default link template by not setting _Diagram.LinkTemplate

      _Diagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "Alpha", Color = "lightblue" },
          new NodeData { Key = "Beta", Color = "orange" },
          new NodeData { Key = "Gamma", Color = "lightgreen" },
          new NodeData { Key = "Delta", Color = "pink" },
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = "Alpha", To = "Beta" },
          new LinkData { From = "Alpha", To = "Gamma" },
          new LinkData { From = "Beta", To = "Beta" },
          new LinkData { From = "Gamma", To = "Delta" },
          new LinkData { From = "Delta", To = "Alpha" },
        }
      };
    }
  }

  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Color { get; set; }
    public string Loc { get; set; }
    public double? Angle { get; set; }
  }
  public class LinkData : Model.LinkData { }
}
