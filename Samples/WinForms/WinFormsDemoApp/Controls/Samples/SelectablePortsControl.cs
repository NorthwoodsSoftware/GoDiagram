/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace Demo.Samples.SelectablePorts {
  [ToolboxItem(false)]
  public partial class SelectablePortsControl : DemoControl {
    private Diagram myDiagram;
    public static Brush _UnselectedBrush = "lightgray";  // item appearance, if not "selected"
    public static Brush _SelectedBrush = "dodgerblue";  // item appearance, if "selected"

    public SelectablePortsControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"
        <p>
      Click on a port to toggle its selection.
      The Delete command will only delete selected ports, if there are any; otherwise it will delete Nodes and Links as it normally would.
        </p>
";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      // automatically show the state of the diagram's model on the page
      myDiagram.ModelChanged += (_, e) => {
        if (e.IsTransactionFinished) ShowModel();
      };
      myDiagram.UndoManager.IsEnabled = true;
      // override CommandHandler
      myDiagram.CommandHandler = new SelectablePortsCommandHandler();

      var setPortSelected = (GraphObject item, bool sel) => {
        if (item is not Shape shape) return;
        if (sel) {
          shape.Fill = _SelectedBrush;
        } else {
          shape.Fill = _UnselectedBrush;
        }
      };

      var onPortClick = (InputEvent e, GraphObject tb) => {
        var shape = tb.Panel.FindElement("SHAPE");
        if (shape != null) {
          var oldskips = shape.Diagram.SkipsUndoManager;
          shape.Diagram.SkipsUndoManager = true;
          if (e.Control || e.Meta) {
            setPortSelected(shape, !IsPortSelected(shape));
            shape.Part.IsSelected = (shape.Part as Node).Ports.Any(IsPortSelected);
          } else if (e.Shift) {
            // alternative policy: select all ports between this shape and some other one??
            if (!IsPortSelected(shape)) setPortSelected(shape, true);
            shape.Part.IsSelected = true;
          } else {
            if (!IsPortSelected(shape)) {
              // deselect all sibling shapes
              foreach (var it in (shape.Part as Node).Ports) {
                if (it != shape) setPortSelected(it, false);
              }
              setPortSelected(shape, true);
            }
            shape.Part.IsSelected = true;
          }
          shape.Diagram.SkipsUndoManager = oldskips;
        }
      };

      Panel MakeItemTemplate(bool leftside) {
        return
          new Panel(PanelType.Auto) {
            Margin = new Margin(1, 0) // some space between ports
          }.Add(
            new Shape {
              Name = "SHAPE",
              Fill = _UnselectedBrush,
              Stroke = "gray",
              GeometryString = "F1 m 0,0 l 5,0 1,4 -1,4 -5,0 1,-4 -1,-4 z",
              Spot1 = new Spot(0, 0, 5, 1),  // keep the text inside the shape
              Spot2 = new Spot(1, 1, -5, 0),
              // some port-related properties
              ToSpot = Spot.Left,
              ToLinkable = leftside,
              FromSpot = Spot.Right,
              FromLinkable = !leftside,
              Cursor = "pointer"
            }.Bind(
              new Binding("PortId", "Name")
            ),
            new TextBlock { // allow the user to select items -- the background color indicates whether "selected"
              IsActionable = true,
              Click = onPortClick
            }.Bind(
              new Binding("Text", "Name")
            )
          );
      }

      // node template
      myDiagram.NodeTemplate =
        new Node(PanelType.Spot) {
          SelectionAdorned = false,
          LocationSpot = Spot.Center,
          LocationElementName = "BODY"
        }.Bind(
          new Binding("Location", "Loc", Point.Parse).MakeTwoWay(Point.Stringify)
        ).Add(
          new Panel(PanelType.Auto) {
            Name = "BODY"
          }.Add(
            new Shape {
              Figure = "RoundedRectangle",
              Stroke = "gray",
              StrokeWidth = 2,
              Fill = "transparent"
            }.Bind(
              new Binding("Stroke", "IsSelected", (b, _) => { return (b as bool? ?? false) ? _SelectedBrush : _UnselectedBrush; }).OfElement()
            ),
            new Panel(PanelType.Vertical) {
              Margin = 6
            }.Add(
              new TextBlock {
                Alignment = Spot.Left
              }.Bind(
                new Binding("Text", "Name")
              ),
              new Picture {
                Source = "https://nwoods.com/go/images/samples/60x90.png",
                Width = 30,
                Height = 45,
                Margin = new Margin(10, 10)
              }
            )
          ),
          new Panel(PanelType.Vertical) {
            Name = "LEFTPORTS",
            Alignment = new Spot(0, 0.5, 0, 7),
            ItemTemplate = MakeItemTemplate(true)
          }.Bind(
            new Binding("ItemList", "Inservices")
          ),
          new Panel(PanelType.Vertical) {
            Name = "RIGHTPORTS",
            Alignment = new Spot(1, 0.5, 0, 7),
            ItemTemplate = MakeItemTemplate(false)
          }.Bind(
            new Binding("ItemList", "Outservices")
          )
        );

      myDiagram.LinkTemplate =
        new Link {
          Routing = LinkRouting.Orthogonal,
          Corner = 10,
          ToShortLength = -3,
          RelinkableFrom = true,
          RelinkableTo = true,
          Reshapable = true,
          Resegmentable = true
        }.Add(
          new Shape {
            Stroke = "gray",
            StrokeWidth = 2.5
          }
        );

      // model data
      myDiagram.Model = new Model {
        LinkFromPortIdProperty = "FromPort",
        LinkToPortIdProperty = "ToPort",
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = 1, Name = "Server", Inservices = new List<FieldData> { new FieldData { Name = "s1" }, new FieldData { Name = "s2" } }, Outservices = new List<FieldData> { new FieldData { Name = "o1" } }, Loc = "0 0" },
          new NodeData { Key = 2, Name = "Other", Inservices = new List<FieldData> { new FieldData { Name = "s1" }, new FieldData { Name = "s2" } }, Loc = "200 60" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = 1, FromPort = "o1", To = 2, ToPort = "s2" }
        }
      };

      // load the model in blazor
      ShowModel();
    }

    internal static bool IsPortSelected(GraphObject item) {
      return item != null && (item as Shape).Fill != _UnselectedBrush;  // assume the port is a Shape
    }

    private void ShowModel() {
      modelJson1.JsonText = myDiagram.Model.ToJson();
    }
  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Name { get; set; }
    public List<FieldData> Inservices { get; set; }
    public List<FieldData> Outservices { get; set; }
    public string Loc { get; set; }

    public override object Clone() {
      return new NodeData {
        Name = Name,
        Inservices = Inservices.ConvertAll((d) => {
          return d.Clone();
        }),
        Outservices = Outservices.ConvertAll((d) => {
          return d.Clone();
        }),
        Loc = Loc
      };
    }
  }

  public class LinkData : Model.LinkData { }

  public class FieldData {
    public string Name { get; set; }

    public FieldData Clone() {
      return new FieldData {
        Name = Name
      };
    }
  }

  // override the standard CommandHandler DeleteSelection and CanDeleteSelection behavior
  // If there are any selected items, delete them instead of deleting selected nodes or links
  public class SelectablePortsCommandHandler : CommandHandler {
    private List<GraphObject> FindAllSelectedItems() {
      var items = new List<GraphObject>();
      var nit = Diagram.Nodes.GetEnumerator();
      while (nit.MoveNext()) {
        var node = nit.Current;
        foreach (var port in node.Ports) {
          if (SelectablePortsControl.IsPortSelected(port)) items.Add(port.Panel);
        }
      }
      return items;
    }

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
          var item = items[i] as Panel;
          var nodedata = item.Part.Data as NodeData;
          var itemdata = item.Data as FieldData;
          // find the item list that the item data is in; try "inservices" first
          var itemarray = nodedata.Inservices;
          var itemindex = itemarray.IndexOf(itemdata);
          if (itemindex < 0) {  // otherwise try "outservices"
            itemarray = nodedata.Outservices;
            itemindex = itemarray.IndexOf(itemdata);
          }
          if (itemindex >= 0) {
            Diagram.Model.RemoveListItem(itemarray, itemindex);
          }
        }
        Diagram.CommitTransaction("delete items");
      } else {  // otherwise just delete nodes and/or links, as usual
        base.DeleteSelection();
      }
    }
  }

}
