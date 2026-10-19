/* Copyright (c) Northwoods Software Corporation. */

using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace Demo.Samples.SequentialFunction; 
public partial class SequentialFunction : DemoControl {
  private Diagram _Diagram;

  public SequentialFunction() {
    InitializeComponent();
    _Diagram = diagramControl1.Diagram;

    modelJson1.SaveClick = SaveModel;
    modelJson1.LoadClick = LoadModel;

    desc1.MdText = DescriptionReader.Read("Samples.SequentialFunction.md");

    modelJson1.JsonText = @"{
  ""nodeDataSource"": [
    { ""key"": ""S1"", ""category"": ""step"", ""text"": ""Step 1"" },
    { ""key"": ""TR1"", ""category"": ""transition"", ""text"": ""Transition 1"" },
    { ""key"": ""S2"", ""category"": ""step"", ""text"": ""Step 2"" },
    { ""key"": ""TR2"", ""category"": ""transition"", ""text"": ""Transition 2"" },
    { ""key"": ""BAR1"", ""category"": ""parallel"" },
    { ""key"": ""S3"", ""category"": ""step"", ""text"": ""Step 3"" },
    { ""key"": ""S4"", ""category"": ""step"", ""text"": ""Step 4"" },
    { ""key"": ""BAR2"", ""category"": ""parallel"" },
    { ""key"": ""TR3"", ""category"": ""transition"", ""text"": ""Transition 3"" },
    { ""key"": ""S5"", ""category"": ""step"", ""text"": ""Step 5"" }
  ],
  ""linkDataSource"": [
    { ""from"": ""S1"", ""to"": ""TR1"" },
    { ""from"": ""TR1"", ""to"": ""S2"" },
    { ""from"": ""S2"", ""to"": ""TR2"" },
    { ""from"": ""TR2"", ""to"": ""BAR1"" },
    { ""from"": ""BAR1"", ""to"": ""S3"" },
    { ""from"": ""BAR1"", ""to"": ""S4"" },
    { ""from"": ""S3"", ""to"": ""BAR2"" },
    { ""from"": ""S4"", ""to"": ""BAR2"" },
    { ""from"": ""BAR2"", ""to"": ""TR3"" },
    { ""from"": ""TR3"", ""to"": ""S5"" }
  ]
}";

    Setup();
  }

  private void Setup() {
    // diagram properties
    _Diagram.Layout = new LayeredDigraphLayout {
      Direction = 90,
      LayerSpacing = 10,
      AlignOption = LayeredDigraphAlign.All,
      SetsPortSpots = false
    };
    _Diagram.UndoManager.IsEnabled = true; // enable undo and redo

    // define the step node template
    _Diagram.NodeTemplateMap.Add("step",
      new Node("Spot") {
        LocationSpot = Spot.Center
      }
        .BindTwoWay("Location", "Loc", Point.Parse, Point.Stringify)
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
            Wrap = TextWrap.Fit,
            Editable = true
          }
            .BindTwoWay("Text")
        )
    );

    // define the transition node template
    _Diagram.NodeTemplateMap.Add("transition",
      new Node("Horizontal") {
        LocationSpot = Spot.Center, LocationElementName = "BAR"
      }
        .BindTwoWay("Location", "Loc", Point.Parse, Point.Stringify)
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
            .BindTwoWay("Text")
        )
    );

    // define the parallel node template
    _Diagram.NodeTemplateMap.Add("parallel",
      new Node {
        LocationSpot = Spot.Center
      }
        .BindTwoWay("Location", "Loc", Point.Parse, Point.Stringify)
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
