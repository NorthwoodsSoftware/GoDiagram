/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools.Extensions;

namespace Demo.Extensions.ColumnResizing {
  public partial class ColumnResizing : DemoControl {
    private Diagram _Diagram;

    public ColumnResizing() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      desc1.MdText = DescriptionReader.Read("Extensions.ColumnResizing.md");
    }

    private void Setup() {
      _Diagram.ValidCycle = CycleMode.NotDirected;
      _Diagram.UndoManager.IsEnabled = true;
      _Diagram.ToolManager.MouseDownTools.Add(new RowResizingTool());
      _Diagram.ToolManager.MouseDownTools.Add(new ColumnResizingTool());

      // this template is a panel that is used to represent each item in a Panel.ItemList
      // the panel is data bound to the item object
      var fieldTemplate =
        new Panel("TableRow") {  // this Panel is a row in the containing Table
            Background = new Brush("transparent"),  // so this port's background can be picked by the mouse
            FromSpot = Spot.Right,  // links only go from the right side to the left side
            ToSpot = Spot.Left,
            // allow drawing links from or to this port:
            FromLinkable = true, ToLinkable = true
          }
          .Bind(new Binding("PortId", "Name"))  // this Panel is a "port"
          .Add(
            new Shape {
                Column = 0,
                Width = 12, Height = 12, Margin = 4,
                // but disallow drawing links from or to this shape:
                FromLinkable = false, ToLinkable = false
              }
             .Bind("Figure", "Figure")
             .Bind("Fill", "Color"),
            new TextBlock {
                Column = 1,
                Margin = new Margin(0, 2),
                Stretch = Stretch.Horizontal,
                Font = new Font("Segoe UI", 13, Northwoods.Go.FontWeight.Bold),
                Wrap = Wrap.None,
                Overflow = Overflow.Ellipsis,
                // and disallow drawing links from or to this text:
                FromLinkable = false, ToLinkable = false
              }
              .Bind("Text", "Name"),
            new TextBlock {
                Column = 2,
                Margin = new Margin(0, 2),
                Stretch = Stretch.Horizontal,
                Font = new Font("Segoe UI", 13),
                MaxLines = 3,
                Overflow = Overflow.Ellipsis,
                Editable = true
              }
              .Bind(new Binding("Text", "Info").MakeTwoWay())
          );

      // return initialization for a ColumnDefinition, specifying a particular column
      // and adding a Binding of ColumnDefinition.Width to the IDX'th number in the data.Widths Array
      Binding MakeWidthBinding(int idx) {
        // These two conversion functions are closed over the IDX variable.
        // This source-to-target conversion extracts a number from the Array at the given index.
        object GetColumnWidth(object val) {
          if (val is List<double> arr && idx < arr.Count) return arr[idx];
          return double.NaN;
        }
        // This target-to-source conversion sets a number in the Array at the given index.
        List<double> SetColumnWidth(object val, object data, IModel model) {
          var w = (double)val;
          var arr = (data as NodeData).Widths;
          if (arr == null) arr = new List<double>();
          if (idx >= arr.Count) {
            for (var i = arr.Count; i <= idx; i++) arr[i] = double.NaN; // default to NaN
          }
          arr[idx] = w;
          return arr;  // need to return the Array (as the value of data.Widths)
        }

        return new Binding("Width", "Widths", GetColumnWidth, SetColumnWidth);
      }

      // This template represents a whole "record"
      _Diagram.NodeTemplate =
        new Node("Auto")
          .Bind("Location", "Loc", Point.Parse, Point.Stringify)
          .Add(
            // this rectangular shape surrounds the contents of the node
            new Shape { Fill = "#EEEEEE" },
            // the content consists of a header and a list of items
            new Panel("Vertical") { Stretch = Stretch.Horizontal, Margin = 0.5 }
              .Add(
                // this is the header for the whole node
                new Panel("Auto") { Stretch = Stretch.Horizontal }  // as wide as the whole node
                  .Add(
                    new Shape { Fill = "#1570A6", StrokeWidth = 0 },
                    new TextBlock {
                        Alignment = Spot.Center,
                        Margin = 3,
                        Stroke = "white",
                        TextAlign = TextAlign.Center,
                        Font = new Font("Segoe UI", 16, Northwoods.Go.FontWeight.Bold),
                      }
                      .Bind("Text", "Key")
                  ),
                // this panel holds a panel for each item object in the ItemList
                // each item panel is defined by the ItemTemplate to be a TableRow in this Table
                new Panel("Table") {
                    Name = "TABLE", Stretch = Stretch.Horizontal,
                    MinSize = new Size(100, 10),
                    DefaultAlignment = Spot.Left,
                    DefaultStretch = Stretch.Horizontal,
                    DefaultColumnSeparatorStroke = "gray",
                    DefaultRowSeparatorStroke = "gray",
                    ItemTemplate = fieldTemplate
                  }
                  .Add(
                    new ColumnDefinition { Column = 0 }.Bind(MakeWidthBinding(0)),
                    new ColumnDefinition { Column = 1 }.Bind(MakeWidthBinding(1)),
                    new ColumnDefinition { Column = 2 }.Bind(MakeWidthBinding(2))
                  )
                  .Bind("ItemList", "Fields")  // end Table Panel of items
              )  // end Vertical Panel
          );  // end Node

      _Diagram.LinkTemplate =
        new Link { RelinkableFrom = true, RelinkableTo = true, ToShortLength = 4 }  // let user reconnect links
          .Add(
            new Shape { StrokeWidth = 1.5 },
            new Shape { ToArrow = "Standard", Stroke = null }
          );

      _Diagram.Model = new Model {
        LinkFromPortIdProperty = "FromPort",
        LinkToPortIdProperty = "ToPort",
        NodeDataSource = new List<NodeData>() {
          new NodeData {
            Key = "Record1",
            Widths = new() { double.NaN, double.NaN, 60 },
            Fields = new() {
              new FieldData { Name = "field1", Info = "first field", Color = "#F7B84B", Figure = "Ellipse" },
              new FieldData { Name = "field2", Info = "the second one", Color = "#F25022", Figure = "Ellipse" },
              new FieldData { Name = "fieldThree", Info = "3rd", Color = "#00BCF2" }
            },
            Loc = "0 0"
          },
          new NodeData {
            Key = "Record2",
            Widths = new() { double.NaN, double.NaN, double.NaN },
            Fields = new() {
              new FieldData { Name = "fieldA", Info = "", Color = "#FFB900", Figure = "Ellipse" },
              new FieldData { Name = "fieldB", Info = "", Color = "#F25022", Figure = "Rectangle" },
              new FieldData { Name = "fieldC", Info = "", Color = "#7FBA00", Figure = "Diamond" },
              new FieldData { Name = "fieldD", Info = "fourth", Color = "#00BCF2", Figure = "Rectangle" }
            },
            Loc = "250 0"
          }
        },
        LinkDataSource = new List<LinkData>() {
          new LinkData { From = "Record1", FromPort = "field1", To = "Record2", ToPort = "fieldA" },
          new LinkData { From = "Record1", FromPort = "field2", To = "Record2", ToPort = "fieldD" },
          new LinkData { From = "Record1", FromPort = "fieldThree", To = "Record2", ToPort = "fieldB" }
        }
      };
      _Diagram.Model.Changed += (obj, e) => {
        if (e.IsTransactionFinished) Save();
      };

      Save();  // show the diagram's initial model
    }

    private void Save() {
      if (_Diagram == null) return;
      modelJson1.JsonText = _Diagram.Model.ToJson();
    }
  }

  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { };
  public class NodeData : Model.NodeData {
    public List<double> Widths { get; set; }
    public List<FieldData> Fields { get; set; }
    public string Loc { get; set; }
  }
  public class LinkData : Model.LinkData { }
  public class FieldData {
    public string Name { get; set; }
    public string Info { get; set; }
    public string Color { get; set; }
    public string Figure { get; set; }
  }
}
