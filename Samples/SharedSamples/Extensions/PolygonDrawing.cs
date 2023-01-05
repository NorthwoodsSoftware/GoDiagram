/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools.Extensions;

namespace Demo.Extensions.PolygonDrawing {
  public partial class PolygonDrawing : DemoControl {
    private Diagram _Diagram;

    public PolygonDrawing() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      selectBtn.Click += (e, obj) => _SelectShape();
      drawPolygonBtn.Click += (e, obj) => _DrawPolygon();
      drawPolylineBtn.Click += (e, obj) => _DrawPolyline();
      finishBtn.Click += (e, obj) => _Finish(true);
      cancelBtn.Click += (e, obj) => _Finish(false);
      undoPtBtn.Click += (e, obj) => _Undo();
      _InitCheckBoxes();

      modelJson1.SaveClick = SaveModel;
      modelJson1.LoadClick = LoadModel;
      modelJson1.JsonText = @"{
        ""NodeDataSource"": [ { ""Loc"":""183 148"", ""Geo"":""F M0 145 L75 2 L131 87 L195 0 L249 143z"", ""Key"":""-1"", ""Stroke"": ""black"", ""StrokeWidth"": 1} ],
        ""SharedData"": { ""Position"":""0 0"" }
}";

      Setup();

      desc1.MdText = DescriptionReader.Read("Extensions.PolygonDrawing.md");
    }

    private void Setup() {
      _Diagram.ToolManager.MouseDownTools.Insert(3, new GeometryReshapingTool { IsResegmenting = true });

      _Diagram.NodeTemplate =
        new Node {
            SelectionElementName = "SHAPE",
            SelectionAdornmentTemplate = // custom selection adornment: blue rectangle
              new Adornment("Auto")
                .Add(
                  new Shape { Stroke = "dodgerblue", Fill = null },
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
      _Diagram.ToolManager.MouseDownTools.Insert(0, tool);

      LoadModel(); // load a simple diagram from textarea
    }

    private void Mode(bool draw, bool polygon) {
      // assume PolygonDrawingTool is the first tool in the mouse-down-tools list
      var tool = _Diagram.ToolManager.MouseDownTools.ElementAt(0) as PolygonDrawingTool;
      tool.IsEnabled = draw;
      tool.IsPolygon = polygon;
      (tool.ArchetypePartData as NodeData).Fill = (polygon ? "yellow" : "transparent");
      tool.TemporaryShape.Fill = (polygon ? "yellow" : "transparent");
      if (draw) _Diagram.CurrentTool = tool;
    }

    private void SaveModel() {
      if (_Diagram == null) return;
      (_Diagram.Model.SharedData as SharedData).Position = Point.Stringify(_Diagram.Position);
      modelJson1.JsonText = _Diagram.Model.ToJson();
    }

    private void LoadModel() {
      if (_Diagram == null) return;
      var model = Model.FromJson<Model>(modelJson1.JsonText);
      var pos = model.SharedData.Position;
      _Diagram.InitialPosition = Point.Parse(pos);
      _Diagram.Model = model;
      _Diagram.Model.UndoManager.IsEnabled = true;
      _SelectShape();
    }

    // called after checkboxes change Diagram.Allow...
    private void UpdateAllAdornments() {
      foreach (var p in _Diagram.Selection) {
        p.UpdateAdornments();
      }
    }

    private void _SelectShape() {
      Mode(false, false);
    }

    private void _DrawPolygon() {
      _Finish(false);
      Mode(true, true);
    }

    private void _DrawPolyline() {
      _Finish(false);
      Mode(true, false);
    }

    private void _Finish(bool commit) {
      if (!(_Diagram.CurrentTool is PolygonDrawingTool tool)) return;
      if (commit && tool != null) {
        var lastInput = _Diagram.LastInput;
        if (lastInput.EventType == "mousedown" || lastInput.EventType == "mouseup" || lastInput.EventType == "pointerdown" || lastInput.EventType == "pointerup") {
          tool.RemoveLastPoint();
        }
        tool.FinishShape();
      } else {
        tool.DoCancel();
      }
    }

    // this command removes the last clicked point from the temporary Shape
    private void _Undo() {
      if (_Diagram.CurrentTool is PolygonDrawingTool tool) {
        var lastInput = _Diagram.LastInput;
        if (lastInput.EventType == "mousedown" || lastInput.EventType == "mouseup" || lastInput.EventType == "pointerdown" || lastInput.EventType == "pointerup") {
          tool.RemoveLastPoint();
        }
        tool.Undo();
      }
    }

    private void _ToggleResizing() {
      _Diagram.Commit(d => {
        d.AllowResize = !d.AllowResize;
        UpdateAllAdornments();
      });
    }

    private void _ToggleReshaping() {
      _Diagram.Commit(d => {
        d.AllowReshape = !d.AllowReshape;
        UpdateAllAdornments();
      });
    }

    private void _ToggleResegmenting() {
      _Diagram.Commit(d => {
        var tool = d.ToolManager.FindTool("GeometryReshaping") as GeometryReshapingTool;
        tool.IsResegmenting = !tool.IsResegmenting;
        UpdateAllAdornments();
      });
    }

    private void _ToggleRotating() {
      _Diagram.Commit(d => {
        d.AllowRotate = !d.AllowRotate;
        UpdateAllAdornments();
      });
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
}
