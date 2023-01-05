/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Extensions;
using Northwoods.Go.Layouts.Extensions;
using Northwoods.Go.Models;

namespace Demo.Samples.Flowgrammer {
  [ToolboxItem(false)]
  public partial class FlowgrammerControl : DemoControl {
    private Diagram myDiagram;
    private Palette myPalette;
    private Overview myOverview;

    private Dictionary<string, Part> sharedNodeTemplateMap;


    public FlowgrammerControl() {
      InitializeComponent();

      myDiagram = diagramControl1.Diagram;
      myPalette = paletteControl1.Diagram as Palette;
      myOverview = overviewControl1.Diagram as Overview;

      modelJson1.LoadClick = LoadModel;
      btnNewDiagram.Click += (e, obj) => NewDiagram();

      goWebBrowser1.Html = @"
        <p>
          The Flowgrammer sample demonstrates how one can build a flowchart with a constrained syntax.
          You can drag and drop Nodes onto Links and Nodes in the diagram in order to splice them into the graph.
          There is visual feedback during the dragging process.
          Nodes dropped onto the diagram's background are automatically deleted.
          Edit text by clicking on the text of selected nodes.
          Deleting an action or step Node excises it from the chain of steps that it is in.
          The ""For"", ""While"", and ""If"" are not deletable, but you can select and delete the Group holding the
          whole body of the loop or conditional.
          The ""Start"" and ""End"" nodes and Links are not deletable.
        </p>
        <p>
          The automatic layout of the diagram is accomplished with the <a>ParallelLayout</a> extension.
        </p>
      ";

      modelJson1.JsonText = @"{
  ""NodeDataSource"": [
    { ""Key"":1, ""Text"":""S"", ""Category"":""Start""},
    { ""Key"":-1, ""IsGroup"":true, ""Cat"":""For""},
    { ""Key"":2, ""Text"":""For Each"", ""Category"":""For"", ""Group"":-1},
    { ""Key"":3, ""Text"":""Action 1"", ""Group"":-1},
    { ""Key"":-2, ""IsGroup"":true, ""Cat"":""If"", ""Group"":-1},
    { ""Key"":4, ""Text"":""If"", ""Category"":""If"", ""Group"":-2},
    { ""Key"":5, ""Text"":""Action 2"", ""Group"":-2},
    { ""Key"":6, ""Text"":""Action 3"", ""Group"":-2},
    { ""Key"":-3, ""IsGroup"":true, ""Cat"":""For"", ""Group"":-2},
    { ""Key"":7, ""Text"":""For Each\n(nested)"", ""Category"":""For"", ""Group"":-3},
    { ""Key"":8, ""Text"":""Action 4"", ""Group"":-3},
    { ""Key"":9, ""Text"":"""", ""Category"":""EndFor"", ""Group"":-3},
    { ""Key"":10, ""Text"":"""", ""Category"":""EndIf"", ""Group"":-2},
    { ""Key"":11, ""Text"":""Action 5"", ""Group"":-1},
    { ""Key"":12, ""Text"":"""", ""Category"":""EndFor"", ""Group"":-1},
    { ""Key"":13, ""Text"":""E"", ""Category"":""End""}
  ],
  ""LinkDataSource"": [
    {""From"":1, ""To"":2},
    { ""From"":2, ""To"":3},
    { ""From"":3, ""To"":4},
    { ""From"":4, ""To"":5, ""Text"":""true""},
    { ""From"":4, ""To"":6, ""Text"":""false""},
    { ""From"":6, ""To"":7},
    { ""From"":7, ""To"":8},
    { ""From"":8, ""To"":9},
    { ""From"":5, ""To"":10},
    { ""From"":9, ""To"":10},
    { ""From"":9, ""To"":7},
    { ""From"":10, ""To"":11},
    { ""From"":11, ""To"":12},
    { ""From"":12, ""To"":2},
    { ""From"":12, ""To"":13}
  ]
}";

      Setup();
      SetupPalette();
      SetupOverview();
    }

    private void Setup() {
      myDiagram.AllowCopy = false;
      myDiagram.AllowMove = false;
      myDiagram.Layout = new ParallelLayout {
        Angle = 90,
        LayerSpacing = 21,
        NodeSpacing = 30,
      };
      myDiagram.SelectionDeleting += (obj, e) => {  // before a delete happens
        // Handle deletions by excising the node and reconnecting the link where the node had been
        var sel = new List<Part>(e.Diagram.Selection);
        foreach (var part in sel) {
          _DeletingNode(part);
        }
      };
      myDiagram.ExternalElementsDropped += (obj, e) => {
        var newnode = e.Diagram.Selection.FirstOrDefault() as Node;
        if (newnode == null) return;
        if (!(newnode is Group) && newnode.LinksConnected.Count() == 0) {
          // When the selection is dropped but not hooked up to the rest of the graph, delete it
          e.Diagram.RemoveParts(e.Diagram.Selection, false);
        } else {
          e.Diagram.CommandHandler.ScrollToPart(newnode);
        }
      };
      myDiagram.UndoManager.IsEnabled = true;

      // Dragged nodes are translucent so that the user can see highlighting of links and nodes
      myDiagram.FindLayer("Tool").Opacity = 0.5;

      // Define node templates for various categories of nodes
      DefineNodeTemplates();
      myDiagram.NodeTemplateMap = sharedNodeTemplateMap;

      object groupColor(object obj, object _) {
        var cat = obj as string;
        switch (cat) {
          case "If":
            return "rgba(255, 0, 0, 0.05)";
          case "For":
            return "rgba(0, 255, 0, 0.05)";
          case "While":
            return "rgba(0, 0, 255, 0.05)";
          default:
            return "rgba(0, 0, 0, 0.05)";
        }
      }

      // Define the group template, required but unseen
      myDiagram.GroupTemplate =
        new Group("Spot") {
            LocationSpot = Spot.Center,
            AvoidableMargin = 10,  // extra space on the sides
            Layout = new ParallelLayout { Angle = 90, LayerSpacing = 24, NodeSpacing = 30 },
            MouseDragEnter = (e, obj, prev) => {
              var group = (Group)obj;
              var sh = group.FindElement("SHAPE") as Shape;
              if (sh != null) sh.Stroke = "lime";
            },
            MouseDragLeave = (e, obj, prev) => {
              var group = (Group)obj;
              var sh = group.FindElement("SHAPE") as Shape;
              if (sh != null) sh.Stroke = null;
            },
            MouseDrop = _DropOntoNode
          }
          .Add(
            new Panel("Auto")
              .Add(
                new Shape("RoundedRectangle") {
                    Fill = "rgba(0, 0, 0, 0.05)",
                    StrokeWidth = 0,
                    Spot1 = Spot.TopLeft,
                    Spot2 = Spot.BottomRight
                  }
                  .Bind("Fill", "Cat", groupColor),
                new Placeholder()
              ),
              new Shape("LineH") {
                Name = "SHAPE",
                Alignment = Spot.Bottom,
                Height = 0, Stretch = Stretch.Horizontal,
                Stroke = null, StrokeWidth = 8
              }
          );

      // Define the link template
      myDiagram.LinkTemplate = new Link {
          Selectable = false,
          Deletable = false,  // links cannot be deleted
          Routing = LinkRouting.Orthogonal, Corner = 5,
          FromEndSegmentLength = 4, ToEndSegmentLength = 4,
          ToShortLength = 2,
          // If a node from the Palette is dragged over this link, its outline will turn green
          MouseDragEnter = (e, obj, prev) => {
            var link = (Link)obj;
            if (!_IsLoopBack(link)) link.IsHighlighted = true;
          },
          MouseDragLeave = (e, obj, prev) => {
            ((Link)obj).IsHighlighted = false;
          },
          // If a node from the Palette is dropped on a link, the link is replaced by links to and from the new node
          MouseDrop = _DropOntoLink,
        }
        .Add(
          new Shape() { IsPanelMain = true, Stroke = "transparent", StrokeWidth = 8 }
            .Bind(
              new Binding("Stroke", "IsHighlighted", (h, _) => {
                return (bool)h ? "lime" : "transparent";
              }).OfElement()
            ),
          new Shape() { IsPanelMain = true, Stroke = "black", StrokeWidth = 1.5 },
          new Shape() { ToArrow = "Standard", StrokeWidth = 0 }
          //new TextBlock { SegmentIndex = -2, SegmentFraction = .75, Editable = true }
          //  .Bind(
          //    new Binding("Text").MakeTwoWay(),
          //    new Binding("Background", "Text", (val, _) => {
          //      var t = val as string;
          //      return t != null && t != "" ? "white" : null;
          //    })
          //  )
        );

      LoadModel();  // read model from textarea and initialize MyDiagram
    }

    private void SetupPalette() {
      // initialize Palette
      myPalette.MaxSelectionCount = 1;
      // define templates for the Palette
      DefineNodeTemplates();
      myPalette.NodeTemplateMap = sharedNodeTemplateMap;

      myPalette.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Text = "Action" },
          new NodeData { Text = "For Each", Category = "For" },
          new NodeData { Text = "While", Category = "While" },
          new NodeData { Text = "If", Category = "If" },
          new NodeData { Text = "Switch", Category = "Switch" },
        }
      };
    }

    private void SetupOverview() {
      // initialize Overview
      myOverview.Observed = myDiagram;
      myOverview.ContentAlignment = Spot.Center;
    }

    private bool _IsLoopBack(Link link) {
      if (link == null) return false;
      if (link.FromNode.ContainingGroup != link.ToNode.ContainingGroup) return false;
      var cat = link.FromNode.Category;
      return (cat == "EndFor" || cat == "EndWhile" || cat == "EndIf");
    }

    // A node dropped onto a Merge node is spliced into a link coming into that node;
    // otherwise it is spliced into a link that is coming out of that node.
    private void _DropOntoNode(InputEvent e, GraphObject obj) {
      if (obj is Group oldgroup) {
        var merge = (oldgroup.Layout as ParallelLayout).MergeNode;
        if (merge != null) {
          foreach (var link in merge.FindLinksOutOf()) {
            if (link.FromNode.ContainingGroup != link.ToNode.ContainingGroup) {
              _DropOntoLink(e, link);
              break;
            }
          }
        }
      } else if (obj is Node oldnode) {
        var cat = oldnode.Category;
        if (cat == "Merge" || cat == "End" || cat == "EndFor" || cat == "EndWhile" || cat == "EndIf") {
          var link = oldnode.FindLinksInto().FirstOrDefault();
          if (link != null) _DropOntoLink(e, link);
        }
        else {
          var link = oldnode.FindLinksOutOf().FirstOrDefault();
          if (link != null) _DropOntoLink(e, link);
        }
      }
    }

    // Splice a node into a link.
    // If the new node is of category "For" or "While" or "If", create a Group and splice it in,
    // and add the new node to that group, and add any other desired nodes and links to that group.
    private void _DropOntoLink(InputEvent e, GraphObject obj) {
      if (!(obj is Link oldlink)) return;
      var diagram = e.Diagram;
      var sel = diagram.Selection.First();
      if (!(sel is Node newnode)) return;
      if (!newnode.IsTopLevel) return;
      if (_IsLoopBack(oldlink)) {
        diagram.Remove(newnode);
        return;
      }

      var model = diagram.Model as Model;
      var fromnode = oldlink.FromNode;
      var tonode = oldlink.ToNode;

      if (newnode.Category == "") {
        newnode.ContainingGroup = oldlink.ContainingGroup;
        // Reconnect the existing link to the new node
        oldlink.ToNode = (newnode as Node);
        // Then add links from the new node to the old node
        if (newnode.Category == "If") {
          model.AddLinkData(new LinkData { From = (int)newnode.Key, To = (int)tonode.Key });
          model.AddLinkData(new LinkData { From = (int)newnode.Key, To = (int)tonode.Key });
        } else {
          model.AddLinkData(new LinkData { From = (int)newnode.Key, To = (int)tonode.Key });
        }
      } else if (newnode.Category == "For" || newnode.Category == "While") {
        // Add group for loop
        var groupdata = new NodeData { IsGroup = true, Cat = newnode.Category };
        diagram.Model.AddNodeData(groupdata);
        var group = diagram.FindNodeForData(groupdata) as Group;
        group.ContainingGroup = oldlink.ContainingGroup;
        diagram.Select(group);

        newnode.ContainingGroup = group;

        var enddata = new NodeData { Category = "End" + newnode.Category };
        diagram.Model.AddNodeData(enddata);
        var endnode = diagram.FindNodeForData(enddata);
        endnode.ContainingGroup = group;
        endnode.Location = e.DocumentPoint;

        model.AddLinkData(new LinkData { From = (int)newnode.Key, To = (int)endnode.Key });
        model.AddLinkData(new LinkData { From = (int)endnode.Key, To = (int)newnode.Key });

        // Reconnect the existing link to the new node
        oldlink.ToNode = newnode;
        // Then add a link from the  end node to the old node
        model.AddLinkData(new LinkData { From = (int)endnode.Key, To = (int)tonode.Key });
      } else if (newnode.Category == "If") {
        // add group for conditional
        var groupdata = new NodeData { IsGroup = true, Cat = newnode.Category };
        diagram.Model.AddNodeData(groupdata);
        var group = diagram.FindNodeForData(groupdata) as Group;
        group.ContainingGroup = oldlink.ContainingGroup;
        diagram.Select(group);

        newnode.ContainingGroup = group;

        var enddata = new NodeData { Category = "EndIf" };
        diagram.Model.AddNodeData(enddata);
        var endnode = diagram.FindNodeForData(enddata);
        endnode.ContainingGroup = group;
        endnode.Location = e.DocumentPoint;

        var truedata = new LinkData { From = (int)newnode.Key, To = (int)endnode.Key, Text = "true"};
        model.AddLinkData(truedata);
        var truelink = diagram.FindLinkForData(truedata);
        var falsedata = new LinkData { From = (int)newnode.Key, To = (int)endnode.Key, Text = "false" };
        model.AddLinkData(falsedata);
        var faselink = diagram.FindLinkForData(falsedata);

        // Reconnect the existing link to the new node
        oldlink.ToNode = newnode;
        // Then add a link from the new node to the old node
        model.AddLinkData(new LinkData { From = (int)endnode.Key, To = (int)tonode.Key });
      } else if (newnode.Category == "Switch") {
        // add group for loop
        var groupdata = new NodeData { IsGroup = true, Cat = newnode.Category };
        diagram.Model.AddNodeData(groupdata);
        var group = diagram.FindNodeForData(groupdata) as Group;
        group.ContainingGroup = oldlink.ContainingGroup;
        diagram.Select(group);

        newnode.ContainingGroup = group;

        var enddata = new NodeData { Category = "Merge" };
        diagram.Model.AddNodeData(enddata);
        var endnode = diagram.FindNodeForData(enddata);
        endnode.ContainingGroup = group;
        endnode.Location = e.DocumentPoint;

        var yesdata = new NodeData { Text = "yes,\ndo it" };
        diagram.Model.AddNodeData(yesdata);
        var yesnode = diagram.FindNodeForData(yesdata);
        yesnode.ContainingGroup = group;
        yesnode.Location = e.DocumentPoint;
        model.AddLinkData(new LinkData { From = (int)newnode.Key, To = (int)yesnode.Key, Text = "yes" });
        model.AddLinkData(new LinkData { From = (int)yesnode.Key, To = (int)endnode.Key });

        var nodata = new NodeData { Text = "no,\ndon't" };
        diagram.Model.AddNodeData(nodata);
        var nonode = diagram.FindNodeForData(nodata);
        nonode.ContainingGroup = group;
        nonode.Location = e.DocumentPoint;
        model.AddLinkData(new LinkData { From = (int)newnode.Key, To = (int)nonode.Key, Text = "no" });
        model.AddLinkData(new LinkData { From = (int)nonode.Key, To = (int)endnode.Key });

        var maybedata = new NodeData { Text = "??" };
        diagram.Model.AddNodeData(maybedata);
        var maybenode = diagram.FindNodeForData(maybedata);
        maybenode.ContainingGroup = group;
        maybenode.Location = e.DocumentPoint;
        model.AddLinkData(new LinkData { From = (int)newnode.Key, To = (int)maybenode.Key, Text = "maybe" });
        model.AddLinkData(new LinkData { From = (int)maybenode.Key, To = (int)endnode.Key });

        // Reconnect the existing link to the new node
        oldlink.ToNode = newnode;
        // Then add a link from the end node to the old node
        model.AddLinkData(new LinkData { From = (int)endnode.Key, To = (int)tonode.Key });
      }
      diagram.LayoutDiagram(true);
    }

    private void _DeletingNode(Part p) {
      if (!(p is Node node)) return;
      if (node is Group) {
        var externals = node.FindExternalTreeLinksConnected();
        Node next = null;
        foreach (var link in externals) {
          if (link.FromNode.IsMemberOf(node)) {
            next = link.ToNode;
          }
        }
        if (next != null) {
          foreach (var link in externals) {
            if (link.ToNode.IsMemberOf(node)) {
              link.ToNode = next;
            }
          }
        } else if (node.Category == "") {
          next = node.FindNodesOutOf().FirstOrDefault();
          if (next != null) {
            foreach (var link in node.FindLinksInto()) {
              link.ToNode = next;
            }
          }
        }
      }
    }

    private void DefineNodeTemplates() {
      if (sharedNodeTemplateMap != null) return;  // already defined

      Figures.DefineExtraFigures();
      DefineForEachFigure();
      DefineEndForEachFigure();

      // define some common property settings
      var nodeStyle = new {
        LocationSpot = Spot.Center,
        Deletable = false, // do not allow this node to be removed by the user
        MouseDragEnter = new Action<InputEvent, GraphObject, GraphObject>((e, obj, prev) => {
          var sh = (obj as Node).FindElement("SHAPE");
          if (sh != null) (sh as Shape).Fill = "lime";
        }),
        MouseDragLeave = new Action<InputEvent, GraphObject, GraphObject>((e, obj, prev) => {
          var sh = (obj as Node).FindElement("SHAPE");
          if (sh != null) (sh as Shape).Fill = "white";
        }),
        MouseDrop = new Action<InputEvent, GraphObject>(_DropOntoNode),
      };

      var shapeStyle = new { Name = "SHAPE", Fill = "white" };

      var textStyle = new {
        Name = "TEXTBLOCK",
        TextAlign = TextAlign.Center,
        Editable = true,
      };

      Binding TextBind() {
        return new Binding("Text").MakeTwoWay();
      }

      // define templates for each type of node
      var actionTemplate =
        new Node("Auto") {
            Deletable = true,
            MinSize = new Size(10, 20)
          }
          .Set(nodeStyle)
          .Add(
            new Shape("Rectangle").Set(shapeStyle),
            new TextBlock("Action") { Margin = 4, Editable = true }.Set(textStyle).Bind(TextBind())
          );

      var startTemplate =
        new Node("Auto") {
            DesiredSize = new Size(32, 32)
          }
          .Set(nodeStyle)
          .Add(
            new Shape("Circle").Set(shapeStyle),
            new TextBlock("Start").Set(textStyle).Bind(TextBind())
          );

      var endTemplate =
        new Node("Auto") {
            DesiredSize = new Size(32, 32)
          }
          .Set(nodeStyle)
          .Add(
            new Shape("Circle").Set(shapeStyle),
            new TextBlock("End").Set(textStyle).Bind(TextBind())
          );

      var forTemplate =
        new Node("Auto") {
            MinSize = new Size(64, 32)
          }
          .Set(nodeStyle)
          .Add(
            new Shape("ForEach").Set(shapeStyle),
            new TextBlock("For Each") { Margin = 4 }.Set(textStyle).Bind(TextBind())
          );

      var endForTemplate =
        new Node()
          .Set(nodeStyle)
          .Add(
            new Shape("EndForEach") { DesiredSize = new Size(4, 4) }.Set(shapeStyle)
          );

      var whileTemplate =
        new Node("Auto") {
            MinSize = new Size(32, 32)
          }
          .Set(nodeStyle)
          .Add(
            new Shape("ForEach") { Angle = -90, Spot2 = new Spot(1, 1, -6, 0) }
              .Set(shapeStyle),
            new TextBlock("While") { Margin = 4 }.Set(textStyle).Bind(TextBind())
          );

      var endWhileTemplate =
        new Node()
          .Set(nodeStyle)
          .Add(
            new Shape("Circle") { DesiredSize = new Size(4, 4) }.Set(shapeStyle)
          );

      var ifTemplate =
        new Node("Auto") {
            MinSize = new Size(64, 32)
          }
          .Set(nodeStyle)
          .Add(
            new Shape("Diamond").Set(shapeStyle),
            new TextBlock("If").Set(textStyle).Bind(TextBind())
          );

      var endIfTemplate =
        new Node()
          .Set(nodeStyle)
          .Add(
            new Shape("Diamond") { DesiredSize = new Size(4, 4) }.Set(shapeStyle)
          );

      var switchTemplate =
        new Node("Auto") {
            MinSize = new Size(64, 32)
          }
          .Set(nodeStyle)
          .Add(
            new Shape("TriangleUp").Set(shapeStyle),
            new TextBlock("Switch") { Margin = 4 }.Set(textStyle).Bind(TextBind())
          );

      var mergeTemplate =
        new Node()
          .Set(nodeStyle)
          .Add(
            new Shape("TriangleDown") { DesiredSize = new Size(4, 4) }.Set(shapeStyle)
          );


      // Add the templates created above to the shared template map
      sharedNodeTemplateMap = new Dictionary<string, Part> {
        { "", actionTemplate },
        { "Start", startTemplate },
        { "End", endTemplate },
        { "For", forTemplate },
        { "EndFor", endForTemplate },
        { "While", whileTemplate },
        { "EndWhile", endWhileTemplate },
        { "If", ifTemplate },
        { "EndIf", endIfTemplate },
        { "Switch", switchTemplate },
        { "Merge", mergeTemplate },
      };

    }

    private void DefineForEachFigure () {
      if (Shape.GetFigureGenerators().ContainsKey("ForEach")) return;
      // taken from ../extensions/Figures.cs:
      Shape.DefineFigureGenerator("ForEach", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        if (param1 is double.NaN) param1 = 10;
        var d = Math.Min(h / 2, param1);
        var geo = new Geometry();
        var fig = new PathFigure(w, h - d, true);
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Line, w / 2, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h - d));
        fig.Add(new PathSegment(SegmentType.Line, 0, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, 0).Close());
        geo.Spot1 = Northwoods.Go.Spot.TopLeft;
        geo.Spot2 = new Spot(1, 1, 0, Math.Min(-d + 2, 0));
        return geo;
      });

    }

    private void DefineEndForEachFigure () {
      if (Shape.GetFigureGenerators().ContainsKey("EndForEach")) return;
      // taken from ../extensions/Figures.cs:
      Shape.DefineFigureGenerator("EndForEach", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        if (param1 is double.NaN) param1 = 10;
        var d = Math.Min(h / 2, param1);
        var geo = new Geometry();
        var fig = new PathFigure(w, d, true);
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, d));
        fig.Add(new PathSegment(SegmentType.Line, w / 2, 0).Close());
        geo.Spot2 = Northwoods.Go.Spot.BottomRight;
        geo.Spot1 = new Spot(0, 0, 0, Math.Min(d, 0));
        return geo;
      });

    }

    private void SaveModel() {
      if (myDiagram == null) return;
      modelJson1.JsonText = myDiagram.Model.ToJson();
    }

    private void LoadModel() {
      if (myDiagram == null) return;
      myDiagram.Model = Model.FromJson<Model>(modelJson1.JsonText);
      myDiagram.Model.UndoManager.IsEnabled = true;
    }

    private void NewDiagram() {
      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData{Key = 1, Text = "S", Category = "Start"},
          new NodeData{Key = 2, Text = "E", Category = "End"}

        },
        LinkDataSource = new List<LinkData> {
          new LinkData{From = 1, To = 2}
        }
      };
    }

  }
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }

  public class NodeData : Model.NodeData {
    public string Cat { get; set; }
  }

  public class LinkData : Model.LinkData {
    public string Side { get; set; }
  }

}
