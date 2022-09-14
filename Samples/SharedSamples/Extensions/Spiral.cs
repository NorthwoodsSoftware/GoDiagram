/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts.Extensions;

namespace Demo.Extensions.Spiral {
  public partial class Spiral : DemoControl {
    public Diagram _Diagram;

    public Spiral() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();
      desc1.MdText = DescriptionReader.Read("Extensions.Spiral.md");
    }

    private void Setup() {
      _Diagram.InitialAutoScale = AutoScale.Uniform;
      _Diagram.IsTreePathToChildren = false; // links go from child to parent
      _Diagram.Layout = new SpiralLayout {
        Clockwise = true
      };

      _Diagram.NodeTemplate = new Node(PanelType.Auto) {
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

      _Diagram.LinkTemplate = new Link() {
        Curve = LinkCurve.Bezier,
        Curviness = 10
      }.Add(
        new Shape(),
        new Shape() { ToArrow = "Standard" }
      );

      _Diagram.Model = new Model {
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
