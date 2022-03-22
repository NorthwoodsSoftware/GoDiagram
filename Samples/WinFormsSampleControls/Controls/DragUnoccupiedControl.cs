using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;
using Northwoods.Go.WinForms;

namespace WinFormsSampleControls.DragUnoccupied {
  [ToolboxItem(false)]
  public partial class DragUnoccupiedControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    private Palette myPalette;
    private Part sharedNodeTemplate;

    public DragUnoccupiedControl() {
      InitializeComponent();

      Setup();
      SetupPalette();

      goWebBrowser1.Html = @"

  <p>
    Drag a node around.
    Notice how you cannot force the dragged node to overlap any other (stationary) node.
    If you drag more than one node, notice how the relative positions of the dragged nodes are maintained
    except when forced to be shifted in order to avoid overlapping other nodes.
  </p>
  <p>
    This functionality is implemented by a custom <a>Part.DragComputation</a> property function,
    which affects how the <a>DraggingTool</a> can move selected nodes.
    You will want to adjust how it finds an empty spot for the dragged node when dragging from another Diagram.
  </p>
";

    }

    // nested function used by Layer.FindObjectsIn, below
    // only consider Parts, and ignore the given Node, any Links, and Group members
    private Part _Navigate(GraphObject obj, Part node) {
      var part = obj.Part;
      if (part == node) return null;
      if (part is Link) return null;
      if (part.IsMemberOf(node)) return null;
      if (node.IsMemberOf(part)) return null;
      return part;
    }

    // R is a rect in document coords
    // NODE is the node being moved, ignore when looking for Parts intersecting the Rect
    private bool IsUnoccupied(Rect r, Part node) {
      var Diagram = node.Diagram;

      // only consider non-temporary layers
      var lit = Diagram.Layers;
      foreach (var lay in lit) {
        if (lay.IsTemporary) continue;
        if (lay.FindElementsIn(r, obj => _Navigate(obj, node), null, true).Count > 0) return false;
      }
      return true;
    }

    // a Part.dragComputation function that prevents a Part from being dragged to overlap another Part
    // use PT instead of GRIDPT if DraggingTool.isGridSnapEnabled but movement should not snap to grid
    private Point avoidNodeOverlap(Part node, Point pt, Point gridpt) {
      if (node.Diagram is Palette) return gridpt;
      // assume each node is fully rectangular
      var bnds = node.ActualBounds;
      var loc = node.Location;
      var flag = new HashSet<Part>();

      // use PT instead of GRIDPT if you want to ignore any grid snapping behavior
      // see if the area at the proposed location is unoccupied
      var r = new Rect(gridpt.X - (loc.X - bnds.X), gridpt.Y - (loc.Y - bnds.Y), bnds.Width, bnds.Height);
      // maybe inflate R if you want some space between the node and any other nodes
      r.Inflate(-0.5, -0.5); // by default, defalte to avoid edge overlaps with exact fits
      // when dragging a node from another Diagra, choose an unoccupied area
      if (!(node.Diagram.CurrentTool is DraggingTool) &&
        (!flag.Contains(node) || !node.Layer.IsTemporary)) { // in Temporary Layer during external drag-and-drop
        flag.Add(node); // flag to avoid repeated searches during external drag-and-drop
        while (!IsUnoccupied(r, node)) {
          r.X += 10;
          r.Y += 10;
          // note: this an unimaginitive search algorithm, and can be improved
        }
        r.Inflate(0.5, 0.5); // restore to actual size
        // return the proposed new location point
        return new Point(r.X - (loc.X - bnds.X), r.Y - (loc.Y - bnds.Y));
      }
      if (IsUnoccupied(r, node)) return gridpt; // OK

      return loc; // give up -- don't allow the node to be moved to the new location
    }

    // Must use sharedNodeTemplate because don't know if palette or diagram will be initialized first
    private void DefineNodeTemplate() {
      if (sharedNodeTemplate != null) return;  // already defined

      sharedNodeTemplate = new Node(PanelLayoutAuto.Instance) {
        DragComputation = avoidNodeOverlap,
        MinSize = new Size(50, 20),
        Resizable = true
      }.Bind(
        new Binding("DesiredSize", "Size", Northwoods.Go.Size.Parse, Northwoods.Go.Size.Stringify),
        new Binding("Position", "Pos", Point.Parse, Point.Stringify),
        // temporarily put selected nodes in Foreground layer
        new Binding("LayerName", "IsSelected", (s, obj) => (bool)s ? "Foreground" : "").OfElement()
      ).Add(
        new Shape {
          Figure = "Rectangle"
        }.Bind("Fill", "Color"),
        new TextBlock()
         .Bind("Text", "Color")
      );
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      DefineNodeTemplate();
      myDiagram.NodeTemplate = sharedNodeTemplate;

      myDiagram.UndoManager.IsEnabled = true;

      // define the template for nodes, just some text inside a colored rectangle

      myDiagram.Model = new Model() {
        NodeDataSource = new List<NodeData> {
          new NodeData { Pos = "-30 0", Size = "50 300" , Color = Brush.RandomColor() },
          new NodeData { Pos = "120 20", Size = "300 50", Color = Brush.RandomColor() },
          new NodeData { Pos = "100 200", Size = "300 50", Color = Brush.RandomColor() },
          new NodeData { Pos = "500 50", Size = "50 300", Color = Brush.RandomColor() },
          new NodeData { Key = 1, Pos = "100 100", Size = "50 50", Color = "gray" },
          new NodeData { Key = 2, Pos = "200 140", Size = "50 50", Color = "gray" }
        }
      };

      myDiagram.FindNodeForKey(1).IsSelected = true;
    }

    private void SetupPalette() {
      // initialize the palette that is on the left side of the page
      myPalette = paletteControl1.Diagram as Palette;
      DefineNodeTemplate();
      myPalette.NodeTemplate = sharedNodeTemplate; // share the templates used by MyDiagram
      myPalette.Model = new Model() { // specify the contents of the Palette
        NodeDataSource = new List<NodeData> {
          new NodeData { Size = "50 50", Color = Brush.RandomColor() },
          new NodeData { Size = "60 40", Color = Brush.RandomColor() }
        }
      };
    }

  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }

  public class NodeData : Model.NodeData {
    public string Pos { get; set; }
    public string Size { get; set; }
    public string Color { get; set; }
  }

  public class LinkData : Model.LinkData { }

}
