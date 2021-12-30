using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.WinForms;

namespace WinFormsSampleControls.Flowchart {
  [ToolboxItem(false)]
  public partial class FlowchartControl : System.Windows.Forms.UserControl {
    private Diagram _Diagram;
    private Palette _Palette;

    private Dictionary<string, Part> _SharedNodeTemplateMap;

    public FlowchartControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;
      paletteControl1.AfterRender = SetupPalette;
      goWebBrowser1.Html = @"
        <p>
          The FlowChart sample demonstrates several key features of GoDiagram,
          namely <a href=""intro/palette.html"">Palette</a>s,
          <a href=""intro/links.html"">Linkable nodes</a>, Drag/Drop behavior,
          <a href=""intro/textBlocks.html"">Text Editing</a>, and the use of
          <a href=""intro/templateMaps.html"">Node Template Maps</a> in Diagrams.
        </p>
        <p>
          Mouse-over a Node to view its ports.
          Drag from these ports to create new Links.
          Selecting Links allows you to re-shape and re-link them.
          Selecting a Node and then clicking its TextBlock will allow
          you to edit text (except on the Start and End Nodes).
        </p>
      ";

      saveLoadModel1.ModelJson = @"{
  ""LinkFromPortIdProperty"": ""FromPort"",
  ""LinkToPortIdProperty"": ""ToPort"",
  ""NodeDataSource"": [
    {""Category"":""Comment"", ""Loc"":""360 -10"", ""Text"":""Kookie Brittle"", ""Key"":-13},
    {""Key"":-1, ""Category"":""Start"", ""Loc"":""175 0"", ""Text"":""Start""},
    {""Key"":10, ""Loc"":""-5 75"", ""Text"":""Preheat oven to 375 F""},
    {""Key"":1, ""Loc"":""175 100"", ""Text"":""In a bowl, blend: 1 cup margarine, 1.5 teaspoon vanilla, 1 teaspoon salt""},
    {""Key"":2, ""Loc"":""175 200"", ""Text"":""Gradually beat in 1 cup sugar and 2 cups sifted flour""},
    {""Key"":3, ""Loc"":""175 290"", ""Text"":""Mix in 6 oz (1 cup) Nestle's Semi-Sweet Chocolate Morsels""},
    {""Key"":4, ""Loc"":""175 380"", ""Text"":""Press evenly into ungreased 15x10x1 pan""},
    {""Key"":5, ""Loc"":""355 85"", ""Text"":""Finely chop 1/2 cup of your choice of nuts""},
    {""Key"":6, ""Loc"":""175 450"", ""Text"":""Sprinkle nuts on top""},
    {""Key"":7, ""Loc"":""175 515"", ""Text"":""Bake for 25 minutes and let cool""},
    {""Key"":8, ""Loc"":""175 585"", ""Text"":""Cut into rectangular grid""},
    {""Key"":-2, ""Category"":""End"", ""Loc"":""175 660"", ""Text"":""Enjoy!""}
  ],
  ""LinkDataSource"": [
    {""From"":1, ""To"":2, ""FromPort"":""B"", ""ToPort"":""T""},
    {""From"":2, ""To"":3, ""FromPort"":""B"", ""ToPort"":""T""},
    {""From"":3, ""To"":4, ""FromPort"":""B"", ""ToPort"":""T""},
    {""From"":4, ""To"":6, ""FromPort"":""B"", ""ToPort"":""T""},
    {""From"":6, ""To"":7, ""FromPort"":""B"", ""ToPort"":""T""},
    {""From"":7, ""To"":8, ""FromPort"":""B"", ""ToPort"":""T""},
    {""From"":8, ""To"":-2, ""FromPort"":""B"", ""ToPort"":""T""},
    {""From"":-1, ""To"":10, ""FromPort"":""B"", ""ToPort"":""T""},
    {""From"":-1, ""To"":1, ""FromPort"":""B"", ""ToPort"":""T""},
    {""From"":-1, ""To"":5, ""FromPort"":""B"", ""ToPort"":""T""},
    {""From"":5, ""To"":4, ""FromPort"":""B"", ""ToPort"":""T""},
    {""From"":10, ""To"":4, ""FromPort"":""B"", ""ToPort"":""T""}
  ]
}";
      saveLoadModel1.SaveClick += (obj, e) => SaveModel();
      saveLoadModel1.LoadClick += (obj, e) => LoadModel();
    }

    // Define a function for creating a "port" that is normally transparent.
    // The "name" is used as the GraphObject.portId,
    // the "align" is used to determine where to position the port relative to the body of the node,
    // the "spot" is used to control how links connect with the port and whether the port
    // stretches along the side of the node,
    // and the boolean "output" and "input" arguments control whether the user can draw links from or to the port.
    private GraphObject MakePort(string name, Spot align, Spot spot, bool output, bool input) {
      var horizontal = align.Equals(Spot.Top) || align.Equals(Spot.Bottom);
      // the port is basically just a transparent rectangle that stretches along the side of the node,
      // and becomes colored when the mouse passes over it
      return new Shape {
        Fill = "transparent",
        StrokeWidth = 0,
        Width = horizontal ? double.NaN : 8,
        Height = !horizontal ? double.NaN : 8,
        Alignment = align,
        Stretch = horizontal ? Stretch.Horizontal : Stretch.Vertical,
        PortId = name,
        FromSpot = spot,
        FromLinkable = output,
        ToSpot = spot,
        ToLinkable = input,
        Cursor = "pointer",
        // here PORT is this shape
        MouseEnter = (e, port, last) => {
          if (!e.Diagram.IsReadOnly) (port as Shape).Fill = "rgba(255,0,255,0.5)";
        },
        MouseLeave = (e, port, next) => {
          (port as Shape).Fill = "transparent";
        }
      };
    }

    private void DefineNodeTemplates() {
      if (_SharedNodeTemplateMap != null) return;  // already defined
      DefineFileFigure();

      void nodeStyle(Node node) {
        node.Bind("Location", "Loc", Point.Parse, Point.Stringify);
      }

      var textStyle = new {
        Font = "Lato, 11pt, style=bold",
        Stroke = "#F8F8F8"
      };

      _SharedNodeTemplateMap = new Dictionary<string, Part> {
        {
          "",
          new Node(PanelLayoutTable.Instance) {
            LocationSpot = Spot.Center
          }
          .Apply(nodeStyle)
          .Add(
            new Panel(PanelLayoutAuto.Instance).Add(
              new Shape("Rectangle") {
                Fill = "#282C34", Stroke = "#00A9C9", StrokeWidth = 3.5
              }.Bind("Figure"),
              new TextBlock {
                Margin = 8,
                MaxSize = new Size(160, double.NaN),
                Wrap = Wrap.Fit,
                Editable = true
              }.Set(textStyle).Bind(
                new Binding("Text").MakeTwoWay()
              )
            ),
            MakePort("T", Spot.Top, Spot.TopSide, false, true),
            MakePort("L", Spot.Left, Spot.LeftSide, true, true),
            MakePort("R", Spot.Right, Spot.RightSide, true, true),
            MakePort("B", Spot.Bottom, Spot.BottomSide, true, false)
          )
        },
        {
          "Conditional",
          new Node(PanelLayoutTable.Instance) {
            LocationSpot = Spot.Center
          }
          .Apply(nodeStyle)
          .Add(
            new Panel(PanelLayoutAuto.Instance).Add(
              new Shape("Diamond") {
                Fill = "#282C34", Stroke = "#00A9C9", StrokeWidth = 3.5
              }.Bind("Figure"),
              new TextBlock {
                Margin = 8,
                MaxSize = new Size(160, double.NaN),
                Wrap = Wrap.Fit,
                Editable = true
              }.Set(textStyle).Bind(
                new Binding("Text").MakeTwoWay()
              )
            ),
            MakePort("T", Spot.Top, Spot.TopSide, false, true),
            MakePort("L", Spot.Left, Spot.LeftSide, true, true),
            MakePort("R", Spot.Right, Spot.RightSide, true, true),
            MakePort("B", Spot.Bottom, Spot.BottomSide, true, false)
          )
        },
        {
          "Start",
          new Node(PanelLayoutTable.Instance) {
            LocationSpot = Spot.Center
          }
          .Apply(nodeStyle)
          .Add(
            new Panel(PanelLayoutAuto.Instance).Add(
              new Shape("Circle") {
                MinSize = new Size(70, 70),
                Fill = "#282C34", Stroke = "#09D3AC",
                StrokeWidth = 3.5
              },
              new TextBlock("Start")
                .Set(textStyle).Bind("Text")
            ),
            MakePort("L", Spot.Left, Spot.Left, true, false),
            MakePort("R", Spot.Right, Spot.Right, true, false),
            MakePort("B", Spot.Bottom, Spot.Bottom, true, false)
          )
        },
        {
          "End",
          new Node(PanelLayoutTable.Instance) {
            LocationSpot = Spot.Center
          }
          .Apply(nodeStyle)
          .Add(
            new Panel(PanelLayoutAuto.Instance).Add(
              new Shape("Circle") {
                MinSize = new Size(60, 60),
                Fill = "#282C34", Stroke = "#DC3C00",
                StrokeWidth = 3.5
              },
              new TextBlock("End")
                .Set(textStyle).Bind("Text")
            ),
            MakePort("T", Spot.Top, Spot.Top, false, true),
            MakePort("L", Spot.Left, Spot.Left, false, true),
            MakePort("R", Spot.Right, Spot.Right, false, true)
          )
        },
        {
          "Comment",
          new Node(PanelLayoutAuto.Instance) {
            LocationSpot = Spot.Center
          }
          .Apply(nodeStyle)
          .Add(
            new Shape("File") {
              Fill = "#282C34", Stroke = "#DEE0A3", StrokeWidth = 3.5
            },
            new TextBlock {
              Margin = 5,
              MaxSize = new Size(200, double.NaN),
              Wrap = Wrap.Fit,
              TextAlign = TextAlign.Center,
              Editable = true
            }.Set(textStyle).Bind(
              new Binding("Text").MakeTwoWay()
            ) // no ports, since links are not allowed to connect with a comment
          )
        }
      };
    }

    private void DefineFileFigure() {
      if (Shape.GetFigureGenerators().ContainsKey("File")) return;
      // taken from ../extensions/Figures.cs:
      Shape.DefineFigureGenerator("File", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0, true); // starting point
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Line, .75 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, .25 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h).Close());
        var fig2 = new PathFigure(.75 * w, 0, false);
        geo.Add(fig2);
        // The Fold
        fig2.Add(new PathSegment(SegmentType.Line, .75 * w, .25 * h));
        fig2.Add(new PathSegment(SegmentType.Line, w, .25 * h));
        geo.Spot1 = new Spot(0, .25);
        geo.Spot2 = Spot.BottomRight;
        return geo;
      });
    }


    private void Setup() {
      _Diagram = diagramControl1.Diagram;

      _Diagram.LinkDrawn += showLinkLabel;
      _Diagram.LinkRelinked += showLinkLabel;
      _Diagram.UndoManager.IsEnabled = true;

      DefineNodeTemplates();
      _Diagram.NodeTemplateMap = _SharedNodeTemplateMap;

      _Diagram.LinkTemplate = new Link {
        Routing = LinkRouting.AvoidsNodes,
        Curve = LinkCurve.JumpOver,
        Corner = 5,
        ToShortLength = 4,
        RelinkableFrom = true,
        RelinkableTo = true,
        Reshapable = true,
        Resegmentable = true,
        // mouseovers subtly highlight links:
        MouseEnter = (e, link, last) => {
          ((link as Link).FindElement("HIGHLIGHT") as Shape).Stroke = "rgba(30,144,255,0.2)";
        },
        MouseLeave = (e, link, next) => {
          ((link as Link).FindElement("HIGHLIGHT") as Shape).Stroke = "transparent";
        },
        SelectionAdorned = false
      }
      .Bind(new Binding("Points").MakeTwoWay())
      .Add(new Shape { // The highlight shape, normally transparent
        IsPanelMain = true, StrokeWidth = 8, Stroke = "transparent", Name = "HIGHLIGHT"
      },
        new Shape { // the link path shape
          IsPanelMain = true, Stroke = "gray", StrokeWidth = 2
        }.Bind(new Binding("Stroke", "IsSelected", (sel, _) => { return (bool)sel ? "dodgerblue" : "gray"; }).OfElement()),
        new Shape { // the arrowhead
          ToArrow = "standard", StrokeWidth = 0, Fill = "gray"
        },
        new Panel(PanelLayoutAuto.Instance) { // the LinkLabel, normally not visible
          Visible = false, Name = "LABEL", SegmentIndex = 2, SegmentFraction = 0.5
        }
        .Bind(new Binding("Visible", "Visible").MakeTwoWay())
        .Add(
          new Shape("RoundedRectangle") {  // the label shape
            Fill = "#F8F8F8", StrokeWidth = 0
          },
          new TextBlock("Yes") {  // the label
            TextAlign = TextAlign.Center,
            Font = "Arial, 10pt",
            Stroke = "#333333",
            Editable = true
          }.Bind(new Binding("Text").MakeTwoWay())
        )
      );

      void showLinkLabel(object s, DiagramEvent e) {
        var label = e.Subject;
        e.Diagram.Layout.InvalidateLayout();
      }

      _Diagram.ToolManager.LinkingTool.TemporaryLink.Routing = LinkRouting.Orthogonal;
      _Diagram.ToolManager.RelinkingTool.TemporaryLink.Routing = LinkRouting.Orthogonal;

      LoadModel();
    }

    private void SetupPalette() {
      _Palette = paletteControl1.Diagram as Palette;

      DefineNodeTemplates();
      _Palette.NodeTemplateMap = _SharedNodeTemplateMap;
      _Palette.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Category = "Start", Text = "Start" },
          new NodeData { Text = "Step" },
          new NodeData { Category = "Conditional", Text = "???" },
          new NodeData { Category = "End", Text = "End" },
          new NodeData { Category = "Comment", Text = "Comment" }
        }
      };
    }

    private void SaveModel() {
      if (_Diagram == null) return;
      saveLoadModel1.ModelJson = _Diagram.Model.ToJson();
    }

    private void LoadModel() {
      if (_Diagram == null) return;
      _Diagram.Model = Model.FromJson<Model>(saveLoadModel1.ModelJson);
    }
    
  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }

  public class NodeData : Model.NodeData {
    public string Loc { get; set; }
    public string Figure { get; set; }
  }

  public class LinkData : Model.LinkData {
    public IEnumerable<Point> Points { get; set; }
  }
}
