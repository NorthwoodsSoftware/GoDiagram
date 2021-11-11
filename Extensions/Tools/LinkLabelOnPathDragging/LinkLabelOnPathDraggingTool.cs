/*
*  Copyright (C) 1998-2020 by Northwoods Software Corporation. All Rights Reserved.
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
  /// The LinkLabelOnPathDraggingTool class lets the user move a label on a <see cref="Link"/> while keeping the label on the link's path.
  /// This tool only works when the Link has a label marked by the "_IsLinkLabel" property.
  ///
  /// If you want to experiment with this extension, try the <a href="../../extensionsTS/LinkLabelOnPathDragging.Html">Link Label On Path Dragging</a> sample.
  /// </summary>
  /// @category Tool Extension
  public class LinkLabelOnPathDraggingTool : Tool {
    /// <summary>
    /// The label being dragged.
    /// </summary>
    public GraphObject Label;
    private double _OriginalFraction = 0.0;

    /// <summary>
    /// Constructs a LinkLabelOnPathDraggingTool and sets the name for the tool.
    /// </summary>
    public LinkLabelOnPathDraggingTool() : base() {
      Name = "LinkLabelOnPathDragging";
    }

    /// <summary>
    /// From the GraphObject at the mouse point, search up the visual tree until we get to
    /// an object that has the "_IsLinkLabel" property set to true and that is an immediate child of a Link Panel.
    /// </summary>
    /// <returns>This returns null if no such label is at the mouse down point.</returns>
    public GraphObject FindLabel() {
      var diagram = Diagram;
      var e = diagram.LastInput;
      var elt = diagram.FindElementAt(e.DocumentPoint, null, null);

      if (elt == null || !(elt.Part is Link)) return null;
      while (elt != null && elt.Panel != elt.Part) {
        elt = elt.Panel;
      }
      // If it"s not marked as "_IsLinkLabel", don't consider it a label:
      if (!((elt["_IsLinkLabel"] as bool?) ?? false)) return null; // null-coaslecing operator for casting to bool
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
    /// Start a transaction, call FindLabel and remember it as the "label" property,
    /// and remember the original value for the label's SegmentFraction property.
    /// </summary>
    public override void DoActivate() {
      StartTransaction("Shifted Label");
      Label = FindLabel();
      if (Label != null) {
        _OriginalFraction = Label.SegmentFraction;
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
    /// Restore the label's original value for GraphObject.SegmentFraction property.
    /// </summary>
    public override void DoCancel() {
      if (Label != null) {
        Label.SegmentFraction = _OriginalFraction;
      }
      base.DoCancel();
    }

    /// <summary>
    /// During the drag, call <see cref="UpdateSegmentOffset"/> in order to set the SegmentFraction property of the label.
    /// </summary>
    public override void DoMouseMove() {
      if (!IsActive) return;
      UpdateSegmentOffset();
    }

    /// <summary>
    /// At the end of the drag, update the segment properties of the label and finish the tool,
    /// completing a transaction.
    /// </summary>
    public override void DoMouseUp() {
      if (!IsActive) return;
      UpdateSegmentOffset();
      TransactionResult = "Shifted Label";
      StopTool();
    }

    /// <summary>
    /// Save the label's <see cref="GraphObject.SegmentFraction"/>
    /// at the closest point to the mouse.
    /// </summary>
    public void UpdateSegmentOffset() {
      var lab = Label;
      if (lab == null) return;
      var link = lab.Part as Link;
      if (!(link is Link) || link.Path == null) return;

      var last = Diagram.LastInput.DocumentPoint;
      // find the fractional distance along the link path closest to this point
      var path = link.Path;
      if (path.Geometry == null) return;
      var localpt = path.GetLocalPoint(last);
      lab.SegmentFraction = path.Geometry.GetFractionForPoint(localpt);
    }
  }
}
