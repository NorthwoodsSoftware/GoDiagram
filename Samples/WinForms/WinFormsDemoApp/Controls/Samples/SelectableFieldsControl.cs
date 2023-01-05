/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace Demo.Samples.SelectableFields {
  [ToolboxItem(false)]
  public partial class SelectableFieldsControl : DemoControl {
    private Diagram myDiagram;
    private static Brush _UnselectedBrush = "transparent";  // item appearance, if not "selected"
    private static Brush _SelectedBrush = "dodgerblue";  // item appearance, if "selected"

    public SelectableFieldsControl() {
      InitializeComponent();

      myDiagram = diagramControl1.Diagram;
      Setup();

      goWebBrowser1.Html = @"
        <p>
      This shows a variable number of selectable ""fields"" for each ""record"".
        </p>
        <p>
      Draw new links by dragging from the background of any field.
      Reconnect a selected link by dragging its diamond-shaped handle.
      The user can delete a selected field.
        </p>";

      goWebBrowser2.Html = @"
        <p>
      This sample was derived from the <a href=""demo/Records"">Records</a> sample.
        </p>";
    }

    private void Setup() {
      myDiagram.ValidCycle = CycleMode.NotDirected;  // don't allow loops
      // For this sample, automatically show the state of the diagram's model on the page
      myDiagram.ModelChanged += (object sender, ChangedEvent e) => {
        if (e.IsTransactionFinished) ShowModel();
      };
      myDiagram.UndoManager.IsEnabled = true;
      myDiagram = diagramControl1.Diagram;
      // override CommandHandler
      myDiagram.CommandHandler = new SelectableFieldsCommandHandler();

      var setFieldSelected = (GraphObject item, bool sel) => {
        if (sel) {
          item.Background = _SelectedBrush;
        } else {
          item.Background = _UnselectedBrush;
        }
      };

      var onFieldClick = (InputEvent e, GraphObject item) => {
        var oldskips = item.Diagram.SkipsUndoManager;
        item.Diagram.SkipsUndoManager = true;
        if (e.Control || e.Meta) {
          setFieldSelected(item, !IsFieldSelected(item));
          item.Part.IsSelected = item.Panel.Elements.Any(IsFieldSelected);
        } else if (e.Shift) {
          // alternative policy: select all fields between this item and some other one??
          if (!IsFieldSelected(item)) setFieldSelected(item, true);
          item.Part.IsSelected = true;
        } else {
          if (!IsFieldSelected(item)) {
            // deselect all sibling items
            foreach (var it in item.Panel.Elements) {
              if (it != item) setFieldSelected(it, false);
            }
            setFieldSelected(item, true);
          }
          item.Part.IsSelected = true;
        }
        item.Diagram.SkipsUndoManager = oldskips;
      };

      // this template is a panel that is used to represent each item in a Panel.ItemList
      // the panel is data bound to the item object
      var fieldTemplate =
        new Panel("TableRow") {  // this Panel is a row in the containing Table
            Background = _UnselectedBrush,  // so this port's background can be picked by the mouse
            FromSpot = Spot.Right,  // links only go from the right side to the left side
            ToSpot = Spot.Left,
            // allow drawing links from or to this port:
            FromLinkable = true, ToLinkable = true,
            // select items -- the background indicates "selected" when not UnselectedBrush
            Click = onFieldClick
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
                Margin = new Margin(0, 5), Column = 1, Font = new Font("Segoe UI", 13, FontWeight.Bold),
                Alignment = Spot.Left,
                // and disallow drawing links from or to this text:
                FromLinkable = false, ToLinkable = false
              }
              .Bind("Text", "Name"),
            new TextBlock {
                Margin = new Margin(0, 5), Column = 2, Font = new Font("Segoe UI", 13), Alignment = Spot.Left
              }
              .Bind("Text", "Info")
          );


      // This template represents a whole "record"
      myDiagram.NodeTemplate =
        new Node("Auto") { Copyable = false, Deletable = false }
          .Bind("Location", "Loc", Point.Parse, Point.Stringify)
          .Add(
            // this rectangular shape surrounds the contents of the node
            new Shape { Fill = "#EEEEEE" },
            // the content consists of a header and a list of items
            new Panel("Vertical")
              .Add(
                // this is the header for the whole node
                new Panel("Auto") { Stretch = Stretch.Horizontal }  // as wide as the whole node
                  .Add(
                    new Shape { Fill = "#1570A6", Stroke = null },
                    new TextBlock {
                        Alignment = Spot.Center,
                        Margin = 3,
                        Stroke = "white",
                        TextAlign = TextAlign.Center,
                        Font = new Font("Segoe UI", 16, FontWeight.Bold)
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

    internal static bool IsFieldSelected(GraphObject item) {
      return item.Background != _UnselectedBrush;
    }

    private void ShowModel() {
      if (myDiagram == null) return;
      modelJson1.JsonText = myDiagram.Model.ToJson();
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
            if (SelectableFieldsControl.IsFieldSelected(itempanel)) items.Add(itempanel);
          }
        }
      }
      return items;
    }
  }

}
