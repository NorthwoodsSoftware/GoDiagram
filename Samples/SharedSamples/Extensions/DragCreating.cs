/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools.Extensions;

namespace Demo.Extensions.DragCreating {
  public partial class DragCreating : DemoControl {
    private Diagram _Diagram;

    public DragCreating() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      _InitCheckBox();

      desc1.MdText = DescriptionReader.Read("Extensions.DragCreating.md");
    }

    // link enabled checkbox to tool IsEnabled property
    private void _EnableTool(bool enable) {
      if (_Diagram == null) return;
      var tool = _Diagram.ToolManager.FindTool("DragCreating");
      if (tool != null && enable) tool.IsEnabled = true;
      else tool.IsEnabled = false;
    }

    private void Setup() {
      _Diagram.NodeTemplate =
        new Node(PanelType.Auto) {
          MinSize = new Size(60, 20),
          Resizable = true
        }.Bind(
          new Binding("DesiredSize", "Size", Northwoods.Go.Size.Parse).MakeTwoWay(Northwoods.Go.Size.Stringify),
          new Binding("Position", "Pos", Point.Parse).MakeTwoWay(Point.Stringify),
          new Binding("LayerName", "IsSelected", (s, t) => { return ((bool)s) ? "Foreground" : ""; }).OfElement())
        .Add(
          new Shape {
            Figure = "Rectangle"
          }.Bind(
            new Binding("Fill", "Color")
          ),
          new TextBlock {
            Margin = new Margin(2)
          }.Bind(
            new Binding("Text", "Color")
          )
        );
      _Diagram.UndoManager.IsEnabled = true;
      _Diagram.Add(
        new Part {
          LayerName = "Grid",
          Location = new Point(0, 0)
        }.Add(
          new TextBlock {
            Text = "Mouse-down and then drag in the background\nto add a Node there with the drawn size.",
            Stroke = new Brush("brown")
          }
        )
      );

      _Diagram.ToolManager.MouseMoveTools.Insert(2,
        new CustomDragCreatingTool {
          IsEnabled = true,
          Delay = TimeSpan.FromMilliseconds(0),
          Box = new Part {
            LayerName = "Tool"
          }.Add(
            new Shape {
              Name = "SHAPE",
              Fill = (Brush)null,
              Stroke = new Brush("cyan"),
              StrokeWidth = 2
            }
          ),
          ArchetypeNodeData = new NodeData {
            Color = "white"
          }
        }
      );

      _Diagram.Model = new Model();

    }
  }

  // define the model types
  public class Model : Model<NodeData, string, object> { }
  public class NodeData : Model.NodeData {
    public string Color { get; set; }
    public string Size { get; set; }
    public string Pos { get; set; }
  }

  // define a custom drag creating tool
  public class CustomDragCreatingTool : DragCreatingTool {
    public override Part InsertPart(Rect bounds) {
      if (ArchetypeNodeData == null) return null;
      // use a different color each time
      (ArchetypeNodeData as NodeData).Color = Brush.RandomColor();
      // call the base method for normal behavior
      return base.InsertPart(bounds);
    }
  }
}
