/*
*  Copyright (C) 1998-2022 by Northwoods Software Corporation. All Rights Reserved.
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
  /// Given a root <see cref="Node"/>, this arranges connected nodes in concentric rings,
  /// layered by the minimum link distance from the root.
  /// </summary>
  /// <remarks>
  /// If you want to experiment with this extension, try the <a href="../../extensions/Radial.html">Radial Layout</a> sample.
  /// </remarks>
  /// @category Layout Extension
  public class RadialLayout : NetworkLayout<RadialNetwork, RadialVertex, RadialEdge, RadialLayout> {
    private Node _Root = null;
    private int _LayerThickness = 100;
    private int _MaxLayers = int.MaxValue;

    private delegate RadialVertex LeastVertex(ISet<RadialVertex> vertexes);

    /// <summary>
    /// Create a Radial layout.
    /// </summary>
    public RadialLayout() : base() { }

    /// <summary>
    /// Copies properties to a cloned Layout.
    /// </summary>
    [Undocumented]
    protected override void CloneProtected(Layout c) {
      if (c == null) return;

      base.CloneProtected(c);
      var copy = (RadialLayout)c;
      // don't copy root
      copy._LayerThickness = _LayerThickness;
      copy._MaxLayers = _MaxLayers;
    }

    /// <summary>
    /// Gets or sets the <see cref="Node"/> that acts as the root or central node of the radial layout.
    /// </summary>
    public Node Root {
      get {
        return _Root;
      }
      set {
        if (_Root != value) {
          _Root = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Gets or sets the thickness of each ring representing a layer.
    /// </summary>
    /// <remarks>
    /// The default value is 100.
    /// </remarks>
    public int LayerThickness {
      get {
        return _LayerThickness;
      }
      set {
        if (_LayerThickness != value) {
          _LayerThickness = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Gets or sets the maximum number of layers to be shown, in addition to the root node at layer zero.
    /// </summary>
    /// <remarks>
    /// The default value is int.MaxValue.
    /// </remarks>
    public int MaxLayers {
      get {
        return _MaxLayers;
      }
      set {
        if (_MaxLayers != value) {
          _MaxLayers = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Find distances between root and vertexes, and then lay out radially.
    /// </summary>
    public override void DoLayout(IEnumerable<Part> coll = null) {
      if (Network == null) {
        Network = MakeNetwork(coll);
      }
      if (Network.Vertexes.Count == 0) {
        Network = null;
        return;
      }

      if (_Root == null) {
        // If no root supplied, choose one without any incoming edges
        foreach (var v in Network.Vertexes) {
          if (v.Node != null && v.SourceEdges.Count == 0) {
            _Root = v.Node;
            break;
          }
        }
      }

      if (_Root == null && Network != null) {
        // If could not find any default root, choose a random one
        var first = Network.Vertexes.First();
        _Root = first?.Node;
      }
      if (_Root == null) {
        Network = null;
        return;
      }

      var rootvert = Network.FindVertex(_Root);
      if (rootvert == null) throw new Exception("RadialLayout.root must be a Node in the LayoutNetwork that the RadialLayout is opearting on");

      ArrangementOrigin = InitialOrigin(ArrangementOrigin);
      _FindDistances(rootvert);

      // now recursively position nodes (using _RadLay1()), starting with the root
      rootvert.CenterX = ArrangementOrigin.X;
      rootvert.CenterY = ArrangementOrigin.Y;
      _RadLay1(rootvert, 1, 0, 360);

      // Update the "physical" positions of the nodes and links.
      UpdateParts();

      Network = null;
    }

    // Recursively position vertexes in a radial layout
    private void _RadLay1(RadialVertex vert, int layer, double angle, double sweep) {
      if (layer > _MaxLayers) return; // no need to position nodes outside of MaxLayers
      var verts = vert.Children; // array of all RadialVertexes connected to 'vert' in layer 'layer'
      var found = verts.Count;
      if (found == 0) return;

      var fracs = new List<double>(); // relative proportions that each child vertex should occupy
      var tot = 0.0;
      for (var i = 0; i < found; i++) {
        var v = verts[i];
        var f = ComputeBreadth(v);
        fracs.Add(f);
        tot += f;
      }
      if (tot <= 0) return;
      // convert into fractions 0.0 <= frac <= 1.0
      for (var i = 0; i < found; i++) fracs[i] /= tot;

      var radius = layer * _LayerThickness;
      var a = angle - sweep / 2; // the angle to rotate the node to
      // for each vertex in this layer, place it in its correct layer and position
      for (var i = 0; i < found; i++) {
        var v = verts[i];
        var breadth = fracs[i] * sweep;
        a += breadth / 2;
        if (a < 0) a += 360; else if (a > 360) a -= 360;
        // the point to place the node at -- this corresponds with the layer the node is in
        // all nodes in the same layer are placed at a constant point, then rotated accordingly
        var p = new Point(radius, 0);
        p = p.Rotate(a);
        v.CenterX = p.X + ArrangementOrigin.X;
        v.CenterY = p.Y + ArrangementOrigin.Y;
        v.Angle = a;
        v.Sweep = breadth;
        v.Radius = radius;
        // keep going for all layers
        _RadLay1(v, layer + 1, a, sweep * fracs[i]);
        a += breadth / 2;
        if (a < 0) a += 360; else if (a > 360) a -= 360;
      }
    }

    /// <summary>
    /// Compute the proportion of arc that the given vertex should take relative to its siblings.
    /// </summary>
    /// <remarks>
    /// The default behavior is to give each child arc according to the sum of the maximum breadths of each of its children.
    /// This assumes that all nodes have the same breadth -- i.e. that they will occupy the same sweep of arc.
    /// It does not take the Node.ActualBounds into account, nor the angle at which the node will be location relative to the origin,
    /// nor the distance the node will be from the root node.
    ///
    /// In order to give each child of a vertex the same fraction of arc, override this method:
    /// <code language="cs">public override double ComputeBreadth(RadialVertex v) { return 1; }</code>
    ///
    /// In order to give each child of a vertex a fraction of arc proportional to how many children the child has:
    /// <code language="cs">public override double ComputeBreadth(RadialVertex v) { return Math.Max(1, v.Children.Count); }</code>
    /// </remarks>
    /// <param name="v"></param>
    public virtual double ComputeBreadth(RadialVertex v) {
      var b = 0.0;
      foreach (var w in v.Children) {
        b += ComputeBreadth(w);  // inefficient
      }
      return Math.Max(b, 1);
    }

    // Update RadialVertex.Distance for every vertex.
    private void _FindDistances(RadialVertex source) {
      if (Network == null) return;

      // keep track of distances from the source node.
      foreach (var v in Network.Vertexes) {
        v.Distance = int.MaxValue;
        v.Laid = false;
      }
      // the source node starts with distance 0
      source.Distance = 0;
      // keep track of nodes for we have set a non-Infinity distance,
      // but which we have not yet finished examining
      var seen = new HashSet<RadialVertex> {
        source
      };

      // local function for finding a vertex with the smallest distance in a given collection
      static RadialVertex leastVertex(ISet<RadialVertex> vertexes) {
        var bestdist = int.MaxValue;
        RadialVertex bestvert = null;

        foreach (var v in vertexes) {
          if (v.Distance < bestdist) {
            bestdist = v.Distance;
            bestvert = v;
          }
        }

        return bestvert;
      }

      // keep track of vertexes we have finished examining;
      // this avoids unnecessary traversals and helps keep the SEEN collection small
      var finished = new HashSet<RadialVertex>();
      while (seen.Count > 0) {
        // look at the unfinished vertex with the shortest distance so far
        var least = leastVertex(seen);
        if (least == null) break;
        var leastdist = least.Distance;
        // by the end of this loop we will have finished examining this LEAST vertex
        seen.Remove(least);
        finished.Add(least);
        // look at all edges connected with this vertex
        foreach (var edge in least.Edges) {
          var neighbor = edge.GetOtherVertex(least);
          if (neighbor == null) continue;
          // skip vertexes that we have finished
          if (finished.Contains(neighbor)) continue;
          var neighbordist = neighbor.Distance;
          // assume "distance" along a link is unitary, but could be any non-negative number.
          var dist = leastdist + 1;
          if (dist < neighbordist) {
            // if haven't seen that vertex before, add it to the SEEN collection
            if (neighbordist == int.MaxValue) {
              seen.Add(neighbor);
            }
            // record the new best distance so far to that node
            neighbor.Distance = dist;
          }
        }
      }

      // now update the RadialVertex.Children Arrays to form a tree-structure
      foreach (var v in Network.Vertexes) {
        var dist = v.Distance;
        var arr = v.Children;
        if (arr == null) arr = v.Children = new List<RadialVertex>();
        foreach (var w in v.Vertexes) {  // use LayoutVertex.Vertexes to remove duplicates
          // use the RadialVertex.Laid property for avoiding already-traversed vertexes
          if (!w.Laid && w != v && w.Distance == dist + 1) {
            arr.Add(w);
            w.Laid = true;
          }
        }
      }

      // reset RadialVertex.Laid in case of future use
      foreach (var v in Network.Vertexes) {
        v.Laid = false;
      }
    }

    /// <inheritdoc/>
    protected override void CommitLayout() {
      base.CommitLayout();
      if (Network != null) {
        foreach (var v in Network.Vertexes) {
          if (v.Node != null) {
            v.Node.Visible = (v.Distance <= _MaxLayers);
            RotateNode(v.Node, v.Angle, v.Sweep, v.Radius);
          }
        }
      }
      CommitLayers();
    }

    /// <summary>
    /// Override this method in order to modify each node as it is laid out.
    /// </summary>
    /// <remarks>
    /// By default this method does nothing.
    /// </remarks>
    public virtual void RotateNode(Node node, double angle, double sweep, double radius) { }

    /// <summary>
    /// Override this method in order to create background circles indicating the layers of the radial layout.
    /// </summary>
    /// <remarks>
    /// By default this method does nothing.
    /// </remarks>
    public virtual void CommitLayers() { }
  }
}
