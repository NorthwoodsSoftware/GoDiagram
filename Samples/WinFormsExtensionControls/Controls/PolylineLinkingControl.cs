/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using System.ComponentModel;
using Northwoods.Go.Tools.Extensions;


namespace WinFormsExtensionControls.PolylineLinking {
  [ToolboxItem(false)]
  public partial class PolylineLinkingControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    public PolylineLinkingControl() {
      InitializeComponent();

      saveLoadModel1.SaveClick += (e, obj) => SaveModel();
      saveLoadModel1.LoadClick += (e, obj) => LoadModel();

      goWebBrowser1.Html = @"
          <p>
    This sample demonstrates the PolylineLinkingTool, which replaces the standard LinkingTool.
    The tool is defined in its own file, as <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/PolylineLinking/PolylineLinkingTool.cs"">PolylineLinkingTool.cs</a>.
          </p>
           <p>
    The user starts drawing a new link from a node in the normal manner, by dragging from a port,
    which for feedback purposes has a ""pointer"" cursor.
    Normally the user would have to release the mouse near the target port/node.
    However with the PolylineLinkingTool the user may click at various points to cause the new link
    to be routed along those points.
    Clicking on the target port completes the new link.
    Press <b>Escape</b> to cancel, or <b>Z</b> to remove the last point.
          </p>
           <p>
    Furthermore, because <a>Link.Resegmentable</a> is true, the user can easily add or remove segments
    from the route of a selected link. To insert a segment, the user can start dragging the small
    diamond resegmenting handle. To remove a segment, the user needs to move a regular reshaping handle
    to cause the adjacent two segments to be in a straight line.
          </p>
           <p>
    The PolylineLinkingTool also works with orthogonally routed links.
    To demonstrate this, uncomment the two lines that initialize <a>Link.Routing</a> to be <a>LinkRounting.Orthogonal</a>.
          </p>
";
      saveLoadModel1.ModelJson = @"
        {
        ""NodeDataSource"": [
          {""Key"":1,""Text"":""Node 1"",""Fill"":""blueviolet"",""Loc"":""51 -18""},
          {""Key"":2,""Text"":""Node 2"",""Fill"":""orange"",""Loc"":""400 100""}
        ],
        ""LinkDataSource"": []}
";

      Setup();
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // install custom linking tool, defined in PolylineLinkingTool.cs
      var tool = new PolylineLinkingTool();
      //tool.TemporaryLink.Routing = LinkRouting.Orthogonal; // optional, but need to keep link template in sync below
      myDiagram.ToolManager.LinkingTool = tool;
      myDiagram.UndoManager.IsEnabled = true;

      // node template
      myDiagram.NodeTemplate =
        new Node(PanelLayoutSpot.Instance) {
          LocationSpot = Spot.Center
        }.Bind(new Binding("Location", "Loc", Point.Parse).MakeTwoWay(Point.Stringify))
        .Add(
          new Shape {
            Width = 100,
            Height = 100,
            Fill = "lightgray",
            PortId = "",
            Cursor = "Pointer",
            FromLinkable = true,
            FromLinkableSelfNode = true,
            FromLinkableDuplicates = true, // optional
            ToLinkable = true,
            ToLinkableSelfNode = true,
            ToLinkableDuplicates = true // optional
          }.Bind(
            new Binding("Fill")
          ),
          new Shape {
            Width = 70,
            Height = 70,
            Fill = "transparent",
            Stroke = (Brush)null
          },
          new TextBlock().Bind(
            new Binding("Text")
          )
        );

      // link template
      myDiagram.LinkTemplate =
        new Link {
          Reshapable = true,
          Resegmentable = true,
          //Routing = LinkRouting.Orthogonal, // optional, but need to keep LinkingTool.TemporaryLink in sync above
          Adjusting = LinkAdjusting.Stretch
        }.Bind(new Binding("Points").MakeTwoWay())
        .Add(
          new Shape {
            StrokeWidth = 1.5
          },
          new Shape {
            ToArrow = "OpenTriangle"
          }
        );

      // TEMPORARY until loading from document works
      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = 1, Text = "Node 1", Fill = "blueviolet", Loc = "100 100" },
          new NodeData { Key = 2, Text = "Node 2", Fill = "orange", Loc = "400 100" }
        }
      };
      myDiagram.Model.UndoManager.IsEnabled = true;
      // END TEMPORARY
      // myDiagram.Model = new PolylineLinkingModel();
      // myDiagram.Model.UndoManager.IsEnabled = true;
      // Load();
    }

    private void SaveModel() {
      if (myDiagram == null) return;
      saveLoadModel1.ModelJson = myDiagram.Model.ToJson();
    }

    private void LoadModel() {
      if (myDiagram == null) return;
      myDiagram.Model = Model.FromJson<Model>(saveLoadModel1.ModelJson);
      myDiagram.Model.UndoManager.IsEnabled = true;
    }

  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Loc { get; set; }
    public string Fill { get; set; }
  }
  public class LinkData : Model.LinkData {
    public List<Point> Points { get; set; }
  }
}
