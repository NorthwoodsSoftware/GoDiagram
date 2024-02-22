/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Layouts.Extensions;
using Northwoods.Go.Models;

namespace Demo.Samples.Virtualized {
  [ToolboxItem(false)]
  public partial class VirtualizedControl : DemoControl {
    private Diagram myDiagram;
    private Model myWholeModel;
    private Part myLoading;
    private Quadtree<NodeData> _WholeQuadTree;
    private readonly string[] _LinkColors = new string[4] { "black", "green", "blue", "red" };

    public VirtualizedControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"
   <p>
    This uses a <a>GraphLinksModel</a> but not any <a>Layout</a>.
    It demonstrates the virtualization of Links as well as simple Nodes.
  </p>
";

    }

    // Don't load the graph until the control has rendered;
    // allows loading indicator to be shown as graph loads and ensures timing
    protected override void OnLoad(EventArgs e) {
      base.OnLoad(e);

      // Allow the loading indicator to be shown,
      // but allow objects added in load to also be considered part of the initial Diagram.
      // If you are not going to add temporary initial Parts, don't call DelayInitialization.
      myDiagram.DelayInitialization(_Load);
    }

    private void Setup() {
      // The Diagram just shows what should be visible in the viewport.
      // Its model does NOT include node data for the whole graph, but only that
      // which might be visible in the viewport.
      myDiagram = diagramControl1.Diagram;
      myDiagram.InitialDocumentSpot = Spot.Center;
      myDiagram.InitialViewportSpot = Spot.Center;
      myDiagram.MaxSelectionCount = 1;
      myDiagram.AnimationManager.IsEnabled = false;

      // Assume there's no layout -- all data.bounds are provided
      myDiagram.Layout = new Layout {
        IsInitial = false,
        IsOngoing = false
      }; // never invalidates

      // Define the template for Nodes, used by virtualization.
      myDiagram.NodeTemplate =
        new Node("Auto") {
          IsLayoutPositioned = false, // optimization
          Width = 70, Height = 70, // in cooperation with the load function, below
          ToolTip = Builder.Make<Adornment>("ToolTip").Add(
              new TextBlock { Margin = 3 }
                .Bind("Text", "", (d, _) => {
                  var data = (NodeData)d;
                  return $"Key: {data.Key}\nBounds: {data.Bounds}\nProp1: {data.Prop1}\nProp2: {data.Prop2}";
                }))
          }
          .Add(
            new Shape("Rectangle").Bind("Fill", "Color"),
            new TextBlock { Margin = 2 }.Bind("Text", "Color")
          )
          .Bind("Position", "Bounds",
            (b, _) => ((Rect)b).Position,
            (p, d, _) => {
              var pt = (Point)p;
              var data = (NodeData)d;
              return new Rect(pt.X, pt.Y, data.Bounds.Width, data.Bounds.Height);
            }
          );

      // Define the template for Links
      myDiagram.LinkTemplate =
        new Link {
            IsLayoutPositioned = false // optimization
          }
          .Add(
            new Shape { StrokeWidth = 2 }
              .Bind("Stroke", "State", s => _LinkColors[(int)s]),
            new Shape { ToArrow = "OpenTriangle", StrokeWidth = 2 }
              .Bind("Stroke", "State", s => _LinkColors[(int)s])
          );



      // This model includes all of the data
      myWholeModel = new Model(); // must match the model used by the Diagram, below
      _WholeQuadTree = new Quadtree<NodeData>();

      // Do not set myDiagram.model = myWholeModel -- that would create a zillion Nodes and Links!
      // In the future Diagram may have built-in support for virtualization.
      // For now, we have to implement virtualization ourselves by having the Diagram's model
      // be different than the "real" model.
      myDiagram.Model = new Model(); // this only holds nodes that should be in the viewport, must match the model, above

      // for now, we have to implement virtualization ourselves
      myDiagram.IsVirtualized = true;
      myDiagram.ViewportBoundsChanged += _OnViewportChanged;

      myDiagram.ModelChanged += _OnModelChanged;
      (myDiagram.Model as Model).MakeUniqueKeyFunction = _VirtualUniqueKey; // ensure uniqueness in MyWholeModel

      // This is a status message
      myLoading =
        new Part() {  // this has to set the location or position explicitly
            Location = new Point(0, 0)
          }
          .Add(
            new TextBlock("loading...") {
              Stroke = "red", Font = new Font("Segoe UI", 20, FontUnit.Point)
            }
          );

      // temporarily add the status indicator
      myDiagram.Add(myLoading);
    }

    private void _Load(Diagram d) {
      var rand = new Random();

      // create a lot of data for the myWholeModel
      var total = 123456;
      var sqrt = (int)Math.Sqrt(total);
      var data = new List<NodeData>();
      var ldata = new List<LinkData>();
      _WholeQuadTree.Clear();
      for (var i = 0; i < total; i++) {
        var nd = new NodeData {
          Key = i + 1, // this node's data key
          Color = Brush.RandomColor(), // the node's color
          //!!!???@@@ this needs to be customized to account for your chosen Node template
          Bounds = new Rect(i % sqrt * 140, i / sqrt * 140, 70, 70),
          Text = "Node " + (i + 1).ToString(),
          Prop1 = rand.NextDouble() * 100,
          Prop2 = rand.NextDouble() * 100
        };
        data.Add(nd);
        _WholeQuadTree.Add(nd, nd.Bounds);

        if (i > 0 && i % sqrt > 0) { // link sequential nodes
          ldata.Add(new LinkData {
            From = i,
            To = i + 1,
            State = rand.Next(0, 4)
          });
        }

        if (i > sqrt) { // link nodes vertically
          ldata.Add(new LinkData {
            From = i - sqrt,
            To = i,
            State = rand.Next(0, 4)
          });
        }
      }

      myWholeModel.NodeDataSource = data;
      myWholeModel.LinkDataSource = ldata;

      d.Remove(myLoading);

      // there's no virtualized layout to perform, but we still
      // can't depend on regular bounds computation that depends on all Nodes existing
      d.FixedBounds = _ComputeDocumentBounds();
    }

    // The following functions implement virtualization of the Diagram
    // Assume data.bounds is a Rect of the area occupied by the Node in document coordinates.

    // It's not good enough to ensure keys are unique in the limited model that is myDiagram.Model --
    // need to ensure uniqueness in MyWholeModel.
    private int _VirtualUniqueKey(Model<NodeData, int, object> model, NodeData data) {
      myWholeModel.MakeNodeDataKeyUnique(data);
      return myWholeModel.GetKeyForNodeData(data);
    }

    // The normal mechanism for determining the size of the document depends on all of the
    // Nodes and Links existing, so we need to use a function that depends only on the model data.
    private Rect _ComputeDocumentBounds() {
      var b = new Rect();
      var ndata = myWholeModel.NodeDataSource;
      foreach (var d in ndata) {
        if (!d.Bounds.IsReal()) continue;
        if (b.IsEmpty()) {
          b = d.Bounds;
        } else {
          b = b.Union(d.Bounds);
        }
      }
      return b;
    }

    // As the user scrolls or zooms, make sure the Parts (Nodes and Links) exist in the viewport.
    private void _OnViewportChanged(object obj, DiagramEvent e) {
      var diagram = e.Diagram;
      // make sure there are Nodes for each node data that is in the viewport
      // or that is connected to such a Node
      var viewb = diagram.ViewportBounds; // the new viewportBounds
      var model = diagram.Model;

      var oldskips = diagram.SkipsUndoManager;
      diagram.SkipsUndoManager = true;

      //?? this does NOT remove Nodes or Links that are outside of the viewport

      Rect b;
      var ndata = _WholeQuadTree.Intersecting(viewb);
      foreach (var n in ndata) {
        if (model.ContainsNodeData(n)) continue;
        if (!n.Bounds.IsReal()) continue;
        if (n.Bounds.Intersects(viewb)) {
          _AddNode(diagram, myWholeModel, n);
        }
      }

      if (model is Model m) {
        var ldata = myWholeModel.LinkDataSource;
        foreach (var l in ldata) {
          if (m.ContainsLinkData(l)) continue;

          var fromkey = myWholeModel.GetFromKeyForLinkData(l);
          if (fromkey == 0) continue; // make sure there isn't a real node with key 0
          var from = myWholeModel.FindNodeDataForKey(fromkey);
          if (from == null || !from.Bounds.IsReal()) continue;

          var tokey = myWholeModel.GetToKeyForLinkData(l);
          if (tokey == 0) continue;
          var to = myWholeModel.FindNodeDataForKey(tokey);
          if (to == null || !to.Bounds.IsReal()) continue;

          b = from.Bounds;
          b = b.Union(to.Bounds);
          if (b.Intersects(viewb)) {
            // also make sure both connected nodes are present,
            // so that link routing is authentic
            _AddNode(diagram, myWholeModel, from);
            _AddNode(diagram, myWholeModel, to);
            m.AddLinkData(l);
            var link = diagram.FindLinkForData(l);
            if (link != null) {
              // do this now to avoid delayed routing outside of transaction
              link.UpdateRoute();
            }
          }
        }
      }

      diagram.SkipsUndoManager = oldskips;

      _UpdateCounts();
    }

    private void _AddNode(Diagram diagram, Model wholeModel, NodeData data) {
      var model = diagram.Model;
      if (model.ContainsNodeData(data)) return;
      model.AddNodeData(data);
      var n = diagram.FindNodeForData(data);
      if (n != null) n.EnsureBounds();
    }

    private void _OnModelChanged(object obj, ChangedEvent e) {
      if (e.Model.SkipsUndoManager) return;
      if (e.Change == ChangeType.Property) {
        if (e.PropertyName == "Bounds") {
          _WholeQuadTree.Move(e.Object as NodeData, ((Rect)e.NewValue).X, ((Rect)e.NewValue).Y);
        }
      } else if (e.Change == ChangeType.Insert) {
        if (e.PropertyName == "NodeDataSource") {
          var nd = e.NewValue as NodeData;
          myWholeModel.AddNodeData(nd);
          _WholeQuadTree.Add(nd, nd.Bounds);
        } else if (e.PropertyName == "LinkDataSource") {
          myWholeModel.AddLinkData(e.NewValue as LinkData);
        }
      } else if (e.Change == ChangeType.Remove) {
        if (e.PropertyName == "NodeDataSource") {
          var nd = e.OldValue as NodeData;
          myWholeModel.RemoveNodeData(nd);
          _WholeQuadTree.Remove(nd);
        } else if (e.PropertyName == "LinkDataSource") {
          myWholeModel.RemoveLinkData(e.OldValue as LinkData);
        }
      }
    }
    // end of virtualized Diagram

    private void _UpdateCounts() {
      lblNodes.Text = "Node data in Model: " + myWholeModel.NodeDataSource.Count() + ". Actual Nodes in Diagram: " + myDiagram.Nodes.Count();
      lblLinks.Text = "Link data in Model: " + myWholeModel.LinkDataSource.Count() + ". Actual Links in Diagram: " + myDiagram.Links.Count();
    }
  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }

  public class NodeData : Model.NodeData {
    public Rect Bounds { get; set; }
    public string Color { get; set; }
    public double Prop1 { get; set;}
    public double Prop2 { get; set; }
  }

  public class LinkData : Model.LinkData {
    public int State { get; set; }
  }

}
