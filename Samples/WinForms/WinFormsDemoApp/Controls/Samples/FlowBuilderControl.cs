/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;

namespace Demo.Samples.FlowBuilder {
  [ToolboxItem(false)]
  public partial class FlowBuilderControl : DemoControl {
    private Diagram myDiagram;

    private GraphObject OldTarget = null;

    public FlowBuilderControl() {
      InitializeComponent();

      btnDoLayout.Click += (e, obj) => doLayout();

      modelJson1.SaveClick = SaveModel;
      modelJson1.LoadClick = LoadModel;

      modelJson1.JsonText = @"
    {
      ""NodeDataSource"": [
        { ""Key"":1, ""Text"":""Loading Screen"", ""Category"":""Loading"" },
        { ""Key"":2, ""Text"":""Beginning"" },
        { ""Key"":3, ""Text"":""Segment 1"" },
        { ""Key"":4, ""Text"":""Segment 2"" },
        { ""Key"":5, ""Text"":""Segment 3""},
        { ""Key"":6, ""Text"":""End Screen"", ""Category"":""End"" },
        { ""Key"":-2, ""Category"":""Recycle"" }
      ],
      ""LinkDataSource"": [
        { ""From"":1, ""To"":2 },
        { ""From"":2, ""To"":3 },
        { ""From"":2, ""To"":5 },
        { ""From"":3, ""To"":4 },
        { ""From"":4, ""To"":6 }
      ]
    }";

      Setup();
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties

      myDiagram.AllowCopy = false;
      myDiagram.Layout = new LayeredDigraphLayout {
        SetsPortSpots = false,  // Links already know their fromSpot and toSpot
        ColumnSpacing = 5,
        IsInitial = false,
        IsOngoing = false
      };
      myDiagram.ValidCycle = CycleMode.NotDirected;
      myDiagram.UndoManager.IsEnabled = true;

      var graygrad = new Brush(new LinearGradientPaint(new Dictionary<float, string> {
          { 0, "white" },
          { 0.1f, "whitesmoke" },
          { 0.9f, "whitesmoke" },
          { 1, "lightgray" }
        }
      ));

      // node template
      myDiagram.NodeTemplate =  // the default node template
        new Node(PanelType.Spot) {
          SelectionAdorned = false,
          TextEditable = true,
          LocationElementName = "BODY"
        }.Bind(
          new Binding("Location", "Loc", Point.Parse).MakeTwoWay(Point.Stringify)
        ).Add(
          // the main body consists of a Rectangle surrounding the text
          new Panel(PanelType.Auto) {
            Name = "BODY"
          }.Add(
            new Shape {
              Figure = "Rectangle",
              Fill = graygrad,
              Stroke = "gray",
              MinSize = new Size(120, 21)
            }.Bind(
              new Binding("Fill", "IsSelected", (s, _) => { return (s as bool? ?? false) ? "dodgerblue" : graygrad; }).OfElement()
            ),
            new TextBlock {
              Stroke = "black",
              Font = new Font("Segoe UI", 12),
              Editable = true,
              Margin = new Margin(3, 3 + 11, 3, 3 + 4),
              Alignment = Spot.Left
            }.Bind(
              new Binding("Text").MakeTwoWay()
            )
          ),
          // output port
          new Panel(PanelType.Auto) {
            Alignment = Spot.Right,
            PortId = "from",
            FromLinkable = true,
            Cursor = "pointer",
            Click = AddNodeAndLink
          }.Add(
            new Shape {
              Figure = "Circle",
              Width = 22,
              Height = 22,
              Fill = "white",
              Stroke = "dodgerblue",
              StrokeWidth = 3
            },
            new Shape {
              Figure = "PlusLine",
              Width = 11,
              Height = 11,
              Fill = null,
              Stroke = "dodgerblue",
              StrokeWidth = 3
            }
          ),
          // input port
          new Panel(PanelType.Auto) {
            Alignment = Spot.Left,
            PortId = "to",
            ToLinkable = true
          }.Add(
            new Shape {
              Figure = "Circle",
              Width = 8,
              Height = 8,
              Fill = "white",
              Stroke = "gray"
            },
            new Shape {
              Figure = "Circle",
              Width = 4,
              Height = 4,
              Fill = "dodgerblue",
              Stroke = null
            }
          )
        );

      myDiagram.NodeTemplate.ContextMenu =
        Builder.Make<Adornment>("ContextMenu").Add(
          Builder.Make<Panel>("ContextMenuButton").Add(
            new TextBlock {
              Text = "Rename"
            }.Set(
              new {
                Click = new Action<InputEvent, GraphObject>((e, obj) => {
                  e.Diagram.CommandHandler.EditTextBlock();
                })
              }
            )
          ).Bind(
            new Binding("Visible", "", (oAsObj, _) => {
              var o = oAsObj as GraphObject;
              return (o.Diagram != null) && o.Diagram.CommandHandler.CanEditTextBlock();
            }).OfElement()
          ),
          // add one for Editing...
          Builder.Make<Panel>("ContextMenuButton").Add(
            new TextBlock {
              Text = "Delete"
            }.Set(
              new {
                Click = new Action<InputEvent, GraphObject>((e, obj) => {
                  e.Diagram.CommandHandler.DeleteSelection();
                })
              }
            ).Bind(
              new Binding("Visible", "", (oAsObj, _) => {
                var o = oAsObj as GraphObject;
                return (o.Diagram != null) && o.Diagram.CommandHandler.CanDeleteSelection();
              }).OfElement()
            )
          )
        );

      myDiagram.NodeTemplateMap.Add("Loading",
        new Node(PanelType.Spot) {
          SelectionAdorned = false,
          TextEditable = true,
          LocationElementName = "BODY"
        }.Bind(
          new Binding("Location", "Loc", Point.Parse).MakeTwoWay(Point.Stringify)
        ).Add(
          // the main body consists of a Rectangle surrounding the text
          new Panel(PanelType.Auto) {
            Name = "BODY"
          }.Add(
            new Shape {
              Figure = "Rectangle",
              Fill = graygrad,
              Stroke = "gray",
              MinSize = new Size(120, 21)
            }.Bind(
              new Binding("Fill", "IsSelected", (s, _) => { return (s as bool? ?? false) ? "dodgerblue" : graygrad; }).OfElement()
            ),
            new TextBlock {
              Stroke = "black",
              Font = new Font("Segoe UI", 12),
              Editable = true,
              Margin = new Margin(3, 3 + 11, 3, 3 + 4),
              Alignment = Spot.Left
            }.Bind(
              new Binding("Text", "Text")
            )
          ),
          // output port
          new Panel(PanelType.Auto) {
            Alignment = Spot.Right,
            PortId = "from",
            FromLinkable = true,
            Click = AddNodeAndLink
          }.Add(
            new Shape {
              Figure = "Circle",
              Width = 22,
              Height = 22,
              Fill = "white",
              Stroke = "dodgerblue",
              StrokeWidth = 3
            },
            new Shape {
              Figure = "PlusLine",
              Width = 11,
              Height = 11,
              Fill = null,
              Stroke = "dodgerblue",
              StrokeWidth = 3
            }
          )
        )
      );

      myDiagram.NodeTemplateMap.Add("End",
        new Node(PanelType.Spot) {
          SelectionAdorned = false,
          TextEditable = true,
          LocationElementName = "BODY"
        }.Bind(
          new Binding("Location", "Loc", Point.Parse).MakeTwoWay(Point.Stringify)
        ).Add(
          // the main body consists of a Rectangle surrounding the text
          new Panel(PanelType.Auto) {
            Name = "BODY"
          }.Add(
            new Shape {
              Figure = "Rectangle",
              Fill = graygrad,
              Stroke = "gray",
              MinSize = new Size(120, 21)
            }.Bind(
              new Binding("Fill", "IsSelected", (s, _) => { return (s as bool? ?? false) ? "dodgerblue" : graygrad; }).OfElement()
            ),
            new TextBlock {
              Stroke = "black",
              Font = new Font("Segoe UI", 12),
              Editable = true,
              Margin = new Margin(3, 3 + 11, 3, 3 + 4),
              Alignment = Spot.Left
            }.Bind(
              new Binding("Text", "Text")
            )
          ),
          // input port
          new Panel(PanelType.Auto) {
            Alignment = Spot.Left,
            PortId = "to",
            ToLinkable = true
          }.Add(
            new Shape {
              Figure = "Circle",
              Width = 8,
              Height = 8,
              Fill = "white",
              Stroke = "gray"
            },
            new Shape {
              Figure = "Circle",
              Width = 4,
              Height = 4,
              Fill = "dodgerblue",
              Stroke = null
            }
          )
        )
      );

      // dropping a node on this special node will cause the selection to be deleted;
      // linking or relinking to this special node will cause the link to be deleted
      myDiagram.NodeTemplateMap.Add("Recycle",
        new Node(PanelType.Auto) {
          PortId = "to",
          ToLinkable = true,
          Deletable = false,
          LayerName = "Background",
          LocationSpot = Spot.Center,
          DragComputation = (node, pt, gridpt) => { return pt; },
          MouseDrop = (e, obj) => { myDiagram.CommandHandler.DeleteSelection(); }
        }.Bind(
          new Binding("Location", "Loc", Point.Parse).MakeTwoWay(Point.Stringify)
        ).Add(
          new Shape { Fill = "lightgray", Stroke = "gray" },
          new TextBlock {
            Text = "Drop Here\nTo Delete",
            Margin = 5,
            TextAlign = TextAlign.Center
          }
        )
      );

      // this is a click event handler that adds a node and a link to the diagram,
      // connecting with the node on which the click occurred
      void AddNodeAndLink(InputEvent e, GraphObject obj) {
        var fromNode = obj.Part;
        var diagram = fromNode.Diagram;
        var dragtool = diagram.ToolManager.DraggingTool;
        diagram.StartTransaction("Add State");
        // get the node data for which the user clicked the button
        var fromData = fromNode.Data as NodeData;
        // create a new "State" data object, positioned off to the right of the fromNode
        var p = fromNode.Location;
        p.X += dragtool.GridSnapCellSize.Width;
        var toData = new NodeData {
          Text = "new",
          Loc = Point.Stringify(p)
        };
        // add the new node data to the model
        var model = diagram.Model as Model;
        model.AddNodeData(toData);
        // create a link data from the old node data to the new node data
        var linkdata = new LinkData {
          From = model.GetKeyForNodeData(fromData),
          To = model.GetKeyForNodeData(toData)
        };
        // and add the link data to the model
        model.AddLinkData(linkdata);
        // select the new Node
        var newnode = diagram.FindNodeForData(toData);
        diagram.Select(newnode);
        // snap the new node to a valid location
        newnode.Location = diagram.ComputeMove(newnode, p, dragtool.DragOptions);
        // then account for any overlap
        ShiftNodesToEmptySpaces();
        diagram.CommitTransaction("Add State");
      }

      // Highlight ports when they are targets for linking or relinking.
      void Highlight(GraphObject port) {
        if (OldTarget != port) {
          Lowlight();  // remove highlight from any old port
          OldTarget = port;
          port.Scale = 1.3;  // highlight by enlarging
        }
      }
      void Lowlight() {  // remove any highlight
        if (OldTarget != null) {
          OldTarget.Scale = 1.0;
          OldTarget = null;
        }
      }

      // Connecting a link with the Recycle node removes the link
      myDiagram.LinkDrawn += (_, e) => {
        var link = e.Subject as Link;
        if (link.ToNode.Category == "Recycle") myDiagram.Remove(link);
        Lowlight();
      };
      myDiagram.LinkRelinked += (_, e) => {
        var link = e.Subject as Link;
        if (link.ToNode.Category == "Recycle") myDiagram.Remove(link);
        Lowlight();
      };

      myDiagram.LinkTemplate =
        new Link {
          SelectionAdorned = false,
          FromPortId = "from",
          ToPortId = "to",
          RelinkableTo = true
        }.Add(
          new Shape {
            Stroke = "gray",
            StrokeWidth = 2,
            MouseEnter = (e, objAsObj, _) => {
              var obj = objAsObj as Shape;
              obj.StrokeWidth = 5; obj.Stroke = "dodgerblue";
            },
            MouseLeave = (e, objAsObj, _) => {
              var obj = objAsObj as Shape;
              obj.StrokeWidth = 2; obj.Stroke = "gray";
            }
          }
        );

      void CommonLinkingToolInit(LinkingTool tool) {
        // the temporary link drawn during a link drawing operation (LinkingTool) is thick and blue
        tool.TemporaryLink =
          new Link {
            LayerName = "Tool"
          }.Add(
            new Shape {
              Stroke = "dodgerblue",
              StrokeWidth = 5
            }
          );

        // change the standard proposed ports feedback from blue rectangles to transparent circles
        (tool.TemporaryFromPort as Shape).Figure = "Circle";
        (tool.TemporaryFromPort as Shape).Stroke = null;
        (tool.TemporaryFromPort as Shape).StrokeWidth = 0;
        (tool.TemporaryToPort as Shape).Figure = "Circle";
        (tool.TemporaryToPort as Shape).Stroke = null;
        (tool.TemporaryToPort as Shape).StrokeWidth = 0;

        // provide customized visual feedback as ports are targeted or not
        tool.PortTargeted = (realnode, realport, tempnode, tempport, toend) => {
          if (realport == null) {  // no valid port nearby
            Lowlight();
          } else if (toend) {
            Highlight(realport);
          }
        };
      }

      void CommonRelinkingToolInit(RelinkingTool tool) {
        // the temporary link drawn during a link drawing operation (LinkingTool) is thick and blue
        tool.TemporaryLink =
          new Link {
            LayerName = "Tool"
          }.Add(
            new Shape {
              Stroke = "dodgerblue",
              StrokeWidth = 5
            }
          );

        // change the standard proposed ports feedback from blue rectangles to transparent circles
        (tool.TemporaryFromPort as Shape).Figure = "Circle";
        (tool.TemporaryFromPort as Shape).Stroke = null;
        (tool.TemporaryFromPort as Shape).StrokeWidth = 0;
        (tool.TemporaryToPort as Shape).Figure = "Circle";
        (tool.TemporaryToPort as Shape).Stroke = null;
        (tool.TemporaryToPort as Shape).StrokeWidth = 0;

        // provide customized visual feedback as ports are targeted or not
        tool.PortTargeted = (realnode, realport, tempnode, tempport, toend) => {
          if (realport == null) {  // no valid port nearby
            Lowlight();
          } else if (toend) {
            Highlight(realport);
          }
        };
      }

      var ltool = myDiagram.ToolManager.LinkingTool;
      CommonLinkingToolInit(ltool);
      ltool.Direction = LinkingDirection.ForwardsOnly;

      var rtool = myDiagram.ToolManager.RelinkingTool;
      CommonRelinkingToolInit(rtool);
      // change the standard relink handle to be a shape that takes the shape of the link
      rtool.ToHandleArchetype =
        new Shape {
          IsPanelMain = true,
          Fill = null,
          Stroke = "dodgerblue",
          StrokeWidth = 5
        };

      // use a special DraggingTool to cause the dragging of a Link to start relinking it
      myDiagram.ToolManager.DraggingTool = new FlowBuilderDragLinkingTool();

      // detect when dropped onto an occupied cell
      myDiagram.SelectionMoved += (_, __) => {
        ShiftNodesToEmptySpaces();
      };

      // prevent nodes from being dragged to the left of where the layout placed them
      myDiagram.LayoutCompleted += (_, e) => {
        foreach (var node in myDiagram.Nodes) {
          if (node.Category == "Recycle") return;
          node.MinLocation = new Point(node.Location.X, double.NegativeInfinity);
        }
      };

      LoadModel();
      doLayout();
    }

    private void ShiftNodesToEmptySpaces() {
      foreach (var node in myDiagram.Selection) {
        if (!(node is Node)) continue;
        // look for Parts overlapping the node
        while (true) {
          node.EnsureBounds();
          var list = myDiagram.FindElementsIn(node.ActualBounds,
            // only consider Parts
            (obj) => { return obj.Part; },
            // ignore Links and the dropped node itself
            (part) => { return part is Node && part != node; },
            // check for any overlap, not complete containment
            true);
          if (list == null || list.Count <= 0) break;
          var exist = list.First();
          if (exist == null) break;
          // try shifting down beyond the existing node to see if there's empty space
          node.Move(node.ActualBounds.X, exist.ActualBounds.Bottom + 10);
        }
      }
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

    private void doLayout() {
      myDiagram.LayoutDiagram(true);
    }

    private void DebugAction() {
      myDiagram.CurrentTool.DoMouseUp();
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Loc { get; set; }
  }

  public class LinkData : Model.LinkData { }

  // Define a custom tool that changes a drag operation on a Link to a relinking operation,
  // but that operates like a normal DraggingTool otherwise.
  public class FlowBuilderDragLinkingTool : DraggingTool {
    public FlowBuilderDragLinkingTool() : base() {
      IsGridSnapEnabled = true;
      IsGridSnapRealtime = false;
      GridSnapCellSize = new Size(185, 1);
      GridSnapOrigin = new Point(5.5, 0);
    }

    // Handle dragging a link specially -- by starting the RelinkingTool on that Link
    public override void DoActivate() {
      var diagram = Diagram;
      if (diagram == null) return;
      StandardMouseSelect();
      var main = CurrentPart;  // this is set by the standardMouseSelect
      if (main is Link) { // maybe start relinking instead of dragging
        var relinkingtool = diagram.ToolManager.RelinkingTool;
        // tell the RelinkingTool to work on this Link, not what is under the mouse
        relinkingtool.OriginalLink = main as Link;
        // start the RelinkingTool
        diagram.CurrentTool = relinkingtool;
        // can activate it right now, because it already has the originalLink to reconnect
        relinkingtool.DoActivate();
        relinkingtool.DoMouseMove();
      } else {
        base.DoActivate();
      }
    }
  }

}
