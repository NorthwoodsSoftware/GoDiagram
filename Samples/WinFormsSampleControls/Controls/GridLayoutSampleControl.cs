using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;

namespace WinFormsSampleControls.GridLayoutSample {
  [ToolboxItem(false)]
  public partial class GridLayoutSampleControl : System.Windows.Forms.UserControl {

    private Diagram myDiagram;

    public GridLayoutSampleControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      columnBox.Leave += (s, e) => _RedoLayout();
      widthBox.Leave += (s, e) => _RedoLayout();
      cellSizeBox.Leave += (s, e) => _RedoLayout();
      spacingBox.Leave += (s, e) => _RedoLayout();

      positionBtn.CheckedChanged += (s, e) => _RedoLayout();
      locationBtn.CheckedChanged += (s, e) => _RedoLayout();
      leftToRightBtn.CheckedChanged += (s, e) => _RedoLayout();
      rightToLeftBtn.CheckedChanged += (s, e) => _RedoLayout();

      sorting.DataSource = Enum.GetNames(typeof(GridSorting));
      sorting.SelectedIndexChanged += (s, e) => _RedoLayout();

      goWebBrowser1.Html = @"
           <p>
            For information on <b>GridLayout</b> and its properties, see the <a>GridLayout</a> documentation page.
          </p>
";
    }

    private void Setup() {

      myDiagram = diagramControl1.Diagram;

      var rand = new Random();

      myDiagram.Layout = new GridLayout() {
        Comparer = GridLayout.SmartComparer
      };

      myDiagram.NodeTemplate = new Node(PanelLayoutSpot.Instance) {
        // make sure the Node.location is different from the Node.position
        LocationSpot = Spot.Center
      }.Bind("Text") // for sorting
      .Add(
        new Shape {
          Figure = "Ellipse",
          Fill = "lightgray",
          Stroke = null,
          DesiredSize = new Size(30, 30)
        }.Bind("Fill").Bind("DesiredSize", "Size"),
        new TextBlock {
          // the default alignment is Spot.Center
        }.Bind("Text")
      );

      // create an array of data describing randomly colored and sized nodes
      var nodeDataSource = new List<NodeData>();
      for (var i = 0; i < 100; i++) {
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

      _RedoLayout();
    }

    private void _RedoLayout() {
      myDiagram = diagramControl1.Diagram;

      myDiagram.StartTransaction("change layout");
      var lay = myDiagram.Layout as GridLayout;

      lay.WrappingColumn = GetWrappingColumn(columnBox.Text);
      lay.WrappingWidth = GetWrappingWidth(widthBox.Text);
      lay.CellSize = Northwoods.Go.Size.Parse(cellSizeBox.Text);
      lay.Spacing = Northwoods.Go.Size.Parse(spacingBox.Text);
      lay.Alignment = positionBtn.Checked ? GridAlignment.Position : GridAlignment.Location;
      lay.Arrangement = leftToRightBtn.Checked ? GridArrangement.LeftToRight : GridArrangement.RightToLeft;
      lay.Sorting = (GridSorting)Enum.Parse(typeof(GridSorting), (string)sorting.SelectedItem);

      myDiagram.CommitTransaction("change layout");
    }

    private int GetWrappingColumn(string _WrappingColumn) {
      if (int.TryParse(_WrappingColumn, out var c)) return c;
      return 0;
    }

    private double GetWrappingWidth(string _WrappingWidth) {
      if (double.TryParse(_WrappingWidth, out var w))
        return w;
      return double.NaN;
    }
  }

  // Custom Model for the sample. Note that no link structure is needed, so the base Model class
  // can be used.
  public class Model : Model<NodeData, int, object> { }

  public class NodeData : Model.NodeData {
    public Brush Fill { get; set; }
    public Size Size { get; set; }
  }
}
