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
using System.Collections.Generic;
using System.Linq;

namespace Northwoods.Go.Extensions {
  /// <summary>
  /// This enumeration specifies possible values for <see cref="DrawCommandHandler.ArrowKeyBehavior"/>.
  /// </summary>
  public enum ArrowBehavior {
    /// <summary>
    /// Moves the <see cref="Diagram.Selection"/>.
    /// </summary>
    Move,
    /// <summary>
    /// Changes the <see cref="Diagram.Selection"/> to a nearby Part in the arrow's direction.
    /// </summary>
    Select,
    /// <summary>
    /// Scrolls the diagram in the arrow's direction.
    /// </summary>
    Scroll,
    /// <summary>
    /// Changes the selected node in a tree and expands or collapses subtrees.
    /// </summary>
    Tree,
    /// <summary>
    /// Does nothing on arrow key press.
    /// </summary>
    None
  }

  /// <summary>
  /// This CommandHandler class allows the user to position selected Parts in a diagram relative to the
  /// first part selected, in addition to overriding the DoKeyDown method
  /// of the CommandHandler for handling the arrow keys in additional manners.
  /// </summary>
  public class DrawCommandHandler : CommandHandler {
    private ArrowBehavior _ArrowKeyBehavior = ArrowBehavior.Move;
    private Point _PasteOffset = new(10, 10);
    private Point _LastPasteOffset = new(0, 0);

    /// <summary>
    /// Gets or sets the arrow key behavior. Possible values can be found in the <see cref="ArrowBehavior"/> enum.
    /// </summary>
    /// <remarks>
    /// The default value is <see cref="ArrowBehavior.Move"/>.
    /// </remarks>
    public ArrowBehavior ArrowKeyBehavior {
      get {
        return _ArrowKeyBehavior;
      }
      set {
        if (value != _ArrowKeyBehavior) {
          _ArrowKeyBehavior = value;
        }
      }
    }

    /// <summary>
    /// Gets or sets the offset at which each repeated <see cref="CommandHandler.PasteSelection"/> puts the new copied parts from the clipboard.
    /// </summary>
    /// <remarks>
    /// The default value is (10, 10).
    /// </remarks>
    public Point PasteOffset {
      get {
        return _PasteOffset;
      }
      set {
        if (_PasteOffset != value) {
          _PasteOffset = value;
        }
      }
    }

    /// <summary>
    /// This controls whether or not the user can invoke the <see cref="AlignLeft"/>, <see cref="AlignRight"/>,
    /// <see cref="AlignTop"/>, <see cref="AlignBottom"/>, <see cref="AlignCenterX"/>, <see cref="AlignCenterY"/> commands.
    /// </summary>
    /// <returns>
    /// This returns true:
    ///   if the diagram is not <see cref="Diagram.IsReadOnly"/>,
    ///   if the model is not <see cref="Models.Model{TNodeData, TNodeKey, TSharedData}.IsReadOnly"/>,
    ///   if there are at least two selected <see cref="Part"/>s.
    /// </returns>
    public bool CanAlignSelection() {
      var diagram = Diagram;
      if (diagram.IsReadOnly || diagram.IsModelReadOnly) return false;
      if (diagram.Selection.Count < 2) return false;
      return true;
    }

    /// <summary>
    /// Aligns selected parts along the left-most edge of the left-most part.
    /// </summary>
    public void AlignLeft() {
      var diagram = Diagram;
      diagram.StartTransaction("aligning left");
      var minPosition = double.PositiveInfinity;
      foreach (var current in diagram.Selection) {
        if (current is Link) continue;  // skips over Link
        minPosition = Math.Min(current.Position.X, minPosition);
      }
      foreach (var current in diagram.Selection) {
        if (current is Link) continue;  // skips over Links
        current.Move(new Point(minPosition, current.Position.Y));
      }
      diagram.CommitTransaction("aligning left");
    }

    /// <summary>
    /// Aligns selected parts along the left-most edge of the left-most part.
    /// </summary>
    public void AlignRight() {
      var diagram = Diagram;
      diagram.StartTransaction("aligning right");
      var maxPosition = double.NegativeInfinity;
      foreach (var current in diagram.Selection) {
        if (current is Link) continue;  // skips over Link
        maxPosition = Math.Max(current.Position.X + current.ActualBounds.Width, maxPosition);
      }
      foreach (var current in diagram.Selection) {
        if (current is Link) continue;  // skips over Links
        current.Move(new Point(maxPosition - current.ActualBounds.Width, current.Position.Y));
      }
      diagram.CommitTransaction("aligning right");
    }

    /// <summary>
    /// Aligns selected parts along the top-most edge of the top-most part.
    /// </summary>
    public void AlignTop() {
      var diagram = Diagram;
      diagram.StartTransaction("aligning top");
      var minPosition = double.PositiveInfinity;
      foreach (var current in diagram.Selection) {
        if (current is Link) continue;  // skips over Link
        minPosition = Math.Min(minPosition, current.Position.Y);
      }
      foreach (var current in diagram.Selection) {
        if (current is Link) continue;  // skips over Links
        current.Move(new Point(current.Position.X, minPosition));
      }
      diagram.CommitTransaction("aligning top");
    }

    /// <summary>
    /// Aligns selected parts along the bottom-most edge of the bottom-most part.
    /// </summary>
    public void AlignBottom() {
      var diagram = Diagram;
      diagram.StartTransaction("aligning bottom");
      var maxPosition = double.NegativeInfinity;
      foreach (var current in diagram.Selection) {
        if (current is Link) continue;  // skips over Link
        maxPosition = Math.Max(maxPosition, current.Position.Y + current.ActualBounds.Height);
      }
      foreach (var current in diagram.Selection) {
        if (current is Link) continue;  // skips over Links
        current.Move(new Point(current.Position.X + current.ActualBounds.Width, maxPosition));
      }
      diagram.CommitTransaction("aligning bottom");
    }

    /// <summary>
    /// Aligns selected parts at the x-value of the center point of the first selected part.
    /// </summary>
    public void AlignCenterX() {
      var diagram = Diagram;
      var firstSelection = diagram.Selection.First();
      if (firstSelection == null) return;
      diagram.StartTransaction("aligning Center X");
      var centerX = firstSelection.ActualBounds.X + firstSelection.ActualBounds.Width / 2;
      foreach (var current in diagram.Selection) {
        if (current is Link) return;  // skips over Links
        current.Move(new Point(centerX - current.ActualBounds.Width / 2, current.ActualBounds.Y));
      }
      diagram.CommitTransaction("aligning Center X");
    }

    /// <summary>
    /// Aligns selected parts at the y-value of the center point of the first selected part.
    /// </summary>
    public void AlignCenterY() {
      var diagram = Diagram;
      var firstSelection = diagram.Selection.First();
      if (firstSelection == null) return;
      diagram.StartTransaction("aligning Center Y");
      var centerY = firstSelection.ActualBounds.Y + firstSelection.ActualBounds.Height / 2;
      foreach (var current in diagram.Selection) {
        if (current is Link) return;  // skips over Links
        current.Move(new Point(current.ActualBounds.X, centerY - current.ActualBounds.Height / 2));
      }
      diagram.CommitTransaction("aligning Center Y");
    }

    /// <summary>
    /// Aligns selected parts top-to-bottom in order of the order selected.
    /// </summary>
    /// <remarks>
    /// Distance between parts can be specified. Default distance is 0.
    /// </remarks>
    public void AlignColumn(int distance = 0) {
      var diagram = Diagram;
      diagram.StartTransaction("align Column");
      var selectedParts = new List<Part>();
      foreach (var current in diagram.Selection) {
        if (current is Link) return;
        selectedParts.Add(current);
      }
      for (var i = 0; i < selectedParts.Count - 1; i++) {
        var current = selectedParts[i];
        // adds distance specified between parts
        var curBottomSideLoc = current.ActualBounds.Y + current.ActualBounds.Height + distance;
        var next = selectedParts[i + 1];
        next.Move(new Point(current.ActualBounds.X, curBottomSideLoc));
      }
      diagram.CommitTransaction("align Column");
    }

    /// <summary>
    /// Aligns selected parts left-to-right in order of the order selected.
    /// </summary>
    /// <remarks>
    /// Distance between parts can be specified. Default distance is 0.
    /// </remarks>
    public void AlignRow(int distance = 0) {
      var diagram = Diagram;
      diagram.StartTransaction("align Row");
      var selectedParts = new List<Part>();
      foreach (var current in diagram.Selection) {
        if (current is Link) return;
        selectedParts.Add(current);
      }
      for (var i = 0; i < selectedParts.Count - 1; i++) {
        var current = selectedParts[i];
        // adds distance specified between parts
        var curRightSideLoc = current.ActualBounds.X + current.ActualBounds.Width + distance;
        var next = selectedParts[i + 1];
        next.Move(new Point(curRightSideLoc, current.ActualBounds.Y));
      }
      diagram.CommitTransaction("align Row");
    }

    /// <summary>
    /// This controls whether or not the user can invoke the <see cref="Rotate"/> method.
    /// </summary>
    /// <returns>
    /// This returns true:
    ///   if the diagram is not <see cref="Diagram.IsReadOnly"/>,
    ///   if the model is not <see cref="Models.Model{TNodeData, TNodeKey, TSharedData}.IsReadOnly"/>,
    ///   if there is at least one selected <see cref="Part"/>s.
    /// </returns>
    public bool CanRotate() {
      var diagram = Diagram;
      if (diagram.IsReadOnly || diagram.IsModelReadOnly) return false;
      if (diagram.Selection.Count < 1) return false;
      return true;
    }

    /// <summary>
    /// Change the angle of the parts connected with the given part.
    /// </summary>
    /// <remarks>
    /// This is in the command handler
    /// so it can easily be accessed for the purpose of creating commands that change the rotation of parts.
    /// </remarks>
    /// <param name="angle">the positive (clockwise) or negative (counter-clockwise) change in the angle of each Part, in degrees.</param>
    public void Rotate(double angle = 90) {
      var diagram = Diagram;
      diagram.StartTransaction("rotate " + angle.ToString());
      foreach (var current in diagram.Selection) {
        if (current is Link || current is Group) return;  // skip over links and Groups
        current.Angle += angle;
      }
      diagram.CommitTransaction("rotate " + angle.ToString());
    }

    /// <summary>
    /// Change the z-ordering of selected parts to pull them forward, in front of all other parts
    /// in their respective layers.
    /// </summary>
    /// <remarks>
    /// All unselected parts in each layer with a selected Part with a non-numeric <see cref="Part.ZOrder"/> will get a ZOrder of 0.
    /// </remarks>
    public void PullToFront() {
      var diagram = Diagram;
      diagram.StartTransaction("pullToFront");
      // find the affected Layers
      var layers = new Dictionary<Layer, double>();
      foreach (var part in diagram.Selection) {
        if (part.Layer != null) {
          if (!layers.ContainsKey(part.Layer)) layers.Add(part.Layer, 0);
        }
      }
      var keys = new Layer[layers.Count];
      layers.Keys.CopyTo(keys, 0);
      // find the maximum z-order in each Layer
      foreach (var layer in keys) {
        var max = 0.0;
        foreach (var part in layer.Parts) {
          if (part.IsSelected) continue;
          var z = part.ZOrder;
          if (double.IsNaN(z)) {
            part.ZOrder = 0;
          } else {
            max = Math.Max(max, z);
          }
        }
        layers[layer] = max;
      }
      // assign each selected Part.ZOrder to the computed value for each Layer
      foreach (var part in diagram.Selection) {
        var z = layers.ContainsKey(part.Layer) ? layers[part.Layer] : 0;
        _AssignZOrder(part, z + 1);
      }
      diagram.CommitTransaction("pullToFront");
    }

    /// <summary>
    /// Change the z-ordering of selected parts to push them backward, behind all other parts
    /// in their respective layers.
    /// </summary>
    /// <remarks>
    /// All unselected parts in each layer with a selected Part with a non-numeric <see cref="Part.ZOrder"/> will get a ZOrder of 0.
    /// </remarks>
    public void PushToBack() {
      var diagram = Diagram;
      diagram.StartTransaction("pushToBack");
      // find the affected Layers
      var layers = new Dictionary<Layer, double>();
      foreach (var part in diagram.Selection) {
        if (part.Layer != null) {
          if (!layers.ContainsKey(part.Layer)) layers.Add(part.Layer, 0);
        }
      }
      var keys = new Layer[layers.Count];
      layers.Keys.CopyTo(keys, 0);
      // find the maximum z-order in each Layer
      foreach (var layer in keys) {
        var min = 0.0;
        foreach (var part in layer.Parts) {
          if (part.IsSelected) continue;
          var z = part.ZOrder;
          if (double.IsNaN(z)) {
            part.ZOrder = 0;
          } else {
            min = Math.Min(min, z);
          }
        }
        layers[layer] = min;
      }
      // assign each selected Part.ZOrder to the computed value for each Layer
      foreach (var part in diagram.Selection) {
        var z = layers.ContainsKey(part.Layer) ? layers[part.Layer] : 0;
        _AssignZOrder(part, z - 1 - _FindGroupDepth(part));
      }
      diagram.CommitTransaction("pushToBack");
    }

    private static void _AssignZOrder(Part part, double z, Part root = null) {
      if (root == null) root = part;
      if (part.Layer == root.Layer) part.ZOrder = z;
      if (part is Group g) {
        foreach (var m in g.MemberParts) {
          _AssignZOrder(m, z + 1, root);
        }
      }
    }

    private static int _FindGroupDepth(Part part) {
      if (part is Group g) {
        var d = 0;
        foreach (var m in g.MemberParts) {
          d = Math.Max(d, _FindGroupDepth(m));
        }
        return d + 1;
      }
      return 0;
    }

    /// <summary>
    /// This implements custom behaviors for arrow key keyboard events.
    /// </summary>
    /// <remarks>
    /// Set <see cref="ArrowKeyBehavior"/> to select, move, scroll, or none
    /// This affects the behavior when a user types an arrow key.
    /// </remarks>
    public override void DoKeyDown() {
      var diagram = Diagram;
      var e = diagram.LastInput;

      // determines the function of the arrow keys
      if (e.Key == "ARROWUP" || e.Key == "ARROWDOWN" || e.Key == "ARROWLEFT" || e.Key == "ARROWRIGHT") {
        var behavior = ArrowKeyBehavior;
        switch (behavior) {
          case ArrowBehavior.None:
            break;
          case ArrowBehavior.Select:
            _ArrowKeySelect();
            break;
          case ArrowBehavior.Move:
            _ArrowKeyMove();
            break;
          case ArrowBehavior.Tree:
            _ArrowKeyTree();
            break;
          case ArrowBehavior.Scroll:
          // drop through to default to get the default scrolling behavior
          default:
            base.DoKeyDown();
            break;
        }
      } else {
        base.DoKeyDown();
      }
    }

    /// <summary>
    /// Collects in an Array all of the nonLink parts currently in the Diagram.
    /// </summary>
    /// <returns></returns>
    private List<Part> _GetAllParts() {
      var allparts = new List<Part>();
      foreach (var node in Diagram.Nodes) allparts.Add(node);
      foreach (var part in Diagram.Parts) allparts.Add(part);
      return allparts;  // note that this ignores Links (and Adornments)
    }

    /// <summary>
    /// To be called when the arrow keys should move the <see cref="Diagram.Selection"/>.
    /// </summary>
    private void _ArrowKeyMove() {
      var diagram = Diagram;
      var e = diagram.LastInput;
      // moves all selected parts in the specified direction
      var vdistance = 0.0;
      var hdistance = 0.0;
      // if control is held, move pixel by pixel. Else, moves by grid cell size
      if (e.Control || e.Meta) {
        vdistance = hdistance = 1;
      } else if (diagram.Grid != null) {
        var cellsize = diagram.Grid.GridCellSize;
        hdistance = cellsize.Width;
        vdistance = cellsize.Height;
      }
      diagram.StartTransaction("arrowKeyMove");
      foreach (var part in diagram.Selection) {
        if (e.Key == "ARROWUP") {
          part.Move(new Point(part.ActualBounds.X, part.ActualBounds.Y - vdistance));
        } else if (e.Key == "ARROWDOWN") {
          part.Move(new Point(part.ActualBounds.X, part.ActualBounds.Y + vdistance));
        } else if (e.Key == "ARROWLEFT") {
          part.Move(new Point(part.ActualBounds.X - hdistance, part.ActualBounds.Y));
        } else if (e.Key == "ARROWRIGHT") {
          part.Move(new Point(part.ActualBounds.X + hdistance, part.ActualBounds.Y));
        }
      }
      diagram.CommitTransaction("arrowKeyMove");
    }

    /// <summary>
    /// To be called when arrow keys should change selection.
    /// </summary>
    private void _ArrowKeySelect() {
      var diagram = Diagram;
      var e = diagram.LastInput;
      // with a part selected, arrow keys change the selection
      // arrow keys + shift selects the additional part in the specified direction.
      // arrow keys + control toggles the selection of the additional part.
      Part nextPart = null;
      if (e.Key == "ARROWUP") {
        nextPart = _FindNearestPartTowards(270);
      } else if (e.Key == "ARROWDOWN") {
        nextPart = _FindNearestPartTowards(90);
      } else if (e.Key == "ARROWLEFT") {
        nextPart = _FindNearestPartTowards(180);
      } else if (e.Key == "ARROWRIGHT") {
        nextPart = _FindNearestPartTowards(0);
      }
      if (nextPart != null) {
        if (e.Shift) {
          nextPart.IsSelected = true;
        } else if (e.Control || e.Meta) {
          nextPart.IsSelected = !nextPart.IsSelected;
        } else {
          diagram.Select(nextPart);
        }
      }
    }

    /// <summary>
    /// Finds the nearest Part in the specified direction, based on their center points;
    /// if it doesn't find anything it just returns the current part.
    /// </summary>
    /// <param name="dir">the direction in degrees</param>
    /// <returns>closest Part found in the given direction</returns>
    private Part _FindNearestPartTowards(double dir) {
      var originalPart = Diagram.Selection.FirstOrDefault();
      if (originalPart == null) return null;
      var originalPoint = originalPart.ActualBounds.Center;
      var allparts = _GetAllParts();
      var closestDistance = double.PositiveInfinity;
      var closest = originalPart;  // if no part meets criteria, the same part remains selected

      foreach (var nextPart in allparts) {
        if (nextPart == originalPart) continue; // skips over currently selected part
        var nextPoint = nextPart.ActualBounds.Center;
        var angle = originalPoint.Direction(nextPoint);
        var anglediff = _AngleCloseness(angle, dir);
        if (anglediff <= 45) {  // if this part's center is within the desired direction's sector
          var distance = originalPoint.DistanceSquared(nextPoint);
          distance *= 1 + Math.Sin(anglediff * Math.PI / 180);  // based on difference from intended angle
          if (distance < closestDistance) {
            closestDistance = distance;
            closest = nextPart;
          }
        }
      }

      return closest;
    }

    private static double _AngleCloseness(double a, double dir) {
      return Math.Min(Math.Abs(dir - a), Math.Min(Math.Abs(dir + 360 - a), Math.Abs(dir - 360 - a)));
    }

    /// <summary>
    /// To be called when arrow keys should change the selected node in a tree and expand or collapse subtrees.
    /// </summary>
    private void _ArrowKeyTree() {
      var selected = Diagram.Selection.FirstOrDefault();
      if (selected is not Node n) return;

      var e = Diagram.LastInput;
      if (e.Key == "ARROWRIGHT") {
        if (n.IsTreeLeaf) {
          // no-op
        } else if (!n.IsTreeExpanded) {
          if (Diagram.CommandHandler.CanExpandTree(n)) {
            Diagram.CommandHandler.ExpandTree(n);  // expands the tree
          }
        } else {
          var first = _SortTreeChildrenByY(n).FirstOrDefault();
          if (first != null) Diagram.Select(first);
        }
      } else if (e.Key == "ARROWLEFT") {
        if (!n.IsTreeLeaf && n.IsTreeExpanded) {
          if (Diagram.CommandHandler.CanCollapseTree(n)) {
            Diagram.CommandHandler.CollapseTree(n);  // collapses the tree
          }
        } else {  // either a leaf or is already collapsed -- select the parent node
          var parent = n.FindTreeParentNode();
          if (parent != null) Diagram.Select(parent);
        }
      } else if (e.Key == "ARROWUP") {
        var parent = n.FindTreeParentNode();
        if (parent != null) {
          var list = _SortTreeChildrenByY(parent);
          var idx = list.IndexOf(n);
          if (idx > 0) {  // if there is a previous sibling
            var prev = list[idx - 1];
            // keep looking at the last child until it's a leaf or collapsed
            while (prev != null && prev.IsTreeExpanded && !prev.IsTreeLeaf) {
              var children = _SortTreeChildrenByY(prev);
              prev = children.LastOrDefault();
            }
            if (prev != null) Diagram.Select(prev);
          } else {  // no previous sibling -- select parent
            Diagram.Select(parent);
          }
        }
      } else if (e.Key == "ARROWDOWN") {
        // if at an expanded parent, select the first child
        if (n.IsTreeExpanded && !n.IsTreeLeaf) {
          var first = _SortTreeChildrenByY(n).FirstOrDefault();
          if (first != null) Diagram.Select(first);
        } else {
          while (n != null) {
            var parent = n.FindTreeParentNode();
            if (parent == null) break;
            var list = _SortTreeChildrenByY(parent);
            var idx = list.IndexOf(n);
            if (idx < list.Count - 1) {  // select next lower node
              Diagram.Select(list[idx + 1]);
              break;
            } else {  // already at bottom of list of children
              n = parent;
            }
          }
        }
      }

      // make sure the selection is now in the viewport, but not necessarily centered
      var sel = Diagram.Selection.FirstOrDefault();
      if (sel != null) Diagram.ScrollToRect(sel.ActualBounds);
    }

    private static List<Node> _SortTreeChildrenByY(Node node) {
      var list = new List<Node>(node.FindTreeChildrenNodes());
      list.Sort((a, b) => {
        var aloc = a.Location;
        var bloc = b.Location;
        if (aloc.Y < bloc.Y) return -1;
        if (aloc.Y > bloc.Y) return 1;
        if (aloc.X < bloc.X) return -1;
        if (aloc.X > bloc.X) return 1;
        return 0;
      });
      return list;
    }

    /// <summary>
    /// Reset the last offset for pasting.
    /// </summary>
    public override void CopyToClipboard(IEnumerable<Part> coll) {
      base.CopyToClipboard(coll);
      _LastPasteOffset = _PasteOffset;
    }

    /// <summary>
    /// Paste from the clipboard with an offset incremented on each paste, and reset when copied.
    /// </summary>
    /// <returns>a collection of newly pasted <see cref="Part"/>s</returns>
    public override IReadOnlyCollection<Part> PasteFromClipboard() {
      var coll = base.PasteFromClipboard();
      Diagram.MoveParts(coll, _LastPasteOffset, false);
      _LastPasteOffset.Add(_PasteOffset);
      return coll;
    }
  }
}
