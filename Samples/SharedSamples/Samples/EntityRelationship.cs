/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Extensions;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace Demo.Samples.EntityRelationship {
  public partial class EntityRelationship : DemoControl {
    private Diagram _Diagram;

    public EntityRelationship() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      desc1.MdText = DescriptionReader.Read("Samples.EntityRelationship.md");
    }

    private void Setup() {
      // define extra figures
      Figures.DefineExtraFigures();

      _Diagram.AllowDelete = false;
      _Diagram.AllowCopy = false;
      _Diagram.Layout = new ForceDirectedLayout();
      _Diagram.UndoManager.IsEnabled = true;

      var colors = new Dictionary<string, string> {
        { "red", "#be4b15" },
        { "green", "#52ce60" },
        { "blue", "#6ea5f8" },
        { "lightred", "#fd8852" },
        { "lightblue", "#afd4fe" },
        { "lightgreen", "#b9e986" },
        { "pink", "#faadc1" },
        { "purple", "#d689ff" },
        { "orange", "#fdb400" }
      };

      // the template for each attribute in a node's array of item data
      var itemTempl =
        new Panel(PanelType.Horizontal).Add(
          new Shape { DesiredSize = new Size(15, 15), StrokeJoin = LineJoin.Round, StrokeWidth = 3, Stroke = null, Margin = 2 }.Bind(
            new Binding("Figure", "Figure"),
            new Binding("Fill", "Color"),
            new Binding("Stroke", "Color")),
          new TextBlock {
            Stroke = "#333333",
            Font = new Font("Segoe UI", 14, Northwoods.Go.FontWeight.Bold)
          }.Bind(
            new Binding("Text", "Name"))
        );

      var panelExpanderButton =
        Builder.Make<Panel>("PanelExpanderButton", "LIST");  // the name of the element whose visibility this button toggles
      panelExpanderButton.Row = 0;
      panelExpanderButton.Alignment = Spot.TopRight;

      // define the Node template, representing an entity
      _Diagram.NodeTemplate =
        new Node(PanelType.Auto)  // the whole node panel
          {
          SelectionAdorned = true,
          Resizable = true,
          LayoutConditions = LayoutConditions.Standard & ~LayoutConditions.NodeSized,
          FromSpot = Spot.AllSides,
          ToSpot = Spot.AllSides,
          IsShadowed = true,
          ShadowOffset = new Point(3, 3),
          ShadowColor = "#C5C1AA"
        }.Bind(
          new Binding("Location", "Location").MakeTwoWay(),
          // whenever the PanelExpanderButton changes the visible property of the "LIST" panel,
          // clear out any desiredSize set by the ResizingTool.
          new Binding("DesiredSize", "Visible", (v, obj) => { return new Size(double.NaN, double.NaN); }).OfElement("LIST"))
        .Add(
          // define the node's outer shape, which will surround the Table
          new Shape() { Figure = "RoundedRectangle", Fill = "white", Stroke = "#eeeeee", StrokeWidth = 3 },
          new Panel(PanelType.Table) { Margin = 8, Stretch = Stretch.Fill }.Add(
            new RowDefinition { Row = 0, Sizing = Sizing.None })
          .Add(
            // the table header
            new TextBlock {
              Row = 0,
              Alignment = Spot.Center,
              Margin = new Margin(0, 24, 0, 2),  // leave room for Button
              Font = new Font("Segoe UI", 16, Northwoods.Go.FontWeight.Bold)
            }.Bind(
              new Binding("Text", "Key")),
            // the collapse/expand button
            panelExpanderButton,
            // the list of Panels, each showing an attribute
            new Panel(PanelType.Vertical) {
              Name = "LIST",
              Row = 1,
              Padding = 3,
              Alignment = Spot.TopLeft,
              DefaultAlignment = Spot.Left,
              Stretch = Stretch.Horizontal,
              ItemTemplate = itemTempl
            }.Bind(
              new Binding("ItemList", "Items"))
          )  // end Table Panel
        );  // end Node

      // define the Link template, representing a relationship
      _Diagram.LinkTemplate =
        new Link  // the whole link panel
          {
          SelectionAdorned = true,
          LayerName = "Foreground",
          Reshapable = true,
          Routing = LinkRouting.AvoidsNodes,
          Corner = 5,
          Curve = LinkCurve.JumpOver
        }.Add(
          new Shape  // the link shape
            { Stroke = "#303B45", StrokeWidth = 2.5 },
          new TextBlock  // the "from" label
            {
            TextAlign = TextAlign.Center,
            Font = new Font("Segoe UI", 14, Northwoods.Go.FontWeight.Bold),
            Stroke = "#1967B3",
            SegmentIndex = 0,
            SegmentOffset = new Point(double.NaN, double.NaN),
            SegmentOrientation = Orientation.Upright
          }.Bind(
            new Binding("Text", "Text")),
          new TextBlock  // the "to" label
            {
            TextAlign = TextAlign.Center,
            Font = new Font("Segoe UI", 14, Northwoods.Go.FontWeight.Bold),
            Stroke = "#1967B3",
            SegmentIndex = -1,
            SegmentOffset = new Point(double.NaN, double.NaN),
            SegmentOrientation = Orientation.Upright
          }.Bind(
            new Binding("Text", "ToText"))
        );

      // model data
      var model = new Model {
        // deep copy items
        CopyNodeDataFunction = (data, model) => {
          return new NodeData {
            Key = data.Key,
            Items = data.Items.ConvertAll((d) => {
              return new FieldData {
                Name = d.Name,
                Iskey = d.Iskey,
                Figure = d.Figure,
                Color = d.Color
              };
            })
          };
        },
        NodeDataSource = new List<NodeData> {
          new NodeData {
            Key = "Products",
            Items = new List<FieldData> {
              new FieldData { Name = "ProductID", Iskey = true, Figure = "Decision", Color = colors["red"]},
              new FieldData { Name = "ProductName", Iskey = false, Figure = "Hexagon", Color = colors["blue"]},
              new FieldData { Name = "SupplierID", Iskey = false, Figure = "Decision", Color = colors["purple"] },
              new FieldData { Name = "CategoryID", Iskey = false, Figure = "Decision", Color = colors["purple"] }
            }
          },
          new NodeData {
            Key = "Suppliers",
            Items = new List<FieldData> {
              new FieldData { Name = "SupplierID", Iskey = true, Figure = "Decision", Color = colors["red"]},
              new FieldData { Name = "CompanyName", Iskey = false, Figure = "Hexagon", Color = colors["blue"]},
              new FieldData { Name = "ContactName", Iskey = false, Figure = "Hexagon", Color = colors["blue"]},
              new FieldData { Name = "Address", Iskey = false, Figure = "Hexagon", Color = colors["blue"]}
            }
          },
          new NodeData {
            Key = "Categories",
            Items = new List<FieldData> {
              new FieldData { Name = "CategoryID", Iskey = true, Figure = "Decision", Color = colors["red"]},
              new FieldData { Name = "CategoryName", Iskey = false, Figure = "Hexagon", Color = colors["blue"]},
              new FieldData { Name = "Description", Iskey = false, Figure = "Hexagon", Color = colors["blue"]},
              new FieldData { Name = "Picture", Iskey = false, Figure = "TriangleUp", Color = colors["pink"]}
            }
          },
          new NodeData {
            Key = "Order Details",
            Items = new List<FieldData> {
              new FieldData { Name = "OrderID", Iskey = true, Figure = "Decision", Color = colors["red"]},
              new FieldData { Name = "ProductID", Iskey = true, Figure = "Decision", Color = colors["red"]},
              new FieldData { Name = "UnitPrice", Iskey = false, Figure = "Circle", Color = colors["green"]},
              new FieldData { Name = "Quantity", Iskey = false, Figure = "Circle", Color = colors["green"]},
              new FieldData { Name = "Discount", Iskey = false, Figure = "Circle", Color = colors["green"]}
            }
          }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = "Products", To = "Suppliers", Text = "0..N", ToText = "1" },
          new LinkData { From = "Products", To = "Categories", Text = "0..N", ToText = "1" },
          new LinkData { From = "Order Details", To = "Products", Text = "0..N", ToText = "1" }
        }
      };

      _Diagram.Model = model;
    }
  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public List<FieldData> Items { get; set; }
  }

  public class LinkData : Model.LinkData {
    public string ToText { get; set; }
  }

  public class FieldData {
    public string Name { get; set; }
    public bool? Iskey { get; set; }
    public string Figure { get; set; }
    public string Color { get; set; }
  }
}
