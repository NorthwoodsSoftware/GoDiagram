/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Layouts.Extensions;
using Northwoods.Go.Models;

namespace Demo.Samples.DoubleTree {
  public partial class DoubleTree : DemoControl {
    private Diagram _Diagram;

    public DoubleTree() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      desc1.MdText = DescriptionReader.Read("Samples.DoubleTree.md");
    }

    private void Setup() {
      _Diagram.Layout =
        new DoubleTreeLayout() {
          DirectionFunction = (node) => {
            return node.Data != null && node.Data is NodeData && (node.Data as NodeData).Dir != "Left";
          }
        };

      // define all of the gradient brushes
      var graygrad = CreateLinearGradient("#F5F5F5", "#F1F1F1");
      var bluegrad = CreateLinearGradient("#CDDAF0", "#91ADDD");
      var yellowgrad = CreateLinearGradient("#FEC901", "#FEA200");
      var lavgrad = CreateLinearGradient("#EF9EFA", "#A570AD");

      _Diagram.NodeTemplate =
        new Node("Auto") { IsShadowed = true }
          .Add(
            // define the node's outer shape
            new Shape("RoundedRectangle") {
                Fill = graygrad, Stroke = "#D8D8D8"  // default fill is gray
              }
              .Bind("Fill", "Color"),
            // define the node's text
            new TextBlock {
                Margin = 5,
                Font = new Font("Segoe UI", 11, Northwoods.Go.FontWeight.Bold)
              }
              .Bind("Text", "Key")
          );

      _Diagram.LinkTemplate =
        new Link { Selectable = false }.Add(new Shape());

      // create the model for the Double Tree, can be either TreeModel or GraphLinksModel
      _Diagram.Model = new Model {
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
    private static Brush CreateLinearGradient(string start, string end) {
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
