using System;
using System.Linq;

/*
*  Copyright (C) 1998-2020 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main Go library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in GoExamples under the Extensions folder.
* See the Extensions intro page (<replace>) for more information.
*/

namespace Northwoods.Go.Tools.Extensions {


  /// <summary>
  /// The GuidedDraggingTool class makes guidelines visible as the parts are dragged around a diagram
  /// when the selected part is nearly aligned with another part.
  ///
  /// If you want to experiment with this extension, try the <a href="../../extensionsTS/GuidedDragging.Html">Guided Dragging</a> sample.
  /// </summary>
  /// @category Tool Extension
  public class GuidedDraggingTool : DraggingTool {
    // horizontal guidelines
    private Part GuidelineHtop;
    private Part GuidelineHbottom;
    private Part GuidelineHcenter;
    // vertical guidelines
    private Part GuidelineVleft;
    private Part GuidelineVright;
    private Part GuidelineVcenter;

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
      GuidelineHtop =
        new Part().Set(partProperties).Add(
          new Shape { GeometryString = "M0 0 100 0" }.Set(shapeProperties)
        );
      GuidelineHbottom =
        new Part().Set(partProperties).Add(
          new Shape { GeometryString = "M0 0 100 0" }.Set(shapeProperties)
        );
      GuidelineHcenter =
        new Part().Set(partProperties).Add(
          new Shape { GeometryString = "M0 0 100 0" }.Set(shapeProperties)
        );
      // temporary parts for vertical guidelines
      GuidelineVleft =
        new Part().Set(partProperties).Add(
          new Shape { GeometryString = "M0 0 0 100" }.Set(shapeProperties)
        );
      GuidelineVright =
        new Part().Set(partProperties).Add(
          new Shape { GeometryString = "M0 0 0 100" }.Set(shapeProperties)
        );
      GuidelineVcenter =
        new Part().Set(partProperties).Add(
          new Shape { GeometryString = "M0 0 0 100" }.Set(shapeProperties)
        );
    }

    /// <summary>
    /// Gets or sets the margin of error for which guidelines show up.
    ///
    /// The default value is 6.
    /// Guidelines will show up when the aligned nods are ± 6px away from perfect alignment.
    /// </summary>
    public double GuidelineSnapDistance {
      get {
        return _GuidelineSnapDistance;
      }
      set {
        if (double.IsNaN(value) || value < 0) throw new Exception("new value for GuideddraggingTool.GuidelineSnapDistance must be a non-negative number");
        if (_GuidelineSnapDistance != value) {
          _GuidelineSnapDistance = value;
        }
      }
    }

    /// <summary>
    /// Gets or sets whether the guidelines are enabled or disable.
    ///
    /// The default value is true.
    /// </summary>
    public bool IsGuidelineEnabled {
      get {
        return _IsGuidelineEnabled;
      }
      set {
        if (_IsGuidelineEnabled != value) {
          _IsGuidelineEnabled = value;
        }
      }
    }

    /// <summary>
    /// Gets or sets the color of horizontal guidelines.
    ///
    /// The default value is "gray".
    /// </summary>
    public string HorizontalGuidelineColor {
      get {
        return _HorizontalGuidelineColor;
      }
      set {
        if (_HorizontalGuidelineColor != value) {
          _HorizontalGuidelineColor = value;
          (GuidelineHbottom.Elements.First() as Shape).Stroke = _HorizontalGuidelineColor;
          (GuidelineHtop.Elements.First() as Shape).Stroke = _HorizontalGuidelineColor;
        }
      }
    }

    /// <summary>
    /// Gets or sets the color of vertical guidelines.
    ///
    /// The default value is "gray".
    /// </summary>
    public string VerticalGuidelineColor {
      get {
        return _VerticalGuidelineColor;
      }
      set {
        if (_VerticalGuidelineColor != value) {
          _VerticalGuidelineColor = value;
          (GuidelineVleft.Elements.First() as Shape).Stroke = _VerticalGuidelineColor;
          (GuidelineVright.Elements.First() as Shape).Stroke = _VerticalGuidelineColor;
        }
      }
    }

    /// <summary>
    /// Gets or sets the color of center guidelines.
    ///
    /// The default value is "gray".
    /// </summary>
    public string CenterGuidelineColor {
      get {
        return _CenterGuidelineColor;
      }
      set {
        if (_CenterGuidelineColor != value) {
          _CenterGuidelineColor = value;
          (GuidelineVcenter.Elements.First() as Shape).Stroke = _CenterGuidelineColor;
          (GuidelineHcenter.Elements.First() as Shape).Stroke = _CenterGuidelineColor;
        }
      }
    }

    /// <summary>
    /// Gets or sets the width guidelines.
    ///
    /// The default value is 1.
    /// </summary>
    public double GuidelineWidth {
      get {
        return _GuidelineWidth;
      }
      set {
        if (double.IsNaN(value) || value < 0) throw new Exception("new value for GuidedDraggingTool.GuidelineWidth must be a non-negative number.");
        if (_GuidelineWidth != value) {
          _GuidelineWidth = value;
          (GuidelineVcenter.Elements.First() as Shape).StrokeWidth = value;
          (GuidelineHcenter.Elements.First() as Shape).StrokeWidth = value;
          (GuidelineVleft.Elements.First() as Shape).StrokeWidth = value;
          (GuidelineVright.Elements.First() as Shape).StrokeWidth = value;
          (GuidelineHbottom.Elements.First() as Shape).StrokeWidth = value;
          (GuidelineHtop.Elements.First() as Shape).StrokeWidth = value;
        }
      }
    }

    /// <summary>
    /// Gets or sets the distance around the selected part to search for aligned parts.
    ///
    /// The default value is 1000.
    /// Set this to double.PositiveInfinity if you want to search the entire diagram no matter how far away.
    /// </summary>
    public double SearchDistance {
      get {
        return _SearchDistance;
      }
      set {
        if (double.IsNaN(value) || value <= 0) throw new Exception("new value for GuidedDraggingTool.SearchDistance must be a positive number.");
        if (_SearchDistance != value) {
          _SearchDistance = value;
        }
      }
    }

    /// <summary>
    /// Gets or sets whether snapping to guidelines is enabled.
    ///
    /// The default value is true.
    /// </summary>
    public bool IsGuidelineSnapEnabled {
      get {
        return _IsGuidelineSnapEnabled;
      }
      set {
        if (_IsGuidelineSnapEnabled != value) {
          _IsGuidelineSnapEnabled = value;
        }
      }
    }

    /// <summary>
    /// Removes all of the guidelines from the grid.
    /// </summary>
    public void ClearGuidelines() {
      Diagram.Remove(GuidelineHbottom);
      Diagram.Remove(GuidelineHcenter);
      Diagram.Remove(GuidelineHtop);
      Diagram.Remove(GuidelineVleft);
      Diagram.Remove(GuidelineVright);
      Diagram.Remove(GuidelineVcenter);
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
      // clear all existing guidelines in case either show... method decides to show a guideline
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
    public void InvalidateLinks(Part node) {
      if (node is Node n) n.InvalidateConnectedLinks();
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
      // compares with parts within narrow vertical area
      var area = objBounds.Inflate(distance, marginOfError + 1);
      var otherObjs = Diagram.FindElementsIn(area,
        (obj) => obj.Part,
        (obj) => obj is Part p && !p.IsSelected && !(p is Link) && p.IsTopLevel && p.Layer != null && !p.Layer.IsTemporary,
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
            GuidelineHcenter.Position = new Point(x0, bestPoint.Y);
            GuidelineHcenter.Elt(0).Width = x2 - x0;
            Diagram.Add(GuidelineHcenter);
          }
        } else if (bestSpot == Spot.Top) {
          if (snap) {
            part.Move(new Point(objBounds.X - offsetX, bestPoint.Y - offsetY));
            InvalidateLinks(part);
          }
          if (guideline) {
            GuidelineHtop.Position = new Point(x0, bestPoint.Y);
            GuidelineHtop.Elt(0).Width = x2 - x0;
            Diagram.Add(GuidelineHtop);
          }
        } else if (bestSpot == Spot.Bottom) {
          if (snap) {
            part.Move(new Point(objBounds.X - offsetX, bestPoint.Y - objBounds.Height - offsetY));
            InvalidateLinks(part);
          }
          if (guideline) {
            GuidelineHbottom.Position = new Point(x0, bestPoint.Y);
            GuidelineHbottom.Elt(0).Width = x2 - x0;
            Diagram.Add(GuidelineHbottom);
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
    /// <param name="snap">if true, don"t show guidelines but just snap the part to where the guideline would be</param>
    public void ShowVerticalMatches(Part part, bool guideline, bool snap) {
      var objBounds = part.LocationElement.GetDocumentBounds();
      var p0 = objBounds.X;
      var p1 = objBounds.X + objBounds.Width / 2;
      var p2 = objBounds.X + objBounds.Width;

      var marginOfError = GuidelineSnapDistance;
      var distance = SearchDistance;
      // compares with parts within narrow vertical area
      var area = objBounds.Inflate(marginOfError + 1, distance);
      var otherObjs = Diagram.FindElementsIn(area,
        (obj) => obj.Part as Part,
        (obj) => obj is Part p && !p.IsSelected && !(p is Link) && p.IsTopLevel && p.Layer != null && !p.Layer.IsTemporary,
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
        // find bestObj"s desired X
        var bestPoint = Point.SetSpot(bestBounds, bestOtherSpot);
        if (bestSpot == Spot.Center) {
          if (snap) {
            // call Part.Move in order to automatically move member Parts of Groups
            part.Move(new Point(bestPoint.X - objBounds.Width / 2 - offsetX, objBounds.Y - offsetY));
            InvalidateLinks(part);
          }
          if (guideline) {
            GuidelineVcenter.Position = new Point(bestPoint.X, y0);
            GuidelineVcenter.Elt(0).Height = y2 - y0;
            Diagram.Add(GuidelineVcenter);
          }
        } else if (bestSpot == Spot.Left) {
          if (snap) {
            part.Move(new Point(bestPoint.X - offsetX, objBounds.Y - offsetY));
            InvalidateLinks(part);
          }
          if (guideline) {
            GuidelineVleft.Position = new Point(bestPoint.X, y0);
            GuidelineVleft.Elt(0).Height = y2 - y0;
            Diagram.Add(GuidelineVleft);
          }
        } else if (bestSpot == Spot.Right) {
          if (snap) {
            part.Move(new Point(bestPoint.X - objBounds.Width - offsetX, objBounds.Y - offsetY));
            InvalidateLinks(part);
          }
          if (guideline) {
            GuidelineVright.Position = new Point(bestPoint.X, y0);
            GuidelineVright.Elt(0).Height = y2 - y0;
            Diagram.Add(GuidelineVright);
          }
        }
      }
    }
  }
}
