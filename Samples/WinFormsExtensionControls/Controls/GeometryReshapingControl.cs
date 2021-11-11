using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using System.ComponentModel;
using Northwoods.Go.Tools.Extensions;

namespace WinFormsExtensionControls.GeometryReshaping {
  [ToolboxItem(false)]
  public partial class GeometryReshapingControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    public GeometryReshapingControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;
      goWebBrowser1.Html = @"
          <p>
    The GeometryReshapingTool class allows for a Shape's Geometry to be modified by the user via the dragging of tool handles.
    Reshape handles are drawn as Adornments at each point in the geometry.
    It is defined in its own file, as <a href=""GeometryReshapingTool.js"">GeometryReshapingTool.js</a>.
        </p>
        <p>
    Usage can also be seen in the <a href = ""FreehandDrawing.html""> Freehand Drawing </a> and <a href = ""PolygonDrawing.html""> Polygon Drawing </a> samples.             
        </p>       
";
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;
      myDiagram.UndoManager.IsEnabled = true; // enable undo and redo
      myDiagram.ToolManager.MouseDownTools.Insert(3, new GeometryReshapingTool()); // enable geometry reshaping

      // node template
      myDiagram.NodeTemplate =
        new Node {
          Reshapable = true
        }.Add(
          new Shape {
            Name = "SHAPE",
            Fill = "lightgray",
            StrokeWidth = 1.5
          }.Bind(
            new Binding("GeometryString", "Geo").MakeTwoWay()
          )
        );

      // model
      myDiagram.Model = new Model() {
        NodeDataSource = new List<NodeData>() {
          new NodeData {
            Geo = "F M0 145 L75 2 L131 87 L195 0 L249 143z",
            Key = "-1"
          }
        }
      };
    }
  }
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Geo { get; set; }
  }
  public class LinkData : Model.LinkData { }
}
