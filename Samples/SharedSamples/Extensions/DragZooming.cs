/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;
using Northwoods.Go.Tools.Extensions;

namespace Demo.Extensions.DragZooming {
  public partial class DragZooming : DemoControl {
    private Diagram _Diagram;
    private Part _LoadingPart;

    public DragZooming() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      desc1.MdText = DescriptionReader.Read("Extensions.DragZooming.md");
    }

    private void Setup() {
      _Diagram.InitialDocumentSpot = Spot.Center;
      _Diagram.InitialViewportSpot = Spot.Center;

      // define node template, just some text inside a colored rectangle
      _Diagram.NodeTemplate =
        new Node(PanelType.Spot) {
          Width = 70,
          Height = 20
        }.Add(
          new Shape {
            Figure = "Rectangle"
          }.Bind(
            new Binding("Fill", "Color")
          ),
          new TextBlock {
            Margin = 2
          }.Bind(
            new Binding("Text", "Color")
          )
        );

      // define the template for links, just a simple line
      _Diagram.LinkTemplate =
        new Link().Add(
          new Shape {
            Stroke = "black"
          }
        );

      // tree layout
      _Diagram.Layout =
        new TreeLayout {
          Angle = 90,
          NodeSpacing = 4,
          Compaction = TreeCompaction.None
        };

      // Add an instance of the custom tool defined in DragZoomingTool.cs
      // This needs to be inserted before the standard DragSelectingTool,
      // which is normally the third tool in the ToolManager.MouseMoveTools list
      _Diagram.ToolManager.MouseMoveTools.Insert(2, new DragZoomingTool());

      // Status indicator
      _LoadingPart =
        new Part {
          Selectable = false,
          Location = new Point(0, 0)
        }.Add(
          new TextBlock {
            Text = "Loading...",
            Stroke = "red",
            Font = new Font("Microsoft Sans Serif", 20)
          }
        );

      // temporarily add the status indicator
      _Diagram.Add(_LoadingPart);

      // allow the indicator to be shown now
      // but allow objects added in LoadTree to also be considered part of the initial diagram
      _Diagram.DelayInitialization(LoadTree);
    }

    // make node data
    private void LoadTree(Diagram diagram) {
      // set diagram's model data
      var total = 99;
      var rand = new Random();
      var treedata = new List<NodeData>();
      for (var i = 1; i <= total; i++) {
        treedata.Add(new NodeData {
          Key = i.ToString(),
          Color = Brush.RandomColor(),
          Parent = i > 1 ? rand.Next(1, i / 2).ToString() : null
        }); ;
      }



      // give the Diagram's model all the data
      diagram.Model = new Model {
        NodeDataSource = treedata
      };

      // remove status indicator
      diagram.Remove(_LoadingPart);
    }
  }

  // define the model data
  public class Model : TreeModel<NodeData, string, object> { }

  public class NodeData : Model.NodeData {
    public string Color { get; set; }
  }
}
