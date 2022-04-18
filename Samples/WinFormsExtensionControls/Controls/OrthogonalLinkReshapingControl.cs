/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools.Extensions;

namespace WinFormsExtensionControls.OrthogonalLinkReshaping {
  [ToolboxItem(false)]
  public partial class OrthogonalLinkReshapingControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public OrthogonalLinkReshapingControl() {
      InitializeComponent();

      Setup();

      radBtnOrthogonal.CheckedChanged += (e, obj) => SetOrthogonalRouting();
      radBtnAvoidsNodes.CheckedChanged += (e, obj) => SetAvoidsNodesRouting();

      goWebBrowser1.Html = @"
   <p>
    This sample demonstrates the OrthogonalLinkReshapingTool that is defined in its own file,
    as <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/OrthogonalLinkReshaping/OrthogonalLinkReshapingTool.cs"">OrthogonalLinkReshapingTool.cs</a>.
    This tool allow users to shift the sections of orthogonal links in addition to resegmenting them.
    The Diagram's <a>ToolManager.LinkReshapingTool</a> and link template's <a>Part.Reshapable</a> properties must be set to use this tool.
    The <a>Link.Resegmentable</a> property can still optionally be used.
   </p>
";

    }

    private string routingBehavior = "AvoidsNodes";

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.UndoManager.IsEnabled = true;
      myDiagram.ToolManager.LinkReshapingTool = new OrthogonalLinkReshapingTool();


      // node template
      myDiagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance) {
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
      myDiagram.LinkTemplate =
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
      myDiagram.Model = new Model {
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
      myDiagram.InitialLayoutCompleted += (s, e) => {
        var link = myDiagram.Links.First();
        if (link != null) {
          link.IsSelected = true;
        }
      };
    }

    private void UpdateRouting() {
      if (routingBehavior == null) return;
      var newRouting = (routingBehavior == "Orthogonal") ? LinkRouting.Orthogonal : LinkRouting.AvoidsNodes;
      myDiagram.StartTransaction("Update Routing");
      myDiagram.LinkTemplate.Routing = newRouting;
      foreach (var link in myDiagram.Links) {
        link.Routing = newRouting;
      }
      myDiagram.CommitTransaction("Update Routing");
    }

    private void SetOrthogonalRouting() {
      routingBehavior = "Orthogonal";
      UpdateRouting();
    }

    private void SetAvoidsNodesRouting() {
      routingBehavior = "AvoidsNodes";
      UpdateRouting();
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
