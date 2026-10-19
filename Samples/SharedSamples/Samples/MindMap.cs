/* Copyright (c) Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;

namespace Demo.Samples.MindMap; 
public partial class MindMap : DemoControl {
  private Diagram _Diagram;

  public MindMap() {
    InitializeComponent();
    _Diagram = diagramControl1.Diagram;

    modelJson1.SaveClick = SaveModel;
    modelJson1.LoadClick = LoadModel;
    layoutBtn.Click += (e, obj) => _LayoutAll();

    desc1.MdText = DescriptionReader.Read("Samples.MindMap.md");

    modelJson1.JsonText = @"{
       ""nodeDataSource"": [
        {""key"":-1, ""text"":""Mind Map"", ""loc"":""0 0""},
        {""key"":1, ""parent"":-1, ""text"":""Getting more time"", ""brush"":""skyblue"", ""dir"":""right"", ""loc"":""77 -22""},
        {""key"":11, ""parent"":1, ""text"":""Wake up early"", ""brush"":""skyblue"", ""dir"":""right"", ""loc"":""200 -48""},
        {""key"":12, ""parent"":1, ""text"":""Delegate"", ""brush"":""skyblue"", ""dir"":""right"", ""loc"":""200 -22""},
        {""key"":13, ""parent"":1, ""text"":""Simplify"", ""brush"":""skyblue"", ""dir"":""right"", ""loc"":""200 4""},
        {""key"":2, ""parent"":-1, ""text"":""More effective use"", ""brush"":""darkseagreen"", ""dir"":""right"", ""loc"":""77 43""},
        {""key"":21, ""parent"":2, ""text"":""Planning"", ""brush"":""darkseagreen"", ""dir"":""right"", ""loc"":""203 30""},
        {""key"":211, ""parent"":21, ""text"":""Priorities"", ""brush"":""darkseagreen"", ""dir"":""right"", ""loc"":""274 17""},
        {""key"":212, ""parent"":21, ""text"":""Ways to focus"", ""brush"":""darkseagreen"", ""dir"":""right"", ""loc"":""274 43""},
        {""key"":22, ""parent"":2, ""text"":""Goals"", ""brush"":""darkseagreen"", ""dir"":""right"", ""loc"":""203 56""},
        {""key"":3, ""parent"":-1, ""text"":""Time wasting"", ""brush"":""palevioletred"", ""dir"":""left"", ""loc"":""-20 -31.75""},
        {""key"":31, ""parent"":3, ""text"":""Too many meetings"", ""brush"":""palevioletred"", ""dir"":""left"", ""loc"":""-117 -64.25""},
        {""key"":32, ""parent"":3, ""text"":""Too much time spent on details"", ""brush"":""palevioletred"", ""dir"":""left"", ""loc"":""-117 -25.25""},
        {""key"":33, ""parent"":3, ""text"":""Message fatigue"", ""brush"":""palevioletred"", ""dir"":""left"", ""loc"":""-117 0.75""},
        {""key"":331, ""parent"":31, ""text"":""Check messages less"", ""brush"":""palevioletred"", ""dir"":""left"", ""loc"":""-251 -77.25""},
        {""key"":332, ""parent"":31, ""text"":""Message filters"", ""brush"":""palevioletred"", ""dir"":""left"", ""loc"":""-251 -51.25""},
        {""key"":4, ""parent"":-1, ""text"":""Key issues"", ""brush"":""coral"", ""dir"":""left"", ""loc"":""-20 52.75""},
        {""key"":41, ""parent"":4, ""text"":""Methods"", ""brush"":""coral"", ""dir"":""left"", ""loc"":""-103 26.75""},
        {""key"":42, ""parent"":4, ""text"":""Deadlines"", ""brush"":""coral"", ""dir"":""left"", ""loc"":""-103 52.75""},
        {""key"":43, ""parent"":4, ""text"":""Checkpoints"", ""brush"":""coral"", ""dir"":""left"", ""loc"":""-103 78.75""}
      ]
    }";

    Setup();
  }

  private void Setup() {
    _Diagram.CommandHandler.CopiesTree = true;
    _Diagram.CommandHandler.CopiesParentKey = true;
    _Diagram.CommandHandler.DeletesTree = true;
    _Diagram.ToolManager.DraggingTool.DragsTree = true;
    _Diagram.UndoManager.IsEnabled = true;

    // a node consists of some text with a line shape underneath
    _Diagram.NodeTemplate =
      new Node("Vertical") { SelectionElementName = "TEXT" }
        .Add(
           new TextBlock {
               Name = "TEXT",
               MinSize = new Size(30, 15),
               Editable = true
             }
             .BindTwoWay("Text")
             .BindTwoWay("Scale")
             .BindTwoWay("Font"),
           new Shape("LineH") {
               Stretch = Stretch.Horizontal,
               StrokeWidth = 3, Height = 3,
               // this line shape is the port -- what links connect with
               PortId = "",
               FromSpot = Spot.LeftRightSides, ToSpot = Spot.LeftRightSides
             }
             .Bind(
               new Binding("Stroke", "Brush"),
               // make sure links come in from the proper direction and go out appropriately
               new Binding("FromSpot", "Dir", (d, _) => spotConverter(d, true)),
               new Binding("ToSpot", "Dir", (d, _) => spotConverter(d, false))
             )
        )
        .Bind(
           // remember the locations of each node in the node data
           new Binding("Location", "Loc", Point.Parse, Point.Stringify),
           // make sure text grows in the desired direction
           new Binding("LocationSpot", "Dir", (d, _) => spotConverter(d, false))
        );

    var selectionAdornmentButton =
      Builder.Make<Panel>("Button")
        .Add(
          new TextBlock {
            Text = "+", Font = new Font("Segoe UI", 8, Northwoods.Go.FontWeight.Bold)
          }
        )
        .Set(
          new {
            Alignment = Spot.Right, AlignmentFocus = Spot.Left,
            Click = new Action<InputEvent, GraphObject>(addNodeAndLink)  // define click behavior for this button in the Adornment
          }
        );

    // selected nodes show a button for adding children
    _Diagram.NodeTemplate.SelectionAdornmentTemplate =
      new Adornment("Spot")
      .Add(
        new Panel("Auto")
          .Add(
            // this Adornment has a rectangular blue Shape around the selected node
            new Shape { Fill = null, Stroke = "dodgerblue", StrokeWidth = 3 },
            new Placeholder { Margin = new Margin(4, 4, 0, 4) }
          ),
        // and this Adornment has a Button to the right of the selected node
        selectionAdornmentButton
      );

    // the context menu allows users to change the font size and weight,
    // and to perform a limited tree layout starting at that node
    _Diagram.NodeTemplate.ContextMenu =
      Builder.Make<Adornment>("ContextMenu")
        .Add(
          Builder.Make<Panel>("ContextMenuButton")
            .Add(new TextBlock("Bigger"))
            .Set(
              new {
                Click = new Action<InputEvent, GraphObject>((e, obj) => changeTextSize(obj, 1.1))
              }
            ),
          Builder.Make<Panel>("ContextMenuButton")
            .Add(new TextBlock("Smaller"))
            .Set(
              new {
                Click = new Action<InputEvent, GraphObject>((e, obj) => changeTextSize(obj, 1 / 1.1))
              }
            ),
          Builder.Make<Panel>("ContextMenuButton")
            .Add(new TextBlock("Bold/Normal"))
            .Set(
              new {
                Click = new Action<InputEvent, GraphObject>((e, obj) => toggleTextWeight(obj))
              }
            ),
          Builder.Make<Panel>("ContextMenuButton")
            .Add(new TextBlock("Copy"))
            .Set(
              new {
                Click = new Action<InputEvent, GraphObject>((e, obj) => e.Diagram.CommandHandler.CopySelection())
              }
            ),
          Builder.Make<Panel>("ContextMenuButton")
            .Add(new TextBlock("Delete"))
            .Set(
              new {
                Click = new Action<InputEvent, GraphObject>((e, obj) => e.Diagram.CommandHandler.DeleteSelection())
              }
            ),
          Builder.Make<Panel>("ContextMenuButton")
            .Add(new TextBlock("Undo"))
            .Set(
              new {
                Click = new Action<InputEvent, GraphObject>((e, obj) => e.Diagram.CommandHandler.Undo())
              }
            ),
          Builder.Make<Panel>("ContextMenuButton")
            .Add(new TextBlock("Redo"))
            .Set(
              new {
                Click = new Action<InputEvent, GraphObject>((e, obj) => e.Diagram.CommandHandler.Redo())
              }
            ),
          Builder.Make<Panel>("ContextMenuButton")
            .Add(new TextBlock("Layout"))
            .Set(
              new {
                Click = new Action<InputEvent, GraphObject>((e, obj) => {
                  var adorn = obj.Part as Adornment;
                  adorn.Diagram.StartTransaction("Subtree Layout");
                  layoutTree(adorn.AdornedPart);
                  adorn.Diagram.CommitTransaction("Subtree Layout");
                })
              }
            )
        );

    // a link is just a Bezier-curved line of the same color as the node to which it is connected
    _Diagram.LinkTemplate =
      new Link {
          Curve = LinkCurve.Bezier,
          FromShortLength = -2,
          ToShortLength = -2,
          Selectable = false
        }
        .Add(
          new Shape { StrokeWidth = 3 }
            .Bind(
              new Binding("Stroke", "ToNode", (n) => {
                var brush = ((n as Node).Data as NodeData).Brush;
                return brush ?? "black";
              }).OfElement()
            )
        );

    // the Diagram's context menu just displays commands for general functionality
    _Diagram.ContextMenu =
      Builder.Make<Adornment>("ContextMenu")
        .Add(
          Builder.Make<Panel>("ContextMenuButton")
            .Add(new TextBlock("Paste"))
            .Set(
              new {
                Click = new Action<InputEvent, GraphObject>((e, obj) => e.Diagram.CommandHandler.PasteSelection(e.Diagram.ToolManager.ContextMenuTool.MouseDownPoint))
              }
            )
            .Bind(
              new Binding("Visible", "", (o) => (o as GraphObject).Diagram != null && (o as GraphObject).Diagram.CommandHandler.CanPasteSelection()).OfElement()
            ),
          Builder.Make<Panel>("ContextMenuButton")
            .Add(new TextBlock("Undo"))
            .Set(
              new {
                Click = new Action<InputEvent, GraphObject>((e, obj) => e.Diagram.CommandHandler.Undo())
              }
            )
            .Bind(
              new Binding("Visible", "", (o) => (o as GraphObject).Diagram != null && (o as GraphObject).Diagram.CommandHandler.CanUndo()).OfElement()
            ),
          Builder.Make<Panel>("ContextMenuButton")
            .Add(new TextBlock("Redo"))
            .Set(
              new {
                Click = new Action<InputEvent, GraphObject>((e, obj) => e.Diagram.CommandHandler.Redo())
              }
            )
            .Bind(
              new Binding("Visible", "", (o) => (o as GraphObject).Diagram != null && (o as GraphObject).Diagram.CommandHandler.CanRedo()).OfElement()
            ),
          Builder.Make<Panel>("ContextMenuButton")
            .Add(new TextBlock("Save"))
            .Set(
              new {
                Click = new Action<InputEvent, GraphObject>((e, obj) => SaveModel())
              }
            ),
          Builder.Make<Panel>("ContextMenuButton")
            .Add(new TextBlock("Load"))
            .Set(
              new {
                Click = new Action<InputEvent, GraphObject>((e, obj) => LoadModel())
              }
            )
        );

    _Diagram.SelectionMoved += (s, e) => {
      var rootX = _Diagram.FindNodeForKey(-1).Location.X;
      foreach (var node in _Diagram.Selection) {
        var data = node.Data as NodeData;
        if (data.Parent != -1) return; // only consider nodes connected to root
        var nodeX = node.Location.X;
        if (rootX < nodeX && data.Dir != "right") {
          updateNodeDirection(node, "right");
        } else if (rootX > nodeX && data.Dir != "left") {
          updateNodeDirection(node, "left");
        }
        layoutTree(node);
      }
    };

    LoadModel();
  }

  private void SaveModel() {
    if (_Diagram == null) return;
    modelJson1.JsonText = _Diagram.Model.ToJson();
  }

  private void LoadModel() {
    if (_Diagram == null) return;
    _Diagram.Model = Model.FromJson<Model>(modelJson1.JsonText);
    _Diagram.Model.UndoManager.IsEnabled = true;
  }

  private Spot spotConverter(object dir, bool from) {
    if ((string)dir == "left") {
      return from ? Spot.Left : Spot.Right;
    } else {
      return from ? Spot.Right : Spot.Left;
    }
  }

  private void changeTextSize(GraphObject obj, double factor) {
    var adorn = obj.Part as Adornment;
    adorn.Diagram.StartTransaction("Change Text Size");
    var node = adorn.AdornedPart;
    var tb = node.FindElement("TEXT");
    tb.Scale *= factor;
    adorn.Diagram.CommitTransaction("Change Text Size");
  }

  private void toggleTextWeight(GraphObject obj) {
    var adorn = obj.Part as Adornment;
    adorn.Diagram.StartTransaction("Change Text Weight");
    var node = adorn.AdornedPart;
    var tb = node.FindElement("TEXT") as TextBlock;
    // flip the whether font is bold
    if (tb.Font.Weight != Northwoods.Go.FontWeight.Bold) {
      tb.Font = new Font(tb.Font.Family, tb.Font.Size, Northwoods.Go.FontWeight.Bold, tb.Font.Unit);
    } else {
      tb.Font = new Font(tb.Font.Family, tb.Font.Size, Northwoods.Go.FontWeight.Regular, tb.Font.Unit);
    }
    adorn.Diagram.CommitTransaction("Change Text Weight");
  }

  private void updateNodeDirection(Part node, string dir) {
    diagramControl1.Diagram.Model.Set(node.Data, "Dir", dir);
    // recursively update the direction of the child nodes
    var chl = (node as Node).FindTreeChildrenNodes();
    foreach (var child in chl) {
      updateNodeDirection(child, dir);
    }
  }

  private void addNodeAndLink(InputEvent e, GraphObject obj) {
    var adorn = obj.Part as Adornment;
    var diagram = adorn.Diagram;
    diagram.StartTransaction("Add Node");
    var oldnode = adorn.AdornedPart;
    var olddata = oldnode.Data as NodeData;
    // copy the brush and direction to the new node data
    var newData = new NodeData { Text = "idea", Brush = olddata.Brush, Dir = olddata.Dir, Parent = olddata.Key };
    diagram.Model.AddNodeData(newData);
    layoutTree(oldnode);
    diagram.CommitTransaction("Add Node");

    // if the new node is off-screen, scroll the diagram to show the new node
    var newnode = diagram.FindNodeForData(newData);
    if (newnode != null) diagram.ScrollToRect(newnode.ActualBounds);
  }

  private void layoutTree(Part node) {
    var data = node.Data as NodeData;
    if (data.Key == -1) { // adding to the root?
      _LayoutAll(); // layout everything
    } else { // otherwise layout only the subtree starting at this parent node
      var parts = (node as Node).FindTreeParts();
      layoutAngle(parts, data.Dir == "left" ? 180 : 0);
    }
  }

  private void layoutAngle(IEnumerable<Part> parts, double angle) {
    var layout = new TreeLayout {
      Angle = angle,
      Arrangement = TreeArrangement.FixedRoots,
      NodeSpacing = 5,
      LayerSpacing = 20,
      SetsPortSpot = false, // dont set port spots since we're managing them with our spotConverter function
      SetsChildPortSpot = false
    };
    layout.DoLayout(parts);
  }

  private void _LayoutAll() {
    var root = diagramControl1.Diagram.FindNodeForKey(-1);
    if (root == null) return;
    diagramControl1.Diagram.StartTransaction("Layout");
    // split the nodes and links into two collections
    var rightward = new HashSet<Part>();
    var leftward = new HashSet<Part>();
    foreach (var link in root.FindLinksConnected()) {
      var child = link.ToNode;
      if ((child.Data as NodeData).Dir == "left") {
        leftward.Add(root);
        leftward.Add(link);
        foreach (var part in child.FindTreeParts()) leftward.Add(part);
      } else {
        rightward.Add(root);
        rightward.Add(link);
        foreach (var part in child.FindTreeParts()) rightward.Add(part);
      }
    }
    // do one layout and then the other without moving the shared root node
    layoutAngle(rightward, 0);
    layoutAngle(leftward, 180);
    diagramControl1.Diagram.CommitTransaction("Layout");
  }
}

public class Model : TreeModel<NodeData, int, object> { }

public class NodeData : Model.NodeData {
  public string Loc { get; set; }
  public string Brush { get; set; }
  public string Dir { get; set; } // left or right
  public Font Font { get; set; } = new();
  public double Scale { get; set; } = 1;
}
