/*
*  Copyright (C) 1998-2022 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main Go library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in the GoDiagram repository (https://github.com/NorthwoodsSoftware/GoDiagram/tree/main/Extensions).
* See the Extensions intro page (https://godiagram.com/intro/extensions.html) for more information.
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Northwoods.Go.Layouts.Extensions {
  /// <summary>
  /// A custom <see cref="Layout"/> that lays out a chain of nodes in a snake-like fashion.
  /// </summary>
  /// <remarks>
  /// This layout assumes the graph is a chain of Nodes,
  /// positioning nodes in horizontal rows back and forth, alternating between left-to-right
  /// and right-to-left within the <see cref="Wrap"/> limit.
  /// <see cref="Spacing"/> controls the distance between nodes.
  /// <see cref="LeftSpot"/> and <see cref="RightSpot"/> determine the Spots to use for the <see cref="Link.FromSpot"/> and <see cref="Link.ToSpot"/>.
  ///
  /// When this layout is the Diagram.Layout, it is automatically invalidated when the viewport changes size.
  /// </remarks>
  /// @category Layout Extension
  public class SerpentineLayout : Layout {
    private Size _Spacing = new Size(30, 30);
    private double _Wrap = double.NaN;
    private Node _Root;
    private Spot _LeftSpot = Spot.Left;
    private Spot _RightSpot = Spot.Right;

    /// <summary>
    /// Constructs a SerpentineLayout and sets the <see cref="Layout.IsViewportSized"/> property to true.
    /// </summary>
    public SerpentineLayout() : base() {
      IsViewportSized = true;
    }

    /// <summary>
    /// Copies properties to a cloned Layout.
    /// </summary>
    [Undocumented]
    protected override void CloneProtected(Layout c) {
      if (c == null) return;

      base.CloneProtected(c);
      var copy = (SerpentineLayout)c;
      copy._Spacing = _Spacing;
      copy._Wrap = _Wrap;
      // don't copy root
      copy._LeftSpot = _LeftSpot;
      copy._RightSpot = _RightSpot;
    }

    /// <summary>
    /// Gets or sets the <see cref="Size"/> whose width specifies the horizontal space between nodes
    /// and whose height specifies the minimum vertical space between nodes.
    /// </summary>
    /// <remarks>
    /// The default value is 30x30.
    /// </remarks>
    public Size Spacing {
      get {
        return _Spacing;
      }
      set {
        if (_Spacing != value) {
          _Spacing = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Gets or sets the total width of the layout.
    /// </summary>
    /// <remarks>
    /// The default value is NaN, which for <see cref="Diagram.Layout"/>s means that it uses
    /// the <see cref="Diagram.ViewportBounds"/>.
    /// </remarks>
    public double Wrap {
      get {
        return _Wrap;
      }
      set {
        if (_Wrap != value) {
          _Wrap = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Gets or sets the starting node of the sequence.
    /// </summary>
    /// <remarks>
    /// The default value is null, which causes the layout to look for a node without any incoming links.
    /// </remarks>
    public Node Root {
      get {
        return _Root;
      }
      set {
        if (_Root != value) {
          _Root = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Gets or sets the Spot to use on the left side of a Node.
    /// </summary>
    /// <remarks>
    /// The default value is <see cref="Spot.Left"/>.
    /// </remarks>
    public Spot LeftSpot {
      get {
        return _LeftSpot;
      }
      set {
        if (_LeftSpot != value) {
          _LeftSpot = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Gets or sets the Spot to use on the right side of a Node.
    /// </summary>
    /// <remarks>
    /// The default value is <see cref="Spot.Right"/>.
    /// </remarks>
    public Spot RightSpot {
      get {
        return _RightSpot;
      }
      set {
        if (_RightSpot != value) {
          _RightSpot = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// This method actually positions all of the Nodes, assuming that the ordering of the nodes
    /// is given by a single link from one node to the next.
    /// </summary>
    /// <remarks>
    /// This respects the <see cref="Spacing"/> and <see cref="Wrap"/> properties to affect the layout.
    /// </remarks>
    /// <param name="coll">A collection of <see cref="Part"/>s.</param>
    public override void DoLayout(IEnumerable<Part> coll = null) {
      HashSet<Part> allparts;
      if (coll != null) {
        allparts = CollectParts(coll);
      } else if (Group != null) {
        allparts = CollectParts(Group);
      } else if (Diagram != null) {
        allparts = CollectParts(Diagram);
      } else {
        return; // Nothing to layout!
      }

      var root = Root;
      if (root == null) {
        // find a root node -- one without any incoming links
        foreach (var p in allparts) {
          if (p is not Node n) continue;
          if (root == null) root = n;
          if (!n.FindLinksInto().Any()) {
            root = n;
            break;
          }
        }
      }
      // couldn't find a root node
      if (root == null) return;

      var spacing = Spacing;
      // calculate the width at which we should start a new row
      var wrap = Wrap;
      if (Diagram != null && double.IsNaN(wrap)) {
        if (Group == null) {  // for a top-level layout, use the Diagram.ViewportBounds
          var pad = Diagram.Padding;
          wrap = Math.Max(spacing.Width * 2, Diagram.ViewportBounds.Width - 24 - pad.Left - pad.Right);
        } else {
          wrap = 1000; // provide a better default value?
        }
      }

      // implementations of doLayout that do not make use of a LayoutNetwork
      // need to perform their own transactions
      if (Diagram != null) Diagram.StartTransaction("Serpentine Layout");

      // start on the left, at Layout.ArrangementOrigin
      ArrangementOrigin = InitialOrigin(ArrangementOrigin);
      var x = ArrangementOrigin.X;
      var rowh = 0.0;
      var y = ArrangementOrigin.Y;
      var increasing = true;
      var node = root;
      while (node != null) {
        var orignode = node;
        if (node.ContainingGroup != null) node = node.ContainingGroup;
        var b = GetLayoutBounds(node);
        // get the next node, if any
        Link nextlink = null;
        foreach (var l in orignode.FindLinksOutOf()) {
          if (allparts.Contains(l)) { nextlink = l; break; }
        }
        var nextnode = nextlink?.ToNode;
        var orignextnode = nextnode;
        if (nextnode != null && nextnode.ContainingGroup != null) nextnode = nextnode.ContainingGroup;
        var nb = nextnode != null ? GetLayoutBounds(nextnode) : new Rect();
        if (increasing) {
          node.Move(new Point(x, y));
          x += b.Width;
          rowh = Math.Max(rowh, b.Height);
          if (x + spacing.Width + nb.Width > wrap) {
            y += rowh + spacing.Height;
            x = wrap - spacing.Width;
            rowh = 0;
            increasing = false;
            if (nextlink != null) {
              nextlink.FromSpot = Spot.Right;
              nextlink.ToSpot = Spot.Right;
            }
          } else {
            x += spacing.Width;
            if (nextlink != null) {
              nextlink.FromSpot = Spot.Right;
              nextlink.ToSpot = Spot.Left;
            }
          }
        } else {
          x -= b.Width;
          node.Move(new Point(x, y));
          rowh = Math.Max(rowh, b.Height);
          if (x - spacing.Width - nb.Width < 0) {
            y += rowh + spacing.Height;
            x = 0;
            rowh = 0;
            increasing = true;
            if (nextlink != null) {
              nextlink.FromSpot = Spot.Left;
              nextlink.ToSpot = Spot.Left;
            }
          } else {
            x -= spacing.Width;
            if (nextlink != null) {
              nextlink.FromSpot = Spot.Left;
              nextlink.ToSpot = Spot.Right;
            }
          }
        }
        node = orignextnode;
      }

      if (Diagram != null) Diagram.CommitTransaction("Serpentine Layout");
    }

  }

}
