/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;
using Northwoods.Go.Extensions;

namespace WinFormsSampleControls.RoundedGroups {
  [ToolboxItem(false)]
  public partial class RoundedGroupsControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public RoundedGroupsControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"
        <p>
      This Business Process Model and Notation (BPMN) Choreography diagram demonstrates how to define a group template to look like a node template,
      with a rounded header and a rounded footer.
        </p>
        <p>
      The ""RoundedTopRectangle"" and ""RoundedBottomRectangle"" figures are defined in
      <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/GraphObjects/Figures/Figures.cs"">Figures.cs</a> in the extensions directory.
      See also the <a href=""TwoHalves"">Two Halves</a> sample and
      the <a href=""Parallel"">Parallel Layout</a> sample.
        </p>
";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // load rounded rectangle/Bpmn figures
      Figures.DefineExtraFigures();

      // diagram properties
      myDiagram.Layout = new TreeLayout {
        SetsPortSpot = false,
        SetsChildPortSpot = false,
        IsRealtime = false
      };

      // node template
      myDiagram.NodeTemplate =
        new Node(PanelLayoutVertical.Instance) {
          DefaultStretch = Stretch.Horizontal,
          FromSpot = Spot.RightSide,
          ToSpot = Spot.LeftSide
        }.Add(
          new Panel(PanelLayoutAuto.Instance).Add(
            new Shape {
              Figure = "RoundedTopRectangle",
              Fill = "white"
            }.Bind(
              new Binding("Fill", "Role", (rAsObj, _) => {
                var r = rAsObj as string;
                return r[0] == 't' ? "lightgray" : "white";
              })
            ),
            new TextBlock {
              Margin = new Margin(2, 2, 0, 2),
              TextAlign = TextAlign.Center
            }.Bind(
              new Binding("Text", "Header")
            )
          ),
          new Panel(PanelLayoutAuto.Instance) {
            MinSize = new Size(double.NaN, 70)
          }.Add(
            new Shape {
              Figure = "Rectangle",
              Fill = "white"
            },
            new TextBlock {
              Width = 120,
              Margin = new Margin(2, 2, 0, 2),
              TextAlign = TextAlign.Center
            }.Bind(
              new Binding("Text", "Text")
            ),
            new Shape {
              Figure = "BpmnActivityLoop",
              Visible = false,
              Width = 10,
              Height = 10,
              Alignment = new Spot(0.5, 1, 0, -3),
              AlignmentFocus = Spot.Bottom
            }.Bind(
              new Binding("Visible", "Loop")
            )
          ),
          new Panel(PanelLayoutAuto.Instance).Add(
            new Shape {
              Figure = "RoundedBottomRectangle",
              Fill = "white"
            }.Bind(
              new Binding("Fill", "Role", (rAsObj, _) => {
                var r = rAsObj as string;
                return r[0] == 'b' ? "lightgray" : "white";
              })
            ),
            new TextBlock {
              Margin = new Margin(2, 2, 0, 2),
              TextAlign = TextAlign.Center
            }.Bind(
              new Binding("Text", "Footer")
            )
          )
        );

      // group template
      myDiagram.GroupTemplate =
        new Group(PanelLayoutVertical.Instance) {
          Layout = new TreeLayout { SetsPortSpot = false, SetsChildPortSpot = false },
          DefaultStretch = Stretch.Horizontal,
          FromSpot = Spot.RightSide,
          ToSpot = Spot.LeftSide
        }.Add(
          new Panel(PanelLayoutAuto.Instance).Add(
            new Shape {
              Figure = "RoundedTopRectangle",
              Fill = "white"
            }.Bind(
              new Binding("Fill", "Role", (rAsObj, _) => {
                var r = rAsObj as string;
                return r[0] == 't' ? "lightgray" : "white";
              })
            ),
            new TextBlock {
              Margin = new Margin(2, 2, 0, 2),
              TextAlign = TextAlign.Center
            }.Bind(
              new Binding("Text", "Header")
            )
          ),
          new Panel(PanelLayoutAuto.Instance).Add(
            new Shape {
              Fill = "white"
            },
            new Placeholder {
              Padding = 20
            },
            new Shape {
              Figure = "BpmnActivityLoop",
              Visible = false,
              Width = 10,
              Height = 10,
              Alignment = new Spot(0.5, 1, 0, -3),
              AlignmentFocus = Spot.Bottom
            }.Bind(
              new Binding("Visible", "Loop")
            )
          ),
          new Panel(PanelLayoutAuto.Instance).Add(
            new Shape {
              Figure = "RoundedBottomRectangle",
              Fill = "white"
            }.Bind(
              new Binding("Fill", "Role", (rAsObj, _) => {
                var r = rAsObj as string;
                return r[0] == 'b' ? "lightgray" : "white";
              })
            ),
            new TextBlock {
              Margin = new Margin(2, 2, 0, 2),
              TextAlign = TextAlign.Center
            }.Bind(
              new Binding("Text", "Footer")
            )
          )
        );

      // link template
      myDiagram.LinkTemplate =
        new Link {
          Routing = LinkRouting.Orthogonal,
          Corner = 5
        }.Add(
          new Shape(),
          new Shape {
            ToArrow = "Triangle"
          }
        );

      // model data
      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = 1, Header = "Supplier", Text = "Planned Order Variations", Footer = "Retailer", Role = "b" },
          new NodeData { Key = 2, Header = "Supplier", Text = "Order & Delivery Variations", Footer = "Retailer", Role = "t", Loop = true },
          new NodeData { Key = 3, Header = "Supplier", IsGroup = true, Footer = "Shipper", Role = "b" },
          new NodeData { Key = 4, Header = "Supplier", Text = "Planned Order Variations", Footer = "Retailer", Role = "b", Group = 3 },
          new NodeData { Key = 5, Header = "Supplier", Text = "Order & Delivery Variations", Footer = "Retailer", Role = "t", Group = 3 },
          new NodeData { Key = 13, Header = "Supplier", IsGroup = true, Footer = "Shipper", Role = "b", Loop = true },
          new NodeData { Key = 14, Header = "Supplier", Text = "Planned Order Variations", Footer = "Retailer", Role = "b", Group = 13 },
          new NodeData { Key = 15, Header = "Supplier", Text = "Order & Delivery Variations", Footer = "Retailer", Role = "t", Group = 13 }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = 1, To = 2 },
          new LinkData { From = 2, To = 3 },
          new LinkData { From = 2, To = 13 },
          new LinkData { From = 4, To = 5 },
          new LinkData { From = 14, To = 15 }
        }
      };
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Header { get; set; }
    public string Footer { get; set; }
    public string Role { get; set; }
    public bool? Loop { get; set; }
  }

  public class LinkData : Model.LinkData { }

}
