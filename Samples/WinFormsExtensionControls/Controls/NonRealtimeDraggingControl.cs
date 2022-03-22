using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools.Extensions;

namespace WinFormsExtensionControls.NonRealtimeDragging {
  [ToolboxItem(false)]
  public partial class NonRealtimeDraggingControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    
    public NonRealtimeDraggingControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"
   <p>
    This custom <a>DraggingTool</a> class causes the user to drag around a translucent image of the Nodes and Links being moved,
    leaving the selected Parts in place, rather than actually moving those Nodes and Links in realtime.
    Only when the mouse up occurs does the move happen.
  </p>
  <p>
    This tool is defined in its own file, as <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/NonRealtimeDragging/NonRealtimeDraggingTool.cs"">NonRealtimeDraggingTool.cs</a>
  </p>
";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.ToolManager.DraggingTool = new NonRealtimeDraggingTool();
      myDiagram.UndoManager.IsEnabled = true;

      // node template
      myDiagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance) {
          LocationSpot = Spot.Center
        }.Add(
          new Shape {
            Figure = "Circle",
            Fill = "white", // default fill, if there is no data-binding
            PortId = "", // shape is the port, not the whole node
            Cursor = "pointer",
            // allow all kinds of links from and to this port
            FromLinkable = true, FromLinkableDuplicates = true, FromLinkableSelfNode = true,
            ToLinkable = true, ToLinkableDuplicates = true, ToLinkableSelfNode = true
          }.Bind(
            new Binding("Fill", "Color")
          ),
          new TextBlock {
            Font = new Font("Segoe UI", 14, FontWeight.Bold),
            Stroke = "#333",
            Margin = 6, // make some extra space for the shape around the text
            IsMultiline = false, // don't allow newlines in text
            Editable = true // allow in-place editing by user
          }.Bind(
            new Binding("Text").MakeTwoWay() // label shows the node data's text
          )
        );

      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = 1, Text = "Alpha", Color = "lightblue" },
          new NodeData { Key = 2, Text = "Beta", Color = "orange" },
          new NodeData { Key = 3, Text = "Gamma", Color = "lightgreen", Group = 5 },
          new NodeData { Key = 4, Text = "Delta", Color = "lightblue", Group = 5 },
          new NodeData { Key = 5, Text = "Epsilon", Color = "lightblue", IsGroup = true },
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = 1, To = 2, Color = "blue" },
          new LinkData { From = 2, To = 2 },
          new LinkData { From = 3, To = 4, Color = "green" },
          new LinkData { From = 3, To = 1, Color = "purple" },
        }
      };
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Color { get; set; }
  }
  public class LinkData : Model.LinkData {
    public string Color { get; set; }
  }

}
