﻿using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using System.ComponentModel;
using Northwoods.Go.Extensions;

namespace WinFormsExtensionControls.ZoomSliderSample {
  [ToolboxItem(false)]
  public partial class ZoomSliderSampleControl : System.Windows.Forms.UserControl {

    private Diagram myDiagram;
    private double _ZoomValue;

    private ZoomSlider _Slider = null;
    public ZoomSliderSampleControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      trackBar1.ValueChanged += (e, obj) => Rescale();
      plusButton.Click += (e, obj) => plusClick();
      minusButton.Click += (e, obj) => minusClick();

      goWebBrowser1.Html = @"
          <p>
            This sample demostrates the use of the ZoomSlider extension.
            When using the ZoomSlider extension, the diagram Div must always be a direct child
            of a Div using <code>position: relative</code> or <code>position: absolute</code>,
            so that the ZoomSlider HTML DOM can be positioned correctly relative to the Diagram Div.
          </p>
";
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      

      // Create a new ZoomSlider, applying delegates to get, and set, respectively the value associated with
      // the slider.
      _Slider = new ZoomSlider(myDiagram, () => { return ZoomValue; },
                                          (z) => { ZoomValue = z; });

      // use a minimal NodeTemplate, and the default LinkTemplate
      myDiagram.NodeTemplate = new Node(PanelLayoutAuto.Instance).Add(
        new Shape {
          Figure = "RoundedRectangle",
          StrokeWidth = 0
        }.Bind("Fill", "Color"),
        new TextBlock() {
          Margin = 5
        }.Bind("Text", "Key")
      );

      myDiagram.Model = new Model() {
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
      var val = trackBar1.Value / 100.0;
      myDiagram = diagramControl1.Diagram;
      if (myDiagram == null) return;

      myDiagram.StartTransaction("rescale");
      myDiagram.Scale = val;
      myDiagram.UpdateAllTargetBindings();
      myDiagram.CommitTransaction("rescale");
    }

    private void plusClick() {
      if (trackBar1.Value <= trackBar1.Maximum - 50) {
        trackBar1.Value += 50;
      } else {
        trackBar1.Value = trackBar1.Maximum;
      }
    }

    private void minusClick() {
      if (trackBar1.Value >= trackBar1.Minimum + 50) {
        trackBar1.Value -= 50;
      } else {
        trackBar1.Value = trackBar1.Minimum;
      }
    }

    public double ZoomValue {
      get {
        return _ZoomValue;
      }
      set {
        if (_ZoomValue != value) {
          _ZoomValue = value;
          // Directly apply an update to the Diagram.Scale when the ZoomSlider's value is changed
          if (_Slider != null) {
            _Slider.UpdateScale();
          }
        }
      }
    }
  }

  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }

  public class NodeData : Model.NodeData {
    public Brush Color { get; set; }
  }

  public class LinkData : Model.LinkData { }

}