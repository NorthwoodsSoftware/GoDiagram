/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace Demo.Samples.HoverButtons {
  [ToolboxItem(false)]
  public partial class HoverButtonsControl : DemoControl {
    private Diagram myDiagram;

    public HoverButtonsControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"

  <p>
    This sample demonstrates buttons that appear when the user hovers over a node with the mouse.
    The advantage of using an <a>Adornment</a> is that it keeps the Node template simpler.
    That means there are less resources used to create nodes -- only that one adornment can be shown.
  </p>
  <p>
    However, using a template as the <a>Part.SelectionAdornmentTemplate</a> would allow for more
    than one set of buttons to be shown simultaneously, one set for each selected node.
  </p>
  <p>
    This technique does not work on touch devices.
  </p>
  <p>
    If you want to show such an Adornment on MouseEnter and MouseLeave, rather than on MouseHover,
    the code is given in the documentation for the <a>GraphObject.MouseEnter</a> property.
  </p>
";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.ToolManager.HoverDelay = 200; // controls how long to wait motionless (msec) before showing Adornment
      myDiagram.UndoManager.IsEnabled = true;

      // this is shown by the mouseHover event handler
      var nodeHoverAdornment =
        new Adornment(PanelType.Spot) {
          Background = "transparent",
          // hide the Adornment when the mouse leaves it
          MouseLeave = (e, obj, _) => {
            var ad = obj.Part as Adornment;
            ad.AdornedPart.RemoveAdornment("mouseHover");
          }
        }.Add(
          new Placeholder {
            Background = "transparent",  // to allow this Placeholder to be "seen" by mouse events
            IsActionable = true,  // needed because this is in a temporary Layer
            Click = (e, obj) => {
              var node = (obj.Part as Adornment).AdornedPart;
              node.Diagram.Select(node);
            }
          },
          Builder.Make<Panel>("Button").Set(
            new {
              Alignment = Spot.Left,
              AlignmentFocus = Spot.Right,
              Click = new Action<InputEvent, GraphObject>((e, obj) => {
                System.Windows.Forms.MessageBox.Show("GoDiagram.net says: Hi!");
              })
            }
          ).Add(
            new TextBlock { Text = "Hi" }
          ),
          Builder.Make<Panel>("Button").Set(
            new {
              Alignment = Spot.Right,
              AlignmentFocus = Spot.Left,
              Click = new Action<InputEvent, GraphObject>((e, obj) => {
                System.Windows.Forms.MessageBox.Show("GoDiagram.net says: Bye!");
              })
            }
          ).Add(
            new TextBlock { Text = "Bye" }
          )
        );

      // define a simple Node template
      myDiagram.NodeTemplate =
        new Node(PanelType.Auto) { // show the Adornment when a mouseHover event occurs
          MouseHover = (e, obj) => {
            var node = obj.Part;
            nodeHoverAdornment.AdornedElement = node;
            node.AddAdornment("mouseHover", nodeHoverAdornment);
          }
        }.Add(  // the Shape will go around the TextBlock
          new Shape {
            Figure = "RoundedRectangle",
            StrokeWidth = 0
          }.Bind(
            // Shape.Fill is bound to Node.Data.Color
            new Binding("Fill", "Color")
          ),
          new TextBlock {
            Margin = 8
          }.Bind(  // some room around the text
                   // TextBlock.Text is bound to Node.Data.Key
            new Binding("Text", "Key")
          )
        );

      // but use the default Link template, by not setting Diagram.LinkTemplate

      myDiagram.Model = new Model {
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
