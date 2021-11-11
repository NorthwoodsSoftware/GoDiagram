using System;

/*
*  Copyright (C) 1998-2021 by Northwoods Software Corporation. All Rights Reserved.
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
  /// A custom tool for rescaling an object.
  /// </summary>
  /// <remarks>
  /// Install the RescalingTool as a mouse-down tool by calling:
  /// myDiagram.ToolManager.MouseDownTools.Add(new RescalingTool());
  /// 
  /// Normally it would not make sense for the same object to be both resizable and rescalable.
  /// 
  /// Note that there is no <code>Part.RescaleObjectName</code> property and there is no <code>Part.Rescalable</code> property.
  /// So although you cannot customize any Node to affect this tool, you can set
  /// <a>RescalingTool.RescaleObjectName</a> and set <a>RescalingTool.IsEnabled</a> to control
  /// whether objects are rescalable and when.
  /// 
  /// If you want to experiment with this extension, try the <a href="../../extensionsTS/Rescaling.html">Rescaling</a> sample.
  /// </remarks>
  public class RescalingTool : Tool {
    private GraphObject _AdornedObject;
    private Shape _HandleArchetype;
    private GraphObject _Handle;
    private string _RescaleObjectName;
    private Point OriginalPoint;
    private Point OriginalTopLeft;
    private double OriginalScale;

    /// <summary>
    /// Constructs a RescalingTool.
    /// </summary>
    public RescalingTool() : base() {
      RescaleObjectName = "";
      AdornedObject = null;
      Handle = null;
      OriginalPoint = new Point();
      OriginalTopLeft = new Point();
      OriginalScale = 1.0;
      Name = "Rescaling";
      var h = new Shape() {
        DesiredSize = new Size(8, 8),
        Fill = "lightblue",
        Stroke = "didgerblue",
        StrokeWidth = 1,
        Cursor = "nwse-resize"
      };
      HandleArchetype = h;
    }

    /// <summary>
    /// Gets the <see cref="GraphObject"/> that is being rescaled.
    /// </summary>
    /// <remarks>
    /// This may be the same object as the selected <see cref="Part"/> or it may be contained within that Part.
    ///
    /// This property is also settable, but should only be set when overriding functions
    /// in RescalingTool, and not during normal operation.
    /// </remarks>
    public GraphObject AdornedObject {
      get {
        return _AdornedObject;
      }
      set {
        _AdornedObject = value;
      }
    }

    /// <summary>
    /// Gets or sets a small GraphObject that is copied as a rescale handle for the selected part.
    /// </summary>
    /// <remarks>
    /// By default this is a <see cref="Shape"/> that is a small blue square.
    /// Setting this property does not raise any events.
    /// 
    /// Here is an example of changing the default handle to be green "X":
    /// <code language="cs">
    ///   tool.HandleArchetype =
    ///     new Shape("XLine")
    ///       { Width = 8, Height = 8, Stroke = "green", Fill = "transparent" };
    /// </code>
    /// </remarks>
    public Shape HandleArchetype {
      get {
        return _HandleArchetype;
      }
      set {
        _HandleArchetype = value;
      }
    }

    /// <summary>
    /// This property returns the <see cref="GraphObject"/> that is the tool handle being dragged by the user.
    /// </summary>
    /// <remarks>
    /// This will be contained by an <see cref="Adornment"/> whose category is "RescalingTool".
    /// Its <see cref="Adornment.AdornedElement"/> is the same as the <see cref="AdornedObject"/>.
    ///
    /// This property is also settable, but should only be set either within an override of <see cref="DoActivate"/>
    /// or prior to calling <see cref="DoActivate"/>.
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
    /// This property returns the name of the GraphObject that identifies the object to be rescaled by this tool.
    /// </summary>
    /// <remarks>
    /// The default value is the empty string, resulting in the whole Node being rescaled.
    /// This property is used by findRescaleObject when calling <see cref="Panel.FindElement(string)"/>.
    /// </remarks>
    public string RescaleObjectName {
      get {
        return _RescaleObjectName;
      }
      set {
        _RescaleObjectName = value;
      }
    }

    /// <inheritdoc/>
    public override void UpdateAdornments(Part part) {
      if (part == null || part is Link link) return;
      if (part.IsSelected && !Diagram.IsReadOnly) {
        var rescaleObj = FindRescaleObject(part);
        if (rescaleObj != null && part.ActualBounds.IsReal() && part.IsVisible() 
          && rescaleObj.ActualBounds.IsReal() && rescaleObj.IsVisibleElement()) {
          var adornment = part.FindAdornment(Name);
          if (adornment == null || adornment.AdornedElement != rescaleObj) {
            adornment = MakeAdornment(rescaleObj);
          }
          if (adornment != null) {
            adornment.Location = rescaleObj.GetDocumentPoint(Spot.BottomRight);
            part.AddAdornment(Name, adornment);
            return;
          }
        }
      }
      part.RemoveAdornment(Name);
    }

    /// <summary>
    /// Undocumented.
    /// </summary>
    [Undocumented]
    public Adornment MakeAdornment(GraphObject rescaleObj) {
      var adornment = new Adornment();
      adornment.Type = PanelLayoutPosition.Instance;
      adornment.LocationSpot = Spot.Center;
      adornment.Add(_HandleArchetype.Copy());
      adornment.AdornedElement = rescaleObj;
      return adornment;
    }

    /// <summary>
    /// Return the GraphObject to be rescaled by the user.
    /// </summary>
    /// <param name="part"></param>
    /// <returns></returns>
    public GraphObject FindRescaleObject(Part part) {
      var obj = part.FindElement(RescaleObjectName);
      if (obj != null) return obj;
      return part;
    }

    /// <summary>
    /// This tool can start running if the mouse-down happens on a "Rescaling" handle.
    /// </summary>
    public override bool CanStart() {
      var diagram = Diagram;
      if (diagram == null || diagram.IsReadOnly) return false;
      if (!diagram.LastInput.Left) return false;
      var h = FindToolHandleAt(diagram.FirstInput.DocumentPoint, Name);
      return (h != null);
    }

    /// <summary>
    /// Activating this tool remembers the <see cref="Handle"/> that was dragged,
    /// the <see cref="AdornedObject"/> that is being rescaled,
    /// starts a transaction, and captures the mouse.
    /// </summary>
    public override void DoActivate() {
      var diagram = Diagram;
      if (diagram == null) return;
      _Handle = FindToolHandleAt(diagram.FirstInput.DocumentPoint, Name);
      if (_Handle == null) return;
      var ad = _Handle.Part;
      _AdornedObject = ad is Adornment adorn ? (ad as Adornment).AdornedElement : null;
      if (_AdornedObject == null) return;
      OriginalPoint = _Handle.GetDocumentPoint(Spot.Center);
      OriginalTopLeft = _AdornedObject.GetDocumentPoint(Spot.TopLeft);
      OriginalScale = _AdornedObject.Scale;
      diagram.IsMouseCaptured = true;
      diagram.DelaysLayout = true;
      StartTransaction(Name);
      IsActive = true;
    }

    /// <summary>
    /// Stop the current transaction, forget the <see cref="Handle"/> and <see cref="AdornedObject"/>, and release the mouse.
    /// </summary>
    public override void DoDeactivate() {
      var diagram = Diagram;
      if (diagram == null) return;
      StopTransaction();
      _Handle = null;
      _AdornedObject = null;
      diagram.IsMouseCaptured = false;
      IsActive = false;
    }

    /// <summary>
    /// Restore the original <see cref="GraphObject.Scale"/> of the adorned object.
    /// </summary>
    public override void DoCancel() {
      var diagram = Diagram;
      if (diagram != null) diagram.DelaysLayout = false;
      Scale(OriginalScale);
      StopTool();
    }

    /// <summary>
    /// Call <see cref="Scale(double)"/> with a new scale determined by the current mouse point.
    /// This determines the new scale by calling <see cref="ComputeScale(Point)"/>.
    /// </summary>
    public override void DoMouseMove() {
      var diagram = Diagram;
      if (IsActive && diagram != null) {
        var newScale = ComputeScale(diagram.LastInput.DocumentPoint);
        Scale(newScale);
      }
    }

    /// <summary>
    /// Call <see cref="Scale(double)"/> with a new scale determined by the most recent mouse point,
    /// and commit the transaction.
    /// </summary>
    public override void DoMouseUp() {
      var diagram = Diagram;
      if (IsActive && diagram != null) {
        diagram.DelaysLayout = false;
        var newScale = ComputeScale(diagram.LastInput.DocumentPoint);
        Scale(newScale);
        TransactionResult = Name;
      }
      StopTool();
    }

    /// <summary>
    /// Set the <see cref="GraphObject.Scale"/> of the <see cref="FindRescaleObject(Part)"/>.
    /// </summary>
    public void Scale(double newScale) {
      if (_AdornedObject != null) _AdornedObject.Scale = newScale;
    }

    /// <summary>
    /// Compute the new scale given a point.
    /// </summary>
    /// <remarks>
    /// This method is called by both <see cref="DoMouseMove"/> and <see cref="DoMouseUp"/>.
    /// This method may be overridden.
    /// Please read the Introduction page on <a href="../../intro/extensions.html">Extensions</a> for how to override methods and how to call this base method.
    /// </remarks>
    public double ComputeScale(Point newPoint) {
      var scale = OriginalScale;
      var origdist = Math.Sqrt(OriginalPoint.DistanceSquared(OriginalTopLeft));
      var newdist = Math.Sqrt(newPoint.DistanceSquared(OriginalTopLeft));
      return scale * (newdist / origdist);
    }
  }
  
}
