/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Extensions;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.BlockEditor {
  [ToolboxItem(false)]
  public partial class BlockEditorControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public BlockEditorControl() {
      InitializeComponent();

      saveLoadModel1.SaveClick += (e, obj) => SaveModel();
      saveLoadModel1.LoadClick += (e, obj) => LoadModel();

      goWebBrowser1.Html = @"
  <p>
    Double-click in the background to create a new node.
    Create groups by selecting nodes and invoking Ctrl-G; Ctrl-Shift-G to ungroup a selected group.
    A selected node will have four orange triangles that when clicked will automatically copy the node and link to it.
    Use the context menu to change the shape, color, thickness, and dashed-ness.
  </p>
  <p>
    Links can be drawn by dragging from the side of each node.
    A selected link can be reconnected by dragging an end handle.
    Use the context menu to change the color, thickness, dashed-ness, and which side the link should connect with.
    Press the F2 key to start editing the label of a selected link.
  </p>
";

      saveLoadModel1.ModelJson = @"{
  ""NodeDataSource"": [
{ ""Key"": 1, ""Loc"": ""0 0"", ""Text"": ""Alpha"", ""Details"": ""some information about Alpha and its importance"" },
{ ""Key"": 2, ""Loc"": ""170 0"", ""Text"": ""Beta"", ""Color"": ""blue"", ""Thickness"": 2, ""Figure"": ""Procedure"" },
{ ""Key"": 3, ""Loc"": ""0 100"", ""Text"": ""Gamma"", ""Color"": ""green"", ""Figure"": ""Cylinder1"" },
{ ""Key"": 4, ""Loc"": ""80 180"", ""Text"": ""Delta"", ""Color"": ""red"", ""Figure"": ""Terminator"", ""size"":""80 40"" },
{ ""Key"": 5, ""Loc"": ""350 -50"", ""Text"": ""Zeta"", ""Group"": 7, ""Color"": ""blue"", ""Figure"": ""CreateRequest"" },
{ ""Key"": 6, ""Loc"": ""350 50"", ""Text"": ""Eta"", ""Group"": 7, ""Figure"": ""Document"", ""Fill"": ""lightyellow"" },
{ ""Key"": 7, ""IsGroup"": true, ""Text"": ""Theta"", ""Color"": ""green"", ""Fill"": ""lightgreen"" },
{ ""Key"": 8, ""Loc"": ""520 50"", ""Text"": ""Iota"", ""Fill"": ""pink""}
 ],
  ""LinkDataSource"": [
{ ""From"": 1, ""To"": 2, ""Dash"": [ 6,3 ], ""Thickness"": 4 },
{ ""From"": 1, ""To"": 3, ""Dash"": [ 2,4 ], ""Color"": ""green"", ""Text"": ""label"" },
{ ""From"": 3, ""To"": 4, ""Color"": ""red"", ""Text"": ""a red label"", ""FromSpot"": ""RightSide"" },
{ ""From"": 2, ""To"": 1 },
{ ""From"": 5, ""To"": 6, ""Text"": ""in a group"" },
{ ""From"": 2, ""To"": 7 },
{ ""From"": 6, ""To"": 8, ""Dir"": 0 },
{ ""From"": 6, ""To"": 8, ""Dir"": 1 },
{ ""From"": 6, ""To"": 8, ""Dir"": 2 }
 ]
}";

      Setup();
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // add extra figures
      Figures.DefineExtraFigures();

      // diagram properties
      myDiagram.Padding = 20;  // extra space when scrolled all the way
      myDiagram.Grid =  // a simple 10x10 grid
        new Panel(PanelLayoutGrid.Instance).Add(
          new Shape("LineH") { Stroke = "lightgray", StrokeWidth = 0.5 },
          new Shape("LineV") { Stroke = "lightgray", StrokeWidth = 0.5 }
        );
      myDiagram.ToolManager.DraggingTool.IsGridSnapEnabled = true;
      myDiagram.HandlesDragDropForTopLevelParts = true;
      myDiagram.MouseDrop = (e) => {
        // when the selection is dropped in the diagram's background,
        // make sure the selected Parts no longer belong to any Group
        var ok = e.Diagram.CommandHandler.AddTopLevelParts(e.Diagram.Selection, true);
        if (!ok) e.Diagram.CurrentTool.DoCancel();
      };
      myDiagram.CommandHandler = new DrawCommandHandler();  // support offset copy-and-paste
      myDiagram.ToolManager.ClickCreatingTool.ArchetypeNodeData = new NodeData { Text = "NEW NODE" };  // create a new node by double-clicking in background
      myDiagram.PartCreated += (s, e) => {
        var node = e.Subject as Node;  // the newly inserted Node -- now need to snap its location to the grid
        node.Location = node.Location.SnapToGrid(e.Diagram.Grid.GridOrigin, e.Diagram.Grid.GridCellSize);
        Task.Delay(20).ContinueWith((t) => {  // and have the user start editing its text
          e.Diagram.CommandHandler.EditTextBlock();
        });
      };
      myDiagram.CommandHandler.ArchetypeGroupData = new NodeData {
        IsGroup = true,
        Text = "NEW GROUP"
      };
      myDiagram.SelectionGrouped += (s, e) => {
        var group = e.Subject;
        Task.Delay(20).ContinueWith((t) => {  // and have the user start editing its text
          e.Diagram.CommandHandler.EditTextBlock();
        });
      };
      myDiagram.LinkRelinked += (s, e) => {
        // re-spread the connections of other links connected with both old and new nodes
        var oldnode = (e.Parameter as GraphObject).Part as Node;
        oldnode.InvalidateConnectedLinks();
        var link = e.Subject as Link;
        if (e.Diagram.ToolManager.LinkingTool.IsForwards) {
          link.ToNode.InvalidateConnectedLinks();
        } else {
          link.FromNode.InvalidateConnectedLinks();
        }
      };
      myDiagram.UndoManager.IsEnabled = true;

      // node template
      myDiagram.NodeTemplate =
        new Node("Auto") {
          LocationSpot = Spot.Center, LocationElementName = "SHAPE",
          DesiredSize = new Size(120, 60), MinSize = new Size(40, 40),
          Resizable = true, ResizeCellSize = new Size(20, 20)
        }
          .Bind(
            new Binding("Location", "Loc", Point.Parse).MakeTwoWay(Point.Stringify),
            new Binding("DesiredSize", "Size", Northwoods.Go.Size.Parse).MakeTwoWay(Northwoods.Go.Size.Stringify)
          )
          .Add(
            new Shape {  // the border
              Name = "SHAPE", Fill = "white",
              PortId = "", Cursor = "pointer",
              FromLinkable = true, ToLinkable = true,
              FromLinkableDuplicates = true, ToLinkableDuplicates = true,
              FromSpot = Spot.AllSides, ToSpot = Spot.AllSides
            }
              .Bind(
                new Binding("Figure"),
                new Binding("Fill"),
                new Binding("Stroke", "Color"),
                new Binding("StrokeWidth", "Thickness"),
                new Binding("StrokeDashArray", "Dash")
              ),
            // this Shape prevents mouse events from reaching the middle of the port
            new Shape { Width = 100, Height = 40, StrokeWidth = 0, Fill = "transparent" },
            new TextBlock { Margin = 1, TextAlign = TextAlign.Center, Overflow = Overflow.Ellipsis, Editable = true }
              .Bind(
                // this binding is TwoWay due to the user editing the text with the textEditingTool
                new Binding("Text").MakeTwoWay(),
                new Binding("Stroke", "Color")
              )
        );

      myDiagram.NodeTemplate.ToolTip =
        Builder.Make<Adornment>("ToolTip")
          .Add(
            new Panel("Vertical") {
              MaxSize = new Size(200, double.NaN)
            }
              .Add(
                new TextBlock { Font = new Font("Segoe UI", 13, FontWeight.Bold), TextAlign = TextAlign.Center }
                  .Bind("Text"),
                new TextBlock { Font = new Font("Segoe UI", 13), TextAlign = TextAlign.Center }
                  .Bind("Text", "Details")
              )
            );

      // Node selection adornment
      // Include four large triangular buttons so that the user can easily make a copy
      // of the node, move it to be in that direction relative to the original node,
      // and add a link to the new node.

      Shape MakeArrowButton(Spot spot, string fig) {
        void maker(InputEvent e, GraphObject shape) {
          e.Handled = true;
          e.Diagram.Model.Commit((m) => {
            var model = m as Model;
            var selnode = (shape.Part as Adornment).AdornedPart;
            // create a new node in the direction of the spot
            var p = Point.SetSpot(selnode.ActualBounds, spot);
            p = p.Subtract(selnode.Location);
            p = p.Scale(2, 2);
            p = p.Offset(Math.Sign(p.X) * 60, Math.Sign(p.Y) * 60);
            p = p.Add(selnode.Location);
            p = p.SnapToGrid(e.Diagram.Grid.GridOrigin, e.Diagram.Grid.GridCellSize);
            // make the new node a copy of the selected node
            var nodedata = model.CopyNodeData(selnode.Data as NodeData);
            // add to same group as selected node
            model.SetGroupKeyForNodeData(nodedata, model.GetGroupKeyForNodeData(selnode.Data as NodeData));
            model.AddNodeData(nodedata);  // add to model
            // create a link from the selected node to the new node
            var linkdata = new LinkData { From = (int)selnode.Key, To = model.GetKeyForNodeData(nodedata) };
            model.AddLinkData(linkdata);  // add to model
            // move the new node to the computed location, select it, and start to edit it
            var newnode = e.Diagram.FindNodeForData(nodedata);
            newnode.Location = p;
            e.Diagram.Select(newnode);
            Task.Delay(20).ContinueWith((t) => {
              e.Diagram.CommandHandler.EditTextBlock();
            });
          });
        }
        return new Shape {
          Figure = fig,
          Alignment = spot, AlignmentFocus = spot.Opposite(),
          Width = (spot.Equals(Spot.Top) || spot.Equals(Spot.Bottom)) ? 36 : 18,
          Height = (spot.Equals(Spot.Top) || spot.Equals(Spot.Bottom)) ? 18 : 36,
          Fill = "orange", StrokeWidth = 0,
          IsActionable = true, // needed because it's an Adornment
          Click = maker, ContextClick = maker
        };
      }

      Shape CMButton(object options = null) {
        return new Shape {
          Fill = "orange", Stroke = "gray", Background = "transparent",
          GeometryString = "F1 M0 0 M0 4h4v4h-4z M6 4h4v4h-4z M12 4h4v4h-4z M0 12",
          IsActionable = true, Cursor = "context-menu",
          Click = (e, shape) => {
            e.Diagram.CommandHandler.ShowContextMenu((shape.Part as Adornment).AdornedPart);
          }
        }
          .Set(options);
      }

      myDiagram.NodeTemplate.SelectionAdornmentTemplate =
        new Adornment("Spot")
          .Add(
            new Placeholder { Padding = 10 },
            MakeArrowButton(Spot.Top, "TriangleUp"),
            MakeArrowButton(Spot.Left, "TriangleLeft"),
            MakeArrowButton(Spot.Right, "TriangleRight"),
            MakeArrowButton(Spot.Bottom, "TriangleDown"),
            CMButton(new { Alignment = new Spot(0.75, 0) })
          );

      // Common context menu button definitions

      // All buttons in context menu work on both click and context click,
      // in case the user context-clicks on the button.
      // All buttons modify the node data, not the Node, so the Bindings need not be TwoWay.

      // A button-defining helper function that returns a click event handler.
      // PROPNAME is the name of the data property that should be set to the given VALUE.
      Action<InputEvent, GraphObject> ClickFunction(string propname, object value) {
        return (InputEvent e, GraphObject obj) => {
          e.Handled = true; // don't let the click bubble up
          e.Diagram.Model.Commit((m) => {
            m.Set((obj.Part as Adornment).AdornedPart.Data, propname, value);
          });
        };
      }

      // Create a context menu button for setting a data property with a color value.
      Shape ColorButton(string color, string propname = "Color") {
        return new Shape {
          Width = 16, Height = 16, Stroke = "lightgray", Fill = color,
          Margin = 1, Background = "transparent",
          MouseEnter = (e, shape, obj) => { (shape as Shape).Stroke = "dodgerblue"; },
          MouseLeave = (e, shape, obj) => { (shape as Shape).Stroke = "lightgray"; },
          Click = ClickFunction(propname, color), ContextClick = ClickFunction(propname, color)
        };
      }

      // used by multiple context menus
      GraphObject LightFillButton1() {
        return Builder.Make<Panel>("ContextMenuButton")
          .Add(
            new Panel("Horizontal")
              .Add(ColorButton("white", "Fill"), ColorButton("beige", "Fill"), ColorButton("aliceblue", "Fill"), ColorButton("lightyellow", "Fill"))
          );
      }

      GraphObject LightFillButton2() {
        return Builder.Make<Panel>("ContextMenuButton")
          .Add(
            new Panel("Horizontal")
              .Add(ColorButton("lightgray", "Fill"), ColorButton("lightgreen", "Fill"), ColorButton("lightblue", "Fill"), ColorButton("pink", "Fill")
            )
          );
      }

      // used by multiple context menus
      GraphObject DarkColorButton1() {
        return Builder.Make<Panel>("ContextMenuButton")
          .Add(
            new Panel("Horizontal")
              .Add(ColorButton("black"), ColorButton("green"), ColorButton("blue"), ColorButton("red"))
          );
      }

      GraphObject DarkColorButton2() {
        return Builder.Make<Panel>("ContextMenuButton")
          .Add(
            new Panel("Horizontal")
              .Add(ColorButton("brown"), ColorButton("magenta"), ColorButton("purple"), ColorButton("orange"))
          );
      }

      // Create a context menu button for setting a data property with a stroke width value.
      Shape ThicknessButton(double sw, string propname = "Thickness") {
        return new Shape("LineH") {
          Width = 16, Height = 16, StrokeWidth = sw,
          Margin = 1, Background = "transparent",
          MouseEnter = (e, shape, obj) => { shape.Background = "dodgerblue"; },
          MouseLeave = (e, shape, obj) => { shape.Background = "transparent"; },
          Click = ClickFunction(propname, sw), ContextClick = ClickFunction(propname, sw)
        };
      }

      // Create a context menu button for setting a data property with a stroke dash Array value.
      Shape DashButton(float[] dash, string propname = "Dash") {
        return new Shape("LineH") {
          Width = 24, Height = 16, StrokeWidth = 2,
          StrokeDashArray = dash,
          Margin = 1, Background = "transparent",
          MouseEnter = (e, shape, obj) => { shape.Background = "dodgerblue"; },
          MouseLeave = (e, shape, obj) => { shape.Background = "transparent"; },
          Click = ClickFunction(propname, dash), ContextClick = ClickFunction(propname, dash)
        };
      }

      // used by multiple context menus
      GraphObject StrokeOptionsButton1() {
        return Builder.Make<Panel>("ContextMenuButton")
          .Add(
            new Panel("Horizontal")
              .Add(ThicknessButton(1), ThicknessButton(2), ThicknessButton(3), ThicknessButton(4))
          );
      }

      GraphObject StrokeOptionsButton2() {
        return Builder.Make<Panel>("ContextMenuButton")
          .Add(
            new Panel("Horizontal")
              .Add(DashButton(null), DashButton(new float[] { 2, 4 }), DashButton(new float[] { 4, 4 }))
          );
      }

      // Node context menu
      Shape FigureButton(string fig, string propname = "Figure") {
        return new Shape {
          Width = 32, Height = 32, Scale = 0.5, Fill = "lightgray", Figure = fig,
          Margin = 1, Background = "transparent",
          MouseEnter = (InputEvent e, GraphObject shape, GraphObject obj) => { (shape as Shape).Fill = "dodgerblue"; },
          MouseLeave = (InputEvent e, GraphObject shape, GraphObject obj) => { (shape as Shape).Fill = "lightgray"; },
          Click = ClickFunction(propname, fig), ContextClick = ClickFunction(propname, fig)
        };
      }

      myDiagram.NodeTemplate.ContextMenu =
        Builder.Make<Adornment>("ContextMenu")
          .Add(
            Builder.Make<Panel>("ContextMenuButton")
              .Add(
                new Panel("Horizontal")
                  .Add(FigureButton("Rectangle"), FigureButton("RoundedRectangle"), FigureButton("Ellipse"), FigureButton("Diamond"))
              ),
            Builder.Make<Panel>("ContextMenuButton")
              .Add(
                new Panel("Horizontal")
                  .Add(FigureButton("Parallelogram2"), FigureButton("ManualOperation"), FigureButton("Procedure"), FigureButton("Cylinder1"))
              ),
            Builder.Make<Panel>("ContextMenuButton")
              .Add(
                new Panel("Horizontal")
                  .Add(FigureButton("Terminator"), FigureButton("CreateRequest"), FigureButton("Document"), FigureButton("TriangleDown"))
              ),
            LightFillButton1(),
            LightFillButton2(),
            DarkColorButton1(),
            DarkColorButton2(),
            StrokeOptionsButton1(),
            StrokeOptionsButton2()
          );

      // group template
      myDiagram.GroupTemplate =
        new Group("Spot") {
          LayerName = "Background",
          Ungroupable = true,
          LocationSpot = Spot.Center,
          SelectionElementName = "BODY",
          ComputesBoundsAfterDrag = true,  // allow dragging out of a Group that uses a Placeholder
          HandlesDragDropForMembers = true,  // don't need to define handlers on Nodes and Links
          MouseDrop = (e, grp) => {  // add dropped nodes as members of the group
            var ok = (grp as Group).AddMembers(grp.Diagram.Selection, true);
            if (!ok) grp.Diagram.CurrentTool.DoCancel();
          },
          Avoidable = false
        }
          .Bind("Location", "Loc", Point.Parse, Point.Stringify)
          .Add(
            new Panel("Auto") {
              Name = "BODY"
            }
              .Add(
                new Shape {
                  Parameter1 = 10,
                  Fill = "white", StrokeWidth = 2,
                  PortId = "", Cursor = "pointer",
                  FromLinkable = true, ToLinkable = true,
                  FromLinkableDuplicates = true, ToLinkableDuplicates = true,
                  FromSpot = Spot.AllSides, ToSpot = Spot.AllSides
                }
                  .Bind(
                    new Binding("Fill"),
                    new Binding("Stroke", "Color"),
                    new Binding("StrokeWidth", "Thickness"),
                    new Binding("StrokeDashArray", "Dash")
                  ),
                new Placeholder { Background = "transparent", Margin = 10 }
              ),
            new TextBlock {
              Alignment = Spot.Top, AlignmentFocus = Spot.Bottom,
              Font = new Font("Microsoft Sans Serif", 12, FontWeight.Bold, FontUnit.Point), Editable = true
            }
              .Bind(
                new Binding("Text"),
                new Binding("Stroke", "Color")
              )
          );

      myDiagram.GroupTemplate.SelectionAdornmentTemplate =
        new Adornment("Spot")
          .Add(
            new Panel("Auto")
              .Add(
                new Shape { Fill = null, Stroke = "dodgerblue", StrokeWidth = 3 },
                new Placeholder { Margin = 1.5 }
              ),
            CMButton(new { Alignment = Spot.TopRight, AlignmentFocus = Spot.BottomRight })
          );

      myDiagram.GroupTemplate.ContextMenu =
        Builder.Make<Adornment>("ContextMenu")
          .Add(
            LightFillButton1(),
            LightFillButton2(),
            DarkColorButton1(),
            DarkColorButton2(),
            StrokeOptionsButton1(),
            StrokeOptionsButton2()
          );

      // link template
      myDiagram.LinkTemplate =
        new Link {
          LayerName = "Foreground",
          Routing = LinkRouting.AvoidsNodes, Corner = 10,
          ToShortLength = 4,  // assume arrowhead at "to" end, need to avoid bad appearance when path is thick
          RelinkableFrom = true, RelinkableTo = true,
          Reshapable = true, Resegmentable = true
        }
          .Bind(
            new Binding("FromSpot", "FromSpot", Spot.Parse),
            new Binding("ToSpot", "ToSpot", Spot.Parse),
            new Binding("FromShortLength", "Dir", (dir, obj) => { return (int)dir == 2 ? 4 : 0; }),
            new Binding("ToShortLength", "Dir", (dir, obj) => { return (int)dir >= 1 ? 4 : 0; }),
            new Binding("Points").MakeTwoWay()  // TwoWay due to user reshaping with LinkReshapingTool
          )
          .Add(
            new Shape { StrokeWidth = 2 }
              .Bind(
                new Binding("Stroke", "Color"),
                new Binding("StrokeWidth", "Thickness"),
                new Binding("StrokeDashArray", "Dash")
              ),
            new Shape { FromArrow = "Backward", StrokeWidth = 0, Scale = 4 / 3, Visible = false }
              .Bind(
                new Binding("Visible", "Dir", (dir, obj) => { return (int)dir == 2; }),
                new Binding("Fill", "Color"),
                new Binding("Scale", "Thickness", (t, obj) => { return (2 + (float)t) / 3; })
              ),
            new Shape { ToArrow = "Standard", StrokeWidth = 0, Scale = 4 / 3 }
              .Bind(
                new Binding("Visible", "Dir", (dir, obj) => { return (int)dir >= 1; }),
                new Binding("Fill", "Color"),
                new Binding("Scale", "Thickness", (t, obj) => { return (2 + (float)t) / 3; })
              ),
            new TextBlock { AlignmentFocus = new Spot(0, 1, -4, 0), Editable = true }
              .Bind(
                new Binding("Text").MakeTwoWay(),  // TwoWay due to user editing with TextEditingTool
                new Binding("Stroke", "Color")
              )
          );

      myDiagram.LinkTemplate.SelectionAdornmentTemplate =
        new Adornment()  // use a special selection Adornment that does not obscure the link path itself
          .Add(
            new Shape { // this uses a PathPattern with a gap in it, in order to avoid drawing on top of the link path Shape
              IsPanelMain = true,
              Stroke = "transparent", StrokeWidth = 6,
              PathPattern = MakeAdornmentPathPattern(2f, null)  // == thickness or StrokeWidth
            }
              .Bind("PathPattern", "Thickness", MakeAdornmentPathPattern),
            CMButton(new { AlignmentFocus = new Spot(0, 0, -6, 4) })
          );

      Shape MakeAdornmentPathPattern(object w, object _) {
        return new Shape {
          Stroke = "dodgerblue", StrokeWidth = 2, StrokeCap = LineCap.Square,
          GeometryString = "M0 0 M4 2 H3 M4 " + ((float)w + 4).ToString() + " H3"
        };
      }

      // Link context menu
      // All buttons in context menu work on both click and context click,
      // in case the user context-clicks on the button.
      // All buttons modify the link data, not the Link, so the Bindings need not be TwoWay.

      Shape ArrowButton(int num) {
        var geo = "M0 0 M16 16 M0 8 L16 8  M12 11 L16 8 L12 5";
        if (num == 0) {
          geo = "M0 0 M16 16 M0 8 L16 8";
        } else if (num == 2) {
          geo = "M0 0 M16 16 M0 8 L16 8  M12 11 L16 8 L12 5  M4 11 L0 8 L4 5";
        }
        return new Shape {
          GeometryString = geo,
          Margin = 2, Background = "transparent",
          MouseEnter = (e, shape, obj) => { shape.Background = "dodgerblue"; },
          MouseLeave = (e, shape, obj) => { shape.Background = "transparent"; },
          Click = ClickFunction("Dir", num), ContextClick = ClickFunction("Dir", num)
        };
      }

      Shape AllSidesButton(bool to) {
        void setter(InputEvent e, GraphObject shape) {
          e.Handled = true;
          e.Diagram.Model.Commit((m) => {
            var link = (shape.Part as Adornment).AdornedPart as Link;
            m.Set(link.Data, to ? "ToSpot" : "FromSpot", Spot.Stringify(Spot.AllSides));
            // re-spread the connections of other links connected with the node
            (to ? link.ToNode : link.FromNode).InvalidateConnectedLinks();
          });
        }
        return new Shape {
          Width = 12, Height = 12, Fill = "transparent",
          MouseEnter = (e, shape, obj) => { shape.Background = "dodgerblue"; },
          MouseLeave = (e, shape, obj) => { shape.Background = "transparent"; },
          Click = setter, ContextClick = setter
        };
      }

      Shape SpotButton(Spot spot, bool to) {
        var ang = 0.0;
        var side = Spot.RightSide;
        if (spot.Equals(Spot.Top)) { ang = 270; side = Spot.TopSide; } else if (spot.Equals(Spot.Left)) { ang = 180; side = Spot.LeftSide; } else if (spot.Equals(Spot.Bottom)) { ang = 90; side = Spot.BottomSide; }
        if (!to) ang -= 180;
        void setter(InputEvent e, GraphObject shape) {
          e.Handled = true;
          e.Diagram.Model.Commit((m) => {
            var link = (shape.Part as Adornment).AdornedPart as Link;
            m.Set(link.Data, to ? "ToSpot" : "FromSpot", Spot.Stringify(side));
            // re-spread the connections of other links connected with the node
            (to ? link.ToNode : link.FromNode).InvalidateConnectedLinks();
          });
        }
        return new Shape {
          Alignment = spot, AlignmentFocus = spot.Opposite(),
          GeometryString = "M0 0 M12 12 M12 6 L1 6 L4 4 M1 6 L4 8",
          Angle = ang,
          Background = "transparent",
          MouseEnter = (e, shape, obj) => { shape.Background = "dodgerblue"; },
          MouseLeave = (e, shape, obj) => { shape.Background = "transparent"; },
          Click = setter, ContextClick = setter
        };
      }

      myDiagram.LinkTemplate.ContextMenu =
        Builder.Make<Adornment>("ContextMenu")
          .Add(
            DarkColorButton1(),
            DarkColorButton2(),
            StrokeOptionsButton1(),
            StrokeOptionsButton2(),
            Builder.Make<Panel>("ContextMenuButton")
              .Add(
                new Panel("Horizontal")
                  .Add(ArrowButton(0), ArrowButton(1), ArrowButton(2))
              ),
            Builder.Make<Panel>("ContextMenuButton")
              .Add(
                new Panel("Horizontal")
                  .Add(
                    new Panel(PanelLayoutSpot.Instance)
                      .Add(
                        AllSidesButton(false),
                        SpotButton(Spot.Top, false), SpotButton(Spot.Left, false), SpotButton(Spot.Right, false), SpotButton(Spot.Bottom, false)
                      ),
                    new Panel(PanelLayoutSpot.Instance) { Margin = new Margin(0, 0, 0, 2) }
                      .Add(
                        AllSidesButton(true),
                        SpotButton(Spot.Top, true), SpotButton(Spot.Left, true), SpotButton(Spot.Right, true), SpotButton(Spot.Bottom, true)
                      )
                  )
              )
          );

      LoadModel();

    }

    private void SaveModel() {
      if (myDiagram == null) return;
      saveLoadModel1.ModelJson = myDiagram.Model.ToJson();
    }

    private void LoadModel() {
      if (myDiagram == null) return;
      myDiagram.Model = Model.FromJson<Model>(saveLoadModel1.ModelJson);
      myDiagram.Model.UndoManager.IsEnabled = true;
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }
  public class NodeData : Model.NodeData {
    public string Details { get; set; }
    public string Loc { get; set; }
    public string Color { get; set; }
    public string Figure { get; set; }
    public string Size { get; set; }
    public string Fill { get; set; }
    public float? Thickness { get; set; }
    public float[] Dash { get; set; }
  }

  public class LinkData : Model.LinkData {
    public List<Northwoods.Go.Point> Points { get; set; }
    public float[] Dash { get; set; }
    public string Color { get; set; }
    public int? Dir { get; set; }
    public string FromSpot { get; set; }
    public string ToSpot { get; set; }
    public float? Thickness { get; set; }
  }

}
