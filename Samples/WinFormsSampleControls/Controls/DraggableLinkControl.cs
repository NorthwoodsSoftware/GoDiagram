using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Extensions;
using Northwoods.Go.WinForms;

namespace WinFormsSampleControls.DraggableLink {
  [ToolboxItem(false)]
  public partial class DraggableLinkControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    private Palette myPalette;

    private Part mySharedNodeTemplate;

    public DraggableLinkControl() {
      InitializeComponent();

      saveLoadModel1.SaveClick += (e, obj) => SaveModel();
      saveLoadModel1.LoadClick += (e, obj) => LoadModel();

      saveLoadModel1.ModelJson = @"{
  ""LinkFromPortIdProperty"": ""FromPort"",
  ""LinkToPortIdProperty"": ""ToPort"",
  ""NodeDataSource"": [ ],
  ""LinkDataSource"": [ ]
}";

      goWebBrowser1.Html = @"
  <p>
    This sample demonstrates the ability for the user to drag around a Link as if it were a Node.
    When either end of the link passes over a valid port, the port is highlighted.
  </p>
  <p>
    The link-dragging functionality is enabled by setting some or all of the following properties:
    <a>DraggingTool.DragsLink</a>, <a>LinkingTool.IsUnconnectedLinkValid</a>, and
    <a>RelinkingTool.IsUnconnectedLinkValid</a>.
  </p>
  <p>
    Note that a Link is present in the <a>Palette</a> so that it too can be dragged out and onto
    the main Diagram.  Because links are not automatically routed when either end is not connected
    with a Node, the route is provided explicitly when that Palette item is defined.
  </p>
  <p>
    This also demonstrates several custom Adornments:
    <a>Part.SelectionAdornmentTemplate</a>, <a>Part.ResizeAdornmentTemplate</a>, and
    <a>Part.RotateAdornmentTemplate</a>.
  </p>
  <p>
    Finally this sample demonstrates saving and restoring the <a>Diagram.Position</a> as a property
    on the <a>Model.SharedData</a> object that is automatically saved and restored when calling <a>Model.ToJson</a>
    and <a>Model.FromJson</a>.
  </p>
";

      Setup();
      SetupPalette();
    }

    private void DefineNodeTemplate() {
      if (mySharedNodeTemplate != null) return;  // already defined
      // load extra figures
      Figures.DefineExtraFigures();

      // Define a function for creating a "port" that is normally transparent.
      // The "name" is used as the GraphObject.PortId, the "spot" is used to control how links connect
      // and where the port is positioned on the node, and the boolean "output" and "input" arguments
      // control whether the user can draw links from or to the port.
      Shape MakePort(string name, Spot spot, bool output, bool input) {
        // the port is basically just a small transparent square
        return new Shape {
          Figure = "Circle",
          Fill = null,  // not seen, by default; set to a translucent gray by showSmallPorts, defined below
          Stroke = null,
          DesiredSize = new Size(7, 7),
          Alignment = spot,  // align the port on the main Shape
          AlignmentFocus = spot,  // just inside the Shape
          PortId = name,  // declare this object to be a "port"
          FromSpot = spot,
          ToSpot = spot,  // declare where links may connect at this port
          FromLinkable = output,
          ToLinkable = input,  // declare whether the user may draw links to/from here
          Cursor = "pointer"  // show a different cursor to indicate potential link point
        };
      }

      var nodeSelectionAdornmentTemplate =
        new Adornment(PanelLayoutAuto.Instance).Add(
          new Shape {
            Fill = null,
            Stroke = "deepskyblue",
            StrokeWidth = 1.5,
            StrokeDashArray = new float[] { 4, 2 }
          },
          new Placeholder()
        );

      var nodeResizeAdornmentTemplate =
        new Adornment(PanelLayoutSpot.Instance) {
          LocationSpot = Spot.Right
        }.Add(
          new Placeholder(),
          new Shape {
            Alignment = Spot.TopLeft,
            Cursor = "nw-resize",
            DesiredSize = new Size(6, 6),
            Fill = "lightblue",
            Stroke = "deepskyblue"
          },
          new Shape {
            Alignment = Spot.Top,
            Cursor = "n-resize",
            DesiredSize = new Size(6, 6),
            Fill = "lightblue",
            Stroke = "deepskyblue"
          },
          new Shape {
            Alignment = Spot.TopRight,
            Cursor = "ne-resize",
            DesiredSize = new Size(6, 6),
            Fill = "lightblue",
            Stroke = "deepskyblue"
          },

          new Shape {
            Alignment = Spot.Left,
            Cursor = "w-resize",
            DesiredSize = new Size(6, 6),
            Fill = "lightblue",
            Stroke = "deepskyblue"
          },
          new Shape {
            Alignment = Spot.Right,
            Cursor = "e-resize",
            DesiredSize = new Size(6, 6),
            Fill = "lightblue",
            Stroke = "deepskyblue"
          },

          new Shape {
            Alignment = Spot.BottomLeft,
            Cursor = "se-resize",
            DesiredSize = new Size(6, 6),
            Fill = "lightblue",
            Stroke = "deepskyblue"
          },
          new Shape {
            Alignment = Spot.Bottom,
            Cursor = "s-resize",
            DesiredSize = new Size(6, 6),
            Fill = "lightblue",
            Stroke = "deepskyblue"
          },
          new Shape {
            Alignment = Spot.BottomRight,
            Cursor = "sw-resize",
            DesiredSize = new Size(6, 6),
            Fill = "lightblue",
            Stroke = "deepskyblue"
          }
        );

      var nodeRotateAdornmentTemplate =
        new Adornment {
          LocationSpot = Spot.Center,
          LocationElementName = "CIRCLE"
        }.Add(
          new Shape {
            Figure = "Circle",
            Name = "CIRCLE",
            Cursor = "pointer",
            DesiredSize = new Size(7, 7),
            Fill = "lightblue",
            Stroke = "deepskyblue"
          },
          new Shape {
            GeometryString = "M3.5 7 L3.5 30",
            IsGeometryPositioned = true,
            Stroke = "deepskyblue",
            StrokeWidth = 1.5,
            StrokeDashArray = new float[] { 4, 2 }
          }
        );

      mySharedNodeTemplate =
        new Node(PanelLayoutSpot.Instance) {
          LocationSpot = Spot.Center,
          Selectable = true,
          SelectionAdornmentTemplate = nodeSelectionAdornmentTemplate,
          Resizable = true,
          ResizeElementName = "PANEL",
          ResizeAdornmentTemplate = nodeResizeAdornmentTemplate,
          Rotatable = true,
          RotateAdornmentTemplate = nodeRotateAdornmentTemplate,
          // handle mouse enter/leave events to show/hide the ports
          MouseEnter = (e, node, _) => { ShowSmallPorts(node, true); },
          MouseLeave = (e, node, _) => { ShowSmallPorts(node, false); }
        }.Bind(
          new Binding("Location", "Loc", Point.Parse).MakeTwoWay(Point.Stringify),
          new Binding("Angle").MakeTwoWay()
        ).Add(
          // the main object is a Panel that surrounds a TextBlock with a Shape
          new Panel(PanelLayoutAuto.Instance) {
            Name = "PANEL"
          }.Bind(
            new Binding("DesiredSize", "Size", Northwoods.Go.Size.Parse).MakeTwoWay(Northwoods.Go.Size.Stringify)
          ).Add(
            new Shape {
              Figure = "Rectangle",
              PortId = "", // the default Port = if no spot on link data, use closest side
              FromLinkable = true,
              ToLinkable = true,
              Cursor = "pointer",
              Fill = "white",  // default color
              StrokeWidth = 2
            }.Bind(
              new Binding("Figure"),
              new Binding("Fill")
            ),
            new TextBlock {
              Font = new Font("Segoe UI", 11, FontWeight.Bold),
              Margin = 8,
              MaxSize = new Size(160, double.NaN),
              Wrap = Wrap.Fit,
              Editable = true
            }.Bind(
              new Binding("Text").MakeTwoWay()
            )
          ),
          // four small named ports, one on each side:
          MakePort("T", Spot.Top, false, true),
          MakePort("L", Spot.Left, true, true),
          MakePort("R", Spot.Right, true, true),
          MakePort("B", Spot.Bottom, true, false)
        );

      void ShowSmallPorts(GraphObject node, bool show) {
        foreach (var port in (node as Node).Ports) {
          if (port.PortId != "") {  // don't change the default port, which is the big shape
            (port as Shape).Fill = show ? "rgba(0,0,0,.3)" : (Brush)null;
          }
        }
      }
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.Grid =
        new Panel(PanelLayoutGrid.Instance).Add(
          new Shape { Figure = "LineH", Stroke = "lightgray", StrokeWidth = 0.5 },
          new Shape { Figure = "LineH", Stroke = "gray", StrokeWidth = 0.5, Interval = 10 },
          new Shape { Figure = "LineV", Stroke = "lightgray", StrokeWidth = 0.5 },
          new Shape { Figure = "LineV", Stroke = "gray", StrokeWidth = 0.5, Interval = 10 }
        );
      myDiagram.ToolManager.DraggingTool.DragsLink = true;
      myDiagram.ToolManager.DraggingTool.IsGridSnapEnabled = true;
      myDiagram.ToolManager.LinkingTool.IsUnconnectedLinkValid = true;
      myDiagram.ToolManager.LinkingTool.PortGravity = 20;
      myDiagram.ToolManager.RelinkingTool.IsUnconnectedLinkValid = true;
      myDiagram.ToolManager.RelinkingTool.PortGravity = 20;
      myDiagram.ToolManager.RelinkingTool.FromHandleArchetype =
        new Adornment().Add(
          new Shape { Figure = "Diamond", SegmentIndex = 0, Cursor = "pointer", DesiredSize = new Size(8, 8), Fill = "tomato", Stroke = "darkred" }
        );
      myDiagram.ToolManager.RelinkingTool.FromHandleArchetype =
        new Shape { Figure = "Diamond", SegmentIndex = -1, Cursor = "pointer", DesiredSize = new Size(8, 8), Fill = "darkred", Stroke = "tomato" };
      myDiagram.ToolManager.LinkReshapingTool.HandleArchetype =
        new Shape { Figure = "Diamond", DesiredSize = new Size(7, 7), Fill = "lightblue", Stroke = "deepskyblue" };
      myDiagram.ToolManager.RotatingTool.HandleAngle = 270;
      myDiagram.ToolManager.RotatingTool.HandleDistance = 30;
      myDiagram.ToolManager.RotatingTool.SnapAngleMultiple = 15;
      myDiagram.ToolManager.RotatingTool.SnapAngleEpsilon = 15;
      myDiagram.UndoManager.IsEnabled = true;

      DefineNodeTemplate();
      myDiagram.NodeTemplate = mySharedNodeTemplate;

      var linkSelectionAdornmentTemplate =
        new Adornment(PanelLayoutLink.Instance).Add(
          new Shape {
            // isPanelMain declares that this Shape shares the Link.Geometry
            IsPanelMain = true,
            Fill = (Brush)null,
            Stroke = "deepskyblue",
            StrokeWidth = 0
          }  // use selection object's strokeWidth
        );

      // link template
      myDiagram.LinkTemplate =
        new Link {  // the whole link panel
          Selectable = true,
          SelectionAdornmentTemplate = linkSelectionAdornmentTemplate,
          RelinkableFrom = true,
          RelinkableTo = true,
          Reshapable = true,
          Routing = LinkRouting.AvoidsNodes,
          Curve = LinkCurve.JumpOver,
          Corner = 5,
          ToShortLength = 4
        }.Bind(
          new Binding("Points").MakeTwoWay()
        ).Add(
          new Shape { // the link path shape
            IsPanelMain = true,
            StrokeWidth = 2
          },
          new Shape { // the arrowhead
            ToArrow = "Standard",
            Stroke = (Brush)null
          },
          new Panel(PanelLayoutAuto.Instance).Bind(
            new Binding("Visible", "IsSelected").OfElement()
          ).Add(
            new Shape {
              Figure = "RoundedRectangle",
              Fill = "#F8F8F8",
              Stroke = (Brush)null
            },
            new TextBlock {
              TextAlign = TextAlign.Center,
              Font = new Font("Segoe UI", 10),
              Stroke = "#919191",
              Margin = 2,
              MinSize = new Size(10, double.NaN),
              Editable = true
            }.Bind(
              new Binding("Text").MakeTwoWay()
            )
          )
        );

      // load an initial diagram from some JSON text
      LoadModel();
    }

    private void SetupPalette() {
      myPalette = paletteControl1.Diagram as Palette;
      // myPalette properties
      myPalette.MaxSelectionCount = 1;

      DefineNodeTemplate();
      myPalette.NodeTemplate = mySharedNodeTemplate;

      myPalette.LinkTemplate = // simplify the link template, just for the palette
        new Link {
          // because the GridLayout.Alignment is Location and the nodes have locationSpot == Spot.Center,
          // to line up the Link in the same manner we have to pretend the Link has the same location spot
          LocationSpot = Spot.Center,
          SelectionAdornmentTemplate =
            new Adornment(PanelLayoutLink.Instance) {
              LocationSpot = Spot.Center
            }.Add(
              new Shape {
                IsPanelMain = true,
                Fill = null,
                Stroke = "deepskyblue",
                StrokeWidth = 0
              },
              new Shape  // the arrowhead
                            {
                ToArrow = "Standard",
                Stroke = null
              }
            ),
          Routing = LinkRouting.AvoidsNodes,
          Curve = LinkCurve.JumpOver,
          Corner = 5,
          ToShortLength = 4
        }.Bind(
          new Binding("Points")
        ).Add(
          new Shape { // the link path shape
            IsPanelMain = true,
            StrokeWidth = 2
          },
          new Shape { // the arrowhead
            ToArrow = "Standard",
            Stroke = null
          }
        );
      myPalette.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Text = "Start", Figure = "Circle", Fill = "#00AD5F" },
          new NodeData { Text = "Step" },
          new NodeData { Text = "DB", Figure = "Database", Fill = "lightgray" },
          new NodeData { Text = "???", Figure = "Diamond", Fill = "lightskyblue" },
          new NodeData { Text = "End", Figure = "Circle", Fill = "#CE0620" },
          new NodeData { Text = "Comment", Figure = "RoundedRectangle", Fill = "lightyellow" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { Points = new List<Point> { new Point(0, 0), new Point(30, 0), new Point(30, 40), new Point(60, 40) } }
        }
      };
    }

    private void SaveModel() {
      if (myDiagram == null) return;
      SaveDiagramProperties(); // do this first, before writing to JSON
      saveLoadModel1.ModelJson = myDiagram.Model.ToJson();
    }

    private void LoadModel() {
      if (myDiagram == null) return;
      myDiagram.Model = Model.FromJson<Model>(saveLoadModel1.ModelJson);
      myDiagram.Model.UndoManager.IsEnabled = true;
      if (!(myDiagram.Model.SharedData is SharedData)) {
        myDiagram.Model.SharedData = new SharedData();
      }
      LoadDiagramProperties();
    }

    private void SaveDiagramProperties() {
      (myDiagram.Model.SharedData as SharedData).Position = Point.Stringify(myDiagram.Position);
    }

    private void LoadDiagramProperties() {
      // set Diagram.InitialPosition, not Diagram.Position, to handle initialization side-effects
      var pos = (myDiagram.Model.SharedData as SharedData).Position;
      if (pos != null) myDiagram.InitialPosition = Point.Parse(pos);
    }

    // define the model data
    public class Model : GraphLinksModel<NodeData, string, SharedData, LinkData, string, string> { }
    public class NodeData : Model.NodeData {
      public string Figure { get; set; }
      public string Fill { get; set; }
      public string Loc { get; set; }
      public double? Angle { get; set; }
      public string Size { get; set; }
    }

    public class LinkData : Model.LinkData {
      public List<Point> Points { get; set; }
    }

    public class SharedData {
      public string Position { get; set; }
    }
   }
}
