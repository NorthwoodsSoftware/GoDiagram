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

namespace Northwoods.Go.Extensions {
  /// <summary>
  /// This custom <see cref="Link"/> class customizes its <see cref="Shape"/> to surround the comment node (the from node).
  /// If the Shape is filled, it will obscure the comment itself unless the Link is behind the comment node.
  /// Thus the default layer for BalloonLinks is "Background".
  /// </summary>
  /// <remarks>
  /// If you want to experiment with this extension, try the <a href="../../extensions/BalloonLink.html">Balloon Links</a> sample.
  /// </remarks>
  /// @category Part Extension
  public class BalloonLink : Link {
    private double _Base = 10;

    /// <summary>
    /// Constructs a BalloonLink and sets the <see cref="Part.LayerName"/> property to "Background".
    /// </summary>
    public BalloonLink() : base() {
      LayerName = "Background";
    }

    /// <summary>
    /// Copies properties to a cloned BalloonLink.
    /// </summary>
    protected void CloneProtected(BalloonLink copy) {
      base.CloneProtected(copy);
      copy._Base = _Base;
    }

    /// <summary>
    /// Gets or sets width of the base of the triangle at the center point of the <see cref="Link.FromNode"/>.
    /// </summary>
    /// <remarks>
    /// The default value is 10.
    /// </remarks>
    public double Base {
      get {
        return _Base;
      }
      set {
        _Base = value;
      }
    }

    /// <summary>
    /// Produce a Geometry from the Link"s route that draws a "balloon" shape around the <see cref="Link.FromNode"/>
    /// and has a triangular shape with the base at the fromNode and the top at the toNode.
    /// </summary>
    public override Geometry MakeGeometry() {
      var fromnode = FromNode;
      var tonode = ToNode;
      if (fromnode == null || tonode == null) return base.MakeGeometry();
      // assume the FromNode is the comment and the ToNode is the commented-upon node
      var bb = fromnode.ActualBounds;
      var nb = tonode.ActualBounds;

      var p0 = bb.Center;
      var pn = GetPoint(PointsCount - 1);
      if (bb.Intersects(nb)) {
        pn = nb.Center;
      }
      var pos = RouteBounds;

      // compute the intersection points for the triangular arrow
      var ang = pn.Direction(p0);
      var L = new Point(Base, 0).Rotate(ang - 90).Add(p0);
      var R = new Point(Base, 0).Rotate(ang + 90).Add(p0);
      L = GetLinkPointFromPoint(fromnode, fromnode, L, pn, true);
      R = GetLinkPointFromPoint(fromnode, fromnode, R, pn, true);

      // form a triangular arrow from the comment to the commented node
      var fig = new PathFigure(pn.X - pos.X, pn.Y - pos.Y, true);  // filled; start at arrow point at commented node
      fig.Add(new PathSegment(SegmentType.Line, R.X - pos.X, R.Y - pos.Y));  // a triangle base point on comment's edge
      var side = 0;
      if (L.Y >= bb.Bottom || R.Y >= bb.Bottom) side = 2;
      else if (L.X <= bb.X && R.X <= bb.X) side = 1;
      else if (L.X >= bb.Right && R.X >= bb.Right) side = 3;

      PathToCorner(side, bb, fig, pos, L, R);
      PathToCorner(side + 1, bb, fig, pos, L, R);
      PathToCorner(side + 2, bb, fig, pos, L, R);
      PathToCorner(side + 3, bb, fig, pos, L, R);
      fig.Add(new PathSegment(SegmentType.Line, L.X - pos.X, L.Y - pos.Y).Close());  // the other triangle base point on comment's edge

      // return a Geometry
      return new Geometry().Add(fig);
    }

    /// <summary>
    /// Draw a line to a corner, but not if the comment arrow encompasses that corner.
    /// </summary>
    public static void PathToCorner(int side, Rect bb, PathFigure fig, Rect pos, Point L, Point R) {
      switch (side % 4) {
        case 0: if (!(L.Y <= bb.Y && R.X <= bb.X)) fig.Add(new PathSegment(SegmentType.Line, bb.X - pos.X, bb.Y - pos.Y)); break;
        case 1: if (!(L.X <= bb.X && R.Y >= bb.Bottom)) fig.Add(new PathSegment(SegmentType.Line, bb.X - pos.X, bb.Bottom - pos.Y)); break;
        case 2: if (!(L.Y >= bb.Bottom && R.X >= bb.Right)) fig.Add(new PathSegment(SegmentType.Line, bb.Right - pos.X, bb.Bottom - pos.Y)); break;
        case 3: if (!(L.X >= bb.Right && R.Y <= bb.Y)) fig.Add(new PathSegment(SegmentType.Line, bb.Right - pos.X, bb.Y - pos.Y)); break;
      }
    }
  }
}
