using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools.Extensions;

namespace WinFormsExtensionControls.LinkShifting {
  [ToolboxItem(false)]
  public partial class LinkShiftingControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    
    public LinkShiftingControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      goWebBrowser1.Html = @"
   <p>
    This sample demonstrates the LinkShiftingTool, which is an extra tool
    that can be installed in the ToolManager to allow users to shift the end
    point of the link to be anywhere along the sides of the port with which it
    remains connected.
  </p>
  <p>
    This only looks good for ports that occupy the whole of a rectangular node.
    If you want to restrict the user's permitted sides, you can adapt the
    <code>LinkShiftingTool.DoReshape</code> method to do what you want.
  </p>
";

    }

    private void Setup() {

      myDiagram = diagramControl1.Diagram;

      myDiagram.UndoManager.IsEnabled = true;
      myDiagram.ToolManager.MouseDownTools.Add(new LinkShiftingTool());

      // node template
      myDiagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance) {
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
      myDiagram.LinkTemplate =
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
      myDiagram.Model = new Model {
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
