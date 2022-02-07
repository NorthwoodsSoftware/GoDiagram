using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;
using System;

namespace WinFormsSampleControls.MindMap {
  [ToolboxItem(false)]
  public partial class MindMapControl : System.Windows.Forms.UserControl {
    private Diagram MyDiagram;

    public MindMapControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      saveLoadModel1.SaveClick += (e, obj) => SaveModel();
      saveLoadModel1.LoadClick += (e, obj) => LoadModel();
      btnLayout.Click += (e, obj) => _LayoutAll();

      goWebBrowser1.Html = @"
  <p>
    A mind map is a kind of spider diagram that organizes information around a central concept, with connecting branches.
  </p>
  <p>
    The layout is controlled by moving the nodes closest to the tree's root node.
    When one of these nodes is moved horizontally to the other side of the root,
    all of its children will be sent to <a>Layout.DoLayout</a> with a new direction,
    causing text to always be moved outwards from the root. The <b>spotConverter</b> function is used to manage
    <a>GraphObject.FromSpot</a> and <a>GraphObject.ToSpot</a> for nodes manually, so the <a>TreeLayout.SetsPortSpot</a> and <a>TreeLayout.SetsChildPortSpot</a>
    properties are set to false so that laying out the diagram will not overwrite the values.
  </p>
  <p>
    When a node is deleted the <a>CommandHandler.DeletesTree</a> property ensures that
    all of its children are deleted with it. When a node is dragged the <a>DraggingTool.DragsTree</a>
    property ensures that all its children are dragged with it.
    Both of these are set during the the Diagram's initalization.
  </p>
  <p>
    Node templates also have a <a>Part.SelectionAdornmentTemplate</a> defined to allow for new nodes to be created and a <a>GraphObject.ContextMenu</a> with additional commands.
  </p>
";

      saveLoadModel1.ModelJson = @"{
       ""NodeDataSource"": [
        {""Key"":-1, ""Text"":""Mind Map"", ""Loc"":""0 0""},
        {""Key"":1, ""Parent"":-1, ""Text"":""Getting more time"", ""Brush"":""skyblue"", ""Dir"":""right"", ""Loc"":""77 -22""},
        {""Key"":11, ""Parent"":1, ""Text"":""Wake up early"", ""Brush"":""skyblue"", ""Dir"":""right"", ""Loc"":""200 -48""},
        {""Key"":12, ""Parent"":1, ""Text"":""Delegate"", ""Brush"":""skyblue"", ""Dir"":""right"", ""Loc"":""200 -22""},
        {""Key"":13, ""Parent"":1, ""Text"":""Simplify"", ""Brush"":""skyblue"", ""Dir"":""right"", ""Loc"":""200 4""},
        {""Key"":2, ""Parent"":-1, ""Text"":""More effective use"", ""Brush"":""darkseagreen"", ""Dir"":""right"", ""Loc"":""77 43""},
        {""Key"":21, ""Parent"":2, ""Text"":""Planning"", ""Brush"":""darkseagreen"", ""Dir"":""right"", ""Loc"":""203 30""},
        {""Key"":211, ""Parent"":21, ""Text"":""Priorities"", ""Brush"":""darkseagreen"", ""Dir"":""right"", ""Loc"":""274 17""},
        {""Key"":212, ""Parent"":21, ""Text"":""Ways to focus"", ""Brush"":""darkseagreen"", ""Dir"":""right"", ""Loc"":""274 43""},
        {""Key"":22, ""Parent"":2, ""Text"":""Goals"", ""Brush"":""darkseagreen"", ""Dir"":""right"", ""Loc"":""203 56""},
        {""Key"":3, ""Parent"":-1, ""Text"":""Time wasting"", ""Brush"":""palevioletred"", ""Dir"":""left"", ""Loc"":""-20 -31.75""},
        {""Key"":31, ""Parent"":3, ""Text"":""Too many meetings"", ""Brush"":""palevioletred"", ""Dir"":""left"", ""Loc"":""-117 -64.25""},
        {""Key"":32, ""Parent"":3, ""Text"":""Too much time spent on details"", ""Brush"":""palevioletred"", ""Dir"":""left"", ""Loc"":""-117 -25.25""},
        {""Key"":33, ""Parent"":3, ""Text"":""Message fatigue"", ""Brush"":""palevioletred"", ""Dir"":""left"", ""Loc"":""-117 0.75""},
        {""Key"":331, ""Parent"":31, ""Text"":""Check messages less"", ""Brush"":""palevioletred"", ""Dir"":""left"", ""Loc"":""-251 -77.25""},
        {""Key"":332, ""Parent"":31, ""Text"":""Message filters"", ""Brush"":""palevioletred"", ""Dir"":""left"", ""Loc"":""-251 -51.25""},
        {""Key"":4, ""Parent"":-1, ""Text"":""Key issues"", ""Brush"":""coral"", ""Dir"":""left"", ""Loc"":""-20 52.75""},
        {""Key"":41, ""Parent"":4, ""Text"":""Methods"", ""Brush"":""coral"", ""Dir"":""left"", ""Loc"":""-103 26.75""},
        {""Key"":42, ""Parent"":4, ""Text"":""Deadlines"", ""Brush"":""coral"", ""Dir"":""left"", ""Loc"":""-103 52.75""},
        {""Key"":43, ""Parent"":4, ""Text"":""Checkpoints"", ""Brush"":""coral"", ""Dir"":""left"", ""Loc"":""-103 78.75""}
      ]
    }";

    }

    private void Setup() {

      MyDiagram = diagramControl1.Diagram;
      MyDiagram.CommandHandler.CopiesTree = true;
      MyDiagram.CommandHandler.CopiesParentKey = true;
      MyDiagram.CommandHandler.DeletesTree = true;
      MyDiagram.ToolManager.DraggingTool.DragsTree = true;
      MyDiagram.UndoManager.IsEnabled = true;

      // a node consists of some text with a line shape underneath
      MyDiagram.NodeTemplate = new Node(PanelLayoutVertical.Instance) {
        SelectionElementName = "TEXT"
      }.Add(
       new TextBlock {
         Name = "TEXT",
         MinSize = new Size(30, 15),
         Editable = true
       }.Bind(
         new Binding("Text").MakeTwoWay(),
         new Binding("Scale").MakeTwoWay(),
         new Binding("Font").MakeTwoWay()
       ),
       new Shape("LineH") {
         Stretch = Stretch.Horizontal,
         StrokeWidth = 3, Height = 3,
         // this line shape is the port -- what links connect with
         PortId = "",
         FromSpot = Spot.LeftRightSides, ToSpot = Spot.LeftRightSides
       }.Bind(
         new Binding("Stroke", "Brush"),
         // make sure links come in from the proper direction and go out appropriately
         new Binding("FromSpot", "Dir", (d, _) => spotConverter(d, true)),
         new Binding("ToSpot", "Dir", (d, _) => spotConverter(d, false))
       ))
      .Bind(
       // remember the locations of each node in the node data
       new Binding("Location", "Loc", Point.Parse, Point.Stringify),
       // make sure text grows in the desired direction
       new Binding("LocationSpot", "Dir", (d, _) => spotConverter(d, false))
      );

      var selectionAdornmentButton = Builder.Make<Panel>("Button");
      selectionAdornmentButton.Alignment = Spot.Right;
      selectionAdornmentButton.AlignmentFocus = Spot.Left;
      selectionAdornmentButton.Click = addNodeAndLink; // define click behavior for this button in the Adornment
      selectionAdornmentButton = selectionAdornmentButton.Add(new TextBlock {
        Text = "+", Font = new Font("Segoe UI", 8, FontWeight.Bold)
      });

      // selected nodes show a button for adding children
      MyDiagram.NodeTemplate.SelectionAdornmentTemplate = new Adornment(PanelLayoutSpot.Instance).Add(
        new Panel(PanelLayoutAuto.Instance).Add(
          // this Adornment has a rectangular blue Shape around the selected node
          new Shape { Fill = null, Stroke = "dodgerblue", StrokeWidth = 3 },
          new Placeholder { Margin = new Margin(4, 4, 0, 4) }
        ),
        // and this Adornment has a Button to the right of the selected node
        selectionAdornmentButton
      );

      // the context menu allows users to change the font size and weight,
      // and to perform a limited tree layout starting at that node
      MyDiagram.NodeTemplate.ContextMenu =
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
      MyDiagram.LinkTemplate = new Link {
        Curve = LinkCurve.Bezier,
        FromShortLength = -2,
        ToShortLength = -2,
        Selectable = false
      }.Add(
        new Shape {
          StrokeWidth = 3
        }.Bind(
          new Binding("Stroke", "ToNode", (n, _) => {
            var brush = ((n as Node).Data as NodeData).Brush;
            return brush ?? "black";
          }).OfElement()
        )
      );

      // the Diagram's context menu just displays commands for general functionality
      MyDiagram.ContextMenu = Builder.Make<Adornment>("ContextMenu").Add(
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

      MyDiagram.SelectionMoved += (s, e) => {
        var rootX = MyDiagram.FindNodeForKey(-1).Location.X;
        foreach (var node in MyDiagram.Selection) {
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
      if (MyDiagram == null) return;
      saveLoadModel1.ModelJson = MyDiagram.Model.ToJson();
    }

    private void LoadModel() {
      if (MyDiagram == null) return;
      MyDiagram.Model = Model.FromJson<Model>(saveLoadModel1.ModelJson);
      MyDiagram.Model.UndoManager.IsEnabled = true;
    }

    private Spot spotConverter(object dir, bool from) {
      var MyDiagram = diagramControl1.Diagram;

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
      if (tb.Font.Weight != FontWeight.Bold) {
        tb.Font = new Font(tb.Font.Family, tb.Font.Size, FontWeight.Bold, tb.Font.Unit);
      } else {
        tb.Font = new Font(tb.Font.Family, tb.Font.Size, FontWeight.Regular, tb.Font.Unit);
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

}
