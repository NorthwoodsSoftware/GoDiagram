using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.RadialAdornment {
  [ToolboxItem(false)]
  public partial class RadialAdornmentControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    
    public RadialAdornmentControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      //btnSetLayers.Click += (e, obj) => doLayout();

      goWebBrowser1.Html = @"
        <p>
      Click on a Node to center it and show its relationships.
      It is also easy to add more information to each node, including pictures,
      or to put such information into <a href=""intro/toolTips.html"">Tooltips</a>.
        </p>    
        <p>
      The <code>RadialLayout</code> class is an extension defined at <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Layouts/Radial/RadialLayout.cs"">RadialLayout.cs</a>.          
      You can control how many layers to show,
      whether to draw the circles, and whether to rotate the text, by modifying
      RadialLayout properties or changing overrides of <code>RadialLayout.RotateNode</code> and/or <code>RadialLayout.CommitLayers</code>.
        </p>
";

    }

    public void Alert(string message) {
      System.Windows.Forms.MessageBox.Show(message);
    }

    private void Setup() {

      // Show a radial Adornment holding buttons when a node is selected.
      // This is not a template, so there is only one instance of it that can be shown at a time.
      // If you want every selected node to have their own copy of this adornment,
      // set Node.selectionAdornmentTemplate to this and remove the "ChangedSelection" DiagramEvent listener.
      var selectionAdornment = new Adornment(PanelLayoutSpot.Instance) {
        LayerName = "Tool" // so it's in front of other Adornments
      }.Add(
        new Panel(PanelLayoutAuto.Instance).Add(
          new Shape {
            Fill = null,
            Stroke = "dodgerblue",
            StrokeWidth = 4,
            Pickable = false
          },
          new Placeholder {
            Margin = 2
          }
        ),
        new Panel().Add(
          new SectorButton(0, 90) { // start angle, sweep angle
            Click = (e, button) => {
              // start drawing a new Link from this Node
              var node = (button.Part as Adornment).AdornedPart; // this Node
              e.Diagram.ClearSelection(); // hide all Adornments for clarity
              var tool = e.Diagram.ToolManager.LinkingTool; // the Linking Tool
              tool.StartElement = (node as Node).Port; // the default port of the Node
              e.Diagram.CurrentTool = tool; // start the LinkingTool
              tool.DoActivate(); // and activate it
            }
          }.Add(
            new TextBlock { Text = "New\nLink", Alignment = new Spot(0.5, 0.5, 60, 0) }
          ),
          new SectorButton(90, 90) {
            Click = (e, button) => {
              Alert("Show Settings for " + ((button.Part as Adornment).AdornedPart.Data as NodeData).Text);
            }
          }.Add(
            new Shape {
              GeometryString = "F1 M29.181 19.070c-1.679-2.908-0.669-6.634 2.255-8.328l-3.145-5.447c-0.898 0.527-1.943 0.829-3.058 0.829-3.361 0-6.085-2.742-6.085-6.125h-6.289c0.008 1.044-0.252 2.103-0.811 3.070-1.679 2.908-5.411 3.897-8.339 2.211l-3.144 5.447c0.905 0.515 1.689 1.268 2.246 2.234 1.676 2.903 0.672 6.623-2.241 8.319l3.145 5.447c0.895-0.522 1.935-0.82 3.044-0.82 3.35 0 6.067 2.725 6.084 6.092h6.289c-0.003-1.034 0.259-2.080 0.811-3.038 1.676-2.903 5.399-3.894 8.325-2.219l3.145-5.447c-0.899-0.515-1.678-1.266-2.232-2.226zM16 22.479c-3.578 0-6.479-2.901-6.479-6.479s2.901-6.479 6.479-6.479c3.578 0 6.479 2.901 6.479 6.479s-2.901 6.479-6.479 6.479z",
              Alignment = new Spot(0.5, 0.5, 0, 60)
            }
          ),
          new SectorButton(180, 90) {
            Click = (e, button) => {
              Alert("Show Information about " + ((button.Part as Adornment).AdornedPart.Data as NodeData).Text);
            }
          }.Add(
            new TextBlock {
              Text = "?",
              Font = new Font("Segoe UI", 14, FontWeight.Bold),
              Alignment = new Spot(0.5, 0.5, -60, 0)
            }
          ),
          new SectorButton(270, 90) {
            Click = (e, button) => {
              // this is different from CommandHandler.DeleteSelection because this
              // only deletes the one node, not all selected parts
              e.Diagram.Commit(d => {
                d.Remove((button.Part as Adornment).AdornedPart);
              }, "deleted node");
            }
          }.Add(
            new Shape {
              Figure = "XLine",
              Stroke = "red",
              StrokeWidth = 4,
              Width = 16,
              Height = 16,
              Alignment = new Spot(0.5, 0.5, 0, -60)
            }
          )
        )
      );

      myDiagram = diagramControl1.Diagram;

      // show the selection Adornment only on the first selection if it is a Node
      myDiagram.ChangedSelection += (obj, e) => {
        if (e.Diagram.Selection.Count == 0) {
          var oldnode = selectionAdornment.AdornedPart;
          if (oldnode != null) oldnode.RemoveAdornment("Radial");
          selectionAdornment.AdornedElement = null;
        } else if (e.Diagram.Selection.First() is Node node) {
          selectionAdornment.AdornedElement = node;
          node.AddAdornment("Radial", selectionAdornment);
        }
      };
      myDiagram.InitialLayoutCompleted += (obj, e) => {
        // show the Adornment on the "Beta" node
        e.Diagram.Select(e.Diagram.FindNodeForKey(2));
      };
      myDiagram.UndoManager.IsEnabled = true;

      myDiagram.NodeTemplate = new Node(PanelLayoutAuto.Instance) {
        Resizable = true
      }.Add(
        new Shape {
          PortId = "",
          FromLinkable = true,
          ToLinkable = true,
          Cursor = "pointer"
        }.Bind("Fill", "Color"),
        new TextBlock {
          Margin = 8, Editable = true
        }.Bind(new Binding("Text").MakeTwoWay())
      ).Bind("DesiredSize", "Size", Northwoods.Go.Size.Parse, Northwoods.Go.Size.Stringify);

      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = 1, Text = "Alpha", Color = "lightblue" },
          new NodeData { Key = 2, Text = "Beta", Color = "orange" },
          new NodeData { Key = 3, Text = "Gamma", Color = "lightgreen" },
          new NodeData { Key = 4, Text = "Delta", Color = "pink" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = 1, To = 2 },
          new LinkData { From = 1, To = 3 },
          new LinkData { From = 2, To = 2 },
          new LinkData { From = 3, To = 4 },
          new LinkData { From = 4, To = 1 }
        }
      };
    }

  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }

  public class NodeData : Model.NodeData {
    public string Size { get; set; }
    public Brush Color { get; set; }
  }

  public class LinkData : Model.LinkData { }

  // A custom Button Panel with additional properties for the various button Brushes.
  public class SectorButton : Panel {
    public Brush ButtonFillNormal { get; set; } = "whitesmoke";
    public Brush ButtonStrokeNormal { get; set; } = "gray";
    public Brush ButtonFillOver { get; set; } = "dodgerblue";
    public Brush ButtonStrokeOver { get; set; } = "blue";
    public Brush ButtonFillDisabled { get; set; } = "darkgray";

    public static Geometry makeAnnularWedge(double angle, double sweep, double outer, double inner) {
      // the Geometry will be centered about (0,0)
      var p = new Point(outer, 0).Rotate(angle - sweep / 2);
      var q = new Point(inner, 0).Rotate(angle + sweep / 2);
      var geo = new Geometry()
        .Add(new PathFigure(-outer, -outer))  // always make sure the Geometry includes the top left corner
        .Add(new PathFigure(outer, outer))    // and the bottom right corner of the whole circular area
        .Add(new PathFigure(p.X, p.Y)  // start at outer corner, go clockwise
          .Add(new PathSegment(SegmentType.Arc, angle - sweep / 2, sweep, 0, 0, outer, outer))
          .Add(new PathSegment(SegmentType.Line, q.X, q.Y))  // to opposite inner corner, then anticlockwise
          .Add(new PathSegment(SegmentType.Arc, angle + sweep / 2, -sweep, 0, 0, inner, inner).Close()));
      return geo;
    }

    // Produce a "Button" that is shaped as the intersection of a circular sector and an annular ring.
    // The first argument is the angle at which the button will be placed relative to the center.
    // The second argument is the sweep angle over which the button will extend;
    // by default this is 90 degrees, as if there are exactly four buttons around the whole circle.
    public SectorButton(double angle, double sweep) : base(PanelLayoutSpot.Instance) {
      IsActionable = true; // needed so that the actiontool intercepts more events
      EnabledChanged = (button, enabled) => {
        var shape = (button as Panel).FindElement("ButtonBorder");
        if (shape != null) {
          (shape as Shape).Fill = enabled ? ButtonFillNormal : ButtonFillDisabled;
        }
      };

      var geo = makeAnnularWedge(angle, sweep, 80, 40);

      var pt = new Point(60, 0).Rotate(angle);
      var align = new Spot(0.5, 0.5, pt.X, pt.Y);

      // There's no GraphObject inside the button shape -- it must be added as part of the button definition.
      // This way the object could be a TextBlock or a Shape or a Picture or arbitrarily complex Panel.

      MouseEnter = (e, button, prev) => {
        var _shape = (button as Panel).FindElement("ButtonBorder"); // the border Shape
        if (_shape is Shape shape) {
          var sButton = button as SectorButton;
          var brush = sButton.ButtonFillOver;
          shape.Fill = brush;
          brush = sButton.ButtonStrokeOver;
          sButton.ButtonStrokeNormal = shape.Stroke;
          shape.Stroke = brush;
        }
      };

      MouseLeave = (e, button, next) => {
        var _shape = (button as Panel).FindElement("ButtonBorder"); // the border Shape
        if (_shape is Shape shape) {
          shape.Fill = (button as SectorButton).ButtonFillNormal;
          shape.Stroke = (button as SectorButton).ButtonStrokeNormal;
        }
      };

      Add(new Shape {
        Name = "ButtonBorder",
        Fill = ButtonFillNormal,
        Stroke = ButtonStrokeNormal,
        Geometry = geo
      });
    }
  }

}
