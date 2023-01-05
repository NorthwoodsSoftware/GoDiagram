/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace Demo.Samples.LocalView {
  public partial class LocalView : DemoControl {
    private Diagram _FullDiagram;
    private Diagram _LocalDiagram;

    private Part _Highlighter;

    public LocalView() {
      InitializeComponent();
      _LocalDiagram = localDiagram.Diagram;
      _FullDiagram = fullDiagram.Diagram;

      Setup();

      newTreeBtn.Click += (e, obj) => SetupDiagram();

      desc1.MdText = DescriptionReader.Read("Samples.LocalView.md");
    }

    // make the corresponding node in the full diagram to that selected in the local diagram selected,
    // then call showLocalOnFullClick to update the local diagram
    private void showLocalOnLocalClick() {
      var selectedLocal = localDiagram.Diagram.Selection.FirstOrDefault();
      if (selectedLocal != null) {
        fullDiagram.Diagram.Select(fullDiagram.Diagram.FindPartForKey((selectedLocal.Data as NodeData).Key));
      }
    }

    private void showLocalOnFullClick() {

      var _node = _FullDiagram.Selection.FirstOrDefault();
      if (_node is Node node) {
        // make sure the selected node is in the viewport
        _FullDiagram.ScrollToRect(node.ActualBounds);
        // move the large yellow node behind the selected node to highlight it
        _Highlighter.Location = node.Location;
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
        _LocalDiagram.Model = model;
        // select the node at the diagram's focus
        var selectedLocal = _LocalDiagram.FindPartForKey((node.Data as NodeData).Key);
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
      _FullDiagram.Model = new Model {
        NodeDataSource = nodeDataSource
      };
    }

    private void Setup() {
      _FullDiagram.InitialAutoScale = AutoScale.UniformToFill; // automatically scale down to show whole tree
      _FullDiagram.MaxScale = 0.25;
      _FullDiagram.ContentAlignment = Spot.Center; // center the tree in the viewport
      _FullDiagram.IsReadOnly = true; // don't allow user to change the diagram
      _FullDiagram.Layout = new TreeLayout {
        Angle = 90,
        Sorting = TreeSorting.Ascending
      };
      _FullDiagram.MaxSelectionCount = 1; // only one node may be selected at a time in each diagram
      _FullDiagram.ChangedSelection += (obj, e) => showLocalOnFullClick();

      _LocalDiagram.AutoScale = AutoScale.UniformToFill;
      _LocalDiagram.ContentAlignment = Spot.Center;
      _LocalDiagram.IsReadOnly = true;
      _LocalDiagram.Layout = new TreeLayout() {
        Angle = 90,
        Sorting = TreeSorting.Ascending
      };
      _LocalDiagram.LayoutCompleted += (obj, e) => {
        var sel = e.Diagram.Selection.FirstOrDefault();
        if (sel != null) _LocalDiagram.ScrollToRect(sel.ActualBounds);
      };
      _LocalDiagram.MaxSelectionCount = 1;
      _LocalDiagram.ChangedSelection += (obj, e) => showLocalOnLocalClick();

      // define a node template shared by both diagrams
      var myNodeTemplate = new Node(PanelType.Auto) {
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
      _FullDiagram.NodeTemplate = myNodeTemplate;
      _LocalDiagram.NodeTemplate = myNodeTemplate;

      // define a basic link template, not selectable, shared by both diagrams
      var myLinkTemplate = new Link {
        Routing = LinkRouting.Normal,
        Selectable = false
      }.Add(
        new Shape { StrokeWidth = 1 }
      );
      _FullDiagram.LinkTemplate = myLinkTemplate;
      _LocalDiagram.LinkTemplate = myLinkTemplate;

      // create the full tree diagram
      SetupDiagram();

      // create a part in the background of the full diagram to highlight the selected node
      _Highlighter = new Part(PanelType.Auto) {
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
      _FullDiagram.Add(_Highlighter);

      // Start by focusing the diagrams on the node at the top of the tree.
      // Wait until the tree has been laid out before selecting the root node.
      _FullDiagram.InitialLayoutCompleted += (obj, e) => {
        var node0 = _FullDiagram.FindPartForKey(1);
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
