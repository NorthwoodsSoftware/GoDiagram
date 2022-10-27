/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace Demo.Samples.DraggablePorts {
  [ToolboxItem(false)]
  public partial class DraggablePortsControl : DemoControl {
    private Diagram myDiagram;

    public DraggablePortsControl() {
      InitializeComponent();

      Setup();

      modelJson1.SaveClick = _Save;
      modelJson1.LoadClick = LoadModel;

      goWebBrowser1.Html = @"

  <p>
    To allow ports to be selected, dragged, copied, and deleted, they are implemented as Nodes.
    That means the nodes have to be implemented as Groups.
    The user can delete selected ports.
    The user cannot drop a port onto the background, but only onto a node.
  </p>
  <p>
    There is a custom Layout used by such Group nodes, <code>InputOutputGroupLayout</code>,
    to line up the input ports on the left side and the output ports on the right side.
  </p>

";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.UndoManager.IsEnabled = true;
      // don't allow links within a group
      myDiagram.ToolManager.LinkingTool.LinkValidation = DifferentGroups;
      myDiagram.ToolManager.RelinkingTool.LinkValidation = DifferentGroups;
      myDiagram.MouseDrop = (e) => {
        // when the selection is dropped in the diagram's background,
        // and it includes any "port"s, cancel the drop
        if (myDiagram.Selection.Any(SelectionIncludesPorts)) {
          myDiagram.ToolManager.DraggingTool.DoCancel();
        }
      };

      bool DifferentGroups(Node fromnode, GraphObject fromport, Node tonode, GraphObject toport, Link _) {
        return fromnode.ContainingGroup != tonode.ContainingGroup;
      }

      bool SelectionIncludesPorts(Part n) {
        return n.ContainingGroup != null && !myDiagram.Selection.Contains(n.ContainingGroup);
      }

      var UnselectedBrush = "lightgray";  // item appearance, if not "selected"
      var SelectedBrush = "dodgerblue";   // item appearance, if "selected"

      // node template
      myDiagram.NodeTemplate =
        new Node(PanelType.Auto) {
          SelectionAdorned = false,
          MouseDrop = (e, nIn) => {
            var n = nIn as Node;
            // when the selection is entirely ports and is dropped onto a Group, transfer membership
            if (n.ContainingGroup != null && myDiagram.Selection.All(SelectionIncludesPorts)) {
              foreach (var p in myDiagram.Selection) {
                p.ContainingGroup = n.ContainingGroup;
              }
            } else {
              myDiagram.CurrentTool.DoCancel();
            }
          }
        }.Add(
          new Shape {
            Name = "SHAPE",
            Fill = UnselectedBrush,
            Stroke = "gray",
            GeometryString = "F1 m 0,0 l 5,0 1,4 -1,4 -5,0 1,-4 -1,-4 z",
            Spot1 = new Spot(0, 0, 5, 1),  // keep the text inside the shape
            Spot2 = new Spot(1, 1, -5, 0),
            // some port-related properties
            PortId = "",
            ToSpot = Spot.Left,
            FromSpot = Spot.Right,
            ToLinkable = false,
            FromLinkable = false,
            Cursor = "pointer"
          }.Bind(
            new Binding("Fill", "IsSelected", (s, _) => { return (s as bool? ?? false) ? SelectedBrush : UnselectedBrush; }).OfElement(),
            new Binding("ToLinkable", "PIn"),
            new Binding("FromLinkable", "PIn", (to, _) => {
              return !(to as bool? ?? false);
            })
          ),
          new TextBlock().Bind(
            new Binding("Text", "Name")
          )
        );

      // group template
      myDiagram.GroupTemplate =
        new Group(PanelType.Auto) {
          SelectionAdorned = false,
          LocationSpot = Spot.Center,
          LocationElementName = "ICON",
          Layout = new InputOutputGroupLayout()
        }.Bind(
          new Binding("Location", "Loc", Point.Parse).MakeTwoWay(Point.Stringify)
        ).Set(
          new {
            MouseDrop = new Action<InputEvent, GraphObject>((e, g) => {
              // when the selection is entirely ports and is dropped onto a Group, transfer membership
              if (myDiagram.Selection.All(SelectionIncludesPorts)) {
                foreach (var p in myDiagram.Selection) {
                  p.ContainingGroup = g as Group;
                }
              } else {
                myDiagram.CurrentTool.DoCancel();
              }
            })
          }
        ).Add(
          new Shape {
            Figure = "RoundedRectangle",
            Stroke = "gray",
            StrokeWidth = 2,
            Fill = "transparent"
          }.Bind(
            new Binding("Stroke", "IsSelected", (b, _) => { return (b as bool? ?? false) ? SelectedBrush : UnselectedBrush; }).OfElement()
          ),
          new Panel(PanelType.Vertical) {
            Margin = 6
          }.Add(
            new TextBlock().Bind(
              new Binding("Text", "Name")
            ).Set(
              new {
                Alignment = Spot.Left
              }
            ),
            new Panel(PanelType.Spot) {
              Name = "ICON",
              Height = 60
            }.Add(  // an initial height; size will be set by InputOutputGroupLayout
              new Shape {
                Fill = null,
                Stroke = null,
                Stretch = Stretch.Fill
              },
              new Picture {
                Source = "https://nwoods.com/go/images/samples/60x90.png",
                Width = 60,
                Height = 90,
                Scale = 0.5
              }
            )
          )
        );

      // link template
      myDiagram.LinkTemplate =
        new Link {
          Routing = LinkRouting.Orthogonal,
          Corner = 10,
          ToShortLength = -3,
          RelinkableFrom = true,
          RelinkableTo = true
        }.Add(
          new Shape {
            Stroke = "gray",
            StrokeWidth = 2.5
          }
        );

      // load model data
      var nodeDataSource = new List<NodeData> {
        new NodeData {
          Key = "1",
          Name = "Server",
          Ins = new List<NodeData> {
            new NodeData {
              Name = "s1",
              Key = "-3"
            },
            new NodeData {
              Name = "s2",
              Key = "-4"
            }
          },
          Outs = new List<NodeData> {
            new NodeData {
              Name = "o1",
              Key = "-5"
            }
          },
          Loc = "-80 -80"
        },
        new NodeData {
          Key = "2",
          Name = "Other",
          Ins = new List<NodeData> {
            new NodeData {
              Name = "s1",
              Key = "-6"
            },
            new NodeData {
              Name = "s2",
              Key = "-7"
            }
          },
          Outs = new List<NodeData> {
            new NodeData {
              Name = "o1",
              Key = "-8"
            }
          },
          Loc = "80 80"
        }
      };

      var linkDataSource = new List<LinkData> {
        new LinkData {
          From = "1",
          FromPort = "o1",
          To = "2",
          ToPort = "s2"
        }
      };

      SetupDiagram(nodeDataSource, linkDataSource);
      _Save();
    }

    Node FindPortNode(Group g, string name, bool input) {
      var it = g.MemberParts.GetEnumerator();
      while (it.MoveNext()) {
        var n = it.Current as Node;
        if (n == null) continue; // n isn't node
        if ((n.Data as HiddenNodeData).Name == name && (n.Data as HiddenNodeData).PIn == input) return n;
      }
      return null;
    }

    // Transform the given data to the data structures needed internally.
    // For each data object in the "ins" Array of the node data, add a "port" Node to the Group.
    // For each data object in the "outs" Array, add a "port" Node to the Group.
    // For each link data, convert the "from" and "fromPort" information to the actual "port" Node,
    // and then the same for "to" and "toPort".
    // The internal model uses property names starting with "_" to avoid having Model.ToJson() write them out.
    void SetupDiagram(IEnumerable<NodeData> nodes, IEnumerable<LinkData> links) {
      var model = new HiddenModel {
        LinkFromKeyProperty = "F",
        LinkToKeyProperty = "T",
        NodeIsGroupProperty = "IsG",
        NodeGroupKeyProperty = "G"
      };

      // first create all of the nodes, implemented as Groups
      var newnodes = new List<HiddenNodeData>();
      foreach (var nodedata in nodes) {
        var newnodedata = new HiddenNodeData(nodedata) {
          IsG = true
        };
        newnodes.Add(newnodedata);
      }
      model.AddNodeData(newnodes);
      // now each node data will have a unique key, if not already specified

      // then create all of the ports, implemented as Nodes that are members of those Groups
      for (var i = 0; i < newnodes.Count; i++) {
        var nodedata = newnodes[i];
        if (nodedata.Ins != null) {
          for (var j = 0; j < nodedata.Ins.Count; j++) {
            var portdata = nodedata.Ins[j];
            portdata.PIn = true;
            portdata.G = nodedata.Key;
          }
          model.AddNodeData(nodedata.Ins);
          nodedata.Ins = null;
        }
        if (nodedata.Outs != null) {
          for (var j = 0; j < nodedata.Outs.Count; j++) {
            var portdata = nodedata.Outs[j];
            portdata.PIn = false;
            portdata.G = nodedata.Key;
          }
          model.AddNodeData(nodedata.Outs);
          nodedata.Outs = null;
        }
      }
      foreach (var n in model.NodeDataSource) {
        if (n.Ins != null) n.Ins = null;
        if (n.Outs != null) n.Outs = null;
      }

      myDiagram.Model = model;

      // now Groups and Nodes exist, so can find the Node corresponding to a node's port

      // finally process all of the links, to account for ports actually being member nodes
      var newlinks = new List<HiddenLinkData>();
      foreach (var oldlinkdata in links) {
        var linkdata = new HiddenLinkData(oldlinkdata);
        var fromNode = myDiagram.FindNodeForKey(linkdata.From);
        var toNode = myDiagram.FindNodeForKey(linkdata.To);
        if (fromNode != null && toNode != null) {
          // look in the Group for a "port" Node with the right name and directionality
          var fromPortNode = FindPortNode(fromNode as Group, linkdata.FromPort, false);
          var toPortNode = FindPortNode(toNode as Group, linkdata.ToPort, true);
          if (fromPortNode != null && toPortNode != null) {
            linkdata.F = (fromPortNode.Data as HiddenNodeData).Key;
            linkdata.T = (toPortNode.Data as HiddenNodeData).Key;
            linkdata.From = linkdata.To = null;
            linkdata.FromPort = linkdata.ToPort = null;
          }
        }
        newlinks.Add(linkdata);
      }
      model.AddLinkData(newlinks);
    }

    void _Save() {
      // can't just call myDiagram.Model.ToJson() -- need to transform to external format
      var m = new Model();
      m.LinkFromPortIdProperty = "FromPort";
      m.LinkToPortIdProperty = "ToPort";
      foreach (var g in myDiagram.Nodes) {
        if (g is Group) {
          (g.Data as HiddenNodeData).Ins = null;
          (g.Data as HiddenNodeData).Outs = null;
        }
      }
      foreach (var n in myDiagram.Nodes) {
        if (!(n is Group)) {
          var gd = n.ContainingGroup.Data as HiddenNodeData;
          var a = ((n.Data as HiddenNodeData).PIn ?? false) ? gd.Ins : gd.Outs;
          if (a == null) {
            a = new List<HiddenNodeData>();
            if ((n.Data as HiddenNodeData).PIn ?? false) gd.Ins = a; else gd.Outs = a;
          }
          a.Add(n.Data as HiddenNodeData);
        }
      }
      foreach (var g in myDiagram.Nodes) {
        if (g is Group) {
          m.AddNodeData(new NodeData(g.Data as HiddenNodeData));
        }
      }
      foreach (var l in myDiagram.Links) {
        var newData = new LinkData(l.Data as HiddenLinkData);
        newData.From = (l.FromNode.ContainingGroup.Data as HiddenNodeData).Key;
        newData.FromPort = (l.FromNode.Data as HiddenNodeData).Name;
        newData.To = (l.ToNode.ContainingGroup.Data as HiddenNodeData).Key;
        newData.ToPort = (l.ToNode.Data as HiddenNodeData).Name;
        m.AddLinkData(newData);
      }
      modelJson1.JsonText = m.ToJson();
      foreach (var g in myDiagram.Nodes) {
        if (g is Group) {
          (g.Data as HiddenNodeData).Ins = null;
          (g.Data as HiddenNodeData).Outs = null;
        }
      }
      myDiagram.IsModified = false;
    }

    private void LoadModel() {
      if (myDiagram == null) return;
      myDiagram.Model.UndoManager.IsEnabled = true;
      var m = Model.FromJson<Model>(modelJson1.JsonText);
      SetupDiagram(m.NodeDataSource, m.LinkDataSource);
    }
  }
}


  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public NodeData() {
    }
    public NodeData(HiddenNodeData dIn) {
      Key = dIn.Key;
      Name = dIn.Name;
      Loc = dIn.Loc;
      if (dIn.Ins != null) {
        Ins = dIn.Ins.ConvertAll((d) => {
          return new NodeData(d);
        });
      } else {
        Ins = null;
      }
      if (dIn.Outs != null) {
        Outs = dIn.Outs.ConvertAll((d) => {
          return new NodeData(d);
        });
      } else {
        Outs = null;
      }
    }
    public string Name { get; set; }
    public string Loc { get; set; }
    public List<NodeData> Ins { get; set; }
    public List<NodeData> Outs { get; set; }
  }

  public class LinkData : Model.LinkData {
    public LinkData() {
    }
    public LinkData(HiddenLinkData dIn) {
      From = dIn.From;
      To = dIn.To;
      FromPort = dIn.FromPort;
      ToPort = dIn.ToPort;
    }
  }

  public class HiddenModel : GraphLinksModel<HiddenNodeData, string, object, HiddenLinkData, string, string> { }
  public class HiddenNodeData : HiddenModel.NodeData {
    public HiddenNodeData() {
    }
    public HiddenNodeData(NodeData dIn) {
      Key = dIn.Key;
      Name = dIn.Name;
      Loc = dIn.Loc;
      if (dIn.Ins != null) {
        Ins = dIn.Ins.ConvertAll((d) => {
          return new HiddenNodeData(d);
        });
      } else {
        Ins = null;
      }
      if (dIn.Outs != null) {
        Outs = dIn.Outs.ConvertAll((d) => {
          return new HiddenNodeData(d);
        });
      } else {
        Outs = null;
      }
    }
    public string Name { get; set; }
    public string Loc { get; set; }
    public bool? PIn { get; set; }
    public string G { get; set; }
    public bool IsG { get; set; }
    public List<HiddenNodeData> Ins { get; set; }
    public List<HiddenNodeData> Outs { get; set; }
    public override string ToString() {
      return "DraggablePortsHiddenNodeData: { Key: " + Key +
        " Name: " + Name + " Loc: " + Loc + " PIn: " + PIn + " G: " + G + " IsG: " + IsG +
        " Ins: " + Ins + " Outs: " + Outs;
    }
  }

  public class HiddenLinkData : HiddenModel.LinkData {
    public HiddenLinkData() {
    }
    public HiddenLinkData(LinkData dIn) {
      From = dIn.From;
      To = dIn.To;
      FromPort = dIn.FromPort;
      ToPort = dIn.ToPort;
    }
    public string F { get; set; }
    public string T { get; set; }
  }

  public class InputOutputGroupLayout : Layout {
    public override void DoLayout(IEnumerable<Part> coll = null) {
      coll = CollectParts(Group); // only use the Group
      var portSpacing = 2;
      var iconAreaWidth = 60;

      // compute the counts and areas of the inputs and the outputs
      var left = 0;
      var leftwidth = 0.0;  // max
      var leftheight = 0.0; // total
      var right = 0;
      var rightwidth = 0.0;  // max
      var rightheight = 0.0; // total
      foreach (var n in coll) {
        if (n is Link) continue;  // ignore Links
        if ((n.Data as HiddenNodeData).PIn ?? false) {
          left++;
          leftwidth = Math.Max(leftwidth, n.ActualBounds.Width);
          leftheight += n.ActualBounds.Height;
        } else {
          right++;
          rightwidth = Math.Max(rightwidth, n.ActualBounds.Width);
          rightheight += n.ActualBounds.Height;
        }
      }
      if (left > 0) leftheight += portSpacing * (left - 1);
      if (right > 0) rightheight += portSpacing * (right - 1);

      var loc = new Point(0, 0);
      if (Group != null && Group.Location.IsReal()) loc = Group.Location;

      // first lay out the left side, the inputs
      var y = loc.Y - leftheight / 2;
      foreach (var n in coll) {
        if (n is Link) continue;  // ignore Links
        if (!(n.Data as HiddenNodeData).PIn ?? false) continue;  // ignore outputs
        n.Position = new Point(loc.X - iconAreaWidth / 2 - leftwidth, y);
        y += n.ActualBounds.Height + portSpacing;
      }

      // now the right side, the outputs
      y = loc.Y - rightheight / 2;
      foreach (var n in coll) {
        if (n is Link) continue;  // ignore Links
        if ((n.Data as HiddenNodeData).PIn ?? false) continue;  // ignore inputs
        n.Position = new Point(loc.X + iconAreaWidth / 2 + rightwidth - n.ActualBounds.Width, y);
        y += n.ActualBounds.Height + portSpacing;
      }

      // then position the group and size its icon area
      if (Group != null) {
        // position the group so that its ICON is in the middle, between the "ports"
        Group.Location = loc;
        // size the ICON so that it's wide enough to overlap the "ports" and tall enough to hold all of the "ports"
        var icon = Group.FindElement("ICON");
        if (icon != null) icon.DesiredSize = new Size(iconAreaWidth + leftwidth / 2 + rightwidth / 2, Math.Max(leftheight, rightheight) + 10);
      }
    }
  }


