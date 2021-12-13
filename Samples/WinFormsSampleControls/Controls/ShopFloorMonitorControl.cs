using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.ShopFloorMonitor {
  [ToolboxItem(false)]
  public partial class ShopFloorMonitorControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    
    public ShopFloorMonitorControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      btnLoad.Click += (e, obj) => LoadModel();

      goWebBrowser1.Html = @"
        <p>
      This monitoring diagram continuously shows the state of a number of stations on an imaginary shop floor.
      Every two seconds it updates the display, showing some random problems via highlighting.
      You can add nodes and links by adding data to the model text below and then clicking ""Load"".
        </p>
";

      goWebBrowser2.Html = @"
        <p>
      For another monitoring example, see the <a href=""KittenMonitor"">Kitten Monitor</a> sample.
        </p>
";


      txtJSON.Text = myModelData;
    }

    private string myModelData =
@"{
  ""NodeDataSource"": [
    {""Key"":""1"", ""Text"":""Switch 23"", ""Type"":""S2"", ""Loc"":""195 225""},
    {""Key"":""2"", ""Text"":""Machine 17"", ""Type"":""M4"", ""Loc"":""183.5 94""},
    {""Key"":""3"", ""Text"":""Panel 7"", ""Type"":""P2"", ""Loc"":""75 211.5""},
    {""Key"":""4"", ""Text"":""Switch 24"", ""Type"":""S3"", ""Loc"":""306 225""},
    {""Key"":""5"", ""Text"":""Machine 18"", ""Type"":""M5"", ""Loc"":""288.5 95""},
    {""Key"":""6"", ""Text"":""Panel 9"", ""Type"":""P1"", ""Loc"":""426 211""},
    {""Key"":""7"", ""Text"":""Instr 3"", ""Type"":""I1"", ""Loc"":""-50 218""}
  ],
  ""LinkDataSource"": [
    {""From"":""1"", ""To"":""2""},
    {""From"":""1"", ""To"":""3""},
    {""From"":""1"", ""To"":""4""},
    {""From"":""4"", ""To"":""5""},
    {""From"":""4"", ""To"":""6""},
    {""From"":""7"", ""To"":""2""},
    {""From"":""7"", ""To"":""3""}
  ]
}";

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // conversion functions for bindings in the node template
      string NodeTypeImage(object typeAsObj, object _) {
        var retVal = @"https://godiagram.com/samples/";
        switch (typeAsObj as string) {                                   // Image sizes
          case "S2": return retVal + "images/voice atm switch.jpg";      // 55x55
          case "S3": return retVal + "images/server switch.jpg";         // 55x55
          case "P1": return retVal + "images/general processor.jpg";     // 60x85
          case "P2": return retVal + "images/storage array.jpg";         // 55x80
          case "M4": return retVal + "images/iptv broadcast server.jpg"; // 80x50
          case "M5": return retVal + "images/content engine.jpg";        // 90x65
          case "I1": return retVal + "images/pc.jpg";                    // 80x70
          default: return retVal + "images/pc.jpg";                      // 80x70
        }
      }

      object NodeTypeSize(object typeAsObj, object _) {
        switch (typeAsObj as string) {
          case "S2": return new Size(55, 55);
          case "S3": return new Size(55, 55);
          case "P1": return new Size(60, 85);
          case "P2": return new Size(55, 80);
          case "M4": return new Size(80, 50);
          case "M5": return new Size(90, 65);
          case "I1": return new Size(80, 70);
          default: return new Size(80, 70);
        }
      }

      string NodeProblemConverter(object msg, object _) {
        if (msg as string != "") return "red";
        return null;
      }

      string NodeOperationConverter(object sAsObj, object _) {
        var s = sAsObj as double? ?? 0;
        if (s >= 2) return "TriangleDown";
        if (s >= 1) return "Rectangle";
        return "Circle";
      }

      string NodeStatusConverter(object sAsObj, object _) {
        var s = sAsObj as double? ?? 0;
        if (s >= 2) return "red";
        if (s >= 1) return "yellow";
        return "green";
      }

      // node template
      myDiagram.NodeTemplate =
        new Node(PanelLayoutVertical.Instance) {
          LocationElementName = "ICON"
        }.Bind(
          new Binding("Location", "Loc", Point.Parse).MakeTwoWay(Point.Stringify)
        ).Add(
          new Panel(PanelLayoutSpot.Instance).Add(
            new Panel(PanelLayoutAuto.Instance) {
              Name = "ICON"
            }.Add(
              new Shape {
                Fill = null,
                Stroke = null
              }.Bind(
                new Binding("Background", "Problem", NodeProblemConverter)
              ),
              //new AnimationTrigger('background')),
              new Picture {
                Margin = 5
              }.Bind(
                new Binding("Source", "Type", NodeTypeImage),
                new Binding("DesiredSize", "Type", NodeTypeSize)
              )
            ),  // end Auto Panel
            new Shape {
              Figure = "Circle",
              Alignment = Spot.TopLeft,
              AlignmentFocus = Spot.TopLeft,
              Width = 12,
              Height = 12,
              Fill = "orange"
            }.Bind(
              new Binding("Figure", "Operation", NodeOperationConverter)
            ),
            new Shape {
              Figure = "Triangle",
              Alignment = Spot.TopRight,
              AlignmentFocus = Spot.TopRight,
              Width = 12,
              Height = 12,
              Fill = "blue"
            }.Bind(
              new Binding("Fill", "Status", NodeStatusConverter)
            )
          //new AnimationTrigger('fill'))
          ),  // end Spot Panel
          new TextBlock().Bind(
            new Binding("Text")
          )
        );  // end Node

      // conversion function for Bindings in the Link template:

      string LinkProblemConverter(object msg, object _) {
        if (msg as string != "") return "red";
        return "gray";
      }

      myDiagram.LinkTemplate =
        new Link {
          Routing = LinkRouting.AvoidsNodes,
          Corner = 3
        }.Add(
          new Shape {
            StrokeWidth = 2,
            Stroke = "gray"
          }.Bind(
            new Binding("Stroke", "Problem", LinkProblemConverter)
          )
        //new AnimationTrigger('stroke'))
        );

      // RNG to be captured by RandomProblems so we don't keep re-seeding it
      var rand = new Random();

      // simulate some real-time problem monitoring, once every two seconds:
      void RandomProblems() {
        var model = myDiagram.Model as Model;
        // update all nodes
        var arr = model.NodeDataSource as List<NodeData>;
        for (var i = 0; i < arr.Count; i++) {
          var data = arr[i];
          data.Problem = (rand.NextDouble() < 0.8) ? "" : "Power loss due to ...";
          data.Status = rand.NextDouble() * 3;
          data.Operation = rand.NextDouble() * 3;
          model.UpdateTargetBindings(data);
        }
        // and update all links
        var larr = model.LinkDataSource as List<LinkData>;
        for (var i = 0; i < larr.Count; i++) {
          var data = larr[i];
          data.Problem = (rand.NextDouble() < 0.7) ? "" : "No Power";
          model.UpdateTargetBindings(data);
        }
      }

      void Loop() {
        Task.Delay(2000).ContinueWith((t) => {
          RandomProblems();
          Loop();
        });
      }

      LoadModel();
      Loop(); // start the simulation
      
    }

    private void LoadModel() {
      myModelData = txtJSON.Text;
      myDiagram.Model = Model.FromJson<Model>(myModelData);
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Problem { get; set; }
    public double Status { get; set; }
    public double Operation { get; set; }
    public string Type { get; set; }
    public string Loc { get; set; }
  }

  public class LinkData : Model.LinkData {
    public string Problem { get; set; }
  }

}
