/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Extensions;

namespace Demo.Extensions.ParallelRoute {
  public partial class ParallelRoute : DemoControl {
    private Diagram _Diagram;

    public ParallelRoute() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      desc1.MdText = DescriptionReader.Read("Extensions.ParallelRoute.md");
    }

    private void Setup() {
      // enable the Undo Manager
      _Diagram.UndoManager.IsEnabled = true;

      _Diagram.NodeTemplate = new Node(PanelType.Auto)
        .Bind("Location", "Loc", Point.Parse, Point.Stringify).Add(
          new Shape {
            PortId = "",
            FromLinkable = true,
            ToLinkable = true,
            FromLinkableDuplicates = true, // allow duplicate Links both from and to, to utilize parallel routing
            ToLinkableDuplicates = true,
            Cursor = "pointer"
          }.Bind("Fill", "Color"),
          new TextBlock {
            Margin = 8
          }.Bind("Text")
      );

      _Diagram.LinkTemplate = new ParallelRouteLink() {
        RelinkableFrom = true,
        RelinkableTo = true,
        Reshapable = true // allow the algorithm to reshape the ParallelRouteLink
      }.Add(
        new Shape { StrokeWidth = 2 }
          .Bind(new Binding("Stroke", "FromNode", (node, _) => { return ((node as Node).Port as Shape).Fill; }).OfElement()),
        new Shape { ToArrow = "OpenTriangle", StrokeWidth = 1.5 }
          .Bind(new Binding("Stroke", "FromNode", (node, _) => { return ((node as Node).Port as Shape).Fill; }).OfElement())
      );

      _Diagram.Model = new Model() {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = 1, Text = "Alpha", Color = "lightblue", Loc = "0 0" },
          new NodeData { Key = 2, Text = "Beta", Color = "orange", Loc = "130 70" },
          new NodeData { Key = 3, Text = "Gamma", Color = "lightgreen", Loc = "0 130" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = 1, To = 2 },
          new LinkData { From = 2, To = 1 },
          new LinkData { From = 1, To = 3 },
          new LinkData { From = 1, To = 3 },
          new LinkData { From = 3, To = 1 },
          new LinkData { From = 1, To = 3 },
          new LinkData { From = 1, To = 3 },
        }
      };
    }
  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Loc { get; set; }
    public Brush Color { get; set; }
  }
  public class LinkData : Model.LinkData { }
}
