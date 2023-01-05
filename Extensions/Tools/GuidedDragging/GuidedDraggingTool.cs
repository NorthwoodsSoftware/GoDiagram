/*
*  Copyright (C) 1998-2023 by Northwoods Software Corporation. All Rights Reserved.
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
  /// The GuidedDraggingTool class makes guidelines visible as the parts are dragged around a diagram
  /// when the selected part is nearly aligned with another part.
  /// </summary>
  /// @category Tool Extension
  public class GuidedDraggingTool : DraggingTool {
    // horizontal guidelines
    private readonly Part _GuidelineHtop;
    private readonly Part _GuidelineHbottom;
    private readonly Part _GuidelineHcenter;
    // vertical guidelines
    private readonly Part _GuidelineVleft;
    private readonly Part _GuidelineVright;
    private readonly Part _GuidelineVcenter;

    // properties that the programmer can modify
    private double _GuidelineSnapDistance = 6;
    private bool _IsGuidelineEnabled = true;
    private string _HorizontalGuidelineColor = "gray";
    private string _VerticalGuidelineColor = "gray";
    private string _CenterGuidelineColor = "gray";
    private double _GuidelineWidth = 1;
    private double _SearchDistance = 1000;
    private bool _IsGuidelineSnapEnabled = true;

    /// <summary>
    /// Constructs a GuidedDraggingTool and sets up the temporary guideline parts.
    /// </summary>
    public GuidedDraggingTool() : base() {
      var partProperties = new { LayerName = "Tool", IsInDocumentBounds = false };
      var shapeProperties = new { Stroke = "gray", IsGeometryPositioned = true };

      // temporary parts for horizonal guidelines
      _GuidelineHtop =
        new Part().Set(partProperties).Add(
          new Shape { GeometryString = "M0 0 100 0" }.Set(shapeProperties)
        );
      _GuidelineHbottom =
        new Part().Set(partProperties).Add(
          new Shape { GeometryString = "M0 0 100 0" }.Set(shapeProperties)
        );
      _GuidelineHcenter =
        new Part().Set(partProperties).Add(
          new Shape { GeometryString = "M0 0 100 0" }.Set(shapeProperties)
        );
      // temporary parts for vertical guidelines
      _GuidelineVleft =
        new Part().Set(partProperties).Add(
          new Shape { GeometryString = "M0 0 0 100" }.Set(shapeProperties)
        );
      _GuidelineVright =
        new Part().Set(partProperties).Add(
          new Shape { GeometryString = "M0 0 0 100" }.Set(shapeProperties)
        );
      _GuidelineVcenter =
        new Part().Set(partProperties).Add(
          new Shape { GeometryString = "M0 0 0 100" }.Set(shapeProperties)
        );
    }

    /// <summary>
    /// Gets or sets the margin of error for which guidelines show up.
    /// </summary>
    /// <remarks>
    /// The default value is 6.
    /// Guidelines will show up when the aligned nodes are ± 6px away from perfect alignment.
    /// </remarks>
    public double GuidelineSnapDistance {
      get {
        return _GuidelineSnapDistance;
      }
      set {
        if (double.IsNaN(value) || value < 0) throw new Exception("new value for GuidedDraggingTool.GuidelineSnapDistance must be a non-negative number");
        _GuidelineSnapDistance = value;
      }
    }

    /// <summary>
    /// Gets or sets whether the guidelines are enabled or disabled.
    /// </summary>
    /// <remarks>
    /// The default value is true.
    /// </remarks>
    public bool IsGuidelineEnabled {
      get {
        return _IsGuidelineEnabled;
      }
      set {
        _IsGuidelineEnabled = value;
      }
    }

    /// <summary>
    /// Gets or sets the color of horizontal guidelines.
    /// </summary>
    /// <remarks>
    /// The default value is "gray".
    /// </remarks>
    public string HorizontalGuidelineColor {
      get {
        return _HorizontalGuidelineColor;
      }
      set {
        if (_HorizontalGuidelineColor != value) {
          _HorizontalGuidelineColor = value;
          (_GuidelineHbottom.Elements.First() as Shape).Stroke = _HorizontalGuidelineColor;
          (_GuidelineHtop.Elements.First() as Shape).Stroke = _HorizontalGuidelineColor;
        }
      }
    }

    /// <summary>
    /// Gets or sets the color of vertical guidelines.
    /// </summary>
    /// <remarks>
    /// The default value is "gray".
    /// </remarks>
    public string VerticalGuidelineColor {
      get {
        return _VerticalGuidelineColor;
      }
      set {
        if (_VerticalGuidelineColor != value) {
          _VerticalGuidelineColor = value;
          (_GuidelineVleft.Elements.First() as Shape).Stroke = _VerticalGuidelineColor;
          (_GuidelineVright.Elements.First() as Shape).Stroke = _VerticalGuidelineColor;
        }
      }
    }

    /// <summary>
    /// Gets or sets the color of center guidelines.
    /// </summary>
    /// <remarks>
    /// The default value is "gray".
    /// </remarks>
    public string CenterGuidelineColor {
      get {
        return _CenterGuidelineColor;
      }
      set {
        if (_CenterGuidelineColor != value) {
          _CenterGuidelineColor = value;
          (_GuidelineVcenter.Elements.First() as Shape).Stroke = _CenterGuidelineColor;
          (_GuidelineHcenter.Elements.First() as Shape).Stroke = _CenterGuidelineColor;
        }
      }
    }

    /// <summary>
    /// Gets or sets the StrokeWidth of the guidelines.
    /// </summary>
    /// <remarks>
    /// The default value is 1.
    /// </remarks>
    public double GuidelineWidth {
      get {
        return _GuidelineWidth;
      }
      set {
        if (double.IsNaN(value) || value < 0) throw new Exception("new value for GuidedDraggingTool.GuidelineWidth must be a non-negative number.");
        if (_GuidelineWidth != value) {
          _GuidelineWidth = value;
          (_GuidelineVcenter.Elements.First() as Shape).StrokeWidth = value;
          (_GuidelineHcenter.Elements.First() as Shape).StrokeWidth = value;
          (_GuidelineVleft.Elements.First() as Shape).StrokeWidth = value;
          (_GuidelineVright.Elements.First() as Shape).StrokeWidth = value;
          (_GuidelineHbottom.Elements.First() as Shape).StrokeWidth = value;
          (_GuidelineHtop.Elements.First() as Shape).StrokeWidth = value;
        }
      }
    }

    /// <summary>
    /// Gets or sets the distance around the selected part to search for aligned parts.
    /// </summary>
    /// <remarks>
    /// The default value is 1000.
    /// Set this to double.PositiveInfinity if you want to search the entire diagram no matter how far away.
    /// </remarks>
    public double SearchDistance {
      get {
        return _SearchDistance;
      }
      set {
        if (double.IsNaN(value) || value <= 0) throw new Exception("new value for GuidedDraggingTool.SearchDistance must be a positive number.");
        _SearchDistance = value;
      }
    }

    /// <summary>
    /// Gets or sets whether snapping to guidelines is enabled.
    /// </summary>
    /// <remarks>
    /// The default value is true.
    /// </remarks>
    public bool IsGuidelineSnapEnabled {
      get {
        return _IsGuidelineSnapEnabled;
      }
      set {
        _IsGuidelineSnapEnabled = value;
      }
    }

    /// <summary>
    /// Removes all of the guidelines from the grid.
    /// </summary>
    public void ClearGuidelines() {
      Diagram.Remove(_GuidelineHbottom);
      Diagram.Remove(_GuidelineHcenter);
      Diagram.Remove(_GuidelineHtop);
      Diagram.Remove(_GuidelineVleft);
      Diagram.Remove(_GuidelineVright);
      Diagram.Remove(_GuidelineVcenter);
    }

    /// <summary>
    /// Calls the base method and removes the guidelines from the graph.
    /// </summary>
    public override void DoDeactivate() {
      base.DoDeactivate();
      // clear any guidelines when dragging is done
      ClearGuidelines();
    }

    /// <summary>
    /// Shows vertical and horizontal guidelines for the dragged part.
    /// </summary>
    public override void DoDragOver(Point pt, GraphObject obj) {
      // clear all existing guidelines in case either Show... method decides to show a guideline
      ClearGuidelines();

      // gets the selected part
      var draggingParts = CopiedParts ?? DraggedParts;
      if (draggingParts == null) return;
      foreach (var kvp in draggingParts) {
        var part = kvp.Key;

        ShowHorizontalMatches(part, IsGuidelineEnabled, false);
        ShowVerticalMatches(part, IsGuidelineEnabled, false);
      }
    }

    /// <summary>
    /// On a mouse-up, snaps the selected part to the nearest guideline.
    /// If not snapping, the part remains at its position.
    /// </summary>
    public override void DoDropOnto(Point pt, GraphObject obj) {
      ClearGuidelines();

      // gets the selected (perhaps copied) Part
      var draggingParts = CopiedParts ?? DraggedParts;
      if (draggingParts == null) return;
      foreach (var kvp in draggingParts) {
        var part = kvp.Key;

        // snaps only when the mouse is released without shift modifier
        var e = Diagram.LastInput;
        var snap = IsGuidelineSnapEnabled && !e.Shift;

        ShowHorizontalMatches(part, false, snap);  // false means don't show guidelines
        ShowVerticalMatches(part, false, snap);
      }
    }

    /// <summary>
    /// When nodes are shifted due to being guided upon a drop, make sure all connected link routes are invalidated,
    /// since the node is likely to have moved a different amount than all its connected links in the regular
    /// operation of the DraggingTool.
    /// </summary>
    public static void InvalidateLinks(Part node) {
      if (node is Node n) n.InvalidateConnectedLinks();
    }

    /// <summary>
    /// This predicate decides whether or not the given Part should guide the draggred part.
    /// </summary>
    /// <param name="part">a stationary Part to which the dragged part might be aligned</param>
    /// <param name="guidedpart">the Part being dragged</param>
    /// <returns></returns>
    protected virtual bool IsGuiding(Part part, Part guidedpart) {
      return part != null &&
        !part.IsSelected &&
        part is not Link &&
        guidedpart != null &&
        part.ContainingGroup == guidedpart.ContainingGroup &&
        part.Layer != null && !part.Layer.IsTemporary;
    }

    /// <summary>
    /// This finds parts that are aligned near the selected part along horizontal lines. It compares the selected
    /// part to all parts within a rectangle approximately twice the <see cref="SearchDistance"/> wide.
    /// The guidelines appear when a part is aligned within a margin-of-error equal to <see cref="GuidelineSnapDistance"/>.
    /// </summary>
    /// <param name="part"></param>
    /// <param name="guideline">if true, show guideline</param>
    /// <param name="snap">if true, snap the part to where the guideline would be</param>
    public void ShowHorizontalMatches(Part part, bool guideline, bool snap) {
      var objBounds = part.LocationElement.GetDocumentBounds();
      var p0 = objBounds.Y;
      var p1 = objBounds.Y + objBounds.Height / 2;
      var p2 = objBounds.Y + objBounds.Height;

      var marginOfError = GuidelineSnapDistance;
      var distance = SearchDistance;
      if (distance == double.PositiveInfinity) distance = Diagram.DocumentBounds.Width;
      // compares with parts within narrow vertical area
      var area = objBounds.Inflate(distance, marginOfError + 1);
      var otherObjs = Diagram.FindElementsIn(area,
        (obj) => obj.Part,
        (obj) => IsGuiding(obj as Part, part),
        true);

      var bestDiff = marginOfError;
      Part bestObj = null;
      var bestSpot = Spot.Default;
      var bestOtherSpot = Spot.Default;
      // horizontal line -- comparing y-values
      foreach (Part other in otherObjs) {
        if (other == part) return; // ignore itself

        var otherBounds = other.LocationElement.GetDocumentBounds();
        var q0 = otherBounds.Y;
        var q1 = otherBounds.Y + otherBounds.Height / 2;
        var q2 = otherBounds.Y + otherBounds.Height;

        // compare center with center of OTHER part
        if (Math.Abs(p1 - q1) < bestDiff) {
          bestDiff = Math.Abs(p1 - q1);
          bestObj = other;
          bestSpot = Spot.Center;
          bestOtherSpot = Spot.Center;
        }
        // compare top side with top and bottom sides of OTHER part
        if (Math.Abs(p0 - q0) < bestDiff) {
          bestDiff = Math.Abs(p0 - q0);
          bestObj = other;
          bestSpot = Spot.Top;
          bestOtherSpot = Spot.Top;
        } else if (Math.Abs(p0 - q2) < bestDiff) {
          bestDiff = Math.Abs(p0 - q2);
          bestObj = other;
          bestSpot = Spot.Top;
          bestOtherSpot = Spot.Bottom;
        }
        // compare bottom side with top and bottom sides of OTHER part
        if (Math.Abs(p2 - q0) < bestDiff) {
          bestDiff = Math.Abs(p2 - q0);
          bestObj = other;
          bestSpot = Spot.Bottom;
          bestOtherSpot = Spot.Top;
        } else if (Math.Abs(p2 - q2) < bestDiff) {
          bestDiff = Math.Abs(p2 - q2);
          bestObj = other;
          bestSpot = Spot.Bottom;
          bestOtherSpot = Spot.Bottom;
        }
      }

      if (bestObj != null) {
        var offsetX = objBounds.X - part.ActualBounds.X;
        var offsetY = objBounds.Y - part.ActualBounds.Y;
        var bestBounds = bestObj.LocationElement.GetDocumentBounds();
        // line extends from x0 to x2
        var x0 = Math.Min(objBounds.X, bestBounds.X) - 10;
        var x2 = Math.Max(objBounds.X + objBounds.Width, bestBounds.X + bestBounds.Width) + 10;
        // find bestObj's desired Y
        var bestPoint = Point.SetSpot(bestBounds, bestOtherSpot);
        if (bestSpot == Spot.Center) {
          if (snap) {
            // call Part.Move in order to automatically move member Parts of Groups
            part.Move(new Point(objBounds.X - offsetX, bestPoint.Y - objBounds.Height / 2 - offsetY));
            InvalidateLinks(part);
          }
          if (guideline) {
            _GuidelineHcenter.Position = new Point(x0, bestPoint.Y);
            _GuidelineHcenter.Elt(0).Width = x2 - x0;
            Diagram.Add(_GuidelineHcenter);
          }
        } else if (bestSpot == Spot.Top) {
          if (snap) {
            part.Move(new Point(objBounds.X - offsetX, bestPoint.Y - offsetY));
            InvalidateLinks(part);
          }
          if (guideline) {
            _GuidelineHtop.Position = new Point(x0, bestPoint.Y);
            _GuidelineHtop.Elt(0).Width = x2 - x0;
            Diagram.Add(_GuidelineHtop);
          }
        } else if (bestSpot == Spot.Bottom) {
          if (snap) {
            part.Move(new Point(objBounds.X - offsetX, bestPoint.Y - objBounds.Height - offsetY));
            InvalidateLinks(part);
          }
          if (guideline) {
            _GuidelineHbottom.Position = new Point(x0, bestPoint.Y);
            _GuidelineHbottom.Elt(0).Width = x2 - x0;
            Diagram.Add(_GuidelineHbottom);
          }
        }
      }
    }

    /// <summary>
    /// This finds parts that are aligned near the selected part along vertical lines. It compares the selected
    /// part to all parts within a rectangle approximately twice the <see cref="SearchDistance"/> tall.
    /// The guidelines appear when a part is aligned within a margin-of-error equal to <see cref="GuidelineSnapDistance"/>.
    /// </summary>
    /// <param name="part"></param>
    /// <param name="guideline">if true, show guideline</param>
    /// <param name="snap">if true, don't show guidelines but just snap the part to where the guideline would be</param>
    public void ShowVerticalMatches(Part part, bool guideline, bool snap) {
      var objBounds = part.LocationElement.GetDocumentBounds();
      var p0 = objBounds.X;
      var p1 = objBounds.X + objBounds.Width / 2;
      var p2 = objBounds.X + objBounds.Width;

      var marginOfError = GuidelineSnapDistance;
      var distance = SearchDistance;
      if (distance == double.PositiveInfinity) distance = Diagram.DocumentBounds.Height;
      // compares with parts within narrow vertical area
      var area = objBounds.Inflate(marginOfError + 1, distance);
      var otherObjs = Diagram.FindElementsIn(area,
        (obj) => obj.Part as Part,
        (obj) => IsGuiding(obj as Part, part),
        true);

      var bestDiff = marginOfError;
      Part bestObj = null;
      var bestSpot = Spot.Default;
      var bestOtherSpot = Spot.Default;
      // vertical line -- comparing x-values
      foreach (Part other in otherObjs) {
        if (other == part) return; // ignore itself

        var otherBounds = other.LocationElement.GetDocumentBounds();
        var q0 = otherBounds.X;
        var q1 = otherBounds.X + otherBounds.Width / 2;
        var q2 = otherBounds.X + otherBounds.Width;

        // compare center with center of OTHER part
        if (Math.Abs(p1 - q1) < bestDiff) {
          bestDiff = Math.Abs(p1 - q1);
          bestObj = other;
          bestSpot = Spot.Center;
          bestOtherSpot = Spot.Center;
        }
        // compare left side with left and right sides of OTHER part
        if (Math.Abs(p0 - q0) < bestDiff) {
          bestDiff = Math.Abs(p0 - q0);
          bestObj = other;
          bestSpot = Spot.Left;
          bestOtherSpot = Spot.Left;
        } else if (Math.Abs(p0 - q2) < bestDiff) {
          bestDiff = Math.Abs(p0 - q2);
          bestObj = other;
          bestSpot = Spot.Left;
          bestOtherSpot = Spot.Right;
        }
        // compare right side with left and right sides of OTHER part
        if (Math.Abs(p2 - q0) < bestDiff) {
          bestDiff = Math.Abs(p2 - q0);
          bestObj = other;
          bestSpot = Spot.Right;
          bestOtherSpot = Spot.Left;
        } else if (Math.Abs(p2 - q2) < bestDiff) {
          bestDiff = Math.Abs(p2 - q2);
          bestObj = other;
          bestSpot = Spot.Right;
          bestOtherSpot = Spot.Right;
        }
      }

      if (bestObj != null) {
        var offsetX = objBounds.X - part.ActualBounds.X;
        var offsetY = objBounds.Y - part.ActualBounds.Y;
        var bestBounds = bestObj.LocationElement.GetDocumentBounds();
        // line extends from y0 to y2
        var y0 = Math.Min(objBounds.Y, bestBounds.Y) - 10;
        var y2 = Math.Max(objBounds.Y + objBounds.Height, bestBounds.Y + bestBounds.Height) + 10;
        // find bestObj's desired X
        var bestPoint = Point.SetSpot(bestBounds, bestOtherSpot);
        if (bestSpot == Spot.Center) {
          if (snap) {
            // call Part.Move in order to automatically move member Parts of Groups
            part.Move(new Point(bestPoint.X - objBounds.Width / 2 - offsetX, objBounds.Y - offsetY));
            InvalidateLinks(part);
          }
          if (guideline) {
            _GuidelineVcenter.Position = new Point(bestPoint.X, y0);
            _GuidelineVcenter.Elt(0).Height = y2 - y0;
            Diagram.Add(_GuidelineVcenter);
          }
        } else if (bestSpot == Spot.Left) {
          if (snap) {
            part.Move(new Point(bestPoint.X - offsetX, objBounds.Y - offsetY));
            InvalidateLinks(part);
          }
          if (guideline) {
            _GuidelineVleft.Position = new Point(bestPoint.X, y0);
            _GuidelineVleft.Elt(0).Height = y2 - y0;
            Diagram.Add(_GuidelineVleft);
          }
        } else if (bestSpot == Spot.Right) {
          if (snap) {
            part.Move(new Point(bestPoint.X - objBounds.Width - offsetX, objBounds.Y - offsetY));
            InvalidateLinks(part);
          }
          if (guideline) {
            _GuidelineVright.Position = new Point(bestPoint.X, y0);
            _GuidelineVright.Elt(0).Height = y2 - y0;
            Diagram.Add(_GuidelineVright);
          }
        }
      }
    }
  }
}
