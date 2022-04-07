using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;

namespace WinFormsSampleControls.GLayout {
  [ToolboxItem(false)]
  public partial class GLayoutControl : System.Windows.Forms.UserControl {

    private Diagram myDiagram;

    public GLayoutControl() {
      InitializeComponent();
      myDiagram = diagramControl1.Diagram;

      columnBox.Leave += (s, e) => _Layout();
      widthBox.Leave += (s, e) => _Layout();
      cellSizeBox.Leave += (s, e) => _Layout();
      spacingBox.Leave += (s, e) => _Layout();

      positionBtn.CheckedChanged += (s, e) => _Layout();
      locationBtn.CheckedChanged += (s, e) => _Layout();
      leftToRightBtn.CheckedChanged += (s, e) => _Layout();
      rightToLeftBtn.CheckedChanged += (s, e) => _Layout();

      sortingCBox.DataSource = Enum.GetNames(typeof(GridSorting));
      sortingCBox.SelectedIndexChanged += (s, e) => _Layout();

      Setup();

      goWebBrowser1.Html = @"
        <p>
        For information on <b>GridLayout</b> and its properties, see the <a>GridLayout</a> documentation page.
        </p>";
    }

    private void Setup() {
      var rand = new Random();

      myDiagram.Layout = new GridLayout() {
        Comparer = GridLayout.SmartComparer
      };

      myDiagram.NodeTemplate =
        new Node("Spot") {
            // make sure the Node.location is different from the Node.position
            LocationSpot = Spot.Center
          }
          .Bind("Text")  // for sorting
          .Add(
            new Shape {
                Figure = "Ellipse",
                Fill = "lightgray",
                Stroke = null,
                DesiredSize = new Size(30, 30)
              }
              .Bind("Fill")
              .Bind("DesiredSize", "Size"),
            new TextBlock().Bind("Text")  // the default alignment is Spot.Center
          );

      // create a list of data describing randomly colored and sized nodes
      var nodeDataSource = new List<NodeData>();
      for (var i = 1; i <= 100; i++) {
        nodeDataSource.Add(new NodeData {
          Key = i,
          Text = i.ToString(),
          Fill = Brush.RandomColor(),
          Size = new Size(rand.Next(30, 80), rand.Next(30, 80))
        });
      }

      // randomize the data
      for (var i = 0; i < 100; i++) {
        var swap = rand.Next(nodeDataSource.Count);
        var temp = nodeDataSource[swap];
        nodeDataSource[swap] = nodeDataSource[i];
        nodeDataSource[i] = temp;
      }

      // create a Model that does not know about Link or Group relationships
      myDiagram.Model = new Model {
        NodeDataSource = nodeDataSource
      };

      _Layout();
    }

    private void _Layout() {
      myDiagram = diagramControl1.Diagram;

      myDiagram.StartTransaction("change layout");
      var lay = myDiagram.Layout as GridLayout;

      if (!int.TryParse(columnBox.Text.Trim(), out var c)) c = 0;
      lay.WrappingColumn = c;
      if (double.TryParse(widthBox.Text.Trim(), out var w)) w = double.NaN;
      lay.WrappingWidth = w;
      lay.CellSize = Northwoods.Go.Size.Parse(cellSizeBox.Text);
      lay.Spacing = Northwoods.Go.Size.Parse(spacingBox.Text);
      lay.Alignment = positionBtn.Checked ? GridAlignment.Position : GridAlignment.Location;
      lay.Arrangement = leftToRightBtn.Checked ? GridArrangement.LeftToRight : GridArrangement.RightToLeft;
      lay.Sorting = (GridSorting)Enum.Parse(typeof(GridSorting), (string)sortingCBox.SelectedItem);

      myDiagram.CommitTransaction("change layout");
    }

    // Custom Model for the sample. Note that no link structure is needed, so the base Model class can be used.
    public class Model : Model<NodeData, int, object> { }

    public class NodeData : Model.NodeData {
      public Brush Fill { get; set; }
      public Size Size { get; set; }
    }
  }
}
