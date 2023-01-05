/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts.Extensions;

namespace Demo.Extensions.Serpentine {
  public partial class Serpentine : DemoControl {
    private Diagram _Diagram;

    public Serpentine() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();
      desc1.MdText = DescriptionReader.Read("Extensions.Serpentine.md");
    }

    private void Setup() {
      _Diagram.IsTreePathToChildren = false;
      _Diagram.Layout = new SerpentineLayout();

      _Diagram.NodeTemplate =
        new Node(PanelType.Auto).Add(
          new Shape {
            Figure = "RoundedRectangle",
            Fill = "white"
          }.Bind("Fill", "Color"),
          new TextBlock {
            Margin = 4
          }.Bind("Text", "Key")
        );

      _Diagram.LinkTemplate =
        new Link {
          Routing = LinkRouting.Orthogonal,
          Corner = 5
        }.Add(
          new Shape(),
          new Shape { ToArrow = "Standard" }
        );

      // Create the Diagram's Model:
      _Diagram.Model = new Model {
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
