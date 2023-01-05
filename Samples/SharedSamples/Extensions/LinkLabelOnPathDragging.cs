/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;
using Northwoods.Go.Tools.Extensions;

namespace Demo.Extensions.LinkLabelOnPathDragging {
  public partial class LinkLabelOnPathDragging : DemoControl {
    private Diagram _Diagram;

    public LinkLabelOnPathDragging() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      desc1.MdText = DescriptionReader.Read("Extensions.LinkLabelOnPathDragging.md");
    }

    private void Setup() {
      _Diagram.UndoManager.IsEnabled = true;
      _Diagram.Layout = new ForceDirectedLayout() { DefaultSpringLength = 50, DefaultElectricalCharge = 50 };

      // install the LinkLabelOnPathDraggingTool as a mouse-move tool
      _Diagram.ToolManager.MouseMoveTools.Insert(0, new LinkLabelOnPathDraggingTool());

      // node template
      _Diagram.NodeTemplate =
        new Node(PanelType.Auto) {
          LocationSpot = Spot.Center
        }.Add(
          new Shape {
            Fill = "orange",
            PortId = "",
            FromLinkable = true,
            FromSpot = Spot.AllSides,
            ToLinkable = true,
            ToSpot = Spot.AllSides,
            Cursor = "pointer"
          }.Bind(new Binding("Fill", "Color")),
          new TextBlock {
            Margin = 10,
            Font = new Font("Segoe UI", 12, Northwoods.Go.FontWeight.Bold)
          }.Bind(
            new Binding("Text")
          )
        );

      var panel =
        new Panel(PanelType.Auto) {
          SegmentIndex = double.NaN,
          SegmentFraction = 0.5
        }.Add(
          new Shape {
            Fill = "white"
          },
          new TextBlock {
            Text = "?",
            Margin = 3
          }.Bind(
            new Binding("Text", "Color")
          )).Bind(
          // remember any modified segment properties in the link data object
          new Binding("SegmentFraction").MakeTwoWay()
        );
      // add _IsLinkLabel ad-hoc property
      panel["_IsLinkLabel"] = true;


      // link template
      _Diagram.LinkTemplate =
        new Link {
          Routing = LinkRouting.AvoidsNodes,
          Corner = 5,
          RelinkableFrom = true,
          RelinkableTo = true,
          Reshapable = true,
          Resegmentable = true
        }.Add(
          new Shape(),
          new Shape {
            ToArrow = "OpenTriangle"
          },
          panel
        );

      // create a few nodes and links
      _Diagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = 1, Text = "one", Color = "lightyellow" },
          new NodeData { Key = 2, Text = "two", Color = "brown" },
          new NodeData { Key = 3, Text = "three", Color = "green" },
          new NodeData { Key = 4, Text = "four", Color = "slateblue" },
          new NodeData { Key = 5, Text = "five", Color = "aquamarine" },
          new NodeData { Key = 6, Text = "six", Color = "lightgreen" },
          new NodeData { Key = 7, Text = "seven" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = 5, To = 6, Color = "orange" },
          new LinkData { From = 1, To = 2, Color = "red" },
          new LinkData { From = 1, To = 3, Color = "blue" },
          new LinkData { From = 1, To = 4, Color = "goldenrod" },
          new LinkData { From = 2, To = 5, Color = "fuchsia" },
          new LinkData { From = 3, To = 5, Color = "green" },
          new LinkData { From = 4, To = 5, Color = "black" },
          new LinkData { From = 6, To = 7 },
        }
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
    public double SegmentFraction { get; set; } = .5;
  }
}
