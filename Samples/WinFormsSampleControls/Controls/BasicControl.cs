using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.Basic {
  [ToolboxItem(false)]
  public partial class BasicControl : System.Windows.Forms.UserControl {
    private Diagram _Diagram;

    public BasicControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;
      goWebBrowser1.Html = @"
        <p>
          This sample demonstrates tooltips and context menus for all parts and for the diagram background,
          as well as several other powerful <a>Diagram</a> editing abilities.
        </p>
        <p>
          Unlike the <a href=""Minimal"">Minimal</a> sample, this sample has templates for Links and for Groups,
          plus tooltips and context menus for Nodes, for Links, for Groups, and for the Diagram.
        </p>
        <p>This sample has all of the functionality of the Minimal sample, but additionally allows the user to:</p>
        <ul>
          <li>create new nodes: double-click in the background of the diagram</li>
          <li>edit text: select the node and then click on the text, or select the node and press F2</li>
          <li>draw new links: drag from the inner edge of the node's or the group's shape</li>
          <li>reconnect existing links: select the link and then drag the diamond-shaped handle at either end of the link</li>
          <li>group nodes and links: select some nodes and links and then type Ctrl-G (or invoke via context menu)</li>
          <li>ungroup an existing group: select a group and then type Ctrl-Shift-G (or invoke via context menu)</li>
        </ul>
        <p>
          GoDiagram contains many other possible commands, which can be invoked by either mouse/keyboard/touch or programatically.
          <a href=""intro/commands.html"">See an overview of possible commands here.</a>
          On a Mac, use CMD instead of Ctrl.
        </p>
        <p>
          On touch devices, hold your finger stationary to bring up a context menu.
          The default context menu supports most of the standard commands that are enabled at that time for that object.
        </p>
      ";
    }

    private void Setup() {
      _Diagram = diagramControl1.Diagram;

      // diagram properties

      // allow double-click in background to create a new node
      _Diagram.ToolManager.ClickCreatingTool.ArchetypeNodeData = new NodeData { Text = "Node", Color = "gray" };
      // allow Ctrl-G to call GroupSelection()
      _Diagram.CommandHandler.ArchetypeGroupData = new NodeData { Text = "Group", IsGroup = true, Color = "blue" };
      // enable undo and redo
      _Diagram.UndoManager.IsEnabled = true;

      // for calling alert
      void Alert(string str) {
        var result = System.Windows.Forms.MessageBox.Show(str);
      }


      // Define the appearance and behavior for Nodes:

      // First, define the shared context menu for all Nodes, Links, and Groups.

      // To simplify this code we define a function for creating a context menu button:
      Panel MakeButton(string text, Action<InputEvent, GraphObject> action, Func<GraphObject, bool> visiblePredicate = null) {
        object converter(object obj) {
          var elt = obj as GraphObject;
          return elt.Diagram != null && visiblePredicate(elt);
        }

        return Builder.Make<Panel>("ContextMenuButton")
          .Add(new TextBlock(text))
          .Set(new { Click = action })
          .Bind(visiblePredicate != null ? new Binding("Visible", "", converter).OfElement() : null);
      }

      // a context menu is an Adornment with a bunch of buttons in them
      var partContextMenu =
        Builder.Make<Adornment>("ContextMenu")
          .Add(
            MakeButton("Properties",
              (e, obj) => {  // OBJ is this Button
                var contextmenu = obj.Part as Adornment;  // the Button is in the context menu Adornment
                var part = contextmenu.AdornedPart;  // the adornedPart is the Part that the context menu adorns
                // now can do something with PART, or with its data, or with the Adornment (the context menu)
                if (part is Link l) Alert(LinkInfo(l.Data));
                else if (part is Group) Alert(GroupInfo(contextmenu));
                else Alert(NodeInfo(part.Data));
              }),
            MakeButton("Cut",
              (e, obj) => { e.Diagram.CommandHandler.CutSelection(); },
              (o) => { return o.Diagram.CommandHandler.CanCutSelection(); }),
            MakeButton("Copy",
              (e, obj) => { e.Diagram.CommandHandler.CopySelection(); },
              (o) => { return o.Diagram.CommandHandler.CanCopySelection(); }),
            MakeButton("Paste",
              (e, obj) => { e.Diagram.CommandHandler.PasteSelection(e.Diagram.ToolManager.ContextMenuTool.MouseDownPoint); },
              (o) => { return o.Diagram.CommandHandler.CanPasteSelection(o.Diagram.ToolManager.ContextMenuTool.MouseDownPoint); }),
            MakeButton("Delete",
              (e, obj) => { e.Diagram.CommandHandler.DeleteSelection(); },
              (o) => { return o.Diagram.CommandHandler.CanDeleteSelection(); }),
            MakeButton("Undo",
              (e, obj) => { e.Diagram.CommandHandler.Undo(); },
              (o) => { return o.Diagram.CommandHandler.CanUndo(); }),
            MakeButton("Redo",
              (e, obj) => { e.Diagram.CommandHandler.Redo(); },
              (o) => { return o.Diagram.CommandHandler.CanRedo(); }),
            MakeButton("Group",
              (e, obj) => { e.Diagram.CommandHandler.GroupSelection(); },
              (o) => { return o.Diagram.CommandHandler.CanGroupSelection(); }),
            MakeButton("Ungroup",
              (e, obj) => { e.Diagram.CommandHandler.UngroupSelection(); },
              (o) => { return o.Diagram.CommandHandler.CanUngroupSelection(); })
          );

      string NodeInfo(object d, object o = null) {  // Tooltip info for a node data object
        var nd = d as NodeData;
        var str = "Node " + nd.Key + ": " + nd.Text + "\n";
        if (nd.Group != default) {
          str += "member of " + nd.Group;
        } else {
          str += "top-level node";
        }
        return str;
      }

      // These nodes have text surrounded by a rounded rectangle
      // whose fill color is bound to the node data.
      // The user can drag a node by dragging its TextBlock label.
      // Dragging from the Shape will start drawing a new link.
      _Diagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance) {
          LocationSpot = Spot.Center,
          // this tooltip Adornment is shared by all nodes
          ToolTip =
              Builder.Make<Adornment>("ToolTip").Add(  // the tooltip shows the result of calling NodeInfo(data)
                new TextBlock { Margin = 4 }.Bind("Text", "", NodeInfo)
              ),
          // this context menu Adornment is shared by all nodes
          ContextMenu = partContextMenu
        }
          .Add(
            new Shape("RoundedRectangle") {
                Fill = "white", // the default fill, if there is no data bound value
                PortId = "", Cursor = "pointer",  // the Shape is the port, not the whole Node
                // allow all kinds of links from and to this port
                FromLinkable = true, FromLinkableSelfNode = true, FromLinkableDuplicates = true,
                ToLinkable = true, ToLinkableSelfNode = true, ToLinkableDuplicates = true
              }
              .Bind("Fill", "Color"),
            new TextBlock {
                Font = "Segoe UI, 14px, style=bold",
                Stroke = "#333",
                Margin = 6,  // make some extra space for the shape around the text
                IsMultiline = false,  // don't allow newlines in text
                Editable = true  // allow in-place editing by user
              }
              .Bind(new Binding("Text").MakeTwoWay())  // the label shows the node data's text
          );

      // Define the appearance and behavior for Links:

      string LinkInfo(object d, object o = null) {  // Tooltip info for a link data object
        var ld = d as LinkData;
        return "Link " + ld.Key + ":\nfrom " + ld.From + " to " + ld.To;
      }

      // The link shape and arrowhead have their stroke brush data bound to the "color" property
      _Diagram.LinkTemplate =
        new Link {
            ToShortLength = 3,
            // allow the user to relink existing links
            RelinkableFrom = true,
            RelinkableTo = true,
            // this tooltip Adornment is shared by all links
            ToolTip =
                Builder.Make<Adornment>("ToolTip").Add(  // the tooltip shows the result of calling LinkInfo(data)
                  new TextBlock { Margin = 4 }.Bind("Text", "", LinkInfo)
                ),
            // the same context menu Adornment is shared by all links
            ContextMenu = partContextMenu
          }
          .Add(  // allow the user to relink existing links
            new Shape { StrokeWidth = 2 }.Bind("Stroke", "Color"),
            new Shape { ToArrow = "Standard", Stroke = null }.Bind("Fill", "Color")
          );

      // Define the appearance and behavior for Groups:

      string GroupInfo(object a, object o = null) {  // takes the tooltip or context menu, not a group node data object
        var adorn = a as Adornment;
        var g = adorn.AdornedPart as Group;  // get the Group that the tooltip adorns
        var data = g.Data as NodeData;
        var mems = g.MemberParts.Count;
        var links = 0;
        foreach (var part in g.MemberParts) {
          if (part is Link) links++;
        }
        return "Group " + data.Key + ": " + data.Text + "\n" + mems + " members including " + links + " links";
      }

      // Groups consist of a title in the color given by the group node data
      // above a translucent gray rectangle surrounding the member parts
      _Diagram.GroupTemplate =
        new Group(PanelLayoutVertical.Instance) {
            SelectionElementName = "PANEL",  // selection handle goes around shape, not label
            Ungroupable = true,  // enable Ctrl-Shift-G to ungroup a selected Group
            // this tooltip Adornment is shared by all groups
            ToolTip =
                Builder.Make<Adornment>("ToolTip")
                  .Add(
                    new TextBlock { Margin = 4 }
                      .Bind(
                        // bind to tooltip, not to Group.Data, to allow access to Group properties
                        new Binding("Text", "", GroupInfo).OfElement()
                      )
                  ),
            // the same context menu Adornment is shared by all groups
            ContextMenu = partContextMenu
          }
          .Add(
            new TextBlock {
                Font = "Segoe UI, 19px, style=bold",
                IsMultiline = false,  // don't allow newlines in text
                Editable = true  // allow in-place editing by user
              }
              .Bind(
                new Binding("Text").MakeTwoWay(),
                new Binding("Stroke", "Color")
              ),
            new Panel(PanelLayoutAuto.Instance) { Name = "PANEL" }
              .Add(
                new Shape("Rectangle") {  // the rectangular shape around the members
                    Fill = "rgba(128,128,128,0.2)", Stroke = "gray", StrokeWidth = 3,
                    PortId = "", Cursor = "pointer",  // the Shape is the port, not the whole Node
                    // allow all kinds of links from and to this port
                    FromLinkable = true, FromLinkableSelfNode = true, FromLinkableDuplicates = true,
                    ToLinkable = true, ToLinkableSelfNode = true, ToLinkableDuplicates = true
                  },
                new Placeholder { Margin = 10, Background = "transparent" }  // represents where the members are
              )
          );

      // Define the behavior for the Diagram background:

      string DiagramInfo(object m, object o = null) {  // Tooltip info for the diagram's model
        var model = m as Model;
        return "Model:\n" + model.NodeDataSource.Count() + " nodes, " + model.LinkDataSource.Count() + " links";
      }

      // provide a tooltip for the background of the Diagram, when not over any Part
      _Diagram.ToolTip =
        Builder.Make<Adornment>("ToolTip")
          .Add(
            new TextBlock { Margin = 4 }.Bind("Text", "", DiagramInfo)
          );

      // provide a context menu for the background of the Diagram, when not over any Part
      _Diagram.ContextMenu =
        Builder.Make<Adornment>("ContextMenu")
          .Add(
            MakeButton("Paste",
              (e, obj) => { e.Diagram.CommandHandler.PasteSelection(e.Diagram.ToolManager.ContextMenuTool.MouseDownPoint); },
              (o) => { return o.Diagram.CommandHandler.CanPasteSelection(o.Diagram.ToolManager.ContextMenuTool.MouseDownPoint); }),
            MakeButton("Undo",
              (e, obj) => { e.Diagram.CommandHandler.Undo(); },
              (o) => { return o.Diagram.CommandHandler.CanUndo(); }),
            MakeButton("Redo",
              (e, obj) => { e.Diagram.CommandHandler.Redo(); },
              (o) => { return o.Diagram.CommandHandler.CanRedo(); })
          );

      // Create the Diagram's Model:
      _Diagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = 1, Text = "Alpha", Color = "lightblue" },
          new NodeData { Key = 2, Text = "Beta", Color = "orange" },
          new NodeData { Key = 3, Text = "Gamma", Color = "lightgreen", Group = 5 },
          new NodeData { Key = 4, Text = "Delta", Color = "pink", Group = 5 },
          new NodeData { Key = 5, Text = "Epsilon", Color = "green", IsGroup = true }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = 1, To = 2, Color = "blue" },
          new LinkData { From = 2, To = 2 },
          new LinkData { From = 3, To = 4, Color = "green" },
          new LinkData { From = 3, To = 1, Color = "purple" }
        }
      };
    }
  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Color { get; set; }
  }

  public class LinkData : Model.LinkData {
    public string Color { get; set; }
  }
}
