/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace Demo.Samples.DynamicPieChart {
  [ToolboxItem(false)]
  public partial class DynamicPieChartControl : DemoControl {
    private Diagram myDiagram;

    public DynamicPieChartControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"

  <p>
    This sample demonstrates the ability to build an updateable pie chart with selectable slices.
    The Geometry for each slice is built using a <a>PathFigure</a> with a <a>SegmentType.Arc</a>.
    Slices use a custom <b>Click</b> function, which sets a stroke and offsets slices as they are selected.
    Functionality for ""selection"" and deletion of these slices is similar to the <a href=""demo/SelectableFields"">Selectable Fields sample</a>,
    using some overridden <a>CommandHandler</a> functions.
    Each slice also has a tooltip showing the text and percentage of votes.
  </p>
  <p>
    Poll results can be adjusted and the pie chart will automatically update to reflect any changes.
    This includes deleting selected slices, updating the count using a TextBlock, or using the +/- buttons.
  </p>
";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // const
      const double pieRadius = 100;

      // diagram properties
      // myDiagram.ToolManager.TextEditingTool = TextEditingStarting.SingleClick;
      myDiagram.ModelChanged += OnModelChanged;
      myDiagram.UndoManager.IsEnabled = true;
      myDiagram.CommandHandler = new DynamicPieChartCommandHandler();

      // When a count changes in our model, ensure we trigger a redrawing of each slice in the pie
      void OnModelChanged(object _, ChangedEvent e) {
        if (e.Change == ChangeType.Property && e.PropertyName == "Count") {
          var slicedata = e.Object as SliceData;
          var nodedata = FindNodeDataForSlice(slicedata);
          if (nodedata != null) {
            // Update the count binding to force makeGeo/positionSlice
            (myDiagram.Model as Model).UpdateTargetBindings(nodedata, "Count");
            // If the count went to 0, hide the slice
            var sliceindex = nodedata.Slices.IndexOf(slicedata);
            var slice = (myDiagram.FindNodeForKey(nodedata.Key).FindElement("PIE") as Panel).Elt(sliceindex) as Panel;
            var sliceshape = slice.FindElement("SLICE");
            if (slicedata.Count == 0)
              sliceshape.Visible = false;
            else
              sliceshape.Visible = true;
          }
        }
      }

      var sliceTemplate =
        new Panel().Add(
          new Shape {
            Name = "SLICE",
            StrokeWidth = 2,
            Stroke = "transparent",
            IsGeometryPositioned = true
          }.Bind(
            new Binding("Fill", "Color"),
            new Binding("Geometry", "", MakeGeo)
          )
        ).Bind(
          new Binding("Position", "", PositionSlice)
        ).Set(
          new { // Allow the user to "select" slices when clicking them
            Click = new Action<InputEvent, GraphObject>((e, sliceAsGraphObject) => {
              var slice = sliceAsGraphObject as Panel;
              var sliceShape = slice.FindElement("SLICE") as Shape;
              var sliceData = slice.Data as SliceData;
              var oldskips = slice.Diagram.SkipsUndoManager;
              slice.Diagram.SkipsUndoManager = true;
              if (sliceShape.Stroke == "transparent") {
                sliceShape.Stroke = Brush.Darken(sliceData.Color, 0.4);
                // Move the slice out from the pie when selected
                var nodedata = FindNodeDataForSlice(sliceData);
                if (nodedata != null) {
                  var sliceindex = nodedata.Slices.IndexOf(sliceData);
                  var (start, sweep) = GetAngles(nodedata, sliceindex);
                  if (Math.Abs(sweep - 360) > 0.0001) {
                    var angle = start + sweep / 2.0;
                    var offsetPoint = new Point(pieRadius / 10, 0);
                    slice.Position = offsetPoint.Rotate(angle).Offset(pieRadius / 10, pieRadius / 10);
                  }
                }
              } else {
                sliceShape.Stroke = "transparent";
                slice.Position = new Point(pieRadius / 10, pieRadius / 10);
              }
              slice.Diagram.SkipsUndoManager = oldskips;
            }),
            ToolTip =
              Builder.Make<Adornment>("ToolTip").Add(
                new TextBlock {
                  Font = new Font("Segoe UI", 10, FontWeight.Bold),
                  Margin = 4
                }.Bind(
                  new Binding("Text", "", (dataAsObj, _) => {
                    // Display text and percentage rounded to 2 decimals
                    var data = dataAsObj as SliceData;
                    var nodedata = FindNodeDataForSlice(data);
                    if (nodedata != null) {
                      var pct = (double)data.Count / GetTotalCount(nodedata);
                      return data.Text + ": " +  pct.ToString("P", CultureInfo.InvariantCulture);
                    }
                    return "";
                  })
                )
              )
          }
        );

      var optionTemplate =
        new Panel(PanelType.TableRow).Add(
          new TextBlock {
            Column = 0,
            Font = new Font("Segoe UI", 10, FontWeight.Bold),
            Alignment = Spot.Left,
            Margin = 5
          }.Bind(
            new Binding("Text")
          ),
          new Panel(PanelType.Auto) {
            Column = 1
          }.Add(
            new Shape {
              Fill = "#F2F2F2"
            },
            new TextBlock {
              Font = new Font("Segoe UI", 10),
              TextAlign = TextAlign.Right,
              Margin = 2,
              Wrap = Wrap.None,
              Width = 40,
              Editable = true,
              IsMultiline = false,
              TextValidation = IsValidCount
            }.Bind(
              new Binding("Text", "Count", (c, _) => {
                return (c as int? ?? 0).ToString();
              }).MakeTwoWay((count, _, __) => {
                int num;
                var success = int.TryParse(count as string, out num);
                return success ? num : 0;
              })
            )
          ),
          new Panel(PanelType.Horizontal) {
            Column = 2
          }.Add(
            Builder.Make<Panel>("Button").Set(
              new {
                Click = new Action<InputEvent, GraphObject>(IncrementCount)
              }
            ).Add(
              new Shape {
                Figure = "PlusLine",
                Margin = 3,
                DesiredSize = new Size(7, 7)
              }
            ),
            Builder.Make<Panel>("Button").Set(
              new {
                Click = new Action<InputEvent, GraphObject>(DecrementCount)
              }
            ).Add(
              new Shape {
                Figure = "MinusLine",
                Margin = 3,
                DesiredSize = new Size(7, 7)
              }
            )
          )
        );

      myDiagram.NodeTemplate =
        new Node(PanelType.Vertical) {
          Deletable = false
        }.Add(
          new TextBlock {
            Font = new Font("Segoe UI", 11, FontWeight.Bold),
            Margin = 5
          }.Bind(
            new Binding("Text")
          ),
          new Panel(PanelType.Horizontal).Add(
            new Panel(PanelType.Position) {
              Name = "PIE",
              // account for slices offsetting when selected so the node won't change size
              DesiredSize = new Size(pieRadius * 2.2 + 5, pieRadius * 2.2 + 5),
              ItemTemplate = sliceTemplate
            }.Bind(
              new Binding("ItemList", "Slices")
            ),
            new Panel(PanelType.Table) {
              Margin = 5,
              ItemTemplate = optionTemplate
            }.Bind(
              new Binding("ItemList", "Slices")
            )
          )
        );

      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData {
            Key = 1,
            Text = "Sample Poll",
            Slices = new List<SliceData> {
              new SliceData { Text = "Option 1", Count = 21, Color = "#B378C1" },
              new SliceData { Text = "Option 2", Count = 11, Color = "#F25F5C" },
              new SliceData { Text = "Option 3", Count = 5, Color = "#FFE066" },
              new SliceData { Text = "Option 4", Count = 2, Color = "#2B98C5" },
              new SliceData { Text = "Option 5", Count = 1, Color = "#70C1B3" }
            }
          }
        }
      };

      // Validation function for editing text
      bool IsValidCount(TextBlock textblock, string oldstr, string newstr) {
        if (newstr == "") return false;
        var success = int.TryParse(newstr, out var num); // quick way to convert a string to a number
        return success && num >= 0;
      }

      // Given some slice data, find the corresponding node data
      NodeData FindNodeDataForSlice(SliceData slice) {
        var arr = myDiagram.Model.NodeDataSource as List<NodeData>;
        for (var i = 0; i < arr.Count; i++) {
          var data = arr[i];
          if (data.Slices.IndexOf(slice) >= 0) {
            return data;
          }
        }
        return null;
      }

      object MakeGeo(object dataAsObj, object _) {
        var data = dataAsObj as SliceData;
        var nodedata = FindNodeDataForSlice(data);
        var sliceindex = nodedata.Slices.IndexOf(data);
        var (start, sweep) = GetAngles(nodedata, sliceindex);

        // Constructing the Geomtery this way is much more efficient than calling GraphObject.Make:
        return new Geometry()
          .Add(new PathFigure(pieRadius, pieRadius)  // start point
            .Add(new PathSegment(SegmentType.Arc,
              start, sweep,  // angles
              pieRadius, pieRadius,  // center
              pieRadius, pieRadius)  // radius
              .Close()));
      }

      // Ensure slices get the proper positioning after we update any counts
      object PositionSlice(object dataAsObj, object objAsObj) {
        var data = dataAsObj as SliceData;
        var obj = objAsObj as Panel;
        var nodedata = FindNodeDataForSlice(data);
        var sliceindex = nodedata.Slices.IndexOf(data);
        var (start, sweep) = GetAngles(nodedata, sliceindex);

        var selected = (obj.FindElement("SLICE") as Shape).Stroke != "transparent";
        if (selected && Math.Abs(sweep - 360) > 0.0001) {
          var offsetPoint = new Point(pieRadius / 10, 0); // offset by 1/10 the radius
          offsetPoint = offsetPoint.Rotate(start + sweep / 2); // rotate to the correct angle
          offsetPoint = offsetPoint.Offset(pieRadius / 10, pieRadius / 10); // translate center toward middle of pie panel
          return offsetPoint;
        }
        return new Point(pieRadius / 10, pieRadius / 10);
      }

      // Return total count of a given node
      int GetTotalCount(NodeData nodedata) {
        var totCount = 0;
        for (var i = 0; i < nodedata.Slices.Count; i++) {
          totCount += (nodedata.Slices[i] as SliceData).Count;
        }
        return totCount;
      }

      // Determine start and sweep angles given some node data and the index of the slice
      (double start, double sweep) GetAngles(NodeData nodedata, int index) {
        var totCount = GetTotalCount(nodedata);
        var startAngle = -90.0;
        for (var i = 0; i < index; i++) {
          startAngle += 360.0 * (nodedata.Slices[i] as SliceData).Count / totCount;
        }
        return (startAngle, 360.0 * (nodedata.Slices[index] as SliceData).Count / totCount);
      }

      // When user hits + button, increment count on that option
      void IncrementCount(InputEvent e, GraphObject obj) {
        myDiagram.Model.StartTransaction("increment count");
        var slicedata = obj.Panel.Panel.Data as SliceData;
        myDiagram.Model.Set(slicedata, "Count", slicedata.Count + 1);
        myDiagram.Model.CommitTransaction("increment count");
      }

      // When user hits - button, decrement count on that option
      void DecrementCount(InputEvent e, GraphObject obj) {
        myDiagram.Model.StartTransaction("decrement count");
        var slicedata = obj.Panel.Panel.Data as SliceData;
        if (slicedata.Count > 0)
          myDiagram.Model.Set(slicedata, "Count", slicedata.Count - 1);
        myDiagram.Model.CommitTransaction("decrement count");
      }
    }

  }

  // define the model data
  public class Model : Model<NodeData, int, object> { }
  public class NodeData : Model.NodeData {
    public List<SliceData> Slices { get; set; }
  }


  public class SliceData {
    public string Text { get; set; }
    public int Count { get; set; }
    public string Color { get; set; }
  }

  // Override the standard CommandHandler deleteSelection behavior.
  // If there are any selected slices, delete them instead of deleting any selected nodes or links.
  public class DynamicPieChartCommandHandler : CommandHandler {
    // This is a bit inefficient, but should be OK for normal-sized graphs with reasonable numbers of slices per node
    private List<Panel> FindAllSelectedItems() {
      var slices = new List<Panel>();
      var nit = Diagram.Nodes.GetEnumerator();
      while (nit.MoveNext()) {
        var node = nit.Current;
        var pie = node.FindElement("PIE") as Panel;
        if (pie != null) {
          var sit = pie.Elements.GetEnumerator();
          while (sit.MoveNext()) {
            var slicepanel = sit.Current as Panel;
            if ((slicepanel.FindElement("SLICE") as Shape).Stroke != "transparent") slices.Add(slicepanel);
          }
        }
      }
      return slices;
    }
    public override bool CanDeleteSelection() {
      return base.CanDeleteSelection() || FindAllSelectedItems().Count > 0;
    }

    public override void DeleteSelection() {
      var slices = FindAllSelectedItems();
      if (slices.Count > 0) {  // if there are any selected slices, delete them
        Diagram.StartTransaction("delete slices");
        var nodeset = new HashSet<NodeData>();
        for (var i = 0; i < slices.Count; i++) {
          var panel = slices[i];
          var nodedata = panel.Part.Data as NodeData;
          var slicearray = nodedata.Slices;
          var slicedata = panel.Data as SliceData;
          var sliceindex = slicearray.IndexOf(slicedata);
          // Remove the slice from the model
          Diagram.Model.RemoveListItem(slicearray, sliceindex);
          nodeset.Add(nodedata);
        }
        // Force geometries to be redrawn on any node that had slices deleted
        foreach (var data in nodeset) {
          (Diagram.Model as Model).UpdateTargetBindings(data, "Count");
        }
        Diagram.CommitTransaction("delete slices");
      } else {  // otherwise just delete nodes and/or links, as usual
        base.DeleteSelection();
      }
    }
  }

}
