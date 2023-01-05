/* Copyright 1998-2023 by Northwoods Software Corporation. */

using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace Demo.Samples.SequentialFunction {
  public partial class SequentialFunction : DemoControl {
    private Diagram _Diagram;

    public SequentialFunction() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      modelJson1.SaveClick = SaveModel;
      modelJson1.LoadClick = LoadModel;

      desc1.MdText = DescriptionReader.Read("Samples.SequentialFunction.md");

      modelJson1.JsonText = @"{
  ""NodeDataSource"": [
    { ""Key"": ""S1"", ""Category"": ""step"", ""Text"": ""Step 1"" },
    { ""Key"": ""TR1"", ""Category"": ""transition"", ""Text"": ""Transition 1"" },
    { ""Key"": ""S2"", ""Category"": ""step"", ""Text"": ""Step 2"" },
    { ""Key"": ""TR2"", ""Category"": ""transition"", ""Text"": ""Transition 2"" },
    { ""Key"": ""BAR1"", ""Category"": ""parallel"" },
    { ""Key"": ""S3"", ""Category"": ""step"", ""Text"": ""Step 3"" },
    { ""Key"": ""S4"", ""Category"": ""step"", ""Text"": ""Step 4"" },
    { ""Key"": ""BAR2"", ""Category"": ""parallel"" },
    { ""Key"": ""TR3"", ""Category"": ""transition"", ""Text"": ""Transition 3"" },
    { ""Key"": ""S5"", ""Category"": ""step"", ""Text"": ""Step 5"" }
  ],
  ""LinkDataSource"": [
    { ""From"": ""S1"", ""To"": ""TR1"" },
    { ""From"": ""TR1"", ""To"": ""S2"" },
    { ""From"": ""S2"", ""To"": ""TR2"" },
    { ""From"": ""TR2"", ""To"": ""BAR1"" },
    { ""From"": ""BAR1"", ""To"": ""S3"" },
    { ""From"": ""BAR1"", ""To"": ""S4"" },
    { ""From"": ""S3"", ""To"": ""BAR2"" },
    { ""From"": ""S4"", ""To"": ""BAR2"" },
    { ""From"": ""BAR2"", ""To"": ""TR3"" },
    { ""From"": ""TR3"", ""To"": ""S5"" }
  ]
}";

      Setup();
    }

    private void Setup() {
      // diagram properties
      _Diagram.Layout = new LayeredDigraphLayout {
        Direction = 90,
        LayerSpacing = 10,
        SetsPortSpots = false
      };
      _Diagram.UndoManager.IsEnabled = true; // enable undo and redo

      // define the step node template
      _Diagram.NodeTemplateMap.Add("step",
        new Node("Spot") {
          LocationSpot = Spot.Center
        }
          .Bind("Location", "Loc", Point.Parse, Point.Stringify)
          .Add(
            new Shape("Rectangle") {
              Fill = "whitesmoke",
              Stroke = "gray",
              StrokeWidth = 2,
              DesiredSize = new Size(160, 60),
              PortId = "",  // so that links connect to the Shape, not to the whole Node
              FromSpot = Spot.BottomSide,
              ToSpot = Spot.TopSide,
              Alignment = Spot.Center
            },
            new TextBlock {
              Font = new Font("Segoe UI", 16, Northwoods.Go.FontWeight.Bold),
              Alignment = Spot.Center,
              Wrap = Wrap.Fit,
              Editable = true
            }
              .Bind(new Binding("Text").MakeTwoWay())
          )
      );

      // define the transition node template
      _Diagram.NodeTemplateMap.Add("transition",
        new Node("Horizontal") {
          LocationSpot = Spot.Center, LocationElementName = "BAR"
        }
          .Bind("Location", "Loc", Point.Parse, Point.Stringify)
          .Add(
            new Shape("Rectangle") {
              Name = "BAR",
              Fill = "black",
              Stroke = null,
              DesiredSize = new Size(60, 8),
              PortId = "",
              FromSpot = Spot.BottomSide,
              ToSpot = Spot.TopSide
            },
            new TextBlock {
              Editable = true, Margin = 3
            }
              .Bind(new Binding("Text").MakeTwoWay())
          )
      );

      // define the parallel node template
      _Diagram.NodeTemplateMap.Add("parallel",
        new Node {
          LocationSpot = Spot.Center
        }
          .Bind("Location", "Loc", Point.Parse, Point.Stringify)
          .Add(
            new Shape("Rectangle") {
              Fill = "whitesmoke",
              Stroke = "black",
              DesiredSize = new Size(200, 6),
              PortId = "",
              FromSpot = Spot.BottomSide,
              ToSpot = Spot.TopSide
            }
          )
      );

      // link template
      _Diagram.LinkTemplate =
        new Link { Routing = LinkRouting.Orthogonal }
          .Add(new Shape { Stroke = "black", StrokeWidth = 2 });

      LoadModel();
    }

    private void SaveModel() {
      if (_Diagram == null) return;
      modelJson1.JsonText = _Diagram.Model.ToJson();
    }

    private void LoadModel() {
      if (_Diagram == null) return;
      _Diagram.Model = Model.FromJson<Model>(modelJson1.JsonText);
      _Diagram.Model.UndoManager.IsEnabled = true;
    }
  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Loc { get; set; }
  }

  public class LinkData : Model.LinkData { }
}
