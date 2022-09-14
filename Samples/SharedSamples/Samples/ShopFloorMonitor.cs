/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace Demo.Samples.ShopFloorMonitor {
  public partial class ShopFloorMonitor : DemoControl {
    private Diagram _Diagram;

    public ShopFloorMonitor() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      desc1.MdText = DescriptionReader.Read("Samples.ShopFloorMonitor.md");

      modelJson1.SaveClick = SaveModel;
      modelJson1.LoadClick = LoadModel;
      modelJson1.JsonText = @"{
  ""NodeDataSource"": [
    {""Key"":""1"", ""Text"":""Switch 23"", ""Type"":""S2"", ""Loc"":""195 218""},
    {""Key"":""2"", ""Text"":""Machine 17"", ""Type"":""M4"", ""Loc"":""195 94""},
    {""Key"":""3"", ""Text"":""Panel 7"", ""Type"":""P2"", ""Loc"":""75 218""},
    {""Key"":""4"", ""Text"":""Switch 24"", ""Type"":""S3"", ""Loc"":""306 218""},
    {""Key"":""5"", ""Text"":""Machine 18"", ""Type"":""M5"", ""Loc"":""306 95""},
    {""Key"":""6"", ""Text"":""Panel 9"", ""Type"":""P1"", ""Loc"":""426 218""},
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

      Setup();
    }

    private void Setup() {
      // conversion functions for bindings in the node template
      string NodeTypeImage(object type) {
        var str = "https://godiagram.com/samples/";
        switch (type as string) {                                     // Image sizes
          case "S2": return $"{str}images/voice atm switch.jpg";      // 55x55
          case "S3": return $"{str}images/server switch.jpg";         // 55x55
          case "P1": return $"{str}images/general processor.jpg";     // 60x85
          case "P2": return $"{str}images/storage array.jpg";         // 55x80
          case "M4": return $"{str}images/iptv broadcast server.jpg"; // 80x50
          case "M5": return $"{str}images/content engine.jpg";        // 90x65
          case "I1": return $"{str}images/pc.jpg";                    // 80x70
          default: return $"{str}images/pc.jpg";                      // 80x70
        }
      }

      object NodeTypeSize(object type) {
        switch (type as string) {
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

      object NodeProblemConverter(object msg) {
        if (msg as string != "") return "red";
        return (Brush)null;
      }

      string NodeOperationConverter(object operation) {
        var o = operation as int? ?? 0;
        if (o == 2) return "TriangleDown";
        if (o == 1) return "Rectangle";
        return "Circle";
      }

      string NodeStatusConverter(object status) {
        var s = status as int? ?? 0;
        if (s == 2) return "red";
        if (s == 1) return "yellow";
        return "green";
      }

      // node template
      _Diagram.NodeTemplate =
        new Node("Vertical") { LocationElementName = "ICON", LocationSpot = Spot.Center }
          .Bind("Location", "Loc", Point.Parse, Point.Stringify)
          .Add(
            new Panel("Spot")
              .Add(
                new Panel("Auto") { Name = "ICON" }
                  .Add(
                    new Shape { Fill = null, Stroke = null }
                      .Bind("Background", "Problem", NodeProblemConverter)
                      .Trigger("Background"),
                    new Picture { Margin = 5 }
                      .Bind("Source", "Type", NodeTypeImage)
                      .Bind("DesiredSize", "Type", NodeTypeSize)
                  ),  // end Auto Panel
                new Shape("Circle") {
                    Alignment = Spot.TopLeft,
                    AlignmentFocus = Spot.TopLeft,
                    Width = 12, Height = 12,
                    Fill = "orange"
                  }
                  .Bind("Figure", "Operation", NodeOperationConverter),
                new Shape("Triangle") {
                    Alignment = Spot.TopRight,
                    AlignmentFocus = Spot.TopRight,
                    Width = 12, Height = 12,
                    Fill = "blue"
                  }
                  .Bind("Fill", "Status", NodeStatusConverter)
                  .Trigger("Fill")
              ),  // end Spot Panel
            new TextBlock().Bind("Text")
          );  // end Node

      // conversion function for Bindings in the Link template:

      string LinkProblemConverter(object msg) {
        if (msg as string != "") return "red";
        return "gray";
      }

      _Diagram.LinkTemplate =
        new Link {
            Routing = LinkRouting.AvoidsNodes,
            Corner = 3
          }
          .Add(
            new Shape { StrokeWidth = 2, Stroke = "gray" }
              .Bind("Stroke", "Problem", LinkProblemConverter)
              .Trigger("Stroke")
          );

      // RNG to be captured by RandomProblems so we don't keep re-seeding it
      var rand = new Random();

      // simulate some real-time problem monitoring, once every two seconds:
      void RandomProblems() {
        var model = _Diagram.Model as Model;
        model.Commit(m => {
          // update all nodes
          var arr = m.NodeDataSource as List<NodeData>;
          for (var i = 0; i < arr.Count; i++) {
            var data = arr[i];
            m.Set(data, "Problem", (rand.NextDouble() < 0.8) ? "" : "Power loss due to ...");
            m.Set(data, "Status", rand.Next(3));
            m.Set(data, "Operation", rand.Next(3));
          }
          // and update all links
          var larr = model.LinkDataSource as List<LinkData>;
          for (var i = 0; i < larr.Count; i++) {
            var data = larr[i];
            m.Set(data, "Problem", (rand.NextDouble() < 0.7) ? "" : "No Power");
          }
        }, null);  // null temporarily sets SkipsUndoManager to true, to avoid recording these changes
      }

      async void Loop() {
        await Task.Delay(2000);
        RandomProblems();
        Loop();
      }

      LoadModel();
      Loop(); // start the simulation
    }

    private void SaveModel() {
      if (_Diagram == null) return;
      modelJson1.JsonText = _Diagram.Model.ToJson();
    }

    private void LoadModel() {
      if (_Diagram == null) return;
      _Diagram.Model = Model.FromJson<Model>(modelJson1.JsonText);
    }
  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Problem { get; set; }
    public int Status { get; set; }
    public int Operation { get; set; }
    public string Type { get; set; }
    public string Loc { get; set; }
  }
  public class LinkData : Model.LinkData {
    public string Problem { get; set; }
  }
}
