/* Copyright (c) Northwoods Software Corporation. */

using System;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace Demo.Samples.DataFlowVertical {
  [ToolboxItem(false)]
  public partial class DataFlowVerticalControl : DemoControl {
    private Diagram myDiagram;


    public DataFlowVerticalControl() {
      InitializeComponent();

      modelJson1.SaveClick = SaveModel;
      modelJson1.LoadClick = LoadModel;

      goWebBrowser1.Html = @"

   <p>
    This sample demonstrates a data flow or workflow graph with labeled ports on nodes. These ports are set up as panels, created within
    the <b>makePort</b> function. This function sets various properties of the <a>Shape</a> and
    <a>TextBlock</a> that make up the panel, and properties of the panel itself. Most notable are
    <a>GraphObject.PortId</a> to declare the shape as a port, and <a>GraphObject.FromLinkable</a> and
    <a>GraphObject.ToLinkable</a> to set the way the ports can be linked.
  </p>
  <p>
    The diagram also uses the <b>makeTemplate</b> function to create the node templates with shared features.
    This function takes a type, an image, a background color, and arrays of ports to create the node
    to be added to the <a>Diagram.NodeTemplateMap</a>.
  </p>
  <p>
    For the same data model rendered somewhat differently, see the <a href=""demo/DataFlow"">Data Flow (horizontal)</a> sample.
  </p>
";

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
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.InitialContentAlignment = Spot.Top;
      myDiagram.InitialAutoScale = AutoScale.UniformToFill;
      myDiagram.Layout = new LayeredDigraphLayout {
        Direction = 90,
        AlignOption = LayeredDigraphAlign.All
      };
      myDiagram.UndoManager.IsEnabled = true;

      // when the diagram is vertically oriented, left means top and right means bottom
      Panel makePort(string name, bool leftside) {
        var port = new Shape("Circle") {
          Fill = "#555555", Stroke = null,
          DesiredSize = new Size(10, 10),
          PortId = name,  // declare this object to be a "port"
          ToMaxLinks = 1,  // don't allow more than one link into a port
          Cursor = "pointer"  // show a different cursor to indicate potential link point
        };

        var lab = new TextBlock {
          Text = name,
          Font = new Font("Segoe UI", 7)
        };

        var panel = new Panel("Vertical") {
          Margin = new Margin(0, 2)
        };

        // set up the port/panel based on which side of the node it will be on
        if (leftside) {
          port.ToSpot = Spot.Top;
          port.ToLinkable = true;
          lab.Margin = new Margin(1, 0, 0, 1);
          panel.Alignment = Spot.TopLeft;
          panel.Add(port);
          panel.Add(lab);
        } else {
          port.FromSpot = Spot.Bottom;
          port.FromLinkable = true;
          lab.Margin = new Margin(1, 1, 0, 0);
          panel.Alignment = Spot.TopRight;
          panel.Add(lab);
          panel.Add(port);
        }
        return panel;
      }

      void makeTemplate(string typename, string icon, string background, Panel[] inports, Panel[] outports) {
        var fill = Brush.Lighten(background);
        var node = new Node("Spot") { SelectionAdorned = false }
          .Add(
            new Panel("Auto") { Width = 200, Height = 90 }
              .Add(
                new Shape("RoundedRectangle") {
                    Fill = fill, Stroke = "gray",
                    Spot1 = Spot.TopLeft, Spot2 = Spot.BottomRight
                  }
                  .Bind(new Binding("Fill", "IsSelected", s => (bool)s ? "dodgerblue" : fill).OfElement()),
                new Panel("Table")
                  .Add(
                    new TextBlock(typename) {
                        Column = 0,
                        Margin = 3,
                        MaxSize = new Size(80, double.NaN),
                        Stroke = "black",
                        Font = new Font("Segoe UI", 13, FontWeight.Bold)
                      },
                    new Picture(icon) {
                        Column = 1, Width = 55, Height = 55
                      },
                    new TextBlock {
                        Column = 2,
                        Margin = 3,
                        Editable = true,
                        MaxSize = new Size(80, 40),
                        Stroke = "black",
                        Font = new Font("Segoe UI", 11, FontWeight.Bold)
                      }
                      .BindTwoWay("Text", "Name")
                  )
              ),
            new Panel("Horizontal") {
                Alignment = Spot.Top,
                AlignmentFocus = new Spot(0.5, 0, 0, 4)
              }
              .Add(inports),
            new Panel("Horizontal") {
                Alignment = Spot.Bottom,
                AlignmentFocus = new Spot(0.5, 1, 0, -4)
              }
              .Add(outports)
        );
        myDiagram.NodeTemplateMap[typename] = node;
      }

      makeTemplate("Table", "table", "forestgreen",
        Array.Empty<Panel>(),
        new Panel[] { makePort("OUT", false) });

      makeTemplate("Join", "join", "mediumorchid",
        new Panel[] { makePort("L", true), makePort("R", true) },
        new Panel[] { makePort("UL", false), makePort("ML", false), makePort("M", false), makePort("MR", false), makePort("UR", false) });

      makeTemplate("Project", "project", "darkcyan",
        new Panel[] { makePort("", true) },
        new Panel[] { makePort("OUT", false) });

      makeTemplate("Filter", "filter", "cornflowerblue",
        new Panel[] { makePort("", true) },
        new Panel[] { makePort("OUT", false), makePort("INV", false) });

      makeTemplate("Group", "group", "mediumpurple",
        new Panel[] { makePort("", true) },
        new Panel[] { makePort("OUT", false) });

      makeTemplate("Sort", "sort", "sienna",
        new Panel[] { makePort("", true) },
        new Panel[] { makePort("OUT", false) });

      makeTemplate("Export", "upload", "darkred",
        new Panel[] { makePort("", true) },
        Array.Empty<Panel>());

      myDiagram.LinkTemplate =
        new Link {
            Curve = LinkCurve.Bezier,
            FromEndSegmentLength = 30, ToEndSegmentLength = 30,
            RelinkableFrom = true, RelinkableTo = true
          }
          .Add(
            new Shape { Stroke = "#555555", StrokeWidth = 2 }
          );

      // read in the JSON data from the "mySavedModel" element
      LoadModel();

    }

    private void SaveModel() {
      if (myDiagram == null) return;
      modelJson1.JsonText = myDiagram.Model.ToJson();
    }

    private void LoadModel() {
      if (myDiagram == null) return;
      myDiagram.Model = Model.FromJson<Model>(modelJson1.JsonText);
      myDiagram.Model.UndoManager.IsEnabled = true;
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
