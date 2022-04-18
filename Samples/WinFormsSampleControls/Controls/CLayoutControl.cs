/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.WinForms;
using Northwoods.Go.Tools;
using Northwoods.Go.Layouts;
using System.Linq;
using System.Windows.Forms;
using Northwoods.Go.Extensions;

namespace WinFormsSampleControls.CLayout {
  [ToolboxItem(false)]
  public partial class CLayoutControl : System.Windows.Forms.UserControl {

    private Diagram myDiagram;

    private double _Radius = double.NaN;
    private double _AspectRatio = 1;
    private double _StartAngle = 0;
    private double _SweepAngle = 360;
    private int _Spacing = 6;
    private CircularArrangement _Arrangement = CircularArrangement.ConstantSpacing;
    private CircularNodeDiameterFormula _DiameterFormula = CircularNodeDiameterFormula.Pythagorean;
    private CircularDirection _Direction = CircularDirection.Clockwise;
    private CircularSorting _Sorting = CircularSorting.Forwards;

    private static int numNodes = 16;
    private double width = 25;
    private double height = 25;
    private bool randSizes = true;
    private bool circ = false;
    private bool cyclic = false;
    private int minLinks = 1;
    private int maxLinks = 2;



    public CLayoutControl() {
      InitializeComponent();

      Setup();

      numOfNodes.Leave += (e, obj) => numNodes = int.Parse(numOfNodes.Text);
      xNodeSize.Leave += (e, obj) => width = double.Parse(xNodeSize.Text);
      yNodeSize.Leave += (e, obj) => height = double.Parse(yNodeSize.Text);

      randomSizes.CheckedChanged += (e, obj) => randSizes = randomSizes.Checked;
      circularNodes.CheckedChanged += (e, obj) => circ = circularNodes.Checked;
      simpleRing.CheckedChanged += (e, obj) => cyclic = simpleRing.Checked;

      minLinksFromNode.Leave += (e, obj) => minLinks = int.Parse(minLinksFromNode.Text);
      maxLinksFromNode.Leave += (e, obj) => maxLinks = int.Parse(maxLinksFromNode.Text);

      generateCircle.Click += (e, obj) => RebuildGraph();

      radius.Leave += (e, obj) => _Layout();
      aspectRatio.Leave += (e, obj) => _Layout();
      startAngle.Leave += (e, obj) => _Layout();
      sweepAngle.Leave += (e, obj) => _Layout();
      spacing.Leave += (e, obj) => _Layout();

      arrangement.DataSource = Enum.GetNames(typeof(CircularArrangement));
      arrangement.SelectedIndexChanged += (s, e) => { Arrangement = (CircularArrangement)Enum.Parse(typeof(CircularArrangement), (string)arrangement.SelectedItem); };

      pythagorean.CheckedChanged += (e, obj) => _DiameterFormula = CircularNodeDiameterFormula.Pythagorean;
      circular.CheckedChanged += (e, obj) => _DiameterFormula = CircularNodeDiameterFormula.Circular;

      direction.DataSource = Enum.GetNames(typeof(CircularDirection));
      direction.SelectedIndexChanged += (s, e) => { Direction = (CircularDirection)Enum.Parse(typeof(CircularDirection), (string)direction.SelectedItem); };

      sorting.DataSource = Enum.GetNames(typeof(CircularSorting));
      sorting.SelectedIndexChanged += (s, e) => { Sorting = (CircularSorting)Enum.Parse(typeof(CircularSorting), (string)sorting.SelectedItem); };

      goWebBrowser1.Html = @"
        <html>
        <head>
       </head>
        <body>
          <p>
            For information on <b>CircularLayout</b> and its properties, see the <a>CircularLayout</a> documentation page.
          </p>
        </body>
        </html>
";
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      myDiagram.InitialAutoScale = AutoScale.UniformToFill;
      myDiagram.Layout = new CircularLayout {
        Comparer = CircularVertex.SmartComparer
      };

      myDiagram.NodeTemplate = new Node(PanelLayoutSpot.Instance) {
        LocationSpot = Spot.Center
      }.Bind("Text").Add(
        new Shape {
          Figure = "Ellipse",
          Fill = "lightgray",
          Stroke = null,
          DesiredSize = new Size(30, 30),
        }.Bind("Figure").Bind("Fill").Bind("DesiredSize", "Size", Northwoods.Go.Size.Parse),
        new TextBlock().Bind("Text")
        );

      myDiagram.LinkTemplate = new Link() {
        Selectable = false
      }.Add(
        new Shape {
          StrokeWidth = 3,
          Stroke = "#333"
        }
        );

      RebuildGraph();



    }

    private void _Layout() {
      myDiagram.StartTransaction("change Layout");
      var lay = myDiagram.Layout as CircularLayout;

      lay.Radius = double.Parse(radius.Text);
      lay.AspectRatio = double.Parse(aspectRatio.Text);
      lay.StartAngle = double.Parse(startAngle.Text);
      lay.SweepAngle = double.Parse(sweepAngle.Text);
      lay.Spacing = int.Parse(spacing.Text);

      lay.Arrangement = Arrangement;
      lay.NodeDiameterFormula = NodeDiameterFormula;
      lay.Direction = Direction;
      lay.Sorting = Sorting;
      myDiagram.CommitTransaction("change Layout");
    }

    private List<NodeData> GenerateNodeData(int numNodes, double width, double height, bool randSizes, bool circ) {
      var nodeArray = new List<NodeData>();
      var rand = new Random();

      for (var i = 1; i <= numNodes; i++) {
        Size size;// prob not needed
        if (randSizes) {
          size = new Size(Math.Floor(rand.NextDouble() * (65 - width + 1)) + width, Math.Floor(rand.NextDouble() * (65 - height + 1)) + height);
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
          Size = Northwoods.Go.Size.Stringify(size) // burruhruh isnt string the input?
        });
      }

      for (var i = 0; i < nodeArray.Count; i++) {
        var swap = Math.Floor(rand.NextDouble() * nodeArray.Count);
        var temp = nodeArray[(int)swap];
        nodeArray[(int)swap] = nodeArray[i];
        nodeArray[i] = temp;
      }
      return nodeArray;
    }

    private void GenerateLinkData(int min, int max, bool cyclic) {
      var rand = new Random();
      var linkArray = new List<LinkData>();

      if (myDiagram.Nodes.Count < 2) return;

      var nodes = new List<NodeData>();
      foreach (var n in myDiagram.Nodes) {
        nodes.Add(n.Data as NodeData);
      }
      var num = myDiagram.Nodes.Count;
      if (cyclic) {
        for (var i = 1; i <= num; i++) {
          if (i >= num - 1) {
            linkArray.Add(new LinkData { From = i, To = 1 });
          } else {
            linkArray.Add(new LinkData { From = i, To = 1 });

          }
        }
      } else {
        if (min < 0) min = 0;
        if (max < min) max = min;
        for (var i = 0; i < num; i++) {
          var next = nodes[i];
          var children = Math.Floor(rand.NextDouble() * (max - min + 1)) + min;
          for (var j = 0; j < children; j++) {
            var to = nodes[(int)Math.Floor(rand.NextDouble() * num)];
            var nextKey = next.Key;
            var toKey = to.Key;
            if (nextKey != toKey) {
              linkArray.Add(new LinkData { From = nextKey, To = toKey });
            }
          }
        }
      }
      (myDiagram.Model as Model).LinkDataSource = linkArray;
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

    private void RebuildGraph() {
      var nodeDataSource = GenerateNodeData(numNodes, width, height, randSizes, circ);

      myDiagram.Model = new Model {
        NodeDataSource = nodeDataSource,
      };

      GenerateCircle(numNodes, width, height, minLinks, maxLinks, randSizes, circ, cyclic);
    }

    public double Radius {
      get {
        return _Radius;
      }
      set {
        if(_Radius != value) {
          _Radius = value;
          _Layout();
        }
      }
    }

    public double AspectRatio {
      get {
        return _AspectRatio;
      }
      set {
        if (_AspectRatio != value) {
          _AspectRatio = value;
          _Layout();
        }
      }
    }

    public double StartAngle {
      get {
        return _StartAngle;
      }
      set {
        if (_StartAngle != value) {
          _StartAngle = value;
          _Layout();
        }
      }
    }

    public double SweepAngle {
      get {
        return _SweepAngle;
      }
      set {
        if (_SweepAngle != value) {
          _SweepAngle = value;
          _Layout();
        }
      }
    }

    public int Spacing {
      get {
        return _Spacing;
      }
      set {
        if (_Spacing != value) {
          _Spacing = value;
          _Layout();
        }
      }
    }

    public CircularArrangement Arrangement {
      get {
        return _Arrangement;
      }
      set {
        if (_Arrangement != value) {
          _Arrangement = value;
          _Layout();
        }
      }
    }

    public CircularNodeDiameterFormula NodeDiameterFormula {
      get {
        return _DiameterFormula;
      }
      set {
        if (_DiameterFormula != value) {
          _DiameterFormula = value;
          _Layout();
        }
      }
    }

    public CircularDirection Direction {
      get {
        return _Direction;
      }
      set {
        if (_Direction != value) {
          _Direction = value;
          _Layout();
        }
      }
    }

    public CircularSorting Sorting {
      get {
        return _Sorting;
      }
      set {
        if (_Sorting != value) {
          _Sorting = value;
          _Layout();
        }
      }
    }

  }
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }

  public class NodeData : Model.NodeData {
    public string Figure { get; set; }
    public string Size { get; set; }
    public Brush Fill { get; set; }
  }

  public class LinkData : Model.LinkData { }
}
