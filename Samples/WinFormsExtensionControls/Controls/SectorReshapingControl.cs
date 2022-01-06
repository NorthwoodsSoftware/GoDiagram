using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using System.ComponentModel;
using Northwoods.Go.Tools.Extensions;

namespace WinFormsExtensionControls.SectorReshaping {
  [ToolboxItem(false)]
  public partial class SectorReshapingControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    public SectorReshapingControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;
      goWebBrowser1.Html = @"
          <p>
          Two of the handles permit changing the angles of the sector; one handle permits changing the radius of the sector.
          </p>
          <p>
          Note that the <a>Geometry</a> returned by <code>MakeSector</code> always returns a Geometry that
          occupies the area that would be occupied by a full circle.  That Geometry-creating function also
          depends on three data properties, <code>Radius</code>, <code>Angle</code>, and <code>Sweep</code>.
          </p>
";
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.UndoManager.IsEnabled = true;

      // install the SectorReshapingTool as a mouse-down tool
      myDiagram.ToolManager.MouseDownTools.Add(new SectorReshapingTool());

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
      myDiagram.NodeTemplate =
        new Node(PanelLayoutSpot.Instance) {
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
          new Panel(PanelLayoutSpot.Instance) {
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
      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Name = "Alpha", Radius = 70, Sweep = 120 },
          new NodeData { Name = "Beta", Radius = 70, Sweep = 80, Angle = 200 }
        }
      };

      // show tool handles
      myDiagram.CommandHandler.SelectAll();
    }
  }

  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Name { get; set; }
    public double Radius { get; set; }
    public double Sweep { get; set; }
    public double Angle { get; set; }
    public string Spot { get; set; }
  }
  public class LinkData : Model.LinkData {

  }
}
