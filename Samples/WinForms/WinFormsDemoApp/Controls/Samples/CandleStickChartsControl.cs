/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace Demo.Samples.CandlestickCharts {
  [ToolboxItem(false)]
  public partial class CandlestickChartsControl : DemoControl {
    private Diagram myDiagram;

    public CandlestickChartsControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"

   <p>
    Demonstrates candlestick charts in GoDiagram.
  </p>
";

    }

    private void Setup() {

      myDiagram = diagramControl1.Diagram;

      // the template for each attribute in a node's array of item data
      var itemTempl =
        new Panel(PanelType.TableRow).Add(
          new TextBlock { Column = 0 }.Bind(
            new Binding("Text")),
          new Shape {
            Column = 1,
            Alignment = Spot.Left,
            Fill = "slateblue",
            Stroke = "darkblue"
          }.Bind(
            new Binding("Geometry", "", ProduceRange)));

      // node template
      myDiagram.NodeTemplate =
        new Node(PanelType.Auto).Add(
          new Shape { Fill = "white" },
          new Panel(PanelType.Table) {
            Margin = 6,
            ItemTemplate = itemTempl
          }.Bind(
            new Binding("ItemList", "Items")));

      Geometry ProduceRange(object dIn, object _) {
        var d = dIn as FieldData;
        var h = 12;  // total height for the markers
        var w = 3;  // half width for the median marker
        // using constructors is more efficient than calling GraphObject.Make:
        return new Geometry()
          .Add(new PathFigure(d.Min, h / 2, false)
            .Add(new PathSegment(SegmentType.Line, d.Max, h / 2)))
          .Add(new PathFigure(d.Min, 0, false)
            .Add(new PathSegment(SegmentType.Line, d.Min, h)))
          .Add(new PathFigure(d.Max, 0, false)
            .Add(new PathSegment(SegmentType.Line, d.Max, h)))
          .Add(new PathFigure(d.Val - w, 0)
            .Add(new PathSegment(SegmentType.Line, d.Val + w, 0))
            .Add(new PathSegment(SegmentType.Line, d.Val + w, h))
            .Add(new PathSegment(SegmentType.Line, d.Val - w, h).Close()));
      }

      myDiagram.Model = new Model {
        CopyNodeDataFunction = (data, model) => {
          return new NodeData {
            Items = data.Items.ConvertAll(DeepCopyFieldData)
          };
        },
        NodeDataSource = new List<NodeData> {
          new NodeData {
            Items = new List<FieldData> {
              new FieldData { Text = "first", Min = 10, Val = 50, Max = 60 },
              new FieldData { Text = "second", Min = 20, Val = 70, Max = 90 },
              new FieldData { Text = "third", Min = 40, Val = 60, Max = 110 },
              new FieldData { Text = "fourth", Min = 50, Val = 80, Max = 130 } }
          }
        }
      };

      FieldData DeepCopyFieldData(object dIn) {
        var d = dIn as FieldData;
        return new FieldData {
          Text = d.Text,
          Min = d.Min,
          Val = d.Val,
          Max = d.Max
        };
      }

      if (tableLayoutPanel1.HorizontalScroll.Visible) {
        int newWid = tableLayoutPanel1.Width -
                      (2 * System.Windows.Forms.SystemInformation.VerticalScrollBarWidth);
        tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 0, newWid, 0);
        foreach (System.Windows.Forms.Control ctl in tableLayoutPanel1.Controls) {
          ctl.Width = newWid;
        }
      }

    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public List<FieldData> Items { get; set; }
  }

  public class FieldData {
    public string Text { get; set; }
    public int Min { get; set; }
    public int Val { get; set; }
    public int Max { get; set; }
  }

  public class LinkData : Model.LinkData {

  }

}
