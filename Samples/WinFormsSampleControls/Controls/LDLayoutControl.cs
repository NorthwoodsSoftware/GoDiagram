using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;
using System.Windows.Forms;

namespace WinFormsSampleControls.LDLayout {
  [ToolboxItem(false)]
  public partial class LDLayoutControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public LDLayoutControl() {
      InitializeComponent();
      myDiagram = diagramControl1.Diagram;

      Setup();

      goWebBrowser1.Html = @"
          <p>
            For information on <b>LayeredDigraphLayout</b> and its properties, see the <a>LayeredDigraphLayout</a> documentation page.
          </p>
";
    }

    private void Setup() {
      myDiagram.InitialAutoScale = AutoScale.UniformToFill;
      myDiagram.Layout = new LayeredDigraphLayout();

      myDiagram.NodeTemplate =
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
      myDiagram.LinkTemplate =
        new Link { Selectable = false }
          .Add(new Shape { StrokeWidth = 3, Stroke = "#333" });

      myDiagram.Model = new Model();

      // generate a tree with the default values
      _RebuildGraph();
    }

    private void _RebuildGraph() {
      if (!int.TryParse(minNodesTB.Text.Trim(), out var minNodes)) minNodes = 20;
      if (!int.TryParse(maxNodesTB.Text.Trim(), out var maxNodes)) maxNodes = 100;

      _GenerateDigraph(minNodes, maxNodes);
    }

    private void _GenerateDigraph(int minNodes, int maxNodes) {
      myDiagram.StartTransaction("generateDigraph");
      // replace the diagram's model's NodeDataSource
      _GenerateNodes(minNodes, maxNodes);
      // replace the diagram's model's LinkDataSource
      _GenerateLinks();
      // force a diagram layout
      _Layout();
      myDiagram.CommitTransaction("generateDigraph");
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

      myDiagram.Model.NodeDataSource = nodeList;
    }

    private void _GenerateLinks() {
      if (myDiagram.Nodes.Count < 2) return;
      var rand = new Random();
      var linkList = new List<LinkData>();
      var nodes = new List<Node>(myDiagram.Nodes);
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

      (myDiagram.Model as Model).LinkDataSource = linkList;
    }

    private void _Layout() {
      var lay = myDiagram.Layout as LayeredDigraphLayout;

      lay.Direction = _Direction;
      if (!double.TryParse(layerSpacingTB.Text.Trim(), out var layerSpacing)) layerSpacing = 25;
      lay.LayerSpacing = layerSpacing;
      if (!double.TryParse(columnSpacingTB.Text.Trim(), out var columnSpacing)) columnSpacing = 25;
      lay.ColumnSpacing = columnSpacing;

      lay.CycleRemoveOption = _CycleRemove;
      lay.LayeringOption = _Layering;
      lay.InitializeOption = _Init;
      lay.AggressiveOption = _Aggressive;

      var packOption = LayeredDigraphPack.None;
      if (expandCB.Checked) packOption |= LayeredDigraphPack.Expand;
      if (straightenCB.Checked) packOption |= LayeredDigraphPack.Straighten;
      if (medianCB.Checked) packOption |= LayeredDigraphPack.Median;
      lay.PackOption = packOption;

      lay.SetsPortSpots = setsPortSpotsCB.Checked;

      myDiagram.CommitTransaction("change Layout");
    }

    private void button1_Click(object sender, EventArgs e) {
      _RebuildGraph();
    }

    private void _PropertyChanged(object sender, EventArgs e) {
      if (sender is RadioButton rb) {
        if (rb.Checked) {
          _RadioChanged(rb);
        } else {
          return;  // ignore radio button changes that aren't checked
        }
      }
      _Layout();
    }

    private int _Direction = 0;
    private LayeredDigraphCycleRemove _CycleRemove = LayeredDigraphCycleRemove.DepthFirst;
    private LayeredDigraphLayering _Layering = LayeredDigraphLayering.OptimalLinkLength;
    private LayeredDigraphInit _Init = LayeredDigraphInit.DepthFirstOut;
    private LayeredDigraphAggressive _Aggressive = LayeredDigraphAggressive.Less;
    private void _RadioChanged(RadioButton rb) {
      if (rb.Parent == direction) {  // angle radio changed
        switch (rb.Name) {
          case "rightRB": _Direction = 0; break;
          case "downRB": _Direction = 90; break;
          case "leftRB": _Direction = 180; break;
          case "upRB": _Direction = 270; break;
          default: _Direction = 0; break;
        }
      } else if (rb.Parent == cycleRemove) {  // alt angle radio changed
        switch (rb.Name) {
          case "depthFirstRB": _CycleRemove = LayeredDigraphCycleRemove.DepthFirst; break;
          case "greedyRB": _CycleRemove = LayeredDigraphCycleRemove.Greedy; break;
          default: _CycleRemove = LayeredDigraphCycleRemove.DepthFirst; break;
        }
      } else if (rb.Parent == layering) {  // compaction radio changed
        switch (rb.Name) {
          case "optimalLinkLengthRB": _Layering = LayeredDigraphLayering.OptimalLinkLength; break;
          case "longestPathSourceRB": _Layering = LayeredDigraphLayering.LongestPathSource; break;
          case "longestPathSinkRB": _Layering = LayeredDigraphLayering.LongestPathSink; break;
          default: _Layering = LayeredDigraphLayering.OptimalLinkLength; break;
        }
      } else if (rb.Parent == initialize) {  // alt compaction radio changed
        switch (rb.Name) {
          case "depthFirstOutRB": _Init = LayeredDigraphInit.DepthFirstOut; break;
          case "depthFirstInRB": _Init = LayeredDigraphInit.DepthFirstIn; break;
          case "naiveRB": _Init = LayeredDigraphInit.Naive; break;
          default: _Init = LayeredDigraphInit.DepthFirstOut; break;
        }
      } else if (rb.Parent == aggressive) {
        switch (rb.Name) {
          case "noneRB": _Aggressive = LayeredDigraphAggressive.None; break;
          case "lessRB": _Aggressive = LayeredDigraphAggressive.Less; break;
          case "moreRB": _Aggressive = LayeredDigraphAggressive.More; break;
          default: _Aggressive = LayeredDigraphAggressive.Less; break;
        }
      }
    }

    public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }
    public class NodeData : Model.NodeData {
      public Brush Fill { get; set; }
    }
    public class LinkData : Model.LinkData { }
  }
}
