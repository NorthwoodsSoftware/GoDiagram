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
  /// The NodeLabelDraggingTool class lets the user move a label on a Node.
  ///
  /// This tool only works when the Node has a label (any GraphObject) marked with
  /// { _isNodeLabel: true } that is positioned in a Spot Panel.
  /// It works by modifying that label"s <see cref="GraphObject.Alignment"/> property to have an
  /// offset from the center of the panel.
  ///
  /// If you want to experiment with this extension, try the <a href="../../extensionsTS/NodeLabelDragging.Html">Node Label Dragging</a> sample.
  /// </summary>
  /// @category Tool Extension
  public class NodeLabelDraggingTool : Tool {
    /// <summary>
    /// The label being dragged.
    /// </summary>
    public GraphObject Label;
    private Point _Offset;  // of the mouse relative to the center of the label object
    private Spot _OriginalAlignment;
    private Point _OriginalCenter;

    /// <summary>
    /// Constructs a NodeLabelDraggingTool and sets the name for the tool.
    /// </summary>
    public NodeLabelDraggingTool() : base() {
      Name = "NodeLabelDragging";
      _OriginalCenter = new Point();
      _OriginalAlignment = Spot.Default;
      _Offset = new Point();
    }

    /// <summary>
    /// From the GraphObject at the mouse point, search up the visual tree until we get to
    /// an object that has the "_isNodeLabel" property set to true, that is in a Spot Panel,
    /// and that is not the first element of that Panel (i.E. not the main element of the panel).
    /// </summary>
    /// <returns>This returns null if no such label is at the mouse down point.</returns>
    public GraphObject FindLabel() {
      var diagram = Diagram;
      var e = diagram.FirstInput;
      var elt = diagram.FindElementAt(e.DocumentPoint, null, null);

      if (elt == null || !(elt.Part is Node)) return null;
      while (elt.Panel != null) {
        if ((elt["_IsNodeLabel"] as bool? ?? false) && elt.Panel.Type == PanelLayoutSpot.Instance && elt.Panel.FindMainElement() != elt) return elt;
        elt = elt.Panel;
      }
      return null;
    }

    /// <summary>
    /// This tool can only start if the mouse has moved enough so that it is not a click,
    /// and if the mouse down point is on a GraphObject "label" in a Spot Panel,
    /// as determined by findLabel().
    /// </summary>
    public override bool CanStart() {
      if (!base.CanStart()) return false;
      var diagram = Diagram;
      // require left button & that it has moved far enough away from the mouse down point, so it isn"t a click
      var e = diagram.LastInput;
      if (!e.Left) return false;
      if (!IsBeyondDragSize()) return false;

      return FindLabel() != null;
    }

    /// <summary>
    /// Start a transaction, call <see cref="FindLabel"/> and remember it as the "label" property,
    /// and remember the original value for the label"s <see cref="GraphObject.Alignment"/> property.
    /// </summary>
    public override void DoActivate() {
      StartTransaction("Shifted Label");
      Label = FindLabel();
      if (Label != null) {
        // compute the offset of the mouse-down point relative to the center of the label
        _Offset = Diagram.FirstInput.DocumentPoint.Subtract(Label.GetDocumentPoint(Spot.Center));
        _OriginalAlignment = Label.Alignment;
        var panel = Label.Panel;
        if (panel != null) {
          var main = panel.FindMainElement();
          if (main != null) _OriginalCenter = main.GetDocumentPoint(Spot.Center);
        }
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
    /// Restore the label"s original value for GraphObject.Alignment.
    /// </summary>
    public override void DoCancel() {
      if (Label != null) {
        Label.Alignment = _OriginalAlignment;
      }
      base.DoCancel();
    }

    /// <summary>
    /// During the drag, call updateAlignment in order to set the <see cref="GraphObject.Alignment"/> of the label.
    /// </summary>
    public override void DoMouseMove() {
      if (!IsActive) return;
      UpdateAlignment();
    }

    /// <summary>
    /// At the end of the drag, update the alignment of the label and finish the tool,
    /// completing a transaction.
    /// </summary>
    public override void DoMouseUp() {
      if (!IsActive) return;
      UpdateAlignment();
      TransactionResult = "Shifted Label";
      StopTool();
    }

    /// <summary>
    /// Save the label"s <see cref="GraphObject.Alignment"/> as an absolute offset from the center of the Spot Panel
    /// that the label is in.
    /// </summary>
    public void UpdateAlignment() {
      if (Label == null) return;
      var last = Diagram.LastInput.DocumentPoint;
      var cntr = _OriginalCenter;
      Label.Alignment = new Spot(0.5, 0.5, last.X - _Offset.X - cntr.X, last.Y - _Offset.Y - cntr.Y);
    }
  }
}
