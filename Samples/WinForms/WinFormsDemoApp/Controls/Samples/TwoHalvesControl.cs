/* Copyright (c) Northwoods Software Corporation. */

using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Extensions;
using Northwoods.Go.Models;

namespace Demo.Samples.TwoHalves {
  [ToolboxItem(false)]
  public partial class TwoHalvesControl : DemoControl {
    private Diagram myDiagram;

    public TwoHalvesControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"
      <p>
    This defines a node template consisting of a top half and a bottom half.
    Each half's text and color are data bound.
    However the size of each node is fixed, so if the text is too long, it will be clipped.
      </p>
      <p>
    The ""RoundedTopRectangle"" and ""RoundedBottomRectangle"" figures are predefined.
    See also the <a href=""demo/RoundedGroups"">Rounded Groups</a> sample.
      </p>
";

    }

    private string myModelData =
@"{
  ""nodeDataSource"": [
    { ""key"":""Alpha"", ""topText"":""A"", ""topColor"":""Lightgray"", ""bottomText"":""B"", ""bottomColor"":""Green"" },
    { ""key"":""Beta"", ""topText"":""C"", ""bottomText"":""D"", ""bottomColor"":""Red"", ""star"": true }
  ],
  ""linkDataSource"": [
    { ""from"":""Alpha"", ""to"":""Beta"" }
  ]
}";

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // load additional figures
      Figures.DefineExtraFigures();

      // diagram properties
      myDiagram.InitialScale = 2.0;
      myDiagram.UndoManager.IsEnabled = true;

      var nodeInfoSize = new Size(50, 25); // the size of each half

      // node template
      myDiagram.NodeTemplate =
        new Node(PanelType.Spot) {
          SelectionAdorned = false,
          LocationSpot = Spot.Center
        }.Add(
          new Panel(PanelType.Auto).Add(
            new Shape {
              Figure = "RoundedRectangle",
              Stroke = "transparent",
              StrokeWidth = 3,
              Spot1 = Spot.TopLeft,
              Spot2 = Spot.BottomRight
            }.Bind(
              new Binding("Stroke", "IsSelected", (s, _) => { return (s as bool? ?? false) ? "dodgerblue" : "transparent"; }).OfElement()
            ),
            new Panel().Add(
              new Panel(PanelType.Auto) {
                DesiredSize = nodeInfoSize
              }.Add(
                new Shape {
                  Figure = "RoundedTopRectangle",
                  Fill = "white"
                }.Bind(
                  new Binding("Fill", "TopColor")
                ),
                new TextBlock().Bind(
                  new Binding("Text", "TopText")
                )
              ),
              new Panel(PanelType.Auto) {
                DesiredSize = nodeInfoSize,
                Position = new Point(0, nodeInfoSize.Height - 1)
              }.Add(  // overlap the top side of this shape with the bottom side of the top shape
                new Shape {
                  Figure = "RoundedBottomRectangle",
                  Fill = "white"
                }.Bind(
                  new Binding("Fill", "BottomColor")
                ),
                new TextBlock().Bind(
                  new Binding("Text", "BottomText")
                )
              )
            )
          ),
          // decorations...
          new Shape {
            Figure = "FivePointedStar",
            DesiredSize = new Size(12, 12),
            Fill = "yellow",
            Alignment = new Spot(1, 0, -2, 2),
            Opacity = 0.0
          }.Bind(
            new Binding("Opacity", "Star", (v, _) => {
              return (v as bool? ?? false) ? 1.0 : 0.0;
            })
          )
        );

      // Load the model data into textbox
      textBox1.Text = myModelData;
      myDiagram.Model = Model.FromJson<Model>(myModelData);
    }

   }
  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string TopText { get; set; }
    public string TopColor { get; set; }
    public string BottomText { get; set; }
    public string BottomColor { get; set; }
    public bool? Star { get; set; }
  }

  public class LinkData : Model.LinkData { }


}
