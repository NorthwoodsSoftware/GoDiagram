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

using System;

namespace Northwoods.Go.Tools.Extensions {
  /// <summary>
  /// The DragZoomingTool lets the user zoom into a diagram by stretching a box
  /// to indicate the new contents of the diagram's viewport (the area of the
  /// model shown by the Diagram).
  /// Hold down the Shift key in order to zoom out.
  /// </summary>
  /// <remarks>
  /// The default drag selection box is a magenta rectangle.
  /// You can modify the <see cref="Box"/> to customize its appearance.
  ///
  /// The diagram that is zoomed by this tool is specified by the <see cref="ZoomedDiagram"/> property.
  /// If the value is null, the tool zooms its own <see cref="Tool.Diagram"/>.
  ///
  /// You can use this tool in a modal manner by executing:
  /// <code language="cs">
  ///   diagram.CurrentTool = new DragZoomingTool();
  /// </code>
  ///
  /// Use this tool in a mode-less manner by executing:
  /// <code language="cs">
  ///   myDiagram.ToolManager.MouseMoveTools.InsertAt(2, new DragZoomingTool());
  /// </code>
  ///
  /// However when used mode-lessly as a mouse-move tool, in <see cref="ToolManager.MouseMoveTools"/>,
  /// this cannot start running unless there has been a motionless delay
  /// after the mouse-down event of at least <see cref="Delay"/> milliseconds.
  ///
  /// This tool does not utilize any <see cref="Adornment"/>s or tool handles,
  /// but it does temporarily add the <see cref="Box"/> part to the diagram.
  /// This tool does not modify the model or conduct any transaction.
  /// </remarks>
  /// @category Tool Extension
  public class DragZoomingTool : Tool {
    private Part _Box;
    private TimeSpan _Delay = TimeSpan.FromMilliseconds(175);
    private Diagram _ZoomedDiagram;

    /// <summary>
    /// Constructs a DragZoomingTool, sets <see cref="Box"/> to a magenta rectangle, and sets name of the tool.
    /// </summary>
    public DragZoomingTool() : base() {
      var b = new Part();
      var r = new Shape();
      b.LayerName = "Tool";
      b.Selectable = false;
      r.Name = "SHAPE";
      r.Figure = "Rectangle";
      r.Fill = null;
      r.Stroke = "magenta";
      r.Position = new Point(0, 0);
      b.Add(r);
      _Box = b;
      Name = "DragZooming";
    }

    /// <summary>
    /// Gets or sets the <see cref="Part"/> used as the "rubber-band zoom box"
    /// that is stretched to follow the mouse, as feedback for what area will
    /// be passed to <see cref="ZoomToRect"/> upon a mouse-up.
    /// </summary>
    /// <remarks>
    /// Initially this is a <see cref="Part"/> containing only a simple magenta rectangular <see cref="Shape"/>.
    /// The object to be resized should be named "SHAPE".
    /// Setting this property does not raise any events.
    ///
    /// Modifying this property while this tool <see cref="Tool.IsActive"/> might have no effect.
    /// </remarks>
    public Part Box {
      get {
        return _Box;
      }
      set {
        _Box = value;
      }
    }

    /// <summary>
    /// Gets or sets the TimeSpan for which the mouse must be stationary
    /// before this tool can be started.
    /// </summary>
    /// <remarks>
    /// The default value is 175 milliseconds.
    /// Setting this property does not raise any events.
    /// </remarks>
    public TimeSpan Delay {
      get {
        return _Delay;
      }
      set {
        _Delay = value;
      }
    }

    /// <summary>
    /// Gets or sets the <see cref="Diagram"/> whose <see cref="Diagram.Position"/> and <see cref="Diagram.Scale"/>
    /// should be set to display the drawn <see cref="Box"/> rectangular bounds.
    /// </summary>
    /// <remarks>
    /// The default value is null, which causes <see cref="ZoomToRect"/> to modify this tool's <see cref="Tool.Diagram"/>.
    /// Setting this property does not raise any events.
    /// </remarks>
    public Diagram ZoomedDiagram {
      get {
        return _ZoomedDiagram;
      }
      set {
        _ZoomedDiagram = value;
      }
    }

    /// <summary>
    /// This tool can run when there has been a mouse-drag, far enough away not to be a click,
    /// and there has been delay of at least <see cref="Delay"/> milliseconds
    /// after the mouse-down before a mouse-move.
    /// </summary>
    public override bool CanStart() {
      if (!IsEnabled) return false;
      var diagram = Diagram;
      var e = diagram.LastInput;
      // require left button & that it has moved far enough away from the mouse down point, so it isn't a click
      if (!e.Left) return false;
      // don't include the following checks when this tool is running modally
      if (diagram.CurrentTool != this) {
        if (!IsBeyondDragSize()) return false;
        // must wait for "delay" milliseconds before that tool can run
        if (e.Timestamp - diagram.FirstInput.Timestamp < Delay) return false;
      }
      return true;
    }

    /// <summary>
    /// Capture the mouse and show the <see cref="Box"/>.
    /// </summary>
    public override void DoActivate() {
      var diagram = Diagram;
      IsActive = true;
      diagram.IsMouseCaptured = true;
      diagram.SkipsUndoManager = true;
      diagram.Add(Box);
      DoMouseMove();
    }

    /// <summary>
    /// Release the mouse and remove any <see cref="Box"/>.
    /// </summary>
    public override void DoDeactivate() {
      var diagram = Diagram;
      diagram.Remove(Box);
      diagram.SkipsUndoManager = false;
      diagram.IsMouseCaptured = false;
      IsActive = false;
    }

    /// <summary>
    /// Update the <see cref="Box"/>'s position and size according to the value
    /// of <see cref="ComputeBoxBounds"/>.
    /// </summary>
    public override void DoMouseMove() {
      var diagram = Diagram;
      if (IsActive && Box != null) {
        var r = ComputeBoxBounds();
        var shape = Box.FindElement("SHAPE");
        if (shape == null) shape = Box.FindMainElement();
        if (shape != null) shape.DesiredSize = r.Size;
        Box.Position = r.Position;
      }
    }

    /// <summary>
    /// Call <see cref="ZoomToRect"/> with the value of a call to <see cref="ComputeBoxBounds"/>.
    /// </summary>
    public override void DoMouseUp() {
      if (IsActive) {
        var diagram = Diagram;
        diagram.Remove(Box);
        try {
          diagram.CurrentCursor = "wait";
          ZoomToRect(ComputeBoxBounds());
        } finally {
          diagram.CurrentCursor = "";
        }
      }
      StopTool();
    }

    /// <summary>
    /// This just returns a <see cref="Rect"/> stretching from the mouse-down point to the current mouse point
    /// while maintaining the aspect ratio of the <see cref="ZoomedDiagram"/>.
    /// </summary>
    /// <returns>a <see cref="Rect"/> in document coordinates.</returns>
    public Rect ComputeBoxBounds() {
      var diagram = Diagram;
      var start = diagram.FirstInput.DocumentPoint;
      var latest = diagram.LastInput.DocumentPoint;
      var adx = latest.X - start.X;
      var ady = latest.Y - start.Y;

      var observed = ZoomedDiagram;
      if (observed == null) observed = diagram;
      if (observed == null) {
        return new Rect(start, latest);
      }
      var vrect = observed.ViewportBounds;
      if (vrect.Height == 0 || ady == 0) {
        return new Rect(start, latest);
      }

      var vratio = vrect.Width / vrect.Height;
      double lx;
      double ly;
      if (Math.Abs(adx / ady) < vratio) {
        lx = start.X + adx;
        ly = start.Y + Math.Ceiling(Math.Abs(adx) / vratio) * (ady < 0 ? -1 : 1);
      } else {
        lx = start.X + Math.Ceiling(Math.Abs(ady) * vratio) * (adx < 0 ? -1 : 1);
        ly = start.Y + ady;
      }
      return new Rect(start, new Point(lx, ly));
    }

    /// <summary>
    /// This method is called to change the <see cref="ZoomedDiagram"/>'s viewport to match the given rectangle.
    /// </summary>
    /// <param name="r">a rectangular bounds in document coordinates.</param>
    public void ZoomToRect(Rect r) {
      if (r.Width < 0.1) return;
      var diagram = Diagram;
      var observed = ZoomedDiagram;
      if (observed == null) observed = diagram;
      if (observed == null) return;

      // zoom out when using the Shift modifier
      if (diagram.LastInput.Shift) {
        observed.Commit((d) => {
          observed.Scale = Math.Max(observed.Scale * r.Width / observed.ViewportBounds.Width, observed.MinScale);
          observed.CenterRect(r);
        }, null);
      } else {
        // do scale first, so the Diagram's position normalization isn't constrained unduly when increasing scale
        observed.Commit((d) => {
          observed.Scale = Math.Min(observed.ViewportBounds.Width * observed.Scale / r.Width, observed.MaxScale);
          observed.Position = new Point(r.X, r.Y);
        }, null);
      }
    }
  }
}
