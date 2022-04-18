/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;


namespace WinFormsExtensionControls.BalloonLink {
  [ToolboxItem(false)]
  public partial class BalloonLinkControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public BalloonLinkControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"
   <p>
    A <b>BalloonLink</b> is a custom <a>Link</a> that draws a ""balloon"" shape around the Link.FromNode.
    It will create a triangular shape with the base at the FromNode and the other point at the ToNode.
    It is defined in its own file, as <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/GraphObjects/BalloonLink/BalloonLink.cs"">BalloonLink.cs</a>.
   </p>

   <p>
     Usage can also be seen in the <a href=""Comments"">Comments</a> sample.
   </p>
";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      myDiagram.UndoManager.IsEnabled = true;

      // define a simple Node template
      myDiagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance).Add(  // the Shape will go around the TextBlock
          new Shape {
            Figure = "Rectangle",
            StrokeWidth = 0
          }.Bind("Fill", "Color"),  // Shape.Fill is bound to Node.Data.Color
          new TextBlock {
            Margin = 8  // some room around the text
          }.Bind("Text", "Key")  // TextBlock.Text is bound to Node.Data.Key
        );

      // use BalloonLink extension as default link template
      myDiagram.LinkTemplate = new Northwoods.Go.Extensions.BalloonLink().Add(
        new Shape {
          Stroke = "limegreen", StrokeWidth = 3, Fill = "limegreen"
        }
      );

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
