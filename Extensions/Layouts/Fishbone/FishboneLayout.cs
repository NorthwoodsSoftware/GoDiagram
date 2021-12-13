/*
*  Copyright (C) 1998-2021 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main Go library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in the GoDiagram repository (https://github.com/NorthwoodsSoftware/GoDiagram/tree/main/Extensions).
* See the Extensions intro page (https://godiagram.com/intro/extensions.html) for more information.
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Northwoods.Go.Layouts.Extensions {
  /// <summary>
  /// FishboneLayout is a custom <see cref="Layout"/> derived from <see cref="TreeLayout"/> for creating "fishbone" diagrams.
  /// A fishbone diagram also requires a <see cref="Link"/> class that implements custom routing, <see cref="FishboneLink"/>.
  ///
  /// This only works for angle == 0 or angle == 180.
  ///
  /// This layout assumes Links are automatically routed in the way needed by fishbone diagrams
  /// by using the FishboneLink class instead of <see cref="Link"/>.
  ///
  /// If you want to experiment with this extension, try the <a href="../../extensionsTS/Fishbone.html">Fishbone Layout</a> sample.
  /// </summary>
  public class FishboneLayout : TreeLayout {
    private readonly Dictionary<TreeVertex, int> _Direction = new Dictionary<TreeVertex, int>();

    /// <summary>
    /// Create a Fishbone layout.
    /// </summary>
    public FishboneLayout() : base() {
      Alignment = TreeAlignment.BusBranching;
      SetsPortSpot = false;
      SetsChildPortSpot = false;
    }

    /// <summary>
    /// Create and initialize a <see cref="TreeNetwork"/> with the given nodes and links.
    /// This override creates dummy vertexes, when necessary, to allow for proper positioning within the fishbone.
    /// </summary>
    /// <param name="coll"></param>
    /// <returns><see cref="TreeNetwork"/></returns>
    public override TreeNetwork MakeNetwork(IEnumerable<Part> coll = null) {
      var net = base.MakeNetwork(coll);
      if (net == null) return net;

      // make a copy of the collection of TreeVertexes
      // because we will be modifying the TreeNetwork.Vertexes collection in the loop
      var verts = new List<TreeVertex>(net.Vertexes);
      foreach (var v in verts) {
        // ignore leaves of tree
        if (v.DestinationEdges.Count == 0) continue;
        if (v.DestinationEdges.Count % 2 == 1) {
          // if there's an odd number of real children, add two dummies
          var dummy = net.CreateVertex();
          dummy.Bounds = new Rect();
          dummy.Focus = new Point();
          net.AddVertex(dummy);
          net.LinkVertexes(v, dummy, null);
        }
        // make sure there's an odd number of children, including at least one dummy;
        // commitNodes will move the parent node to where this dummy child node is placed
        var dummy2 = net.CreateVertex();
        dummy2.Bounds = v.Bounds;
        dummy2.Focus = v.Focus;
        net.AddVertex(dummy2);
        net.LinkVertexes(v, dummy2, null);
      }

      return net;
    }

    /// <summary>
    /// Add a direction property to each vertex and modify <see cref="TreeVertex.LayerSpacing"/>.
    /// </summary>
    /// <param name="v"></param>
    protected override void AssignTreeVertexValues(TreeVertex v) {
      base.AssignTreeVertexValues(v);
      _Direction[v] = 0;  // assign direction to each TreeVertex
      if (v.Parent != null) {
        // The parent node will be moved to where the last dummy will be;
        // reduce the space to account for the future hole.
        if (v.Angle == 0 || v.Angle == 180) {
          v.LayerSpacing -= v.Bounds.Width;
        } else {
          v.LayerSpacing -= v.Bounds.Height;
        }
      }
    }

    /// <inheritdoc/>
    protected override void CommitNodes() {
      if (Network == null) return;

      // vertex Angle is set by BusBranching inheritance;
      // assign spots assuming overall Angle == 0 or 180
      // and links are always connecting horizontal with vertical
      foreach (var e in Network.Edges) {
        var link = e.Link;
        if (link == null) continue;
        link.FromSpot = Spot.None;
        link.ToSpot = Spot.None;

        var v = e.FromVertex as TreeVertex;
        var w = e.ToVertex as TreeVertex;

        if (v.Angle == 0) {
          link.FromSpot = Spot.Left;
        } else if (v.Angle == 180) {
          link.FromSpot = Spot.Right;
        }

        if (w.Angle == 0) {
          link.ToSpot = Spot.Left;
        } else if (w.Angle == 180) {
          link.ToSpot = Spot.Right;
        }
      }

      // move the parent node to the location of the last dummy
      foreach (var v in Network.Vertexes) {
        var len = v.Children.Length;
        if (len == 0) continue; // ignore leaf nodes
        if (v.Parent == null) continue; // dont move root node
        var dummy2 = v.Children[len - 1];

        v.CenterX = dummy2.CenterX;
        v.CenterY = dummy2.CenterY;
      }

      //perform a shift from the root
      foreach (var v in Network.Vertexes) {
        if (v.Parent == null) {
          Shift(v);
        }
      }

      // now actually change the Node.location of all nodes
      base.CommitNodes();
    }

    /// <summary>
    /// This override stops links from being committed since the work is handled by the <see cref="FishboneLink"/> class.
    /// </summary>
    protected override void CommitLinks() { }

    /// <summary>
    /// Shifts subtrees within the fishbone based on angle and node spacing.
    /// </summary>
    /// <param name="v"></param>
    public void Shift(TreeVertex v) {
      var p = v.Parent;
      if (p != null && (v.Angle == 90 || v.Angle == 270)) {
        var g = p.Parent;
        if (g != null) {
          var shift = v.NodeSpacing;
          if (_Direction[g] > 0) {
            if (g.Angle == 90) {
              if (p.Angle == 0) {
                _Direction[v] = 1;
                if (v.Angle == 270) ShiftAll(2, -shift, p, v);
              } else if (p.Angle == 180) {
                _Direction[v] = -1;
                if (v.Angle == 90) ShiftAll(-2, shift, p, v);
              }
            } else if (g.Angle == 270) {
              if (p.Angle == 0) {
                _Direction[v] = 1;
                if (v.Angle == 90) ShiftAll(2, -shift, p, v);
              } else if (p.Angle == 180) {
                _Direction[v] = -1;
                if (v.Angle == 270) ShiftAll(-2, shift, p, v);
              }
            }
          } else if (_Direction[g] < 0) {
            if (g.Angle == 90) {
              if (p.Angle == 0) {
                _Direction[v] = 1;
                if (v.Angle == 90) ShiftAll(2, -shift, p, v);
              } else if (p.Angle == 180) {
                _Direction[v] = -1;
                if (v.Angle == 270) ShiftAll(-2, shift, p, v);
              }
            } else if (g.Angle == 270) {
              if (p.Angle == 0) {
                _Direction[v] = 1;
                if (v.Angle == 270) ShiftAll(2, -shift, p, v);
              } else if (p.Angle == 180) {
                _Direction[v] = -1;
                if (v.Angle == 90) ShiftAll(-2, shift, p, v);
              }
            }
          }
        } else { // g == null: V is a child of the tree Root
          var dir = p.Angle == 0 ? 1 : -1;
          _Direction[v] = dir;
          ShiftAll(dir, 0, p, v);
        }
      }
      foreach (var c in v.Children) {
        Shift(c);
      }
    }

    /// <summary>
    /// Shifts a subtree.
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="absolute"></param>
    /// <param name="root"></param>
    /// <param name="v"></param>
    public void ShiftAll(int direction, double absolute, TreeVertex root, TreeVertex v) {
      var locx = v.CenterX;
      locx += direction * Math.Abs(root.CenterY - v.CenterY) / 2;
      locx += absolute;
      v.CenterX = locx;
      foreach (var c in v.Children) {
        ShiftAll(direction, absolute, root, c);
      }
    }
    // end Fishbone Layout
  }

  /// <summary>
  /// Custom <see cref="Link"/> class for <see cref="FishboneLayout"/>.
  /// </summary>
  public class FishboneLink : Link {
    /// <inheritdoc/>
    public override LinkAdjusting ComputeAdjusting() {
      return Adjusting;
    }

    /// <summary>
    /// Determines the points for this link based on spots and maintains horizontal lines.
    /// </summary>
    /// <returns></returns>
    public override bool ComputePoints() {
      var result = base.ComputePoints();
      if (result) {
        // insert middle point to maintain horizontal lines
        if (FromSpot == Spot.Right || FromSpot == Spot.Left) {
          // deal with root node being on the wrong side
          var fromnode = FromNode;
          var fromport = FromPort;
          Point p1;

          if (fromnode != null && fromport != null && fromnode.FindLinksInto().Count() == 0) {
            // pretend the link is coming from the opposite direction than the declared FromSpot
            var fromctr = fromport.GetDocumentPoint(Spot.Center);
            var fromfar = new Point(fromctr.X, fromctr.Y);

            fromfar.X += (FromSpot == Spot.Left) ? 99999 : -99999;
            p1 = GetLinkPointFromPoint(fromnode, fromport, fromctr, fromfar, true);

            // update the route points
            SetPoint(0, p1);
            var endseg = FromEndSegmentLength;
            if (double.IsNaN(endseg)) endseg = fromport.FromEndSegmentLength;
            p1.X += (FromSpot == Spot.Left) ? endseg : -endseg;
            SetPoint(1, p1);
          } else {
            p1 = GetPoint(1);
          }

          var tonode = ToNode;
          var toport = ToPort;
          if (tonode != null && toport != null) {
            var toctr = toport.GetDocumentPoint(Spot.Center);
            var far = new Point(toctr.X, toctr.Y);

            far.X += (FromSpot == Spot.Left) ? -99999 / 2 : 99999 / 2;
            far.Y += (toctr.Y < p1.Y) ? 99999 : -99999;

            var p2 = GetLinkPointFromPoint(tonode, toport, toctr, far, false);
            SetPoint(2, p2);

            var dx = Math.Abs(p2.Y - p1.Y) / 2;
            if (FromSpot == Spot.Left) dx = -dx;
            InsertPoint(2, new Point(p2.X + dx, p1.Y));
          }
        } else if (ToSpot == Spot.Right || ToSpot == Spot.Left) {
          var p1 = GetPoint(1);
          var fromnode = FromNode;
          var fromport = FromPort;

          if (fromnode != null && FromPort != null) {
            var parentlink = fromnode.FindLinksInto().First();
            var fromctr = fromport.GetDocumentPoint(Spot.Center);
            var far = new Point(fromctr.X, fromctr.Y);

            far.X += (parentlink != null && parentlink.FromSpot == Spot.Left) ? -99999 / 2 : 99999 / 2;
            far.Y += (fromctr.Y < p1.Y) ? 99999 : -99999;
            var p0 = GetLinkPointFromPoint(fromnode, fromport, fromctr, far, true);
            SetPoint(0, p0);

            var dx = Math.Abs(p1.Y - p0.Y) / 2;
            if (parentlink != null && parentlink.FromSpot == Spot.Left) dx = -dx;
            InsertPoint(1, new Point(p0.X + dx, p1.Y));
          }
        }
      }
      return result;
    }
  }
}
