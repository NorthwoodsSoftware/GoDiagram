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
using System.Collections.Generic;
using System.Linq;

namespace Northwoods.Go.Tools.Extensions {

  /// <summary>
  /// The LinkShiftingTool class lets the user shift the end of a link to be anywhere along the edges of the port.
  /// </summary>
  /// <remarks>
  /// This tool may be installed as a mouse down tool:
  /// <code language="cs">
  /// myDiagram.ToolManager.MouseDownTools.Add(new LinkShiftingTool());
  /// </code>
  /// </remarks>
  /// @category Tool Extension
  public class LinkShiftingTool : Tool {
    // these are archetypes for the two shift handles, one at each end of the Link:
    private GraphObject _FromHandleArchetype;
    private GraphObject _ToHandleArchetype;

    // transient state
    private GraphObject _Handle;
    private List<Point> _OriginalPoints;

    private readonly Random _Rand = new();

    /// <summary>
    /// Constructs a LinkShiftingTool and sets the handles and name of the tool.
    /// </summary>
    public LinkShiftingTool() : base() {
      var h = new Shape {
        GeometryString = "F1 M0 0 L8 0 M8 4 L0 4",
        Fill = (Brush)null,
        Stroke = "dodgerblue",
        Background = "lightblue",
        Cursor = "pointer",
        SegmentIndex = 0,
        SegmentFraction = 1,
        SegmentOrientation = Orientation.Along
      };
      var g = new Shape {
        GeometryString = "F1 M0 0L 8 0 M8 4 L0 4",
        Fill = (Brush)null,
        Stroke = "dodgerblue",
        Background = "lightblue",
        Cursor = "pointer",
        SegmentIndex = -1,
        SegmentFraction = 1,
        SegmentOrientation = Orientation.Along
      };

      _FromHandleArchetype = h;
      _ToHandleArchetype = g;
      _OriginalPoints = null;
      Name = "LinkShifting";
    }

    /// <summary>
    /// A small GraphObject used as a shifting handle.
    /// </summary>
    public GraphObject FromHandleArchetype {
      get {
        return _FromHandleArchetype;
      }
      set {
        _FromHandleArchetype = value;
      }
    }

    /// <summary>
    /// A small GraphObject used as a shifting handle.
    /// </summary>
    public GraphObject ToHandleArchetype {
      get {
        return _ToHandleArchetype;
      }
      set {
        _ToHandleArchetype = value;
      }
    }

    /// <summary>
    /// Show an <see cref="Adornment"/> with a reshape handle at each end of the link which allows for shifting of the end points.
    /// </summary>
    public override void UpdateAdornments(Part part) {
      if (part == null || part is not Link) return;  // this tool only applies to Links
      var link = part as Link;
      // show handles if link is selected, remove them if no longer selected
      var category = "LinkShiftingFrom";
      Adornment adornment = null;
      if (link.IsSelected && !Diagram.IsReadOnly && link.FromPort != null) {
        var selelt = link.SelectionElement;
        if (selelt != null && link.ActualBounds.IsReal() && link.IsVisible() &&
          selelt.ActualBounds.IsReal() && selelt.IsVisibleElement()) {
          var spot = link.ComputeSpot(true);
          if (spot.IsSide() || spot.IsSpot()) {
            adornment = link.FindAdornment(category);
            if (adornment == null) {
              adornment = MakeAdornment(selelt, false);
              adornment.Category = category;
              link.AddAdornment(category, adornment);
            } else {
              // This is just to invalidate the measure, so it recomputes itself based on the adorned link
              adornment.SegmentFraction = _Rand.NextDouble();
            }
          }
        }
      }
      if (adornment == null) link.RemoveAdornment(category);

      category = "LinkShiftingTo";
      adornment = null;
      if (link.IsSelected && !Diagram.IsReadOnly && link.ToPort != null) {
        var selelt = link.SelectionElement;
        if (selelt != null && link.ActualBounds.IsReal() && link.IsVisible() &&
          selelt.ActualBounds.IsReal() && selelt.IsVisibleElement()) {
          var spot = link.ComputeSpot(false);
          if (spot.IsSide() || spot.IsSpot()) {
            adornment = link.FindAdornment(category);
            if (adornment == null) {
              adornment = MakeAdornment(selelt, true);
              adornment.Category = category;
              link.AddAdornment(category, adornment);
            } else {
              // This is just to invalidate the measure, so it recomputes itself based on the adorned link
              adornment.SegmentFraction = _Rand.NextDouble();
            }
          }
        }
      }
      if (adornment == null) link.RemoveAdornment(category);
    }

    /// <summary>
    /// Undocumented.
    /// </summary>
    [Undocumented]
    public virtual Adornment MakeAdornment(GraphObject selelt, bool toend) {
      var adornment = new Adornment(PanelType.Link);
      var h = (toend ? ToHandleArchetype : FromHandleArchetype);
      if (h != null) {
        // add a single handle for shifting at one end
        adornment.Add(h.Copy());
      }
      adornment.AdornedElement = selelt;
      return adornment;
    }

    /// <summary>
    /// This tool may run when there is a mouse-down event on a reshaping handle.
    /// </summary>
    public override bool CanStart() {
      if (!IsEnabled) return false;
      var diagram = Diagram;
      if (diagram.IsReadOnly || diagram.IsModelReadOnly) return false;
      if (!diagram.LastInput.Left) return false;
      var h = FindToolHandleAt(diagram.FirstInput.DocumentPoint, "LinkShiftingFrom");
      if (h == null) h = FindToolHandleAt(diagram.FirstInput.DocumentPoint, "LinkShiftingTo");
      return (h != null);
    }

    /// <summary>
    /// Start shifting, if <see cref="Tool.FindToolHandleAt"/> finds a reshaping handle at the mouse down point.
    /// </summary>
    /// <remarks>
    /// If successful this sets the handle to be the reshape handle that it finds.
    /// It also remembers the original points in case this tool is cancelled.
    /// And it starts a transaction.
    /// </remarks>
    public override void DoActivate() {
      var diagram = Diagram;
      var h = FindToolHandleAt(diagram.FirstInput.DocumentPoint, "LinkShiftingFrom");
      if (h == null) h = FindToolHandleAt(diagram.FirstInput.DocumentPoint, "LinkShiftingTo");
      if (h == null) return;
      if (h.Part is not Adornment ad || ad.AdornedElement == null) return;
      if (ad.AdornedElement.Part is not Link link) return;

      _Handle = h;
      _OriginalPoints = link.Points.ToList(); // shallow copy should work because points are structs (value type)
      StartTransaction(Name);
      diagram.IsMouseCaptured = true;
      diagram.CurrentCursor = "pointer";
      IsActive = true;
    }

    /// <summary>
    /// This stops the current shifting operation with the link as it is.
    /// </summary>
    public override void DoDeactivate() {
      IsActive = false;
      var diagram = Diagram;
      diagram.IsMouseCaptured = false;
      diagram.CurrentCursor = "";
      StopTransaction();
    }

    /// <summary>
    /// Perform cleanup of tool state.
    /// </summary>
    public override void DoStop() {
      _Handle = null;
      _OriginalPoints = null;
    }

    /// <summary>
    /// Restore the link route to be the original points and stop this tool.
    /// </summary>
    public override void DoCancel() {
      if (_Handle != null) {
        var ad = _Handle.Part as Adornment;
        if (ad.AdornedElement == null) return;
        var link = ad.AdornedElement.Part as Link;
        if (_OriginalPoints != null) link.Points = _OriginalPoints;
      }
      StopTool();
    }

    /// <summary>
    /// Call <see cref="DoReshape"/> with a new point determined by the mouse
    /// to change the end point of the link.
    /// </summary>
    public override void DoMouseMove() {
      if (IsActive) {
        DoReshape(Diagram.LastInput.DocumentPoint);
      }
    }

    /// <summary>
    /// Reshape the link's end with a point based on the most recent mouse point by calling <see cref="DoReshape"/>,
    /// and then stop this tool.
    /// </summary>
    public override void DoMouseUp() {
      if (IsActive) {
        DoReshape(Diagram.LastInput.DocumentPoint);
        TransactionResult = Name;
      }
      StopTool();
    }

    /// <summary>
    /// Find the closest point along the edge of the link's port and shift the end of the link to that point.
    /// </summary>
    public void DoReshape(Point pt) {
      if (_Handle == null) return;
      var ad = _Handle.Part as Adornment;
      if (ad.AdornedElement == null) return;
      var link = ad.AdornedElement.Part as Link;
      var fromend = ad.Category == "LinkShiftingFrom";
      GraphObject port = null;
      if (fromend) {
        port = link.FromPort;
      } else {
        port = link.ToPort;
      }
      if (port == null) return;
      // support rotated ports
      var portang = port.GetDocumentAngle();
      var center = port.GetDocumentPoint(Spot.Center);
      var farpt = pt.Offset((pt.X - center.X) * 1000, (pt.Y - center.Y) * 1000);
      var portb = new Rect(port.GetDocumentPoint(Spot.TopLeft).Subtract(center).Rotate(-portang).Add(center),
                                port.GetDocumentPoint(Spot.BottomRight).Subtract(center).Rotate(-portang).Add(center));
      var lp = link.GetLinkPointFromPoint(port.Part as Node, port, center, farpt, fromend);
      lp = lp.Subtract(center).Rotate(-portang).Add(center);
      var spot = new Spot(Math.Max(0, Math.Min(1, (lp.X - portb.X) / (!double.IsNaN(portb.Width) ? portb.Width : 1))),
                          Math.Max(0, Math.Min(1, (lp.Y - portb.Y) / (!double.IsNaN(portb.Height) ? portb.Height : 1))));
      if (fromend) {
        link.FromSpot = spot;
      } else {
        link.ToSpot = spot;
      }
    }
  }
}

