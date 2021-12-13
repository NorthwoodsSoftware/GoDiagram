using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.WinForms;
using Northwoods.Go.Extensions;
using Northwoods.Go.Layouts;

namespace WinFormsSampleControls.Regrouping {
  [ToolboxItem(false)]
  public partial class RegroupingControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    private Palette myPalette;

    private Part sharedNodeTemplate;
    private Group sharedGroupTemplate;

    private int sliderValue = 3;
    public RegroupingControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;
      paletteControl1.AfterRender = SetupPalette;

      saveLoadModel1.SaveClick += (e, obj) => SaveModel();
      saveLoadModel1.LoadClick += (e, obj) => LoadModel();

      trackBar1.ValueChanged += (obj, e) => Reexpand();
      saveLoadModel1.ModelJson = @"
    {
  ""NodeDataSource"": [
    {""Key"":1, ""Text"":""Main 1"", ""IsGroup"":true, ""Horiz"":true},
    {""Key"":2, ""Text"":""Main 2"", ""IsGroup"":true, ""Horiz"":true},
    {""Key"":3, ""Text"":""Group A"", ""IsGroup"":true, ""Group"":1},
    {""Key"":4, ""Text"":""Group B"", ""IsGroup"":true, ""Group"":1},
    {""Key"":5, ""Text"":""Group C"", ""IsGroup"":true, ""Group"":2},
    {""Key"":6, ""Text"":""Group D"", ""IsGroup"":true, ""Group"":2},
    {""Key"":7, ""Text"":""Group E"", ""IsGroup"":true, ""Group"":6},
    {""Text"":""first A"", ""Group"":3, ""Key"":-7},
    {""Text"":""second A"", ""Group"":3, ""Key"":-8},
    {""Text"":""first B"", ""Group"":4, ""Key"":-9},
    {""Text"":""second B"", ""Group"":4, ""Key"":-10},
    {""Text"":""third B"", ""Group"":4, ""Key"":-11},
    {""Text"":""first C"", ""Group"":5, ""Key"":-12},
    {""Text"":""second C"", ""Group"":5, ""Key"":-13},
    {""Text"":""first D"", ""Group"":6, ""Key"":-14},
    {""Text"":""first E"", ""Group"":7, ""Key"":-15}
  ],
  ""LinkDataSource"": [  ]}
  ";

      goWebBrowser1.Html = @"
        <p>
          This sample allows the user to drag nodes, including groups, into and out of groups,
          both from the Palette as well as from within the Diagram.
          See the <a href=""intro/groups.html"">Groups Intro page</a> for an explanation of GoDiagram Groups.
        </p>
        <p>
          Highlighting to show feedback about potential addition to a group during a drag is implemented
          using <a>GraphObject.MouseDragEnter</a> and <a>GraphObject.MouseDragLeave</a> event handlers.
          Because <a>Group.ComputesBoundsAfterDrag</a> is true, the Group's <a>Placeholder</a>'s bounds are
          not computed until the drop happens, so the group does not continuously expand as the user drags
          a member of a group.
        </p>
        <p>
          When a drop occurs on a Group or a regular Node, the <a>GraphObject.MouseDrop</a> event handler
          adds the selection (the dragged Nodes) to the Group as a new member.
          The <a>Diagram.MouseDrop</a> event handler changes the dragged selected Nodes to be top-level,
          rather than members of whatever Groups they had been in.
        </p>
        <p>
          The slider controls how many nested levels of Groups are expanded. </br>
          Semantic zoom level:
        </p>
      ";
    }

    public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }

    public class NodeData : Model.NodeData {
      public bool Horiz { get; set; }
      public string Color { get; set; }
    }

    public class LinkData : Model.LinkData { }

    /*public partial class RegroupingPage : ComponentBase {
      private DiagramComponent myDiagramComponent;
      private Diagram myDiagram;
      private PaletteComponent myPaletteComponent;
      private Palette myPalette;

      private Part sharedNodeTemplate;
      private Group sharedGroupTemplate;

      private int sliderValue = 3;

    } */

    private void SaveModel() {
      if (myDiagram == null) return;
      saveLoadModel1.ModelJson = myDiagram.Model.ToJson();
    }

    private void LoadModel() {
      if (myDiagram == null) return;
      myDiagram.Model = Model.FromJson<Model>(saveLoadModel1.ModelJson);
      myDiagram.Model.UndoManager.IsEnabled = true;
    }

    private void DefineNodeTemplate() {
      if (sharedNodeTemplate != null) return;  // already defined

      sharedNodeTemplate =
        new Node("Auto") {
          // dropping on a Node is the same as dropping on its containing Group, even if it's top-level
          MouseDrop = (e, nod) => _FinishDrop(e, (nod as Node).ContainingGroup)
        }
          .Add(
            new Shape {
              Figure = "Rectangle",
              Fill = "#ACE600", Stroke = null
            }
              .Bind("Fill", "Color"),
            new TextBlock {
              Margin = 5,
              Editable = true,
              Font = "bold 13px sans-serif",
              Opacity = 0.75,
              Stroke = "#404040"
            }
              .Bind(new Binding("Text", "Text").MakeTwoWay())
          );
    }

    // The one template for Groups can be configured to be either layout out its members
    // horizontally or vertically, each with a different default color.
    private void DefineGroupTemplate() {
      if (sharedGroupTemplate != null) return;  // already defined

      Layout makeLayout(bool horiz) {
        if (horiz) {
          return new GridLayout {
            WrappingWidth = double.PositiveInfinity, Alignment = GridAlignment.Position,
            CellSize = new Size(1, 1), Spacing = new Size(4, 4)
          };
        } else {
          return new GridLayout {
            WrappingColumn = 1, Alignment = GridAlignment.Position,
            CellSize = new Size(1, 1), Spacing = new Size(4, 4)
          };
        }
      }

      string defaultColor(bool horiz) {
        return horiz ? "#FFDD33" : "#33D3E5";
      }

      string defaultFont(bool horiz) {
        return horiz ? "bold 18px sans-serif" : "bold 16px sans-serif";
      }

      // this function is used to highlight a Group that the selection may be dropped into
      void highlightGroup(InputEvent e, Group grp, bool show) {
        if (grp == null) return;
        e.Handled = true;
        if (show) {
          // cannot depend on the grp.Diagram.Selection in the case of external drag-and-drops;
          // instead depend on the DraggingTool.DraggedParts or .CopiedParts
          var tool = grp.Diagram.ToolManager.DraggingTool;
          var map = tool.DraggedParts ?? tool.CopiedParts;
          // now we can check to see if the Group will accept membership of the dragged Parts
          if (grp.CanAddMembers(map.Keys)) {
            grp.IsHighlighted = true;
            return;
          }
        }
        grp.IsHighlighted = false;
      }

      sharedGroupTemplate =
        new Group("Auto") {
          Background = "transparent",
          // highlight when dragging into the Group
          MouseDragEnter = (e, grp, prev) => highlightGroup(e, grp as Group, true),
          MouseDragLeave = (e, grp, next) => highlightGroup(e, grp as Group, false),
          ComputesBoundsAfterDrag = true,
          // when the selection is dropped into a Group, add the selected Parts into that Group;
          // if it fails, cancel the tool, rolling back any changes
          MouseDrop = _FinishDrop,
          HandlesDragDropForMembers = true,  // don't need to define handlers on member Nodes and Links
                                             // Groups containing Groups lay out their members Horizontally
          Layout = makeLayout(false)
        }
          .Bind(
            new Binding("Layout", "Horiz", (h) => makeLayout((bool)h)),
            new Binding("Background", "IsHighlighted", (h) => (bool)h ? "rgba(255,0,0,0.2)" : "transparent").OfElement()
          )
          .Add(
            new Shape {
              Figure = "Rectangle",
              Fill = null, Stroke = defaultColor(false), StrokeWidth = 2
            }
              .Bind(
                new Binding("Stroke", "Horiz", (h) => defaultColor((bool)h)),
                new Binding("Stroke", "Color")
              ),
            new Panel("Vertical")  // title above Placeholder
              .Add(
                new Panel("Horizontal") { // button next to TextBlock
                  Stretch = Stretch.Horizontal, Background = defaultColor(false)
                }
                  .Bind(
                    new Binding("Background", "Horiz", (h) => defaultColor((bool)h)),
                    new Binding("Background", "Color")
                  )
                  .Add(
                    Builder.Make<Panel>("SubGraphExpanderButton")
                      .Set(new { Alignment = Spot.Right, Margin = 5 }),
                    new TextBlock {
                      Alignment = Spot.Left,
                      Editable = true,
                      Margin = 5,
                      Font = defaultFont(false),
                      Opacity = 0.75,  // allow some color to show through
                      Stroke = "#404040"
                    }
                      .Bind(
                        new Binding("Font", "Horiz", (h) => defaultFont((bool)h)),
                        new Binding("Text", "Text").MakeTwoWay()
                      )
                  ), // end Horizontal Panel
                new Placeholder { Padding = 5, Alignment = Spot.TopLeft }
              ) // end Vertical Panel
          );
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

     

      // what to do when a drag-drop occurs on the Background
      myDiagram.MouseDrop = (e) => _FinishDrop(e, null);
      myDiagram.Layout = new GridLayout {
        WrappingWidth = double.PositiveInfinity,
        Alignment = GridAlignment.Position,
        CellSize = new Size(1, 1)
      };
      myDiagram.CommandHandler.ArchetypeGroupData = new NodeData {
        IsGroup = true,
        Text = "Group"
      };

      myDiagram.UndoManager.IsEnabled = true;

      DefineNodeTemplate();
      myDiagram.NodeTemplate = sharedNodeTemplate;
      DefineGroupTemplate();
      myDiagram.GroupTemplate = sharedGroupTemplate;

      LoadModel();
    }

    private void SetupPalette() {
      myPalette = paletteControl1.Diagram as Palette;
      // initialize the Palette and its contents
      DefineNodeTemplate();
      myPalette.NodeTemplate = sharedNodeTemplate;
      DefineGroupTemplate();
      myPalette.GroupTemplate = sharedGroupTemplate;
      myPalette.Layout = new GridLayout {
        WrappingColumn = 1,
        Alignment = GridAlignment.Position
      };

      myPalette.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Text = "lightgreen", Color = "#ACE600" },
          new NodeData { Text = "yellow", Color = "#FFDD33" },
          new NodeData { Text = "lightblue", Color = "#33D3E5" },
          new NodeData { Text = "H Group", Color = "#FFDD33", IsGroup = true },
          new NodeData { Text = "V Group", Color = "#33D3E5", IsGroup = true}
        }
      };
    }

    // Upon a drop onto a Group, we try to add the selection as members of the Group.
    // Upon a drop onto the background, or onto a top-level Node, make selection top-level.
    // If this is OK, we're done; otherwise we cancel the operation to rollback everything.
    private void _FinishDrop(InputEvent e, GraphObject grp) {
      var ok = (grp != null
        ? (grp as Group).AddMembers(grp.Diagram.Selection, true)
        : e.Diagram.CommandHandler.AddTopLevelParts(e.Diagram.Selection, true));
      if (!ok) e.Diagram.CurrentTool.DoCancel();
    }

    private void Reexpand() { 
      sliderValue = trackBar1.Value;
      myDiagram.Commit((d) => {
        var tlg = d.FindTopLevelGroups();
        while (tlg.MoveNext()) {
          var g = tlg.Current;
          ExpandGroups(g, 0, sliderValue);
        }
      }, "reexpand"); 
    }

    

    private void ExpandGroups(Part p, int i, int level) {
      if (!(p is Group g)) return;
      g.IsSubGraphExpanded = i < level;
      foreach (var m in g.MemberParts) {
        ExpandGroups(m, i + 1, level);
      }
    }
  }
}
