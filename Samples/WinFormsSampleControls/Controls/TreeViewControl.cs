using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.TreeView {
  [ToolboxItem(false)]
  public partial class TreeViewControl : System.Windows.Forms.UserControl {
    private Diagram MyDiagram;

    public TreeViewControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      goWebBrowser1.Html = @"
        <p>
      This shows how to create a traditional ""TreeView"" in a <b>GoDiagram</b> diagram.
      There are 500 nodes in the tree.
        </p>
        <p>
      The node template makes use of a ""TreeExpanderButton"" panel to implement the expand/collapse button.
      It also implements a custom DoubleClick function to allow nodes to be expanded/collapsed on double-click.
      Lastly, the source of the picture on each node is bound to two different properties, <a>Node.IsTreeLeaf</a> and
      <a>Node.IsTreeExpanded</a>; the <b>imageConverter</b> function is used to select the correct image
      based on these properties.
        </p>
        <p>
      There are two link templates in the source code, one which uses no lines, and one which connects the items with dotted lines.
        </p>  
        <p>
      See the <a href=""intro/buttons.html"">Intro page on Buttons</a> for more GoDiagram button information.
      The <a href=""TriStateCheckBoxTree"">Tri-state CheckBox Tree</a> sample demonstrates a ""tree view"" where each item           
      has a three - state checkbox. The <a href=""TreeMapper"">Tree Mapper</a> sample demonstrates how to map (draw associations) between items in two trees.
      The <a href=""UpdateDemo"">Update Demo</a> sample also uses a ""tree view"" for its own purposes.       
        </p>      
        <p> 
      The icons in this sample are from <a href=""https://icons8.com/"">icons8.com</a>
        </p>                      
";

    }

    private void Setup() {
      // use a V figure instead of MinusLine in the TreeExpanderButton
      Shape.DefineFigureGenerator("ExpandedLine", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(0, 0.25 * h, false)
            .Add(new PathSegment(SegmentType.Line, .5 * w, 0.75 * h))
            .Add(new PathSegment(SegmentType.Line, w, 0.25 * h)));
      });

      // use a sideways V figure instead of PlusLine in the TreeExpanderButton
      Shape.DefineFigureGenerator("CollapsedLine", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(0.25 * w, 0, false)
            .Add(new PathSegment(SegmentType.Line, 0.75 * w, .5 * h))
            .Add(new PathSegment(SegmentType.Line, 0.25 * w, h)));
      });

      MyDiagram = diagramControl1.Diagram;

      MyDiagram.AllowMove = false;
      MyDiagram.AllowCopy = false;
      MyDiagram.AllowDelete = false;
      MyDiagram.AllowHorizontalScroll = false;
      MyDiagram.Layout = new TreeLayout {
        Alignment = TreeAlignment.Start,
        Angle = 0,
        Compaction = TreeCompaction.None,
        LayerSpacing = 16,
        LayerSpacingParentOverlap = 1,
        NodeIndentPastParent = 1.0,
        NodeSpacing = 0,
        SetsPortSpot = false,
        SetsChildPortSpot = false
      };

      // takes a property change on either IsTreeLeaf or IsTreeExpanded and selects the correct image to use
      static string imageConverter(object prop, object picture) {
        var node = (picture as GraphObject).Part as Node;
        if (node.IsTreeLeaf) {
          return "https://img.icons8.com/small/2x/document.png";
        } else {
          if (node.IsTreeExpanded) {
            return "https://img.icons8.com/small/2x/opened-folder--v1.png";
          } else {
            return "https://img.icons8.com/small/2x/folder-invoices--v1.png";
          }
        }
      }

      MyDiagram.NodeTemplate = new Node {
        // no Adornment: instead change panel background color by binding to Node.IsSelected
        SelectionAdorned = false,
        // a custom function to allow expanding/collapsing on double-click
        // this uses similar logic to a TreeExpanderButton
        DoubleClick = (e, _node) => {
          var node = _node as Node;
          var cmd = MyDiagram.CommandHandler;
          if (node.IsTreeExpanded) {
            if (!cmd.CanCollapseTree(node)) return;
          } else {
            if (!cmd.CanExpandTree(node)) return;
          }
          e.Handled = true;
          if (node.IsTreeExpanded) {
            cmd.CollapseTree(node);
          } else {
            cmd.ExpandTree(node);
          }
        }
      }.Add(
        Builder.Make<Panel>("TreeExpanderButton")
          .Set(new {
            _TreeExpandedFigure = "ExpandedLine",
            _TreeCollapsedFigure = "CollapsedLine",
            ButtonBorder_Fill = "whitesmoke",
            ButtonBorder_Stroke = (Brush)null,
            _ButtonFillOver = "rgba(0, 128, 255, .25)",
            _ButtonStrokeOver = (Brush)null
          }),
        new Panel(PanelLayoutHorizontal.Instance) {
          Position = new Point(18, 0)
        }.Bind(
          new Binding("Background", "IsSelected", (s, _) => (bool)s ? "lightblue" : "white").OfElement()
        ).Add(
          new Picture {
            Width = 18,
            Height = 18,
            Margin = new Margin(0, 4, 0, 0),
            ImageStretch = ImageStretch.Uniform
          }.Bind(
            // bind the picture source on two properties of the Node
            // to display open folder, closed folder, or document
            new Binding("Source", "IsTreeExpanded", imageConverter).OfElement(),
            new Binding("Source", "IsTreeLeaf", imageConverter).OfElement()
          ),
          new TextBlock {
            Font = new Font("Segoe UI", 9)
          }.Bind("Text", "Key", (s, _) => "item " + s)
        ) // end Horizontal panel
      ); // end Node

      // without lines
      MyDiagram.LinkTemplate = new Link();

      // // with lines
      // MyDiagram.LinkTemplate = new Link {
      //   Selectable = false,
      //   Routing = LinkRouting.Orthogonal,
      //   FromEndSegmentLength = 4,
      //   ToEndSegmentLength = 4,
      //   FromSpot = new Spot(0.001, 1, 7, 0),
      //   ToSpot = Spot.Left
      // }.Add(
      //   new Shape {
      //     Stroke = "gray",
      //     StrokeDashArray = new double[] { 1, 2 }
      //   }
      //);

      // create a random tree
      var nodeDataSource = new List<NodeData> {
        new NodeData { Key = 1 }
      };
      var max = 500;
      var count = 1;
      while (count < max) {
        count = _MakeTree(3, count, max, nodeDataSource, nodeDataSource[0]);
      }
      MyDiagram.Model = new Model {
        NodeDataSource = nodeDataSource
      };
    }

    private int _MakeTree(int level, int count, int max, List<NodeData> nodeDataSource, NodeData parentdata) {
      var rand = new Random();
      var numchildren = rand.Next(10);
      for (var i = 0; i < numchildren; i++) {
        if (count >= max) return count;
        count++;
        var childdata = new NodeData { Key = count, Parent = parentdata.Key };
        nodeDataSource.Add(childdata);
        if (level > 0 && rand.NextDouble() > 0.5) {
          count = _MakeTree(level - 1, count, max, nodeDataSource, childdata);
        }
      }
      return count;
    }

  }

  public class Model : TreeModel<NodeData, int, object> { }

  public class NodeData : Model.NodeData {

  }

}
