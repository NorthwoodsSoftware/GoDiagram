using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.RecordMapper {
  [ToolboxItem(false)]
  public partial class RecordMapperControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    public RecordMapperControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;
      goWebBrowser1.Html = @"
        <p>
      This record mapper shows a number of ""fields"" for each ""record"" and how they are mapped between each other.
        </p>
        <p>
      Draw new links by dragging from the background of any field.
      Reconnect a selected link by dragging its diamond-shaped handle.
      The ""record"" Nodes use a <a>PanelLayoutTable</a> to place the various fields into rows.
      Records are not copyable or deletable.
        </p>
        <p>
      For a variation on this sample with selectable fields in the record nodes, see the <a href=""SelectableFields"">selectable fields</a> sample.
        </p>
        <p>
      Diagram Model saved in JSON format, automatically updated after each change or undo or redo:
        </p>
";
      textBox1.Leave += (e, obj) => Setup();
      

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      myDiagram.ValidCycle = CycleMode.NotDirected;  // don't allow loops
      // For this sample, automatically show the state of the diagram's model on the page
      myDiagram.ModelChanged += (object sender, ChangedEvent e) => {
        if (e.IsTransactionFinished) ShowModel();
      };
      myDiagram.UndoManager.IsEnabled = true;

      // This template is a Panel that is used to represent each item in a Panel.ItemList.
      // The Panel is data bound to the item object.
      var fieldTemplate =
        new Panel("TableRow") {  // this Panel is a row in the containing Table
          Background = "transparent",  // so this port's background can be picked by the mouse
          FromSpot = Spot.Right,  // links only go from the right side to the left side
          ToSpot = Spot.Left,
          // allow drawing links from or to this port:
          FromLinkable = true, ToLinkable = true
        }
          .Bind("PortId", "Name")
          .Add(
            new Shape {
              Width = 12, Height = 12, Column = 0, StrokeWidth = 2, Margin = 4,
              // but disallow drawing links from or to this shape:
              FromLinkable = false, ToLinkable = false
            }
              .Bind("Figure")
              .Bind("Fill", "Color"),
            new TextBlock {
              Margin = new Margin(0, 5), Column = 1, Font = "Segoe UI, 13px, style=bold",
              Alignment = Spot.Left,
              // and disallow drawing links from or to this text:
              FromLinkable = false, ToLinkable = false
            }
              .Bind("Text", "Name"),
            new TextBlock {
              Margin = new Margin(0, 5), Column = 2, Font = "Segoe UI, 13px, style=bold", Alignment = Spot.Left
            }
              .Bind("Text", "Info")
          );

      // This template represents a whole "record"
      myDiagram.NodeTemplate =
        new Node("Auto") {
          Copyable = false, Deletable = false
        }
          .Bind("Location", "Loc", Point.Parse, Point.Stringify)
          .Add(
            // this rectangular shape surrounds the contents of the node
            new Shape { Fill = "#EEEEEE" },
            // the content consists of a header and a list of items
            new Panel("Vertical")
              .Add(
                // this is the header for the whole node
                new Panel("Auto") {
                  Stretch = Stretch.Horizontal  // as wide as the whole node
                }
                  .Add(
                    new Shape { Fill = "#1570A6", Stroke = null },
                    new TextBlock {
                      Alignment = Spot.Center,
                      Margin = 3,
                      Stroke = "white",
                      TextAlign = TextAlign.Center,
                      Font = "Segoe UI, 12px, style=bold"
                    }
                      .Bind("Text", "Key")
                  ),
                // this panel holds a panel for each item object in the ItemList
                // each item panel is defined by the ItemTemplate to be a TableRow in this Table
                new Panel("Table") {
                  Padding = 2,
                  MinSize = new Size(100, 10),
                  DefaultStretch = Stretch.Horizontal,
                  ItemTemplate = fieldTemplate
                }
                  .Bind("ItemList", "Fields")
            ) // end vertical panel
          ); // end node

      myDiagram.LinkTemplate =
        new Link {
          RelinkableFrom = true, RelinkableTo = true,  // let user reconnect links
          ToShortLength = 4, FromShortLength = 2
        }
          .Add(
            new Shape { StrokeWidth = 1.5 },
            new Shape { ToArrow = "Standard", Stroke = null }
          );

      myDiagram.Model = new Model {
        LinkFromPortIdProperty = "FromPort",
        LinkToPortIdProperty = "ToPort",
        NodeDataSource = new List<NodeData>() {
          new NodeData {
            Key = "Record1",
            Fields = new List<FieldData>() {
              new FieldData { Name = "field1", Info = "", Color = "#F7B84B", Figure = "Ellipse" },
              new FieldData { Name = "field2", Info = "the second one", Color = "#F25022", Figure = "Ellipse" },
              new FieldData { Name = "fieldThree", Info = "3rd", Color = "#00BCF2" }
            },
            Loc = "0 0"
          },
          new NodeData {
            Key = "Record2",
            Fields = new List<FieldData>() {
              new FieldData { Name = "fieldA", Info = "", Color = "#FFB900", Figure = "Diamond" },
              new FieldData { Name = "fieldB", Info = "", Color = "#F25022", Figure = "Rectangle" },
              new FieldData { Name = "fieldC", Info = "", Color = "#7FBA00", Figure = "Diamond" },
              new FieldData { Name = "fieldD", Info = "fourth", Color = "#00BCF2", Figure = "Rectangle" }
            },
            Loc = "280 0"
          }
        },
        LinkDataSource = new List<LinkData>() {
          new LinkData { From = "Record1", FromPort = "field1", To = "Record2", ToPort = "fieldA" },
          new LinkData { From = "Record1", FromPort = "field2", To = "Record2", ToPort = "fieldD" },
          new LinkData { From = "Record1", FromPort = "fieldThree", To = "Record2", ToPort = "fieldB" }
        }
      };


      ShowModel();  // show the diagram's initial model

      
    }

    private void ShowModel() {
      textBox1.Text = myDiagram.Model.ToJson();
      textBox1.ScrollToCaret();
      //myDiagram.Model = Model.FromJson<Model>(textBox1.Text);
      //await InvokeAsync(StateHasChanged);  // tell Blazor to rerender without using an event
    }






  }

  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { };

  public class NodeData : Model.NodeData {
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
