/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;
using System.Linq;

namespace WinFormsSampleControls.CLayout {
  [ToolboxItem(false)]
  public partial class CLayoutControl : UserControl {
    private Diagram myDiagram;

    public CLayoutControl() {
      InitializeComponent();
      myDiagram = diagramControl1.Diagram;

      arrangement.DataSource = Enum.GetNames(typeof(CircularArrangement));
      direction.DataSource = Enum.GetNames(typeof(CircularDirection));
      sorting.DataSource = Enum.GetNames(typeof(CircularSorting));

      Setup();

      goWebBrowser1.Html = @"
        <p>
          For information on <b>CircularLayout</b> and its properties, see the <a>CircularLayout</a> documentation page.
        </p>";
    }

    private void Setup() {
      myDiagram.Model = new Model();
      myDiagram.InitialAutoScale = AutoScale.UniformToFill;
      myDiagram.Layout = new CircularLayout {
        Comparer = CircularVertex.SmartComparer
      };

      myDiagram.NodeTemplate =
        new Node("Spot") {
            LocationSpot = Spot.Center
          }
          .Bind("Text")
          .Add(
            new Shape("Ellipse") {
                Fill = "lightgray",
                Stroke = null,
                DesiredSize = new Size(30, 30),
              }
              .Bind("Figure")
              .Bind("Fill")
              .Bind("DesiredSize", "Size"),
            new TextBlock().Bind("Text")
          );

      myDiagram.LinkTemplate =
        new Link { Selectable = false }
          .Add(new Shape { StrokeWidth = 3, Stroke = "#333" });

      _RebuildGraph();
    }

    private void _RebuildGraph() {
      GenerateCircle((int)numNodes.Value, (double)width.Value, (double)height.Value, (int)minLinks.Value, (int)maxLinks.Value, randSizes.Checked, circ.Checked, cyclic.Checked);
    }

    private void GenerateCircle(int numNodes, double width, double height, int minLinks, int maxLinks, bool randSizes, bool circ, bool cyclic) {
      myDiagram.StartTransaction("GenerateCircle");
      // replace the diagram's model's NodeDataSource
      GenerateNodeData(numNodes, width, height, randSizes, circ);
      // replace the diagram's model's NodeDataSource
      GenerateLinkData(minLinks, maxLinks, cyclic);
      // force a diagram layout
      _Layout();
      myDiagram.CommitTransaction("GenerateCircle");
    }

    private void GenerateNodeData(int numNodes, double width, double height, bool randSizes, bool circ) {
      var nodeArray = new List<NodeData>();
      var rand = new Random();

      for (var i = 1; i <= numNodes; i++) {
        Size size;
        if (randSizes) {
          size = new Size(rand.Next((int)(65 - width + 1)) + width, rand.Next((int)(65 - height + 1)) + height);
        } else {
          size = new Size(width, height);
        }

        if (circ) size.Height = size.Width;

        var figure = "Rectangle";
        if (circ) figure = "Ellipse";

        nodeArray.Add(new NodeData {
          Key = i,
          Text = i.ToString(),
          Figure = figure,
          Fill = Brush.RandomColor(),
          Size = size
        });
      }

      for (var i = 0; i < nodeArray.Count; i++) {
        var swap = rand.Next(nodeArray.Count);
        var temp = nodeArray[swap];
        nodeArray[swap] = nodeArray[i];
        nodeArray[i] = temp;
      }

      myDiagram.Model.NodeDataSource = nodeArray;
    }

    private void GenerateLinkData(int min, int max, bool cyclic) {
      if (myDiagram.Nodes.Count < 2) return;

      var rand = new Random();
      var linkArray = new List<LinkData>();

      var nodes = myDiagram.Nodes.ToList();
      var num = nodes.Count;
      if (cyclic) {
        for (var i = 1; i <= num; i++) {
          if (i >= num) {
            linkArray.Add(new LinkData { From = i, To = 1 });
          } else {
            linkArray.Add(new LinkData { From = i, To = i + 1 });
          }
        }
      } else {
        if (min < 0) min = 0;
        if (max < min) max = min;
        for (var i = 0; i < num; i++) {
          var next = nodes[i];
          var children = rand.Next(max - min + 1) + min;
          for (var j = 1; j <= children; j++) {
            var to = nodes[rand.Next(num)];
            var nextKey = (int)next.Key;
            var toKey = (int)to.Key;
            if (nextKey != toKey) {
              linkArray.Add(new LinkData { From = nextKey, To = toKey });
            }
          }
        }
      }
      (myDiagram.Model as Model).LinkDataSource = linkArray;
    }

    private void _Layout() {
      if (myDiagram.Layout is not CircularLayout lay) return;
      myDiagram.StartTransaction("change Layout");

      if (!double.TryParse(radius.Text, out var rad)) rad = double.NaN;
      lay.Radius = rad;
      lay.AspectRatio = (double)aspectRatio.Value;
      lay.StartAngle = (double)startAngle.Value;
      lay.SweepAngle = (double)sweepAngle.Value;
      if (!double.TryParse(spacing.Text, out var spc)) spc = 1;
      lay.Spacing = spc;

      lay.Arrangement = (CircularArrangement)Enum.Parse(typeof(CircularArrangement), (string)arrangement.SelectedItem);
      lay.NodeDiameterFormula = _DiamFormula;
      lay.Direction = (CircularDirection)Enum.Parse(typeof(CircularDirection), (string)direction.SelectedItem);
      lay.Sorting = (CircularSorting)Enum.Parse(typeof(CircularSorting), (string)sorting.SelectedItem);
      myDiagram.CommitTransaction("change Layout");
    }

    private void button1_Click(object sender, EventArgs e) {
      _RebuildGraph();
    }

    private void _PropertyChanged(object sender, EventArgs e) {
      if (sender is RadioButton rb) {
        if (rb.Checked) {
          _DiamFormulaChanged(rb);
        } else {
          return;  // ignore radio button changes that aren't checked
        }
      }
      _Layout();
    }

    private CircularNodeDiameterFormula _DiamFormula = CircularNodeDiameterFormula.Pythagorean;
    private void _DiamFormulaChanged(RadioButton rb) {
      switch (rb.Text) {
        case "Pythagorean": _DiamFormula = CircularNodeDiameterFormula.Pythagorean; break;
        case "Circular": _DiamFormula = CircularNodeDiameterFormula.Circular; break;
        default: _DiamFormula = CircularNodeDiameterFormula.Pythagorean; break;
      }
    }
  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }
  public class NodeData : Model.NodeData {
    public string Figure { get; set; }
    public Size Size { get; set; }
    public Brush Fill { get; set; }
  }
  public class LinkData : Model.LinkData { }
}
