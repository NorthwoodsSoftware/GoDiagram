using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools.Extensions;
using Northwoods.Go.Layouts;
using System.ComponentModel;

namespace WinFormsExtensionControls.Rescaling {
  [ToolboxItem(false)]
  public partial class RescalingControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    public RescalingControl() {
      InitializeComponent();

      Setup();
      goWebBrowser1.Html = @"
        <p>
      Selecting a node will show a rescaling handle that when dragged will modify the node's <a>GraphObject.Scale</a> property.
        </p>
        <p>
      Just as the <a>ResizingTool</a> changes the <a>GraphObject.DesiredSize</a> of an object,
      and just as the <a>RotatingTool</a> changes the <a>GraphObject.Angle</a> of an object,
      the <a>RescalingTool</a> changes the <a>GraphObject.Scale</a> of an object.
        </p>
";
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;
      myDiagram.Layout = new TreeLayout();
      myDiagram.UndoManager.IsEnabled = true;

      myDiagram.ToolManager.MouseDownTools.Add(new RescalingTool());

      myDiagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance) {
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

      myDiagram.Model = new Model {
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
  }
  public class LinkData : Model.LinkData { }
}


