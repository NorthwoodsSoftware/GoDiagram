/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;
using Northwoods.Go.Tools.Extensions;

namespace Demo.Extensions.Rescaling {
  public partial class Rescaling : DemoControl {
    private Diagram _Diagram;

    public Rescaling() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      desc1.MdText = DescriptionReader.Read("Extensions.Rescaling.md");
    }

    private void Setup() {
      _Diagram.Layout = new TreeLayout();
      _Diagram.UndoManager.IsEnabled = true;

      _Diagram.ToolManager.MouseDownTools.Add(new RescalingTool());

      _Diagram.NodeTemplate =
        new Node(PanelType.Auto) {
          LocationSpot = Spot.Center
        }.Bind(new Binding("Scale").MakeTwoWay())
        .Add(new Shape {
          Figure = "RoundedRectangle",
          StrokeWidth = 0
        }.Bind("Fill", "Color"),
        new TextBlock {
          Margin = 8
        }.Bind("Text")
      );

      _Diagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = 1, Text = "Alpha", Color = "lightblue" },
          new NodeData { Key = 2, Text = "Beta", Color = "orange" },
          new NodeData { Key = 3, Text = "Gamma", Color = "lightgreen" },
          new NodeData { Key = 4, Text = "Delta", Color = "pink" },
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = 1, To = 2 },
          new LinkData { From = 1, To = 3 },
          new LinkData { From = 3, To = 4 },

        }
      };
    }
  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Color { get; set; }
    public string Loc { get; set; }
    public string Size { get; set; }
    public double Scale { get; set; } = 1;
  }
  public class LinkData : Model.LinkData { }
}
