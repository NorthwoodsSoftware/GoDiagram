/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Layouts.Extensions;
using Northwoods.Go.Models;

namespace Demo.Extensions.Parallel {
  public partial class Parallel : DemoControl {
    private Diagram _Diagram;

    public Parallel() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      desc1.MdText = DescriptionReader.Read("Extensions.Parallel.md");
    }

    private void Setup() {
      _Diagram.AllowCopy = false;
      _Diagram.AllowDelete = false;
      _Diagram.Layout = new ParallelLayout {
        LayerSpacing = 20,
        NodeSpacing = 10
      };

      // define the Node template(s)
      _Diagram.NodeTemplate = new Node(PanelType.Auto) {
        LocationSpot = Spot.Center
      }.Add(
       new Shape {
         Figure = "Rectangle",
         Fill = "wheat",
         Stroke = null,
         StrokeWidth = 0
       },
       new TextBlock {
         Margin = 3
       }.Bind("Text")
      );

      _Diagram.NodeTemplateMap.Add("Split",
        new Node(PanelType.Auto) {
          LocationSpot = Spot.Center
        }.Add(
          new Shape {
            Figure = "Diamond",
            Fill = "deepskyblue",
            Stroke = null,
            StrokeWidth = 0,
            DesiredSize = new Size(28, 28)
          },
          new TextBlock().Bind("Text")
      ));

      _Diagram.NodeTemplateMap.Add("Merge",
        new Node(PanelType.Auto) {
          LocationSpot = Spot.Center
        }.Add(
          new Shape {
            Figure = "Circle",
            Fill = "deepskyblue",
            Stroke = null,
            StrokeWidth = 0,
            DesiredSize = new Size(28, 28)
          },
          new TextBlock().Bind("Text")
      ));

      // define the Link template to be minimal
      _Diagram.LinkTemplate = new Link {
        Routing = LinkRouting.Orthogonal,
        Corner = 5
      }.Add(
        new Shape {
          Stroke = "gray",
          StrokeWidth = 1.5
        }
      );

      // define the Group template to be fairly simple
      _Diagram.GroupTemplate = new Group(PanelType.Auto) {
        Layout = new ParallelLayout {
          LayerSpacing = 20,
          NodeSpacing = 10
        }
      }.Add(
        new Shape {
          Fill = "transparent",
          Stroke = "darkgoldenrod"
        },
        new Placeholder {
          Padding = 10
        },
        Builder.Make<Panel>("SubGraphExpanderButton").Set(new { Alignment = Spot.TopLeft })  // SubGraphExpanderButton?
      );

      _Diagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = -1, IsGroup = true },
          new NodeData { Key = -2, IsGroup = true },
          new NodeData { Key = -3, IsGroup = true },

          new NodeData { Key = 1, Text = "S", Category = "Split", Group = -1 },
          new NodeData { Key = 2, Text = "C", Group = -1 },
          new NodeData { Key = 3, Text = "Longer Node", Group = -1 },
          new NodeData { Key = 4, Text = "A", Group = -1 },
          new NodeData { Key = 5, Text = "B\nB", Group = -1 },
          new NodeData { Key = 6, Text = "Another", Group = -1 },
          new NodeData { Key = 9, Text = "J", Category = "Merge", Group = -1 },
          new NodeData { Key = 11, Text = "T", Category = "Split", Group = -2 },
          new NodeData { Key = 12, Text = "C", Group = -2 },
          new NodeData { Key = 13, Text = "Here", Group = -2 },
          new NodeData { Key = 14, Text = "D", Group = -2 },
          new NodeData { Key = 15, Text = "Everywhere", Group = -2 },
          new NodeData { Key = 16, Text = "EEEEE", Group = -2 },
          new NodeData { Key = 19, Text = "K", Category = "Merge", Group = -2 },
          new NodeData { Key = 21, Text = "U", Category = "Split", Group = -3 },
          new NodeData { Key = 22, Text = "F", Group = -3 },
          new NodeData { Key = 23, Text = "Medium\nTall\nNode", Group = -3 },
          new NodeData { Key = 24, Text = "G", Group = -3 },
          new NodeData { Key = 25, Text = "AS", Group = -3 },
          new NodeData { Key = 26, Text = "H\nHH\nHHH", Group = -3 },
          new NodeData { Key = 27, Text = "I", Group = -3 },
          new NodeData { Key = 29, Text = "L", Category = "Merge", Group = -3 },
          new NodeData { Key = 101, Text = "0", Category = "Split" },
          new NodeData { Key = 107, Text = "ABCDEFG" },
          new NodeData { Key = 109, Text = "*", Category = "Merge" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = 1, To = 2 },
          new LinkData { From = 2, To = 3 },
          new LinkData { From = 3, To = 4 },
          new LinkData { From = 4, To = 9 },
          new LinkData { From = 1, To = 5 },
          new LinkData { From = 5, To = 6 },
          new LinkData { From = 6, To = 9 },
          new LinkData { From = 9, To = 11 },
          new LinkData { From = 9, To = 21 },
          new LinkData { From = 11, To = 12 },
          new LinkData { From = 12, To = 13 },
          new LinkData { From = 13, To = 14 },
          new LinkData { From = 14, To = 19 },
          new LinkData { From = 11, To = 15 },
          new LinkData { From = 15, To = 16 },
          new LinkData { From = 16, To = 19 },
          new LinkData { From = 21, To = 22 },
          new LinkData { From = 22, To = 24 },
          new LinkData { From = 24, To = 26 },
          new LinkData { From = 23, To = 29 },
          new LinkData { From = 21, To = 25 },
          new LinkData { From = 25, To = 23 },
          new LinkData { From = 21, To = 27 },
          new LinkData { From = 26, To = 29 },
          new LinkData { From = 27, To = 29 },
          new LinkData { From = 101, To = 1 },
          new LinkData { From = 19, To = 109 },
          new LinkData { From = 29, To = 107 },
          new LinkData { From = 107, To = 109 },
        }
      };
    }
  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }

  public class NodeData : Model.NodeData { }

  public class LinkData : Model.LinkData { }
}
