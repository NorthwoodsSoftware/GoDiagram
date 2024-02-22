/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Layouts.Extensions;
using Northwoods.Go.Models;

namespace Demo.Extensions.SwimLane {
  public partial class SwimLane : DemoControl {
    private readonly int DIRECTION = 90;
    private Diagram _Diagram;

    public SwimLane() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      _InitRadioButtons();

      Setup();

      desc1.MdText = DescriptionReader.Read("Extensions.SwimLane.md");
    }

    private void Setup() {
      // automatically scale the diagram to fit the viewport's size
      _Diagram.InitialAutoScale = AutoScale.Uniform;
      // disable user copying of parts
      _Diagram.AllowCopy = false;
      // position all of the nodes and route all of the links
      _Diagram.Layout = new CustomSwimLaneLayout {
        LaneProperty = "Group",  // needs to know how to assign vertexes/nodes into lanes/groups
        Direction = DIRECTION,
        SetsPortSpots = false,
        LayerSpacing = 20,
        ColumnSpacing = 5
      };

      // replace the default Node template in the NodeTemplateMap
      _Diagram.NodeTemplate =
        new Node("Vertical") {
            // when the DIRECTION is vertical, use the whole Node as the port
            FromSpot = Spot.TopBottomSides,
            ToSpot = Spot.TopBottomSides
          }
          .Add(
            new TextBlock().Bind("Text", "Key"),  // the text label
            new Picture {  // the icon showing the logo
                // You should set the DesiredSize (or width and height)
                // whenever you know what size the Picture should be
                DesiredSize = new Size(50, 50),
                // when the DIRECTION is horizontal, use this icon as the port
                PortId = (DIRECTION == 0 || DIRECTION == 180) ? "" : null,
                FromSpot = Spot.LeftRightSides,
                ToSpot = Spot.LeftRightSides
              }
              .Bind("Source", "Key", convertKeyImage)
          );

      static string convertKeyImage(object key) {
        if (key == null) key = "NE";
        return "https://www.nwoods.com/go/beatpaths/" + (string)key + "_logo-50x50.png";
      }

      // replace the default Link template in the LinkTemplateMap
      _Diagram.LinkTemplate =
        new Link { // the whole link panel
            Routing = LinkRouting.AvoidsNodes,Corner = 10
          }
          .Add(
            new Shape { StrokeWidth = 1.5 },  // the link shape
            new Shape { ToArrow = "Standard", Stroke = null }  // the arrowhead
          );

      _Diagram.GroupTemplate =  // assumes SwimLaneLayout.Direction == 0
        new Group((DIRECTION == 0 || DIRECTION == 180) ? "Horizontal" : "Vertical") {
            LayerName = "Background",  // always behind all regular nodes and links
            Movable = false,  // user cannot move or copy any lanes
            Copyable = false,
            LocationElementName = "PLACEHOLDER",  // this object will be sized and located by SwimLaneLayout
            Layout = null,  // no lane lays out its member nodes
            Avoidable = false  // don't affect any AvoidNodes link routes
          }
          .Add(
            new TextBlock { Font = new Font("Segoe UI", 12, Northwoods.Go.FontWeight.Bold, FontUnit.Point), Angle = (DIRECTION == 0 || DIRECTION == 180) ? 270 : 0 }
              .Bind("Text", "Key"),
            new Panel("Auto")
              .Add(
                new Shape { Fill = "transparent", Stroke = "orange" },
                new Shape { Name = "PLACEHOLDER", Fill = null, Stroke = null, StrokeWidth = 0 }
              ),
            new TextBlock { Font = new Font("Segoe UI", 12, Northwoods.Go.FontWeight.Bold), Angle = (DIRECTION == 0 || DIRECTION == 180) ? 90 : 0 }
              .Bind("Text", "Key")
          );

      _PartitionBy('d');
    }

    private readonly List<NodeData> _NodeDataSource = new() {
      new NodeData { Key = "AFC", IsGroup = true},
      new NodeData { Key = "NFC", IsGroup = true},
      new NodeData { Key = "AFCE", IsGroup = true},
      new NodeData { Key = "AFCN", IsGroup = true},
      new NodeData { Key = "AFCS", IsGroup = true},
      new NodeData { Key = "AFCW", IsGroup = true},
      new NodeData { Key = "NFCE", IsGroup = true},
      new NodeData { Key = "NFCN", IsGroup = true},
      new NodeData { Key = "NFCS", IsGroup = true},
      new NodeData { Key = "NFCW", IsGroup = true},
      new NodeData { Key = "NE", Conf = "AFC", Div = "AFCE" },
      new NodeData { Key = "PIT", Conf = "AFC", Div = "AFCN" },
      new NodeData { Key = "DAL", Conf = "NFC", Div = "NFCE" },
      new NodeData { Key = "CLE", Conf = "AFC", Div = "AFCN" },
      new NodeData { Key = "NYG", Conf = "NFC", Div = "NFCE" },
      new NodeData { Key = "GB", Conf = "NFC", Div = "NFCN" },
      new NodeData { Key = "SEA", Conf = "NFC", Div = "NFCW" },
      new NodeData { Key = "IND", Conf = "AFC", Div = "AFCS" },
      new NodeData { Key = "MIN", Conf = "NFC", Div = "NFCN" },
      new NodeData { Key = "PHI", Conf = "NFC", Div = "NFCE" },
      new NodeData { Key = "DET", Conf = "NFC", Div = "NFCN" },
      new NodeData { Key = "JAC", Conf = "AFC", Div = "AFCS" },
      new NodeData { Key = "SD", Conf = "AFC", Div = "AFCW" },
      new NodeData { Key = "CHI", Conf = "NFC", Div = "NFCN" },
      new NodeData { Key = "TB", Conf = "NFC", Div = "NFCS" },
      new NodeData { Key = "KC", Conf = "AFC", Div = "AFCW" },
      new NodeData { Key = "DEN", Conf = "AFC", Div = "AFCW" },
      new NodeData { Key = "TEN", Conf = "AFC", Div = "AFCS" },
      new NodeData { Key = "BUF", Conf = "AFC", Div = "AFCE" },
      new NodeData { Key = "OAK", Conf = "AFC", Div = "AFCW" },
      new NodeData { Key = "HOU", Conf = "AFC", Div = "AFCS" },
      new NodeData { Key = "ATL", Conf = "NFC", Div = "NFCS" },
      new NodeData { Key = "WAS", Conf = "NFC", Div = "NFCE" },
      new NodeData { Key = "CIN", Conf = "AFC", Div = "AFCN" },
      new NodeData { Key = "NYJ", Conf = "AFC", Div = "AFCE" },
      new NodeData { Key = "CAR", Conf = "NFC", Div = "NFCS" },
      new NodeData { Key = "NO", Conf = "NFC", Div = "NFCS" },
      new NodeData { Key = "BAL", Conf = "AFC", Div = "AFCN" },
      new NodeData { Key = "MIA", Conf = "AFC", Div = "AFCE" },
      new NodeData { Key = "ARI", Conf = "NFC", Div = "NFCW" },
      new NodeData { Key = "STL", Conf = "NFC", Div = "NFCW" },
      new NodeData { Key = "SF", Conf = "NFC", Div = "NFCW" }
    };

    private readonly List<LinkData> _LinkDataSource = new() {
      new LinkData { From = "NE", To = "CLE" },
      new LinkData { From = "NE", To = "DAL" },
      new LinkData { From = "NE", To = "IND" },
      new LinkData { From = "PIT", To = "CLE" },
      new LinkData { From = "DAL", To = "NYG" },
      new LinkData { From = "DAL", To = "GB" },
      new LinkData { From = "CLE", To = "SEA" },
      new LinkData { From = "NYG", To = "DET" },
      new LinkData { From = "GB", To = "MIN" },
      new LinkData { From = "GB", To = "PHI" },
      new LinkData { From = "SEA", To = "PHI" },
      new LinkData { From = "SEA", To = "CIN" },
      new LinkData { From = "IND", To = "TB" },
      new LinkData { From = "IND", To = "JAC" },
      new LinkData { From = "MIN", To = "SD" },
      new LinkData { From = "PHI", To = "NYJ" },
      new LinkData { From = "DET", To = "CHI" },
      new LinkData { From = "DET", To = "DEN" },
      new LinkData { From = "JAC", To = "DEN" },
      new LinkData { From = "SD", To = "DEN" },
      new LinkData { From = "CHI", To = "OAK" },
      new LinkData { From = "TB", To = "TEN" },
      new LinkData { From = "DEN", To = "TEN" },
      new LinkData { From = "DEN", To = "KC" },
      new LinkData { From = "DEN", To = "BUF" },
      new LinkData { From = "TEN", To = "OAK" },
      new LinkData { From = "TEN", To = "ATL" },
      new LinkData { From = "TEN", To = "HOU" },
      new LinkData { From = "BUF", To = "WAS" },
      new LinkData { From = "OAK", To = "MIA" },
      new LinkData { From = "HOU", To = "MIA" },
      new LinkData { From = "HOU", To = "CAR" },
      new LinkData { From = "WAS", To = "NYJ" },
      new LinkData { From = "WAS", To = "ARI" },
      new LinkData { From = "CIN", To = "BAL" },
      new LinkData { From = "NYJ", To = "MIA" },
      new LinkData { From = "CAR", To = "ARI" },
      new LinkData { From = "CAR", To = "STL" },
      new LinkData { From = "CAR", To = "SF" },
      new LinkData { From = "NO", To = "SF" },
      new LinkData { From = "BAL", To = "STL" },
      new LinkData { From = "BAL", To = "SF" }
    };

    private void _PartitionBy(char a) {
      // create the model and assign it to the Diagram
      var model = new Model() {
        // depending on how we are partitioning the graph, each node belongs either
        // to a conference group or to a division group
        NodeGroupKeyProperty = (a == 'c') ? "Conf" : "Div",
        NodeDataSource = _NodeDataSource,
        LinkDataSource = _LinkDataSource
      };
      var layout = _Diagram.Layout as CustomSwimLaneLayout;
      // each node's lane information is the same as the group information
      layout.LaneProperty = model.NodeGroupKeyProperty;
      // optionally, specify the order of known lane names, without setting LaneComparer
      layout.LaneNames = (a == 'c') ?
        new List<string> { "AFC", "NFC" } :
        new List<string> { "AFCE", "AFCN", "AFCS", "AFCW", "NFCE", "NFCN", "NFCS", "NFCW" };
      _Diagram.Model = model;
    }
  }

  // define the model types
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { };

  // define the node data
  public class NodeData : Model.NodeData {
    public string Conf { get; set; }
    public string Div { get; set; }
  }

  // define the link data
  public class LinkData : Model.LinkData { }

  public class CustomSwimLaneLayout : SwimLaneLayout {
    protected override void CommitLayers(List<Rect> layerRects, Point offset) {
      if (layerRects.Count == 0) return;

      var horiz = Direction == 0 || Direction == 180;
      var forwards = Direction == 0 || Direction == 90;

      var rect = layerRects[forwards ? layerRects.Count - 1 : 0];
      var totallength = horiz ? rect.Right : rect.Bottom;

      foreach (var lane in LaneNames) {
        // assume lane names do not conflict with node names
        var group = Diagram.FindNodeForKey(lane);
        if (group == null) {
          Diagram.Model.AddNodeData(new NodeData { Key = lane, IsGroup = true });
          group = Diagram.FindNodeForKey(lane);
        }
        if (horiz) {
          group.Location = new Point(-LayerSpacing / 2, LanePositions[lane] * ColumnSpacing + offset.Y);
        } else {
          group.Location = new Point(LanePositions[lane] * ColumnSpacing + offset.X, -LayerSpacing / 2);
        }
        var ph = group.FindElement("PLACEHOLDER");  // won't be a go.Placeholder, but just a regular Shape
        if (ph == null) ph = group;
        if (horiz) {
          ph.DesiredSize = new Size(totallength, LaneBreadths[lane] * ColumnSpacing);
        } else {
          ph.DesiredSize = new Size(LaneBreadths[lane] * ColumnSpacing, totallength);
        }
      }
    }
  }
}
