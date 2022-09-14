/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;
using Northwoods.Go.Tools.Extensions;

namespace Demo.Extensions.OverviewResizing {
  public partial class OverviewResizing : DemoControl {
    private Diagram _Diagram;
    private Overview _Overview;

    private Random _Rand = new();

    public OverviewResizing() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;
      _Overview = overviewControl1.Diagram as Overview;

      Setup();
      SetupOverview();

      zoomFitBtn.Click += (e, obj) => ZoomToFit();
      expandBtn.Click += (e, obj) => ExpandAtRandom();

      desc1.MdText = DescriptionReader.Read("Extensions.OverviewResizing.md");
    }

    private void Setup() {
      _Diagram.Layout = new ForceDirectedLayout();
      _Diagram.UndoManager.IsEnabled = true;

      var expanderButton = Builder.Make<Panel>("TreeExpanderButton");
      expanderButton.Name = "TREEBUTTON";
      expanderButton.Width = 20;
      expanderButton.Height = 20;
      expanderButton.Alignment = Spot.TopRight;
      expanderButton.AlignmentFocus = Spot.Center;
      expanderButton.Click = (e, obj) => { // OBJ is the button
        // get the Node containing this button
        if (obj.Part is not Node node) return;
        e.Handled = true;
        _ExpandNode(node);
      };

      _Diagram.NodeTemplate =
        new Node(PanelType.Spot) {
          SelectionElementName = "PANEL",
          IsTreeExpanded = false,
          IsTreeLeaf = false
        }.Add(new Panel("Auto") {
          Name = "PANEL"
        }.Add(new Shape {
          Figure = "Circle",
          Fill = "#03A9F4",
          Stroke = "black"
        },
        new TextBlock {
          Font = new Font("Microsoft Sans Serif", 12),
          Margin = 5
        }.Bind("Text", "Key")),
        expanderButton
        );


      _Diagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "0", EverExpanded = false, RootDistance = 0 }
        }
      };
    }

    private void SetupOverview() {
      _Overview.Observed = _Diagram;
      _Overview.ContentAlignment = Spot.Center;
      _Overview.Box.Resizable = true;
      _Overview.ToolManager.ResizingTool = new OverviewResizingTool();
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
      var numchildren = _Rand.Next(10);

      if (_Diagram.Nodes.Count <= 1) {
        numchildren += 1; // make sure the root node has at least one child
      }
      // create several node data objects and add them to the model
      var model = _Diagram.Model;
      var parent = _Diagram.FindNodeForData(parentdata);

      var degrees = 1;
      var grandparent = parent.FindTreeParentNode();
      while (grandparent != null) {
        degrees++;
        grandparent = grandparent.FindTreeParentNode();
      }

      for (var i = 0; i < numchildren; i++) {
        var childdata = new NodeData {
          Key = model.NodeDataSource.Count().ToString(),
          Parent = parentdata.Key,
          RootDistance = degrees
        };
        // add to model.NodeDataSource and create a node
        model.AddNodeData(childdata);
        // position the new child node close to the parent
        var child = _Diagram.FindNodeForData(childdata);
        child.Location = parent.Location;
      }
      return numchildren;
    }

    private void ExpandAtRandom() {
      var eligibleNodes = new List<Node>();
      foreach (var n in _Diagram.Nodes) {
        if (!n.IsTreeExpanded) eligibleNodes.Add(n);
      }
      var node = eligibleNodes[_Rand.Next(eligibleNodes.Count)];
      _ExpandNode(node);
    }

    private void ZoomToFit() {
      _Diagram.ZoomToFit();
    }
  }

  public class Model : TreeModel<NodeData, string, object> { }
  public class NodeData : Model.NodeData {
    public int RootDistance { get; set; }
    public bool EverExpanded { get; set; }
  }
}
