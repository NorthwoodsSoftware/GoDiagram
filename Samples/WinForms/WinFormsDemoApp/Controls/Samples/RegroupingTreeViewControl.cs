/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace Demo.Samples.RegroupingTreeView {
  [ToolboxItem(false)]
  public partial class RegroupingTreeViewControl : DemoControl {
    private Diagram myDiagram;
    private Diagram myTreeView;

    public RegroupingTreeViewControl() {
      InitializeComponent();

      modelJson1.SaveClick = SaveModel;
      modelJson1.LoadClick = LoadModel;

      goWebBrowser1.Html = @"
        <p>
      This sample demonstrates the synchronization of two different models, necessitated by their being different types:
      <a>TreeModel</a> for the tree view and <a>GraphLinksModel</a> for the general diagram on the right.
      Normally in such situations one would have a single model with two diagrams showing the shared model.
      However in this case there are two separate models but the model data, including the <a>Model.NodeDataSource</a>, are shared.
      That means the ""Group"" property is used in the normal fashion in the GraphLinksModel but is used as the ""Parent""
      reference in the TreeModel.
        </p>
        <p>
      That introduces some complications when there are changes to the data, since they need to be reflected in other other model
      even though the data properties have already been changed!
      This is accomplished by having a Model Changed listener on each model that explicitly updates the other model.
        </p>";


      modelJson1.JsonText = @"{
  ""NodeDataSource"": [
    { ""Key"":1, ""Text"":""Main 1"", ""IsGroup"":true, ""Category"":""OfGroups""},
    { ""Key"":2, ""Text"":""Main 2"", ""IsGroup"":true, ""Category"":""OfGroups""},
    { ""Key"":3, ""Text"":""Group A"", ""IsGroup"":true, ""Category"":""OfNodes"", ""Group"":1},
    { ""Key"":4, ""Text"":""Group B"", ""IsGroup"":true, ""Category"":""OfNodes"", ""Group"":1},
    { ""Key"":5, ""Text"":""Group C"", ""IsGroup"":true, ""Category"":""OfNodes"", ""Group"":2},
    { ""Key"":6, ""Text"":""Group D"", ""IsGroup"":true, ""Category"":""OfNodes"", ""Group"":2},
    { ""Key"":7, ""Text"":""Group E"", ""IsGroup"":true, ""Category"":""OfNodes"", ""Group"":6},
    { ""Text"":""first A"", ""Group"":3, ""Key"":-7},
    { ""Text"":""second A"", ""Group"":3, ""Key"":-8},
    { ""Text"":""first B"", ""Group"":4, ""Key"":-9},
    { ""Text"":""second B"", ""Group"":4, ""Key"":-10},
    { ""Text"":""third B"", ""Group"":4, ""Key"":-11},
    { ""Text"":""first C"", ""Group"":5, ""Key"":-12},
    { ""Text"":""second C"", ""Group"":5, ""Key"":-13},
    { ""Text"":""first D"", ""Group"":6, ""Key"":-14},
    { ""Text"":""first E"", ""Group"":7, ""Key"":-15}
  ],
  ""LinkDataSource"": [  ]
}";

      Setup();
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;
      var myChangingSelection = false;

      myDiagram.MouseDrop = (e) => finishDrop(e, null);
      myDiagram.Layout = new GridLayout {
        WrappingWidth = double.PositiveInfinity,
        Alignment = GridAlignment.Position,
        CellSize = new Size(1, 1)
      };
      myDiagram.CommandHandler.ArchetypeGroupData = new NodeData {
        IsGroup = true,
        Category = "OfNodes"
      };
      myDiagram.UndoManager.IsEnabled = true;
      // when a node is selected in the main Diagram, select the corresponding tree node
      myDiagram.ChangedSelection += (obj, e) => {
        if (myChangingSelection) return;
        myChangingSelection = true;
        var diagnodes = new HashSet<Node>();
        foreach (var n in myDiagram.Selection) {
          diagnodes.Add(myTreeView.FindNodeForData(n.Data));
        }
        myTreeView.ClearSelection();
        myTreeView.Select(diagnodes);
        myChangingSelection = false;
      };

      // There are two templates for groups, "OfGroups" and "OfNodes"

      // this function is used to highlight a group that the selection may be dropped into
      void highlightGroup(InputEvent e, Group grp, bool show) {
        if (grp == null) return;
        e.Handled = true;
        if (show) {
          // cannot depend on the grp.Diagram.Selection in the case of external drag-and-drops;
          // instead depend on the DraggingTool.DraggedParts or .CopiedParts
          var tool = grp.Diagram.ToolManager.DraggingTool;
          // now we can check to see if the Group will accept membership of the dragged parts
          if (grp.CanAddMembers(tool.DraggedParts.Keys) || grp.CanAddMembers(tool.CopiedParts.Keys)) {
            grp.IsHighlighted = true;
            return;
          }
        }
        grp.IsHighlighted = false;
      }

      // Upon a drop onto a Group, we try to add the selection as members of the Group.
      // Upon a drop onto the background, or onto a top-level Node, make selection top-level.
      // If this is OK, we're done; otherwise we cancel the operation to rollback everything.
      void finishDrop(InputEvent e, GraphObject grp) {
        var ok = grp != null
          ? (grp as Group).AddMembers(grp.Diagram.Selection, true)
          : e.Diagram.CommandHandler.AddTopLevelParts(e.Diagram.Selection, true);
        if (!ok) e.Diagram.CurrentTool.DoCancel();
      }

      myDiagram.GroupTemplateMap.Add("OfGroups",
        new Group("Auto") {
            Background = "transparent",
            // highlight when dragging into the Group
            MouseDragEnter = (e, grp, prev) => highlightGroup(e, grp as Group, true),
            MouseDragLeave = (e, grp, next) => highlightGroup(e, grp as Group, false),
            ComputesBoundsAfterDrag = true,
            // when the selection is dropped into a group, add the selected Parts into that Group;
            // if it fails, cancel the tool, rolling back any changes
            MouseDrop = finishDrop,
            HandlesDragDropForMembers = true,  // don't need to define handlers on member Nodes and Links
            // groups containing groups lay out their members horizontally
            Layout = new GridLayout {
              WrappingWidth = double.PositiveInfinity, Alignment = GridAlignment.Position,
              CellSize = new Size(1, 1), Spacing = new Size(4, 4)
            }
          }
          .Bind(new Binding("Background", "IsHighlighted", (h, _) => (bool)h ? "rgba(255,0,0,0.2)" : "transparent").OfElement())
          .Add(
            new Shape("Rectangle") { Fill = null, Stroke = "#E69900", StrokeWidth = 2 },
            new Panel("Vertical")  // title above Placeholder
              .Add(
                new Panel("Horizontal") {  // button next to Textblock
                    Stretch = Stretch.Horizontal, Background = "#FFDD33", Margin = 1
                  }
                  .Add(
                    Builder.Make<Panel>("SubGraphExpanderButton")
                      .Set(new { Alignment = Spot.Right, Margin = 5 }),
                    new TextBlock {
                        Alignment = Spot.Left,
                        Editable = true,
                        Margin = 5,
                        Font = new Font("Segoe UI", 18, FontWeight.Bold),
                        Stroke = "#9A6600"
                      }
                      .Bind(new Binding("Text").MakeTwoWay())
                  ),  // end horizontal Panel
                new Placeholder { Padding = 5, Alignment = Spot.TopLeft }
              )  // end Vertical panel
          )
      );

      myDiagram.GroupTemplateMap.Add("OfNodes",
        new Group("Auto") {
            Background = "transparent",
            Ungroupable = true,
            // highlight when dragging into the Group
            MouseDragEnter = (e, grp, prev) => highlightGroup(e, grp as Group, true),
            MouseDragLeave = (e, grp, next) => highlightGroup(e, grp as Group, false),
            ComputesBoundsAfterDrag = true,
            // when the selection is dropped into a group, add the selected Parts into that Group;
            // if it fails, cancel the tool, rolling back any changes
            MouseDrop = finishDrop,
            HandlesDragDropForMembers = true,  // don't need to define handlers on member Nodes and Links
            // groups containing groups lay out their members horizontally
            Layout = new GridLayout {
              WrappingColumn = 1, Alignment = GridAlignment.Position,
              CellSize = new Size(1, 1), Spacing = new Size(4, 4)
            }
          }
          .Bind(new Binding("Background", "IsHighlighted", (h, _) => (bool)h ? "rgba(255,0,0,0.2)" : "transparent").OfElement())
          .Add(
            new Shape("Rectangle") { Fill = null, Stroke = "#0099CC", StrokeWidth = 2 },
            new Panel("Vertical")  // title above Placeholder
              .Add(
                new Panel("Horizontal") {  // button next to Textblock
                    Stretch = Stretch.Horizontal, Background = "#33D3E5", Margin = 1
                  }
                  .Add(
                    Builder.Make<Panel>("SubGraphExpanderButton")
                      .Set(new { Alignment = Spot.Right, Margin = 5 }),
                    new TextBlock {
                      Alignment = Spot.Left,
                      Editable = true,
                      Margin = 5,
                      Font = new Font("Segoe UI", 16, FontWeight.Bold),
                      Stroke = "#006080"
                    }.Bind(new Binding("Text").MakeTwoWay())
                  ),  // end horizontal Panel
                new Placeholder { Padding = 5, Alignment = Spot.TopLeft }
              )  // end Vertical panel
          )
      );

      // Nodes have a trivial definition
      myDiagram.NodeTemplate =
        new Node("Auto") {
            // dropping onto a node is the same as dropping on its containing Group, even if its top-level
            MouseDrop = (e, nod) => finishDrop(e, (nod as Node).ContainingGroup)
          }
          .Add(
            new Shape("Rectangle") { Fill = "#ACE600", Stroke = "#558000", StrokeWidth = 2 }
              .Bind("Fill", "Color"),
            new TextBlock {
                Margin = 5,
                Editable = true,
                Font = new Font("Segoe UI", 13, FontWeight.Bold),
                Stroke = "#446700"
              }
              .Bind(new Binding("Text").MakeTwoWay())
          );

      var myChangingModel = false;

      myDiagram.ModelChanged += (obj, e) => {
        if (e.Model.SkipsUndoManager) return;
        if (myChangingModel) return;
        myChangingModel = true;
        // don't need to start/commit a transaction because the UndoManager is shared with myTreeView
        if (e.ModelChange == ModelChangeType.NodeGroupKey || e.ModelChange == ModelChangeType.NodeParentKey) {
          // handle structural change: group memberships
          var treenode = myTreeView.FindNodeForData(e.Object);
          if (treenode != null) treenode.UpdateRelationshipsFromData();
        } else if (e.Change == ChangeType.Property) {
          var treenode = myTreeView.FindNodeForData(e.Object);
          if (treenode != null) treenode.UpdateTargetBindings();
        } else if (e.Change == ChangeType.Insert && e.PropertyName == "NodeDataSource") {
          // pretend the new data isn't already in the nodeDataSource for myTreeView
          (myTreeView.Model.NodeDataSource as List<NodeData>).Remove(e.NewValue as NodeData);
          // now add to the mytreeview model using the normal mechanisms
          myTreeView.Model.AddNodeData(e.NewValue);
        } else if (e.Change == ChangeType.Remove && e.PropertyName == "NodeDataSource") {
          // remove the corresponding node from mytreeview
          var treenode = myTreeView.FindNodeForData(e.OldValue);
          if (treenode != null) myTreeView.Remove(treenode);
        }
        myChangingModel = false;
      };

      // setup the tree view; will be initialized with data by the _Load() function
      myTreeView = treeViewControl1.Diagram;

      myTreeView.InitialContentAlignment = Spot.TopLeft;
      myTreeView.AllowMove = false;  // don't let users mess up this tree
      myTreeView.AllowCopy = true;  // but you might want this to be false
      myTreeView.CommandHandler.CopiesTree = true;
      myTreeView.CommandHandler.CopiesParentKey = true;
      myTreeView.AllowDelete = true;  // but you might want this to be false
      myTreeView.CommandHandler.DeletesTree = true;
      myTreeView.AllowHorizontalScroll = false;
      myTreeView.Layout = new TreeLayout {
        Alignment = TreeAlignment.Start,
        Angle = 0,
        Compaction = TreeCompaction.None,
        LayerSpacing = 16,
        LayerSpacingParentOverlap = 1,
        NodeIndentPastParent = 1.0,
        NodeSpacing = 0,
        SetsPortSpot = false,
        SetsChildPortSpot = false,
        ArrangementSpacing = new Size(0, 0)
      };
      // when a node is selected in the tree, select the corresponding node in the main Diagram
      myTreeView.ChangedSelection += (obj, e) => {
        if (myChangingSelection) return;
        myChangingSelection = true;
        var diagnodes = new HashSet<Node>();
        foreach (var n in myTreeView.Selection) {
          diagnodes.Add(myDiagram.FindNodeForData(n.Data));
        }
        myDiagram.ClearSelection();
        myDiagram.Select(diagnodes);
        myChangingSelection = false;
      };

      myTreeView.NodeTemplate =
        new Node {
            // no Adornment: instead change panel background color by binding to Node.IsSelected
            SelectionAdorned = false
          }
         .Add(
           Builder.Make<Panel>("TreeExpanderButton")
             .Set(new {
               Width = 14,
               ButtonBorder_Fill = "white",
               ButtonBorder_Stroke = (Brush)null,
               _ButtonFillOver = "rgba(0, 128, 255, .25)",
               _ButtonStrokeOver = (Brush)null
             }),
           new Panel("Horizontal") { Position = new Point(16, 0) }
             .Bind(new Binding("Background", "IsSelected", (s, _) => (bool)s ? "lightblue" : "white").OfElement())
             .Add(
                new TextBlock { Editable = true }
                  .Bind(new Binding("Text").MakeTwoWay())
             ) // end Horizontal Panel
         ); // end Node

      // without lines
      myTreeView.LinkTemplate = new Link();

      // cannot share the model itself, but can share all of the node data from the main Diagram,
      // pretending the "group" relationship is the tree parent relationship
      myTreeView.Model = new ModelTree { NodeParentKeyProperty = "Group" };

      myTreeView.ModelChanged += (obj, e) => {
        if (e.Model.SkipsUndoManager) return;
        if (myChangingModel) return;
        myChangingModel = true;
        // don't need to start/commit a transaction because the UndoManager is shared with MyDiagram
        if (e.ModelChange == ModelChangeType.NodeGroupKey || e.ModelChange == ModelChangeType.NodeParentKey) {
          // handle structural change: tree parent/children
          var node = myDiagram.FindNodeForData(e.Object);
          if (node != null) node.UpdateRelationshipsFromData();
        } else if (e.Change == ChangeType.Property) {
          // propagate simple data property changes back to the main diagram
          var node = myDiagram.FindNodeForData(e.Object);
          if (node != null) node.UpdateTargetBindings();
        } else if (e.Change == ChangeType.Insert && e.PropertyName == "NodeDataSource") {
          // pretend the new data isn't already in the nodeDataSource for the main Diagram model
          (myDiagram.Model.NodeDataSource as List<NodeData>).Remove(e.NewValue as NodeData);
          // now add to the MyDiagram Model using the normal mechanisms
          myDiagram.Model.AddNodeData(e.NewValue);
        } else if (e.Change == ChangeType.Remove && e.PropertyName == "NodeDataSource") {
          // remove the corresponding node from the main Diagram
          var node = myDiagram.FindNodeForData(e.OldValue);
          if (node != null) myDiagram.Remove(node);
        }
        myChangingModel = false;
        //ReverseTree();
      };

      LoadModel();
    }

    private void SaveModel() {
      modelJson1.JsonText = myDiagram.Model.ToJson();
    }

    private void LoadModel() {
      myDiagram.Model = Model.FromJson<Model>(modelJson1.JsonText);

      // share all of the data with the tree view
      myTreeView.Model.NodeDataSource = myDiagram.Model.NodeDataSource;

      // share the UndoManager too!
      myTreeView.Model.UndoManager = myDiagram.Model.UndoManager;
    }
  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }
  public class NodeData : Model.NodeData { }
  public class LinkData : Model.LinkData { }

  public class ModelTree : TreeModel<NodeData, int, object> { }
}
