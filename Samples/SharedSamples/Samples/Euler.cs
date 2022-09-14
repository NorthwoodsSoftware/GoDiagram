/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Extensions;
using Northwoods.Go.Models;

namespace Demo.Samples.Euler {
  public partial class Euler : DemoControl {
    private Diagram _Diagram;

    public Euler() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      desc1.MdText = DescriptionReader.Read("Samples.Euler.md");
    }

    private void Setup() {
      // add hyperlink text builder
      HyperlinkText.DefineBuilders();

      _Diagram.IsReadOnly = true;
      _Diagram.AllowSelect = false;
      _Diagram.ContentAlignment = Spot.Center;

      // HyperlinkText functions
      Func<object, string> func1 = (node) => {
        return "https://en.wikipedia.org/w/index.php?search=" + Uri.EscapeDataString(((node as Node).Data as NodeData).Text);
      };
      Func<object, string> func2 = (node) => {
        return ((node as Node).Data as NodeData).Text;
      };

      // node template
      _Diagram.NodeTemplate =
        new Node(PanelType.Auto) { LocationSpot = Spot.Center }
        .Bind("Location", "Loc", Point.Parse)
        .Add(
          new Shape() { Figure = "Ellipse", Fill = "transparent" }.Bind(
            new Binding("Stroke", "Color"),
            new Binding("StrokeWidth", "Width"),
            new Binding("StrokeDashArray", "Dash")),
          Builder.Make<TextBlock>("HyperlinkText",
            func1,
            func2
          ).Set(new { Margin = 1, MaxSize = new Size(85, 85), TextAlign = TextAlign.Center })
        );

      _Diagram.NodeTemplateMap.Add("center",
        new Node(PanelType.Spot) { LocationSpot = Spot.Center }
        .Bind("Location", "Loc", Point.Parse)
        .Add(
          new Shape() {
            Figure = "Circle",
            Fill = "rgba(128,128,128,0.1)",
            Stroke = null,
            Width = 550,
            Height = 550
          },
          new Shape() {
            Figure = "Circle",
            Fill = "rgba(128,128,128,0.05)",
            Stroke = null,
            Width = 400,
            Height = 400
          },
          new Shape() {
            Figure = "Circle",
            Fill = "rgba(128,128,128,0.033)",
            Stroke = null,
            Width = 250,
            Height = 250
          },
          new Panel(PanelType.Spot).Add(
            new Shape() { Figure = "Circle", IsPanelMain = true, Fill = "transparent", PortId = "" }.Bind(
              new Binding("Stroke", "Hicolor"),
              new Binding("StrokeWidth", "Hiwidth")),
            new Shape() { Figure = "Circle", IsPanelMain = true, Fill = "transparent" }.Bind(
              new Binding("Stroke", "Color"),
              new Binding("StrokeWidth", "Width"),
              new Binding("StrokeDashArray", "Dash")),
            Builder.Make<TextBlock>("HyperlinkText",
              func1,
              func2
            ).Set(new { Margin = 1, MaxSize = new Size(80, 80), TextAlign = TextAlign.Center })
          )
        ));

      // link template
      _Diagram.LinkTemplate =
        new Link().Add(
          new Shape().Bind(
            new Binding("Stroke", "Color"),
            new Binding("StrokeWidth", "Width"),
            new Binding("StrokeDashArray", "Dash"))
        );

      // model data
      _Diagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = 1, Text = "Cognitive Procedural", Loc = "300 300", Category = "center" },
          new NodeData { Key = 2, Text = "Cognitive Problem Solving", Loc = "600 300", Category = "center", Hicolor = "lightblue", Hiwidth = 7 },
          new NodeData { Key = 11, Text = "Logical Reasoning", Loc = "450 275" },
          new NodeData { Key = 12, Text = "Scaffolding", Loc = "450 325" },
          new NodeData { Key = 13, Text = "Part Task Training", Loc = "425 400" },
          new NodeData { Key = 21, Text = "Training Wheels", Loc = "325 125" },
          new NodeData { Key = 22, Text = "Exploratory Learning", Loc = "250 150" },
          new NodeData { Key = 23, Text = "Learner Control", Loc = "650 150" },
          new NodeData { Key = 31, Text = "Overlearning", Loc = "450 475" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = 1, To = 11, Color = "gray" },
          new LinkData { From = 1, To = 12, Color = "gray", Dash = new float[] { 3, 2 } },
          new LinkData { From = 1, To = 13, Color = "olive", Width = 2 },
          new LinkData { From = 1, To = 21, Color = "olive", Width = 3 },
          new LinkData { From = 1, To = 22, Color = "olive", Width = 2 },
          new LinkData { From = 1, To = 23, Color = "crimson", Width = 2 },
          new LinkData { From = 1, To = 31 },
          new LinkData { From = 2, To = 11, Color = "gray" },
          new LinkData { From = 2, To = 12, Color = "olive", Width = 2 },
          new LinkData { From = 2, To = 13, Color = "gray", Dash = new float[] { 3, 2 } },
          new LinkData { From = 2, To = 21, Color = "crimson", Width = 2 },
          new LinkData { From = 2, To = 22, Color = "crimson", Width = 2 },
          new LinkData { From = 2, To = 23, Color = "black", Width = 3 },
          new LinkData { From = 2, To = 31, Color = "black", Dash = new float[] { 3, 2 } }
        }
      };
    }
  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Loc { get; set; }
    public string Hicolor { get; set; }
    public double? Hiwidth { get; set; }
  }

  public class LinkData : Model.LinkData {
    public float[] Dash { get; set; }
    public double? Width { get; set; }
    public string Color { get; set; }
  }
}

