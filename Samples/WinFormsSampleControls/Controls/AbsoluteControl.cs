using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.Absolute {
  [ToolboxItem(false)]
  public partial class AbsoluteControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    
    public AbsoluteControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      goWebBrowser1.Html = @"


   <p>
      Absolute positioning within the viewport, with no scrolling (or panning) or zooming allowed.
    </p>
    <p>
      There is a special colored background Part that shows the fixed area where Parts may be.
      It is in the ""Grid"" Layer so that it is not selectable and is always behind the regular Parts.
    </p>
    <p>
      Parts may not be dragged outside of the fixed document area of the diagram.
      This is implemented by a custom <a>Part.DragComputation</a> function.
   </p>

   <p>
     Note that the user may still scroll or zoom the whole page.
   </p>

   <p>The model data, automatically updated after each change or undo or redo:</p>
";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.FixedBounds = new Rect(0, 0, 500, 300);   // document is always 500x300 units
      myDiagram.AllowHorizontalScroll = false;   // disallow scrolling or panning
      myDiagram.AllowVerticalScroll = false;
      myDiagram.AllowZoom = false;
      // myDiagram.AnimationManager.IsEnabled = false
      myDiagram.UndoManager.IsEnabled = true;
      myDiagram.ModelChanged += (obj, e) => {
        if (e.IsTransactionFinished) {  // show the model data in the page's TextArea
          Save();
        }
      };

      // the background Part showing the fixed bounds of the diagram contents
      myDiagram.Add(
        new Part {
          LayerName = "Grid",
          Position = myDiagram.FixedBounds.Position
        }.Add(
          new Shape {
            Fill = "oldlace",
            StrokeWidth = 0,
            DesiredSize = myDiagram.FixedBounds.Size
          })
        );

      // this function is the Node.DragComputation, to limit the movement of the parts
      // use GRIDPT instead of PT if DraggingTool.IsGridSnapEnabled and movement should snap to grid
      Point StayInFixedArea(Part part, Point pt, Point gridpt) {
        var diagram = part.Diagram;
        if (diagram == null) return pt;
        // compute the document area without padding
        var v = diagram.DocumentBounds;
        v.SubtractMargin(diagram.Padding);
        // get the bounds of the part being dragged
        var b = part.ActualBounds;
        var loc = part.Location;
        // now limit the location appropriately
        var x = Math.Max(v.X, Math.Min(pt.X, v.Right - b.Width)) + (loc.X - b.X);
        var y = Math.Max(v.Y, Math.Min(pt.Y, v.Bottom - b.Height)) + (loc.Y - b.Y);
        return new Point(x, y);
      }

      // node template
      myDiagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance) {
          DragComputation = StayInFixedArea
        }.Bind(
          // get the size from the model data
          new Binding("DesiredSize", "Size", Northwoods.Go.Size.Parse),
          // get and set the position in the model data
          new Binding("Position", "Pos", Point.Parse).MakeTwoWay(Point.Stringify),
          // temporarily put selected nodes in Foreground layer
          new Binding("LayerName", "IsSelected", (s, _) => { return (s as bool? ?? false) ? "Foreground" : ""; }).OfElement())
        .Add(
          new Shape {
            Figure = "Rectangle",
            StrokeWidth = 0
          }.Bind(   // avoid extra thickness from the stroke
            new Binding("Fill", "Color")),
          new TextBlock() {
            Wrap = Wrap.None, }.Bind(
            new Binding("Text", "Color"))
        );

      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key =  "Alpha", Pos =  "0 0", Size =  "50 50", Color =  "lightblue" },
          new NodeData { Key =  "Beta", Pos =  "276 19", Size =  "100 100", Color =  "orange" },
          new NodeData { Key =  "Gamma", Pos =  "44 214", Size =  "100 50", Color =  "lightgreen" },
          new NodeData { Key =  "Delta", Pos =  "239 171", Size =  "50 100", Color =  "pink" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = "Alpha", To = "Beta" },
          new LinkData { From = "Alpha", To = "Gamma" },
          new LinkData { From = "Gamma", To = "Delta" },
          new LinkData { From = "Delta", To = "Alpha" }
        }
      };
    }

    private void Save() {
      if (myDiagram == null) return;
      txtJSON.Text = myDiagram.Model.ToJson();
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Pos { get; set; }
    public string Size { get; set; }
    public string Color { get; set; }
  }

  public class LinkData : Model.LinkData { }

}
