/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace Demo.Extensions.ZoomSlider {
  public partial class ZoomSlider : DemoControl {
    private Diagram _Diagram;

    private Northwoods.Go.Extensions.ZoomSlider _Slider = null;
    public ZoomSlider() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      _InitSlider();
      plusBtn.Click += (e, obj) => plusClick();
      minusBtn.Click += (e, obj) => minusClick();

      desc1.MdText = DescriptionReader.Read("Extensions.ZoomSlider.md");
    }

    private void Setup() {
      // Create a new ZoomSlider, applying delegates to get, and set,
      // respectively the value associated with the slider.
      _Slider =
        new Northwoods.Go.Extensions.ZoomSlider(
          _Diagram,
          () => { return slider.Value; },
          _SetSlider
        );

      // use a minimal NodeTemplate, and the default LinkTemplate
      _Diagram.NodeTemplate = new Node(PanelType.Auto).Add(
        new Shape {
          Figure = "RoundedRectangle",
          StrokeWidth = 0
        }.Bind("Fill", "Color"),
        new TextBlock() {
          Margin = 5
        }.Bind("Text", "Key")
      );

      _Diagram.Model = new Model() {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "Alpha", Color = "red" },
          new NodeData { Key = "Beta", Color = "lightblue" },
          new NodeData { Key = "Gamma", Color = "yellow" },
          new NodeData { Key = "Delta", Color = "lightgreen" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = "Alpha", To = "Beta" },
          new LinkData { From = "Beta", To = "Beta" },
          new LinkData { From = "Alpha", To = "Gamma" },
          new LinkData { From = "Gamma", To = "Delta" }
        }
      };
    }

    private void Rescale() {
      _Slider.UpdateScale();
    }

    private void plusClick() {
      if (slider.Value <= slider.Maximum - 1) {
        slider.Value += 1;
      } else {
        slider.Value = slider.Maximum;
      }
    }

    private void minusClick() {
      if (slider.Value >= slider.Minimum + 1) {
        slider.Value -= 1;
      } else {
        slider.Value = slider.Minimum;
      }
    }
  }

  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public Brush Color { get; set; }
  }
  public class LinkData : Model.LinkData { }
}
