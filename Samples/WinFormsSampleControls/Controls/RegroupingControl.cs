/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;

namespace WinFormsSampleControls.Regrouping {
  [ToolboxItem(false)]
  public partial class RegroupingControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    private Palette myPalette;

    private Part sharedNodeTemplate;
    private Group sharedGroupTemplate;

    public RegroupingControl() {
      InitializeComponent();
      myDiagram = diagramControl1.Diagram;
      myPalette = paletteControl1.Diagram as Palette;

      DefineTemplates();

      modelJson1.SaveClick += (e, obj) => SaveModel();
      modelJson1.LoadClick += (e, obj) => LoadModel();

      trackBar1.ValueChanged += (obj, e) => Reexpand();
      modelJson1.JsonText = @"
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

      Setup();
      SetupPalette();
    }


    private void DefineTemplates() {
      // The one template for Groups can be configured to be either layout out its members
      // horizontally or vertically, each with a different default color.

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
        return horiz ? "rgba(255, 221, 51, 0.55)" : "rgba(51,211,229, 0.5)";
      }

      Font defaultFont(bool horiz) {
        return horiz ? new Font("Segoe UI", 20, FontWeight.Bold) : new Font("Segoe UI", 16, FontWeight.Bold);
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
            Background = "blue",
            Ungroupable = true,
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
            new Shape { Stroke = defaultColor(false), Fill = defaultColor(false), StrokeWidth = 2 }
              .Bind(
                new Binding("Stroke", "Horiz", (h) => defaultColor((bool)h)),
                new Binding("Fill", "Horiz", (h) => defaultColor((bool)h))
              ),
            new Panel("Vertical")  // title above Placeholder
              .Add(
                new Panel("Horizontal") {  // button next to TextBlock
                    Stretch = Stretch.Horizontal, Background = defaultColor(false)
                  }
                  .Bind("Background", "Horiz", (h) => defaultColor((bool)h))
                  .Add(
                    Builder.Make<Panel>("SubGraphExpanderButton")
                      .Set(new { Alignment = Spot.Right, Margin = 5 }),
                    new TextBlock {
                        Alignment = Spot.Left,
                        Editable = true,
                        Margin = 5,
                        Font = defaultFont(false),
                        Opacity = 0.90,  // allow some color to show through
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

      sharedNodeTemplate =
        new Node("Auto") {
            // dropping on a Node is the same as dropping on its containing Group, even if it's top-level
            MouseDrop = (e, node) => _FinishDrop(e, (node as Node).ContainingGroup)
          }
          .Add(
            new Shape("RoundedRectangle") { Fill = "rgba(172, 230, 0, 0.9)", Stroke = "white", StrokeWidth = .5 },
            new TextBlock {
                Margin = 7,
                Editable = true,
                Font = new Font("Segoe UI", 13, FontWeight.Bold),
                Opacity = 0.90,
                Stroke = "#404040"
              }
              .Bind(new Binding("Text", "Text").MakeTwoWay())
          );
    }

    private void Setup() {
      // when a drag-drop occurs in the Diagram's background, make it a top-level node
      myDiagram.MouseDrop = (e) => _FinishDrop(e, null);
      // Diagram has simple horizontal layout
      myDiagram.Layout = new GridLayout {
        WrappingWidth = double.PositiveInfinity,
        Alignment = GridAlignment.Position,
        CellSize = new Size(1, 1)
      };
      myDiagram.CommandHandler.ArchetypeGroupData = new NodeData {
        IsGroup = true, Text = "Group", Horiz = false
      };
      myDiagram.UndoManager.IsEnabled = true;

      myDiagram.NodeTemplate = sharedNodeTemplate;
      myDiagram.GroupTemplate = sharedGroupTemplate;

      LoadModel();
    }

    private void SetupPalette() {
      // initialize the Palette and its contents
      myPalette.NodeTemplate = sharedNodeTemplate;
      myPalette.GroupTemplate = sharedGroupTemplate;

      myPalette.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Text = "New Node" },
          new NodeData { IsGroup = true, Text = "H Group", Horiz = true },
          new NodeData { IsGroup = true, Text = "V Group", Horiz = false }
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
      myDiagram.Commit((d) => {
        var tlg = d.FindTopLevelGroups();
        while (tlg.MoveNext()) {
          var g = tlg.Current;
          ExpandGroups(g, 0, trackBar1.Value);
        }
      }, "reexpand");
    }

    private void ExpandGroups(Part p, int i, int level) {
      if (p is not Group g) return;
      g.IsSubGraphExpanded = i < level;
      foreach (var m in g.MemberParts) {
        ExpandGroups(m, i + 1, level);
      }
    }

    private void SaveModel() {
      if (myDiagram == null) return;
      modelJson1.JsonText = myDiagram.Model.ToJson();
    }

    private void LoadModel() {
      if (myDiagram == null) return;
      myDiagram.Model = Model.FromJson<Model>(modelJson1.JsonText);
    }

    public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }

    public class NodeData : Model.NodeData {
      public bool Horiz { get; set; }
    }

    public class LinkData : Model.LinkData { }
  }
}
