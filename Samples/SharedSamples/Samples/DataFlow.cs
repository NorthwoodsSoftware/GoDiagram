/* Copyright (c) Northwoods Software Corporation. */

using System;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace Demo.Samples.DataFlow; 
public partial class DataFlow : DemoControl {
  private Diagram _Diagram;

  public DataFlow() {
    InitializeComponent();
    _Diagram = diagramControl1.Diagram;

    modelJson1.SaveClick = SaveModel;
    modelJson1.LoadClick = LoadModel;

    desc1.MdText = DescriptionReader.Read("Samples.DataFlow.md");

    modelJson1.JsonText = @"{
  ""nodeCategoryProperty"": ""Type"",
  ""linkFromPortIdProperty"": ""Frompid"",
  ""linkToPortIdProperty"": ""Topid"",
  ""nodeDataSource"": [
    { ""key"": 1, ""type"": ""Table"", ""name"": ""Product"" },
    { ""key"": 2, ""type"": ""Table"", ""name"": ""Sales"" },
    { ""key"": 3, ""type"": ""Table"", ""name"": ""Period"" },
    { ""key"": 4, ""type"": ""Table"", ""name"": ""Store"" },
    { ""key"": 11, ""type"": ""Join"", ""name"": ""Product, Class"" },
    { ""key"": 12, ""type"": ""Join"", ""name"": ""Period"" },
    { ""key"": 13, ""type"": ""Join"", ""name"": ""Store"" },
    { ""key"": 21, ""type"": ""Project"", ""name"": ""Product, Class"" },
    { ""key"": 31, ""type"": ""Filter"", ""name"": ""Boston, Jan2014"" },
    { ""key"": 32, ""type"": ""Filter"", ""name"": ""Boston, 2014"" },
    { ""key"": 41, ""type"": ""Group"", ""name"": ""Sales"" },
    { ""key"": 42, ""type"": ""Group"", ""name"": ""Total Sales"" },
    { ""key"": 51, ""type"": ""Join"", ""name"": ""Product Name"" },
    { ""key"": 61, ""type"": ""Sort"", ""name"": ""Product Name"" },
    { ""key"": 71, ""type"": ""Export"", ""name"": ""File"" }
  ],
  ""linkDataSource"": [
    { ""from"": 1, ""frompid"": ""OUT"", ""to"": 11, ""topid"": ""L"" },
    { ""from"": 2, ""frompid"": ""OUT"", ""to"": 11, ""topid"": ""R"" },
    { ""from"": 3, ""frompid"": ""OUT"", ""to"": 12, ""topid"": ""R"" },
    { ""from"": 4, ""frompid"": ""OUT"", ""to"": 13, ""topid"": ""R"" },
    { ""from"": 11, ""frompid"": ""M"", ""to"": 12, ""topid"": ""L"" },
    { ""from"": 12, ""frompid"": ""M"", ""to"": 13, ""topid"": ""L"" },
    { ""from"": 13, ""frompid"": ""M"", ""to"": 21 },
    { ""from"": 21, ""frompid"": ""OUT"", ""to"": 31 },
    { ""from"": 21, ""frompid"": ""OUT"", ""to"": 32 },
    { ""from"": 31, ""frompid"": ""OUT"", ""to"": 41 },
    { ""from"": 32, ""frompid"": ""OUT"", ""to"": 42 },
    { ""from"": 41, ""frompid"": ""OUT"", ""to"": 51, ""topid"": ""L"" },
    { ""from"": 42, ""frompid"": ""OUT"", ""to"": 51, ""topid"": ""R"" },
    { ""from"": 51, ""frompid"": ""OUT"", ""to"": 61 },
    { ""from"": 61, ""frompid"": ""OUT"", ""to"": 71 }
  ]
}";

    Setup();
  }

  private void Setup() {
    _Diagram.InitialContentAlignment = Spot.Left;
    _Diagram.InitialAutoScale = AutoScale.UniformToFill;
    _Diagram.Layout = new LayeredDigraphLayout {
      Direction = 0,
      AlignOption = LayeredDigraphAlign.All
    };
    _Diagram.UndoManager.IsEnabled = true;

    Panel makePort(string name, bool leftside) {
      var port = new Shape("Rectangle") {
        Fill = "gray", Stroke = null,
        DesiredSize = new Size(8, 8),
        PortId = name,  // declare this object to be a "port"
        ToMaxLinks = 1,  // don't allow more than one link into a port
        Cursor = "pointer"  // show a different cursor to indicate potential link point
      };

      var lab = new TextBlock(name) { Font = new Font("Segoe UI", 7) };

      var panel = new Panel("Horizontal") { Margin = new Margin(2, 0) };

      // set up the port/panel based on which side of the node it will be on
      if (leftside) {
        port.ToSpot = Spot.Left;
        port.ToLinkable = true;
        lab.Margin = new Margin(1, 0, 0, 1);
        panel.Alignment = Spot.TopLeft;
        panel.Add(port);
        panel.Add(lab);
      } else {
        port.FromSpot = Spot.Right;
        port.FromLinkable = true;
        lab.Margin = new Margin(1, 1, 0, 0);
        panel.Alignment = Spot.TopRight;
        panel.Add(lab);
        panel.Add(port);
      }
      return panel;
    }

    void makeTemplate(string typename, string icon, string background, Panel[] inports, Panel[] outports) {
      var node = new Node("Spot")
        .Add(
          new Panel("Auto") { Width = 100, Height = 120 }
            .Add(
              new Shape("Rectangle") {
                  Fill = background, Stroke = null, StrokeWidth = 0,
                  Spot1 = Spot.TopLeft, Spot2 = Spot.BottomRight
                },
              new Panel("Table")
                .Add(
                  new TextBlock(typename) {
                      Row = 0,
                      Margin = 3,
                      MaxSize = new Size(80, double.NaN),
                      Stroke = "black",
                      Font = new Font("Segoe UI", 13, Northwoods.Go.FontWeight.Bold)
                    },
                  new Picture(icon) {
                      Row = 1, Width = 16, Height = 16, Scale = 3
                    },
                  new TextBlock {
                      Row = 2,
                      Margin = 3,
                      Editable = true,
                      MaxSize = new Size(80, 40),
                      Stroke = "white",
                      Font = new Font("Segoe UI", 11, Northwoods.Go.FontWeight.Bold)
                    }
                    .BindTwoWay("Text", "Name")
                )
            ),
          new Panel("Vertical") {
              Alignment = Spot.Left,
              AlignmentFocus = new Spot(0, 0.5, 8, 0)
            }
            .Add(inports),
          new Panel("Vertical") {
              Alignment = Spot.Right,
              AlignmentFocus = new Spot(1, 0.5, -8, 0)
            }
            .Add(outports)
      );
      _Diagram.NodeTemplateMap[typename] = node;
    }

    makeTemplate("Table", "https://nwoods.com/images/samples/table.png", "forestgreen",
      Array.Empty<Panel>(),
      new Panel[] { makePort("OUT", false) });

    makeTemplate("Join", "https://nwoods.com/images/samples/join.png", "mediumorchid",
      new Panel[] { makePort("L", true), makePort("R", true) },
      new Panel[] { makePort("UL", false), makePort("ML", false), makePort("M", false), makePort("MR", false), makePort("UR", false) });

    makeTemplate("Project", "https://nwoods.com/images/samples/project.png", "darkcyan",
      new Panel[] { makePort("", true) },
      new Panel[] { makePort("OUT", false) });

    makeTemplate("Filter", "https://nwoods.com/images/samples/filter.png", "cornflowerblue",
      new Panel[] { makePort("", true) },
      new Panel[] { makePort("OUT", false), makePort("INV", false) });

    makeTemplate("Group", "https://nwoods.com/images/samples/group.png", "mediumpurple",
      new Panel[] { makePort("", true) },
      new Panel[] { makePort("OUT", false) });

    makeTemplate("Sort", "https://nwoods.com/images/samples/sort.png", "sienna",
      new Panel[] { makePort("", true) },
      new Panel[] { makePort("OUT", false) });

    makeTemplate("Export", "https://nwoods.com/images/samples/upload.png", "darkred",
      new Panel[] { makePort("", true) },
      Array.Empty<Panel>());

    _Diagram.LinkTemplate =
      new Link {
          Routing = LinkRouting.Orthogonal, Corner = 25,
          RelinkableFrom = true, RelinkableTo = true
        }
        .Add(
          new Shape { Stroke = "gray", StrokeWidth = 2 },
          new Shape { Stroke = "gray", Fill = "gray", ToArrow = "Standard" }
        );

    // read in the JSON data from the "mySavedModel" element
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
public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
public class NodeData : Model.NodeData {
  public string Type { get; set; }
  public string Name { get; set; }
}

public class LinkData : Model.LinkData {
  public string Frompid { get; set; }
  public string Topid { get; set; }
}
