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

namespace WinFormsSampleControls.CustomExpandCollapse {
  [ToolboxItem(false)]
  public partial class CustomExpandCollapseControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    
    public CustomExpandCollapseControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      goWebBrowser1.Html = @"

  <p>
      The ""TreeExpanderButton"", which changes the <a>Node.IsTreeExpanded</a> property, really only works with tree structures.
      When you want to hide/show the ""downstream"" nodes from a given node, using the ""TreeExpanderButton"" might not do what you like,
      especially when there are cycles in the graph structure.
    </p>
    <p>
      Instead, this sample implements a ""Button"" with custom behavior to modify the visibility of each Node.
      If this behavior is still not quite right for your app, you can adapt the behavior implemented in the 
      <code>CollapseFrom</code> and <code>ExpandFrom</code> functions to use different criteria for when to stop recursion.
     </p>
     ";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.Padding = 10;
      myDiagram.Layout = new LayeredDigraphLayout {
        Direction = 90,
        LayeringOption = LayeredDigraphLayering.LongestPathSource
      };
      myDiagram.UndoManager.IsEnabled = true;

      myDiagram.NodeTemplate =
        new Node(PanelLayoutVertical.Instance) {
          PortId = "",
          FromLinkable = true,
          ToLinkable = true
        }.Bind(
          new Binding("Visible")
        ).Add(
          new Panel(PanelLayoutAuto.Instance).Add(
            new Shape {
              Fill = "white",
              MinSize = new Size(30, 30),
              StrokeWidth = 0,
              Cursor = "pointer"
            }.Bind(  // indicate that linking may start here
              new Binding("Fill", "Color")
            ),
            new TextBlock {
              Margin = 2,
              FromLinkable = false,
              ToLinkable = false
            }.Bind(  // don't start drawing a link from the text
              new Binding("Text", "Key")
            )
          ),
          Builder.Make<Panel>("Button").Set(  // a replacement for "TreeExpanderButton" that works for non-tree-structured graphs
            new { Visible = false } // assume initially not visible because there are no links coming out
          ).Bind(
            // bind the button visibility to whether it's not a leaf node
            new Binding("Visible", "IsTreeLeaf",
              (leaf, _) => { return !(leaf as bool? ?? false); })
              .OfElement()
          ).Add(
            new Shape {
              Name = "ButtonIcon",
              Figure = "MinusLine",
              DesiredSize = new Size(6, 6)
            }.Bind(
              new Binding("Figure", "IsCollapsed",  // data.IsCollapsed remembers "collapsed" or "expanded"
                (collapsed, _) => {
                  return (collapsed as bool? ?? false) ? "PlusLine" : "MinusLine";
                }
              )
            )
          ).Set(
            new {
              Click = new Action<InputEvent, GraphObject>((e, obj) => {
                e.Diagram.StartTransaction("toggled visibility of dependencies");
                var node = obj.Part as Node;
                if ((node.Data as NodeData).IsCollapsed ?? false) {
                  ExpandFrom(node, node);
                } else {
                  CollapseFrom(node, node);
                }
                e.Diagram.CommitTransaction("toggled visibility of dependencies");
              })
            }
          )
        );

      void CollapseFrom(Node node, Node start) {
        if ((node.Data as NodeData).IsCollapsed ?? false) return;
        node.Diagram.Model.Set(node.Data, "IsCollapsed", true);
        if (node != start) node.Diagram.Model.Set(node.Data, "Visible", false);
        foreach (var n in node.FindNodesOutOf()) {
          CollapseFrom(n, start);
        }
      }

      void ExpandFrom(Node node, Node start) {
        if (!(node.Data as NodeData).IsCollapsed ?? false) return;
        node.Diagram.Model.Set(node.Data, "IsCollapsed", false);
        if (node != start) node.Diagram.Model.Set(node.Data, "Visible", true);
        foreach (var n in node.FindNodesOutOf()) {
          ExpandFrom(n, start);
        }
      }

      // link template
      myDiagram.LinkTemplate =
        new Link {
          RelinkableFrom = true,
          RelinkableTo = true,
          Corner = 10
        }.Add(
          new Shape(),
          new Shape {
            ToArrow = "Standard"
          }
        );

      // model data
      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "A", Color = "lightgreen" },
          new NodeData { Key = "B1", Color = "yellow" },
          new NodeData { Key = "B2", Color = "yellow" },
          new NodeData { Key = "C", Color = "lightblue" },
          new NodeData { Key = "D1", Color = "orange" },
          new NodeData { Key = "D2", Color = "orange" },
          new NodeData { Key = "E", Color = "pink" },
          new NodeData { Key = "F", Color = "lightgreen" },
          new NodeData { Key = "Z1", Color = "lightgreen" },
          new NodeData { Key = "Z2", Color = "yellow" },
          new NodeData { Key = "Z3", Color = "orange" },
          new NodeData { Key = "Z4", Color = "pink" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = "A", To = "B1" },
          new LinkData { From = "B1", To = "C" },
          new LinkData { From = "A", To = "B2" },
          new LinkData { From = "B2", To = "D2" },
          new LinkData { From = "C", To = "D1" },
          new LinkData { From = "C", To = "D2" },
          new LinkData { From = "D1", To = "E" },
          new LinkData { From = "D2", To = "E" },
          new LinkData { From = "D2", To = "F" },
          new LinkData { From = "Z1", To = "Z2" },
          new LinkData { From = "Z2", To = "Z3" },
          new LinkData { From = "Z3", To = "Z4" },
          new LinkData { From = "Z4", To = "Z1" }
        }
      };
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public bool? IsCollapsed { get; set; }
    public bool? Visible { get; set; }
    public string Color { get; set; }
  }

  public class LinkData : Model.LinkData { }

}
