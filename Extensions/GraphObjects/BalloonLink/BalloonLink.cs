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
using System.Linq;

namespace Northwoods.Go.Extensions {
  /// <summary>
  /// This custom <see cref="Link"/> class customizes its <see cref="Shape"/> to surround the comment node (the from node).
  /// </summary>
  /// <remarks>
  /// If the Shape is filled, it will obscure the comment itself unless the Link is behind the comment node.
  /// Thus the default layer for BalloonLinks is "Background".
  ///
  /// The <see cref="Link.Corner"/> property controls the radius of the curves at the corners of the rectangular area surrounding the comment node,
  /// rather than the curve at corners along the route, which is always straight.
  /// The default value is 10.
  /// </remarks>
  /// @category Part Extension
  public class BalloonLink : Link {
    private double _Base = 15;

    /// <summary>
    /// Constructs a BalloonLink and sets the <see cref="Part.LayerName"/> property to "Background".
    /// </summary>
    public BalloonLink() : base() {
      LayerName = "Background";
      Corner = 10;
      DefaultToPoint = new Point(0, 0);
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
    /// The default value is 15.
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
    /// Produce a Geometry from the Link's route that draws a "balloon" shape around the <see cref="Link.FromNode"/>
    /// and has a triangular shape with the base at the FromNode and the top at the ToNode.
    /// </summary>
    public override Geometry MakeGeometry() {
      if (FromNode == null) return base.MakeGeometry();
      // assume the FromNode is the comment and the ToNode is the commented-upon node
      var bb = FromNode.ActualBounds.Inflate(FromNode.Margin);

      var pn = PointsCount == 0 ? bb.Center : GetPoint(PointsCount - 1);
      if (ToNode != null && bb.Intersects(ToNode.ActualBounds)) {
        pn = ToNode.ActualBounds.Center;
      } else if (ToNode == null && PointsCount == 0) {
        pn = new Point(bb.Center.X, bb.Bottom + 50);
      }

      var geobase = Math.Max(0, Base);
      var corner = Math.Min(Math.Max(0, Corner), Math.Min(bb.Width / 2, bb.Height / 2));
      var cornerext = Math.Min(geobase, corner + geobase / 2);

      var fig = new PathFigure();
      var prevx = 0.0;
      var prevy = 0.0;

      // helper functions
      void start(double x, double y) {
        fig.StartX = prevx = x;
        fig.StartY = prevy = y;
      }
      void point(double x, double y, double v, double w) {
        fig.Add(new PathSegment(SegmentType.Line, x, y));
        fig.Add(new PathSegment(SegmentType.Line, v, w));
        prevx = v;
        prevy = w;
      }
      void turn(double x, double y) {
        if (prevx == x && prevy > y) {  // top left
          fig.Add(new PathSegment(SegmentType.Line, x, y + corner));
          fig.Add(new PathSegment(SegmentType.Arc, 180, 90, x + corner, y + corner, corner, corner));
        } else if (prevx < x && prevy == y) {  // top right
          fig.Add(new PathSegment(SegmentType.Line, x - corner, y));
          fig.Add(new PathSegment(SegmentType.Arc, 270, 90, x - corner, y + corner, corner, corner));
        } else if (prevx == x && prevy < y) {  // bottom right
          fig.Add(new PathSegment(SegmentType.Line, x, y - corner));
          fig.Add(new PathSegment(SegmentType.Arc, 0, 90, x - corner, y - corner, corner, corner));
        } else if (prevx > x && prevy == y) {  // bottom left
          fig.Add(new PathSegment(SegmentType.Line, x + corner, y));
          fig.Add(new PathSegment(SegmentType.Arc, 90, 90, x + corner, y - corner, corner, corner));
        } // else if prevx == x && prevy == y, no-op
        prevx = x;
        prevy = y;
      }

      if (pn.X < bb.Left) {
        if (pn.Y < bb.Top) {
          start(bb.Left, Math.Min(bb.Top + cornerext, bb.Bottom - corner));
          point(pn.X, pn.Y, Math.Min(bb.Left + cornerext, bb.Right - corner), bb.Top);
          turn(bb.Right, bb.Top); turn(bb.Right, bb.Bottom); turn(bb.Left, bb.Bottom);
        } else if (pn.Y > bb.Bottom) {
          start(Math.Min(bb.Left + cornerext, bb.Right - corner), bb.Bottom);
          point(pn.X, pn.Y, bb.Left, Math.Max(bb.Bottom - cornerext, bb.Top + corner));
          turn(bb.Left, bb.Top); turn(bb.Right, bb.Top); turn(bb.Right, bb.Bottom);
        } else {  // pn.Y >= bb.Top && pn.Y <= bb.Bottom
          var y = Math.Min(Math.Max(pn.Y + geobase / 3, bb.Top + corner + geobase), bb.Bottom - corner);
          start(bb.Left, y);
          point(pn.X, pn.Y, bb.Left, Math.Max(y - geobase, bb.Top + corner));
          turn(bb.Left, bb.Top); turn(bb.Right, bb.Top); turn(bb.Right, bb.Bottom); turn(bb.Left, bb.Bottom);
        }
      } else if (pn.X > bb.Right) {
        if (pn.Y < bb.Top) {
          start(Math.Max(bb.Right - cornerext, bb.Left + corner), bb.Top);
          point(pn.X, pn.Y, bb.Right, Math.Min(bb.Top + cornerext, bb.Bottom - corner));
          turn(bb.Right, bb.Bottom); turn(bb.Left, bb.Bottom); turn(bb.Left, bb.Top);
        } else if (pn.Y > bb.Bottom) {
          start(bb.Right, Math.Max(bb.Bottom - cornerext, bb.Top + corner));
          point(pn.X, pn.Y, Math.Max(bb.Right - cornerext, bb.Left + corner), bb.Bottom);
          turn(bb.Left, bb.Bottom); turn(bb.Left, bb.Top); turn(bb.Right, bb.Top);
        } else {  // pn.Y >= bb.Top && pn.Y <= bb.Bottom
          var y = Math.Min(Math.Max(pn.Y + geobase / 3, bb.Top + corner + geobase), bb.Bottom - corner);
          start(bb.Right, Math.Max(y - geobase, bb.Top + corner));
          point(pn.X, pn.Y, bb.Right, y);
          turn(bb.Right, bb.Bottom); turn(bb.Left, bb.Bottom); turn(bb.Left, bb.Top); turn(bb.Right, bb.Top);
        }
      } else {  // pn.X >= bb.Left && pn.X <= bb.Right
        var x = Math.Min(Math.Max(pn.X + geobase / 3, bb.Left + corner + geobase), bb.Right - corner);
        if (pn.Y < bb.Top) {
          start(Math.Max(x - geobase, bb.Left + corner), bb.Top);
          point(pn.X, pn.Y, x, bb.Top);
          turn(bb.Right, bb.Top); turn(bb.Right, bb.Bottom); turn(bb.Left, bb.Bottom); turn(bb.Left, bb.Top);
        } else if (pn.Y > bb.Bottom) {
          start(x, bb.Bottom);
          point(pn.X, pn.Y, Math.Max(x - geobase, bb.Left + corner), bb.Bottom);
          turn(bb.Left, bb.Bottom); turn(bb.Left, bb.Top); turn(bb.Right, bb.Top); turn(bb.Right, bb.Bottom);
        } else { // inside
          start(bb.Left, bb.Top + bb.Height / 2);
          // no "point", just corners
          turn(bb.Left, bb.Top); turn(bb.Right, bb.Top); turn(bb.Right, bb.Bottom); turn(bb.Left, bb.Bottom);
        }
      }

      var geo = new Geometry();
      fig.Segments.Last().Close();
      geo.Add(fig);
      geo.Offset(-RouteBounds.X, -RouteBounds.Y);
      return geo;
    }
  }
}
