using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;

namespace WinFormsSampleControls.SwimLanes {
  [ToolboxItem(false)]
  public partial class SwimLanesControl : System.Windows.Forms.UserControl {
    private Diagram _Diagram;
    
    public SwimLanesControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      saveLoadModel1.SaveClick += (e, obj) => SaveModel();
      saveLoadModel1.LoadClick += (e, obj) => LoadModel();
      btnLayout.Click += (e, obj) => _RelayoutLanes();

      goWebBrowser1.Html = @"
        <p>
      In this design each swimlane is implemented by a <a>Group</a>, and all lanes are inside a ""Pool"" Group.
      Each lane Group has its own <a>Group.Layout</a>, which in this case is a <a>LayeredDigraphLayout</a>.
      Each pool Group has its own custom <a>GridLayout</a> that arranges all of its lanes in a vertical stack.
      That custom layout makes sure all of the pool's lanes have the same length.
      If you don't want each lane/group to have its own layout,
      you could use set the lane group's <a>Group.Layout</a> to null and set the pool group's 
      <a>Group.layout </a> to an instance of <a>SwimLaneLayout</a>, shown at <a href=""SwimLaneLayout"">Swim Lane Layout</a>.  
        </p>
        <p>
      When dragging nodes note that the nodes are limited to stay within the lanes.
      This is implemented by a custom <a>Part.DragComputation</a> function, here named <b>stayInGroup</b>.
      Hold down the Shift key while dragging simple nodes to move the selection to another lane.
      Lane groups cannot be moved between pool groups.
        </p>
        <p>
      A Group (i.e. swimlane) is movable but not copyable.
      When the user moves a lane up or down the lanes automatically re-order.
      You can prevent lanes from being moved and thus re-ordered by setting Group.Movable to false.
        </p>
        <p>
      Each Group is collapsible.
      The previous breadth of that lane is saved in the _SavedBreadth property, to be restored when expanded.
        </p>
        <p>
      When a Group/lane is selected, its custom <a>Part.ResizeAdornmentTemplate</a>
      gives it a broad resize handle at the bottom of the Group
      and a broad resize handle at the right side of the Group.
      This allows the user to resize the ""breadth"" of the selected lane
      as well as the ""length"" of all of the lanes.
      However, the custom <a>ResizingTool</a> prevents the lane from being too narrow
      to hold the <a>Group.Placeholder</a> that represents the subgraph,
      and it prevents the lane from being too short to hold any of the contents of the lanes.
      Each Group/lane is also has a <a>GraphObject.MinSize</a> to keep it from
      being too narrow even if there are no member <a>Part</a>s at all.
        </p>
        <p>
      A different sample has its swim lanes vertically oriented: <a href=""SwimLanesVertical"">Swim Lanes (vertical)</a>.
       </p>
";
    }

    // These parameters need to be set before defining the templates.
    private const double _MINLENGTH = 200; // this controls the minimum length of any swimlane
    private const double _MINBREADTH = 20; // this controls the minimum breadth of any non-collapsed swimlane

    // this may be called to force the lanes to layout again
    private void _RelayoutLanes() {
      foreach (var lane in _Diagram.Nodes) {
        if (lane is not Group g) continue;
        if (g.Category == "Pool") continue;
        g.Layout.IsValidLayout = false;  // force it to be invalid
      }
      _Diagram.LayoutDiagram();
    }

    // this is called after nodes have been moved or lanes resized, to layout all of the Pool Groups again
    public void RelayoutDiagram(Diagram diag) {
      diag.Layout.InvalidateLayout();
      var itr = diag.FindTopLevelGroups();
      while (itr.MoveNext()) {
        var g = itr.Current;
        if (g.Category == "Pool") g.Layout.InvalidateLayout();
      }
      diag.LayoutDiagram();
    }

    // compute the minimum size of a Pool Group needed to hold all the Lane Groups
    public static Size ComputeMinPoolSize(Group pool) {
      var len = _MINLENGTH;
      foreach (var lane in pool.MemberParts) {
        // pools should only contain lanes, not plain Nodes
        if (lane is not Group group) continue;
        var holder = group.Placeholder;
        if (holder != null) {
          var sz = holder.ActualBounds;
          len = Math.Max(len, sz.Width);
        }
      }
      return new Size(len, double.NaN);
    }

    // Compute the minimum size for a particular Lane
    public static Size ComputeLaneSize(Group lane) {
      var sz = ComputeMinLaneSize(lane);
      if (lane.IsSubGraphExpanded) {
        var holder = lane.Placeholder;
        if (holder != null) {
          var hsz = holder.ActualBounds;
          sz.Height = Math.Ceiling(Math.Max(sz.Height, hsz.Height));
        }
      }
      // minimum breadth needs to be big enough to hold the header
      var hdr = lane.FindElement("HEADER");
      if (hdr != null) sz.Height = Math.Ceiling(Math.Max(sz.Height, hdr.ActualBounds.Height));
      return sz;
    }

    // determine the minimum size of a Lane Group, even if collapsed
    public static Size ComputeMinLaneSize(Group lane) {
      if (!lane.IsSubGraphExpanded) return new Size(_MINLENGTH, 1);
      return new Size(_MINLENGTH, _MINBREADTH);
    }

    private void Setup() {
      _Diagram = diagramControl1.Diagram;

      // diagram properties
      // use a custom ResizingTool (along with a custom ResizeAdornment on each Group)
      _Diagram.ToolManager.ResizingTool = new LaneResizingTool(this);
      // use a simple layout that ignores links to stack the top-level Pool Groups next to each other
      _Diagram.Layout = new PoolLayout();
      // don't allow dropping onto the diagram's background unless they are all Groups (lanes or pools)
      _Diagram.MouseDragOver += (e) => {
        if (!e.Diagram.Selection.All((n) => { return n is Group; })) {
          e.Diagram.CurrentCursor = "not-allowed";
        }
      };
      _Diagram.MouseDrop += (e) => {
        if (!e.Diagram.Selection.All((n) => { return n is Group; })) {
          e.Diagram.CurrentTool.DoCancel();
        }
      };
      // a clipboard copied node is pasted into the original node's group (i.e. lane)
      _Diagram.CommandHandler.CopiesGroupKey = true;
      // automatically relayout the swim lanes after dragging the selection
      _Diagram.SelectionMoved += (s, e) => RelayoutDiagram(e.Diagram);   // this DiagramEvent listener is
      _Diagram.SelectionCopied += (s, e) => RelayoutDiagram(e.Diagram);  // defined above
      _Diagram.AnimationManager.IsEnabled = false;
      // enable undo and redo
      _Diagram.UndoManager.IsEnabled = true;

      // this is a Part.DragComputation function for limiting where a Node may be dragged
      // use GRIDPT instead of PT if DraggingTool.IsGridSnapEnabled and movement should snap to grid
      Point stayInGroup(Part part, Point pt, Point gridpt) {
        // dont constrain top level nodes
        var grp = part.ContainingGroup;
        if (grp == null) return pt;
        // try to stay within the background Shape of the Group
        var back = grp.ResizeElement;
        if (back == null) return pt;
        // allow dragging a Node out of a Group if the Shift key is down
        if (part.Diagram.LastInput.Shift) return pt;
        var p1 = back.GetDocumentPoint(Spot.TopLeft);
        var p2 = back.GetDocumentPoint(Spot.BottomRight);
        var b = part.ActualBounds;
        var loc = part.Location;
        // find the padding inside the group's placeholder that is around the member parts
        var m = grp.Placeholder.Padding;
        // now limit the location appropriately
        var x = Math.Max(p1.X + m.Left, Math.Min(pt.X, p2.X - m.Right - b.Width - 1)) + (loc.X - b.X);
        var y = Math.Max(p1.Y + m.Top, Math.Min(pt.Y, p2.Y - m.Bottom - b.Height - 1)) + (loc.Y - b.Y);
        return new Point(x, y);
      }

      _Diagram.NodeTemplate =
        new Node("Auto") {
            DragComputation = stayInGroup // limit dragging of Nodes to stay within the containing Group, defined above
          }
          .Bind("Location", "Loc", Point.Parse, Point.Stringify)
          .Add(
            new Shape("Rectangle") {
              Fill = "white",
              PortId = "",
              Cursor = "pointer",
              FromLinkable = true,
              ToLinkable = true
            },
            new TextBlock {
              Margin = 5
            }.Bind("Text", "Key")
          );

      var groupStyle = new {
        LayerName = "Background",  // all pools and lanes are always behind nodes/links
        Background = "transparent",  // can grab anywhere in bounds
        Movable = true,  // allows users to reorder by dragging
        Copyable = false,  // can't copy lanes or pools
        Avoidable = false,  // don't impede AvoidsNodes routed links
        MinLocation = new Point(double.NaN, double.NegativeInfinity),  // only allow vertical movement
        MaxLocation = new Point(double.NaN, double.PositiveInfinity)
      };

      // hide links between lanes when either lane is collapsed
      void updateCrossLaneLinks(Group group) {
        foreach (var l in group.FindExternalLinksConnected()) {
          l.Visible = l.FromNode.IsVisible() && l.ToNode.IsVisible();
        }
      }

      // each Group is a "swimlane" with a header on the left and a resizable lane on the right
      _Diagram.GroupTemplate =
        new Group("Horizontal") {
            SelectionElementName = "SHAPE",  // selecting a lane causes body of the lane to be highlit, not the whole lane
            Resizable = true, ResizeElementName = "SHAPE",  // the custom ResizeAdornmentTemplate only permits two kinds of resizing
            Layout = new LayeredDigraphLayout {  // automatically lay out the lane's subgraph
              IsInitial = false,  // don't even do initial layout
              IsOngoing = false,  // don't invalidate layout when nodes or links are added or removed
              Direction = 0,
              ColumnSpacing = 10,
              LayeringOption = LayeredDigraphLayering.LongestPathSource
            },
            ComputesBoundsAfterDrag = true,  // needed to prevent recomputing Group.placeholder bounds too soon
            ComputesBoundsIncludingLinks = false,  // to reduce occurrences of links going briefly outside the lane
            ComputesBoundsIncludingLocation = true,  // to support empty space at top-left corner of Links
            HandlesDragDropForMembers = true,  // dont need to define handlers on member Nodes and Links
            MouseDrop = (e, grp) => {  // dropping a copy of some Nodes and Links onto this Group adds them to this Group
              if (!e.Shift) return;  // cannot change groups with an unmodified drag and drop
              // dont allow drag-and-dropping a mix of regular nodes and groups
              if (!e.Diagram.Selection.Any((n) => n is Group)) {
                var ok = (grp as Group).AddMembers(grp.Diagram.Selection, true);
                if (ok) {
                  updateCrossLaneLinks(grp as Group);
                } else {
                  grp.Diagram.CurrentTool.DoCancel();
                }
              } else {
                e.Diagram.CurrentTool.DoCancel();
              }
            },
            SubGraphExpandedChanged = (grp) => {
              var shp = grp.ResizeElement;
              if (grp.Diagram.UndoManager.IsUndoingRedoing) return;
              if (grp.IsSubGraphExpanded) {
                shp.Height = (double)grp["_SavedBreadth"];
              } else {
                grp["_SavedBreadth"] = shp.Height;
                shp.Height = double.NaN;
              }
              updateCrossLaneLinks(grp);
            }
          }
          .Set(groupStyle)
          .Bind("Location", "Loc", Point.Parse, Point.Stringify)
          .Bind(new Binding("IsSubGraphExpanded", "Expanded").MakeTwoWay())
          .Add(
            // the lane header consisting of a Shape and a textblock
            new Panel("Horizontal") {
                Name = "HEADER",
                Angle = 270,  // maybe rotate the header to read sideways going up
                Alignment = Spot.Center
              }
              .Add(
                new Panel("Horizontal")  // this is hidden when the swimlane is collapsed
                  .Bind(new Binding("Visible", "IsSubGraphExpanded").OfElement())
                  .Add(
                    new Shape("Diamond") { Width = 8, Height = 8, Fill = "white" }
                      .Bind("Fill", "Color"),
                    new TextBlock { // the lane label
                        Font = "Segoe UI, 13pt, style=bold",
                        Editable = true,
                        Margin = new Margin(2, 0, 0, 0)
                      }
                      .Bind(new Binding("Text").MakeTwoWay())
                  ),
                Builder.Make<Panel>("SubGraphExpanderButton").Set(new { Margin = 5 })
              ), // end Horizontal Panel
            new Panel("Auto")  // the lane consisting of a background Shape and a Placeholder representing the subgraph
              .Add(
                new Shape("Rectangle") { Name = "SHAPE", Fill = "white" }  // this is the resized object
                  .Bind("Fill", "Color")
                  .Bind("DesiredSize", "Size", Northwoods.Go.Size.Parse, Northwoods.Go.Size.Stringify),
                new Placeholder { Padding = 12, Alignment = Spot.TopLeft },
                new TextBlock {  // this TextBlock is only seen when the swimlane is collapsed
                    Name = "LABEL",
                    Font = "Segoe UI, 13pt, style=bold", Editable = true,
                    Angle = 0, Alignment = Spot.TopLeft, Margin = new Margin(2, 0, 0, 4)
                  }
                  .Bind(
                    new Binding("Visible", "IsSubGraphExpanded", (e, _) => { return !(bool)e; }).OfElement(),
                    new Binding("Text").MakeTwoWay()
                  )
              ) // end Auto panel
          ); // end Group

      // define a custom resize adornment that has two resize handles if the group is expanded
      _Diagram.GroupTemplate.ResizeAdornmentTemplate =
        new Adornment("Spot")
          .Add(
            new Placeholder(),
            new Shape {  // for changing the length of a lane
                Alignment = Spot.Right,
                DesiredSize = new Size(7, 50),
                Fill = "lightblue", Stroke = "dodgerblue",
                Cursor = "col-resize"
              }
              .Bind(
                new Binding("Visible", "", (ad, _) => {
                  var adornedPart = (ad as Adornment).AdornedPart;
                  if (adornedPart is not Group adornedGroup) return false;
                  return adornedGroup.IsSubGraphExpanded;
                }).OfElement()
              ),
            new Shape {  // for changing the breadth of a line
                Alignment = Spot.Bottom,
                DesiredSize = new Size(50, 7),
                Fill = "lightblue", Stroke = "dodgerblue",
                Cursor = "row-resize"
              }
              .Bind(
                new Binding("Visible", "", (ad, _) => {
                  var adornedPart = (ad as Adornment).AdornedPart;
                  if (adornedPart is not Group adornedGroup) return false;
                  return adornedGroup.IsSubGraphExpanded;
                }).OfElement()
              )
          );

      _Diagram.GroupTemplateMap.Add("Pool",
        new Group("Auto") {
            // use a simple Layout that ignores links to stack the "lane" groups on top of each other
            Layout = new PoolLayout { Spacing = new Size(0, 0) } // no space between lanes
          }
          .Set(groupStyle)
          .Bind("Location", "Loc", Point.Parse, Point.Stringify)
          .Add(
            new Shape { Fill = "white" }
              .Bind("Fill", "Color"),
            new Panel("Table") { DefaultColumnSeparatorStroke = "black" }
              .Add(
                new Panel("Horizontal") { Column = 0, Angle = 270 }
                  .Add(
                    new TextBlock {
                      Font = "Segoe UI, 16pt, style=bold",
                      Editable = true,
                      Margin = new Margin(2, 0, 0, 0)
                    }
                    .Bind(new Binding("Text").MakeTwoWay())
                  ),
                new Placeholder { Column = 1 }
              )
          )
      ); // end Pool template

      _Diagram.LinkTemplate =
        new Link {
            Routing = LinkRouting.AvoidsNodes, Corner = 5,
            RelinkableFrom = true, RelinkableTo = true
          }
          .Add(
            new Shape(),
            new Shape { ToArrow = "Standard" }
          );

      // define sample graphs in some of the lanes
      _Diagram.Model = new Model {
        // node data
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "Pool1", Text = "Pool", IsGroup = true, Category = "Pool" },
          new NodeData { Key = "Pool2", Text = "Pool2", IsGroup = true, Category = "Pool" },
          new NodeData { Key = "Lane1", Text = "Lane1", IsGroup = true, Group = "Pool1", Color = "lightblue" },
          new NodeData { Key = "Lane2", Text = "Lane2", IsGroup = true, Group = "Pool1", Color = "lightgreen" },
          new NodeData { Key = "Lane3", Text = "Lane3", IsGroup = true, Group = "Pool1", Color = "lightyellow" },
          new NodeData { Key = "Lane4", Text = "Lane4", IsGroup = true, Group = "Pool1", Color = "orange" },
          new NodeData { Key = "oneA", Group = "Lane1" },
          new NodeData { Key = "oneB", Group = "Lane1" },
          new NodeData { Key = "oneC", Group = "Lane1" },
          new NodeData { Key = "oneD", Group = "Lane1" },
          new NodeData { Key = "twoA", Group = "Lane2" },
          new NodeData { Key = "twoB", Group = "Lane2" },
          new NodeData { Key = "twoC", Group = "Lane2" },
          new NodeData { Key = "twoD", Group = "Lane2" },
          new NodeData { Key = "twoE", Group = "Lane2" },
          new NodeData { Key = "twoF", Group = "Lane2" },
          new NodeData { Key = "twoG", Group = "Lane2" },
          new NodeData { Key = "fourA", Group = "Lane4" },
          new NodeData { Key = "fourB", Group = "Lane4" },
          new NodeData { Key = "fourC", Group = "Lane4" },
          new NodeData { Key = "fourD", Group = "Lane4" },
          new NodeData { Key = "Lane5", Text = "Lane5", IsGroup = true, Group = "Pool2", Color = "lightyellow" },
          new NodeData { Key = "Lane6", Text = "Lane6", IsGroup = true, Group = "Pool2", Color = "lightgreen" },
          new NodeData { Key = "fiveA", Group = "Lane5" },
          new NodeData { Key = "sixA", Group = "Lane6" }
        },
        // link data
        LinkDataSource = new List<LinkData> {
          new LinkData { From = "oneA", To = "oneB" },
          new LinkData { From = "oneA", To = "oneC" },
          new LinkData { From = "oneB", To = "oneD" },
          new LinkData { From = "oneC", To = "oneD" },
          new LinkData { From = "twoA", To = "twoB" },
          new LinkData { From = "twoA", To = "twoC" },
          new LinkData { From = "twoA", To = "twoF" },
          new LinkData { From = "twoB", To = "twoD" },
          new LinkData { From = "twoC", To = "twoD" },
          new LinkData { From = "twoD", To = "twoG" },
          new LinkData { From = "twoE", To = "twoG" },
          new LinkData { From = "twoF", To = "twoG" },
          new LinkData { From = "fourA", To = "fourB" },
          new LinkData { From = "fourB", To = "fourC" },
          new LinkData { From = "fourC", To = "fourD" }
        }
      };

      //force all lanes layouts to be performed
      _RelayoutLanes();
    }

    private void SaveModel() {
      if (_Diagram == null) return;
      saveLoadModel1.ModelJson = _Diagram.Model.ToJson();
    }

    private void LoadModel() {
      if (_Diagram == null) return;
      _Diagram.Model = Model.FromJson<Model>(saveLoadModel1.ModelJson);
      _Diagram.DelayInitialization(RelayoutDiagram);
    }
  }

  // custom GraphLinksModel used for the Swim Lanes app
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }

  public class NodeData : Model.NodeData {
    public string Loc { get; set; }
    public string Color { get; set; }
    public string Size { get; set; }
    public bool Expanded { get; set; } = true;
  }

  public class LinkData : Model.LinkData { }

  // define a custom Resizing Tool to limit how far one can shrink a lane Group
  public class LaneResizingTool : ResizingTool {
    private readonly SwimLanesControl _App;

    public LaneResizingTool(SwimLanesControl app) : base() {
      _App = app;
    }

    public bool IsLengthening() {
      return Handle.Alignment == Spot.Right;
    }

    public override Size ComputeMinSize() {
      var lane = AdornedElement.Part as Group;

      var msz = SwimLanesControl.ComputeMinLaneSize(lane);  // get the absolute minimum size
      if (IsLengthening()) {  // compute the minimum length of all lanes
        var sz = SwimLanesControl.ComputeMinPoolSize(lane.ContainingGroup);
        msz.Width = Math.Max(msz.Width, sz.Width);
      } else { // find the minimum size of this single lane
        var sz = SwimLanesControl.ComputeLaneSize(lane);
        msz.Width = Math.Max(msz.Width, sz.Width);
        msz.Height = Math.Max(msz.Height, sz.Height);
      }
      return msz;
    }

    public override void Resize(Rect newr) {
      var lane = AdornedElement.Part;
      if (IsLengthening()) {  // changing the length of all lanes
        foreach (var l in lane.ContainingGroup.MemberParts) {
          if (l is not Group) continue;
          var shape = l.ResizeElement;
          if (shape != null) {  // set its DesiredSize length, but leave each breadth alone
            shape.Width = newr.Width;
          }
        }
      } else {  // changing the breadth of a single lane
        base.Resize(newr);
      }
      _App.RelayoutDiagram(Diagram);  // now that the lane has changed size, layout the pool again
    }
  }
  // end LaneResizingTool class

  // define a custom grid layout that makes sure the length of each lane is the same
  // and that each lane is broad enough to hold its subgraph
  public class PoolLayout : GridLayout {
    public PoolLayout() : base() {
      CellSize = new Size(1, 1);
      WrappingColumn = 1;
      WrappingWidth = double.PositiveInfinity;
      IsRealtime = false;  // don't continously layout while dragging
      Alignment = GridAlignment.Position;
      // This sorts based on the location of each Group.
      // This is useful when Groups can be moved up and down in order to change their order.
      Comparer = (a, b) => {
        var ay = a.Location.Y;
        var by = b.Location.Y;
        if (double.IsNaN(ay) || double.IsNaN(by)) return 0;
        if (ay < by) return -1;
        if (ay > by) return 1;
        return 0;
      };
      BoundsComputation = (part, layout) => {
        var rect = part.GetDocumentBounds();
        return rect.Inflate(-1, -1);  // negative strokeWidth of the border Shape
      };
    }

    public override void DoLayout(IEnumerable<Part> coll = null) {
      if (Diagram == null) return;
      Diagram.StartTransaction("PoolLayout");
      var pool = Group;
      if (pool != null && pool.Category == "Pool") {
        // make sure all the Group Shapes are big enough
        var minsize = SwimLanesControl.ComputeMinPoolSize(pool);
        foreach (var lane in pool.MemberParts) {
          if (lane is not Group group) continue;
          if (group.Category != "Pool") {
            var shape = group.ResizeElement;
            if (shape != null) {  // change the DesiredSize to be big enough in both directions
              var sz = SwimLanesControl.ComputeLaneSize(group);
              shape.Width = double.IsNaN(shape.Width) ? minsize.Width : Math.Max(shape.Width, minsize.Width);
              shape.Height = double.IsNaN(shape.Height) ? sz.Height : Math.Max(shape.Height, sz.Height);
              var cell = group.ResizeCellSize;
              if (!double.IsNaN(shape.Width) && !double.IsNaN(cell.Width) && cell.Width > 0) shape.Width = Math.Ceiling(shape.Width / cell.Width) * cell.Width;
              if (!double.IsNaN(shape.Height) && !double.IsNaN(cell.Height) && cell.Height > 0) shape.Height = Math.Ceiling(shape.Height / cell.Height) * cell.Height;
            }
          }
        }
      }
      // now do all of the usual stuff, according to whatever properties have been set on this GridLayout
      base.DoLayout(coll);
      Diagram.CommitTransaction("PoolLayout");
    }
  }

}
