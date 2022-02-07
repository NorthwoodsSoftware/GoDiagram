using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.WinForms;
using Northwoods.Go.Tools;
using Northwoods.Go.Layouts;
using System.Linq;

namespace WinFormsSampleControls.Navigation {
  [ToolboxItem(false)]
  public partial class NavigationControl : System.Windows.Forms.UserControl {

    private Diagram myDiagram;
    public NavigationControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      unhighlightAllBtn.CheckedChanged += (e, obj) => _UpdateHighlights("unhighlight");
      linksIntoBtn.CheckedChanged += (e, obj) => _UpdateHighlights("linksIn");
      linksOutOfBtn.CheckedChanged += (e, obj) => _UpdateHighlights("linksOut");
      linksConnectedBtn.CheckedChanged += (e, obj) => _UpdateHighlights("linksAll");
      nodesIntoBtn.CheckedChanged += (e, obj) => _UpdateHighlights("nodesIn");
      nodesOutOfBtn.CheckedChanged += (e, obj) => _UpdateHighlights("nodesOut");
      nodesConnectedBtn.CheckedChanged += (e, obj) => _UpdateHighlights("nodesConnect");
      nodesReachableBtn.CheckedChanged += (e, obj) => _UpdateHighlights("nodesReach");
      containingGroupParentBtn.CheckedChanged += (e, obj) => _UpdateHighlights("group");
      containingGroupsAllBtn.CheckedChanged += (e, obj) => _UpdateHighlights("groupsAll");
      memberNodesChildrenBtn.CheckedChanged += (e, obj) => _UpdateHighlights("nodesMember");
      memberNodesAllBtn.CheckedChanged += (e, obj) => _UpdateHighlights("nodesMembersAll");
      memberLinksChildrenBtn.CheckedChanged += (e, obj) => _UpdateHighlights("linksMember");
      memberLinksAllBtn.CheckedChanged += (e, obj) => _UpdateHighlights("linksMembersAll");
      

      goWebBrowser1.Html = @"
           <p>This sample displays relationships between different parts of a diagram.</p>
        <p>
      Select a node or link and one of the radio buttons to highlight parts with a certain relationship to the one selected.
      The highlighting color depends on the ""distance"" between the parts.
        </p>
";
    }

    private void Setup() {

      myDiagram = diagramControl1.Diagram;

      myDiagram.MaxSelectionCount = 1; // no more than one element can be selected at a time

      // define the node template
      myDiagram.NodeTemplate = new Node(PanelLayoutAuto.Instance) {
        // define a tooltip for each node that displays its information
        ToolTip = Builder.Make<Adornment>("ToolTip").Add(
          new TextBlock {
            Margin = 4
          }.Bind(new Binding("Text", "", _GetInfo))
        ),
        LocationSpot = Spot.Center,
        ToEndSegmentLength = 30, FromEndSegmentLength = 30
      }.Bind(
        new Binding("Location", "Loc")
      ).Add(
        new Shape {
          Figure = "Rectangle",
          Name = "OBJSHAPE",
          Fill = "white",
          DesiredSize = new Size(30, 30)
        },
        new TextBlock {
          Margin = 4
        }.Bind(new Binding("Text", "Key"))
      );

      // define a link template
      myDiagram.LinkTemplate = new Link {
        SelectionAdornmentTemplate = new Adornment().Add(
          new Shape {
            IsPanelMain = true,
            Stroke = "dodgerblue",
            StrokeWidth = 3
          },
          new Shape {
            ToArrow = "Standard",
            Fill = "dodgerblue",
            Stroke = null,
            Scale = 1
          }
        ),
        Routing = LinkRouting.Normal,
        Curve = LinkCurve.Bezier,
        ToShortLength = 2,
        ToolTip = Builder.Make<Adornment>("ToolTip").Add(
          // define a tooltip for each link that displays its information
          new TextBlock {
            Margin = 4
          }.Bind(new Binding("Text", "", _GetInfo))
        )
      }.Add(
        new Shape { Name = "OBJSHAPE" }, // the link shape
        new Shape { Name = "ARWSHAPE", ToArrow = "Standard" } // the arrowhead
      );

      // define the group template
      myDiagram.GroupTemplate = new Group(PanelLayoutSpot.Instance) {
        SelectionAdornmentTemplate = new Adornment(PanelLayoutAuto.Instance).Add(
          // adornment when a group is selected
          new Shape {
            Figure = "Rectangle",
            Fill = null,
            Stroke = "dodgerblue",
            StrokeWidth = 3
          },
          new Placeholder()
        ),
        ToSpot = Spot.AllSides, // links coming into groups at any side
        ToEndSegmentLength = 30, FromEndSegmentLength = 30,
        ToolTip = Builder.Make<Adornment>("ToolTip").Add(
          // define a tooltip for each group that displays its information
          new TextBlock {
            Margin = 4
          }.Bind(new Binding("Text", "", _GetInfo))
        )
      }.Add(
        new Panel(PanelLayoutAuto.Instance).Add(
          new Shape {
            Figure = "Rectangle",
            Name = "OBJSHAPE",
            Parameter1 = 14,
            Fill = "rgba(255,0,0,0.10)"
          }.Bind(new Binding("DesiredSize", "DS")),
          new Placeholder { Padding = 16 }
        ),
        new TextBlock {
          Name = "GROUPTEXT",
          Alignment = Spot.TopLeft,
          AlignmentFocus = new Spot(0, 0, -4, -4),
          Font = new Font("Segoe UI", 13, FontWeight.Bold)
        }.Bind(new Binding("Text", "Key"))
      );

      // add nodes, including groups, and links to the model
      myDiagram.Model = new Model {
        // node data
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "A", Loc = new Point(320, 100) },
          new NodeData { Key = "B", Loc = new Point(420, 200) },
          new NodeData { Key = "C", Group = "Psi", Loc = new Point(250, 225) },
          new NodeData { Key = "D", Group = "Omega", Loc = new Point(270, 325) },
          new NodeData { Key = "E", Group = "Phi", Loc = new Point(120, 225) },
          new NodeData { Key = "F", Group = "Omega", Loc = new Point(200, 350) },
          new NodeData { Key = "G", Loc = new Point(180, 450) },
          new NodeData { Key = "Chi", IsGroup = true },
          new NodeData { Key = "Psi", IsGroup = true, Group = "Chi" },
          new NodeData { Key = "Phi", IsGroup = true, Group = "Psi" },
          new NodeData { Key = "Omega", IsGroup = true, Group = "Psi" }
        },
        // link data
        LinkDataSource = new List<LinkData> {
          new LinkData { From = "A", To = "B" },
          new LinkData { From = "A", To = "C" },
          new LinkData { From = "A", To = "C" },
          new LinkData { From = "B", To = "B" },
          new LinkData { From = "B", To = "C" },
          new LinkData { From = "B", To = "Omega" },
          new LinkData { From = "C", To = "A" },
          new LinkData { From = "C", To = "Psi" },
          new LinkData { From = "C", To = "D" },
          new LinkData { From = "D", To = "F" },
          new LinkData { From = "E", To = "F" },
          new LinkData { From = "F", To = "G" }
        }
      };

      myDiagram.ChangedSelection += myDiagram_ChangedSelection;
      myDiagram.Select(myDiagram.FindNodeForKey("C"));
    }

    private void myDiagram_ChangedSelection(object sender, DiagramEvent e) {
      _UpdateHighlights("");
    }

    public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }

    public class NodeData : Model.NodeData {
      public Point Loc { get; set; }
      public int Highlight { get; set; }
    }

    public class LinkData : Model.LinkData {
      public int Highlight { get; set; }
    }

    // If selected object is a link, highlight its fromNode.
    // Otherwise highlight the fromNode of each link coming into the selected node.
    // Return a List of the keys of the nodes.
    private List<string> _NodesTo(GraphObject x, int i) {
      var nodesToList = new List<string>();
      if (x is Link link) {
        (link.FromNode.Data as NodeData).Highlight = i;
        nodesToList.Add((link.Data as LinkData).From);
      } else {
        foreach (var node in (x as Node).FindLinksInto()) {
          (node.FromNode.Data as NodeData).Highlight = i;
          nodesToList.Add((node.FromNode.Data as NodeData).Key);
        }
      }
      return nodesToList;
    }

    // same as NodesTo, but 'from' instead of 'to'
    private List<string> _NodesFrom(GraphObject x, int i) {
      var nodesToList = new List<string>();
      if (x is Link link) {
        (link.ToNode.Data as NodeData).Highlight = i;
        nodesToList.Add((link.Data as LinkData).To);
      } else {
        foreach (var node in (x as Node).FindLinksOutOf()) {
          (node.ToNode.Data as NodeData).Highlight = i;
          nodesToList.Add((node.ToNode.Data as NodeData).Key);
        }
      }
      return nodesToList;
    }

    // highlight all nodes linked to this one
    private void _NodesConnect(GraphObject x, int i) {
      if (x is Link link) {
        (link.ToNode.Data as NodeData).Highlight = i;
        (link.FromNode.Data as NodeData).Highlight = i;
      } else {
        foreach (var node in (x as Node).FindNodesConnected()) (node.Data as NodeData).Highlight = i;
      }
    }

    // If x is a link, highlight its toNode, or if it is a node, the node(s) it links to,
    // and then call nodesReach on the highlighted node(s), with the next color.
    // Do not highlight any node that has already been highlit with a color
    // indicating a closer relationship to the original node.
    private void _NodesReach(GraphObject x, int i) {
      if (x is Link link) {
        (link.Data as LinkData).Highlight = i;
        _NodesReach(link.ToNode, i + 1);
      } else {
        foreach (var node in (x as Node).FindNodesOutOf()) {
          if ((node.Data as NodeData).Highlight == 0 || (node.Data as NodeData).Highlight > i) {
            (node.Data as NodeData).Highlight = i;
            _NodesReach(node, i + 1);
          }
        }
      }
    }

    // highlights the group containing this object, specific method for links
    // returns the containing group of x
    private Group _Containing(GraphObject x, int i) {
      if (x is Link link) { return null; }
      var container = (x as Node).ContainingGroup;
      if (container != null) (container.Data as NodeData).Highlight = i;
      return container;
    }

    // container is the group that contains this node and
    // will be the parameter x for the next call of this function.
    // Calling containing(x,i) highlights each group the appropriate color
    private void _ContainingAll(GraphObject x, int i) {
      _Containing(x, i);
      if (x is Link link) { return; }
      var container = (x as Node).ContainingGroup;
      if (container != null) _ContainingAll(container, i + 1);
    }

    // if the Node's containingGroup is x, highlight it
    private List<string> _ChildNodes(GraphObject x, int i) {
      var childLst = new List<string>();
      if (x is Group group) {
        foreach (var node in myDiagram.Nodes) {
          if (node.ContainingGroup == x) {
            (node.Data as NodeData).Highlight = i;
            childLst.Add((node.Data as NodeData).Key);
          }
        }
      }
      return childLst;
    }

    // same as childNodes, then run allMemberNodes for each child Group with the next color
    private void _AllMemberNodes(GraphObject x, int i) {
      if (x is Group group) {
        foreach (var node in myDiagram.Nodes) {
          if (node.ContainingGroup == x) {
            (node.Data as NodeData).Highlight = i;
            _AllMemberNodes(node, i + 1);
          }
        }
      }
    }

    // if the link's containing Group is x, highlight it
    private List<Link> _ChildLinks(GraphObject x, int i) {
      var childLst = new List<Link>();
      foreach (var link in myDiagram.Links) {
        if (link.ContainingGroup == x) {
          (link.Data as LinkData).Highlight = i;
          childLst.Add(link);
        }
      }
      return childLst;
    }

    // same as childLinks, then run allMemberLinks for each child Group with the next color
    private void _AllMemberLinks(GraphObject x, int i) {
      _ChildLinks(x, i);
      foreach (var node in myDiagram.Nodes) {
        if (node is Group group && group.ContainingGroup == x) {
          _AllMemberLinks(node, i + 1);
        }
      }
    }

    // if the link connects to this node, highlight it
    private void _LinksTo(GraphObject x, int i) {
      if (x is Node node) {
        foreach (var link in node.FindLinksInto()) (link.Data as LinkData).Highlight = i;
      }
    }

    // if the link comes from this node, highlight it
    private void _LinksFrom(GraphObject x, int i) {
      if (x is Node node) {
        foreach (var link in node.FindLinksOutOf()) (link.Data as LinkData).Highlight = i;
      }
    }

    // highlight all links connected to this node
    private void _LinksAll(GraphObject x, int i) {
      if (x is Node node) {
        foreach (var link in node.LinksConnected) (link.Data as LinkData).Highlight = i;
      }
    }

    // This highlights all graph objects that should be highlighted
    // whenever a radio button is checked or selection changes.
    // Parameter e is the checked radio button.
    private void _UpdateHighlights(string e) {
      // Set highlight to 0 for everything before updating
      foreach (var node in myDiagram.Nodes) (node.Data as NodeData).Highlight = 0;
      foreach (var link in myDiagram.Links) (link.Data as LinkData).Highlight = 0;

      // Get the selected GraphObject and run the appropriate method
      if (myDiagram.Selection.Count > 0) {
        var sel = myDiagram.Selection.First();

        switch (e) {
          case "linksIn": _LinksTo(sel, 1); break;
          case "linksOut": _LinksFrom(sel, 1); break;
          case "linksAll": _LinksAll(sel, 1); break;
          case "nodesIn": _NodesTo(sel, 1); break;
          case "nodesOut": _NodesFrom(sel, 1); break;
          case "nodesConnect": _NodesConnect(sel, 1); break;
          case "nodesReach": _NodesReach(sel, 1); break;
          case "group": _Containing(sel, 1); break;
          case "groupsAll": _ContainingAll(sel, 1); break;
          case "nodesMember": _ChildNodes(sel, 1); break;
          case "nodesMembersAll": _AllMemberNodes(sel, 1); break;
          case "linksMember": _ChildLinks(sel, 1); break;
          case "linksMembersAll": _AllMemberLinks(sel, 1); break;
        }
      }

      // Give everything the appropriate highlighting ( color and width of stroke )
      // nodes, including groups
      foreach (var node in myDiagram.Nodes) {
        var shp = node.FindElement("OBJSHAPE");
        var grp = node.FindElement("GROUPTEXT");
        var hl = (node.Data as NodeData).Highlight;
        _Highlight(shp, grp, hl);
      }
      // links
      foreach (var link in myDiagram.Links) {
        var hl = (link.Data as LinkData).Highlight;
        var shp = link.FindElement("OBJSHAPE");
        var arw = link.FindElement("ARWSHAPE");
        _Highlight(shp, arw, hl);
      }
    }

    private void _Highlight(GraphObject shp, GraphObject obj2, int hl) {
      string color;
      var width = 3.0;
      if (hl == 0) { color = "black"; width = 1; } else if (hl == 1) { color = "blue"; } else if (hl == 2) { color = "green"; } else if (hl == 3) { color = "orange"; } else if (hl == 4) { color = "red"; } else { color = "purple"; }
      (shp as Shape).Stroke = color;
      (shp as Shape).StrokeWidth = width;
      if (obj2 != null) {
        if (obj2 is TextBlock block) {
          // TextBlock property setters
          block.Stroke = color;
        }
        if (obj2 is Shape shape) {
          // Shape property setters
          shape.Stroke = color;
          shape.Fill = color;
        }
      }
    }

    private string _GetInfo(object model, object obj) {
      var x = ((obj as GraphObject).Panel as Adornment).AdornedPart;
      var text = ""; // what will be displayed
      if (x is Node node) {
        if (node is Group) text += "Group: "; else text += "Node: ";
        text += (x.Data as NodeData).Key;
        var toLst = _NodesTo(x, 0); // display names of nodes going into this node
        if (toLst.Count > 0) {
          toLst.Sort();
          text += "\nNodes into: ";
          foreach (var key in toLst) {
            if (key != text.Substring(text.Length - 3, text.Length - 2)) {
              text += key + ", ";
            }
          }
          text = text.Substring(0, text.Length - 2);
        }
        var frLst = _NodesFrom(x, 0); // display names of nodes coming out of this node
        if (frLst.Count > 0) {
          frLst.Sort();
          text += "\nNodes out of: ";
          foreach (var key in frLst) {
            if (key != text.Substring(text.Length - 3, text.Length - 2)) {
              text += key + ", ";
            }
          }
          text = text.Substring(0, text.Length - 2);
        }
        var grpC = _Containing(x as Node, 0); // if the node is in a group, display its name
        if (grpC != null) text += "\nContaining SubGraph: " + (grpC.Data as NodeData).Key;
        if (x is Group group) {
          // if it's a group, also display nodes and links contained in it
          text += "\nMember nodes: ";
          var children = _ChildNodes(x as Node, 0);
          children.Sort();
          foreach (var key in children) {
            if (key != text.Substring(text.Length - 3, text.Length - 2)) {
              text += key + ", ";
            }
          }
          text = text.Substring(0, text.Length - 2);

          var linkChildren = _ChildLinks(x, 0);
          if (linkChildren.Count > 0) {
            text += "\nMember links: ";
            var linkStrings = new List<string>();
            foreach (var link in linkChildren) {
              linkStrings.Add((link.Data as LinkData).From + " --> " + (link.Data as LinkData).To);
            }
            linkStrings.Sort();
            foreach (var str in linkStrings) {
              text += str + ", ";
            }
            text = text.Substring(0, text.Length - 2);
          }
        }
      } else if (x is Link link) {
        var data = link.Data as LinkData;
        // if it's a link, display its to and from nodes
        text += "Link: " + data.From + " --> " + data.To +
          "\nNode to: " + data.To + "\nNode from: " + data.From;
        var grp = _Containing(x as Node, 0); // and containing group, if it has one
        if (grp != null) text += "\nContaining SubGraph: " + data.Key;
      }
      return text;
    }




  }
}
