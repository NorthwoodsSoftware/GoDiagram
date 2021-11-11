/*
*  Copyright (C) 1998-2020 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main Go library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in GoExamples under the Extensions folder.
* See the Extensions intro page (<replace>) for more information.
*/

using System;
using System.Collections.Generic;


namespace Northwoods.Go.Layouts.Extensions {
  /**
  * A custom <see cref="TreeLayout"/> that can be used for laying out stylized flowcharts.
  * Each layout requires a single "Split" node and a single "Merge" node.
  * The "Split" node should be the root of a tree-like structure if one excludes links to the "Merge" node.
  * This will position the "Merge" node to line up with the "Split" node.
  *
  * You can set all of the TreeLayout properties that you like,
  * except that for simplicity this code just works for angle == 0 or angle == 90.
  *
  * If you want to experiment with this extension, try the Parallel Layout sample.
  * @category Layout Extension
  */
  public class ParallelLayout : TreeLayout {

    /// <summary>
    /// Constructs a Parallel layout and sets the following properties:
    ///   - <see cref="Layout.IsRealtime"/> = false
    ///   - <see cref="TreeLayout.Alignment"/> = <see cref="TreeAlignment.CenterChildren"/>
    ///   - <see cref="TreeLayout.Compaction"/> = <see cref="TreeCompaction.None"/>
    ///   - <see cref="TreeLayout.AlternateAlignment"/> = <see cref="TreeAlignment.CenterChildren"/>
    ///   - <see cref="TreeLayout.AlternateCompaction"/> = <see cref="TreeCompaction.None"/>
    /// </summary>
    public ParallelLayout() : base() {
      IsRealtime = false;
      Alignment = TreeAlignment.CenterChildren;
      Compaction = TreeCompaction.None;
      AlternateAlignment = TreeAlignment.CenterChildren;
      AlternateCompaction = TreeCompaction.None;
    }

    /// <summary>
    /// This read-only property returns the node that the tree will extend from.
    /// </summary>
    public Node SplitNode { get; private set; } = null;

    /// <summary>
    /// This read-only property returns the node that the tree will converge at.
    /// </summary>
    public Node MergeNode { get; private set; } = null;

    /// <summary>
    /// Overridable predicate for deciding if a Node is a Split node.
    /// </summary>
    /// <remarks>
    /// By default this checks the node's <see cref="Part.Category"/> to see if it is
    /// "Split", "Start", "For", "While", "If", or "Switch".
    /// </remarks>
    /// <param name="node"></param>
    /// <returns></returns>
    public virtual bool IsSplit(Node node) {
      if (node == null) return false;
      var cat = node.Category;
      return cat == "Split" || cat == "Start" || cat == "For" || cat == "While" || cat == "If" || cat == "Switch";
    }

    /// <summary>
    /// Overridable predicate for deciding if a Node is a Merge node.
    /// </summary>
    /// <remarks>
    /// By default this checks the node's <see cref="Part.Category"/> to see if it is
    /// "Merge", "End", "EndFor", "EndWhile", "EndIf", or "EndSwitch".
    /// </remarks>
    /// <param name="node"></param>
    /// <returns></returns>
    public virtual bool IsMerge(Node node) {
      if (node == null) return false;
      var cat = node.Category;
      return cat == "Merge" || cat == "End" || cat == "EndFor" || cat == "EndWhile" || cat == "EndIf" || cat == "EndSwitch";
    }

    /// <summary>
    /// Overridable predicate for deciding if a Node is a conditional or "If" type of Split Node
    /// expecting to have two links coming out of the sides.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public virtual bool IsConditional(Node node) {
      if (node == null) return false;
      return node.Category == "If";
    }

    /// <summary>
    /// Overridable predicate for deciding if a Node is a "Switch" type of Split Node
    /// expecting to have three links coming out of the bottom/right side.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public virtual bool IsSwitch(Node node) {
      if (node == null) return false;
      return node.Category == "Switch";
    }

    /// <summary>
    /// Determines the split and merge nodes for a set of vertexes.
    /// </summary>
    /// <remarks>
    /// This signals an error if there is not exactly one Node that <see cref="IsSplit(Node)"/>
    /// and exactly one Node that <see cref="IsMerge(Node)"/>.
    /// This can be overridden; any override must set <see cref="SplitNode"/> and <see cref="MergeNode"/>.
    /// </remarks>
    /// <param name="vertexes"></param>
    public virtual void FindSplitMerge(IEnumerable<TreeVertex> vertexes) {
      Node split = null;
      Node merge = null;
      foreach (var v in vertexes) {
        if (v.Node == null) continue;
        if (IsSplit(v.Node)) {
          if (split != null) Trace.Error("Split node already exists in " + this + " -- existing: " + split + " new: " + v.Node);
          split = v.Node;
        } else if (IsMerge(v.Node)) {
          if (merge != null) Trace.Error("Merge node already exists in " + this + " -- existing: " + merge + " new: " + v.Node);
          merge = v.Node;
        }
      }
      if (split == null) Trace.Error("Missing Split node in " + this);
      if (merge == null) Trace.Error("Missing Merge node in " + this);
      SplitNode = split;
      MergeNode = merge;
    }

    /// <summary>
    /// Create and initialize a <see cref="TreeNetwork"/> with the given nodes and links.
    /// This override finds the split and merge nodes and sets the focus of any <see cref="Group"/>s.
    /// </summary>
    /// <param name="coll"></param>
    /// <returns>TreeNetwork</returns>
    [Undocumented]
    public override TreeNetwork MakeNetwork(IEnumerable<Part> coll = null) {
      var net = base.MakeNetwork(coll);
      // Groups might be unbalanced -- position them so that the Split node is centered under the parent node.
      foreach (var v in net.Vertexes) {
        var node = v.Node;
        if (node is Group g && g.IsSubGraphExpanded && g.Placeholder != null && g.Layout is ParallelLayout pl) {
          var split = pl.SplitNode;
          if (split != null) {
            if (Angle == 0) {
              v.FocusY = split.Location.Y - g.Position.Y;
            } else if (Angle == 90) {
              v.FocusX = split.Location.X - g.Position.X;
            }
          }
        }
      }
      if (Group != null && !Group.IsSubGraphExpanded) return net;
      // Look for and remember the one Split node and the one Merge node
      FindSplitMerge(net.Vertexes);
      // Don't have TreeLayout lay out the Merge node; CommitNodes will do it
      if (MergeNode != null) net.DeleteNode(MergeNode);
      return net;
    }

    /// <summary>
    /// Assigns a position for the merge node once the other nodes have been committed.
    /// </summary>
    [Undocumented]
    protected override void CommitNodes() {
      base.CommitNodes();
      // Line up the Merge node to the center of the Split node
      if (MergeNode == null || SplitNode == null || Network == null) return;
      var splitVertex = Network.FindVertex(SplitNode);
      if (splitVertex == null) return;
      // line up the "Merge" node to the center of the "Split" node
      if (Angle == 0) {
        MergeNode.Location = new Point(splitVertex.X + splitVertex.SubtreeSize.Width + LayerSpacing + MergeNode.ActualBounds.Width / 2,
                                       splitVertex.CenterY);
      } else if (Angle == 90) {
        MergeNode.Location = new Point(splitVertex.CenterX,
                                       splitVertex.Y + splitVertex.SubtreeSize.Height + LayerSpacing + MergeNode.ActualBounds.Height / 2);
      }
      MergeNode.EnsureBounds();
    }

    /// <inheritdoc/>
    protected override void CommitLinks() {
      if (SplitNode == null || MergeNode == null || Network == null) return;
      // Set default link spots based on Angle
      foreach (var e in Network.Edges) {
        var link = e.Link;
        if (link == null) continue;
        if (Angle == 0) {
          if (SetsPortSpot) link.FromSpot = Spot.Right;
          if (SetsChildPortSpot) link.ToSpot = Spot.Left;
        } else if (Angle == 90) {
          if (SetsPortSpot) link.FromSpot = Spot.Bottom;
          if (SetsChildPortSpot) link.ToSpot = Spot.Top;
        }
      }
      // Make sure links coming into and going out of a Split node come in the correct way
      if (SplitNode != null) {
        var cond = IsConditional(SplitNode);
        var swtch = IsSwitch(SplitNode);
        var first = true;
        foreach (var link in SplitNode.FindLinksOutOf()) {
          if (Angle == 0) {
            if (SetsPortSpot) link.FromSpot = cond ? (first ? Spot.Top : Spot.Bottom) : (swtch ? Spot.RightSide : Spot.Right);
            if (SetsChildPortSpot) link.ToSpot = Spot.Left;
          } else if (Angle == 90) {
            if (SetsPortSpot) link.FromSpot = cond ? (first ? Spot.Left : Spot.Right) : (swtch ? Spot.BottomSide : Spot.Bottom);
            if (SetsChildPortSpot) link.ToSpot = Spot.Top;
          }
          first = false;
        }
      }
      if (MergeNode != null) {
        // Handle links going into the Merge node
        foreach (var link in MergeNode.FindLinksInto()) {
          if (!IsSplit(link.FromNode)) {  // if link connects Split with Merge directly, only set FromSpot once
            if (Angle == 0) {
              if (SetsPortSpot) link.FromSpot = Spot.Right;
              if (SetsChildPortSpot) link.ToSpot = Spot.Left;
            } else if (Angle == 90) {
              if (SetsPortSpot) link.FromSpot = Spot.Bottom;
              if (SetsChildPortSpot) link.ToSpot = Spot.Top;
            }
          }
          if (!link.IsOrthogonal) continue;
          // Have all of the links coming into the Merge node have segments
          // that share a common X (or if angle == 90, Y) coordinate
          link.UpdateRoute();
          if (link.PointsCount >= 6) {
            var pts = new List<Point>(link.Points);
            var p2 = pts[pts.Count - 4];
            var p3 = pts[pts.Count - 3];
            if (Angle == 0 && p2.X == p3.X) {
              var x = MergeNode.Position.X - LayerSpacing / 2;
              pts[pts.Count - 4] = new Point(x, p2.Y);
              pts[pts.Count - 3] = new Point(x, p3.Y);
            } else if (Angle == 90 && p2.Y == p3.Y) {
              var y = MergeNode.Position.Y - LayerSpacing / 2;
              pts[pts.Count - 4] = new Point(p2.X, y);
              pts[pts.Count - 3] = new Point(p3.X, y);
            }
            link.Points = pts;
          }
        }
        // Handle links coming out of the Merge node, looping back left/up
        foreach (var link in MergeNode.FindLinksOutOf()) {
          // If connects internal with external node, it isn't a loop-back link
          if (link.ToNode != null && link.ToNode.ContainingGroup != MergeNode.ContainingGroup) continue;
          if (Angle == 0) {
            if (SetsPortSpot) link.FromSpot = Spot.TopBottomSides;
            if (SetsChildPortSpot) link.ToSpot = Spot.TopBottomSides;
          } else if (Angle == 90) {
            if (SetsPortSpot) link.FromSpot = Spot.LeftRightSides;
            if (SetsChildPortSpot) link.ToSpot = Spot.LeftRightSides;
          }
          link.Routing = LinkRouting.AvoidsNodes;
        }
      }
    }
  }
}
