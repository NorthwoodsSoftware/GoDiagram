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

namespace Northwoods.Go.Tools.Extensions {

  /// <summary>
  /// The LinkLabelDraggingTool class lets the user move a label on a <see cref="Link"/>.
  /// </summary>
  /// <remarks>
  /// This tool only works when the Link has a label
  /// that is positioned at the <see cref="Link.MidPoint"/> plus some offset.
  /// It does not work for labels that have a particular <see cref="GraphObject.SegmentIndex"/>.
  ///
  /// If you want to experiment with this extension, try the <a href="../../extensions/LinkLabelDragging.html">Link Label Dragging</a> sample.
  /// </remarks>
  /// @category Tool Extension
  public class LinkLabelDraggingTool : Tool {
    /// <summary>
    /// The label being dragged.
    /// </summary>
    public GraphObject Label;
    private Point _Offset;  // of the mouse relative to the center of the label object
    private Point _OriginalOffset;

    /// <summary>
    /// Constructs a LinkLabelDraggingTool and sets the name for the tool.
    /// </summary>
    public LinkLabelDraggingTool() : base() {
      _Offset = new Point();
      Name = "LinkLabelDragging";
    }

    /// <summary>
    /// From the GraphObject at the mouse point, search up the visual tree until we get to
    /// an object that is a label of a Link.
    /// </summary>
    /// <returns>This returns null if no such label is at the mouse down point.</returns>
    public GraphObject FindLabel() {
      var diagram = Diagram;
      var e = diagram.LastInput;
      var elt = diagram.FindElementAt(e.DocumentPoint, null, null);

      if (elt == null || elt.Part is not Link) return null;
      while (elt != null && elt.Panel != elt.Part) {
        elt = elt.Panel;
      }
      // If it's at an arrowhead segment index, don't consider it a label:
      if (elt != null && (elt.SegmentIndex == 0 || elt.SegmentIndex == -1)) return null;
      return elt;
    }

    /// <summary>
    /// This tool can only start if the mouse has moved enough so that it is not a click,
    /// and if the mouse down point is on a GraphObject "label" in a Link Panel,
    /// as determined by <see cref="FindLabel"/>.
    /// </summary>
    public override bool CanStart() {
      if (!base.CanStart()) return false;
      var diagram = Diagram;
      // require left button & that it has moved far enough away from the mouse down point, so it isn't a click
      var e = diagram.LastInput;
      if (!e.Left) return false;
      if (!IsBeyondDragSize()) return false;

      return FindLabel() != null;
    }

    /// <summary>
    /// Start a transaction, call <see cref="FindLabel"/> and remember it as the <see cref="Label"/> property,
    /// and remember the original value for the label's <see cref="GraphObject.SegmentOffset"/> property.
    /// </summary>
    public override void DoActivate() {
      StartTransaction("Shifted Label");
      Label = FindLabel();
      if (Label != null) {
        // compute the offset of the mouse-down point relative to the center of the label
        _Offset = Diagram.FirstInput.DocumentPoint.Subtract(Label.GetDocumentPoint(Spot.Center));
        _OriginalOffset = Label.SegmentOffset;
      }
      base.DoActivate();
    }

    /// <summary>
    /// Stop any ongoing transaction.
    /// </summary>
    public override void DoDeactivate() {
      base.DoDeactivate();
      StopTransaction();
    }

    /// <summary>
    /// Clear any reference to a label element.
    /// </summary>
    public override void DoStop() {
      Label = null;
      base.DoStop();
    }

    /// <summary>
    /// Restore the label's original value for <see cref="GraphObject.SegmentOffset"/>.
    /// </summary>
    public override void DoCancel() {
      if (Label != null) {
        Label.SegmentOffset = _OriginalOffset;
      }
      base.DoCancel();
    }

    /// <summary>
    /// During the drag, call <see cref="UpdateSegmentOffset"/> in order to set
    /// the <see cref="GraphObject.SegmentOffset"/> of the label.
    /// </summary>
    public override void DoMouseMove() {
      if (!IsActive) return;
      UpdateSegmentOffset();
    }

    /// <summary>
    /// At the end of the drag, update the segment offset of the label and finish the tool,
    /// completing a transaction.
    /// </summary>
    public override void DoMouseUp() {
      if (!IsActive) return;
      UpdateSegmentOffset();
      TransactionResult = "Shifted Label";
      StopTool();
    }

    /// <summary>
    /// Save the label's <see cref="GraphObject.SegmentOffset"/> as a rotated offset from the midpoint of the
    /// Link that the label is in.
    /// </summary>
    public void UpdateSegmentOffset() {
      var lab = Label;
      if (lab == null) return;
      if (lab.Part is not Link link) return;

      var last = Diagram.LastInput.DocumentPoint;
      var idx = (int)lab.SegmentIndex;
      var numpts = link.PointsCount;
      // if the label is a "mid" label, assume it is positioned differently from a label at a particular segment
      if (idx < -numpts || idx >= numpts) {
        var mid = link.MidPoint;
        // need to rotate this point to account for the angle of the link segment at the mid-point
        var p = new Point(last.X - _Offset.X - mid.X, last.Y - _Offset.Y - mid.Y);
        lab.SegmentOffset = p.Rotate(-link.MidAngle);
      } else {  // handle the label point being on a partiular segment with a given fraction
        var frac = lab.SegmentFraction;
        Point a;
        Point b;
        if (idx >= 0) {  // indexing forwards
          a = link.GetPoint(idx);
          b = (idx < numpts - 1) ? link.GetPoint(idx + 1) : a;
        } else {  // or backwards if SegmentIndex is negative
          var i = numpts + idx;
          a = link.GetPoint(i);
          b = (i > 0) ? link.GetPoint(i - 1) : a;
        }
        var labx = a.X + (b.X - a.X) * frac;
        var laby = a.Y + (b.Y - a.Y) * frac;
        var p = new Point(last.X - _Offset.X - labx, last.Y - _Offset.Y - laby);
        var segangle = (idx >= 0) ? a.Direction(b) : b.Direction(a);
        lab.SegmentOffset = p.Rotate(-segangle);
      }
    }
  }
}
