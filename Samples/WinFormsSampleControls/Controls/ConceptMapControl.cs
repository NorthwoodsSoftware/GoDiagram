using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;
using System.ComponentModel;

namespace WinFormsSampleControls.ConceptMap {
  [ToolboxItem(false)]
  public partial class ConceptMapControl : System.Windows.Forms.UserControl {
    private Diagram _Diagram;
    public ConceptMapControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;
      goWebBrowser1.Html = @"
        <p>
          A concept map sample depicting various suggested relationships between different ideas.
          See also the <a href=""InteractiveForce"">Interactive Force</a> sample that uses the exact same data
          but a different node template and an interactive <a>ForceDirectedLayout</a>.
        </p>
      ";
    }

    private void Setup() {
      _Diagram = diagramControl1.Diagram;

      // diagram properties
      _Diagram.InitialAutoScale = AutoScaleType.Uniform; // an initial automatic zoom-to-fit
      _Diagram.ContentAlignment = Spot.Center;  // align document to the center of the viewport
      _Diagram.Layout = new ForceDirectedLayout {
        MaxIterations = 200,
        DefaultSpringLength = 30,
        DefaultElectricalCharge = 100
      };

      // define each Node's appearance
      _Diagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance)  // the whole node panel
          { LocationSpot = Spot.Center }.Add(
          // define the node's outer shape, which will surround the TextBlock
          new Shape {
            Figure = "Rectangle",
            Fill = new Brush(new LinearGradientPaint(new Dictionary<float, string> {
                { 0, "rgb(254, 201, 0)"},
                { 1, "rgb(254, 162, 0)"}
              }
            )),
            Stroke = "black"
          },
          new TextBlock { Font = "Arial, 10px, style=bold", Margin = 4, Stroke = "#555555"}.Bind(
            new Binding("Text", "Text"))
        );

      // replace the default Link template in the linkTemplateMap
      _Diagram.LinkTemplate =
        new Link().Add(  // the whole link panel
          new Shape  // the link shape
            { Stroke = "black" },
          new Shape  // the arrowhead
            { ToArrow = "standard", Stroke = (Brush)null },
          new Panel(PanelLayoutAuto.Instance).Add(
            new Shape  // the label background, which becomes transparent around the edges
              {
              Fill = new Brush(new RadialGradientPaint(new Dictionary<float, string> {
                    { 0, "rgb(240, 240, 240)" },
                    { 0.3f, "rgb(240, 240, 240)" },
                    { 1, "rgba(240, 240, 240, 0)" }
                  }
              )),
              Stroke = (Brush)null
            },
            new TextBlock  // the label text
              {
              TextAlign = TextAlign.Center,
              Font = "Arial, 10px",
              Stroke = "#555555",
              Margin = 4
            }.Bind(
              new Binding("Text", "Text"))
          )
        );

      // model data
      _Diagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = 1, Text = "Concept Maps" },
          new NodeData { Key = 2, Text = "Organized Knowledge" },
          new NodeData { Key = 3, Text = "Context Dependent" },
          new NodeData { Key = 4, Text = "Concepts" },
          new NodeData { Key = 5, Text = "Propositions" },
          new NodeData { Key = 6, Text = "Associated Feelings or Affect" },
          new NodeData { Key = 7, Text = "Perceived Regularities" },
          new NodeData { Key = 8, Text = "Labeled" },
          new NodeData { Key = 9, Text = "Hierarchically Structured" },
          new NodeData { Key = 10, Text = "Effective Teaching" },
          new NodeData { Key = 11, Text = "Crosslinks" },
          new NodeData { Key = 12, Text = "Effective Learning" },
          new NodeData { Key = 13, Text = "Events (Happenings)" },
          new NodeData { Key = 14, Text = "Objects (Things)" },
          new NodeData { Key = 15, Text = "Symbols" },
          new NodeData { Key = 16, Text = "Words" },
          new NodeData { Key = 17, Text = "Creativity" },
          new NodeData { Key = 18, Text = "Interrelationships" },
          new NodeData { Key = 19, Text = "Infants" },
          new NodeData { Key = 20, Text = "Different Map Segments" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = 1, To = 2, Text = "represent" },
          new LinkData { From = 2, To = 3, Text = "is" },
          new LinkData { From = 2, To = 4, Text = "is" },
          new LinkData { From = 2, To = 5, Text = "is" },
          new LinkData { From = 2, To = 6, Text = "includes" },
          new LinkData { From = 2, To = 10, Text = "necessary\nfor" },
          new LinkData { From = 2, To = 12, Text = "necessary\nfor" },
          new LinkData { From = 4, To = 5, Text = "combine\nto form" },
          new LinkData { From = 4, To = 6, Text = "include" },
          new LinkData { From = 4, To = 7, Text = "are" },
          new LinkData { From = 4, To = 8, Text = "are" },
          new LinkData { From = 4, To = 9, Text = "are" },
          new LinkData { From = 5, To = 9, Text = "are" },
          new LinkData { From = 5, To = 11, Text = "may be" },
          new LinkData { From = 7, To = 13, Text = "in" },
          new LinkData { From = 7, To = 14, Text = "in" },
          new LinkData { From = 7, To = 19, Text = "begin\nwith" },
          new LinkData { From = 8, To = 15, Text = "with" },
          new LinkData { From = 8, To = 16, Text = "with" },
          new LinkData { From = 9, To = 17, Text = "aids" },
          new LinkData { From = 11, To = 18, Text = "show" },
          new LinkData { From = 12, To = 19, Text = "begins\nwith" },
          new LinkData { From = 17, To = 18, Text = "needed\nto see" },
          new LinkData { From = 18, To = 20, Text = "between" }
        }
      };
    }

    // define the model data
    public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
    public class NodeData : Model.NodeData { }
    public class LinkData : Model.LinkData { }


  }
}
