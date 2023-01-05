/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace Demo.Samples.ControlGauges {
  [ToolboxItem(false)]
  public partial class ControlGaugesControl : DemoControl {
    private Diagram myDiagram;

    public ControlGaugesControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"

  <p>
    Instruments are Panels that include:
  </p>
  <ul>
    <li>a scale which is a ""Graduated"" Panel showing a possible range of values</li>
    <li> one or more indicators that show the instrument's value</li>
  </ul>
  <p>
    Optionally there are other TextBlocks or Shapes that show additional information.
    Indicators can be Shapes or TextBlocks or more complicated Panels.
    For more about scales, please read <a href=""intro/graduatedPanels.html"">Graduated Panels</a>.
    For simplicity, all of these instruments only show one value.
    But you could define instruments that show multiple values on the same scale,
    or that have multiple scales.
  </p>
  <p>
    When an instrument is also a control, the user can modify the instrument's value.
    When the instrument is editable, there may be a handle that the user can drag.
    This might be the same as the indicator or might be a different object.
  </p>
  <p>
    This sample defines five different types of instruments.
  </p>
  <ul>
    <li><b>Horizontal</b>, a horizontal scale with a bar indicator and a slider handle</li>
    <li><b>Vertical</b>, a vertical scale with a bar indicator and a slider handle</li>
    <li><b>NeedleMeter</b>, a curved scale with a straight needle indicator</li>
    <li><b>CircularMeter</b>, a circular scale with a polygonal needle indicator</li>
    <li><b>BarMeter</b>, a circular scale with an annular bar indicator</li>
  </ul>
  <p>
    The value to be shown by the instrument is assumed to be the <code>Data.Value</code> property.
    The value is shown both textually in a TextBlock and graphically using an indicator on the scale.
    If the value of<code>Data.Editable</code> is true,
  </p>
  <ul>
    <li>
      the user can drag something to change the instrument's value --
      the value is limited by the<a>Panel.GraduatedMin</a> and <a>Panel.GraduatedMax</a> values
    </li>
    <li>the user can in-place edit the TextBlock showing the value (if the node is selected, hit the F2 key)</li>
  </ul>
  <p>
    Of course you can change the details of anything you want to use.
    You might want to add more TextBlocks to show more information.
    A few properties already have data Bindings, such as:
  </p>
  <ul>
    <li><a>TextBlock.Text</a> from <code>Data.Text</code>, for the name of the instrument </li>
    <li><a>Panel.GraduatedMin</a> from <code>Data.Min</code>, to control the range of the scale </li>
    <li><a>Panel.GraduatedMax</a> from <code>Data.Max</code>, to control the range of the scale </li>
    <li>(various) from <code>Data.Color</code>, to control some colors used by the instrument </li>
  </ul>
";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.UndoManager.IsEnabled = true;

      // These properties are what change an object from being a value indicator,
      // such as a needle or a bar or a thumb of a slider, to being a controller
      // that the user can drag to change the value of the instrument.
      // This assumes that the scale (a "Graduated" Panel) is named "SCALE".
      object SliderActions() {
        return new {
          IsActionable = true,
          ActionDown = new Action<InputEvent, GraphObject>((e, obj) => {
            obj["_Dragging"] = true;
            obj["_Original"] = (obj.Part.Data as NodeData).Value;
          }),
          ActionMove = new Action<InputEvent, GraphObject>((e, obj) => {
            if (!(obj["_Dragging"] as bool? ?? false)) return;
            var scale = obj.Part.FindElement("SCALE") as Panel;
            var pt = e.Diagram.LastInput.DocumentPoint;
            var loc = scale.GetLocalPoint(pt);
            var val = Math.Round(scale.GraduatedValueForPoint(loc));
            // just set the data.Value temporarily, not recorded in UndoManager
            e.Diagram.Model.Commit((m) => {
              m.Set(obj.Part.Data, "Value", val);
            }, null);  // null means skipsUndoManager
          }),
          ActionUp = new Action<InputEvent, GraphObject>((e, obj) => {
            if (!(obj["_Dragging"] as bool? ?? false)) return;
            obj["_Dragging"] = false;
            var scale = obj.Part.FindElement("SCALE") as Panel;
            var pt = e.Diagram.LastInput.DocumentPoint;
            var loc = scale.GetLocalPoint(pt);
            var val = Math.Round(scale.GraduatedValueForPoint(loc));
            e.Diagram.Model.Commit((m) => {
              m.Set(obj.Part.Data, "Value", obj["_Original"]);
            }, null);  // null means skipsUndoManager
                       // now set the data.Value for real
            e.Diagram.Model.Commit((m) => {
              m.Set(obj.Part.Data, "Value", val);
            }, "dragged slider");
          }),
          ActionCancel = new Action<InputEvent, GraphObject>((e, obj) => {
            obj["_Dragging"] = false;
            e.Diagram.Model.Commit((m) => {
              m.Set(obj.Part.Data, "Value", obj["_Original"]);
            }, null);  // null means skipsUndoManager
          })
        };
      }

      // slider bindings
      Binding[] SliderBindings(bool alwaysVisible) {
        return (alwaysVisible ? // ternary operator to simplify into one return statement
          new Binding[] {
            new Binding("Visible", "IsEnabled").OfElement("SCALE"),
            new Binding("Cursor", "IsEnabled", (e, _) => { return (e as bool? ?? false) ? "pointer" : ""; }).OfElement("SCALE")
          }
          : // above this line is true for ternary operator, below is false
          new Binding[] {
            new Binding("Cursor", "IsEnabled", (e, _) => { return (e as bool? ?? false) ? "pointer" : ""; }).OfElement("SCALE")
          }
        );
      }

      // These helper functions simplify the node templates

      Binding[] CommonScaleBindings() {
        return new Binding[] {
          new Binding("GraduatedMin", "Min"),
          new Binding("GraduatedMax", "Max"),
          new Binding("GraduatedTickUnit", "Unit"),
          new Binding("IsEnabled", "Editable")
        };
      }

      Shape CommonSlider(bool vert) {
        return new Shape {
          Figure = "RoundedRectangle",
          Name = "SLIDER",
          Fill = "white",
          DesiredSize = (vert ? new Size(20, 6) : new Size(6, 20)),
          Alignment = (vert ? Spot.Top : Spot.Right)
        }.Set(
          SliderActions()
        ).Bind(
          SliderBindings(false)
        );
      }

      object CommonNodeStyle() {
        return new {
          LocationSpot = Spot.Center,
          FromSpot = Spot.BottomRightSides,
          ToSpot = Spot.TopLeftSides
        };
      }

      Binding CommonNodeBinding() {
        return new Binding("Location", "Loc", Point.Parse).MakeTwoWay(Point.Stringify);
      }

      myDiagram.NodeTemplateMap.Add("Horizontal",
        new Node(PanelType.Auto).Set(
          CommonNodeStyle()
        ).Bind(
          CommonNodeBinding()
        ).Add(
          new Shape { Fill = "lightgray", Stroke = "gray" },
          new Panel(PanelType.Table) { Margin = 1, Stretch = Stretch.Fill }.Add(
            // header information
            new TextBlock { Row = 0, Font = new Font("Segoe UI", 13, FontWeight.Bold) }.Bind(
              new Binding("Text")
            ),
            new Panel(PanelType.Spot) {
              Row = 1
            }.Add(
              new Panel(PanelType.Graduated) {
                Name = "SCALE",
                Margin = new Margin(0, 6),
                GraduatedTickUnit = 10,
                IsEnabled = false
              }.Bind(
                CommonScaleBindings()
              ).Add(
                new Shape { GeometryString = "M0 0 H200", Height = 0, Name = "PATH" },
                new Shape { GeometryString = "M0 0 V16", AlignmentFocus = Spot.Center, Stroke = "gray" },
                new Shape { GeometryString = "M0 0 V20", AlignmentFocus = Spot.Center, Interval = 5, StrokeWidth = 1.5 }
              ),
              new Panel(PanelType.Spot) {
                Alignment = Spot.Left,
                AlignmentFocus = Spot.Left,
                AlignmentFocusName = "BAR"
              }.Add(
                // the indicator (a bar)
                new Shape { Name = "BAR", Fill = "red", StrokeWidth = 0, Height = 8 }.Bind(
                  new Binding("Fill", "Color"),
                  new Binding("DesiredSize", "Value", (v, shp) => {
                    var scale = (shp as Shape).Part.FindElement("SCALE") as Panel;
                    var path = scale.FindMainElement() as Shape;
                    var len = ((v as double? ?? double.NaN) - scale.GraduatedMin) / (scale.GraduatedMax - scale.GraduatedMin) * path.Geometry.Bounds.Width;
                    return new Size(len, 10);
                  })
                ),
                CommonSlider(false)
              )
            ),
            // state information
            new TextBlock {
              Text = "0",
              Row = 2,
              Alignment = Spot.Left
            }.Bind(
              new Binding("Text", "Min", (min, _) => {
                return (min as double? ?? 0).ToString();
              })
            ),
            new TextBlock {
              Text = "100",
              Row = 2,
              Alignment = Spot.Right
            }.Bind(
              new Binding("Text", "Max", (max, _) => {
                return (max as double? ?? double.NaN).ToString();
              })
            ),
            new TextBlock {
              Row = 2,
              Background = "white",
              Font = new Font("Segoe UI", 13, FontWeight.Bold),
              IsMultiline = false,
              Editable = true
            }.Bind(
              new Binding("Text", "Value", (v, _) => {
                return (v as double? ?? double.NaN).ToString();
              }).MakeTwoWay((s, _, __) => { return double.Parse(s as string); })
            )
          )
        )
      );



      myDiagram.NodeTemplateMap.Add("Vertical",
        new Node(PanelType.Auto).Set(
          CommonNodeStyle()
        ).Bind(
          CommonNodeBinding()
        ).Add(
          // {
          //   Resizable = true,
          //   ResizeElementName = "PATH",
          //   resizeAdornmentTemplate:
          //     $(Adornment, "Spot",
          //       $(Placeholder),
          //       new Shape { Fill = "dodgerblue", Width = 8, Height = 8, Alignment = Spot.Top, Cursor = "n-resize" }))
          // },
          new Shape {
            Fill = "lightgray",
            Stroke = "gray"
          },
          new Panel(PanelType.Table) {
            Margin = 1,
            Stretch = Stretch.Fill
          }.Add(
            // header information
            new TextBlock {
              Row = 0,
              Font = new Font("Segoe UI", 13, FontWeight.Bold)
            }.Bind(
              new Binding("Text")
            ),
            new Panel(PanelType.Spot) {
              Row = 1
            }.Add(
              new Panel(PanelType.Graduated) {
                Name = "SCALE",
                Margin = new Margin(6, 0),
                GraduatedTickUnit = 10,
                IsEnabled = false
              }.Bind(
                CommonScaleBindings()
              ).Add(
                // NOTE = path goes upward!
                new Shape { GeometryString = "M0 0 V-200", Width = 0, Name = "PATH" },
                new Shape { GeometryString = "M0 0 V16", AlignmentFocus = Spot.Center, Stroke = "gray" },
                new Shape { GeometryString = "M0 0 V20", AlignmentFocus = Spot.Center, Interval = 5, StrokeWidth = 1.5 }
              ),
              new Panel(PanelType.Spot) {
                Alignment = Spot.Bottom,
                AlignmentFocus = Spot.Bottom,
                AlignmentFocusName = "BAR"
              }.Add(
                // the indicator (a bar)
                new Shape {
                  Name = "BAR",
                  Fill = "red",
                  StrokeWidth = 0,
                  Height = 8
                }.Bind(
                  new Binding("Fill", "Color"),
                  new Binding("DesiredSize", "Value", (v, shp) => {
                    var scale = (shp as Shape).Part.FindElement("SCALE") as Panel;
                    var path = scale.FindMainElement() as Shape;
                    var len = ((v as double? ?? double.NaN) - scale.GraduatedMin) / (scale.GraduatedMax - scale.GraduatedMin) * path.Geometry.Bounds.Height;
                    return new Size(10, len);
                  })
                ),
                CommonSlider(true)
              )
            ),
            // state information
            new TextBlock {
              Text = "0",
              Row = 2,
              Alignment = Spot.Left
            }.Bind(
              new Binding("Text", "Min", (min, _) => {
                return (min as double? ?? 0).ToString();
              })
            ),
            new TextBlock {
              Text = "100",
              Row = 2,
              Alignment = Spot.Right
            }.Bind(
              new Binding("Text", "Max", (max, _) => {
                return (max as double? ?? double.NaN).ToString();
              })
            ),
            new TextBlock {
              Row = 2,
              Background = "white",
              Font = new Font("Segoe UI", 13, FontWeight.Bold),
              IsMultiline = false,
              Editable = true
            }.Bind(
              new Binding("Text", "Value", (v, _) => {
                return (v as double? ?? double.NaN).ToString();
              }).MakeTwoWay((s, _, __) => { return double.Parse(s as string); })
            )
          )
        )
      );

      myDiagram.NodeTemplateMap.Add("NeedleMeter",
        new Node(PanelType.Auto).Set(
          CommonNodeStyle()
        ).Bind(
          CommonNodeBinding()
        ).Add(
          new Shape {
            Fill = "darkslategray"
          },
          new Panel(PanelType.Spot).Add(
            new Panel(PanelType.Position).Add(
              new Panel(PanelType.Graduated) {
                Name = "SCALE",
                Margin = 10
              }.Bind(
                CommonScaleBindings()
              ).Add(
                new Shape { Name = "PATH", GeometryString = "M0 0 A120 120 0 0 1 200 0", Stroke = "white" },
                new Shape { GeometryString = "M0 0 V10", Stroke = "white" },
                new TextBlock { SegmentOffset = new Point(0, 12), SegmentOrientation = Orientation.Along, Stroke = "white" }
              ),
              new Shape {
                Stroke = "red",
                StrokeWidth = 4,
                IsGeometryPositioned = true
              }.Bind(
                new Binding("Geometry", "Value", (v, shp) => {
                  var scale = (shp as Shape).Part.FindElement("SCALE") as Panel;
                  var pt = scale.GraduatedPointForValue(v as double? ?? double.NaN);
                  var geo = new Geometry(GeometryType.Line);
                  geo.StartX = 100 + scale.Margin.Left;
                  geo.StartY = 90 + scale.Margin.Top;
                  geo.EndX = pt.X + scale.Margin.Left;
                  geo.EndY = pt.Y + scale.Margin.Top;
                  return geo;
                })
              ).Set(
                SliderActions()
              ).Bind(
                SliderBindings(true)
              )
            ),
            new TextBlock {
              Alignment = new Spot(0.5, 0.5, 0, 20),
              Stroke = "white",
              Font = new Font("Segoe UI", 13, FontWeight.Bold)
            }.Bind(
              new Binding("Text"),
              new Binding("Stroke", "Color")
            ),
            new TextBlock {
              Alignment = Spot.Top,
              Margin = new Margin(4, 0, 0, 0),
              Stroke = "white",
              Font = new Font("Segoe UI", 17, FontStyle.Italic, FontWeight.Bold),
              IsMultiline = false,
              Editable = true
            }.Bind(
              new Binding("Text", "Value", (v, _) => {
                return (v as double? ?? double.NaN).ToString();
              }).MakeTwoWay((s, _, __) => { return float.Parse(s as string); }),
              new Binding("Stroke", "Color")
            )
          )
        )
      );

      myDiagram.NodeTemplateMap.Add("CircularMeter",
        new Node(PanelType.Table).Set(
          CommonNodeStyle()
        ).Bind(
          CommonNodeBinding()
        ).Add(
          new Panel(PanelType.Auto) {
            Row = 0
          }.Add(
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
                Stretch = Stretch.None  // needed to avoid unnecessary re-measuring!!!
              }.Bind(
                CommonScaleBindings()
              ).Add(
                // the main path of the graduated panel, an arc starting at 135 degrees and sweeping for 270 degrees
                new Shape { Name = "PATH", GeometryString = "M-70.7107 70.7107 B135 270 0 0 100 100 M0 100", Stroke = "white", StrokeWidth = 4 },
                // three differently sized tick marks
                new Shape { GeometryString = "M0 0 V10", Stroke = "white", StrokeWidth = 1 },
                new Shape { GeometryString = "M0 0 V12", Stroke = "white", StrokeWidth = 2, Interval = 2 },
                new Shape { GeometryString = "M0 0 V15", Stroke = "white", StrokeWidth = 3, Interval = 4 },
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
                Stroke = "white",
                Font = new Font("Segoe UI", 19, FontStyle.Italic, FontWeight.Bold),
                Editable = true
              }.Bind(
                new Binding("Text", "Value", (v, _) => {
                  return (v as double? ?? double.NaN).ToString();
                }).MakeTwoWay((s, _, __) => { return double.Parse(s as string); }),
                new Binding("Stroke", "Color")
              ),
              new Shape {
                Fill = "red",
                StrokeWidth = 0,
                GeometryString = "F1 M-6 0 L0 -6 100 0 0 6z x M-100 0"
              }.Bind(
                new Binding("Angle", "Value", (v, shp) => {
                  // this determines the angle of the needle, based on the data.Value argument
                  var scale = (shp as Shape).Part.FindElement("SCALE") as Panel;
                  var p = scale.GraduatedPointForValue((v as double? ?? double.NaN));
                  var path = (shp as Shape).Part.FindElement("PATH") as Shape;
                  var c = path.ActualBounds.Center;
                  return c.Direction(p);
                })
              ).Set(
                SliderActions()
              ).Bind(
                SliderBindings(true)
              ),
              new Shape {
                Figure = "Circle",
                Width = 2,
                Height = 2,
                Fill = "#444"
              }
            )
          ),
          new TextBlock {
            Row = 1,
            Font = new Font("Segoe UI", 15, FontWeight.Bold)
          }.Bind(
            new Binding("Text")
          )
        )
      );

      myDiagram.NodeTemplateMap.Add("BarMeter",
        new Node(PanelType.Table) {
          Scale = 0.8
        }.Set(
          CommonNodeStyle()
        ).Bind(
          CommonNodeBinding()
        ).Add(
          new Panel(PanelType.Auto) {
            Row = 0
          }.Add(
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
                Stretch = Stretch.None  // needed to avoid unnecessary re-measuring!!!
              }.Bind(
                CommonScaleBindings()
              ).Add(
                // the main path of the graduated panel, an arc starting at 135 degrees and sweeping for 270 degrees
                new Shape { Name = "PATH", GeometryString = "M-70.7107 70.7107 B135 270 0 0 100 100 M0 100", Stroke = "white", StrokeWidth = 4 },
                // three differently sized tick marks
                new Shape { GeometryString = "M0 0 V10", Stroke = "white", StrokeWidth = 1 },
                new Shape { GeometryString = "M0 0 V12", Stroke = "white", StrokeWidth = 2, Interval = 2 },
                new Shape { GeometryString = "M0 0 V15", Stroke = "white", StrokeWidth = 3, Interval = 4 },
                new TextBlock { // each tick label
                  Interval = 4,
                  AlignmentFocus = Spot.Center,
                  Font = new Font("Segoe UI", 19, FontStyle.Italic, FontWeight.Bold),
                  Stroke = "white",
                  SegmentOffset = new Point(0, 30)
                }
              ),
              new TextBlock {
                Alignment = Spot.Center,
                Stroke = "white",
                Font = new Font("Segoe UI", 19, FontStyle.Italic, FontWeight.Bold),
                Editable = true
              }.Bind(
                new Binding("Text", "Value", (v, _) => {
                  return (v as double? ?? double.NaN).ToString();
                }).MakeTwoWay((s, _, __) => { return double.Parse(s as string); }),
                new Binding("Stroke", "Color")
              ),
              new Shape {
                Fill = "red",
                StrokeWidth = 0
              }.Bind(
                new Binding("Geometry", "Value", (v, shp) => {
                  var scale = (shp as Shape).Part.FindElement("SCALE") as Panel;
                  var p0 = scale.GraduatedPointForValue(scale.GraduatedMin);
                  var pv = scale.GraduatedPointForValue(v as double? ?? double.NaN);
                  var path = (shp as Shape).Part.FindElement("PATH");
                  var radius = path.ActualBounds.Width / 2;
                  var c = path.ActualBounds.Center;
                  var a0 = c.Direction(p0);
                  var av = c.Direction(pv);
                  var sweep = av - a0;
                  if (sweep < 0) sweep += 360;
                  var layerThickness = 8;
                  return new Geometry()
                      .Add(new PathFigure(-radius, -radius))  // always make sure the Geometry includes the top left corner
                      .Add(new PathFigure(radius, radius))    // and the bottom right corner of the whole circular area
                      .Add(new PathFigure(p0.X - radius, p0.Y - radius)
                          .Add(new PathSegment(SegmentType.Arc, a0, sweep, 0, 0, radius, radius))
                          .Add(new PathSegment(SegmentType.Line, pv.X - radius, pv.Y - radius))
                          .Add(new PathSegment(SegmentType.Arc, av, -sweep, 0, 0, radius - layerThickness, radius - layerThickness).Close()));
                })
              ).Set(
                SliderActions()
              ).Bind(
                SliderBindings(true)
              ),
              new Shape {
                Figure = "Circle",
                Width = 2,
                Height = 2,
                Fill = "#444"
              }
            )
          ),
          new TextBlock {
            Row = 1,
            Font = new Font("Segoe UI", 15, FontWeight.Bold)
          }.Bind(
            new Binding("Text")
          )
        )
      );

      // link template
      myDiagram.LinkTemplate =
        new Link {
          Routing = LinkRouting.AvoidsNodes,
          Corner = 12
        }.Add(
          new Shape { IsPanelMain = true, Stroke = "gray", StrokeWidth = 9 },
          new Shape { IsPanelMain = true, Stroke = "lightgray", StrokeWidth = 5 },
          new Shape { IsPanelMain = true, Stroke = "whitesmoke" }
        );

      // model data
      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = 1, Value = 87, Text = "Vertical", Category = "Vertical", Loc = "30 0", Editable = true, Color = "yellow" },
          new NodeData { Key = 2, Value = 23, Text = "Circular Meter", Category = "CircularMeter", Loc = "250 -120", Editable = true, Color = "skyblue" },
          new NodeData { Key = 3, Value = 56, Text = "Needle Meter", Category = "NeedleMeter", Loc = "250 110", Editable = true, Color = "lightsalmon" },
          new NodeData { Key = 4, Value = 16, Max = 120, Text = "Horizontal", Category = "Horizontal", Loc = "550 0", Editable = true, Color = "green" },
          new NodeData { Key = 5, Value = 23, Max = 200, Unit = 5, Text = "Bar Meter", Category = "BarMeter", Loc = "550 200", Editable = true, Color = "orange" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = 1, To = 2 },
          new LinkData { From = 1, To = 3 },
          new LinkData { From = 2, To = 4 },
          new LinkData { From = 3, To = 4 },
          new LinkData { From = 4, To = 5 }
        }
      };

      // capture a RNG so we don't have to re-seed every time Loop is called
      var rand = new Random();

      // start a simple simulation
      Loop();

      void Loop() {
        Task.Delay(500).ContinueWith((t) => {
          myDiagram.Commit((_) => {
            foreach (var l in myDiagram.Links) {
              if (rand.NextDouble() < 0.2) return;
              var fromData = (l.FromNode.Data as NodeData);
              var toData = (l.ToNode.Data as NodeData);
              var prev = fromData.Value;
              var now = toData.Value;
              if (prev > (fromData.Min ?? 0) && now < (toData.Max ?? 100)) {
                myDiagram.Model.Set(l.FromNode.Data, "Value", prev - 1);
                myDiagram.Model.Set(l.ToNode.Data, "Value", now + 1);
              }
            }
          });
          Loop();
        });
      }
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public double Value { get; set; }
    public double? Max { get; set; }
    public double? Min { get; set; }
    public string Loc { get; set; }
    public bool Editable { get; set; }
    public string Color { get; set; }
    public double? Unit { get; set; }
  }

  public class LinkData : Model.LinkData { }

}
