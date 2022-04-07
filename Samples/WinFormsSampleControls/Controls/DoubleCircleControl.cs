using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.DoubleCircle {
  [ToolboxItem(false)]
  public partial class DoubleCircleControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public DoubleCircleControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"

   <p>
  This sample displays a diagram of two sets of nodes intended to be arranged in different circles.
  </p>
  <p>
  Unlike many <b>GoDiagram</b> apps, there is no <a>Diagram.Layout</a> assigned.
  Layouts are performed explicitly in code -- a separate <a>CircularLayout</a> for each subset of nodes.
  The code will actually work with a variable number of layers/circles, not just two.
  </p>
";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.InitialAutoScale = AutoScale.Uniform;
      myDiagram.UndoManager.IsEnabled = true;

      // node template
      myDiagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance) {
          LocationSpot = Spot.Center
        }.Add(
          new Shape {
            Figure = "Circle",
            Fill = "gray",
            Stroke = "#D8D8D8"
          }.Bind(
            new Binding("Fill", "Color")
          ),
          // define the node's text
          new TextBlock {
            Margin = 5,
            Font = new Font("Segoe UI", 11, FontWeight.Bold)
          }.Bind(
            new Binding("Text", "Key")
          )
        );

      // model data
      var data = new List<NodeData>();

      // if you want a node in the center, set its Layer = 0
      for (var i = 0; i < 10; i++) {
        data.Add(new NodeData {
          Layer = 1,
          Color = Brush.RandomColor()
        });
      }
      for (var i = 0; i < 25; i++) {
        data.Add(new NodeData {
          Layer = 2,
          Color = Brush.RandomColor()
        });
      }
      myDiagram.Model = new Model {
        NodeDataSource = data
      };

      DoubleCircleLayout(myDiagram);
    }

    private void DoubleCircleLayout(Diagram diagram) {
      diagram.StartTransaction("Multi Circle Layout");

      var radius = 100;
      var layer = 1;
      List<Node> nodes = NodesByLayer(diagram, layer);
      while (nodes.Count > 0) {
        var layout = new CircularLayout {
          Radius = radius
        };
        layout.DoLayout(nodes);
        // recenter at (0, 0)
        var cntr = layout.ActualCenter;
        diagram.MoveParts(nodes, new Point(-cntr.X, -cntr.Y), true);
        // next layout uses a larger radius
        radius += 100;
        layer++;
        nodes = NodesByLayer(diagram, layer);
      }

      foreach (var n in NodesByLayer(diagram, 0)) {
        n.Location = new Point(0, 0);
      }

      diagram.CommitTransaction("Multi Circle Layout");
    }

    List<Node> NodesByLayer(Diagram diagram, int layer) {
      var set = new List<Node>();
      foreach (var part in diagram.Nodes) {
        if (part is Node && (part.Data as NodeData).Layer == layer) set.Add(part);
      }
      return set;
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public int Layer { get; set; }
    public string Color { get; set; }
  }

  public class LinkData : Model.LinkData {

  }

}
