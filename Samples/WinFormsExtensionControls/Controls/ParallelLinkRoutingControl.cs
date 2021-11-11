using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using System.ComponentModel;
using Northwoods.Go.Extensions;


namespace WinFormsExtensionControls.ParallelLinkRouting {
  [ToolboxItem(false)]
  public partial class ParallelLinkRoutingControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    public ParallelLinkRoutingControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;
      goWebBrowser1.Html = @"
          <p>
          A <b>ParallelRouteLink</b> is a custom <a>Link</a> that overrides <a>Link.computePoints</a>
          in order to produce a middle segment that is parallel to the routes of other <b>ParallelRouteLink</b>s
          connecting the same two ports.
          </p>
";
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // enable the Undo Manager
      myDiagram.UndoManager.IsEnabled = true;

      myDiagram.NodeTemplate = new Node(PanelLayoutAuto.Instance)
        .Bind("Location", "Loc", Point.Parse).Add(
          new Shape {
            PortId = "",
            FromLinkable = true,
            ToLinkable = true,
            FromLinkableDuplicates = true, // allow duplicate Links both from and to, to utilize parallel routing
            ToLinkableDuplicates = true,
            Cursor = "pointer"
          }.Bind("Fill", "Color"),
          new TextBlock {
            Margin = 8
          }.Bind("Text")
      );

      myDiagram.LinkTemplate = new ParallelRouteLink() {
        RelinkableFrom = true,
        RelinkableTo = true,
        Reshapable = true // allow the algorithm to reshape the ParallelRouteLink
      }.Add(
        new Shape { StrokeWidth = 2 }
          .Bind(new Binding("Stroke", "FromNode", (node, _) => { return ((node as Node).Port as Shape).Fill; }).OfElement()),
        new Shape { ToArrow = "OpenTriangle", StrokeWidth = 1.5 }
          .Bind(new Binding("Stroke", "FromNode", (node, _) => { return ((node as Node).Port as Shape).Fill; }).OfElement())
      );

      myDiagram.Model = new Model() {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = 1, Text = "Alpha", Color = "lightblue", Loc = "0 0" },
          new NodeData { Key = 2, Text = "Beta", Color = "orange", Loc = "130 70" },
          new NodeData { Key = 3, Text = "Gamma", Color = "lightgreen", Loc = "0 130" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = 1, To = 2 },
          new LinkData { From = 2, To = 1 },
          new LinkData { From = 1, To = 3 },
          new LinkData { From = 1, To = 3 },
          new LinkData { From = 3, To = 1 },
          new LinkData { From = 1, To = 3 },
          new LinkData { From = 1, To = 3 },
        }
      };
    }
  }
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }

  public class NodeData : Model.NodeData {
    public string Loc { get; set; }
    public Brush Color { get; set; }
  }

  public class LinkData : Model.LinkData { }
}
