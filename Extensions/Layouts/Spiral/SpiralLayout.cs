/*
*  Copyright (C) 1998-2021 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main GoDiagram library.
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
  /// A custom <see cref="Layout" /> that lays out a chain of nodes in a spiral.
  ///
  /// This layout assumes the graph is a chain of <see cref="Node"/>s,
  /// <see cref="Spacing"/> controls the spacing between nodes.
  ///
  /// If you want to experiment with this extension, try the <a href="../../extensionsTS/Spiral.html">Spiral Layout</a> sample.
  /// @category Layout Extension
  /// </summary>
  public class SpiralLayout : NetworkLayout<SpiralNetwork, SpiralVertex, SpiralEdge, SpiralLayout> {
    private double _Radius = double.NaN;
    private double _Spacing = 100;
    private bool _Clockwise = true;

    /// <summary>
    /// Create a Spiral Layout.
    /// </summary>
    public SpiralLayout() : base() { }

    /// <summary>
    /// Copies properties to a cloned Layout.
    /// </summary>
    /// <param name="c"></param>
    [Undocumented]
    protected override void CloneProtected(Layout c) {
      if (c == null) return;

      base.CloneProtected(c);
      var copy = (SpiralLayout)c;
      copy._Radius = _Radius;
      copy._Spacing = _Spacing;
      copy._Clockwise = _Clockwise;
    }

    /// <summary>
    /// Gets or sets the radius distance.
    ///
    /// The default value is NaN.
    /// </summary>
    public double Radius {
      get {
        return _Radius;
      }
      set {
        if (_Radius != value) {
          _Radius = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Gets or sets the spacing between nodes.
    ///
    /// The default value is 100.
    /// </summary>
    public double Spacing {
      get {
        return _Spacing;
      }
      set {
        if (_Spacing != value) {
          _Spacing = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Gets or sets whether the spiral should go clockwise or counter-clockwise.
    ///
    /// The default value is true.
    /// </summary>
    public bool Clockwise {
      get {
        return _Clockwise;
      }
      set {
        if (_Clockwise != value) {
          _Clockwise = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// This method actually positions all of the Nodes, assuming that
    /// the ordering of the nodes is given by a single link from one node
    /// to the next.
    /// This resepects the <see cref="Spacing"/> property to affect the layout.
    /// </summary>
    public override void DoLayout(IEnumerable<Part> coll = null) {
      if (Network == null) {
        Network = MakeNetwork(coll);
      }
      ArrangementOrigin = InitialOrigin(ArrangementOrigin);
      var originx = ArrangementOrigin.X;
      var originy = ArrangementOrigin.Y;

      SpiralVertex root = null;
      foreach (var v in Network.Vertexes) {
        if (root == null) root = v; // in case there are only circles
        if (v.SourceEdges.Count == 0) {
          root = v;
          break;
        }
      }

      // couldn't find a root vertex
      if (root == null) {
        Network = null;
        return;
      }

      var space = _Spacing;
      var cw = _Clockwise ? 1 : -1;
      var rad = _Radius;

      if (rad <= 0 || !Util.IsFinite(rad))
        rad = Diameter(root) / 4;

      // treat the root specially: it goes in the center
      var angle = cw * Math.PI;
      root.CenterX = originx;
      root.CenterY = originy;

      var edge = root.DestinationEdges.First();
      var link = edge?.Link;
      if (link != null) link.Curviness = cw * rad;

      // now locate each of the following nodes, in order, along a spiral
      var vert = edge?.ToVertex;
      while (vert != null) {
        // involute spiral
        var cos = Math.Cos(angle);
        var sin = Math.Sin(angle);

        var x = rad * (cos + angle * sin);
        var y = rad * (sin - angle * cos);
        //the link might connect to a member node of a group
        if (link != null && vert.Node is Group && link.ToNode != null && link.ToNode != vert.Node) {
          var offset = link.ToNode.Location.Subtract(vert.Node.Location);
          x -= offset.X;
          y -= offset.Y;
        }
        vert.CenterX = x + originx;
        vert.CenterY = y + originy;

        var nextedge = (vert.DestinationEdges.Count > 0) ? vert.DestinationEdges.First() : null;
        var nextvert = nextedge?.ToVertex;
        if (nextvert != null) {
          // clockwise curves want positive Link.curviness
          if (IsRouting && nextedge != null && nextedge.Link != null) {
            if (!double.IsNaN(nextedge.Link.Curviness)) {
              nextedge.Link.Curviness = cw * Math.Abs(nextedge.Link.Curviness);
            }
          }

          // determine next node's angle
          var dia = Diameter(vert) / 2 + Diameter(nextvert) / 2;
          angle += cw * Math.Atan((dia + space) / Math.Sqrt(x * x + y * y));
        }
        edge = nextedge;
        vert = nextvert;
      }

      UpdateParts();
      Network = null;
    }

    /// <summary>
    /// Compute the effective diameter of a Node.
    /// </summary>
    public double Diameter(SpiralVertex v) {
      if (v == null) return 0;
      var b = v.Bounds;
      return Math.Sqrt(b.Width * b.Width + b.Height * b.Height);
    }
  }
}
