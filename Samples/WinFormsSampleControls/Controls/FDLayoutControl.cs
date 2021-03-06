/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;

namespace WinFormsSampleControls.FDLayout {
  [ToolboxItem(false)]
  public partial class FDLayoutControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public FDLayoutControl() {
      InitializeComponent();
      myDiagram = diagramControl1.Diagram;

      Setup();

      goWebBrowser1.Html = @"
          <p>
            For information on <b>ForceDirectedLayout</b> and its properties, see the <a>ForceDirectedLayout</a> documentation page.
          </p>
       ";
    }

    private void Setup() {
      myDiagram.InitialAutoScale = AutoScale.Uniform; // zoom to make everything fit in the viewport
      myDiagram.Layout = new ForceDirectedLayout(); // use custom layout

      // define the Node template
      myDiagram.NodeTemplate =
        new Node("Spot") {
            // make sure the Node.location is different from the node.position
            LocationSpot = Spot.Center
          }
          .Bind("Text")
          .Add(
            new Shape {
                Fill = "lightgray",
                Stroke = null,
                DesiredSize = new Size(30, 30)
              }
              .Bind("Fill"),
            new TextBlock().Bind("Text")
          );

      // define the Link template
      myDiagram.LinkTemplate =
        new Link { Selectable = false }
          .Add(new Shape { StrokeWidth = 3, Stroke = "#333" });

      myDiagram.Model = new Model();

      // generate a tree using the default values
      _RebuildGraph();
    }


    private void _RebuildGraph() {
      if (!int.TryParse(minNodes.Text.Trim(), out var min)) min = 20;
      if (!int.TryParse(maxNodes.Text.Trim(), out var max)) max = 100;
      if (!int.TryParse(minChil.Text.Trim(), out var minC)) minC = 1;
      if (!int.TryParse(maxChil.Text.Trim(), out var maxC)) maxC = 10;

      _GenerateTree(min, max, minC, maxC);
    }

    private void _GenerateTree(int min, int max, int minC, int maxC) {
      myDiagram.StartTransaction("generateTree");
      // replace the diagram's model's nodeDataSource
      _GenerateNodes(min, max);
      // replace the diagram's model's linkDataSource
      _GenerateLinks(minC, maxC);
      // perform a diagram layout with the latest parameters
      _Layout();
      myDiagram.CommitTransaction("generateTree");
    }

    // Creates a random number of randomly colored nodes.
    private void _GenerateNodes(int min, int max) {
      var rand = new Random();
      var nodeList = new List<NodeData>();
      if (min < 0) min = 0;
      if (max < min) max = min;
      var numNodes = rand.Next(min, max + 1);  // random number of Nodes between min and max
      for (var i = 1; i <= numNodes; i++) {
        nodeList.Add(new NodeData {
          Key = i,
          Text = i.ToString(),
          Fill = Brush.RandomColor()
        });
      }

      // randomize the node data
      for (var i = 0; i < numNodes; i++) {
        var swap = rand.Next(numNodes);
        var temp = nodeList[swap];
        nodeList[swap] = nodeList[i];
        nodeList[i] = temp;
      }

      // set the NodeDataSource to this list of objects
      myDiagram.Model.NodeDataSource = nodeList;
    }

    // Takes the random collection of nodes and creates a random tree with them.
    // Respects the minimum and maximum number of links from each node.
    // (The minimum can be disregarded if we run out of nodes to link to)
    private void _GenerateLinks(int min, int max) {
      var rand = new Random();
      myDiagram = diagramControl1.Diagram;

      if (myDiagram.Nodes.Count < 2) return;
      if (min < 1) min = 1;
      if (max < min) max = min;
      var linkArray = new List<LinkData>();
      // make two lists of nodes to keep track of where links already exist
      var nit = myDiagram.Nodes;
      var nodes = new List<Node>();
      nodes.AddRange(nit);
      var available = new List<Node>();
      available.AddRange(nodes);
      for (var i = 0; i < nodes.Count; i++) {
        var next = nodes[i];
        available.Remove(next);
        var children = rand.Next(min, max + 1);
        for (var j = 1; j <= children; j++) {
          if (available.Count == 0) break;
          var to = available[0];
          available.Remove(to);
          // get keys from the Node.text strings
          var nextKey = int.Parse(next.Text);
          var toKey = int.Parse(to.Text);
          linkArray.Add(new LinkData {
            From = nextKey, To = toKey
          });
        }
      }
      (myDiagram.Model as Model).LinkDataSource = linkArray;
    }

    // Update the layout from the controls.
    // Changing the properties will invalidate the layout
    private void _Layout() {
      if (myDiagram.Layout is not ForceDirectedLayout lay) return;

      myDiagram.StartTransaction("changed Layout");

      if (!int.TryParse(maxIter.Text.Trim(), out var iters)) iters = 100;
      lay.MaxIterations = iters;
      if (!double.TryParse(epsilon.Text.Trim(), out var ep)) ep = 1;
      lay.EpsilonDistance = ep;
      if (!double.TryParse(infinity.Text.Trim(), out var inf)) inf = 1000;
      lay.InfinityDistance = inf;

      lay.ArrangementSpacing = Northwoods.Go.Size.Parse(arrangement.Text);

      if (!double.TryParse(charge.Text.Trim(), out var chg)) chg = 150;
      lay.DefaultElectricalCharge = chg;
      if (!double.TryParse(epsilon.Text.Trim(), out var ms)) ms = 0;
      lay.DefaultGravitationalMass = ms;
      if (!double.TryParse(stiffness.Text.Trim(), out var stf)) stf = 0.05;
      lay.DefaultSpringStiffness = stf;
      if (!double.TryParse(length.Text.Trim(), out var len)) len = 50;
      lay.DefaultSpringLength = len;

      myDiagram.CommitTransaction("changed Layout");
    }

    private void button1_Click(object sender, EventArgs e) {
      _RebuildGraph();
    }

    private void _PropertyChanged(object sender, EventArgs e) {
      _Layout();
    }

    public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }
    public class NodeData : Model.NodeData {
      public Brush Fill { get; set; }
    }
    public class LinkData : Model.LinkData { }

    // define a custom ForceDirectedLayout for this sample
    public class DemoForceDirectedLayout : ForceDirectedLayout {
      public override ForceDirectedNetwork MakeNetwork(IEnumerable<Part> coll = null) {
        // call base method for standard behavior
        var net = base.MakeNetwork(coll);
        foreach (var vertex in net.Vertexes) {
          var node = vertex.Node;
          if (node != null) vertex.IsFixed = node.IsSelected;
        }
        return net;
      }
    }
  }
}
