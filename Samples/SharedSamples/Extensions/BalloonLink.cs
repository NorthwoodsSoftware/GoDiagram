/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace Demo.Extensions.BalloonLink {
  public partial class BalloonLink : DemoControl {
    private Diagram myDiagram;

    public BalloonLink() {
      InitializeComponent();

      Setup();

      desc1.MdText = DescriptionReader.Read("Extensions.BalloonLink.md");
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      myDiagram.UndoManager.IsEnabled = true;

      // define a simple Node template
      myDiagram.NodeTemplate =
        new Node("Auto") { Margin = 2 }
          .Add(  // the Shape will go around the TextBlock
            new Shape("Rectangle") { StrokeWidth = 0 }
              .Bind("Fill", "Color"),  // Shape.Fill is bound to Node.Data.Color
            new TextBlock { Margin = 8 } // some room around the text
              .Bind("Text", "Key")  // TextBlock.Text is bound to Node.Data.Key
          );

      // use BalloonLink extension as default link template
      myDiagram.LinkTemplate =
        new Northwoods.Go.Extensions.BalloonLink()
          .Add(new Shape { Stroke = "limegreen", StrokeWidth = 3, Fill = "limegreen" });

      myDiagram.Model = new Model() {
        NodeDataSource = new List<NodeData>() {
          new NodeData{ Key = "Alpha", Color = "lightblue" },
          new NodeData{ Key = "Beta", Color = "orange" }
        },
        LinkDataSource = new List<LinkData>() {
          new LinkData{ From = "Alpha", To = "Beta" }
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
