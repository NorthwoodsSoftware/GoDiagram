/*
*  Copyright (C) 1998-2021 by Northwoods Software Corporation. All Rights Reserved.
*/

using System;

/*
* This is an extension and not part of the main GoDiagram library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in the GoDiagram repository (https://github.com/NorthwoodsSoftware/GoDiagram/tree/main/Extensions).
* See the Extensions intro page (https://godiagram.com/intro/extensions.html) for more information.
*/

namespace Northwoods.Go.Extensions {
  /// <summary>
  /// This class implements a zoom slider for GoDiagram diagrams.
  /// The constructor has the following arguments:
  ///   - `diagram` ***Diagram*** a reference to a GoDiagram diagram
  ///   
  /// Unlike the GoJS version, this version of the ZoomSlider extension only provides an interface between
  /// a "range" input and the zoom state of a connected Diagram. As such, all properties such
  /// as size, button size, alignment, etc should be handled separately and are not passed as parameters to this Extension.
  /// 
  /// The Extension takes as parameters two delegates, which should respectively get and set the value parameter of the Range
  /// component. This is best implemented via accessors to a two-way binding property. An example of this implementation can
  /// be found in the ZoomSlider sample.
  /// </summary>
  public class ZoomSlider {
    private Diagram _Diagram;
    private double _InitialScale;

    //Function used to keep the slider up to date
    private readonly Func<double> _Get;
    private readonly Action<double> _Set;

    /// <summary>
    /// This read-only property returns the diagram for which the slider is handling zoom.
    /// </summary>
    public Diagram Diagram {
      get { return _Diagram; }
    }

    /// <summary>
    /// Modify the Diagram.Scale according to the value of the ZoomSlider.
    /// 
    /// This function should be called when the value of the ZoomSlider changes.
    /// </summary>
    public void UpdateScale() {
      _ValueToScale();
    }

    private void _ValueToScale() {
      if (Diagram == null) return;

      var x = _Get(); // Use getter delegate to access Slider value

      var A = _InitialScale;
      var B = Diagram.CommandHandler.ZoomFactor;
      Diagram.Scale = A * Math.Pow(B, x);
    }

    private void _ScaleToValue() {
      if (Diagram == null) return;

      var A = _InitialScale;
      var B = Diagram.CommandHandler.ZoomFactor;
      var y1 = Diagram.Scale;

      _Set(Math.Log(y1 / A) / Math.Log(B)); // Use setter delegate to modify Slider value
    }

    /// <summary>
    /// Constructs a ZoomSlider and sets up properties based on
    /// the options provided. Also sets up change listeners on the
    /// Diagram so the ZoomSlider stays up to date.
    /// </summary>
    /// <param name="diagram"></param>
    /// <param name="get"></param>
    /// <param name="set"></param>
    public ZoomSlider(Diagram diagram, Func<double> get, Action<double> set) {
      _Diagram = diagram;
      _Get = get;
      _Set = set;
      _InitialScale = diagram.Scale;

      _Init();
    }

    /// <summary>
    /// Initialize the ZoomSlider.
    /// </summary>
    private void _Init() {
      // Add a listener to this Diagram to call _ScaleToValue when the diagram's viewport is changed
      _Diagram.ViewportBoundsChanged += (s, e) => {
        _ScaleToValue();
      };
    }
  }
}
