using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.IncrementalTree {
  [ToolboxItem(false)]
  public partial class IncrementalTreeControl : System.Windows.Forms.UserControl {
    private Diagram MyDiagram;

    private Random _Rand = new();

    public IncrementalTreeControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      btnZoomToFit.Click += (e, obj) => ZoomToFit();
      btnExpandRandomNode.Click += (e, obj) => ExpandAtRandom();

      goWebBrowser1.Html = @"
        <p>
          This diagram demonstrates the expansion of a tree where nodes are only created ""on-demand"",
          when the user clicks on the ""expand"" Button.
          As you expand the tree, it automatically performs a force-directed layout,
          which will make some room for the additional nodes.
        </p>
        <p>
          The data for each node is implemented by a JavaScript object held by the Diagram's model.
          Each node data has an <b>everExpanded</b> property that indicates whether we have already
          created all of its ""child"" data and added them to the model.
          The <b>everExpanded</b> property defaults to false,
          to match the initial value of <a>Node.IsTreeExpanded</a> in the node template,
          which specifies that the nodes start collapsed.
        </p>
        <p>
          When the user clicks on the ""expand"" Button, the button click event handler finds the corresponding
          data object by going up the visual tree to find the Node via the <a>GraphObject.Part</a> property
          and then getting the node data that the Node is bound to via <a>Part.Data</a>.
          If <b>everExpanded</b> is false, the code creates a random number of
          child data for that node, each with a random <b>color</b> property.
          Then the button click event handler changes the value of <b>Node.IsExpandedTree</b>,
          thereby expanding or collapsing the node.
        </p>
        <p>
          Some initial node expansions result in there being no children at all.
          In this case the Button is made invisible.
        </p>
        <p>
          All changes are performed within a transaction.
          You should always surround your code with calls to <a>Model.StartTransaction</a> and <a>Model.CommitTransaction</a>,
          or the same methods on <a>Diagram</a>.
        </p>
        <p>
          The diagram's <a>Diagram.Layout</a> is an instance of <a>ForceDirectedLayout</a>.
          The standard conditions under which the layout will be performed include
          when nodes or links or group-memberships are added or removed from the model,
          or when they change visibility.
          In this case that means that when the user expands or collapses a node,
          another force-directed layout occurs again, even upon repeated expansions or collapses.
        </p>
      ";
    }

    private void Setup() {

      MyDiagram = diagramControl1.Diagram;

      var blues = new List<Brush> {
        "#E1F5FE", "#B3E5FC", "#81D4FA", "#4FC3F7", "#29B6F6", "#03A9F4", "#039BE5", "#0288D1", "#0277BD", "#01579B"
      };

      MyDiagram.Layout = new ForceDirectedLayout();
      MyDiagram.InitialContentAlignment = Spot.Center;
      // moving and copying nodes also moves and copies their subtrees
      MyDiagram.CommandHandler.CopiesTree = true; // for the copy command
      MyDiagram.CommandHandler.DeletesTree = true; // for the delete command
      MyDiagram.ToolManager.DraggingTool.DragsTree = true; // dragging for both move and copy
      MyDiagram.UndoManager.IsEnabled = true;

      var expanderButton = Builder.Make<Panel>("TreeExpanderButton");
      expanderButton.Name = "TREEBUTTON";
      expanderButton.Width = 20;
      expanderButton.Height = 20;
      expanderButton.Alignment = Spot.TopRight;
      expanderButton.AlignmentFocus = Spot.Center;
      expanderButton.Click = (e, obj) => { // OBJ is the button
        // get the Node containing this button
        if (!(obj.Part is Node node)) return;
        e.Handled = true;
        _ExpandNode(node);
      };

      // Define the Node template.
      // This uses a Spot Panel to position a button relative
      // to the ellipse surrounding the text.
      MyDiagram.NodeTemplate = new Node(PanelLayoutSpot.Instance) {
        SelectionElementName = "PANEL",
        IsTreeExpanded = false,
        IsTreeLeaf = false
      }.Add(
       new Panel(PanelLayoutAuto.Instance) {
         Name = "PANEL"
       }.Add(
         new Shape("Circle") {
           Fill = "whitesmoke", Stroke = "black"
         }.Bind("Fill", "RootDistance", (dist, _) => {
           dist = Math.Min(blues.Count - 1, (int)dist);
           return blues[(int)dist];
         }),
         new TextBlock {
           Font = "General sans-serif, 12px",
           Margin = 5
         }.Bind("Text", "Key", (key, _) => { if ((int)key == -1) return "0"; return key.ToString(); })
       ),
       expanderButton
      );

      // create the model with a root node data
      MyDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = 0, Color = blues[0], EverExpanded = false, RootDistance = 0 }
        }
      };
    }

    private void _ExpandNode(Node node) {
      var diagram = node.Diagram;
      diagram.StartTransaction("CollapseExpandTree");
      // this behavior is specific to the incrementalTree sample:
      var data = node.Data as NodeData;
      if (!data.EverExpanded) {
        // only create children once per node
        diagram.Model.Set(data, "EverExpanded", true);
        var numchildren = _CreateSubTree(data);
        if (numchildren == 0) {
          node.FindElement("TREEBUTTON").Visible = false;
        }
      }
      // this behavior is generic for most expand/collapse tree buttons:
      if (node.IsTreeExpanded) {
        diagram.CommandHandler.CollapseTree(node);
      } else {
        diagram.CommandHandler.ExpandTree(node);
      }
      diagram.CommitTransaction("CollapseExpandTree");
      diagram.ZoomToFit();
    }

    // This dynamically creates children for a node.
    // The sample assumes we have no idea whether there are any
    // children for a node until we look for them the first time,
    // which happens upon the first tree-expand of a node.
    private int _CreateSubTree(NodeData parentdata) {
      var MyDiagram = diagramControl1.Diagram;
      var numchildren = _Rand.Next(10);

      if (MyDiagram.Nodes.Count <= 1) {
        numchildren += 1; // make sure the root node has at least one child
      }
      // create several node data objects and add them to the model
      var model = MyDiagram.Model;
      var parent = MyDiagram.FindNodeForData(parentdata);

      var degrees = 1;
      var grandparent = parent.FindTreeParentNode();
      while (grandparent != null) {
        degrees++;
        grandparent = grandparent.FindTreeParentNode();
      }

      for (var i = 0; i < numchildren; i++) {
        var childdata = new NodeData {
          Key = model.NodeDataSource.Count(),
          Parent = parentdata.Key,
          RootDistance = degrees
        };
        // add to model.NodeDataSource and create a node
        model.AddNodeData(childdata);
        // position the new child node close to the parent
        var child = MyDiagram.FindNodeForData(childdata);
        child.Location = parent.Location;
      }
      return numchildren;
    }

    private void ExpandAtRandom() {
      var eligibleNodes = new List<Node>();
      foreach (var n in MyDiagram.Nodes) {
        if (!n.IsTreeExpanded) eligibleNodes.Add(n);
      }
      var node = eligibleNodes[_Rand.Next(eligibleNodes.Count)];
      _ExpandNode(node);
    }

    private void ZoomToFit() {
      MyDiagram.ZoomToFit();
    }

  }

  public class Model : TreeModel<NodeData, int, object> { }

  public class NodeData : Model.NodeData {
    public int RootDistance { get; set; }
    public Brush Color { get; set; }
    public bool EverExpanded { get; set; }
  }

}
