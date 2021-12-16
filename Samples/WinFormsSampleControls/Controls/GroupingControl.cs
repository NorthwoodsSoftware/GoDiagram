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

namespace WinFormsSampleControls.Grouping {
  [ToolboxItem(false)]
  public partial class GroupingControl : System.Windows.Forms.UserControl {
    private Diagram MyDiagram;

    public GroupingControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      goWebBrowser1.Html = @"

 <p>
  This sample demonstrates subgraphs that are created only as groups are expanded.
  </p>
  <p>
  The model is initially a random number of nodes, including some groups, in a tree layout.
  When a group is expanded, the <a>Group.SubGraphExpandedChanged</a> event handler calls a function to generate a random number of nodes
  in a tree layout inside the group if it did not contain none any.
  Each non-group node added has a unique random color, and links are added by giving each node one link to another node.
  </p>
  <p>
  The addition of nodes and links is performed within a transaction to ensure that the diagram updates itself properly.
  The diagram's tree layout and the tree layouts within each group are performed again when a sub-graph is expanded or collapsed.
  </p>
";

    }

    private void Setup() {

      MyDiagram = diagramControl1.Diagram;

      MyDiagram.Layout = new TreeLayout { // the layout for the entire diagram
        Angle = 90,
        Arrangement = TreeArrangement.Horizontal,
        IsRealtime = false
      };
      MyDiagram.Model = new Model();

      // define the node template for non-groups
      MyDiagram.NodeTemplate = new Node(PanelLayoutAuto.Instance).Add(
        new Shape {
          Figure = "Rectangle",
          Stroke = null, StrokeWidth = 0
        }.Bind("Fill", "Key"),
        new TextBlock {
          Margin = 7, Font = "Segoe UI, 14px, style=bold"
        }.Bind("Text", "Key") // text, color, and key are all bound to the same property in the node data
      );

      MyDiagram.LinkTemplate = new Link {
        Routing = LinkRouting.Orthogonal,
        Corner = 10
      }.Add(
        new Shape { StrokeWidth = 2 },
        new Shape { ToArrow = "OpenTriangle" }
      );

      // define the group template
      MyDiagram.GroupTemplate = new Group(PanelLayoutAuto.Instance) {
        // define the group's internal layout
        Layout = new TreeLayout {
          Angle = 90,
          Arrangement = TreeArrangement.Horizontal,
          IsRealtime = false
        },
        // the group begins unexpanded;
        // upon expansion, a Diagram Listener will generate contents for the group
        IsSubGraphExpanded = false,
        // when a group is expanded, if it contains no parts, generate a SubGraph inside of it
        SubGraphExpandedChanged = (group) => {
          if (group.MemberParts.Count == 0) {
            _RandomGroup((group.Data as NodeData).Key);
          }
        }
      }.Add(
        new Shape {
          Figure = "Rectangle",
          Fill = null,
          Stroke = "gray",
          StrokeWidth = 2
        },
        new Panel(PanelLayoutVertical.Instance) {
          DefaultAlignment = Spot.Left,
          Margin = 4
        }.Add(
          new Panel(PanelLayoutHorizontal.Instance) {
            DefaultAlignment = Spot.Top
          }.Add(
            // the SubGraphExpanderButton is a panel that functions as a button to expand or collapse the sub-graph
            Builder.Make<Panel>("SubGraphExpanderButton"),
            new TextBlock {
              Font = "Segoe UI, 18px, style=bold",
              Margin = 4
            }.Bind("Text", "Key")
          ),
          // create a placeholder to represent the area where the contents of the group are
          new Placeholder {
            Padding = new Margin(0, 10)
          }
        )
      );

      // generate the initial model
      _RandomGroup();
    }

    // Generate a random number of nodes, including groups.
    // If a group's key is given as a parameter, put these nodes inside it
    private void _RandomGroup(string group = null) {
      var rand = new Random();

      // all modification to the diagram is within this transaction
      MyDiagram = diagramControl1.Diagram;
      MyDiagram.StartTransaction("AddGroupContents");
      var addedKeys = new List<string>(); // this will contain the keys of all nodes created
      var groupCount = 0; // the number of groups in the diagram, to determine the numbers in the keys of new groups
      foreach (var node in MyDiagram.Nodes) {
        if (node is Group) groupCount++;
      }
      // create a random number of groups
      // ensure there are at least 10 groups in the diagram
      var groups = rand.Next(2);
      if (groupCount < 10) groups++;
      for (var i = 0; i < groups; i++) {
        var name = "group" + (i + groupCount);
        if (group == null) {
          MyDiagram.Model.AddNodeData(new NodeData {
            Key = name,
            IsGroup = true
          });
        } else {
          MyDiagram.Model.AddNodeData(new NodeData {
            Key = name,
            IsGroup = true,
            Group = group
          });
        }
        addedKeys.Add(name);
      }
      var nodes = rand.Next(2, 3);
      // create a random number of non-group nodes
      for (var i = 0; i < nodes; i++) {
        var color = Brush.RandomColor();
        // make sure the color, which will be the node's key, is unique in the diagram before adding the new node
        if (MyDiagram.FindPartForKey(color) == null) {
          MyDiagram.Model.AddNodeData(new NodeData {
            Key = color, Group = group
          });
          addedKeys.Add(color);
        }
      }
      // add at least one link from each node to another
      // this could result in clusters of nodes unreachable from each other, but no lone nodes
      var arr = new List<string>();
      foreach (var x in addedKeys) arr.Add(x);
      arr.Sort(Comparer<string>.Create(
        (x, y) => (rand.NextDouble() * 2 > 1) ? 1 : -1
      ));
      for (var i = 0; i < arr.Count; i++) {
        var from = rand.Next(i, arr.Count);
        if (from != i) {
          (MyDiagram.Model as Model).AddLinkData(new LinkData {
            From = arr[from], To = arr[i]
          });
        }
      }
      MyDiagram.CommitTransaction("AddGroupContents");
    }

  }

  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData { }
  public class LinkData : Model.LinkData { }

}
