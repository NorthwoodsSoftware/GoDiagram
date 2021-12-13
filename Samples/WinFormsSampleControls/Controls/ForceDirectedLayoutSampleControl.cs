using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;

namespace WinFormsSampleControls.ForceDirectedLayoutSample {
  [ToolboxItem(false)]
  public partial class ForceDirectedLayoutSampleControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    // begin ForceDirectedLayout properties
    private int _MinNodes = 20;
    private int _MaxNodes = 100;
    private int _MinChildren = 1;
    private int _MaxChildren = 10;

    private int _MaxIter = 100;
    private double _Epsilon = 1;
    private double _Infinity = 1000;
    private Size _ArrangementSpacing = new(100, 100);

    private double _Charge = 150;
    private double _Mass = 0;

    private double _Stiffness = 0.05;
    private double _Length = 50;
    // end ForceDirectedLayout properties

    // begin public data bindings

    public ForceDirectedLayoutSampleControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      txtMinNodes.Leave += (e, obj) => MinNodes(txtMinNodes.Text);
      txtMaxNodes.Leave += (e, obj) => MaxNodes(txtMaxNodes.Text);
      txtMinChildren.Leave += (e, obj) => MinChildren(txtMinChildren.Text);
      txtMaxChildren.Leave += (e, obj) => MaxChildren(txtMaxChildren.Text);

      maxIterations.Leave += (e, obj) => MaxIter(maxIterations.Text);
      txtEpsilon.Leave += (e, obj) => Epsilon(txtEpsilon.Text);
      txtInfinity.Leave += (e, obj) => Infinity(txtInfinity.Text);
      txtArrangementSpacing.Leave += (e, obj) => ArrangementSpacing(txtArrangementSpacing.Text);

      electricalCharge.Leave += (e, obj) => Charge(electricalCharge.Text);
      gravitationalMass.Leave += (e, obj) => Mass(gravitationalMass.Text);
      
      springStiffness.Leave += (e, obj) => Stiffness(springStiffness.Text);
      springLength.Leave += (e, obj) => Length(springLength.Text);

      generateTree.Click += (e, obj) => _RebuildGraph();

      goWebBrowser1.Html = @"
          <p>
            For information on <b>ForceDirectedLayout</b> and its properties, see the <a>ForceDirectedLayout</a> documentation page.
          </p>
       ";
    }
    private void Setup() {

      myDiagram = diagramControl1.Diagram;

      myDiagram.InitialAutoScale = AutoScaleType.Uniform; // zoom to make everything fit in the viewport
      myDiagram.Layout = new ForceDirectedLayout(); // use custom layout

      myDiagram.Model = new Model();

      // define the Node template
      myDiagram.NodeTemplate = new Node(PanelLayoutSpot.Instance) {
        // make sure the Node.location is different from the node.position
        LocationSpot = Spot.Center
      }.Bind("Text").Add(
        new Shape {
          Fill = "lightgray",
          Stroke = null,
          DesiredSize = new Size(30, 30)
        }.Bind("Fill"),
        new TextBlock().Bind("Text")
      );

      // define the Link template
      myDiagram.LinkTemplate = new Link {
        Selectable = false
      }.Add(new Shape {
        StrokeWidth = 3,
        Stroke = "#333"
      });

      // generate a tree using the default values
      _RebuildGraph();
      }
    

    private void _RebuildGraph() {
      _GenerateTree();
    }

    private void _GenerateTree() {
      myDiagram = diagramControl1.Diagram;
      
      myDiagram.StartTransaction("generateTree");
      // replace the diagram's model's nodeDataSource
      _GenerateNodes();
      // replace the diagram's model's linkDataSource
      _GenerateLinks();
      // perform a diagram layout with the latest parameters
      _RedoLayout();
      myDiagram.CommitTransaction("generateTree");
    }

    // Creates a random number of randomly colored nodes.
    private void _GenerateNodes() {
      var rand = new Random();
      var nodeArray = new List<NodeData>();
      var min = _MinNodes;
      var max = _MaxNodes;
      if (double.IsNaN(min) || min < 0) min = 0;
      if (double.IsNaN(max) || max < min) max = min;
      var numNodes = rand.Next(min, max + 1); // random number of Nodes between min and max
      for (var i = 0; i < numNodes; i++) {
        nodeArray.Add(new NodeData {
          Key = i,
          Text = i.ToString(),
          Fill = Brush.RandomColor()
        });
      }

      // randomize the node data
      for (var i = 0; i < numNodes; i++) {
        var swap = rand.Next(numNodes);
        var temp = nodeArray[swap];
        nodeArray[swap] = nodeArray[i];
        nodeArray[i] = temp;
      }

      // set the NodeDataSource to this array of objects
      myDiagram.Model.NodeDataSource = nodeArray;
    }

    // Takes the random collection of nodes and creates a random
    // tree with them. Respects the minimum and maximum number of links
    // from each node. (The minimum can be disregarded if we run out of nodes to link to)
    private void _GenerateLinks() {
      var rand = new Random();
      myDiagram = diagramControl1.Diagram;
      var min = _MinChildren;
      var max = _MaxChildren;

      if (myDiagram.Nodes.Count < 2) return;
      if (double.IsNaN(min) || min < 1) min = 1;
      if (double.IsNaN(max) || max < min) max = min;
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
      (myDiagram.Model as Model).LinkDataSource = linkArray; // need to cast to at least GraphLinksModel to see LinkDataSource
    }

    // Update the layout from the controls.
    // Changing the properties will invalidate the layout
    private void _RedoLayout() {
      myDiagram = diagramControl1.Diagram;

      myDiagram.StartTransaction("changed Layout");
      var lay = myDiagram.Layout as ForceDirectedLayout;

      lay.MaxIterations = _MaxIter;
      lay.EpsilonDistance = _Epsilon;
      lay.InfinityDistance = _Infinity;

      lay.ArrangementSpacing = _ArrangementSpacing;

      lay.DefaultElectricalCharge = _Charge;
      lay.DefaultGravitationalMass = _Mass;
      lay.DefaultSpringStiffness = _Stiffness;
      lay.DefaultSpringLength = _Length;

      myDiagram.CommitTransaction("changed Layout");
    }

      // The minimum number of Nodes in the Layout.
    private void MinNodes(string minNodes) {
      if (double.TryParse(minNodes.Trim(), out double i)) {
        _MinNodes = Convert.ToInt32(i);
      }
    }

    // The maximum number of Nodes in the Layout.
    private void MaxNodes(string maxNodes) {
      if (double.TryParse(maxNodes.Trim(), out double i)) {
        _MaxNodes = Convert.ToInt32(i);
      }
    }

    // The minimum number of children of each Node.
    private void MinChildren(string minChildren) {
      if (double.TryParse(minChildren.Trim(), out double i)) {
        _MinChildren = Convert.ToInt32(i);
      }
    }

    // The maximum number of children of each Node.
    private void MaxChildren(string maxChildren) {
      if (double.TryParse(maxChildren.Trim(), out double i)) {
        _MaxChildren = Convert.ToInt32(i);
      }
    }

    // The maximum number of iterations performed in the ForceDirectedLayout.
    private void MaxIter(string maxIter) {
      if (double.TryParse(maxIter.Trim(), out double i)) {
        _MaxIter = Convert.ToInt32(i);
      }
    }

    // The step size in each iteration of the ForceDirectedLayout.
    private void Epsilon(string epsilon) {
      if(double.TryParse(epsilon.Trim(), out double i)) {
        _Epsilon = i;
      }
    }

    // Return the maximum value used by the ForceDirectedLayout.
    private void Infinity(string infinity) {
      if (double.TryParse(infinity.Trim(), out double i)) {
        _Infinity = i;
      }
    }

    // Get or set the minimum spacing between nodes in the ForceDirectedLayout.
    private void ArrangementSpacing(string arrangementSpacing) {
      var lay = diagramControl1.Diagram.Layout as ForceDirectedLayout;
      string trimmed = arrangementSpacing.Trim();

      // Case: one double
      if (trimmed.IndexOf(" ") == -1) {
        if (double.TryParse(trimmed, out double i)) {
          _ArrangementSpacing = Northwoods.Go.Size.Parse(i + " 0");
        }
      }
      // Case: two doubles
      else {
        string xSize = trimmed.Substring(0, trimmed.IndexOf(" ") - 0);
        string ySize = trimmed.Substring(trimmed.LastIndexOf(" ") + 1);
        if (double.TryParse(xSize, out double x) && double.TryParse(ySize, out double y)) {
          _ArrangementSpacing = Northwoods.Go.Size.Parse(xSize + " " + ySize);
        }
      }
      _ArrangementSpacing = new Size(0,0);
    }

    // The electrical charge value for nodes in the ForceDirectedLayout.
    private void Charge(string charge) {
      if (double.TryParse(charge.Trim(), out double i)) {
        _Charge = i;
      }
    }

    // The gravitational mass between nodes in the ForceDirectedLayout.
    private void Mass(string mass) {
      if (double.TryParse(mass.Trim(), out double i)) {
        _Mass = i;
      }
    }

    // The spring stiffness (velocity-based term) in the ForceDirectedLayout.
    private void Stiffness(string stiffness) {
      if (double.TryParse(stiffness.Trim(), out double i)) {
        _Stiffness = i;
      }
    }

    // The spring length (velocity-based term) in the ForceDirectedLayout.
    private void Length(string length) {
      if (double.TryParse(length.Trim(), out double i)) {
        _Length = i;
      }
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
