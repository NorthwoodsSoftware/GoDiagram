/* Copyright (c) Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Extensions;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace Demo.Samples.Comments; 
public partial class Comments : DemoControl {
  private Diagram _Diagram;

  public Comments() {
    InitializeComponent();

    desc1.MdText = DescriptionReader.Read("Samples.Comments.md");

    modelJson1.JsonText = @"{
  ""nodeDataSource"": [
{ ""key"": ""Alpha"", ""color"": ""orange"" },
{ ""key"": ""Beta"", ""color"": ""lightgreen"" },
{ ""key"": ""Gamma"", ""color"": ""lightgreen"" },
{ ""key"": ""Delta"", ""color"": ""pink"" },
{ ""key"": ""A comment"", ""text"": ""comment\nabout Alpha"", ""category"": ""Comment"" },
{ ""key"": ""B comment"", ""text"": ""comment\nabout Beta"", ""category"": ""Comment"" },
{ ""key"": ""G comment"", ""text"": ""comment about Gamma"", ""category"": ""Comment"" }
],
  ""linkDataSource"": [
{ ""from"": ""Alpha"", ""to"": ""Beta"" },
{ ""from"": ""Alpha"", ""to"": ""Gamma"" },
{ ""from"": ""Alpha"", ""to"": ""Delta"" },
{ ""from"": ""A comment"", ""to"": ""Alpha"", ""category"": ""Comment"" },
{ ""from"": ""B comment"", ""to"": ""Beta"", ""category"": ""Comment"" },
{ ""from"": ""G comment"", ""to"": ""Gamma"", ""category"": ""Comment"" }
]}";

    Setup();
  }

  private void Setup() {
    _Diagram = diagramControl1.Diagram;

    // load custom figures
    Figures.DefineExtraFigures();

    // diagram properties
    _Diagram.Layout = new TreeLayout {
      Angle = 90,
      SetsPortSpot = false,
      SetsChildPortSpot = false
    };
    _Diagram.UndoManager.IsEnabled = true;
    // When a Node is deleted by the user, also delete all of its Comment Nodes.
    // When a Comment Link is deleted, also delete the corresponding Comment Node.
    _Diagram.SelectionDeleting += (_, e) => {
      var parts = e.Subject as HashSet<Part>;  // the collection of Parts to be deleted, the Diagram.Selection
                                               // iterate over a copy of this collection,
                                               // because we may add to the collection by selecting more Parts
      foreach (var p in parts) {
        if (p is Node) {
          var node = p as Node;
          foreach (var n in node.FindNodesConnected()) {
            // remove every Comment Node that is connected with this node
            if (n.Category == "Comment") {
              n.IsSelected = true;  // include in normal deletion process
            }
          }
        } else if (p is Link && p.Category == "Comment") {
          var comlink = p as Link;  // a "Comment" Link
          var comnode = comlink.FromNode;
          // remove the Comment Node that is associated with this Comment Link,
          if (comnode.Category == "Comment") {
            comnode.IsSelected = true;  // include in normal deletion process
          }
        }
      }
    };

    // node template
    _Diagram.NodeTemplate =
      new Node(PanelType.Auto).Add(
        new Shape { Figure = "CreateRequest", Fill = "white" }.Bind(
        new Binding("Fill", "Color")),
      new TextBlock { Margin = 4 }.Bind(
        new Binding("Text", "Key"))
      );

    // link template
    _Diagram.LinkTemplate =
      new Link().Add(
        new Shape { StrokeWidth = 1.5 },
        new Shape { ToArrow = "Standard", Stroke = null }
      );

    // comment node
    _Diagram.NodeTemplateMap.Remove("Comment");
    _Diagram.NodeTemplateMap.Add("Comment",
      new Node { // this needs to act as a rectangular shape for BalloonLink,
        Background = "transparent"
      }.Add(  // which can be accomplished by setting the background.
        new TextBlock { Stroke = "brown", Margin = 3 }.Bind(
          new Binding("Text"))
      )
    );

    // comment link
    _Diagram.LinkTemplateMap.Remove("Comment");
    _Diagram.LinkTemplateMap.Add("Comment",
      // if the BalloonLink class has been loaded from the Extensions directory, use it
      new BalloonLink().Add(
        new Shape { // the Shape.Geometry will be computed to surround the comment node and
                    // point all the way to the commented node
          Stroke = "brown",
          StrokeWidth = 1,
          Fill = "lightyellow"
        }
      )
    );

    _Diagram.Model = Model.FromJson<Model>(modelJson1.JsonText);
    _Diagram.Model.UndoManager.IsEnabled = true;
  }
}

// define the model data
public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
public class NodeData : Model.NodeData {
  public string Color { get; set; }
}
public class LinkData : Model.LinkData { }
