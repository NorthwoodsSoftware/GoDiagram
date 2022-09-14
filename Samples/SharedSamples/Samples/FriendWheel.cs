/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace Demo.Samples.FriendWheel {
  public partial class FriendWheel : DemoControl {
    private Diagram _Diagram;

    public FriendWheel() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      desc1.MdText = DescriptionReader.Read("Samples.FriendWheel.md");
    }

    private void Setup() {
      // color parameterization
      var highlightColor = "red";

      _Diagram.InitialAutoScale = AutoScale.Uniform;
      _Diagram.Padding = 10;
      _Diagram.ContentAlignment = Spot.Center;
      _Diagram.Layout = new WheelLayout {
        Arrangement = CircularArrangement.ConstantDistance,
        NodeDiameterFormula = CircularNodeDiameterFormula.Circular,
        Spacing = 10,
        AspectRatio = 0.7,
        Sorting = CircularSorting.Optimized
      };
      _Diagram.IsReadOnly = true;
      _Diagram.Click = (e) => { // background click clears any remaining highlighteds
        e.Diagram.StartTransaction("clear");
        e.Diagram.ClearHighlighteds();
        e.Diagram.CommitTransaction("clear");
      };

      // node template
      // define the Node template
      _Diagram.NodeTemplate =
        new Node(PanelType.Horizontal) {
          SelectionAdorned = false,
          LocationSpot = Spot.Center,  // Node.Location is the center of the Shape
          LocationElementName = "SHAPE",
          MouseEnter = (e, nodeAsObj, obj) => {
            var node = nodeAsObj as Node;
            node.Diagram.ClearHighlighteds();
            foreach (var l in node.LinksConnected) { HighlightLink(l, true); }
            node.IsHighlighted = true;
            var tb = node.FindElement("TEXTBLOCK") as TextBlock;
            if (tb != null) tb.Stroke = highlightColor;
          },
          MouseLeave = (e, nodeAsObj, obj) => {
            var node = nodeAsObj as Node;
            node.Diagram.ClearHighlighteds();
            var tb = node.FindElement("TEXTBLOCK") as TextBlock;
            if (tb != null) tb.Stroke = "black";
          }
        }
        .Bind(new Binding("Text", "Text"))  // for sorting the nodes
        .Add(
          new Shape {
            Figure = "Ellipse",
            Name = "SHAPE",
            Fill = "lightgray",  // default value, but also data-bound
            Stroke = "transparent",  // modified by highlighting
            StrokeWidth = 2,
            DesiredSize = new Size(20, 20),
            PortId = ""
          }.Bind(  // so links will go to the shape, not the whole node
            new Binding("Fill", "Color"),
            new Binding("Stroke", "IsHighlighted",
              (h, obj) => { return (h as bool? ?? false) ? highlightColor : "transparent"; })
              .OfElement()),
          new TextBlock { Name = "TEXTBLOCK" }.Bind(  // for search
            new Binding("Text", "Text"))
        );

      void HighlightLink(Link link, bool show) {
        link.IsHighlighted = show;
        if (link.FromNode != null) link.FromNode.IsHighlighted = show;
        if (link.ToNode != null) link.ToNode.IsHighlighted = show;
      }

      // define the Link template
      _Diagram.LinkTemplate =
        new Link {
          Routing = LinkRouting.Normal,
          Curve = LinkCurve.Bezier,
          SelectionAdorned = false,
          MouseEnter = (e, link, obj) => { HighlightLink(link as Link, true); },
          MouseLeave = (e, link, obj) => { HighlightLink(link as Link, false); }
        }.Add(
          new Shape().Bind(
            new Binding("Stroke", "IsHighlighted", (h, shape) => {
              return (h as bool? ?? false) ? highlightColor : (((shape as Shape).Part as Link).Data as LinkData).Color;
            }).OfElement(),
            new Binding("StrokeWidth", "IsHighlighted", (h, obj) => {
              return (h as bool? ?? false) ? 2 : 1;
            }).OfElement())
        // no arrowhead -- assume directionality of relationship need not be shown
        );


      // load the model data
      GenerateGraph();
    }

    // generates model data
    private void GenerateGraph() {
      var names = new string[] {
        "Joshua", "Daniel", "Robert", "Noah", "Anthony",
        "Elizabeth", "Addison", "Alexis", "Ella", "Samantha",
        "Joseph", "Scott", "James", "Ryan", "Benjamin",
        "Walter", "Gabriel", "Christian", "Nathan", "Simon",
        "Isabella", "Emma", "Olivia", "Sophia", "Ava",
        "Emily", "Madison", "Tina", "Elena", "Mia",
        "Jacob", "Ethan", "Michael", "Alexander", "William",
        "Natalie", "Grace", "Lily", "Alyssa", "Ashley",
        "Sarah", "Taylor", "Hannah", "Brianna", "Hailey",
        "Christopher", "Aiden", "Matthew", "David", "Andrew",
        "Kaylee", "Juliana", "Leah", "Anna", "Allison",
        "John", "Samuel", "Tyler", "Dylan", "Jonathan"
      };

      var num = names.Length;
      var nodeDataSource = new NodeData[num];
      for (var i = 0; i < num; i++) {
        nodeDataSource[i] = new NodeData { Key = i + 1, Text = names[i], Color = Brush.RandomColor(128, 240) };
      }

      var rand = new Random();
      var linkDataSource = new LinkData[num * 2];
      for (var i = 0; i < num * 2; i++) {
        var a = (int)Math.Floor(i / 2f);
        var b = rand.Next(num / 4) + 1;
        linkDataSource[i] = new LinkData { Key = -1 - i, From = a, To = (a + b) % num + 1, Color = Brush.RandomColor(0, 127) };
      }

      _Diagram.Model = new Model {
        NodeDataSource = nodeDataSource,
        LinkDataSource = linkDataSource
      };
    }
  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }
  public class NodeData : Model.NodeData {
    public string Color { get; set; }
  }

  public class LinkData : Model.LinkData {
    public string Color { get; set; }
  }

  // custom layout extending CircularLayout
  public class WheelLayout : CircularLayout {
    // override makeNetwork to set the diameter of each node and ignore the TextBlock label
    public override CircularNetwork MakeNetwork(IEnumerable<Part> coll = null) {
      var net = base.MakeNetwork();
      foreach (var cv in net.Vertexes) {
        cv.Diameter = 20; // because our desiredSize for nodes is (20, 20)
      }
      return net;
    }

    // override commitNodes to rotate nodes so the text goes away from the center,
    // and flip text if it would be upside-down
    protected override void CommitNodes() {
      base.CommitNodes();
      foreach (var v in Network.Vertexes) {
        var node = v.Node;
        if (node == null) return;
        // get the angle of the node towards the center, and rotate it accordingly
        var a = v.ActualAngle;
        if (a > 90 && a < 270) {  // make sure the text isn't upside down
          var textBlock = node.FindElement("TEXTBLOCK");
          textBlock.Angle = 180;
        }
        node.Angle = a;
      }
    }

    // override commitLinks in order to make sure all of the Bezier links are "inside" the ellipse;
    // this helps avoid links crossing over any other nodes
    protected override void CommitLinks() {
      base.CommitLinks();
      if (Network.Vertexes.Count > 4) {
        foreach (var v in Network.Vertexes) {
          foreach (var de in v.DestinationEdges) {
            var dv = de.ToVertex;
            var da = dv.ActualAngle;
            var sa = v.ActualAngle;
            if (da - sa > 180) da -= 360;
            else if (sa - da > 180) sa -= 360;
            de.Link.Curviness = (sa > da) ? 15 : -15;
          }
        }
      }
    }
  }
}
