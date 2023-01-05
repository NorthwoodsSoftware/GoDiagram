/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace Demo.Samples.BarCharts {
  [ToolboxItem(false)]
  public partial class BarChartsControl : DemoControl {
    private Diagram myDiagram;

    public BarChartsControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"

   <p>
    Each node contains a Table Panel whose <a>Panel.ItemList</a> is data bound to the ""items"" property which holds a list of data objects.
    That Table Panel has an <a>Panel.ItemTemplate</a> which creates a bar (a rectangular Shape) and a TextBlock label for each item.

    Each bar also has a tooltip showing the value.
    </p>
";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // the template for each item in a node's array of item data
      var itemTempl =
        new Panel(PanelType.TableColumn) {
          ToolTip = Builder.Make<Adornment>("ToolTip").Add(
            new TextBlock {
              Margin = 4
            }.Bind(
              new Binding("Text", "Val")
            )
          ) as Adornment
        }.Add(
          new Shape {
            Row = 0,
            Alignment = Spot.Bottom,
            Fill = "slateblue",
            Stroke = null,
            Width = 40
          }.Bind(
            new Binding("Height", "Val"),
            new Binding("Fill", "Color")),
          new TextBlock {
            Row = 1
          }.Bind(
            new Binding("Text"))
        );

      // node template
      myDiagram.NodeTemplate =
        new Node(PanelType.Auto).Add(
          new Shape { Fill = "white" },
          new Panel(PanelType.Vertical).Add(
            new Panel(PanelType.Table) { Margin = 6, ItemTemplate = itemTempl }.Bind(
              new Binding("ItemList", "Items")),
            new TextBlock { Font = new Font("Segoe UI", 16, FontWeight.Bold) }.Bind(
              new Binding("Text"))
          )
        );


      // model data
      var model = new Model {
        CopyNodeDataFunction = (data, model) => {
          return new NodeData {
            Key = data.Key,
            Text = data.Text,
            Items = data.Items.ConvertAll(DeepCopyFieldData)
          };
        },
        NodeDataSource = new List<NodeData> {
          new NodeData {
            Key = 1,
            Text = "Before",
            Items = new List<FieldData> {
              new FieldData { Text = "first", Val = 50 },
              new FieldData { Text = "second", Val = 70 },
              new FieldData { Text = "third", Val = 60 },
              new FieldData { Text = "fourth", Val = 80 }
            }
          },
          new NodeData {
            Key = 2,
            Text = "After",
            Items = new List<FieldData> {
              new FieldData { Text = "first", Val = 50 },
              new FieldData { Text = "second", Val = 70 },
              new FieldData { Text = "third", Val = 75, Color = "red" },
              new FieldData { Text = "fourth", Val = 80 }
            }
          }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData {
            From = 1,
            To = 2
          }
        }
      };

      myDiagram.Model = model;

      FieldData DeepCopyFieldData(object dIn) {
        var d = dIn as FieldData;
        return new FieldData {
          Text = d.Text,
          Val = d.Val,
          Color = d.Color
        };
      }
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public List<FieldData> Items { get; set; }
  }

  public class LinkData : Model.LinkData { }

  public class FieldData {
    public string Text { get; set; }
    public int Val { get; set; }
    public string Color { get; set; }
  }

}
