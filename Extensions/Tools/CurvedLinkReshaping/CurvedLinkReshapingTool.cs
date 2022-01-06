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

using System;

namespace Northwoods.Go.Tools.Extensions {

  /// <summary>
  /// This CurvedLinkReshapingTool class allows for a <see cref="Link"/>'s path to be modified by the user
  /// via the dragging of a single tool handle at the middle of the link.
  /// Dragging the handle changes the value of <see cref="Link.Curviness"/>.
  /// </summary>
  /// <remarks>
  /// If you want to experiment with this extension, try the <a href="../../extensions/CurvedLinkReshaping.html">Curved Link Reshaping</a> sample.
  /// </remarks>
  /// @category Tool Extension
  public class CurvedLinkReshapingTool : LinkReshapingTool {
    private double _OriginalCurviness = double.NaN;

    /// <summary>
    /// Constructs a CurvedLinkReshapingTool.
    /// </summary>
    public CurvedLinkReshapingTool() : base() {
      IsEnabled = true;
    }

    /// <summary>
    /// Undocumented.
    /// </summary>
    [Undocumented]
    public override Adornment MakeAdornment(GraphObject pathshape) {
      if (pathshape.Part is Link link && link.Curve == LinkCurve.Bezier && link.PointsCount == 4) {
        var adornment = new Adornment(PanelLayoutLink.Instance);
        var h = MakeHandle(pathshape, 0);
        SetReshapingBehavior(h, ReshapingBehavior.All);
        h.Cursor = "move";
        adornment.Add(h);
        adornment.Category = Name;
        adornment.AdornedElement = pathshape;
        return adornment;
      } else {
        return base.MakeAdornment(pathshape);
      }
    }

    /// <summary>
    /// Start reshaping, if <see cref="Tool.FindToolHandleAt"/> finds a reshape handle at the mouse down point.
    /// </summary>
    /// <remarks>
    /// If successful this sets <see cref="LinkReshapingTool.Handle"/> to be the reshape handle that it finds
    /// and <see cref="LinkReshapingTool.AdornedLink"/> to be the <see cref="Link"/> being routed.
    /// It also remembers the original link route (a list of Points) and curviness in case this tool is cancelled.
    /// And it starts a transaction.
    /// </remarks>
    public override void DoActivate() {
      base.DoActivate();
      if (AdornedLink != null) {
        _OriginalCurviness = AdornedLink.Curviness;
      }
    }

    /// <summary>
    /// Restore the link route to be the original points and curviness and stop this tool.
    /// </summary>
    public override void DoCancel() {
      if (AdornedLink != null) {
        AdornedLink.Curviness = _OriginalCurviness;
      }
      base.DoCancel();
    }

    /// <summary>
    /// Change the route of the <see cref="LinkReshapingTool.AdornedLink"/> by moving the point corresponding to the current
    /// <see cref="LinkReshapingTool.Handle"/> to be at the given <see cref="Point"/>.
    /// </summary>
    /// <remarks>
    /// This is called by <see cref="ToolManager.DoMouseMove"/> and <see cref="ToolManager.DoMouseUp"/> with the result of calling
    /// <see cref="LinkReshapingTool.ComputeReshape"/> to constrain the input point.
    /// </remarks>
    /// <param name="newPoint">the value of the call to <see cref="Reshape"/></param>
    public override void Reshape(Point newPoint) {
      if (AdornedLink is Link link && link.Curve == LinkCurve.Bezier && link.PointsCount == 4) {
        var start = link.GetPoint(0);
        var end = link.GetPoint(3);
        var ang = start.Direction(end);
        var mid = new Point((start.X + end.X) / 2, (start.Y + end.Y) / 2);
        var a = new Point(9999, 0).Rotate(ang + 90).Add(mid);
        var b = new Point(9999, 0).Rotate(ang - 90).Add(mid);
        var q = newPoint.ProjectOntoLineSegment(a, b);
        var curviness = Math.Sqrt(mid.DistanceSquared(q));
        var port = link.FromPort;
        if ((port == link.ToPort) && (port != null)) {
          if (newPoint.Y < port.GetDocumentPoint(Go.Spot.Center).Y) {
            curviness = -curviness;
          }
        } else {
          var diff = (mid.Direction(q) - ang);
          if (((diff > 0) && (diff < 180)) || (diff < -180)) {
            curviness = -curviness;
          }
        }
        link.Curviness = curviness;
      } else {
        base.Reshape(newPoint);
      }
    }
  }
}
