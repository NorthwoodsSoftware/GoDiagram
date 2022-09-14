/*
*  Copyright (C) 1998-2022 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main GoDiagram library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in the GoDiagram repository (https://github.com/NorthwoodsSoftware/GoDiagram/tree/main/Extensions).
* See the Extensions intro page (https://godiagram.com/intro/extensions.html) for more information.
*/

using System;
using System.Linq;

namespace Northwoods.Go.Tools.Extensions {
  /// <summary>
  /// The SnapLinkReshapingTool class lets the user snap link reshaping handles to the nearest grid point.
  /// </summary>
  /// <remarks>
  /// If <see cref="AvoidsNodes"/> is true and the link is orthogonal,
  /// it also avoids reshaping the link so that any adjacent segments cross over any avoidable nodes.
  /// </remarks>
  /// @category Tool Extension
  public class SnapLinkReshapingTool : LinkReshapingTool {
    private Size _GridCellSize = new(double.NaN, double.NaN);
    private Point _GridOrigin = new(double.NaN, double.NaN);
    private bool _IsGridSnapEnabled = true;
    private bool _AvoidsNodes = true;
    // internal state
    private Point _SafePoint = new(double.NaN, double.NaN);
    private bool _PrevSegHoriz = false;
    private bool _NextSegHoriz = false;

    /// <summary>
    /// Gets or sets the <see cref="Size"/> of each grid cell to which link points will be snapped.
    /// </summary>
    /// <remarks>
    /// The default value is NaNxNaN, which means use the <see cref="Diagram.Grid"/>'s <see cref="Panel.GridCellSize"/>.
    /// </remarks>
    public Size GridCellSize {
      get {
        return _GridCellSize;
      }
      set {
        _GridCellSize = value;
      }
    }

    /// <summary>
    /// Gets or sets the <see cref="Point"/> origin for the grid to which link points will be snapped.
    /// </summary>
    /// <remarks>
    /// The default value is NaNxNaN, which means use the <see cref="Diagram.Grid"/>'s <see cref="Panel.GridOrigin"/>.
    /// </remarks>
    public Point GridOrigin {
      get {
        return _GridOrigin;
      }
      set {
        _GridOrigin = value;
      }
    }

    /// <summary>
    /// Gets or sets whether a reshape handle's position should be snapped to a grid point.
    /// This affects the behavior of <see cref="ComputeReshape"/>.
    /// </summary>
    /// <remarks>
    /// The default value is true.
    /// </remarks>
    public bool IsGridSnapEnabled {
      get {
        return _IsGridSnapEnabled;
      }
      set {
        _IsGridSnapEnabled = value;
      }
    }

    /// <summary>
    /// Gets or sets whether a reshape handle's position should only be dragged where the
    /// adjacent segments do not cross over any nodes.
    /// This affects the behavior of <see cref="ComputeReshape"/>.
    /// </summary>
    /// <remarks>
    /// The default value is true.
    /// </remarks>
    public bool AvoidsNodes {
      get {
        return _AvoidsNodes;
      }
      set {
        _AvoidsNodes = value;
      }
    }

    /// <summary>
    /// This override records information about the original point of the handle being dragged,
    /// if the <see cref="LinkReshapingTool.AdornedLink"/> is Orthogonal and if <see cref="AvoidsNodes"/> is true.
    /// </summary>
    public override void DoActivate() {
      base.DoActivate();
      if (IsActive && AvoidsNodes && AdornedLink != null && AdornedLink.IsOrthogonal && Handle != null) {
        // assume the Link's route starts off correctly avoiding all nodes
        _SafePoint = Diagram.LastInput.DocumentPoint;
        var link = AdornedLink;
        var idx = (int)Handle.SegmentIndex;
        _PrevSegHoriz = Math.Abs(link.GetPoint(idx - 1).Y - link.GetPoint(idx).Y) < 0.5;
        _NextSegHoriz = Math.Abs(link.GetPoint(idx + 1).Y - link.GetPoint(idx).Y) < 0.5;
      }
    }

    /// <summary>
    /// Pretend while dragging a reshape handle the mouse point is at the nearest grid point, if <see cref="IsGridSnapEnabled"/> is true.
    /// </summary>
    /// <remarks>
    /// This uses <see cref="GridCellSize"/> and <see cref="GridOrigin"/>, unless those are not real values,
    /// in which case this uses the <see cref="Diagram.Grid"/>"s <see cref="Panel.GridCellSize"/> and <see cref="Panel.GridOrigin"/>.
    ///
    /// If <see cref="AvoidsNodes"/> is true and the adorned Link is <see cref="Link.IsOrthogonal"/>,
    /// this method also avoids returning a Point that causes the adjacent segments, both before and after
    /// the current handle's index, to cross over any Nodes that are <see cref="Node.Avoidable"/>.
    /// </remarks>
    public override Point ComputeReshape(Point p) {
      var pt = p;
      var diagram = Diagram;
      if (IsGridSnapEnabled) {
        // first, find the grid to which we should snap
        var cell = GridCellSize;
        var orig = GridOrigin;
        if (!cell.IsReal() || cell.Width == 0 || cell.Height == 0) cell = diagram.Grid.GridCellSize;
        if (!orig.IsReal()) orig = diagram.Grid.GridOrigin;
        // second, compute the closest grid point
        pt = p.SnapToGrid(orig.X, orig.Y, cell.Width, cell.Height);
      }
      if (AvoidsNodes && AdornedLink != null && AdornedLink.IsOrthogonal) {
        if (_CheckSegmentsOverlap(pt)) {
          _SafePoint = pt;
        } else {
          pt = _SafePoint;
        }
      }
      // then do whatever LinkReshapingTool would normally do as if the mouse were at that point
      return base.ComputeReshape(pt);
    }

    /// <summary>
    /// Internal method for seeing whether a moved handle will cause any
    /// adjacent orthogonal segments to cross over any avoidable nodes.
    /// Returns true if everything would be OK.
    /// </summary>
    private bool _CheckSegmentsOverlap(Point pt) {
      if (Handle == null) return true;
      if (AdornedLink == null) return true;
      var index = (int)Handle.SegmentIndex;

      if (index >= 1) {
        var p1 = AdornedLink.GetPoint(index - 1);
        var r = new Rect(pt.X, pt.Y, 0, 0);
        var q1 = p1;
        if (_PrevSegHoriz) {
          q1.Y = pt.Y;
        } else {
          q1.X = pt.X;
        }
        r = r.Union(q1);
        var overlaps = Diagram.FindPartsIn(r, true, false);
        if (Enumerable.Any(overlaps, (Part p) => { return p is Node && (p as Node).Avoidable; })) return false;

        if (index >= 2) {
          var p0 = AdornedLink.GetPoint(index - 2);
          r = new Rect(q1.X, q1.Y, 0, 0);
          if (_PrevSegHoriz) {
            r = r.Union(new Point(q1.X, p0.Y));
          } else {
            r = r.Union(new Point(p0.X, q1.Y));
          }
          overlaps = Diagram.FindPartsIn(r, true, false);
          if (Enumerable.Any(overlaps, (Part p) => { return p is Node && (p as Node).Avoidable; })) return false;
        }
      }

      if (index < AdornedLink.PointsCount - 1) {
        var p2 = AdornedLink.GetPoint(index + 1);
        var r = new Rect(pt.X, pt.Y, 0, 0);
        var q2 = p2;
        if (_NextSegHoriz) {
          q2.Y = pt.Y;
        } else {
          q2.X = pt.X;
        }
        r = r.Union(q2);
        var overlaps = Diagram.FindPartsIn(r, true, false);
        if (Enumerable.Any(overlaps, (Part p) => { return p is Node && (p as Node).Avoidable; })) return false;

        if (index < AdornedLink.PointsCount - 2) {
          var p3 = AdornedLink.GetPoint(index + 2);
          r = new Rect(q2.X, q2.Y, 0, 0);
          if (_NextSegHoriz) {
            r = r.Union(new Point(q2.X, p3.Y));
          } else {
            r = r.Union(new Point(p3.X, q2.Y));
          }
          overlaps = Diagram.FindPartsIn(r, true, false);
          if (Enumerable.Any(overlaps, (Part p) => { return p is Node && (p as Node).Avoidable; })) return false;
        }
      }

      return true;
    }
  }
}
