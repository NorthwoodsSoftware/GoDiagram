/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;
using System.Linq;

namespace WinFormsSampleControls.Gantt {
  [ToolboxItem(false)]
  public partial class GanttControl : System.Windows.Forms.UserControl {
    // Custom layout for myGantt Diagram
    public class GanttLayout : Layout {
      public int CellHeight { get; set; }

      public GanttLayout() : base() {
        CellHeight = GridCellHeight;
      }

      public override void DoLayout(IEnumerable<Part> coll) {
        var diagram = Diagram;
        if (diagram == null) return;

        diagram.StartTransaction("Gantt Layout");
        var bars = new List<Node>();
        _AssignTimes(diagram, bars);
        ArrangementOrigin = InitialOrigin(ArrangementOrigin);
        var y = ArrangementOrigin.Y;
        foreach (var node in bars) {
          var nd = node.Data as NodeData;
          var tasknode = myTasks.FindNodeForData(nd);
          node.Visible = tasknode.IsVisible();
          node.MoveTo(ConvertStartToX(nd.Start), y);
          if (node.Visible) y += CellHeight;
        }
        diagram.CommitTransaction("Gantt Layout");
      }

      private void _AssignTimes(Diagram diagram, List<Node> bars) {
        var roots = diagram.FindTreeRoots();
        while (roots.MoveNext()) {
          var root = roots.Current;
          _WalkTree(root, 0, bars);
        }
      }

      private double _WalkTree(Node node, double start, List<Node> bars) {
        bars.Add(node);
        var model = node.Diagram.Model as Model;
        var nd = node.Data as NodeData;
        if (node.IsTreeLeaf) {
          var dur = nd.Duration;
          if (double.IsNaN(dur)) {
            dur = ConvertDaysToUnits(1);
            model.Set(nd, "Duration", dur);  // default task length?
          }
          var st = nd.Start;
          if (double.IsNaN(st)) {
            st = start;
            model.Set(nd, "Start", st);
          }
          return st + dur;
        } else {
          // first recurse to fill in any missing data
          foreach (var n in node.FindTreeChildrenNodes()) {
            start = _WalkTree(n, start, bars);
          };
          // now can calculate this non-leaf node's data
          var min = double.PositiveInfinity;
          var max = double.NegativeInfinity;
          var colors = new HashSet<string>();
          foreach (var cn in node.FindTreeChildrenNodes()) {
            var cnd = cn.Data as NodeData;
            min = Math.Min(min, cnd.Start);
            max = Math.Max(max, cnd.Start + cnd.Duration);
            if (cnd.Color != null) colors.Add(cnd.Color);
          }
          model.Set(nd, "Start", min);
          model.Set(nd, "Duration", max - min);
          return max;
        }
      } 
    }
    // end of GanttLayout

    private static readonly int GridCellHeight = 20;  // document units
    private static int GridCellWidth = 12;  // document units per day
    private static readonly int TimelineHeight = 24;  // document units

    // By default the values for the data properties Start and Duration are in days,
    // and the Start value is relative to the StartDate.
    // If you want the Start and Duration properties to be in a unit other than days,
    // you only need to change the implementation of ConvertDaysToUnits and ConvertUnitsToDays.
    private static Func<double, double> ConvertDaysToUnits = n => n;
    private static Func<double, double> ConvertUnitsToDays = n => n;

    private static double ConvertStartToX(double start) {
      return ConvertUnitsToDays(start) * GridCellWidth;
    }

    private static double ConvertXToStart(double x) {
      return ConvertDaysToUnits(x / GridCellWidth);
    }

    private static DateTime StartDate = DateTime.UtcNow;

    private static bool ChangingView = false; // for preventing recursive viewport updates

    private static Diagram myTasks;
    private static Diagram myGantt;
    private static Part myTimeline;

    public GanttControl() {
      InitializeComponent();
      myTasks = tasksControl.Diagram;
      myGantt = ganttControl.Diagram;

      Setup();

      trackBar1.ValueChanged += (e, obj) => rescale();

      goWebBrowser1.Html = @"
           <p>
            This sample demonstrates a simple Gantt chart.

            Gantt charts are used to illustrate project schedules, denoting the start and end dates for terminal and summary elements of the project.
          </p>
          <p>
            You can zoom in on the diagram by changing the ""Spacing"" value,
            which scales the diagram using a data binding function for nodes' widths and locations.
            This is in place of changing the <a>Diagram.Scale</a>.
          </p>
        ";
    }

    private void Setup() {
      Shape.DefineFigureGenerator("RangeBar", (shape, w, h) => {
        var b = Math.Min(5, w);
        var d = Math.Min(5, h);
        return new Geometry()
          .Add(new PathFigure(0, 0, true)
            .Add(new PathSegment(SegmentType.Line, w, 0))
            .Add(new PathSegment(SegmentType.Line, w, h))
            .Add(new PathSegment(SegmentType.Line, w - b, h - d))
            .Add(new PathSegment(SegmentType.Line, b, h - d))
            .Add(new PathSegment(SegmentType.Line, 0, h).Close()));
      });

      static void standardContextMenus(Node node) {
        node.ContextMenu =
          Builder.Make<Adornment>("ContextMenu")
            .Add(
              Builder.Make<Panel>("ContextMenuButton")
                .Add(new TextBlock("Details ..."))
                .Apply(btn => {
                  btn.Click = (e, button) => {
                    var task = (button.Part as Adornment).AdornedPart;
                    System.Windows.Forms.MessageBox.Show($"Details: {task.Data}");
                  };
                }),
              Builder.Make<Panel>("ContextMenuButton")
                .Add(new TextBlock("New Task"))
                .Apply(btn => {
                  btn.Click = (e, button) => {
                    var task = (button.Part as Adornment).AdornedPart;
                    e.Diagram.Model.Commit(m => {
                      var newdata = new NodeData { Text = "New Task", Color = (task.Data as NodeData).Color, Duration = ConvertDaysToUnits(5) };
                      m.AddNodeData(newdata);
                      (m as Model).AddLinkData(new LinkData { From = (int)task.Key, To = newdata.Key });
                      e.Diagram.Select(e.Diagram.FindNodeForData(newdata));
                    });
                  };
                })
            );
      }

      // the left side of the whole diagram
      myTasks.InitialContentAlignment = Spot.Right;
      myTasks.Padding = new Margin(TimelineHeight, 0, 0, 0);
      myTasks.HasVerticalScrollbar = false;
      myTasks.AllowMove = false;
      myTasks.AllowCopy = false;
      myTasks.CommandHandler.DeletesTree = true;
      myTasks.Layout =
        new TreeLayout {
          Alignment = TreeAlignment.Start,
          Compaction = TreeCompaction.None,
          LayerSpacing = 16,
          LayerSpacingParentOverlap = 1,
          NodeIndentPastParent = 1,
          NodeSpacing = 0,
          PortSpot = Spot.Bottom,
          ChildPortSpot = Spot.Left,
          ArrangementSpacing = new Size(0, 0)
        };
      myTasks.AnimationManager.IsInitial = false;
      myTasks.TreeCollapsed += (s, e) => myGantt.LayoutDiagram(true);
      myTasks.TreeExpanded += (s, e) => myGantt.LayoutDiagram(true);

      myTasks.NodeTemplate =
        new Node("Horizontal") { Height = 20 }
          .Bind(new Binding("IsTreeExpanded").MakeTwoWay())
          .Add(
            Builder.Make<Panel>("TreeExpanderButton")
              .Set(new { PortId = "", Scale = 0.85 }),
            new TextBlock { Editable = true }
              .Bind(new Binding("Text").MakeTwoWay())
          )
          .Apply(standardContextMenus);

      myTasks.LinkTemplate =
        new Link {
            Routing = LinkRouting.Orthogonal,
            FromEndSegmentLength = 1,
            ToEndSegmentLength = 1
          }
          .Add(new Shape());

      myTasks.LinkTemplateMap["Dep"] =  // ignore these links in the Tasks diagram
        new Link { Visible = false, IsTreeLink = false };

      // the right side of the whole diagram
      myGantt.InitialPosition = new Point(-7, -100);  // show labels
      myGantt.Padding = new Margin(TimelineHeight, 0, 0, 0);
      myGantt.ScrollMargin = new Margin(0, GridCellWidth * 7, 0, 0);  // show a week beyond
      myGantt.AllowCopy = false;
      myGantt.CommandHandler.DeletesTree = true;
      myGantt.ToolManager.DraggingTool.IsGridSnapEnabled = true;
      myGantt.ToolManager.DraggingTool.GridSnapCellSize = new Size(GridCellWidth, GridCellHeight);
      myGantt.ToolManager.DraggingTool.DragsTree = true;
      myGantt.ToolManager.ResizingTool.IsGridSnapEnabled = true;
      myGantt.ToolManager.ResizingTool.CellSize = new Size(GridCellWidth, GridCellHeight);
      myGantt.ToolManager.ResizingTool.MinSize = new Size(GridCellWidth, GridCellHeight);
      myGantt.Layout = new GanttLayout();
      myGantt.AnimationManager.IsInitial = false;
      myGantt.SelectionMoved += (s, e) => e.Diagram.LayoutDiagram(true);
      myGantt.DocumentBoundsChanged += (s, e) => {
        // the grid extends to only the area needed
        var b = e.Diagram.DocumentBounds;
        var gridpart = e.Diagram.Parts.FirstOrDefault();
        if (gridpart != null && gridpart.Type == PanelLayoutGrid.Instance) {
          gridpart.DesiredSize = new Size(b.Right - gridpart.Position.X, b.Bottom);
        }
        // the timeline only covers the needed area
        myTimeline.FindElement("MAIN").Width = b.Right;
        myTimeline.FindElement("TICKS").Height = e.Diagram.ViewportBounds.Height;
        myTimeline.GraduatedMax = b.Right;
      };

      myGantt.Add(
        new Part("Grid") {
            LayerName = "Grid", Position = new Point(-10, 0), GridCellSize = new Size(3000, GridCellHeight)
          }
          .Add(new Shape("LineH") { StrokeWidth = 0.5 })
      );

      myGantt.NodeTemplate =
        new Node("Spot") {
            SelectionAdorned = false,
            SelectionChanged = node => {
              node.Diagram.Commit(diag => {
                (node.FindElement("SHAPE") as Shape).Fill = node.IsSelected ? "dodgerblue" : (node.Data as NodeData).Color ?? "gray";
              }, null);
            },
            MinLocation = new Point(0, double.NaN),
            MaxLocation = new Point(double.PositiveInfinity, double.NaN),
            ToolTip =
                Builder.Make<Adornment>("ToolTip")
                  .Add(
                    new Panel("Table") { DefaultAlignment = Spot.Left }
                      .Add(new ColumnDefinition { Column = 1, SeparatorPadding = 3 })
                      .Add(
                        new TextBlock { Row = 0, Column = 0, ColumnSpan = 9, Font = new Font("Segoe UI", 12, FontWeight.Bold) }.Bind("Text"),
                        new TextBlock("start:") { Row = 1, Column = 0 },
                        new TextBlock { Row = 1, Column = 1 }.Bind("Text", "Start", d => "day " + ConvertUnitsToDays((double)d).ToString("F0")),
                        new TextBlock("length:") { Row = 2, Column = 0 },
                        new TextBlock { Row = 2, Column = 1 }.Bind("Text", "Duration", d => ConvertUnitsToDays((double)d).ToString("F0") + " days")
                      )
                  ),
            Resizable = true,
            ResizeElementName = "SHAPE",
            ResizeAdornmentTemplate =
                new Adornment("Spot")
                  .Add(
                    new Placeholder(),
                    new Shape("Diamond") {
                      Alignment = Spot.Right,
                      Width = 8, Height = 8,
                      StrokeWidth = 0,
                      Fill = "fuchsia",
                      Cursor = "e-resize"
                    }
                  )
          }
          .Apply(standardContextMenus)
          .Bind(
            new Binding("Position", "Start",
              (start, node) => new Point(ConvertStartToX((double)start), ((Node)node).Position.Y),
              pos => ConvertXToStart(((Point)pos).X)
            ),
            new Binding("Resizable", "IsTreeLeaf").OfElement(),
            new Binding("IsTreeExpanded").MakeTwoWay()
          )
          .Add(
            new Shape {
                Name = "SHAPE", Height = 18, Margin = new Margin(1, 0), StrokeWidth = 0, Fill = "gray"
              }
              .Bind("Fill", "Color")
              .Bind("Width", "Duration",
                duration => ConvertUnitsToDays((double)duration) * GridCellWidth,
                w => ConvertDaysToUnits((double)w) / GridCellWidth
              )
              .Bind(new Binding("Figure", "IsTreeLeaf", leaf => (bool)leaf ? "Rectangle" : "RangeBar").OfElement()),
            new TextBlock {
                Font = new Font("Segoe UI", 8), Alignment = Spot.TopLeft, AlignmentFocus = new Spot(0, 0, 0, -2)
              }
              .Bind("Text")
              .Bind("Stroke", "Color", c => Brush.IsDark((string)c) ? "#ddd" : "#333")
          );

      // create the link template
      myGantt.LinkTemplate = new Link { Visible = false };

      myGantt.LinkTemplateMap["Dep"] =
        new Link {
            Routing = LinkRouting.Orthogonal,
            IsTreeLink = false, IsLayoutPositioned = false,
            FromSpot = new Spot(0.999999, 1), ToSpot = new Spot(0.000001, 0)
          }
          .Add(
            new Shape { Stroke = "brown" },
            new Shape { ToArrow = "Standard", Fill = "brown", StrokeWidth = 0, Scale = 0.75 }
          );

      // the timeline
      myTimeline =
        new Part("Graduated") {
            LayerName = "Adornment",
            Location = new Point(0, 0),
            LocationSpot = Spot.Left,
            LocationElementName = "MAIN",
            GraduatedTickUnit = GridCellWidth
          }
          .Add(
            new Shape("LineH") { Name = "MAIN", StrokeWidth = 0, Height = TimelineHeight, Background = "lightgray" },
            new Shape { Name = "TICKS", GeometryString = "M0 0 V1000", Interval = 7, Stroke = "lightgray", StrokeWidth = 0.5 },
            new TextBlock {
              AlignmentFocus = Spot.Left,
              Interval = 7,  // once per week
              GraduatedFunction = (n, tb) => {  // n is in days since StartDate
                return StartDate.AddDays(n).ToShortDateString();
              }
            }
          );
      myGantt.Add(myTimeline);

      // The Model that is shared by both Diagrams
      var myModel = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = -1, Text = "Project X" },
            new NodeData { Key = 1, Text = "Task 1", Color = "darkgreen" },
              new NodeData { Key = 11, Text = "Task 1.1", Color = "green", Duration = ConvertDaysToUnits(10) },
              new NodeData { Key = 12, Text = "Task 1.2", Color = "green" },
                new NodeData { Key = 121, Text = "Task 1.2.1", Color = "lightgreen", Duration = ConvertDaysToUnits(3) },
                new NodeData { Key = 122, Text = "Task 1.2.2", Color = "lightgreen", Duration = ConvertDaysToUnits(5) },
                new NodeData { Key = 123, Text = "Task 1.2.3", Color = "lightgreen", Duration = ConvertDaysToUnits(4) },
            new NodeData { Key = 2, Text = "Task 2", Color = "darkblue" },
              new NodeData { Key = 21, Text = "Task 2.1", Color = "blue", Duration = ConvertDaysToUnits(15), Start = ConvertDaysToUnits(10) },
              new NodeData { Key = 22, Text = "Task 2.2", Color = "goldenrod" },
                new NodeData { Key = 221, Text = "Task 2.2.1", Color = "yellow", Duration = ConvertDaysToUnits(8) },
                new NodeData { Key = 222, Text = "Task 2.2.2", Color = "yellow", Duration = ConvertDaysToUnits(6) },
              new NodeData { Key = 23, Text = "Task 2.3", Color = "darkorange" },
                new NodeData { Key = 231, Text = "Task 2.3.1", Color = "orange", Duration = ConvertDaysToUnits(11) },
            new NodeData { Key = 3, Text = "Task 3", Color = "maroon" },
              new NodeData { Key = 31, Text = "Task 3.1", Color = "brown", Duration = ConvertDaysToUnits(10) },
              new NodeData { Key = 32, Text = "Task 3.2", Color = "brown" },
                new NodeData { Key = 321, Text = "Task 3.2.1", Color = "lightsalmon", Duration = ConvertDaysToUnits(8) },
                new NodeData { Key = 322, Text = "Task 3.2.2", Color = "lightsalmon", Duration = ConvertDaysToUnits(3) },
                new NodeData { Key = 323, Text = "Task 3.2.3", Color = "lightsalmon", Duration = ConvertDaysToUnits(7) },
                new NodeData { Key = 324, Text = "Task 3.2.4", Color = "lightsalmon", Duration = ConvertDaysToUnits(5), Start = ConvertDaysToUnits(71) },
                new NodeData { Key = 325, Text = "Task 3.2.5", Color = "lightsalmon", Duration = ConvertDaysToUnits(4) },
                new NodeData { Key = 326, Text = "Task 3.2.6", Color = "lightsalmon", Duration = ConvertDaysToUnits(5) }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = -1, To = 1 },
          new LinkData { From = 1, To = 11 },
          new LinkData { From = 1, To = 12 },
          new LinkData { From = 12, To = 121 },
          new LinkData { From = 12, To = 122 },
          new LinkData { From = 12, To = 123 },
          new LinkData { From = -1, To = 2 },
          new LinkData { From = 2, To = 21 },
          new LinkData { From = 2, To = 22 },
          new LinkData { From = 22, To = 221 },
          new LinkData { From = 22, To = 222 },
          new LinkData { From = 2, To = 23 },
          new LinkData { From = 23, To = 231 },
          new LinkData { From = -1, To = 3 },
          new LinkData { From = 3, To = 31 },
          new LinkData { From = 3, To = 32 },
          new LinkData { From = 32, To = 321 },
          new LinkData { From = 32, To = 322 },
          new LinkData { From = 32, To = 323 },
          new LinkData { From = 32, To = 324 },
          new LinkData { From = 32, To = 325 },
          new LinkData { From = 32, To = 326 },
          new LinkData { From = 11, To = 2, Category = "Dep" }
        }
      };

      // share model
      myTasks.Model = myModel;
      myGantt.Model = myModel;
      myModel.UndoManager.IsEnabled = true;

      // just for debugging:
      myModel.Changed += (s, e) => {
        if (e.IsTransactionFinished) {  // show the model data in the page's ModelJson control
          modelJson1.JsonText = e.Model.ToJson();
        }
      };

      // sync viewports
      myTasks.ViewportBoundsChanged += (s, e) => {
        if (ChangingView) return;
        ChangingView = true;
        myGantt.Scale = myTasks.Scale;
        myGantt.Position = new Point(myGantt.Position.X, myTasks.Position.Y);
        myTimeline.Position = new Point(myTimeline.Position.X, myTasks.ViewportBounds.Position.Y);
        ChangingView = false;
      };
      myGantt.ViewportBoundsChanged += (s, e) => {
        if (ChangingView) return;
        ChangingView = true;
        myTasks.Scale = myTasks.Scale;
        myTasks.Position = new Point(myTasks.Position.X, myGantt.Position.Y);
        myTimeline.Position = new Point(myTimeline.Position.X, myGantt.ViewportBounds.Position.Y);
        ChangingView = false;
      };
    }

    // change horizontal scale
    private void rescale() {
      if (myTasks == null || myGantt == null || myTimeline == null) return;
      var val = trackBar1.Value;
      myGantt.Commit(diag => {
        GridCellWidth = val;
        diag.ScrollMargin = new Margin(0, GridCellWidth * 7, 0, 0);
        diag.ToolManager.DraggingTool.GridSnapCellSize = new Size(GridCellWidth, GridCellHeight);
        diag.ToolManager.ResizingTool.CellSize = new Size(GridCellWidth, GridCellHeight);
        diag.ToolManager.ResizingTool.MinSize = new Size(GridCellWidth, GridCellHeight);
        diag.UpdateAllTargetBindings();
        (diag.Layout as GanttLayout).CellHeight = GridCellHeight;
        diag.LayoutDiagram(true);
        myTimeline.GraduatedTickUnit = GridCellWidth;
        diag.Padding = new Margin(TimelineHeight, 0, 0, 0);
        myTasks.Padding = new Margin(TimelineHeight, 0, 0, 0);
      }, null);  // SkipsUndoManager
    }
  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }
  public class NodeData : Model.NodeData {
    public string Color { get; set; }
    public double Start { get; set; } = double.NaN;
    public double Duration { get; set; } = double.NaN;
    public bool IsTreeExpanded { get; set; } = true;

    public override string ToString() {
      return $"Key: {Key}, Text: {Text}";
    }
  }
  public class LinkData : Model.LinkData { }
}
