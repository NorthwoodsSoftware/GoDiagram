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

namespace Northwoods.Go.Extensions {
  /// <summary>
  /// A class for simulating mouse and keyboard input.
  /// </summary>
  /// <remarks>
  /// As a special hack, this supports limited simulation of drag-and-drop between Diagrams,
  /// by setting the SourceDiagram and TargetDiagram properties
  /// on the "eventprops" arguments of the MouseDown/MouseMove/MouseUp methods.
  /// Although <see cref="InputEvent.TargetDiagram"/> is a real property, the SourceDiagram property
  /// is only used by these Robot methods.
  /// </remarks>
  public class Robot {
    private Diagram _Diagram;

    /// <summary>
    /// Construct a Robot for a given Diagram. If none is provided, a new Diagram will be created.
    /// </summary>
    /// <param name="diagram"></param>
    public Robot(Diagram diagram = null) {
      if (diagram != null) {
        _Diagram = diagram;
      } else {
        _Diagram = new Diagram();
      }
    }

    /// <summary>
    /// Gets or sets the <see cref="Go.Diagram"/> associated with this Robot.
    /// </summary>
    public Diagram Diagram {
      get {
        return _Diagram;
      }
      set {
        if (_Diagram != value) {
          _Diagram = value;
        }
      }
    }

    /// <summary>
    /// Simulate a mouse down event.
    /// </summary>
    /// <param name="x">the X-coordinate of the mouse point in document coordinates.</param>
    /// <param name="y">the Y-coordinate of the mouse point in document coordinates.</param>
    /// <param name="time">the timestamp of the simulated event, in milliseconds; default zero</param>
    /// <param name="evtFn">an optional function to apply to the InputEvent, which can be used to set properties</param>
    /// <param name="sourceDiagram">the optional source diagram to apply the InputEvent</param>
    public void MouseDown(double x, double y, long time = 0, Action<InputEvent> evtFn = null, Diagram sourceDiagram = null) {
      var diagram = _Diagram;
      if (sourceDiagram != null) diagram = sourceDiagram;
      if (!diagram.IsEnabled) return;

      var n = new InputEvent {
        Diagram = diagram,
        DocumentPoint = new Point(x, y)
      };
      n.ViewPoint = diagram.TransformDocToView(n.DocumentPoint);

      var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      n.Timestamp = start.AddMilliseconds(time).ToLocalTime();
      n.Down = true;

      evtFn?.Invoke(n);
      diagram.LastInput = n;
      diagram.FirstInput = new InputEvent(n);
      diagram.CurrentTool.DoMouseDown();
    }

    /// <summary>
    /// Simulate a mouse move event.
    /// </summary>
    /// <param name="x">the X-coordinate of the mouse point in document coordinates.</param>
    /// <param name="y">the Y-coordinate of the mouse point in document coordinates.</param>
    /// <param name="time">the timestamp of the simulated event, in milliseconds; default zero</param>
    /// <param name="evtFn">an optional function to apply to the InputEvent, which can be used to set properties</param>
    /// <param name="sourceDiagram">the optional source diagram to apply the InputEvent</param>
    public void MouseMove(double x, double y, long time = 0, Action<InputEvent> evtFn = null, Diagram sourceDiagram = null) {
      var diagram = _Diagram;
      if (sourceDiagram != null) diagram = sourceDiagram;
      if (!diagram.IsEnabled) return;

      var n = new InputEvent {
        Diagram = diagram,
        DocumentPoint = new Point(x, y)
      };
      n.ViewPoint = diagram.TransformDocToView(n.DocumentPoint);

      var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      n.Timestamp = start.AddMilliseconds(time).ToLocalTime();

      evtFn?.Invoke(n);
      diagram.LastInput = n;
      diagram.CurrentTool.DoMouseMove();
    }

    /// <summary>
    /// Simulate a mouse up event.
    /// </summary>
    /// <param name="x">the X-coordinate of the mouse point in document coordinates.</param>
    /// <param name="y">the Y-coordinate of the mouse point in document coordinates.</param>
    /// <param name="time">the timestamp of the simulated event, in milliseconds; default zero</param>
    /// <param name="evtFn">an optional function to apply to the InputEvent, which can be used to set properties</param>
    /// <param name="sourceDiagram">the optional source diagram to apply the InputEvent</param>
    public void MouseUp(double x, double y, long time = 0, Action<InputEvent> evtFn = null, Diagram sourceDiagram = null) {
      var diagram = _Diagram;
      if (sourceDiagram != null) diagram = sourceDiagram;
      if (!diagram.IsEnabled) return;

      var n = new InputEvent {
        Diagram = diagram,
        DocumentPoint = new Point(x, y)
      };
      n.ViewPoint = diagram.TransformDocToView(n.DocumentPoint);

      var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      n.Timestamp = start.AddMilliseconds(time).ToLocalTime();
      n.Up = true;

      if (diagram.FirstInput.DocumentPoint.Equals(n.DocumentPoint)) n.ClickCount = 1;

      evtFn?.Invoke(n);
      diagram.LastInput = n;
      diagram.CurrentTool.DoMouseUp();
    }

    /// <summary>
    /// Simulate a mouse down event.
    /// </summary>
    /// <param name="delta">non-zero turn</param>
    /// <param name="time">the timestamp of the simulated event, in milliseconds; default zero</param>
    /// <param name="evtFn">an optional function to apply to the InputEvent, which can be used to set properties</param>
    public void MouseWheel(double delta, long time = 0, Action<InputEvent> evtFn = null) {
      var diagram = _Diagram;
      if (!diagram.IsEnabled) return;

      var n = new InputEvent(diagram.LastInput) {
        Diagram = diagram,
        Delta = delta
      };

      var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      n.Timestamp = start.AddMilliseconds(time).ToLocalTime();

      evtFn?.Invoke(n);
      diagram.LastInput = n;
      diagram.CurrentTool.DoMouseWheel();
    }

    /// <summary>
    /// Simulate a key down event.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="time">the timestamp of the simulated event, in milliseconds; default zero</param>
    /// <param name="evtFn">an optional function to apply to the InputEvent, which can be used to set properties</param>
    public void KeyDown(string key, long time = 0, Action<InputEvent> evtFn = null) {
      var diagram = _Diagram;
      if (!diagram.IsEnabled) return;

      var n = new InputEvent(diagram.LastInput) {
        Diagram = Diagram,
        Key = key
      };

      var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      n.Timestamp = start.AddMilliseconds(time).ToLocalTime();

      n.Down = true;
      evtFn?.Invoke(n);
      diagram.LastInput = n;
      diagram.CurrentTool.DoKeyDown();
    }

    /// <summary>
    /// Simulate a key up event.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="time">the timestamp of the simulated event, in milliseconds; default zero</param>
    /// <param name="evtFn">an optional function to apply to the InputEvent, which can be used to set properties</param>
    public void KeyUp(string key, long time = 0, Action<InputEvent> evtFn = null) {
      var diagram = _Diagram;
      if (!diagram.IsEnabled) return;
      var n = new InputEvent(diagram.LastInput) {
        Diagram = diagram,

        Key = key
      };

      var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      n.Timestamp = start.AddMilliseconds(time).ToLocalTime();

      n.Up = true;
      evtFn?.Invoke(n);
      diagram.LastInput = n;
      diagram.CurrentTool.DoKeyUp();
    }
  }
}
