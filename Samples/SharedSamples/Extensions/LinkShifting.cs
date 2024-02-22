/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools.Extensions;

namespace Demo.Extensions.LinkShifting {
  public partial class LinkShifting : DemoControl {
    private Diagram _Diagram;

    public LinkShifting() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      desc1.MdText = DescriptionReader.Read("Extensions.LinkShifting.md");
    }

    private void Setup() {
      _Diagram.UndoManager.IsEnabled = true;
      _Diagram.ToolManager.MouseDownTools.Add(new LinkShiftingTool());

      // node template
      _Diagram.NodeTemplate =
        new Node(PanelType.Auto) {
          FromSpot = Spot.AllSides,
          ToSpot = Spot.AllSides,
          FromLinkable = true,
          ToLinkable = true,
          LocationSpot = Spot.Center
        }.Bind(new Binding("Location", "Loc", Point.Parse).MakeTwoWay(Point.Stringify))
        .Add(
          new Shape {
            Fill = "lightgray"
          },
          new TextBlock {
            Margin = 10,
            FromLinkable = false,
            ToLinkable = false
          }.Bind(
            new Binding("Text", "Key")
          )
        );

      // link template
      _Diagram.LinkTemplate =
        new Link {
          Reshapable = true,
          Resegmentable = true,
          RelinkableFrom = true,
          RelinkableTo = true,
          Adjusting = LinkAdjusting.Stretch
        }.Bind(
          // remember the (potentially) user-modified route
          new Binding("Points").MakeTwoWay(),
          // remember any spots modified by LinkShiftingTool
          new Binding("FromSpot", "FromSpot", Spot.Parse).MakeTwoWay(Spot.Stringify),
          new Binding("ToSpot", "ToSpot", Spot.Parse).MakeTwoWay(Spot.Stringify))
        .Add(
          new Shape(),
          new Shape {
            ToArrow = "standard"
          }
        );

      // model
      _Diagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData {
            Key = "Alpha", Loc = "0 0"
          },
          new NodeData {
            Key = "Beta", Loc = "0 100"
          }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData {
            From = "Alpha", To = "Beta"
          }
        }
      };
    }
  }

  // model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Loc { get; set; }
  }
  public class LinkData : Model.LinkData {
    public List<Point> Points { get; set; }
    public string FromSpot { get; set; }
    public string ToSpot { get; set; }
  }
}
