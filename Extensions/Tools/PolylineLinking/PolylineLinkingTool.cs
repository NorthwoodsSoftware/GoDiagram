/*
*  Copyright (C) 1998-2022 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main GoDiagram library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in the GoDiagram repository (https://github.com/NorthwoodsSoftware/GoDiagram/tree/main/Extensions).
* See the Extensions intro page (https://godiagram.com/intro/extensions.html) for more information.
*/

using System.Collections.Generic;
using System.Linq;

namespace Northwoods.Go.Tools.Extensions {

  /// <summary>
  /// The PolylineLinkingTool class the user to draw a new <see cref="Link"/> by clicking where the route should go,
  /// until clicking on a valid target port.
  /// </summary>
  /// <remarks>
  /// This tool supports routing both orthogonal and straight links.
  /// You can customize the <see cref="LinkingBaseTool.TemporaryLink"/> as needed to affect the
  /// appearance and behavior of the temporary link that is shown during the linking operation.
  /// You can customize the <see cref="LinkingTool.ArchetypeLinkData"/> to specify property values
  /// that can be data-bound by your link template for the Links that are actually created.
  /// </remarks>
  /// @category Tool Extension
  public class PolylineLinkingTool : LinkingTool {
    private bool _FirstMouseDown = false;
    private bool _Horizontal = false;

    /// <summary>
    /// Constructs an PolylineLinkingTool, sets <see cref="LinkingBaseTool.PortGravity"/> to 0, and sets the name for the tool.
    /// </summary>
    public PolylineLinkingTool() : base() {
      PortGravity = 0; // must click on a target port in order to complete the link
      Name = "PolylineLinking";
    }

    /// <summary>
    /// This internal method adds a point to the route.
    /// </summary>
    /// <remarks>
    /// During the operation of this tool, the very last point changes to follow the mouse point.
    /// This method is called by <see cref="DoMouseDown"/> in order to add a new "last" point.
    /// </remarks>
    private void AddPoint(Point p) {
      if (_FirstMouseDown) return;
      var pts = TemporaryLink.Points.ToList();
      _Horizontal = !_Horizontal;
      pts.Add(p);
      TemporaryLink.Points = pts;
    }

    /// <summary>
    /// This internal method moves the last point of the temporary Link's route.
    /// </summary>
    /// <remarks>
    /// This is called by <see cref="DoMouseMove"/> and other methods that want to adjust the end of the route.
    /// </remarks>
    private void MoveLastPoint(Point p) {
      if (_FirstMouseDown) return;
      var pts = TemporaryLink.Points.ToList();
      if (TemporaryLink.IsOrthogonal) {
        var q = pts.ElementAt(pts.Count - 3);
        if (_Horizontal) {
          q.Y = p.Y;
        } else {
          q.X = p.X;
        }
        pts[pts.Count - 2] = q;
      }
      pts[pts.Count - 1] = p;
      TemporaryLink.Points = pts;
    }

    /// <summary>
    /// This internal method removes the last point of the temporary Link's route.
    /// </summary>
    /// <remarks>
    /// This is called by the "Z" command in <see cref="DoKeyDown"/>
    /// and by <see cref="DoMouseUp"/> when a valid target port is found and we want to
    /// discard the current mouse point from the route.
    /// </remarks>
    private void RemoveLastPoint() {
      if (_FirstMouseDown) return;
      var pts = TemporaryLink.Points.ToList();
      if (pts.Count == 0) return;
      pts.RemoveAt(pts.Count - 1);
      TemporaryLink.Points = pts;
      _Horizontal = !_Horizontal;
    }

    /// <summary>
    /// Use a "crosshair" cursor.
    /// </summary>
    public override void DoActivate() {
      base.DoActivate();
      Diagram.CurrentCursor = "crosshair";
      // until a mouse down occurs, allow the temporary link to be routed to the temporary node/port
      _FirstMouseDown = true;
    }

    /// <summary>
    /// Add a point to the route that the temporary Link is accumulating.
    /// </summary>
    public override void DoMouseDown() {
      if (!IsActive) {
        DoActivate();
      }
      if (Diagram.LastInput.Left) {
        if (_FirstMouseDown) {
          _FirstMouseDown = false;
          // disconnect the temporary node/port from the temporary link
          // so that it doesn't lose the points that are accumulating
          if (IsForwards) {
            TemporaryLink.ToNode = null;
          } else {
            TemporaryLink.FromNode = null;
          }
          var pts = TemporaryLink.Points;
          var ult = pts.ElementAt(pts.Count - 1);
          var penult = pts.ElementAt(pts.Count - 2);
          _Horizontal = (ult.X == penult.X);
        }
        // a new temporary end point, the previous one is now "accepted"
        AddPoint(Diagram.LastInput.DocumentPoint);
      } else {  // e.g. right mouse down
        DoCancel();
      }
    }

    /// <summary>
    /// Have the temporary link reach to the last mouse point.
    /// </summary>
    public override void DoMouseMove() {
      if (IsActive) {
        MoveLastPoint(Diagram.LastInput.DocumentPoint);
        base.DoMouseMove();
      }
    }

    /// <summary>
    /// If this event happens on a valid target port (as determined by <see cref="LinkingBaseTool.FindTargetPort"/>),
    /// we complete the link drawing operation.
    /// </summary>
    /// <remarks>
    /// <see cref="InsertLink"/> is overridden to transfer the accumulated
    /// route drawn by user clicks to the new <see cref="Link"/> that was created.
    ///
    /// If this event happens elsewhere in the diagram, this tool is not stopped: the drawing of the route continues.
    /// </remarks>
    public override void DoMouseUp() {
      if (!IsActive) return;
      var target = FindTargetPort(IsForwards);
      if (target != null) {
        if (_FirstMouseDown) {
          base.DoMouseUp();
        } else {
          List<Point> pts;
          RemoveLastPoint();  // remove temporary point
          var spot = IsForwards ? target.ToSpot : target.FromSpot;
          if (spot.Equals(Spot.None)) {
            var pt = TemporaryLink.GetLinkPointFromPoint(target.Part as Node, target,
              target.GetDocumentPoint(Spot.Center),
              TemporaryLink.Points.ElementAt(TemporaryLink.Points.Count - 2),
              !IsForwards);
            MoveLastPoint(pt);
            pts = TemporaryLink.Points.ToList();
            if (TemporaryLink.IsOrthogonal) {
              pts.Insert(pts.Count - 2, pts.ElementAt(pts.Count - 2));
            }
          } else {
            // copy the route of saved points, because we're about to recompute it
            pts = TemporaryLink.Points.ToList();
            // terminate the link in the expected manner by letting the
            // temporary link connect with the temporary node/port and letting the
            // normal route computation take place
            if (IsForwards) {
              CopyPortProperties(target.Part as Node, target, TemporaryToNode, TemporaryToPort, true);
              TemporaryLink.ToNode = target.Part as Node;
            } else {
              CopyPortProperties(target.Part as Node, target, TemporaryFromNode, TemporaryFromPort, false);
              TemporaryLink.FromNode = target.Part as Node;
            }
            TemporaryLink.UpdateRoute();
            // now copy the final one or two points of the temporary link's route
            // into the route built up in the PTS List.
            var natpts = TemporaryLink.Points;
            var numnatpts = natpts.Count;
            if (numnatpts >= 2) {
              if (numnatpts >= 3) {
                var penult = natpts.ElementAt(numnatpts - 2);
                pts.Insert(pts.Count - 1, penult);
                if (TemporaryLink.IsOrthogonal) {
                  pts.Insert(pts.Count - 1, penult);
                }
              }
              var ult = natpts.ElementAt(numnatpts - 1);
              pts[pts.Count - 1] = ult;
            }
          }
          // save desired route in temporary link;
          // InsertLink will copy the route into the new real Link
          TemporaryLink.Points = pts;
          base.DoMouseUp();
        }
      }
    }

    /// <summary>
    /// This method overrides the standard link creation method by additionally
    /// replacing the default link route with the custom one laid out by the user.
    /// </summary>
    public override Link InsertLink(Node fromnode, GraphObject fromport, Node tonode, GraphObject toport) {
      var link = base.InsertLink(fromnode, fromport, tonode, toport);
      if (link != null && !_FirstMouseDown) {
        // ignore natural route by replacing with route accumulated by this tool
        link.Points = TemporaryLink.Points;
      }
      return link;
    }

    /// <summary>
    /// This supports the "Z" command during this tool's operation to remove the last added point of the route.
    /// Type ESCAPE to completely cancel the operation of the tool.
    /// </summary>
    public override void DoKeyDown() {
      if (!IsActive) return;
      var e = Diagram.LastInput;
      if (e.Key == "Z" && TemporaryLink.Points.Count > (TemporaryLink.IsOrthogonal ? 4 : 3)) {  // undo                                                                                                // remove a point, and then treat the last one as a temporary one
        RemoveLastPoint();
        MoveLastPoint(e.DocumentPoint);
      } else {
        base.DoKeyDown();
      }
    }
  }
}
