using System;

/*
*  Copyright (C) 1998-2020 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main GoJS library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in the GoJS kit under the extensions or extensionsTS folders.
* See the Extensions intro page (https://gojs.Net/latest/intro/extensions.Html) for more information.
*/
namespace Northwoods.Go.Tools.Extensions {

  /// <summary>
  /// The DragCreatingTool lets the user create a new node by dragging in the background
  /// to indicate its size and position.
  ///
  /// The default drag selection box is a magenta rectangle.
  /// You can modify the <see cref="Box"/> to customize its appearance.
  ///
  /// This tool will not be able to start running unless you have set the
  /// <see cref="ArchetypeNodeData"/> property to an object that can be copied and added to the diagram"s model.
  ///
  /// You can use this tool in a modal manner by executing:
  /// ```js
  ///   diagram.CurrentTool = new DragCreatingTool();
  /// ```
  ///
  /// Use this tool in a mode-less manner by executing:
  /// ```js
  ///   myDiagram.ToolManager.MouseMoveTools.InsertAt(2, new DragCreatingTool());
  /// ```
  ///
  /// However when used mode-lessly as a mouse-move tool, in <see cref="ToolManager.MouseMoveTools"/>,
  /// this cannot start running unless there has been a motionless delay
  /// after the mouse-down event of at least <see cref="Delay"/> milliseconds.
  ///
  /// This tool does not utilize any <see cref="Adornment"/>s or tool handles,
  /// but it does temporarily add the <see cref="Box"/> Part to the diagram.
  /// This tool does conduct a transaction when inserting the new node.
  ///
  /// If you want to experiment with this extension, try the <a href="../../extensionsTS/DragCreating.Html">Drag Creating</a> sample.
  /// </summary>
  /// @category Tool Extension
  public class DragCreatingTool : Tool {
    private Part _Box;
    private object _ArchetypeNodeData;
    private TimeSpan _Delay = TimeSpan.FromMilliseconds(175);

    /// <summary>
    /// Constructs a DragCreatingTool, sets <see cref="Box"/> to a magenta rectangle, and sets name of the tool.
    /// </summary>
    public DragCreatingTool() : base() {
      Part b = new Part();
      Shape r = new Shape();
      b.LayerName = "Tool";
      b.Selectable = false;
      r.Name = "SHAPE";
      r.Figure = "Rectangle";
      r.Fill = (Brush)null;
      r.Stroke = new Brush("magenta");
      r.Position = new Point(0, 0);
      b.Add(r);
      _Box = b;
      Name = "DragCreating";
    }

    /// <summary>
    /// Gets or sets the <see cref="Part"/> used as the "rubber-band box"
    /// that is stretched to follow the mouse, as feedback for what area will
    /// be passed to <see cref="InsertPart"/> upon a mouse-up.
    ///
    /// Initially this is a <see cref="Part"/> containing only a simple magenta rectangular <see cref="Shape"/>.
    /// The object to be resized should be named "SHAPE".
    /// Setting this property does not raise any events.
    ///
    /// Modifying this property while this tool <see cref="Tool.IsActive"/> might have no effect.
    /// </summary>
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
    ///
    /// The default value is 175 milliseconds.
    /// A value of zero will allow this tool to run without any wait after the mouse down.
    /// Setting this property does not raise any events.
    /// </summary>
    public TimeSpan Delay {
      get {
        return _Delay;
      }
      set {
        _Delay = value;
      }
    }

    /// <summary>
    /// Gets or sets a data object that will be copied and added to the diagram"s model each time this tool executes.
    ///
    /// The default value is null.
    /// The value must be non-null for this tool to be able to run.
    /// Setting this property does not raise any events.
    /// </summary>
    public object ArchetypeNodeData {
      get {
        return _ArchetypeNodeData;
      }
      set {
        _ArchetypeNodeData = value;
      }
    }

    /// <summary>
    /// This tool can run when there has been a mouse-drag, far enough away not to be a click,
    /// and there has been delay of at least <see cref="Delay"/> milliseconds
    /// after the mouse-down before a mouse-move.
    /// </summary>
    public override bool CanStart() {
      if (!IsEnabled) return false;

      // gotta have some node data that can be copied
      if (ArchetypeNodeData == null) return false;

      var diagram = Diagram;
      // heed IsReadOnly & AllowInsert
      if (diagram.IsReadOnly || diagram.IsModelReadOnly) return false;
      if (!diagram.AllowInsert) return false;

      var e = diagram.LastInput;
      // require left button & that it has moved far enough away from the mouse down point, so it isn"t a click
      if (!e.Left) return false;
      // don"t include the following checks when this tool is running modally
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
      diagram.Add(Box);
      DoMouseMove();
    }

    /// <summary>
    /// Release the mouse and remove any <see cref="Box"/>.
    /// </summary>
    public override void DoDeactivate() {
      var diagram = Diagram;
      diagram.Remove(Box);
      diagram.IsMouseCaptured = false;
      IsActive = false;
    }

    /// <summary>
    /// Update the <see cref="Box"/>"s position and size according to the value
    /// of <see cref="ComputeBoxBounds"/>.
    /// </summary>
    public override void DoMouseMove() {
      if (IsActive && Box != null) {
        var r = ComputeBoxBounds();
        var shape = Box.FindElement("SHAPE");
        if (shape == null) shape = Box.FindMainElement();
        if (shape != null) shape.DesiredSize = r.Size;
        Box.Position = r.Position;
      }
    }

    /// <summary>
    /// Call <see cref="InsertPart"/> with the value of a call to <see cref="ComputeBoxBounds"/>.
    /// </summary>
    public override void DoMouseUp() {
      if (IsActive) {
        var diagram = Diagram;
        diagram.Remove(Box);
        try {
          diagram.CurrentCursor = "wait";
          InsertPart(ComputeBoxBounds());
        } finally {
          diagram.CurrentCursor = "";
        }
      }
      StopTool();
    }

    /// <summary>
    /// This just returns a <see cref="Rect"/> stretching from the mouse-down point to the current mouse point.
    /// </summary>
    /// @return {Rect} a <see cref="Rect"/> in document coordinates.
    public Rect ComputeBoxBounds() {
      var diagram = Diagram;
      var start = diagram.FirstInput.DocumentPoint;
      var latest = diagram.LastInput.DocumentPoint;
      return new Rect(start, latest);
    }

    /// <summary>
    /// Create a node by adding a copy of the <see cref="ArchetypeNodeData"/> object
    /// to the diagram"s model, assign its <see cref="GraphObject.Position"/> and <see cref="GraphObject.DesiredSize"/>
    /// according to the given bounds, and select the new part.
    ///
    /// The actual part that is added to the diagram may be a <see cref="Part"/>, a <see cref="Node"/>,
    /// or even a <see cref="Group"/>, depending on the properties of the <see cref="ArchetypeNodeData"/>
    /// and the type of the template that is copied to create the part.
    /// </summary>
    /// <param name="bounds">a Point in document coordinates.</param>
    /// <returns>the newly created Part, or null if it failed.</returns>
    public virtual Part InsertPart(Rect bounds) {
      var diagram = Diagram;
      var arch = ArchetypeNodeData;
      if (arch == null) return null;
      Part part = null;

      diagram.RaiseChangingSelection(diagram.Selection);
      StartTransaction(Name);
      if (arch != null) {
        var data = diagram.Model.CopyNodeData(arch);
        if (data != null) {
          diagram.Model.AddNodeData(data);
          part = diagram.FindPartForData(data);
        }
      }
      if (part != null) {
        part.Position = bounds.Position;
        part.ResizeElement.DesiredSize = bounds.Size;
        if (diagram.AllowSelect) {
          diagram.ClearSelection();
          part.IsSelected = true;
        }
      }

      // set the TransactionResult before raising event, in case it changes the result or cancels the tool
      TransactionResult = Name;
      StopTransaction();
      diagram.RaiseChangedSelection(diagram.Selection);
      return part;
    }
  }
}
