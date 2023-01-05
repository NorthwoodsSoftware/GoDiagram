/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools.Extensions;

namespace Demo.Extensions.ResizeMultiple {
  public partial class ResizeMultiple : DemoControl {
    private Diagram _Diagram;

    public ResizeMultiple() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();
      desc1.MdText = DescriptionReader.Read("Extensions.ResizeMultiple.md");
    }

    private void Setup() {
      _Diagram.ToolManager.ResizingTool = new ResizeMultipleTool();
      _Diagram.UndoManager.IsEnabled = true;

      // simple node template
      _Diagram.NodeTemplate =
        new Node(PanelType.Auto) {
          Resizable = true
        }.Bind(
          new Binding("Location", "Loc", Point.Parse).MakeTwoWay(Point.Stringify),
          // save the modified size in the node data
          new Binding("DesiredSize", "Size", Northwoods.Go.Size.Parse).MakeTwoWay(Northwoods.Go.Size.Stringify))
        .Add(
          new Shape {
            Figure = "RoundedRectangle",
            StrokeWidth = 0
          }.Bind(
            // Shape.Fill is bound to node data Color
            new Binding("Fill", "Color")
          ),
          new TextBlock {
            Margin = 8 // some room around the text
          }.Bind(
            new Binding("Text", "Key")
          )
        );

      // use default link template by not setting myDiagram.LinkTemplate

      // model data
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
    public string Size { get; set; }
  }
  public class LinkData : Model.LinkData { }
}
