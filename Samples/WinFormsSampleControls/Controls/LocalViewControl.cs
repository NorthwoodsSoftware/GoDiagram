using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.LocalView {
  [ToolboxItem(false)]
  public partial class LocalViewControl : System.Windows.Forms.UserControl {
    private Diagram myFullDiagram;
    private Diagram myLocalDiagram;

    private Part highlighter;

    public LocalViewControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      btnCreateNewTree.Click += (e, obj) => SetupDiagram();

      goWebBrowser1.Html = @"
  <p>
  This sample includes two diagrams, the one on top showing a full tree and the one below
  focusing on a specific node in the tree and those nodes that are logically ""near"" it.
  When the selection changes in either diagram, the lower diagram changes its focus to the selected node.
  To show which node in the full tree is selected,
  a large yellow highlighter part employing a radial <a>Brush</a> is placed in the background layer of the upper diagram behind the selected node.
  The Create New Tree button will randomly generate a new <a>TreeModel</a> to be used by the diagrams.
   </p>
      
   <p>
  Although it is not demonstrated in this sample,
  one could well use very simple templates for Nodes and for Links in the top Diagram.
  This would make the top Diagram more efficient to construct when there are very many more nodes.
  And one could use more detailed templates in the bottom Diagram,
  where there is more room to show information for each node.
  </p>
";
    }

    // make the corresponding node in the full diagram to that selected in the local diagram selected,
    // then call showLocalOnFullClick to update the local diagram
    private void showLocalOnLocalClick() {
      var selectedLocal = diagramControl2.Diagram.Selection.FirstOrDefault();
      if (selectedLocal != null) {
        diagramControl1.Diagram.Select(diagramControl1.Diagram.FindPartForKey((selectedLocal.Data as NodeData).Key));
      }
    }

    private void showLocalOnFullClick() {

      var _node = myFullDiagram.Selection.FirstOrDefault();
      if (_node is Node node) {
        // make sure the selected node is in the viewport
        myFullDiagram.ScrollToRect(node.ActualBounds);
        // move the large yellow node behind the selected node to highlight it
        highlighter.Location = node.Location;
        // create a new model for the local Diagram
        var model = new Model();
        // add the selected node and its children and grandchildren to the local diagram
        var nearby = node.FindTreeParts(3); // three levels of the tree
        // add parent and grandparent
        var parent = node.FindTreeParentNode();
        if (parent != null) {
          model.AddNodeData(parent.Data as NodeData);          
          var grandparent = parent.FindTreeParentNode();
          if (grandparent != null) {
            model.AddNodeData(grandparent.Data as NodeData);
          }
        }
        // create the model using the same node data as in myFullDiagram.Model
        
        foreach (var n in nearby) {
          if (n is Node m) model.AddNodeData(m.Data as NodeData);
        }
        myLocalDiagram.Model = model;
        // select the node at the diagram's focus
        var selectedLocal = myLocalDiagram.FindPartForKey((node.Data as NodeData).Key);
        if (selectedLocal != null) selectedLocal.IsSelected = true;
      }
    }

    // Create the tree model containing 100 nodes, with each node having a variable number of children
    private void SetupDiagram() {
      var total = 100; // default to 100 nodes
      var nodeDataSource = new List<NodeData>();
      for (var i = 0; i < total; i++) {
        nodeDataSource.Add(new NodeData {
          Key = nodeDataSource.Count + 1,
          Color = Brush.RandomColor()
        });
      }
      var j = 1;
      var rand = new Random();
      for (var i = 1; i < total; i++) {
        var data = nodeDataSource[i];
        data.Parent = j;
        if (rand.NextDouble() < 0.3) j++; // this controls the likelihood that there are enough children
      }
      diagramControl1.Diagram.Model = new Model {
        NodeDataSource = nodeDataSource
      };
    }

    private void Setup() {

      myLocalDiagram = diagramControl2.Diagram;
      myFullDiagram = diagramControl1.Diagram;

      myFullDiagram.InitialAutoScale = AutoScaleType.UniformToFill; // automatically scale down to show whole tree
      myFullDiagram.MaxScale = 0.25;
      myFullDiagram.ContentAlignment = Spot.Center; // center the tree in the viewport
      myFullDiagram.IsReadOnly = true; // don't allow user to change the diagram
      myFullDiagram.Layout = new TreeLayout {
        Angle = 90,
        Sorting = TreeSorting.Ascending
      };
      myFullDiagram.MaxSelectionCount = 1; // only one node may be selected at a time in each diagram
      myFullDiagram.ChangedSelection += (obj, e) => showLocalOnFullClick();

      myLocalDiagram.AutoScale = AutoScaleType.UniformToFill;
      myLocalDiagram.ContentAlignment = Spot.Center;
      myLocalDiagram.IsReadOnly = true;
      myLocalDiagram.Layout = new TreeLayout() {
        Angle = 90,
        Sorting = TreeSorting.Ascending
      };
      myLocalDiagram.LayoutCompleted += (obj, e) => {
        var sel = e.Diagram.Selection.First();
        if (sel != null) myLocalDiagram.ScrollToRect(sel.ActualBounds);
      };
      myLocalDiagram.MaxSelectionCount = 1;
      myLocalDiagram.ChangedSelection += (obj, e) => showLocalOnLocalClick();

      // define a node template shared by both diagrams
      var myNodeTemplate = new Node(PanelLayoutAuto.Instance) {
        LocationSpot = Spot.Center
      }.Bind("Text", "Key", (k, _) => "" + k)
       .Add(
        new Shape("Rectangle") {
          Stroke = null
        }.Bind("Fill", "Color"),
        new TextBlock {
          Margin = 5
        }.Bind("Text", "Key", (k, _) => "node" + k)
      );
      myFullDiagram.NodeTemplate = myNodeTemplate;
      myLocalDiagram.NodeTemplate = myNodeTemplate;

      // define a basic link template, not selectable, shared by both diagrams
      var myLinkTemplate = new Link {
        Routing = LinkRouting.Normal,
        Selectable = false
      }.Add(
        new Shape { StrokeWidth = 1 }
      );
      myFullDiagram.LinkTemplate = myLinkTemplate;
      myLocalDiagram.LinkTemplate = myLinkTemplate;

      // create the full tree diagram
      SetupDiagram();

      // create a part in the background of the full diagram to highlight the selected node
      highlighter = new Part(PanelLayoutAuto.Instance) {
        LayerName = "Background",
        Selectable = false,
        IsInDocumentBounds = false,
        LocationSpot = Spot.Center
      }.Add(
        new Shape("Ellipse") {
          Fill = new Brush(new RadialGradientPaint(new Dictionary<float, string> { { 0, "yellow" }, { 1, "white" } })),
          Stroke = null,
          DesiredSize = new Size(400, 400)
        }
      );
      myFullDiagram.Add(highlighter);

      // Start by focusing the diagrams on the node at the top of the tree.
      // Wait until the tree has been laid out before selecting the root node.
      myFullDiagram.InitialLayoutCompleted += (obj, e) => {
        var node0 = myFullDiagram.FindPartForKey(1);
        if (node0 != null) node0.IsSelected = true;
        showLocalOnFullClick();
      };
    }

  }

  public class Model : TreeModel<NodeData, int, object> { }

  public class NodeData : Model.NodeData {
    public string Color { get; set; }
  }

}
