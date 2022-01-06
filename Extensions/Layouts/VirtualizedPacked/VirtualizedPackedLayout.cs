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
using System.Collections.Generic;

namespace Northwoods.Go.Layouts.Extensions {
  /// <summary>
  /// Used to represent the perimeter of the currently packed
  /// shape when packing rectangles. Segments are always assumed
  /// to be either horizontal or vertical, and store whether or
  /// not their first point is concave (this makes sense in the
  /// context of representing a perimeter, as the next segment
  /// will always be connected to the last).
  /// </summary>
  internal class Segment {
    public double X1;
    public double Y1;
    public double X2;
    public double Y2;
    public Rect Bounds;
    public bool P1Concave;
    public bool IsHorizontal; // if the segment is not horizontal, it is assumed to be vertical

    /// <summary>
    /// Constructs a new Segment. Segments are assumed to be either
    /// horizontal or vertical, and the given coordinates should
    /// reflect that.
    /// </summary>
    /// <param name="x1">the x coordinate of the first point</param>
    /// <param name="y1">the y coordinate of the first point</param>
    /// <param name="x2">the x coordinate of the second point</param>
    /// <param name="y2">the y coordinate of the second point</param>
    /// <param name="p1Concave">whether or not the first point is concave</param>
    public Segment(double x1, double y1, double x2, double y2, bool p1Concave) {
      X1 = x1;
      Y1 = y1;
      X2 = x2;
      Y2 = y2;
      Bounds = Segment.RectFromSegment(this);
      P1Concave = p1Concave;
      IsHorizontal = Math.Abs(y2 - y1) < 1e-7;
    }

    /// <summary>
    /// Gets a rectangle representing the bounds of a given segment.
    /// Used to supply bounds of segments to the quadtree.
    /// </summary>
    /// <param name="segment">the segment to get a rectangle for</param>
    public static Rect RectFromSegment(Segment segment) {
      if (Math.Abs(segment.X1 - segment.X2) < 1e-7) {
        return new Rect(segment.X1, Math.Min(segment.Y1, segment.Y2), 0, Math.Abs(segment.Y1 - segment.Y2));
      }
      return new Rect(Math.Min(segment.X1, segment.X2), segment.Y1, Math.Abs(segment.X1 - segment.X2), 0);
    }
  }

  /// <summary>
  /// Defines the possible orientations that two adjacent
  /// horizontal/vertical segments can form.
  /// </summary>
  internal enum Orientation {
    NE,
    NW,
    SW,
    SE
  }

  /// <summary>
  /// Structure for storing possible placements when packing
  /// rectangles. Fits have a cost associated with them (lower
  /// cost placements are preferred), and can be placed relative
  /// to either one or two segments. If the fit is only placed
  /// relative to one segment, s2 will be undefined. Fits placed
  /// relative to multiple segments will hereafter be referred to
  /// as "skip fits".
  /// </summary>
  internal class Fit {
    public Rect Bounds;
    public double Cost;
    public VirtualizedPackedLayout.ListNode<Segment> S1;
    public VirtualizedPackedLayout.ListNode<Segment> S2;

    /// <summary>
    /// Constructs a new Fit.
    /// </summary>
    /// <param name="bounds">the boundaries of the placement, including defined x and y coordinates</param>
    /// <param name="cost">the cost of the placement, lower cost fits will be preferred</param>
    /// <param name="s1">the segment that the placement was made relative to</param>
    /// <param name="s2">the second segment that the placement was made relative to, if the fit is a skip fit</param>
    public Fit(Rect bounds, double cost, VirtualizedPackedLayout.ListNode<Segment> s1, VirtualizedPackedLayout.ListNode<Segment> s2 = null) {
      Bounds = bounds;
      Cost = cost;
      S1 = s1;
      S2 = s2;
    }
  }

  [Undocumented]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
  public interface IBounded {
    Rect Bounds { get; set; }
  }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member


#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved
  /// <summary>
  /// A custom Layout that attempts to pack nodes as close together as possible
  /// without overlap.
  /// </summary>
  /// <remarks>
  /// Each node is assumed to be either rectangular or
  /// circular (dictated by the <see cref="HasCircularNodes"/> property). This layout
  /// supports packing nodes into either a rectangle or an ellipse, with the
  /// shape determined by the PackShape property and the aspect ratio determined
  /// by either the AspectRatio property or the specified width and height
  /// (depending on the PackMode).
  ///
  /// Nodes with 0 width or height cannot be packed, so they are treated by this
  /// layout as having a width or height of 0.1 instead.
  ///
  /// Unlike other "Virtualized..." layouts, this does not inherit from <see cref="PackedLayout"/>
  /// because there were a lot of internal changes that needed to be made. That may
  /// change in the future if PackedLayout's implementation is generalized.
  /// </remarks>
  /// @category Layout Extension
  [Undocumented]
#pragma warning restore CS1574 // XML comment has cref attribute that could not be resolved
  public class VirtualizedPackedLayout : Layout {
    /// <summary>
    /// Copies properties to a cloned Layout.
    /// </summary>
    [Undocumented]
    protected override void CloneProtected(Layout c) {
      if (c == null) return;

      base.CloneProtected(c);
      var copy = (VirtualizedPackedLayout)c;
      copy._PackShape = _PackShape;
      copy._PackMode = _PackMode;
      copy._SortMode = _SortMode;
      copy._SortOrder = _SortOrder;
      copy._Comparer = _Comparer;
      copy._AspectRatio = _AspectRatio;
      copy._Size = _Size;
      copy._Spacing = _Spacing;
      copy._HasCircularNodes = _HasCircularNodes;
      copy._ArrangesToOrigin = _ArrangesToOrigin;
    }

    /// <summary>
    /// Gets or sets the shape that nodes will be packed into. Valid values are
    /// <see cref="VPackShape.Elliptical"/>, <see cref="VPackShape.Rectangular"/>, and
    /// <see cref="VPackShape.Spiral"/>.
    /// </summary>
    /// <remarks>
    /// In <see cref="VPackShape.Spiral"/> mode, nodes are not packed into a particular
    /// shape, but rather packed consecutively one after another in a spiral fashion.
    /// The <see cref="AspectRatio"/> property is ignored in this mode, and
    /// the <see cref="Size"/> property (if provided) is expected to be square.
    /// If it is not square, the largest dimension given will be used. This mode
    /// currently only works with circular nodes, so setting it cause the assume that
    /// layout to assume that <see cref="HasCircularNodes"/> is true.
    ///
    /// Note that this property sets only the shape, not the aspect ratio. The aspect
    /// ratio of this shape is determined by either <see cref="AspectRatio"/>
    /// or <see cref="Size"/>, depending on the <see cref="PackMode"/>.
    ///
    /// When the <see cref="PackMode"/> is <see cref="VPackMode.Fit"/> or
    /// <see cref="VPackMode.ExpandToFit"/> and this property is set to true, the
    /// layout will attempt to make the diameter of the enclosing circle of the
    /// layout approximately equal to the greater dimension of the given
    /// <see cref="Size"/> property.
    ///
    /// The default value is <see cref="VPackShape.Elliptical"/>.
    /// </remarks>
    public VPackShape PackShape {
      get {
        return _PackShape;
      }
      set {
        if (_PackShape != value && (value == VPackShape.Elliptical || value == VPackShape.Rectangular || value == VPackShape.Spiral)) {
          _PackShape = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Gets or sets the mode that the layout will use to determine its size.
    /// </summary>
    /// <remarks>
    /// The default value is <see cref="VPackMode.AspectOnly"/>. In this mode, the layout will simply
    /// grow as needed, attempting to keep the aspect ratio defined by <see cref="AspectRatio"/>.
    /// </remarks>
    public VPackMode PackMode {
      get {
        return _PackMode;
      }
      set {
        if (value == VPackMode.AspectOnly || value == VPackMode.Fit || value == VPackMode.ExpandToFit) {
          _PackMode = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Gets or sets the method by which nodes will be sorted before being packed. To change
    /// the order, see <see cref="SortOrder"/>.
    /// </summary>
    /// <remarks>
    /// The default value is <see cref="VSortMode.None"/>, in which nodes will not be sorted at all.
    /// </remarks>
    public VSortMode SortMode {
      get {
        return _SortMode;
      }
      set {
        if (value == VSortMode.None || value == VSortMode.MaxSide || value == VSortMode.Area) {
          _SortMode = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Gets or sets the order that nodes will be sorted in before being packed. To change
    /// the sort method, see <see cref="SortMode"/>.
    /// </summary>
    /// <remarks>
    /// The default value is <see cref="VSortOrder.Descending"/>
    /// </remarks>
    public VSortOrder SortOrder {
      get {
        return _SortOrder;
      }
      set {
        if (value == VSortOrder.Descending || value == VSortOrder.Ascending) {
          _SortOrder = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Gets or sets the comparison function used for sorting nodes.
    /// </summary>
    /// <remarks>
    /// By default, the comparison function is set according to the values of <see cref="SortMode"/>
    /// and <see cref="SortOrder"/>.
    ///
    /// Whether this comparison function is used is determined by the value of <see cref="SortMode"/>.
    /// Any value except <see cref="VSortMode.None"/> will result in the comparison function being used.
    /// <code language="cs">
    ///   myDiagram.Layout = new VirtualizedPackedLayout {
    ///       SortMode = VSortMode.Area,
    ///       Comparer = (na, nb) => {
    ///         var na = na.Data;
    ///         var nb = nb.Data;
    ///         if (da.SomeProperty &lt; db.SomeProperty) return -1;
    ///         if (da.SomeProperty &gt; db.SomeProperty) return 1;
    ///         return 0;
    ///       }
    ///     };
    /// </code>
    /// </remarks>
    public Comparison<IBounded> Comparer {
      get {
        return _Comparer;
      }
      set {
        _Comparer = value;
      }
    }

    /// <summary>
    /// Gets or sets the aspect ratio for the shape that nodes will be packed into.
    /// </summary>
    /// <remarks>
    /// The provided aspect ratio should be a nonzero postive number.
    /// 
    /// Note that this only applies if the <see cref="PackMode"/> is
    /// <see cref="VPackMode.AspectOnly"/>. Otherwise, the <see cref="Size"/>
    /// will determine the aspect ratio of the packed shape.
    ///
    /// The default value is 1.
    /// </remarks>
    public double AspectRatio {
      get {
        return _AspectRatio;
      }
      set {
        if (Util.IsFinite(value) && value > 0) {
          _AspectRatio = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Gets or sets the size for the shape that nodes will be packed into.
    /// </summary>
    /// <remarks>
    /// To fill the viewport, set a size with a width and height of NaN. Size
    /// values of 0 are considered for layout purposes to instead be 1.
    ///
    /// If the width and height are set to NaN (to fill the viewport), but this
    /// layout has no diagram associated with it, the default value of size will
    /// be used instead.
    ///
    /// Note that this only applies if the <see cref="PackMode"/> is
    /// <see cref="VPackMode.Fit"/> or <see cref="VPackMode.ExpandToFit"/>.
    ///
    /// The default value is 500x500.
    /// </remarks>
    public Size Size {
      get {
        return _Size;
      }
      set {
        // check if both width and height are NaN, as per https://stackoverflow.Com/a/16988441
        if (value.Width != value.Width && value.Height != value.Height) {
          _Size = value;
          _FillViewport = true;
          InvalidateLayout();
        } else if (Util.IsFinite(value.Width) && value.Width >= 0 &&
                   Util.IsFinite(value.Height) && value.Height >= 0) {
          _Size = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Gets or sets the spacing between nodes.
    /// </summary>
    /// <remarks>
    /// This value can be set to any
    /// real number (a negative spacing will compress nodes together, and a
    /// positive spacing will leave space between them).
    ///
    /// Note that the spacing value is only respected in the <see cref="VPackMode.Fit"/>
    /// <see cref="PackMode"/> if it does not cause the layout to grow outside
    /// of the specified bounds. In the <see cref="VPackMode.ExpandToFit"/>
    /// <see cref="PackMode"/>, this property does not do anything.
    ///
    /// The default value is 0.
    /// </remarks>
    public double Spacing {
      get {
        return _Spacing;
      }
      set {
        if (Util.IsFinite(value)) {
          _Spacing = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Gets or sets whether or not to assume that nodes are circular. This changes
    /// the packing algorithm to one that is much more efficient for circular nodes.
    /// </summary>
    /// <remarks>
    /// As this algorithm expects circles, it is assumed that if this property is set
    /// to true that the given nodes will all have the same height and width. All
    /// calculations are done using the width of the given nodes, so unexpected results
    /// may occur if the height differs from the width.
    ///
    /// The default value is false.
    /// </remarks>
    public bool HasCircularNodes {
      get {
        return _HasCircularNodes;
      }
      set {
        if (value != _HasCircularNodes) {
          _HasCircularNodes = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// This read-only property is the effective spacing calculated after <see cref="PerformLayout(List{IBounded})"/>.
    /// </summary>
    /// <remarks>
    /// If the <see cref="PackMode"/> is <see cref="VPackMode.AspectOnly"/>, this will simply be the
    /// <see cref="Spacing"/> property. However, in the <see cref="VPackMode.Fit"/> and
    /// <see cref="VPackMode.ExpandToFit"/> modes, this property will include the forced spacing added by
    /// the modes themselves.
    ///
    /// Note that this property will only return a valid value after a layout has been performed. Before
    /// then, its behavior is undefined.
    /// </remarks>
    public double ActualSpacing {
      get {
        return Spacing + _FixedSizeModeSpacing;
      }
    }

    /// <summary>
    /// This read-only property returns the actual rectangular bounds occupied by the packed nodes.
    /// </summary>
    /// <remarks>
    /// This property does not take into account any kind of spacing around the packed nodes.
    ///
    /// Note that this property will only return a valid value after a layout has been performed. Before
    /// then, its behavior is undefined.
    /// </remarks>
    public Rect ActualBounds {
      get {
        return _ActualBounds;
      }
    }

    /// <summary>
    /// This read-only property returns the smallest enclosing circle around the packed nodes.
    /// </summary>
    /// <remarks>
    /// It makes use of the <see cref="HasCircularNodes"/> property to determine whether or not to make
    /// enclosing circle calculations for rectangles or for circles. This property does not take into
    /// account any kind of spacing around the packed nodes. The enclosing circle calculation is
    /// performed the first time this property is retrieved, and then cached to prevent slow accesses
    /// in the future.
    ///
    /// Note that this property will only return a valid value after a layout has been performed. Before
    /// then, its behavior is undefined.
    ///
    /// This property is included as it may be useful for some data visualizations.
    /// </remarks>
    public Rect EnclosingCircle {
      get {
        if (_EnclosingCircle == null) {
          if (HasCircularNodes || PackShape == VPackShape.Spiral) { // remember, spiral mode assumes HasCircularNodes
            var circles = new List<Circle>(_NodeBounds.Count);
            for (var i = 0; i < circles.Count; i++) {
              var bounds = _NodeBounds[i];
              var r = bounds.Width / 2;
              circles[i] = new Circle(bounds.X + r, bounds.Y + r, r);
            }
            _EnclosingCircle = Enclose(circles);
          } else {
            var points = new List<Point>(); // TODO: make this work with segments, not the whole nodebounds list
            var segment = _Segments.Start;
            if (segment != null) {
              do {
                points.Add(new Point(segment.Data.X1, segment.Data.Y1));
                segment = segment.Next;
              } while (segment != _Segments.Start);
            }
            _EnclosingCircle = Enclose(points);
          }
        }

        return _EnclosingCircle.Value;
      }
    }

    /// <summary>
    /// Gets or sets whether or not to use the <see cref="Layout.ArrangementOrigin"/>
    /// property when placing nodes.
    /// </summary>
    /// <remarks>
    /// The default value is true.
    /// </remarks>
    public bool ArrangesToOrigin {
      get {
        return _ArrangesToOrigin;
      }
      set {
        if (value != _ArrangesToOrigin) {
          _ArrangesToOrigin = value;
          InvalidateLayout();
        }
      }
    }

    // configuration defaults
    private VPackShape _PackShape = VPackShape.Elliptical;
    private VPackMode _PackMode = VPackMode.AspectOnly;
    private VSortMode _SortMode = VSortMode.None;
    private VSortOrder _SortOrder = VSortOrder.Descending;
    private Comparison<IBounded> _Comparer = null;
    private double _AspectRatio = 1;
    private Size _Size = new(500, 500);
    private Size _DefaultSize = new(500, 500);
    private bool _FillViewport = false; // true if size is (NaN, NaN)
    private double _Spacing = 0;
    private bool _HasCircularNodes = false;
    private bool _ArrangesToOrigin = true;

    /// <summary>
    /// The forced spacing value applied in the <see cref="VPackMode.Fit"/>
    /// and <see cref="VPackMode.ExpandToFit"/> modes.
    /// </summary>
    private double _FixedSizeModeSpacing = 0;

    /// <summary>
    /// The actual target aspect ratio, set from either <see cref="AspectRatio"/>
    /// or from the <see cref="Size"/>, depending on the <see cref="PackMode"/>.
    /// </summary>
    private double _EAspectRatio = 1;

    // layout state
    private Point _Center = new();
    private Rect _Bounds = new();
    private Rect _ActualBounds = new();
    private Rect? _EnclosingCircle = null;
    private Segment _MinXSegment = null;
    private Segment _MinYSegment = null;
    private Segment _MaxXSegment = null;
    private Segment _MaxYSegment = null;
    private readonly Quadtree<Segment> _Tree = new();

    // saved node bounds and segment list to use to calculate enclosing circle in the EnclosingCircle getter
    private List<Rect> _NodeBounds = new();
    private CircularDoublyLinkedList<Segment> _Segments = new();

    private readonly Random _Rand = new();

    /// <summary>
    /// Performs the VirtualizedPackedLayout.
    /// </summary>
    /// <param name="nodes">a collection of bounded <see cref="Part"/>s</param>
    public void PerformLayout(List<IBounded> nodes) {
      if (nodes == null) {
        return;
      }

      var diagram = Diagram;
      if (diagram != null) diagram.StartTransaction("Layout");
      _Bounds = new Rect();
      _EnclosingCircle = default;

      // push all nodes in parts iterator to an array for easy sorting
      var averageSize = 0.0;
      var maxSize = 0.0;
      foreach (var node in nodes) {
        averageSize += node.Bounds.Width + node.Bounds.Height;
        if (node.Bounds.Width > maxSize) {
          maxSize = node.Bounds.Width;
        } else if (node.Bounds.Height > maxSize) {
          maxSize = node.Bounds.Height;
        }
      }
      averageSize /= (nodes.Count * 2);
      if (averageSize < 1) {
        averageSize = 1;
      }

      if (SortMode != VSortMode.None) {
        if (Comparer == null) {
          var sortOrder = SortOrder;
          var sortMode = SortMode;
          Comparer = (a, b) => {
            var sortVal = sortOrder == VSortOrder.Ascending ? 1 : -1;
            if (sortMode == VSortMode.MaxSide) {
              var aMax = Math.Max(a.Bounds.Width, a.Bounds.Height);
              var bMax = Math.Max(b.Bounds.Width, b.Bounds.Height);
              if (aMax > bMax) {
                return sortVal;
              } else if (bMax > aMax) {
                return -sortVal;
              }
              return 0;
            } else if (sortMode == VSortMode.Area) {
              var area1 = a.Bounds.Width * a.Bounds.Height;
              var area2 = b.Bounds.Width * b.Bounds.Height;
              if (area1 > area2) {
                return sortVal;
              } else if (area2 > area1) {
                return -sortVal;
              }
              return 0;
            }
            return 0;
          };
        }
        nodes.Sort(Comparer);
      }

      var targetWidth = Size.Width != 0 ? Size.Width : 1;
      var targetHeight = Size.Height != 0 ? Size.Height : 1;
      if (_FillViewport && Diagram != null) {
        targetWidth = Diagram.ViewportBounds.Width != 0 ? Diagram.ViewportBounds.Width : 1;
        targetHeight = Diagram.ViewportBounds.Height != 0 ? Diagram.ViewportBounds.Height : 1;
      } else if (_FillViewport) {
        targetWidth = _DefaultSize.Width != 0 ? _DefaultSize.Width : 1;
        targetHeight = _DefaultSize.Height != 0 ? _DefaultSize.Height : 1;
      }

      // set the target aspect ratio using the given bounds if necessary
      if (PackMode == VPackMode.Fit || PackMode == VPackMode.ExpandToFit) {
        _EAspectRatio = targetWidth / targetHeight;
      } else {
        _EAspectRatio = AspectRatio;
      }
      var fits = HasCircularNodes || PackShape == VPackShape.Spiral ? FitCircles(nodes) : FitRects(nodes);
      // in the Fit and ExpandToFit modes, we need to run the packing another time to figure out what the correct
      // _FixedModeSpacing should be. Then the layout is run a final time with the correct spacing.
      if (PackMode == VPackMode.Fit || PackMode == VPackMode.ExpandToFit) {
        var bounds0 = new Rect(_Bounds.X, _Bounds.Y, _Bounds.Width, _Bounds.Height);
        _Bounds = new Rect();
        _FixedSizeModeSpacing = Math.Floor(averageSize);
        fits = HasCircularNodes || PackShape == VPackShape.Spiral ? FitCircles(nodes) : FitRects(nodes);

        if ((HasCircularNodes || PackShape == VPackShape.Spiral) && PackShape == VPackShape.Spiral) {
          var targetDiameter = Math.Max(targetWidth, targetHeight);
          var oldDiameter = targetDiameter == targetWidth ? bounds0.Width : bounds0.Height;
          var newDiameter = targetDiameter == targetWidth ? _Bounds.Width : _Bounds.Height;

          var diff = (newDiameter - oldDiameter) / _FixedSizeModeSpacing;

          _FixedSizeModeSpacing = (targetDiameter - oldDiameter) / diff;
        } else {
          var dx = (_Bounds.Width - bounds0.Width) / _FixedSizeModeSpacing;
          var dy = (_Bounds.Height - bounds0.Height) / _FixedSizeModeSpacing;
          var paddingX = (targetWidth - bounds0.Width) / dx;
          var paddingY = (targetHeight - bounds0.Height) / dy;

          _FixedSizeModeSpacing = Math.Abs(paddingX) > Math.Abs(paddingY) ? paddingX : paddingY;
        }


        if (PackMode == VPackMode.Fit) {
          // make sure that the spacing is not positive in this mode
          _FixedSizeModeSpacing = Math.Min(_FixedSizeModeSpacing, 0);
        }
        if (_FixedSizeModeSpacing == double.PositiveInfinity) {
          _FixedSizeModeSpacing = -maxSize;
        }

        _Bounds = new Rect();
        fits = HasCircularNodes || PackShape == VPackShape.Spiral ? FitCircles(nodes) : FitRects(nodes);
      }
      // move the nodes and calculate the ActualBounds property
      if (ArrangesToOrigin) {
        _ActualBounds = new Rect(ArrangementOrigin.X, ArrangementOrigin.Y, 0, 0);
      }
      var nodeBounds = new List<Rect>(nodes.Count);
      for (var i = 0; i < fits.Count; i++) {
        var fit = fits[i];
        var node = nodes[i];
        if (ArrangesToOrigin) {
          // translate coordinates to respect ArrangementOrigin
          // ArrangementOrigin should be the top left corner of the bounding box around the layout
          fit.X = fit.X - _Bounds.X + ArrangementOrigin.X;
          fit.Y = fit.Y - _Bounds.Y + ArrangementOrigin.Y;
        }
        MoveNode(node, fit.X, fit.Y);
        nodeBounds.Add(node.Bounds);
        _ActualBounds = _ActualBounds.Union(nodeBounds[i]);
      }
      _NodeBounds = nodeBounds; // save node bounds in case we want to calculate the smallest enclosing circle later

      // can be overriden to change layout behavior, doesn't do anything by default
      CommitLayout();

      if (diagram != null) diagram.CommitTransaction("Layout");

      IsValidLayout = true;
    }

    /// <summary>
    /// Cause the vertex to be moved so that its position is at (nx,ny).
    /// The default implementation assumes the node.Bounds is a Rect that may be modified.
    /// </summary>
    public virtual void MoveNode(IBounded node, double nx, double ny) {
      node.Bounds = new Rect(nx, ny);
    }

    /// <summary>
    /// This method is called at the end of <see cref="PerformLayout"/>, but
    /// before the layout transaction is committed. It can be overriden and
    /// used to customize layout behavior. By default, the method does nothing.
    /// </summary>
    public virtual void CommitLayout() { }

    /// <summary>
    /// Runs a circle packing algorithm on the given array of nodes. The
    /// algorithm used is a slightly modified version of the one proposed
    /// by Wang et al. in "Visualization of large hierarchical data by
    /// circle packing", 2006.
    /// </summary>
    /// <param name="nodes">the array of Nodes to pack</param>
    /// <returns>an array of positioned rectangles corresponding to the nodes argument</returns>
    private List<Rect> FitCircles(List<IBounded> nodes) {
      Rect place(Rect a, Rect b, Rect c) {
        var ax = a.CenterX;
        var ay = a.CenterY;
        var da = (b.Width + c.Width) / 2;
        var db = (a.Width + c.Width) / 2;
        var dx = b.CenterX - ax;
        var dy = b.CenterY - ay;
        var dc = dx * dx + dy * dy;
        if (dc != 0) {
          var x = 0.5 + ((db *= db) - (da *= da)) / (2 * dc);
          var y = Math.Sqrt(Math.Max(0, 2 * da * (db + dc) - (db -= dc) * db - da * da)) / (2 * dc);
          c.X = (ax + x * dx + y * dy) - (c.Width / 2);
          c.Y = (ay + x * dy - y * dx) - (c.Height / 2);
        } else {
          c.X = ax + db;
          c.Y = ay;
        }
        return c;
      }

      bool intersects(Rect a, Rect b) {
        var ar = a.Height / 2;
        var br = b.Height / 2;
        var dist = Math.Sqrt(a.Center.DistanceSquared(b.Center));
        var difference = dist - (ar + br);
        return difference < -0.0000001;
      }

      var aspect = _EAspectRatio;
      var shape = PackShape;
      double score(ListNode<Rect> n) {
        var a = n.Data;
        var b = n.Next.Data;
        var ar = a.Width / 2;
        var br = b.Width / 2;
        var ab = ar + br;
        var dx = (a.CenterX * br + b.CenterX * ar) / ab;
        var dy = (a.CenterY * br + b.CenterY * ar) / ab * aspect;
        return shape == VPackShape.Elliptical ? dx * dx + dy * dy : Math.Max(dx * dx, dy * dy);
      }


      var sideSpacing = (Spacing + _FixedSizeModeSpacing) / 2;
      var fits = new List<Rect>();
      var frontChain = new CircularDoublyLinkedList<Rect>();

      if (nodes.Count == 0) return fits;

      if (nodes[0].Bounds == default) Trace.Warn("warning: a node in VirtualizedPackedLayout was specified without explicit bounds.");
      var n1 = new ListNode<Rect>(nodes[0].Bounds.Inflate(sideSpacing, sideSpacing));
      n1.Data.X = 0;
      n1.Data.Y = 0;
      n1.Data.Width = n1.Data.Width == 0 ? 0.1 : n1.Data.Width;
      n1.Data.Height = n1.Data.Height == 0 ? 0.1 : n1.Data.Height;
      fits.Add(n1.Data);
      _Bounds = _Bounds.Union(n1.Data);
      if (nodes.Count < 2) return fits;

      if (nodes[1].Bounds == default) Trace.Warn("warning: a node in VirtualizedPackedLayout was specified without explicit bounds.");
      var n2 = new ListNode<Rect>(nodes[1].Bounds.Inflate(sideSpacing, sideSpacing));
      n2.Data.X = 0;
      n2.Data.Y = 0;
      n2.Data.Width = n2.Data.Width == 0 ? 0.1 : n2.Data.Width;
      n2.Data.Height = n2.Data.Height == 0 ? 0.1 : n2.Data.Height;
      n2.Data.X = -n2.Data.Width;
      n2.Data.Y = n1.Data.CenterY - n2.Data.Width / 2;
      fits.Add(n2.Data);
      _Bounds = _Bounds.Union(n2.Data);
      if (nodes.Count < 3) return fits;

      if (nodes[2].Bounds == default) Trace.Warn("warning: a node in VirtualizedPackedLayout was specified without explicit bounds.");
      var n3 = new ListNode<Rect>(nodes[2].Bounds.Inflate(sideSpacing, sideSpacing));
      n3.Data.X = 0;
      n3.Data.Y = 0;
      n3.Data.Width = n3.Data.Width == 0 ? 0.1 : n3.Data.Width;
      n3.Data.Height = n3.Data.Height == 0 ? 0.1 : n3.Data.Height;
      n3.Data = place(n2.Data, n1.Data, n3.Data);
      _Bounds = _Bounds.Union(n3.Data);

      n2 = frontChain.Push(n2.Data);
      n3 = frontChain.Push(n3.Data);
      n1 = frontChain.Push(n1.Data);

      for (var i = 3; i < nodes.Count; i++) {
        if (nodes[i].Bounds == default) Trace.Warn("warning: a node in VirtualizedPackedLayout was specified without explicit bounds.");
        n3 = new ListNode<Rect>(nodes[i].Bounds.Inflate(sideSpacing, sideSpacing));
        n3.Data.X = 0;
        n3.Data.Y = 0;
        n3.Data.Width = n3.Data.Width == 0 ? 0.1 : n3.Data.Width;
        n3.Data.Height = n3.Data.Height == 0 ? 0.1 : n3.Data.Height;
        n3.Data = place(n1.Data, n2.Data, n3.Data);

        var j = n2.Next;
        var k = n1.Prev;
        var sj = n2.Data.Width / 2;
        var sk = n1.Data.Width / 2;
        var skip = false;
        do {
          if (sj <= sk) {
            if (intersects(j.Data, n3.Data)) {
              n2 = frontChain.RemoveBetween(n1, j); i--;
              skip = true; break;
            }
            sj += j.Data.Width / 2; j = j.Next;
          } else {
            if (intersects(k.Data, n3.Data)) {
              frontChain.RemoveBetween(k, n2);
              n1 = k; i--;
              skip = true; break;
            }
            sk += k.Data.Width / 2; k = k.Prev;
          }
        } while (j != k.Next);

        if (skip) continue;

        fits.Add(n3.Data);
        _Bounds = _Bounds.Union(n3.Data);

        n3 = frontChain.InsertAfter(n3.Data, n1);
        n2 = n3;

        if (PackShape != VPackShape.Spiral) {
          var aa = score(n1);
          while ((n3 = n3.Next) != n2) {
            var ca = score(n3);
            if (ca < aa) {
              n1 = n3; aa = ca;
            }
          }
          n2 = n1.Next;
        }

      }

      return fits;
    }

    /// <summary>
    /// Runs a rectangle packing algorithm on the given array of nodes.
    /// </summary>
    /// <remarks>
    /// The algorithm presented is original, and operates by maintaining
    /// a representation (with segments) of the perimeter of the already
    /// packed shape. The perimeter of segments is stored in both a linked
    /// list (for ordered iteration) and a quadtree (for fast intersection
    /// detection). Similar to the circle packing algorithm presented
    /// above, this is a greedy algorithm.
    ///
    /// For each node, a large list of possible placements is created,
    /// each one relative to a segment on the perimeter. These placements
    /// are sorted according to a cost function, and then the lowest cost
    /// placement with no intersections is picked. The perimeter
    /// representation is then updated according to the new placement.
    ///
    /// However, in addition to placements made relative to a single segment
    /// on the perimeter, the algorithm also attempts to make placements
    /// between two nonsequential segments ("skip fits"), closing gaps in the
    /// packed shape. If a placement made in this way has no intersections
    /// and a lower cost than any of the original placements, it is picked
    /// instead. This step occurs simultaneously to checking intersections on
    /// the original placement list.
    ///
    /// Intersections for new placements are checked only against the current
    /// perimeter of segments, rather than the entire packed shape.
    /// Additionally, before the quadtree is queried at all, a few closely
    /// surrounding segments to the placement are checked in case an
    /// intersection can be found more quickly. The combination of these two
    /// strategies enables intersection checking to take place extremely
    /// quickly, when it would normally be the slowest part of the entire
    /// algorithm.
    /// </remarks>
    /// <param name="nodes">the array of Nodes to pack</param>
    /// <returns>an array of positioned rectangles corresponding to the nodes argument</returns>
    private List<Rect> FitRects(List<IBounded> nodes) {
      var sideSpacing = (Spacing + _FixedSizeModeSpacing) / 2;
      var fits = new List<Rect>();
      var segments = new CircularDoublyLinkedList<Segment>();

      // reset layout state
      _Tree.Clear();
      _MinXSegment = null;
      _MaxXSegment = null;
      _MinYSegment = null;
      _MaxYSegment = null;

      if (nodes.Count < 1) {
        return fits;
      }

      // place first node at 0, 0
      var bounds0 = nodes[0].Bounds;
      if (bounds0 == default) Trace.Warn("warning: a node in VirtualizedPackedLayout was specified without explicit bounds.");
      fits.Add(new Rect(sideSpacing, sideSpacing, bounds0.Width, bounds0.Height));
      fits[0] = fits[0].Inflate(sideSpacing, sideSpacing);
      fits[0] = new Rect(0, 0, fits[0].X == 0 ? 0.1 : fits[0].X, fits[0].Y == 0 ? 0.1 : fits[0].Y);
      _Bounds = _Bounds.Union(fits[0]);
      _Center = fits[0].Center;

      var s1 = new Segment(0, 0, fits[0].Width, 0, false);
      var s2 = new Segment(fits[0].Width, 0, fits[0].Width, fits[0].Height, false);
      var s3 = new Segment(fits[0].Width, fits[0].Height, 0, fits[0].Height, false);
      var s4 = new Segment(0, fits[0].Height, 0, 0, false);
      _Tree.Add(s1, s1.Bounds);
      _Tree.Add(s2, s2.Bounds);
      _Tree.Add(s3, s3.Bounds);
      _Tree.Add(s4, s4.Bounds);
      segments.Push(s1, s2, s3, s4);
      FixMissingMinMaxSegments(true);

      for (var i = 1; i < nodes.Count; i++) {
        var node = nodes[i];
        if (node.Bounds == default) Trace.Warn("warning: a node in VirtualizedPackedLayout was specified without explicit bounds.");
        var bounds = node.Bounds.Inflate(sideSpacing, sideSpacing);
        bounds.X = 0;
        bounds.Y = 0;
        bounds.Width = bounds.Width == 0 ? 0.1 : bounds.Width;

        var possibleFits = new List<Fit>(segments.Length);
        var j = 0;
        var s = segments.Start as ListNode<Segment>;
        do {

          // make sure segment is perfectly straight (fixing some floating point error)
          var sdata = s.Data;
          sdata.X1 = s.Prev.Data.X2;
          sdata.Y1 = s.Prev.Data.Y2;
          if (sdata.IsHorizontal) {
            sdata.Y2 = sdata.Y1;
          } else {
            sdata.X2 = sdata.X1;
          }

          var fitBounds = GetBestFitRect(s, bounds.Width, bounds.Height);
          var cost = PlacementCost(fitBounds);
          possibleFits[j] = new Fit(fitBounds, cost, s);

          s = s.Next;
          j++;
        } while (s != segments.Start);

        possibleFits.Sort((a, b) => {
          return (int)(a.Cost - b.Cost);
        });

        /* scales the cost of skip fits. a number below
         * one makes skip fits more likely to appear,
         * which is preferable because they are more
         * aesthetically pleasing and reduce the total
         * double of segments.
         */
        var skipFitScaleFactor = 0.98;

        Fit bestFit = null;
        var onlyCheckSkipFits = false;
        foreach (var fit in possibleFits) {
          if (bestFit != null && bestFit.Cost <= fit.Cost) {
            onlyCheckSkipFits = true;
          }

          var hasIntersections = true; // set initially to true to make skip fit checking work when onlyCheckSkipFits = true
          if (!onlyCheckSkipFits) {
            hasIntersections = FastFitHasIntersections(fit) || FitHasIntersections(fit);
            if (!hasIntersections) {
              bestFit = fit;
              continue;
            }
          }

          // check skip fits
          if (hasIntersections && !fit.S1.Data.P1Concave && (fit.S1.Next.Data.P1Concave || fit.S1.Next.Next.Data.P1Concave)) {
            (var nextSegment, var usePreviousSegment) = FindNextOrientedSegment(fit, fit.S1.Next);
            var nextSegmentTouchesFit = false;
            while (hasIntersections && nextSegment != null) {
              fit.Bounds = RectAgainstMultiSegment(fit.S1, nextSegment, bounds.Width, bounds.Height);
              hasIntersections = FastFitHasIntersections(fit) || FitHasIntersections(fit);
              nextSegmentTouchesFit = SegmentIsOnFitPerimeter(nextSegment.Data, fit.Bounds);

              if (hasIntersections || !nextSegmentTouchesFit) {
                (nextSegment, usePreviousSegment) = FindNextOrientedSegment(fit, nextSegment);
              }
            }

            if (!hasIntersections && nextSegment != null && nextSegmentTouchesFit) {
              fit.Cost = PlacementCost(fit.Bounds) * skipFitScaleFactor;
              if (bestFit == null || fit.Cost <= bestFit.Cost) {
                bestFit = fit;
                bestFit.S2 = nextSegment;
                if (usePreviousSegment) {
                  bestFit.S1 = bestFit.S1.Prev;
                }
              }
            }
          }
        }

        if (bestFit != null) {
          UpdateSegments(bestFit, segments);

          fits.Add(bestFit.Bounds);
          _Bounds = _Bounds.Union(bestFit.Bounds);
        }

      }

      // save segments in case we want to calculate the enclosing circle later
      _Segments = segments;

      return fits;
    }

    /// <summary>
    /// Attempts to find a segment which can be used to create a new skip fit
    /// between fit.S1 and the found segment. A number of conditions are checked
    /// before returning a segment, ensuring that if the skip fit *does* intersect
    /// with the already packed shape, it will do so along the perimeter (so that it
    /// can be detected with only knowledge about the perimeter). Multiple oriented
    /// segments can be found for a given fit, so this function starts searching at
    /// the segment after the given lastSegment parameter.
    ///
    /// Oriented segments can be oriented with either fit.S1, or fit.S1.Prev. The
    /// second return value (usePreviousSegment) indicates which the found segment is.
    /// </summary>
    /// <param name="fit">the fit to search for a new segment for</param>
    /// <param name="lastSegment">the last segment found.</param>
    private (ListNode<Segment>, bool) FindNextOrientedSegment(Fit fit, ListNode<Segment> lastSegment) {
      lastSegment = lastSegment.Next;
      var orientation = SegmentOrientation(fit.S1.Prev.Data, fit.S1.Data);
      var targetOrientation = (Orientation)((int)(orientation + 1) % 4);

      while (!SegmentIsMinOrMax(lastSegment.Data)) {
        var usePreviousSegment = lastSegment.Data.IsHorizontal == fit.S1.Data.IsHorizontal;

        Orientation lastOrientation;
        if (usePreviousSegment) {
          lastOrientation = SegmentOrientation(lastSegment.Data, lastSegment.Next.Data);
          if (lastSegment.Next.Data.P1Concave) {
            lastOrientation = (Orientation)((int)(lastOrientation + 1) % 4);
          }
        } else {
          lastOrientation = SegmentOrientation(lastSegment.Prev.Data, lastSegment.Data);
          if (lastSegment.Data.P1Concave) {
            lastOrientation = (Orientation)((int)(lastOrientation + 1) % 4);
          }
        }
        var validLastOrientation = lastOrientation == targetOrientation;

        var exceededPrimaryDimension = fit.S1.Data.IsHorizontal ?
          Math.Abs(lastSegment.Data.Y1 - fit.S1.Data.Y1) + 1e-7 > fit.Bounds.Height :
          Math.Abs(lastSegment.Data.X1 - fit.S1.Data.X1) + 1e-7 > fit.Bounds.Width;

        bool validCornerPlacement;
        bool exceededSecondaryDimension;
        switch (orientation) {
          case Orientation.NE:
            validCornerPlacement = fit.S1.Data.X1 < lastSegment.Data.X1;
            exceededSecondaryDimension = usePreviousSegment ? fit.S1.Data.Y1 - fit.Bounds.Height >= lastSegment.Data.Y1 : fit.S1.Data.Y2 + fit.Bounds.Height <= lastSegment.Data.Y1;
            break;
          case Orientation.NW:
            validCornerPlacement = fit.S1.Data.Y1 > lastSegment.Data.Y1;
            exceededSecondaryDimension = usePreviousSegment ? fit.S1.Data.X1 - fit.Bounds.Width >= lastSegment.Data.X1 : fit.S1.Data.X2 + fit.Bounds.Width <= lastSegment.Data.X1;
            break;
          case Orientation.SW:
            validCornerPlacement = fit.S1.Data.X1 > lastSegment.Data.X1;
            exceededSecondaryDimension = usePreviousSegment ? fit.S1.Data.Y1 + fit.Bounds.Height <= lastSegment.Data.Y1 : fit.S1.Data.Y2 - fit.Bounds.Height >= lastSegment.Data.Y1;
            break;
          case Orientation.SE:
            validCornerPlacement = fit.S1.Data.Y1 < lastSegment.Data.Y1;
            exceededSecondaryDimension = usePreviousSegment ? fit.S1.Data.X1 + fit.Bounds.Width <= lastSegment.Data.X1 : fit.S1.Data.X2 - fit.Bounds.Width >= lastSegment.Data.X1;
            break;
          default:
            throw new Exception("Unknown orientation " + orientation);
        }

        if (!exceededPrimaryDimension && !exceededSecondaryDimension && validCornerPlacement && validLastOrientation) {
          return (lastSegment, usePreviousSegment);
        }

        lastSegment = lastSegment.Next;
      }

      return (null, false);
    }

    /// <summary>
    /// Returns the orientation of two adjacent segments. s2
    /// is assumed to start at the end of s1.
    /// </summary>
    /// <param name="s1">the first segment</param>
    /// <param name="s2">the second segment</param>
    private static Orientation SegmentOrientation(Segment s1, Segment s2) {
      if (s1.IsHorizontal) {
        if (s1.X1 < s2.X1) {
          return s2.P1Concave ? Orientation.SE : Orientation.NE;
        } else {
          return s2.P1Concave ? Orientation.NW : Orientation.SW;
        }
      } else {
        if (s1.Y1 < s2.Y1) {
          return s2.P1Concave ? Orientation.SW : Orientation.SE;
        } else {
          return s2.P1Concave ? Orientation.NE : Orientation.NW;
        }
      }
    }

    /// <summary>
    /// Fits a rectangle between two segments (used for skip fits). This is an operation
    /// related more to corners than segments, so fit.S1 should always be supplied for
    /// segment a (even if usePreviousSegment was true in the return value for
    /// <see cref="FindNextOrientedSegment"/>).
    /// </summary>
    /// <param name="a">the first segment to fit between, should always be fit.S1</param>
    /// <param name="b"></param> the second segment to fit between, found with <see cref="FindNextOrientedSegment"/>
    /// <param name="width">the width of the rectangle, should be fit.Width</param>
    /// <param name="height">the height of the rectangle, should be fit.Height</param>
    private static Rect RectAgainstMultiSegment(ListNode<Segment> a, ListNode<Segment> b, double width, double height) {
      switch (SegmentOrientation(a.Prev.Data, a.Data)) {
        case Orientation.NE:
          if (a.Data.Y1 > b.Data.Y2) {
            return new Rect(b.Data.X1 - width, a.Data.Y1 - height, width, height);
          } else {
            return new Rect(a.Data.X1, b.Data.Y1 - height, width, height);
          }
        case Orientation.NW:
          if (a.Data.X1 > b.Data.X2) {
            return new Rect(a.Data.X1 - width, b.Data.Y1, width, height);
          } else {
            return new Rect(b.Data.X1 - width, a.Data.Y1 - height, width, height);
          }
        case Orientation.SW:
          if (a.Data.Y1 < b.Data.Y2) {
            return new Rect(b.Data.X1, a.Data.Y1, width, height);
          } else {
            return new Rect(a.Data.X1 - width, b.Data.Y1, width, height);
          }
        case Orientation.SE:
          if (a.Data.X1 < b.Data.X2) {
            return new Rect(a.Data.X1, b.Data.Y1 - height, width, height);
          } else {
            return new Rect(b.Data.X1, a.Data.Y1, width, height);
          }
        default: return default;
      }
    }

    /// <summary>
    /// Gets the rectangle placed against the given segment with the lowest
    /// placement cost. Rectangles can be placed against a segment either at
    /// the top/left side, the bottom/right side, or at the center coordinate
    /// of the entire packed shape (if the segment goes through either the x
    /// or y coordinate of the center).
    /// </summary>
    /// <param name="s">the segment to place against</param>
    /// <param name="width">the width of the fit, fit.Width</param>
    /// <param name="height">the height of the fit, fit.Height</param>
    private Rect GetBestFitRect(ListNode<Segment> s, double width, double height) {
      var x1 = s.Data.X1;
      var y1 = s.Data.Y1;
      var x2 = s.Data.X2;
      var y2 = s.Data.Y2;
      var dir = SegmentOrientation(s.Prev.Data, s.Data);
      if (s.Data.P1Concave) {
        dir = (Orientation)((int)(dir + 3) % 4);
      }

      var coordIsX = dir == Orientation.NW || dir == Orientation.SE;
      if (dir == Orientation.NE) {
        y2 -= height;
      } else if (dir == Orientation.SE) {
        x1 -= width;
      } else if (dir == Orientation.SW) {
        x1 -= width;
        y1 -= height;
        x2 -= width;
      } else if (dir == Orientation.NW) {
        y1 -= height;
        x2 -= width;
        y2 -= height;
      }

      var r = new Rect(x1, y1, width, height);
      var cost1 = PlacementCost(r);
      r = new Rect(x2, y2, width, height);
      var cost2 = PlacementCost(r);
      var cost3 = double.PositiveInfinity;
      if (coordIsX && (_Center.X - (x1 + width / 2)) * (_Center.X - (x2 + width / 2)) < 0) {
        r = new Rect(_Center.X - width / 2, y1, width, height);
        cost3 = PlacementCost(r);
      } else if (!coordIsX && (_Center.Y - (y1 + height / 2)) * (_Center.Y - (y2 + height / 2)) < 0) {
        r = new Rect(x1, _Center.Y - height / 2, width, height);
        cost3 = PlacementCost(r);
      }

      return cost3 < cost2 && cost3 < cost1 ? r
       : (cost2 < cost1 ? new Rect(x2, y2, width, height)
        : new Rect(x1, y1, width, height));

    }

    /// <summary>
    /// Checks if a segment is on the perimeter of the given fit bounds.
    /// Also returns true if the segment is within the rect, but that
    /// shouldn't matter for any of the cases where this function is used.
    /// </summary>
    /// <param name="s">the segment to test</param>
    /// <param name="bounds">the fit bounds</param>
    private static bool SegmentIsOnFitPerimeter(Segment s, Rect bounds) {
      var xCoordinatesTogether = NumberIsBetween(s.X1, bounds.Left, bounds.Right)
        || NumberIsBetween(s.X2, bounds.Left, bounds.Right)
        || NumberIsBetween(bounds.Left, s.X1, s.X2)
        || NumberIsBetween(bounds.Right, s.X1, s.X2);
      var yCoordinatesTogether = NumberIsBetween(s.Y1, bounds.Top, bounds.Bottom)
        || NumberIsBetween(s.Y2, bounds.Top, bounds.Bottom)
        || NumberIsBetween(bounds.Top, s.Y1, s.Y2)
        || NumberIsBetween(bounds.Bottom, s.Y1, s.Y2);
      return (s.IsHorizontal && (ApproxEqual(s.Y1, bounds.Top) || ApproxEqual(s.Y1, bounds.Bottom)) && xCoordinatesTogether)
        || (!s.IsHorizontal && (ApproxEqual(s.X1, bounds.Left) || ApproxEqual(s.X1, bounds.Right)) && yCoordinatesTogether);
    }

    /// <summary>
    /// Checks if a point is on the perimeter of the given fit bounds.
    /// Also returns true if the point is within the rect, but that
    /// shouldn't matter for any of the cases where this function is used.
    /// </summary>
    /// <param name="x">the x coordinate of the point to test</param>
    /// <param name="y">the y coordinate of the point to test</param>
    /// <param name="bounds">the fit bounds</param>
    private static bool PointIsOnFitPerimeter(double x, double y, Rect bounds) {
      return (x >= bounds.Left - 1e-7 && x <= bounds.Right + 1e-7 && y >= bounds.Top - 1e-7 && y <= bounds.Bottom + 1e-7);
    }

    /// <summary>
    /// Checks if a point is on the corner of the given fit bounds.
    /// </summary>
    /// <param name="x">the x coordinate of the point to test</param>
    /// <param name="y">the y coordinate of the point to test</param>
    /// <param name="bounds">the fit bounds</param>
    private static bool PointIsFitCorner(double x, double y, Rect bounds) {
      return (ApproxEqual(x, bounds.Left) && ApproxEqual(y, bounds.Top)) ||
             (ApproxEqual(x, bounds.Right) && ApproxEqual(y, bounds.Top)) ||
             (ApproxEqual(x, bounds.Left) && ApproxEqual(y, bounds.Bottom)) ||
             (ApproxEqual(x, bounds.Right) && ApproxEqual(y, bounds.Bottom));
    }

    /// <summary>
    /// Updates the representation of the perimeter of segments after
    /// a new placement is made. This modifies the given segments list,
    /// as well as the quadtree class variable <see cref="_Tree"/>.
    /// Also updates the minimum/maximum segments if they have changed as
    /// a result of the new placement.
    /// </summary>
    /// <param name="fit">the fit to add</param>
    /// <param name="segments">the list of segments to update</param>
    private void UpdateSegments(Fit fit, CircularDoublyLinkedList<Segment> segments) {
      var s0 = fit.S1;
      while (PointIsOnFitPerimeter(s0.Data.X1, s0.Data.Y1, fit.Bounds)) {
        s0 = s0.Prev;
      }
      if (!SegmentIsMinOrMax(s0.Data) || !SegmentIsMinOrMax(s0.Prev.Data)) {
        var sTest = s0.Prev.Prev; // test for conflicting segments
        ListNode<Segment> sFound = null;
        var minMaxCount = 0;
        while (minMaxCount < 2) {
          if (SegmentIsOnFitPerimeter(sTest.Data, fit.Bounds)) {
            sFound = sTest;
          }
          sTest = sTest.Prev;
          if (SegmentIsMinOrMax(sTest.Next.Data)) {
            minMaxCount++;
          }
        }
        if (sFound != null) {
          while (PointIsOnFitPerimeter(sFound.Data.X1, sFound.Data.Y1, fit.Bounds)) {
            sFound = sFound.Prev;
          }
          RemoveBetween(segments, sFound, s0);
          s0 = sFound;
        }
      }

      Orientation nextConvexCornerOrientation;
      Orientation lastConvexCornerOrientation;

      var testOrientation = SegmentOrientation(s0.Prev.Data, s0.Data);
      if (s0.Data.P1Concave) {
        testOrientation = (Orientation)((int)(testOrientation + 3) % 4);
      }
      (var cornerX, var cornerY) = CornerFromRect(testOrientation, fit.Bounds);
      var extended0 = ApproxEqual(cornerX, s0.Data.X2) && ApproxEqual(cornerY, s0.Data.Y2);
      bool shortened0Precond;
      (var cornerX2, var cornerY2) = CornerFromRect((Orientation)((int)(testOrientation + 1) % 4), fit.Bounds);
      if (s0.Data.IsHorizontal) {
        shortened0Precond = NumberIsBetween(cornerX2, s0.Data.X1, s0.Data.X2) && ApproxEqual(cornerY2, s0.Data.Y1);
      } else {
        shortened0Precond = NumberIsBetween(cornerY2, s0.Data.Y1, s0.Data.Y2) && ApproxEqual(cornerX2, s0.Data.X1);
      }
      var shortened0 = !extended0 && PointIsFitCorner(s0.Data.X2, s0.Data.Y2, fit.Bounds)
                          || !PointIsOnFitPerimeter(s0.Data.X2, s0.Data.Y2, fit.Bounds)
                          || (PointIsOnFitPerimeter(s0.Data.X2, s0.Data.Y2, fit.Bounds)
                          && !PointIsOnFitPerimeter(s0.Data.X1, s0.Data.Y1, fit.Bounds)
                          && shortened0Precond);
      if (extended0) {
        // extend s0
        (s0.Data.X2, s0.Data.Y2) = CornerFromRect((Orientation)((int)(testOrientation + 3) % 4), fit.Bounds);
        _Tree.SetTo(s0.Data, Segment.RectFromSegment(s0.Data));
        nextConvexCornerOrientation = (Orientation)((int)(testOrientation + 3) % 4);
        UpdateMinMaxSegments(s0.Data);
      } else {
        if (shortened0) {
          (s0.Data.X2, s0.Data.Y2) = CornerFromRect((Orientation)((int)(testOrientation + 1) % 4), fit.Bounds);
          _Tree.SetTo(s0.Data, Segment.RectFromSegment(s0.Data));
        }
        var newSegment = new Segment(s0.Data.X2, s0.Data.Y2, cornerX, cornerY, true);
        s0 = segments.InsertAfter(newSegment, s0);
        _Tree.Add(newSegment, newSegment.Bounds);
        nextConvexCornerOrientation = testOrientation;
        UpdateMinMaxSegments(newSegment);
      }

      var sNext = fit.S2 ?? s0;
      while (PointIsOnFitPerimeter(sNext.Data.X2, sNext.Data.Y2, fit.Bounds)) {
        sNext = sNext.Next;
      }
      if (!SegmentIsMinOrMax(sNext.Data) || !SegmentIsMinOrMax(sNext.Next.Data)) {
        var sTest = sNext.Next.Next; // test for conflicting segments
        ListNode<Segment> sFound = null;
        var minMaxCount = 0;
        while (minMaxCount < 2) {
          if (SegmentIsOnFitPerimeter(sTest.Data, fit.Bounds)) {
            sFound = sTest;
          }
          sTest = sTest.Next;
          if (SegmentIsMinOrMax(sTest.Prev.Data)) {
            minMaxCount++;
          }
        }
        if (sFound != null) {
          sNext = sFound;
          while (PointIsOnFitPerimeter(sNext.Data.X2, sNext.Data.Y2, fit.Bounds)) {
            sNext = sNext.Next;
          }
        }
      }

      testOrientation = SegmentOrientation(sNext.Data, sNext.Next.Data);
      if (sNext.Data.P1Concave) {
        testOrientation = (Orientation)((int)(testOrientation + 2) % 4);
      }
      if (sNext.Next.Data.P1Concave) {
        testOrientation = (Orientation)((int)(testOrientation + 1) % 4);
      }
      (cornerX2, cornerY2) = CornerFromRect((Orientation)((int)(testOrientation + 3) % 4), fit.Bounds);
      if (sNext.Data.IsHorizontal && NumberIsBetween(cornerX2, sNext.Data.X1, sNext.Data.X2) && ApproxEqual(cornerY2, sNext.Data.Y1)
      || (!sNext.Data.IsHorizontal && NumberIsBetween(cornerY2, sNext.Data.Y1, sNext.Data.Y2) && ApproxEqual(cornerX2, sNext.Data.X1))
      || (sNext.Data.IsHorizontal && NumberIsBetween(fit.Bounds.Left, sNext.Data.X1, sNext.Data.X2) && NumberIsBetween(fit.Bounds.Right, sNext.Data.X1, sNext.Data.X2)
          && (ApproxEqual(fit.Bounds.Top, sNext.Data.Y1) || ApproxEqual(fit.Bounds.Bottom, sNext.Data.Y1)))
      || (!sNext.Data.IsHorizontal && NumberIsBetween(fit.Bounds.Top, sNext.Data.Y1, sNext.Data.Y2) && NumberIsBetween(fit.Bounds.Bottom, sNext.Data.Y1, sNext.Data.Y2)
          && (ApproxEqual(fit.Bounds.Left, sNext.Data.X1) || ApproxEqual(fit.Bounds.Right, sNext.Data.X1)))) {
        sNext = sNext.Next;
        testOrientation = SegmentOrientation(sNext.Data, sNext.Next.Data);
        if (sNext.Data.P1Concave) {
          testOrientation = (Orientation)((int)(testOrientation + 2) % 4);
        }
        if (sNext.Next.Data.P1Concave) {
          testOrientation = (Orientation)((int)(testOrientation + 1) % 4);
        }
      }

      RemoveBetween(segments, s0, sNext);

      (cornerX, cornerY) = CornerFromRect(testOrientation, fit.Bounds);

      if (ApproxEqual(cornerX, sNext.Data.X1) && ApproxEqual(cornerY, sNext.Data.Y1)) {
        // extend sNext
        if (s0.Data.IsHorizontal == sNext.Data.IsHorizontal && (s0.Data.IsHorizontal ? ApproxEqual(s0.Data.Y1, sNext.Data.Y1) : ApproxEqual(s0.Data.X1, sNext.Data.X1))) {
          s0.Data.X2 = sNext.Data.X2;
          s0.Data.Y2 = sNext.Data.Y2;
          RemoveSegmentFromLayoutState(sNext);
          segments.Remove(sNext);
          _Tree.SetTo(s0.Data, Segment.RectFromSegment(s0.Data));
          lastConvexCornerOrientation = nextConvexCornerOrientation; // no additional segments need to be added
          UpdateMinMaxSegments(s0.Data);
        } else {
          (sNext.Data.X1, sNext.Data.Y1) = CornerFromRect((Orientation)((int)(testOrientation + 1) % 4), fit.Bounds);
          _Tree.SetTo(sNext.Data, Segment.RectFromSegment(sNext.Data));
          lastConvexCornerOrientation = (Orientation)((int)(testOrientation + 1) % 4);
          UpdateMinMaxSegments(sNext.Data);
        }
      } else if (extended0 && (s0.Data.IsHorizontal ?
                              ApproxEqual(s0.Data.Y1, sNext.Data.Y1) && NumberIsBetween(sNext.Data.X1, s0.Data.X1, s0.Data.X2) :
                              ApproxEqual(s0.Data.X1, sNext.Data.X1) && NumberIsBetween(sNext.Data.Y1, s0.Data.Y1, s0.Data.Y2))) {
        if (s0.Data.IsHorizontal) {
          s0.Data.X2 = sNext.Data.X1;
        } else {
          s0.Data.Y2 = sNext.Data.Y1;
        }
        _Tree.SetTo(s0.Data, Segment.RectFromSegment(s0.Data));
        lastConvexCornerOrientation = nextConvexCornerOrientation;
        sNext.Data.P1Concave = true;
        UpdateMinMaxSegments(s0.Data);
      } else if (!PointIsFitCorner(sNext.Data.X1, sNext.Data.Y1, fit.Bounds)) {
        // add concave segment
        var newSegment = new Segment(cornerX, cornerY, sNext.Data.X1, sNext.Data.Y1, false);
        if (PointIsOnFitPerimeter(sNext.Data.X1, sNext.Data.Y1, fit.Bounds)) {
          sNext.Data.P1Concave = true;
        } else {
          newSegment.P1Concave = true;
        }
        if (ApproxEqual(sNext.Prev.Data.X1, cornerX) && ApproxEqual(sNext.Prev.Data.Y1, cornerY) && newSegment.IsHorizontal == sNext.Prev.Data.IsHorizontal) {
          sNext.Prev.Data.X2 = sNext.Data.X1;
          sNext.Prev.Data.Y2 = sNext.Data.Y1;
          _Tree.SetTo(sNext.Prev.Data, Segment.RectFromSegment(sNext.Prev.Data));
          lastConvexCornerOrientation = nextConvexCornerOrientation;
        } else {
          segments.InsertAfter(newSegment, sNext.Prev);
          _Tree.Add(newSegment, newSegment.Bounds);
          lastConvexCornerOrientation = testOrientation;
          UpdateMinMaxSegments(newSegment);
        }
      } else { // if (PointIsOnFitPerimeter(sNext.Data.X1, sNext.Data.Y1, fit.Bounds))
        // shorten existing segment
        (sNext.Data.X1, sNext.Data.Y1) = CornerFromRect((Orientation)((int)(testOrientation + 3) % 4), fit.Bounds);
        sNext.Data.P1Concave = true;
        _Tree.SetTo(sNext.Data, Segment.RectFromSegment(sNext.Data));
        lastConvexCornerOrientation = (Orientation)((int)(testOrientation + 3) % 4);
      }

      while (nextConvexCornerOrientation != lastConvexCornerOrientation) {
        (cornerX, cornerY) = CornerFromRect((Orientation)((int)(nextConvexCornerOrientation + 3) % 4), fit.Bounds);
        var newSegment = new Segment(s0.Data.X2, s0.Data.Y2, cornerX, cornerY, false);
        s0 = segments.InsertAfter(newSegment, s0);
        _Tree.Add(newSegment, newSegment.Bounds);
        nextConvexCornerOrientation = (Orientation)((int)(nextConvexCornerOrientation + 3) % 4);
        UpdateMinMaxSegments(newSegment);
      }

      FixMissingMinMaxSegments();
    }

    /// <summary>
    /// Finds the new minimum and maximum segments in the packed shape if
    /// any of them have been deleted. To do this quickly, the quadtree
    /// is used.
    /// </summary>
    /// <param name="force">whether or not to force an update based on the quadtree even if none of the segments were deleted</param>
    private void FixMissingMinMaxSegments(bool force = false) {
      if (_MinXSegment == null || _MaxXSegment == null || _MinYSegment == null || _MaxYSegment == null || force) {
        (_MinXSegment, _MaxXSegment, _MinYSegment, _MaxYSegment) = _Tree.FindExtremeObjects();
      }
    }

    /// <summary>
    /// Updates the minimum or maximum segments with a new segment if that
    /// segment is a new minimum or maximum.
    /// </summary>
    /// <param name="s">the new segment to test</param>
    private void UpdateMinMaxSegments(Segment s) {
      var centerX = (s.X1 + s.X2) / 2;
      var centerY = (s.Y1 + s.Y2) / 2;
      if (_MinXSegment != null && centerX < (_MinXSegment.X1 + _MinXSegment.X2) / 2) {
        _MinXSegment = s;
      }
      if (_MinYSegment != null && centerY < (_MinYSegment.Y1 + _MinYSegment.Y2) / 2) {
        _MinYSegment = s;
      }
      if (_MaxXSegment != null && centerX > (_MaxXSegment.X1 + _MaxXSegment.X2) / 2) {
        _MaxXSegment = s;
      }
      if (_MaxYSegment != null && centerY > (_MaxYSegment.Y1 + _MaxYSegment.Y2) / 2) {
        _MaxYSegment = s;
      }
    }

    /// <summary>
    /// Gets the x and y coordinates of a corner of a given rectangle.
    /// </summary>
    /// <param name="orientation">the orientation of the corner to get</param>
    /// <param name="bounds">the bounds of the rectangle to get the corner from</param>
    private static (double, double) CornerFromRect(Orientation orientation, Rect bounds) {
      var x = bounds.X;
      var y = bounds.Y;
      if (orientation == Orientation.NE || orientation == Orientation.SE) {
        x = bounds.Right;
      }
      if (orientation == Orientation.SW || orientation == Orientation.SE) {
        y = bounds.Bottom;
      }
      return (x, y);
    }

    /// <summary>
    /// Tests if a number is in between two other numbers, with included
    /// allowance for some floating point error with the supplied values.
    /// The order of the given boundaries does not matter.
    /// </summary>
    /// <param name="n">the double to test</param>
    /// <param name="b1">the first boundary</param>
    /// <param name="b2">the second boundary</param>
    private static bool NumberIsBetween(double n, double b1, double b2) {
      var tmp = b1;
      b1 = Math.Min(b1, b2);
      b2 = Math.Max(tmp, b2);
      return n + 1e-7 >= b1 && n - 1e-7 <= b2;
    }

    /// <summary>
    /// Tests whether or not a given segment is a minimum or maximum segment.
    /// </summary>
    /// <param name="s">the segment to test</param>
    private bool SegmentIsMinOrMax(Segment s) {
      return s == _MinXSegment || s == _MinYSegment || s == _MaxXSegment || s == _MaxYSegment;
    }

    /// <summary>
    /// Removes a segment from the layout state. This includes removing it
    /// from the quadtree, as well as setting the corresponding minimum or
    /// maximum segment to null if the given segment is a minimum or
    /// maximum.
    /// </summary>
    /// <param name="s">the segment to remove</param>
    private void RemoveSegmentFromLayoutState(ListNode<Segment> s) {
      if (s.Data == _MinXSegment) {
        _MinXSegment = null;
      }
      if (s.Data == _MaxXSegment) {
        _MaxXSegment = null;
      }
      if (s.Data == _MinYSegment) {
        _MinYSegment = null;
      }
      if (s.Data == _MaxYSegment) {
        _MaxYSegment = null;
      }

      _Tree.Remove(s.Data);
    }

    /// <summary>
    /// Removes all segments between the two given segments (exclusive).
    /// This includes removing them from the layout state, as well as
    /// the given segment list.
    /// </summary>
    /// <param name="segments">the full list of segments</param>
    /// <param name="s1">the first segment</param>
    /// <param name="s2">the second segment</param>
    private void RemoveBetween(CircularDoublyLinkedList<Segment> segments, ListNode<Segment> s1, ListNode<Segment> s2) {
      if (s1 == s2) return;
      var last = s1.Next;
      var count = 0;
      while (last != s2) {
        if (last == segments.Start) {
          segments.Start = s2;
        }

        RemoveSegmentFromLayoutState(last);

        count++;
        last = last.Next;
      }
      s1.Next = s2;
      s2.Prev = s1;
      segments.Length -= count;
    }

    /// <summary>
    /// Calculates the cost of a given fit placement, depending on the
    /// <see cref="PackShape"/> and <see cref="_EAspectRatio"/>.
    /// </summary>
    /// <param name="fit">the fit to calculate the cost of</param>
    private double PlacementCost(Rect fit) {
      if (PackShape == VPackShape.Rectangular) {
        if (_Bounds.Contains(fit)) {
          return 0;
        }
        return Math.Max(Math.Abs(_Center.X - fit.Center.X), Math.Abs(_Center.Y - fit.Center.Y) * _EAspectRatio);
      } else { // if (PackShape == VirtualizedPackedLayout.Elliptical)
        return Math.Pow((fit.Center.X - _Center.X) / _EAspectRatio, 2) + Math.Pow(fit.Center.Y - _Center.Y, 2);
      }
    }

    /// <summary>
    /// Uses the quadtree to determine if the given fit has any
    /// intersections anywhere along the perimeter.
    /// </summary>
    /// <param name="fit">the fit to check</param>
    private bool FitHasIntersections(Fit fit) {
      return _Tree.Intersecting(fit.Bounds).Count > 0;
    }

    /// <summary>
    /// Checks if a few nearby segments intersect with the given fit,
    /// producing faster interesection detection than a complete check
    /// with the quadtree in many cases. However, since it doesn't check
    /// the entire perimeter, this function is susceptible to false
    /// negatives and should only be used with a more comprehensive check.
    /// </summary>
    /// <param name="fit">the fit to check</param>
    private static bool FastFitHasIntersections(Fit fit) {
      var sNext = fit.S1.Next;
      var sPrev = fit.S1.Prev;
      for (var i = 0; i < 2; i++) {
        if (SegmentIntersects(sNext.Data, fit.Bounds)) {
          return true;
        }
        if (SegmentIntersects(sPrev.Data, fit.Bounds)) {
          return true;
        }
        sNext = sNext.Next;
        sPrev = sPrev.Prev;
      }
      return false;
    }

    /// <summary>
    /// Checks whether or not a segment intersects with a given rect.
    /// Used for <see cref="FastFitHasIntersections"/>.
    /// </summary>
    /// <param name="s">the segment to test</param>
    /// <param name="r">the rectangle to test</param>
    private static bool SegmentIntersects(Segment s, Rect r) {
      var left = Math.Min(s.X1, s.X2);
      var right = Math.Max(s.X1, s.X2);
      var top = Math.Min(s.Y1, s.Y2);
      var bottom = Math.Min(s.Y1, s.Y2);
      return !(left + 1e-7 >= r.Right || right - 1e-7 <= r.Left || top + 1e-7 >= r.Bottom || bottom - 1e-7 <= r.Top);
    }

    /// <summary>
    /// Checks if two numbers are approximately equal, used for
    /// eliminating mistakes caused by floating point error.
    /// </summary>
    /// <param name="x">the first number</param>
    /// <param name="y">the second number</param>
    private static bool ApproxEqual(double x, double y) {
      return Math.Abs(x - y) < 1e-7;
    }

    /// <summary>
    /// Class for a node in a {<see cref="CircularDoublyLinkedList{T}"/>.
    /// Stores a pointer to the previous and next node.
    /// </summary>
    internal class ListNode<T> {
      public T Data;
      public ListNode<T> Prev;
      public ListNode<T> Next;

      public ListNode(T data, ListNode<T> prev = null, ListNode<T> next = null) {
        Data = data;
        Prev = prev ?? this;
        Next = next ?? this;
      }
    }

    /// <summary>
    /// A Circular doubly linked list, used by <see cref="VirtualizedPackedLayout"/> to
    /// efficiently store <see cref="Segment"/>s with fast insertion and
    /// deletion time.
    /// </summary>
    internal class CircularDoublyLinkedList<T> {
      /// <summary>
      /// The start of the list, null when the list is empty.
      /// </summary>
      public ListNode<T> Start = null;
      /// <summary>
      /// The length of the list.
      /// </summary>
      public int Length = 0;

      /// <summary>
      /// Constructs a new list with an optional list of values.
      /// </summary>
      /// <param name="vals">values to create the list with</param>
      public CircularDoublyLinkedList(params T[] vals) {
        if (vals.Length > 0) {
          foreach (var val in vals) Push(val);
        }
      }

      /// <summary>
      /// Inserts the given value directly after the given node.
      /// </summary>
      /// <param name="val">the value to insert</param>
      /// <param name="node">the node to insert after</param>
      /// <returns>the new node</returns>
      public ListNode<T> InsertAfter(T val, ListNode<T> node) {
        if (node == null) {
          var newnode = new ListNode<T>(val);
          newnode.Prev = newnode;
          newnode.Next = newnode;
          Length = 1;
          return Start = newnode;
        }
        var tmp = node.Next;
        node.Next = new ListNode<T>(val, node, tmp);
        tmp.Prev = node.Next;
        Length++;
        return node.Next;
      }

      /// <summary>
      /// Inserts the given value or values at the end of the list.
      /// </summary>
      /// <param name="vals">the value(s) to insert</param>
      /// <returns>the node for the last value inserted (a list of values is inserted sequentially)</returns>
      public ListNode<T> Push(params T[] vals) {
        if (vals.Length == 0) {
          throw new Exception("You must push at least one element!");
        }
        var sp = Start?.Prev;
        var last = InsertAfter(vals[0], sp);
        for (var i = 1; i < vals.Length; i++) {
          last = InsertAfter(vals[i], last);
        }
        return last;
      }

      /// <summary>
      /// Removes the given node from the list.
      /// </summary>
      /// <param name="node">the node to remove</param>
      public void Remove(ListNode<T> node) {
        Length--;
        if (Length > 0) {
          node.Prev.Next = node.Next;
          node.Next.Prev = node.Prev;
          if (node == Start) {
            Start = node.Next;
          }
        } else {
          Start = null;
        }
      }

      /// <summary>
      /// Removes all nodes between the given start and end point (exclusive).
      /// Returns the given end node.
      /// </summary>
      /// <param name="start">node to start removing after</param>
      /// <param name="end">node to stop removing at</param>
      /// <returns>the end node</returns>
      public ListNode<T> RemoveBetween(ListNode<T> start, ListNode<T> end) {
        if (start != end) {
          var last = start.Next;
          var count = 0;
          while (last != end) {
            if (last == Start) {
              Start = end;
            }
            count++;
            last = last.Next;
          }
          start.Next = end;
          end.Prev = start;
          Length -= count;
          return end;
        }
        return start;
      }

    }

    // The following is a BSD-licensed implementation of the
    // Matousek-Sharir-Welzl algorithm for finding the smallest
    // enclosing circle around a given set of circles. The
    // original algorithm was adapted to support enclosing points
    // by assuming that the radius of a point is 0.

    // Copyright 2010-2016 Mike Bostock
    // All rights reserved.
    //
    // Redistribution and use in source and binary forms, with or without modification,
    // are permitted provided that the following conditions are met:
    //
    // * Redistributions of source code must retain the above copyright notice, this
    //   list of conditions and the following disclaimer.
    //
    // * Redistributions in binary form must reproduce the above copyright notice,
    //   this list of conditions and the following disclaimer in the documentation
    //   and/or other materials provided with the distribution.
    //
    // * Neither the name of the author nor the names of contributors may be used to
    //   endorse or promote products derived from this software without specific prior
    //   written permission.
    //
    // THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
    // ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
    // WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
    // DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
    // ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
    // (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
    // LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
    // ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
    // (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
    // SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

    /// <summary>
    /// Represents a circle for the purposes of the smallest-enclosing
    /// circle algorithm. The x and y values represent the center of
    /// the circle.
    /// </summary>
    internal class Circle {
      public double R;
      private Point _P;
      public Circle(double x, double y, double r) {
        R = r;
        _P = new Point(x, y);
      }

      public double X {
        get {
          return _P.X;
        }
        set {
          if (_P.X != value) _P.X = value;
        }
      }

      public double Y {
        get {
          return _P.Y;
        }
        set {
          if (_P.Y != value) _P.Y = value;
        }
      }
    }

    private Rect Enclose(List<Point> points) {
      var circles = new List<Circle>();
      foreach (var point in points) {
        circles.Add(new Circle(point.X, point.Y, 0));
      }
      return Enclose(circles);
    }

    /// <param name="circles">array of circles of points to find the enclosing circle for</param>
    private Rect Enclose(List<Circle> circles) {
      var i = 0;
      var n = (circles = Shuffle(new List<Circle>(circles))).Count;
      var B = new List<Circle>();
      Circle p;
      Circle e = null;

      while (i < n) {
        p = circles[i];
        if (e != null && EnclosesWeak(e, p)) ++i;
        else e = EncloseBasis(B = ExtendBasis(B, p)); i = 0;
      }

      if (e != null) {
        return CircleToRect(e);
      } else { // this will never happen, but needs to be here for strict TypeScript compilation
        throw new Exception("Assertion error");
      }
    }

    /// <summary>
    /// Converts a Circle to a go.Rect object.
    /// </summary>
    /// <param name="c">the Circle to convert</param>
    private static Rect CircleToRect(Circle c) {
      return new Rect(c.X - c.R, c.Y - c.R, c.R * 2, c.R * 2);
    }

    private static List<Circle> ExtendBasis(List<Circle> B, Circle p) {
      if (EnclosesWeakAll(p, B)) return new List<Circle> { p };

      // If we get here then B must have at least one element.
      for (var i = 0; i < B.Count; ++i) {
        if (EnclosesNot(p, B[i])
            && EnclosesWeakAll(EncloseBasis2(B[i], p), B)) {
          return new List<Circle> { B[i], p };
        }
      }

      // If we get here then B must have at least two elements.
      for (var i = 0; i < B.Count - 1; ++i) {
        for (var j = i + 1; j < B.Count; ++j) {
          if (EnclosesNot(EncloseBasis2(B[i], B[j]), p)
              && EnclosesNot(EncloseBasis2(B[i], p), B[j])
              && EnclosesNot(EncloseBasis2(B[j], p), B[i])
              && EnclosesWeakAll(EncloseBasis3(B[i], B[j], p), B)) {
            return new List<Circle> { B[i], B[j], p };
          }
        }
      }

      // If we get here then something is very wrong.
      throw new Exception("Assertion error");
    }

    private static bool EnclosesNot(Circle a, Circle b) {
      var ar = a != null ? a.R : 0;
      var br = b != null ? b.R : 0;
      var dr = ar - br;
      var dx = b.X - a.X;
      var dy = b.Y - a.Y;
      return dr < 0 || dr * dr < dx * dx + dy * dy;
    }

    private static bool EnclosesWeak(Circle a, Circle b) {
      var ar = a != null ? a.R : 0;
      var br = b != null ? b.R : 0;
      var dr = ar - br + 1e-6;
      var dx = b.X - a.X;
      var dy = b.Y - a.Y;
      return dr > 0 && dr * dr > dx * dx + dy * dy;
    }

    private static bool EnclosesWeakAll(Circle a, List<Circle> B) {
      for (var i = 0; i < B.Count; ++i) {
        if (!EnclosesWeak(a, B[i])) {
          return false;
        }
      }
      return true;
    }

    private static Circle EncloseBasis(List<Circle> B) {
      switch (B.Count) {
        case 2: return EncloseBasis2(B[0], B[1]);
        case 3: return EncloseBasis3(B[0], B[1], B[2]);
        default: return EncloseBasis1(B[0]); // case 1
      }
    }

    private static Circle EncloseBasis1(Circle a) {
      var ar = a != null ? a.R : 0;
      return new Circle(a.X, a.Y, ar);
    }

    private static Circle EncloseBasis2(Circle a, Circle b) {
      var ar = a != null ? a.R : 0;
      var br = b != null ? b.R : 0;
      var x1 = a.X;
      var y1 = a.Y;
      var r1 = ar;
      var x2 = b.X;
      var y2 = b.Y;
      var r2 = br;
      var x21 = x2 - x1;
      var y21 = y2 - y1;
      var r21 = r2 - r1;
      var l = Math.Sqrt(x21 * x21 + y21 * y21);
      return new Circle(
        (x1 + x2 + x21 / l * r21) / 2,
        (y1 + y2 + y21 / l * r21) / 2,
        (l + r1 + r2) / 2
      );
    }

    private static Circle EncloseBasis3(Circle a, Circle b, Circle c) {
      var ar = a != null ? a.R : 0;
      var br = b != null ? b.R : 0;
      var cr = c != null ? c.R : 0;
      var x1 = a.X;
      var y1 = a.Y;
      var r1 = ar;
      var x2 = b.X;
      var y2 = b.Y;
      var r2 = br;
      var x3 = c.X;
      var y3 = c.Y;
      var r3 = cr;
      var a2 = x1 - x2;
      var a3 = x1 - x3;
      var b2 = y1 - y2;
      var b3 = y1 - y3;
      var c2 = r2 - r1;
      var c3 = r3 - r1;
      var d1 = x1 * x1 + y1 * y1 - r1 * r1;
      var d2 = d1 - x2 * x2 - y2 * y2 + r2 * r2;
      var d3 = d1 - x3 * x3 - y3 * y3 + r3 * r3;
      var ab = a3 * b2 - a2 * b3;
      var xa = (b2 * d3 - b3 * d2) / (ab * 2) - x1;
      var xb = (b3 * c2 - b2 * c3) / ab;
      var ya = (a3 * d2 - a2 * d3) / (ab * 2) - y1;
      var yb = (a2 * c3 - a3 * c2) / ab;
      var A = xb * xb + yb * yb - 1;
      var B = 2 * (r1 + xa * xb + ya * yb);
      var C = xa * xa + ya * ya - r1 * r1;
      var r = -((A != 0) ? (B + Math.Sqrt(B * B - 4 * A * C)) / (2 * A) : C / B);
      return new Circle(
        x1 + xa + xb * r,
        y1 + ya + yb * r,
        r
      );
    }

    /// <summary>
    /// Shuffles array in place.
    /// </summary>
    /// <param name="a">items An array containing the items.</param>
    List<T> Shuffle<T>(List<T> a) {
      int j;
      T x;
      int i;
      for (i = a.Count - 1; i > 0; i--) {
        j = _Rand.Next(i + 1);
        x = a[i];
        a[i] = a[j];
        a[j] = x;
      }
      return a;
    }
  }

  /// <summary>
  /// These values determine the shape of the final layout.
  /// Used for <see cref="VirtualizedPackedLayout.PackShape"/>
  /// </summary>
  public enum VPackShape {
    /// <summary>
    /// This value for <see cref="VPackShape"/> causes nodes to be packed
    /// into an ellipse.
    ///
    /// The aspect ratio of this ellipse is determined by either
    /// <see cref="VirtualizedPackedLayout.AspectRatio"/> or <see cref="VirtualizedPackedLayout.Size"/>.
    /// </summary>
    Elliptical = 0,

    /// <summary>
    /// Causes nodes to be packed into a rectangle; this value is used for
    /// <see cref="VPackShape"/>.
    ///
    /// The aspect ratio of this rectangle is determined by either
    /// <see cref="VirtualizedPackedLayout.AspectRatio"/> or <see cref="VirtualizedPackedLayout.Size"/>.
    /// </summary>
    Rectangular = 1,

    /// <summary>
    /// Causes nodes to be packed into a spiral shape; this value is used
    /// for <see cref="VPackShape"/>.
    ///
    /// The <see cref="VirtualizedPackedLayout.AspectRatio"/> property is ignored in this mode, the
    /// <see cref="VirtualizedPackedLayout.Size"/> is expected to be square, and <see cref="VirtualizedPackedLayout.HasCircularNodes"/>
    /// will be assumed "true". Please see <see cref="VPackShape"/> for more details.
    /// </summary>
    Spiral = 2
  }

  /// <summary>
  /// These values determine the size of the layout.
  /// Used for <see cref="VirtualizedPackedLayout.PackMode"/>.
  /// </summary>
  public enum VPackMode {
    /// <summary>
    /// Nodes will be packed using the <see cref="VirtualizedPackedLayout.AspectRatio"/> property, with
    /// no size considerations; this value is used for <see cref="VPackMode"/>.
    ///
    /// The <see cref="VirtualizedPackedLayout.Spacing"/> property will be respected in this mode.
    /// </summary>
    AspectOnly = 0,

    /// <summary>
    /// Nodes will be compressed if necessary (using negative spacing) to fit the given
    /// <see cref="VirtualizedPackedLayout.Size"/>. However, if the <see cref="VirtualizedPackedLayout.Size"/> is bigger
    /// than the packed shape (with 0 spacing), it will not expand to fit it. This value
    /// is used for <see cref="VPackMode"/>.
    ///
    /// The <see cref="VirtualizedPackedLayout.Spacing"/> property will be respected in this mode, but only
    /// if it does not cause the layout to grow larger than the <see cref="VirtualizedPackedLayout.Size"/>.
    /// </summary>
    Fit = 1,

    /// <summary>
    /// Nodes will be either compressed or spaced evenly to fit the given
    /// <see cref="VirtualizedPackedLayout.Size"/>; this value is used for <see cref="VPackMode"/>.
    ///
    /// The <see cref="VirtualizedPackedLayout.Spacing"/> property will not be respected in this mode, and
    /// will not do anything if set.
    /// </summary>
    ExpandToFit = 2
  }

  /// <summary>
  /// These values specify an optional method by which to sort nodes before packing.
  /// Used for <see cref="VirtualizedPackedLayout.SortMode"/>.
  /// </summary>
  public enum VSortMode {
    /// <summary>
    /// Nodes will not be sorted before packing; this value is used for <see cref="VSortMode"/>.
    /// </summary>
    None = 0,

    /// <summary>
    /// Nodes will be sorted by their maximum side length before packing; this value is
    /// used for <see cref="VSortMode"/>.
    /// </summary>
    MaxSide = 1,

    /// <summary>
    /// Nodes will be sorted by their area; this value is used for <see cref="VSortMode"/>.
    /// </summary>
    Area = 2
  }

  /// <summary>
  /// These values specify the order that nodes will be sorted, if applicable.
  /// Used for <see cref="VirtualizedPackedLayout.SortOrder"/>.
  /// </summary>
  public enum VSortOrder {
    /// <summary>
    /// Nodes will be sorted in descending order; this value is used for <see cref="VSortOrder"/>.
    ///
    /// Does nothing if <see cref="VSortMode"/> is set to <see cref="VSortMode.None"/>.
    /// </summary>
    Descending = 0,

    /// <summary>
    /// Nodes will be sorted in ascending order; this value is used for <see cref="VSortOrder"/>.
    ///
    /// Does nothing if <see cref="VSortMode"/> is set to <see cref="VSortMode.None"/>.
    /// </summary>
    Ascending = 1
  }
}

