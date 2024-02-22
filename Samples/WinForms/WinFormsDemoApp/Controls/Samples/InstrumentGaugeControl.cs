/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.PanelLayouts;

namespace Demo.Samples.InstrumentGauge {
  [ToolboxItem(false)]
  public partial class InstrumentGaugeControl : DemoControl {
    private Diagram myDiagram;

    public InstrumentGaugeControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"

  <p>
      This makes use of a <a href=""intro/graduatedPanels.html"">""Graduated""</a> <a>Panel</a>,
      which holds the main path of the scale, a Shape whose <a>Shape.Geometry</a> is a circular arc.
      In addition that Graduated Panel holds three different Shapes acting as templates for tick marks and
      a TextBlock acting as a template for tick labels.
    </p>
    <p>
      In a Spot Panel with the Graduated Panel scale are an italic TextBlock showing the node identifier and a red elongated diamond ""needle"" Shape.
      The needle's angle is determined by <code>ConvertValueToAngle</code>, which finds the point on the Graduated Panel's
      main path element corresponding to <code>Data.Value</code> and computes the angle from the center to that point.
      The data value is updated several times per second.
      A circle Shape surrounds the Spot Panel.
    </p>
";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // node template
      myDiagram.NodeTemplate =
        new Node(PanelType.Auto).Add(
          new Shape {
            Figure = "Circle",
            Stroke = "orange",
            StrokeWidth = 5,
            Spot1 = Spot.TopLeft,
            Spot2 = Spot.BottomRight
          }.Bind(
            new Binding("Stroke", "Color")
          ),
          new Panel(PanelType.Spot).Add(
            new Panel(PanelType.Graduated) {
              Name = "SCALE",
              Margin = 14,
              GraduatedTickUnit = 2.5,  // tick marks at each multiple of 2.5
              GraduatedMax = 100,  // this is actually the default value
              Stretch = Stretch.None  // needed to avoid unnecessary re-measuring!!!
            }.Bind(
              new Binding("GraduatedMax", "Max")
            ).Add(  // controls the range of the gauge
                    // the main path of the graduated panel, an arc starting at 135 degrees and sweeping for 270 degrees
              new Shape {
                Name = "SHAPE",
                GeometryString = "M-70.7107 70.7107 B135 270 0 0 100 100 M0 100",
                Stroke = "white",
                StrokeWidth = 4
              },
              // three differently sized tick marks
              new Shape {
                GeometryString = "M0 0 V10",
                Stroke = "white",
                StrokeWidth = 1.5
              },
              new Shape {
                GeometryString = "M0 0 V12",
                Stroke = "white",
                StrokeWidth = 2.5,
                Interval = 2
              },
              new Shape {
                GeometryString = "M0 0 V15",
                Stroke = "white",
                StrokeWidth = 3.5,
                Interval = 4
              },
              new TextBlock { // each tick label
                Interval = 4,
                AlignmentFocus = Spot.Center,
                Font = new Font("Segoe UI", 19, FontStyle.Italic, FontWeight.Bold),
                Stroke = "white",
                SegmentOffset = new Point(0, 30)
              }
            ),
            new TextBlock {
              Alignment = new Spot(0.5, 0.9),
              Stroke = "orange",
              Font = new Font("Segoe UI", 19, FontStyle.Italic, FontWeight.Bold)
            }.Bind(
              new Binding("Text", "Key"),
              new Binding("Stroke", "Color")
            ),
            new Shape {
              Fill = "red",
              StrokeWidth = 0,
              GeometryString = "F1 M-6 0 L0 -6 100 0 0 6z x M-100 0"
            }.Bind(
              new Binding("Angle", "Value", ConvertValueToAngle)
            ),
            new Shape {
              Figure = "Circle",
              Width = 2,
              Height = 2,
              Fill = "#444"
            }
          )
        );

      // this determines the angle of the needle, based on the data.Value argument
      object ConvertValueToAngle(object vIn, object shapeIn) {
        var v = vIn as double? ?? 0;
        var shape = shapeIn as Shape;
        var scale = shape.Part.FindElement("SCALE") as Panel;
        var p = scale.GraduatedPointForValue(v);
        shape = shape.Part.FindElement("SHAPE") as Shape;
        var c = shape.ActualBounds.Center;
        return c.Direction(p);
      }

      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "Alpha", Value = 35 },
          new NodeData { Key = "Beta", Color = "green", Max = 140, Value = 70 }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = "Alpha", To = "Beta" }
        }
      };

      // capture RNG so we don't keep re-seeding
      var rand = new Random();
      void Loop() {
        // change each gauge's value several times a second
        Task.Delay((1000 / 6)).ContinueWith((t) => {
          myDiagram.StartTransaction();
          foreach (var node in myDiagram.Nodes) {
            var scale = node.FindElement("SCALE") as Panel;
            if (scale == null || scale.Type != PanelLayoutGraduated.Instance) return;
            // keep the new value within the range of the graduated panel
            var min = scale.GraduatedMin;
            var max = scale.GraduatedMax;
            var v = (node.Data as NodeData).Value;
            if (v < min) v++;
            else if (v > max) v--;
            else v += (rand.NextDouble() < 0.5) ? -0.5 : 0.5;  // random walk
            myDiagram.Model.Set(node.Data, "Value", v);
          }
          myDiagram.CommitTransaction("modified Graduated Panel");
          Loop();
        });
      }
      // start the sim
      Loop();
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Color { get; set; }
    public double Value { get; set; }
    public double? Max { get; set; }
  }

  public class LinkData : Model.LinkData { }

}
