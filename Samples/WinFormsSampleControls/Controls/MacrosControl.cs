/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.WinForms;

namespace WinFormsSampleControls.Macros {
  [ToolboxItem(false)]
  public partial class MacrosControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    private Palette myPalette;
    private Part sharedNodeTemplate;
    private Part sharedGroupTemplate;

    public MacrosControl() {
      InitializeComponent();

      Setup();
      SetupPalette();

      goWebBrowser1.Html = @"
    <p>
    When one drags the ""Macro"" node (actually a <a>Group</a>) from the Palette into the main Diagram,
    the ""ExternalObjectsDropped"" <a>DiagramEvent</a> listener automatically ungroups that group node
    to show all of its members nodes and links that were copied by the drag-and-drop.
    </p>

    <p>
    Note also that a drop causes the <a>TextEditingTool</a> to automatically start editing the text in the node.
    </p>
";

    }

    private void DefineNodeTemplates() {
      if (sharedNodeTemplate != null) return;  // already defined

      sharedNodeTemplate = new Node(PanelLayoutAuto.Instance) { // the default category
        // The node.Location comes from the "Loc" property of the node data,
        // converted by the Point.Parse method.
        // If the Node.Location is changed, it updates the "Loc" property,
        // converting back using the Point.Stringify method.
      }.Bind("Location", "Loc", Point.Parse, Point.Stringify)
       .Add(
         new Shape {
           Figure = "Rectangle",
           Fill = "white",
           StrokeWidth = 2,
           PortId = "",
           FromLinkable = true,
           ToLinkable = true,
           Cursor = "pointer"
         }.Bind("Stroke", "Color"),
         new TextBlock {
           MaxSize = new Size(150, double.NaN),
           TextAlign = TextAlign.Center,
           Margin = 6,
           Editable = true,
           Name = "TEXT",
           Font = new Font("Segoe UI", 16)
         }.Bind(new Binding("Text", "Text").MakeTwoWay())
      );

      sharedGroupTemplate = new Group(PanelLayoutAuto.Instance) {
        IsSubGraphExpanded = false, // only show the group itself, not any of its members
        Ungroupable = true
      }.Add( // allow the ExternalObjectsDropped event handler to ungroup
             // the members to be top-level parts, via a command
        new Shape {
          Figure = "Rectangle", // the rectangular shape around the members
          Fill = "rgba(128,128,128,0.2)",
          Stroke = "gray",
          StrokeWidth = 3
        },
        new Placeholder { Alignment = Spot.TopLeft },
        new TextBlock {
          Font = new Font("Segoe UI", 16, FontWeight.Bold), Margin = 10
        }.Bind("Text", "Text")
      );

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      myDiagram.UndoManager.IsEnabled = true;

      DefineNodeTemplates();
      myDiagram.NodeTemplate = sharedNodeTemplate;
      myDiagram.GroupTemplate = (Group)sharedGroupTemplate;

      myDiagram.LinkTemplate = new Link().Add(
        new Shape { StrokeWidth = 1.5 },
        new Shape { ToArrow = "Standard", Stroke = null }
      );

      // this DiagramEvent is raised when the user has drag-and-dropped something
      // from another Diagram (a Palette in this case) into this Diagram
      myDiagram.ExternalElementsDropped += (sender, e) => {
        // expand any macros
        myDiagram.CommandHandler.UngroupSelection();
        // start editing the first ndoe that was dropped after ungrouping
        var tb = myDiagram.Selection.FirstOrDefault().FindElement("TEXT");
        if (tb is TextBlock _tb) myDiagram.CommandHandler.EditTextBlock(_tb);
      };


      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData>()
      };

      // Update JSON if model changes
      myDiagram.ModelChanged += (obj, e) => {
        if (e.IsTransactionFinished) {  // show the model data in the page's TextArea
          modelJson1.JsonText = myDiagram.Model.ToJson();
        }
      };

    }

    private void SetupPalette() {
      myPalette = paletteControl1.Diagram as Palette;
      DefineNodeTemplates();
      myPalette.NodeTemplate = sharedNodeTemplate;
      myPalette.GroupTemplate = (Group)sharedGroupTemplate;

      myPalette.Model = new Model {
        NodeDataSource = new List<NodeData> {
          // a regular node
          new NodeData { Key = "B", Text = "some block", Color = "blue" },
          // a group node, plus three member nodes in that group
          new NodeData { Key = "G", Text = "Macro", IsGroup = true },
          new NodeData { Key = "Ga", Text = "A block", Color = "green", Group = "G", Loc = "0 0" },
          new NodeData { Key = "Gb", Text = "B block", Color = "orange", Group = "G", Loc = "150 25" },
          new NodeData { Key = "Gc", Text = "C block", Color = "red", Group = "G", Loc = "50 100" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = "Ga", To = "Gb" },
          new LinkData { From = "Ga", To = "Gc" },
          new LinkData { From = "Gb", To = "Gc" }
        }
      };
    }

  }

  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }

  public class NodeData : Model.NodeData {
    public string Color { get; set; }
    public string Loc { get; set; }
  }

  public class LinkData : Model.LinkData { }

}
