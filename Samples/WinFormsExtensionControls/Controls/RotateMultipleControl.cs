using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using System.ComponentModel;
using Northwoods.Go.Tools.Extensions;

namespace WinFormsExtensionControls.RotateMultiple {
  [ToolboxItem(false)]
  public partial class RotateMultipleControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    public RotateMultipleControl() {
      InitializeComponent();

      Setup();
      goWebBrowser1.Html = @"
           <p>
          This sample demonstrates a custom <a>RotatingTool</a> which allows the user to rotate many selected objects at once.
          It is defined in its own file, as <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/RotateMultiple/RotateMultipleTool.cs"">RotateMultipleTool.cs</a>.
          </p>
          <p>
          Hold down the control key in order to rotate each selected node individually, rather than all of them collectively.
          </p>
";
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.ToolManager.RotatingTool = new RotateMultipleTool();
      myDiagram.UndoManager.IsEnabled = true;

      // simple node template
      myDiagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance) {
          Rotatable = true
        }
        .Bind(
          new Binding("Location", "Loc", Point.Parse).MakeTwoWay(Point.Stringify),
          // save the modified size in the node data
          new Binding("Angle").MakeTwoWay())
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
      myDiagram.Model = new Model {
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
    public double? Angle { get; set; }
  }
  public class LinkData : Model.LinkData { }
}
