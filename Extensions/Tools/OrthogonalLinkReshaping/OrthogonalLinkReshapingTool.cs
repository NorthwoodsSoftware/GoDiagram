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
  /// The OrthogonalLinkReshapingTool class lets a user drag a tool handle along the link segment, which will move the whole segment.
  /// </summary>
  /// <remarks>
  /// If you want to experiment with this extension, try the <a href="../../extensions/OrthogonalLinkReshaping.html">Orthogonal Link Reshaping</a> sample.
  /// </remarks>
  /// @category Tool Extension
  public class OrthogonalLinkReshapingTool : LinkReshapingTool {
    /// <summary>
    /// Constructs an OrthogonalLinkReshapingTool and sets the name for the tool.
    /// </summary>
    public OrthogonalLinkReshapingTool() : base() {
      Name = "OrthogonalLinkReshaping";
    }

    private bool _AlreadyAddedPoint = false;

    /// <summary>
    /// Undocumented.
    /// </summary>
    [Undocumented]
    public override Adornment MakeAdornment(GraphObject pathshape) {
      // add all normal handles first
      var adornment = base.MakeAdornment(pathshape);

      // add long reshaping handles for orthogonal, straight links
      if (pathshape.Part is Link link && link.IsOrthogonal && link.Curve != LinkCurve.Bezier) {
        var firstindex = link.FirstPickIndex + (link.Resegmentable ? 0 : 1);
        var lastindex = link.LastPickIndex - (link.Resegmentable ? 0 : 1);
        for (var i = firstindex; i < lastindex; i++) {
          MakeSegmentDragHandle(link, adornment, i);
        }
      }
      return adornment;
    }

    /// <summary>
    /// This stops the current reshaping operation and updates any link handles.
    /// </summary>
    public override void DoDeactivate() {
      _AlreadyAddedPoint = false;
      // when we finish, recreate adornment to ensure proper reshaping behavior/cursor
      var link = AdornedLink;
      if (link != null && link.IsOrthogonal && link.Curve != LinkCurve.Bezier) {
        var pathshape = link.Path;
        if (pathshape != null) {
          var adornment = MakeAdornment(pathshape);
          if (adornment != null) {
            link.AddAdornment(Name, adornment);
            adornment.Location = link.Position;
          }
        }
      }
      base.DoDeactivate();
    }

    /// <summary>
    /// Change the route of the <see cref="LinkReshapingTool.AdornedLink"/> by moving the segment corresponding to the current
    /// <see cref="LinkReshapingTool.Handle"/> to be at the given <see cref="Point"/>.
    /// </summary>
    public override void Reshape(Point newpt) {
      var link = AdornedLink;

      // identify if the handle being dragged is a segment dragging handle
      if (link != null && link.IsOrthogonal && link.Curve != LinkCurve.Bezier && Handle != null && Handle.ToMaxLinks == 999) {
        link.StartRoute();
        var index = (int)Handle.SegmentIndex; // for these handles, FirstPickIndex <= index < LastPickIndex
        if (!_AlreadyAddedPoint && link.Resegmentable) {  // only change the double of points if Link.Resegmentable
          _AlreadyAddedPoint = true;
          if (index == link.FirstPickIndex) {
            link.InsertPoint(index, link.GetPoint(index));
            index++;
            Handle.SegmentIndex = index;
          } else if (index == link.LastPickIndex - 1) {
            link.InsertPoint(index, link.GetPoint(index));
            if (index - 1 == link.FirstPickIndex + 1) Handle.SegmentIndex = index - 1;
          }
        }
        var behavior = GetReshapingBehavior(Handle);
        if (behavior == ReshapingBehavior.Vertical) {
          // move segment vertically
          link.SetPointAt(index, link.GetPoint(index - 1).X, newpt.Y);
          link.SetPointAt(index + 1, link.GetPoint(index + 2).X, newpt.Y);
        } else if (behavior == ReshapingBehavior.Horizontal) {
          // move segment horizontally
          link.SetPointAt(index, newpt.X, link.GetPoint(index - 1).Y);
          link.SetPointAt(index + 1, newpt.X, link.GetPoint(index + 2).Y);
        }
        link.CommitRoute();
      } else {
        base.Reshape(newpt);
      }
    }

    /// <summary>
    /// Create the segment dragging handles.
    /// </summary>
    /// <remarks>
    /// There are two parts: one invisible handle that spans the segment, and a visible handle at the middle of the segment.
    /// These are inserted at the front of the adornment such that the normal handles have priority.
    /// </remarks>
    public void MakeSegmentDragHandle(Link link, Adornment adornment, int index) {
      var a = link.GetPoint(index);
      var b = link.GetPoint(index + 1);
      var seglength = Math.Max(Math.Abs(a.X - b.X), Math.Abs(a.Y - b.Y));
      // determine segment orientation
      var orient = "";
      if (IsApprox(a.X, b.X) && IsApprox(a.Y, b.Y)) {
        b = link.GetPoint(index - 1);
        if (IsApprox(a.X, b.X)) {
          orient = "vertical";
        } else if (IsApprox(a.Y, b.Y)) {
          orient = "horizontal";
        }
      } else {
        if (IsApprox(a.X, b.X)) {
          orient = "vertical";
        } else if (IsApprox(a.Y, b.Y)) {
          orient = "horizontal";
        }
      }

      // make an invisible handle along the whole segment
      var h = new Shape {
        StrokeWidth = 6,
        Opacity = 0,
        SegmentOrientation = Orientation.Along,
        SegmentIndex = index,
        SegmentFraction = 0.5,
        ToMaxLinks = 999 // set this unsused property to easily identify that we have a segment dragging handle
      };
      if (orient == "horizontal") {
        SetReshapingBehavior(h, ReshapingBehavior.Vertical);
        h.Cursor = "n-resize";
      } else {
        SetReshapingBehavior(h, ReshapingBehavior.Horizontal);
        h.Cursor = "w-resize";
      }
      h.GeometryString = "M 0 0 L " + seglength + " 0";
      adornment.InsertAt(0, h);
    }

    private static bool IsApprox(double x, double y) {
      var d = x - y;
      return d < 0.5 && d > -0.5;
    }
  }
}
