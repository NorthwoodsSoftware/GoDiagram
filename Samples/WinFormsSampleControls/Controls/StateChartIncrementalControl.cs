using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;

namespace WinFormsSampleControls.StateChartIncremental {
  [ToolboxItem(false)]
  public partial class StateChartIncrementalControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    
    public StateChartIncrementalControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      saveLoadModel1.SaveClick += (e, obj) => SaveModel();
      saveLoadModel1.LoadClick += (e, obj) => LoadModel();

      goWebBrowser1.Html = @"
        <p>
      This sample is derived from the <a href=""stateChart.html"">State Chart</a> sample.
      This makes use of the new <a>GraphLinksModel.linkKeyProperty</a> property
      and the <a>Model.toIncrementalJson</a> and <a>Model.applyIncrementalJson</a> methods.
      It also demonstrates custom functions for <a>Model.makeUniqueKeyFunction</a> and
      <a>GraphLinksModel.makeUniqueLinkKeyFunction</a>, which assign odd numbers to new node
      data and even numbers to new link data.
      Unlike most models, this example uses ""id"" as the name of the <a>Model.nodeKeyProperty</a> rather than ""key"".
        </p>
        <p>
      Last <a>Transaction</a> saved in incremental JSON format:
        </p>
";

      saveLoadModel1.ModelJson = @"{
  ""NodeKeyProperty"": ""Id"",
  ""LinkKeyProperty"": ""Id"",
  ""NodeDataSource"": [
    { ""Id"": 1, ""Loc"":""120 120"", ""Text"":""Initial"" },
    { ""Id"": 3, ""Loc"":""330 120"", ""Text"":""First down"" },
    { ""Id"": 5, ""Loc"":""226 376"", ""Text"":""First up"" },
    { ""Id"": 7, ""Loc"":""60 276"", ""Text"":""Second down"" },
    { ""Id"": 9, ""Loc"":""226 226"", ""Text"":""Wait"" }
  ],
  ""LinkDataSource"": [
    { ""Id"": 2, ""From"": 1, ""To"": 1, ""Text"":""Up or timer"", ""Curviness"": -20 },
    { ""Id"": 4, ""From"": 1, ""To"": 3, ""Text"":""Down"", ""Curviness"": 20 },
    { ""Id"": 6, ""From"": 3, ""To"": 1, ""Text"":""Up(moved)\nPOST"", ""Curviness"": 20 },
    { ""Id"": 8, ""From"": 3, ""To"": 3, ""Text"":""Down"", ""Curviness"": -20 },
    { ""Id"": 10, ""From"": 3, ""To"": 5, ""Text"":""Up(no move)"" },
    { ""Id"": 12, ""From"": 3, ""To"": 9, ""Text"":""Timer"" },
    { ""Id"": 14, ""From"": 5, ""To"": 1, ""Text"":""Timer\nPOST"" },
    { ""Id"": 16, ""From"": 5, ""To"": 7, ""Text"":""Down"" },
    { ""Id"": 18, ""From"": 7, ""To"": 1, ""Text"":""Up\nPOST\n(dblclick\nif no move)"" },
    { ""Id"": 20, ""From"": 7, ""To"": 7, ""Text"":""Down or timer"", ""Curviness"": 20 },
    { ""Id"": 22, ""From"": 9, ""To"": 1, ""Text"":""Up\nPOST"" },
    { ""Id"": 24, ""From"": 9, ""To"": 9, ""Text"":""Down"" }
  ]
}";

    }

    private string myTransaction = "InitialLayout";

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.ToolManager.MouseWheelBehavior = WheelMode.Zoom;
      // double-click in background to create a new node
      myDiagram.ToolManager.ClickCreatingTool.ArchetypeNodeData = new NodeData {
        Text = "new node"
      };
      myDiagram.InitialLayoutCompleted += (_, __) => { ShowIncremental("InitialLayout"); };
      myDiagram.ModelChanged += (_, e) => {
        if (e.IsTransactionFinished) {
          // this records each Transaction as a JSON-format string
          //txtSave.Text = myTransaction;
          // try-catch until GraphLinksModel.RecurseData is implemented :(
          try {
            ShowIncremental((myDiagram.Model as Model).ToIncrementalJson(e));
          } catch {
            ShowIncremental("Could not load last transaction as JSON");
          }

        }
      };
      // enable undo and redo
      myDiagram.UndoManager.IsEnabled = true;

      // define the Node template
      myDiagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance).Bind(
          new Binding("Location", "Loc", Point.Parse).MakeTwoWay(Point.Stringify)
        ).Add(
          // define the node's outer shape, which will surround the TextBlock
          new Shape {
            Figure = "RoundedRectangle",
            Parameter1 = 20,  // the corner has a large radius
            Fill = new Brush(new LinearGradientPaint(new Dictionary<float, string> {
                { 0, "rgb(254, 201, 0)" },
                { 1, "rgb(254, 162, 0)" }
              }
            )),
            Stroke = "black",
            PortId = "",
            FromLinkable = true,
            FromLinkableSelfNode = true,
            FromLinkableDuplicates = true,
            ToLinkable = true,
            ToLinkableSelfNode = true,
            ToLinkableDuplicates = true,
            Cursor = "pointer"
          },
          new TextBlock {
            Font = "Segoe UI, 11px, style=bold",
            Editable = true  // editing the text automatically updates the model data
          }.Bind(
            new Binding("Text", "Text").MakeTwoWay())
        );

      // unlike the normal selection Adornment, this one includes a Button
      myDiagram.NodeTemplate.SelectionAdornmentTemplate =
        new Adornment(PanelLayoutSpot.Instance).Add(
          new Panel(PanelLayoutAuto.Instance).Add(
            new Shape {
              Fill = null,
              Stroke = "blue",
              StrokeWidth = 2
            },
            new Placeholder() // this represents the selected Node
          ),
          // the button to create a "next" node, at the top-right corner
          Builder.Make<Panel>("Button").Set(
            new {
              Alignment = Spot.TopRight,
              Click = new Action<InputEvent, GraphObject>(AddNodeAndLink)  // this function is defined below
            }
          ).Add(
            new Shape {
              Figure = "PlusLine",
              DesiredSize = new Size(6, 6)
            }
          ) // end button
        ); // end Adornment

      void AddNodeAndLink(InputEvent e, GraphObject obj) {
        var adorn = obj.Part as Adornment;
        var fromNode = adorn.AdornedPart;
        if (fromNode == null) return;

        e.Handled = true;
        var diagram = e.Diagram;
        diagram.StartTransaction("Add State");

        // get the node data for which the user clicked the button
        var fromData = fromNode.Data as NodeData;
        Point p = fromNode.Location.Offset(200, 0);
        // create a new "state" data object, positioned to the right of the adorned node
        var toData = new NodeData { Text = "new", Loc = Point.Stringify(p) };
        var model = diagram.Model as Model;
        model.AddNodeData(toData);

        // create a link data from the old node data to the new node data
        var linkdata = new LinkData {
          From = fromData.Id,
          To = toData.Id,
          Text = "transition"
        };
        // add the link data to model
        model.AddLinkData(linkdata);

        // select new node
        var newnode = diagram.FindNodeForData(toData);
        diagram.Select(newnode);

        diagram.CommitTransaction("Add State");

        // if new node is off-screen, scroll to show new node
        if (newnode != null) diagram.ScrollToRect(newnode.ActualBounds);
      }

      // replace the default Link template in the linkTemplateMap
      myDiagram.LinkTemplate =
        new Link  // the whole link panel
          {
          Curve = LinkCurve.Bezier,
          Adjusting = LinkAdjusting.Stretch,
          Reshapable = true,
          RelinkableFrom = true,
          RelinkableTo = true
        }.Bind(
          new Binding("Points").MakeTwoWay(),
          new Binding("Curviness", "Curviness")
        ).Add(
          new Shape  // the link shape
            {
            StrokeWidth = 1.5
          },
          new Shape  // the arrowhead
            {
            ToArrow = "standard",
            Stroke = null
          },
          new Panel(PanelLayoutAuto.Instance).Add(
            new Shape  // the label background, which becomes transparent around the edges
              {
              Fill = new Brush(new RadialGradientPaint(new Dictionary<float, string> {
                  { 0, "rgb(240, 240, 240)" },
                  { 0.3f, "rgb(240, 240, 240)" },
                  { 1, "rgba(240, 240, 240, 0)" }
                }
              )),
              Stroke = (Brush)null
            },
            new TextBlock { // the label text
              Text = "transition",
              TextAlign = TextAlign.Center,
              Font = "Segoe UI, 10px",
              Stroke = "black",
              Margin = 4,
              Editable = true  // editing the text automatically updates the model data
            }.Bind(
              new Binding("Text", "Text").MakeTwoWay()
            )
          )
        );

      // read in the JSON-format data
      LoadModel();
    }

    private void SaveModel() {
      if (myDiagram == null) return;
      saveLoadModel1.ModelJson = myDiagram.Model.ToJson();
      ShowIncremental("");
    }

    private void LoadModel() {
      var model = Model.FromJson<Model>(saveLoadModel1.ModelJson);
      // establish GraphLinksModel functions:
      // node data id's are odd numbers
      model.MakeUniqueKeyFunction = (model, data) => {
        var i = model.NodeDataSource.Count() * 2 + 1;
        while (model.FindNodeDataForKey(i) != null) i += 2;
        data.Id = i;  // assume Model.NodeKeyProperty === "id"
        return i;
      };
      // link data id's are even numbers
      model.MakeUniqueLinkKeyFunction = (model, data) => {
        var i = model.LinkDataSource.Count() * 2 + 2;
        while (model.FindLinkDataForKey(i) != null) i += 2;
        data.Id = i;  // assume GraphLinksModel.LinkKeyProperty === "id"
        return i;
      };
      myDiagram.Model = model;
      ShowIncremental("");
    }

    private void ShowIncremental(string str) {
      // show the last transaction as an incremental update in JSON-formatted text
      if (myTransaction == "InitialLayout") str = "";
      myTransaction = str;
      txtSave.Text = myTransaction;
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }
  public class NodeData : Model.NodeData {
    public int Id { get; set; }
    public string Loc { get; set; }
  }

  public class LinkData : Model.LinkData {
    public int Id { get; set; }
  }

}
