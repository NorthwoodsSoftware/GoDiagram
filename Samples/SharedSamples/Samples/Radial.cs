/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Layouts.Extensions;
using Northwoods.Go.Models;

namespace Demo.Samples.Radial {
  public partial class Radial : DemoControl {
    private Diagram _Diagram;

    public Radial() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      maxLayersBtn.Click += (e, obj) => ChangeMaxLayers(int.Parse(layersTxt.Text));

      desc1.MdText = DescriptionReader.Read("Samples.Radial.md");

      // since Setup calls an immediate layout and auto scales content,
      // ensure the Diagram has loaded and thus has its correct size
      AfterLoad(Setup);
    }

    private void ChangeMaxLayers(int value) {
      if (value < 1 || value > 10) {
        ShowDialog("Please enter a number between 1 and 10.");
      } else {
        var rlay = (diagramControl1.Diagram.Layout as RadialLayout);
        rlay.MaxLayers = value;
        for (var i = 1; i < 60; i++) {
          if (_Diagram.FindNodeForKey(i).Category == "Root")
            rlay.Root = _Diagram.FindNodeForKey(i);
        }
        _NodeClicked(null, rlay.Root);
      }
    }

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

      var someone = nodeDataSource[0];
      _NodeClicked(null, _Diagram.FindNodeForData(someone));
    }

    private void _NodeClicked(InputEvent e, GraphObject obj) {
      var root = obj as Node;
      var diagram = root.Diagram;
      if (diagram == null) return;
      // all other nodes should be visible and use the default category
      foreach (var node in diagram.Nodes) {
        node.Visible = true;
        if (node != root) node.Category = "";
      }
      // make this Node the root
      root.Category = "Root";
      // tell the RadialLayout what the root node should be
      (diagram.Layout as RadialLayout).Root = root;
      diagram.LayoutDiagram(true);
    }

    private void Setup() {
      _Diagram.InitialAutoScale = AutoScale.Uniform;
      _Diagram.Padding = 10;
      _Diagram.IsReadOnly = true;
      _Diagram.Layout = new MyRadialLayout {
        MaxLayers = int.Parse(layersTxt.Text),
        LayerThickness = 100
      };
      _Diagram.AnimationManager.IsEnabled = false;

      var commonToolTip =
        Builder.Make<Adornment>("ToolTip").Add(
          new Panel(PanelType.Vertical) {
            Margin = 3
          }.Add(
            new TextBlock {
              Margin = 4,
              Font = new Font("Segoe UI", 12, Northwoods.Go.FontWeight.Bold)
            }.Bind("Text"),
            new TextBlock().Bind("Text", "Color", (c, _) => { return "Color: " + c; }),
            new TextBlock().Bind(new Binding("Text", "", (ad, _) => {
              var node = (ad as Adornment).AdornedPart as Node;
              return "Connections: " + node.LinksConnected.Count();
            }).OfElement())
          )
        );

      _Diagram.NodeTemplate =
        new Node("Spot") {
            LocationSpot = Spot.Center,
            LocationElementName = "SHAPE",  // Node.Location is the center of the Shape
            SelectionAdorned = false,
            Click = _NodeClicked,
            ToolTip = commonToolTip
          }
          .Add(
            new Shape("Circle") {
                Name = "SHAPE",
                Fill = "lightgray",  // default value, but also data-bound
                Stroke = "transparent",
                StrokeWidth = 2,
                DesiredSize = new Size(20, 20),
                PortId = ""  // so links will go to the shape, not the whole node
              }
              .Bind("Fill", "Color"),
            new TextBlock {
                Name = "TEXTBLOCK",
                Alignment = Spot.Right,
                AlignmentFocus = Spot.Left
              }
              .Bind("Text")
        );

      _Diagram.NodeTemplateMap["Root"] =
        new Node("Auto") {
            LocationSpot = Spot.Center,
            SelectionAdorned = false,
            ToolTip = commonToolTip
          }
          .Add(
            new Shape("Circle") { Fill = "White" },
            new TextBlock { Font = new Font("Segoe UI", 12, Northwoods.Go.FontWeight.Bold), Margin = 5 }
              .Bind("Text")
          );

      _Diagram.LinkTemplate =
        new Link() {
            Routing = LinkRouting.Normal,
            Curve = LinkCurve.Bezier,
            SelectionAdorned = false,
            LayerName = "Background"
          }
          .Add(
            new Shape { Stroke = "black", StrokeWidth = 1 }
              .Bind("Fill", "Color")
          );

      GenerateGraph();
    }
  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { };

  public class NodeData : Model.NodeData {
    public Brush Color { get; set; }
  };

  public class LinkData : Model.LinkData {
    public Brush Color { get; set; }
  };

  public class MyRadialLayout : RadialLayout {
    public override void CommitLayers() {
      // optional: add circles in the background
      // need to remove any old ones first
      var gridlayer = Diagram.FindLayer("Grid");
      var circles = new HashSet<Part>();
      foreach (var part in gridlayer.Parts) {
        if (part.Name == "CIRCLE") circles.Add(part);
      }

      foreach (var circle in circles) {
        Diagram.Remove(circle);
      }

      // add circles centered at the root
      for (var layer = 1; layer <= MaxLayers; layer++) {
        var radius = layer * LayerThickness;
        var circle =
          new Part {
              Name = "CIRCLE", LayerName = "Grid",
              LocationSpot = Spot.Center, Location = Root.Location
            }
            .Add(
              new Shape {
                Figure = "Circle",
                Width = radius * 2, Height = radius * 2,
                Fill = "rgba(200,200,200,0.2)", Stroke = null
              }
            );
        Diagram.Add(circle);
      }
    }

    public override void RotateNode(Node node, double angle, double sweep, double radius) {
      // rotate the nodes and make sure the text is not upside-down
      node.Angle = angle;
      var label = node.FindElement("TEXTBLOCK");
      if (label != null) {
        label.Angle = ((angle > 90 && angle < 270 || angle < -90) ? 180 : 0);
      }
    }
  }
}
