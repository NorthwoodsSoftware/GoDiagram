/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;

using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;

namespace Demo.Samples.LDLayout {
  public partial class LDLayout : DemoControl {
    private Diagram _Diagram;

    public LDLayout() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      _InitControls();
      generateBtn.Click += (s, e) => _RebuildGraph();

      desc1.MdText = DescriptionReader.Read("Samples.LDLayout.md");

      AfterLoad(Setup);
    }

    private void Setup() {
      _Diagram.InitialAutoScale = AutoScale.UniformToFill;
      _Diagram.Layout = new LayeredDigraphLayout();

      _Diagram.NodeTemplate =
        new Node("Spot") { LocationSpot = Spot.Center }
          .Add(
            new Shape {
              Figure = "Rectangle",
              Fill = "lightgray", // the initial value, may be changed by data binding
              Stroke = null,
              DesiredSize = new Size(30, 30)
            }
            .Bind("Fill"),
            new TextBlock().Bind("Text")
          );

      // define the Link template to be minimal
      _Diagram.LinkTemplate =
        new Link { Selectable = false }
          .Add(new Shape { StrokeWidth = 3, Stroke = "#333" });

      _Diagram.Model = new Model();

      // generate a tree with the default values
      _RebuildGraph();
    }

    private void _RebuildGraph() {
      if (!int.TryParse(minNodes.Text.Trim(), out var min)) min = 20;
      if (!int.TryParse(maxNodes.Text.Trim(), out var max)) max = 100;

      _GenerateDigraph(min, max);
    }

    private void _GenerateDigraph(int minNodes, int maxNodes) {
      _Diagram.StartTransaction("generateDigraph");
      // replace the diagram's model's NodeDataSource
      _GenerateNodes(minNodes, maxNodes);
      // replace the diagram's model's LinkDataSource
      _GenerateLinks();
      // force a diagram layout
      _Layout();
      _Diagram.CommitTransaction("generateDigraph");
    }

    private void _GenerateNodes(int min, int max) {
      var rand = new Random();
      var nodeList = new List<NodeData>();

      if (min < 0) min = 0;
      if (max < min) max = min;
      var numNodes = rand.Next(min, max + 1);
      for (var i = 1; i <= numNodes; i++) {
        nodeList.Add(
          new NodeData {
            Key = i,
            Text = i.ToString(),
            Fill = Brush.RandomColor()
          }
        );
      }

      // randomize the node data
      for (var i = 0; i < nodeList.Count; i++) {
        var swap = rand.Next(nodeList.Count);
        var temp = nodeList[swap];
        nodeList[swap] = nodeList[i];
        nodeList[i] = temp;
      }

      _Diagram.Model.NodeDataSource = nodeList;
    }

    private void _GenerateLinks() {
      if (_Diagram.Nodes.Count < 2) return;
      var rand = new Random();
      var linkList = new List<LinkData>();
      var nodes = new List<Node>(_Diagram.Nodes);
      for (var i = 0; i < nodes.Count - 1; i++) {
        var from = nodes[i];
        var numto = Math.Floor(1 + rand.NextDouble() * 3 / 2);
        for (var j = 0; j < numto; j++) {
          var idx = Math.Floor(i + 5 + rand.NextDouble() * 10);
          if (idx >= nodes.Count) idx = i + rand.Next(nodes.Count - i) | 0;
          var to = nodes[(int)idx];
          linkList.Add(new LinkData { From = (from.Data as NodeData).Key, To = (to.Data as NodeData).Key });
        }
      }

      (_Diagram.Model as Model).LinkDataSource = linkList;
    }

    private void _Layout() {
      if (_Diagram.Layout is not LayeredDigraphLayout lay) return;
      _Diagram.StartTransaction("change layout");

      lay.Direction = _Direction;
      if (!double.TryParse(layerSpacing.Text.Trim(), out var ls)) ls = 25;
      lay.LayerSpacing = ls;
      if (!double.TryParse(columnSpacing.Text.Trim(), out var cs)) cs = 25;
      lay.ColumnSpacing = cs;

      lay.CycleRemoveOption = _CycleRemove;
      lay.LayeringOption = _Layering;
      lay.InitializeOption = _Init;
      lay.AggressiveOption = _Aggressive;

      var packOption = LayeredDigraphPack.None;
      if (_GetChecked(expand)) packOption |= LayeredDigraphPack.Expand;
      if (_GetChecked(straighten)) packOption |= LayeredDigraphPack.Straighten;
      if (_GetChecked(median)) packOption |= LayeredDigraphPack.Median;
      lay.PackOption = packOption;

      lay.SetsPortSpots = _GetChecked(setsPortSpots);

      _Diagram.CommitTransaction("change layout");
    }

    private int _Direction = 0;
    private LayeredDigraphCycleRemove _CycleRemove = LayeredDigraphCycleRemove.DepthFirst;
    private LayeredDigraphLayering _Layering = LayeredDigraphLayering.OptimalLinkLength;
    private LayeredDigraphInit _Init = LayeredDigraphInit.DepthFirstOut;
    private LayeredDigraphAggressive _Aggressive = LayeredDigraphAggressive.Less;
  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }
  public class NodeData : Model.NodeData {
    public Brush Fill { get; set; }
  }
  public class LinkData : Model.LinkData { }
}
