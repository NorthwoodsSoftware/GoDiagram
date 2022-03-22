using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using System.ComponentModel;
using Northwoods.Go.Layouts.Extensions;

namespace WinFormsExtensionControls.Serpentine {
  [ToolboxItem(false)]
  public partial class SerpentineControl : System.Windows.Forms.UserControl {

    private Diagram myDiagram;
    public SerpentineControl() {
      InitializeComponent();

      Setup();
      goWebBrowser1.Html = @"
          <p>
          This sample demonstrates a custom Layout, SerpentineLayout, which assumes the graph consists of a chain of nodes.
          The layout is defined in its own file, as <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Layouts/Serpentine/SerpentineLayout.cs"">SerpentineLayout.cs</a>.
        </p>
        <p>
          It also has <a>Layout.IsViewportSized</a> set to true, so that resizing the Diagram will automatically re-layout.
        </p> 
";
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      myDiagram.IsTreePathToChildren = false;
      myDiagram.Layout = new SerpentineLayout();

      myDiagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance).Add(
          new Shape {
            Figure = "RoundedRectangle",
            Fill = "white"
          }.Bind("Fill", "Color"),
          new TextBlock {
            Margin = 4
          }.Bind("Text", "Key")
        );

      myDiagram.LinkTemplate =
        new Link {
          Routing = LinkRouting.Orthogonal,
          Corner = 5
        }.Add(
          new Shape(),
          new Shape { ToArrow = "Standard" }
        );

      // Create the Diagram's Model:
      myDiagram.Model = new Model {
        NodeParentKeyProperty = "Next",
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "Alpha", Next = "Beta", Color = "coral" },
          new NodeData { Key = "Beta", Next = "Gamma", Color = "tomato" },
          new NodeData { Key = "Gamma", Next = "Delta", Color = "goldenrod" },
          new NodeData { Key = "Delta", Next = "Epsilon", Color = "orange" },
          new NodeData { Key = "Epsilon", Next = "Zeta", Color = "coral" },
          new NodeData { Key = "Zeta", Next = "Eta", Color = "tomato" },
          new NodeData { Key = "Eta", Next = "Theta", Color = "goldenrod" },
          new NodeData { Key = "Theta", Next = "Iota", Color = "orange" },
          new NodeData { Key = "Iota", Next = "Kappa", Color = "coral" },
          new NodeData { Key = "Kappa", Next = "Lambda", Color = "tomato" },
          new NodeData { Key = "Lambda", Next = "Mu", Color = "goldenrod" },
          new NodeData { Key = "Mu", Next = "Nu", Color = "orange" },
          new NodeData { Key = "Nu", Next = "Xi", Color = "coral" },
          new NodeData { Key = "Xi", Next = "Omicron", Color = "tomato" },
          new NodeData { Key = "Omicron", Next = "Pi", Color = "goldenrod" },
          new NodeData { Key = "Pi", Next = "Rho", Color = "orange" },
          new NodeData { Key = "Rho", Next = "Sigma", Color = "coral" },
          new NodeData { Key = "Sigma", Next = "Tau", Color = "tomato" },
          new NodeData { Key = "Tau", Next = "Upsilon", Color = "goldenrod" },
          new NodeData { Key = "Upsilon", Next = "Phi", Color = "orange" },
          new NodeData { Key = "Phi", Next = "Chi", Color = "coral" },
          new NodeData { Key = "Chi", Next = "Psi", Color = "tomato" },
          new NodeData { Key = "Psi", Next = "Omega", Color = "goldenrod" },
          new NodeData { Key = "Omega", Color = "orange" }
        }
      };
    }
  }

  public class Model : TreeModel<NodeData, string, object> { }

  public class NodeData : Model.NodeData {
    public string Next { get; set; }
    public Brush Color { get; set; }
  }
}
