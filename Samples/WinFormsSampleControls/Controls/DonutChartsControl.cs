using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.DonutCharts {
  [ToolboxItem(false)]
  public partial class DonutChartsControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    
    public DonutChartsControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      btnChangeSelectedValue.Click += (e, obj) => ChangeValue();

      goWebBrowser1.Html = @"

   <p>
      Each node contains a Position Panel containing two Shape elements that get Geometry values
      that show a data value as an annular bar in a circle.  One can also specify the colors of
      the two bars -- the bar showing the value and the rest of the circle.
    </p>
";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // node template
      myDiagram.NodeTemplate =
      new Node(PanelLayoutSpot.Instance).Add(
        new Panel().Add(
          new Shape {
            Figure = "Circle",
            Width = 100,
            Height = 100,
            StrokeWidth = 0,
            Fill = "transparent"
          },
          new Shape {
            Fill = "transparent",
            Stroke = "cyan",
            StrokeWidth = 8
          }.Bind(
            new Binding("Geometry", "Value", MakeArc),
            new Binding("Stroke", "Color1")
          ),
          new Shape { Fill = "transparent", Stroke = "gray", StrokeWidth = 8 }.Bind(
            new Binding("Geometry", "Value", MakeArcRest),
            new Binding("Stroke", "Color2")
          )
        ),
        new TextBlock().Bind(
          new Binding("Text")
        )
      );

      // These arcs assume the angle starts at 270 degrees, at the top of the circle.
      // They all assume the circle is 100x100 in size.
      Geometry MakeArc(object sweepIn, object _) {
        var sweep = sweepIn as double? ?? double.NaN;
        return new Geometry()
            .Add(new PathFigure(50, 0)
                .Add(new PathSegment(SegmentType.Arc, -90, sweep, 50, 50, 50, 50)));
      }

      Geometry MakeArcRest(object sweepIn, object _) {
        var sweep = sweepIn as double? ?? double.NaN;
        var p = new Point(50, 0).Rotate(-90 + sweep).Offset(50, 50);
        return new Geometry()
            .Add(new PathFigure(p.X, p.Y)
                .Add(new PathSegment(SegmentType.Arc, sweep - 90, 360 - sweep, 50, 50, 50, 50)));
      }

      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = 1, Text = "Alpha", Value = 0 },
          new NodeData { Key = 2, Text = "Beta", Value = 90 },
          new NodeData { Key = 3, Text = "Gamma", Value = 135 },
          new NodeData { Key = 4, Text = "Delta", Value = 330, Color1 = "red", Color2 = "green" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = 1, To = 2 },
          new LinkData { From = 1, To = 3 },
          new LinkData { From = 3, To = 4 },
          new LinkData { From = 4, To = 1 }
        }
      };
    }

    private void ChangeValue() {
      if (myDiagram.Selection.Count == 0) return;
      var node = myDiagram.Selection.First();
      if (node is Node) {
        myDiagram.Model.Commit((m) => {
          var val = (node.Data as NodeData).Value;
          val += (new Random().NextDouble()) * 40 - 20;
          if (val < 0) val = 20;
          else if (val >= 360) val = 340;
          m.Set(node.Data, "Value", val);
        }, "changed data value");
      }
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public double? Value { get; set; }
    public string Color1 { get; set; }
    public string Color2 { get; set; }
  }

  public class LinkData : Model.LinkData { }


}
