/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace Demo.Samples.ScrollModes {
  [ToolboxItem(false)]
  public partial class ScrollModesControl : DemoControl {
    private Diagram myDiagram;

    public ScrollModesControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"
        <p>
      This demonstrates scrolling and scaling options available in <b>GoDiagram</b>.
        </p>
";

      checkBxScroll.CheckedChanged += (e, obj) => InfScroll();
      checkBxPosComp.CheckedChanged += (e, obj) => PosComp();
      checkBxScaleComp.CheckedChanged += (e, obj) => ScaleComp();

    }

    private bool infscrollCheckbox = false;
    private bool poscompCheckbox = false;
    private bool scalecompCheckbox = false;

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.MinScale = 0.25; // so the contents and grid cannot appear too small
      myDiagram.Grid =
        new Panel(PanelType.Grid).Add(
          new Shape {
            Figure = "LineH",
            Stroke = "gray",
            StrokeWidth = 0.5
          },
          new Shape {
            Figure = "LineH",
            Stroke = "darkslategray",
            StrokeWidth = 1.5,
            Interval = 10
          },
          new Shape {
            Figure = "LineV",
            Stroke = "gray",
            StrokeWidth = 0.5
          },
          new Shape {
            Figure = "LineV",
            Stroke = "darkslategray",
            StrokeWidth = 1.5,
            Interval = 10
          }
        );
      myDiagram.ToolManager.DraggingTool.IsGridSnapEnabled = true;
      myDiagram.UndoManager.IsEnabled = true; // enaable undo and redo

      myDiagram.NodeTemplate =
        new Node(PanelType.Auto).Add(  // the Shape will go around the TextBlock
          new Shape {
            Figure = "RoundedRectangle",
            StrokeWidth = 0
          }.Bind(
            // Shape.Fill is bound to Node.Data.Color
            new Binding("Fill", "Color")
          ),
          new TextBlock {
            Margin = 8
          }.Bind(  // some room around the text
                   // TextBlock.Text is bound to Node.Data.Key
            new Binding("Text", "Key"))
        );

      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "Alpha", Color = "lightblue" },
          new NodeData { Key = "Beta", Color = "orange" },
          new NodeData { Key = "Gamma", Color = "lightgreen" },
          new NodeData { Key = "Delta", Color = "pink" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = "Alpha", To = "Beta" },
          new LinkData { From = "Alpha", To = "Gamma" },
          new LinkData { From = "Beta", To = "Beta" },
          new LinkData { From = "Gamma", To = "Delta" },
          new LinkData { From = "Delta", To = "Alpha" }
        }
      };
    }

    private Point PositionFunc(Diagram diagram, Point pos) {
      var size = diagram.Grid.GridCellSize;
      return new Point(
        Math.Round(pos.X / size.Width) * size.Width,
        Math.Round(pos.Y / size.Height) * size.Height);
    }

    private double ScaleFunc(Diagram diagram, double scale) {
      var oldscale = diagram.Scale;
      if (scale > oldscale) {
        return oldscale + 0.25;
      } else if (scale < oldscale) {
        return oldscale - 0.25;
      }
      return oldscale;
    }

    private void InfScroll() {
      infscrollCheckbox = !infscrollCheckbox;
      myDiagram.StartTransaction("change scroll mode");
      myDiagram.ScrollMode = infscrollCheckbox ? ScrollMode.Infinite : ScrollMode.Document;
      myDiagram.CommitTransaction("change scroll mode");
    }

    private void PosComp() {
      poscompCheckbox = !poscompCheckbox;
      myDiagram.StartTransaction("change position computation");
      myDiagram.PositionComputation = poscompCheckbox ? PositionFunc : (Func<Diagram, Point, Point>)null;
      myDiagram.CommitTransaction("change position computation");
    }

    private void ScaleComp() {
      scalecompCheckbox = !scalecompCheckbox;
      myDiagram.StartTransaction("change scale computation");
      myDiagram.ScaleComputation = scalecompCheckbox ? ScaleFunc : (Func<Diagram, double, double>)null;
      myDiagram.CommitTransaction("change scale computation");
    }


  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Color { get; set; }
  }

  public class LinkData : Model.LinkData { }


}
