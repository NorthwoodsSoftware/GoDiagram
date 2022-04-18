/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.WinForms;
using Northwoods.Go.Layouts;
using Northwoods.Go.Tools;
using System.Linq;
using System.Collections.ObjectModel;

namespace WinFormsSampleControls.SeatingChart {
  [ToolboxItem(false)]
  public partial class SeatingChartControl : System.Windows.Forms.UserControl {
    private static Diagram myDiagram;
    private Diagram myGuests;

    private Dictionary<string, Part> sharedNodeTemplateMap;
    private UndoManager sharedUndoManager = new UndoManager();

    private static bool _DisableSelectionDeleted = false;
    public SeatingChartControl() {
      InitializeComponent();

      Setup();
      SetupGuests();

      goWebBrowser1.Html = @"
        <p>
      This sample demonstrates how a ""Person"" node can be dropped onto a ""Table"" node,
      causing the person to be assigned a position at the closest empty seat at that table.
      When a person is dropped into the background of the diagram, that person is no longer assigned a seat.
      People dragged between diagrams are automatically removed from the diagram they came from.
        </p>
        <p>
      ""Table"" nodes are defined by separate templates, to permit maximum customization of the shapes and
      sizes and positions of the tables and chairs.
        </p>
        <p>
      ""Person"" nodes in the <code>myGuests</code> diagram can also represent a group of people,
      for example a named person plus one whose name might not be known.
      When such a person is dropped onto a table, additional nodes are created in <code>myDiagram</code>.
      Those people are seated at the table if there is room.
        </p>
        <p>
      Tables can be moved or rotated.Moving or rotating a table automatically repositions the people seated at that table.
        </p>
        <p>
      The <a>UndoManager</a> is shared between the two Diagrams, so that one can undo/redo in either diagram
      and have it automatically handle drags between diagrams, as well as the usual changes within the diagram.
        </p>
";
    }

    private Panel Seat(int number, string align, string focus) {
      return Seat(number, Spot.Parse(align), Spot.Parse(focus));
    }

    // Create a seat element at a particular alignment relative to the table.
    private Panel Seat(int number, Spot? align = null, Spot? focus = null) {
      if (align == null) align = Spot.Right;
      if (focus == null) focus = align.Value.Opposite();

      return new Panel("Spot") {
        Name = number.ToString(),
        Alignment = align.Value,
        AlignmentFocus = focus.Value
      }
        .Add(
          new Shape {
            Figure = "Circle",
            Name = "SEATSHAPE", DesiredSize = new Size(40, 40), Fill = "burlywood", Stroke = "white", StrokeWidth = 2
          }
            .Bind("Fill"),
          new TextBlock {
            Text = number.ToString(),
            Font = new Font("Verdana", 10)
          }
            .Bind("Angle", "Angle", (n, _) => -(double)n)
        );
    }



    private void DefineNodeTemplates() {
      if (sharedNodeTemplateMap != null) return;  // already defined

      var tableStyle = new {
        Background = "transparent",
        LayerName = "Background", // behind all Persons
        LocationSpot = Spot.Center,
        LocationElementName = "TABLESHAPE",
        Rotatable = true,
        // what to do when a drag-over or a drag-drop occurs on a Node, representing a table
        MouseDragEnter = new Action<InputEvent, GraphObject, GraphObject>((e, node, prev) => {
          var dragCopy = node.Diagram.ToolManager.DraggingTool.CopiedParts?.Keys.ToList(); // could be copied from palette
          _HighlightSeats(node as Node, dragCopy ?? node.Diagram.Selection, true);
        }),
        MouseDragLeave = new Action<InputEvent, GraphObject, GraphObject>((e, node, next) => {
          var dragCopy = node.Diagram.ToolManager.DraggingTool.CopiedParts?.Keys.ToList();
          _HighlightSeats(node as Node, dragCopy ?? node.Diagram.Selection, false);
        }),
        MouseDrop = new Action<InputEvent, GraphObject>((e, node) => {
          _AssignPeopleToSeats(node as Node, node.Diagram.Selection, e.DocumentPoint);
        })
      };
      Binding[] tableBind() {
        return new[] {
          new Binding("Location", "Loc", Point.Parse, Point.Stringify),
          new Binding("Angle").MakeTwoWay()
        };
      }

      sharedNodeTemplateMap = new Dictionary<string, Part> {
        {
          "",
          new Node("Auto") { // default template, for people
              Background = "transparent", // in front of all tables
              LocationSpot = Spot.Center,
              // what to do when a drag-over or a drag-drop occurs on a Node representing a table
              MouseDragEnter = (e, node, prev) => {
                var dragCopy = node.Diagram.ToolManager.DraggingTool.CopiedParts?.Keys.ToList(); // could be copied from palette
                _HighlightSeats(node as Node, dragCopy ?? node.Diagram.Selection, true);
              },
              MouseDragLeave = (e, node, next) => {
                var dragCopy = node.Diagram.ToolManager.DraggingTool.CopiedParts?.Keys.ToList();
                _HighlightSeats(node as Node, dragCopy ?? node.Diagram.Selection, false);
              },
              MouseDrop = (e, node) => {
                _AssignPeopleToSeats(node as Node, node.Diagram.Selection, e.DocumentPoint);
              }
            }
            .Add(
              new Shape {
                  Figure = "Rectangle",
                  Fill = "blanchedalmond",
                  Stroke = null
                },
              new Panel(PanelLayoutViewbox.Instance) {
                  DesiredSize = new Size(50, 38)
                }
                .Add(
                  new TextBlock {
                      Margin = 2,
                      DesiredSize = new Size(55, double.NaN),
                      Font = new Font("Verdana", 8),
                      TextAlign = TextAlign.Center,
                      Stroke = "darkblue"
                    }
                    .Bind(
                      new Binding("Text", "", (data, _) => {
                        var _data = data as NodeData;
                        var s = "" + _data.Key;
                        if (_data.Plus != 0) s += " +" + _data.Plus;
                        return s;
                      })
                    )
                )
            )
        },
        // various kinds of tables:
        {
          "TableR8",  // rectangular with 8 seats
          new Node("Spot")
            .Set(tableStyle)
            .Bind(tableBind())
            .Add(
              new Panel("Spot")
                .Add(
                  new Shape {
                      Figure = "Rectangle",
                      Name = "TABLESHAPE",
                      DesiredSize = new Size(160, 80),
                      Fill = "burlywood",
                      Stroke = null
                    }
                    .Bind("DesiredSize", "Size", Northwoods.Go.Size.Parse, Northwoods.Go.Size.Stringify)
                    .Bind("Fill"),
                  new TextBlock {
                      Editable = true, Font = new Font("Verdana", 11, FontWeight.Bold)
                    }
                    .Bind(
                      new Binding("Text", "Name").MakeTwoWay(),
                      new Binding("Angle", "Angle", (n, _) => -(double)n)
                    )
                ),
              Seat(1, "0.2 0", "0.5 1"),
              Seat(2, "0.5 0", "0.5 1"),
              Seat(3, "0.8 0", "0.5 1"),
              Seat(4, "1 0.5", "0 0.5"),
              Seat(5, "0.8 1", "0.5 0"),
              Seat(6, "0.5 1", "0.5 0"),
              Seat(7, "0.2 1", "0.5 0"),
              Seat(8, "0 0.5", "1 0.5")
            )
        },
        {
          "TableR3",  // rectangular with 3 seats in a line
          new Node("Spot")
            .Set(tableStyle)
            .Bind(tableBind())
            .Add(
              new Panel("Spot")
                .Add(
                  new Shape {
                      Figure = "Rectangle",
                      Name = "TABLESHAPE",
                      DesiredSize = new Size(160, 60),
                      Fill = "burlywood",
                      Stroke = null
                    }
                    .Bind("DesiredSize", "Size", Northwoods.Go.Size.Parse, Northwoods.Go.Size.Stringify)
                    .Bind("Fill"),
                  new TextBlock {
                      Editable = true,
                      Font = new Font("Verdana", 11, FontWeight.Bold)
                    }
                    .Bind(
                      new Binding("Text", "Name").MakeTwoWay(),
                      new Binding("Angle", "Angle", (n, _) => -(double)n)
                    )
                ),
              Seat(1, "0.2 0", "0.5 1"),
              Seat(2, "0.5 0", "0.5 1"),
              Seat(3, "0.8 0", "0.5 1")
            )
        },
        {
          "TableC8",  // circular with 8 seats
          new Node("Spot")
            .Set(tableStyle)
            .Bind(tableBind())
            .Add(
              new Panel("Spot")
                .Add(
                  new Shape {
                      Figure = "Circle",
                      Name = "TABLESHAPE",
                      DesiredSize = new Size(120, 120),
                      Fill = "burlywood",
                      Stroke = null
                    }
                    .Bind("DesiredSize", "Size", Northwoods.Go.Size.Parse, Northwoods.Go.Size.Stringify)
                    .Bind("Fill"),
                  new TextBlock {
                      Editable = true,
                      Font = new Font("Verdana", 11, FontWeight.Bold)
                    }
                    .Bind(
                      new Binding("Text", "Name").MakeTwoWay(),
                      new Binding("Angle", "Angle", (n, _) => -(double)n)
                    )
                ),
              Seat(1, "0.50 0", "0.5 1"),
              Seat(2, "0.85 0.15", "0.15 0.85"),
              Seat(3, "1 0.5", "0 0.5"),
              Seat(4, "0.85 0.85", "0.15 0.15"),
              Seat(5, "0.50 1", "0.5 0"),
              Seat(6, "0.15 0.85", "0.85 0.15"),
              Seat(7, "0 0.5", "1 0.5"),
              Seat(8, "0.15 0.15", "0.85 0.85")
            )
        }
      };

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      myDiagram.AllowDragOut = true; // to myguests
      myDiagram.AllowClipboard = false;
      myDiagram.ToolManager.DraggingTool = new SpecialDraggingTool();
      myDiagram.ToolManager.RotatingTool = new HorizontalTextRotatingTool();
      // For this sample, automatically show the state of the diagram's model on the page
      myDiagram.ModelChanged += (s, e) => {
        if (e.IsTransactionFinished) textBox1.Text = myDiagram.Model.ToJson();

      };
      myDiagram.UndoManager.IsEnabled = true;

      DefineNodeTemplates();
      myDiagram.NodeTemplateMap = sharedNodeTemplateMap;

      // what to do when a drag-drop occurs in the Diagram's background
      myDiagram.MouseDrop = (e) => {
        // when the selection is dropped in the diagram's background
        // make sure the selected people no longer belong to any table
        foreach (var n in e.Diagram.Selection) {
          if (_IsPerson(n)) _UnassignSeat(n.Data as NodeData);
        }
      };

      // to simulate a "move" from the Palette, the source Node must be deleted
      myDiagram.ExternalElementsDropped += (obj, e) => {
        // if any Tables were dropped, don't delete from MyGuests
        if (!(e.Subject as HashSet<Part>).Any(_IsTable)) {
          myGuests.CommandHandler.DeleteSelection();
        }
      };

      // put deleted people back in the myGuests diagram (Palette)
      myDiagram.SelectionDeleting += (obj, e) => {
        // no-op if deleted by myguests' ExternalObjectsDropped listener
        if (_DisableSelectionDeleted) return;
        // e.Subject is the MyDiagram.Selection collection
        var sel = new List<Part>(e.Diagram.Selection);
        foreach (var part in sel) {
          if(_IsPerson(part)) {
            myGuests.Model.AddNodeData(myGuests.Model.CopyNodeData(part.Data as NodeData));



            // add line to allow new person to sit
          }
        }
      };

      // create some initial tables
      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "1", Category = "TableR3", Name = "Head 1", Guests = new Dictionary<string, string>(), Loc = "143.5 58" },
          new NodeData { Key = "2", Category = "TableR3", Name = "Head 2", Guests = new Dictionary<string, string>(), Loc = "324.5 58" },
          new NodeData { Key = "3", Category = "TableR8", Name = "3", Guests = new Dictionary<string, string>(), Loc = "121.5 203.5" },
          new NodeData { Key = "4", Category = "TableC8", Name = "4", Guests = new Dictionary<string, string>(), Loc = "364.5 223.5" }
        }, // this sample does not make use of any links
        UndoManager = sharedUndoManager
      };
    }

    private void SetupGuests() {
      myGuests = guestsControl.Diagram;
      // initialize the Palette
      myGuests.Layout = new GridLayout {
        Sorting = GridSorting.Ascending // sort by Node.Text value
      };
      myGuests.AllowDragOut = true; // to myDiagram
      myGuests.AllowMove = false;

      DefineNodeTemplates();
      myGuests.NodeTemplateMap = sharedNodeTemplateMap;

      // specify the contents of the Palette
      myGuests.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "Tyrion Lannister" },
          new NodeData { Key = "Daenerys Targaryen", Plus = 3 }, // dragons, of course
          new NodeData { Key = "Jon Snow" },
          new NodeData { Key = "Stannis Baratheon" },
          new NodeData { Key = "Arya Stark" },
          new NodeData { Key = "Jorah Mormont" },
          new NodeData { Key = "Sandor Clegane" },
          new NodeData { Key = "Joffrey Baratheon" },
          new NodeData { Key = "Brienne of Tarth" },
          new NodeData { Key = "Hodor" }
        },
        UndoManager = sharedUndoManager  // shared UndoManager !
      };

      // To simulate a "move" from the Diagram back to the Palette, the source Node must be deleted.
      myGuests.ExternalElementsDropped += (obj, e) => {
        // e.Subject is the MyGuests.Selection collection
        // if the user dragged a Table to the myGuests diagram, cancel the drag
        if ((e.Subject as HashSet<Part>).Any(_IsTable)) {
          myDiagram.CurrentTool.DoCancel();
          myGuests.CurrentTool.DoCancel();
          return;
        }
        foreach (var n in myDiagram.Selection) {
          if (_IsPerson(n)) _UnassignSeat(n.Data as NodeData);
        }
        _DisableSelectionDeleted = true;
        myDiagram.CommandHandler.DeleteSelection();
        _DisableSelectionDeleted = false;
        foreach (var n in myGuests.Selection) {
          if (_IsPerson(n)) _UnassignSeat(n.Data as NodeData);
        }
      };
    }

    private static bool _IsPerson(Part n) { return n != null && n.Category == ""; }

    private static bool _IsTable(Part n) { return n != null && n.Category != ""; }

    // Highlight the empty and occupied seats at a "Table" node
    private static void _HighlightSeats(Node node, IEnumerable<Part> coll, bool show) {
      if (_IsPerson(node)) { // refer to the person's table instead
        node = node.Diagram.FindNodeForKey((node.Data as NodeData).Table);
        if (node == null) return;
      }
      foreach (var n in coll) {
        // if dragging a Table, don't do any highlighting
        if (_IsTable(n as Node)) return;
      }

      var guests = (node.Data as NodeData).Guests;
      foreach (var seat in node.Elements) {
        if (seat.Name != null) {
          if (!double.TryParse(seat.Name, out var num)) continue;
          var _seatshape = (seat as Panel).FindElement("SEATSHAPE");
          if (_seatshape == null || !(_seatshape is Shape seatshape)) continue;
          if (show) {
            if (guests.TryGetValue(seat.Name, out var val) && val != null) {
              seatshape.Stroke = "red";
            } else {
              seatshape.Stroke = "green";
            }
          } else {
            seatshape.Stroke = "white";
          }
        }
      }
    }

    // Given a table node, assign seats for all of the people in the given collection of Nodes;
    // the optional Point argument indicates where the collection of people may have been dropped.
    private static void _AssignPeopleToSeats(Node node, IEnumerable<Part> coll, Point pt) {
      if (_IsPerson(node)) { // refer to the person's table instead
        node = node.Diagram.FindNodeForKey((node.Data as NodeData).Table);
        if (node == null) return;
      }
      if (coll.Any(_IsTable)) {
        // if dragging a Table, don't allow it to be dropped onto another table
        myDiagram.CurrentTool.DoCancel();
        return;
      }

      // ok -- all Nodes are people, call assign seat on each person data
      foreach (var n in coll) { _AssignSeat(node, n.Data as NodeData, pt); }
      _PositionPeopleAtSeats(node);
    }

    // Given a table node, assign one guest data to a seat at that table.
    // Also handles cases where the guest represents multiple people, because guest.Plus > 0.
    // this tries to assign the unoccupied seat that is closest to the given point in document coordinates.
    private static void _AssignSeat(Node node, NodeData guest, Point pt) {
      if (_IsPerson(node)) { // refer to the person's table instead
        node = node.Diagram.FindNodeForKey((node.Data as NodeData).Table);
        if (node == null) return;
      }

      // in case the guest used to be assigned to a different seat, perhaps at a different table
      _UnassignSeat(guest);

      var model = node.Diagram.Model as Model;
      var guests = (node.Data as NodeData).Guests;
      // iterate over all seats in the Node to find one that is not occupied
      var closestseatname = _FindClosestUnoccupiedSeat(node, pt);
      if (closestseatname != null && int.TryParse(closestseatname, out var m)) {
        model.Set(guests, closestseatname, guest.Key);
        model.Set(guest, "Table", (node.Data as NodeData).Key);
        model.Set(guest, "Seat", m);
      }

      var plus = guest.Plus;
      if (plus != 0) { // represents several people
        // forget the "plus" info, since next we create N copies of the node/data
        guest.Plus = 0;
        model.UpdateTargetBindings(guest);
        for (var i = 0; i < plus; i++) {
          var copy = model.CopyNodeData(guest);
          // don't copy the seat assignment of the first person
          copy.Table = null;
          copy.Seat = 0;
          model.AddNodeData(copy);
          _AssignSeat(node, copy, pt);
        }
      }
    }

    // Declare that the guest represented by the data is no longer assigned to a seat at a table.
    // If the guest had been at a table, the guest is removed from the table's list of guests.
    private static void _UnassignSeat(NodeData guest) {
      var model = myDiagram.Model;
      // remove from any table that the guest is assigned to
      if (guest.Table != null) {
        var table = model.FindNodeDataForKey(guest.Table);
        if (table != null) {
          var guests = (table as NodeData).Guests;
          if (guests != null) model.Set(guests, guest.Seat.ToString(), null);
        }
      }
      model.Set(guest, "Table", null);
      model.Set(guest, "Seat", 0);
    }

    // Find the name of the unoccupied seat that is closest to the given Point.
    // This returns null if no seat is available at this table.
    private static string _FindClosestUnoccupiedSeat(Node node, Point pt) {
      if (_IsPerson(node)) { // refer to the person's table instead
        node = node.Diagram.FindNodeForKey((node.Data as NodeData).Table);
        if (node == null) return null;
      }
      var guests = (node.Data as NodeData).Guests;
      string closestseatname = null;
      var closestseatdist = double.PositiveInfinity;
      // iterate over all seats in the node to find one that is not occupied
      foreach (var seat in node.Elements) {
        if (seat.Name != null) {
          if (!double.TryParse(seat.Name, out var num) || double.IsNaN(num)) continue;  // not really a seat
          if (guests.TryGetValue(seat.Name, out var val) && val != null) continue; // already assigned
          var seatloc = seat.GetDocumentPoint(Spot.Center);
          var seatdist = seatloc.DistanceSquared(pt);
          if (seatdist < closestseatdist) {
            closestseatdist = seatdist;
            closestseatname = seat.Name;
          }
        }
      }
      return closestseatname;
    }

    private static void _PositionPersonAtSeat(NodeData guest) {
      if (guest == null || guest.Table == null || guest.Seat == 0) return;
      var diagram = myDiagram;
      var table = diagram.FindPartForKey(guest.Table.ToString());
      var person = diagram.FindPartForData(guest);
      if (table != null && person != null) {
        var seat = table.FindElement(guest.Seat.ToString());
        var loc = seat.GetDocumentPoint(Spot.Center);
        person.Location = loc;
      }
    }

    public static void _PositionPeopleAtSeats(Node node) {
      if (_IsPerson(node)) { // refer to the person's table instead
        node = node.Diagram.FindNodeForKey((node.Data as NodeData).Table);
        if (node == null) return;
      }
      var guests = (node.Data as NodeData).Guests;
      var model = node.Diagram.Model;
      foreach (var seatname in guests.Keys) {
        var guestkey = guests[seatname];
        var guestdata = model.FindNodeDataForKey(guestkey);
        _PositionPersonAtSeat(guestdata as NodeData);
      }
    }
  }

  // Automatically drag people Nodes along with the table Node at which they are seated.
  public class SpecialDraggingTool : DraggingTool {
    public SpecialDraggingTool() : base() {
      IsCopyEnabled = false; // don't want to copy people except between Diagrams
    }

    public override IDictionary<Part, DraggingInfo> ComputeEffectiveCollection(IEnumerable<Part> parts, DraggingOptions options) {
      var map = base.ComputeEffectiveCollection(parts, options);
      // for each node representing a Table, also drag all of the people seated at that table
      foreach (var table in parts) {
        if (table.Category == "") continue; // ignore persons
        // this is a table Node, find all people Nodes using the same table key
        foreach (var n in table.Diagram.Nodes) {
          if (n.Category == "" && (n.Data as NodeData).Table.ToString() == (table.Data as NodeData).Key) {
            if (!map.ContainsKey(n)) map.Add(n, new DraggingInfo(new Point(n.Location.X, n.Location.Y)));
          }
        }
      }
      return map;
    }
  }
  // end SpecialDraggingTool

  // Automatically move seated people as a table is rotated, to keep them in their seats.
  // Note that because people are separate Nodes, rotating a table Node means the people Nodes
  // are not rotated, so their names (TextBlocks) remain horizontal.
  public class HorizontalTextRotatingTool : RotatingTool {
    public override void Rotate(double newangle) {
      base.Rotate(newangle);
      var node = AdornedElement.Part;
      SeatingChartControl._PositionPeopleAtSeats(node as Node);
    }
  }
  // end HorizontalTextRotatingTool

  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }

  public class NodeData : Model.NodeData {
    public string Name { get; set; }
    public Dictionary<string, string> Guests { get; set; } = null;
    public string Loc { get; set; }
    public string Size { get; set; }
    public string Table { get; set; }
    public int Seat { get; set; }
    public int Plus { get; set; }
    public double Angle { get; set; }
    public string Fill { get; set; }
  }

  public class LinkData : Model.LinkData { }

}
