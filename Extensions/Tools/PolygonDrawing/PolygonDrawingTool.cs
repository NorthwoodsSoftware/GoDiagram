/*
*  Copyright (C) 1998-2024 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main GoDiagram library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in the GoDiagram repository (https://github.com/NorthwoodsSoftware/GoDiagram/tree/main/Extensions).
* See the Extensions intro page (https://godiagram.com/intro/extensions.html) for more information.
*/

using System.Linq;

namespace Northwoods.Go.Tools.Extensions {

  /// <summary>
  /// The PolygonDrawingTool class lets the user draw a new polygon or polyline shape by clicking where the corners should go.
  /// </summary>
  /// <remarks>
  /// Right click or type ENTER to finish the operation.
  ///
  /// Set <see cref="IsPolygon"/> to false if you want this tool to draw open unfilled polyline shapes.
  /// Set <see cref="ArchetypePartData"/> to customize the node data object that is added to the model.
  /// Data-bind to those properties in your node template to customize the appearance and behavior of the part.
  ///
  /// This tool uses a temporary <see cref="Shape"/>, <see cref="TemporaryShape"/>, held by a <see cref="Part"/> in the "Tool" layer,
  /// to show interactively what the user is drawing.
  /// </remarks>
  /// @category Tool Extension
  public class PolygonDrawingTool : Tool {
    private bool _IsPolygon = true;
    private bool _HasArcs = false;
    private bool _IsOrthoOnly = false;
    private bool _IsGridSnapEnabled = false;
    private object _ArchetypePartData; // the data to copy for a new polygon Part

    // this is the Shape that is shown during a drawing operation
    private Shape _TemporaryShape;

    /// <summary>
    /// Constructs an PolygonDrawingTool and sets the name for the tool.
    /// </summary>
    public PolygonDrawingTool() : base() {
      _TemporaryShape =
        new Shape {
          Name = "SHAPE",
          Fill = "lightgray",
          StrokeWidth = 1.5
        };
      // the Shape has to be inside a temporary Part that is used during the drawing operation
      var temp = new Part { LayerName = "Tool" }.Add(_TemporaryShape);
      Name = "PolygonDrawing";
    }

    /// <summary>
    /// Gets or sets whether this tools draws a filled polygon or an unfilled open polyline.
    /// </summary>
    /// <remarks>
    /// The default value is true.
    /// </remarks>
    public bool IsPolygon {
      get {
        return _IsPolygon;
      }
      set {
        _IsPolygon = value;
      }
    }


    /// <summary>
    /// Gets or sets whether this tool draws shapes with quadratic bezier curves for each segment, or just straight lines.
    /// </summary>
    /// <remarks>
    /// The default value is false -- only use straight lines.
    /// </remarks>
    public bool HasArcs {
      get {
        return _HasArcs;
      }
      set {
        _HasArcs = value;
      }
    }

    /// <summary>
    /// Gets or sets whether this tool draws shapes with only orthogonal segments, or segments in any direction.
    /// </summary>
    /// <remarks>
    /// The default value is false -- draw segments in any direction. This does not restrict the closing segment, which may not be orthogonal.
    /// </remarks>
    public bool IsOrthoOnly {
      get {
        return _IsOrthoOnly;
      }
      set {
        _IsOrthoOnly = value;
      }
    }

    /// <summary>
    /// Gets or sets whether this tool only places the shape's corners on the Diagram"s visible grid.
    /// </summary>
    /// <remarks>
    /// The default value is false
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
    /// Gets or sets the node data object that is copied and added to the model
    /// when the drawing operation completes.
    /// </summary>
    public object ArchetypePartData {
      get {
        return _ArchetypePartData;
      }
      set {
        _ArchetypePartData = value;
      }
    }

    /// <summary>
    /// Gets or sets the Shape that is used to hold the line as it is being drawn.
    /// </summary>
    /// <remarks>
    /// The default value is a simple Shape drawing an unfilled open thin black line.
    /// </remarks>
    public Shape TemporaryShape {
      get {
        return _TemporaryShape;
      }
      set {
        if (_TemporaryShape != value && value != null) {
          value.Name = "SHAPE";
          var panel = _TemporaryShape.Panel;
          if (panel != null) {
            panel.Remove(_TemporaryShape);
            _TemporaryShape = value;
            panel.Add(_TemporaryShape);
          }
        }
      }
    }

    /// <summary>
    /// Don't start this tool in a mode-less fashion when the user's mouse-down is on an existing Part.
    /// </summary>
    /// <remarks>
    /// When this tool is a mouse-down tool, it requires using the left mouse button in the background of a modifiable Diagram.
    /// Modal uses of this tool will not call this canStart predicate.
    /// </remarks>
    public override bool CanStart() {
      if (!IsEnabled) return false;
      var diagram = Diagram;
      if (diagram.IsReadOnly || diagram.IsModelReadOnly) return false;
      var model = diagram.Model;
      if (model == null) return false;
      // require left button
      if (!diagram.FirstInput.Left) return false;
      // can't start when mouse-down on an existing Part
      var obj = diagram.FindElementAt(diagram.FirstInput.DocumentPoint, null, null);
      return obj == null;
    }

    /// <summary>
    /// Start a transaction, capture the mouse, use a "crosshair" cursor,
    /// and start accumulating points in the geometry of the <see cref="TemporaryShape"/>.
    /// </summary>
    public override void DoStart() {
      base.DoStart();
      var diagram = Diagram;
      if (diagram == null) return;
      StartTransaction(Name);
      diagram.CurrentCursor = diagram.DefaultCursor = "crosshair";
      if (!diagram.LastInput.IsTouchEvent) diagram.IsMouseCaptured = true;
    }

    /// <summary>
    /// Start a transaction, capture the mouse, use a "crosshair" cursor,
    /// and start accumulating points in the geometry of the <see cref="TemporaryShape"/>.
    /// </summary>
    public override void DoActivate() {
      base.DoActivate();
      var diagram = Diagram;
      if (diagram == null) return;
      // the first point
      if (!diagram.LastInput.IsTouchEvent) AddPoint(diagram.LastInput.DocumentPoint);
    }

    /// <summary>
    /// Stop the transaction and clean up.
    /// </summary>
    public override void DoStop() {
      base.DoStop();
      var diagram = Diagram;
      if (diagram == null) return;
      diagram.CurrentCursor = diagram.DefaultCursor = "auto";
      if (TemporaryShape != null && TemporaryShape.Part != null) {
        diagram.Remove(TemporaryShape.Part);
      }
      if (diagram.IsMouseCaptured) diagram.IsMouseCaptured = false;
      StopTransaction();
    }

    /// <summary>
    /// Given a potential Point for the next segment, return a Point it to snap to the grid, and remain orthogonal, if either is applicable.
    /// </summary>
    internal Point ModifyPointForGrid(Point p) {
      var pregrid = p;
      var grid = Diagram.Grid;
      if (grid != null && grid.Visible && IsGridSnapEnabled) {
        var cell = grid.GridCellSize;
        var orig = grid.GridOrigin;
        p = p.SnapToGrid(orig.X, orig.Y, cell.Width, cell.Height); // compute the closest grid point (modifies p)
      }
      if (TemporaryShape.Geometry == null) return p;
      var geometry = TemporaryShape.Geometry;
      if (geometry == null) return p;
      var fig = geometry.Figures.First();
      if (fig == null) return p;
      var segments = fig.Segments;
      if (IsOrthoOnly && segments.Count > 0) {
        var lastPt = new Point(fig.StartX, fig.StartY); // assuming segments.Count == 1
        if (segments.Count > 1) {
          // the last segment is the current temporary segment, which we might be altering. We want the segment before
          var secondLastSegment = (segments.ElementAt(segments.Count - 2));
          lastPt = new Point(secondLastSegment.EndX, secondLastSegment.EndY);
        }
        if (pregrid.DistanceSquared(lastPt.X, pregrid.Y) < pregrid.DistanceSquared(pregrid.X, lastPt.Y)) { // closer to X coord
          return new Point(lastPt.X, p.Y);
        } else { // closer to Y coord
          return new Point(p.X, lastPt.Y);
        }
      }
      return p;
    }

    /// <summary>
    /// This internal method adds a segment to the geometry of the <see cref="TemporaryShape"/>.
    /// </summary>
    internal void AddPoint(Point p) {
      var diagram = Diagram;
      var shape = TemporaryShape;
      if (shape == null) return;

      // for the temporary Shape, normalize the geometry to be in the viewport
      var viewpt = diagram.ViewportBounds.Position;
      var q = ModifyPointForGrid(new Point(p.X - viewpt.X, p.Y - viewpt.Y));

      var part = shape.Part;
      Geometry geo = null;
      // if it's not in the Diagram, re-initialize the Shape's geometry and add the Part to the Diagram
      if (part != null && part.Diagram == null) {
        var fig = new PathFigure(q.X, q.Y, true);  // possibly filled, depending on Shape.Fill
        geo = new Geometry().Add(fig);  // the Shape.Geometry consists of a single PathFigure
        TemporaryShape.Geometry = geo;
        // position the Shape's Part, accounting for the stroke width
        part.Position = viewpt.Offset(-shape.StrokeWidth / 2, -shape.StrokeWidth / 2);
        diagram.Add(part);
      } else if (shape.Geometry != null) {
        // must copy whole Geometry in order to add a PathSegment
        geo = shape.Geometry.Copy();
        var fig = geo.Figures.First();
        if (fig != null) {
          if (HasArcs) {
            var lastseg = fig.Segments.Last();
            if (lastseg == null) {
              fig.Add(new PathSegment(SegmentType.QuadraticBezier, q.X, q.Y, (fig.StartX + q.X) / 2, (fig.StartY + q.Y) / 2));
            } else {
              fig.Add(new PathSegment(SegmentType.QuadraticBezier, q.X, q.Y, (lastseg.EndX + q.X) / 2, (lastseg.EndY + q.Y) / 2));
            }
          } else {
            fig.Add(new PathSegment(SegmentType.Line, q.X, q.Y));
          }
        }
      }
      shape.Geometry = geo;
    }

    /// <summary>
    /// This internal method changes the last segment of the geometry of the <see cref="TemporaryShape"/> to end at the given point.
    /// </summary>
    internal void MoveLastPoint(Point p) {
      p = ModifyPointForGrid(p);
      var diagram = Diagram;
      // must copy whole Geometry in order to change a PathSegment
      var shape = TemporaryShape;
      if (shape.Geometry == null) return;
      var geo = shape.Geometry.Copy();
      var fig = geo.Figures.First();
      if (fig == null) return;
      var segs = fig.Segments;
      if (segs.Count > 0) {
        // for the temporary Shape, normalize the geometry to be in the viewport
        var viewpt = diagram.ViewportBounds.Position;
        var seg = segs.ElementAt(segs.Count - 1);
        // modify the last PathSegment to be the given Point p
        seg.EndX = p.X - viewpt.X;
        seg.EndY = p.Y - viewpt.Y;
        if (seg.Type == SegmentType.QuadraticBezier) {
          var prevx = 0.0;
          var prevy = 0.0;
          if (segs.Count > 1) {
            var prevseg = segs.ElementAt(segs.Count - 2);
            prevx = prevseg.EndX;
            prevy = prevseg.EndY;
          } else {
            prevx = fig.StartX;
            prevy = fig.StartY;
          }
          seg.Point1X = (seg.EndX + prevx) / 2;
          seg.Point1Y = (seg.EndY + prevy) / 2;
        }
        shape.Geometry = geo;
      }
    }

    /// This internal method removes the last segment of the geometry of the <see cref="TemporaryShape"/>.
    /// <summary>
    /// Undocumented.
    /// </summary>
    [Undocumented]
    public void RemoveLastPoint() {
      // must copy whole Geometry in order to remove a PathSegment
      var shape = TemporaryShape;
      if (shape.Geometry == null) return;
      var geo = shape.Geometry.Copy();
      var fig = geo.Figures.First();
      if (fig == null) return;
      var segs = fig.Segments;
      if (segs.Count > 0) {
        segs.RemoveAt(segs.Count - 1);
        shape.Geometry = geo;
      }
    }

    /// <summary>
    /// Add a new node data object to the model and initialize the Part's
    /// position and its Shape's geometry by copying the <see cref="TemporaryShape"/>'s <see cref="Shape.Geometry"/>.
    /// </summary>
    public void FinishShape() {
      var diagram = Diagram;
      var shape = TemporaryShape;
      if (shape != null && ArchetypePartData != null) {
        // remove the temporary point, which is last, except on touch devices
        if (!diagram.LastInput.IsTouchEvent) RemoveLastPoint();
        var tempgeo = shape.Geometry;
        // require 3 points (2 segments) if polygon; 2 points (1 segment) if polyline
        if (tempgeo != null) {
          var tempfig = tempgeo.Figures.First();
          if (tempfig != null && tempfig.Segments.Count >= (IsPolygon ? 2 : 1)) {
            // normalize geometry and node position
            var viewpt = diagram.ViewportBounds.Position;
            var copygeo = tempgeo.Copy();
            var copyfig = copygeo.Figures.First();
            if (IsPolygon && copyfig != null) {
              // if polygon, close the last segment
              var segs = copyfig.Segments;
              var seg = segs.ElementAt(segs.Count - 1);
              seg.IsClosed = true;
            }
            // create the node data for the model
            var d = diagram.Model.CopyNodeData(ArchetypePartData);
            if (d != null) {
              // adding data to model creates the actual Part
              diagram.Model.AddNodeData(d);
              var part = diagram.FindPartForData(d);
              if (part != null) {
                // assign the position for the whole Part
                var pos = copygeo.Normalize();
                pos.X = viewpt.X - pos.X - shape.StrokeWidth / 2;
                pos.Y = viewpt.Y - pos.Y - shape.StrokeWidth / 2;
                part.Position = pos;
                // assign the Shape.Geometry
                if (part.FindElement("SHAPE") is Shape pShape) pShape.Geometry = copygeo;
                TransactionResult = Name;
              }
            }
          }
        }
      }
      StopTool();
    }

    /// <summary>
    /// Add another point to the geometry of the <see cref="TemporaryShape"/>.
    /// </summary>
    public override void DoMouseDown() {
      var diagram = Diagram;
      if (!IsActive) {
        DoActivate();
      }
      // a new temporary end point, the previous one is now "accepted"
      AddPoint(diagram.LastInput.DocumentPoint);
      if (!diagram.LastInput.Left) {  // e.g. right mouse down
        FinishShape();
      } else if (diagram.LastInput.ClickCount > 1) {  // e.g. double-click
        RemoveLastPoint();
        FinishShape();
      }
    }

    /// <summary>
    /// Move the last point of the <see cref="TemporaryShape"/>'s geometry to follow the mouse point.
    /// </summary>
    public override void DoMouseMove() {
      var diagram = Diagram;
      if (IsActive) {
        MoveLastPoint(diagram.LastInput.DocumentPoint);
      }
    }

    /// <summary>
    /// Do not stop this tool, but continue to accumulate Points via mouse-down events.
    /// </summary>
    public override void DoMouseUp() {
      // don't stop this tool (the default behavior is to call StopTool)
    }

    /// <summary>
    /// Typing the "ENTER" key accepts the current geometry (excluding the current mouse point)
    /// and creates a new part in the model by calling <see cref="FinishShape"/>.
    ///
    /// Typing the "Z" key causes the previous point to be discarded.
    ///
    /// Typing the "ESCAPE" key causes the temporary Shape and its geometry to be discarded and this tool to be stopped.
    /// </summary>
    public override void DoKeyDown() {
      var diagram = Diagram;
      if (!IsActive) return;
      var e = diagram.LastInput;
      if (e.Key == "ENTER") {  // accept
        FinishShape();  // all done!
      } else if (e.Key == "Z") {  // undo
        Undo();
      } else {
        base.DoKeyDown();
      }
    }

    /// <summary>
    /// Undo: remove the last point and continue the drawing of new points.
    /// </summary>
    public void Undo() {
      var diagram = Diagram;
      // remove a point, and then treat the last one as a temporary one
      RemoveLastPoint();
      var lastInput = diagram.LastInput;
      // if lastInput.Event is mouse event
      if (lastInput.EventType == "mousedown" || lastInput.EventType == "mouseup" || lastInput.EventType == "pointerdown" || lastInput.EventType == "pointerup") MoveLastPoint(lastInput.DocumentPoint);
    }
  }
}
