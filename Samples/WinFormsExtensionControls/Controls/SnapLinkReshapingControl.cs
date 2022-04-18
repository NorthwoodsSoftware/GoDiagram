/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using System.ComponentModel;
using Northwoods.Go.Extensions;
using Northwoods.Go.WinForms;
using Northwoods.Go.Tools.Extensions;
using System.Linq;

namespace WinFormsExtensionControls.SnapLinkReshaping {
  [ToolboxItem(false)]
  public partial class SnapLinkReshapingControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    private Palette myPalette;
    private Dictionary<string, Part> mySharedNodeTemplateMap;
    public SnapLinkReshapingControl() {
      InitializeComponent();

      saveLoadModel1.SaveClick += (e, obj) => SaveModel();
      saveLoadModel1.LoadClick += (e, obj) => LoadModel();

      avoidsNodes.CheckedChanged += (e, obj) => (myDiagram.ToolManager.LinkReshapingTool as SnapLinkReshapingTool).AvoidsNodes = avoidsNodes.Checked;

      goWebBrowser2.Html = @"
          <p>
           This sample is a simplified version of the <a href=""DraggableLink"">Draggable Link</a> sample.
           Links are not draggable, there are no custom <a>Adornment</a>s, nodes are not rotatable, and links
           do not have text labels.
          </p>
          <p>
           Its purpose is to demonstrate the <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/SnapLinkReshaping/SnapLinkReshapingTool.cs"">SnapLinkReshapingTool</a>,
           an extension of <a>LinkReshapingTool</a> that snaps each dragged reshape handle of selected Links to
           the nearest grid point.If the <a>SnapLinkReshapingTool.AvoidsNodes</a> option is true,
           as it is by default, then the reshaping is limited to points where the adjacent segments would not
           be crossing over any avoidable nodes.
          </p>
          <p>
           Note how the ""LinkReshaped"" DiagramEvent listener changes the <a>Link.Routing</a> of the reshaped Link,
           so that it is no longer AvoidsNodes routing but simple Orthogonal routing.
           This combined with <a>Link.Adjusting</a> being End permits the middle points of the link route to be
           retained even after the user moves or resizes nodes.
           Furthermore there is a TwoWay <a>Binding</a> on <a>Link.Routing</a>, so that the model remembers
           whether the link route had ever been reshaped manually.
          </p>
";

      saveLoadModel1.ModelJson = @"{
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
      if (mySharedNodeTemplateMap != null) return;  // already defined
      DefineFileFigure();
      // load extra figures
      Figures.DefineExtraFigures();

      var textStyle = new {
        Font = new Font("Lato", 11, FontWeight.Bold, FontUnit.Point),
        Stroke = "#F8F8F8"
      };

      mySharedNodeTemplateMap = new Dictionary<string, Part> {
        {
          "",
          new Node(PanelLayoutTable.Instance) {
            LocationSpot = Spot.Center
          }
          .Bind(new Binding("Location", "Loc", Point.Parse, Point.Stringify))
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
          .Bind(new Binding("Location", "Loc", Point.Parse, Point.Stringify))
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
          .Bind(new Binding("Location", "Loc", Point.Parse, Point.Stringify))
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
          .Bind(new Binding("Location", "Loc", Point.Parse, Point.Stringify))
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
      myDiagram = diagramControl1.Diagram;

      DefineNodeTemplates();
      myDiagram.NodeTemplateMap = mySharedNodeTemplateMap;

      myDiagram.ToolManager.DraggingTool.IsGridSnapEnabled = true;
      myDiagram.AnimationManager.IsEnabled = false;
      myDiagram.UndoManager.IsEnabled = true;
      myDiagram.LinkReshaped += (obj, e) => (e.Subject as Link).Routing = LinkRouting.Orthogonal;
      myDiagram.ToolManager.LinkReshapingTool = new SnapLinkReshapingTool();


      myDiagram.Grid = new Panel("Grid") {
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

      myDiagram.NodeTemplate = new Node(PanelLayoutSpot.Instance) {
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
          Font = new Font("Arial", 11, FontWeight.Bold),
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

      myDiagram.LinkTemplate = new Link {
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

      var link = myDiagram.Links.FirstOrDefault();
      if (link != null) link.IsSelected = true;
    }

    private void SetupPalette() {

      myPalette = paletteControl1.Diagram as Palette;

      myPalette.MaxSelectionCount = 1;
      DefineNodeTemplates();
      myPalette.NodeTemplateMap = mySharedNodeTemplateMap;

      myPalette.Model = new Model {
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
      if (myDiagram == null) return;
      (myDiagram.Model.SharedData as SharedData).Position = Point.Stringify(myDiagram.Position);
      saveLoadModel1.ModelJson = myDiagram.Model.ToJson();
    }
    private void LoadModel() {
      if (myDiagram == null) return;
      myDiagram.Model = Model.FromJson<Model>(saveLoadModel1.ModelJson);
      myDiagram.Model.UndoManager.IsEnabled = true;
      if (!(myDiagram.Model.SharedData is SharedData)) {
        myDiagram.Model.SharedData = new SharedData();
        var pos = (myDiagram.Model.SharedData as SharedData).Position;
        if (pos != null) myDiagram.InitialPosition = Point.Parse(pos);
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

