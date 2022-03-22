using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.PanelLayout {
  [ToolboxItem(false)]
  public partial class PanelLayoutControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public PanelLayoutControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"
        <p>
      This sample demonstrates creating a simple custom <a>PanelLayout</a>. It merely cascades the elements diagonally,
      as if combining a Horizontal and Vertical panel.
        </p>
        <p>
      Creating your own Panel layouts is very uncommon, and you should not need to create your own to use GoDiagram effectively.
      However there may be apps that require creating a custom Panel layout because the standard panel layouts
      do not offer the precise behavior that you want.
        </p>
";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // make an instance of the PanelLayout so we can reference it when defining templates
      var cascadingPanelLayout = new PanelLayoutCascading();

      // diagram properties
      myDiagram.UndoManager.IsEnabled = true;

      // Define a simple Node template that includes a Cascading Panel holding
      // some number of item Panels holding Shapes, based on data.Items being an Array
      // of item descriptor objects.
      myDiagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance) {
          Width = 100,
          Height = 100
        }.Add(
          new Shape {
            Fill = "transparent",
            StrokeWidth = 2
          }.Bind(
            new Binding("Stroke", "Color")
          ),
          new Panel(cascadingPanelLayout) {
            ItemTemplate =
              new Panel().Add(
                new Shape {
                  Width = 20,
                  Height = 20,
                  StrokeWidth = 0
                }.Bind(
                  new Binding("Figure", "Fig"),
                  new Binding("Fill", "Color")
                )
              )
          }.Bind(
            new Binding("ItemList", "Items")
          )
        );

      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData {
            Key = 1,
            Color = "lightgreen",
            Items = new List<FieldData> {
              new FieldData { Fig = "RoundedRectangle", Color = "lightblue" },
              new FieldData { Fig = "Triangle", Color = "pink" }
            }
          },
          new NodeData {
            Key = 2,
            Color = "lightblue",
            Items = new List<FieldData> {
              new FieldData { Fig = "RoundedRectangle", Color = "lightgray" },
              new FieldData { Fig = "Square", Color = "yellow" },
               new FieldData { Fig = "Circle", Color = "orange" }
            }
          },
          new NodeData {
            Key = 3,
            Color = "purple",
            Items = new List<FieldData> {
              new FieldData { Fig = "Diamond", Color = "red" }
            }
          },
          new NodeData {
            Key = 4,
            Color = "orange",
            Items = new List<FieldData> {
              new FieldData { Fig = "Circle", Color = "green" },
              new FieldData { Fig = "Square", Color = "blue" },
              new FieldData { Fig = "Triangle", Color = "red" },
              new FieldData { Fig = "TriangleRight", Color = "green" }
            }
          }
        }
      };
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Color { get; set; }
    public List<FieldData> Items { get; set; }
  }

  public class LinkData : Model.LinkData { }

  public class FieldData {
    public string Fig { get; set; }
    public string Color { get; set; }
  }

  // extend PanelLayout
  public class PanelLayoutCascading : Northwoods.Go.PanelLayout {
    /**
     * Given the available size, measure the Panel and
     * determine its expected drawing size. Sets the measuredBounds of the object.
     *
     * This must call {@link #measureElement} with each Panel element.
     *
     * This must also construct the union.width and union.height of the passed in union Rect argument.
     *
     * @this {PanelLayout}
     * @param {Panel} panel Panel which called this layout
     * @param {number} width expected width of the panel
     * @param {number} height expected width of the panel
     * @param {Array.<GraphObject>} elements Array of Panel elements
     * @param {Rect} union rectangle to contain the expected union bounds of every element in the Panel. Useful for arrange.
     * @param {number} minw minimum width of the panel
     * @param {number} minh minimum height of the panel
     */
    public override void Measure(Panel panel, double width, double height, IList<GraphObject> elements, ref Rect union, double minw, double minh) {
      var l = elements.Count;
      for (var i = 0; i < l; i++) {
        var elem = elements[i];
        MeasureElement(elem, width, height, minw, minh);
        var mb = elem.MeasuredBounds;
        union.Width += mb.Width;
        union.Height += mb.Height;
      }
    }

    /**
     * Given the panel and its list of elements, arrange each element.
     *
     * This must call {@link #arrangeElement} with each Panel element, which will set that element's {@link GraphObject#actualBounds}.
     *
     * For arranging some elements, it is useful to know the total unioned area of every element.
     * This Rect can be used to right-align or center-align, etc, elements within an area.
     *
     * @this {PanelLayout}
     * @param {Panel} panel Panel which called this layout
     * @param {Rect} ar arranged bounds of the panel
     * @param {Array.<GraphObject>} elements Array of Panel elements
     * @param {Rect} union rectangle, if properly constructed in {@link #measure}, that contains the expected union bounds of every element in the Panel.
     */
    public override void Arrange(Panel panel, IList<GraphObject> elements, Rect union) {
      var l = elements.Count;
      var x = 0.0;
      var y = 0.0;
      for (var i = 0; i < l; i++) {
        var elem = elements[i];
        var mb = elem.MeasuredBounds;
        ArrangeElement(elem, x, y, mb.Width, mb.Height);
        /*
        * By incrementing the arranged x and y by the width and height, we arrange each object in a diagonal fashion:
        *  A
        *    B
        *      C
        *        D
        */
        x += mb.Width;
        y += mb.Height;
      }
    }
  }

}
