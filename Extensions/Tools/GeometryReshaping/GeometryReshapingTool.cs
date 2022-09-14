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

using System.Collections.Generic;
using System.Linq;

namespace Northwoods.Go.Tools.Extensions {
  /// <summary>
  /// The GeometryReshapingTool class allows for a Shape's Geometry to be modified by the user
  /// via the dragging of tool handles.
  /// </summary>
  /// <remarks>
  /// This tool does not handle Links, whose routes should be reshaped by the LinkReshapingTool.
  /// The <see cref="ReshapeElementName"/> needs to identify the named <see cref="Shape"/> within the
  /// selected <see cref="Part"/>.
  /// If the shape cannot be found or if its <see cref="Shape.Geometry"/> is not of type <see cref="GeometryType.Path"/>,
  /// this will not show any GeometryReshaping <see cref="Adornment"/>.
  /// At the current time this tool does not support adding or removing <see cref="PathSegment"/>s to the Geometry.
  /// </remarks>
  /// @category Tool Extension
  public class GeometryReshapingTool : Tool {

    private GraphObject _HandleArchetype;
    private GraphObject _MidHandleArchetype;
    private bool _IsResegmenting;
    private int _ResegmentingDistance = 3;
    private string _ReshapeElementName = "SHAPE";  // ??? can't add Part.ReshapeElementName property
    // there's no Part.ReshapeAdornmentTemplate either

    // internal state
    private GraphObject _Handle;
    private Shape _AdornedShape;
    private Geometry _OriginalGeometry;  // in case the tool is cancelled and the UndoManager is not enabled

    /// <summary>
    /// Constructs a GeometryReshapingTool and sets the handle and name of the tool.
    /// </summary>
    public GeometryReshapingTool() : base() {
      Name = "GeometryReshaping";

      var h = new Shape {
        Figure = "Diamond",
        DesiredSize = new Size(8, 8),
        Fill = "lightblue",
        Stroke = "dodgerblue",
        Cursor = "move"
      };
      _HandleArchetype = h;

      h = new Shape {
        Figure = "Circle",
        DesiredSize = new Size(7, 7),
        Fill = "lightblue",
        Stroke = "dodgerblue",
        Cursor = "move"
      };
      _MidHandleArchetype = h;
    }

    /// <summary>
    /// A small GraphObject used as a reshape handle for each segment.
    /// The default GraphObject is a small blue diamond.
    /// </summary>
    public GraphObject HandleArchetype {
      get {
        return _HandleArchetype;
      }
      set {
        _HandleArchetype = value;
      }
    }

    /// <summary>
    /// A small GraphObject used as a reshape handle at the middle of each segment for inserting a new segment.
    /// The default GraphObject is a small blue circle.
    /// </summary>
    public GraphObject MidHandleArchetype {
      get {
        return _MidHandleArchetype;
      }
      set {
        _MidHandleArchetype = value;
      }
    }

    /// <summary>
    /// Gets or sets whether this tool supports the user's addition or removal of segments in the geometry.
    /// </summary>
    /// <remarks>
    /// The default value is false.
    /// When the value is true, copies of the <see cref="MidHandleArchetype"/> will appear in the middle of each segment.
    /// At the current time, resegmenting is limited to straight segments, not curved ones.
    /// </remarks>
    public bool IsResegmenting {
      get {
        return _IsResegmenting;
      }
      set {
        _IsResegmenting = value;
      }
    }

    /// <summary>
    /// The maximum distance at which a resegmenting handle being positioned on a straight line
    /// between the adjacent points will cause one of the segments to be removed from the geometry.
    /// </summary>
    /// <remarks>
    /// The default value is 3.
    /// </remarks>
    public int ResegmentingDistance {
      get {
        return _ResegmentingDistance;
      }
      set {
        _ResegmentingDistance = value;
      }
    }

    /// <summary>
    /// The name of the GraphObject to be reshaped.
    /// </summary>
    /// <remarks>
    /// The default name is "SHAPE".
    /// </remarks>
    public string ReshapeElementName {
      get {
        return _ReshapeElementName;
      }
      set {
        _ReshapeElementName = value;
      }
    }

    /// <summary>
    /// This read-only property returns the <see cref="GraphObject"/> that is the tool handle being dragged by the user.
    /// </summary>
    /// <remarks>
    /// This will be contained by an <see cref="Adornment"/> whose category is "GeometryReshaping".
    /// Its <see cref="Adornment.AdornedElement"/> is the same as the <see cref="AdornedShape"/>.
    /// </remarks>
    public GraphObject Handle {
      get {
        return _Handle;
      }
      set {
        _Handle = value;
      }
    }

    /// <summary>
    /// Gets the <see cref="Shape"/> that is being reshaped.
    /// This must be contained within the selected Part.
    /// </summary>
    public Shape AdornedShape {
      get {
        return _AdornedShape;
      }
    }

    /// <summary>
    /// This read-only property remembers the original value for <see cref="Shape.Geometry"/>,
    /// so that it can be restored if this tool is cancelled.
    /// </summary>
    public Geometry OriginalGeometry {
      get {
        return _OriginalGeometry;
      }
    }

    /// <summary>
    /// Show an <see cref="Adornment"/> with a reshape handle at each point of the geometry.
    /// </summary>
    /// <remarks>
    /// Don't show anything if <see cref="ReshapeElementName"/> doesn't return a <see cref="Shape"/>
    /// that has a <see cref="Shape.Geometry"/> of type <see cref="GeometryType.Path"/>.
    /// </remarks>
    public override void UpdateAdornments(Part part) {
      if (part == null || part is Link) return;  // this tool never applies to Links
      if (part.IsSelected && !Diagram.IsReadOnly) {
        if (part.FindElement(ReshapeElementName) is Shape selelt && selelt.Geometry != null &&
            selelt.ActualBounds.IsReal() && selelt.IsVisibleElement() &&
            part.CanReshape() && part.ActualBounds.IsReal() && part.IsVisible() &&
            selelt.Geometry.Type == GeometryType.Path) {
          var geo = selelt.Geometry;
          var adornment = part.FindAdornment(Name);
          if (adornment == null || _CountHandles(geo) != adornment.Elements.Count() - 1) {
            adornment = MakeAdornment(selelt);
          }
          if (adornment != null) {
            // update the position/alignment of each handle
            var b = geo.Bounds;
            // update the size of the adornment
            var body = adornment.FindElement("BODY");
            if (body != null) body.DesiredSize = b.Size;
            List<GraphObject> unneeded = null;
            foreach (var h in adornment.Elements) {
              if (h["_Typ"] is not int) continue;
              var typ = (int)h["_Typ"];
              if (h["_Fig"] is not int) continue;
              var figi = (int)h["_Fig"];
              if (figi >= geo.Figures.Count) {
                if (unneeded == null) unneeded = new();
                unneeded.Add(h);
                continue;
              }
              var fig = geo.Figures[figi];
              if (h["_Seg"] is not int) continue;
              var segi = (int)h["_Seg"];
              if (segi >= fig.Segments.Count) {
                if (unneeded == null) unneeded = new();
                unneeded.Add(h);
                continue;
              }
              var seg = fig.Segments[segi];
              var x = 0d;
              var y = 0d;
              switch (typ) {
                case 0: x = fig.StartX; y = fig.StartY; break;
                case 1: x = seg.EndX; y = seg.EndY; break;
                case 2: x = seg.Point1X; y = seg.Point1Y; break;
                case 3: x = seg.Point2X; y = seg.Point2Y; break;
                case 4: x = (fig.StartX + seg.EndX) / 2; y = (fig.StartY + seg.EndY) / 2; break;
                case 5: x = (fig.Segments[segi - 1].EndX + seg.EndX) / 2; y = (fig.Segments[segi - 1].EndY + seg.EndY) / 2; break;
                case 6: x = (fig.StartX + seg.EndX) / 2; y = (fig.StartY + seg.EndY) / 2; break;
                default: throw new System.Exception("unexpected handle type");
              }
              h.Alignment = new Spot(0, 0, x - b.X, y - b.Y);
            }
            if (unneeded != null) {
              foreach (var h in unneeded) { if (adornment != null) adornment.Remove(h); }
            }

            part.AddAdornment(Name, adornment);
            adornment.Location = selelt.GetDocumentPoint(Spot.TopLeft);
            adornment.Angle = selelt.GetDocumentAngle();
            return;
          }
        }
      }
      part.RemoveAdornment(Name);
    }

    private int _CountHandles(Geometry geo) {
      var reseg = IsResegmenting;
      var c = 0;
      foreach (var fig in geo.Figures) {
        c++;
        foreach (var seg in fig.Segments) {
          if (reseg) {
            if (seg.Type == SegmentType.Line) c++;
            if (seg.IsClosed) c++;
          }
          c++;
          if (seg.Type == SegmentType.QuadraticBezier) c++;
          else if (seg.Type == SegmentType.Bezier) c += 2;
        }
      }
      return c;
    }

    /// <summary>
    /// (undocumented)
    /// </summary>
    [Undocumented]
    public Adornment MakeAdornment(Shape selelt) {
      var adornment = new Adornment("Spot") {
        LocationElementName = "BODY",
        LocationSpot = new Spot(0, 0, -selelt.StrokeWidth / 2, -selelt.StrokeWidth / 2)
      };
      var body = new Shape {
        Name = "BODY",
        Fill = (Brush)null,
        Stroke = (Brush)null,
        StrokeWidth = 0
      };
      adornment.Add(body);

      var geo = selelt.Geometry;
      GraphObject h = null;
      if (geo != null) {
        if (IsResegmenting) {
          for (var f = 0; f < geo.Figures.Count; f++) {
            var fig = geo.Figures[f];
            for (var g = 0; g < fig.Segments.Count; g++) {
              var seg = fig.Segments[g];
              if (seg.Type == SegmentType.Line) {
                h = MakeResegmentHandle(selelt, fig, seg);
                if (h != null) {
                  h["_Typ"] = (g == 0) ? 4 : 5;
                  h["_Fig"] = f;
                  h["_Seg"] = g;
                  adornment.Add(h);
                }
              }
              if (seg.IsClosed) {
                h = MakeResegmentHandle(selelt, fig, seg);
                if (h != null) {
                  h["_Typ"] = 6;
                  h["_Fig"] = f;
                  h["_Seg"] = g;
                  adornment.Add(h);
                }
              }
            }
          }
        }

        // requires Path Geometry, checked above in UpdateAdornments
        for (var f = 0; f < geo.Figures.Count; f++) {
          var fig = geo.Figures[f];
          for (var g = 0; g < fig.Segments.Count; g++) {
            var seg = fig.Segments[g];
            if (g == 0) {
              h = MakeHandle(selelt, fig, seg);
              if (h != null) {
                h["_Typ"] = 0;
                h["_Fig"] = f;
                h["_Seg"] = g;
                adornment.Add(h);
              }
            }
            h = MakeHandle(selelt, fig, seg);
            if (h != null) {
              h["_Typ"] = 1;
              h["_Fig"] = f;
              h["_Seg"] = g;
              adornment.Add(h);
            }
            if (seg.Type == SegmentType.QuadraticBezier || seg.Type == SegmentType.Bezier) {
              h = MakeHandle(selelt, fig, seg);
              if (h != null) {
                h["_Typ"] = 2;
                h["_Fig"] = f;
                h["_Seg"] = g;
                adornment.Add(h);
              }
              if (seg.Type == SegmentType.Bezier) {
                h = MakeHandle(selelt, fig, seg);
                if (h != null) {
                  h["_Typ"] = 3;
                  h["_Fig"] = f;
                  h["_Seg"] = g;
                  adornment.Add(h);
                }
              }
            }
          }
        }
      }
      adornment.Category = Name;
      adornment.AdornedElement = selelt;
      return adornment;
    }

    /// <summary>
    /// (undocumented)
    /// </summary>
    [Undocumented]
    public GraphObject MakeHandle(Shape selelt, PathFigure fig, PathSegment seg) {
      var h = HandleArchetype;
      if (h == null) return null;
      return h.Copy();
    }

    /// <summary>
    /// (undocumented)
    /// </summary>
    [Undocumented]
    public GraphObject MakeResegmentHandle(Shape selelt, PathFigure fig, PathSegment seg) {
      var h = MidHandleArchetype;
      if (h == null) return null;
      return h.Copy();
    }

    /// <summary>
    /// This tool may run when there is a mouse-down event on a reshape handle.
    /// </summary>
    public override bool CanStart() {
      if (!IsEnabled) return false;

      var diagram = Diagram;
      if (diagram.IsReadOnly) return false;
      if (!diagram.AllowReshape) return false;
      if (!diagram.LastInput.Left) return false;
      var h = FindToolHandleAt(diagram.FirstInput.DocumentPoint, Name);
      return (h != null);
    }

    /// <summary>
    /// Start reshaping, if <see cref="Tool.FindToolHandleAt"/> finds a reshape handle at the mouse down point.
    /// </summary>
    /// <remarks>
    /// If successful this sets <see cref="Handle"/> to be the reshape handle that it finds
    /// and <see cref="AdornedShape"/> to be the <see cref="Shape"/> being reshaped.
    /// It also remembers the original geometry in case this tool is cancelled.
    /// And it starts a transaction.
    /// </remarks>
    public override void DoActivate() {
      var diagram = Diagram;
      if (diagram == null) return;
      _Handle = FindToolHandleAt(diagram.FirstInput.DocumentPoint, Name);
      var h = _Handle;
      if (h == null) return;
      if ((h.Part as Adornment).AdornedElement is not Shape shape || shape.Part == null) return;
      _AdornedShape = shape;
      diagram.IsMouseCaptured = true;
      StartTransaction(Name);

      var typ = (int)h["_Typ"];
      var figi = (int)h["_Fig"];
      var segi = (int)h["_Seg"];
      if (IsResegmenting && typ >= 4 && shape.Geometry != null) {
        var locpt = shape.GetLocalPoint(diagram.FirstInput.DocumentPoint);
        var geo = shape.Geometry.Copy();
        var fig = geo.Figures[figi];
        var seg = fig.Segments[segi];
        var newseg = seg.Copy();
        switch (typ) {
          case 4: {
            newseg.EndX = (fig.StartX + seg.EndX) / 2;
            newseg.EndY = (fig.StartY + seg.EndY) / 2;
            newseg.IsClosed = false;
            fig.Segments.Insert(segi, newseg);
            break;
          }
          case 5: {
            var prevseg = fig.Segments[segi - 1];
            newseg.EndX = (prevseg.EndX + seg.EndX) / 2;
            newseg.EndY = (prevseg.EndY + seg.EndY) / 2;
            newseg.IsClosed = false;
            fig.Segments.Insert(segi, newseg);
            break;
          }
          case 6: {
            newseg.EndX = (fig.StartX + seg.EndX) / 2;
            newseg.EndY = (fig.StartY + seg.EndY) / 2;
            newseg.IsClosed = seg.IsClosed;
            seg.IsClosed = false;
            fig.Add(newseg);
            break;
          }
        }
        shape.Geometry = geo;  // modify the Shape
        var part = shape.Part;
        part.EnsureBounds();
        UpdateAdornments(part);  // update any Adornments of the Part
        _Handle = FindToolHandleAt(diagram.FirstInput.DocumentPoint, Name);
        if (_Handle == null) {
          DoDeactivate();  // need to rollback the transaction and not set .isActive
          return;
        }
      }

      _OriginalGeometry = shape.Geometry;
      IsActive = true;
    }

    /// <summary>
    /// This stops the current reshaping operation with the Shape as it is.
    /// </summary>
    public override void DoDeactivate() {
      StopTransaction();

      _Handle = null;
      _AdornedShape = null;
      var diagram = Diagram;
      if (diagram != null) diagram.IsMouseCaptured = false;
      IsActive = false;
    }

    /// <summary>
    /// Restore the shape to be the original geometry and stop this tool.
    /// </summary>
    public override void DoCancel() {
      var shape = _AdornedShape;
      if (shape != null) {
        // explicitly restore the original route, in case !UndoManager.IsEnabled
        shape.Geometry = _OriginalGeometry;
      }
      StopTool();
    }

    /// <summary>
    /// Call <see cref="Reshape"/> with a new point determined by the mouse
    /// to change the geometry of the <see cref="AdornedShape"/>.
    /// </summary>
    public override void DoMouseMove() {
      var diagram = Diagram;
      if (IsActive && diagram != null) {
        var newpt = ComputeReshape(diagram.LastInput.DocumentPoint);
        Reshape(newpt);
      }
    }

    /// <summary>
    /// Reshape the Shape's geometry with a point based on the most recent mouse point by calling <see cref="Reshape"/>,
    /// and then stop this tool.
    /// </summary>
    public override void DoMouseUp() {
      var diagram = Diagram;
      if (IsActive && diagram != null) {
        var newpt = ComputeReshape(diagram.LastInput.DocumentPoint);
        Reshape(newpt);
        var shape = AdornedShape;
        if (IsResegmenting && shape != null && shape.Geometry != null && shape.Part != null) {
          var typ = (int)Handle["_Typ"];
          var figi = (int)Handle["_Fig"];
          var segi = (int)Handle["_Seg"];
          var fig = shape.Geometry.Figures.ElementAtOrDefault(figi);
          if (fig != null && fig.Segments.Count > 2) {  // avoid making a degenerate polygon
            double ax, ay, bx, by, cx, cy;
            if (typ == 0) {
              var lastseg = fig.Segments.Count - 1;
              ax = fig.Segments[lastseg].EndX; ay = fig.Segments[lastseg].EndY;
              bx = fig.StartX; by = fig.StartY;
              cx = fig.Segments[0].EndX; cy = fig.Segments[0].EndY;
            } else {
              if (segi <= 0) {
                ax = fig.StartX; ay = fig.StartY;
              } else {
                ax = fig.Segments[segi - 1].EndX; ay = fig.Segments[segi - 1].EndY;
              }
              bx = fig.Segments[segi].EndX; by = fig.Segments[segi].EndY;
              if (segi >= fig.Segments.Count - 1) {
                cx = fig.StartX; cy = fig.StartY;
              } else {
                cx = fig.Segments[segi + 1].EndX; cy = fig.Segments[segi + 1].EndY;
              }
            }
            var q = new Point(bx, by);
            q = q.ProjectOntoLineSegment(ax, ay, cx, cy);
            // if B is within resegmentingDistance of the line from A to C,
            // and if Q is between A and C, remove that point from the geometry
            var dist = q.DistanceSquared(new Point(bx, by));
            if (dist < ResegmentingDistance * ResegmentingDistance) {
              var geo = shape.Geometry.Copy();
              fig = geo.Figures[figi];
              if (typ == 0) {
                var first = fig.Segments.FirstOrDefault();
                if (first != null) { fig.StartX = first.EndX; fig.StartY = first.EndY; }
              }
              if (segi > 0) {
                var prev = fig.Segments[segi - 1];
                var seg = fig.Segments[segi];
                prev.IsClosed = seg.IsClosed;
              }
              fig.Segments.RemoveAt(segi);
              shape.Geometry = geo;
              shape.Part.RemoveAdornment(Name);
              UpdateAdornments(shape.Part);
            }
          }
        }
        TransactionResult = Name;  // success
      }
      StopTool();
    }

    /// <summary>
    /// Change the geometry of the <see cref="AdornedShape"/> by moving the point corresponding to the current
    /// <see cref="Handle"/> to be at the given <see cref="Point"/>.
    /// </summary>
    /// <remarks>
    /// This is called by <see cref="DoMouseMove"/> and <see cref="DoMouseUp"/> with the result of calling
    /// <see cref="ComputeReshape"/> to constrain the input point.
    /// </remarks>
    /// <param name="newPoint">the value of the call to <see cref="ComputeReshape"/>.</param>
    public void Reshape(Point newPoint) {
      var shape = AdornedShape;
      if (shape == null || shape.Geometry == null) return;
      var locpt = shape.GetLocalPoint(newPoint);
      var geo = shape.Geometry.Copy();
      var h = Handle;
      if (h == null) return;
      if (h["_Typ"] is not int) return;
      var type = (int)h["_Typ"];
      if (h["_Fig"] is not int) return;
      var fig = geo.Figures[(int)h["_Fig"]];
      if (h["_Seg"] is not int) return;
      var seg = fig.Segments[(int)h["_Seg"]];
      switch (type) {
        case 0: fig.StartX = locpt.X; fig.StartY = locpt.Y; break;
        case 1: seg.EndX = locpt.X; seg.EndY = locpt.Y; break;
        case 2: seg.Point1X = locpt.X; seg.Point1Y = locpt.Y; break;
        case 3: seg.Point2X = locpt.X; seg.Point2Y = locpt.Y; break;
      }
      var offset = geo.Normalize();  // avoid any negative coordinates in the geometry
      shape.DesiredSize = new Size(double.NaN, double.NaN);  // clear the DesiredSize so Geometry can determine size
      shape.Geometry = geo;  // modify the Shape
      var part = shape.Part;  // move the Part holding the Shape
      if (part == null) return;
      part.EnsureBounds();
      if (part.LocationElement != shape && !part.LocationSpot.Equals(Spot.Center)) {  // but only if the locationSpot isn't Center
        // support the whole Node being rotated
        part.Move(part.Position.Subtract(offset.Rotate(part.Angle)));
      }
      UpdateAdornments(part);  // update any Adornments of the Part
      Diagram.MaybeUpdate();  // force more frequent drawing for smoother looking behavior
    }

    /// <summary>
    /// This is called by <see cref="DoMouseMove"/> and <see cref="DoMouseUp"/> to limit the input point
    /// before calling <see cref="Reshape"/>.
    /// </summary>
    /// <remarks>
    /// By default, this doesn't limit the input point.
    /// </remarks>
    /// <param name="p">the point where the handle is being dragged.</param>
    public virtual Point ComputeReshape(Point p) {
      return p;  // no constraints on the points
    }
  }
}
