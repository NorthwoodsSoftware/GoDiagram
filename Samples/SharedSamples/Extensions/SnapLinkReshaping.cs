/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Extensions;
using Northwoods.Go.Tools.Extensions;
using System.Linq;

namespace Demo.Extensions.SnapLinkReshaping {
  public partial class SnapLinkReshaping : DemoControl {
    private Diagram _Diagram;
    private Palette _Palette;
    private Dictionary<string, Part> _SharedNodeTemplateMap;

    public SnapLinkReshaping() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;
      _Palette = paletteControl1.Diagram as Palette;

      _InitCheckBoxes();

      desc1.MdText = DescriptionReader.Read("Extensions.SnapLinkReshaping.md");

      modelJson1.SaveClick = SaveModel;
      modelJson1.LoadClick = LoadModel;
      modelJson1.JsonText = @"{
  ""LinkFromPortIdProperty"": ""FromPort"",
  ""LinkToPortIdProperty"": ""ToPort"",
  ""NodeDataSource"": [
    { ""Text"":""DB"", ""Figure"":""Database"", ""Fill"":""lightgray"", ""Key"":-3, ""Loc"":""184 176""},
    { ""Text"":""DB"", ""Figure"":""Database"", ""Fill"":""lightgray"", ""Key"":-2, ""Loc"":""248 248""},
    { ""Text"":""DB"", ""Figure"":""Database"", ""Fill"":""lightgray"", ""Key"":-4, ""Loc"":""424 192""},
    { ""Text"":""DB"", ""Figure"":""Database"", ""Fill"":""lightgray"", ""Key"":-5, ""Loc"":""320 152""},
    { ""Text"":""DB"", ""Figure"":""Database"", ""Fill"":""lightgray"", ""Key"":-6, ""Loc"":""424 320""},
    { ""Text"":""DB"", ""Figure"":""Database"", ""Fill"":""lightgray"", ""Key"":-7, ""Loc"":""352 256""},
    { ""Text"":""DB"", ""Figure"":""Database"", ""Fill"":""lightgray"", ""Key"":-8, ""Loc"":""176 296""},
    { ""Text"":""DB"", ""Figure"":""Database"", ""Fill"":""lightgray"", ""Key"":-9, ""Loc"":""288 344""},
    { ""Text"":""Step"", ""Key"":-10, ""Loc"":""96 240""},
    { ""Text"":""Step"", ""Key"":-11, ""Loc"":""536 280""}
  ],
  ""LinkDataSource"": [
    { ""From"":-10, ""To"":-11, ""FromPort"":""R"", ""ToPort"":""L"", ""Routing"": 2,
      ""Points"": [
        ""118.60896682739258 240"",
        ""128.60896682739258 240"",
        ""132 240"",
        ""132 240"",
        ""216 240"",
        ""216 192"",
        ""264 192"",
        ""264 104"",
        ""392 104"",
        ""392 240"",
        ""472 240"",
        ""472 280"",
        ""503.3910331726074 280"",
        ""513.3910331726074 280""
      ] }
  ]
}";

      Setup();
      SetupPalette();
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
      // load extra figures
      Figures.DefineExtraFigures();

      var textStyle = new {
        Font = new Font("Lato", 11, Northwoods.Go.FontWeight.Bold, FontUnit.Point),
        Stroke = "#F8F8F8"
      };

      _SharedNodeTemplateMap = new Dictionary<string, Part> {
        {
          "",
          new Node(PanelType.Table) {
            LocationSpot = Spot.Center
          }
          .Bind(new Binding("Location", "Loc", Point.Parse, Point.Stringify))
          .Add(
            new Panel(PanelType.Auto).Add(
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
          new Node(PanelType.Table) {
            LocationSpot = Spot.Center
          }
          .Bind(new Binding("Location", "Loc", Point.Parse, Point.Stringify))
          .Add(
            new Panel(PanelType.Auto).Add(
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
          new Node(PanelType.Table) {
            LocationSpot = Spot.Center
          }
          .Bind(new Binding("Location", "Loc", Point.Parse, Point.Stringify))
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
          .Bind(new Binding("Location", "Loc", Point.Parse, Point.Stringify))
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
          .Bind(new Binding("Location", "Loc", Point.Parse, Point.Stringify))
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

    public void Setup() {
      DefineNodeTemplates();
      _Diagram.NodeTemplateMap = _SharedNodeTemplateMap;

      _Diagram.ToolManager.DraggingTool.IsGridSnapEnabled = true;
      _Diagram.AnimationManager.IsEnabled = false;
      _Diagram.UndoManager.IsEnabled = true;
      _Diagram.LinkReshaped += (obj, e) => (e.Subject as Link).Routing = LinkRouting.Orthogonal;
      _Diagram.ToolManager.LinkReshapingTool = new SnapLinkReshapingTool();


      _Diagram.Grid = new Panel("Grid") {
        GridCellSize = new Size(8, 8)
      }.Add(
        new Shape {
          Figure = "LineH",
          Stroke = "lightgray",
          StrokeWidth = 0.5
        }).Add(
          new Shape {
            Figure = "LineV",
            Stroke = "lightgray",
            StrokeWidth = 0.5
          });
      Shape MakePort(string name, Spot spot, bool output, bool input) {
        return new Shape {
          Figure = "Circle",
          Fill = null,
          Stroke = null,
          DesiredSize = new Size(7, 7),
          Alignment = spot,
          AlignmentFocus = spot,
          PortId = name,
          FromSpot = spot, ToSpot = spot,
          FromLinkable = output, ToLinkable = input,
          Cursor = "Pointer",
        };
      }

      _Diagram.NodeTemplate = new Node(PanelType.Spot) {
        LocationSpot = Spot.Center,
        Selectable = true,
        Resizable = true, ResizeElementName = "PANEL",
        MouseEnter = (e, node, _) => { ShowSmallPorts(node, true); },
        MouseLeave = (e, node, _) => { ShowSmallPorts(node, false); }
      }.Bind(new Binding("Location", "Loc", Point.Parse).MakeTwoWay(Point.Stringify))
      .Add(
        new Panel("Auto") {
          Name = "PANEL"
        }.Bind(new Binding("DesiredSize", "Size", Northwoods.Go.Size.Parse).MakeTwoWay(Northwoods.Go.Size.Stringify))
        .Add(new Shape("Rectangle") {
          PortId = "",
          FromLinkable = true, ToLinkable = true, Cursor = "Pointer",
          Fill = "white"
        }.Bind("Figure").Bind("Fill"),
        new TextBlock() {
          Font = new Font("Arial", 11, Northwoods.Go.FontWeight.Bold),
          Margin = 8,
          MaxSize = new Size(160, double.NaN),
          Wrap = Wrap.Fit,
          Editable = true
        }.Bind(
          new Binding("Text").MakeTwoWay()
        )),
        MakePort("T", Spot.Top, false, true),
        MakePort("L", Spot.Left, true, true),
        MakePort("R", Spot.Right, true, true),
        MakePort("B", Spot.Bottom, true, false)

        );

      void ShowSmallPorts(GraphObject node, bool show) {
        foreach (var port in (node as Node).Ports) {
          if (port.PortId != "") {  // don't change the default port, which is the big shape
            (port as Shape).Fill = show ? "rgba(0,0,0,0.3)" : (Brush)null;
          }
        }
      }

      _Diagram.LinkTemplate = new Link {
        RelinkableFrom = true,
        RelinkableTo = true,
        Reshapable = true,
        Resegmentable = true,
        Routing = LinkRouting.AvoidsNodes,
        Adjusting = LinkAdjusting.End,
        Curve = LinkCurve.JumpOver,
        Corner = 5,
        ToShortLength = 4
      }.Bind(new Binding("Points").MakeTwoWay())
       .Bind(new Binding("Routing").MakeTwoWay()).Add(
        new Shape {
          IsPanelMain = true,
          StrokeWidth = 2
        }).Add(
        new Shape {
          ToArrow = "Standard",
          Stroke = null
        });



      LoadModel();

      var link = _Diagram.Links.FirstOrDefault();
      if (link != null) link.IsSelected = true;
    }

    private void SetupPalette() {



      _Palette.MaxSelectionCount = 1;
      DefineNodeTemplates();
      _Palette.NodeTemplateMap = _SharedNodeTemplateMap;

      _Palette.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Text = "Start", Figure = "Circle", Fill = "green"},
          new NodeData { Text = "Step" },
          new NodeData { Text = "DB", Figure = "Database", Fill = "lightgray"},
          new NodeData { Text = "???", Figure = "Diamond", Fill = "lightskyblue"},
          new NodeData { Text = "End", Figure = "Circle", Fill = "red"},
          new NodeData { Text = "Comment", Figure = "RoundedRectangle", Fill = "lightyellow"},
        }
      };
    }

    private void SaveModel() {
      if (_Diagram == null) return;
      (_Diagram.Model.SharedData as SharedData).Position = Point.Stringify(_Diagram.Position);
      modelJson1.JsonText = _Diagram.Model.ToJson();
    }
    private void LoadModel() {
      if (_Diagram == null) return;
      _Diagram.Model = Model.FromJson<Model>(modelJson1.JsonText);
      _Diagram.Model.UndoManager.IsEnabled = true;
      if (!(_Diagram.Model.SharedData is SharedData)) {
        _Diagram.Model.SharedData = new SharedData();
        var pos = (_Diagram.Model.SharedData as SharedData).Position;
        if (pos != null) _Diagram.InitialPosition = Point.Parse(pos);
      }


    }
    public class Model : GraphLinksModel<NodeData, int, SharedData, LinkData, string, string> { }

    public class NodeData : Model.NodeData {
      public string Figure { get; set; }
      public string Fill { get; set; }

      public string Loc { get; set; }
    }

    public class SharedData {
      public string Position { get; set; }
    }

    public class LinkData : Model.LinkData {
      public List<Point> Points { get; set; }
      public LinkRouting Routing { get; set; }
    }

  }
}

