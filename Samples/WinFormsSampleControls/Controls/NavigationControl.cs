/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using System.Linq;

namespace WinFormsSampleControls.Navigation {
  [ToolboxItem(false)]
  public partial class NavigationControl : System.Windows.Forms.UserControl {

    private Diagram myDiagram;
    public NavigationControl() {
      InitializeComponent();
      myDiagram = diagramControl1.Diagram;

      Setup();

      goWebBrowser1.Html = @"
           <p>This sample displays relationships between different parts of a diagram.</p>
        <p>
      Select a node or link and one of the radio buttons to highlight parts with a certain relationship to the one selected.
      The highlighting color depends on the ""distance"" between the parts.
        </p>
";
    }

    private void Setup() {
      myDiagram.MaxSelectionCount = 1; // no more than one element can be selected at a time

      // define the node template
      myDiagram.NodeTemplate =
        new Node("Auto") {
            LocationSpot = Spot.Center,
            ToEndSegmentLength = 30, FromEndSegmentLength = 30,
            ToolTip =  // define a tooltip for each node that displays its information
              Builder.Make<Adornment>("ToolTip")
                .Add(
                  new TextBlock { Margin = 4 }
                    .Bind("Text", "", _GetInfo)
                )
          }
          .Bind(new Binding("Location").MakeTwoWay())
          .Add(
            new Shape {
                Name = "OBJSHAPE",
                Fill = "white",
                DesiredSize = new Size(30, 30)
              },
            new TextBlock { Margin = 4 }
              .Bind("Text", "Key")
          );

      // define a link template
      myDiagram.LinkTemplate =
        new Link {
            SelectionAdornmentTemplate =
              new Adornment()
                .Add(
                  new Shape { IsPanelMain = true, Stroke = "dodgerblue", StrokeWidth = 3 },
                  new Shape { ToArrow = "Standard", Fill = "dodgerblue", Stroke = null, Scale = 1 }
                ),
            Routing = LinkRouting.Normal,
            Curve = LinkCurve.Bezier,
            ToShortLength = 2,
            ToolTip =  // define a tooltip for each link that displays its information
              Builder.Make<Adornment>("ToolTip")
                .Add(
                  new TextBlock { Margin = 4 }
                    .Bind("Text", "", _GetInfo)
                )
          }
          .Add(
            new Shape { Name = "OBJSHAPE" }, // the link shape
            new Shape { Name = "ARWSHAPE", ToArrow = "Standard" } // the arrowhead
          );

      // define the group template
      myDiagram.GroupTemplate =
        new Group("Spot") {
            SelectionAdornmentTemplate =  // adornment when a group is selected
              new Adornment("Auto")
                .Add(
                  new Shape { Fill = null, Stroke = "dodgerblue", StrokeWidth = 3 },
                  new Placeholder()
                ),
            ToSpot = Spot.AllSides, // links coming into groups at any side
            ToEndSegmentLength = 30, FromEndSegmentLength = 30,
            ToolTip =  // define a tooltip for each group that displays its information
              Builder.Make<Adornment>("ToolTip")
                .Add(
                  new TextBlock { Margin = 4 }
                    .Bind("Text", "", _GetInfo)
                )
          }
          .Add(
            new Panel("Auto")
              .Add(
                new Shape {
                    Name = "OBJSHAPE",
                    Parameter1 = 14,
                    Fill = "rgba(255,0,0,0.10)"
                  },
                new Placeholder { Padding = 16 }
              ),
            new TextBlock {
                Name = "GROUPTEXT",
                Alignment = Spot.TopLeft,
                AlignmentFocus = new Spot(0, 0, -4, -4),
                Font = new Font("Segoe UI", 13, FontWeight.Bold)
              }
              .Bind("Text", "Key")
          );

      // add nodes, including groups, and links to the model
      myDiagram.Model = new Model {
        // node data
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "A", Location = new Point(320, 100) },
          new NodeData { Key = "B", Location = new Point(420, 270) },
          new NodeData { Key = "C", Group = "Psi", Location = new Point(250, 215) },
          new NodeData { Key = "D", Group = "Omega", Location = new Point(270, 325) },
          new NodeData { Key = "E", Group = "Phi", Location = new Point(120, 225) },
          new NodeData { Key = "F", Group = "Omega", Location = new Point(200, 350) },
          new NodeData { Key = "G", Location = new Point(180, 450) },
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

      myDiagram.ChangedSelection += (s, e) => _UpdateHighlights(_GetRadioButton());
      myDiagram.Select(myDiagram.FindNodeForKey("A"));
    }

    // This highlights all graph objects that should be highlighted
    // whenever a radio button is checked or selection changes.
    // Parameter e is the checked radio button.
    private void _UpdateHighlights(string e) {
      // Set highlight to 0 for everything before updating
      foreach (var node in myDiagram.Nodes) node["_Highlight"] = 0;
      foreach (var link in myDiagram.Links) link["_Highlight"] = 0;

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
          case "unhighlightAll":
          default: break;
        }
      }

      // Give everything the appropriate highlighting ( color and width of stroke )
      // nodes, including groups
      foreach (var node in myDiagram.Nodes) {
        var shp = node.FindElement("OBJSHAPE");
        var grp = node.FindElement("GROUPTEXT");
        var hl = (int)node["_Highlight"];
        _Highlight(shp, grp, hl);
      }
      // links
      foreach (var link in myDiagram.Links) {
        var hl = (int)link["_Highlight"];
        var shp = link.FindElement("OBJSHAPE");
        var arw = link.FindElement("ARWSHAPE");
        _Highlight(shp, arw, hl);
      }
    }

    // Functions for highlighting, called by _UpdateHighlights.
    // x in each case is the selected object or the object being treated as such.
    // Some have return values for use by each other or for tooltips.

    // if the link connects to this node, highlight it
    private void _LinksTo(GraphObject x, int i) {
      if (x is Node node) {
        foreach (var link in node.FindLinksInto()) link["_Highlight"] = i;
      }
    }

    // if the link comes from this node, highlight it
    private void _LinksFrom(GraphObject x, int i) {
      if (x is Node node) {
        foreach (var link in node.FindLinksOutOf()) link["_Highlight"] = i;
      }
    }

    // highlight all links connected to this node
    private void _LinksAll(GraphObject x, int i) {
      if (x is Node node) {
        foreach (var link in node.LinksConnected) link["_Highlight"] = i;
      }
    }

    // If selected object is a link, highlight its FromNode.
    // Otherwise highlight the FromNode of each link coming into the selected node.
    // Return a List of the keys of the nodes.
    private List<string> _NodesTo(GraphObject x, int i) {
      var nodesToList = new List<string>();
      if (x is Link l) {
        l.FromNode["_Highlight"] = i;
        nodesToList.Add((l.Data as LinkData).From);
      } else if (x is Node n){
        foreach (var node in n.FindNodesInto()) {
          node["_Highlight"] = i;
          nodesToList.Add((node.Data as NodeData).Key);
        }
      }
      return nodesToList;
    }

    // same as _NodesTo, but 'from' instead of 'to'
    private List<string> _NodesFrom(GraphObject x, int i) {
      var nodesFromList = new List<string>();
      if (x is Link l) {
        l.ToNode["_Highlight"] = i;
        nodesFromList.Add((l.Data as LinkData).To);
      } else if (x is Node n) {
        foreach (var node in n.FindNodesOutOf()) {
          node["_Highlight"] = i;
          nodesFromList.Add((node.Data as NodeData).Key);
        }
      }
      return nodesFromList;
    }

    // If x is a link, highlight its ToNode, or if it is a node, the node(s) it links to,
    // and then call _NodesReach on the highlighted node(s), with the next color.
    // Do not highlight any node that has already been highlit with a color
    // indicating a closer relationship to the original node.
    private void _NodesReach(GraphObject x, int i) {
      if (x is Link l) {
        l.ToNode["_Highlight"] = i;
        _NodesReach(l.ToNode, i + 1);
      } else if (x is Node n) {
        foreach (var node in n.FindNodesOutOf()) {
          var hl = (int)node["_Highlight"];
          if (hl == 0 || hl > i) {
            node["_Highlight"] = i;
            _NodesReach(node, i + 1);
          }
        }
      }
    }

    // highlight all nodes linked to this one
    private void _NodesConnect(GraphObject x, int i) {
      if (x is Link l) {
        l.ToNode["_Highlight"] = i;
        l.FromNode["_Highlight"] = i;
      } else if (x is Node n) {
        foreach (var node in n.FindNodesConnected()) node["_Highlight"] = i;
      }
    }

    // highlights the group containing this object, specific method for links
    // returns the containing group of x
    private Group _Containing(GraphObject x, int i) {
      var container = (x as Part).ContainingGroup;
      if (container != null) container["_Highlight"] = i;
      return container;
    }

    // container is the group that contains this node and
    // will be the parameter x for the next call of this function.
    // Calling _Containing(x,i) highlights each group the appropriate color
    private void _ContainingAll(GraphObject x, int i) {
      _Containing(x, i);
      var container = (x as Part).ContainingGroup;
      if (container != null) _ContainingAll(container, i + 1);
    }

    // if the Node's ContainingGroup is x, highlight it
    private List<string> _ChildNodes(GraphObject x, int i) {
      var childLst = new List<string>();
      if (x is Group g) {
        foreach (var node in myDiagram.Nodes) {
          if (node.ContainingGroup == g) {
            node["_Highlight"] = i;
            childLst.Add((node.Data as NodeData).Key);
          }
        }
      }
      return childLst;
    }

    // same as _ChildNodes, then run _AllMemberNodes for each child Group with the next color
    private void _AllMemberNodes(GraphObject x, int i) {
      if (x is Group g) {
        foreach (var node in myDiagram.Nodes) {
          if (node.ContainingGroup == g) {
            node["_Highlight"] = i;
            _AllMemberNodes(node, i + 1);
          }
        }
      }
    }

    // if the link's containing Group is x, highlight it
    private List<Link> _ChildLinks(GraphObject x, int i) {
      var childLst = new List<Link>();
      if (x is Group g) {
        foreach (var link in myDiagram.Links) {
          if (link.ContainingGroup == g) {
            link["_Highlight"] = i;
            childLst.Add(link);
          }
        }
      }
      return childLst;
    }

    // same as childLinks, then run allMemberLinks for each child Group with the next color
    private void _AllMemberLinks(GraphObject x, int i) {
      if (x is Group g) {
        _ChildLinks(x, i);
        foreach (var node in myDiagram.Nodes) {
          if (node is Group group && group.ContainingGroup == g) {
            _AllMemberLinks(node, i + 1);
          }
        }
      }
    }

    // perform the actual highlighting
    private void _Highlight(GraphObject shp, GraphObject obj2, int hl) {
      string color;
      var width = 3.0;
      if (hl == 0) { color = "black"; width = 1; }
      else if (hl == 1) { color = "blue"; }
      else if (hl == 2) { color = "green"; }
      else if (hl == 3) { color = "orange"; }
      else if (hl == 4) { color = "red"; }
      else { color = "purple"; }
      (shp as Shape).Stroke = color;
      (shp as Shape).StrokeWidth = width;
      if (obj2 != null) {
        if (obj2 is TextBlock tb) {
          tb.Stroke = color;
        } else if (obj2 is Shape shape) {
          shape.Stroke = color;
          shape.Fill = color;
        }
      }
    }

    // returns the text for a tooltip, param obj is the text itself
    private string _GetInfo(object model, object obj) {
      var x = ((obj as GraphObject).Panel as Adornment).AdornedPart;  // the object that the mouse is over
      var text = ""; // what will be displayed
      if (x is Node node) {
        if (node is Group) text += "Group: "; else text += "Node: ";
        text += (x.Data as NodeData).Key;
        var toLst = _NodesTo(x, 0); // display names of nodes going into this node
        if (toLst.Count > 0) {
          toLst.Sort();
          text += "\nNodes into: ";
          foreach (var key in toLst) {
            if (key != text.Substring(text.Length - 3, 1)) {
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
            if (key != text.Substring(text.Length - 3, 1)) {
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
            if (key != text.Substring(text.Length - 3, 1)) {
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
        var grp = _Containing(x, 0);  // and containing group, if it has one
        if (grp != null) text += "\nContaining SubGraph: " + (grp.Data as NodeData).Key;
      }
      return text;
    }

    private void _RadioChanged(object sender, EventArgs e) {
      if (sender is System.Windows.Forms.RadioButton rb && rb.Checked) {
        _UpdateHighlights(rb.Name);
      }
    }

    private string _GetRadioButton() {
      var radiogroup = tableLayoutPanel3;
      foreach (System.Windows.Forms.Control ctrl in radiogroup.Controls) {
        if (ctrl is System.Windows.Forms.RadioButton rb && rb.Checked) return rb.Name;
      }
      return null;
    }

    public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
    public class NodeData : Model.NodeData { }
    public class LinkData : Model.LinkData { }
  }
}
