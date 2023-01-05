/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools.Extensions;

namespace Demo.Extensions.GuidedDragging {
  public partial class GuidedDragging : DemoControl {
    private Diagram _Diagram;

    public GuidedDragging() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      desc1.MdText = DescriptionReader.Read("Extensions.GuidedDragging.md");
    }

    private void Setup() {
      // defined in GuidedDraggingTool.cs
      _Diagram.ToolManager.DraggingTool = new GuidedDraggingTool {
        HorizontalGuidelineColor = "blue",
        VerticalGuidelineColor = "blue",
        CenterGuidelineColor = "green",
        GuidelineWidth = 1
      };
      _Diagram.UndoManager.IsEnabled = true;  // enable undo & redo

      // define a simple Node template
      _Diagram.NodeTemplate =
        new Node(PanelType.Auto).Add(  // the Shape will go around the TextBlock
          new Shape {
            Figure = "RoundedRectangle",
            StrokeWidth = 0
          }.Bind("Fill", "Color"),  // Shape.Fill is bound to Node.Data.Color
          new TextBlock {
            Margin = 8  // some room around the text
          }.Bind("Text", "Key")  // TextBlock.Text is bound to Node.Data.Key
        );

      // but use the default Link template, by not setting Diagram.LinkTemplate

      // Create the Diagram's Model:
      _Diagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "Alpha", Color = "lightblue" },
          new NodeData { Key = "Beta", Color = "orange" },
          new NodeData { Key = "Gamma", Color = "lightgreen" },
          new NodeData { Key = "Delta", Color = "pink" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = "Alpha", To = "Beta" },
          new LinkData { From = "Alpha", To = "Gamma" },
          new LinkData { From = "Beta", To = "Beta" },
          new LinkData { From = "Gamma", To = "Delta" },
          new LinkData { From = "Delta", To = "Alpha" }
        }
      };
    }
  }

  // define the model types
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public Brush Color { get; set; }
  }
  public class LinkData : Model.LinkData { }
}
