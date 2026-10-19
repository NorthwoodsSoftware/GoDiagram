/* Copyright (c) Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;
using System.Globalization;

namespace Demo.Samples.Sankey; 
public partial class Sankey : DemoControl {
  private Diagram _Diagram;

  public Sankey() {
    InitializeComponent();
    _Diagram = diagramControl1.Diagram;

    modelJson1.SaveClick = SaveModel;
    modelJson1.LoadClick = LoadModel;

    desc1.MdText = DescriptionReader.Read("Samples.Sankey.md");

    modelJson1.JsonText = @"
        {
     ""nodeDataSource"": [
    {""key"":""Coal reserves"", ""text"":""Coal reserves"", ""color"":""#9d75c2""},
    {""key"":""Coal imports"", ""text"":""Coal imports"", ""color"":""#9d75c2""},
    {""key"":""Oil reserves"", ""text"":""Oil\nreserves"", ""color"":""#9d75c2""},
    {""key"":""Oil imports"", ""text"":""Oil imports"", ""color"":""#9d75c2""},
    {""key"":""Gas reserves"", ""text"":""Gas reserves"", ""color"":""#a1e194""},
    {""key"":""Gas imports"", ""text"":""Gas imports"", ""color"":""#a1e194""},
    {""key"":""UK land based bioenergy"", ""text"":""UK land based bioenergy"", ""color"":""#f6bcd5""},
    {""key"":""Marine algae"", ""text"":""Marine algae"", ""color"":""#681313""},
    {""key"":""Agricultural 'waste'"", ""text"":""Agricultural 'waste'"", ""color"":""#3483ba""},
    {""key"":""Other waste"", ""text"":""Other waste"", ""color"":""#c9b7d8""},
    {""key"":""Biomass imports"", ""text"":""Biomass imports"", ""color"":""#fea19f""},
    {""key"":""Biofuel imports"", ""text"":""Biofuel imports"", ""color"":""#d93c3c""},
    {""key"":""Coal"", ""text"":""Coal"", ""color"":""#9d75c2""},
    {""key"":""Oil"", ""text"":""Oil"", ""color"":""#9d75c2""},
    {""key"":""Natural gas"", ""text"":""Natural\ngas"", ""color"":""#a6dce6""},
    {""key"":""Solar"", ""text"":""Solar"", ""color"":""#c9a59d""},
    {""key"":""Solar PV"", ""text"":""Solar PV"", ""color"":""#c9a59d""},
    {""key"":""Bio-conversion"", ""text"":""Bio-conversion"", ""color"":""#b5cbe9""},
    {""key"":""Solid"", ""text"":""Solid"", ""color"":""#40a840""},
    {""key"":""Liquid"", ""text"":""Liquid"", ""color"":""#fe8b25""},
    {""key"":""Gas"", ""text"":""Gas"", ""color"":""#a1e194""},
    {""key"":""Nuclear"", ""text"":""Nuclear"", ""color"":""#fea19f""},
    {""key"":""Thermal generation"", ""text"":""Thermal\ngeneration"", ""color"":""#3483ba""},
    {""key"":""CHP"", ""text"":""CHP"", ""color"":""yellow""},
    {""key"":""Electricity imports"", ""text"":""Electricity imports"", ""color"":""yellow""},
    {""key"":""Wind"", ""text"":""Wind"", ""color"":""#cbcbcb""},
    {""key"":""Tidal"", ""text"":""Tidal"", ""color"":""#6f3a5f""},
    {""key"":""Wave"", ""text"":""Wave"", ""color"":""#8b8b8b""},
    {""key"":""Geothermal"", ""text"":""Geothermal"", ""color"":""#556171""},
    {""key"":""Hydro"", ""text"":""Hydro"", ""color"":""#7c3e06""},
    {""key"":""Electricity grid"", ""text"":""Electricity grid"", ""color"":""#e483c7""},
    {""key"":""H2 conversion"", ""text"":""H2 conversion"", ""color"":""#868686""},
    {""key"":""Solar Thermal"", ""text"":""Solar Thermal"", ""color"":""#c9a59d""},
    {""key"":""H2"", ""text"":""H2"", ""color"":""#868686""},
    {""key"":""Pumped heat"", ""text"":""Pumped heat"", ""color"":""#96665c""},
    {""key"":""District heating"", ""text"":""District heating"", ""color"":""#c9b7d8""},
    {""key"":""Losses"", ""lText"":""Losses"", ""color"":""#fec184""},
    {""key"":""Over generation / exports"", ""lText"":""Over generation / exports"", ""color"":""#f6bcd5""},
    {""key"":""Heating and cooling - homes"", ""lText"":""Heating and cooling - homes"", ""color"":""#c7a39b""},
    {""key"":""Road transport"", ""lText"":""Road transport"", ""color"":""#cbcbcb""},
    {""key"":""Heating and cooling - commercial"", ""lText"":""Heating and cooling - commercial"", ""color"":""#c9a59d""},
    {""key"":""Industry"", ""lText"":""Industry"", ""color"":""#96665c""},
    {""key"":""Lighting & appliances - homes"", ""lText"":""Lighting & appliances - homes"", ""color"":""#2dc3d2""},
    {""key"":""Lighting & appliances - commercial"", ""lText"":""Lighting & appliances - commercial"", ""color"":""#2dc3d2""},
    {""key"":""Agriculture"", ""lText"":""Agriculture"", ""color"":""#5c5c10""},
    {""key"":""Rail transport"", ""lText"":""Rail transport"", ""color"":""#6b6b45""},
    {""key"":""Domestic aviation"", ""lText"":""Domestic aviation"", ""color"":""#40a840""},
    {""key"":""National navigation"", ""lText"":""National navigation"", ""color"":""#a1e194""},
    {""key"":""International aviation"", ""lText"":""International aviation"", ""color"":""#fec184""},
    {""key"":""International shipping"", ""lText"":""International shipping"", ""color"":""#fec184""},
    {""key"":""Geosequestration"", ""lText"":""Geosequestration"", ""color"":""#fec184""}
  ], ""linkDataSource"": [
    {""from"":""Coal reserves"", ""to"":""Coal"", ""width"":31},
    {""from"":""Coal imports"", ""to"":""Coal"", ""width"":86},
    {""from"":""Oil reserves"", ""to"":""Oil"", ""width"":244},
    {""from"":""Oil imports"", ""to"":""Oil"", ""width"":1},
    {""from"":""Gas reserves"", ""to"":""Natural gas"", ""width"":182},
    {""from"":""Gas imports"", ""to"":""Natural gas"", ""width"":61},
    {""from"":""UK land based bioenergy"", ""to"":""Bio-conversion"", ""width"":1},
    {""from"":""Marine algae"", ""to"":""Bio-conversion"", ""width"":1},
    {""from"":""Agricultural 'waste'"", ""to"":""Bio-conversion"", ""width"":1},
    {""from"":""Other waste"", ""to"":""Bio-conversion"", ""width"":8},
    {""from"":""Other waste"", ""to"":""Solid"", ""width"":1},
    {""from"":""Biomass imports"", ""to"":""Solid"", ""width"":1},
    {""from"":""Biofuel imports"", ""to"":""Liquid"", ""width"":1},
    {""from"":""Coal"", ""to"":""Solid"", ""width"":117},
    {""from"":""Oil"", ""to"":""Liquid"", ""width"":244},
    {""from"":""Natural gas"", ""to"":""Gas"", ""width"":244},
    {""from"":""Solar"", ""to"":""Solar PV"", ""width"":1},
    {""from"":""Solar PV"", ""to"":""Electricity grid"", ""width"":1},
    {""from"":""Solar"", ""to"":""Solar Thermal"", ""width"":1},
    {""from"":""Bio-conversion"", ""to"":""Solid"", ""width"":3},
    {""from"":""Bio-conversion"", ""to"":""Liquid"", ""width"":1},
    {""from"":""Bio-conversion"", ""to"":""Gas"", ""width"":5},
    {""from"":""Bio-conversion"", ""to"":""Losses"", ""width"":1},
    {""from"":""Solid"", ""to"":""Over generation / exports"", ""width"":1},
    {""from"":""Liquid"", ""to"":""Over generation / exports"", ""width"":18},
    {""from"":""Gas"", ""to"":""Over generation / exports"", ""width"":1},
    {""from"":""Solid"", ""to"":""Thermal generation"", ""width"":106},
    {""from"":""Liquid"", ""to"":""Thermal generation"", ""width"":2},
    {""from"":""Gas"", ""to"":""Thermal generation"", ""width"":87},
    {""from"":""Nuclear"", ""to"":""Thermal generation"", ""width"":41},
    {""from"":""Thermal generation"", ""to"":""District heating"", ""width"":2},
    {""from"":""Thermal generation"", ""to"":""Electricity grid"", ""width"":92},
    {""from"":""Thermal generation"", ""to"":""Losses"", ""width"":142},
    {""from"":""Solid"", ""to"":""CHP"", ""width"":1},
    {""from"":""Liquid"", ""to"":""CHP"", ""width"":1},
    {""from"":""Gas"", ""to"":""CHP"", ""width"":1},
    {""from"":""CHP"", ""to"":""Electricity grid"", ""width"":1},
    {""from"":""CHP"", ""to"":""Losses"", ""width"":1},
    {""from"":""Electricity imports"", ""to"":""Electricity grid"", ""width"":1},
    {""from"":""Wind"", ""to"":""Electricity grid"", ""width"":1},
    {""from"":""Tidal"", ""to"":""Electricity grid"", ""width"":1},
    {""from"":""Wave"", ""to"":""Electricity grid"", ""width"":1},
    {""from"":""Geothermal"", ""to"":""Electricity grid"", ""width"":1},
    {""from"":""Hydro"", ""to"":""Electricity grid"", ""width"":1},
    {""from"":""Electricity grid"", ""to"":""H2 conversion"", ""width"":1},
    {""from"":""Electricity grid"", ""to"":""Over generation / exports"", ""width"":1},
    {""from"":""Electricity grid"", ""to"":""Losses"", ""width"":6},
    {""from"":""Gas"", ""to"":""H2 conversion"", ""width"":1},
    {""from"":""H2 conversion"", ""to"":""H2"", ""width"":1},
    {""from"":""H2 conversion"", ""to"":""Losses"", ""width"":1},
    {""from"":""Solar Thermal"", ""to"":""Heating and cooling - homes"", ""width"":1},
    {""from"":""H2"", ""to"":""Road transport"", ""width"":1},
    {""from"":""Pumped heat"", ""to"":""Heating and cooling - homes"", ""width"":1},
    {""from"":""Pumped heat"", ""to"":""Heating and cooling - commercial"", ""width"":1},
    {""from"":""CHP"", ""to"":""Heating and cooling - homes"", ""width"":1},
    {""from"":""CHP"", ""to"":""Heating and cooling - commercial"", ""width"":1},
    {""from"":""District heating"", ""to"":""Heating and cooling - homes"", ""width"":1},
    {""from"":""District heating"", ""to"":""Heating and cooling - commercial"", ""width"":1},
    {""from"":""District heating"", ""to"":""Industry"", ""width"":2},
    {""from"":""Electricity grid"", ""to"":""Heating and cooling - homes"", ""width"":7},
    {""from"":""Solid"", ""to"":""Heating and cooling - homes"", ""width"":3},
    {""from"":""Liquid"", ""to"":""Heating and cooling - homes"", ""width"":3},
    {""from"":""Gas"", ""to"":""Heating and cooling - homes"", ""width"":81},
    {""from"":""Electricity grid"", ""to"":""Heating and cooling - commercial"", ""width"":7},
    {""from"":""Solid"", ""to"":""Heating and cooling - commercial"", ""width"":1},
    {""from"":""Liquid"", ""to"":""Heating and cooling - commercial"", ""width"":2},
    {""from"":""Gas"", ""to"":""Heating and cooling - commercial"", ""width"":19},
    {""from"":""Electricity grid"", ""to"":""Lighting & appliances - homes"", ""width"":21},
    {""from"":""Gas"", ""to"":""Lighting & appliances - homes"", ""width"":2},
    {""from"":""Electricity grid"", ""to"":""Lighting & appliances - commercial"", ""width"":18},
    {""from"":""Gas"", ""to"":""Lighting & appliances - commercial"", ""width"":2},
    {""from"":""Electricity grid"", ""to"":""Industry"", ""width"":30},
    {""from"":""Solid"", ""to"":""Industry"", ""width"":13},
    {""from"":""Liquid"", ""to"":""Industry"", ""width"":34},
    {""from"":""Gas"", ""to"":""Industry"", ""width"":54},
    {""from"":""Electricity grid"", ""to"":""Agriculture"", ""width"":1},
    {""from"":""Solid"", ""to"":""Agriculture"", ""width"":1},
    {""from"":""Liquid"", ""to"":""Agriculture"", ""width"":1},
    {""from"":""Gas"", ""to"":""Agriculture"", ""width"":1},
    {""from"":""Electricity grid"", ""to"":""Road transport"", ""width"":1},
    {""from"":""Liquid"", ""to"":""Road transport"", ""width"":122},
    {""from"":""Electricity grid"", ""to"":""Rail transport"", ""width"":2},
    {""from"":""Liquid"", ""to"":""Rail transport"", ""width"":1},
    {""from"":""Liquid"", ""to"":""Domestic aviation"", ""width"":2},
    {""from"":""Liquid"", ""to"":""National navigation"", ""width"":4},
    {""from"":""Liquid"", ""to"":""International aviation"", ""width"":38},
    {""from"":""Liquid"", ""to"":""International shipping"", ""width"":13},
    {""from"":""Electricity grid"", ""to"":""Geosequestration"", ""width"":1},
    {""from"":""Gas"", ""to"":""Losses"", ""width"":2}
     ]}";

    Setup();
  }

  private void Setup() {
    _Diagram.InitialAutoScale = AutoScale.UniformToFill;
    _Diagram.AnimationManager.IsEnabled = false;
    _Diagram.Layout = new SankeyLayout {
      SetsPortSpots = false,  // to allow the "Side" spots on the nodes to take effect
      Direction = 0,  // rightwards
      LayeringOption = LayeredDigraphLayering.OptimalLinkLength,
      LayerSpacing = 150,  // lots of space between layers, for nicer thick links
      ColumnSpacing = 1
    };

    var colors = new List<string> {
      "#AC193D/#BF1E4B", "#2672EC/#2E8DEF",
      "#8C0095/#A700AE", "#5133AB/#643EBF",
      "#008299/#00A0B1", "#D24726/#DC572E",
      "#008A00/#00A600", "#094AB2/#0A5BC4"
    };

    void textStyle(TextBlock tb) {
      tb.Font = new Font("Segoe UI", 12, Northwoods.Go.FontWeight.Bold);
      tb.Stroke = "black";
      tb.Margin = 5;
    }

    _Diagram.NodeTemplate =
      new Node("Horizontal") {
          LocationElementName = "SHAPE",
          LocationSpot = Spot.Left,
          PortSpreading = PortSpreading.Packed  // rather than the default PortSpreading.SpreadingEvenly
        }
        .Add(
          new TextBlock { Name = "LTEXT" }
            .Apply(textStyle)
            .Bind("Text", "LText"),
          new Shape("Rectangle") {
              Name = "SHAPE",
              Fill = "#2E8DEF",  // default fill color
              StrokeWidth = 0,
              PortId = "",
              FromSpot = Spot.RightSide,
              ToSpot = Spot.LeftSide,
              Height = 10,
              Width = 20
            }
            .Bind("Fill", "Color"),
          new TextBlock { Name = "TEXT" }
            .Apply(textStyle)
            .Bind("Text")
      );

    object getAutoLinkColor(object data) {
      var linkdata = data as LinkData;
      var nodedata = _Diagram.Model.FindNodeDataForKey(linkdata.From) as NodeData;
      var hex = nodedata.Color;
      if (hex[0] == '#') {
        var rgb = int.Parse(hex.Substring(1, 6), NumberStyles.HexNumber);
        var r = rgb >> 16;
        var g = rgb >> 8 & 0xFF;
        var b = rgb & 0xFF;
        var alpha = 0.4;
        if (linkdata.Width <= 2) alpha = 1;
        var rgba = "rgba(" + r + "," + g + "," + b + "," + alpha + ")";
        return rgba;
      }
      return "rgba(173, 173, 173, 0.25)";
    }

    // define the Link template
    var linkSelectionAdornmentTemplate =
      new Adornment("Link")
        .Add(
          new Shape { IsPanelMain = true, Fill = null, Stroke = "rgba(0, 0, 255, 0.3)", StrokeWidth = 0 }  // use selection object's StrokeWidth
        );

    _Diagram.LinkTemplate =
      new Link {
          Curve = LinkCurve.Bezier,
          SelectionAdornmentTemplate = linkSelectionAdornmentTemplate,
          LayerName = "Background",
          FromEndSegmentLength = 150, ToEndSegmentLength = 150,
          Adjusting = LinkAdjusting.End
        }
        .Add(
          new Shape { StrokeWidth = 4, Stroke = "rgba(173, 173, 173, 0.25)" }
            .Bind("Stroke", "", getAutoLinkColor)
            .Bind("StrokeWidth", "Width")
        );

    // read in the JSON-format data from the Textarea element
    LoadModel();
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

public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
public class NodeData : Model.NodeData {
  public string LText { get; set; }
  public string Color { get; set; }
}
public class LinkData : Model.LinkData {
  public double Width { get; set; }
}

public class SankeyLayout : LayeredDigraphLayout {
  // determine the desired height of each node/vertex,
  // based on the thicknesses of the connected links;
  // actually modify the height of each node's SHAPE
  public override LayeredDigraphNetwork CreateNetwork() {
    var net = base.CreateNetwork();
    foreach (var node in Diagram.Nodes) {
      // figure out how tall the node's bar should be
      var height = _GetAutoHeightForNode(node);
      var font = new Font("Segoe UI", (float)Math.Max(12, Math.Round(height / 8)), FontWeight.Bold);
      var shape = node.FindElement("SHAPE");
      var text = node.FindElement("TEXT") as TextBlock;
      var ltext = node.FindElement("LTEXT") as TextBlock;
      if (shape != null) shape.Height = height;
      if (text != null) text.Font = font;
      if (ltext != null) ltext.Font = font;
      // and update the vertex's dimensions accordingly
      var v = net.FindVertex(node);
      if (v != null) {
        node.EnsureBounds();
        var r = node.ActualBounds;
        v.Width = r.Width;
        v.Height = r.Height;
        v.FocusY = v.Height / 2;
      }
    }
    return net;
  }

  private double _GetAutoHeightForNode(Node node) {
    var heightIn = 0.0;
    var it = node.FindLinksInto();
    foreach (var link in it) {
      heightIn += link.ComputeThickness();
    }
    var heightOut = 0.0;
    it = node.FindLinksOutOf();
    foreach (var link in it) {
      heightOut += link.ComputeThickness();
    }
    var h = Math.Max(heightIn, heightOut);
    if (h < 10) h = 10;
    return h;
  }

  // treat dummy vertexes as having the thicknesss of the link that they are in
  protected override int NodeMinColumnSpace(LayeredDigraphVertex v, bool topleft) {
    if (v.Node == null) {
      if (v.Edges.Count >= 1) {
        var max = 1.0;
        var it = v.Edges;
        foreach (var edge in it) {
          if (edge.Link != null) {
            var t = edge.Link.ComputeThickness();
            if (t > max) max = t;
            break;
          }
        }
        return Math.Max(2, (int)Math.Ceiling(max / ColumnSpacing));
      }
      return 2;
    }
    return base.NodeMinColumnSpace(v, topleft);
  }

  // treat dummy vertexes as being thicker, so that the Bezier curves are gentler
  protected override double NodeMinLayerSpace(LayeredDigraphVertex v, bool topleft) {
    if (v.Node == null) return 100;
    return base.NodeMinLayerSpace(v, topleft);
  }

  protected override void AssignLayers() {
    base.AssignLayers();
    var maxlayer = MaxLayer;
    // now make sure every vertex with no outputs is maxlayer
    foreach (var v in Network.Vertexes) {
      var node = v.Node;
      if (v.DestinationVertexes.Count == 0) {
        v.Layer = 0;
      }
      if (v.SourceVertexes.Count == 0) {
        v.Layer = maxlayer;
      }
    }
    // from now on, the LayeredDigraphLayout will think that the Node is bigger than it really is
    // (other than the ones that are the widest or tallest in their respective layer).
  }

  protected override void CommitLayout() {
    base.CommitLayout();
    foreach (var e in Network.Edges) {
      var link = e.Link;
      if (link != null && link.Curve == LinkCurve.Bezier) {
        // depend on Link.Adjusting == LinkAdjusting.End to fix up the end points of the links
        // without losing the intermediate points of the route as determined by LayeredDigraphLayout
        link.InvalidateRoute();
      }
    }
  }
}
// end of SankeyLayout
