using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.MultiNodePathLinks {
  [ToolboxItem(false)]
  public partial class MultiNodePathLinksControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public MultiNodePathLinksControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      goWebBrowser1.Html = @"
  <p>
  This sample demonstrates customization of the <a>Link</a>'s routing to go through multiple Nodes.
  The nodes are specified by key in the link data's ""path"" property, which must be an Array of node keys.
  </p>
  <p>
  As the user drags around Nodes on the ""path"", the routing is automatically recomputed to maintain a smooth curve.
  </p>
";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.AllowCopy = false;  // would need to copy linkdata.Path and update all of the refenced node keys
      myDiagram.AllowDelete = false;  // would need to update linkdata.Path for all links going through that node
      myDiagram.Changed += InvalidateLinkRoutes;
      myDiagram.UndoManager.IsEnabled = true;

      myDiagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance) {
          LocationSpot = Spot.Center
        }.Bind(
          new Binding("Location", "Loc", Point.Parse)
        ).Add(
          new Shape {
            Figure = "Circle",
            Fill = "white"
          }.Bind(
            new Binding("Fill", "Color")
          ),
          new TextBlock {
            Font = "Segoe UI, 11px, style=bold"
          }.Bind(
            new Binding("Text")
          )
        );

      myDiagram.LinkTemplate =
        new MultiNodePathLink {  // subclass of Link, defined below
          Curve = LinkCurve.Bezier,
          LayerName = "Background",
          ToShortLength = 4
        }.Add(
          new Shape {
            StrokeWidth = 4
          }.Bind(
            new Binding("Stroke", "Color")
          ),
          new Shape {
            ToArrow = "Standard",
            Scale = 3,
            StrokeWidth = 0
          }.Bind(
            new Binding("Fill", "Color")
          )
        );

      void InvalidateLinkRoutes(object _, ChangedEvent e) {
        // when a Node is moved, invalidate the route for all MultiNodePathLinks that go through it
        if (e.Change == ChangeType.Property && e.PropertyName == "Location" && e.Object is Node) {
          var diagram = e.Diagram;
          var node = e.Object as Node;
          if (node["_PathLinks"] != null) {
            foreach (var l in (node["_PathLinks"] as HashSet<MultiNodePathLink>)) {
              l.InvalidateRoute();
            }
          }
        } else if (e.Change == ChangeType.Remove && e.Object is Layer) {
          // when a Node is deleted that has MultiNodePathLinks going through it, invalidate those link routes
          if (e.OldValue is Node) {
            var node = e.OldValue as Node;
            if (node["_PathLinks"] != null) {
              foreach (var l in (node["_PathLinks"] as HashSet<MultiNodePathLink>)) {
                l.InvalidateRoute();
              }
            }
          } else if (e.OldValue is MultiNodePathLink) {
            // when deleting a MultiNodePathLink, remove all references to it in Node._PathLinks
            var link = e.OldValue as MultiNodePathLink;
            var diagram = e.Diagram;
            var midkeys = (link.Data as LinkData).Path;
            if (midkeys != null) {
              for (var i = 0; i < midkeys.Count; i++) {
                var node = diagram.FindNodeForKey(midkeys[i]);
                if (node != null && node["_PathLinks"] != null) {
                  (node["_PathLinks"] as HashSet<MultiNodePathLink>).Remove(link);
                }
              }
            }
          }
        }
      }

      // create a few nodes and links
      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = 1, Text = "Alpha", Color = "lightyellow", Loc = "0 0" },
          new NodeData { Key = 2, Text = "Beta", Color = "brown", Loc = "200 0" },
          new NodeData { Key = 3, Text = "Gamma", Color = "green", Loc = "300 100" },
          new NodeData { Key = 4, Text = "Delta", Color = "slateblue", Loc = "100 200" },
          new NodeData { Key = 5, Text = "Epsilon", Color = "aquamarine", Loc = "300 350" },
          new NodeData { Key = 6, Text = "Zeta", Color = "tomato", Loc = "0 100" },
          new NodeData { Key = 7, Text = "Eta", Color = "goldenrod", Loc = "0 300" },
          new NodeData { Key = 8, Text = "Theta", Color = "orange", Loc = "300 200" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = 1, To = 5, Path = new List<int> { 2, 3, 4 }, Color = "blue" },
          new LinkData { From = 6, To = 5, Path = new List<int> { 7, 4, 8 }, Color = "red" }
        }
      };
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Color { get; set; }
    public string Loc { get; set; }
  }

  public class LinkData : Model.LinkData {
    public List<int> Path { get; set; }
    public string Color { get; set; }
  }

  // extend link
  public class MultiNodePathLink : Link {
    // ignores this.Routing, this.Adjusting, this.Corner, this.Smoothness, this.Curviness
    public override bool ComputePoints() {
      // get the list of Nodes that should be along the path
      var nodes = new List<Node>();
      if (this.FromNode != null && this.FromNode.Location.IsReal()) {
        nodes.Add(this.FromNode);
      }
      var midkeys = (this.Data as LinkData).Path;
      if (midkeys != null) {
        var diagram = this.Diagram;
        for (var i = 0; i < midkeys.Count; i++) {
          var node = diagram.FindNodeForKey(midkeys[i]);
          if (node is Node && node.Location.IsReal()) {
            nodes.Add(node);
            // Optimization? = remember on each path Node all of
            // the MultiNodePathLinks that go through it;
            // but this optimization requires maintaining this cache
            // in a Diagram Changed event listener.
            var set = node["_PathLinks"] as HashSet<MultiNodePathLink>;
            if (set == null) {
              node["_PathLinks"] = new HashSet<MultiNodePathLink>();
              set = node["_PathLinks"] as HashSet<MultiNodePathLink>;
            }
            set.Add(this);
          }
        }
      }
      if (this.ToNode != null && this.ToNode.Location.IsReal()) {
        nodes.Add(this.ToNode);
      }

      // now do the routing
      this.ClearPoints();
      Point prevloc = new Point(double.NaN, double.NaN); ;
      Point thisloc = new Point(double.NaN, double.NaN); ;
      Point nextloc = new Point(double.NaN, double.NaN); ;
      for (var i = 0; i < nodes.Count; i++) {
        var node = nodes[i];
        thisloc = node.Location;
        nextloc = (i < nodes.Count - 1) ? nodes[i + 1].Location : new Point(double.NaN, double.NaN); ;

        Point prevpt = new Point(double.NaN, double.NaN);
        Point nextpt = new Point(double.NaN, double.NaN);
        if (this.Curve == LinkCurve.Bezier) {
          if (prevloc.IsReal() && nextloc.IsReal() && thisloc.IsReal()) {
            var prevang = thisloc.Direction(prevloc);
            var nextang = thisloc.Direction(nextloc);
            var avg = (prevang + nextang) / 2;
            var clockwise = prevang > nextang;
            if (Math.Abs(prevang - nextang) > 180) {
              avg += 180;
              clockwise = !clockwise;
            }
            if (avg >= 360) avg -= 360;
            prevpt = new Point(Math.Sqrt(thisloc.DistanceSquared(prevloc)) / 4, 0);
            prevpt = prevpt.Rotate(avg + (clockwise ? 90 : -90)).Add(thisloc);
            nextpt = new Point(Math.Sqrt(thisloc.DistanceSquared(nextloc)) / 4, 0);
            nextpt = nextpt.Rotate(avg - (clockwise ? 90 : -90)).Add(thisloc);
          } else if (nextloc.IsReal()) {
            prevpt = new Point(double.NaN, double.NaN);
            nextpt = thisloc;  // fix this point after the loop
          } else if (prevloc.IsReal()) {
            var lastpt = this.GetPoint(this.PointsCount - 1);
            prevpt = thisloc;  // fix this point after the loop
            nextpt = new Point(double.NaN, double.NaN);
          }
        }

        if (prevpt.IsReal()) this.AddPoint(prevpt);
        this.AddPoint(thisloc);
        if (nextpt.IsReal()) this.AddPoint(nextpt);
        prevloc = thisloc;
      }

      // fix up the end points when it's Bezier
      if (this.Curve == LinkCurve.Bezier) {
        // fix up the first point and the first control point
        var start = this.GetLinkPointFromPoint(this.FromNode, this.FromPort, this.FromPort.GetDocumentPoint(Spot.Center), this.GetPoint(3), true);
        var ctrl2 = this.GetPoint(2);
        this.SetPoint(0, start);
        this.SetPoint(1, new Point((start.X * 3 + ctrl2.X) / 4, (start.Y * 3 + ctrl2.Y) / 4));
        // fix up the last point and the last control point
        var end = this.GetLinkPointFromPoint(this.ToNode, this.ToPort, this.ToPort.GetDocumentPoint(Spot.Center), this.GetPoint(this.PointsCount - 4), false);
        var ctrl1 = this.GetPoint(this.PointsCount - 3);
        this.SetPoint(this.PointsCount - 2, new Point((end.X * 3 + ctrl1.X) / 4, (end.Y * 3 + ctrl1.Y) / 4));
        this.SetPoint(this.PointsCount - 1, end);
      }
      return true;
    }
  }

}
