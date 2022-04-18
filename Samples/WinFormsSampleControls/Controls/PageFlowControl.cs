/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;
using Northwoods.Go.WinForms;

namespace WinFormsSampleControls.PageFlow {
  [ToolboxItem(false)]
  public partial class PageFlowControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    private Palette myPalette;
    private Dictionary<string, Part> sharedNodeTemplateMap;

    public PageFlowControl() {
      InitializeComponent();

      saveLoadModel1.SaveClick += (e, obj) => SaveModel();
      saveLoadModel1.LoadClick += (e, obj) => LoadModel();
      btnLayout.Click += (e, obj) => doLayout();

      goWebBrowser1.Html = @"
  <p>
    This workflow diagram uses the <a>LayeredDigraphLayout</a> to display some data about the flow of a fictional web site.
    You can add to the Diagram by dragging Nodes from the <a>Palette</a> and by buttons that
    appear when clicking on the Page (yellow) and Drop (red) Nodes.
  </p>
  <p>
    All nodes in this sample have editable text.
    To activate the <a>TextEditingTool</a>, click on a node to select it and click on its text once selected.
  </p>
  <p>
    Several Link relationships are defined.
    Hovering over the sides of many nodes changes the mouse cursor to a pointer.
    Clicking and dragging in these areas creates a new link with the <a>LinkingTool</a>.
    The node definitions contain several rules, for instance you cannot drag links to Source (blue) Nodes,
    only from them, and you cannot have multiple links between the same two nodes, among others.
  </p>
  <p>
    Most of the source code for this sample is in defining pleasing Node templates.
    Much of the functionality seen is done with built-in GoDiagram components.
    This is by no means an exhaustive sample, so be sure to check out the other samples to the left,
    or take a look at the <a href=""intro/index.html"">Introductory Documents</a> for
    a more structured tutorial on different GoDiagram concepts.
  </p>
";

      saveLoadModel1.ModelJson = @"{
  ""NodeDataSource"": [
    { ""Key"": -1, ""Category"": ""Source"", ""Text"": ""Search"" },
    { ""Key"": -2, ""Category"": ""Source"", ""Text"": ""Referral"" },
    { ""Key"": -3, ""Category"": ""Source"", ""Text"": ""Advertising"" },

    { ""Key"": 1, ""Text"": ""Homepage"" },
    { ""Key"": 2, ""Text"": ""Products"" },
    { ""Key"": 3, ""Text"": ""Buy"" },
    { ""Key"": 4, ""Text"": ""Samples"" },
    { ""Key"": 5, ""Text"": ""Documentation"" },
    { ""Key"": 6, ""Text"": ""Download"" },

    { ""Key"": 101, ""Category"": ""DesiredEvent"", ""Text"": ""Ordered!"" },
    { ""Key"": 102, ""Category"": ""DesiredEvent"", ""Text"": ""Downloaded!"" },

    {
      ""Key"": 201, ""Category"": ""UndesiredEvent"",
      ""ReasonsList"": [
        { ""Text"":""Needs redesign?""},
        { ""Text"":""Wrong Product?""}
      ]
    },
    {
      ""Key"": 202, ""Category"": ""UndesiredEvent"",
      ""ReasonsList"": [
        { ""Text"":""Need better samples?""},
        { ""Text"":""Bad landing page for Advertising?""}
      ]
    },
    {
      ""Key"": 203, ""Category"": ""UndesiredEvent"",
      ""ReasonsList"": [
        { ""Text"":""Reconsider Pricing?""},
        { ""Text"":""Confusing Cart?""}
      ]
    },

    { ""Category"": ""Comment"", ""Text"": ""Add notes with general comments for the next team meeting"" }

  ],
  ""LinkDataSource"": [
    { ""From"": -1, ""To"": 1 },
    { ""From"": -2, ""To"": 1 },
    { ""From"": -2, ""To"": 4 },
    { ""From"": -3, ""To"": 4 },
    { ""From"":  1, ""To"": 2 },
    { ""From"":  2, ""To"": 3 },
    { ""From"":  2, ""To"": 4 },
    { ""From"":  1, ""To"": 5 },
    { ""From"":  5, ""To"": 4 },
    { ""From"":  4, ""To"": 3 },


    { ""From"":  4, ""To"": 6 },

    { ""From"":  3, ""To"": 101 },
    { ""From"":  6, ""To"": 102 },

    { ""From"":  1, ""To"": 201 },
    { ""From"":  4, ""To"": 202 },
    { ""From"":  3, ""To"": 203 }
  ]
}";

      Setup();
      SetupPalette();
    }

    private void DefineNodeTemplates() {
      if (sharedNodeTemplateMap != null) return;  // already defined

      var mygrad = new Brush("Linear", new[] { (0f, "#e5e5e5"), (1, "#333333") });

      var yellowgrad = new Brush(new LinearGradientPaint(new Dictionary<float, string> {
        { 0, "rgb(254, 201, 0)" }, { 1, "rgb(254, 162, 0)" } }
      ));
      var greengrad = new Brush(new LinearGradientPaint(new Dictionary<float, string> {
        { 0, "#98FB98" }, { 1, "#9ACD32" } }
      ));
      var bluegrad = new Brush(new LinearGradientPaint(new Dictionary<float, string> {
        { 0, "#B0E0E6" }, { 1, "#87CEEB" } }
      ));
      var redgrad = new Brush(new LinearGradientPaint(new Dictionary<float, string> {
        { 0, "#C45245" }, { 1, "#871E1B" } }
      ));
      var whitegrad = new Brush(new LinearGradientPaint(new Dictionary<float, string> {
        { 0, "#F0F8FF" }, { 1, "#E6E6FA" } }
      ));

      var bigFont = new Font("Segoe UI", 13, FontWeight.Bold);
      var smallFont = new Font("Segoe UI", 11, FontWeight.Bold);

      var textStyle = new {
        Margin = 6,
        Wrap = Wrap.Fit,
        TextAlign = TextAlign.Center,
        Editable = true,
        Font = bigFont
      };

      var defaultAdornment =
        new Adornment("Spot")
          .Add(
            new Panel("Auto")
              .Add(
                new Shape { Fill = null, Stroke = "dodgerblue", StrokeWidth = 4 },
                new Placeholder()
              ),
            // the button to create a "next" node, at the top-right corner
            Builder.Make<Panel>("Button")
              .Set(new { Alignment = Spot.TopRight, Click = new Action<InputEvent, GraphObject>(_AddNodeAndLink) })
              .Add(new Shape("PlusLine") { DesiredSize = new Size(6, 6) })
              .Bind(new Binding("Visible", "", (a, _) => !(a as GraphObject).Diagram.IsReadOnly).OfElement()
        )
      );

      // Undesired events have a special adornment that allows adding additional "reasons"
      var UndesiredEventAdornment =
        new Adornment("Spot")
          .Add(
            new Panel("Auto")
              .Add(
                new Shape { Fill = null, Stroke = "dodgerblue", StrokeWidth = 4 },
                new Placeholder()
              ),
            // the button to add additional reasons
            Builder.Make<Panel>("Button")
              .Set(new { Alignment = Spot.BottomRight, Click = new Action<InputEvent, GraphObject>(_AddReason) })
              .Add(new Shape("TriangleDown") { DesiredSize = new Size(10, 10) })
              .Bind(new Binding("Visible", "", (a, _) => !(a as GraphObject).Diagram.IsReadOnly).OfElement())
          );

      var reasonTemplate =
        new Panel("Horizontal")
          .Add(
            new TextBlock("Reason") {
              Margin = new Margin(4, 0, 0, 0),
              MaxSize = new Size(200, double.NaN),
              Wrap = Wrap.Fit,
              Stroke = "whitesmoke",
              Editable = true,
              Font = smallFont
            }
              .Bind(new Binding("Text").MakeTwoWay())
          );

      sharedNodeTemplateMap = new Dictionary<string, Part>() {
        {
          "",
          new Node("Auto") { SelectionAdornmentTemplate = defaultAdornment }
            .Bind("Location", "Loc", Point.Parse, Point.Stringify)
            .Add(
              // define the node's outer shape, which will surround the TextBlock
              new Shape("Rectangle") {
                  Fill = yellowgrad, Stroke = "black",
                  PortId = "", FromLinkable = true, ToLinkable = true, Cursor = "pointer",
                  ToEndSegmentLength = 50, FromEndSegmentLength = 40
                },
              new TextBlock("Page") {
                  Margin = 6,
                  Font = bigFont,
                  Editable = true
                }
                .Bind(new Binding("Text").MakeTwoWay())
            )
        },
        {
          "Source",
          new Node("Auto")
            .Bind("Location", "Loc", Point.Parse, Point.Stringify)
            .Add(
              new Shape("RoundedRectangle") {
                  Fill = bluegrad,
                  PortId = "", FromLinkable = true, Cursor = "pointer", FromEndSegmentLength = 40
                },
              new TextBlock("Source")
                .Set(textStyle)
                .Bind(new Binding("Text").MakeTwoWay())
            )
        },
        {
          "DesiredEvent",
          new Node("Auto")
            .Bind("Location", "Loc", Point.Parse, Point.Stringify)
            .Add(
              new Shape("RoundedRectangle") {
                  Fill = greengrad, PortId = "", ToLinkable = true, ToEndSegmentLength = 50
                },
              new TextBlock("Success!")
                .Set(textStyle)
                .Bind(new Binding("Text").MakeTwoWay())
            )
        },
        {
          "UndesiredEvent",
          new Node("Auto") { SelectionAdornmentTemplate = UndesiredEventAdornment }
            .Bind("Location", "Loc", Point.Parse, Point.Stringify)
            .Add(
              new Shape("RoundedRectangle") {
                  Fill = redgrad, PortId = "", ToLinkable = true, ToEndSegmentLength = 50
                },
              new Panel("Vertical") { DefaultAlignment = Spot.TopLeft }
                .Add(
                  new TextBlock("Drop") {
                      Stroke = "whitesmoke",
                      MinSize = new Size(80, double.NaN)
                    }
                    .Set(textStyle)
                    .Bind(new Binding("Text").MakeTwoWay()),
                  new Panel("Vertical") {
                      DefaultAlignment = Spot.TopLeft,
                      ItemTemplate = reasonTemplate
                    }
                    .Bind(new Binding("ItemList", "ReasonsList").MakeTwoWay())
                )
            )
        },
        {
          "Comment",
          new Node("Auto")
            .Bind("Location", "Loc", Point.Parse, Point.Stringify)
            .Add(
              new Shape("Rectangle") {
                  PortId = "", Fill = whitegrad, FromLinkable = true
                },
              new TextBlock("A comment") {
                  Margin = 9,
                  MaxSize = new Size(200, double.NaN),
                  Wrap = Wrap.Fit,
                  Editable = true,
                  Font = smallFont
                }
                .Bind(new Binding("Text").MakeTwoWay())
            )
        }
      };
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // have mouse wheel events zoom in and out instead of scroll up and down
      myDiagram.ToolManager.MouseWheelBehavior = WheelMode.Zoom;
      myDiagram.InitialAutoScale = AutoScale.Uniform;
      myDiagram.ToolManager.LinkingTool.Direction = LinkingDirection.ForwardsOnly;
      myDiagram.Layout = new LayeredDigraphLayout {
        IsInitial = false,
        IsOngoing = false,
        LayerSpacing = 50
      };
      myDiagram.UndoManager.IsEnabled = true;

      DefineNodeTemplates();
      myDiagram.NodeTemplateMap = sharedNodeTemplateMap;

      // replace the default Link template in the linkTemplateMap
      myDiagram.LinkTemplate = new Link { // the whole link panel
        Curve = LinkCurve.Bezier,
        ToShortLength = 15
      }
        .Bind(
          new Binding("Points").MakeTwoWay(),
          new Binding("Curviness")
        )
        .Add(
          new Shape { Stroke = "#2F4F4F", StrokeWidth = 2.5 },  // the link shape
          new Shape { ToArrow = "kite", Fill = "#2F4F4F", Stroke = null, Scale = 2 }  // the arrowhead
        );

      myDiagram.LinkTemplateMap["Comment"] =
        new Link { Selectable = false }
          .Add(new Shape { StrokeWidth = 2, Stroke = "darkgreen" });

      // read in the JSON-format data from the JSON element
      LoadModel();
      doLayout();
    }

    private void SetupPalette() {
      myPalette = paletteControl1.Diagram as Palette;
      DefineNodeTemplates();
      myPalette.NodeTemplateMap = sharedNodeTemplateMap;
      //myPalette.AutoScale = AutoScale.Uniform; // everything always fits in viewport

      myPalette.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Category = "Source" },
          new NodeData(), // default node
          new NodeData { Category = "DesiredEvent" },
          new NodeData { Category = "UndesiredEvent", ReasonsList = new List<ReasonData> { new ReasonData { Text = "Reason" } } },
          new NodeData { Category = "Comment" }
        }
      };
    }

    // clicking the button on an UndesiredEvent node inserts a new text object into the panel
    private void _AddReason(InputEvent e, GraphObject obj) {
      var adorn = obj.Part as Adornment;
      if (adorn == null) return;
      e.Handled = true;
      var arr = (adorn.AdornedPart.Data as NodeData).ReasonsList;
      myDiagram.StartTransaction("add reason");
      myDiagram.Model.InsertListItem(arr, arr.Count, new ReasonData());
      myDiagram.CommitTransaction("add reason");
    }

    // clicking the button of a default node inserts a new node to the right of the selected node,
    // and adds a link to that new node
    private void _AddNodeAndLink(InputEvent e, GraphObject obj) {
      var adorn = obj.Part as Adornment;
      if (adorn == null) return;
      e.Handled = true;
      var diagram = adorn.Diagram;
      diagram.StartTransaction("Add State");
      // get the node data for which the user clicked a button
      var fromNode = adorn.AdornedPart;
      var fromData = fromNode.Data as NodeData;
      // create a new State data object, positioned off to the right of the adorned Node
      var toData = new NodeData { Text = "new" };
      var p = fromNode.Location;
      toData.Loc = p.X + 200 + " " + p.Y; // the Loc property is a string, not a Point object
      // add the new node data to the model
      var model = diagram.Model as Model;
      model.AddNodeData(toData);
      // create a link data from the old node data to the new node data
      var linkdata = new LinkData();
      linkdata.From = model.GetKeyForNodeData(fromData);
      linkdata.To = model.GetKeyForNodeData(toData);
      // and add the link data to the model
      model.AddLinkData(linkdata);
      // select the new Node
      var newnode = diagram.FindNodeForData(toData);
      diagram.Select(newnode);
      diagram.CommitTransaction("Add State");
    }

    private void doLayout() {
      myDiagram.LayoutDiagram(true);
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

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }

  public class NodeData : Model.NodeData {
    public List<ReasonData> ReasonsList { get; set; }
    public string Loc { get; set; }
  }

  public class LinkData : Model.LinkData {
    public List<Point> Points { get; set; }
  }

  public class ReasonData {
    public string Text { get; set; }
  }

}
