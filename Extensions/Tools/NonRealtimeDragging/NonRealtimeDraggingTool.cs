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

using System.Collections.Generic;

namespace Northwoods.Go.Tools.Extensions {

  /// <summary>
  /// The NonRealtimeDraggingTool class lets the user drag an image instead of actually moving any selected nodes,
  /// until the mouse-up event.
  /// </summary>
  /// @category Tool Extension
  public class NonRealtimeDraggingTool : DraggingTool {
    private int _Duration;  // duration of movement animation; <= 0 to disable
    private Part _ImagePart;  // a Part holding a translucent image of what would be dragged
    private IDictionary<Part, DraggingInfo> _GhostDraggedParts;  // a Map of the _ImagePart and its dragging information
    private IDictionary<Part, DraggingInfo> _OriginalDraggedParts;  // the saved normal value of DraggingTool.DraggedParts

    /// <summary>
    /// Gets or sets how long the movement animation should be to move the actual parts upon a mouse-up.
    /// </summary>
    /// <remarks>
    /// The default value is zero -- there is no animation of the movement.
    /// </remarks>
    public int Duration {
      get { return _Duration; }
      set { _Duration = value; }
    }

    /// <summary>
    /// Call the base method, and then make an image of the returned collection,
    /// show it using a Picture, and hold the Picture in a temporary Part.
    /// </summary>
    /// <param name="coll">an IEnumerable of <see cref="Part"/>s</param>
    /// <param name="options"></param>
    public override IDictionary<Part, DraggingInfo> ComputeEffectiveCollection(IEnumerable<Part> coll, DraggingOptions options) {
      var map = base.ComputeEffectiveCollection(coll, DragOptions);
      if (IsActive && _ImagePart == null) {
        var bounds = Diagram.ComputePartsBounds(map.Keys);
        var offset = Diagram.LastInput.DocumentPoint.Subtract(bounds.Position);
        _ImagePart =
          new Part {
            LayerName = "Tool",
            Opacity = 0.5,
            LocationSpot = new Spot(0, 0, offset.X, offset.Y)
          }.Add(
            new Picture {
              Source = Diagram.MakeImageData(new ImageDataProperties { Parts = map.Keys })
            }
          ); ;
      }
      return map;
    }

    /// <summary>
    /// When activated, replace the <see cref="DraggingTool.DraggedParts"/> with the ghost dragged parts, which
    /// consists of just one Part, the image, added to the Diagram at the current mouse point.
    /// </summary>
    public override void DoActivate() {
      base.DoActivate();
      //if (_ImagePart != null) {
      _ImagePart.Location = Diagram.LastInput.DocumentPoint;
      Diagram.Add(_ImagePart);
      _OriginalDraggedParts = DraggedParts;
      _GhostDraggedParts = base.ComputeEffectiveCollection(new List<Part>() { _ImagePart }, DragOptions);
      DraggedParts = _GhostDraggedParts;
      //}
    }

    /// <summary>
    /// When deactivated, make sure any image is removed from the Diagram and all references are cleared out.
    /// </summary>
    public override void DoDeactivate() {
      if (_ImagePart != null) {
        Diagram.Remove(_ImagePart);
      }
      _ImagePart = null;
      _GhostDraggedParts = null;
      _OriginalDraggedParts = null;
      base.DoDeactivate();
    }

    /// <summary>
    /// Do the normal mouse-up behavior, but only after restoring <see cref="DraggingTool.DraggedParts"/>.
    /// </summary>
    public override void DoMouseUp() {
      var partsmap = _OriginalDraggedParts;
      if (partsmap != null) {
        DraggedParts = partsmap;
      }
      base.DoMouseUp();
      if (partsmap != null && Duration > 0) {
        var anim = new Animation { Duration = Duration };
        foreach (var kvp in partsmap) {
          var part = kvp.Key;
          anim.Add(part, "Location", kvp.Value.Point, part.Location);
        }
        anim.Start();
      }
    }

    /// <summary>
    /// If the user changes to "copying" mode by holding down the Control key,
    /// return to the regular behavior and remove the image.
    /// </summary>
    public override void DoKeyDown() {
      if (_ImagePart != null && _OriginalDraggedParts != null &&
        (Diagram.LastInput.Control || Diagram.LastInput.Meta) && MayCopy()) {
        DraggedParts = _OriginalDraggedParts;
        Diagram.Remove(_ImagePart);
      }
      base.DoKeyDown();
    }

    /// <summary>
    /// If the user changes back to "moving" mode,
    /// show the image again and go back to dragging the ghost dragged parts.
    /// </summary>
    public override void DoKeyUp() {
      if (_ImagePart != null && _GhostDraggedParts != null && MayMove()) {
        _ImagePart.Location = Diagram.LastInput.DocumentPoint;
        Diagram.Add(_ImagePart);
        DraggedParts = _GhostDraggedParts;
      }
      base.DoKeyUp();
    }
  }
}
