/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.SelectableFields {
  [ToolboxItem(false)]
  public partial class SelectableFieldsControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public SelectableFieldsControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"
        <p>
      This shows a variable number of selectable ""fields"" for each ""record"".
        </p>
        <p>
      Draw new links by dragging from the background of any field.
      Reconnect a selected link by dragging its diamond-shaped handle.
      The user can delete a selected field.
        </p>
        <p>
      The model data, automatically updated after each change or undo or redo:
        </p>
";

      goWebBrowser2.Html = @"
        <p>
      This sample was derived from the <a href=""Records"">Records</a> sample.
        </p>
";

    }

    private void Save() {
      if (myDiagram == null) return;
      textBox1.Text = myDiagram.Model.ToJson();
    }

    private void Setup() {

      myDiagram = diagramControl1.Diagram;

      // set diagram properties
      myDiagram.ValidCycle = CycleMode.NotDirected;
      myDiagram.UndoManager.IsEnabled = true;
      // override CommandHandler
      myDiagram.CommandHandler = new SelectableFieldsCommandHandler();

      // this template is a panel that is used to represent each item in a Panel.ItemList
      // the panel is data bound to the item object
      var fieldTemplate =
        new Panel(PanelLayoutTableRow.Instance) {
          Background = new Brush("transparent"),
          FromSpot = Spot.Right,
          ToSpot = Spot.Left,
          FromLinkable = true,
          ToLinkable = true,
          // allow the user to select items -- background color indicates whether selected
          Click = (e, item) => {
            // assume "transparent" means not "selected", for items
            var oldskips = item.Diagram.SkipsUndoManager;
            item.Diagram.SkipsUndoManager = true;
            if (item.Background == "transparent") {
              item.Background = "dodgerblue";
            } else {
              item.Background = "transparent";
            }
            item.Diagram.SkipsUndoManager = oldskips;
          }
        }.Bind(new Binding("PortId", "Name"))
        .Add(
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
            Margin = new Margin(0, 5),
            Font = new Font("Segoe UI", 13, FontWeight.Bold),
            Alignment = Spot.Left,
            FromLinkable = false,
            ToLinkable = false
          }.Bind(
            new Binding("Text", "Name")
          ),
          new TextBlock {
            Column = 2,
            Margin = new Margin(0, 5),
            Font = new Font("Segoe UI", 13),
            Alignment = Spot.Left
          }.Bind(
            new Binding("Text", "Info").MakeTwoWay()
          )
        );


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
        }.Bind(new Binding("ItemList", "Fields"));



      // This template represents a whole "record"
      myDiagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance)
        .Bind(new Binding("Location", "Loc", Point.Parse).MakeTwoWay(Point.Stringify))
        .Add(
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
                Font = new Font("Segoe UI", 12, FontWeight.Bold)
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
          Fields = new List<FieldData>() {
            new FieldData {
              Name = "fieldA", Info = "", Color = "#FFB900", Figure = "Diamond"
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

      Save();

      // Update JSON if model changes
      myDiagram.ModelChanged += (obj, e) => {
        if (e.IsTransactionFinished) {  // show the model data in the page's TextArea
          Save();
        }
      };

    }

  }

  // define the model types
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { };

  // define the node data
  public class NodeData : Model.NodeData {
    public List<FieldData> Fields { get; set; }
    public string Loc { get; set; }

    public override object Clone() {
      return new NodeData {
        Fields = Fields.ConvertAll((d) => {
          return new FieldData {
            Name = d.Name,
            Info = d.Info,
            Color = d.Color,
            Figure = d.Figure
          };
        }),
        Loc = Loc
      };
    }
  }

  // define the link data
  public class LinkData : Model.LinkData { }

  // define the field data
  public class FieldData {
    public string Name { get; set; }
    public string Info { get; set; }
    public string Color { get; set; }
    public string Figure { get; set; }
  }

  // override the standard CommandHandler DeleteSelection behavior
  public class SelectableFieldsCommandHandler : CommandHandler {

    // If there are any selected items, delete them instead of deleting any selected nodes or links.
    public override bool CanDeleteSelection() {
      // true if there are any selected deletable nodes or links,
      // or if there are any selected items within nodes
      return base.CanDeleteSelection() || FindAllSelectedItems().Count > 0;
    }

    public override void DeleteSelection() {
      var items = FindAllSelectedItems();
      if (items.Count > 0) {  // if there are any selected items, delete them
        Diagram.StartTransaction("delete items");
        for (var i = 0; i < items.Count; i++) {
          var panel = items[i] as Panel;
          var nodedata = panel.Part.Data as NodeData;
          var itemarray = nodedata.Fields;
          var itemdata = panel.Data as FieldData;
          var itemindex = itemarray.IndexOf(itemdata);
          Diagram.Model.RemoveListItem(itemarray, itemindex);
        }
        Diagram.CommitTransaction("delete items");
      } else {  // otherwise just delete nodes and/or links, as usual
        base.DeleteSelection();
      }
    }

    private List<GraphObject> FindAllSelectedItems() {
      var items = new List<GraphObject>();
      var nit = Diagram.Nodes.GetEnumerator();
      while (nit.MoveNext()) {
        var node = nit.Current;
        var table = node.FindElement("TABLE") as Panel;
        if (table != null) {
          var iit = table.Elements.GetEnumerator();
          while (iit.MoveNext()) {
            var itempanel = iit.Current;
            if (itempanel.Background != "transparent") items.Add(itempanel);
          }
        }
      }
      return items;
    }
  }

}
