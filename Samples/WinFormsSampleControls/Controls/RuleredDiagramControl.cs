/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;

namespace WinFormsSampleControls.RuleredDiagram {
  [ToolboxItem(false)]
  public partial class RuleredDiagramControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public RuleredDiagramControl() {
      InitializeComponent();

      Setup();
      diagramControl1.MouseEnter += (s, e) => ShowIndicators();
      diagramControl1.MouseLeave += (s, e) => HideIndicators();

      goWebBrowser1.Html = @"
        <p>
          This sample demonstrates a diagram with rulers at its edges and indicators which display the mouse's position.
        </p>
        <p>
          The rulers are implemented using <a href=""intro/graduatedPanels.html"">Graduated Panels</a>. The main element of each panel is sized
          according to the width/height of the viewport, with the <a>Panel.GraduatedMin</a> and <a>Panel.GraduatedMax</a>
          being set to the edges of the viewport.
        </p>
        <p>
          Event listeners and Tool overrides are used to keep the rulers and indicators in sync as the viewport bounds change
          or the mouse moves around the diagram.
        </p>
        <ul>
          <li>
            <code>ViewportBoundsChanged</code> listeners are used to keep the rulers and indicators against
            the edge of the diagram and to update the min and max values of the rulers.
          </li>
          <li>
            An <code>InitialLayoutCompleted</code> listener is used for initial placement after the diagram
            has positioned the rest of the nodes.
          </li>
          <li>
            <a>ToolManager.DoMouseMove</a>, <a>LinkingTool.DoMouseMove</a>, <a>DraggingTool.DoMouseMove</a>,
            and <a>DragSelectingTool.DoMouseMove</a> are overridden to update the mouse indicators after executing
            their default behavior. Each is overridden so that whichever tool is active will properly adjust the
            indicators in addition to its normal functionality.
          </li>
        </ul>
        <p>
          The rulers and the indicators are implemented using simple <a>Part</a>s, not <a>Node</a>s, so that they
          are not treated as nodes by some layouts and so that they do not show up in the collection of <a>Diagram.Nodes</a>.
          They are put in the ""Grid"" <a>Layer</a> so that any changes to them are not recorded
          by the UndoManager, because the ""Grid"" Layer has all of its Parts ignored by the UndoManager.
        </p>
";

    }

    // instance variables
    private Part gradIndicatorHoriz = null;
    private Part gradIndicatorVert = null;

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.UndoManager.IsEnabled = true;
      myDiagram.ScrollMode = ScrollMode.Infinite; // allow the diagram to be scrolled beyond content
      myDiagram.Padding = 0; // scales should be allowed right up against the edges of the viewport
      myDiagram.Grid.Visible = true;

      // override tools and ToolManager
      var toolManager = new RuleredDiagramToolManager(this);
      toolManager.InitializeStandardTools();
      toolManager.LinkingTool = new RuleredDiagramLinkingTool(this);
      toolManager.DraggingTool = new RuleredDiagramDraggingTool(this);
      toolManager.DragSelectingTool = new RuleredDiagramDragSelectingTool(this);
      myDiagram.ToolManager = toolManager;
      myDiagram.DefaultTool = toolManager;

      // node template
      myDiagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance).Add(
          new Shape {
            Figure = "RoundedRectangle",
            StrokeWidth = 0,
            PortId = "",
            FromLinkable = true,
            ToLinkable = true
          }.Bind(
            new Binding("Fill", "Color")
          ),
          new TextBlock {
            Margin = 8
          }.Bind(
            new Binding("Text", "Key")
          )
        );

      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "Alpha", Color = "lightblue" },
          new NodeData { Key = "Beta", Color = "orange" },
          new NodeData { Key = "Gamma", Color = "lightgreen" },
          new NodeData { Key = "Delta", Color = "pink" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = "Alpha", To = "Beta" },
          new LinkData { From = "Alpha", To = "Gamma" },
          new LinkData { From = "Beta", To = "Beta" },
          new LinkData { From = "Gamma", To = "Delta" },
          new LinkData { From = "Delta", To = "Alpha" }
        }
      };

      // Keep references to the scales and indicators to easily update them
      var gradScaleHoriz =
        new Part(PanelLayoutGraduated.Instance) {
          GraduatedTickUnit = 10,
          Pickable = false,
          LayerName = "Grid",
          IsAnimated = false
        }.Add(
          new Shape {
            GeometryString = "M0 0 H400"
          },
          new Shape {
            GeometryString = "M0 0 V3",
            Interval = 1
          },
          new Shape {
            GeometryString = "M0 0 V15",
            Interval = 5
          },
          new TextBlock {
            Font = new Font("Segoe UI", 10),
            Interval = 5,
            AlignmentFocus = Spot.TopLeft,
            SegmentOffset = new Point(0, 7)
          }
        );

      var gradScaleVert =
        new Part(PanelLayoutGraduated.Instance) {
          GraduatedTickUnit = 10,
          Pickable = false,
          LayerName = "Grid",
          IsAnimated = false
        }.Add(
          new Shape {
            GeometryString = "M0 0 V400"
          },
          new Shape {
            GeometryString = "M0 0 V3",
            Interval = 1,
            AlignmentFocus = Spot.Bottom
          },
          new Shape {
            GeometryString = "M0 0 V15",
            Interval = 5,
            AlignmentFocus = Spot.Bottom
          },
          new TextBlock {
            Font = new Font("Segoe UI", 10),
            SegmentOrientation = Orientation.Opposite,
            Interval = 5,
            AlignmentFocus = Spot.BottomLeft,
            SegmentOffset = new Point(0, -7)
          }
        );

      // These indicators are globally defined so they can be accessed by the div's mouseevents
      gradIndicatorHoriz =
        new Part {
          Pickable = false,
          LayerName = "Grid",
          Visible = false,
          IsAnimated = false,
          LocationSpot = Spot.Top
        }.Add(
          new Shape {
            GeometryString = "M0 0 V15",
            StrokeWidth = 2,
            Stroke = "red"
          }
        );

      gradIndicatorVert =
        new Part {
          Pickable = false,
          LayerName = "Grid",
          Visible = false,
          IsAnimated = false,
          LocationSpot = Spot.Left
        }.Add(
          new Shape {
            GeometryString = "M0 0 H15",
            StrokeWidth = 2,
            Stroke = "red"
          }
        );

      myDiagram.InitialLayoutCompleted += SetupScalesAndIndicators;
      myDiagram.ViewportBoundsChanged += UpdateScales;
      myDiagram.ViewportBoundsChanged += UpdateIndicators;

      void SetupScalesAndIndicators(object _ = null, DiagramEvent e = null) {
        myDiagram.Commit((d) => {
          // Add each node to the diagram
          d.Add(gradScaleHoriz);
          d.Add(gradScaleVert);
          d.Add(gradIndicatorHoriz);
          d.Add(gradIndicatorVert);
          UpdateScales();
          UpdateIndicators();
        }, null);  // null says to skip UndoManager recording of changes
      }

      void UpdateScales(object _ = null, DiagramEvent e = null) {
        var vb = myDiagram.ViewportBounds;
        if (!vb.IsReal()) return;
        myDiagram.Commit((diag) => {
          // Update properties of horizontal scale to reflect viewport
          gradScaleHoriz.Elt(0).Width = vb.Width * diag.Scale;
          gradScaleHoriz.Location = new Point(vb.X, vb.Y);
          gradScaleHoriz.GraduatedMin = vb.X;
          gradScaleHoriz.GraduatedMax = vb.Right;
          gradScaleHoriz.Scale = 1 / diag.Scale;
          // Update properties of vertical scale to reflect viewport
          gradScaleVert.Elt(0).Height = vb.Height * diag.Scale;
          gradScaleVert.Location = new Point(vb.X, vb.Y);
          gradScaleVert.GraduatedMin = vb.Y;
          gradScaleVert.GraduatedMax = vb.Bottom;
          gradScaleVert.Scale = 1 / diag.Scale;
        }, null);
      }
    }

    public void UpdateIndicators(object _ = null, DiagramEvent e = null) {
      if (myDiagram == null) return;
      var vb = myDiagram.ViewportBounds;
      if (!vb.IsReal()) return;
      myDiagram.Commit((diag) => {
        var mouseCoords = diag.LastInput.DocumentPoint;
        // Keep the indicators in line with the mouse as viewport changes or mouse moves
        gradIndicatorHoriz.Location = new Point(Math.Max(mouseCoords.X, vb.X), vb.Y);
        gradIndicatorHoriz.Scale = 1 / diag.Scale;
        gradIndicatorVert.Location = new Point(vb.X, Math.Max(mouseCoords.Y, vb.Y));
        gradIndicatorVert.Scale = 1 / diag.Scale;
      }, null);
    }

    // Show indicators on mouseover of the diagram div
    private void ShowIndicators() {
      myDiagram.Commit((diag) => {
        gradIndicatorHoriz.Visible = true;
        gradIndicatorVert.Visible = true;
      }, null);
    }

    // Hide indicators on mouseout of the diagram div
    private void HideIndicators() {
      myDiagram.Commit((diag) => {
        gradIndicatorHoriz.Visible = false;
        gradIndicatorVert.Visible = false;
      }, null);
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Color { get; set; }
  }

  public class LinkData : Model.LinkData { }

  // override tools to update rulers
  public class RuleredDiagramToolManager : ToolManager {
    // need to keep track of app instance to update indicator variables
    private RuleredDiagramControl _AppInstance;
    public RuleredDiagramToolManager(RuleredDiagramControl appInstance) : base() {
      _AppInstance = appInstance;
    }
    public override void DoMouseMove() {
      base.DoMouseMove();
      _AppInstance.UpdateIndicators();
    }
  }
  public class RuleredDiagramLinkingTool : LinkingTool {
    // need to keep track of app instance to update indicator variables
    private RuleredDiagramControl _AppInstance;
    public RuleredDiagramLinkingTool(RuleredDiagramControl appInstance) : base() {
      _AppInstance = appInstance;
    }
    public override void DoMouseMove() {
      base.DoMouseMove();
      _AppInstance.UpdateIndicators();
    }
  }
  // no need to override PanningTool since the ViewportBoundsChanged listener will fire
  public class RuleredDiagramDraggingTool : DraggingTool {
    // need to keep track of app instance to update indicator variables
    private RuleredDiagramControl _AppInstance;
    public RuleredDiagramDraggingTool(RuleredDiagramControl appInstance) : base() {
      _AppInstance = appInstance;
    }
    public override void DoMouseMove() {
      base.DoMouseMove();
      _AppInstance.UpdateIndicators();
    }
  }

  public class RuleredDiagramDragSelectingTool : DragSelectingTool {
    // need to keep track of app instance to update indicator variables
    private RuleredDiagramControl _AppInstance;
    public RuleredDiagramDragSelectingTool(RuleredDiagramControl appInstance) : base() {
      _AppInstance = appInstance;
    }
    public override void DoMouseMove() {
      base.DoMouseMove();
      _AppInstance.UpdateIndicators();
    }
  }

}
