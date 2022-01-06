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
  /// The PortShiftingTool class lets a user move a port on a <see cref="Node"/>.
  /// </summary>
  /// <remarks>
  /// This tool only works when the Node has a port (any GraphObject) marked with
  /// a non-null and non-empty PortId that is positioned in a Spot Panel,
  /// and the user holds down the Shift key.
  /// It works by modifying that port's <see cref="GraphObject.Alignment"/> property.
  ///
  /// If you want to experiment with this extension, try the <a href="../../extensions/PortShifting.html">Port Shifting</a> sample.
  /// </remarks>
  /// @category Tool Extension
  public class PortShiftingTool : Tool {
    /// <summary>
    /// The port being shifted.
    /// </summary>
    public GraphObject Port;
    private Spot _OriginalAlignment;

    /// <summary>
    /// Constructs a PortShiftingTool and sets the name for the tool.
    /// </summary>
    public PortShiftingTool() : base() {
      _OriginalAlignment = Spot.Default;
      Name = "PortShifting";
    }

    /// <summary>
    /// This tool can only start if the mouse has moved enough so that it is not a click,
    /// and if the mouse down point is on a GraphObject "port" in a Spot Panel,
    /// as determined by <see cref="FindPort"/>.
    /// </summary>
    public override bool CanStart() {
      var diagram = Diagram;
      if (!base.CanStart()) return false;
      // require left button & that it has moved far enough away from the mouse down point, so it isn't a click
      var e = diagram.LastInput;
      if (!e.Left || !e.Shift) return false;
      if (!IsBeyondDragSize()) return false;

      return FindPort() != null;
    }

    /// <summary>
    /// From the <see cref="GraphObject"/> at the mouse point, search up the visual tree until we get to
    /// an object that has the <see cref="GraphObject.PortId"/> property set to a non-empty string, that is in a Spot Panel,
    /// and that is not the main element of the panel (typically the first element).
    /// </summary>
    /// <returns>This returns null if no such port is at the mouse down point.</returns>
    public GraphObject FindPort() {
      var diagram = Diagram;
      var e = diagram.FirstInput;
      var elt = diagram.FindElementAt(e.DocumentPoint, null, null);
      if (elt == null || elt.Part is not Node) return null;

      while (elt != null && elt.Panel != null) {
        if (elt.Panel.Type == PanelLayoutSpot.Instance && elt.Panel.FindMainElement() != elt &&
          elt.PortId != null && elt.PortId != "") {
          return elt;
        }
        elt = elt.Panel;
      }
      return null;
    }

    /// <summary>
    /// Start a transaction, call <see cref="FindPort"/> and remember it as the "Port" property,
    /// and remember the original value for the port"s <see cref="GraphObject.Alignment"/> property.
    /// </summary>
    public override void DoActivate() {
      StartTransaction("Shifted Label");
      Port = FindPort();
      if (Port != null) {
        _OriginalAlignment = Port.Alignment;
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
    /// Clear any reference to a port element.
    /// </summary>
    public override void DoStop() {
      Port = null;
      base.DoStop();
    }

    /// <summary>
    /// Restore the port's original value for <see cref="GraphObject.Alignment"/>.
    /// </summary>
    public override void DoCancel() {
      if (Port != null) {
        Port.Alignment = _OriginalAlignment;
      }
      base.DoCancel();
    }

    /// <summary>
    /// During the drag, call <see cref="UpdateAlignment"/> in order to set the <see cref="GraphObject.Alignment"/> of the port.
    /// </summary>
    public override void DoMouseMove() {
      if (!IsActive) return;
      UpdateAlignment();
    }

    /// <summary>
    /// At the end of the drag, update the alignment of the port and finish the tool,
    /// completing a transaction.
    /// </summary>
    public override void DoMouseUp() {
      if (!IsActive) return;
      UpdateAlignment();
      TransactionResult = "Shifted Label";
      StopTool();
    }

    /// <summary>
    /// Save the port's <see cref="GraphObject.Alignment"/> as a fractional Spot in the Spot Panel
    /// that the port is in.
    /// </summary>
    /// <remarks>
    /// If the main element changes size, the relative positions
    /// of the ports will be maintained. But that does assume that the port must remain
    /// inside the main element -- it cannot wander away from the node.
    /// This does not modify the port's <see cref="GraphObject.AlignmentFocus"/> property.
    /// </remarks>
    public void UpdateAlignment() {
      if (Port == null || Port.Panel == null) return;
      var last = Diagram.LastInput.DocumentPoint;
      var main = Port.Panel.FindMainElement();
      if (main == null) return;
      var tl = main.GetDocumentPoint(Spot.TopLeft);
      var br = main.GetDocumentPoint(Spot.BottomRight);
      var x = Math.Max(0, Math.Min((last.X - tl.X) / (br.X - tl.X), 1));
      var y = Math.Max(0, Math.Min((last.Y - tl.Y) / (br.Y - tl.Y), 1));
      Port.Alignment = new Spot(x, y);
    }
  }
}
