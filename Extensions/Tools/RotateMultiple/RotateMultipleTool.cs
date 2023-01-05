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
using System.Collections.Generic;

namespace Northwoods.Go.Tools.Extensions {

  /// <summary>
  /// The RotateMultipleTool class lets the user rotate multiple objects at a time.
  /// </summary>
  /// <remarks>
  /// When more than one part is selected, rotates all parts, revolving them about their collective center.
  /// If the control key is held down during rotation, rotates all parts individually.
  ///
  /// Caution: this only works for Groups that do *not* have a Placeholder.
  /// </remarks>
  /// @category Tool Extension
  public class RotateMultipleTool : RotatingTool {
    private Dictionary<Part, PartInfo> _InitialInfo;  // Holds references to all selected non-Link Parts and their offset & angles
    private double _InitialAngle = 0;  // Initial angle when rotating as a whole
    private Point _CenterPoint;  // Rotation point of selection

    /// <summary>
    /// Constructs a RotateMultipleTool and sets the name for the tool.
    /// </summary>
    public RotateMultipleTool() : base() {
      _InitialAngle = 0;
      _InitialInfo = new Dictionary<Part, PartInfo>();
      _CenterPoint = new Point();
      Name = "RotateMultiple";
    }

    /// <summary>
    /// Calls <see cref="RotatingTool.DoActivate"/>, and then remembers the center point of the collection,
    /// and the initial distances and angles of selected parts to the center.
    /// </summary>
    public override void DoActivate() {
      base.DoActivate();
      var diagram = Diagram;
      // center point of the collection
      _CenterPoint = diagram.ComputePartsBounds(diagram.Selection).Center;

      // remember the angle relative to the center point when rotating the whole collection
      _InitialAngle = _CenterPoint.Direction(diagram.LastInput.DocumentPoint);

      // remember initial angle and distance for each Part
      var infos = new Dictionary<Part, PartInfo>();
      var tool = this;
      foreach (var part in diagram.Selection) {
        tool.WalkTree(part, infos);
      }
      _InitialInfo = infos;

      // forget the RotationPoint since we use _CenterPoint instead
      RotationPoint = new Point(double.NaN, double.NaN);
    }

    private void WalkTree(Part part, Dictionary<Part, PartInfo> infos) {
      if (part == null || part is Link) return;
      // distance from _CenterPoint to LocationSpot of part
      var dist = Math.Sqrt(_CenterPoint.DistanceSquared(part.Location));
      // calculate initial relative angle
      var dir = _CenterPoint.Direction(part.Location);
      // saves part-angle combination in array
      infos.Add(part, new PartInfo(dir, dist, part.RotateElement.Angle));
      // recurse into Groups
      if (part is Group) {
        var it = (part as Group).MemberParts.GetEnumerator();
        while (it.MoveNext()) WalkTree(it.Current, infos);
      }
    }

    /// <summary>
    /// Clean up any references to Parts.
    /// </summary>
    public override void DoDeactivate() {
      _InitialInfo = null;
      base.DoDeactivate();
    }

    /// <summary>
    /// Rotate all selected objects about their collective center.
    /// When the control key is held down while rotating, all selected objects are rotated individually.
    /// </summary>
    public override void Rotate(double newangle) {
      var diagram = Diagram;
      if (_InitialInfo == null) return;
      var node = AdornedElement?.Part;
      if (node == null) return;
      var e = diagram.LastInput;
      // when rotating individual parts, remember the original angle difference
      var angleDiff = newangle - node.RotateElement.Angle;
      var tool = this;
      foreach (var kvp in _InitialInfo) {
        var part = kvp.Key;
        if (part is Link) return; // only Nodes and simple Parts
        var partInfo = kvp.Value;
        // rotate every selected non-Link Part
        // find information about the part set in RotateMultipleTool._InitialInformation
        if (e.Control || e.Meta) {
          if (node == part) {
            part.RotateElement.Angle = newangle;
          } else {
            part.RotateElement.Angle += angleDiff;
          }
        } else {
          var radAngle = newangle * (Math.PI / 180); // converts the angle traveled from degrees to radians
          // calculate the part's x-y location relative to the central rotation point
          var offsetX = partInfo.Distance * Math.Cos(radAngle + partInfo.PlacementAngle);
          var offsetY = partInfo.Distance * Math.Sin(radAngle + partInfo.PlacementAngle);
          // move part
          part.Location = new Point(tool._CenterPoint.X + offsetX, tool._CenterPoint.Y + offsetY);
          // rotate part
          part.RotateElement.Angle = partInfo.RotationAngle + newangle;
        }
      }
    }

    /// <summary>
    /// Calculate the desired angle with different rotation points,
    /// depending on whether we are rotating the whole selection as one, or Parts individually.
    /// </summary>
    /// <param name="newPoint">in document coordinates</param>
    public override double ComputeRotate(Point newPoint) {
      var diagram = Diagram;
      if (AdornedElement == null) return 0.0;
      var angle = 0.0;
      var e = diagram.LastInput;
      if (e.Control || e.Meta) {  // relative to the center of the Node whose handle we are rotating
        var part = AdornedElement.Part;
        if (part != null) {
          var rotationPoint = part.GetDocumentPoint(part.LocationSpot);
          angle = rotationPoint.Direction(newPoint);
        }
      } else {  // relative to the center of the whole selection
        angle = _CenterPoint.Direction(newPoint) - _InitialAngle;
      }
      if (angle >= 360) angle -= 360;
      else if (angle < 0) angle += 360;
      var interval = Math.Min(Math.Abs(SnapAngleMultiple), 180);
      var epsilon = Math.Min(Math.Abs(SnapAngleEpsilon), interval / 2);
      // if it's close to a multiple of INTERVAL degrees, make it exactly so
      if (!diagram.LastInput.Shift && interval > 0 && epsilon > 0) {
        if (angle % interval < epsilon) {
          angle = Math.Floor(angle / interval) * interval;
        } else if (angle % interval > interval - epsilon) {
          angle = (Math.Floor(angle / interval) + 1) * interval;
        }
        if (angle >= 360) angle -= 360;
        else if (angle < 0) angle += 360;
      }
      return angle;
    }
  }

  /// <summary>
  /// Internal class to remember a Part's offset and angle.
  /// </summary>
  internal class PartInfo {
    public double PlacementAngle;
    public double Distance;
    public double RotationAngle;
    public PartInfo(double placementAngle, double distance, double rotationAngle) {
      PlacementAngle = placementAngle * (Math.PI / 180);  // in radians
      Distance = distance;
      RotationAngle = rotationAngle;  // in degrees
    }
  }
}
