/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.KittenMonitor {
  [ToolboxItem(false)]
  public partial class KittenMonitorControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public KittenMonitorControl() {
      InitializeComponent();
      myDiagram = diagramControl1.Diagram;

      Setup();

      goWebBrowser1.Html = @"
<p>This diagram displays a monitored floor plan with several nodes (representing kittens) to view in real-time.</p>
<p>Every two seconds the kitten positions are updated</p>
<p>The <a href=""intro/toolTips.html"">Tooltip</a> for each kitten shows its name and photo.</p>
<p>There is a custom <a>Diagram.ScaleComputation</a> that limits the <a>Diagram.Scale</a> values to multiples of 0.1.</p>
";

    }

    private void Setup() {
      var INTERVAL = 2000;

      // diagram properties
      myDiagram.InitialContentAlignment = Spot.TopLeft;
      myDiagram.IsReadOnly = true;
      myDiagram.ScaleComputation = (d, newsc) => {
        // only allow scales that are a multiple of 0.1
        var oldsc = Math.Round(d.Scale * 10);
        var sc = oldsc + ((newsc * 10 > oldsc) ? 1 : -1);
        if (sc < 1) sc = 1;  // but disallow zero or negative!
        return sc / 10;
      };
      myDiagram.ToolManager.HoverDelay = 100; // how quickly tooltips are shown

      // the background image, a floor plan
      myDiagram.Add(
        new Part {  // this Part is not bound to any model data
            Width = 840, Height = 570,
            LayerName = "Background", Position = new Point(0, 0),
            Selectable = false, Pickable = false
          }
          .Add(new Picture { Source = "https://upload.wikimedia.org/wikipedia/commons/9/9a/Sample_Floorplan.jpg" })
      );

      // the template for each kitten, for now just a colored circle
      myDiagram.NodeTemplate =
        new Node { // this tooltip shows the name and picture of the kitten
            ToolTip =
              Builder.Make<Adornment>("ToolTip")
                .Add(
                  new Panel("Vertical")
                    .Add(
                      new Picture { Margin = 3 }
                        .Bind("Source", "Src", (s) => { return "https://godiagram.com/samples/images/" + s + ".png"; }),
                      new TextBlock { Margin = 3 }
                        .Bind("Text", "Key")
                  )
                ),  // end Adornment
            LocationSpot = Spot.Center // at center of node
          }
          .Bind("Location", "Loc")  // specified by data
          .Add(
            new Shape("Circle") {
                Width = 15, Height = 15, StrokeWidth = 3
              }
              .Bind("Fill", "Color", MakeFill)
              .Bind("Stroke", "Color", MakeStroke)  // also specified by data
          )
          // don't animate if INTERVAL is <= 20 milliseconds
          .Trigger(INTERVAL > 20 ? new AnimationTrigger("Position", (INTERVAL, null, Animation.EaseLinear)) : null);

      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "Alonzo", Src = "50x40", Loc = new Point(220, 130), Color = 2 },
          new NodeData { Key = "Coricopat", Src = "55x55", Loc = new Point(420, 250), Color = 4 },
          new NodeData { Key = "Garfield", Src = "60x90", Loc = new Point(640, 450), Color = 6 },
          new NodeData { Key = "Demeter", Src = "80x50", Loc = new Point(140, 350), Color = 8 }
        }
      };

      // simulate some real-time position monitoring, once every INTERVAL milliseconds
      void RandomMovement() {
        var rand = new Random();
        var model = myDiagram.Model;
        model.StartTransaction("update locations");
        var arr = model.NodeDataSource as List<NodeData>;
        var picture = myDiagram.Parts.First();
        for (var i = 0; i < arr.Count; i++) {
          var data = arr[i];
          // determine the new random location
          var pt = data.Loc;
          var x = pt.X + 25 * (rand.NextDouble() - 0.5);
          var y = pt.Y + 25 * (rand.NextDouble() - 0.5);
          // make sure the kittens stay inside the house
          var b = picture.ActualBounds;
          if (x < b.X + 40 || x > b.Right - 80) x = pt.X;
          if (y < b.Y + 40 || y > b.Bottom - 40) y = pt.Y;
          model.Set(data, "Loc", new Point(x, y));
        }
        model.CommitTransaction("update locations");
      }

      async void Loop() {
        await Task.Delay(INTERVAL + 20);
        RandomMovement();
        Loop();
      }
      Loop();

      // generate some colors based on hue value
      string MakeFill(object num, object _) {
        var number = (int)num;
        return HSVtoRGB(0.1 * number, 0.5, 0.7);
      }
      string MakeStroke(object num, object _) {
        var number = (int)num;
        return HSVtoRGB(0.1 * number, 0.5, 0.5); // same color but darker (less V in HSV)
      }
      string HSVtoRGB(double h, double s, double v) {
        int i;
        double r, g, b, f, p, q, t;
        i = Convert.ToInt32(Math.Floor(h * 6));
        f = h * 6 - i;
        p = v * (1 - s);
        q = v * (1 - f * s);
        t = v * (1 - (1 - f) * s);
        switch (i % 6) {
          default: r = v; g = t; b = p; break; // default instead of 0 for compiler errors
          case 1: r = q; g = v; b = p; break;
          case 2: r = p; g = v; b = t; break;
          case 3: r = p; g = q; b = v; break;
          case 4: r = t; g = p; b = v; break;
          case 5: r = v; g = p; b = q; break;
        }
        return "rgb(" +
          Convert.ToInt32(Math.Floor(r * 255)) + ',' +
          Convert.ToInt32(Math.Floor(g * 255)) + ',' +
          Convert.ToInt32(Math.Floor(b * 255)) + ')';
      }
    }

  }

  // define the model data
  public class Model : Model<NodeData, string, object> { }
  public class NodeData : Model.NodeData {
    public string Src { get; set; }
    public Northwoods.Go.Point Loc { get; set; }
    public int Color { get; set; }
  }

}
