using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.WinForms;
using Northwoods.Go.Tools;
using Northwoods.Go.Layouts;
using System.Linq;

namespace WinFormsSampleControls.Pipes {
  [ToolboxItem(false)]
  public partial class PipesControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    private Palette myPalette;

    private Part sharedNodeTemplate;
    public PipesControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;
      paletteControl1.AfterRender = SetupPalette;

      saveLoadModel1.SaveClick += (e, obj) => SaveModel();
      saveLoadModel1.LoadClick += (e, obj) => LoadModel();
      saveLoadModel1.ModelJson = @"{
  ""LinkFromPortIdProperty"": ""Fid"",
  ""LinkToPortIdProperty"": ""Tid"",
  ""NodeDataSource"": [
    { ""Key"": 1, ""Category"": ""Comment"", ""Text"": ""Use Shift to disconnect a shape"", ""Loc"": ""0 -13"" },
    { ""Key"": 2, ""Category"": ""Comment"", ""Text"": ""The Context Menu has more commands"", ""Loc"": ""0 20"" },
    { ""Key"": 3, ""Category"": ""Comment"", ""Text"": ""Gray Xs are unconnected ports"", ""Loc"": ""0 -47"" },
    { ""Key"": 4, ""Category"": ""Comment"", ""Text"": ""Dragged shapes snap to unconnected ports"", ""Loc"": ""0 -80"" },
    { ""Key"": 11, ""Geo"": ""F1 M0 40 L0 30 Q0 0 30 0 L40 0 40 20 30 20 Q20 20 20 30 L20 40z"", ""Ports"": [ { ""ID"": ""U0"", ""Spot"": ""1 0.25 -0.5 0.25"" }, { ""ID"": ""U2"", ""Spot"": ""0.25 1 0.25 -0.5""} ], ""Loc"": ""-187.33333333333331 -69.33333333333331"", ""Angle"": 0 },
    { ""Key"": 12, ""Angle"": 90, ""Geo"": ""F1 M0 40 L0 30 Q0 0 30 0 L40 0 40 20 30 20 Q20 20 20 30 L20 40z"", ""Ports"": [ { ""ID"": ""U0"", ""Spot"": ""1 0.25 -0.5 0.25"" }, { ""ID"": ""U2"", ""Spot"": ""0.25 1 0.25 -0.5""} ], ""Loc"": ""-147.33333333333331 -69.33333333333331"" },
    { ""Key"": 21, ""Geo"": ""F1 M0 0 L60 0 60 20 50 20 Q40 20 40 30 L40 40 20 40 20 30 Q20 20 10 20 L0 20z"", ""Ports"": [ { ""ID"": ""U0"", ""Spot"": ""1 0.25 -0.5 0.25"" }, { ""ID"": ""U4"", ""Spot"": ""0 0.25 0.5 0.25"" }, { ""ID"": ""U2"", ""Spot"": ""0.5 1 0 -0.5""} ], ""Loc"": ""-137.33333333333331 -9.333333333333314"", ""Angle"": 0 },
    { ""Key"": 5, ""Geo"": ""F1 M0 0 L20 0 20 60 0 60z"", ""Ports"": [ { ""ID"": ""U6"", ""Spot"": ""0.5 0 0 0.5"" }, { ""ID"": ""U2"", ""Spot"": ""0.5 1 0 -0.5""} ], ""Loc"": ""-197.33333333333331 -19.333333333333314"", ""Angle"": 0 },
    { ""Key"": 13, ""Angle"": 180, ""Geo"": ""F1 M0 40 L0 30 Q0 0 30 0 L40 0 40 20 30 20 Q20 20 20 30 L20 40z"", ""Ports"": [ { ""ID"": ""U0"", ""Spot"": ""1 0.25 -0.5 0.25"" }, { ""ID"": ""U2"", ""Spot"": ""0.25 1 0.25 -0.5""} ], ""Loc"": ""-147.33333333333331 30.666666666666685"" },
    { ""Key"": 14, ""Angle"": 270, ""Geo"": ""F1 M0 40 L0 30 Q0 0 30 0 L40 0 40 20 30 20 Q20 20 20 30 L20 40z"", ""Ports"": [ { ""ID"": ""U0"", ""Spot"": ""1 0.25 -0.5 0.25"" }, { ""ID"": ""U2"", ""Spot"": ""0.25 1 0.25 -0.5""} ], ""Loc"": ""-187.33333333333331 30.666666666666685"" },
    { ""Key"": -7, ""Geo"": ""F1 M0 40 L0 30 Q0 0 30 0 L40 0 40 20 30 20 Q20 20 20 30 L20 40z"", ""Ports"": [ { ""ID"": ""U0"", ""Spot"": ""1 0.25 -0.5 0.25"" }, { ""ID"": ""U2"", ""Spot"": ""0.25 1 0.25 -0.5""} ], ""Loc"": ""-76.66666666666663 -8.666666666666657"", ""Angle"": 0},
    { ""Key"": -8, ""Angle"": 90, ""Geo"": ""F1 M0 40 L0 30 Q0 0 30 0 L40 0 40 20 30 20 Q20 20 20 30 L20 40z"", ""Ports"": [ { ""ID"": ""U0"", ""Spot"": ""1 0.25 -0.5 0.25"" }, { ""ID"": ""U2"", ""Spot"": ""0.25 1 0.25 -0.5""} ], ""Loc"": ""-36.66666666666663 -8.666666666666657"" },
    { ""Key"": -9, ""Angle"": 180, ""Geo"": ""F1 M0 40 L0 30 Q0 0 30 0 L40 0 40 20 30 20 Q20 20 20 30 L20 40z"", ""Ports"": [ { ""ID"": ""U0"", ""Spot"": ""1 0.25 -0.5 0.25"" }, { ""ID"": ""U2"", ""Spot"": ""0.25 1 0.25 -0.5""} ], ""Loc"": ""-36.66666666666663 31.333333333333343"" },
    { ""Key"": -10, ""Angle"": 270, ""Geo"": ""F1 M0 40 L0 30 Q0 0 30 0 L40 0 40 20 30 20 Q20 20 20 30 L20 40z"", ""Ports"": [ { ""ID"": ""U0"", ""Spot"": ""1 0.25 -0.5 0.25"" }, { ""ID"": ""U2"", ""Spot"": ""0.25 1 0.25 -0.5""} ], ""Loc"": ""-76.66666666666663 31.333333333333343""}
  ],
  ""LinkDataSource"": [
    { ""Key"": 101, ""From"": 12, ""To"": 11, ""Fid"": ""U2"", ""Tid"": ""U0"" },
    { ""Key"": 102, ""From"": 5, ""To"": 11, ""Fid"": ""U6"", ""Tid"": ""U2"" },
    { ""Key"": 103, ""From"": 13, ""To"": 21, ""Fid"": ""U2"", ""Tid"": ""U2"" },
    { ""Key"": 104, ""From"": 14, ""To"": 5, ""Fid"": ""U0"", ""Tid"": ""U2"" },
    { ""Key"": 105, ""From"": 13, ""To"": 14, ""Fid"": ""U0"", ""Tid"": ""U2"" },
    { ""Key"": 106, ""From"": -8, ""To"": -7, ""Fid"": ""U2"", ""Tid"": ""U0"" },
    { ""Key"": 107, ""From"": -9, ""To"": -8, ""Fid"": ""U2"", ""Tid"": ""U0"" },
    { ""Key"": 108, ""From"": -10, ""To"": -7, ""Fid"": ""U0"", ""Tid"": ""U2"" },
    { ""Key"": 109, ""From"": -10, ""To"": -9, ""Fid"": ""U2"", ""Tid"": ""U0"" }
  ]
}";

      goWebBrowser1.Html = @"
        <p>
          Nodes in this sample use <a>Shape.GeometryString</a> to determine their shape.
          You can see more custom geometry examples and read about GeometryString
          on the <a href=""intro/geometry.html"">Geometry Path Strings Introduction page.</a>
        </p>
        <p>
          As a part's unconnected port (shown by an X) comes close to a stationary port
          with which it is compatible, the dragged selection snaps so that those ports coincide.
          A custom <a>DraggingTool</a>, called <b>SnappingTool</b>, is used to check compatibility.
        </p>
        <p>
          Dragging automatically drags all connected parts.
          Hold down the Shift key before dragging in order to detach a part from the parts it is connected with.
          These functionalities are also controlled by the custom SnappingTool.
        </p>
        <p>
          Use the <a>GraphObject.ContextMenu</a> to rotate, detach, or delete a node.
          If it is connected with other parts, the whole collection rotates.
        </p>
      ";

    }

    // To simplify this code we define a function for creating a context menu button:
    Panel MakeButton(string text, Action<InputEvent, GraphObject> action, Func<GraphObject, bool> visiblePredicate = null) {
      object converter(object obj) {
        var elt = obj as GraphObject;
        return elt.Diagram != null && visiblePredicate(elt);
      }

      return Builder.Make<Panel>("ContextMenuButton")
        .Add(new TextBlock(text))
        .Set(new { Click = action })
        .Bind(visiblePredicate != null ? new Binding("Visible", "", converter).OfElement() : null);
    }

    private void DefineNodeTemplate() {
      if (sharedNodeTemplate != null) return;  // already defined

      // Define the generic "pipe" Node.
      // The Shape gets its geometry from a geometry path string in the bound data.
      // This node also gets all of its ports from an array of port data in the bound data.
      sharedNodeTemplate =
        new Node("Spot") {
            LocationElementName = "SHAPE",
            LocationSpot = Spot.Center,
            SelectionAdorned = false,  // use a Binding on the shape.stroke to show selection
            ItemTemplate =
              // each port is an "X" shape whose alignment spot and port ID are given by the item data
              new Panel()
                .Add(
                  new Shape {
                      Figure = "XLine",
                      Width = 6,
                      Height = 6,
                      Background = "transparent",
                      Fill = null,
                      Stroke = "gray"
                    }
                    .Bind(
                      new Binding("Figure", "ID", portFigure), // portFigure defined below
                      new Binding("Angle", "Angle")
                    )
                )
                .Bind(
                  new Binding("PortId", "ID"),
                  new Binding("Alignment", "Spot", Spot.Parse)
                ),
            // hide a port when it is connected
            LinkConnected = (node, link, port, connected) => {
              if (connected) {
                if (link.Category == "") port.Visible = false;
              } else {
                if (link.Category == "") port.Visible = true;
              }
            }
          }
          .Bind(
            // this creates the variable number of ports for this Spot Panel based on the data
            new Binding("ItemList", "Ports"),
            // remember the location of this node
            new Binding("Location", "Loc", Point.Parse, Point.Stringify),
            // remember the angle of this Node
            new Binding("Angle", "Angle").MakeTwoWay(),
            // move a selected part into the foreground layer, so it isn't obscured by any non-selected parts
            new Binding("LayerName", "IsSelected", (s, _) => (bool)s ? "Foreground" : "").OfElement()
          )
          .Add(
            new Shape {
                Name = "SHAPE",
                // the following are default values;
                // actual values may come from the node data object via data binding
                GeometryString = "F1 M0 0 L20 0 20 20 0 20 z",
                Fill = "rgba(128, 128, 128, 0.5)"
              }
              .Bind(
                // this determines the actual shape of the Shape
                new Binding("GeometryString", "Geo"),
                // selection causes the stroke to be blue instead of black
                new Binding("Stroke", "IsSelected", (s, _) => (bool)s ? "dodgerblue" : "black").OfElement()
              )
          );

      // Show different kinds of port fittings by using different shapes in this Binding converter
      string portFigure(object pid, object _) {
        var portId = (string)pid;
        if (portId == null || portId == "") return "XLine";
        if (portId[0] == 'F') return "CircleLine";
        if (portId[0] == 'M') return "PlusLine";
        return "XLine"; // including when the first character is 'U'
      }

      sharedNodeTemplate.ContextMenu =
        Builder.Make<Adornment>("ContextMenu")
          .Add(
            MakeButton("Rotate +45",
              (e, obj) => rotate((obj.Part as Adornment).AdornedPart, 45)),
            MakeButton("Rotate -45",
              (e, obj) => rotate((obj.Part as Adornment).AdornedPart, -45)),
            MakeButton("Rotate 180",
              (e, obj) => rotate((obj.Part as Adornment).AdornedPart, 180)),
            MakeButton("Detach",
              (e, obj) => detachSelection(obj)),
            MakeButton("Cut",
              (e, obj) => e.Diagram.CommandHandler.CutSelection(),
              (o) => o.Diagram.CommandHandler.CanCutSelection()),
            MakeButton("Copy",
              (e, obj) => e.Diagram.CommandHandler.CopySelection(),
              (o) => o.Diagram.CommandHandler.CanCopySelection()),
            MakeButton("Paste",
              (e, obj) => e.Diagram.CommandHandler.PasteSelection(e.Diagram.LastInput.DocumentPoint),
              (o) => o.Diagram.CommandHandler.CanPasteSelection()),
            MakeButton("Delete",
              (e, obj) => e.Diagram.CommandHandler.DeleteSelection(),
              (o) => o.Diagram.CommandHandler.CanDeleteSelection()),
            MakeButton("Undo",
              (e, obj) => e.Diagram.CommandHandler.Undo(),
              (o) => o.Diagram.CommandHandler.CanUndo()),
            MakeButton("Redo",
              (e, obj) => e.Diagram.CommandHandler.Redo(),
              (o) => o.Diagram.CommandHandler.CanRedo())
        );

      // Change the angle of the parts connected with the given node
      void rotate(Part node, double angle) {
        var tool = node.Diagram.ToolManager.DraggingTool;
        node.Diagram.StartTransaction("Rotate " + angle);
        var sel = new HashSet<Part>();
        sel.Add(node);
        var coll = tool.ComputeEffectiveCollection(sel, new DraggingOptions()).Keys;
        var bounds = node.Diagram.ComputePartsBounds(coll);
        var center = bounds.Center;
        foreach (var n in coll) {
          n.Angle += angle;
          n.Location = new Point(n.Location.X, n.Location.Y).Subtract(center).Rotate(angle).Add(center);
        }
        node.Diagram.CommitTransaction("Rotate " + angle);
      }

      void detachSelection(GraphObject obj) {
        obj.Diagram.StartTransaction("Detach");
        var coll = new HashSet<Part>();
        foreach (var part in obj.Diagram.Selection) {
          if (!(part is Node node)) continue;
          foreach (var link in node.LinksConnected) {
            if (link.Category != "") continue; // ignore comments
            // ignore links to other selected Nodes
            if (link.GetOtherNode(node).IsSelected) continue;
            // disconnect this link
            coll.Add(link);
          }
        }
        obj.Diagram.RemoveParts(coll, false);
        obj.Diagram.CommitTransaction("Detach");
      }
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      myDiagram.InitialScale = 1.5;
      myDiagram.DefaultScale = 1.5;
      myDiagram.AllowLink = false; // no user-drawn links
      // use a custom DraggingTool instead of the standard one, defined below
      myDiagram.ToolManager.DraggingTool = new SnappingTool();
      // provide a context menu for the background of the Diagram, when not over any Part
      myDiagram.ContextMenu =
        Builder.Make<Adornment>("ContextMenu")
          .Add(
            MakeButton("Paste",
              (e, obj) => { e.Diagram.CommandHandler.PasteSelection(e.Diagram.ToolManager.ContextMenuTool.MouseDownPoint); },
              (o) => { return o.Diagram.CommandHandler.CanPasteSelection(o.Diagram.ToolManager.ContextMenuTool.MouseDownPoint); }),
            MakeButton("Undo",
              (e, obj) => { e.Diagram.CommandHandler.Undo(); },
              (o) => { return o.Diagram.CommandHandler.CanUndo(); }),
            MakeButton("Redo",
              (e, obj) => { e.Diagram.CommandHandler.Redo(); },
              (o) => { return o.Diagram.CommandHandler.CanRedo(); })
          );
      myDiagram.UndoManager.IsEnabled = true;

      myDiagram.NodeTemplateMap["Comment"] =
        new Node()
          .Add(
            new TextBlock {
              Stroke = "brown",
              Font = "Segoe UI, 12px"
            }
              .Bind("Text")
          )
          .Bind("Location", "Loc", Point.Parse, Point.Stringify);

      DefineNodeTemplate();
      myDiagram.NodeTemplate = sharedNodeTemplate;

      // no visual representation of any link data
      myDiagram.LinkTemplate = new Link { Visible = false };

      // support optional links from comment nodes to pipe nodes
      myDiagram.LinkTemplateMap["Comment"] =
        new Link {
          Curve = LinkCurve.Bezier
        }
          .Add(
            new Shape { Stroke = "brown", StrokeWidth = 2 },
            new Shape { ToArrow = "OpenTriangle", Stroke = "brown" }
          );

      LoadModel();
    }

    private void SetupPalette() {
      int keyCompare(Part a, Part b) {
        var at = (a.Data as NodeData).Key;
        var bt = (b.Data as NodeData).Key;
        if (at < bt) return -1;
        if (at > bt) return 1;
        return 0;
      }

      myPalette = paletteControl1.Diagram as Palette;
      myPalette.InitialScale = 1.2;
      myPalette.ContentAlignment = Spot.Center;
      DefineNodeTemplate();
      myPalette.NodeTemplate = sharedNodeTemplate; // shared with the main Diagram
      myPalette.ToolManager.ContextMenuTool.IsEnabled = false;
      myPalette.Layout = new GridLayout {
        CellSize = new Size(1, 1),
        Spacing = new Size(5, 5),
        WrappingColumn = 12,
        Comparer = keyCompare
      };

      // initialize the Palette with a few "pipe" nodes
      myPalette.Model = new Model {
        LinkFromPortIdProperty = "Fid",
        LinkToPortIdProperty = "Tid",
        NodeDataSource = new List<NodeData> {
          // Several different kinds of pipe objects, some already rotated for convenience.
          // Each "glue point" is implemented by a port.
          // The port's identifier's first letter must be the type of connector or "fitting".
          // The port's identifier's second letter must be indicate the direction in which a
          // connection may be made: 0-7, indicating multiples of 45-degree angles starting at zero.
          // If you want more than one port of a particular type in the same direction,
          // you will need to add a suffix to the port identifier.
          // The Spot determines the approximate location of the port on the shape.
          // The exact position is offset in order to account for the thickness of the stroke.
          // Each should be shifted towards the center of the shape by the fraction of its
          // distance from the center times the stroke thickness.
          // The following offsets assume the strokeWidth == 1.
          new NodeData {
            Key = 1,
            Geo = "F1 M0 0 L20 0 20 20 0 20z",
            Ports = new List<PortData> {
              new PortData { ID ="U6", Spot = "0.5 0 0 0.5" },
              new PortData { ID ="U2", Spot = "0.5 1 0 -0.5" }
            }
          },
          new NodeData {
            Key = 3,
            Angle = 90,
            Geo = "F1 M0 0 L20 0 20 20 0 20z",
            Ports = new List<PortData> {
              new PortData { ID = "U6", Spot = "0.5 0 0 0.5" },
              new PortData { ID = "U2", Spot = "0.5 1 0 -0.5" }
            }
          },
          new NodeData {
            Key = 5,
            Geo = "F1 M0 0 L20 0 20 60 0 60z",
            Ports = new List<PortData> {
              new PortData { ID = "U6", Spot = "0.5 0 0 0.5" },
              new PortData { ID = "U2", Spot = "0.5 1 0 -0.5" }
            }
          },
          new NodeData {
            Key = 7, Angle = 90,
            Geo = "F1 M0 0 L20 0 20 60 0 60z",
            Ports = new List<PortData> {
              new PortData { ID = "U6", Spot = "0.5 0 0 0.5" },
              new PortData { ID = "U2", Spot = "0.5 1 0 -0.5" }
            }
          },
          new NodeData {
              Key = 11,
              Geo = "F1 M0 40 L0 30 Q0 0 30 0 L40 0 40 20 30 20 Q20 20 20 30 L20 40z",
              Ports = new List<PortData> {
                new PortData { ID = "U0", Spot = "1 0.25 -0.5 0.25" },
                new PortData { ID = "U2", Spot = "0.25 1 0.25 -0.5" }
              }
            },
            new NodeData {
              Key = 12, Angle = 90,
              Geo = "F1 M0 40 L0 30 Q0 0 30 0 L40 0 40 20 30 20 Q20 20 20 30 L20 40z",
              Ports = new List<PortData> {
                new PortData { ID = "U0", Spot = "1 0.25 -0.5 0.25" },
                new PortData { ID = "U2", Spot = "0.25 1 0.25 -0.5" }
              }
            },
            new NodeData {
              Key = 13, Angle = 180,
              Geo = "F1 M0 40 L0 30 Q0 0 30 0 L40 0 40 20 30 20 Q20 20 20 30 L20 40z",
              Ports = new List<PortData> {
                new PortData { ID = "U0", Spot = "1 0.25 -0.5 0.25" },
                new PortData { ID = "U2", Spot = "0.25 1 0.25 -0.5" }
              }
            },
            new NodeData {
              Key = 14, Angle = 270,
              Geo = "F1 M0 40 L0 30 Q0 0 30 0 L40 0 40 20 30 20 Q20 20 20 30 L20 40z",
              Ports = new List<PortData> {
                new PortData { ID = "U0", Spot = "1 0.25 -0.5 0.25" },
                new PortData { ID = "U2", Spot = "0.25 1 0.25 -0.5" }
              }
            },
            new NodeData {
              Key = 21,
              Geo = "F1 M0 0 L60 0 60 20 50 20 Q40 20 40 30 L40 40 20 40 20 30 Q20 20 10 20 L0 20z",
              Ports = new List<PortData> {
                new PortData { ID = "U0", Spot = "1 0.25 -0.5 0.25" },
                new PortData { ID = "U4", Spot = "0 0.25 0.5 0.25" },
                new PortData { ID = "U2", Spot = "0.5 1 0 -0.5" }
              }
            },
            new NodeData {
              Key = 22, Angle = 90,
              Geo = "F1 M0 0 L60 0 60 20 50 20 Q40 20 40 30 L40 40 20 40 20 30 Q20 20 10 20 L0 20z",
              Ports = new List<PortData> {
                new PortData { ID = "U0", Spot = "1 0.25 -0.5 0.25" },
                new PortData { ID = "U4", Spot = "0 0.25 0.5 0.25" },
                new PortData { ID = "U2", Spot = "0.5 1 0 -0.5" }
              }
            },
            new NodeData {
              Key = 23, Angle = 180,
              Geo = "F1 M0 0 L60 0 60 20 50 20 Q40 20 40 30 L40 40 20 40 20 30 Q20 20 10 20 L0 20z",
              Ports = new List<PortData> {
                new PortData { ID = "U0", Spot = "1 0.25 -0.5 0.25" },
                new PortData { ID = "U4", Spot = "0 0.25 0.5 0.25" },
                new PortData { ID = "U2", Spot = "0.5 1 0 -0.5" }
              }
            },
            new NodeData {
              Key = 24, Angle = 270,
              Geo = "F1 M0 0 L60 0 60 20 50 20 Q40 20 40 30 L40 40 20 40 20 30 Q20 20 10 20 L0 20z",
              Ports = new List<PortData> {
                new PortData { ID = "U0", Spot = "1 0.25 -0.5 0.25" },
                new PortData { ID = "U4", Spot = "0 0.25 0.5 0.25" },
                new PortData { ID = "U2", Spot = "0.5 1 0 -0.5" }
              }
            },
            new NodeData {
              Key = 31,
              Geo = "F1 M0 0 L20 0 20 10 Q20 14.142 22.929 17.071 L30 24.142 15.858 38.284 8.787 31.213 Q0 22.426 0 10z",
              Ports = new List<PortData> {
                new PortData { ID = "U6", Spot = "0 0 10.5 0.5" },
                new PortData { ID = "U1", Spot = "1 1 -7.571 -7.571", Angle = 45 }
              }
            },
            new NodeData {
              Key = 32, Angle = 90,
              Geo = "F1 M0 0 L20 0 20 10 Q20 14.142 22.929 17.071 L30 24.142 15.858 38.284 8.787 31.213 Q0 22.426 0 10z",
              Ports = new List<PortData> {
                new PortData { ID = "U6", Spot = "0 0 10.5 0.5" },
                new PortData { ID = "U1", Spot = "1 1 -7.571 -7.571", Angle = 45 }
              }
            },
            new NodeData {
              Key = 33, Angle = 180,
              Geo = "F1 M0 0 L20 0 20 10 Q20 14.142 22.929 17.071 L30 24.142 15.858 38.284 8.787 31.213 Q0 22.426 0 10z",
              Ports = new List<PortData> {
                new PortData { ID = "U6", Spot = "0 0 10.5 0.5" },
                new PortData { ID = "U1", Spot = "1 1 -7.571 -7.571", Angle = 45 }
              }
            },
            new NodeData {
              Key = 34, Angle = 270,
              Geo = "F1 M0 0 L20 0 20 10 Q20 14.142 22.929 17.071 L30 24.142 15.858 38.284 8.787 31.213 Q0 22.426 0 10z",
              Ports = new List<PortData> {
                new PortData { ID = "U6", Spot = "0 0 10.5 0.5" },
                new PortData { ID = "U1", Spot = "1 1 -7.571 -7.571", Angle = 45 }
              }
            },
            new NodeData {
              Key = 41,
              Geo = "F1 M14.142 0 L28.284 14.142 14.142 28.284 0 14.142z",
              Ports = new List<PortData> {
                new PortData { ID = "U1", Spot = "1 1 -7.321 -7.321" },
                new PortData { ID = "U3", Spot = "0 1 7.321 -7.321" },
                new PortData { ID = "U5", Spot = "0 0 7.321 7.321" },
                new PortData { ID = "U7", Spot = "1 0 -7.321 7.321" }
              }
            }
          }
      };
    }

    private void SaveModel() {
      if (myDiagram == null) return;
      saveLoadModel1.ModelJson = myDiagram.Model.ToJson();
    }

    private void LoadModel() {
      if (myDiagram == null) return;
      myDiagram.Model = Model.FromJson<Model>(saveLoadModel1.ModelJson);
    }
  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }

  public class NodeData : Model.NodeData {
    public string Loc { get; set; }
    public List<PortData> Ports { get; set; }
    public string Geo { get; set; }
    public double Angle { get; set; }
    // copy port List and contents
    public override object Clone() {
      var data = base.Clone() as NodeData;

      var newPorts = new List<PortData>();
      foreach (PortData item in Ports) {
        newPorts.Add(new PortData {
          ID = item.ID,
          Spot = item.Spot
        });
      }
      data.Ports = newPorts;

      return data;
    }
  }

  public class LinkData : Model.LinkData {
    public string Fid { get; set; }
    public string Tid { get; set; }
  }

  public class PortData {
    public string ID { get; set; }
    public string Spot { get; set; }
    public double Angle { get; set; }
  }

  // Define a custom DraggingTool
  public class SnappingTool : DraggingTool {
    private Point? _SnapOffset { get; set; }

    // This predicate checks to see if the ports can snap together.
    // The first letter of the port id should be "U", "F", or "M" to indicate which kinds of port may connect.
    // The second letter of the port id should be a digit to indicate which direction it may connect.
    // The ports also need to not already have any link connections and need to face opposite directions.
    public bool CompatiblePorts(GraphObject p1, GraphObject p2) {
      // already connected?
      var part1 = p1.Part;
      var id1 = p1.PortId;
      if (id1 == null || id1 == "") return false;
      if ((part1 as Node).FindLinksConnected(id1).Any(l => l.Category == "")) return false;
      var part2 = p2.Part;
      var id2 = p2.PortId;
      if (id2 == null || id2 == "") return false;
      if ((part2 as Node).FindLinksConnected(id2).Any(l => l.Category == "")) return false;
      // compatible fittings?
      if ((id1[0] == 'U' && id2[0] == 'U') ||
          (id1[0] == 'F' && id2[0] == 'M') ||
          (id1[0] == 'M' && id2[0] == 'F')) {
        // find their effective sides, after rotation
        var a1 = _EffectiveAngle(id1, part1.Angle);
        var a2 = _EffectiveAngle(id2, part2.Angle);
        // are they in opposite directions?
        if (Math.Abs(a1 - a2) - 180 < 0.001) return true;
      }
      return false;
    }

    private double _EffectiveAngle(string id, double angle) {
      var dir = id[1];
      var a = 0.0;
      if (dir == '1') a = 45;
      else if (dir == '2') a = 90;
      else if (dir == '3') a = 135;
      else if (dir == '4') a = 180;
      else if (dir == '5') a = 225;
      else if (dir == '6') a = 270;
      else if (dir == '7') a = 315;
      a += angle;
      if (a < 0) a += 360;
      else if (a >= 360) a -= 360;
      return a;
    }

    // Override this method to find the offset such that a moving port can
    // be snapped to be coincident with a compatible stationary port,
    // then move all of the parts by that offset.
    public override void MoveParts(IDictionary<Part, DraggingInfo> parts, Point offset, bool check) {
      // when moving an actually copied collection of Parts, use the offset that was calculated during the drag
      if (_SnapOffset != null && IsActive && Diagram.LastInput.Up && parts == CopiedParts) {
        base.MoveParts(parts, _SnapOffset.Value, check);
        _SnapOffset = null;
        return;
      }

      var commonOffset = offset;

      // find out if any snapping is desired for any Node being dragged
      foreach (var kvp in parts) {
        if (!(kvp.Key is Node node)) continue;
        var info = kvp.Value;
        var newloc = info.Point.Add(offset);

        // now calculate snap point for this Node
        var snapoffset = newloc.Subtract(node.Location);
        HashSet<GraphObject> nearbyports = null;
        var closestdistance = 20 * 20.0;
        GraphObject closestPort = null;
        Point closestPortPt = default;
        GraphObject nodePort = null;
        var mit = node.Ports;
        foreach (var port in mit) {
          if (node.FindLinksConnected(port.PortId).Any(l => l.Category == "")) continue;
          var portPt = port.GetDocumentPoint(Spot.Center);
          portPt = portPt.Add(snapoffset); // where it would be without snapping

          if (nearbyports == null) {
            // this collects the Nodes that intersect with the Node's bounds,
            // excluding nodes that are being dragged (i.e. in the parts collection)
            var nearbyparts = Diagram.FindElementsIn(node.ActualBounds,
              x => x.Part,
              p => !parts.ContainsKey(p as Part),
              true
            );

            // gather a collection of GraphObjects that are stationary "ports" for this Node
            nearbyports = new HashSet<GraphObject>();
            foreach (var nbp in nearbyparts) {
              if (nbp is Node n) {
                nearbyports.UnionWith(n.Ports);
              }
            }
          }

          foreach (var p in nearbyports) {
            if (!CompatiblePorts(port, p)) continue;
            var ppt = p.GetDocumentPoint(Spot.Center);
            var d = ppt.DistanceSquared(portPt);
            if (d < closestdistance) {
              closestdistance = d;
              closestPort = p;
              closestPortPt = ppt;
              nodePort = port;
            }
          }
        }

        // found something to snap to!
        if (closestPort != null) {
          // move the node so that the compatible ports coincide
          var noderelpt = nodePort.GetDocumentPoint(Spot.Center).Subtract(node.Location);
          var snappt = closestPortPt.Subtract(noderelpt);
          // save the offset, to ensure everything moves together
          commonOffset = snappt.Subtract(newloc).Add(offset);
          // ignore any node.DragComputation function
          // ignore any node.MinLocation and node.MaxLocation
          break;
        }
      }

      // now do the standard movement with the single, perhaps snapped, offset
      _SnapOffset = commonOffset;
      base.MoveParts(parts, commonOffset, check);
    }

    // Establish links between snapped ports,
    // and remove obsolete links because their ports are no longer coincident.
    public override void DoDropOnto(Point pt, GraphObject obj) {
      base.DoDropOnto(pt, obj);
      var tool = this;
      // Need to iterate over all of the dropped nodes to see which ports happen to be snapped to stationary locations
      var coll = CopiedParts ?? DraggedParts;
      foreach (var kvp in coll) {
        if (!(kvp.Key is Node node)) continue;
        // connect all snapped ports of this Node with links
        foreach (var port in node.Ports) {
          // maybe add a link -- see if the port is at another port that is compatible
          var portPt = port.GetDocumentPoint(Spot.Center);
          if (!portPt.IsReal()) continue;
          var nearbyports = Diagram.FindElementsAt(portPt,
            x => { // some GraphObject at portPt
              var o = x;
              // walk up the chain of panels
              while (o != null && o.PortId == null) o = o.Panel;
              return o;
            },
            p => { // a port Panel
              // the parent Node must not be in the dragged collection, and
              // this port P must be compatible with the Node's Port
              if (coll.ContainsKey(p.Part)) return false;
              var ppt = p.GetDocumentPoint(Spot.Center);
              if (portPt.DistanceSquared(ppt) >= 0.25) return false;
              return tool.CompatiblePorts(port, p);
            }
          );
          // did we find a compatible port?
          var np = nearbyports.FirstOrDefault();
          if (np != null) {
            // connect the NODE's PORT with the other port found at the same point
            Diagram.ToolManager.LinkingTool.InsertLink(node, port, np.Part as Node, np);
          }
        }
      }
    }

    // Just move selected nodes when SHIFT moving, causing nodes to be unsnapped.
    // When SHIFTing, must disconnect all links that connect with nodes not being dragged.
    // Without SHIFT, move all nodes that are snapped to selected nodes, even indirectly.
    public override IDictionary<Part, DraggingInfo> ComputeEffectiveCollection(IEnumerable<Part> parts, DraggingOptions options) {
      if (Diagram.LastInput.Shift) {
        var links = new HashSet<Link>();
        var coll = base.ComputeEffectiveCollection(parts, options);
        foreach (var n in coll.Keys) {
          // disconnect all links of this node that connect with stationary node
          if (!(n is Node node)) continue;
          foreach (var link in node.FindLinksConnected()) {
            if (link.Category != "") continue;
            // see if this link connects with a node that is being dragged
            var othernode = link.GetOtherNode(node);
            if (othernode != null && !coll.ContainsKey(othernode)) {
              links.Add(link); // remember for later deletion
            }
          }
        }
        // outside of nested loops we can actually delete the links
        foreach (var l in links) l.Diagram.Remove(l);
        return coll;
      } else {
        var map = new Dictionary<Part, DraggingInfo>();
        if (parts == null) return map;
        foreach (var n in parts) {
          GatherConnecteds(map, n as Node);
        }
        return map;
      }
    }

    // Find other attached nodes.
    private void GatherConnecteds(Dictionary<Part, DraggingInfo> map, Node node) {
      if (map.ContainsKey(node)) return;
      // record the original node location, for relative positioning and for cancellation
      map[node] = new DraggingInfo(node.Location);
      // now recursively collect all connected Nodes and the links to them
      foreach (var link in node.FindLinksConnected()) {
        if (link.Category != "") continue; // ignore comment links
        map[link] = new DraggingInfo();
        GatherConnecteds(map, link.GetOtherNode(node));
      }
    }
  }
  
}
