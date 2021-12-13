using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.WinForms;
using Northwoods.Go.Tools;
using Northwoods.Go.Layouts;
using System.Linq;

namespace WinFormsSampleControls.Gantt {
  [ToolboxItem(false)]
  public partial class GanttControl : System.Windows.Forms.UserControl {

    private Diagram myDiagram;

    private List<string> _Months = new List<string> {
      "", "January", "February", "March", "April",
      "May", "June", "July", "August", "September",
      "October", "November", "December"
    };

    public double WidthFactor { get; set; } = 1;

    Part dateScale = null;

    public GanttControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      trackBar1.ValueChanged += (e, obj) => rescale();

      goWebBrowser1.Html = @"
           <p>
            This sample demonstrates a simple Gantt chart. Gantt charts are used to illustrate project schedules, denoting the start and end dates for terminal and summary elements of the project.
          </p>
          <p>
            You can zoom in on the diagram by changing the ""Spacing"" value,
            which scales the diagram using a data binding function for nodes' widths and locations.
            This is in place of changing the <a>Diagram.Scale</a>.
          </p>
        ";
    }

    private void Setup() {

      myDiagram = diagramControl1.Diagram;

      myDiagram.IsReadOnly = true;
      myDiagram.AllowZoom = false;
      myDiagram.Grid.Visible = true;
      myDiagram.Grid.GridCellSize = new Size(30, 150);

      // create the template for the standard nodes
      myDiagram.NodeTemplate = new Node(PanelLayoutAuto.Instance) {
        // links come from the right and go to the left side of the top of the node
        FromSpot = Spot.Right,
        ToSpot = new Spot(0.001, 0, 11, 0)
      }.Add(
        new Shape {
          Figure = "Rectangle",
          Height = 15
        }.Bind("Fill", "Color")
         .Bind("Width", "Width", (w, _) => scaleWidth((double)w)),
        new TextBlock {
          Margin = 2,
          Alignment = Spot.Left
        }.Bind("Text", "Key"))
      .Bind(
        // using a function in the Binding allows the value to change
        // when Diagram.UpdateAllTargetBindings is called
        new Binding("Location", "Loc", (l, _) => new Point(scaleWidth(((Point)l).X), ((Point)l).Y))
      );

      // create the template for the start node
      myDiagram.NodeTemplateMap.Add("Start", new Node() {
        FromSpot = Spot.Right, ToSpot = Spot.Top, Selectable = false
      }.Add(
       new Shape {
         Figure = "Diamond",
         Height = 15,
         Width = 15,
         Fill = "black"
       }).Bind(
       // make sure the location of the start node is not scalable
       new Binding("Location", "Loc")
      ));

      myDiagram.NodeTemplateMap.Add("End",
        new Node() {
          FromSpot = Spot.Right, ToSpot = Spot.Top, Selectable = false
        }
        .Add(
         new Shape {
           Figure = "Diamond",
           Height = 15,
           Width = 15,
           Fill = "black"
         })
        .Bind(
         // make the location of the end node (with location.X < 0) scalable
         new Binding("Location", "Loc", (l, _) => {
           var pt = (Point)l;
           if (pt.X >= 0) return new Point(scaleWidth(pt.X), pt.Y);
           return pt;
         })
        ));

      // create the link template
      myDiagram.LinkTemplate = new Link {
        Routing = LinkRouting.Orthogonal,
        Corner = 3,
        ToShortLength = 2,
        Selectable = false
      }.Add(
        new Shape { StrokeWidth = 2 },
        new Shape { ToArrow = "OpenTriangle" }
      );

      // add the nodes and links to the model
      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key =  "a", Color =  "coral", Width =  120, Loc =  new Point(scaleWidth(0), 40) },
          new NodeData { Key =  "b", Color =  "turquoise", Width =  160, Loc =  new Point(scaleWidth(0), 60) },
          new NodeData { Key =  "c", Color =  "coral", Width =  150, Loc =  new Point(scaleWidth(120), 80) },
          new NodeData { Key =  "d", Color =  "turquoise", Width =  190, Loc =  new Point(scaleWidth(120), 100) },
          new NodeData { Key =  "e", Color =  "coral", Width =  150, Loc =  new Point(scaleWidth(270), 120) },
          new NodeData { Key =  "f", Color =  "turquoise", Width =  130, Loc =  new Point(scaleWidth(310), 140) },
          new NodeData { Key =  "g", Color =  "coral", Width =  155, Loc =  new Point(scaleWidth(420), 160) },
          new NodeData { Key =  "begin", Category =  "Start", Loc =  new Point(-15, 20) },
          new NodeData { Key =  "end", Category =  "End", Loc =  new Point(scaleWidth(575), 180) }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = "begin", To = "a" },
          new LinkData { From = "begin", To = "a" },
          new LinkData { From = "a", To = "c" },
          new LinkData { From = "a", To = "d" },
          new LinkData { From = "c", To = "e" },
          new LinkData { From = "d", To = "f" },
          new LinkData { From = "e", To = "g" },
          new LinkData { From = "f", To = "end" },
          new LinkData { From = "g", To = "end" },
        }
      };

      dateScale = new Part(PanelLayoutGraduated.Instance) {
        GraduatedTickUnit = 1,
        GraduatedMin = 0,
        GraduatedMax = 3,
        Pickable = false,
        Location = new Point(0, 0),
        Name = "dateScale"
      }.Add(
        new Shape {
          Name = "line",
          StrokeWidth = 0,
          GeometryString = "M0 0 H" + scaleWidth(450)
        },
        new TextBlock {
          Name = "labels",
          Font = "10pt sans-serif",
          AlignmentFocus = new Spot(0, 0, -3, -3),
          GraduatedFunction = v => {
            var d = new DateTime(2017, 6, 23);
            d = d.AddDays(v * 7);
            // format date output to string
            return _Months[d.Month] + " " + d.Day;
          }
        }
      );
      myDiagram.Add(dateScale);
    }

    private double scaleWidth(double num) {
      return num * WidthFactor;
    }

    // change the grid's cell size and the width factor,
    // then update Bindings to scale the widths and positions of nodes,
    // as well as the width of the date scale
    private void rescale() {
      var val = trackBar1.Value;
      myDiagram = diagramControl1.Diagram;
      if (myDiagram == null) return;

      myDiagram.StartTransaction("rescale");
      myDiagram.Grid.GridCellSize = new Size(val, 150);
      WidthFactor = val / 30.0;
      myDiagram.UpdateAllTargetBindings();
      // update width of date scale and maybe change interval of labels if too small
      var width = scaleWidth(450);
      
      
      (dateScale.FindElement("line") as Shape).GeometryString = "M0 0 H" + width;
      if (width >= 140) (dateScale.FindElement("labels") as TextBlock).Interval = 1;
      if (width < 140) (dateScale.FindElement("labels") as TextBlock).Interval = 2;
      if (width < 70) (dateScale.FindElement("labels") as TextBlock).Interval = 4;
      myDiagram.CommitTransaction("rescale");
    }
  }
  
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }

  public class NodeData : Model.NodeData {
    public string Color { get; set; }
    public double Width { get; set; }
    public Point Loc { get; set; }
  }

  public class LinkData : Model.LinkData { }
}
