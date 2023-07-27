/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;
using Northwoods.Go.Tools.Extensions;

namespace Demo.Extensions.RealtimeDragSelecting {
  public partial class RealtimeDragSelecting : DemoControl {
    private Diagram _Diagram;

    public RealtimeDragSelecting() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();
      desc1.MdText = DescriptionReader.Read("Extensions.RealtimeDragSelecting.md");
    }

    private void Setup() {
      _Diagram.InitialDocumentSpot = Spot.Center;
      _Diagram.InitialViewportSpot = Spot.Center;

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
      _Diagram.ToolManager.DragSelectingTool = dragSelectingTool;

      // node template
      _Diagram.NodeTemplate =
        new Node(PanelType.Spot) {
          Width = 70, Height = 25
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
      _Diagram.LinkTemplate =
        new Link().Add(
          new Shape {
            Stroke = "black"
          }
        );

      // layout
      _Diagram.Layout = new TreeLayout();

      // model
      _Diagram.Model = LoadTree();
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
