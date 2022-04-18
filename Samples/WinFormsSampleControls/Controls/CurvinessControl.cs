/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.Curviness {
  [ToolboxItem(false)]
  public partial class CurvinessControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public CurvinessControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"

  <p>
    This sample explicitly binds the <a>Link.Curviness</a> property, so that some links bend out farther than others.
  </p>
  <p>
    The link template also places an arrowhead at the middle of the link,
    by explicitly setting the arrowhead's <a>GraphObject.SegmentIndex</a> to -Infinity
    <i>after</i> setting <a>Shape.ToArrow</a>.
  </p>
";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.UndoManager.IsEnabled = true;

      // define a simple Node template
      myDiagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance).Bind(
          new Binding("Position", "Position")
        ).Add(
          new Shape {
            Figure = "RoundedRectangle"
          }.Bind(
            // Shape.Fill is bound to Node.Data.Color
            new Binding("Fill", "Color")
          ),
          new TextBlock {
            Margin = 3
          }.Bind(  // some room around the text
                   // TextBlock.Text is bound to Node.Data.Key
            new Binding("Text", "Key")
          )
        );

      myDiagram.LinkTemplate =
        new Link {
          Curve = LinkCurve.Bezier,
          // when using fromSpot/toSpot:
          FromSpot = Spot.Left,
          ToSpot = Spot.Left
        }.Bind(
          new Binding("FromEndSegmentLength", "Curviness"),
          new Binding("ToEndSegmentLength", "Curviness")
        // if not using fromSpot/toSpot, use a binding to curviness instead:
        //new Binding("Curviness", "Curviness"),
        ).Add(

          new Shape  // the link shape
            { Stroke = "black", StrokeWidth = 1.5 },

          new Shape  // the arrowhead, at the mid point of the link
            { ToArrow = "OpenTriangle", SegmentIndex = double.NegativeInfinity }
        );

      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Position = new Point(100, 100), Key = "Alpha", Color = "lightblue" },
          new NodeData { Position = new Point(100, 200), Key = "Beta", Color = "orange" },
          new NodeData { Position = new Point(100, 300), Key = "Gamma", Color = "lightgreen" },
          new NodeData { Position = new Point(100, 400), Key = "Delta", Color = "pink" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = "Alpha", To = "Beta", Curviness = 20 },
          new LinkData { From = "Alpha", To = "Gamma", Curviness = 40 },
          new LinkData { From = "Gamma", To = "Delta", Curviness = 20 },
          new LinkData { From = "Delta", To = "Alpha", Curviness = 60 }
        }
      };
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public Northwoods.Go.Point Position { get; set; }
    public string Color { get; set; }
  }

  public class LinkData : Model.LinkData {
    public double? Curviness { get; set; }
  }

}
