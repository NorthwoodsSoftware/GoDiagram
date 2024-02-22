/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;

namespace Demo.Samples.Kanban {
  public partial class Kanban : DemoControl {
    private Diagram _Diagram;

    public Kanban() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      modelJson1.SaveClick = SaveModel;
      modelJson1.LoadClick = LoadModel;

      desc1.MdText = DescriptionReader.Read("Samples.Kanban.md");

      modelJson1.JsonText = @"{
  ""NodeDataSource"": [
    { ""Key"":""Problems"", ""Text"":""Problems"", ""IsGroup"":true, ""Loc"":""0 23.52284749830794"" },
    { ""Key"":""Reproduced"", ""Text"":""Reproduced"", ""IsGroup"":true, ""Color"":0, ""Loc"":""109 23.52284749830794"" },
    { ""Key"":""Identified"", ""Text"":""Identified"", ""IsGroup"":true, ""Color"":0, ""Loc"":""235 23.52284749830794"" },
    { ""Key"":""Fixing"", ""Text"":""Fixing"", ""IsGroup"":true, ""Color"":0, ""Loc"":""343 23.52284749830794"" },
    { ""Key"":""Reviewing"", ""Text"":""Reviewing"", ""IsGroup"":true, ""Color"":0, ""Loc"":""451 23.52284749830794"" },
    { ""Key"":""Testing"", ""Text"":""Testing"", ""IsGroup"":true, ""Color"":0, ""Loc"":""562 23.52284749830794"" },
    { ""Key"":""Customer"", ""Text"":""Customer"", ""IsGroup"":true, ""Color"":0, ""Loc"":""671 23.52284749830794"" },
    { ""Key"":""1"", ""Text"":""text for oneA"", ""Group"":""Problems"", ""Color"":0, ""Loc"":""12 35.52284749830794"" },
    { ""Key"":""2"", ""Text"":""text for oneB"", ""Group"":""Problems"", ""Color"":1, ""Loc"":""12 65.52284749830794"" },
    { ""Key"":""3"", ""Text"":""text for oneC"", ""Group"":""Problems"", ""Color"":0, ""Loc"":""12 95.52284749830794"" },
    { ""Key"":""4"", ""Text"":""text for oneD"", ""Group"":""Problems"", ""Color"":1, ""Loc"":""12 125.52284749830794"" },
    { ""Key"":""5"", ""Text"":""text for twoA"", ""Group"":""Reproduced"", ""Color"":1, ""Loc"":""121 35.52284749830794"" },
    { ""Key"":""6"", ""Text"":""text for twoB"", ""Group"":""Reproduced"", ""Color"":1, ""Loc"":""121 65.52284749830794"" },
    { ""Key"":""7"", ""Text"":""text for twoC"", ""Group"":""Identified"", ""Color"":0, ""Loc"":""247 35.52284749830794"" },
    { ""Key"":""8"", ""Text"":""text for twoD"", ""Group"":""Fixing"", ""Color"":0, ""Loc"":""355 35.52284749830794"" },
    { ""Key"":""9"", ""Text"":""text for twoE"", ""Group"":""Reviewing"", ""Color"":0, ""Loc"":""463 35.52284749830794"" },
    { ""Key"":""10"", ""Text"":""text for twoF"", ""Group"":""Reviewing"", ""Color"":1, ""Loc"":""463 65.52284749830794"" },
    { ""Key"":""11"", ""Text"":""text for twoG"", ""Group"":""Testing"", ""Color"":0, ""Loc"":""574 35.52284749830794"" },
    { ""Key"":""12"", ""Text"":""text for fourA"", ""Group"":""Customer"", ""Color"":1, ""Loc"":""683 35.52284749830794"" },
    { ""Key"":""13"", ""Text"":""text for fourB"", ""Group"":""Customer"", ""Color"":1, ""Loc"":""683 65.52284749830794"" },
    { ""Key"":""14"", ""Text"":""text for fourC"", ""Group"":""Customer"", ""Color"":1, ""Loc"":""683 95.52284749830794"" },
    { ""Key"":""15"", ""Text"":""text for fourD"", ""Group"":""Customer"", ""Color"":0, ""Loc"":""683 125.52284749830794"" },
    { ""Key"":""16"", ""Text"":""text for fiveA"", ""Group"":""Customer"", ""Color"":0, ""Loc"":""683 155.52284749830795"" }
  ],
  ""LinkDataSource"": []
}";

      Setup();
    }

    private void Setup() {
      // make sure the top-left corner of the viewport is occupied
      _Diagram.ContentAlignment = Spot.TopLeft;
      // use a simple layout to stack the top-level Groups next to each other
      _Diagram.Layout = new PoolLayout();
      // Disallow nodes to be dragged to the diagram's background
      _Diagram.MouseDrop = (e) => {
        e.Diagram.CurrentTool.DoCancel();
      };
      // a clipboard copied node is pasted into the original node's lane
      _Diagram.CommandHandler.CopiesGroupKey = true;
      // automatically relayout the swim lanes after dragging the selection
      _Diagram.SelectionMoved += relayoutDiagram; // this DiagramEvent listener is
      _Diagram.SelectionCopied += relayoutDiagram; // defined above
      _Diagram.UndoManager.IsEnabled = true;
      // allow TextEditingTool to start without selecting first
      _Diagram.ToolManager.TextEditingTool.Starting = TextEditingStarting.SingleClick;

      // Customize the dragging tool:
      // When dragging a node set its opacity to 0.6 and move it to be in front of other nodes
      _Diagram.ToolManager.DraggingTool = new CustomDraggingTool();

      var noteColors = new[] { "#009CCC", "#CC293D", "#FFD700" };
      object getNoteColor(object num, object _) {
        return noteColors[Math.Min((int)num, noteColors.Length - 1)];
      }

      _Diagram.NodeTemplate =
        new Node("Horizontal")
          .Add(
            new Shape("Rectangle") {
                Fill = "#009CCC", StrokeWidth = 1, Stroke = "#009CCC",
                Width = 6, Stretch = Stretch.Vertical, Alignment = Spot.Left,
                // if a user clicks the colored portion of a node, cycle through colors
                Click = (e, obj) => {
                  _Diagram.StartTransaction("Update node color");
                  var newColor = 1 + (obj.Part.Data as NodeData).Color;
                  if (newColor > noteColors.Length - 1) newColor = 0;
                  _Diagram.Model.Set(obj.Part.Data, "Color", newColor);
                  _Diagram.CommitTransaction("Update node color");
                }
              }
              .Bind("Fill", "Color", getNoteColor)
              .Bind("Stroke", "Color", getNoteColor),
            new Panel("Auto")
              .Add(
                new Shape("Rectangle") { Fill = "white", Stroke = "#CCCCCC" },
                new Panel("Table") {
                    Width = 130, MinSize = new Size(double.NaN, 50)
                  }
                  .Add(
                    new TextBlock {
                        Name = "TEXT",
                        Margin = 6, Font = new Font("Segoe UI", 11), Editable = true,
                        Stroke = "#000", MaxSize = new Size(130, double.NaN),
                        Alignment = Spot.TopLeft
                      }
                      .Bind(new Binding("Text").MakeTwoWay())
                  )
              )
          );

      // While dragging, highlight the dragged-over group
      void highlightGroup(object grp, object show) {
        if ((bool)show) {
          var part = _Diagram.ToolManager.DraggingTool.CurrentPart;
          if (part.ContainingGroup != grp) {
            (grp as Group).IsHighlighted = true;
            return;
          }
        }
        (grp as Group).IsHighlighted = false;
      }

      _Diagram.GroupTemplate =
        new Group("Vertical") {
            Selectable = false,
            SelectionElementName = "SHAPE",  // even though its not selectable, this is used in the layout
            LayerName = "Background",  // all lanes are always behind all nodes and links
            Layout = new GridLayout {  // automatically lay out the lane's subgraph
              WrappingColumn = 1,
              CellSize = new Size(1, 1),
              Spacing = new Size(5, 5),
              Alignment = GridAlignment.Position,
              Comparer = (a, b) => {  // can re-order tasks within a lane
                var ay = a.Location.Y;
                var by = b.Location.Y;
                if (double.IsNaN(ay) || double.IsNaN(by)) return 0;
                if (ay < by) return -1;
                if (ay > by) return 1;
                return 0;
              }
            },
            Click = (e, grp) => {  // allow simple click on group to clear selection
              if (!e.Shift && !e.Control && !e.Meta) e.Diagram.ClearSelection();
            },
            ComputesBoundsAfterDrag = true,  // needed to prevent recomputing Group.placeholder bounds too soon
            HandlesDragDropForMembers = true,  // don't need to define handlers on member nodes and links
            MouseDragEnter = (e, grp, prev) => highlightGroup(grp, true),
            MouseDragLeave = (e, grp, next) => highlightGroup(grp, false),
            MouseDrop = (e, grp) => {  // dropping a copy of some Nodes and links onto theis Group adds them to this Group
              // don't allow drag-and-dropping a mix of regular Nodes and Groups
              if (e.Diagram.Selection.All(n => !(n is Group))) {
                var ok = (grp as Group).AddMembers(grp.Diagram.Selection, true);
                if (!ok) grp.Diagram.CurrentTool.DoCancel();
              }
            },
            SubGraphExpandedChanged = (grp) => {
              var shp = grp.SelectionElement;
              if (grp.Diagram.UndoManager.IsUndoingRedoing) return;
              if (grp.IsSubGraphExpanded) {
                shp.Width = (grp.Data as NodeData).SavedBreadth;
              } else {   // remember the original width
                if (!double.IsNaN(shp.Width)) grp.Diagram.Model.Set(grp.Data, "SavedBreadth", shp.Width);
                shp.Width = double.NaN;
              }
            }
          }
          .Bind(
            new Binding("Location", "Loc", Point.Parse, Point.Stringify),
            new Binding("IsSubGraphExpanded", "Expanded").MakeTwoWay()
          )
          .Add(
            // the lane header consisting of a TextBlock and an expander button
            new Panel("Horizontal") {
                Name = "HEADER", Alignment = Spot.Left
              }
              .Add(
                Builder.Make<Panel>("SubGraphExpanderButton").Set(new { Margin = 5 }),  // this remains always visible
                new TextBlock { // the lane label
                    Font = new Font("Segoe UI", 15), Editable = true, Margin = new Margin(2, 0, 0, 0)
                  }
                  .Bind(
                    // this is hidden when the swimlane is collapsed
                    new Binding("Visible", "IsSubGraphExpanded").OfElement(),
                    new Binding("Text").MakeTwoWay()
                  )
              ), // end Horizontal Panel
            new Panel("Auto")  // the lane consisting of a background Shape and a Placeholder representing the subgraph
              .Add(
                new Shape("Rectangle") {  // this is the resized object
                    Name = "SHAPE", Fill = "#F1F1F1", Stroke = null, StrokeWidth = 4 // strokeWidth controls spacing
                  }
                  .Bind(
                    new Binding("Fill", "IsHighlighted", (h, _) => (bool)h ? "#D6D6D6" : "#F1F1F1").OfElement(),
                    new Binding("DesiredSize", "Size", Northwoods.Go.Size.Parse, Northwoods.Go.Size.Stringify)
                  ),
                new Placeholder { Padding = 12, Alignment = Spot.TopLeft },
                new TextBlock {  // this TextBlock is only seen when the swimlane is collapsed
                    Name = "LABEL", Font = new Font("Segoe UI", 15, Northwoods.Go.FontWeight.Bold), Editable = true,
                    Angle = 90, Alignment = Spot.TopLeft, Margin = new Margin(4, 0, 0, 2)
                  }
                  .Bind(
                    new Binding("Visible", "IsSubGraphExpanded", (e, _) => !(bool)e).OfElement(),
                    new Binding("Text").MakeTwoWay()
                  )
              ) // end Auto Panel
          ); // end Group

      // Set up an unmodeled Part as a legend, and place it directly on the diagram.
      _Diagram.Add(
        new Part("Table") {
            Position = new Point(10, 10), Selectable = false, Name = "LEGEND"
          }
          .Add(
            new TextBlock("Key") { Row = 0, Font = new Font("Segoe UI", 14, Northwoods.Go.FontWeight.Bold) },  // end Row 0
            new Panel("Horizontal") { Row = 1, Alignment = Spot.Left }
              .Add(
                new Shape("Rectangle") { DesiredSize = new Size(10, 10), Fill = "#CC293D", Margin = 5 },
                new TextBlock("Halted") { Font = new Font("Segoe UI", 13, Northwoods.Go.FontWeight.Bold) }
              ), // end row 1
            new Panel("Horizontal") { Row = 2, Alignment = Spot.Left }
              .Add(
                new Shape("Rectangle") { DesiredSize = new Size(10, 10), Fill = "#FFD700", Margin = 5 },
                new TextBlock("In Progress") { Font = new Font("Segoe UI", 13, Northwoods.Go.FontWeight.Bold) }
              ), // end row 2
            new Panel("Horizontal") { Row = 3, Alignment = Spot.Left }
              .Add(
                new Shape("Rectangle") { DesiredSize = new Size(10, 10), Fill = "#009CCC", Margin = 5 },
                new TextBlock("Completed") { Font = new Font("Segoe UI", 13, Northwoods.Go.FontWeight.Bold) }
              ), // end row 3
            new Panel("Horizontal") {
                Row = 4,
                Click = (e, node) => {
                  var d = e.Diagram;
                  d.StartTransaction("add node");
                  var sel = d.Selection.FirstOrDefault();
                  if (sel == null) {
                    var it = d.FindTopLevelGroups();
                    if (it.MoveNext()) sel = it.Current;
                  }
                  if (sel != null && sel is not Group) sel = sel.ContainingGroup;
                  if (sel == null) return;
                  var newdata = new NodeData { Group = (string)sel.Key, Loc = "0 9999", Text = "New item " + ((Group)sel).MemberParts.Count, Color = 0 };
                  d.Model.AddNodeData(newdata);
                  d.CommitTransaction("add node");
                  var newnode = d.FindNodeForData(newdata);
                  d.Select(newnode);
                  d.CommandHandler.EditTextBlock();
                  d.CommandHandler.ScrollToPart(newnode);
                },
                Background = "white",
                Margin = new Margin(10, 4, 4, 4)
              }
              .Add(
                new Panel("Auto")
                  .Add(
                    new Shape("Rectangle") { StrokeWidth = 0, Stroke = null, Fill = "#6FB583" },
                    new Shape("PlusLine") { Margin = 6, StrokeWidth = 2, Width = 12, Height = 12, Stroke = "white", Background = "#6FB583" }
                  ),
                new TextBlock("New item") { Font = new Font("Segoe UI", 10), Margin = 6 }
              )
          )
        );

      LoadModel();
    }



    // define a custom grid layout that makes sure the length of each lane is the same
    // and that each lane is broad enough to hold its subgraph
    public class PoolLayout : GridLayout {
      public int MINLENGTH { get; set; }
      public int MINBREADTH { get; set; }

      public PoolLayout() : base() {
        MINLENGTH = 200; // this controls the minimum length of any swim lane
        MINBREADTH = 100; // this controls the minimum breadth of any swim lane
        CellSize = new Size(1, 1);
        WrappingColumn = int.MaxValue;
        WrappingWidth = int.MaxValue;
        Spacing = new Size(0, 0);
        Alignment = GridAlignment.Position;

        Comparer = (pa, pb) => {
          if (pa.Name == "LEGEND") return -1;
          if (pb.Name == "LEGEND") return 1;
          return 0;
        };
      }


      public override void DoLayout(IEnumerable<Part> coll = null) {
        var diagram = Diagram;
        if (Diagram == null) return;
        Diagram.StartTransaction("PoolLayout");
        // make sure all of the group shapes are big enough
        var minlen = _ComputeMinPoolLength();
        var it = Diagram.FindTopLevelGroups();
        while (it.MoveNext()) {
          var lane = it.Current;
          var shape = lane.SelectionElement;
          if (shape != null) { // change the desired size to be big enough in both direction
            var sz = _ComputeLaneSize(lane);
            shape.Width = (!double.IsNaN(shape.Width)) ? Math.Max(shape.Width, sz.Width) : sz.Width;
            // if you want the height of all the lanes to shrink as the maximum needed height decreases:
            shape.Height = minlen;
            // if you want the height of all the lanes to remain at the maximum height ever needed:
            // shape.Height = (isNaN(shape.Height)) ? minlen : Math.Max(shape.Height, minlen);
            var cell = lane.ResizeCellSize;
            if (!double.IsNaN(shape.Width) && !double.IsNaN(cell.Width) && cell.Width > 0) shape.Width = Math.Ceiling(shape.Width / cell.Width) * cell.Width;
            if (!double.IsNaN(shape.Height) && !double.IsNaN(cell.Height) && cell.Height > 0) shape.Height = Math.Ceiling(shape.Height / cell.Height) * cell.Height;
          }
        }
        // now do all the usual stuff, according to whatever properties have been set on this GridLayout
        base.DoLayout(coll);
        Diagram.CommitTransaction("PoolLayout");
      }

      // compute the minimum length of the whole diagram needed to hold all of the lane Groups
      private double _ComputeMinPoolLength() {
        double len = MINLENGTH;
        var it = Diagram.FindTopLevelGroups();
        while (it.MoveNext()) {
          var lane = it.Current;
          var holder = lane.Placeholder;
          if (holder != null) {
            var sz = holder.ActualBounds;
            len = Math.Max(len, sz.Height);
          }
        }
        return len;
      }

      // compute the minimum size for a particular lane group
      private Size _ComputeLaneSize(Group lane) {
        var sz = new Size(lane.IsSubGraphExpanded ? MINBREADTH : 1, MINLENGTH);
        if (lane.IsSubGraphExpanded) {
          var holder = lane.Placeholder;
          if (holder != null) {
            var hsz = holder.ActualBounds;
            sz.Width = Math.Max(sz.Width, hsz.Width);
          }
        }
        // minimum breadth needs to be big enough to hold the header
        var hdr = lane.FindElement("HEADER");
        if (hdr != null) sz.Width = Math.Max(sz.Width, hdr.ActualBounds.Width);
        return sz;
      }
    }

    // Customize the dragging tool:
    // When dragging a node set its opacity to 0.6 and move it to be in front of other nodes
    public class CustomDraggingTool : DraggingTool {
      public override void DoActivate() {
        base.DoActivate();
        CurrentPart.Opacity = 0.6;
        CurrentPart.LayerName = "Foreground";
      }

      public override void DoDeactivate() {
        CurrentPart.Opacity = 1;
        CurrentPart.LayerName = "";
        base.DoDeactivate();
      }
    }

    // this is called after nodes have been moved
    private void relayoutDiagram(object obj, DiagramEvent e) {
      foreach (var n in _Diagram.Selection) n.InvalidateLayout();
      _Diagram.LayoutDiagram();
    }

    private void SaveModel() {
      if (_Diagram == null) return;
      modelJson1.JsonText = _Diagram.Model.ToJson();
    }

    private void LoadModel() {
      if (_Diagram == null) return;
      _Diagram.Model = Model.FromJson<Model>(modelJson1.JsonText);
      _Diagram.Model.UndoManager.IsEnabled = true;
    }
  }

  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }

  public class NodeData : Model.NodeData {
    public int Color { get; set; }
    public string Loc { get; set; }
    public string Size { get; set; }
    public bool Expanded { get; set; } = true;
    public double SavedBreadth { get; set; } = double.NaN;
  }

  public class LinkData : Model.LinkData { }
}
