/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Extensions;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.AddToPalette {
  [ToolboxItem(false)]
  public partial class AddToPaletteControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    private Palette myPalette;
    private Overview myOverview;
    private Inspector myInspector;

    private Part sharedNodeTemplate;

    public AddToPaletteControl() {
      InitializeComponent();

      myDiagram = diagramControl1.Diagram;
      myPalette = paletteControl1.Diagram as Palette;
      myOverview = overviewControl1.Diagram as Overview;

      DefineTemplates();

      modelJson2.SaveClick += (e, obj) => SaveModel();
      modelJson2.LoadClick += (e, obj) => LoadModel();
      btnAdd.Click += (e, obj) => AddToPalette();
      btnDelete.Click += (e, obj) => RemoveFromPalette();


      goWebBrowser1.Html = @"

   <p>
    This sample supports the normal kind of drag-and-drop from a <a>Palette</a> to a <a>Diagram</a>.
    The Data <a>Inspector</a> allows you to edit the properties of a selected node in the diagram.
  </p>
  <p>
    This sample also supports dynamically adding a copy of a selected node in the diagram to the
    palette by the ""Add To Palette"" button.
    See the current state of the palette's model in the top textarea.
    The palette is <a>Diagram.IsReadOnly</a>, so the user cannot delete selected nodes from the palette.
    But the ""Delete From Palette"" button removes any selected nodes from the palette.
  </p>
";

      modelJson1.JsonText = "";

      modelJson2.JsonText = @"{
  ""NodeDataSource"": [
    { ""Key"": 1, ""Text"":""Hello"", ""Figure"":""Circle"", ""Color"":""Green"", ""Loc"":""0 0"" },
    { ""Key"": 2, ""Text"":""World"", ""Figure"":""Rectangle"", ""Color"":""Red"", ""Loc"":""100 0"" }
  ],
  ""LinkDataSource"": [
    { ""From"":1, ""To"":2 }
  ]
}";

      Setup();
      SetupPalette();
      SetupOverview();
    }

    private void DefineTemplates() {
      sharedNodeTemplate =
        new Node("Auto") {
            LocationSpot = Spot.Center
          }
          .Bind("Location", "Loc", Point.Parse, Point.Stringify)
          .Add(
            new Shape("Circle") {
                Fill = "white", Stroke = "gray", StrokeWidth = 2,
                PortId = "", FromLinkable = true, ToLinkable = true,
                FromLinkableDuplicates = true, ToLinkableDuplicates = true,
                FromLinkableSelfNode = true, ToLinkableSelfNode = true
              }
              .Bind("Stroke", "Color")
              .Bind("Figure"),
            new TextBlock {
                Margin = new Margin(5, 5, 3, 5), Font = new Font("Segoe UI", 10),
                MinSize = new Size(16, 16), MaxSize = new Size(120, double.NaN),
                TextAlign = TextAlign.Center, Editable = true
              }
              .Bind(new Binding("Text").MakeTwoWay())
          );
    }

    private void Setup() {
      myDiagram.UndoManager.IsEnabled = true;
      myDiagram.NodeTemplate = sharedNodeTemplate;

      // load some intial data
      LoadModel();

      myInspector = new Inspector(inspectorControl1, myDiagram, new Inspector.Options {
        // comment this line to inspect all properties on each object:
        IncludesOwnProperties = false,
        Properties = new Dictionary<string, Inspector.PropertyOptions> {
          { "Text", new Inspector.PropertyOptions { } },
          { "Key", new Inspector.PropertyOptions { ReadOnly = true } },
          { "Color", new Inspector.PropertyOptions { } },
          { "Figure", new Inspector.PropertyOptions { } }
        }
      });
    }

    private void SetupPalette() {
      myPalette.NodeTemplate = sharedNodeTemplate;
      myPalette.ContentAlignment = Spot.Center;
      myPalette.Layout = new GridLayout {
        WrappingColumn = 1,
        CellSize = new Size(2, 2)
      };
      myPalette.ModelChanged += (_, e) => {  // just for demonstration purposes,
        if (e.IsTransactionFinished) {  // show the model data in the page's text box
          modelJson1.JsonText = e.Model.ToJson();
        }
      };

      // now add the initial contents of the Palette
      myPalette.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Text = "Circle", Color = "blue", Figure = "Circle" },
          new NodeData { Text = "Square", Color = "purple", Figure = "Square" },
          new NodeData { Text = "Ellipse", Color = "orange", Figure = "Ellipse" },
          new NodeData { Text = "Rectangle", Color = "red", Figure = "Rectangle" },
          new NodeData { Text = "Rounded\nRectangle", Color = "green", Figure = "RoundedRectangle" },
          new NodeData { Text = "Triangle", Color = "purple", Figure = "Triangle" }
        }
      };
    }

    private void SetupOverview() {
      myOverview.Observed = myDiagram;
      myOverview.ContentAlignment = Spot.Center;
    }

    private void SaveModel() {
      if (myDiagram == null) return;
      modelJson2.JsonText = myDiagram.Model.ToJson();
    }

    private void LoadModel() {
      if (myDiagram == null) return;
      myDiagram.Model = Model.FromJson<Model>(modelJson2.JsonText);
    }

    private void AddToPalette() {
      var node = myDiagram.Selection.Where((p) => { return p is Node; }).FirstOrDefault() as Node;
      if (node != null) {
        myPalette.StartTransaction();
        var item = (myPalette.Model as Model).CopyNodeData(node.Data as NodeData);
        (myPalette.Model as Model).AddNodeData(item);
        myPalette.CommitTransaction("added item to palette");
      }
    }

    // The user cannot delete selected nodes in the Palette with the Delete key or Control-X,
    // but they can if they do so programmatically.
    private void RemoveFromPalette() {
      myPalette.CommandHandler.DeleteSelection();
    }
  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Color { get; set; }
    public string Figure { get; set; }
    public string Loc { get; set; }
  }

  public class LinkData : Model.LinkData { }

}
