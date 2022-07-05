/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.Minimal {
  [ToolboxItem(false)]
  public partial class MinimalControl : System.Windows.Forms.UserControl {
    private Diagram _Diagram;

    public MinimalControl() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;
      Setup();
      goWebBrowser1.Html = @"
        <p>
          This isn't a truly <i>minimal</i> demonstration of <b>GoDiagram</b>,
          because we do specify a custom Node template, but it's pretty simple.
          The whole source for the sample is shown below if you click on the link.
        </p>
        <p>
          This sample sets the <a>Diagram.NodeTemplate</a>, with a <a>Node</a> template that data binds both the text string and the shape's fill color.
          For an overview of building your own templates and model data, see the <a href=""learn/index.html"">Getting Started tutorial.</a>
        </p>
        <p>
          Using the mouse and common keyboard commands, you can pan, select, move, copy, delete, and undo/redo.
          On touch devices, use your finger to act as the mouse, and hold your finger stationary to bring up a context menu.
          The default context menu supports most of the standard commands that
          are enabled at that time for the selected object.
        </p>
        <p>
          For a more elaborate and capable sample, see the <a href=""Basic"">Basic</a> sample.
        </p>
      ";
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
                Font = new Font("Segoe UI", 14, FontWeight.Bold),
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
