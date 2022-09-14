/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.Collections.Generic;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Extensions;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace Demo.Samples.Shapes {
  public partial class Shapes : DemoControl {
    private Diagram _Diagram;

    public Shapes() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      desc1.MdText = DescriptionReader.Read("Samples.Shapes.md");
    }

    private void Setup() {
      // load extra figures
      Figures.DefineExtraFigures();

      // use a gridlayout
      _Diagram.Layout = new GridLayout {
        Sorting = GridSorting.Forwards
      };
      // to see the names of shapes on the bottom row
      _Diagram.Padding = new Margin(5, 5, 25, 5);

      void MouseEnter(InputEvent e, GraphObject obj, GraphObject _) {
        var node = obj as Node;
        node.IsHighlighted = true;
      }

      void MouseLeave(InputEvent e, GraphObject obj, GraphObject _) {
        var node = obj as Node;
        node.IsHighlighted = false;
      }

      // Names of the built in shapes, which we will color green instead of pink.
      // The pinks shapes are instead defined in the extensions "Figures.cs" file.
      var builtIn = new HashSet<string>() {
      "Rectangle", "Square", "RoundedRectangle", "Border", "Ellipse", "Circle", "TriangleRight", "TriangleDown", "TriangleLeft", "TriangleUp", "Triangle", "Diamond", "LineH", "LineV", "None", "BarH", "BarV", "MinusLine", "PlusLine", "XLine"
      };
      bool IsBuiltIn(string shapeName) {
        return builtIn.Contains(shapeName);
      }

      // node template
      _Diagram.NodeTemplate =
        new Node(PanelType.Vertical) {
          MouseEnter = MouseEnter,
          MouseLeave = MouseLeave,
          LocationSpot = Spot.Center,  // the location is the center of the Shape
          LocationElementName = "SHAPE",
          SelectionAdorned = false,  // no selection handle when selected
          Resizable = true,
          ResizeElementName = "SHAPE",  // user can resize the Shape
          Rotatable = true,
          RotateElementName = "SHAPE",  // rotate the Shape without rotating the label
          // don't re-layout when node changes size
          LayoutConditions = LayoutConditions.Standard & ~LayoutConditions.NodeSized
        }.Bind(
          new Binding("LayerName", "IsHighlighted", (h, _) => { return (h as bool? ?? false) ? "Foreground" : ""; }).OfElement()
        ).Add(
          new Shape {
            Name = "SHAPE",  // named so that the above properties can refer to this GraphObject
            Width = 70,
            Height = 70,
            StrokeWidth = 3
          }.Bind(
            // Color the built in shapes green, and the Figures.cs shapes Pink
            new Binding("Fill", "Key", (k, _) => {
              return IsBuiltIn(k as string) ? "palegreen" : "lightpink";
            }),
            new Binding("Stroke", "Key", (k, _) => {
              return IsBuiltIn(k as string) ? "darkgreen" : "#C2185B";
            }),
            // bind the Shape.Figure to the figure name, which automatically gives the Shape a Geometry
            new Binding("Figure", "Key")
          ),
          new TextBlock  // the label
            {
            Margin = 4,
            Font = new Font("Segoe UI", 18, Northwoods.Go.FontWeight.Bold),
            Background = "white"
          }.Bind(
            new Binding("Visible", "IsHighlighted").OfElement(),
            new Binding("Text", "Key")
          )
        );

      // initialize the model
      _Diagram.Model = new Model {
        NodeDataSource = Shape.GetFigureGenerators().Keys.ToList().ConvertAll((str) => {
          var key = "" + char.ToUpper(str[0]) + str.Substring(1);
          return new NodeData {
            Key = key
          };
        })
      };
    }
  }

  // define the model data
  public class Model : Model<NodeData, string, object> { }
  public class NodeData : Model.NodeData { }
}
