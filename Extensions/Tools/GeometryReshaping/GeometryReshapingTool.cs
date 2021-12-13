using System.Linq;
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

namespace Northwoods.Go.Tools.Extensions {

  /// <summary>
  /// The GeometryReshapingTool class allows for a Shape"s Geometry to be modified by the user
  /// via the dragging of tool handles.
  /// This does not handle Links, whose routes should be reshaped by the LinkReshapingTool.
  /// The <see cref="ReshapeObjectName"/> needs to identify the named <see cref="Shape"/> within the
  /// selected <see cref="Part"/>.
  /// If the shape cannot be found or if its <see cref="Shape.Geometry"/> is not of type <see cref="GeometryType.Path"/>,
  /// this will not show any GeometryReshaping <see cref="Adornment"/>.
  /// At the current time this tool does not support adding or removing <see cref="PathSegment"/>s to the Geometry.
  ///
  /// If you want to experiment with this extension, try the <a href="../../extensionsTS/GeometryReshaping.Html">Geometry Reshaping</a> sample.
  /// </summary>
  /// @category Tool Extension
  public class GeometryReshapingTool : Tool {

    private GraphObject _HandleArchetype;
    private string _ReshapeObjectName = "SHAPE";  // ??? can"t add Part.ReshapeObjectName property
                                                  // there"s no Part.ReshapeAdornmentTemplate either

    // internal state
    private GraphObject _Handle;
    private Shape _AdornedShape;
    private Geometry _OriginalGeometry;  // in case the tool is cancelled and the UndoManager is not enabled

    /// <summary>
    /// Constructs a GeometryReshapingTool and sets the handle and name of the tool.
    /// </summary>
    public GeometryReshapingTool() : base() {
      var h = new Shape();
      h.Figure = "Diamond";
      h.DesiredSize = new Size(7, 7);
      h.Fill = "lightblue";
      h.Stroke = "dodgerblue";
      h.Cursor = "move";
      _HandleArchetype = h;
      Name = "GeometryReshaping";
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
    /// The name of the GraphObject to be reshaped.
    /// </summary>
    public string ReshapeObjectName {
      get {
        return _ReshapeObjectName;
      }
      set {
        _ReshapeObjectName = value;
      }
    }

    /// <summary>
    /// This read-only property returns the <see cref="GraphObject"/> that is the tool handle being dragged by the user.
    /// This will be contained by an <see cref="Adornment"/> whose category is "GeometryReshaping".
    /// Its <see cref="Adornment.AdornedElement"/> is the same as the <see cref="AdornedShape"/>.
    /// </summary>
    public GraphObject Handle {
      get {
        return _Handle;
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
    /// Don"t show anything if <see cref="ReshapeObjectName"/> doesn"t identify a <see cref="Shape"/>
    /// that has a <see cref="Shape.Geometry"/> of type <see cref="GeometryType.Path"/>.
    /// </summary>
    public override void UpdateAdornments(Part part) {
      if (part == null || part is Link) return;  // this tool never applies to Links
      if (part.IsSelected && !Diagram.IsReadOnly) {
        var selelt = part.FindElement(ReshapeObjectName) as Shape;
        if (selelt is Shape && selelt.Geometry != null &&
          selelt.ActualBounds.IsReal() && selelt.IsVisibleElement() &&
          part.CanReshape() && part.ActualBounds.IsReal() && part.IsVisible() &&
          selelt.Geometry.Type == GeometryType.Path) {
          var adornment = part.FindAdornment(Name);
          if (adornment == null) {
            adornment = MakeAdornment(selelt);
          }
          if (adornment != null) {
            // update the position/alignment of each handle
            var geo = selelt.Geometry;
            var b = geo.Bounds;
            // update the size of the adornment
            var body = adornment.FindElement("BODY");
            if (body != null) body.DesiredSize = b.Size;
            foreach (GraphObject h in adornment.Elements) {
              if (h["_Typ"] == null) continue;
              // null-coalesce operator so ElementAt throws exception if property isn't set
              var figIdx = h["_Fig"] as int? ?? -1;
              var segIdx = h["_Seg"] as int? ?? -1;
              var fig = geo.Figures.ElementAt(figIdx);
              var seg = fig.Segments.ElementAt(segIdx);
              var x = 0d;
              var y = 0d;
              switch (h["_Typ"]) {
                case 0: x = fig.StartX; y = fig.StartY; break;
                case 1: x = seg.EndX; y = seg.EndY; break;
                case 2: x = seg.Point1X; y = seg.Point1Y; break;
                case 3: x = seg.Point2X; y = seg.Point2Y; break;
              }
              h.Alignment = new Spot(0, 0, x - b.X, y - b.Y);
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

    /// @hidden @internal
    public Adornment MakeAdornment(Shape selelt) {
      var adornment = new Adornment {
        Type = PanelLayoutSpot.Instance,
        LocationElementName = "BODY",
        LocationSpot = new Spot(0, 0, -selelt.StrokeWidth / 2, -selelt.StrokeWidth / 2)
      };
      var h = new Shape {
        Name = "BODY",
        Fill = (Brush)null,
        Stroke = (Brush)null,
        StrokeWidth = 0
      };
      adornment.Add(h);

      var geo = selelt.Geometry;
      if (geo != null) {
        // requires Path Geometry, checked above in UpdateAdornments
        for (var f = 0; f < geo.Figures.Count; f++) {
          var fig = geo.Figures.ElementAt(f);
          for (var g = 0; g < fig.Segments.Count; g++) {
            var seg = fig.Segments.ElementAt(g);
            if (g == 0) {
              var h0 = MakeHandle(selelt, fig, seg);
              if (h0 != null) {
                h0["_Typ"] = 0;
                h0["_Fig"] = f;
                h0["_Seg"] = g;
                adornment.Add(h0);
              }
            }
            var h1 = MakeHandle(selelt, fig, seg);
            if (h1 != null) {
              h1["_Typ"] = 1;
              h1["_Fig"] = f;
              h1["_Seg"] = g;
              adornment.Add(h1);
            }
            if (seg.Type == SegmentType.QuadraticBezier || seg.Type == SegmentType.Bezier) {
              var h2 = MakeHandle(selelt, fig, seg);
              if (h2 != null) {
                h2["_Typ"] = 2;
                h2["_Fig"] = f;
                h2["_Seg"] = g;
                adornment.Add(h2);
              }
              if (seg.Type == SegmentType.Bezier) {
                var h3 = MakeHandle(selelt, fig, seg);
                if (h3 != null) {
                  h3["_Typ"] = 3;
                  h3["_Fig"] = f;
                  h3["_Seg"] = g;
                  adornment.Add(h3);
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

    /// @hidden @internal
    public GraphObject MakeHandle(GraphObject selelt, PathFigure fig, PathSegment seg) {
      var h = HandleArchetype;
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
    ///
    /// If successful this sets <see cref="Handle"/> to be the reshape handle that it finds
    /// and <see cref="AdornedShape"/> to be the <see cref="Shape"/> being reshaped.
    /// It also remembers the original geometry in case this tool is cancelled.
    /// And it starts a transaction.
    /// </summary>
    public override void DoActivate() {
      var diagram = Diagram;
      _Handle = FindToolHandleAt(diagram.FirstInput.DocumentPoint, Name);
      if (_Handle == null) return;
      var shape = (_Handle.Part as Adornment).AdornedElement as Shape;
      if (shape == null) return;
      _AdornedShape = shape;
      diagram.IsMouseCaptured = true;
      StartTransaction(Name);
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
      diagram.IsMouseCaptured = false;
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
      if (IsActive) {
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
      if (IsActive) {
        var newpt = ComputeReshape(diagram.LastInput.DocumentPoint);
        Reshape(newpt);
        TransactionResult = Name;  // success
      }
      StopTool();
    }

    /// <summary>
    /// Change the geometry of the <see cref="AdornedShape"/> by moving the point corresponding to the current
    /// <see cref="Handle"/> to be at the given <see cref="Point"/>.
    /// This is called by <see cref="DoMouseMove"/> and <see cref="DoMouseUp"/> with the result of calling
    /// <see cref="ComputeReshape"/> to constrain the input point.
    /// </summary>
    /// <param name="newPoint">the value of the call to <see cref="ComputeReshape"/>.</param>
    public void Reshape(Point newPoint) {
      var shape = AdornedShape;
      if (shape == null || shape.Geometry == null) return;
      var locpt = shape.GetLocalPoint(newPoint);
      var geo = shape.Geometry.Copy();
      var type = Handle["_Typ"];
      if (type == null) return;
      // null-coalesce operator so ElementAt throws exception if property isn't set
      var figIdx = Handle["_Fig"] as int? ?? -1;
      var segIdx = Handle["_Seg"] as int? ?? -1;
      var fig = geo.Figures.ElementAt(figIdx);
      var seg = fig.Segments.ElementAt(segIdx);
      switch (type) {
        case 0: fig.StartX = locpt.X; fig.StartY = locpt.Y; break;
        case 1: seg.EndX = locpt.X; seg.EndY = locpt.Y; break;
        case 2: seg.Point1X = locpt.X; seg.Point1Y = locpt.Y; break;
        case 3: seg.Point2X = locpt.X; seg.Point2Y = locpt.Y; break;
      }
      var offset = geo.Normalize();  // avoid any negative coordinates in the geometry
      shape.DesiredSize = new Size(double.NaN, double.NaN); // clear the desiredSize so Geometry can determine size
      shape.Geometry = geo;  // modify the Shape
      var part = shape.Part;  // move the Part holding the Shape
      if (part == null) return;
      part.EnsureBounds();
      if (part.LocationElement != shape && !part.LocationSpot.Equals(Spot.Center)) {  // but only if the locationSpot isn"t Center
        // support the whole Node being rotated
        part.Move(part.Position.Subtract(offset.Rotate(part.Angle)));
      }
      UpdateAdornments(AdornedShape.Part);  // update any Adornments of the Part
      Diagram.RequestFrame(Diagram.MaybeUpdate);  // force more frequent drawing for smoother looking behavior
    }

    /// <summary>
    /// This is called by <see cref="DoMouseMove"/> and <see cref="DoMouseUp"/> to limit the input point
    /// before calling <see cref="Reshape"/>.
    /// By default, this doesn"t limit the input point.
    /// </summary>
    /// <param name="p">the point where the handle is being dragged.</param>
    /// <returns></returns>
    public Point ComputeReshape(Point p) {
      return p;  // no constraints on the points
    }
  }
}
