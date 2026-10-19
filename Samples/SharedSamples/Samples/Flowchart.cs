/* Copyright (c) Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace Demo.Samples.Flowchart; 
public partial class Flowchart : DemoControl {
  private Diagram _Diagram;
  private Palette _Palette;

  private Dictionary<string, Part> _SharedNodeTemplateMap;

  public Flowchart() {
    InitializeComponent();

    desc1.MdText = DescriptionReader.Read("Samples.Flowchart.md");

    modelJson1.JsonText = @"{
  ""linkFromPortIdProperty"": ""FromPort"",
  ""linkToPortIdProperty"": ""ToPort"",
  ""nodeDataSource"": [
    {""category"":""Comment"", ""loc"":""360 -10"", ""text"":""Kookie Brittle"", ""key"":-13},
    {""key"":-1, ""category"":""Start"", ""loc"":""175 0"", ""text"":""Start""},
    {""key"":10, ""loc"":""-5 75"", ""text"":""Preheat oven to 375 F""},
    {""key"":1, ""loc"":""175 100"", ""text"":""In a bowl, blend: 1 cup margarine, 1.5 teaspoon vanilla, 1 teaspoon salt""},
    {""key"":2, ""loc"":""175 200"", ""text"":""Gradually beat in 1 cup sugar and 2 cups sifted flour""},
    {""key"":3, ""loc"":""175 290"", ""text"":""Mix in 6 oz (1 cup) Nestle's Semi-Sweet Chocolate Morsels""},
    {""key"":4, ""loc"":""175 380"", ""text"":""Press evenly into ungreased 15x10x1 pan""},
    {""key"":5, ""loc"":""355 85"", ""text"":""Finely chop 1/2 cup of your choice of nuts""},
    {""key"":6, ""loc"":""175 450"", ""text"":""Sprinkle nuts on top""},
    {""key"":7, ""loc"":""175 515"", ""text"":""Bake for 25 minutes and let cool""},
    {""key"":8, ""loc"":""175 585"", ""text"":""Cut into rectangular grid""},
    {""key"":-2, ""category"":""End"", ""loc"":""175 660"", ""text"":""Enjoy!""}
  ],
  ""linkDataSource"": [
    {""from"":1, ""to"":2, ""fromPort"":""B"", ""toPort"":""T""},
    {""from"":2, ""to"":3, ""fromPort"":""B"", ""toPort"":""T""},
    {""from"":3, ""to"":4, ""fromPort"":""B"", ""toPort"":""T""},
    {""from"":4, ""to"":6, ""fromPort"":""B"", ""toPort"":""T""},
    {""from"":6, ""to"":7, ""fromPort"":""B"", ""toPort"":""T""},
    {""from"":7, ""to"":8, ""fromPort"":""B"", ""toPort"":""T""},
    {""from"":8, ""to"":-2, ""fromPort"":""B"", ""toPort"":""T""},
    {""from"":-1, ""to"":10, ""fromPort"":""B"", ""toPort"":""T""},
    {""from"":-1, ""to"":1, ""fromPort"":""B"", ""toPort"":""T""},
    {""from"":-1, ""to"":5, ""fromPort"":""B"", ""toPort"":""T""},
    {""from"":5, ""to"":4, ""fromPort"":""B"", ""toPort"":""T""},
    {""from"":10, ""to"":4, ""fromPort"":""B"", ""toPort"":""T""}
  ]
}";
    modelJson1.SaveClick = SaveModel;
    modelJson1.LoadClick = LoadModel;

    // load palette and diagram at the same time, after the page is ready
    AfterLoad(() => {
      Setup();
      SetupPalette();
    });
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
      node.BindTwoWay("Location", "Loc", Point.Parse, Point.Stringify);
    }

    var textStyle = new {
      Font = new Font("Segoe UI", 11, Northwoods.Go.FontWeight.Bold, FontUnit.Point),
      Stroke = "#F8F8F8"
    };

    _SharedNodeTemplateMap = new Dictionary<string, Part> {
      {
        "",
        new Node(PanelType.Table) {
          LocationSpot = Spot.Center
        }
        .Apply(nodeStyle)
        .Add(
          new Panel(PanelType.Auto).Add(
            new Shape("Rectangle") {
              Fill = "#282C34", Stroke = "#00A9C9", StrokeWidth = 3.5
            }.Bind("Figure"),
            new TextBlock {
              Margin = 8,
              MaxSize = new Size(160, double.NaN),
              Wrap = TextWrap.Fit,
              Editable = true
            }.Set(textStyle).BindTwoWay("Text")
          ),
          MakePort("T", Spot.Top, Spot.TopSide, false, true),
          MakePort("L", Spot.Left, Spot.LeftSide, true, true),
          MakePort("R", Spot.Right, Spot.RightSide, true, true),
          MakePort("B", Spot.Bottom, Spot.BottomSide, true, false)
        )
      },
      {
        "Conditional",
        new Node(PanelType.Table) {
          LocationSpot = Spot.Center
        }
        .Apply(nodeStyle)
        .Add(
          new Panel(PanelType.Auto).Add(
            new Shape("Diamond") {
              Fill = "#282C34", Stroke = "#00A9C9", StrokeWidth = 3.5
            }.Bind("Figure"),
            new TextBlock {
              Margin = 8,
              MaxSize = new Size(160, double.NaN),
              Wrap = TextWrap.Fit,
              Editable = true
            }.Set(textStyle).BindTwoWay("Text")
          ),
          MakePort("T", Spot.Top, Spot.TopSide, false, true),
          MakePort("L", Spot.Left, Spot.LeftSide, true, true),
          MakePort("R", Spot.Right, Spot.RightSide, true, true),
          MakePort("B", Spot.Bottom, Spot.BottomSide, true, false)
        )
      },
      {
        "Start",
        new Node(PanelType.Table) {
          LocationSpot = Spot.Center
        }
        .Apply(nodeStyle)
        .Add(
          new Panel(PanelType.Auto).Add(
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
        new Node(PanelType.Table) {
          LocationSpot = Spot.Center
        }
        .Apply(nodeStyle)
        .Add(
          new Panel(PanelType.Auto).Add(
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
        new Node(PanelType.Auto) {
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
            Wrap = TextWrap.Fit,
            TextAlign = TextAlign.Center,
            Editable = true
          }.Set(textStyle).BindTwoWay("Text")
          // no ports, since links are not allowed to connect with a comment
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
    .BindTwoWay("Points")
    .Add(new Shape { // The highlight shape, normally transparent
      IsPanelMain = true, StrokeWidth = 8, Stroke = "transparent", Name = "HIGHLIGHT"
    },
      new Shape { // the link path shape
        IsPanelMain = true, Stroke = "gray", StrokeWidth = 2
      }.Bind(new Binding("Stroke", "IsSelected", (sel, _) => { return (bool)sel ? "dodgerblue" : "gray"; }).OfElement()),
      new Shape { // the arrowhead
        ToArrow = "standard", StrokeWidth = 0, Fill = "gray"
      },
      new Panel(PanelType.Auto) { // the LinkLabel, normally not visible
        Visible = false, Name = "LABEL", SegmentIndex = 2, SegmentFraction = 0.5
      }
      .BindTwoWay("Visible")
      .Add(
        new Shape("RoundedRectangle") {  // the label shape
          Fill = "#F8F8F8", StrokeWidth = 0
        },
        new TextBlock("Yes") {  // the label
          TextAlign = TextAlign.Center,
          Font = new Font("Arial", 10, FontUnit.Point),
          Stroke = "#333333",
          Editable = true
        }.BindTwoWay("Text")
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
    modelJson1.JsonText = _Diagram.Model.ToJson();
  }

  private void LoadModel() {
    if (_Diagram == null) return;
    _Diagram.Model = Model.FromJson<Model>(modelJson1.JsonText);
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
