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
  /// The FreehandDrawingTool allows the user to draw a shape using the mouse.
  /// </summary>
  /// <remarks>
  /// This tool collects all of the points from a mouse-down, all mouse-moves, until a mouse-up,
  /// and puts all of those points in a <see cref="Geometry"/> used by a <see cref="Shape"/>.
  /// It then adds a node data object to the diagram's model.
  ///
  /// This tool may be installed as the first mouse down tool:
  /// <code language="cs">
  ///   myDiagram.ToolManager.MouseDownTools.InsertAt(0, new FreehandDrawingTool());
  /// </code>
  ///
  /// The Shape used during the drawing operation can be customized by setting <see cref="TemporaryShape"/>.
  /// The node data added to the model can be customized by setting <see cref="ArchetypePartData"/>.
  /// </remarks>
  /// @category Tool Extension
  public class FreehandDrawingTool : Tool {
    // this is the Shape that is shown during a drawing operation
    private GraphObject _TemporaryShape;
    private object _ArchetypePartData; // the data to copy for a new polyline Part
    private bool _IsBackgroundOnly = true; // affects CanStart()

    /// <summary>
    /// Constructs a FreehandDrawingTool.
    /// </summary>
    public FreehandDrawingTool() : base() {
      Name = "FreehandDrawing";
      _TemporaryShape = new Shape { Name = "SHAPE", Fill = null, StrokeWidth = 1.5 };
      // the Shape has to be inside a temporary Part that is used during the drawing operation
      var temp = new Part { LayerName = "Tool" }.Add(_TemporaryShape);
    }

    /// <summary>
    /// Gets or sets the Shape that is used to hold the line as it is being drawn.
    /// </summary>
    /// <remarks>
    /// The default value is a simple Shape drawing an unfilled open thin black line.
    /// </remarks>
    public Shape TemporaryShape {
      get {
        return _TemporaryShape as Shape;
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
    /// Gets or sets the node data object that is copied and added to the model
    /// when the freehand drawing operation completes.
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
    /// Gets or sets whether this tool can only run if the user starts in the diagram's background
    /// rather than on top of an existing Part.
    /// </summary>
    /// <remarks>
    /// The default value is true.
    /// </remarks>
    public bool IsBackgroundOnly {
      get {
        return _IsBackgroundOnly;
      }
      set {
        _IsBackgroundOnly = value;
      }
    }

    /// <summary>
    /// Only start if the diagram is modifiable and allows insertions.
    /// </summary>
    /// <remarks>
    /// OPTIONAL: if the user is starting in the diagram's background, not over an existing Part.
    /// </remarks>
    public override bool CanStart() {
      if (!IsEnabled) return false;
      var diagram = Diagram;
      if (diagram.IsReadOnly || diagram.IsModelReadOnly) return false;
      if (!diagram.AllowInsert) return false;
      // don't include the following check when this tool is running modally
      if (diagram.CurrentTool != this && IsBackgroundOnly) {
        // only operates in the background, not on some Part
        var part = diagram.FindPartAt(diagram.LastInput.DocumentPoint, true);
        if (part != null) return false;
      }
      return true;
    }

    /// <summary>
    /// Capture the mouse and use a "crosshair" cursor.
    /// </summary>
    public override void DoActivate() {
      base.DoActivate();
      Diagram.IsMouseCaptured = true;
      Diagram.CurrentCursor = "crosshair";
    }

    /// <summary>
    /// Release the mouse and reset the cursor.
    /// </summary>
    public override void DoDeactivate() {
      base.DoDeactivate();
      if (TemporaryShape != null && TemporaryShape.Part != null) {
        Diagram.Remove(TemporaryShape.Part);
      }
      Diagram.CurrentCursor = "";
      Diagram.IsMouseCaptured = false;
    }

    /// <summary>
    /// This adds a Point to the <see cref="TemporaryShape"/>'s geometry.
    /// </summary>
    /// <remarks>
    /// If the Shape is not yet in the Diagram, its geometry is initialized and
    /// its parent Part is added to the Diagram.
    ///
    /// If the point is less than half a pixel away from the previous point, it is ignored.
    /// </remarks>
    public void AddPoint(Point p) {
      var shape = TemporaryShape;
      if (shape == null) return;
      var part = shape.Part;
      if (part == null) return;

      // for the temporary Shape, normalize the geometry to be in the viewport
      var viewpt = Diagram.ViewportBounds.Position;
      var q = new Point(p.X - viewpt.X, p.Y - viewpt.Y);

      if (part.Diagram == null) {
        var f = new PathFigure(q.X, q.Y, true);  // possibly filled, depending on Shape.Fill
        var g = new Geometry().Add(f);  // the Shape.Geometry consists of a single PathFigure
        shape.Geometry = g;
        // position the Shape's Part, accounting for the strokeWidth
        part.Position = new Point(viewpt.X - shape.StrokeWidth / 2, viewpt.Y - shape.StrokeWidth / 2);
        Diagram.Add(part);
      }

      // only add a point if it isn't too close to the last one
      var geo = shape.Geometry;
      if (geo != null) {
        var fig = geo.Figures.First();
        if (fig != null) {
          var segs = fig.Segments;
          var idx = segs.Count - 1;
          if (idx >= 0) {
            var last = segs.ElementAt(idx);
            if (Math.Abs(q.X - last.EndX) < 0.5 && Math.Abs(q.Y - last.EndY) < 0.5) return;
          }

          // must copy whole Geometry in order to add a PathSegment
          var geo2 = geo.Copy();
          var fig2 = geo2.Figures.First();
          if (fig2 != null) {
            fig2.Add(new PathSegment(SegmentType.Line, q.X, q.Y));
            shape.Geometry = geo2;
          }
        }
      }
    }

    /// <summary>
    /// Start drawing the line by starting to accumulate points in the <see cref="TemporaryShape"/>'s geometry.
    /// </summary>
    public override void DoMouseDown() {
      if (!IsActive) {
        DoActivate();
        // the first point
        AddPoint(Diagram.LastInput.DocumentPoint);
      }
    }

    /// <summary>
    /// Keep accumulating points in the <see cref="TemporaryShape"/>'s geometry.
    /// </summary>
    public override void DoMouseMove() {
      if (IsActive) {
        AddPoint(Diagram.LastInput.DocumentPoint);
      }
    }

    /// <summary>
    /// Finish drawing the line by adding a node data object holding the
    /// geometry string and the node position that the node template can bind to.
    /// </summary>
    /// <remarks>
    /// This copies the <see cref="ArchetypePartData"/> and adds it to the model.
    /// </remarks>
    public override void DoMouseUp() {
      var diagram = Diagram;
      var started = false;
      if (IsActive) {
        started = true;
        // the last point
        AddPoint(diagram.LastInput.DocumentPoint);
        // normalize geometry and node position
        var viewpt = diagram.ViewportBounds.Position;
        if (TemporaryShape.Geometry != null) {
          var geo = TemporaryShape.Geometry.Copy();
          var pos = geo.Normalize();
          pos.X = viewpt.X - pos.X;
          pos.Y = viewpt.Y - pos.Y;

          diagram.StartTransaction(Name);
          // create the node data for the model
          var d = diagram.Model.CopyNodeData(ArchetypePartData);
          if (d != null) {
            // adding data to model creates the actual Part
            diagram.Model.AddNodeData(d);
            var part = diagram.FindPartForData(d);
            if (part != null) {
              // assign the location
              part.Location = new Point(pos.X + geo.Bounds.Width / 2, pos.Y + geo.Bounds.Height / 2);
              // assign the Shape.Geometry
              if (part.FindElement("SHAPE") is Shape shape) shape.Geometry = geo;
            }
          }
        }
      }
      StopTool();
      if (started) diagram.CommitTransaction(Name);
    }
  }
}
