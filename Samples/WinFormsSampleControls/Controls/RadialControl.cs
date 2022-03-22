using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Layouts.Extensions;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.Radial {
  [ToolboxItem(false)]
  public partial class RadialControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public RadialControl() {
      InitializeComponent();

      Setup();

      btnLayers.Click += (e, obj) => ChangeMaxLayers(int.Parse(txtLayers.Text));

      goWebBrowser1.Html = @"
      <p>
      Click on a Node to center it and show its relationships.
      It is also easy to add more information to each node, including pictures,
      or to put such information into <a href=""intro/toolTips.html"">Tooltips</a>.
      </p>

      <p>
      The <code>RadialLayout</code> class is an extension defined at <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Layouts/Radial/RadialLayout.cs"">RadialLayout.cs</a>.
      You can control how many layers to show, whether to draw the circles, and whether to rotate the text, by modifying
      RadialLayout properties or changing overrides of <code>RadialLayout.RotateNode</code> and/or <code>RadialLayout.CommitLayers</code>.
      </p>
";

    }

    private void ChangeMaxLayers(int value) {
      if (value < 1 || value > 10) {
        System.Windows.Forms.MessageBox.Show("Please enter a number between 1 and 10.");
      } else {
        var rlay = (diagramControl1.Diagram.Layout as RadialLayout);
        rlay.MaxLayers = value;
        for (var i = 1; i < 60; i++) {
          if (myDiagram.FindNodeForKey(i).Category == "Root")
            rlay.Root = myDiagram.FindNodeForKey(i);
        }
        _NodeClicked(null, rlay.Root);
      }
    }

    private void GenerateGraph() {
      string[] names = {
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
      var rand = new Random();

      var nodeDataSource = new NodeData[names.Length];
      for (var i = 0; i < names.Length; i++) {
        nodeDataSource[i] = new NodeData { Key = i + 1, Text = names[i], Color = Brush.RandomColor(128, 240) };
      }

      var linkDataSource = new LinkData[num * 2];
      for (var i = 0; i < num * 2; i++) {
        var a = rand.Next(num);
        var b = rand.Next(num / 4) + 1;
        linkDataSource[i] = new LinkData { Key = -1 - i, From = a, To = (a + b) % num, Color = Brush.RandomColor(0, 127) };
      }

      myDiagram.Model = new Model {
        NodeDataSource = nodeDataSource,
        LinkDataSource = linkDataSource
      };

      var someone = nodeDataSource[0];
      _NodeClicked(null, myDiagram.FindNodeForData(someone));
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
      myDiagram = diagramControl1.Diagram;
      myDiagram.InitialAutoScale = AutoScale.Uniform;
      myDiagram.Padding = 10;
      myDiagram.IsReadOnly = true;
      myDiagram.Layout = new MyRadialLayout {
        MaxLayers = int.Parse(txtLayers.Text),
        LayerThickness = 100
      };
      myDiagram.AnimationManager.IsEnabled = false;

      var commonToolTip =
        Builder.Make<Adornment>("ToolTip").Add(
          new Panel(PanelLayoutVertical.Instance) {
            Margin = 3
          }.Add(
            new TextBlock {
              Margin = 4,
              Font = new Font("Segoe UI", 12, FontWeight.Bold)
            }.Bind("Text"),
            new TextBlock().Bind("Text", "Color", (c, _) => { return "Color: " + c; }),
            new TextBlock().Bind(new Binding("Text", "", (ad, _) => {
              var node = (ad as Adornment).AdornedPart as Node;
              return "Connections: " + node.LinksConnected.Count();
            }).OfElement())
          )
        );

      myDiagram.NodeTemplate =
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

      myDiagram.NodeTemplateMap["Root"] =
        new Node("Auto") {
            LocationSpot = Spot.Center,
            SelectionAdorned = false,
            ToolTip = commonToolTip
          }
          .Add(
            new Shape("Circle") { Fill = "White" },
            new TextBlock { Font = new Font("Segoe UI", 12, FontWeight.Bold), Margin = 5 }
              .Bind("Text")
          );

      myDiagram.LinkTemplate =
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
