/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools.Extensions;

namespace Demo.Extensions.GeometryReshaping {
  public partial class GeometryReshaping : DemoControl {
    private Diagram _Diagram;

    public GeometryReshaping() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      desc1.MdText = DescriptionReader.Read("Extensions.GeometryReshaping.md");
    }

    private void Setup() {
      _Diagram.UndoManager.IsEnabled = true; // enable undo and redo
      _Diagram.ToolManager.MouseDownTools.Insert(3,
        new GeometryReshapingTool { IsResegmenting = true }); // enable geometry reshaping

      // node template
      _Diagram.NodeTemplate =
        new Node {
            Resizable = true, ResizeElementName = "SHAPE",
            Reshapable = true,  // GeometryReshapingTool assumes nonexistent Part.reshapeObjectName would be "SHAPE"
            Rotatable = true, RotationSpot = Spot.Center
          }
          .Add(
            new Shape { Name = "SHAPE", Fill = "lightgray", StrokeWidth = 1.5 }
              .Bind(new Binding("GeometryString", "Geo").MakeTwoWay())
          );

      _Diagram.Model = new Model {
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
