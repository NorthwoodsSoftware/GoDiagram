/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;

namespace Demo.Samples.ConstantSize {
  [ToolboxItem(false)]
  public partial class ConstantSizeControl : DemoControl {
    private Diagram myDiagram;

    public ConstantSizeControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"

   <p>The tooltip for each kitten shows its name and photo.</p>
  <p>When you zoom in or out the effective size of each Node is kept constant by changing its <a>GraphObject.Scale</a>.</p>

";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.InitialContentAlignment = Spot.TopLeft;
      myDiagram.IsReadOnly = true; // allow selection but not moving or copying or deleting
      myDiagram.ToolManager.HoverDelay = 100; // how quickly tooltips are shown
      myDiagram.ToolManager.MouseWheelBehavior = WheelMode.Zoom;

      // the background image, a floor plan
      myDiagram.Add(
        new Part {  // this Part is not bound to any model data
          LayerName = "Background",
          Position = new Point(0, 0),
          Selectable = false,
          Pickable = false
        }.Add(
          new Picture {
            DesiredSize = new Size(842, 569),
            Source = "https://upload.wikimedia.org/wikipedia/commons/9/9a/Sample_Floorplan.jpg"
          }
        )
      );

      // the template for each kitten, for now just a colored circle
      myDiagram.NodeTemplate =
        new Node { // at center of node
          LocationSpot = Spot.Center,
          ToolTip = Builder.Make<Adornment>("ToolTip").Add(
            new Panel(PanelType.Vertical).Add(
              new Picture().Bind(
                new Binding("Source", "Src", (s, _) => {
                  return "https://godiagram.com/samples/images/" + (s as string) + ".png";
                })),
              new TextBlock {
                Margin = 3
              }.Bind(
                new Binding("Text", "Key")
              )
            )
          )
        }.Bind( // specified by data
          new Binding("Location", "Loc")
        ).Add(
          new Shape {
            Figure = "Circle",
            Width = 12,
            Height = 12,
            Stroke = (Brush)null
          }.Bind(
            new Binding("Fill", "Color") // also specified by data
          )
        );

      // pretend there are four kittens
      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "Alonzo", Src = "50x40", Loc = new Point(220, 130), Color = "blue" },
          new NodeData { Key = "Coricopat", Src = "55x55", Loc = new Point(420, 250), Color = "green" },
          new NodeData { Key = "Garfield", Src = "60x90", Loc = new Point(640, 450), Color = "red" },
          new NodeData { Key = "Demeter", Src = "80x50", Loc = new Point(140, 350), Color = "purple" }
        }
      };

      // This code keeps all nodes at a constant size in the viewport,
      // by adjusting for any scaling done by zooming in or out.
      // This code ignores simple Parts;
      // Links will automatically be rerouted as Nodes change size.
      var origscale = double.NaN;
      myDiagram.InitialLayoutCompleted += (_, e) => {
        origscale = myDiagram.Scale;
      };
      myDiagram.ViewportBoundsChanged += (_, e) => {
        if (double.IsNaN(origscale)) return;
        var newscale = myDiagram.Scale;
        var subject = (ValueTuple<double, Point, Rect, bool>)e.Subject;
        if (subject.Item1 == newscale) return;  // Optimization = don't scale Nodes when just scrolling/panning
        myDiagram.SkipsUndoManager = true;
        myDiagram.StartTransaction("scale Nodes");
        foreach (var node in myDiagram.Nodes) {
          node.Scale = origscale / newscale;
        }
        myDiagram.CommitTransaction("scale Nodes");
        myDiagram.SkipsUndoManager = false;
      };

      // simulate some real-time position monitoring, once every 2 seconds
      void RandomMovement() {
        var model = myDiagram.Model;
        model.StartTransaction("update locations");
        var rand = new Random();
        var arr = model.NodeDataSource as List<NodeData>;
        var picture = myDiagram.Parts.First();
        for (var i = 0; i < arr.Count; i++) {
          var data = arr[i];
          var pt = data.Loc;
          var x = pt.X + 20 * rand.NextDouble() - 10;
          var y = pt.Y + 20 * rand.NextDouble() - 10;
          // make sure the kittens stay inside the house
          var b = picture.ActualBounds;
          if (x < b.X || x > b.Right) x = pt.X;
          if (y < b.Y || y > b.Bottom) y = pt.Y;
          (model as Model).Set(data, "Loc", new Point(x, y));
        }
        model.CommitTransaction("update locations");
      }
      void Loop() {
        Task.Delay(2000).ContinueWith((t) => {
          RandomMovement();
          Loop();
        });
      }
      Loop();  // start the simulation
    }

  }

  // define the model data
  public class Model : Model<NodeData, string, object> { }
  public class NodeData : Model.NodeData {
    public string Src { get; set; }
    public Point Loc { get; set; }
    public string Color { get; set; }
  }

}
