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

namespace Northwoods.Go.Layouts.Extensions {
  /// <summary>
  /// A custom Layout that provides one way to have a layout of layouts.
  /// It partitions nodes and links into separate subgraphs, applies a primary
  /// layout to each subgraph, and then arranges those results by an
  /// arranging layout. Any disconnected nodes are laid out later by a
  /// side layout, by default in a grid underneath the main body of subgraphs.
  /// </summary>
  /// <remarks>
  /// This layout uses three separate Layouts.
  ///
  /// One is used for laying out nodes and links that are connected together: <see cref="PrimaryLayout"/>.
  /// This defaults to null and must be set to an instance of a <see cref="Layout"/>,
  /// such as a TreeLayout or a ForceDirectedLayout or a custom Layout.
  ///
  /// One is used to arrange separate subnetworks of the main graph: <see cref="ArrangeLayout"/>.
  /// This defaults to an instance of <see cref="GridLayout"/>.
  ///
  /// One is used for laying out the additional nodes along one of the sides of the main graph: <see cref="SideLayout"/>.
  /// This also defaults to an instance of <see cref="GridLayout"/>.
  /// 
  /// A filter predicate, <see cref="Filter"/>, splits up the collection of nodes and links into two subsets,
  /// one for the main layout and one for the side layout.
  /// By default, when there is no filter, it puts all nodes that have no link connections into the
  /// subset to be processed by the side layout.
  ///
  /// If all pairs of nodes in the main graph can be reached by some path of undirected links,
  /// there are no separate subnetworks, so the <see cref="ArrangeLayout"/> need not be used and
  /// the <see cref="PrimaryLayout"/> would apply to all of those nodes and links.
  ///
  /// But if there are disconnected subnetworks, the <see cref="PrimaryLayout"/> is applied to each subnetwork,
  /// and then all of those results are arranged by the <see cref="ArrangeLayout"/>.
  ///
  /// In either case if there are any nodes in the side graph, those are arranged by the <see cref="SideLayout"/>
  /// to be on the side of the arrangement of the main graph of nodes and links.
  ///
  /// Note: if you do not want to have singleton nodes be arranged by <see cref="SideLayout"/>,
  /// set <see cref="Filter"/> to <code language="cs">(part) => { return true; }</code>.
  /// That will cause all singleton nodes to be arranged by <see cref="ArrangeLayout"/> as if they
  /// were each their own subgraph.
  ///
  /// If you both don't want to use <see cref="SideLayout"/> and you don't want to use <see cref="ArrangeLayout"/>
  /// to lay out connected subgraphs, don't use this ArrangingLayout at all --
  /// just use whatever Layout you would have assigned to <see cref="PrimaryLayout"/>.
  /// </remarks>
  /// @category Layout Extension
  public class ArrangingLayout : NetworkLayout<ArrangingNetwork, ArrangingVertex, ArrangingEdge, ArrangingLayout> {
    private Func<Part, bool> _Filter;
    private Spot _Side;
    private Size _Spacing;
    private Layout _PrimaryLayout;
    private Layout _ArrangeLayout;
    private Layout _SideLayout;

    /// <summary>
    /// Constructs a new ArrangingLayout.
    /// </summary>
    public ArrangingLayout() : base() {
      Filter = null;
      Side = Spot.BottomSide;
      Spacing = new Size(20, 20);
      var play = new GridLayout() {
        CellSize = new Size(1, 1)
      };
      PrimaryLayout = play;
      var alay = new GridLayout() {
        CellSize = new Size(1, 1)
      };
      ArrangeLayout = alay;
      var slay = new GridLayout() {
        CellSize = new Size(1, 1)
      };
      SideLayout = slay;
    }

    /// <summary>
    /// Gets or sets the predicate function to call on each non-Link.
    /// </summary>
    /// <remarks>
    /// If the predicate returns true, the part will be laid out by the main layouts,
    /// the PrimaryLayouts and the ArrangingLayout, otherwise by the SideLayout.
    /// The default value is a function that is true when there are any links connecting with the node.
    /// Such default behavior will have the SideLayout position all of the singleton nodes.
    /// </remarks>
    public Func<Part, bool> Filter {
      get {
        return _Filter;
      }
      set {
        if (_Filter != value) {
          _Filter = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Gets or sets the side <see cref="Spot"/> where the side nodes and links should be laid out,
    /// relative to the results of the main Layout.
    /// </summary>
    /// <remarks>
    /// The default value is Spot.BottomSide.
    /// Currently only handles a single side.
    /// </remarks>
    public Spot Side {
      get {
        return _Side;
      }
      set {
        if (!value.IsSide()) {
          throw new Exception("new value for ArrangingLayout.Side must be a side Spot, not: " + value);
        }
        if (_Side != value) {
          _Side = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Gets or sets the space between the main layout and the side layout.
    /// </summary>
    /// <remarks>
    /// The default value is Size(20, 20).
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
    /// Gets or sets the Layout used for the main part of the diagram.
    /// </summary>
    /// <remarks>
    /// The default value is an instance of GridLayout.
    /// Any new value must not be null.
    /// </remarks>
    public Layout PrimaryLayout {
      get {
        return _PrimaryLayout;
      }
      set {
        if (value is not Layout layout) {
          throw new Exception("layout does not inherit from Layout: " + value);
        }
        _PrimaryLayout = value;
        InvalidateLayout();
      }
    }

    /// <summary>
    /// Gets or sets the Layout used to arrange multiple separate connected subgraphs of the main graph.
    /// </summary>
    /// <remarks>
    /// The default value is an instance of GridLayout.
    /// Set this property to null in order to get the default behavior of the <see cref="PrimaryLayout"/>
    /// when dealing with multiple connected graphs as a whole.
    /// </remarks>
    public Layout ArrangeLayout {
      get {
        return _ArrangeLayout;
      }
      set {
        if (value is not Layout layout) { // extra condition here before
          throw new Exception("layout does not inherit from Layout: " + value);
        }
        _ArrangeLayout = value;
        InvalidateLayout();
      }
    }

    /// <summary>
    /// Gets or sets the Layout used to arrange the "side" nodes and links -- those outside of the main layout.
    /// </summary>
    /// <remarks>
    /// The default value is an instance of GridLayout.
    /// Any new value must not be null.
    /// </remarks>
    public Layout SideLayout {
      get {
        return _SideLayout;
      }
      set {
        if (value is not Layout layout) {
          throw new Exception("layout does not inherit from Layout: " + value);
        }
        _SideLayout = value;
        InvalidateLayout();
      }
    }

    /// <summary>
    /// Copies properties to a cloned Layout.
    /// </summary>
    [Undocumented]
    protected override void CloneProtected(Layout c) {
      if (c == null) return;

      base.CloneProtected(c);
      var copy = (ArrangingLayout)c;
      copy._Filter = _Filter;
      if (_PrimaryLayout != null)
        copy._PrimaryLayout = _PrimaryLayout.Copy();
      if (_ArrangeLayout != null)
        copy._ArrangeLayout = _ArrangeLayout.Copy();
      if (_SideLayout != null)
        copy._SideLayout = _SideLayout.Copy();
      copy._Side = _Side;
      copy._Spacing = _Spacing;
    }

    /// <summary>
    /// Perform the layout.
    /// </summary>
    /// <param name="coll">the collection of Parts to layout.</param>
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
      if (allparts.Count == 0) return; // do nothing for an empty collection

      if (Diagram == null) throw new Exception("No Diagram for this Layout");
      // implementations of doLayout that do not make use of a LayoutNetwork
      // need to perform their own transactions
      Diagram.StartTransaction("Arranging Layout");
      var maincoll = new HashSet<Part>();
      var sidecoll = new HashSet<Part>();
      SplitParts(allparts, maincoll, sidecoll);
      ArrangingNetwork mainnet = null;
      IEnumerator<ArrangingNetwork> subnets = null;
      if (ArrangeLayout != null) {
        mainnet = MakeNetwork(maincoll);
        subnets = mainnet.SplitIntoSubNetworks<ArrangingNetwork>();
      }
      var bounds = new Rect();
      if (ArrangeLayout != null && mainnet != null && subnets != null && subnets.MoveNext()) {
        var groups = new Dictionary<Part, (IEnumerable<Part> Parts, Rect Bounds)>();
        subnets.Reset();
        while (subnets.MoveNext()) {
          var net = subnets.Current;
          var subcoll = net.FindAllParts();
          PreparePrimaryLayout(PrimaryLayout, subcoll);
          PrimaryLayout.DoLayout(subcoll);
          _AddMainNode(groups, subcoll, Diagram);
        }
        foreach (var v in mainnet.Vertexes) {
          if (v.Node != null) {
            var subcoll = new HashSet<Part> { v.Node };
            PreparePrimaryLayout(PrimaryLayout, subcoll);
            PrimaryLayout.DoLayout(subcoll);
            _AddMainNode(groups, subcoll, Diagram);
          }
        }
        ArrangeLayout.DoLayout(groups.Keys);
        foreach (var git in groups) {
          var grp = git.Key;
          var (Parts, Bounds) = git.Value;
          MoveSubgraph(Parts, Bounds, new Rect(grp.Position, grp.DesiredSize));
        }
        bounds = Diagram.ComputePartsBounds(groups.Keys); // not maincoll due to links without real bounds
      } else { // no ArrangingLayout
        PreparePrimaryLayout(PrimaryLayout, maincoll);
        PrimaryLayout.DoLayout(maincoll);
        bounds = Diagram.ComputePartsBounds(maincoll);
        MoveSubgraph(maincoll, bounds, bounds);
      }
      if (!bounds.IsReal())
        bounds = new Rect(0, 0, 0, 0);
      PrepareSideLayout(SideLayout, sidecoll, bounds);
      if (sidecoll.Count > 0) {
        SideLayout.DoLayout(sidecoll);
        var sidebounds = Diagram.ComputePartsBounds(sidecoll);
        if (!sidebounds.IsReal())
          sidebounds = new Rect(0, 0, 0, 0);
        MoveSideCollection(sidecoll, bounds, sidebounds);
      }
      Diagram.CommitTransaction("Arranging Layout");
    }

    private static void _AddMainNode(Dictionary<Part, (IEnumerable<Part> Parts, Rect Bounds)> groups, IEnumerable<Part> subcoll, Diagram diagram) {
      var grp = new Node {
        LocationSpot = Spot.Center
      };
      var grpb = diagram.ComputePartsBounds(subcoll);
      grp.DesiredSize = grpb.Size;
      grp.Position = grpb.Position;
      groups.Add(grp, (subcoll, grpb));
    }

    /// <summary>
    /// Assign all of the Parts in the given collection into either the
    /// set of Nodes and Links for the main graph or the set of Nodes and Links
    /// for the side graph.
    /// </summary>
    /// <remarks>
    /// By default this just calls the <see cref="Filter"/> on each non-Link to decide,
    /// and then looks at each Link's connected Nodes to decide.
    ///
    /// A null filter assigns all Nodes that have connected Links to the main graph, and
    /// all Links will be assigned to the main graph, and the side graph will only contain
    /// Parts with no connected Links.
    /// </remarks>
    public virtual void SplitParts(HashSet<Part> coll, HashSet<Part> maincoll, HashSet<Part> sidecoll) {
      // first consider all Nodes
      var pred = Filter;
      foreach (var p in coll) {
        if (p is Link) continue;
        var main = false;
        if (pred != null) {
          main = pred(p);
        } else if (p is Node node) {
          main = node.LinksConnected.Any();
        } else {
          main = p is Link;
        }
        if (main) {
          maincoll.Add(p);
        } else {
          sidecoll.Add(p);
        }
      }

      // now assign Links based on which Nodes they connect with
      foreach (var p in coll) {
        if (p is Link link) {
          if (link.FromNode == null || link.ToNode == null) continue;
          if (maincoll.Contains(link.FromNode) && maincoll.Contains(link.ToNode)) {
            maincoll.Add(p);
          } else if (sidecoll.Contains(link.FromNode) && sidecoll.Contains(link.ToNode)) {
            sidecoll.Add(p);
          }
        }
      }
    }

    /// <summary>
    /// This method is called just before the PrimaryLayout is performed so that
    /// there can be adjustments made to the PrimaryLayout, if desired.
    /// </summary>
    /// <remarks>
    /// By default this method makes no adjustments to the PrimaryLayout.
    /// </remarks>
    /// <param name="primaryLayout">the sideLayout that may be modified for the results of the PrimaryLayout</param>
    /// <param name="mainColl">the Nodes and Links to be laid out by PrimaryLayout after being separated into subnetworks</param>
    public virtual void PreparePrimaryLayout(Layout primaryLayout, IEnumerable<Part> mainColl) {
      // by default this is a no-op
    }

    /// <summary>
    /// Move a Set of Nodes and Links to the given area.
    /// </summary>
    /// <param name="subColl">the Set of Nodes and Links that form a separate connected subgraph</param>
    /// <param name="subbounds">the area occupied by the subColl</param>
    /// <param name="bounds">the area where they should be moved according to the ArrangingLayout</param>
    public virtual void MoveSubgraph(IEnumerable<Part> subColl, Rect subbounds, Rect bounds) {
      var diagram = Diagram;
      if (diagram == null) return;
      diagram.MoveParts(subColl, bounds.Position.Subtract(subbounds.Position), false);
    }

    /// <summary>
    /// This method is called just after the main layouts (the PrimaryLayouts and ArrangingLayout)
    /// have been performed and just before the SideLayout is performed so that there can be
    /// adjustments made to the sideLayout, if desired.
    /// </summary>
    /// <remarks>
    /// By default this method makes no adjustments to the sideLayout.
    /// </remarks>
    /// <param name="sideLayout">the SideLayout that may be modified for the results of the main layouts</param>
    /// <param name="sideColl">the Nodes and Links filtered out to be laid out by SideLayout</param>
    /// <param name="mainBounds">the area occupied by the nodes and links of the main layout, after it was performed</param>
    public virtual void PrepareSideLayout(Layout sideLayout, IEnumerable<Part> sideColl, Rect mainBounds) {
      // by default this is a no-op
    }

    /// <summary>
    /// This method is called just after the SideLayout has been performed in order to move
    /// its parts to the desired area relative to the results of the main layouts.
    /// </summary>
    /// <remarks>
    /// By default this calls <see cref="Diagram.MoveParts"/> on the sidecoll collection to the <see cref="Side"/> of the mainbounds.
    /// This won't get called if there are no Parts in the sidecoll collection.
    /// </remarks>
    /// <param name="sidecoll">a collection of Parts that were laid out by the SideLayout</param>
    /// <param name="mainbounds">the area occupied by the results of the main layouts</param>
    /// <param name="sidebounds">the area occupied by the results of the SideLayout</param>
    public virtual void MoveSideCollection(IEnumerable<Part> sidecoll, Rect mainbounds, Rect sidebounds) {
      var diagram = Diagram;
      if (diagram == null) return;
      if (Side.IncludesSide(Spot.BottomSide)) {
        diagram.MoveParts(sidecoll, new Point(mainbounds.X - sidebounds.X, mainbounds.Y + mainbounds.Height + Spacing.Height - sidebounds.Y), false);
      }
      else if (Side.IncludesSide(Spot.RightSide)) {
        diagram.MoveParts(sidecoll, new Point(mainbounds.X + mainbounds.Width + Spacing.Width - sidebounds.X, mainbounds.Y - sidebounds.Y), false);
      }
      else if (Side.IncludesSide(Spot.TopSide)) {
        diagram.MoveParts(sidecoll, new Point(mainbounds.X - sidebounds.X, mainbounds.Y - sidebounds.Height - Spacing.Height - sidebounds.Y), false);
      }
      else if (Side.IncludesSide(Spot.LeftSide)) {
        diagram.MoveParts(sidecoll, new Point(mainbounds.X - sidebounds.Width - Spacing.Width - sidebounds.X, mainbounds.Y - sidebounds.Y), false);
      }
    }
  }
}
