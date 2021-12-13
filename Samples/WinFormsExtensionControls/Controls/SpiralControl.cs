using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using System.ComponentModel;
using Northwoods.Go.Layouts.Extensions;

namespace WinFormsExtensionControls.Spiral {
  [ToolboxItem(false)]
  public partial class SpiralControl : System.Windows.Forms.UserControl {
    public Diagram myDiagram;
    public SpiralControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;
      goWebBrowser1.Html = @"
           <p>
          This sample demonstrates a custom Layout, SpiralLayout, which assumes the graph consists of a chain of nodes.
          The layout is defined in its own file, as <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Layouts/Spiral/SpiralLayout.cs"">SpiralLayout.cs</a>.
          </p>
";
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      myDiagram.InitialAutoScale = AutoScaleType.Uniform;
      myDiagram.IsTreePathToChildren = false; // links go from child to parent
      myDiagram.Layout = new SpiralLayout {
        Clockwise = true
      };

      myDiagram.NodeTemplate = new Node(PanelLayoutAuto.Instance) {
        LocationSpot = Spot.Center
      }.Add(
        new Shape {
          Figure = "Circle",
          Fill = "white"
        }.Bind("Fill", "Color"),
        new TextBlock {
          Margin = 4
        }.Bind("Text", "Key")
      );

      myDiagram.LinkTemplate = new Link() {
        Curve = LinkCurve.Bezier,
        Curviness = 10
      }.Add(
        new Shape(),
        new Shape() { ToArrow = "Standard" }
      );

      myDiagram.Model = new Model {
        NodeParentKeyProperty = "Next",
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "Alpha", Next = "Beta", Color = Brush.RandomColor(128) },
          new NodeData { Key = "Beta", Next = "Gamma", Color = Brush.RandomColor(128) },
          new NodeData { Key = "Gamma", Next = "Delta", Color = Brush.RandomColor(128) },
          new NodeData { Key = "Delta", Next = "Epsilon", Color = Brush.RandomColor(128) },
          new NodeData { Key = "Epsilon", Next = "Zeta", Color = Brush.RandomColor(128) },
          new NodeData { Key = "Zeta", Next = "Eta", Color = Brush.RandomColor(128) },
          new NodeData { Key = "Eta", Next = "Theta", Color = Brush.RandomColor(128) },
          new NodeData { Key = "Theta", Next = "Iota", Color = Brush.RandomColor(128) },
          new NodeData { Key = "Iota", Next = "Kappa", Color = Brush.RandomColor(128) },
          new NodeData { Key = "Kappa", Next = "Lambda", Color = Brush.RandomColor(128) },
          new NodeData { Key = "Lambda", Next = "Mu", Color = Brush.RandomColor(128) },
          new NodeData { Key = "Mu", Next = "Nu", Color = Brush.RandomColor(128) },
          new NodeData { Key = "Nu", Next = "Xi", Color = Brush.RandomColor(128) },
          new NodeData { Key = "Xi", Next = "Omicron", Color = Brush.RandomColor(128) },
          new NodeData { Key = "Omicron", Next = "Pi", Color = Brush.RandomColor(128) },
          new NodeData { Key = "Pi", Next = "Rho", Color = Brush.RandomColor(128) },
          new NodeData { Key = "Rho", Next = "Sigma", Color = Brush.RandomColor(128) },
          new NodeData { Key = "Sigma", Next = "Tau", Color = Brush.RandomColor(128) },
          new NodeData { Key = "Tau", Next = "Upsilon", Color = Brush.RandomColor(128) },
          new NodeData { Key = "Upsilon", Next = "Phi", Color = Brush.RandomColor(128) },
          new NodeData { Key = "Phi", Next = "Chi", Color = Brush.RandomColor(128) },
          new NodeData { Key = "Chi", Next = "Psi", Color = Brush.RandomColor(128) },
          new NodeData { Key = "Psi", Next = "Omega", Color = Brush.RandomColor(128) },
          new NodeData { Key = "Omega", Color = Brush.RandomColor(128) }
        }
      };
    }
  }

  public class Model : TreeModel<NodeData, string, object> { };

  public class NodeData : Model.NodeData {
    public Brush Color { get; set; }
    public string Next { get; set; }
  }
}
