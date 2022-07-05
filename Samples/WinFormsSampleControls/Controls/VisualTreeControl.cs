/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;
using System.ComponentModel;

namespace WinFormsSampleControls.VisualTree {
  [ToolboxItem(false)]
  public partial class VisualTreeControl : System.Windows.Forms.UserControl {

    private Diagram myDiagram;
    private Diagram myVisualTree;


    public VisualTreeControl() {
      InitializeComponent();

      Setup();
      SetupVisual();

      visualTreeBtn.Click += (e, obj) => DrawVisualTree();

      goWebBrowser1.Html = @"
          <p>
          <b>myDiagram</b>, the diagram being inspected:
          </p>
";

      goWebBrowser2.Html = @"
          <p>
          <b>myVisualTree</b>, showing the Layers, Nodes and Links that are in <b>myDiagram</b> above:
          </p>
";
      goWebBrowser3.Html = @"
          <p>
        This sample shows the actual visual tree of a running Diagram.
        The Diagram that we inspect is named ""myDiagram"" and initially contains two simple Nodes and two Links.
        The Diagram below it is named ""myVisualTree"" and shows the visual tree of ""myDiagram"".
          </p>
          <p>
        You can also try selecting, copying, and deleting parts in <b>myDiagram</b>
        and then click on ""Draw Visual Tree"" again to see how the visual tree in <b>myDiagram</b> changes.
          </p>
          <p>
        The <b>TraverseVisualTree</b> function is what walks the visual tree of ""myDiagram""
        and constructs the corresponding Nodes and Links used in ""myVisualTree"".
        The text for each Node in ""myVisualTree"" is data - bound to the actual Diagram/Layer/Part/GraphObject object.
        That object is converted to a text string by using the <b>ToString</b> method.
          </p>
          <p>
        See also the <a href=""VisualTreeGrouping"">Visual Tree Using Groups</a> sample,
        to show the same visual tree using nested groups. For more use of the <a>TreeLayout</a>,
        see the <a href=""ClassHierarchy"">Class Hierarchy Tree</a> sample.
          </p>
";
    }

    // Draw the visual tree after each control has loaded
    protected override void OnLoad(EventArgs e) {
      DrawVisualTree();
      base.OnLoad(e);
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;
      myDiagram.UndoManager.IsEnabled = true;

      myDiagram.NodeTemplate =
        new Node("Auto")
          .Bind(new Binding("Location").MakeTwoWay())
          .Add(
            new Shape("RoundedRectangle") { Fill = "white", StrokeWidth = 0 }.Bind("Fill", "Color"),
            new TextBlock { Margin = 5 }.Bind("Text", "Key")
          );

      myDiagram.LinkTemplate =
        new Link { Curve = LinkCurve.Bezier }
          .Add(
            new Shape { StrokeWidth = 1.5 },
            new Shape { ToArrow = "Standard" }
          );

      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "Alpha", Color = "lightblue" , Location = new Point(0, 0)},
          new NodeData { Key = "Beta", Color = "pink", Location = new Point(60, 80) }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = "Alpha", To = "Beta" },
          new LinkData { From = "Beta", To = "Alpha" }
        }
      };
    }

    private void SetupVisual() {
      myVisualTree = diagramControl2.Diagram;
      myVisualTree.InitialContentAlignment = Spot.Left;
      myVisualTree.InitialAutoScale = AutoScale.Uniform;
      myVisualTree.IsReadOnly = true;
      myVisualTree.AllowSelect = false;
      myVisualTree.Layout = new TreeLayout { NodeSpacing = 5 };

      myVisualTree.NodeTemplate =
        new Node("Auto")
          .Add(
            new Shape { Fill = "darkkhaki", Stroke = null },
            new TextBlock {
                Font = new Font("Arial", 13, FontWeight.Bold),
                Stroke = "black",
                Margin = 3
              }
              .Bind("Text")
          );

      myVisualTree.LinkTemplate =
        new Link()
          .Add(
            new Shape { Stroke = "darkkhaki", StrokeWidth = 2 }
          );
    }

    private void DrawVisualTree() {
      if (myDiagram == null) return;
      var visualNodeDataArray = new List<TreeNodeData>();

      void TraverseVisualTree(object obj, int parent = 0) {
        var text = obj.ToString();
        if (obj is Link li) {
          var ld = li.Data as LinkData;
          text = "Link#" + li.GetHashCode(); //same as hashId?
          text += "(" + ld.From + " to " + ld.To + ")";
        }
        var nd = new TreeNodeData() {
          Key = visualNodeDataArray.Count + 1,
          Parent = parent,
          Text = text
        };

        visualNodeDataArray.Add(nd);
        if (obj is Diagram d) {
          foreach (var layer in d.Layers) TraverseVisualTree(layer, nd.Key);
        } else if (obj is Layer l) {
          foreach (var part in l.Parts) TraverseVisualTree(part, nd.Key);
        } else if (obj is Panel p) {
          foreach (var element in p.Elements) TraverseVisualTree(element, nd.Key);
        }
      }

      TraverseVisualTree(myDiagram);

      myVisualTree.Model = new TreeModel {
        NodeDataSource = visualNodeDataArray
      };
    }
  }

  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }

  public class NodeData : Model.NodeData {
    public string Color { get; set; }
  }

  public class LinkData : Model.LinkData { }

  public class TreeModel : TreeModel<TreeNodeData, int, object> { }

  public class TreeNodeData : TreeModel.NodeData {
    public int ParentKey { get; set; }
  }
}
