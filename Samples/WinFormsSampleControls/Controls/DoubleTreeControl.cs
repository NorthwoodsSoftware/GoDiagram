using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Layouts.Extensions;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.DoubleTree {
  [ToolboxItem(false)]
  public partial class DoubleTreeControl : System.Windows.Forms.UserControl {
    private Diagram MyDiagram;

    public DoubleTreeControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;
      goWebBrowser1.Html = @"
         <p>
          This sample displays a bow-tie diagram of two trees sharing a single root node growing in opposite directions.
          The immediate child data of the root node have a ""Dir"" property
          that describes the direction that subtree should grow.
        </p>
        <p>
          The <a>Diagram.Layout</a> is an instance of the <a>DoubleTreeLayout</a> extension layout,
          defined in <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Layouts/DoubleTree/DoubleTreeLayout.cs"">DoubleTreeLayout.cs</a>.
          The layout requires a <a>DoubleTreeLayout.DirectionFunction</a> predicate to decide for a child node
          of the root node which way the subtree should grow.
        </p>
      ";

    }

    private void Setup() {
      MyDiagram = diagramControl1.Diagram;

      MyDiagram.Layout = new DoubleTreeLayout() {
        DirectionFunction = (node) => {
          return node.Data != null && node.Data is NodeData && (node.Data as NodeData).Dir != "Left";
        }
      };

      // define all of the gradient brushes
      var graygrad = CreateLinearGradient("#F5F5F5", "#F1F1F1");
      var bluegrad = CreateLinearGradient("#CDDAF0", "#91ADDD");
      var yellowgrad = CreateLinearGradient("#FEC901", "#FEA200");
      var lavgrad = CreateLinearGradient("#EF9EFA", "#A570AD");

      MyDiagram.NodeTemplate = new Node(PanelLayoutAuto.Instance) {
        IsShadowed = true
      }.Add(
        // define the node's outer shape
        new Shape("RoundedRectangle") {
          Fill = graygrad, Stroke = "#D8D8D8"  // default fill is gray
        }.Bind("Fill", "Color"),
        // define the node's text
        new TextBlock {
          Margin = 5,
          Font = "Segoe UI, 11px, style=bold"
        }.Bind("Text", "Key")
      );

      MyDiagram.LinkTemplate = new Link {
        Selectable = false
      }.Add(
        new Shape()
      );

      // create the model for the Double Tree, can be either TreeModel or GraphLinksModel
      MyDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "Root", Color = lavgrad },
          new NodeData { Key = "Left1", Parent = "Root", Dir = "Left", Color = bluegrad },
          new NodeData { Key = "Leaf1", Parent = "Left1" },
          new NodeData { Key = "Leaf2", Parent = "Left1" },
          new NodeData { Key = "Left2", Parent = "Left1", Color = bluegrad },
          new NodeData { Key = "Leaf3", Parent = "Left2" },
          new NodeData { Key = "Leaf4", Parent = "Left2" },
          new NodeData { Key = "Leaf5", Parent = "Left1" },
          new NodeData { Key = "Right1", Parent = "Root", Dir = "Right", Color = yellowgrad },
          new NodeData { Key = "Right2", Parent = "Right1", Color = yellowgrad },
          new NodeData { Key = "Leaf11", Parent = "Right2" },
          new NodeData { Key = "Leaf12", Parent = "Right2" },
          new NodeData { Key = "Leaf13", Parent = "Right2" },
          new NodeData { Key = "Leaf14", Parent = "Right1" },
          new NodeData { Key = "Leaf15", Parent = "Right1" },
          new NodeData { Key = "Right3", Parent = "Root", Dir = "Right", Color = yellowgrad },
          new NodeData { Key = "Leaf16", Parent = "Right3" },
          new NodeData { Key = "Leaf17", Parent = "Right3" }
        }
      };
    }

    /// <summary>
    /// Create a brush representing a linear gradient between the two colors represented by the parameters.
    /// </summary>
    /// <param name="start">The desired color at the <see cref="Spot.Top"/> of the gradient</param>
    /// <param name="end">The desired color at the <see cref="Spot.Bottom"/> of the gradient</param>
    /// <returns></returns>
    protected Brush CreateLinearGradient(string start, string end) {
      var paint = new LinearGradientPaint(new Dictionary<float, string> {
        { 0, start }, { 1, end }
      });
      return new Brush(paint);
    }

  }

  public class Model : TreeModel<NodeData, string, object> { }

  public class NodeData : Model.NodeData {
    public string Dir { get; set; }
    public Brush Color { get; set; } = "gray";
  }

}
