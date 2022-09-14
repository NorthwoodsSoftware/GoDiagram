/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.Collections.Generic;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools.Extensions;

namespace Demo.Extensions.OrthogonalLinkReshaping {
  public partial class OrthogonalLinkReshaping : DemoControl {
    private Diagram _Diagram;

    public OrthogonalLinkReshaping() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      _InitRadioButtons();

      Setup();

      desc1.MdText = DescriptionReader.Read("Extensions.OrthogonalLinkReshaping.md");
    }

    private void Setup() {
      _Diagram.UndoManager.IsEnabled = true;
      _Diagram.ToolManager.LinkReshapingTool = new OrthogonalLinkReshapingTool();

      // node template
      _Diagram.NodeTemplate =
        new Node(PanelType.Auto) {
          Width = 80,
          Height = 50,
          LocationSpot = Spot.Center
        }.Bind(new Binding("Location", "Loc", Point.Parse).MakeTwoWay(Point.Stringify))
        .Add(
          new Shape {
            Fill = "lightgray"
          },
          new TextBlock {
            Margin = 10
          }.Bind(
            new Binding("Text", "Key")
          )
        );

      // link template
      _Diagram.LinkTemplate =
        new Link {
          Routing = LinkRouting.AvoidsNodes,
          Reshapable = true,
          Resegmentable = true
        }.Bind(new Binding("Points").MakeTwoWay())
        .Add(
          new Shape {
            StrokeWidth = 2
          }
        );

      // model
      _Diagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "Alpha", Loc = "0 0" },
          new NodeData { Key = "Beta", Loc = "200 0" },
          new NodeData { Key = "Gamma", Loc = "100 0" },
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = "Alpha", To = "Beta" }
        }
      };

      // select the link to show its two additional adornments for shifting the ends
      _Diagram.InitialLayoutCompleted += (s, e) => {
        var link = _Diagram.Links.First();
        if (link != null) {
          link.IsSelected = true;
        }
      };
    }

    private void _UpdateRouting(LinkRouting routing) {
      _Diagram.StartTransaction("Update Routing");
      _Diagram.LinkTemplate.Routing = routing;
      foreach (var link in _Diagram.Links) {
        link.Routing = routing;
      }
      _Diagram.CommitTransaction("Update Routing");
    }
  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Loc { get; set; }
  }
  public class LinkData : Model.LinkData {
    public List<Point> Points { get; set; }
  }
}
