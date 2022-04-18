/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Extensions;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.Comments {
  [ToolboxItem(false)]
  public partial class CommentsControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public CommentsControl() {
      InitializeComponent();

      goWebBrowser1.Html = @"

 <p>
      <b>GoDiagram</b> supports the notion of ""Comment""s.
      A ""Comment"" is a node that is linked with another node but is positioned by some layouts to go along with that other node,
      rather than be laid out like a regular node and link.
    </p>
    <p>
      In this sample there are three ""Comment"" nodes, connected with regular nodes by three ""Comment"" links.
      Node and link data are marked as ""Comment""s by specifying ""Comment"" as the category.
      But the ""Comment"" nodes and links have a different default template, and thus a different appearance, than regular nodes and links.
      You can specify your own templates for ""Comment"" nodes and ""Comment"" links.
      The ""Comment"" link template defined here uses the <code>BalloonLink</code> class defined in
      <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/GraphObjects/BalloonLink/BalloonLink.cs"">BalloonLink.cs</a> in the Extensions directory.
    </p>
";

      textBox1.Text = @"{
  ""NodeDataSource"": [
{ ""Key"": ""Alpha"", ""Color"": ""orange"" },
{ ""Key"": ""Beta"", ""Color"": ""lightgreen"" },
{ ""Key"": ""Gamma"", ""Color"": ""lightgreen"" },
{ ""Key"": ""Delta"", ""Color"": ""pink"" },
{ ""Key"": ""A comment"", ""Text"": ""comment\nabout Alpha"", ""Category"": ""Comment"" },
{ ""Key"": ""B comment"", ""Text"": ""comment\nabout Beta"", ""Category"": ""Comment"" },
{ ""Key"": ""G comment"", ""Text"": ""comment about Gamma"", ""Category"": ""Comment"" }
],
  ""LinkDataSource"": [
{ ""From"": ""Alpha"", ""To"": ""Beta"" },
{ ""From"": ""Alpha"", ""To"": ""Gamma"" },
{ ""From"": ""Alpha"", ""To"": ""Delta"" },
{ ""From"": ""A comment"", ""To"": ""Alpha"", ""Category"": ""Comment"" },
{ ""From"": ""B comment"", ""To"": ""Beta"", ""Category"": ""Comment"" },
{ ""From"": ""G comment"", ""To"": ""Gamma"", ""Category"": ""Comment"" }
]}";

      Setup();
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // load custom figures
      Figures.DefineExtraFigures();

      // diagram properties
      myDiagram.Layout = new TreeLayout {
        Angle = 90,
        SetsPortSpot = false,
        SetsChildPortSpot = false
      };
      myDiagram.UndoManager.IsEnabled = true;
      // When a Node is deleted by the user, also delete all of its Comment Nodes.
      // When a Comment Link is deleted, also delete the corresponding Comment Node.
      myDiagram.SelectionDeleting += (_, e) => {
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
      myDiagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance).Add(
          new Shape { Figure = "CreateRequest", Fill = "white" }.Bind(
          new Binding("Fill", "Color")),
        new TextBlock { Margin = 4 }.Bind(
          new Binding("Text", "Key"))
        );

      // link template
      myDiagram.LinkTemplate =
        new Link().Add(
          new Shape { StrokeWidth = 1.5 },
          new Shape { ToArrow = "Standard", Stroke = null }
        );

      // comment node
      myDiagram.NodeTemplateMap.Remove("Comment");
      myDiagram.NodeTemplateMap.Add("Comment",
        new Node { // this needs to act as a rectangular shape for BalloonLink,
          Background = "transparent"
        }.Add(  // which can be accomplished by setting the background.
          new TextBlock { Stroke = "brown", Margin = 3 }.Bind(
            new Binding("Text"))
        )
      );

      // comment link
      myDiagram.LinkTemplateMap.Remove("Comment");
      myDiagram.LinkTemplateMap.Add("Comment",
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

      myDiagram.Model = Model.FromJson<Model>(textBox1.Text);
      myDiagram.Model.UndoManager.IsEnabled = true;
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Color { get; set; }
  }

  public class LinkData : Model.LinkData { }

}
