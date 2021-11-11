using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.PipeTree {
  [ToolboxItem(false)]
  public partial class PipeTreeControl : UserControl {
    private Diagram myDiagram;
    
    public PipeTreeControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      goWebBrowser1.Html = @"
        <p>
      This diagram does not display <a>Link</a>s.
      Instead the <a>TreeLayout.layerSpacing</a> is set to 0,
      so that each node and its children have no space between them.
        </p>
        <p>
      The <a>TreeLayout.treeStyle</a> is set to StyleAlternating,
      so that alternating layers of the tree grow in each of two directions.
      Each node's <a>TextBlock</a> is angled according to the direction
      of the layer of the tree that it is in, and the <a>Shape</a>'s
      size is set according to direction and the position of the node's children.
        </p>
";

    }

    private string getText(double flow) {
      if (flow < 10) return "SubLateral -- Current: " + flow + " gpm";
      if (flow < 25) return "Lateral -- Current: " + flow + " gpm";
      if (flow < 50) return "SubMain -- Curent: " + flow + " gpm";
      return "Main -- Max: 100 gpm  Current: " + flow + " gpm";
    }

    // check if the children of this node have all had their sizes changed from the initial one
    // if they have been updated, their widths and heights cannot be equal
    private bool areChildrenUpdated(Node node) {
      return node.FindTreeChildrenNodes().All(child => {
        var shape = child.FindElement("SHAPE");
        return shape.Width != shape.Height;
      });
    }

    // give all shapes the appropriate dimensions and text color, size, and orientation.
    private void updatePipes() {
      myDiagram = diagramControl1.Diagram;
      var updated = 1; // when this is 0, no more nodes are in need of updating
      while (updated != 0) {
        // have layout determine node position first, but don't draw
        myDiagram.Layout.DoLayout();
        updated = 0;
        var nodes = myDiagram.Nodes;
        foreach (var node in nodes) {
          var shape = node.FindElement("SHAPE") as Shape;
          if (!areChildrenUpdated(node) || !(shape.Width == shape.Height)) { continue; }
          // update the node if all of its children have been updated and it has not
          // this allows its size to be determined based on its childrens' positions once they have been updated and repositioned
          else { updated++; }
          // depending on the lightness of the node's color, make the text black or white
          if (shape.Fill.IsDark()) (node.FindElement("TEXTBLOCK") as TextBlock).Stroke = "white";
          var horiz = false;
          var linkIn = node.FindTreeParentLink();
          if (linkIn == null) horiz = true;
          // the root node grows horizontally from the left, as do any nodes with links entering their left side
          else horiz = linkIn.ToSpot == Spot.Left;
          var longSide = 70.0;
          // the length of the longer side of the shape
          if (node.FindTreeChildrenLinks().Count() == 0) longSide = 170;
          var shortSide = 20.0;
          // the length of the shorter side of the shape
          var flow = (node.Data as NodeData).Flow;
          // size of the shape depends on the node's "current"
          if (flow > 20) { shortSide = 50; longSide += 30; }
          if (flow > 50) shortSide = 100;
          // font size also depends on current
          (node.FindElement("TEXTBLOCK") as TextBlock).Font = $"Segoe UI, {Math.Floor(10 + flow / 11)}px";
          var chl = node.FindTreeChildrenNodes();
          if (horiz) {
            var min = node.Location.X;
            var max = min;
            foreach (var value in chl) {
              if (min == max) if (value.Location.X < min) min = value.Location.X;
              if (value.Location.X > max) max = value.Location.X;
            }
            longSide += max - min;
            // make sure the shape is large enough to reach all children
            if (longSide < 160) longSide = 160;
            // a minimum shape size
            shape.Height = shortSide;
            shape.Width = longSide;
            // the horizontal side is longer; set the shape's size
          } else {
            var min = node.Location.Y;
            var max = min;
            foreach (var value in chl) {
              if (min == max) if (value.Location.Y < min) min = value.Location.Y;
              if (value.Location.Y > max) max = value.Location.Y;
            }
            longSide += max - min;
            if (longSide < 160) longSide = 160;
            shape.Height = longSide;
            shape.Width = shortSide;
            // the longer size is the vertical one in this case
            (node.FindElement("TEXTBLOCK") as TextBlock).Angle = 90;
            // rotate the textblock if the shape is longer vertically
          }
        }
      }
    }

    private void Setup() {

      myDiagram = diagramControl1.Diagram;
      myDiagram.IsReadOnly = true;

      myDiagram.Layout = new TreeLayout {
        TreeStyle = TreeStyle.Alternating,
        Angle = 90,
        LayerSpacing = 0,
        AlternateAngle = 0,
        AlternateLayerSpacing = 0
      };

      // the node template
      // the shape will be resized appropriately when the model is set up
      myDiagram.NodeTemplate = new Node(PanelLayoutAuto.Instance).Add(
        new Shape {
          Figure = "Rectangle",
          Name = "SHAPE",
          Width = 30, Height = 30
        }.Bind("Fill", "Color").Bind("Stroke", "Color"),
        new TextBlock {
          Name = "TEXTBLOCK", Margin = 5
        }.Bind("Text", "Flow", (flow, _) => getText((double)flow))
      );

      // the Links should have no graphical representation
      myDiagram.LinkTemplate = new Link();

      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = 1, Flow = 92, Color = "#808080" },
          new NodeData { Key = 2, Parent = 1, Flow = 47, Color = "#808080" },
          new NodeData { Key = 3, Parent = 1, Flow = 45, Color = "#808080" },
          new NodeData { Key = 4, Parent = 2, Flow = 15, Color = "#808080" },
          new NodeData { Key = 5, Parent = 2, Flow = 17, Color = "#808080" },
          new NodeData { Key = 6, Parent = 2, Flow = 15, Color = "#808080" },
          new NodeData { Key = 7, Parent = 5, Flow = 8, Color = "#FFFF00" },
          new NodeData { Key = 8, Parent = 5, Flow = 9, Color = "#FF0000" },
          new NodeData { Key = 9, Parent = 6, Flow = 5, Color = "#808080" },
          new NodeData { Key = 10, Parent = 6, Flow = 5, Color = "#808080" },
          new NodeData { Key = 11, Parent = 6, Flow = 5, Color = "#808080" }
        }
      };

      myDiagram.DelayInitialization(updatePipes);
    }

  }

  public class Model : TreeModel<NodeData, int, object> { }

  public class NodeData : Model.NodeData {
    public double Flow { get; set; }
    public string Color { get; set; }
  }

}
