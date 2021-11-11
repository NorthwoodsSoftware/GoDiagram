using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.IVR {
  [ToolboxItem(false)]
  public partial class IVRControl : System.Windows.Forms.UserControl {
    private Diagram MyDiagram;

    public IVRControl() 
    {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;
      goWebBrowser1.Html = @"
        <p>
          An <em>IVR tree</em>, or Interactive Voice Response Tree, is typically used by
          automated answering systems to direct calls to the correct party. This particular example
          is for a car dealership to route calls.
        </p>
        <p>
          This Interactive Voice Response Tree (IVR tree) has nodes that contain a collapsible list of actions, controlled by a <b>PanelExpanderButton</b>,
          with a <b>TreeExpanderButton</b> underneath the body. See the <a href=""../intro/buttons.html"">Intro page on Buttons</a> for more GoDiagram button information.
        </p>
      ";
    }

    private void Setup() {

      Shape.DefineFigureGenerator("IrritationHazard", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(.2 * w, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, .5 * w, .3 * h));
        fig.Add(new PathSegment(SegmentType.Line, .8 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, .2 * h));
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, .8 * h));
        fig.Add(new PathSegment(SegmentType.Line, .8 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, .5 * w, .7 * h));
        fig.Add(new PathSegment(SegmentType.Line, .2 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, .8 * h));
        fig.Add(new PathSegment(SegmentType.Line, .3 * w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, .2 * h).Close());
        geo.Spot1 = new Spot(.3, .3);
        geo.Spot2 = new Spot(.7, .7);
        return geo;
      });

      Shape.DefineFigureGenerator("ElectricalHazard", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(.37 * w, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, .5 * w, .11 * h));
        fig.Add(new PathSegment(SegmentType.Line, .77 * w, .04 * h));
        fig.Add(new PathSegment(SegmentType.Line, .33 * w, .49 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, .37 * h));
        fig.Add(new PathSegment(SegmentType.Line, .63 * w, .86 * h));
        fig.Add(new PathSegment(SegmentType.Line, .77 * w, .91 * h));
        fig.Add(new PathSegment(SegmentType.Line, .34 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, .34 * w, .78 * h));
        fig.Add(new PathSegment(SegmentType.Line, .44 * w, .8 * h));
        fig.Add(new PathSegment(SegmentType.Line, .65 * w, .56 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, .68 * h).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("FireHazard", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(.1 * w, h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Bezier, .29 * w, 0, -.25 * w, .63 * h,
          .45 * w, .44 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, .51 * w, .42 * h, .48 * w, .17 * h,
          .54 * w, .35 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, .59 * w, .18 * h, .59 * w, .29 * h,
          .58 * w, .28 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, .75 * w, .6 * h, .8 * w, .34 * h,
          .88 * w, .43 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, .88 * w, .31 * h, .87 * w, .48 * h,
          .88 * w, .43 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, .9 * w, h, 1.17 * w, .76 * h,
          .82 * w, .8 * h).Close());
        geo.Spot1 = new Spot(.07, .445);
        geo.Spot2 = new Spot(.884, .958);
        return geo;
      });

      MyDiagram = diagramControl1.Diagram;

      MyDiagram.AllowCopy = false;
      MyDiagram.ToolManager.DraggingTool.DragsTree = true;
      MyDiagram.CommandHandler.DeletesTree = true;
      MyDiagram.Layout = new TreeLayout {
        Angle = 90,
        Arrangement = TreeArrangement.FixedRoots
      };
      MyDiagram.UndoManager.IsEnabled = true;

      var bluegrad = new Brush(new LinearGradientPaint(new Dictionary<float, string> {
          { 0, "#C4ECFF" },
          { 1, "#70D4FF" }
        }
      ));

      var greengrad = new Brush(new LinearGradientPaint(new Dictionary<float, string> {
          { 0, "#B1E2A5" },
          { 1, "#7AE060" }
        }
      ));

      // each action is represented by a shape and some text
      var actionTemplate = new Panel(PanelLayoutHorizontal.Instance).Add(
        new Shape {
          Width = 12, Height = 12
        }.Bind("Figure").Bind("Fill"),
        new TextBlock {
          Font = "Segoe UI, 10px",
          Stroke = "black",
        }.Bind("Text")
      );

      // each regular Node has body consisting of a title followed by a collapsible list of actions,
      // controlled by a PanelExpanderButton, with a TreeExpanderButton underneath the body
      MyDiagram.NodeTemplate = new Node(PanelLayoutVertical.Instance) {
        SelectionElementName = "BODY"
      }.Add(
        // the main body consists of a RoundedRectangle surrounding nested Panels
        new Panel(PanelLayoutAuto.Instance) {
          Name = "BODY"
        }.Add(
          new Shape("Rectangle") {
            Fill = bluegrad, Stroke = null
          },
          new Panel(PanelLayoutVertical.Instance) {
            Margin = 3
          }.Add(
            // the title
            new TextBlock {
              Stretch = Stretch.Horizontal,
              Font = "Segoe UI, 12px, style=bold",
              Stroke = "black",
            }.Bind("Text", "Question"),
            // the optional list of actions
            new Panel(PanelLayoutVertical.Instance) {
              Stretch = Stretch.Horizontal, Visible = false
            }.Bind(
              "Visible", "Actions", (acts, _) => {
                return acts is List<FieldData> l && l.Count() > 0;
              }).Add(
              // headered by a label and a PanelExpanderButton inside a table
              new Panel(PanelLayoutTable.Instance) {
                Stretch = Stretch.Horizontal
              }.Add(
                new TextBlock("Choices") {
                  Alignment = Spot.Left,
                  Font = "Segoe UI, 10px",
                  Stroke = "black",
                },
                Builder.Make<Panel>("PanelExpanderButton")
                  .Set(new { Column = 1, Alignment = Spot.Right })
              ), // end Table panel
                 // with the list data bound in the Vertical Panel
              new Panel(PanelLayoutVertical.Instance) {
                Name = "COLLAPSIBLE",
                Padding = 2,
                Stretch = Stretch.Horizontal, // take up whole available width
                Background = "white", // to distinguish from the node's body
                DefaultAlignment = Spot.Left, // thus no need to specify alignment on each element
                ItemTemplate = actionTemplate // the Panel created for each item in Panel.ItemList
              }.Bind("ItemList", "Actions") // bind Panel.ItemList to nodedata.actions
            )
          )
        ),
        new Panel {
          Height = 17 // underneath the body, always this height
        }.Add(Builder.Make<Panel>("TreeExpanderButton"))
      );

      MyDiagram.NodeTemplateMap.Add("Terminal", new Node(PanelLayoutSpot.Instance).Add(
        new Shape("Circle") {
          Width = 55, Height = 55, Fill = greengrad, Stroke = null
        },
        new TextBlock {
          Font = "Segoe UI, 10px, style=bold",
          Stroke = "black",
        }.Bind("Text")
      ));

      MyDiagram.LinkTemplate = new Link {
        Routing = LinkRouting.Orthogonal,
        Deletable = false,
        Corner = 10
      }.Add(
        new Shape { StrokeWidth = 2 },
        new TextBlock {
          Background = "white",
          Visible = false, // unless the binding sets it to true for a non-empty string
          SegmentIndex = -2,
          SegmentOrientation = Orientation.None
        }.Bind(
          new Binding("Text", "Answer", (a, _) => a.ToString()),
          new Binding("Visible", "Answer", (a, _) => (int)a != 0)
        )
      );

      var nodeDataSource = new List<NodeData> {
        new NodeData {
          Key = 1, Question = "Greeting",
          Actions = new List<FieldData> {
            new FieldData { Text = "Sales", Figure = "ElectricalHazard", Fill = "blue" },
            new FieldData { Text = "Parts and Services", Figure = "FireHazard", Fill = "red" },
            new FieldData { Text = "Representative", Figure = "IrritationHazard", Fill = "yellow" }
          }
        },
        new NodeData {
          Key = 2, Question = "Sales",
          Actions = new List<FieldData> {
            new FieldData { Text = "Compact", Figure = "ElectricalHazard", Fill = "blue" },
            new FieldData { Text = "Mid-Size", Figure = "FireHazard", Fill = "red" },
            new FieldData { Text = "Large", Figure = "IrritationHazard", Fill = "yellow" }
          }
        },
        new NodeData {
          Key = 3, Question = "Parts and Services",
          Actions = new List<FieldData> {
            new FieldData { Text = "Maintenance", Figure = "ElectricalHazard", Fill = "blue" },
            new FieldData { Text = "Repairs", Figure = "FireHazard", Fill = "red" },
            new FieldData { Text = "State Inspection", Figure = "IrritationHazard", Fill = "yellow" }
          }
        },
        new NodeData { Key = 4, Question = "Representative" },
        new NodeData { Key = 5, Question = "Compact" },
        new NodeData { Key = 6, Question = "Mid-Size" },
        new NodeData {
          Key = 7, Question = "Large",
          Actions = new List<FieldData> {
            new FieldData { Text = "SUV", Figure = "ElectricalHazard", Fill = "blue" },
            new FieldData { Text = "Van", Figure = "FireHazard", Fill = "red" }
          }
        },
        new NodeData { Key = 8, Question = "Maintenance" },
        new NodeData { Key = 9, Question = "Repairs" },
        new NodeData { Key = 10, Question = "State Inspection" },
        new NodeData { Key = 11, Question = "SUV" },
        new NodeData { Key = 12, Question = "Van" },
        new NodeData { Key = 13, Category = "Terminal", Text = "Susan" },
        new NodeData { Key = 14, Category = "Terminal", Text = "Eric" },
        new NodeData { Key = 15, Category = "Terminal", Text = "Steven" },
        new NodeData { Key = 16, Category = "Terminal", Text = "Tom" },
        new NodeData { Key = 17, Category = "Terminal", Text = "Emily" },
        new NodeData { Key = 18, Category = "Terminal", Text = "Tony" },
        new NodeData { Key = 19, Category = "Terminal", Text = "Ken" },
        new NodeData { Key = 20, Category = "Terminal", Text = "Rachel" }
      };

      var linkDataSource = new List<LinkData> {
        new LinkData { From = 1, To = 2, Answer = 1 },
        new LinkData { From = 1, To = 3, Answer = 2 },
        new LinkData { From = 1, To = 4, Answer = 3 },
        new LinkData { From = 2, To = 5, Answer = 1 },
        new LinkData { From = 2, To = 6, Answer = 2 },
        new LinkData { From = 2, To = 7, Answer = 3 },
        new LinkData { From = 3, To = 8, Answer = 1 },
        new LinkData { From = 3, To = 9, Answer = 2 },
        new LinkData { From = 3, To = 10, Answer = 3 },
        new LinkData { From = 7, To = 11, Answer = 1 },
        new LinkData { From = 7, To = 12, Answer = 2 },
        new LinkData { From = 5, To = 13 },
        new LinkData { From = 6, To = 14 },
        new LinkData { From = 11, To = 15 },
        new LinkData { From = 12, To = 16 },
        new LinkData { From = 8, To = 17 },
        new LinkData { From = 9, To = 18 },
        new LinkData { From = 10, To = 19 },
        new LinkData { From = 4, To = 20 }
      };

      // create the Model with the above data, and assign to the Diagram
      MyDiagram.Model = new Model {
        NodeDataSource = nodeDataSource,
        LinkDataSource = linkDataSource
      };
    }

  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }

  public class NodeData : Model.NodeData {
    public string Question { get; set; }
    public List<FieldData> Actions { get; set; }
  }

  public class LinkData : Model.LinkData {
    public int Answer { get; set; }
  }

  public class FieldData {
    public string Text { get; set; }
    public string Figure { get; set; }
    public Brush Fill { get; set; }
  }

}
