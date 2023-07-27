/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace Demo.Samples.Grouping {
  public partial class Grouping : DemoControl {
    private Diagram _Diagram;

    public Grouping() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      desc1.MdText = DescriptionReader.Read("Samples.Grouping.md");
    }

    private void Setup() {
      _Diagram.Layout = new TreeLayout { // the layout for the entire diagram
        Angle = 90,
        Arrangement = TreeArrangement.Horizontal,
        IsRealtime = false
      };
      _Diagram.Model = new Model();

      // define the node template for non-groups
      _Diagram.NodeTemplate =
        new Node("Auto")
          .Add(
            new Shape {
                Figure = "Rectangle",
                Stroke = null, StrokeWidth = 0
              }
              .Bind("Fill", "Key"),
            new TextBlock {
                Margin = 7, Font = new Font("Segoe UI", 14, Northwoods.Go.FontWeight.Bold)
              }
              .Bind("Text", "Key") // text, color, and key are all bound to the same property in the node data
          );

      _Diagram.LinkTemplate =
        new Link {
            Routing = LinkRouting.Orthogonal,
            Corner = 10
          }
          .Add(
            new Shape { StrokeWidth = 2 },
            new Shape { ToArrow = "OpenTriangle" }
          );

      // define the group template
      _Diagram.GroupTemplate =
        new Group("Auto") {
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
          }
          .Add(
            new Shape {
                Figure = "Rectangle",
                Fill = null,
                Stroke = "gray",
                StrokeWidth = 2
              },
            new Panel("Vertical") { DefaultAlignment = Spot.Left, Margin = 4 }
              .Add(
                new Panel("Horizontal") { DefaultAlignment = Spot.Top }
                  .Add(
                    // the SubGraphExpanderButton is a panel that functions as a button to expand or collapse the sub-graph
                    Builder.Make<Panel>("SubGraphExpanderButton"),
                    new TextBlock {
                        Font = new Font("Segoe UI", 18, Northwoods.Go.FontWeight.Bold),
                        Margin = 4
                      }
                      .Bind("Text", "Key")
                  ),
                // create a placeholder to represent the area where the contents of the group are
                new Placeholder { Padding = new Margin(0, 10) }
              )
          );

      // generate the initial model (and consider the random groups part of initialization)
      _Diagram.DelayInitialization((_Diagram) => _RandomGroup());
    }

    // Generate a random number of nodes, including groups.
    // If a group's key is given as a parameter, put these nodes inside it
    private void _RandomGroup(string group = null) {
      var rand = new Random();

      // all modification to the diagram is within this transaction
      _Diagram.StartTransaction("AddGroupContents");
      var addedKeys = new List<string>(); // this will contain the keys of all nodes created
      var groupCount = 0; // the number of groups in the diagram, to determine the numbers in the keys of new groups
      foreach (var node in _Diagram.Nodes) {
        if (node is Group) groupCount++;
      }
      // create a random number of groups
      // ensure there are at least 10 groups in the diagram
      var groups = rand.Next(2);
      if (groupCount < 10) groups++;
      for (var i = 0; i < groups; i++) {
        var name = "group" + (i + groupCount);
        if (group == null) {
          _Diagram.Model.AddNodeData(new NodeData {
            Key = name,
            IsGroup = true
          });
        } else {
          _Diagram.Model.AddNodeData(new NodeData {
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
        if (_Diagram.FindPartForKey(color) == null) {
          _Diagram.Model.AddNodeData(new NodeData {
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
          (_Diagram.Model as Model).AddLinkData(new LinkData {
            From = arr[from], To = arr[i]
          });
        }
      }
      _Diagram.CommitTransaction("AddGroupContents");
    }
  }

  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData { }
  public class LinkData : Model.LinkData { }
}
