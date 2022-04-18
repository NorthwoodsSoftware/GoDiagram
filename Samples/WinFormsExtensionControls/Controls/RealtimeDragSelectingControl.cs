/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;
using System.ComponentModel;
using Northwoods.Go.Tools.Extensions;

namespace WinFormsExtensionControls.RealtimmeDragSelecting {
  [ToolboxItem(false)]
  public partial class RealtimeDragSelectingControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    public RealtimeDragSelectingControl() {
      InitializeComponent();

      Setup();
      goWebBrowser1.Html = @"
           <p>
          This sample demonstrates the RealtimeDragSelectingTool, which replaces the standard <a>DragSelectingTool</a>.
          Press in the background, wait briefly, and then drag to start selecting Nodes or Links that intersect with the box.
          You can press or release Control or Shift while dragging to see how the selection changes.
          </p>
          <p>
          Load it in your own app by including <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/RealtimeDragSelecting/RealtimeDragSelectingTool.cs"">RealtimeDragSelectingTool.cs</a>.
          Initialize your Diagram by setting <a>ToolManager.DragSelectingTool</a> to a new instance of this tool.
          For example:
          </p>
";
      goWebBrowser2.Html = @"
           <p> or </p>
";
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.InitialDocumentSpot = Spot.Center;
      myDiagram.InitialViewportSpot = Spot.Center;

      // replace the standard DragSelectingTool with one that selects while dragging
      // and also only requires overlapping bounds with the dragged box to be selected
      var dragSelectingTool = new RealtimeDragSelectingTool() {
        IsPartialInclusion = true, Delay = new TimeSpan(0, 0, 0, 0, 50),
        Box = new Part() {
          LayerName = "Tool",
          Selectable = false
        }.Add(
          new Shape {
            Name = "SHAPE",
            Fill = "rgba(255,0,0,0.1)",
            Stroke = "red",
            StrokeWidth = 2
          }
        )
      };
      myDiagram.ToolManager.DragSelectingTool = dragSelectingTool;

      // node template
      myDiagram.NodeTemplate =
        new Node(PanelLayoutSpot.Instance) {
          Width = 70, Height = 20
        }.Add(
          new Shape {
            Figure = "Rectangle"
          }.Bind(
            new Binding("Fill", "C")
          ),
          new TextBlock {
            Margin = 2
          }.Bind(
            new Binding("Text", "C")
          )
        );

      // link template
      myDiagram.LinkTemplate =
        new Link().Add(
          new Shape {
            Stroke = "black"
          }
        );

      // layout
      myDiagram.Layout = new TreeLayout();

      // model
      myDiagram.Model = LoadTree();
    }

    private Model LoadTree() {
      // create some tree data
      var total = 49;
      var treedata = new List<NodeData>();
      var rand = new Random();
      for (var i = 0; i < total; i++) {
        var d = new NodeData {
          Key = i, // key
          C = Brush.RandomColor(), // color
          // TEMPORARY until nullable parent type works
          Parent = (i > 0 ? (int)Math.Floor(rand.NextDouble() * i / 2) : int.MinValue)
          // END TEMPORARY
        };
        treedata.Add(d);
      }
      return new Model {
        NodeDataSource = treedata
      };
    }
  }

  public class Model : TreeModel<NodeData, int, object> { }
  public class NodeData : Model.NodeData {
    public string C { get; set; }

  }
}
