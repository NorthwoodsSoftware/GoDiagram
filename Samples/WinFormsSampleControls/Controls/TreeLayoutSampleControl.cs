using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;
using System.Linq;

namespace WinFormsSampleControls.TreeLayoutSample {
  [ToolboxItem(false)]
  public partial class TreeLayoutSampleControl : System.Windows.Forms.UserControl {

    private Diagram myDiagram;

    /** begin TreeLayout properties **/
    private TreeStyle _Style = TreeStyle.Layered;
    private TreeLayerStyle _LayerStyle = TreeLayerStyle.Individual;

    private int _MinNodes = 20;
    private int _MaxNodes = 100;
    private int _MinChildren = 1;
    private int _MaxChildren = 3;

    private bool _RandomSizes = false;

    private double _Angle = 0;
    private TreeAlignment _Alignment = TreeAlignment.CenterChildren;

    private double _NodeSpacing = 20;
    private double _NodeIndent = 0;
    private double _NodeIndentPastParent = 0;
    private double _LayerSpacing = 50;
    private double _LayerSpacingParentOverlap = 0;

    private TreeSorting _Sorting = TreeSorting.Forwards;
    private TreeCompaction _Compaction = TreeCompaction.Block;

    private double _BreadthLimit = 0;
    private double _RowSpacing = 25;
    private double _RowIndent = 10;
    private bool _SetsPortSpot = true;
    private bool _SetsChildPortSpot = true;

    // alternates if the _Style != TreeStyle.Layered
    private double _AltAngle = 0;
    private TreeAlignment _AltAlignment = TreeAlignment.CenterChildren;

    private double _AltNodeSpacing = 20;
    private double _AltNodeIndent = 0;
    private double _AltNodeIndentPastParent = 0;
    private double _AltLayerSpacing = 50;
    private double _AltLayerSpacingParentOverlap = 0;

    private TreeSorting _AltSorting = TreeSorting.Forwards;
    private TreeCompaction _AltCompaction = TreeCompaction.Block;

    private double _AltBreadthLimit = 0;
    private double _AltRowSpacing = 25;
    private double _AltRowIndent = 10;
    private bool _AltSetsPortSpot = true;
    private bool _AltSetsChildPortSpot = true;
    //end tree properties
    public TreeLayoutSampleControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      generateTree.Click += (s, e) => _RebuildGraph();

      treeStyle.DataSource = Enum.GetNames(typeof(TreeStyle));
      treeStyle.SelectedIndexChanged += (s, e) => {
        Style = (TreeStyle)Enum.Parse(typeof(TreeStyle), (string)treeStyle.SelectedItem);
      };

      layerStyle.DataSource = Enum.GetNames(typeof(TreeLayerStyle));
      layerStyle.SelectedIndexChanged += (s, e) => {
        LayerStyle = (TreeLayerStyle)Enum.Parse(typeof(TreeLayerStyle), (string)layerStyle.SelectedItem);
      };

      txtMinNodes.Leave += (s, e) => MinNodes(txtMinNodes.Text);
      txtMaxNodes.Leave += (s, e) => MaxNodes(txtMaxNodes.Text);
      txtMinChildren.Leave += (s, e) => MinChildren(txtMinChildren.Text);
      txtMaxChildren.Leave += (s, e) => MaxChildren(txtMaxChildren.Text);

      randomSizes.CheckedChanged += (s, e) => RandomSizes = randomSizes.Checked;

      defaultRight.CheckedChanged += (s, e) => Angle = 0;
      defaultDown.CheckedChanged += (s, e) => Angle = 90;
      defaultLeft.CheckedChanged += (s, e) => Angle = 180;
      defaultUp.CheckedChanged += (s, e) => Angle = 270;

      defaultAlignment.DataSource = Enum.GetNames(typeof(TreeAlignment));
      defaultAlignment.SelectedIndexChanged += (s, e) => {
        Alignment = (TreeAlignment)Enum.Parse(typeof(TreeAlignment), (string)defaultAlignment.SelectedItem);
      };

      defaultNodeSpacing.Leave += (s, e) => NodeSpacing(defaultNodeSpacing.Text);
      defaultNodeIndent.Leave += (s, e) => NodeIndent(defaultNodeIndent.Text);
      defaultNodeIndentPastParent.Leave += (s, e) => NodeIndentPastParent(defaultNodeIndentPastParent.Text);
      defaultLayerSpacing.Leave += (s, e) => LayerSpacing(defaultLayerSpacing.Text);
      defaultLayerSpacingParentOverlap.Leave += (s, e) => LayerSpacingParentOverlap(defaultLayerSpacingParentOverlap.Text);

      defaultSorting.DataSource = Enum.GetNames(typeof(TreeSorting));
      defaultSorting.SelectedIndexChanged += (s, e) => {
        Sorting = (TreeSorting)Enum.Parse(typeof(TreeSorting), (string)defaultSorting.SelectedItem);
      };

      defaultBlock.CheckedChanged += (s, e) => Compaction = TreeCompaction.Block; 
      defaultNone.CheckedChanged += (s, e) => Compaction = TreeCompaction.None;

      defaultBreadthLimit.Leave += (s, e) => BreadthLimit(defaultBreadthLimit.Text);
      defaultRowSpacing.Leave += (s, e) => RowSpacing(defaultBreadthLimit.Text);
      defaultRowIndent.Leave += (s, e) => RowIndent(defaultRowIndent.Text);

      defaultSetsPortSpot.CheckedChanged += (s, e) => SetsPortSpot = defaultSetsPortSpot.Checked;
      defaultSetsChildPortSpot.CheckedChanged += (s, e) => SetsChildPortSpot = defaultSetsChildPortSpot.Checked;

      alternateNodeSpacing.Leave += (s, e) => AltNodeSpacing(alternateNodeSpacing.Text);
      alternateNodeIndent.Leave += (s, e) => AltNodeIndent(alternateNodeIndent.Text);
      alternateNodeIndentPastParent.Leave += (s, e) => AltNodeIndentPastParent(alternateNodeIndentPastParent.Text);
      alternateLayerSpacing.Leave += (s, e) => AltLayerSpacing(alternateLayerSpacing.Text);
      alternateLayerSpacingParentOverlap.Leave += (s, e) => AltLayerSpacingParentOverlap(alternateLayerSpacingParentOverlap.Text);

      alternateRight.CheckedChanged += (s, e) => AltAngle = 0;
      alternateDown.CheckedChanged += (s, e) => AltAngle = 270;
      alternateLeft.CheckedChanged += (s, e) => AltAngle = 180;
      alternateUp.CheckedChanged += (s, e) => AltAngle = 90;

      alternateAlignment.DataSource = Enum.GetNames(typeof(TreeAlignment));
      alternateAlignment.SelectedIndexChanged += (s, e) => {
        AltAlignment = (TreeAlignment)Enum.Parse(typeof(TreeAlignment), (string)alternateAlignment.SelectedItem);
      };

      alternateSorting.DataSource = Enum.GetNames(typeof(TreeSorting));
      alternateSorting.SelectedIndexChanged += (s, e) => {
        AltSorting = (TreeSorting)Enum.Parse(typeof(TreeSorting), (string)alternateSorting.SelectedItem);
      };

      alternateBlock.CheckedChanged += (s, e) => AltCompaction = TreeCompaction.Block;
      alternateNone.CheckedChanged += (s, e) => AltCompaction = TreeCompaction.None;

      alternateBreadthLimit.Leave += (s, e) => AltBreadthLimit(alternateBreadthLimit.Text);
      alternateRowSpacing.Leave += (s, e) => AltRowSpacing(alternateBreadthLimit.Text);
      alternateRowIndent.Leave += (s, e) => AltRowIndent(alternateRowIndent.Text);

      alternateSetsPortSpot.CheckedChanged += (s, e) => AltSetsPortSpot = alternateSetsPortSpot.Checked;
      alternateSetsChildPortSpot.CheckedChanged += (s, e) => AltSetsChildPortSpot = alternateSetsChildPortSpot.Checked;

      goWebBrowser1.Html = @"
         <p>
      For information on <b>TreeLayout</b> and its properties, see the <a>TreeLayout</a> documentation page.
         </p>

";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      myDiagram.InitialAutoScale = AutoScaleType.UniformToFill;
      myDiagram.Layout = new TreeLayout {
        Comparer = TreeVertex.SmartComparer // have the comparer sort by numbers as well as letters
      }; // other properties are set by the layout function, defined below

      myDiagram.NodeTemplate = new Node(PanelLayoutSpot.Instance) {
        LocationSpot = Spot.Center
      }.Bind("Text").Add(
        new Shape {
          Figure = "Ellipse",
          Fill = "lightgray", // data binding may provide different value
          Stroke = null,
          DesiredSize = new Size(30, 30)
        }.Bind("DesiredSize", "Size").Bind("Fill"),
        new TextBlock().Bind("Text")
      );

      myDiagram.LinkTemplate = new Link() {
        Routing = LinkRouting.Orthogonal,
        Selectable = false
      }.Add(
        new Shape {
          StrokeWidth = 3,
          Stroke = "#333"
        }
      );

      _RebuildGraph();
    }

    // Creates a random number of randomly colored nodes.
    private List<NodeData> GenerateNodeData() {
      var nodeArray = new List<NodeData>();
      var rand = new Random();

      // Create a bunch of node data
      var min = _MinNodes;
      var max = _MaxNodes;

      if (double.IsNaN(min) || min < 0) min = 0;
      if (double.IsNaN(max) || max < min) max = min;

      var numNodes = rand.Next(min, max + 1);
      for (var i = 1; i <= numNodes; i++) {
        nodeArray.Add(new NodeData {
          Key = i, // the unique identifier
          // Parent is set by code below that assigns children
          Text = (i - 1).ToString(),
          Fill = Brush.RandomColor(),
          Size = RandomSizes ? new Size(rand.Next(20, 70), rand.Next(20, 70)) : new Size(30, 30)
        });
      }

      for (var i = 0; i < nodeArray.Count; i++) {
        var swap = rand.Next(nodeArray.Count);
        var temp = nodeArray[swap];
        nodeArray[swap] = nodeArray[i];
        nodeArray[i] = temp;
      }

      // Takes the random collection of node data and creates a random tree with them.
      // Respects the minimum and maximum number of links from each node.
      // The minimum can be disregarded fi we run out of nodes to link to.
      if (nodeArray.Count > 1) {
        // keep the Set of node data that do not yet have a parent
        var available = new HashSet<NodeData>();
        foreach (var n in nodeArray) available.Add(n);
        for (var i = 0; i < nodeArray.Count; i++) {
          var parent = nodeArray[i];
          available.Remove(parent);

          // assign some number of node data as children of this parent node data
          var children = rand.Next(_MinChildren, 1 + _MaxChildren);
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

      return nodeArray;
    }

    // Update the layout from the controls, and then perform the layout again
    private void _Layout() {
      myDiagram = diagramControl1.Diagram;

      myDiagram.StartTransaction("change Layout");
      var lay = myDiagram.Layout as TreeLayout;

      // set all properties of this TreeLayout based on bindings
      lay.TreeStyle = Style;
      lay.LayerStyle = LayerStyle;
      lay.Angle = Angle;

      lay.Alignment = Alignment;

      lay.NodeSpacing = _NodeSpacing;
      lay.NodeIndent = _NodeIndent;
      lay.NodeIndentPastParent = _NodeIndentPastParent;
      lay.LayerSpacing = _LayerSpacing;
      lay.LayerSpacingParentOverlap = _LayerSpacingParentOverlap;

      lay.Sorting = Sorting;
      lay.Compaction = Compaction;

      lay.BreadthLimit = _BreadthLimit;
      lay.RowSpacing = _RowSpacing;
      lay.RowIndent = _RowIndent;
      lay.SetsPortSpot = SetsPortSpot;
      lay.SetsChildPortSpot = SetsChildPortSpot;

      if (lay.TreeStyle != TreeStyle.Layered) {
        lay.AlternateAngle = AltAngle;

        lay.AlternateAlignment = AltAlignment;

        lay.AlternateNodeSpacing = _AltNodeSpacing;
        lay.AlternateNodeIndent = _AltNodeIndent;
        lay.AlternateNodeIndentPastParent = _AltNodeIndentPastParent;
        lay.AlternateLayerSpacing = _AltLayerSpacing;
        lay.AlternateLayerSpacingParentOverlap = _AltLayerSpacingParentOverlap;

        lay.AlternateSorting = AltSorting;
        lay.AlternateCompaction = AltCompaction;

        lay.AlternateBreadthLimit = _AltBreadthLimit;
        lay.AlternateRowSpacing = _AltRowSpacing;
        lay.AlternateRowIndent = _AltRowIndent;
        lay.AlternateSetsPortSpot = AltSetsPortSpot;
        lay.AlternateSetsChildPortSpot = AltSetsChildPortSpot;
      }
      myDiagram.CommitTransaction("change Layout");
    }

    private void _RebuildGraph() {
      myDiagram = diagramControl1.Diagram;

      var nodeDataSource = GenerateNodeData();
      myDiagram.Model = new Model {
        NodeDataSource = nodeDataSource
      };

      _Layout();
    }

    // Default Textbox methods
    private void MinNodes(string minNodes) {
      if (double.TryParse(minNodes.Trim(), out double i)) {
        _MinNodes = Convert.ToInt32(i);
        _Layout();
      }
    }

    private void MaxNodes(string maxNodes) {
      if (double.TryParse(maxNodes.Trim(), out double i)) {
        _MaxNodes = Convert.ToInt32(i);
        _Layout();
      }
    }

    private void MinChildren(string minChildren) {
      if (double.TryParse(minChildren.Trim(), out double i)) {
        _MinChildren = Convert.ToInt32(i);
        _Layout();
      }
    }

    private void MaxChildren(string maxChildren) {
      if (double.TryParse(maxChildren.Trim(), out double i)) {
        _MaxChildren = Convert.ToInt32(i);
        _Layout();
      }
    }

    private void NodeSpacing(string nodeSpacing) {
      if (double.TryParse(nodeSpacing.Trim(), out double i)) {
        _NodeSpacing = i;
        _Layout();
      }
    }

    private void NodeIndent(string nodeIndent) {
      if (double.TryParse(nodeIndent.Trim(), out double i)) {
        _NodeIndent = i;
        _Layout();
      }
    }

    private void NodeIndentPastParent(string nodeIndentPastParent) {
      if (double.TryParse(nodeIndentPastParent.Trim(), out double i)) {
        _NodeIndentPastParent = i;
        _Layout();
      }
    }

    private void LayerSpacing(string layerSpacing) {
      if (double.TryParse(layerSpacing.Trim(), out double i)) {
        _LayerSpacing = i;
        _Layout();
      }
    }

    private void LayerSpacingParentOverlap(string layerSpacingParentOverlap) {
      if (double.TryParse(layerSpacingParentOverlap.Trim(), out double i)) {
        _LayerSpacingParentOverlap = i;
        _Layout();
      }
    }

    private void BreadthLimit(string breadthLimit) {
      if (double.TryParse(breadthLimit.Trim(), out double i)) {
        _BreadthLimit = i;
        _Layout();
      }
    }

    private void RowSpacing(string rowSpacing) {
      if (double.TryParse(rowSpacing.Trim(), out double i)) {
        _RowSpacing = i;
        _Layout();
      }
    }

    private void RowIndent(string rowIndent) {
      if (double.TryParse(rowIndent.Trim(), out double i)) {
        _RowIndent = i;
        _Layout();
      }
    }

    // Alternate Textbox methods
    private void AltNodeSpacing(string altNodeSpacing) {
      if (double.TryParse(altNodeSpacing.Trim(), out double i)) {
        _AltNodeSpacing = i;
        _Layout();
      }
    }

    private void AltNodeIndent(string altNodeIndent) {
      if (double.TryParse(altNodeIndent.Trim(), out double i)) {
        _AltNodeIndent = i;
        _Layout();
      }
    }

    private void AltNodeIndentPastParent(string altNodeIndentPastParent) {
      if (double.TryParse(altNodeIndentPastParent.Trim(), out double i)) {
        _AltNodeIndentPastParent = i;
        _Layout();
      }
    }

    private void AltLayerSpacing(string altLayerSpacing) {
      if (double.TryParse(altLayerSpacing.Trim(), out double i)) {
        _AltLayerSpacing = i;
        _Layout();
      }
    }

    private void AltLayerSpacingParentOverlap(string altLayerSpacingParentOverlap) {
      if (double.TryParse(altLayerSpacingParentOverlap.Trim(), out double i)) {
        _AltLayerSpacingParentOverlap = i;
        _Layout();
      }
    }

    private void AltBreadthLimit(string altBreadthLimit) {
      if (double.TryParse(altBreadthLimit.Trim(), out double i)) {
        _AltBreadthLimit = i;
        _Layout();
      }
    }

    private void AltRowSpacing(string altRowSpacing) {
      if (double.TryParse(altRowSpacing.Trim(), out double i)) {
        _AltRowSpacing = i;
        _Layout();
      }
    }

    private void AltRowIndent(string altRowIndent) {
      if (double.TryParse(altRowIndent.Trim(), out double i)) {
        _AltRowIndent = i;
        _Layout();
      }
    }


    // begin exposed public tree properties
    public TreeStyle Style {
      get {
        return _Style;
      }
      set {
        if (_Style != value) {
          _Style = value;
          _Layout();
        }
      }
    }

    public TreeLayerStyle LayerStyle {
      get {
        return _LayerStyle;
      }
      set {
        if (_LayerStyle != value) {
          _LayerStyle = value;
          _Layout();
        }
      }
    }

    public bool RandomSizes {
      get {
        return _RandomSizes;
      }
      set {
        if (_RandomSizes != value) {
          _RandomSizes = value;
          _Layout();
        }
      }
    }

    public double Angle {
      get {
        return _Angle;
      }
      set {
        if (_Angle != value) {
          _Angle = value;
          _Layout();
        }
      }
    }

    public TreeAlignment Alignment {
      get {
        return _Alignment;
      }
      set {
        if (_Alignment != value) {
          _Alignment = value;
          _Layout();
        }
      }
    }

    public TreeSorting Sorting {
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

    public TreeCompaction Compaction {
      get {
        return _Compaction;
      }
      set {
        if (_Compaction != value) {
          _Compaction = value;
          _Layout();
        }
      }
    }

    public bool SetsPortSpot {
      get {
        return _SetsPortSpot;
      }
      set {
        if (_SetsPortSpot != value) {
          _SetsPortSpot = value;
          _Layout();
        }
      }
    }

    public bool SetsChildPortSpot {
      get {
        return _SetsChildPortSpot;
      }
      set {
        if (_SetsChildPortSpot != value) {
          _SetsChildPortSpot = value;
          _Layout();
        }
      }
    }
    public double AltAngle {
      get {
        return _AltAngle;
      }
      set {
        if (_AltAngle != value) {
          _AltAngle = value;
          _Layout();
        }
      }
    }

    public TreeAlignment AltAlignment {
      get {
        return _AltAlignment;
      }
      set {
        if (_AltAlignment != value) {
          _AltAlignment = value;
          _Layout();
        }
      }
    }

    public TreeSorting AltSorting {
      get {
        return _AltSorting;
      }
      set {
        if (_AltSorting != value) {
          _AltSorting = value;
          _Layout();
        }
      }
    }

    public TreeCompaction AltCompaction {
      get {
        return _AltCompaction;
      }
      set {
        if (_AltCompaction != value) {
          _AltCompaction = value;
          _Layout();
        }
      }
    }

    

    public bool AltSetsPortSpot {
      get {
        return _AltSetsPortSpot;
      }
      set {
        if (_AltSetsPortSpot != value) {
          _AltSetsPortSpot = value;
          _Layout();
        }
      }
    }

    public bool AltSetsChildPortSpot {
      get {
        return _AltSetsChildPortSpot;
      }
      set {
        if (_AltSetsChildPortSpot != value) {
          _AltSetsChildPortSpot = value;
          _Layout();
        }
      }
    }
    //end exposed public tree properties

    public class Model : TreeModel<NodeData, int, object> { }

    public class NodeData : Model.NodeData {
      public Size Size { get; set; }
      public Brush Fill { get; set; }
    }

    
  }
}
