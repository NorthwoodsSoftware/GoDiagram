using System;
using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go.Layouts.Extensions;
using Northwoods.Go.Tools;

namespace WinFormsExtensionControls.TreeMapSample {
  [ToolboxItem(false)]
  public partial class TreeMapSampleControl : System.Windows.Forms.UserControl {

    public Diagram myDiagram;

    private int _MinNodes = 300;
    private int _MaxNodes = 500;
    private int _MinChil = 2;
    private int _MaxChil = 5;
    public TreeMapSampleControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      minNodes.Leave += (e, obj) => _MinNodes = int.Parse(minNodes.Text);
      maxNodes.Leave += (e, obj) => _MaxNodes = int.Parse(maxNodes.Text);
      minChildren.Leave += (e, obj) => _MinChil = int.Parse(minChildren.Text);
      maxChildren.Leave += (e, obj) => _MaxChil = int.Parse(maxChildren.Text);

      generateTree.Click += (e, obj) => RebuildGraph();

      goWebBrowser1.Html = @"
          <p>
          This sample demonstrates a custom Layout, TreeMapLayout, which assumes that the diagram consists of nested Groups and simple Nodes.
          Each node is positioned and sized to fill an area of the viewport proportionate to its ""size"", as determined by its Node.Data.Size property.
          Each Group gets a size that is the sum of all of its member Nodes.
          </p>
          <p>
          The layout is defined in its own file, as <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Layouts/TreeMap/TreeMapLayout.cs"">TreeMapLayout.cs</a>. 
          </p>
 
          <p>
          Clicking repeatedly at the same point will initially select the Node at that point, and then its containing Group, and so on up the chain of containers.
          </p>
";
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      myDiagram.InitialAutoScale = AutoScaleType.Uniform;
      myDiagram.AllowMove = false;
      myDiagram.AllowCopy = false;
      myDiagram.AllowDelete = false;

      myDiagram.Layout = new TreeMapLayout();

      myDiagram.ToolManager.ClickSelectingTool = new TreeMapClickSelectingTool();

      myDiagram.NodeTemplate = new Node() {
        Background = "rgba(99,99,99,0.2)"
      }.Bind("Background", "Fill");

      myDiagram.GroupTemplate = new Group(PanelLayoutAuto.Instance) {
        Layout = null,
        Background = "rgba(99,99,99,0.2)",
      }.Bind("Background", "Fill");

      myDiagram.Model = new Model {
        NodeGroupKeyProperty = "Parent",
        NodeDataSource = GenerateNodeData()
      };

    }

    public void RebuildGraph() {
      myDiagram = diagramControl1.Diagram;

      var model = new Model {
        NodeGroupKeyProperty = "Parent",
        NodeDataSource = GenerateNodeData()
      };
      myDiagram.Model = model;
    }

    private List<NodeData> GenerateNodeData() {
      var nodeArray = new List<NodeData>();
      if (_MinNodes < 1 || double.IsNaN(_MinNodes)) _MinNodes = 1;
      if (_MaxNodes < 1 || double.IsNaN(_MaxNodes)) _MaxNodes = _MinNodes;

      var random = new Random();

      // Create a bunch of node data
      var numNodes = random.Next(_MinNodes, 1 + _MaxNodes);
      for (var i = 0; i < numNodes; i++) {
        var size = random.NextDouble() * random.NextDouble() * 10000; // non-uniform distribution
        nodeArray.Add(new NodeData {
          Key = i, // the unique identifier
          IsGroup = false, // many of these turn into groups, by code below
          Parent = int.MaxValue, // is set by code below that assigns children
          Text = i.ToString(), // some text to be shown by the node template
          Fill = Brush.RandomColor(), // a color to be shown by the node template
          Size = (int)size,
          Total = -1 // use a negative value to indicate that the total for the group has not been computed
        });
      }

      // takes the random collection of node data and creates a random tree with them.
      // respects the minimum and maximum number of links from each node.
      // The minimum can be disregarded if we run out of nodes to link to.
      if (nodeArray.Count > 1) {
        if (double.IsNaN(_MinChil) || _MinChil < 0) _MinChil = 0;
        if (double.IsNaN(_MaxChil) || _MaxChil < _MinChil) _MaxChil = _MinChil;

        // keep the Set of node data that do not yet have a parent
        var avail = new HashSet<NodeData>(nodeArray);
        foreach (var parent in nodeArray) {
          avail.Remove(parent);

          // assign some number of node data as children of this parent node data
          var children = random.Next(_MinChil, 1 + _MaxChil);
          for (var j = 0; j < children; j++) {
            if (avail.Count == 0) break; // already ran out !!
            var child = avail.First();

            avail.Remove(child);
            // have the child node data refer to the parent node data by its key
            child.Parent = parent.Key;
            if (!parent.IsGroup) { // make sure the parent is a group
              parent.IsGroup = true;
            }
            var par = parent;
            while (par != null) {
              par.Total += child.Total; // sum up sizes of all children
              if (par.Parent != int.MaxValue) {
                par = nodeArray[par.Parent];
              } else {
                break;
              }
            }
          }
        }
      }
      return nodeArray;
    }
  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }

  public class NodeData : Model.NodeData, ITreeMapData {
    public int Total { get; set; } = -1; // override from ITreeMapData, must be initialized to a negative value
    public int Size { get; set; } // override from ITreeMapData
    public Brush Fill { get; set; }
    public int Parent { get; set; }
  }

  public class LinkData : Model.LinkData { }

  public class TreeMapClickSelectingTool : ClickSelectingTool {
    public override void StandardMouseSelect() {
      var diagram = Diagram;
      if (diagram == null || !diagram.AllowSelect) return;

      var e = diagram.LastInput;
      if (!(e.Control || e.Meta) && !e.Shift) {
        var part = diagram.FindPartAt(e.DocumentPoint, false);
        if (part != null) {
          Part firstselected = null;
          var node = part;

          while (node != null) {
            if (node.IsSelected) {
              firstselected = node;
              break;
            } else {
              node = node.ContainingGroup;
            }
          }
          if (firstselected != null) {
            firstselected.IsSelected = false;
            var group = firstselected.ContainingGroup;
            if (group != null) group.IsSelected = true;
            return;
          }
        }
      }

      base.StandardMouseSelect();
    }
  }
}
