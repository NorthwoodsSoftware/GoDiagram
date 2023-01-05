/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace Demo.Samples.DataFlow {
  public partial class DataFlow : DemoControl {
    private Diagram _Diagram;

    public DataFlow() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      modelJson1.SaveClick = SaveModel;
      modelJson1.LoadClick = LoadModel;

      desc1.MdText = DescriptionReader.Read("Samples.DataFlow.md");

      modelJson1.JsonText = @"{
  ""NodeCategoryProperty"": ""Type"",
  ""LinkFromPortIdProperty"": ""Frompid"",
  ""LinkToPortIdProperty"": ""Topid"",
  ""NodeDataSource"": [
    { ""Key"": 1, ""Type"": ""Table"", ""Name"": ""Product"" },
    { ""Key"": 2, ""Type"": ""Table"", ""Name"": ""Sales"" },
    { ""Key"": 3, ""Type"": ""Table"", ""Name"": ""Period"" },
    { ""Key"": 4, ""Type"": ""Table"", ""Name"": ""Store"" },
    { ""Key"": 11, ""Type"": ""Join"", ""Name"": ""Product, Class"" },
    { ""Key"": 12, ""Type"": ""Join"", ""Name"": ""Period"" },
    { ""Key"": 13, ""Type"": ""Join"", ""Name"": ""Store"" },
    { ""Key"": 21, ""Type"": ""Project"", ""Name"": ""Product, Class"" },
    { ""Key"": 31, ""Type"": ""Filter"", ""Name"": ""Boston, Jan2014"" },
    { ""Key"": 32, ""Type"": ""Filter"", ""Name"": ""Boston, 2014"" },
    { ""Key"": 41, ""Type"": ""Group"", ""Name"": ""Sales"" },
    { ""Key"": 42, ""Type"": ""Group"", ""Name"": ""Total Sales"" },
    { ""Key"": 51, ""Type"": ""Join"", ""Name"": ""Product Name"" },
    { ""Key"": 61, ""Type"": ""Sort"", ""Name"": ""Product Name"" },
    { ""Key"": 71, ""Type"": ""Export"", ""Name"": ""File"" }
  ],
  ""LinkDataSource"": [
    { ""From"": 1, ""Frompid"": ""OUT"", ""To"": 11, ""Topid"": ""L"" },
    { ""From"": 2, ""Frompid"": ""OUT"", ""To"": 11, ""Topid"": ""R"" },
    { ""From"": 3, ""Frompid"": ""OUT"", ""To"": 12, ""Topid"": ""R"" },
    { ""From"": 4, ""Frompid"": ""OUT"", ""To"": 13, ""Topid"": ""R"" },
    { ""From"": 11, ""Frompid"": ""M"", ""To"": 12, ""Topid"": ""L"" },
    { ""From"": 12, ""Frompid"": ""M"", ""To"": 13, ""Topid"": ""L"" },
    { ""From"": 13, ""Frompid"": ""M"", ""To"": 21 },
    { ""From"": 21, ""Frompid"": ""OUT"", ""To"": 31 },
    { ""From"": 21, ""Frompid"": ""OUT"", ""To"": 32 },
    { ""From"": 31, ""Frompid"": ""OUT"", ""To"": 41 },
    { ""From"": 32, ""Frompid"": ""OUT"", ""To"": 42 },
    { ""From"": 41, ""Frompid"": ""OUT"", ""To"": 51, ""Topid"": ""L"" },
    { ""From"": 42, ""Frompid"": ""OUT"", ""To"": 51, ""Topid"": ""R"" },
    { ""From"": 51, ""Frompid"": ""OUT"", ""To"": 61 },
    { ""From"": 61, ""Frompid"": ""OUT"", ""To"": 71 }
  ]
}";

      Setup();
    }

    private void Setup() {
      _Diagram.InitialContentAlignment = Spot.Left;
      _Diagram.InitialAutoScale = AutoScale.UniformToFill;
      _Diagram.Layout = new LayeredDigraphLayout {
        Direction = 0
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
                      .Bind(new Binding("Text", "Name").MakeTwoWay())
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

      makeTemplate("Table", "https://nwoods.com/go/images/samples/table.png", "forestgreen",
        Array.Empty<Panel>(),
        new Panel[] { makePort("OUT", false) });

      makeTemplate("Join", "https://nwoods.com/go/images/samples/join.png", "mediumorchid",
        new Panel[] { makePort("L", true), makePort("R", true) },
        new Panel[] { makePort("UL", false), makePort("ML", false), makePort("M", false), makePort("MR", false), makePort("UR", false) });

      makeTemplate("Project", "https://nwoods.com/go/images/samples/project.png", "darkcyan",
        new Panel[] { makePort("", true) },
        new Panel[] { makePort("OUT", false) });

      makeTemplate("Filter", "https://nwoods.com/go/images/samples/filter.png", "cornflowerblue",
        new Panel[] { makePort("", true) },
        new Panel[] { makePort("OUT", false), makePort("INV", false) });

      makeTemplate("Group", "https://nwoods.com/go/images/samples/group.png", "mediumpurple",
        new Panel[] { makePort("", true) },
        new Panel[] { makePort("OUT", false) });

      makeTemplate("Sort", "https://nwoods.com/go/images/samples/sort.png", "sienna",
        new Panel[] { makePort("", true) },
        new Panel[] { makePort("OUT", false) });

      makeTemplate("Export", "https://nwoods.com/go/images/samples/upload.png", "darkred",
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
}
