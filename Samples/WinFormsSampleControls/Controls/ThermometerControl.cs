using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;

namespace WinFormsSampleControls.Thermometer {
  [ToolboxItem(false)]
  public partial class ThermometerControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public ThermometerControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"
        <p>
      This sample uses <a href=""intro/graduatedPanels.html"">Graduated Panels</a> and <code>Panel.AlignmentFocusName</code> to line up thermometer scales.
        </p>

        <p>
      The thermometers are resizable, with two types. For the first two (default), resizing the thermometer reduces
      or increases the range of the values. For the second two (<code>Type = ""scaling""</code>), resizing
      the thermometer keeps the range, and scales the thermometer.
        </p>
";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.Layout = new GridLayout {
        IsOngoing = false,
        WrappingColumn = 4
      };
      myDiagram.AnimationManager.IsEnabled = false;
      myDiagram.UndoManager.IsEnabled = true;
      myDiagram.ToolManager.ResizingTool = new ThermometerResizingTool();

      var ORIGINAL_HEIGHT = 400.0;

      /* The thermometer node consists of a Spot Panel with 6 children:
          0 = The BarSpace, which is the main element and
          1 = The Farenheit scale, on the left
          2 = The Celsius scale, on the right
          3 = The shape that represents alcohol or mercury in the thermometer
          4 = (Cosmetic) The stem that attaches to the bulb
          5 = (Cosmetic) The bulb

          The two Graduated Panels use alignmentFocusName to make sure their main Shapes
          line up with the Spot Panel"s main element, BarSpace.

          1|0|2
          1|0|2
          1|0|2
          1|0|2
          1|3|2
          1|3|2
           |4|
           |5|
      */

      myDiagram.NodeTemplate =
        new Node("Spot") {
            Resizable = true,
            ResizeElementName = "BarSpace",
            LocationElementName = "Bulb",
            LocationSpot = Spot.Center
          }
          .Add(
            new Shape {
                Name = "BarSpace",
                Width = 25,
                Height = ORIGINAL_HEIGHT,
                StrokeWidth = 0,
                Fill = "rgba(0,0,0,.05)"
              }
              .Bind(new Binding("Height").MakeTwoWay()),

            // Farenheit scale, on the left:
            new Panel("Graduated") {
                Background = "white",
                GraduatedMin = -40, GraduatedMax = 80,
                GraduatedTickBase = 0, GraduatedTickUnit = 1,
                Alignment = Spot.BottomLeft,
                AlignmentFocus = Spot.BottomLeft,
                AlignmentFocusName = "line"
              }
              .Bind("GraduatedMax", "", (d, _) => {
                var data = d as NodeData;
                if (data.Type == "scaling") return 80;
                return (data.Height * 0.3) - 40;
              })
              .Add(
                new Shape { Name = "line", GeometryString = "M0 0 V-" + ORIGINAL_HEIGHT, Stroke = "gray" }
                  .Bind("Height"),
                new Shape { AlignmentFocus = Spot.Bottom, Interval = 2, StrokeWidth = 1, GeometryString = "M0 0 V15" },
                new Shape { AlignmentFocus = Spot.Bottom, Interval = 10, StrokeWidth = 2, GeometryString = "M0 0 V20" },
                new TextBlock { Interval = 20, Font = new Font("Georgia", 22), AlignmentFocus = new Spot(1, 0.5, 20, 0) },
                // Mark 32 degrees on the farenheit scale:
                new TextBlock {
                    Interval = 32, GraduatedFunction = (n) => { return n == 32 ? "32—" : ""; },
                    Font = new Font("Georgia", 12, FontWeight.Bold), Stroke = "red", AlignmentFocus = new Spot(1, 0.5, 20, 0)
                  }
              ),

            // Mercury/alcohol bar, which represents the values of farenheit and celsius
            new Shape {
                Name = "Mercury",
                Width = 25, StrokeWidth = 0, Fill = "red",
                Alignment = Spot.Bottom,
                AlignmentFocus = Spot.Bottom,
              }
              .Bind("Fill", "Type", (t, _) => {
                return (t as string) == "scaling" ? "dimgray" : "red";
              })
              // To determine the level, look at both data.Farenheit and data.Celsius, but prefer celsius
              .Bind("Height", "", (d, _) => {
                var data = d as NodeData;
                var thermometerHeight = ORIGINAL_HEIGHT;
                if (data.Type == "scaling") thermometerHeight = data.Height;
                if (data.Celsius != null) {
                  // (celsius + minimum value) / (total celsius range) * height
                  return Math.Max(0, (((double)data.Celsius + 40) / 67) * thermometerHeight);
                } else if (data.Farenheit != null) {
                  // (farenheit + minimum value) / (total farenheit range) * height
                  return Math.Max(0, (((double)data.Farenheit + 40) / 120) * thermometerHeight);
                } else {
                  return 0;
                }
              })
              .Bind("MaxSize", "Height", (h, _) => {
                return new Size(double.NaN, h as double? ?? double.NaN);
              }),

            // Celsius scale, on the right:
            new Panel("Graduated") {
                Background = "white",
                // -40 to 27 because we picked -40 to 80 for farenheit, and want them to line up
                GraduatedMin = -40, GraduatedMax = 27,
                GraduatedTickBase = 0, GraduatedTickUnit = 1,
                Alignment = Spot.BottomRight,
                AlignmentFocus = Spot.BottomRight,
                AlignmentFocusName = "line2"
              }
              .Bind("GraduatedMax", "", (d, _) => {
                var data = d as NodeData;
                if (data.Type == "scaling") return 27;
                return (data.Height * 0.1675) - 40;
              })
              .Add(
                new Shape { Name = "line2", GeometryString = "M0 0 V-" + ORIGINAL_HEIGHT, Stroke = "gray" }
                  .Bind("Height"),
                new Shape { Interval = 2, StrokeWidth = 1, GeometryString = "M0 0 V15" },
                new Shape { Interval = 10, StrokeWidth = 2, GeometryString = "M0 0 V20" },
                new TextBlock {
                    Interval = 20, TextAlign = TextAlign.Left, Font = new Font("Georgia", 22),
                    AlignmentFocus = Spot.Left,
                    SegmentOffset = new Point(0, 22)
                  }
              ),

            // Cosmetic = The stem and bulb
            new Shape {
                Width = 25, Height = 10, StrokeWidth = 0, Fill = "red",
                Alignment = Spot.Bottom
              }
              .Bind("Fill", "Type", (t, _) => {
                return t as string == "scaling" ? "dimgray" : "red";
              }),
            new Shape {
                Figure = "Circle",
                Name = "Bulb",
                Width = 55,
                Height = 55,
                StrokeWidth = 0,
                Fill = "red",
                Alignment = Spot.Bottom,
                AlignmentFocus = Spot.Top,
              }
              .Bind("Fill", "Type", (t, _) => {
                return t as string == "scaling" ? "dimgray" : "red";
              })
          ); // end node template

      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Height = 400, Celsius = 20 },
          new NodeData { Height = 250, Celsius = -10 },
          new NodeData { Type = "scaling", Height = 400, Farenheit = 22 /*, Celsius = 0*/ },
          new NodeData { Type = "scaling", Height = 250, Farenheit = 68 /*, Celsius = 20*/ }
        }
      };
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public double Height { get; set; }
    public double? Celsius { get; set; }
    public string Type { get; set; } = "";
    public double? Farenheit { get; set; }
  }

  public class LinkData : Model.LinkData {

  }

  // extend resizingtool
  public class ThermometerResizingTool : ResizingTool {
    public override Adornment MakeAdornment(GraphObject resizeObj) {
      return new Adornment(PanelLayoutSpot.Instance) {
        LocationSpot = Spot.Center,
        Category = Name,
        AdornedElement = resizeObj
      }.Add(
        new Placeholder(),
        MakeHandle(resizeObj, Spot.Top)
      );
    }
  }

}
