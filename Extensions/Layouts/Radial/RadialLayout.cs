using System;
using System.Collections.Generic;
using System.Linq;

/*
*  Copyright (C) 1998-2020 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main Go library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in GoExamples under the Extensions folder.
* See the Extensions intro page (<replace>) for more information.
*/

namespace Northwoods.Go.Layouts.Extensions {
  /**
  * <summary>
  * Given a root {@link Node}, this arranges connected nodes in concentric rings,
  * layered by the minimum link distance from the root.
  *
  * If you want to experiment with this extension, try the <a href="../../extensionsTS/Radial.html">Radial Layout</a> sample.
  * </summary>
  * @category Layout Extension
  */
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
    /// <param name="c"></param>
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
    ///
    /// The default value is 100.
    /// </summary>
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
    ///
    /// The default value is int.MaxValue.
    /// </summary>
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
      if (Network.Vertexes.Count > 0) {
        if (_Root == null) {
          //If no root supplied, choose one without any incoming edges
          foreach (var v in Network.Vertexes) {
            if (v.Node != null && v.SourceEdges.Count == 0) {
              _Root = v.Node;
              break;
            }
          }
        }

        if (_Root == null && Network != null) {
          //If could not find any default root, choose a random one
          var first = Network.Vertexes.First();
          _Root = (first == null) ? null : first.Node;
        }
        if (_Root == null) return;

        var rootvert = Network.FindVertex(_Root);
        if (rootvert == null) throw new Exception("RadialLayout.root must be a Node in the LayoutNetwork that the RadialLayout is opearting on");
        ArrangementOrigin = InitialOrigin(ArrangementOrigin);
        findDistances(rootvert);
        //sort all results into Arrays of RadialVertexes with the same distance
        var maxlayer = 0;
        foreach (var v in Network.Vertexes) {
          v.Laid = false;

          if (v.Distance == int.MaxValue) continue;
          if (v.Distance > maxlayer) maxlayer = v.Distance;
        }

        IList<RadialVertex>[] verts = new List<RadialVertex>[1 + maxlayer];

        foreach (var v in Network.Vertexes) {
          var layer = v.Distance;
          if (v.Distance == int.MaxValue) continue;

          if (verts[layer] == null) {
            verts[layer] = new List<RadialVertex>();
          }
          verts[layer].Add(v);
        }
        //now recursively position nodes (using radlay1()), starting with the root
        rootvert.CenterX = ArrangementOrigin.X;
        rootvert.CenterY = ArrangementOrigin.Y;
        radlay1(rootvert, 1, 0, 360);
        UpdateParts();
      }

      // restore in case of future re-use
      _Root = null;
      Network = null;
      IsValidLayout = true;
    }

    /*
     * Recursively position vertexes in a radial layout
     */
    private void radlay1(RadialVertex vert, int layer, double angle, double sweep) {
      if (layer > _MaxLayers) return; //no need to position nodes outside of maxLayers
      IList<RadialVertex> verts = new List<RadialVertex>(); //array of all RadialVertexes connected to 'vert' in layer 'layer'

      foreach (var v in vert.Vertexes) {
        if (v.Laid) continue;
        if (v.Distance == layer) verts.Add(v);
      }

      var found = verts.Count();
      if (found == 0) return;

      var radius = layer * _LayerThickness;
      var separator = sweep / found; //distance between nodes in their sweep portion
      var start = angle - sweep / 2 + separator / 2;

      //for each vertex in this layer, place it in its correct layer and position
      for (var i = 0; i < verts.Count(); i++) {
        var v = verts[i];
        var a = start + i * separator; //the angle to rotate the node to
        if (a < 0) a += 360; else if (a > 360) a -= 360;
        // the point to place the node at -- this corresponds with the layer the node is in
        // all nodes in the same layer are placed at a constant point, then rotated accordingly
        var p = new Point(radius, 0);
        p = p.Rotate(a);

        v.CenterX = p.X + ArrangementOrigin.X;
        v.CenterY = p.Y + ArrangementOrigin.Y;
        v.Laid = true;
        v.Angle = a;
        v.Sweep = separator;
        v.Radius = radius;

        // keep going for all layers
        radlay1(v, layer + 1, a, sweep / found);
      }
    }

    /*
     * Update RadialVertex.distance for every vertex.
     */
    private void findDistances(RadialVertex source) {
      if (Network == null) return;

      //keep track of distances from the source node.
      foreach (var v in Network.Vertexes) {
        v.Distance = int.MaxValue;
      }

      //the source node starts with distance 0
      source.Distance = 0;

      ISet<RadialVertex> seen = new HashSet<RadialVertex> {
        source
      };

      LeastVertex leastVertex = (vertexes) => {
        var bestdist = int.MaxValue;
        RadialVertex bestvert = null;

        foreach (var v in vertexes) {
          if (v.Distance < bestdist) {
            bestdist = v.Distance;
            bestvert = v;
          }
        }

        return bestvert;
      };

      // keep track of vertexes we have finished examining;
      // this avoids unnecessary traversals and helps keep the SEEN collection small
      ISet<RadialVertex> finished = new HashSet<RadialVertex>();

      while (seen.Count() > 0) {
        //look at the unfinished vertex with the shortest distance so far
        var least = leastVertex(seen);
        if (least == null) return;
        var leastdist = least.Distance;

        seen.Remove(least);
        finished.Add(least);

        //look at all edges connected with this vertex
        foreach (var edge in least.Edges) {
          if (least == null) return;
          var neighbor = edge.GetOtherVertex(least);

          //skip vertexes that we have finished
          if (finished.Contains(neighbor)) continue;
          var neighbordist = neighbor.Distance;

          //assume "distance" along a link is unitary, but could be any non-negative number.
          var dist = leastdist + 1;
          if (dist < neighbordist) {
            // if haven't seen that vertex before, add it to the SEEN collection
            if (neighbordist == int.MaxValue) {
              seen.Add(neighbor);
            }

            //record the new best distance so far to that node
            neighbor.Distance = dist;
          }
        }
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
    /// By default this method does nothing.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="angle"></param>
    /// <param name="sweep"></param>
    /// <param name="radius"></param>
    public virtual void RotateNode(Node node, double angle, double sweep, double radius) { }

    /// <summary>
    /// Override this method in order to create background circles indicating the layers of the radial layout.
    /// By default this method does nothing.
    /// </summary>
    public virtual void CommitLayers() { }
  }
}
