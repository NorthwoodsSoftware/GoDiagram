using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.ParseTree {
  [ToolboxItem(false)]
  public partial class ParseTreeControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    
    public ParseTreeControl() {
      InitializeComponent();

      //When page loads
      diagramControl1.AfterRender = Setup;
      goWebBrowser1.Html = @"
        <p>A <em>parse tree</em> is an ordered, rooted tree representing the structure of a sentence, broken down to parts-of-speech.</p>
        <p>
          This diagram uses a custom <a>TreeLayout</a> called <b>FlatTreeLayout</b> that places all leaf nodes at the same Y position.
          It also makes use of a <b>TreeExpanderButton</b> on the node template. See the <a href=""intro/buttons.html"">Intro page on Buttons</a> for more GoDiagram button information.
        </p>
        <p>
          The abbreviations used in this diagram are:
          <ul>
            <li><b>NP</b>, a noun phrase</li>
            <li><b>VP</b>, a verb phrase</li>
            <li><b>PP</b>, a prepositional phrase</li>
            <li><b>DT</b>, a determiner</li>
            <li><b>JJ</b>, an adjective</li>
            <li><b>NN</b>, a common noun</li>
            <li><b>VBZ</b>, a third person singular present verb</li>
            <li><b>VBN</b>, a past participle verb</li>
          </ul>                                                        
        </p>
      ";
    }

    private void Setup() {

      myDiagram = diagramControl1.Diagram;
      myDiagram.AllowCopy = false;
      myDiagram.AllowDelete = false;
      myDiagram.AllowMove = false;
      myDiagram.InitialAutoScale = AutoScaleType.Uniform;
      myDiagram.Layout = new FlatTreeLayout { // custom tree layout, defined below
        Angle = 90,
        Compaction = TreeCompaction.None
      };
      myDiagram.UndoManager.IsEnabled = true;

      myDiagram.NodeTemplate = new Node(PanelLayoutVertical.Instance) {
        SelectionElementName = "BODY"
      }.Add(
        new Panel(PanelLayoutAuto.Instance) { Name = "BODY" }.Add(
          new Shape("RoundedRectangle")
            .Bind("Fill")
            .Bind("Stroke"),
          new TextBlock {
            Font = "Segoe UI, 12px, style=bold",
            Margin = new Margin(4, 2, 2, 2)
          }.Bind("Text")
        ),
        new Panel { // underneath the BODY
          Height = 17
        }.Add(Builder.Make<Panel>("TreeExpanderButton"))
      );

      myDiagram.LinkTemplate = new Link().Add(
        new Shape {
          StrokeWidth = 1.5
        }
      );

      var nodeDataSource = new List<NodeData> {
        new NodeData { Key = 1, Text = "Sentence", Fill =  "#f68c06", Stroke = "#4d90fe" },
        new NodeData { Key = 2, Text = "NP", Fill =  "#f68c06", Stroke = "#4d90fe", Parent = 1 },
        new NodeData { Key = 3, Text = "DT", Fill =  "#ccc", Stroke = "#4d90fe", Parent = 2 },
        new NodeData { Key = 4, Text = "A", Fill =  "#f8f8f8", Stroke = "#4d90fe", Parent = 3 },
        new NodeData { Key = 5, Text = "JJ", Fill =  "#ccc", Stroke = "#4d90fe", Parent = 2 },
        new NodeData { Key = 6, Text = "rare", Fill =  "#f8f8f8", Stroke = "#4d90fe", Parent = 5 },
        new NodeData { Key = 7, Text = "JJ", Fill =  "#ccc", Stroke = "#4d90fe", Parent = 2 },
        new NodeData { Key = 8, Text = "black", Fill =  "#f8f8f8", Stroke = "#4d90fe", Parent = 7 },
        new NodeData { Key = 9, Text = "NN", Fill =  "#ccc", Stroke = "#4d90fe", Parent = 2 },
        new NodeData { Key = 10, Text = "squirrel", Fill =  "#f8f8f8", Stroke = "#4d90fe", Parent = 9 },
        new NodeData { Key = 11, Text = "VP", Fill =  "#f68c06", Stroke = "#4d90fe", Parent = 1 },
        new NodeData { Key = 12, Text = "VBZ", Fill =  "#ccc", Stroke = "#4d90fe", Parent = 11 },
        new NodeData { Key = 13, Text = "has", Fill =  "#f8f8f8", Stroke = "#4d90fe", Parent = 12 },
        new NodeData { Key = 14, Text = "VP", Fill =  "#f68c06", Stroke = "#4d90fe", Parent = 11 },
        new NodeData { Key = 15, Text = "VBN", Fill =  "#ccc", Stroke = "#4d90fe", Parent = 14 },
        new NodeData { Key = 16, Text = "become", Fill =  "#f8f8f8", Stroke = "#4d90fe", Parent = 15 },
        new NodeData { Key = 17, Text = "NP", Fill =  "#f68c06", Stroke = "#4d90fe", Parent = 14 },
        new NodeData { Key = 18, Text = "NP", Fill =  "#f68c06", Stroke = "#4d90fe", Parent = 17 },
        new NodeData { Key = 19, Text = "DT", Fill =  "#ccc", Stroke = "#4d90fe", Parent = 18 },
        new NodeData { Key = 20, Text = "a", Fill =  "#f8f8f8", Stroke = "#4d90fe", Parent = 19 },
        new NodeData { Key = 21, Text = "JJ", Fill =  "#ccc", Stroke = "#4d90fe", Parent = 18 },
        new NodeData { Key = 22, Text = "regular", Fill =  "#f8f8f8", Stroke = "#4d90fe", Parent = 21 },
        new NodeData { Key = 23, Text = "NN", Fill =  "#ccc", Stroke = "#4d90fe", Parent = 18 },
        new NodeData { Key = 24, Text = "visitor", Fill =  "#f8f8f8", Stroke = "#4d90fe", Parent = 23 },
        new NodeData { Key = 25, Text = "PP", Fill =  "#f68c06", Stroke = "#4d90fe", Parent = 17 },
        new NodeData { Key = 26, Text = "TO", Fill =  "#ccc", Stroke = "#4d90fe", Parent = 25 },
        new NodeData { Key = 27, Text = "to", Fill =  "#f8f8f8", Stroke = "#4d90fe", Parent = 26 },
        new NodeData { Key = 28, Text = "NP", Fill =  "#f68c06", Stroke = "#4d90fe", Parent = 25 },
        new NodeData { Key = 29, Text = "DT", Fill =  "#ccc", Stroke = "#4d90fe", Parent = 28 },
        new NodeData { Key = 30, Text = "a", Fill =  "#f8f8f8", Stroke = "#4d90fe", Parent = 29 },
        new NodeData { Key = 31, Text = "JJ", Fill =  "#ccc", Stroke = "#4d90fe", Parent = 28 },
        new NodeData { Key = 32, Text = "suburban", Fill =  "#f8f8f8", Stroke = "#4d90fe", Parent = 31 },
        new NodeData { Key = 33, Text = "NN", Fill =  "#ccc", Stroke = "#4d90fe", Parent = 28 },
        new NodeData { Key = 34, Text = "garden", Fill =  "#f8f8f8", Stroke = "#4d90fe", Parent = 33 },
        new NodeData { Key = 35, Text = ".", Fill =  "#ccc", Stroke = "#4d90fe", Parent = 1 },
        new NodeData { Key = 36, Text = ".", Fill =  "#f8f8f8", Stroke = "#4d90fe", Parent = 35 }
      };

      // create the Model with data for the tree, and assign to the Diagram
      myDiagram.Model = new Model {
        NodeDataSource = nodeDataSource
      };
    }

  }

  public class FlatTreeLayout : TreeLayout {
    // This assumes the TreeLayout.angle == 90 -- growing downward
    protected override void CommitLayout() {
      base.CommitLayout(); // call base method first
      // find maximum Y position of all nodes
      var y = double.NegativeInfinity;

      foreach (var v in Network.Vertexes) {
        y = Math.Max(y, v.Node.Position.Y);
      }

      foreach (var v in Network.Vertexes) {
        if (v.DestinationEdges.Count == 0) {
          // shift the node down to Y
          v.Node.Position = new Point(v.Node.Position.X, y);
          // extend the last segment vertically
          v.Node.ToEndSegmentLength = Math.Abs(v.CenterY - y);
        } else { // restore to normal value
          v.Node.ToEndSegmentLength = 10;
        }
      }
    }
  }

  public class Model : TreeModel<NodeData, int, object> { }

  public class NodeData : Model.NodeData {
    public string Fill { get; set; }
    public string Stroke { get; set; }
  }

}
