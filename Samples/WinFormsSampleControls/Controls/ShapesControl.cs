using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Extensions;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.Shapes {
  [ToolboxItem(false)]
  public partial class ShapesControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public ShapesControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"
        <p>
      This sample showcases all predefined <b>GoDiagram</b> figures.
      This sample also makes use of <a href=""intro/highlighting.html"">GoDiagram Highlighting</a> data bindings: Mouse-hover over a shape to see its name.
        </p>
        <p>
      You can specify a predefined geometry for a <a>Shape</a> by setting its <a>Shape.Figure</a>.
        </p>
        <p>
      In order to reduce the size of the GoDiagram library, most predefined figures are in the
      <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/GraphObjects/Figures/Figures.cs"">Figures.cs</a> file.
      You can load this file or simply load only those figures that you want to use by copying their definitions into your code.
        </p>
        <p>
      A number of very common figures are predefined in GoDiagram: <code>""Rectangle"", ""Square"", ""RoundedRectangle"", ""Border"", ""Ellipse"", ""Circle"", ""TriangleRight"",
      ""TriangleDown"", ""TriangleLeft"", ""TriangleUp"", ""Triangle"", ""Diamond"", ""LineH"", ""LineV"", ""BarH"", ""BarV"", ""MinusLine"", ""PlusLine"", ""XLine""</code>.
      These figures are filled green above, instead of pink.
        </p>
        <p>
      With GoDiagram you can also define your own custom shapes with SVG - like path syntax, see the <a href=""Icons"">SVG icons</a>
      sample for examples or the <a href=""intro/geometry.html"">Geometry Path Strings intro page</a> to learn more.
        </p>
        <p>
      For predefined arrowheads, see the <a href=""Arrowheads"">Arrowheads</a> sample.
        </p>
";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // load extra figures
      Figures.DefineExtraFigures();

      // diagram properties
      // use a gridlayout
      myDiagram.Layout = new GridLayout {
        Sorting = GridSorting.Forward
      };
      // to see the names of shapes on the bottom row
      myDiagram.Padding = new Margin(5, 5, 25, 5);

      void MouseEnter(InputEvent e, GraphObject obj, GraphObject _) {
        var node = obj as Node;
        node.IsHighlighted = true;
      }

      void MouseLeave(InputEvent e, GraphObject obj, GraphObject _) {
        var node = obj as Node;
        node.IsHighlighted = false;
      }

      // Names of the built in shapes, which we will color green instead of pink.
      // The pinks shapes are instead defined in the extensions "Figures.cs" file.
      var builtIn = new HashSet<string>() {
      "Rectangle", "Square", "RoundedRectangle", "Border", "Ellipse", "Circle", "TriangleRight", "TriangleDown", "TriangleLeft", "TriangleUp", "Triangle", "Diamond", "LineH", "LineV", "None", "BarH", "BarV", "MinusLine", "PlusLine", "XLine"
      };
      bool IsBuiltIn(string shapeName) {
        return builtIn.Contains(shapeName);
      }

      // node template
      myDiagram.NodeTemplate =
        new Node(PanelLayoutVertical.Instance) {
          MouseEnter = MouseEnter,
          MouseLeave = MouseLeave,
          LocationSpot = Spot.Center,  // the location is the center of the Shape
          LocationElementName = "SHAPE",
          SelectionAdorned = false,  // no selection handle when selected
          Resizable = true,
          ResizeElementName = "SHAPE",  // user can resize the Shape
          Rotatable = true,
          RotateElementName = "SHAPE",  // rotate the Shape without rotating the label
          // don't re-layout when node changes size
          LayoutConditions = LayoutConditions.Standard & ~LayoutConditions.NodeSized
        }.Bind(
          new Binding("LayerName", "IsHighlighted", (h, _) => { return (h as bool? ?? false) ? "Foreground" : ""; }).OfElement()
        ).Add(
          new Shape {
            Name = "SHAPE",  // named so that the above properties can refer to this GraphObject
            Width = 70,
            Height = 70,
            StrokeWidth = 3
          }.Bind(
            // Color the built in shapes green, and the Figures.cs shapes Pink
            new Binding("Fill", "Key", (k, _) => {
              return IsBuiltIn(k as string) ? "palegreen" : "lightpink";
            }),
            new Binding("Stroke", "Key", (k, _) => {
              return IsBuiltIn(k as string) ? "darkgreen" : "#C2185B";
            }),
            // bind the Shape.Figure to the figure name, which automatically gives the Shape a Geometry
            new Binding("Figure", "Key")
          ),
          new TextBlock  // the label
            {
            Margin = 4,
            Font = new Font("Segoe UI", 18, FontWeight.Bold),
            Background = "white"
          }.Bind(
            new Binding("Visible", "IsHighlighted").OfElement(),
            new Binding("Text", "Key")
          )
        );

      // initialize the model
      myDiagram.Model = new Model {
        NodeDataSource = Shape.GetFigureGenerators().Keys.ToList().ConvertAll((str) => {
          var key = "" + char.ToUpper(str[0]) + str.Substring(1);
          return new NodeData {
            Key = key
          };
        })
      };
    }

  }

  // define the model data
  public class Model : Model<NodeData, string, object> { }
  public class NodeData : Model.NodeData { }

}
