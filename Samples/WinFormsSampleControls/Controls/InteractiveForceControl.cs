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
using Northwoods.Go.Tools;

namespace WinFormsSampleControls.InteractiveForce {
  [ToolboxItem(false)]
  public partial class InteractiveForceControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public InteractiveForceControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"

<html>
<head>
</head>
<body>

 <p>
    As you drag a node around, the custom <a>ForceDirectedLayout</a> operates continuously, causing other nodes to be pushed
    aside or pulled along.
  </p>
  <p>
    The graph is exactly the same as the <a href=""ConceptMap"">Concept Map</a> sample.
    But the node template is different, and the layout is different.
  </p>

</body>
</html>";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.InitialAutoScale = AutoScale.Uniform;
      myDiagram.ContentAlignment = Spot.Center;
      myDiagram.Layout = new ContinuousForceDirectedLayout {
        DefaultSpringLength = 30,
        DefaultElectricalCharge = 100
      };
      // do an extra layout at the end of a move
      myDiagram.SelectionMoved += (_, e) => {
        e.Diagram.Layout.InvalidateLayout();
      };
      myDiagram.ToolManager.DraggingTool = new InteractiveForceDirectedDraggingTool();

      // define each Node's appearance
      myDiagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance).Add(  // the whole node panel
                                                 // define the node's outer shape, which will surround the TextBlock
          new Shape {
            Figure = "Circle",
            Fill = "CornflowerBlue",
            Stroke = "black",
            Spot1 = new Spot(0, 0, 5, 5),
            Spot2 = new Spot(1, 1, -5, -5)
          },
          new TextBlock {
            Font = new Font("Segoe UI", 10, FontWeight.Bold),
            TextAlign = TextAlign.Center,
            MaxSize = new Northwoods.Go.Size(100, double.NaN)
          }.Bind(
            new Binding("Text", "Text")
          )
        );

      // replace the default Link template in the linkTemplateMap
      myDiagram.LinkTemplate =
        new Link().Add(  // the whole link panel
          new Shape  // the link shape
            { Stroke = "black" },
          new Shape  // the arrowhead
            { ToArrow = "standard", Stroke = (Brush)null },
          new Panel(PanelLayoutAuto.Instance).Add(
            new Shape  // the label background, which becomes transparent around the edges
              {
              Fill = new Brush(new RadialGradientPaint(new Dictionary<float, string> {
                  { 0.01f, "rgb(240, 240, 240)" },
                  { 0.3f, "rgb(240, 240, 240)" },
                  { 1, "rgba(240, 240, 240, 0)" }
                }, 0, 1
              )),
              Stroke = (Brush)null
            },
            new TextBlock  // the label text
              {
              TextAlign = TextAlign.Center,
              Font = new Font("Segoe UI", 10),
              Stroke = "#555555",
              Margin = 4
            }.Bind(
              new Binding("Text", "Text"))
          )
        );

      // model data
      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = 1, Text = "Concept Maps" },
          new NodeData { Key = 2, Text = "Organized Knowledge" },
          new NodeData { Key = 3, Text = "Context Dependent" },
          new NodeData { Key = 4, Text = "Concepts" },
          new NodeData { Key = 5, Text = "Propositions" },
          new NodeData { Key = 6, Text = "Associated Feelings or Affect" },
          new NodeData { Key = 7, Text = "Perceived Regularities" },
          new NodeData { Key = 8, Text = "Labeled" },
          new NodeData { Key = 9, Text = "Hierarchically Structured" },
          new NodeData { Key = 10, Text = "Effective Teaching" },
          new NodeData { Key = 11, Text = "Crosslinks" },
          new NodeData { Key = 12, Text = "Effective Learning" },
          new NodeData { Key = 13, Text = "Events (Happenings)" },
          new NodeData { Key = 14, Text = "Objects (Things)" },
          new NodeData { Key = 15, Text = "Symbols" },
          new NodeData { Key = 16, Text = "Words" },
          new NodeData { Key = 17, Text = "Creativity" },
          new NodeData { Key = 18, Text = "Interrelationships" },
          new NodeData { Key = 19, Text = "Infants" },
          new NodeData { Key = 20, Text = "Different Map Segments" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = 1, To = 2, Text = "represent" },
          new LinkData { From = 2, To = 3, Text = "is" },
          new LinkData { From = 2, To = 4, Text = "is" },
          new LinkData { From = 2, To = 5, Text = "is" },
          new LinkData { From = 2, To = 6, Text = "includes" },
          new LinkData { From = 2, To = 10, Text = "necessary\nfor" },
          new LinkData { From = 2, To = 12, Text = "necessary\nfor" },
          new LinkData { From = 4, To = 5, Text = "combine\nto form" },
          new LinkData { From = 4, To = 6, Text = "include" },
          new LinkData { From = 4, To = 7, Text = "are" },
          new LinkData { From = 4, To = 8, Text = "are" },
          new LinkData { From = 4, To = 9, Text = "are" },
          new LinkData { From = 5, To = 9, Text = "are" },
          new LinkData { From = 5, To = 11, Text = "may be" },
          new LinkData { From = 7, To = 13, Text = "in" },
          new LinkData { From = 7, To = 14, Text = "in" },
          new LinkData { From = 7, To = 19, Text = "begin\nwith" },
          new LinkData { From = 8, To = 15, Text = "with" },
          new LinkData { From = 8, To = 16, Text = "with" },
          new LinkData { From = 9, To = 17, Text = "aids" },
          new LinkData { From = 11, To = 18, Text = "show" },
          new LinkData { From = 12, To = 19, Text = "begins\nwith" },
          new LinkData { From = 17, To = 18, Text = "needed\nto see" },
          new LinkData { From = 18, To = 20, Text = "between" }
        }
      };
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData { }
  public class LinkData : Model.LinkData { }

  // extend dragging tool to invalidate layout
  public class InteractiveForceDirectedDraggingTool : DraggingTool {
    public override void DoMouseMove() {
      base.DoMouseMove();
      if (IsActive) Diagram.Layout.InvalidateLayout();
    }
  }

  // This variation on ForceDirectedLayout does not move any selected Nodes
  // but does move all other nodes (vertexes)
  public class ContinuousForceDirectedLayout : ForceDirectedLayout {
    private bool _IsObserving;
    public ContinuousForceDirectedLayout() : base() {
      _IsObserving = false;
    }

    public override bool IsFixed(ForceDirectedVertex v) {
      return v.Node.IsSelected;
    }

    // optimization: reuse the ForceDirectedNetwork rather than re-create it each time
    public override void DoLayout(IEnumerable<Part> coll) {
      if (!_IsObserving) {
        _IsObserving = true;
        // cacheing the network means we need to recreate it if nodes or links have been added or removed or relinked,
        // so we need to track structural model changes to discard the saved network.
        var lay = this;
        Diagram.Model.Changed += (_, e) => {
          // modelChanges include a few cases that we don't actually care about, such as
          // "nodeCategory" or "linkToPortId", but we'll go ahead and recreate the network anyway.
          // Also clear the network when replacing the model.
          if (e.ModelChange != ModelChangeType.None ||
            (e.Change == ChangeType.Transaction && e.PropertyName == "StartingFirstTransaction")) {
            lay.Network = null;
          }
        };
      }
      var net = Network;
      if (net == null) {  // the first time, just create the network as normal
        Network = net = MakeNetwork(coll);
      } else {  // but on reuse we need to update the LayoutVertex.bounds for selected nodes
        foreach (var n in Diagram.Nodes) {
          var v = net.FindVertex(n);
          if (v != null) v.Bounds = n.ActualBounds;
        }
      }
      // now perform the normal layout
      base.DoLayout(coll);
      // doLayout normally discards the LayoutNetwork by setting Layout.network to null;
      // here we remember it for next time
      Network = net;
    }
  }

}
