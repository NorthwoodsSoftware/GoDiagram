/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.Linq;

using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;

namespace Demo.Samples.CLayout {
  public partial class CLayout : DemoControl {
    private Diagram _Diagram;

    public CLayout() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      _InitControls();
      generateBtn.Click += (s, e) => _RebuildGraph();

      desc1.MdText = DescriptionReader.Read("Samples.CLayout.md");

      AfterLoad(Setup);
    }

    private void Setup() {
      _Diagram.InitialAutoScale = AutoScale.UniformToFill;
      _Diagram.Layout = new CircularLayout {
        Comparer = CircularVertex.SmartComparer
      };

      _Diagram.NodeTemplate =
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

      _Diagram.LinkTemplate =
        new Link { Selectable = false }
          .Add(new Shape { StrokeWidth = 3, Stroke = "#333" });

      _Diagram.Model = new Model();

      _RebuildGraph();
    }

    private void _RebuildGraph() {
      if (!int.TryParse(numNodes.Text.Trim(), out var num)) num = 16;
      if (!double.TryParse(width.Text.Trim(), out var w)) w = 25;
      if (!double.TryParse(height.Text.Trim(), out var h)) h = 25;
      if (!int.TryParse(minLinks.Text.Trim(), out var minL)) minL = 1;
      if (!int.TryParse(maxLinks.Text.Trim(), out var maxL)) maxL = 2;


      GenerateCircle(num, w, h, minL, maxL, _GetChecked(randSizes), _GetChecked(circ), _GetChecked(cyclic));
    }

    private void GenerateCircle(int numNodes, double width, double height, int minLinks, int maxLinks, bool randSizes, bool circ, bool cyclic) {
      _Diagram.StartTransaction("GenerateCircle");
      // replace the diagram's model's NodeDataSource
      GenerateNodeData(numNodes, width, height, randSizes, circ);
      // replace the diagram's model's NodeDataSource
      GenerateLinkData(minLinks, maxLinks, cyclic);
      // force a diagram layout
      _Layout();
      _Diagram.CommitTransaction("GenerateCircle");
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

      _Diagram.Model.NodeDataSource = nodeArray;
    }

    private void GenerateLinkData(int min, int max, bool cyclic) {
      if (_Diagram.Nodes.Count < 2) return;

      var rand = new Random();
      var linkArray = new List<LinkData>();

      var nodes = _Diagram.Nodes.ToList();
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
      (_Diagram.Model as Model).LinkDataSource = linkArray;
    }

    private void _Layout() {
      if (_Diagram.Layout is not CircularLayout lay) return;
      _Diagram.StartTransaction("change layout");

      if (!double.TryParse(radius.Text, out var rad)) rad = double.NaN;
      lay.Radius = rad;
      if (!double.TryParse(aspectRatio.Text, out var aspect)) aspect = 1;
      lay.AspectRatio = aspect;
      if (!double.TryParse(startAngle.Text, out var start)) start = 0;
      lay.StartAngle = start;
      if (!double.TryParse(sweepAngle.Text, out var sweep)) sweep = 360;
      lay.SweepAngle = sweep;
      if (!double.TryParse(spacing.Text, out var spc)) spc = 1;
      lay.Spacing = spc;

      lay.Arrangement = (CircularArrangement)Enum.Parse(typeof(CircularArrangement), (string)arrangement.SelectedItem);
      lay.NodeDiameterFormula = _DiamFormula;
      lay.Direction = (CircularDirection)Enum.Parse(typeof(CircularDirection), (string)direction.SelectedItem);
      lay.Sorting = (CircularSorting)Enum.Parse(typeof(CircularSorting), (string)sorting.SelectedItem);

      _Diagram.CommitTransaction("change layout");
    }

    private void button1_Click(object sender, EventArgs e) {
      _RebuildGraph();
    }

    private CircularNodeDiameterFormula _DiamFormula = CircularNodeDiameterFormula.Pythagorean;
  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }
  public class NodeData : Model.NodeData {
    public string Figure { get; set; }
    public Size Size { get; set; }
    public Brush Fill { get; set; }
  }
  public class LinkData : Model.LinkData { }
}
