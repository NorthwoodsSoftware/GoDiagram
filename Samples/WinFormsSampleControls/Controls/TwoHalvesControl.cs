using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Extensions;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.TwoHalves {
  [ToolboxItem(false)]
  public partial class TwoHalvesControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    
    public TwoHalvesControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      goWebBrowser1.Html = @"
      <p>
    This defines a node template consisting of a top half and a bottom half.
    Each half's text and color are data bound.
    However the size of each node is fixed, so if the text is too long, it will be clipped.
      </p>
      <p>
    The ""RoundedTopRectangle"" and ""RoundedBottomRectangle"" figures are defined in
    <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/GraphObjects/Figures/Figures.cs"">Figures.cs</a> in the extensions directory.
    See also the <a href=""RoundedGroups"">Rounded Groups</a> sample.
      </p>
";

    }

    private string myModelData =
@"{
  ""NodeDataSource"": [
    { ""Key"":""Alpha"", ""TopText"":""A"", ""TopColor"":""Lightgray"", ""BottomText"":""B"", ""BottomColor"":""Green"" },
    { ""Key"":""Beta"", ""TopText"":""C"", ""BottomText"":""D"", ""BottomColor"":""Red"", ""Star"": true }
  ],
  ""LinkDataSource"": [
    { ""From"":""Alpha"", ""To"":""Beta"" }
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
        new Node(PanelLayoutSpot.Instance) {
          SelectionAdorned = false,
          LocationSpot = Spot.Center
        }.Add(
          new Panel(PanelLayoutAuto.Instance).Add(
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
              new Panel(PanelLayoutAuto.Instance) {
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
              new Panel(PanelLayoutAuto.Instance) {
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
