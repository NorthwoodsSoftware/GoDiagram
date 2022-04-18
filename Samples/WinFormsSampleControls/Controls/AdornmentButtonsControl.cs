/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;

namespace WinFormsSampleControls.AdornmentButtons {
  [ToolboxItem(false)]
  public partial class AdornmentButtonsControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public AdornmentButtonsControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"

  <p>
    The node template uses a custom <a>Part.SelectionAdornmentTemplate</a> to
    add a row of Buttons when the node is selected.
    Select a node and you will see the Buttons for the node.
  </p>
  <p>
    The first button, ""T"", when clicked, starts in-place editing of the text.
  </p>
  <p>
    The second button, ""C"", when clicked, changes the color of the node,
    rotating through a list of colors.
  </p>
  <p>
    The third button, ""L"", when clicked or dragged, starts the <a>LinkingTool</a>,
    drawing a new link starting at the selected node.
  </p>
  <p>
    The fourth button, ""N"", when clicked, adds a new node and creates a link from
    the selected node to the new node.
    Dragging from the fourth button does the same thing as a click but also activates
    the <a>DraggingTool</a>, allowing the user to drag the new node where they like.
    </p>
";

    }

    // used by NextColor as the list of colors through which we rotate
    private static Brush[] myColors = new Brush[] {
      "lightgray", "lightblue", "lightgreen", "yellow", "orange", "pink"
    };

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.ToolManager.LinkingTool.IsEnabled = false;  // invoked explicitly by drawLink function, below
      myDiagram.ToolManager.LinkingTool.Direction = LinkingDirection.ForwardsOnly;  // only draw "from" towards "to"
      myDiagram.UndoManager.IsEnabled = true; // enable undo & redo
      myDiagram.ToolManager.DraggingTool = new AdornmentButtonsDraggingTool();

      // link template
      myDiagram.LinkTemplate =
        new Link { Routing = LinkRouting.AvoidsNodes, Corner = 5 }.Add(
          new Shape { StrokeWidth = 1.5 },
          new Shape { ToArrow = "OpenTriangle" }
        );

      // node template
      myDiagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance) {
          DesiredSize = new Size(80, 80),
          // rearrange the link points evenly along the sides of the nodes as links are
          // drawn or reconnected -- these event handlers only make sense when the fromSpot
          // and toSpot are Spot.XxxSides
          LinkConnected = (node, link, port, connected) => {
            if (connected) {
              if (link.FromNode != null) link.FromNode.InvalidateConnectedLinks();
              if (link.ToNode != null) link.ToNode.InvalidateConnectedLinks();
            } else {
              if (link.FromNode != null) link.FromNode.InvalidateConnectedLinks();
              if (link.ToNode != null) link.ToNode.InvalidateConnectedLinks();
            }
          },
          LocationSpot = Spot.Center
        }.Bind(new Binding("Location", "Loc", Point.Parse).MakeTwoWay(Point.Stringify))
          .Add(new Shape {
            Name = "SHAPE",  // named so that changeColor can modify it
            StrokeWidth = 0,  // no border
            Fill = "lightgray",  // default fill color
            PortId = "",
            // use the following property if you want users to draw new links
            // interactively by dragging from the Shape, and re-enable the LinkingTool
            // in the initialization of the Diagram
            //cursor = "pointer",
            FromSpot = Spot.AllSides,
            FromLinkable = true,
            FromLinkableDuplicates = true,
            FromLinkableSelfNode = true,
            ToSpot = Spot.AllSides,
            ToLinkable = true,
            ToLinkableDuplicates = true,
            ToLinkableSelfNode = true
          }.Bind(
            new Binding("Fill", "Color").MakeTwoWay((data, srcData, model) => {
              var brush = (Brush)data;
              return brush;
            })),
          new TextBlock {
            Name = "TEXTBLOCK",  // named so that editText can start editing it
            Margin = 3,
            // use the following property if you want users to interactively start
            // editing the text by clicking on it or by F2 if the node is selected:
            //editable = true,
            Overflow = Overflow.Ellipsis,
            MaxLines = 5
          }.Bind(
            new Binding("Text").MakeTwoWay())
        );

      // a selected node shows an Adornment that includes both a blue border
      // and a row of Buttons above the node

      // define buttons
      var button1 = Builder.Make<Panel>("Button").Add( // edit text
        new TextBlock {
          Text = "t",
          Font = new Font("Segoe UI", 13, FontWeight.Bold),
          DesiredSize = new Size(15, 15),
          TextAlign = TextAlign.Center
        }
      );
      button1.Click = EditText;
      var button2 = Builder.Make<Panel>("Button").Add( // change color
        new Shape {
          Fill = (Brush)null,
          Stroke = (Brush)null,
          DesiredSize = new Size(14, 14)
        }
      ) as Panel;
      // bind border fill
      button2.FindElement("ButtonBorder").Bind(
        new Binding("Fill", "Color", NextColor)
      );
      button2.Click = ChangeColor;
      button2["_ButtonFillOver"] = "transparent"; // defined below, to support changing the color of the node
      var button3 = Builder.Make<Panel>("Button").Add( // draw links
        new Shape { GeometryString = "M0 0 L8 0 8 12 14 12 M12 10 L14 12 12 14" }
      );
      button2.Name = "BUTTON2";
      button3.Click = DrawLink;
      button3.ActionMove = DrawLink;
      var button4 = Builder.Make<Panel>("Button").Add( // make new node
        new Shape { GeometryString = "M0 0 L3 0 3 10 6 10 x F1 M6 6 L14 6 14 14 6 14z", Fill = "gray" }
      );
      button4.ActionMove = DragNewNode;
      button4.Click = ClickNewNode;
      button4["_DragData"] = new NodeData {
        Text = "a Node",
        Color = "lightgray"
      };
      myDiagram.NodeTemplate.SelectionAdornmentTemplate =
        new Adornment(PanelLayoutSpot.Instance).Add(
          new Panel(PanelLayoutAuto.Instance).Add(
            new Shape { Stroke = "dodgerblue", StrokeWidth = 2, Fill = (Brush)null },
            new Placeholder()
          ),
          new Panel(PanelLayoutHorizontal.Instance) {
            Alignment = Spot.Top,
            AlignmentFocus = Spot.Bottom
          }.Add(
            button1, // edit text
            button2, // change color
            button3, // draw links
            button4  // make new node
          )
        );

      // model data
      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = 1, Text = "Alpha", Color = "lightblue", Loc = "0 0" },
          new NodeData { Key = 2, Text = "Beta", Color = "orange", Loc = "140 0" },
          new NodeData { Key = 3, Text = "Gamma", Color = "lightgreen", Loc = "0 140" },
          new NodeData { Key = 4, Text = "Delta", Color = "pink", Loc = "140 140" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = 1, To = 2 }
        }
      };


      // functions for buttons

      void EditText(InputEvent e, GraphObject button) {
        var node = (button.Part as Adornment).AdornedPart as Node;
        e.Diagram.CommandHandler.EditTextBlock(node.FindElement("TEXTBLOCK") as TextBlock);
      }

      // used by both the Button Binding and by the changeColor click function
      object NextColor(object cAsObject, object _) {
        var c = (Brush)cAsObject;
        var idx = Array.IndexOf(myColors, c);
        if (idx < 0) return "lightgray";
        if (idx >= myColors.Length - 1) idx = 0;
        return myColors[idx + 1];
      }

      void ChangeColor(InputEvent e, GraphObject button) {
        var node = (button.Part as Adornment).AdornedPart as Node;
        var shape = node.FindElement("SHAPE") as Shape;
        if (shape == null) return;
        node.Diagram.StartTransaction("Change color");
        shape.Fill = (Brush)NextColor(shape.Fill, null);
        button["_ButtonFillNormal"] = NextColor(shape.Fill, null);  // update the button too
        // DEBUG
        Console.Write("button fill: " + button["_ButtonFillNormal"]);
        // END DEBUG
        node.Diagram.CommitTransaction("Change color");
      }

      void DrawLink(InputEvent e, GraphObject button) {
        var node = (button.Part as Adornment).AdornedPart as Node;
        var tool = e.Diagram.ToolManager.LinkingTool;
        tool.StartElement = node.Port;
        e.Diagram.CurrentTool = tool;
        tool.DoActivate();
      }

      // used by both clickNewNode and dragNewNode to create a node and a link
      // from a given node to the new node
      Node CreateNodeAndLink(NodeData data, Node fromnode) {
        var diagram = fromnode.Diagram;
        var model = diagram.Model as Model;
        var nodedata = model.CopyNodeData(data);
        model.AddNodeData(nodedata);
        var newnode = diagram.FindNodeForData(nodedata);
        var linkdata = new LinkData();
        model.SetFromKeyForLinkData(linkdata, model.GetKeyForNodeData(fromnode.Data as NodeData));
        model.SetToKeyForLinkData(linkdata, model.GetKeyForNodeData(newnode.Data as NodeData));
        model.AddLinkData(linkdata);
        diagram.Select(newnode);
        return newnode;
      }

      // the Button.Click event handler, called when the user clicks the "N" button
      void ClickNewNode(InputEvent e, GraphObject button) {
        var data = button["_DragData"] as NodeData;
        if (data == null) return;
        e.Diagram.StartTransaction("Create Node and Link");
        var fromnode = (button.Part as Adornment).AdornedPart as Node;
        var newnode = CreateNodeAndLink(data, fromnode);
        newnode.Location = new Point(fromnode.Location.X + 200, fromnode.Location.Y);
        e.Diagram.CommitTransaction("Create Node and Link");
      }

      // the Button.ActionMove event handler, called when the user drags within the "N" button
      void DragNewNode(InputEvent e, GraphObject button) {
        var tool = e.Diagram.ToolManager.DraggingTool;
        if (tool.IsBeyondDragSize()) {
          var data = button["_DragData"] as NodeData;
          if (data == null) return;
          e.Diagram.StartTransaction("button drag");  // see doDeactivate, below
          var newnode = CreateNodeAndLink(data, (button.Part as Adornment).AdornedPart as Node);
          newnode.Location = e.Diagram.LastInput.DocumentPoint;
          // don't commitTransaction here, but in tool.DoDeactivate, after drag operation finished
          // set tool.CurrentPart to a selected movable Part and then activate the DraggingTool
          tool.CurrentPart = newnode;
          e.Diagram.CurrentTool = tool;
          tool.DoActivate();
        }
      }
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public Brush Color { get; set; }
    public string Loc { get; set; }
  }

  public class LinkData : Model.LinkData { }

  // using DragNewNode also requires modifying the standard DraggingTool so that it
  // only calls commitTransaction when dragNewNode started a "button drag" transaction;
  // do this by overriding DraggingTool.DoDeactivate:
  public class AdornmentButtonsDraggingTool : DraggingTool {
    public override void DoDeactivate() {
      // commit "button drag" transaction, if it is ongoing; see dragNewNode, above
      if (Diagram.UndoManager.NestedTransactionNames.ElementAt(0) == "button drag") {
        Diagram.CommitTransaction();
      }
      base.DoDeactivate();
    }
  }

}
