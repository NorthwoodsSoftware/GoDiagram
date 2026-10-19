/* Copyright (c) Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;

namespace Demo.Samples.StateChartIncremental {
  [ToolboxItem(false)]
  public partial class StateChartIncrementalControl : DemoControl {
    private Diagram myDiagram;

    public StateChartIncrementalControl() {
      InitializeComponent();

      modelJson1.SaveClick = SaveModel;
      modelJson1.LoadClick = LoadModel;

      goWebBrowser1.Html = @"
        <p>
      This sample is derived from the <a href=""demo/StateChart"">State Chart</a> sample.
      This makes use of the <a>GraphLinksModel.LinkKeyProperty</a> property
      and the <a>Model.ToIncrementalJson</a> method.
      It also demonstrates custom functions for <a>Model.MakeUniqueKeyFunction</a> and
      <a>GraphLinksModel.MakeUniqueLinkKeyFunction</a>, which assign odd numbers to new node
      data and even numbers to new link data.
      Unlike most models, this example uses ""Id"" as the name of the <a>Model.NodeKeyProperty</a> rather than ""Key"".
        </p>
        <p>
      Last <a>Transaction</a> saved in incremental JSON format:
        </p>
";

      modelJson1.JsonText = @"{
  ""nodeKeyProperty"": ""Id"",
  ""linkKeyProperty"": ""Id"",
  ""nodeDataSource"": [
    { ""id"": 1, ""loc"":""120 120"", ""text"":""Initial"" },
    { ""id"": 3, ""loc"":""330 120"", ""text"":""First down"" },
    { ""id"": 5, ""loc"":""226 376"", ""text"":""First up"" },
    { ""id"": 7, ""loc"":""60 276"", ""text"":""Second down"" },
    { ""id"": 9, ""loc"":""226 226"", ""text"":""Wait"" }
  ],
  ""linkDataSource"": [
    { ""id"": 2, ""from"": 1, ""to"": 1, ""text"":""Up or timer"", ""curviness"": -20 },
    { ""id"": 4, ""from"": 1, ""to"": 3, ""text"":""Down"", ""curviness"": 20 },
    { ""id"": 6, ""from"": 3, ""to"": 1, ""text"":""Up(moved)\nPOST"", ""curviness"": 20 },
    { ""id"": 8, ""from"": 3, ""to"": 3, ""text"":""Down"", ""curviness"": -20 },
    { ""id"": 10, ""from"": 3, ""to"": 5, ""text"":""Up(no move)"" },
    { ""id"": 12, ""from"": 3, ""to"": 9, ""text"":""Timer"" },
    { ""id"": 14, ""from"": 5, ""to"": 1, ""text"":""Timer\nPOST"" },
    { ""id"": 16, ""from"": 5, ""to"": 7, ""text"":""Down"" },
    { ""id"": 18, ""from"": 7, ""to"": 1, ""text"":""Up\nPOST\n(dblclick\nif no move)"" },
    { ""id"": 20, ""from"": 7, ""to"": 7, ""text"":""Down or timer"", ""curviness"": 20 },
    { ""id"": 22, ""from"": 9, ""to"": 1, ""text"":""Up\nPOST"" },
    { ""id"": 24, ""from"": 9, ""to"": 9, ""text"":""Down"" }
  ]
}";

      Setup();
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
            ShowIncremental(myDiagram.Model.ToIncrementalJson(e));
          } catch {
            ShowIncremental("Could not load last transaction as JSON");
          }

        }
      };
      // enable undo and redo
      myDiagram.UndoManager.IsEnabled = true;

      // define the Node template
      myDiagram.NodeTemplate =
        new Node(PanelType.Auto)
        .BindTwoWay("Location", "Loc", Point.Parse, Point.Stringify)
        .Add(
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
            Font = new Font("Segoe UI", 11, FontWeight.Bold),
            Editable = true  // editing the text automatically updates the model data
          }.BindTwoWay("Text")
        );

      // unlike the normal selection Adornment, this one includes a Button
      myDiagram.NodeTemplate.SelectionAdornmentTemplate =
        new Adornment(PanelType.Spot).Add(
          new Panel(PanelType.Auto).Add(
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
        }
        .BindTwoWay("Points")
        .Bind("Curviness")
        .Add(
          new Shape  // the link shape
            {
            StrokeWidth = 1.5
          },
          new Shape  // the arrowhead
            {
            ToArrow = "standard",
            Stroke = null
          },
          new Panel(PanelType.Auto).Add(
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
              Font = new Font("Segoe UI", 10),
              Stroke = "black",
              Margin = 4,
              Editable = true  // editing the text automatically updates the model data
            }.BindTwoWay("Text")
          )
        );

      // read in the JSON-format data
      LoadModel();
    }

    private void SaveModel() {
      if (myDiagram == null) return;
      modelJson1.JsonText = myDiagram.Model.ToJson();
      ShowIncremental("");
    }

    private void LoadModel() {
      var model = Model.FromJson<Model>(modelJson1.JsonText);
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
    public List<Point> Points { get; set; }
    public int Id { get; set; }
  }

}
