/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.Linq;

using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;
using Northwoods.Go.PanelLayouts;

namespace Demo.Samples.Gantt {
  public partial class Gantt : DemoControl {
    // Custom layout for _Gantt Diagram
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
          var tasknode = _Tasks.FindNodeForData(nd);
          node.Visible = tasknode.IsVisible();
          node.Move(ConvertStartToX(nd.Start), y);
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
            st = start;  // use given START
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

    private static readonly int GridCellHeight = 20;  // document units; cannot be changed dynamically
    private static int GridCellWidth = 12;  // document units per day; this can be modified -- see Rescale()
    private static readonly int TimelineHeight = 24;  // document units; cannot be changed dynamically

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

    private static Diagram _Tasks;
    private static Diagram _Gantt;
    private static Part _Timeline;
    private static Part _Grid;
    private static Part _HighlightDay;
    private static Part _HighlightTask;

    public Gantt() {
      InitializeComponent();
      _Tasks = tasksControl.Diagram;
      _Gantt = ganttControl.Diagram;

      Setup();

      _InitSlider();

      desc1.MdText = DescriptionReader.Read("Samples.Gantt.md");
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

      void standardContextMenus(Node node) {
        node.ContextMenu =
          Builder.Make<Adornment>("ContextMenu")
            .Add(
              Builder.Make<Panel>("ContextMenuButton")
                .Add(new TextBlock("Details ..."))
                .Apply(btn => {
                  btn.Click = (e, button) => {
                    var task = (button.Part as Adornment).AdornedPart;
                    ShowDialog($"Details: {task.Data}");
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

      // the tree on the left side of the page
      _Tasks.InitialContentAlignment = Spot.Right;
      // make room on top for myTimeline and a bit of spacing; on bottom for whole task row and a bit more
      _Tasks.Padding = new Margin(TimelineHeight + 4, 0, GridCellHeight, 0);  // needs to be the same vertically as for _Gantt
      _Tasks.HasVerticalScrollbar = false;
      _Tasks.AllowMove = false;
      _Tasks.AllowCopy = false;
      _Tasks.CommandHandler.DeletesTree = true;
      _Tasks.Layout =
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
      _Tasks.MouseLeave = (e) => { _HighlightTask.Visible = false; };
      _Tasks.AnimationManager.IsInitial = false;
      _Tasks.TreeCollapsed += (s, e) => _Gantt.LayoutDiagram(true);
      _Tasks.TreeExpanded += (s, e) => _Gantt.LayoutDiagram(true);

      _Tasks.NodeTemplate =
        new Node("Horizontal") {
            ColumnSizing = Sizing.None,
            SelectionAdorned = false,
            Height = GridCellHeight,
            MouseEnter = (e, obj, prev) => {
              var node = obj as Node;
              obj.Background = "rgba(0,0,255,0.2)";
              _HighlightTask.Position = new Point(_Grid.ActualBounds.X, node.ActualBounds.Y);
              _HighlightTask.Width = _Grid.ActualBounds.Width;
              _HighlightTask.Visible = true;
            },
            MouseLeave = (e, obj, prev) => {
              var node = obj as Node;
              node.Background = node.IsSelected ? "dodgerblue" : "transparent";
              _HighlightTask.Visible = false;
            }
          }
          .Bind(
            new Binding("Background", "IsSelected", s => (bool)s ? "dodgerblue" : "transparent").OfElement(),
            new Binding("IsTreeExpanded").MakeTwoWay()
          )
          .Add(
            Builder.Make<Panel>("TreeExpanderButton")
              .Set(new { PortId = "", Scale = 0.85 }),
            new TextBlock { Editable = true }
              .Bind(new Binding("Text").MakeTwoWay())
          )
          .Apply(standardContextMenus);

      _Tasks.LinkTemplate =
        new Link {
            Routing = LinkRouting.Orthogonal,
            FromEndSegmentLength = 1,
            ToEndSegmentLength = 1
          }
          .Add(new Shape());

      _Tasks.LinkTemplateMap["Dep"] =  // ignore these links in the Tasks diagram
        new Link { Visible = false, IsTreeLink = false };

      void ganttMouseOver(InputEvent e) {
        if (_Grid == null || _HighlightDay == null) return;
        var lp = _Grid.GetLocalPoint(e.DocumentPoint);
        var day = Math.Floor(ConvertXToStart(lp.X));
        _HighlightDay.Position = new Point(ConvertStartToX(day), _Grid.Position.Y);
        _HighlightDay.Width = GridCellWidth;  // 1 day
        _HighlightDay.Height = _Grid.ActualBounds.Height;
        _HighlightDay.Visible = true;
      }

      // the right side of the page, holding both the timeline and all of the task bars
      _Gantt.InitialPosition = new Point(-10, -100);  // show labels
      // make room on top for _Timeline and a bit of spacing; on bottom for whole task row and a bit more
      _Gantt.Padding = new Margin(TimelineHeight + 4, GridCellWidth * 7, 0, 0);  // needs to be the same vertically as for _Tasks
      _Gantt.ScrollMargin = new Margin(0, GridCellWidth * 7, GridCellHeight, 0);  // and allow scrolling to a week beyond that
      _Gantt.AllowCopy = false;
      _Gantt.CommandHandler.DeletesTree = true;
      _Gantt.ToolManager.DraggingTool.IsGridSnapEnabled = true;
      _Gantt.ToolManager.DraggingTool.GridSnapCellSize = new Size(GridCellWidth, GridCellHeight);
      _Gantt.ToolManager.DraggingTool.DragsTree = true;
      _Gantt.ToolManager.ResizingTool.IsGridSnapEnabled = true;
      _Gantt.ToolManager.ResizingTool.CellSize = new Size(GridCellWidth, GridCellHeight);
      _Gantt.ToolManager.ResizingTool.MinSize = new Size(GridCellWidth, GridCellHeight);
      _Gantt.Layout = new GanttLayout();
      _Gantt.MouseOver = ganttMouseOver;
      _Gantt.MouseLeave = e => _HighlightDay.Visible = false;
      _Gantt.AnimationManager.IsInitial = false;
      _Gantt.SelectionMoved += (s, e) => e.Diagram.LayoutDiagram(true);
      _Gantt.DocumentBoundsChanged += (s, e) => {
        // the grid extends to only the area needed
        var b = e.Diagram.DocumentBounds;
        var gridpart = e.Diagram.Parts.FirstOrDefault();
        if (gridpart != null && gridpart.Type == PanelLayoutGrid.Instance) {
          gridpart.DesiredSize = new Size(b.Width + GridCellWidth * 7, b.Bottom);
        }
        // the timeline, which is not in the DocumentBounds, only covers the needed area
        // widen to cover whole weeks
        _Timeline.GraduatedMax = Math.Ceiling(b.Width / (GridCellWidth * 7)) * (GridCellWidth * 7);
        _Timeline.FindElement("MAIN").Width = _Timeline.GraduatedMax;
        _Timeline.FindElement("TICKS").Height = Math.Max(e.Diagram.DocumentBounds.Height, e.Diagram.ViewportBounds.Height);
      };

      // the timeline at the top of the _Gantt viewport
      _Timeline =
        new Part("Graduated") {
            LayerName = "Adornment",
            Pickable = false,
            Position = new Point(-26, 0),  // position will be set in "ViewportBoundsChanged" listener
            GraduatedTickUnit = GridCellWidth // each tick is one day
            // assume GraduatedMax == length of line
          }
          .Add(
            new Shape("LineH") {
                Name = "MAIN",
                StrokeWidth = 0,  // don't draw the actual line
                Height = TimelineHeight,  // width will be set in "DocumentBoundsChanged" listener
                Background = "lightgray"
              },
            new Shape("LineV") {
                Name = "TICKS",
                Interval = 7,  // once per week
                AlignmentFocus = new Spot(0.5, 0, 0, -TimelineHeight / 2),  // tick marks cross over the timeline itself
                Stroke = "lightgray", StrokeWidth = 0.5
              },
            new TextBlock {
                AlignmentFocus = Spot.Left,
                Interval = 7,  // once per week
                GraduatedFunction = (n, tb) => {  // N document units after StartDate
                  return StartDate.AddDays(n).ToShortDateString();
                },
                GraduatedSkip = (val, tb) => val > tb.Panel.GraduatedMax - GridCellWidth * 7  // don't show last label
              }
          );
      _Gantt.Add(_Timeline);

      // the grid of horizontal lines
      _Grid =
        new Part("Grid") {
            LayerName = "Grid", Pickable = false, Position = new Point(0, 0), GridCellSize = new Size(3000, GridCellHeight)
          }
          .Add(new Shape("LineH") { StrokeWidth = 0.5 });
      _Gantt.Add(_Grid);

      // the vertical highlighter covering the day where the mouse is
      _HighlightDay =
        new Part {
          LayerName = "Grid", Visible = false, Pickable = false, Background = "rgba(255, 0, 0, 0.2)",
          Position = new Point(0, 0), Width = GridCellWidth, Height = GridCellHeight
        };
      _Gantt.Add(_HighlightDay);

      // the horizontal highlighter covering the current task
      _HighlightTask =
        new Part {
          LayerName = "Grid", Visible = false, Pickable = false, Background = "rgba(0, 0, 255, 0.2)",
          Position = new Point(0, 0), Width = GridCellWidth, Height = GridCellHeight
        };
      _Gantt.Add(_HighlightTask);

      _Gantt.NodeTemplate =
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
                        new TextBlock { Row = 0, Column = 0, ColumnSpan = 9, Font = new Font("Segoe UI", 12, Northwoods.Go.FontWeight.Bold) }.Bind("Text"),
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
                  ),
            MouseOver = (e, obj) => ganttMouseOver(e)
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

      _Gantt.LinkTemplate = new Link { Visible = false };

      _Gantt.LinkTemplateMap["Dep"] =
        new Link {
            Routing = LinkRouting.Orthogonal,
            IsTreeLink = false, IsLayoutPositioned = false,
            FromSpot = new Spot(0.999999, 1), ToSpot = new Spot(0.000001, 0)
          }
          .Add(
            new Shape { Stroke = "brown" },
            new Shape { ToArrow = "Standard", Fill = "brown", StrokeWidth = 0, Scale = 0.75 }
          );

      // The Model that is shared by both Diagrams
      var myModel = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = -1, Text = "Project X" },
            new NodeData { Key = 1, Text = "Task 1", Color = "darkgreen" },
              new NodeData { Key = 11, Text = "Task 1.1", Color = "green", Duration = ConvertDaysToUnits(7) },
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
      _Tasks.Model = myModel;
      _Gantt.Model = myModel;
      myModel.UndoManager.IsEnabled = true;

      // sync viewports
      _Tasks.ViewportBoundsChanged += (s, e) => {
        if (ChangingView) return;
        ChangingView = true;
        _Gantt.Scale = _Tasks.Scale;
        _Gantt.Position = new Point(_Gantt.Position.X, _Tasks.Position.Y);
        _Timeline.Position = new Point(_Timeline.Position.X, _Gantt.ViewportBounds.Position.Y);
        ChangingView = false;
      };
      _Gantt.ViewportBoundsChanged += (s, e) => {
        if (ChangingView) return;
        ChangingView = true;
        _Tasks.Scale = _Gantt.Scale;
        _Tasks.Position = new Point(_Tasks.Position.X, _Gantt.Position.Y);
        _Gantt.Position = new Point(_Gantt.Position.X, _Tasks.Position.Y);  // don't scroll more if _Tasks can't scroll more
        _Timeline.Position = new Point(_Timeline.Position.X, _Gantt.ViewportBounds.Position.Y);
        ChangingView = false;
      };

      // just for debugging:
      myModel.Changed += (s, e) => {
        if (e.IsTransactionFinished) {  // show the model data in the page's ModelJson control
          modelJson1.JsonText = e.Model.ToJson();
        }
      };
    }

    // change horizontal scale
    private void _Rescale(int val) {
      if (_Tasks == null || _Gantt == null || _Timeline == null) return;
      _Gantt.Commit(diag => {
        GridCellWidth = val;
        diag.ScrollMargin = new Margin(0, GridCellWidth * 7, 0, 0);
        diag.ToolManager.DraggingTool.GridSnapCellSize = new Size(GridCellWidth, GridCellHeight);
        diag.ToolManager.ResizingTool.CellSize = new Size(GridCellWidth, GridCellHeight);
        diag.ToolManager.ResizingTool.MinSize = new Size(GridCellWidth, GridCellHeight);
        diag.UpdateAllTargetBindings();
        (diag.Layout as GanttLayout).CellHeight = GridCellHeight;
        diag.LayoutDiagram(true);
        _Timeline.GraduatedTickUnit = GridCellWidth;
        diag.Padding = new Margin(TimelineHeight + 4, GridCellWidth * 7, GridCellHeight, 0);
        _Tasks.Padding = new Margin(TimelineHeight + 4, 0, GridCellHeight, 0);
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
