using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools.Extensions;

namespace WinFormsExtensionControls.ColumnResizing {
  [ToolboxItem(false)]
  public partial class ColumnResizingControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    
    public ColumnResizingControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      goWebBrowser1.Html = @"
  <p>
    This makes use of two tools, defined in their own files:
    <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/ColumnResizing/ColumnResizingTool.cs"">ColumnResizingTool.cs</a> and
    <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/RowResizing/RowResizingTool.cs"">RowResizingTool.cs</a>.
    Each tool adds an <a>Adornment</a> to a selected node that has a resize handle for each column or each row of a ""Table"" <a>Panel</a>.
    While resizing, you can press the Tab or the Delete key in order to stop the tool and restore the column or row to its natural size.
  </p>
 
  <p>
    This sample also adds TwoWay Bindings to the <a>RowColumnDefinition.Width</a> property for the columns.
    Each column width is stored in the corresponding index of the node data's ""Widths"" property, which must be an Array of numbers.    
    The default value is NaN, allowing the column to occupy its natural width.      
    Note that there are <b>no</b> Bindings for the row heights.            
  </p>
             
  <p> 
    The model data, automatically updated after each change or undo or redo:
  </p>            
";
      goWebBrowser2.Html = @"
  <p> 
    See also the <a href=""AddRemoveColumns"">Add & Remove Rows & Columns</a> sample.
  </p>
";

    }

    
    private void Save() {
      if (myDiagram == null) return;
      txtJSON.Text = myDiagram.Model.ToJson();
    }

    private void Setup() {

      myDiagram = diagramControl1.Diagram;

      // set diagram properties
      myDiagram.ValidCycle = CycleMode.NotDirected;
      myDiagram.UndoManager.IsEnabled = true;
      myDiagram.ToolManager.MouseDownTools.Add(new RowResizingTool());
      myDiagram.ToolManager.MouseDownTools.Add(new ColumnResizingTool());

      // this template is a panel that is used to represent each item in a Panel.ItemList
      // the panel is data bound to the item object
      var fieldTemplate =
        new Panel(PanelLayoutTableRow.Instance) {
          Background = new Brush("transparent"),
          FromSpot = Spot.Right,
          ToSpot = Spot.Left,
          FromLinkable = true,
          ToLinkable = true
        }.Bind(new Binding("PortId", "Name")).Add(
          new Shape {
            Column = 0,
            Width = 12,
            Height = 12,
            Margin = 4,
            FromLinkable = false,
            ToLinkable = false
          }.Bind(
            new Binding("Figure", "Figure"),
            new Binding("Fill", "Color")
          ),
          new TextBlock {
            Column = 1,
            Margin = new Margin(0, 2),
            Stretch = Stretch.Horizontal,
            Font = "Microsoft Sans Serif, 13px, style=bold",
            Wrap = Wrap.None,
            Overflow = Overflow.Ellipsis,
            FromLinkable = false,
            ToLinkable = false
          }.Bind(
            new Binding("Text", "Name")
          ),
          new TextBlock {
            Column = 2,
            Margin = new Margin(0, 2),
            Stretch = Stretch.Horizontal,
            Font = "Microsoft Sans Serif, 13px, style=bold",
            MaxLines = 3,
            Overflow = Overflow.Ellipsis,
            Editable = true
          }.Bind(
            new Binding("Text", "Info").MakeTwoWay()
          )
        );

      // return initialization for a RowColumnDefinition, specifying a particular column
      // and adding a Binding of RowColumnDefinition.width to the IDX'th number in the data.widths Array
      Binding MakeWidthBinding(int idx) {
        // these two conversion functions are closed over the IDX variable, IDX var is captured
        // this source-to-target conversion extracts a number from the Array at the given index
        object GetColumnWidth(object val, object elt) {
          var arr = val as List<double?>;
          if ((arr != null) && (idx < arr.Count)) return arr.ElementAt(idx);
          return double.NaN;
        }

        List<double?> SetColumnWidth(object val, object dataObj, IModel model) {
          var w = val as double?;
          var data = dataObj as NodeData;
          var arr = data.Widths;
          if (arr == null) {
            arr = new List<double?>();
          }
          if (idx >= arr.Count) {
            for (var i = arr.Count; i <= idx; i++) arr[i] = double.NaN; // default to NaN
          }
          arr[idx] = w;
          return arr;
        }

        return new Binding("Width", "Widths", GetColumnWidth, SetColumnWidth);
      }

      // define panel for each item in the ItemList
      var panel =
        new Panel(PanelLayoutTable.Instance) {
          Name = "TABLE",
          Stretch = Stretch.Horizontal,
          MinSize = new Size(100, 10),
          DefaultAlignment = Spot.Left,
          DefaultColumnSeparatorStroke = new Brush("gray"),
          DefaultRowSeparatorStroke = new Brush("gray"),
          ItemTemplate = fieldTemplate
        };
      // bind width
      panel.GetColumnDefinition(0).Bind(MakeWidthBinding(0));
      panel.GetColumnDefinition(1).Bind(MakeWidthBinding(1));
      panel.GetColumnDefinition(2).Bind(MakeWidthBinding(2));
      // bind itemarray
      panel.Bind(
        new Binding("ItemList", "Fields")
      );



      // This template represents a whole "record"
      myDiagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance).Bind(new Binding("Location", "Loc", Point.Parse).MakeTwoWay(Point.Stringify)).Add(
          // this rectangular shape surrounds the contents of the node
          new Shape {
            Fill = "#EEEEEE"
          },
          // the content consists of a header and a list of items
          new Panel(PanelLayoutVertical.Instance) {
            Stretch = Stretch.Horizontal,
            Alignment = Spot.TopLeft
          }.Add(
            // this is the header for the whole node
            new Panel(PanelLayoutAuto.Instance) {
              Stretch = Stretch.Horizontal  // as wide as the whole node
            }.Add(
              new Shape {
                Fill = "#1570A6",
                Stroke = (Brush)null
              },
              new TextBlock {
                Alignment = Spot.Center,
                Margin = 3,
                Stroke = new Brush("white"),
                TextAlign = TextAlign.Center,
                Font = "Microsoft Sans Serif, 12px, style=bold"
              }.Bind(
                new Binding("Text", "Key")
              )
            ),
            // this panel holds a panel for each item object in the ItemList
            // each item panel is defined by the ItemTemplate to be a TableRow in this Table
            panel
          ) // end vertical panel
        ); // end node

      myDiagram.LinkTemplate =
        new Link {
          RelinkableFrom = true,
          RelinkableTo = true,
          ToShortLength = 4
        }.Add(
          new Shape {
            StrokeWidth = 1.5
          },
          new Shape {
            ToArrow = "Standard",
            Stroke = (Brush)null
          }
        );

      var model =
        new Model {
          LinkFromPortIdProperty = "FromPort",
          LinkToPortIdProperty = "ToPort",
        };

      // set the node data
      model.NodeDataSource = new List<NodeData>() {
        new NodeData {
          Key = "Record1",
          Widths = new List<double?>() { 60, 60, 60 },
          Fields = new List<FieldData>() {
            new FieldData {
              Name = "field1", Info = "first field", Color = "#F7B84B", Figure = "Ellipse"
            },
            new FieldData {
              Name = "field2", Info = "the second one", Color = "#F25022", Figure = "Ellipse"
            },
            new FieldData { // TODO: default figure?
              Name = "fieldThree", Info = "3rd", Color = "#00BCF2", Figure = "Ellipse"
            }
          },
          Loc = "0 0"
        },
        new NodeData {
          Key = "Record2",
          Widths = new List<double?>() { 60, 60, 60 },
          Fields = new List<FieldData>() {
            new FieldData {
              Name = "fieldA", Info = "", Color = "#FFB900", Figure = "Ellipse"
            },
            new FieldData {
              Name = "fieldB", Info = "", Color = "#F25022", Figure = "Rectangle"
            },
            new FieldData {
              Name = "fieldC", Info = "", Color = "#7FBA00", Figure = "Diamond"
            },
            new FieldData {
              Name = "fieldD", Info = "fourth", Color = "#00BCF2", Figure = "Rectangle"
            }
          },
          Loc = "250 0"
        }
      };

      // set the link data
      model.LinkDataSource = new List<LinkData>() {
        new LinkData {
          From = "Record1", FromPort = "field1", To = "Record2", ToPort = "fieldA"
        },
        new LinkData {
          From = "Record1", FromPort = "field2", To = "Record2", ToPort = "fieldD"
        },
        new LinkData {
          From = "Record1", FromPort = "fieldThree", To = "Record2", ToPort = "fieldB"
        }
      };

      myDiagram.Model = model;

      // Dispaly original model data in txtJSON
      Save();

      myDiagram.ModelChanged += (obj, e) => {
        if (e.IsTransactionFinished) {
          Save();
        }
      };

    }

  }

  // define the model types
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { };

  // define the field data
  public class FieldData {
    public string Name { get; set; }
    public string Info { get; set; }
    public string Color { get; set; }
    public string Figure { get; set; }
  }

  // define the node data
  public class NodeData : Model.NodeData {
    public List<double?> Widths { get; set; }
    public List<FieldData> Fields { get; set; }
    public string Loc { get; set; }
  }

  // define the link data
  public class LinkData : Model.LinkData { }


}
