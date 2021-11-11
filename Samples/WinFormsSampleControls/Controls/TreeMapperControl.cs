using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.TreeMapper {
  [ToolboxItem(false)]
  public partial class TreeMapperControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    
    public TreeMapperControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      goWebBrowser1.Html = @"
        <p>
      This sample is like the <a href=""records.html"">Mapping Fields of Records</a> sample,
      but it has a collapsible tree of nodes on each side, rather than a simple list of items.
      The implementation of the trees comes from the <a href= ""treeView.html"">Tree View</a> sample.
        </p>
        <p>
      Draw new links by dragging from any field (i.e.any tree node).
      Reconnect a selected link by dragging its diamond-shaped handle.
        </p>
        <p> 
      The model data, automatically updated after each change or undo or redo:
        </p>
";

    }

    private void Setup() {

      myDiagram = diagramControl1.Diagram;

      myDiagram.CommandHandler.CopiesTree = true;
      myDiagram.CommandHandler.DeletesTree = true;
      // newly drawn links always map a node in one tree to a ndoe in another tree
      myDiagram.ToolManager.LinkingTool.ArchetypeLinkData = new LinkData { Category = "Mapping" };
      myDiagram.ToolManager.LinkingTool.LinkValidation = checkLink;
      myDiagram.ToolManager.RelinkingTool.LinkValidation = checkLink;
      myDiagram.UndoManager.IsEnabled = true;

      // All links must go from a node inside the "Left Side" Group to a node inside the "Right Side" group.
      static bool checkLink(Node fn, GraphObject fp, Node tn, GraphObject tp, Link link) {
        // make sure the nodes are inside different Groups
        if (fn.ContainingGroup == null || (fn.ContainingGroup.Data as NodeData).Key != -1) return false;
        if (tn.ContainingGroup == null || (tn.ContainingGroup.Data as NodeData).Key != -2) return false;
        //// optional limit to a single mapping link per node
        //if (fn.LinksConnected.Any(l => l.Category == "Mapping")) return false;
        //if (tn.LinksConnected.Any(l => l.Category == "Mapping")) return false;
        return true;
      }

      var treeExpander = Builder.Make<Panel>("TreeExpanderButton");
      treeExpander.Width = 14; treeExpander.Height = 14;

      // Each node in a tree is defined using the default NodeTemplate.
      myDiagram.NodeTemplate = new TreeNode {
        Movable = false,
        Copyable = false,
        Deletable = false, // user cannot move an individual node
        // no Adornment: instead change panel background color by binding to Node.IsSelected
        SelectionAdorned = false
      }.Add(
        treeExpander, // support expanding/collapsing subtrees
        new Panel(PanelLayoutHorizontal.Instance) {
          Position = new Point(16, 0)
        }.Add(
          // TODO add optional picture icon
          new TextBlock()
            .Bind("Text", "Key", (s, _) => "item " + ((int)s - 1).ToString())
        ).Bind(
          new Binding("Background", "IsSelected", (s, _) => (bool)s ? "lightblue" : "white").OfElement()
        )
      ).Bind(
        // whether the user can start drawing a link from or to this ndoe depends on which group it's in
        new Binding("FromLinkable", "Group", (k, _) => (int)k == -1),
        new Binding("ToLinkable", "Group", (k, _) => (int)k == -2)
      );

      // These are the links connecting tree nodes within each group.

      // myDiagram.LinkTemplate = new Link(); // without lines

      myDiagram.LinkTemplate = new Link { // with lines
        Selectable = false,
        Routing = LinkRouting.Orthogonal,
        FromEndSegmentLength = 4,
        ToEndSegmentLength = 4,
        FromSpot = new Spot(0.001, 1, 7, 0),
        ToSpot = Spot.Left
      }.Add(new Shape { Stroke = "lightgray" });

      // These are the blue links connecting a tree node on the left side with one on the right side
      myDiagram.LinkTemplateMap.Add("Mapping",
        new MappingLink {
          IsTreeLink = false, IsLayoutPositioned = false, LayerName = "Foreground",
          FromSpot = Spot.Right, ToSpot = Spot.Left,
          RelinkableFrom = true, RelinkableTo = true
        }.Add(
          new Shape {
            Stroke = "blue", StrokeWidth = 2
          }
        )
      );

      myDiagram.GroupTemplate = new Group(PanelLayoutAuto.Instance) {
        Deletable = false,
        Layout = new TreeLayout {
          Alignment = TreeAlignment.Start,
          Angle = 0,
          Compaction = TreeCompaction.None,
          LayerSpacing = 16,
          LayerSpacingParentOverlap = 1,
          NodeIndentPastParent = 1.0,
          NodeSpacing = 0,
          SetsPortSpot = false,
          SetsChildPortSpot = false
        }
      }.Bind(
        new Binding("Position", "XY", Point.Parse, Point.Stringify)
      ).Add(
        new Shape { Fill = "white", Stroke = "lightgray" },
        new Panel(PanelLayoutVertical.Instance) { DefaultAlignment = Spot.Left }.Add(
          new TextBlock {
            Font = "14px, style=bold", Margin = new Margin(5, 5, 0, 5)
          }.Bind("Text"),
          new Placeholder { Padding = 5 }
        )
      );

      var nodeDataSource = new List<NodeData> {
        new NodeData { IsGroup = true, Key = -1, Text = "Left Side", XY = "0 0" },
        new NodeData { IsGroup = true, Key = -2, Text = "Right Side", XY = "300 0" }
      };

      var linkDataSource = new List<LinkData> {
        new LinkData { From = 6, To = 1012, Category = "Mapping" },
        new LinkData { From = 4, To = 1006, Category = "Mapping" },
        new LinkData { From = 9, To = 1004, Category = "Mapping" },
        new LinkData { From = 1, To = 1009, Category = "Mapping" },
        new LinkData { From = 14, To = 1010, Category = "Mapping" }
      };

      // initialize tree on left side
      var root = new NodeData { Key = 1, Group = -1 };
      nodeDataSource.Add(root);
      for (var i = 0; i < 18; i++) {
        i = _MakeTree(3, i, 17, nodeDataSource, linkDataSource, root, -1, root.Key);
      }

      // initialize tree on right side
      root = new NodeData { Key = 1001, Group = -2 };
      nodeDataSource.Add(root);
      for (var i = 0; i < 15;) {
        i = _MakeTree(3, i, 15, nodeDataSource, linkDataSource, root, -2, root.Key);
      }
      myDiagram.Model = new Model {
        NodeDataSource = nodeDataSource,
        LinkDataSource = linkDataSource
      };

      myDiagram.ModelChanged += (obj, e) => {
        if (e.IsTransactionFinished) {  // show the model data in the page's TextArea
          textBox1.Text = myDiagram.Model.ToJson();
        }
      };
      
    }

    private int _MakeTree(int level, int count, int max, List<NodeData> nodeDataSource, List<LinkData> linkDataSource, NodeData parentdata, int groupkey, int rootkey) {
      var rand = new Random();
      var numchildren = rand.Next(10);
      for (var i = 0; i < numchildren; i++) {
        if (count >= max) return count;
        count++;
        var childdata = new NodeData { Key = rootkey + count, Group = groupkey };
        nodeDataSource.Add(childdata);
        linkDataSource.Add(new LinkData { From = parentdata.Key, To = childdata.Key });
        if (level > 0 && rand.NextDouble() > 0.5) {
          count = _MakeTree(level - 1, count, max, nodeDataSource, linkDataSource, childdata, groupkey, rootkey);
        }
      }
      return count;
    }

  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }

  public class NodeData : Model.NodeData {
    public string XY { get; set; }
  }

  public class LinkData : Model.LinkData { }

  public class TreeNode : Node {
    public TreeNode() : base() {
      TreeExpandedChanged = (node) => {
        if (node.ContainingGroup != null) {
          foreach (var l in node.ContainingGroup.FindExternalLinksConnected()) {
            l.InvalidateRoute();
          }
        }
      };
    }

    public override Node FindVisibleNode() {
      // redirect links to lowest visible "ancestor" in the tree
      Node n = this;
      while (n != null && !n.IsVisible()) {
        n = n.FindTreeParentNode();
      }
      return n;
    }
  }
  // end treeNode

  public class MappingLink : Link {
    public override Point GetLinkPoint(Node node, GraphObject port, Spot spot, bool from, bool ortho, Node othernode, GraphObject otherport) {
      var r = port.GetDocumentBounds();
      var group = node.ContainingGroup;
      var b = (group != null) ? group.ActualBounds : node.ActualBounds;
      var op = othernode.GetDocumentPoint(Spot.Center);
      var x = (op.X > r.CenterX) ? b.Right : b.Left;
      return new Point(x, r.CenterY);
    }
  }
  // end mappingLink

}
