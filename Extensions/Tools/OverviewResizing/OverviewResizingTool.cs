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
  /// The OverviewResizingTool class lets the user resize the box within an overview.
  /// </summary>
  /// <remarks>
  /// If you want to experiment with this extension, try the <a href="../../extensions/OverviewResizing.html">Overview Resizing</a> sample.
  /// </remarks>
  /// @category Tool Extension
  public class OverviewResizingTool : ResizingTool {
    // Internal property used to keep track of changing handle size
    private Size _HandleSize;

    /// <summary>
    /// Constructs an OverviewResizingTool and sets the name for the tool.
    /// </summary>
    public OverviewResizingTool() : base() {
      Name = "OverviewResizing";
      _HandleSize = new Size(6, 6);
    }

    /// <summary>
    /// Undocumented.
    /// </summary>
    [Undocumented]
    public virtual Adornment MakeAdornment(Shape resizeBox) {
      _HandleSize = new Size(resizeBox.StrokeWidth * 3, resizeBox.StrokeWidth * 3);
      // Set up the resize adornment
      // placeholder
      var ph = new Placeholder {
        IsPanelMain = true
      };
      // handle
      var hnd = new Shape("Rectangle") {
        Name = "RSZHND",
        DesiredSize = _HandleSize,
        Cursor = "se-resize",
        Alignment = Spot.BottomRight,
        AlignmentFocus = Spot.Center
      };
      var ad = new Adornment {
        Type = PanelLayoutSpot.Instance,
        LocationSpot = Spot.Center,
        AdornedElement = resizeBox
      }.Add(
        ph,
        hnd
      );
      return ad;
    }

    /// Keep the resize handle properly sized as the scale is changing.
    /// This overrides an undocumented method on the ResizingTool.
    /// <summary>
    /// Undocumented.
    /// </summary>
    [Undocumented]
    public override void UpdateResizeHandles(GraphObject elt, double angle) {
      if (elt == null || elt is not Adornment) return;
      var ad = elt as Adornment;
      var handle = ad.FindElement("RSZHND") as Shape;
      var box = ad.AdornedElement as Shape;
      _HandleSize = new Size(box.StrokeWidth * 3, box.StrokeWidth * 3);
      handle.DesiredSize = _HandleSize;
    }

    /// <summary>
    /// Overrides <see cref="ResizingTool.Resize"/> to resize the overview box via setting the observed diagram's scale.
    /// </summary>
    /// <param name="newr">the intended new rectangular bounds the overview box.</param>
    public override void Resize(Rect newr) {
      var overview = Diagram as Overview;
      var observed = overview.Observed;
      if (observed == null) return;
      var oldr = observed.ViewportBounds;
      var oldscale = observed.Scale;
      if (oldr.Width != newr.Width || oldr.Height != newr.Height) {
        if (newr.Width > 0 && newr.Height > 0) {
          observed.Scale = Math.Min(oldscale * oldr.Width / newr.Width, oldscale * oldr.Height / newr.Height);
        }
      }
    }
  }
}
