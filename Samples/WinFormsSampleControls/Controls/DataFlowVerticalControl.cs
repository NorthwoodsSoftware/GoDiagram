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

namespace WinFormsSampleControls.DataFlowVertical {
  [ToolboxItem(false)]
  public partial class DataFlowVerticalControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;


    public DataFlowVerticalControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      saveLoadModel1.SaveClick += (e, obj) => SaveModel();
      saveLoadModel1.LoadClick += (e, obj) => LoadModel();

      goWebBrowser1.Html = @"

   <p>
    This sample demonstrates a data flow or workflow graph with labeled ports on nodes. These ports are set up as panels, created within
    the <b>makePort</b> function. This function sets various properties of the <a>Shape</a> and
    <a>TextBlock</a> that make up the panel, and properties of the panel itself. Most notable are
    <a>GraphObject.portId</a> to declare the shape as a port, and <a>GraphObject.fromLinkable</a> and
    <a>GraphObject.toLinkable</a> to set the way the ports can be linked.
  </p>
  <p>
    The diagram also uses the <b>makeTemplate</b> function to create the node templates with shared features.
    This function takes a type, an image, a background color, and arrays of ports to create the node
    to be added to the <a>Diagram.nodeTemplateMap</a>.
  </p>
  <p>
    For the same data model rendered somewhat differently, see the <a href=""dataFlow.html"">Data Flow (horizontal)</a> sample.
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

      // diagram properties
      myDiagram.InitialContentAlignment = Spot.Top;
      myDiagram.InitialAutoScale = AutoScaleType.UniformToFill;
      myDiagram.Layout = new LayeredDigraphLayout {
        Direction = 90
      };
      myDiagram.UndoManager.IsEnabled = true;

      // when the diagram is vertically oriented, left means top and right means bottom
      Panel makePort(string name, bool leftside) {
        var port = new Shape("Circle") {
          Fill = "black", Stroke = null,
          DesiredSize = new Size(8, 8),
          PortId = name,  // declare this object to be a "port"
          ToMaxLinks = 1,  // don't allow more than one link into a port
          Cursor = "pointer"  // show a different cursor to indicate potential link point
        };

        var lab = new TextBlock {
          Text = name,
          Font = "Segoe UI, 7px"
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
        var node = new Node("Spot")
          .Add(
            new Panel("Auto") { Width = 200, Height = 90 }
              .Add(
                new Shape("RoundedRectangle") {
                    Fill = background,
                    Spot1 = Spot.TopLeft, Spot2 = Spot.BottomRight
                  },
                new Panel("Table")
                  .Add(
                    new TextBlock(typename) {
                        Column = 0,
                        Margin = 3,
                        MaxSize = new Size(80, double.NaN),
                        Stroke = "black",
                        Font = "Segoe UI, 11px, style=bold"
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
                        Font = "Segoe UI, 9px, style=bold"
                      }
                      .Bind(new Binding("Text", "Name").MakeTwoWay())
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
            Routing = LinkRouting.Orthogonal, Corner = 5,
            RelinkableFrom = true, RelinkableTo = true
          }
          .Add(
            new Shape { Stroke = "black", StrokeWidth = 2 }
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
