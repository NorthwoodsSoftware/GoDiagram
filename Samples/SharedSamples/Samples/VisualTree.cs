/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;

namespace Demo.Samples.VisualTree {
  public partial class VisualTree : DemoControl {
    private Diagram _Diagram;
    private Diagram _VisualTree;

    public VisualTree() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;
      _VisualTree = diagramControl2.Diagram;

      Setup();
      SetupVisual();

      drawVisualTreeBtn.Click += (s, e) => DrawVisualTree();

      desc1.MdText = DescriptionReader.Read("Samples.VisualTree.md");

      // Draw the visual tree after each control has loaded
      AfterLoad(DrawVisualTree);
    }

    private void Setup() {
      _Diagram.UndoManager.IsEnabled = true;

      _Diagram.NodeTemplate =
        new Node("Auto")
          .Bind(new Binding("Location").MakeTwoWay())
          .Add(
            new Shape("RoundedRectangle") { Fill = "white", StrokeWidth = 0 }.Bind("Fill", "Color"),
            new TextBlock { Margin = 5 }.Bind("Text", "Key")
          );

      _Diagram.LinkTemplate =
        new Link { Curve = LinkCurve.Bezier }
          .Add(
            new Shape { StrokeWidth = 1.5 },
            new Shape { ToArrow = "Standard" }
          );

      _Diagram.Model = new Model {
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
      _VisualTree.InitialContentAlignment = Spot.Left;
      _VisualTree.InitialAutoScale = AutoScale.Uniform;
      _VisualTree.IsReadOnly = true;
      _VisualTree.AllowSelect = false;
      _VisualTree.Layout = new TreeLayout { NodeSpacing = 5 };

      _VisualTree.NodeTemplate =
        new Node("Auto")
          .Add(
            new Shape { Fill = "darkkhaki", Stroke = null },
            new TextBlock {
                Font = new Font("Arial", 13, Northwoods.Go.FontWeight.Bold),
                Stroke = "black",
                Margin = 3
              }
              .Bind("Text")
          );

      _VisualTree.LinkTemplate =
        new Link()
          .Add(
            new Shape { Stroke = "darkkhaki", StrokeWidth = 2 }
          );
    }

    private void DrawVisualTree() {
      if (_Diagram == null) return;
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

      TraverseVisualTree(_Diagram);

      _VisualTree.Model = new TreeModel {
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
