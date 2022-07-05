/* Copyright 1998-2022 by Northwoods Software Corporation. */

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

      Setup();
      goWebBrowser1.Html = @"
        <p>
          The GeometryReshapingTool class allows for a Shape's Geometry to be modified by the user via the dragging of tool handles.
          Reshape handles are drawn as Adornments at each point in the geometry.
          It is defined in its own file, as <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/GeometryReshaping/GeometryReshapingTool.cs"">GeometryReshapingTool.cs</a>.
        </p>
        <p>
          Usage can also be seen in the <a href=""FreehandDrawing"">Freehand Drawing</a>
          and <a href=""PolygonDrawing"">Polygon Drawing</a> samples.
        </p>
      ";
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;
      myDiagram.UndoManager.IsEnabled = true; // enable undo and redo
      myDiagram.ToolManager.MouseDownTools.Insert(3,
        new GeometryReshapingTool { IsResegmenting = true }); // enable geometry reshaping

      // node template
      myDiagram.NodeTemplate =
        new Node {
            Resizable = true, ResizeElementName = "SHAPE",
            Reshapable = true,  // GeometryReshapingTool assumes nonexistent Part.reshapeObjectName would be "SHAPE"
            Rotatable = true, RotationSpot = Spot.Center
          }
          .Add(
            new Shape { Name = "SHAPE", Fill = "lightgray", StrokeWidth = 1.5 }
              .Bind(new Binding("GeometryString", "Geo").MakeTwoWay())
          );

      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData {
            Key = "1",
            Geo = "F M20 0 40 20 20 40 0 20z"
          },
          new NodeData {
            Key = "2",
            Geo = "F M0 145 L75 8 C100 20 120 40 131 87 C160 70 180 50 195 0 L249 133z"
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
