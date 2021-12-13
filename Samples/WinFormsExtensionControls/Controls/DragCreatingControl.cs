using System;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools.Extensions;

namespace WinFormsExtensionControls.DragCreating {
  [ToolboxItem(false)]
  public partial class DragCreatingControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public DragCreatingControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      checkBxEnable.CheckStateChanged += (e, obj) => ToolEnabled();

      goWebBrowser1.Html = @"
  <p>
    This sample demonstrates the DragCreatingTool, which replaces the standard DragSelectingTool.
    It is defined in its own file, as <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/DragCreating/DragCreatingTool.cs"">DragCreatingTool.cs</a>.
  </p>
  <p>
    Press in the background and then drag to show the area to be occupied by the new node.
    The mouse-up event will add a copy of the DragCreatingTool.ArchetypeNodeData object, causing a new node to be created.
    The tool will assign its <a>GraphObject.Position</a> and <a>GraphObject.DesiredSize</a>.
  </p>
";

    }

    private bool checkedValue = true;
    public bool CheckedValue {
      get {
        return checkedValue;
      }
      set {
        if (checkedValue != value) {
          checkedValue = value;
          ToolEnabled();
        }
      }
    }

    // link enabled checkbox to tool IsEnabled property
    private void ToolEnabled() {
      if (myDiagram == null) return;
      var tool = myDiagram.ToolManager.FindTool("DragCreating");
      if (tool != null && checkBxEnable.CheckState == System.Windows.Forms.CheckState.Checked) tool.IsEnabled = true;
      else tool.IsEnabled = false;
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      myDiagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance) {
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
      myDiagram.UndoManager.IsEnabled = true;
      myDiagram.Add(
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

      myDiagram.ToolManager.MouseMoveTools.Insert(2,
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

      myDiagram.Model = new Model();

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
  class CustomDragCreatingTool : DragCreatingTool {
    public override Part InsertPart(Rect bounds) {
      if (ArchetypeNodeData == null) return null;
      // use a different color each time
      (ArchetypeNodeData as NodeData).Color = Brush.RandomColor();
      // call the base method for normal behavior
      return base.InsertPart(bounds);
    }
  }

}
