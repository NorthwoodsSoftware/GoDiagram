/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace Demo.Samples.Minimal {
  public partial class Minimal : DemoControl {
    private Diagram _Diagram;

    public Minimal() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      desc1.MdText = DescriptionReader.Read("Samples.Minimal.md");
    }

    private void Setup() {
      _Diagram.UndoManager.IsEnabled = true;  // enable undo & redo

      // define a simple Node template
      _Diagram.NodeTemplate =
        new Node("Auto")  // the Shape will go around the TextBlock
          .Add(
            new Shape("RoundedRectangle") {
                StrokeWidth = 0,  // no border
                Fill = "white"  // default fill is white
              }
              // Shape.Fill is bound to Node.Data.Color
              .Bind("Fill", "Color"),
            new TextBlock {
                Margin = 8, // some room around the text
                Font = new Font("Segoe UI", 14, Northwoods.Go.FontWeight.Bold),
                Stroke = "#333"
              }
              // TextBlock.Text is bound to Node.Data.Key
              .Bind("Text", "Key")
          );

      // but use the default Link template, by not setting Diagram.LinkTemplate

      // create the model data that will be represented by Nodes and Links
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

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Color { get; set; }
  }
  public class LinkData : Model.LinkData { }
}
