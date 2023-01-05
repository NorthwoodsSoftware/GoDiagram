/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools.Extensions;

namespace Demo.Extensions.SectorReshaping {
  public partial class SectorReshaping : DemoControl {
    private Diagram _Diagram;

    public SectorReshaping() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      desc1.MdText = DescriptionReader.Read("Extensions.SectorReshaping.md");
    }

    private void Setup() {
      _Diagram.UndoManager.IsEnabled = true;

      // install the SectorReshapingTool as a mouse-down tool
      _Diagram.ToolManager.MouseDownTools.Add(new SectorReshapingTool());

      // geometry converter for the node's "LAMP" Shape
      static Geometry MakeSector(object dataIn, object objIn) {
        var data = dataIn as NodeData;
        var radius = SectorReshapingTool.GetRadius(data);
        var angle = SectorReshapingTool.GetAngle(data);
        var sweep = SectorReshapingTool.GetSweep(data);
        var start = new Point(radius, 0).Rotate(angle);
        var geo = new Geometry()
          .Add(new PathFigure(radius + start.X, radius + start.Y)  // start point
            .Add(new PathSegment(SegmentType.Arc,
              angle, sweep,  // angles
              radius, radius,  // center
              radius, radius))  // radius
            .Add(new PathSegment(SegmentType.Line, radius, radius).Close()))
          .Add(new PathFigure(0, 0))  // make sure the Geometry always includes the whole circle
          .Add(new PathFigure(2 * radius, 2 * radius));  // even if only a small sector is "lit"
        return geo;
      }

      // node template
      _Diagram.NodeTemplate =
        new Node(PanelType.Spot) {
          LocationSpot = Spot.Center, LocationElementName = "LAMP",
          SelectionElementName = "LAMP", SelectionAdorned = false
        }
        .Bind(
          new Binding("Location", "Loc", Point.Parse).MakeTwoWay(Point.Stringify),
          // selecting a Node brings it forward in the z-order
          new Binding("LayerName", "IsSelected", (s, obj) => {
            return ((bool)s ? "Foreground" : "");
          }).OfElement())
        .Add(
          new Panel(PanelType.Spot) {
            Name = "LAMP"
          }.Add(
            new Shape { // arc
              Fill = "yellow", Stroke = "lightgray", StrokeWidth = 0.5
            }.Bind(
              new Binding("Geometry", "", MakeSector)
            ),
            new Shape {
              Figure = "Circle",
              Name = "SHAPE", Width = 6, Height = 6
            }
          ),
          new TextBlock {
            Alignment = new Spot(0.5, 0.5, 0, 3), AlignmentFocus = Spot.Top,
            Stroke = "blue", Background = "rgba(255,255,255,0.3)"
          }.Bind(
            new Binding("Alignment", "Spot", Spot.Parse).MakeTwoWay(Spot.Stringify),
            new Binding("Text", "Name")
          )
        );

      // model
      _Diagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Name = "Alpha", Radius = 70, Sweep = 120 },
          new NodeData { Name = "Beta", Radius = 70, Sweep = 80, Angle = 200 }
        }
      };

      // show tool handles
      _Diagram.CommandHandler.SelectAll();
    }
  }

  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Name { get; set; }
    public string Loc { get; set; }
    public double Radius { get; set; }
    public double Sweep { get; set; }
    public double Angle { get; set; }
    public string Spot { get; set; }
  }
  public class LinkData : Model.LinkData { }
}
