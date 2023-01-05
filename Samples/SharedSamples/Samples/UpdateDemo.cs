/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;
using System.Threading.Tasks;

namespace Demo.Samples.UpdateDemo {
  public partial class UpdateDemo : DemoControl {
    private Diagram blueDiagram;
    private Diagram greenDiagram;
    private Diagram undoDisplay;

    public UpdateDemo() {
      InitializeComponent();

      Setup();

      clearModelLogBtn.Click += (e, obj) => _ClearLog();
      undoBtn.Click += (e, obj) => blueDiagram.CommandHandler.Undo();
      redoBtn.Click += (e, obj) => blueDiagram.CommandHandler.Redo();

      desc1.MdText = DescriptionReader.Read("Samples.UpdateDemo.md");
    }

    private void Setup() {
      blueDiagram = diagramControl1.Diagram;
      greenDiagram = diagramControl2.Diagram;
      undoDisplay = diagramControl3.Diagram;

      // blue diagram properties
      blueDiagram.ToolManager.ClickCreatingTool.ArchetypeNodeData = new NodeData {
        Text = "node"
      };

      // blue diagram node template
      blueDiagram.NodeTemplate =
        new Node(PanelType.Auto).Bind(
          new Binding("Location", "Loc").MakeTwoWay()
        ).Add(
          new Shape {
            Figure = "RoundedRectangle",
            Fill = new Brush(new LinearGradientPaint(new Dictionary<float, string> {
                { 0, "#00ACED" },
                { 0.5f, "#00ACED" },
                { 1, "#0079A6" }
              })),
            Stroke = "#0079A6",
            PortId = "",
            Cursor = "pointer",  // the node's only port is the Shape
            FromLinkable = true,
            FromLinkableDuplicates = true,
            FromLinkableSelfNode = true,
            ToLinkable = true,
            ToLinkableDuplicates = true,
            ToLinkableSelfNode = true
          },
          new TextBlock {
            Margin = 3,
            Font = new Font("Arial", 10, Northwoods.Go.FontWeight.Bold),
            Stroke = "whitesmoke",
            Editable = true
          }.Bind(
            new Binding("Text").MakeTwoWay()
          )
        );

      // blue diagram link template
      blueDiagram.LinkTemplate =
        new Link {
          Curve = LinkCurve.Bezier,
          Adjusting = LinkAdjusting.Stretch,
          RelinkableFrom = true,
          RelinkableTo = true,
          Reshapable = true
        }.Add(
          new Shape { // the link shape
            StrokeWidth = 2,
            Stroke = "blue"
          },
          new Shape {  // the arrowhead
            ToArrow = "standard",
            Fill = "blue",
            Stroke = (Brush)null
          }
        );

      // green diagram properties
      greenDiagram.ToolManager.ClickCreatingTool.ArchetypeNodeData = new NodeData {
        Text = "node"
      };

      // green diagram node template
      greenDiagram.NodeTemplate =
        new Node(PanelType.Vertical).Bind(
          new Binding("Location", "Loc").MakeTwoWay()
        ).Add(
          new Shape {
            Figure = "Ellipse",
            Fill = "lightgreen",
            Width = 20,
            Height = 20,
            PortId = ""
          },
          new TextBlock {
            Margin = 3,
            Font = new Font("Georgia", 12, Northwoods.Go.FontWeight.Bold)
          }.Bind(
            new Binding("Text")
          )
        );

      // green diagram link template
      greenDiagram.LinkTemplate =
        new Link().Add(
          new Shape { // the link shape
            StrokeWidth = 2,
            Stroke = "#76C176"
          },
          new Shape { // the arrowhead
            ToArrow = "standard",
            Fill = "#76C176",
            Stroke = (Brush)null
          }
        );

      // model that will be represented in both diagrams simultaneously
      var model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = 1, Text = "Alpha", Loc = new Point(20, 20) },
          new NodeData { Key = 2, Text = "Beta", Loc = new Point(160, 120) }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = 1, To = 2 }
        }
      };

      // the two Diagrams share the same model
      blueDiagram.Model = model;
      greenDiagram.Model = model;

      // now turn on undo/redo
      model.UndoManager.IsEnabled = true;

      // **********************************************************
      // A third diagram is on this page to display the undo state.
      // It functions as a tree view, showing the Transactions
      // in the UndoManager history.
      // **********************************************************

      // undoDisplay properties
      undoDisplay.AllowMove = false;
      undoDisplay.MaxSelectionCount = 1;
      undoDisplay.InitialContentAlignment = Spot.TopLeft;
      undoDisplay.Layout = new TreeLayout {
        Alignment = TreeAlignment.Start,
        Angle = 0,
        Compaction = TreeCompaction.None,
        LayerSpacing = 16,
        LayerSpacingParentOverlap = 1,
        NodeIndentPastParent = 1.0,
        NodeSpacing = 0,
        SetsPortSpot = false,
        SetsChildPortSpot = false,
        ArrangementSpacing = new Size(2, 2)
      };
      undoDisplay.AnimationManager.IsEnabled = false;

      var treeExpanderButton =
        Builder.Make<Panel>("TreeExpanderButton").Set(
          new {
            Width = 14
          }
        );
      (treeExpanderButton.FindElement("ButtonBorder") as Shape).Fill = "whitesmoke";

      // undoDisplay node template
      undoDisplay.NodeTemplate =
        new Node().Add(
          treeExpanderButton,
          new Panel(PanelType.Horizontal) {
            Position = new Point(16, 0)
          }.Bind(
            new Binding("Background", "Color")
          ).Add(
            new TextBlock {
              Margin = 2
            }.Bind(
              new Binding("Text", "Text")
            )
          )
        );

      undoDisplay.LinkTemplate = new Link(); // not used

      var undoModel = new TreeModel {
        NodeDataSource = new List<TreeNodeData>()
      };
      undoDisplay.Model = undoModel;

      // ******************************************************
      // Add an undo listener to the main model
      // ******************************************************

      Node editToRedo = null;
      var editList = new List<Node>();

      async void ChangedEvent(object _, ChangedEvent e) {
        // do not display some uninteresting kinds of transaction notifications
        if (e.Change == ChangeType.Transaction) {
          if (e.PropertyName == "CommittingTransaction" || e.ModelChange == ModelChangeType.SourceChanged) return;
          // do not display any layout transactions
          if (e.OldValue as string == "Layout") return;
        }  // You will probably want to use e.IsTransactionFinished instead

        // Add entries into the log
        var changes = e.ToString();
        if (changes[0] != '*') changes = "  " + changes;
        _AddToLog(changes);

        // Modify the undoDisplay Diagram, the tree view
        if (e.PropertyName == "CommittedTransaction") {
          if (editToRedo != null) {
            // remove from the undo display diagram all nodes after editToRedo'
            if (editList.Count > 0) {
              for (var i = (editToRedo.Data as TreeNodeData).Index; i < editList.Count; i++) {
                // delete children
                foreach (var n in editList[i].FindTreeChildrenNodes()) {
                  (undoDisplay.Model as TreeModel).RemoveNodeData(n.Data as TreeNodeData);
                }
                (undoDisplay.Model as TreeModel).RemoveNodeData(editList[i].Data as TreeNodeData);
              }
              editList = editList.GetRange(0, (editToRedo.Data as TreeNodeData).Index);
              editToRedo = null;
            }
          }

          // delay the update of the undoDisplay tree, to catch the results of calls to Transaction.Optimize
          await Task.Delay(10);
          var tx = e.Object as Transaction;
          var txname = (tx != null ? tx.Name : "");
          var parentData = new TreeNodeData {
            Text = txname,
            Tag = e.Object != null ? e.Object.GetHashCode() : 0,
            Index = editList.Count
          };
          undoModel.AddNodeData(parentData);
          var parentKey = undoModel.GetKeyForNodeData(parentData);
          var parentNode = undoDisplay.FindNodeForKey(parentKey);
          editList.Add(parentNode);
          if (tx != null) {
            var allChanges = tx.Changes;
            var odd = true;
            foreach (var change in allChanges) {
              var childData = new TreeNodeData {
                Color = (odd ? "white" : "#E0FFED"),
                Text = change.ToString(),
                Parent = parentKey
              };
              undoModel.AddNodeData(childData);
              odd = !odd;
            }
            undoDisplay.CommandHandler.CollapseTree(parentNode);
          }
        } else if (e.PropertyName == "FinishedUndo" || e.PropertyName == "FinishedRedo") {
          var undoManager = model.UndoManager;
          if (editToRedo != null) {
            editToRedo.IsSelected = false;
            editToRedo = null;
          }
          // Find the node that represents the undo or redo state and select it
          var nextEdit = undoManager.TransactionToRedo;
          if (nextEdit != null) {
            var itr = undoDisplay.Nodes.GetEnumerator();
            while (itr.MoveNext()) {
              var node = itr.Current;
              if ((node.Data as TreeNodeData).Tag == nextEdit.GetHashCode()) {
                node.IsSelected = true;
                editToRedo = node;
                break;
              }
            }
          }
        }
      } // end model changed listener

      model.Changed += ChangedEvent;
    }
  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public Point Loc { get; set; }
  }
  public class LinkData : Model.LinkData { }

  public class TreeModel : TreeModel<TreeNodeData, string, object> { }
  public class TreeNodeData : TreeModel.NodeData {
    public string Color { get; set; }
    public int Tag { get; set; }
    public int Index { get; set; }
  }
}
