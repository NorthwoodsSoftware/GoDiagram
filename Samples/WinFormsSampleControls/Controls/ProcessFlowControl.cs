using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Extensions;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.ProcessFlow {
  [ToolboxItem(false)]
  public partial class ProcessFlowControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public ProcessFlowControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      saveLoadModel1.SaveClick += (e, obj) => SaveModel();
      saveLoadModel1.LoadClick += (e, obj) => LoadModel();

      goWebBrowser1.Html = @"
        <p>
      A <em>process flow diagram</em> is commonly used in chemical and process engineering to indicate the general flow of plant processes and equipment.
      A simple SCADA diagram, with animation of the flow along the pipes, is implemented here.
        </p>
        <p>
      The diagram displays the background grid layer by setting <b>Grid.Visible</b> to true,
      and also allows snapping to the grid using <a>DraggingTool.IsGridSnapEnabled</a>,
      <a>ResizingTool.IsGridSnapEnabled</a>, and <a>RotatingTool.SnapAngleMultiple</a> alongside <a>RotatingTool.SnapAngleEpsilon</a>.
        </p>
        <p>
      There is also a custom animation that modifies the <a>Shape.StrokeDashOffset</a> for each link.
        </p>
";

      saveLoadModel1.ModelJson = @"{
  ""NodeDataSource"": [
    { ""Key"": ""P1"", ""Category"": ""Process"", ""Pos"": ""150 120"", ""Text"": ""Process"" },
    { ""Key"": ""P2"", ""Category"": ""Process"", ""Pos"": ""330 320"", ""Text"": ""Tank"" },
    { ""Key"": ""V1"", ""Category"": ""Valve"", ""Pos"": ""270 120"", ""Text"": ""V1"" },
    { ""Key"": ""P3"", ""Category"": ""Process"", ""Pos"": ""150 420"", ""Text"": ""Pump"" },
    { ""Key"": ""V2"", ""Category"": ""Valve"", ""Pos"": ""150 280"", ""Text"": ""VM"", ""Angle"": 270 },
    { ""Key"": ""V3"", ""Category"": ""Valve"", ""Pos"": ""270 420"", ""Text"": ""V2"", ""Angle"": 180 },
    { ""Key"": ""P4"", ""Category"": ""Process"", ""Pos"": ""450 140"", ""Text"": ""Reserve Tank"" },
    { ""Key"": ""V4"", ""Category"": ""Valve"", ""Pos"": ""390 60"", ""Text"": ""VA"" },
    { ""Key"": ""V5"", ""Category"": ""Process"", ""Pos"": ""450 260"", ""Text"": ""VB"", ""Angle"": 90  }
  ],
  ""LinkDataSource"": [
    { ""From"": ""P1"", ""To"": ""V1"" },
    { ""From"": ""P3"", ""To"": ""V2"" },
    { ""From"": ""V2"", ""To"": ""P1"" },
    { ""From"": ""P2"", ""To"": ""V3"" },
    { ""From"": ""V3"", ""To"": ""P3"" },
    { ""From"": ""V1"", ""To"": ""V4"" },
    { ""From"": ""V4"", ""To"": ""P4"" },
    { ""From"": ""V1"", ""To"": ""P2"" },
    { ""From"": ""P4"", ""To"": ""V5"" },
    { ""From"": ""V5"", ""To"": ""P2"" }
  ]
}";

    }


    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // add figures
      Figures.DefineExtraFigures();

      // diagram properties
      myDiagram.Grid.Visible = true;
      myDiagram.Grid.GridCellSize = new Size(30, 20);
      myDiagram.ToolManager.DraggingTool.IsGridSnapEnabled = true;
      myDiagram.ToolManager.ResizingTool.IsGridSnapEnabled = true;
      myDiagram.ToolManager.RotatingTool.SnapAngleMultiple = 90;
      myDiagram.ToolManager.RotatingTool.SnapAngleEpsilon = 45;
      myDiagram.UndoManager.IsEnabled = true;
      myDiagram.AnimationManager.IsEnabled = true;
      

      // node templatemap "Process"
      myDiagram.NodeTemplateMap.Add("Process",
        new Node(PanelLayoutAuto.Instance) {
          LocationSpot = new Spot(0.5, 0.5), LocationElementName = "SHAPE",
          Resizable = true, ResizeElementName = "SHAPE"
        }
          .Bind("Location", "Pos", Point.Parse, Point.Stringify)
          .Add(
            new Shape("Cylinder1") {
              StrokeWidth = 2,
              Fill = new Brush(new LinearGradientPaint(
                    new Dictionary<float, string> { { 0, "gray" }, { .5f, "white" }, { 1, "gray" } },
                    Spot.Left, Spot.Right
                  )
                ),
              MinSize = new Size(50, 50),
              PortId = "", FromSpot = Spot.AllSides, ToSpot = Spot.AllSides
            }
              .Bind("DesiredSize", "Size", Northwoods.Go.Size.Parse, Northwoods.Go.Size.Stringify),
            new TextBlock {
              Alignment = Spot.Center, TextAlign = TextAlign.Center, Margin = 5,
              Editable = true
            }
              .Bind(new Binding("Text").MakeTwoWay())
          )
        );

      // note template map "Valve"
      myDiagram.NodeTemplateMap.Add("Valve",
        new Node(PanelLayoutVertical.Instance) {
          LocationSpot = new Spot(0.5, 1, 0, -21),
          LocationElementName = "SHAPE",
          SelectionElementName = "SHAPE",
          Rotatable = true
        }
          .Bind(
            new Binding("Angle").MakeTwoWay(),
            new Binding("Location", "Pos", Point.Parse).MakeTwoWay(Point.Stringify)
          )
          .Add(
            new TextBlock {
              Alignment = Spot.Center, TextAlign = TextAlign.Center, Margin = 5, Editable = true
            }
              .Bind(
                new Binding("Text").MakeTwoWay(),
                // keep text upright when the node is upside down
                new Binding("Angle", "Angle", (a, obj) => {
                  var b = Convert.ToInt32(a as double? ?? -1d);
                  return (b == 180 ? 180 : 0);
                }).OfElement()
              ),
            new Shape {
              Name = "SHAPE",
              GeometryString = "F1 M0 0 L40 20 40 0 0 20z M20 10 L20 30 M12 30 L28 30",
              StrokeWidth = 2,
              Fill = new Brush(new LinearGradientPaint(new Dictionary<float, string> { { 0, "gray" }, { .35f, "white" }, { .7f, "gray" } })),
              PortId = "", FromSpot = new Spot(1, 0.35), ToSpot = new Spot(0, 0.35)
            }
          )
        );

      // link template
      myDiagram.LinkTemplate =
        new Link {
          Routing = LinkRouting.AvoidsNodes,
          Curve = LinkCurve.JumpGap,
          Corner = 10,
          Reshapable = true,
          ToShortLength = 7
        }
          .Bind(new Binding("Points").MakeTwoWay())
          .Add(
            // mark each Shape to get the link geometry with IsPanelMain = true
            new Shape { IsPanelMain = true, Stroke = "black", StrokeWidth = 7 },
            new Shape { IsPanelMain = true, Stroke = "gray", StrokeWidth = 5 },
            new Shape { IsPanelMain = true, Stroke = "white", StrokeWidth = 3, Name = "PIPE", StrokeDashArray = new float[] { 10, 10 } },
            new Shape { ToArrow = "Triangle", Scale = 1.3, Fill = "gray", Stroke = null }
          );

      LoadModel();

      var animation = new Animation {
        Easing = Animation.EaseLinear
      };
      foreach (var l in myDiagram.Links) {
        animation.Add((l.FindElement("PIPE") as Shape), "StrokeDashOffset", 20f, 0f);
      }
      animation.RunCount = int.MaxValue;
      animation.Start();
    }

    private void SaveModel() {
      if (myDiagram == null) return;
      saveLoadModel1.ModelJson = myDiagram.Model.ToJson();
    }

    private void LoadModel() {
      if (myDiagram == null) return;
      myDiagram.Model = Model.FromJson<Model>(saveLoadModel1.ModelJson);
      myDiagram.Model.UndoManager.IsEnabled = true;
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }

  public class NodeData : Model.NodeData {
    public string Pos { get; set; }
    public string Size { get; set; }
    public float Angle { get; set; }
  }

  public class LinkData : Model.LinkData { }

}
