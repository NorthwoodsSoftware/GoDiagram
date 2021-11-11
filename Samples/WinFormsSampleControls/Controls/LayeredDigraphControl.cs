using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;

namespace WinFormsSampleControls.LayeredDigraph {
  [ToolboxItem(false)]
  public partial class LayeredDigraphControl : System.Windows.Forms.UserControl {

    private Diagram myDiagram;

    //begin LayeredDigraphLayout properties
    private int _MinNodes = 20;
    private int _MaxNodes = 100;

    private double _Direction = 0;

    private double _LayerSpacing = 25;
    private double _ColumnSpacing = 25;

    private LayeredDigraphCycleRemove _CycleRemove = LayeredDigraphCycleRemove.DepthFirst;
    private LayeredDigraphLayering _Layering = LayeredDigraphLayering.OptimalLinkLength;
    private LayeredDigraphInit _Initialize = LayeredDigraphInit.DepthFirstOut;
    private LayeredDigraphAggressive _Aggressive = LayeredDigraphAggressive.Less;

    private bool _PackMedian = true;
    private bool _PackStraighten = true;
    private bool _PackExpand = true;

    private bool _SetsPortSpots = true;
    //end LayeredDigraphLayout properties

    public LayeredDigraphControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      right.CheckedChanged += (e, obj) =>  Direction = 0;
      down.CheckedChanged += (e, obj) => Direction = 90;
      left.CheckedChanged += (e, obj) => Direction = 180;
      up.CheckedChanged += (e, obj) => Direction = 270;

      txtMinNodes.Leave += (e, obj) => MinNodes(txtMinNodes.Text);
      txtMaxNodes.Leave += (e, obj) => MaxNodes(txtMaxNodes.Text);
      txtLayerSpacing.Leave += (e, obj) => LayerSpacing(txtLayerSpacing.Text);
      txtColumnSpacing.Leave += (e, obj) => ColumnSpacing(txtColumnSpacing.Text);

      cycleDepthFirst.CheckedChanged += (e, obj) => CycleRemove = LayeredDigraphCycleRemove.DepthFirst;
      cycleGreedy.CheckedChanged += (e, obj) => CycleRemove = LayeredDigraphCycleRemove.Greedy;

      layerOptimalLinkLength.CheckedChanged += (e, obj) => Layering = LayeredDigraphLayering.OptimalLinkLength;
      layerLongestPathSource.CheckedChanged += (e, obj) => Layering = LayeredDigraphLayering.LongestPathSource;
      layerLongestPathSink.CheckedChanged += (e, obj) => Layering = LayeredDigraphLayering.LongestPathSink;

      initDepthFirstOut.CheckedChanged += (e, obj) => Initialize = LayeredDigraphInit.DepthFirstOut;
      initDepthFirstIn.CheckedChanged += (e, obj) => Initialize = LayeredDigraphInit.DepthFirstIn;
      initNaive.CheckedChanged += (e, obj) => Initialize = LayeredDigraphInit.Naive;

      aggressiveNone.CheckedChanged += (e, obj) => Aggressive = LayeredDigraphAggressive.None;
      aggressiveLess.CheckedChanged += (e, obj) => Aggressive = LayeredDigraphAggressive.Less;
      aggressiveMore.CheckedChanged += (e, obj) => Aggressive = LayeredDigraphAggressive.More;

      packMedian.CheckedChanged += (e, obj) => PackMedian = packMedian.Checked;
      packStraighten.CheckedChanged += (e, obj) => PackStraighten = packStraighten.Checked;
      packExpand.CheckedChanged += (e, obj) => PackExpand = packExpand.Checked;

      setPortSpots.CheckedChanged += (e, obj) => SetsPortSpots = setPortSpots.Checked;

      generateDiagraph.Click += (e, obj) => _RebuildGraph();


      goWebBrowser1.Html = @"
          <p>
            For information on <b>LayeredDigraphLayout</b> and its properties, see the <a>LayeredDigraphLayout</a> documentation page.
          </p>
";
    }

    private void Setup() {

      myDiagram = diagramControl1.Diagram;

      myDiagram.InitialAutoScale = AutoScaleType.UniformToFill;
      myDiagram.Layout = new LayeredDigraphLayout();

      myDiagram.Model = new Model();

      myDiagram.NodeTemplate = new Node(PanelLayoutSpot.Instance) {
        LocationSpot = Spot.Center
      }.Add(
       new Shape {
         Figure = "Rectangle",
         Fill = "lightgray", // the initial value, may be changed by data binding
         Stroke = null,
         DesiredSize = new Size(30, 30)
       }.Bind("Fill"),
       new TextBlock().Bind("Text")
      );

      // define the Link template to be minimal
      myDiagram.LinkTemplate = new Link {
        Selectable = false
      }.Add(
        new Shape {
          StrokeWidth = 3,
          Stroke = "#333"
        }
      );

      // generate a tree with the default values
      _RebuildGraph();
    }

    private void _RebuildGraph() {
      _GenerateDigraph();
    }

    private void _GenerateDigraph() {
      myDiagram = diagramControl1.Diagram;

      myDiagram.StartTransaction("generateDigraph");
      // replace the diagram's model's nodeDataSource
      _GenerateNodes();
      // replace the diagram's model's linkDataSource
      _GenerateLinks();
      // force a diagram layout
      _RedoLayout();
      myDiagram.CommitTransaction("generateDigraph");
    }

    private void _GenerateNodes() {
      var rand = new Random();
      var nodeArray = new List<NodeData>();
      // get the values from the fields and create a random number of nodes within the range
      var min = _MinNodes;
      var max = _MaxNodes;

      if (min < 0) min = 0;
      if (max < min) max = min;

      var numNodes = rand.Next(min, max + 1);

      for (var i = 1; i <= numNodes; i++) {
        nodeArray.Add(
          new NodeData {
            Key = i,
            Text = i.ToString(),
            Fill = Brush.RandomColor()
          }
        );
      }

      // randomize the node data
      for (var i = 0; i < nodeArray.Count; i++) {
        var swap = rand.Next(nodeArray.Count);
        var temp = nodeArray[swap];
        nodeArray[swap] = nodeArray[i];
        nodeArray[i] = temp;
      }

      myDiagram.Model.NodeDataSource = nodeArray;
    }

    private void _GenerateLinks() {
      var rand = new Random();
      myDiagram = diagramControl1.Diagram;

      if (myDiagram.Nodes.Count < 2) return;
      var linkArray = new List<LinkData>();
      var nit = myDiagram.Nodes;
      var nodes = new List<Node>();
      nodes.AddRange(nit);
      for (var i = 0; i < nodes.Count - 1; i++) {
        var from = nodes[i];
        var numto = Math.Floor(1 + rand.NextDouble() * 3 / 2);
        for (var j = 0; j < numto; j++) {
          var idx = Math.Floor(i + 5 + rand.NextDouble() * 10);
          if (idx >= nodes.Count) idx = i + rand.Next(nodes.Count - i) | 0;
          var to = nodes[(int)idx];
          linkArray.Add(new LinkData { From = (from.Data as NodeData).Key, To = (to.Data as NodeData).Key });
        }
      }

      (myDiagram.Model as Model).LinkDataSource = linkArray; // need to cast at least to graphlinksmodel to see LinkDataSource
    }

    private void _RedoLayout() {
      myDiagram = diagramControl1.Diagram;

      var lay = myDiagram.Layout as LayeredDigraphLayout;

      lay.Direction = Direction;
      lay.LayerSpacing = _LayerSpacing;
      lay.ColumnSpacing = _ColumnSpacing;

      lay.CycleRemoveOption = CycleRemove;
      lay.LayeringOption = Layering;
      lay.InitializeOption = Initialize;
      lay.AggressiveOption = Aggressive;

      //TODO implement pack option
      var packOption = 0;
      if (PackExpand) packOption |= 1;
      if (PackStraighten) packOption |= 2;
      if (PackMedian) packOption |= 4;
      lay.PackOption = (LayeredDigraphPack)packOption;

      lay.SetsPortSpots = SetsPortSpots;

      myDiagram.CommitTransaction("change Layout");
    }

    //begin public data bindings

    // The minimum number of nodes in the LayeredDigraphLayout.
    private void MinNodes(string minNodes) {
      if (double.TryParse(minNodes.Trim(), out double i)) {
        _MinNodes = Convert.ToInt32(i);
      }
    }

    // The maximum number of nodes in the LayeredDigraphLayout.
    private void MaxNodes(string maxNodes) {
      if(double.TryParse(maxNodes.Trim(), out double i)) {
        _MaxNodes = Convert.ToInt32(i);
      }
    }

    public double Direction {
      get {
        return _Direction;
      }
      set {
        if (_Direction != value) {
          _Direction = value;
          _RedoLayout();
        }
      }
    }

    private void LayerSpacing(string layerSpacing) {
      if (double.TryParse(layerSpacing.Trim(), out double i)) {
        _LayerSpacing = i;
      }
    }

    private void ColumnSpacing(string columnSpacing) {
      if(double.TryParse(columnSpacing.Trim(), out double i)) {
        _ColumnSpacing = i;
      }
    }

    public LayeredDigraphCycleRemove CycleRemove {
      get {
        return _CycleRemove;
      }
      set {
        if (_CycleRemove != value) {
          _CycleRemove = value;
          _RedoLayout();
        }
      }
    }

    public LayeredDigraphLayering Layering {
      get {
        return _Layering;
      }
      set {
        if (_Layering != value) {
          _Layering = value;
          _RedoLayout();
        }
      }
    }

    public LayeredDigraphInit Initialize {
      get {
        return _Initialize;
      }
      set {
        if (_Initialize != value) {
          _Initialize = value;
          _RedoLayout();
        }
      }
    }

    public LayeredDigraphAggressive Aggressive {
      get {
        return _Aggressive;
      }
      set {
        if (_Aggressive != value) {
          _Aggressive = value;
          _RedoLayout();
        }
      }
    }

    public bool PackMedian {
      get {
        return _PackMedian;
      }
      set {
        if (_PackMedian != value) {
          _PackMedian = value;
          _RedoLayout();
        }
      }
    }

    public bool PackStraighten {
      get {
        return _PackStraighten;
      }
      set {
        if (_PackStraighten != value) {
          _PackStraighten = value;
          _RedoLayout();
        }
      }
    }

    public bool PackExpand {
      get {
        return _PackExpand;
      }
      set {
        if (_PackExpand != value) {
          _PackExpand = value;
          _RedoLayout();
        }
      }
    }

    public bool SetsPortSpots {
      get {
        return _SetsPortSpots;
      }
      set {
        if (_SetsPortSpots != value) {
          _SetsPortSpots = value;
          _RedoLayout();
        }
      }
    }
  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }

  public class NodeData : Model.NodeData {
    public Brush Fill { get; set; }
  }

  public class LinkData : Model.LinkData { }
}
