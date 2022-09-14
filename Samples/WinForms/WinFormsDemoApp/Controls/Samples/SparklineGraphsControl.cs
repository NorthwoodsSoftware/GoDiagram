/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace Demo.Samples.SparklineGraphs {
  [ToolboxItem(false)]
  public partial class SparklineGraphsControl : DemoControl {
    private Diagram myDiagram;

    public SparklineGraphsControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"
        <p>
      This sample shows how to create and align sparkline style charts in a Node, using Spot Panel and <a>Panel.AlignmentFocusName</a>.
        </p>
        <p>
      The Spot Panel's main element is a red vertical line, and its other elements are items in an item list. Instead of aligning to these item list items, which are Horizontal Panels, we want to align to the Shape that represents the sparkline, inside of the Panel.
      To do this we set <code>Panel.AlignmentFocusName</code> to <code>""spark""</code>, and then set the <code>AlignmentFocus</code> in
      <code>makeAlignmentFromValues()</code> to the top left point, plus some offset to normalize the y-axis of the sparklines.
        </p>
        <p>
      The Sparklines exist in a horizontal panel with optional labels at the front, and these sparkline labels also have their alignment computed within the horizontal panel, based on the fractional height of the starting value of the sparkline.
        </p>
";

    }

    private void Setup() {

      myDiagram = diagramControl1.Diagram;

      myDiagram.Layout = new GridLayout {
        WrappingColumn = 2,
        IsOngoing = false
      };
      myDiagram.UndoManager.IsEnabled = true;

      var SPARK_STROKEWIDTH = 1;
      var SPARK_INTERVAL = 3;
      var BASELINE_LENGTH = 75;

      object makeStringFromValues(object _values, object _) {
        var values = _values as List<int>;
        if (values.Count < 1) return "M 0 " + values[0] + " L " + BASELINE_LENGTH + " 0";
        // if only one value, make a line BASELINE_LENGTHpx long at that value
        var str = "M 0 " + Math.Round(-(double)values[0] * SPARK_INTERVAL);
        if (values.Count < 2) str += " L " + BASELINE_LENGTH * SPARK_INTERVAL + " " + Math.Round(-(double)values[0] * SPARK_INTERVAL);

        for (var i = 0; i < values.Count; i++) {
          str += "L " + ((i + 1) * SPARK_INTERVAL) + " " + Math.Round(-(double)values[i] * SPARK_INTERVAL);
        }

        return str;
      }

      // determine y offset
      object makeAlignmentFromValues(object _values, object _) {
        var values = _values as List<int>;
        var min = double.PositiveInfinity;
        for (var i = 0; i < values.Count; i++) {
          min = Math.Min(min, (double)values[i]);
        }
        var y = -min * SPARK_INTERVAL;
        if (min > 0) y = -SPARK_STROKEWIDTH; // add the strokewidth

        return new Spot(0, 1, 0, -y);
      }

      var sparkLine = new Panel(PanelType.Horizontal) {
        Alignment = Spot.Left,
        AlignmentFocusName = "spark",
      }.Bind("AlignmentFocus", "Values", makeAlignmentFromValues).Add(
        new TextBlock {
          Visible = false,
          Margin = 1,
          Font = new Font("Segoe UI", 11),
          Background = "white"
        }.Bind("Alignment", "Values", (v, _) => {
          var values = v as List<int>;
          var min = double.PositiveInfinity;
          var max = double.NegativeInfinity;
          for (var i = 0; i < values.Count; i++) {
            min = Math.Min(min, (double)values[i]);
            max = Math.Max(max, (double)values[i]);
          }
          if (max == min) return Spot.Center;
          return new Spot(0, 1 - Math.Abs(((double)values[0] - min) / (max - min)), 0, 0);
        }
        ).Bind("Stroke", "Color")
         .Bind("Text", "Label")
         .Bind("Visible", "Label", (l, _) => true),
        new Shape {
          Fill = null,
          StrokeWidth = SPARK_STROKEWIDTH,
          Stroke = "gray",
          Name = "spark"
        }.Bind("Stroke", "Color")
         .Bind("GeometryString", "Values", makeStringFromValues)
      ); // end SPARKLINE template

      myDiagram.NodeTemplate = new Node(PanelType.Auto).Add(
        new Shape { Fill = "rgba(200,200,255,0.3)", StrokeWidth = 0 },
        new Panel(PanelType.Spot) {
          ItemTemplate = sparkLine
        }.Bind("ItemList", "Items").Add(
          new Shape {
            Width = 1,
            Height = 200,
            Fill = "red",
            Stroke = null,
            StrokeWidth = 0
          }
        )
      );

      List<int> makeRandomValues(int numberOfValues, int? startValue = null) {
        var rand = new Random();
        var values = new List<int>();
        var last = startValue ?? rand.Next(-30, 30);
        for (var i = 0; i < numberOfValues; i++) {
          var newval = last + rand.Next(-3, 3);
          values.Add(newval);
          last = newval;
        }
        return values;
      }

      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "Alpha",
            Items = new List<ItemData> {
              new ItemData { Color = "#FF69B4", Values = makeRandomValues(50, 20) },
              new ItemData { Color = "#FFB6C1", Values = makeRandomValues(50) },
              new ItemData { Color = "#FF69B4", Values = makeRandomValues(50) },
              new ItemData { Color = "#C71585", Values = makeRandomValues(50, -20) },
              new ItemData { Color = "gray", Values = new List<int> { 0 } }
            }
          },

          new NodeData { Key = "Beta",
            Items = new List<ItemData> {
              new ItemData { Color = "rgba(255,0,0,.6)", Values = makeRandomValues(50), Label = "label A" },
              new ItemData { Color = "rgba(0,0,255,.6)", Values = makeRandomValues(50), Label = "long label B" },
              new ItemData { Color = "darkgray", Values = makeRandomValues(50), Label = "label C" },
              new ItemData { Color = "lime", Values = makeRandomValues(50), Label = "label D" },
              new ItemData { Color = "gray", Values = new List<int> { 0 }  }
            }
          },

          new NodeData { Key = "Gamma",
            Items = new List<ItemData> {
              new ItemData { Color = "rgba(255,0,0,.6)", Values = makeRandomValues(50, -10), Label = "Important\nYear" },
              new ItemData { Color = "gray", Values = makeRandomValues(20, 30) },
              new ItemData { Color = "gray", Values = makeRandomValues(40, 30) },
              new ItemData { Color = "gray", Values = makeRandomValues(50, 30) },
              new ItemData { Color = "gray", Values = new List<int> { 0 } }
            }
          }
        }
      };
    }

  }

  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }

  public class NodeData : Model.NodeData {
    public List<ItemData> Items { get; set; }
  }

  public class LinkData : Model.LinkData { }

  public class ItemData {
    public string Color { get; set; }
    public List<int> Values { get; set; }
    public string Label { get; set; }
  }

}
