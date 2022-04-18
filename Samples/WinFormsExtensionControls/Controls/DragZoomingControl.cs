/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;
using Northwoods.Go.Tools.Extensions;

namespace WinFormsExtensionControls.DragZooming {
  [ToolboxItem(false)]
  public partial class DragZoomingControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public DragZoomingControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"
  <p>
  This sample demonstrates the DragZoomingTool, which replaces the standard DragSelectingTool. It is defined in its own file,
  as <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/DragZooming/DragZoomingTool.cs"">DragZoomingTool.cs</a>.
  </p>
  <p>
  Press in the background, wait briefly, and then drag to zoom in to show the area of the drawn rectangle.
  Hold down the Shift key to zoom out.
  The rectangle always has the same aspect ratio as the viewport of the diagram.
  </p>
";

    }

    private Part myLoading;

    private void Setup() {

      myDiagram = diagramControl1.Diagram;

      myDiagram.InitialDocumentSpot = Spot.Center;
      myDiagram.InitialViewportSpot = Spot.Center;

      // define node template, just some text inside a colored rectangle
      myDiagram.NodeTemplate =
        new Node(PanelLayoutSpot.Instance) {
          Width = 70,
          Height = 20
        }.Add(
          new Shape {
            Figure = "Rectangle",
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
      myDiagram.LinkTemplate =
        new Link().Add(
          new Shape {
            Stroke = "black"
          }
        );

      // tree layout
      myDiagram.Layout =
        new TreeLayout {
          Angle = 90,
          NodeSpacing = 4,
          Compaction = TreeCompaction.None
        };

      // Add an instance of the custom tool defined in DragZoomingTool.cs
      // This needs to be inserted before the standard DragSelectingTool,
      // which is normally the third tool in the ToolManager.MouseMoveTools list
      myDiagram.ToolManager.MouseMoveTools.Insert(2, new DragZoomingTool());

      // Status indicator
      myLoading =
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
      myDiagram.Add(myLoading);

      // allow the indicator to be shown now
      // but allow objects added in LoadTree to also be considered part of the initial diagram
      myDiagram.DelayInitialization(LoadTree);
    }

    // make node data
    private void LoadTree(Diagram diag) {
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
      diag.Model = new Model {
        NodeDataSource = treedata,
      };

      // remove status indicator
      diag.Remove(myLoading);
    }

  }

  // define the model data
  public class Model : TreeModel<NodeData, string, object> { }

  public class NodeData : Model.NodeData {
    public string Color { get; set; }
  }

}
