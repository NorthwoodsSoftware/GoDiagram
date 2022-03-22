using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;

namespace WinFormsSampleControls.UpdateDemo {
  [ToolboxItem(false)]
  public partial class UpdateDemoControl : System.Windows.Forms.UserControl {

    private Diagram blueDiagram;
    private Diagram greenDiagram;
    private Diagram undoDisplay;

    public UpdateDemoControl() {
      InitializeComponent();

      Setup();

      clearLogModelBtn.Click += (e, obj) => ClearLog();
      undoBtn.Click += (e, obj) => blueDiagram.CommandHandler.Undo();
      redoBtn.Click += (e, obj) => blueDiagram.CommandHandler.Redo();

      goWebBrowser2.Html = @"
        <p>
          This sample has two Diagrams, named ""blueDiagram"" and ""greenDiagram"", that display the same Model.
          Each diagram uses its own templates for its nodes and links, causing the appearance of each diagram to be different.
          However making a change in one diagram that changes the model causes those model changes to be reflected in the other diagram.
        </p>
        <p>
          This sample also shows, next to the blue diagram, almost all of the <a>ChangedEvent</a>s that the shared model undergoes.
          (For clarity it leaves out some of the Transaction - oriented events.)
          The model Changed listener adds a line for each ChangedEvent to the log.
          Transaction notification events start with an asterisk ""*"",
          while property changes and collection insertions and removals start with an exclamation mark ""!"".
        </p>
        <p>
          Next to the green diagram there is a tree view display of the <a>UndoManager</a>'s history.
          The <a>UndoManager.History</a> is a List of <a>Transaction</a>s,
          where each <a>Transaction.Changes</a> property holds all of the ChangedEvents that occurred due to some command or tool operation.
          These ChangedEvents are reflective of both changes to the model (prefixed with ""!m"") and to the diagram (prefixed with ""!d"").
          You will note that there are often several diagram changes for each model change.
        </p>
        <p>
          This demo is different from the <a href=""TwoDiagrams"">Two Diagrams</a> sample, which is an example of two Diagrams,
          each sharing/showing a different <a>Model</a>, but sharing the same <a>UndoManager</a>.
        </p>
        <p>
          Many of the other samples demonstrate saving the whole model by calling <a>Model.ToJson</a>.
          If you want to save incrementally, you should do so at the end of each transaction, when <a>ChangedEvent.IsTransactionFinished</a>.
          The <a>ChangedEvent.Object</a> may be a <a>Transaction</a>.
          Look through the <a>Transaction.Changes</a> list for the model changes that you want to save.
        </p>
        <p>
          This code demonstrates the basic idea:
        </p>
";
      goWebBrowser1.Html = @"
           <p>Update Demo <b>GoDiagram</b> Sample</p>
";

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
        new Node(PanelLayoutAuto.Instance).Bind(
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
            Font = new Font("Arial", 10, FontWeight.Bold),
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
        new Node(PanelLayoutVertical.Instance).Bind(
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
            Font = new Font("Georgia", 12, FontWeight.Bold)
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
          new Panel(PanelLayoutHorizontal.Instance) {
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

      void ChangedEvent(object _, ChangedEvent e) {
        // do not display some uninteresting kinds of transaction notifications
        if (e.Change == ChangeType.Transaction) {
          if (e.PropertyName == "CommittingTransaction" || e.ModelChange == ModelChangeType.SourceChanged) return;
          // do not display any layout transactions
          if (e.OldValue as string == "Layout") return;
        }  // You will probably want to use e.IsTransactionFinished instead

        // Add entries into the log
        var changes = e.ToString();
        if (changes[0] != '*') changes = "  " + changes;
        listBox1.Items.Add(changes);
        // scrolls down automatically
        int visibleItems = listBox1.ClientSize.Height / listBox1.ItemHeight;
        listBox1.TopIndex = Math.Max(listBox1.Items.Count - visibleItems + 1, 0);



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

    private void ClearLog() {
      listBox1.Items.Clear();
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
}
