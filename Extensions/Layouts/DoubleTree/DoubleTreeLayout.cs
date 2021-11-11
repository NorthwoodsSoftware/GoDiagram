/*
* Copyright (C) 1998-2020 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main GoJS library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in the GoJS kit under the extensions or extensionsTS folders.
* See the Extensions intro page (https://gojs.net/latest/intro/extensions.html) for more information.
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Northwoods.Go.Layouts.Extensions {
  /// <summary>
  /// Perform two <see cref="TreeLayout"/>s, one going rightwards and one going leftwards.
  /// The choice of direction is determined by the mandatory predicate <see cref="DirectionFunction"/>,
  /// which is called on each child Node of the root Node.
  ///
  /// You can also set <see cref="Vertical"/> to true if you want the DoubleTreeLayout to
  /// perform TreeLayouts both downwards and upwards.
  ///
  /// Normally there should be a single root node.  Hoewver if there are multiple root nodes
  /// found in the nodes and links that this layout is responsible for, this will pretend that
  /// there is a real root node and make all of the apparent root nodes children of that pretend root.
  ///
  /// If there is no root node, all nodes are involved in cycles, so the first given node is chosen.
  ///
  /// If you want to experiment with this extension, try the <a href="../../samples/doubleTree.html">Double Tree</a> sample.
  /// </summary>
  public class DoubleTreeLayout : Layout {
    private bool _Vertical = false;
    private Func<Node, bool> _DirectionFunction = (node) => { return true; };
    private TreeLayout _TopLeftOptions = null;
    private TreeLayout _BottomRightOptions = null;

    /// <summary>
    /// Create a DoubleTree layout.
    /// </summary>
    public DoubleTreeLayout() : base() { }

    /// <summary>
    /// Copies properties to a cloned Layout.
    /// </summary>
    /// <param name="c"></param>
    [Undocumented]
    protected override void CloneProtected(Layout c) {
      if (c == null) return;

      base.CloneProtected(c);
      var copy = (DoubleTreeLayout)c;
      copy._Vertical = _Vertical;
      copy._DirectionFunction = _DirectionFunction;
      copy._TopLeftOptions = _TopLeftOptions;
      copy._BottomRightOptions = _BottomRightOptions;
    }

    /// <summary>
    /// When false, the layout should grow towards the left and towards the right,
    /// when true, the layout should grow upwards and downwards.
    ///
    /// The default value is false.
    /// </summary>
    public bool Vertical {
      get {
        return _Vertical;
      }
      set {
        if (_Vertical != value) {
          _Vertical = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// This function is called on each child node of the root node
    /// in order to determine whether the subtree starting from that child node will grow towards
    /// larger coordinates or towards smaller ones.
    /// The value must be a function and must not be null
    ///
    /// It must return true if <see cref="IsPositiveDirection"/> should return true, otherwise it should return false.
    /// </summary>
    public Func<Node, bool> DirectionFunction {
      get {
        return _DirectionFunction;
      }
      set {
        if (_DirectionFunction != value) {
          _DirectionFunction = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Gets or sets the options to be applied to a <see cref="TreeLayout"/>.
    /// By default this is null -- no properties are set on the TreeLayout
    /// other than the <see cref="TreeLayout.Angle"/>, depending on <see cref="Vertical"/> and
    /// the result of calling <see cref="DirectionFunction"/>.
    /// </summary>
    public TreeLayout BottomRightOptions {
      get {
        return _BottomRightOptions;
      }
      set {
        if (_BottomRightOptions != value) {
          _BottomRightOptions = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Gets or sets the options to be applied to a <see cref="TreeLayout"/>.
    /// By default this is null -- no properties are set on the TreeLayout
    /// other than the <see cref="TreeLayout.Angle"/>, depending on <see cref="Vertical"/> and
    /// the result of calling <see cref="DirectionFunction"/>.
    /// </summary>
    public TreeLayout TopLeftOptions {
      get {
        return _TopLeftOptions;
      }
      set {
        if (_TopLeftOptions != value) {
          _TopLeftOptions = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Perform two <see cref="TreeLayout"/>s by splitting the collection of Parts
    /// into two separate subsets but sharing only a single root Node.
    /// </summary>
    /// <param name="coll"></param>
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
      if (allparts.Count() == 0) return; // do nothing for an empty collection

      if (Diagram != null) Diagram.StartTransaction("Double Tree Layout");

      // split the nodes and links into two Sets, depending on direction
      var leftParts = new HashSet<Part>();
      var rightParts = new HashSet<Part>();
      SeparatePartsForLayout(allparts, leftParts, rightParts);
      // but the ROOT node will be in both collections

      // create and perform two TreeLayouts, one in each direction,
      // without moving the ROOT node, on the different subsets of nodes and links
      var layout1 = CreateTreeLayout(false);
      layout1.Angle = Vertical ? 270 : 180;
      layout1.Arrangement = TreeArrangement.FixedRoots;

      var layout2 = CreateTreeLayout(true);
      layout2.Angle = Vertical ? 90 : 0;
      layout2.Arrangement = TreeArrangement.FixedRoots;

      layout1.DoLayout(leftParts);
      layout2.DoLayout(rightParts);

      if (Diagram != null) Diagram.CommitTransaction("Double Tree Layout");
    }

    /// <summary>
    /// This just returns an instance of <see cref="TreeLayout"/>.
    /// The caller will set the <see cref="TreeLayout.Angle"/> and <see cref="TreeLayout.Arrangement"/>.
    /// </summary>
    /// <param name="positive">true for growth downward or rightward, false otherwise</param>
    protected TreeLayout CreateTreeLayout(bool positive) {
      var lay = new TreeLayout();
      var opts = positive ? BottomRightOptions : TopLeftOptions;
      if (opts == null) return lay;

      // copy all properties of opts into lay using reflection
      foreach (var prop in opts.GetType().GetProperties()) {
        prop.SetValue(lay, prop.GetValue(opts));
      }

      return lay;
    }

    /// <summary>
    /// This is called by <see cref="DoLayout(IEnumerable{Part})"/> to split the collection of Nodes and links into two subsets,
    /// one for the subtrees growing towards the left or upwards, and one for the subtrees
    /// growing towards the right or downwards.
    /// </summary>
    /// <param name="coll"></param>
    /// <param name="leftParts"></param>
    /// <param name="rightParts"></param>
    protected void SeparatePartsForLayout(HashSet<Part> coll, HashSet<Part> leftParts, HashSet<Part> rightParts) {
      Node root = null; // the one root
      var roots = new HashSet<Node>(); // in case there are multiple roots
      foreach (var part in coll) {
        if (part is Node && (part as Node).FindTreeParentNode() == null) roots.Add(part as Node);
      }

      if (roots.Count == 0) { // just choose the first node as the root
        foreach (var part in coll) {
          if (part is Node) {
            root = part as Node;
            break;
          }
        }
      } else if (roots.Count == 1) { // normal case : just one root node
        root = roots.First();
      } else { // multiple root nodes -- create a dummy node to be the one real root
        root = new Node {
          Location = new Point(0, 0)
        };
        var forwards = Diagram == null || Diagram.IsTreePathToChildren;

        // now make dummy links from the one root node to each node
        foreach (var child in roots) {
          var link = new Link();
          if (forwards) {
            link.FromNode = root;
            link.ToNode = child;
          } else {
            link.FromNode = child;
            link.ToNode = root;
          }
        }
      }

      if (root == null) return; // couldn't find a root

      // the ROOT node is shared by both subtrees
      leftParts.Add(root);
      rightParts.Add(root);

      var lay = this;
      // look at all of the immediate children of the ROOT node
      foreach (var child in root.FindTreeChildrenNodes()) {
        // in what direction is this child growing?
        var bottomRight = lay.IsPositiveDirection(child);
        var parts = bottomRight ? rightParts : leftParts;

        // add the whole subtree starting with this child node
        foreach (var part in child.FindTreeParts()) {
          parts.Add(part);
        }

        // and also add the link from the ROOT node to this child node
        var plink = child.FindTreeParentLink();
        if (plink != null) parts.Add(plink);
      }
    }

    /// <summary>
    /// This predicate is called on each child node of the root node,
    /// and only on immediate children of the root.
    /// It should return true if this child node is the root of a subtree that should grow
    /// rightwards or downwards, or false otherwise.
    /// </summary>
    /// <param name="child"></param>
    /// <returns>true if it grows towards right or towards bottom; false otherwise</returns>
    protected bool IsPositiveDirection(Node child) {
      if (DirectionFunction == null) {
        throw new Exception("No DoubleTreeLayout.DirectionFunction supplied on the layout");
      }
      return DirectionFunction(child);
    }
  }
}
