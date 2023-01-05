/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.Linq;

using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;

namespace Demo.Samples.TLayout {
  public partial class TLayout : DemoControl {
    private Diagram _Diagram;

    public TLayout() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      _InitControls();
      generateBtn.Click += (s, e) => _RebuildGraph();

      desc1.MdText = DescriptionReader.Read("Samples.TLayout.md");

      // Ensure page is loaded before we start updating the layout from the UI
      AfterLoad(Setup);
    }

    private void Setup() {
      _Diagram.InitialAutoScale = AutoScale.UniformToFill;
      _Diagram.Layout = new TreeLayout {
        Comparer = TreeVertex.SmartComparer  // have the comparer sort by numbers as well as letters
      }; // other properties are set by the layout function, defined below

      _Diagram.NodeTemplate =
        new Node("Spot") {
            LocationSpot = Spot.Center
          }
          .Bind("Text")
          .Add(
            new Shape("Ellipse") {
                Fill = "lightgray", // data binding may provide different value
                Stroke = null,
                DesiredSize = new Size(30, 30)
              }
              .Bind("DesiredSize", "Size")
              .Bind("Fill"),
            new TextBlock().Bind("Text")
          );

      _Diagram.LinkTemplate =
        new Link { Routing = LinkRouting.Orthogonal, Selectable = false }
          .Add(new Shape { StrokeWidth = 3, Stroke = "#333" });

      // generate a tree with the default values
      _RebuildGraph();
    }

    private void _RebuildGraph() {
      var nodeDataSource = GenerateNodeData((int)minNodes.Value, (int)maxNodes.Value, (int)minChil.Value, (int)maxChil.Value, _GetChecked(randomSizes));
      _Diagram.Model = new Model {
        NodeDataSource = nodeDataSource
      };

      // update the diagram layout customized by the various control values
      _Layout();
    }

    // Creates a random number (between MIN and MAX) of randomly colored nodes.
    private List<NodeData> GenerateNodeData(int minNodes, int maxNodes, int minChil, int maxChil, bool hasRandomSizes) {
      var nodeList = new List<NodeData>();
      var rand = new Random();
      if (minNodes < 1) minNodes = 1;
      if (maxNodes < minNodes) maxNodes = minNodes;

      // Create a bunch of node data
      var numNodes = rand.Next(minNodes, maxNodes + 1);
      for (var i = 1; i <= numNodes; i++) {
        nodeList.Add(new NodeData {
          Key = i, // the unique identifier
          // Parent is set by code below that assigns children
          Text = i.ToString(),
          Fill = Brush.RandomColor(),
          Size = hasRandomSizes ? new Size(rand.Next(20, 70), rand.Next(20, 70)) : new Size(30, 30)
        });
      }

      // Randomize the node data
      for (var i = 0; i < nodeList.Count; i++) {
        var swap = rand.Next(nodeList.Count);
        var temp = nodeList[swap];
        nodeList[swap] = nodeList[i];
        nodeList[i] = temp;
      }

      // Takes the random collection of node data and creates a random tree with them.
      // Respects the minimum and maximum number of links from each node.
      // The minimum can be disregarded if we run out of nodes to link to.
      if (nodeList.Count > 1) {
        if (minChil < 0) minChil = 0;
        if (maxChil < minChil) maxChil = minChil;

        // keep the Set of node data that do not yet have a parent
        var available = new HashSet<NodeData>(nodeList);
        for (var i = 0; i < nodeList.Count; i++) {
          var parent = nodeList[i];
          available.Remove(parent);

          // assign some number of node data as children of this parent node data
          var children = rand.Next(minChil, maxChil + 1);
          for (var j = 0; j < children; j++) {
            if (available.Count == 0) break; // oops, ran out already
            var child = available.First();
            available.Remove(child);
            // have the child node data refer to the parent node data by its key
            child.Parent = parent.Key;
          }
          if (available.Count == 0) break; // nothing left?
        }
      }
      return nodeList;
    }

    // Update the layout from the controls, and then perform the layout again
    private void _Layout() {
      if (_Diagram.Layout is not TreeLayout lay) return;
      _Diagram.StartTransaction("change layout");

      lay.TreeStyle = (TreeStyle)Enum.Parse(typeof(TreeStyle), (string)style.SelectedItem);
      lay.LayerStyle = (TreeLayerStyle)Enum.Parse(typeof(TreeLayerStyle), (string)layerStyle.SelectedItem);

      lay.Angle = _Angle;
      lay.Alignment = (TreeAlignment)Enum.Parse(typeof(TreeAlignment), (string)align.SelectedItem);
      lay.NodeSpacing = (double)nodeSpacing.Value;
      lay.NodeIndent = (double)nodeIndent.Value;
      lay.NodeIndentPastParent = (double)nodeIndentPastParent.Value;
      lay.LayerSpacing = (double)layerSpacing.Value;
      lay.LayerSpacingParentOverlap = (double)layerSpacingParentOverlap.Value;
      lay.Sorting = (TreeSorting)Enum.Parse(typeof(TreeSorting), (string)sorting.SelectedItem);
      lay.Compaction = _Compaction;
      lay.BreadthLimit = (double)breadthLimit.Value;
      lay.RowSpacing = (double)rowSpacing.Value;
      lay.RowIndent = (double)rowIndent.Value;
      lay.SetsPortSpot = _GetChecked(setsPortSpot);
      lay.SetsChildPortSpot = _GetChecked(setsChildPortSpot);

      if (lay.TreeStyle != TreeStyle.Layered) {
        lay.AlternateAngle = _AltAngle;
        lay.AlternateAlignment = (TreeAlignment)Enum.Parse(typeof(TreeAlignment), (string)altAlign.SelectedItem);
        lay.AlternateNodeSpacing = (double)altNodeSpacing.Value;
        lay.AlternateNodeIndent = (double)altNodeIndent.Value;
        lay.AlternateNodeIndentPastParent = (double)altNodeIndentPastParent.Value;
        lay.AlternateLayerSpacing = (double)altLayerSpacing.Value;
        lay.AlternateLayerSpacingParentOverlap = (double)altLayerSpacingParentOverlap.Value;
        lay.AlternateSorting = (TreeSorting)Enum.Parse(typeof(TreeSorting), (string)altSorting.SelectedItem);
        lay.AlternateCompaction = _AltCompaction;
        lay.AlternateBreadthLimit = (double)altBreadthLimit.Value;
        lay.AlternateRowSpacing = (double)altRowSpacing.Value;
        lay.AlternateRowIndent = (double)altRowIndent.Value;
        lay.AlternateSetsPortSpot = _GetChecked(altSetsPortSpot);
        lay.AlternateSetsChildPortSpot = _GetChecked(altSetsChildPortSpot);
      }

      _Diagram.CommitTransaction("change layout");
    }

    private int _Angle = 0;
    private int _AltAngle = 0;
    private TreeCompaction _Compaction = TreeCompaction.Block;
    private TreeCompaction _AltCompaction = TreeCompaction.Block;
  }

  public class Model : TreeModel<NodeData, int, object> { }
  public class NodeData : Model.NodeData {
    public Size Size { get; set; }
    public Brush Fill { get; set; }
  }
}
