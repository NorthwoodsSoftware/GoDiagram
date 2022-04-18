/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;
using Northwoods.Go.Tools.Extensions;

namespace WinFormsExtensionControls.OverviewResizing {
  [ToolboxItem(false)]
  public partial class OverviewResizingControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    private Overview myOverview;

    private Random _Rand = new();

    public OverviewResizingControl() {
      InitializeComponent();

      myDiagram = diagramControl1.Diagram;
      myOverview = overviewControl1.Diagram as Overview;

      Setup();
      SetupOverview();

      zoomToFit.Click += (e, obj) => ZoomToFit();
      expandRandomNode.Click += (e, obj) => ExpandAtRandom();


      goWebBrowser1.Html = @"
      <p>
        This sample demonstrates a custom <a>ResizingTool</a> which allows the user to resize the overview box.
        It is defined in its own file, as <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/OverviewResizing/OverviewResizingTool.cs"">OverviewResizingTool.cs</a>.
      </p>
";
    }

    private void Setup() {
      myDiagram.Layout = new ForceDirectedLayout();
      myDiagram.UndoManager.IsEnabled = true;

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

      myDiagram.NodeTemplate =
        new Node(PanelLayoutSpot.Instance) {
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


      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "0", EverExpanded = false }
        }
      };
    }

    private void SetupOverview() {
      myOverview.Observed = myDiagram;
      myOverview.ContentAlignment = Spot.Center;
      myOverview.Box.Resizable = true;
      myOverview.ToolManager.ResizingTool = new OverviewResizingTool();
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
      myDiagram = diagramControl1.Diagram;
      var numchildren = _Rand.Next(10);

      if (myDiagram.Nodes.Count <= 1) {
        numchildren += 1; // make sure the root node has at least one child
      }
      // create several node data objects and add them to the model
      var model = myDiagram.Model;
      var parent = myDiagram.FindNodeForData(parentdata);

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
        var child = myDiagram.FindNodeForData(childdata);
        child.Location = parent.Location;
      }
      return numchildren;
    }

    private void ExpandAtRandom() {
      var eligibleNodes = new List<Node>();
      foreach (var n in myDiagram.Nodes) {
        if (!n.IsTreeExpanded) eligibleNodes.Add(n);
      }
      var node = eligibleNodes[_Rand.Next(eligibleNodes.Count)];
      _ExpandNode(node);
    }

    private void ZoomToFit() {
      myDiagram.ZoomToFit();
    }
  }

  public class Model : TreeModel<NodeData, string, object> { }

  public class NodeData : Model.NodeData {
    public int RootDistance { get; set; }
    public bool EverExpanded { get; set; }
  }
}
