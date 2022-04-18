﻿/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go.Layouts.Extensions;
using Northwoods.Go.Tools;

namespace WinFormsExtensionControls.Table {
  [ToolboxItem(false)]
  public partial class TableControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    private Palette myPalette;
    private Node myNodeTemplate =  // for regular nodes within cells (groups); you'll want to extend this
      new Node("Auto") {
          Width = 120, Height = 50, Margin = 4  // assume uniform Margin, all around
        }
        .Bind("Row")
        .Bind("Column", "Col")
        .Add(
          new Shape { Fill = "white" }
            .Bind("Fill", "Color"),
          new TextBlock()
            .Bind("Text", "Key")
        );


    public TableControl() {
      InitializeComponent();
      myDiagram = diagramControl1.Diagram;
      myPalette = paletteControl1.Diagram as Palette;

      Setup();
      SetupPalette();
      goWebBrowser1.Html = @"
           <p>
          This sample demonstrates a custom Layout, TableLayout, that is very much like a simplified ""Table"" Panel layout,
          but working on non-Link Parts in a Diagram or a Group rather than on GraphObjects in a Panel.
          The layout is defined in its own file, as <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Layouts/Table/TableLayout.cs"">TableLayout.cs</a>.
           </p>
           <p>
          You can drag-and-drop nodes from the Palette into any Group.
          Dragging into a Group highlights the Group.
          Drops must occur inside Groups; otherwise the action is cancelled.
           </p>
           <p>
          Each row and each column is <a>Part.Resizable</a> and has a custom <a>Part.ResizeAdornmentTemplate</a>
          showing a single resize handle on the right side or on the bottom.
          There is a custom LaneResizingTool to provide a minimum width or height based on the contents of all of the
          groups (cells) in that column or row.
          </p>
          <p>
          This example assumes the Groups are predefined and exist in each cell at a particular row/column,
          but this sample could be extended to allow adding and removing rows and/or columns.
          </p>
";
    }

    private void Setup() {
      myDiagram.Layout =
        new TableLayout()
          .Add(new RowDefinition { Row = 1, Height = 22 })  // fixed size column headers
          .Add(new ColumnDefinition { Column = 1, Width = 22 });  // fixed size row headers

      myDiagram.SelectionMoved += (s, e) => {
        e.Diagram.LayoutDiagram(true);
      };

      myDiagram.ToolManager.ResizingTool = new LaneResizingTool();

      myDiagram.MouseDragOver += e => {
        e.Diagram.CurrentCursor = "not-allowed";
      };

      myDiagram.MouseDrop += e => {
        e.Diagram.CurrentTool.DoCancel();
      };

      myDiagram.NodeTemplateMap.Add("Header",  // an overall table header, at the top
        new Node("Auto") {
            Row = 0, Column = 1, ColumnSpan = 9999,
            Stretch = Stretch.Horizontal,
            Selectable = false, Pickable = false
          }
          .Add(
            new Shape { Fill = "transparent", StrokeWidth = 0 },
            new TextBlock { Alignment = Spot.Center, Font = new Font("Arial", 12, FontWeight.Bold, FontUnit.Point) }
              .Bind("Text")
          ));

      myDiagram.NodeTemplateMap.Add("Sider",  // an overall table header, on the left side
        new Node("Auto") {
            Row = 1, RowSpan = 9999, Column = 0,
            Stretch = Stretch.Vertical,
            Selectable = false, Pickable = false
          }
          .Add(
            new Shape { Fill = "transparent", StrokeWidth = 0 },
            new TextBlock { Alignment = Spot.Center, Font = new Font("Arial", 12, FontWeight.Bold, FontUnit.Point), Angle = 270 }
              .Bind("Text")
          ));

      myDiagram.NodeTemplateMap.Add("Column Header",  // for each column header
        new Node("Spot") {
            Row = 1, RowSpan = 9999, Column = 2,
            MinSize = new Size(100, double.NaN),
            Stretch = Stretch.Fill,
            Movable = false,
            Resizable = true,
            ResizeAdornmentTemplate =
              new Adornment("Spot")
                .Add(
                  new Placeholder(),
                  new Shape {  // for changing the length of a lane
                    Alignment = Spot.Right,
                    DesiredSize = new Size(7, 50),
                    Fill = "lightblue", Stroke = "dodgerblue",
                    Cursor = "col-resize"
                  }
                )
          }
          .Bind("Column", "Col")
          .Add(
            new Shape { Fill = null }
              .Bind("Fill", "Color"),
            new Panel("Auto") {
                // this is positioned above the Shape, in row 1
                Alignment = Spot.Top, AlignmentFocus = Spot.Bottom,
                Stretch = Stretch.Horizontal,
                Height = (myDiagram.Layout as TableLayout).GetRowDefinition(1).Height
              }
              .Add(
                new Shape { Fill = "transparent", StrokeWidth = 0 },
                new TextBlock {
                    Font = new Font("Arial", 10, FontWeight.Bold, FontUnit.Point), IsMultiline = false,
                    Wrap = Wrap.None, Overflow = Overflow.Ellipsis
                  }
                  .Bind("Text")
              )
          )
      );

      myDiagram.NodeTemplateMap.Add("Row Sider",  // for each row header
        new Node("Spot") {
            Row = 2, Column = 1, ColumnSpan = 9999,
            MinSize = new Size(double.NaN, 100),
            Stretch = Stretch.Fill,
            Movable = false,
            Resizable = true,
            ResizeAdornmentTemplate =
              new Adornment("Spot")
                .Add(
                  new Placeholder(),
                  new Shape {  // for changing the breadth of a lane
                    Alignment = Spot.Bottom,
                    DesiredSize = new Size(50, 7),
                    Fill = "lightblue", Stroke = "dodgerblue",
                    Cursor = "row-resize"
                  }
                )
          }
          .Bind("Row")
          .Add(
            new Shape { Fill = null }
              .Bind("Fill", "Color"),
            new Panel("Auto") {
                // this is positioned to the left of the Shape, in column 1
                Alignment = Spot.Left, AlignmentFocus = Spot.Right,
                Stretch = Stretch.Vertical, Angle = 270,
                Height = (myDiagram.Layout as TableLayout).GetColumnDefinition(1).Width
              }
              .Add(
                new Shape { Fill = "transparent", StrokeWidth = 0 },
                new TextBlock {
                    Font = new Font("Arial", 10, FontWeight.Bold, FontUnit.Point), IsMultiline = false,
                    Wrap = Wrap.None, Overflow = Overflow.Ellipsis
                  }
                  .Bind("Text")
              )
          )
      );

      myDiagram.NodeTemplate = myNodeTemplate;

      myDiagram.GroupTemplate =  // for cells
        new Group("Auto") {
            LayerName = "Background",
            Stretch = Stretch.Fill,
            Selectable = false,
            ComputesBoundsAfterDrag = true,
            ComputesBoundsIncludingLocation = true,
            HandlesDragDropForMembers = true,  // don't need to define handlers on member Nodes and Links
            MouseDragEnter = (e, obj, prev) => { if (obj is Group g) g.IsHighlighted = true; },
            MouseDragLeave = (e, obj, prev) => { if (obj is Group g) g.IsHighlighted = false; },
            MouseDrop = (e, obj) => {
              if (!(obj is Group group)) return;
              // if any dropped part wasn't already a member of this group, we'll want to let the group's row
              // column allow themselves to be resized automatically, in case the row height or column width
              // had been set manually by the LaneResizingTool
              var anynew = e.Diagram.Selection.Any((p) => {
                return p.ContainingGroup != group;
              });
              // don't allow headers/siders to be dropped
              var anyHeadersSiders = e.Diagram.Selection.Any((p) => {
                return p.Category == "Column Header" || p.Category == "Row Sider";
              });
              if (!anyHeadersSiders && group.AddMembers(e.Diagram.Selection, true)) {
                if (anynew) {
                  (e.Diagram.Layout as TableLayout).GetRowDefinition(group.Row).Height = double.NaN;
                  (e.Diagram.Layout as TableLayout).GetColumnDefinition(group.Column).Width = double.NaN;
                }
              } else {  // failure upon trying to add parts to this group
                e.Diagram.CurrentTool.DoCancel();
              }
            }
          }
          .Bind("Row")
          .Bind("Column", "Col")
          .Add(
            // the group is normally unseen -- it is completely transparent except when given a color or when highlighted
            new Shape {
                Fill = "transparent", Stroke = "transparent",
                StrokeWidth = myDiagram.NodeTemplate.Margin.Left,
                Stretch = Stretch.Fill
              }
              .Bind("Fill", "Color")
              .Bind(new Binding("Stroke", "IsHighlighted", h => (bool)h ? "red" : "transparent").OfElement()),
            new Placeholder {
              // leave a margin around the member nodes of a group which is the same as the member node margin
              Alignment = new Spot(0, 0, myDiagram.NodeTemplate.Margin.Top, myDiagram.NodeTemplate.Margin.Left),
              Padding = new Margin(0, myDiagram.NodeTemplate.Margin.Right, myDiagram.NodeTemplate.Margin.Bottom, 0)
            }
          );

      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          // headers
          new NodeData { Key = "Header", Text = "Vacation Procedures", Category = "Header" },
          new NodeData { Key = "Sider", Text = "Personnel", Category = "Sider" },
          // column and row headers
          new NodeData { Key = "Request", Text = "Request", Col = 2, Category = "Column Header" },
          new NodeData { Key = "Approval", Text = "Approval", Col = 3, Category = "Column Header" },
          new NodeData { Key = "Employee", Text = "Employee", Row = 2, Category = "Row Sider" },
          new NodeData { Key = "Manager", Text = "Manager", Row = 3, Category = "Row Sider" },
          new NodeData { Key = "Administrator", Text = "Administrator", Row = 4, Category = "Row Sider" },
          // cells, each a group assigned to a row and column
          new NodeData { Key = "EmpReq", Row = 2, Col = 2, IsGroup = true, Color = "lightyellow" },
          new NodeData { Key = "EmpApp", Row = 2, Col = 3, IsGroup = true, Color = "lightgreen" },
          new NodeData { Key = "ManReq", Row = 3, Col = 2, IsGroup = true, Color = "lightgreen" },
          new NodeData { Key = "ManApp", Row = 3, Col = 3, IsGroup = true, Color = "lightyellow" },
          new NodeData { Key = "AdmReq", Row = 4, Col = 2, IsGroup = true, Color = "lightyellow" },
          new NodeData { Key = "AdmApp", Row = 4, Col = 3, IsGroup = true, Color = "lightgreen" },
          // nodes, each assigned to a group/cell
          new NodeData { Key = "Delta", Color = "orange", Size = new Size(100, 100), Group = "EmpReq" },
          new NodeData { Key = "Epsilon", Color = "coral", Size = new Size(100, 50), Group = "EmpReq" },
          new NodeData { Key = "Zeta", Color = "tomato", Size = new Size(50, 70), Group = "ManReq" },
          new NodeData { Key = "Eta", Color = "coral", Size = new Size(50, 50), Group = "ManApp" },
          new NodeData { Key = "Theta", Color = "tomato", Size = new Size(100, 50), Group = "AdmApp" }
        }
      };
    }

    private void SetupPalette() {
      myPalette.NodeTemplate = myNodeTemplate;

      myPalette.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "Alpha", Color = "orange" },
          new NodeData { Key = "Beta", Color = "tomato" },
          new NodeData { Key = "Gamma", Color = "goldenrod" }
        }
      };
    }
  }

  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { };

  public class NodeData : Model.NodeData {
    public int Row { get; set; }
    public int Col { get; set; }
    public Brush Color { get; set; }
    public Size Size { get; set; }
  }

  public class LinkData : Model.LinkData {}

  public class LaneResizingTool : ResizingTool {
    public override Size ComputeMinSize() {
      var lane = AdornedElement.Part; // might be row or column
      var horiz = (lane.Category) == "Column Header"; // or "Row Header"
      var margin = Diagram.NodeTemplate.Margin;
      var bounds = new Rect();

      var groups = Diagram.FindTopLevelGroups();
      while (groups.MoveNext()) {
        var g = groups.Current;

        if (horiz ? (g.Column == lane.Column) : (g.Row == lane.Row)) {
          var b = Diagram.ComputePartsBounds(g.MemberParts);
          if (b.IsEmpty()) return default; // nothing in there? ignore it
          b = b.Union(g.Location); //keep any space on the left and top
          b = b.AddMargin(margin); // assume the same node margin applies to all nodes
          if (bounds.IsEmpty()) {
            bounds = b;
          } else {
            bounds = bounds.Union(b);
          }
        }
      }

      var msz = base.ComputeMinSize();
      if (bounds.IsEmpty()) return msz;
      return new Size(Math.Max(msz.Width, bounds.Width), Math.Max(msz.Height, bounds.Height));
    }

    public override void Resize(Rect newr) {
      var lane = AdornedElement.Part;
      var horiz = (lane.Category == "Column Header");
      var lay = Diagram.Layout as TableLayout;

      if (horiz) {
        var col = lane.Column;
        var coldef = lay.GetColumnDefinition(col);
        coldef.Width = newr.Width;
      } else {
        var row = lane.Row;
        var rowdef = lay.GetRowDefinition(row);
        rowdef.Height = newr.Height;
      }
      lay.InvalidateLayout();
    }
  }
}
