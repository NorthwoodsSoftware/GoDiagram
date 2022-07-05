/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools.Extensions;

namespace WinFormsExtensionControls.PolygonDrawing {
  [ToolboxItem(false)]
  public partial class PolygonDrawingControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    public PolygonDrawingControl() {
      InitializeComponent();

      modelJson1.SaveClick += (e, obj) => SaveModel();
      modelJson1.LoadClick += (e, obj) => LoadModel();

      select.Click += (e, obj) => SelectShape();
      drawPolygon.Click += (e, obj) => DrawPolygon();
      drawPolyline.Click += (e, obj) => DrawPolyline();
      finishDrawing.Click += (e, obj) => Finish(true);
      cancelDrawing.Click += (e, obj) => Finish(false);
      undoLastPoint.Click += (e, obj) => Undo();

      goWebBrowser1.Html = @"
          <p>
    This sample demonstrates the PolygonDrawingTool, a custom <a>Tool</a> added to the Diagram's MouseDownTools.
    It is defined in its own file, as <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/PolygonDrawing/PolygonDrawingTool.cs"">PolygonDrawingTool.cs</a>.
    It also demonstrates the GeometryReshapingTool, another custom tool,
    defined in <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/GeometryReshaping/GeometryReshapingTool.cs"">GeometryReshapingTool.cs</a>.
          </p>

          <p>
    These extensions serve as examples of features that can be added to GoDiagram by writing new classes.
    With the PolygonDrawingTool, a new mode is supported that allows the user to draw custom shapes.
    With the GeometryReshapingTool, users can change the geometry (i.e.the ""shape"") of a <a>Shape</a>s in a selected <a>Node</a>.
         </p>

          <p>
    Click a ""Draw"" button and then click in the diagram to place a new point in a polygon or polyline shape.
    Right-click, double-click, or Enter to finish.  Press <b>Escape</b> to cancel, or <b>Z</b> to remove the last point.
    Click the ""Select"" button to switch back to the normal selection behavior, so that you can select, resize, and rotate the shapes.
    The checkboxes control whether you can resize, reshape, and /or rotate selected shapes.
         </p>

";
      modelJson1.JsonText = @"
        {
          ""NodeDataSource"": [ { ""Loc"":""183 148"", ""Geo"":""F M0 145 L75 2 L131 87 L195 0 L249 143z"", ""Key"":""-1"", ""Stroke"": ""black"", ""StrokeWidth"": 1} ],
          ""SharedData"": { ""Position"":""0 0"" }
        }";

      Setup();
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.ToolManager.MouseDownTools.Insert(3, new GeometryReshapingTool { IsResegmenting = true });

      // node template map "PolygonDrawing"
      myDiagram.NodeTemplate =
        new Node {
            SelectionElementName = "SHAPE",
            SelectionAdornmentTemplate = // custom selection adornment: a blue rectangle
              new Adornment("Auto")
                .Add(
                  new Shape { Stroke = "dodgerblue", Fill = (Brush)null },
                  new Placeholder { Margin = -1 }
                ),
            Resizable = true, ResizeElementName = "SHAPE",
            Rotatable = true, RotationSpot = Spot.Center,
            Reshapable = true
          }
          .Bind("Location", "Loc", Point.Parse, Point.Stringify)
          .Bind(new Binding("Angle").MakeTwoWay())
          .Add(
            new Shape { Name = "SHAPE", Fill = "lightgray", StrokeWidth = 1.5 }
              .Bind(
                new Binding("DesiredSize", "Size", Northwoods.Go.Size.Parse).MakeTwoWay(Northwoods.Go.Size.Stringify),
                new Binding("GeometryString", "Geo").MakeTwoWay(),
                new Binding("Fill"),
                new Binding("Stroke"),
                new Binding("StrokeWidth")
              )
          );

      // create a polygon drawing tool, defined in PolygonDrawingTool.cs
      var tool = new PolygonDrawingTool {
        // provide default model data
        ArchetypePartData = new NodeData { Fill = "yellow", Stroke = "blue", StrokeWidth = 3 },
        IsPolygon = true,
        IsEnabled = false
      };
      // install as first mouse-down tool
      myDiagram.ToolManager.MouseDownTools.Insert(0, tool);

      LoadModel(); // load a simple diagram from textarea
    }

    private void Mode(bool draw, bool polygon) {
      // assume PolygonDrawingTool is the first tool in the mouse-down-tools list
      var tool = myDiagram.ToolManager.MouseDownTools.ElementAt(0) as PolygonDrawingTool;
      tool.IsEnabled = draw;
      tool.IsPolygon = polygon;
      (tool.ArchetypePartData as NodeData).Fill = (polygon ? "yellow" : "transparent");
      tool.TemporaryShape.Fill = (polygon ? "yellow" : "transparent");
      if (draw) myDiagram.CurrentTool = tool;
    }

    private void Finish(bool commit) {
      if (!(myDiagram.CurrentTool is PolygonDrawingTool tool)) return;
      if (commit && tool != null) {
        var lastInput = myDiagram.LastInput;
        if (lastInput.EventType == "mousedown" || lastInput.EventType == "mouseup" || lastInput.EventType == "pointerdown" || lastInput.EventType == "pointerup") {
          tool.RemoveLastPoint();
        }
        tool.FinishShape();
      } else {
        tool.DoCancel();
      }
    }

    // this command removes the last clicked point from the temporary Shape
    private void Undo() {
      if (myDiagram.CurrentTool is PolygonDrawingTool tool) {
        var lastInput = myDiagram.LastInput;
        if (lastInput.EventType == "mousedown" || lastInput.EventType == "mouseup" || lastInput.EventType == "pointerdown" || lastInput.EventType == "pointerup") {
          tool.RemoveLastPoint();
        }
        tool.Undo();
      }
    }

    private void SaveModel() {
      if (myDiagram == null) return;
      (myDiagram.Model.SharedData as SharedData).Position = Point.Stringify(myDiagram.Position);
      modelJson1.JsonText = myDiagram.Model.ToJson();
    }
    private void LoadModel() {
      if (myDiagram == null) return;
      myDiagram.Model = Model.FromJson<Model>(modelJson1.JsonText);
      myDiagram.Model.UndoManager.IsEnabled = true;
      var pos = (myDiagram.Model.SharedData as SharedData).Position;
      myDiagram.InitialPosition = Point.Parse(pos);
      SelectShape();
    }

    private void UpdateAllAdornments() { // called after checkboxes change Diagram.allow...
      foreach (var p in myDiagram.Selection) {
        p.UpdateAdornments();
      }
    }

    private void SelectShape() {
      Mode(false, false);
    }

    private void DrawPolygon() {
      Finish(false);
      Mode(true, true);
    }

    private void DrawPolyline() {
      Finish(false);
      Mode(true, false);
    }

    private void _ToggleResizing(object sender, EventArgs e) {
      myDiagram.AllowResize = !myDiagram.AllowResize;
      UpdateAllAdornments();
    }

    private void _ToggleReshaping(object sender, EventArgs e) {
      myDiagram.AllowReshape = !myDiagram.AllowReshape;
      UpdateAllAdornments();
    }

    private void _ToggleResegmenting(object sender, EventArgs e) {
      var tool = myDiagram.ToolManager.FindTool("GeometryReshaping") as GeometryReshapingTool;
      tool.IsResegmenting = !tool.IsResegmenting;
      UpdateAllAdornments();
    }

    private void _ToggleRotating(object sender, EventArgs e) {
      myDiagram.AllowRotate = !myDiagram.AllowRotate;
      UpdateAllAdornments();
    }
  }

  public class Model : Model<NodeData, string, SharedData> { }
  public class NodeData : Model.NodeData {
    public string Loc { get; set; }
    public string Size { get; set; }
    public double Angle { get; set; }
    public string Geo { get; set; }
    public string Fill { get; set; }
    public string Stroke { get; set; }
    public double StrokeWidth { get; set; }
  }

  public class SharedData {
    public string Position { get; set; }
  }

  // define a class to store save/load data
  public class PolygonDrawingSavedData {
    public string Position { get; set; }
    public Model Model { get; set; }
  }
}
