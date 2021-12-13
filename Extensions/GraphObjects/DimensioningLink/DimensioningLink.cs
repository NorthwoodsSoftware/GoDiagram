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

namespace Northwoods.Go.Extensions {
  // A custom routed Link for showing the distances between a point on one node and a point on another node.

  // Note that because this is a Link, the points being measured must be on Nodes, not simple Parts.
  // The exact point on each Node is determined by the Link.fromSpot and Link.toSpot.

  // Several properties of the DimensioningLink customize the appearance of the dimensioning:
  // direction, for orientation of the dimension line and which side it is on,
  // extension, for how far the dimension line is from the measured points,
  // inset, for leaving room for a text label, and
  // gap, for distance that the extension line starts from the measured points
  /// <summary>
  /// A custom routed <see cref="Link"/> for showing the distances between a point on one node and a point on another node.
  /// </summary>
  /// <remarks>
  /// Note that because this is a Link, the points being measured must be on <see cref="Node"/>s, not simple <see cref="Part"/>s.
  /// The exact point on each Node is determined by the <see cref="Link.FromSpot"/> and <see cref="Link.ToSpot"/>.
  /// </remarks>
  public class DimensioningLink : Link {
    private double _Direction;
    private double _Extension;
    private double _Inset;
    private double _Gap;

    /// <summary>
    /// Constructs a DimensioningLink and sets the following properties:
    ///  - <see cref="Part.IsLayoutPositioned"/> = false
    ///  - <see cref="Link.IsTreeLink"/> = false
    ///  - <see cref="Link.Routing"/> = <see cref="LinkRouting.Orthogonal"/>
    /// </summary>
    public DimensioningLink() : base() {
      IsLayoutPositioned = false;
      IsTreeLink = false;
      Routing = LinkRouting.Orthogonal;

      _Direction = 0;
      _Extension = 0;
      _Inset = 0;
      _Gap = 0;
    }

    /// <summary>
    /// The general angle at which the measurement should be made.
    /// The default value is 0, meaning to go measure only along the X axis,
    /// with the dimension line and label above the two nodes (at lower Y coordinates).
    /// New values must be one of: 0, 90, 180, 270, or NaN.
    /// The value NaN indicates that the measurement is point-to-point and not orthogonal.
    /// </summary>
    public double Direction {
      get {
        return _Direction;
      }
      set {
        if (double.IsNaN(value) || value == 0 || value == 90 || value == 180 || value == 270) {
          _Direction = value;
        } else {
          throw new Exception("DimensioningLink: invalid new direction: " + value);
        }
      }
    }

    /// <summary>
    /// The distance at which the dimension line should be from the points being measured.
    /// The default value is 30.
    /// Larger values mean further away from the nodes.
    /// The new value must be greater than or equal to zero.
    /// </summary>
    public double Extension {
      get {
        return _Extension;
      }
      set {
        if (_Extension != value) {
          _Extension = value;
        }
      }
    }

    /// <summary>
    /// The distance that the dimension line should be indented from the ends of the
    /// extension lines that are orthogonal to the dimension line.
    /// The default value is 10.
    /// </summary>
    public double Inset {
      get {
        return _Inset;
      }
      set {
        if (_Inset != value) {
          if (value >= 0) {
            _Inset = value;
          } else {
            throw new Exception("DimensioningLink: invalid new inset: " + value);
          }
        }
      }
    }

    /// <summary>
    /// The distance that the extension lines should come short of the measured points.
    /// The default value is 10.
    /// </summary>
    public double Gap {
      get {
        return _Gap;
      }
      set {
        if (_Gap != value) {
          if (value >= 0) {
            _Gap = value;
          } else {
            throw new Exception("DimensioningLink: invalid new gap: " + value);
          }
        }
      }
    }

    /// <inheritdoc/>
    public override bool ComputePoints() {
      var fromnode = FromNode;
      if (fromnode == null) return false;
      var fromport = FromPort;
      var fromspot = ComputeSpot(true);
      var tonode = ToNode;
      if (tonode == null) return false;
      var toport = ToPort;
      var tospot = ComputeSpot(false);
      var frompoint = GetLinkPoint(fromnode, fromport, fromspot, true, true, tonode, toport);
      if (!frompoint.IsReal()) return false;
      var topoint = GetLinkPoint(tonode, toport, tospot, false, true, fromnode, fromport);
      if (!topoint.IsReal()) return false;

      ClearPoints();

      var ang = Direction;
      if (double.IsNaN(ang)) {
        ang = frompoint.Direction(topoint);
        var p = new Point(Extension, 0);
        p = p.Rotate(ang + 90);
        var q = new Point(Extension - Inset, 0);
        q = q.Rotate(ang + 90);
        var g = new Point(Gap, 0);
        g = g.Rotate(ang + 90);
        AddPointAt(frompoint.X + g.X, frompoint.Y + g.Y);
        AddPointAt(frompoint.X + p.X, frompoint.Y + p.Y);
        AddPointAt(frompoint.X + q.X, frompoint.Y + q.Y);
        AddPointAt(topoint.X + q.X, topoint.Y + q.Y);
        AddPointAt(topoint.X + p.X, topoint.Y + p.Y);
        AddPointAt(topoint.X + g.X, topoint.Y + g.Y);
      } else {
        var dist = Extension;
        var r = 0.0;
        var s = 0.0;
        var t0 = 0.0;
        var t1 = 0.0;
        if (ang == 0 || ang == 180) {
          if (ang == 0) {
            r = Math.Min(frompoint.Y, topoint.Y) - Extension;
            s = r + Inset;
            t0 = frompoint.Y - Gap;
            t1 = topoint.Y - Gap;
          } else {
            r = Math.Min(frompoint.Y, topoint.Y) + Extension;
            s = r - Inset;
            t0 = frompoint.Y + Gap;
            t1 = topoint.Y + Gap;
          }
          AddPointAt(frompoint.X, t0);
          AddPointAt(frompoint.X + 0.01, r);
          AddPointAt(frompoint.X, s);
          AddPointAt(topoint.X, s);
          AddPointAt(topoint.X - 0.01, r);
          AddPointAt(topoint.X, t1);
        } else if (ang == 90 || ang == 270) {
          if (ang == 90) {
            r = Math.Max(frompoint.X, topoint.X) + Extension;
            s = r - Inset;
            t0 = frompoint.X + Gap;
            t1 = topoint.X + Gap;
          } else {
            r = Math.Min(frompoint.X, topoint.X) - Extension;
            s = r + Inset;
            t0 = frompoint.X - Gap;
            t1 = topoint.X - Gap;
          }
          AddPointAt(t0, frompoint.Y);
          AddPointAt(r, frompoint.Y + 0.01);
          AddPointAt(s, frompoint.Y);
          AddPointAt(s, topoint.Y);
          AddPointAt(r, topoint.Y - 0.01);
          AddPointAt(t1, topoint.Y);
        }
      }

      UpdateTargetBindings();
      return true;
    }
  }
}
