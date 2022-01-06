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

namespace Northwoods.Go.Tools.Extensions {

  /// <summary>
  /// The RealtimeDragSelectingTool class lets the user select and deselect Parts within the <see cref="DragSelectingTool.Box"/>
  /// during a drag, not just at the end of the drag.
  /// </summary>
  /// <remarks>
  /// If you want to experiment with this extension, try the <a href="../../extensions/RealtimeDragSelecting.html">Realtime Drag Selecting</a> sample.
  /// </remarks>
  /// @category Tool Extension
  public class RealtimeDragSelectingTool : DragSelectingTool {
    private HashSet<Part> _OriginalSelection;
    private readonly HashSet<Part> _TemporarySelection;

    /// <summary>
    /// Constructs a RealtimeDragSelectingTool.
    /// </summary>
    public RealtimeDragSelectingTool() : base() {
      _OriginalSelection = new HashSet<Part>();
      _TemporarySelection = new HashSet<Part>();
    }

    /// <summary>
    /// Remember the original collection of selected Parts.
    /// </summary>
    public override void DoActivate() {
      base.DoActivate();
      // keep a copy of the original Set of selected Parts
      _OriginalSelection = new HashSet<Part>(Diagram.Selection);
      // these Part.IsSelected may have been temporarily modified
      _TemporarySelection.Clear();
      Diagram.RaiseChangingSelection();
    }

    /// <summary>
    /// Release any references to selected Parts.
    /// </summary>
    public override void DoDeactivate() {
      Diagram.RaiseChangedSelection();
      _OriginalSelection.Clear();
      _TemporarySelection.Clear();
      base.DoDeactivate();
    }

    /// <summary>
    /// Restore the selection which may have been modified during a drag.
    /// </summary>
    public override void DoCancel() {
      var orig = _OriginalSelection;
      foreach (var p in orig) {
        p.IsSelected = true;
      }
      foreach (var p in _TemporarySelection) {
        if (!orig.Contains(p)) p.IsSelected = false;
      }
      base.DoCancel();
    }

    /// <summary>
    /// Select Parts within the bounds of the drag-select box.
    /// </summary>
    public override void DoMouseMove() {
      if (IsActive) {
        base.DoMouseMove();
        SelectInRect(ComputeBoxBounds());
      }
    }

    /// <summary>
    /// Select Parts within the bounds of the drag-select box.
    /// </summary>
    public override void DoKeyDown() {
      if (IsActive) {
        base.DoKeyDown();
        SelectInRect(ComputeBoxBounds());
      }
    }

    /// <summary>
    /// Select Parts within the bounds of the drag-select box.
    /// </summary>
    public override void DoKeyUp() {
      if (IsActive) {
        base.DoKeyUp();
        SelectInRect(ComputeBoxBounds());
      }
    }

    /// <summary>
    /// For a given rectangle, select Parts that are within that rectangle.
    /// </summary>
    /// <param name="r">rectangular bounds in document coordinates.</param>
    public override void SelectInRect(Rect r) {
      var diagram = Diagram;
      var orig = _OriginalSelection;
      var temp = _TemporarySelection;
      var e = diagram.LastInput;
      var found = diagram.FindPartsIn(r, IsPartialInclusion);
      if (e.Control || e.Meta) {  // toggle or deselect
        if (e.Shift) {  // deselect only
          foreach (var p in temp) {
            if (!found.Contains(p)) p.IsSelected = orig.Contains(p);
          }
          foreach (var p in found) {
            p.IsSelected = false; temp.Add(p);
          }
        } else {  // toggle selectedness of parts based on _OriginalSelection
          foreach (var p in temp) {
            if (!found.Contains(p)) p.IsSelected = orig.Contains(p);
          }
          foreach (var p in found) {
            p.IsSelected = !orig.Contains(p); temp.Add(p);
          }
        }
      } else if (e.Shift) {  // extend selection only
        foreach (var p in temp) {
          if (!found.Contains(p)) p.IsSelected = orig.Contains(p);
        }
        foreach (var p in found) {
          p.IsSelected = true; temp.Add(p);
        }
      } else {  // select found parts, and unselect all other previously selected parts
        foreach (var p in temp) {
          if (!found.Contains(p)) p.IsSelected = false;
        }
        foreach (var p in orig) {
          if (!found.Contains(p)) p.IsSelected = false;
        }
        foreach (var p in found) {
          p.IsSelected = true; temp.Add(p);
        }
      }
    }
  }
}
