/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace Demo.Extensions.Robot {
  public partial class Robot : DemoControl {
    private Diagram _Diagram;
    private Palette _Palette;
    // a shared Robot that can be used by all commands for this one Diagram
    private Northwoods.Go.Extensions.Robot _Robot;

    public Robot() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;
      _Palette = paletteControl1.Diagram as Palette;
      _Robot = new Northwoods.Go.Extensions.Robot(_Diagram);

      dragFromPaletteBtn.Click += _DragFromPaletteBtn_Click;
      copyNodeBtn.Click += _CopyNodeBtn_Click;
      dragSelectBtn.Click += _DragSelectBtn_Click;
      contextMenuBtn.Click += _ContextMenuBtn_Click;
      deleteBtn.Click += _DeleteBtn_Click;
      clickLambdaBtn.Click += _ClickLambdaBtn_Click;
      doubleClickLambdaBtn.Click += _DoubleClickLambdaBtn_Click;
      panBtn.Click += _PanBtn_Click;

      Setup();

      desc1.MdText = DescriptionReader.Read("Extensions.Robot.md");
    }

    private void Setup() {
      _Diagram.UndoManager.IsEnabled = true;

      void showProperties(InputEvent e, GraphObject obj) {  // executed by ContextMenuButton
        var node = ((Adornment)obj.Part).AdornedPart as Node;
        var msg = "Context clicked: " + (node.Data as NodeData).Key + ". ";
        msg += "Selection includes:";
        foreach (var part in _Diagram.Selection) {
          msg += " " + part.ToString();
        }
        outputTb.Text = msg;
      }

      void nodeClicked(InputEvent e, GraphObject obj) {  // executed by Click and DoubleClick handlers
        var node = obj.Part as Node;
        var type = e.ClickCount == 2 ? "Double-Clicked: " : "Clicked: ";
        var msg = type + (node.Data as NodeData).Key;
        outputTb.Text = msg;
      }

      _Diagram.NodeTemplate =
        new Node("Auto") {
            Click = nodeClicked,
            DoubleClick = nodeClicked,
            ContextMenu =
                  Builder.Make<Adornment>("ContextMenu")
                    .Add(
                      Builder.Make<Panel>("ContextMenuButton")
                        .Add(new TextBlock("Properties"))
                        .Apply(btn => btn.Click = showProperties)
                    )
          }
          .Add(
            new Shape("Rectangle") {
                Fill = "lightgray",
                PortId = "", FromLinkable = true, ToLinkable = true, Cursor = "pointer"
              },
            new TextBlock { Margin = 3 }
              .Bind("Text", "Key")
          );

      _Diagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "Lambda" },
          new NodeData { Key = "Mu" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = "Lambda", To = "Mu" }
        }
      };

      // initialize the Palette that is on the left side of the page
      _Palette.NodeTemplate = _Diagram.NodeTemplate;
      _Palette.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "Alpha" },
          new NodeData { Key = "Beta" },
          new NodeData { Key = "Gamma" },
          new NodeData { Key = "Delta" }
        }
      };
    }

    private void _DragFromPaletteBtn_Click(object sender, EventArgs e) {
      // simulate a drag-and-drop between Diagrams:
      var dragdropFn = (InputEvent e) => { e.TargetDiagram = _Diagram; };
      _Robot.MouseDown(5, 5, 0, dragdropFn, _Palette);  // this should be where the Alpha node is in the source _Palette
      _Robot.MouseMove(60, 60, 100, dragdropFn, _Palette);
      _Robot.MouseUp(100, 100, 200, dragdropFn, _Palette);  // this is where the node will be dropped in the target _Diagram
      // If successful in dragging a node from the Palette into the Diagram,
      // the DraggingTool will perform a transaction.
    }

    private void _CopyNodeBtn_Click(object sender, EventArgs e) {
      var alpha = _Diagram.FindNodeForKey("Alpha");
      if (alpha == null) return;
      var loc = alpha.ActualBounds.Center;

      var optionsFn = (InputEvent e) => { e.Control = true; };
      // Simulate a mouse drag to move the Alpha node:
      _Robot.MouseDown(loc.X, loc.Y, 0, optionsFn);
      _Robot.MouseMove(loc.X + 80, loc.Y + 50, 50, optionsFn);
      _Robot.MouseMove(loc.X + 20, loc.Y + 100, 100, optionsFn);
      _Robot.MouseUp(loc.X + 20, loc.Y + 100, 150, optionsFn);
      // If successful, will have made a copy of the "Alpha" node below it.

      // Alternatively you could copy the Node using commands:
      // _Diagram.CommandHandler.CopySelection();
      // _Diagram.CommandHandler.PasteSelection(new Point(loc.X + 20, loc.Y + 100));
    }

    private void _DragSelectBtn_Click(object sender, EventArgs e) {
      var alpha = _Diagram.FindNodeForKey("Alpha");
      if (alpha == null) return;
      var alpha2 = _Diagram.FindNodeForKey("Alpha2");
      if (alpha2 == null) return;
      var coll = new HashSet<Part> {
        alpha,
        alpha2
      };
      var area = _Diagram.ComputePartsBounds(coll);
      area = area.Inflate(30, 30);

      // Simulate dragging in the background around the two Alpha nodes.
      // This uses timestamps to pretend to wait a while to avoid activating the PanningTool.
      // Hopefully this mouse down does not hit any Part, but in the Diagram's background:
      _Robot.MouseDown(area.X, area.Y, 0);
      // NOTE that this MouseMove timestamp needs to be > _Diagram.ToolManager.DragSelectingTool.Delay:
      _Robot.MouseMove(area.CenterX, area.CenterY, 200);
      _Robot.MouseUp(area.Right, area.Bottom, 250);
      // Now should have selected both "Alpha" and "Alpha2" using the DragSelectingTool.

      // Alternatively you could select the Nodes programmatically:
      // alpha.IsSelected = true;
      // alpha2.IsSelected = true;
    }

    private void _ContextMenuBtn_Click(object sender, EventArgs e) {
      var alpha = _Diagram.FindNodeForKey("Alpha");
      if (alpha == null) return;
      var loc = alpha.Location;

      // right click on Alpha
      var optionsFn = (InputEvent e) => { e.Right = true; };
      _Robot.MouseDown(loc.X + 10, loc.Y + 10, 0, optionsFn);
      _Robot.MouseUp(loc.X + 10, loc.Y + 10, 100, optionsFn);

      // Alternatively you could invoke the Show Context Menu command directly:
      // _Diagram.CommandHandler.ShowContextMenu(alpha);

      // move mouse over first context menu button
      _Robot.MouseMove(loc.X + 20, loc.Y + 20, 200);
      // and click that button
      _Robot.MouseDown(loc.X + 20, loc.Y + 20, 300);
      _Robot.MouseUp(loc.X + 20, loc.Y + 20, 350);
      // This should have invoked the ContextMenuButton's click function, showProperties,
      // which should have put a green message in outputLabel.
    }

    private void _DeleteBtn_Click(object sender, EventArgs e) {
      // Simulate clicking the "Del" key:
      _Robot.KeyDown("DELETE");
      _Robot.KeyUp("DELETE");
      // Now the selected Nodes are deleted.

      // Alternatively you could invoke the Delete command directly:
      // _Diagram.CommandHandler.DeleteSelection();
    }

    private void _ClickLambdaBtn_Click(object sender, EventArgs e) {
      var lambda = _Diagram.FindNodeForKey("Lambda");
      if (lambda == null) return;
      var loc = lambda.Location;

      // click on Lambda
      _Robot.MouseDown(loc.X + 10, loc.Y + 10, 0);
      _Robot.MouseUp(loc.X + 10, loc.Y + 10, 100);

      // Clicking is just a sequence of input events.
      // There is no command in CommandHandler for such a basic gesture.
    }

    private void _DoubleClickLambdaBtn_Click(object sender, EventArgs e) {
      var lambda = _Diagram.FindNodeForKey("Lambda");
      if (lambda == null) return;
      var loc = lambda.Location;

      // double-click on Lambda
      var optionsFn = (InputEvent e) => { e.ClickCount = 2; };
      _Robot.MouseDown(loc.X + 10, loc.Y + 10, 0);
      _Robot.MouseUp(loc.X + 10, loc.Y + 10, 100);
      _Robot.MouseDown(loc.X + 10, loc.Y + 10, 200, optionsFn);
      _Robot.MouseUp(loc.X + 10, loc.Y + 10, 300, optionsFn);
    }

    private void _PanBtn_Click(object sender, EventArgs e) {
      var pos1 = _Diagram.Position;

      var pt = new Point(_Diagram.ViewportBounds.X + 30, _Diagram.ViewportBounds.CenterY);
      _Robot.MouseDown(pt.X, pt.Y, 0);
      // Minimal wait after MouseDown when moving, else the PanningTool will be pre-empted
      // by the DragSelectingTool, which is controlled by the DragSelectingTool.Delay property.
      // Remember that these are document coordinates, which are shifted by the panning motion.
      _Robot.MouseMove(pt.X + 20, pt.Y + 10, 10);
      _Robot.MouseMove(pt.X + 20, pt.Y + 10, 30);
      _Robot.MouseMove(pt.X + 20, pt.Y + 10, 50);
      _Robot.MouseUp(pt.X + 20, pt.Y + 10, 70);

      var pos2 = _Diagram.Position;
      outputTb.Text = $"Diagram.Position before: {pos1}, Diagram.Position after: {pos2}";
    }
  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData { }
  public class LinkData : Model.LinkData { }
}
