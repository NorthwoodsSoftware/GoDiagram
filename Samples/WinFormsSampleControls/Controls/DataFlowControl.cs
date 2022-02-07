using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.DataFlow {
  [ToolboxItem(false)]
  public partial class DataFlowControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public DataFlowControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      saveLoadModel1.SaveClick += (e, obj) => SaveModel();
      saveLoadModel1.LoadClick += (e, obj) => LoadModel();

      goWebBrowser1.Html = @"

   <p>
    This sample demonstrates labeled ports on nodes, arranged as a data flow or workflow. These ports are set up as panels, created within
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
    For the same data model rendered somewhat differently, see the <a href=""DataFlowVertical"">Data Flow (vertical)</a> sample.
  </p>
";

      saveLoadModel1.ModelJson = @"{
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

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      myDiagram.InitialContentAlignment = Spot.Left;
      myDiagram.InitialAutoScale = AutoScaleType.UniformToFill;
      myDiagram.Layout = new LayeredDigraphLayout {
        Direction = 0
      };
      myDiagram.UndoManager.IsEnabled = true;

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
                      Stroke = "white",
                      Font = new Font("Segoe UI", 11, FontWeight.Bold)
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
                      Font = new Font("Segoe UI", 9, FontWeight.Bold)
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
          Routing = LinkRouting.Orthogonal, Corner = 5,
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
      if (myDiagram == null) return;
      saveLoadModel1.ModelJson = myDiagram.Model.ToJson();
    }

    private void LoadModel() {
      if (myDiagram == null) return;
      myDiagram.Model = Model.FromJson<Model>(saveLoadModel1.ModelJson);
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
