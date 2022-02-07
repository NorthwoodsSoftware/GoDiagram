using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.WinForms;
using Northwoods.Go.Extensions;
using Northwoods.Go.Layouts;
using Northwoods.Go.Tools;
using System.Linq;

namespace WinFormsSampleControls.Planogram {
  [ToolboxItem(false)]
  public partial class PlanogramControl : System.Windows.Forms.UserControl {

    private Diagram myDiagram;
    private Palette myPaletteSmall;
    private Palette myPaletteTall;
    private Palette myPaletteWide;
    private Palette myPaletteBig;
    public PlanogramControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;
      paletteControl1.AfterRender = SetupPaletteSmall;
      paletteControl2.AfterRender = SetupPaletteTall;
      paletteControl3.AfterRender = SetupPaletteWide;
      paletteControl4.AfterRender = SetupPaletteBig;
      goWebBrowser1.Html = @"
  <p>
  A <em>planogram</em> is a visual representation of a store's products or services, often used as a tool to maximize sales.
</p>
<p>
  Drag-and-drop ""items"" from the Palette onto ""racks"" in the Diagram.
  ""Items"" are implemented as Nodes; ""racks"" are implemented as Groups.
  Once items have been placed on a rack, they can be resized, if necessary.
  The <a>DraggingTool.IsGridSnapEnabled</a> and <a>ResizingTool.IsGridSnapEnabled</a> are both set
  to true to allow for snapping to the background grid.
  Node and Group templates both use dragging functions to allow for highlighting so the user knows which
  rack an item will belong to.
</p>
";
      goWebBrowser2.Html = @"
  <p>
  See also Northwoods Software's planogramming services: <a href=""https://goplanogram.com"">GoPlanogram</a>.
</p>
";
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      myDiagram.ModelChanged += (s, e) => {
        if (e.IsTransactionFinished) textBox1.Text = myDiagram.Model.ToJson();
        
      };

      myDiagram.Grid =
        new Panel("Grid") { GridCellSize = CellSize }
          .Add(
            new Shape { Figure = "LineH", Stroke = "lightgray" },
            new Shape { Figure = "LineV", Stroke = "lightgray" }
          );
      // support grid snapping when dragging and when resizing
      myDiagram.ToolManager.DraggingTool.IsGridSnapEnabled = true;
      myDiagram.ToolManager.DraggingTool.GridSnapCellSpot = Spot.Center;
      myDiagram.ToolManager.ResizingTool.IsGridSnapEnabled = true;
      // for this sample, automatically show the state of the diagram's model on the page
      myDiagram.ModelChanged += (s, e) => {
        if (e.IsTransactionFinished) {
          mySavedModel = myDiagram.Model.ToJson();
        }
      };
      //myDiagram.AnimationManager.IsEnabled = false;
      myDiagram.UndoManager.IsEnabled = true;

      myDiagram.NodeTemplate = sharedNodeTemplate;

      var groupFill = "rgba(128,128,128,0.2)";
      var groupStroke = "gray";
      var dropFill = "rgba(128,255,255,0.2)";
      var dropStroke = "red";

      myDiagram.GroupTemplate =
        new Group {
          LayerName = "Background",
          Resizable = true,
          ResizeElementName = "SHAPE",
          // because the gridSnapCellSpot is Center, offset the Group's location
          LocationSpot = new Spot(0, 0, CellSize.Width / 2, CellSize.Height / 2),
          MouseDragEnter = (e, grp, prev) => {
            if (!_HighlightGroup(grp as Group, true)) {
              e.Diagram.CurrentCursor = "not-allowed";
            } else {
              e.Diagram.CurrentCursor = "";
            }
          },
          MouseDragLeave = (e, grp, next) => {
            _HighlightGroup(grp as Group, false);
          },
          MouseDrop = (e, grp) => {
            var ok = (grp as Group).AddMembers(grp.Diagram.Selection, true);
            if (!ok) grp.Diagram.CurrentTool.DoCancel();
          }
        }
          // always save/load the point that is the top-left corner of the node, not the location
          .Bind("Position", "Pos", Point.Parse, Point.Stringify)
          .Add(
            new Shape {
              Figure = "Rectangle",
              Name = "SHAPE",
              Fill = groupFill,
              Stroke = groupStroke,
              MinSize = new Size(CellSize.Width * 2, CellSize.Height * 2)
            }
              .Bind(
                new Binding("DesiredSize", "Size", Northwoods.Go.Size.Parse, Northwoods.Go.Size.Stringify),
                new Binding("Fill", "IsHighlighted", (h, _) => (bool)h ? dropFill : groupFill).OfElement(),
                new Binding("Stroke", "IsHighlighted", (h, _) => (bool)h ? dropStroke : groupStroke).OfElement()
              )
          );

      // decide what kinds of Parts can be added to a Group
      myDiagram.CommandHandler.MemberValidation = (grp, node) => {
        if (grp is Group && node is Group) return false; // cannot add groups to groups
        // but dropping a group onto bkg is always ok
        return true;
      };

      // what to do when a drag-drop occurs in the Diagram's background
      myDiagram.MouseDragOver = (e) => {
        if (!AllowTopLevel) {
          // OK to drop a group anywhere or any node that is a member of a dragged Group
          var tool = e.Diagram.ToolManager.DraggingTool;
          if (!tool.DraggingParts.All(p => {
            return p is Group || (!p.IsTopLevel && tool.DraggingParts.Contains(p.ContainingGroup));
          })) {
            e.Diagram.CurrentCursor = "not-allowed";
          } else {
            e.Diagram.CurrentCursor = "";
          }
        }
      };

      myDiagram.MouseDrop = (e) => {
        if (AllowTopLevel) {
          // when the selection is dropped in the diagram's background,
          // make sure the selected Parts no longer belong to any Group
          if (!e.Diagram.CommandHandler.AddTopLevelParts(e.Diagram.Selection, true)) {
            e.Diagram.CurrentTool.DoCancel();
          }
        } else {
          // disallow dropping any regular nodes onto the background, but allow dropping "racks",
          // including any selected member nodes
          if (!e.Diagram.Selection.All(p => p is Group || (!p.IsTopLevel && p.ContainingGroup.IsSelected))) {
            e.Diagram.CurrentTool.DoCancel();
          }
        }
      };

      // start off with four "racks" that are positioned next to each other
      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "G1", IsGroup = true, Pos = "0 0", Size = "200 200" },
          new NodeData { Key = "G2", IsGroup = true, Pos = "200 0", Size = "200 200" },
          new NodeData { Key = "G3", IsGroup = true, Pos = "0 200", Size = "200 200" },
          new NodeData { Key = "G4", IsGroup = true, Pos = "200 200", Size = "200 200" }
        }
      };
    }

    private static readonly string _Green = "#B2FF59";
    private static readonly string _Blue = "#81D4FA";
    private static readonly string _Yellow = "#FFEB3B";

    private void SetupPaletteSmall() {
      myPaletteSmall = paletteControl1.Diagram as Palette;
      myPaletteSmall.NodeTemplate = sharedNodeTemplate;
      myPaletteSmall.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "g", Color = _Green },
          new NodeData { Key = "b", Color = _Blue },
          new NodeData { Key = "y", Color = _Yellow }
        }
      };
    }

    private void SetupPaletteTall() {
      myPaletteTall = paletteControl2.Diagram as Palette;
      myPaletteTall.NodeTemplate = sharedNodeTemplate;
      myPaletteTall.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "g", Color = _Green, Size = "50 100" },
          new NodeData { Key = "b", Color = _Blue, Size = "50 100" },
          new NodeData { Key = "y", Color = _Yellow, Size = "50 100" }
        }
      };
    }

    private void SetupPaletteWide() {
      myPaletteWide = paletteControl3.Diagram as Palette;
      myPaletteWide.NodeTemplate = sharedNodeTemplate;
      myPaletteWide.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "g", Color = _Green, Size = "100 50" },
          new NodeData { Key = "b", Color = _Blue, Size = "100 50" },
          new NodeData { Key = "y", Color = _Yellow, Size = "100 50" }
        }
      };
    }

    private void SetupPaletteBig() {
      myPaletteBig = paletteControl4.Diagram as Palette;
      myPaletteBig.NodeTemplate = sharedNodeTemplate;
      myPaletteBig.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "g", Color = _Green, Size = "100 100" },
          new NodeData { Key = "b", Color = _Blue, Size = "100 100" },
          new NodeData { Key = "y", Color = _Yellow, Size = "100 100" }
        }
      };
    }

    // Regular nodes represent items to be put onto racks.
    // Nodes are currently resizable, but if that is not desired just set resizable to false.
    private Part sharedNodeTemplate =
      new Node("Auto") {
        Resizable = true,
        ResizeElementName = "SHAPE",
        // because the gridSnapCellSpot is Center, Offset the node's location
        LocationSpot = new Spot(0, 0, CellSize.Width / 2, CellSize.Height / 2),
        // provide a visual warning about dropping anything onto an "item"
        MouseDragEnter = (e, node, last) => {
          e.Handled = true;
          ((node as Node).FindElement("SHAPE") as Shape).Fill = "red";
          e.Diagram.CurrentCursor = "not-allowed";
          _HighlightGroup((node as Node).ContainingGroup, false);
        },
        MouseDragLeave = (e, node, next) => {
          (node as Node).UpdateTargetBindings();
        },
        MouseDrop = (e, node) => {  // disallow dropping anything on an "item"
          node.Diagram.CurrentTool.DoCancel();
        } 
      }
        .Bind("Position", "Pos", Point.Parse, Point.Stringify)
        .Add(
           // this is the primary thing people see
           new Shape {
             Figure = "Rectangle",
             Name = "SHAPE",
             Fill = "white",
             MinSize = CellSize,
             DesiredSize = CellSize  // initially 1x1 cell
           }
             .Bind("Fill", "Color")
             .Bind("DesiredSize", "Size", Northwoods.Go.Size.Parse, Northwoods.Go.Size.Stringify),
           // with the textual key in the middle
           new TextBlock {
             Alignment = Spot.Center,
             Font = new Font("Segoe UI", 16, FontWeight.Bold),
             Editable = true
           }
             .Bind("Text", "Key")
      ); // end Node

    public static bool AllowTopLevel = false;
    public static Size CellSize = new Size(50, 50);

    public string mySavedModel { get; set; }

    // Groups represent racks where items (Nodes) can be placed.
    // Currently they are movable and resizable, but you can change that
    // if you want the racks to remain "fixed".
    // Groups provide feedback when the user drags nodes onto them.

    private static bool _HighlightGroup(Group grp, bool show) {
      if (grp == null) return false;
      // check that the drop may really happen into the Group
      var tool = grp.Diagram.ToolManager.DraggingTool;
      grp.IsHighlighted = show && grp.CanAddMembers(tool.DraggingParts);
      return grp.IsHighlighted;
    }

    public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }

    public class NodeData : Model.NodeData {
      public string Pos { get; set; }
      public string Size { get; set; }
      public Brush Color { get; set; }
    }

    public class LinkData : Model.LinkData { }

    
  }
}
