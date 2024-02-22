/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Extensions.VirtualizedPacked {
  public partial class VirtualizedPacked : DemoControl {
    private Diagram _Diagram;
    public Model _WholeModel = new Model();
    private Part _LoadingPart;

    public VirtualizedPacked() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();
      desc1.MdText = DescriptionReader.Read("Extensions.VirtualizedPacked.md");

      AfterLoad(() => _AfterLoaded());
    }

    // Don't load the graph until the control has rendered;
    // allows loading indicator to be shown as graph loads and ensures timing
    private void _AfterLoaded() {
      // Allow the loading indicator to be shown,
      // but allow objects added in load to also be considered part of the initial Diagram.
      // If you are not going to add temporary initial Parts, don't call DelayInitialization.
      _Diagram.DelayInitialization(_Load);
    }

    private void Setup() {
      _Diagram.AnimationManager.IsEnabled = false;
      _Diagram.InitialScale = 0.25;
      _Diagram.Layout = new VirtualizedPackedGroupsLayout();
      _Diagram.InitialLayoutCompleted += (s, e) => {
        NodeData first = null;
        var arr = _WholeModel.NodeDataSource;
        for (var i = 0; i < arr.Count(); i++) {
          var d = arr.ElementAt(i);
          if (!d.IsGroup) { first = d; break; }
        }
        if (first != null) {
          e.Diagram.CenterRect(first.Bounds);
        }
      };

      string[] myColors = { "0,0,0", "0,255,0", "255,0,0", "0,0,255" };
      int[] myLayoutFactors = { 16, 8, 4, 2 };

      object fillBinding(object depth) { if ((int)depth >= myColors.Length) depth = 0; return "rgba(" + myColors[(int)depth] + ",0.1)"; }
      object strokeBinding(object depth) { if ((int)depth >= myColors.Length) depth = 0; return "rgb(" + myColors[(int)depth] + ")"; }

      _Diagram.NodeTemplate =
        new Node("Auto") {
            IsLayoutPositioned = false,  // optimization
            Width = 50, Height = 50  // in cooperation with the load function, below
          }
          .Bind("Position", "Bounds", (b, _) => { return ((Rect)b).Position; })
          .Add(
            new Shape("Circle") {
                Spot1 = Spot.TopLeft, Spot2 = Spot.BottomRight,
                PortId = "", Fill = "white", Stroke = "gray"
              }
              .Bind("Fill", "Depth", fillBinding)
              .Bind("Stroke", "Depth", strokeBinding),
            new TextBlock()
              .Bind("Text", "Key", (k) => k.ToString())
          );

      _Diagram.GroupTemplate =
        new Group("Auto") {
            IsLayoutPositioned = false // optimization
          }
          .Bind("Position", "Bounds", (b, _) => { var r = (Rect)b; return new Point(r.X - r.Width * 0.05, r.Y - r.Height * 0.05); })
          .Bind("DesiredSize", "Bounds", (b, _) => { var r = (Rect)b; return new Size(r.Size.Width * 1.1, r.Size.Height * 1.1); })
          .Add(
            new Shape("Ellipse") {
                Spot1 = new Spot(0.05, 0.05), Spot2 = new Spot(0.95, 0.95),
                PortId = "", Fill = "white", Stroke = "gray"
              }
              .Bind("Fill", "Depth", fillBinding)
              .Bind("Stroke", "Depth", strokeBinding),
            new TextBlock()
              .Bind("Text", "Key", (k) => k.ToString())
          );

      // This model includes all of the data
      _WholeModel =
        new Model();  // must match the model used by the Diagram, below

      // The virtualized layout works on the full model, not on the Diagram nodes and links
      (_Diagram.Layout as VirtualizedPackedGroupsLayout).Model = _WholeModel;

      // Do not set myDiagram.Model = myWholeModel -- that would create a zillion Nodes and Links!
      // In the future Diagram may have built-in support for virtualization.
      // For now, we have to implement virtualization ourselves by having the Diagram's model
      // be different than the "real" model.
      _Diagram.Model =  // this only holds nodes that should be in the viewport
        new Model();  // must match the model, above

      // for now we have to implement virtualization ourselves
      _Diagram.IsVirtualized = true;
      _Diagram.ViewportBoundsChanged += _OnViewportChanged;

      // This is a status message
      _LoadingPart =
        new Part() {  // this has to set the location or position explicitly
            Location = new Point(0, 0), Scale = 4
          }
          .Add(
            new TextBlock("loading...") {
              Stroke = "red", Font = new Font("Segoe UI", 20, FontUnit.Point)
            }
          );

      // temporarily add the status indicator
      _Diagram.Add(_LoadingPart);
    }

    // The following code creates a large randomized graph with nested groups in myWholeModel.

    private void _Load(Diagram diag) {
      // create a lot of data for the myWholeModel
      _AddGraph(_WholeModel, 123456, 50, 4, 1.0);
      // remove the status indicator
      diag.Remove(_LoadingPart);
    }

    // The following code creates a large randomized graph with nested groups in mywholemodel.
    private static void _AddGraph(Model model, int totnodes, int maxmembers, int maxdepth, double percentgroup) {
      model.TopGroups = new List<NodeData>();
      _AddGraphInternal(model, totnodes, maxmembers, maxdepth, percentgroup, 0, null);
    }

    private static void _AddGraphInternal(Model model, int totnodes, int maxmembers, int maxdepth, double percentgroup, int depth, NodeData groupdata) {
      // groupdata may be null for top-level nodes
      var rand = new Random();
      var nextkey = model.NodeDataSource.Count() + 1;
      var nextlkey = model.LinkDataSource.Count() + 1;
      if (nextkey > totnodes) return;
      var numnodes = (int)Math.Floor(rand.NextDouble() * (maxmembers - 1)) + 2;
      if (nextkey + numnodes > totnodes) numnodes = totnodes - nextkey + 1;
      var nodes = new List<NodeData>();
      var links = new List<LinkData>();
      var lcount = 0;
      for (var i = 0; i < numnodes; i++) {
        var data = new NodeData { Key = nextkey + i, Depth = depth };  // Bounds = undefined??
        if (groupdata != null) {
          if (!groupdata.IsGroup || groupdata.Members == null) {
            throw new Exception("not a group data: " + groupdata);
          }
          // initially no .Bounds property for group data
          data.Group = groupdata.Key;
          groupdata.Members.Add(data);
        }
        if (depth < maxdepth && rand.NextDouble() < percentgroup) {
          data.IsGroup = true;
          data.Members = new List<IBounded>();
          if (groupdata == null) model.TopGroups.Add(data);  // only remember top-level groups
        } else {
          //!!!???@@@ this needs to be customized to account for your chosen node template
          data.Bounds = new Rect(0, 0, 50, 50);
        }
        nodes.Add(data);
        if (i > 0) {
          links.Add(new LinkData { Key = -(nextlkey + lcount), From = nextkey, To = nextkey + i });
          lcount++;
        }
      }
      for (var i = 1; i <= numnodes / 3; i++) {
        // additional links between nodes other than the first one
        var from = (int)Math.Floor(rand.NextDouble() * (numnodes - 1)) + 1;
        var to = (int)Math.Floor(rand.NextDouble() * (numnodes - 1)) + 1;
        links.Add(new LinkData { Key = -(nextlkey + lcount), From = nodes[from].Key, To = nodes[to].Key });
        lcount++;
      }
      model.AddNodeData(nodes);
      model.AddLinkData(links);
      foreach (var data in nodes) {
        if (data.IsGroup) {
          _AddGraphInternal(model, totnodes, maxmembers, maxdepth, percentgroup, depth + 1, data);
        }
      }
    }

    // The following functions implement virtualization of the Diagram
    // Assume data.Bounds is a Rect of the area occupied by the Node in document coordinates.

    // The normal mechanism for determining the size of the document depends on all of the
    // Nodes and Links existing, so we need to use a function that depends only on the model data.
    private Rect _ComputeDocumentBounds(Model model) {
      var b = new Rect();
      var ndata = model.NodeDataSource;
      var i = 0;
      foreach (var d in ndata) {
        if (d.Bounds == default) continue;
        if (i == 0) {
          b = d.Bounds;
        } else {
          b = b.Union(d.Bounds);
        }
        i++;
      }
      return b;
    }

    // As the user scrolls or zooms, make sure the Parts (Nodes and Links) exist in the viewport.
    private async void _OnViewportChanged(object s, DiagramEvent e) {
      var diagram = e.Diagram;
      // make sure there are Nodes for each node data that is in the viewport
      // or that is connected to such a Node
      var viewb = diagram.ViewportBounds;  // the new viewport bounds
      var model = diagram.Model as Model;

      var oldskips = diagram.SkipsUndoManager;
      diagram.SkipsUndoManager = true;

      var b = new Rect();
      var ndata = _WholeModel.NodeDataSource;
      foreach (var n in ndata) {
        if (n.Bounds == default) continue;
        if (n.Bounds.Intersects(viewb)) {
          model.AddNodeData(n);
        }
      }

      var ldata = _WholeModel.LinkDataSource;
      foreach (var l in ldata) {
        var fromkey = _WholeModel.GetFromKeyForLinkData(l);
        var from = _WholeModel.FindNodeDataForKey(fromkey);
        if (from == null || from.Bounds == default) continue;

        var tokey = _WholeModel.GetToKeyForLinkData(l);
        var to = _WholeModel.FindNodeDataForKey(tokey);
        if (to == null || to.Bounds == default) continue;

        b = from.Bounds;
        b = b.Union(to.Bounds);
        if (b.Intersects(viewb)) {
          // also make sure both connected nodes are present,
          // so that link routing is authentic
          model.AddNodeData(from);
          model.AddNodeData(to);
          model.AddLinkData(l);
          var link = diagram.FindLinkForData(l);
          if (link != null) {
            // do this now to avoid delayed routing outside of transaction
            link.FromNode.EnsureBounds();
            link.ToNode.EnsureBounds();
            link.UpdateRoute();
          }
        }
      }

      diagram.SkipsUndoManager = oldskips;
      await Task.Delay(3000);
      _RemoveOffscreen(diagram);

      _UpdateCounts();
    }

    private void _RemoveOffscreen(Diagram diagram) {
      if (_MaybeInvoke(diagram)) return; // ensure we're on the correct thread

      var viewb = diagram.ViewportBounds;
      var model = diagram.Model as Model;
      var remove = new List<NodeData>();  // collect for later removal
      var removeLinks = new HashSet<Link>();  // links connected to a node data to remove
      var it = diagram.Nodes;
      foreach (var n in it) {
        if (n.Data is not NodeData d) continue;
        if (!n.ActualBounds.Intersects(viewb) && !n.IsSelected) {
          // even if the node is out of the viewport, keep it if it is selected or
          // if any link connecting with the node is still in the viewport
          if (!n.LinksConnected.Any(l => { return l.ActualBounds.Intersects(viewb); })) {
            remove.Add(d);
            if (model is Model) {
              removeLinks.UnionWith(n.LinksConnected);
            }
          }
        }
      }

      if (remove.Count > 0) {
        var oldskips = diagram.SkipsUndoManager;
        diagram.SkipsUndoManager = true;
        model.RemoveNodeData(remove);
        foreach (var l in removeLinks) {
          if (!l.IsSelected) model.RemoveLinkData(l.Data as LinkData);
        }
        diagram.SkipsUndoManager = oldskips;
      }

      _UpdateCounts();  // only for this sample
    }

    // This function is only used in this sample to demonstrate the effects of the virtualization.
    // In a real application you would delete this function and all calls to it.
    private void _UpdateCounts() {
      nodesTb.Text = "Node data in Model: " + _WholeModel.NodeDataSource.Count() + ". Actual Nodes in Diagram: " + _Diagram.Nodes.Count() + ".";
      linksTb.Text = "Link data in Model: " + _WholeModel.LinkDataSource.Count() + ". Actual Links in Diagram: " + _Diagram.Links.Count() + ".";
    }

  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> {
    public List<Demo.Extensions.VirtualizedPacked.NodeData> TopGroups { get; set; } = new List<Demo.Extensions.VirtualizedPacked.NodeData>();
  }

  public class NodeData : Model.NodeData, IBounded {
    public Rect Bounds { get; set; }
    public List<IBounded> Members { get; set; } = null;
    public int Depth { get; set; }
  }

  public class LinkData : Model.LinkData { }

  class VirtualizedPackedGroupsLayout : VirtualizedPackedLayout {
    public Model Model { get; set; }
    private List<IBounded> _TopLevelNodes = new List<IBounded>();

    public VirtualizedPackedGroupsLayout() : base() {
      IsOngoing = false;
      Model = null;
      SortMode = VSortMode.Area;
      HasCircularNodes = true;
    }

    public override void DoLayout(IEnumerable<Part> coll = null) {  // ignore arg
      if (Model == null) return;
      var nodes = Model.NodeDataSource;
      var topGroups = Model.TopGroups;
      var maxdiam = 0d;
      if (topGroups != null) {
        foreach (var g in topGroups) {
          _WalkGroups(g);
          maxdiam = Math.Max(maxdiam, Math.Max(g.Bounds.Width, g.Bounds.Height));
        }
      }
      _TopLevelNodes.Clear();
      foreach (var n in nodes) {
        if (n.Group == 0) _TopLevelNodes.Add(n);
        maxdiam = Math.Max(maxdiam, Math.Max(n.Bounds.Width, n.Bounds.Height));
      }
      Spacing = Math.Max(50, maxdiam * 0.2);
      PerformLayout(_TopLevelNodes);  // only top-level nodes
      Diagram.FixedBounds = ActualBounds;
    }

    private void _WalkGroups(NodeData g) {
      if (g == null || !g.IsGroup || g.Members == null) throw new Exception("not a group data: " + g);
      var mems = g.Members;
      if (g.Members.Count > 0) {
        var maxdiam = 0d;
        foreach (var n in mems) {
          if ((n as NodeData).IsGroup) {
            _WalkGroups(n as NodeData);
          }
          maxdiam = Math.Max(maxdiam, Math.Max(n.Bounds.Width, n.Bounds.Height));
        }
        Spacing = Math.Max(50, maxdiam * 0.2);
        PerformLayout(mems);
        g.Bounds = ActualBounds;
      } else {
        //!!!???@@@ this needs to be customized to account for your chosen Group template
        g.Bounds = new Rect(0, 0, 50, 50);
      }
    }

    public override void MoveNode(IBounded node, double nx, double ny) {
      var dx = nx - node.Bounds.X;
      var dy = ny - node.Bounds.Y;
      _ShiftNode(node, dx, dy);
    }

    private void _ShiftNode(IBounded bounded, double dx, double dy) {
      var node = bounded as NodeData;
      node.Bounds = node.Bounds.Offset(dx, dy);
      if (node.IsGroup) {
        var mems = node.Members;
        if (mems != null) {
          foreach (var n in mems) {
            _ShiftNode(n, dx, dy);
          }
        }
      }
    }
  }
}
