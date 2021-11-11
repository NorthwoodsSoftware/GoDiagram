using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Extensions;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.Relationships {
  [ToolboxItem(false)]
  public partial class RelationshipsControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    
    public RelationshipsControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      goWebBrowser1.Html = @"
   <p>
    This illustrates how one can define custom strokes for Links (or really any Shape that is relatively straight)
    by making use of the <a>Shape.pathPattern</a> property to repeatedly draw a small Shape along the stroke path.
    These examples may be useful in generating diagrams showing social or emotional relationships or other cases
    where it is useful to distinguish kinds of relationships in more manners than just by the <a>Shape.stroke</a> (color)
    or <a>Shape.strokeWidth</a> or <a>Shape.strokeDashArray</a>.
  </p>
  <p>
    The first set of link triplets, at the top, demonstrate the basic pathPatterns defined by the <code>definePathPattern</code> function in this page.
    The last set of link doublets, at the bottom, demonstrate how those basic pathPatterns can be combined in a single <a>Link</a>
    that has two <a>Shape</a>s that have <a>GraphObject.isPanelMain</a> set to true, so that both shapes get the same <a>Geometry</a>
    computed by the link.  Yet each such link shape draws a different path pattern.
  </p>
";

    }

    // dictionary to store patterns
    Dictionary<string, Shape> pathPatterns = new Dictionary<string, Shape>();

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      Figures.DefineExtraFigures();

      // diagram properties
      myDiagram.Layout = new TreeLayout {
        LayerSpacing = 150,
        ArrangementSpacing = new Size(2, 2),
        SetsPortSpot = false,
        SetsChildPortSpot = false
      };

      // this typically represents a person
      myDiagram.NodeTemplate =
        new Node(PanelLayoutVertical.Instance).Add(
          new Shape {
            Figure = "Circle",
            DesiredSize = new Size(28, 28),
            Fill = "white",
            StrokeWidth = 1.5,
            PortId = ""
          }.Bind(
            new Binding("Figure")
          ),
          new TextBlock {
            Text = "name"
          }.Bind(
            new Binding("Text")
          )
        );

      // this template works for all kinds of relationships
      myDiagram.LinkTemplate =
        new Link { // slightly curved, by default
          Curve = LinkCurve.Bezier,
          Reshapable = true
        }.Add(  // users can reshape the link route
          new Shape { // the link's path shape
            IsPanelMain = true,
            Stroke = "transparent"
          }.Bind(
            new Binding("Stroke", "Patt", (f, _) => { return (f as string == "") ? "black" : "transparent"; }),
            new Binding("PathPattern", "Patt", ConvertPathPatternToShape)
          ),
          new Shape { // the link's path shape
            IsPanelMain = true,
            Stroke = "transparent",
            StrokeWidth = 3
          }.Bind(
            new Binding("PathPattern", "Patt2", ConvertPathPatternToShape)
          ),
          new Shape { // the "to" arrowhead
            ToArrow = "",
            Fill = null,
            Scale = 1.2
          }.Bind(
            new Binding("ToArrow"),
            new Binding("Stroke", "Patt", ConvertPathPatternToColor)
          ),
          new TextBlock { // show the path object name
            SegmentOffset = new Point(0, 12)
          }.Bind(
            new Binding("Text", "Patt")
          ),
          new TextBlock { // show the second path object name, if any
            SegmentOffset = new Point(0, -12)
          }.Bind(
            new Binding("Text", "Patt2")
          )
        );

      // model
      myDiagram.Model = new Model();

      // Conversion functions that make use of the PathPatterns store of pattern Shapes
      Shape ConvertPathPatternToShape(object nameAsObj, object _ = null) {
        var name = nameAsObj as string;
        if (name == null || name == "") return null;
        return pathPatterns[name];
      }

      object ConvertPathPatternToColor(object nameAsObj, object _ = null) {
        var name = nameAsObj as string;
        var pattobj = ConvertPathPatternToShape(name);
        return (pattobj != null) ? pattobj.Stroke : "transparent";
      }

      void DefinePathPattern(string name, string geostr, string color = "black", double width = 1, LineCap cap = LineCap.Square) {
        pathPatterns.Add(name,
          new Shape {
            GeometryString = geostr,
            Fill = "transparent",
            Stroke = color,
            StrokeWidth = width,
            StrokeCap = cap
          }
        );
      }

      DefinePathPattern("Single", "M0 0 L1 0");
      DefinePathPattern("Double", "M0 0 L1 0 M0 3 L1 3");
      DefinePathPattern("Triple", "M0 0 L1 0 M0 3 L1 3 M0 6 L1 6");
      DefinePathPattern("DashR", "M0 0 M3 0 L6 0", "red");
      DefinePathPattern("DoubleDashR", "M0 0 M3 0 L6 0 M3 3 L6 3", "red");
      DefinePathPattern("TripleDashR", "M0 0 M3 0 L6 0 M3 3 L6 3 M3 6 L6 6", "red");
      DefinePathPattern("Dash", "M0 0 M3 0 L6 0");
      DefinePathPattern("DoubleDash", "M0 0 M3 0 L6 0 M3 3 L6 3");
      //DefinePathPattern("TripleDash", "M0 0 M3 0 L6 0 M3 3 L6 3 M3 6 L6 6");
      DefinePathPattern("Dot", "M0 0 M4 0 L4.1 0", "black", 2, LineCap.Round);
      DefinePathPattern("DoubleDot", "M0 0 M4 0 L4.1 0 M4 3 L4.1 3", "black", 2, LineCap.Round);
      DefinePathPattern("SingleG", "M0 0 L1 0", "green");
      DefinePathPattern("DoubleG", "M0 0 L1 0 M0 3 L1 3", "green");
      DefinePathPattern("SingleR", "M0 0 L1 0", "red");
      DefinePathPattern("TripleR", "M0 0 L1 0 M0 3 L1 3 M0 6 L1 6", "red");
      DefinePathPattern("ZigzagB", "M0 3 L1 0 3 6 4 3", "blue");
      DefinePathPattern("ZigzagR", "M0 3 L1 0 3 6 4 3", "red");
      DefinePathPattern("BigZigzagR", "M0 4 L2 0 6 8 8 4", "red");
      DefinePathPattern("DoubleZigzagB", "M0 3 L1 0 3 6 4 3 M0 9 L1 6 3 12 4 9", "blue");
      DefinePathPattern("CrossG", "M0 0 M3 0 M1 0 L1 8", "green");
      DefinePathPattern("CrossR", "M0 0 M3 0 M1 0 L1 8", "red");
      //DefinePathPattern("Railroad", "M0 2 L3 2 M0 6 L3 6 M1 0 L1 8");  // also == Double & Cross
      DefinePathPattern("BackSlash", "M0 3 L2 6 M1 0 L5 6 M4 0 L6 3");
      DefinePathPattern("Slash", "M0 3 L2 0 M1 6 L5 0 M4 6 L6 3");
      DefinePathPattern("Coil", "M0 0 C2.5 0  5 2.5  5 5  C5 7.5  5 10  2.5 10  C0 10  0 7.5  0 5  C0 2.5  2.5 0  5 0");
      DefinePathPattern("Square", "M0 0 M1 0 L7 0 7 6 1 6z");
      DefinePathPattern("Circle", "M0 3 A3 3 0 1 0 6 4  A3 3 0 1 0 0 3");
      DefinePathPattern("BigCircle", "M0 5 A5 5 0 1 0 10 5  A5 5 0 1 0 0 5");
      DefinePathPattern("Triangle", "M0 0 L4 4 0 8z");
      DefinePathPattern("Diamond", "M0 4 L4 0 8 4 4 8z");
      DefinePathPattern("Dentil", "M0 0 L2 0  2 6  6 6  6 0  8 0");
      DefinePathPattern("Greek", "M0 0 L1 0  1 3  0 3  M0 6 L4 6  4 0  8 0  M8 3 L7 3  7 6  8 6");
      DefinePathPattern("Seed", "M0 0 A9 9 0 0 0 12 0  A9 9 180 0 0 0 0");
      DefinePathPattern("SemiCircle", "M0 0 A4 4 0 0 1 8 0");
      DefinePathPattern("BlindHem", "M0 4 L2 4  4 0  6 4  8 4");
      DefinePathPattern("Zipper", "M0 4 L1 4 1 0 8 0 8 4 9 4  M0 6 L3 6 3 2 6 2 6 6 9 6");
      //DefinePathPattern("Zipper2", "M0 4 L1 4 1 0 8 0 8 4 9 4  M0 7 L3 7 3 3 6 3 6 7 9 7");
      DefinePathPattern("Herringbone", "M0 2 L2 4 0 6  M2 0 L4 2  M4 6 L2 8");
      DefinePathPattern("Sawtooth", "M0 3 L4 0 2 6 6 3");

      // helper function for creating sequential chains of nodes
      void AddLinks(string patt1a, string patt1b, string patt2a, string patt2b, string patt3a = "", string patt3b = "") {
        var arrow = "OpenTriangle";
        var model = myDiagram.Model as Model;
        var left = new NodeData { Figure = "Square" };
           model.AddNodeData(left);
        var middle = new NodeData { Figure = "Circle" };
        model.AddNodeData(middle);
        model.AddLinkData(new LinkData { From = left.Key, To = middle.Key, Patt = patt1a, Patt2 = patt1b, ToArrow = arrow });

        if (patt2a != "") {
          var right = new NodeData { Figure = "Triangle" };
          model.AddNodeData(right);
          model.AddLinkData(new LinkData { From = middle.Key, To = right.Key, Patt = patt2a, Patt2 = patt2b, ToArrow = arrow });

          if (patt3a != "") {
            var farright = new NodeData { Figure = "Diamond" };
            model.AddNodeData(farright);
            model.AddLinkData(new LinkData { From = right.Key, To = farright.Key, Patt = patt3a, Patt2 = patt3b, ToArrow = arrow });
          }
        }
      }

      // simple path objects
      var it = pathPatterns.GetEnumerator();
      while (it.MoveNext()) {
        AddLinks(it.Current.Key, "", it.MoveNext() ? it.Current.Key : "", "", it.MoveNext() ? it.Current.Key : "");
      }
      // compound path objects
      AddLinks("DoubleG", "CrossG", "Single", "CrossR");
      AddLinks("Dash", "ZigzagR", "Dash", "BigZigzagR");
      AddLinks("Double", "ZigzagR", "Double", "BigZigzagR");
      AddLinks("Triple", "ZigzagR", "Triple", "BigZigzagR");


    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {

    public string Figure { get; set; }
  }

  public class LinkData : Model.LinkData {
    public string Patt { get; set; }
    public string Patt2 { get; set; }
    public string ToArrow { get; set; }
  }

}
