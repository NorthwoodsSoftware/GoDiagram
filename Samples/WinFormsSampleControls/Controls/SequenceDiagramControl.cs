/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;

namespace WinFormsSampleControls.SequenceDiagram {
  [ToolboxItem(false)]
  public partial class SequenceDiagramControl : System.Windows.Forms.UserControl {
    private static Diagram myDiagram;

    public SequenceDiagramControl() {
      InitializeComponent();

      modelJson1.SaveClick += (e, obj) => SaveModel();
      modelJson1.LoadClick += (e, obj) => LoadModel();

      goWebBrowser1.Html = @"
        <p>
          A <em>sequence diagram</em> is an interaction diagram that shows how entities operate with one another and in what order.
          In this sample, we show the interaction between different people in a restaurant.
        </p>
        <p>
          The diagram uses the <a>Diagram.GroupTemplate</a> for ""lifelines,""
          <a>Diagram.NodeTemplate</a> for ""activities,"" and <a>Diagram.LinkTemplate</a> for ""messages"" between the entities.

          Also featured are a custom Link class and custom <a>LinkingTool</a> to draw links
          between lifelines and create activities at the end of the new link. Nodes use a binding function on the location
          property to ensure they are anchored to their lifeline.
        </p>
";

      modelJson1.JsonText = @"{
  ""NodeDataSource"": [
    {""Key"":""Fred"", ""Text"":""Fred: Patron"", ""IsGroup"":true, ""Loc"":""0 0"", ""Duration"":9},
    {""Key"":""Bob"", ""Text"":""Bob: Waiter"", ""IsGroup"":true, ""Loc"":""100 0"", ""Duration"":9},
    {""Key"":""Hank"", ""Text"":""Hank: Cook"", ""IsGroup"":true, ""Loc"":""200 0"", ""Duration"":9},
    {""Key"":""Renee"", ""Text"":""Renee: Cashier"", ""IsGroup"":true, ""Loc"":""300 0"", ""Duration"":9},
    {""Group"":""Bob"", ""Start"":1, ""Duration"":2},
    {""Group"":""Hank"", ""Start"":2, ""Duration"":3},
    {""Group"":""Fred"", ""Start"":3, ""Duration"":1},
    {""Group"":""Bob"", ""Start"":5, ""Duration"":1},
    {""Group"":""Fred"", ""Start"":6, ""Duration"":2},
    {""Group"":""Renee"", ""Start"":8, ""Duration"":1}
  ],
  ""LinkDataSource"": [
    {""From"":""Fred"", ""To"":""Bob"", ""Text"":""order"", ""Time"":1},
    {""From"":""Bob"", ""To"":""Hank"", ""Text"":""order food"", ""Time"":2},
    {""From"":""Bob"", ""To"":""Fred"", ""Text"":""serve drinks"", ""Time"":3},
    {""From"":""Hank"", ""To"":""Bob"", ""Text"":""finish cooking"", ""Time"":5},
    {""From"":""Bob"", ""To"":""Fred"", ""Text"":""serve food"", ""Time"":6},
    {""From"":""Fred"", ""To"":""Renee"", ""Text"":""pay"", ""Time"":8}
  ]
}";

      Setup();
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      myDiagram.AllowCopy = false;
      myDiagram.ToolManager.LinkingTool = new MessagingTool();  // defined below
      myDiagram.ToolManager.ResizingTool.IsGridSnapEnabled = true;
      myDiagram.ToolManager.DraggingTool = new MessageDraggingTool {  // defined below
        GridSnapCellSize = new Size(1, MessageSpacing / 4),
        IsGridSnapEnabled = true
      };
      // automatically extend LifeLines as Activites are moved or resized
      myDiagram.SelectionMoved += _EnsureLifelineHeights;
      myDiagram.PartResized += _EnsureLifelineHeights;
      myDiagram.UndoManager.IsEnabled = true;

      // define the Lifeline Node template
      myDiagram.GroupTemplate =
        new Group("Vertical") {
          LocationSpot = Spot.Bottom,
          LocationElementName = "HEADER",
          MinLocation = new Point(0, 0),
          MaxLocation = new Point(9999, 0),
          SelectionElementName = "HEADER"
        }
          .Bind("Location", "Loc", Point.Parse, Point.Stringify)
          .Add(
            new Panel("Auto") {
              Name = "HEADER"
            }
              .Add(
                new Shape("Rectangle") {
                  Fill = new Brush("Linear", new[] { (0f, "#bbdefb"), (1, Brush.DarkenBy("#bbdefb", .1)) }),
                  Stroke = null
                },
                new TextBlock {
                  Margin = 5,
                  Font = new Font("Segoe UI", 10, FontWeight.Bold)
                }
                  .Bind("Text")
              ),
             new Shape("LineV") {
               Fill = null,
               Stroke = "gray",
               StrokeDashArray = new float[] { 3f, 3f },
               Width = 1,
               Alignment = Spot.Center,
               PortId = "",
               FromLinkable = true,
               FromLinkableDuplicates = true,
               ToLinkable = true,
               ToLinkableDuplicates = true,
               Cursor = "pointer"
             }
               .Bind("Height", "Duration", (duration, _) => _ComputeLifelineHeight((double)duration))
          );

      // define the Activity node template
      myDiagram.NodeTemplate =
        new Node {
          LocationSpot = Spot.Top,
          LocationElementName = "SHAPE",
          MinLocation = new Point(double.NaN, LinePrefix - ActivityStart),
          MaxLocation = new Point(double.NaN, 19999),
          SelectionElementName = "SHAPE",
          Resizable = true,
          ResizeElementName = "SHAPE",
          ResizeAdornmentTemplate =
              new Adornment("Spot")
                .Add(
                  new Placeholder(),
                  new Shape { // only a bottom resize handle
                    Alignment = Spot.Bottom,
                    Cursor = "col-resize",
                    DesiredSize = new Size(6, 6),
                    Fill = "yellow"
                  }
                )
        }
          .Bind("Location", "", (o, _) => _ComputeActivityLocation(o as NodeData), (loc, act, _) => _BackComputeActivityLocation((Point)loc, act as NodeData))
          .Add(
            new Shape("Rectangle") {
              Name = "SHAPE",
              Fill = "white", Stroke = "black",
              Width = ActivityWidth,
              // allow activities to be resized down to 1/4 of a time unit
              MinSize = new Size(ActivityWidth, _ComputeActivityHeight(0.25))
            }
            .Bind("Height", "Duration", (dur, _) => _ComputeActivityHeight((double)dur), (height, _, _) => _BackComputeActivityHeight((double)height))
          );

      // define the Message Link template.
      myDiagram.LinkTemplate =
        new MessageLink {  // defined below
          SelectionAdorned = true, Curviness = 0
        }
          .Add(
            new Shape("Rectangle") { Stroke = "black" },
            new Shape {
              ToArrow = "OpenTriangle", Stroke = "black"
            },
            new TextBlock {
              Font = new Font("Segoe UI", 9, FontWeight.Bold),
              SegmentIndex = 0,
              SegmentOffset = new Point(double.NaN, double.NaN),
              IsMultiline = false,
              Editable = true
            }
              .Bind(new Binding("Text").MakeTwoWay())
          );

      // create the graph by reading the JSONdata saved in the textarea element
      LoadModel();
    }

    // some parameters
    public static readonly double LinePrefix = 20; // vertical starting point in document for all Messages and Activations
    public static readonly double LineSuffix = 30; // vertical length beyond the last message time
    public static readonly double MessageSpacing = 20; // vertical distance between messages at different steps
    public static readonly double ActivityWidth = 10; // width of each vertical activity bar
    public static readonly double ActivityStart = 5; // height before start message time
    public static readonly double ActivityEnd = 5; // height beyond end message time

    public static double _ComputeLifelineHeight(double duration) {
      return LinePrefix + duration * MessageSpacing + LineSuffix;
    }

    public static Point _ComputeActivityLocation(NodeData act) {
      var groupdata = myDiagram.Model.FindNodeDataForKey(act.Group);
      if (groupdata == null) return new Point();

      // get location of lifeline's starting point
      var groupLoc = Point.Parse((groupdata as NodeData).Loc);
      return new Point(groupLoc.X, _ConvertTimeToY(act.Start) - ActivityStart);
    }

    public static object _BackComputeActivityLocation(Point loc, NodeData act) {
      myDiagram.Model.Set(act, "Start", _ConvertYToTime(loc.Y + ActivityStart));
      return null;
    }

    public static double _ComputeActivityHeight(double duration) {
      return ActivityStart + duration * MessageSpacing + ActivityEnd;
    }

    public static double _BackComputeActivityHeight(double height) {
      return (height - ActivityStart - ActivityEnd) / MessageSpacing;
    }

    // time is just an abstract small non-negative integer
    // here we map between time and a vertical position
    public static double _ConvertTimeToY(double t) {
      return t * MessageSpacing + LinePrefix;
    }

    public static double _ConvertYToTime(double y) {
      return (y - LinePrefix) / MessageSpacing;
    }

    public static void _EnsureLifelineHeights(object s = null, DiagramEvent e = null) {
      // iterate all Activities (ignore Groups)
      var arr = myDiagram.Model.NodeDataSource as IEnumerable<NodeData>;
      var max = -1.0;
      foreach (var act in arr) {
        if (act.IsGroup) continue;
        max = Math.Max(max, act.Start + act.Duration);
      }
      if (max > 0) {
        // now iterate over only Groups
        foreach (var gr in arr) {
          if (!gr.IsGroup) continue;
          if (max > gr.Duration) { // this only extends, never shrinks
            myDiagram.Model.Set(gr, "Duration", max);
          }
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

  }

  // a custom routed Link
  public class MessageLink : Link {
    public double Time { get; set; }  // use this "time" value when this is the TemporaryLink
    public MessageLink() : base() {
      Time = 0;
    }

    public override Point GetLinkPoint(Node node, GraphObject port, Spot spot, bool from, bool ortho, Node othernode, GraphObject otherport) {
      var p = port.GetDocumentPoint(Spot.Center);
      var r = port.GetDocumentBounds();
      var op = otherport.GetDocumentPoint(Spot.Center);

      var data = Data as LinkData;
      var time = data != null ? data.Time : Time;  // if not bound, assume this has its own "Time" property

      var aw = _FindActivityWidth(node, time);
      var x = op.X > p.X ? p.X + aw / 2 : p.X - aw / 2;
      var y = SequenceDiagramControl._ConvertTimeToY(time);
      return new Point(x, y);
    }

    private double _FindActivityWidth(Node node, double time) {
      var aw = SequenceDiagramControl.ActivityWidth;
      if (node is Group) {
        // see if there is an Activity Node at this point -- if not, connect the link directly with the Group
        if (!(node as Group).MemberParts.Any(mem => {
          return mem.Data is NodeData act && act.Start <= time && time <= act.Start + act.Duration;
        })) {
          aw = 0;
        }
      }
      return aw;
    }

    public override int GetLinkDirection(Node node, GraphObject port, Point linkpoint, Spot spot, bool from, bool ortho, Node othernode, GraphObject otherport) {
      var p = port.GetDocumentPoint(Spot.Center);
      var op = otherport.GetDocumentPoint(Spot.Center);
      var right = op.X > p.X;
      return right ? 0 : 180;
    }

    public override bool ComputePoints() {
      if (FromNode == ToNode) { // also handle a reflexive link as a simple orthogonal loop
        var data = Data as LinkData;
        var time = data != null ? data.Time : Time; // if not bound, assume this has its own "time" property
        var p = FromNode.Port.GetDocumentPoint(Spot.Center);
        var aw = _FindActivityWidth(FromNode, time);

        var x = p.X + aw / 2;
        var y = SequenceDiagramControl._ConvertTimeToY(time);
        ClearPoints();
        AddPoint(new Point(x, y));
        AddPoint(new Point(x + 50, y));
        AddPoint(new Point(x + 50, y + 5));
        AddPoint(new Point(x, y + 5));
        return true;
      } else {
        return base.ComputePoints();
      }
    }
  }

  // A custom LinkingTool that fixes the "Time" (i.e. the Y coordinate)
  // for both the TemporaryLink and the actual newly created Link
  public class MessagingTool : LinkingTool {
    public MessagingTool() : base() {
      TemporaryLink =
        new MessageLink()
          .Add(
            new Shape("Rectangle") {
              Stroke = "magenta", StrokeWidth = 2
            },
            new Shape {
              ToArrow = "OpenTriangle", Stroke = "magenta"
            }
          );
    }

    public override void DoActivate() {
      base.DoActivate();
      var time = SequenceDiagramControl._ConvertYToTime(Diagram.FirstInput.DocumentPoint.Y);
      (TemporaryLink as MessageLink).Time = Math.Ceiling(time);
    }

    public override Link InsertLink(Node fromnode, GraphObject fromport, Node tonode, GraphObject toport) {
      var newlink = base.InsertLink(fromnode, fromport, tonode, toport);
      if (newlink != null) {
        var model = Diagram.Model;
        // specify the time of the message
        var start = (TemporaryLink as MessageLink).Time;
        var duration = 1;
        (newlink.Data as LinkData).Time = start;
        model.Set(newlink.Data, "Text", "msg");
        // and create a new Activity node data in the "to" group data
        var newact = new NodeData {
          Group = (newlink.Data as LinkData).To,
          Start = start,
          Duration = duration
        };
        model.AddNodeData(newact);
        SequenceDiagramControl._EnsureLifelineHeights();
      }
      return newlink;
    }
  }

  // A custom DraggingTool that supports dragging any number of MessageLinks up and down --
  // changing their Data.Time value.
  public class MessageDraggingTool : DraggingTool {
    public MessageDraggingTool() : base() { }

    // override the standard behavior to include all selected Links,
    // even if not connected with any selected Nodes
    public override IDictionary<Part, DraggingInfo> ComputeEffectiveCollection(IEnumerable<Part> parts, DraggingOptions options) {
      var result = base.ComputeEffectiveCollection(parts, options);
      // add a dummy Node so that the user can select only Links and move them all
      result.Add(new Node(), new DraggingInfo(new Point()));
      // normally this method removes any links not connected to selected nodes;
      // we have to add them back so that they are included in the "parts" argument to MoveParts
      foreach (var part in parts) {
        if (part is Link l) {
          result.Add(l, new DraggingInfo(new Point(l.GetPoint(0).X, l.GetPoint(0).Y)));
        }
      }
      return result;
    }

    // override to allow dragging when the selection only includes Links
    public override bool MayMove() {
      return !Diagram.IsReadOnly && Diagram.AllowMove;
    }

    // override to move Links (which are all assumed to be MessageLinks)
    // by updating their Link.Data.Time property so that their link routes will
    // have the correct vertical position
    public override void MoveParts(IDictionary<Part, DraggingInfo> parts, Point offset, bool check) {
      base.MoveParts(parts, offset, check);
      foreach (var kvp in parts) {
        if (kvp.Key is Link link) {
          var startY = kvp.Value.Point.Y;  // DraggingInfo.Point.Y
          var y = startY + offset.Y;  // determine new Y coordinate value for this link
          var cellY = GridSnapCellSize.Height;
          y = Math.Round(y / cellY) * cellY;  // snap to multiple of gridsnapcellsize.Height
          var t = Math.Max(0, SequenceDiagramControl._ConvertYToTime(y));
          link.Diagram.Model.Set(link.Data, "Time", t);
          link.InvalidateRoute();
        }
      }
    }
  }

  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }

  public class NodeData : Model.NodeData {
    public double Start { get; set; }
    public double Duration { get; set; }
    public string Loc { get; set; }
  }

  public class LinkData : Model.LinkData {
    public double Time { get; set; }
  }

}
