using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.PieCharts {
  [ToolboxItem(false)]
  public partial class PieChartsControl : System.Windows.Forms.UserControl {
    private Diagram MyDiagram;
    
    public PieChartsControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      goWebBrowser1.Html = @"
        <p>
      Each node has a Position Panel whose <a>Panel.ItemList</a> is data bound to the ""Slices"" property of the node data.
      That ""Slices"" property is a list of data objects; for each item the Panel.ItemTemplate</a> produces a Shape whose
      <a>Shape.Geometry</a> is computed using a conversion function to generate a pie - shaped slice given a start angle
      and a sweep angle from the item data. Note that <a>Shape.IsGeometryPositioned</a> is true to make sure all of the Shapes are positioned
      by their computed geometries, independent of any stroke width. Each slice Panel also has a tooltip showing some information.
        </p>
";

    }

    private Geometry _MakeGeo(SliceData data, object obj) {
      // this is much more efficient than calling Builder.Make:
      return new Geometry()
        .Add(new PathFigure(50, 50) // start point
          .Add(new PathSegment(SegmentType.Arc,
            data.Start, data.Sweep, // angles
            50, 50, // center
            50, 50 // radius
            ).Close()));
    }

    private void Setup() {

      MyDiagram = diagramControl1.Diagram;

      var toolTip = Builder.Make<Adornment>("ToolTip").Add(
        new TextBlock {
          Margin = 4
        }.Bind(
          new Binding("Text", "", (_data, _) => {
            var data = _data as SliceData;
            return data.Color + ": " + data.Start +
              " to " + (data.Start + data.Sweep);
          })
        )
      );

      // The ItemTemplate of this NodeTemplate visualizes a pie chart as an array of "slices"
      // created by the _MakeGeo function above.
      MyDiagram.NodeTemplate = new Node(PanelLayoutVertical.Instance).Add(
        new Panel {
          ItemTemplate = new Panel {
            ToolTip = toolTip
          }.Add(
            new Shape {
              Fill = "lightgreen",
              IsGeometryPositioned = true
            }.Bind("Fill", "Color")
             .Bind("Geometry", "", (data, _) => _MakeGeo(data as SliceData, _))
          )
        }.Bind(
          new Binding("ItemList", "Slices")
        ),
        new TextBlock().Bind("Text")
      );

      MyDiagram.Model = new Model {
        // node data
        NodeDataSource = new List<NodeData> {
          new NodeData {
            Key = 1,
            Text = "full circle",
            Slices = new List<SliceData> {
              new SliceData { Start = -30, Sweep = 60, Color = "white" },
              new SliceData { Start = 30, Sweep = 300, Color = "red" }
            }
          },
          new NodeData {
            Key = 2,
            Text = "partial circle",
            Slices = new List<SliceData> {
              new SliceData { Start = 0, Sweep = 120, Color = "lightgreen" },
              new SliceData { Start = 120, Sweep = 70, Color = "blue" },
              new SliceData { Start = 250, Sweep = 20, Color = "yellow" }
            }
          }
        },
        LinkDataSource = new List<LinkData> {
          // link data
          new LinkData { From = 1, To = 2 }
        }
      };
    }

  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }

  public class NodeData : Model.NodeData {
    public List<SliceData> Slices { get; set; }
  }

  public class LinkData : Model.LinkData { }

  public class SliceData {
    public double Start { get; set; }
    public double Sweep { get; set; }
    public string Color { get; set; }
  }

}
