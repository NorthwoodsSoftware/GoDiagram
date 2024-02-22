/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace Demo.Samples.TreeView {
  public partial class TreeView : DemoControl {
    private Diagram _Diagram;

    public TreeView() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      desc1.MdText = DescriptionReader.Read("Samples.TreeView.md");
    }

    private void Setup() {
      _Diagram.AllowMove = false;
      _Diagram.AllowCopy = false;
      _Diagram.AllowDelete = false;
      _Diagram.AllowHorizontalScroll = false;
      _Diagram.Layout = new TreeLayout {
        Alignment = TreeAlignment.Start,
        Angle = 0,
        Compaction = TreeCompaction.None,
        LayerSpacing = 16,
        LayerSpacingParentOverlap = 1,
        NodeIndentPastParent = 1.0,
        NodeSpacing = 0,
        SetsPortSpot = false,
        SetsChildPortSpot = false
      };

      // takes a property change on either IsTreeLeaf or IsTreeExpanded and selects the correct image to use
      static string imageConverter(object prop, object picture) {
        var node = (picture as GraphObject).Part as Node;
        if (node.IsTreeLeaf) {
          return "https://img.icons8.com/small/2x/document.png";
        } else {
          if (node.IsTreeExpanded) {
            return "https://img.icons8.com/small/2x/opened-folder--v1.png";
          } else {
            return "https://img.icons8.com/small/2x/folder-invoices--v1.png";
          }
        }
      }

      _Diagram.NodeTemplate = new Node {
        // no Adornment: instead change panel background color by binding to Node.IsSelected
        SelectionAdorned = false,
        // a custom function to allow expanding/collapsing on double-click
        // this uses similar logic to a TreeExpanderButton
        DoubleClick = (e, _node) => {
          var node = _node as Node;
          var cmd = _Diagram.CommandHandler;
          if (node.IsTreeExpanded) {
            if (!cmd.CanCollapseTree(node)) return;
          } else {
            if (!cmd.CanExpandTree(node)) return;
          }
          e.Handled = true;
          if (node.IsTreeExpanded) {
            cmd.CollapseTree(node);
          } else {
            cmd.ExpandTree(node);
          }
        }
      }.Add(
        Builder.Make<Panel>("TreeExpanderButton")
          .Set(new {
            _TreeExpandedFigure = "LineDown",
            _TreeCollapsedFigure = "LineRight",
            ButtonBorder_Fill = "whitesmoke",
            ButtonBorder_Stroke = (Brush)null,
            _ButtonFillOver = "rgba(0, 128, 255, .25)",
            _ButtonStrokeOver = (Brush)null
          }),
        new Panel(PanelType.Horizontal) {
          Position = new Point(18, 0)
        }.Bind(
          new Binding("Background", "IsSelected", (s, _) => (bool)s ? "lightblue" : "white").OfElement()
        ).Add(
          new Picture {
            Width = 18,
            Height = 18,
            Margin = new Margin(0, 4, 0, 0),
            ImageStretch = ImageStretch.Uniform
          }.Bind(
            // bind the picture source on two properties of the Node
            // to display open folder, closed folder, or document
            new Binding("Source", "IsTreeExpanded", imageConverter).OfElement(),
            new Binding("Source", "IsTreeLeaf", imageConverter).OfElement()
          ),
          new TextBlock {
            Font = new Font("Segoe UI", 9)
          }.Bind("Text", "Key", (s, _) => "item " + s)
        ) // end Horizontal panel
      ); // end Node

      // without lines
      _Diagram.LinkTemplate = new Link();

      // // with lines
      // MyDiagram.LinkTemplate = new Link {
      //   Selectable = false,
      //   Routing = LinkRouting.Orthogonal,
      //   FromEndSegmentLength = 4,
      //   ToEndSegmentLength = 4,
      //   FromSpot = new Spot(0.001, 1, 7, 0),
      //   ToSpot = Spot.Left
      // }.Add(
      //   new Shape {
      //     Stroke = "gray",
      //     StrokeDashArray = new double[] { 1, 2 }
      //   }
      //);

      // create a random tree
      var nodeDataSource = new List<NodeData> {
        new NodeData { Key = 1 }
      };
      var max = 500;
      var count = 1;
      while (count < max) {
        count = _MakeTree(3, count, max, nodeDataSource, nodeDataSource[0]);
      }
      _Diagram.Model = new Model {
        NodeDataSource = nodeDataSource
      };
    }

    private int _MakeTree(int level, int count, int max, List<NodeData> nodeDataSource, NodeData parentdata) {
      var rand = new Random();
      var numchildren = rand.Next(10);
      for (var i = 0; i < numchildren; i++) {
        if (count >= max) return count;
        count++;
        var childdata = new NodeData { Key = count, Parent = parentdata.Key };
        nodeDataSource.Add(childdata);
        if (level > 0 && rand.NextDouble() > 0.5) {
          count = _MakeTree(level - 1, count, max, nodeDataSource, childdata);
        }
      }
      return count;
    }
  }

  public class Model : TreeModel<NodeData, int, object> { }

  public class NodeData : Model.NodeData {

  }
}
