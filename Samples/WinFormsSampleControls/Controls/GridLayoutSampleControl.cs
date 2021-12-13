using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.WinForms;
using Northwoods.Go.Tools;
using Northwoods.Go.Layouts;
using System.Linq;
using System.Windows.Forms.VisualStyles;

namespace WinFormsSampleControls.GridLayoutSample {
  [ToolboxItem(false)]
  public partial class GridLayoutSampleControl : System.Windows.Forms.UserControl {

    private Diagram myDiagram;

    
    private GridAlignment _Alignment = GridAlignment.Position;
    private GridArrangement _Arrangement = GridArrangement.LeftToRight;


    private GridSorting _Sorting = GridSorting.Forward;
    public GridLayoutSampleControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      columnBox.Leave += (e, obj) => WrappingColumn(columnBox.Text);
      widthBox.Leave += (e, obj) => WrappingWidth(widthBox.Text);
      cellSizeBox.Leave += (e, obj) => CellSize(cellSizeBox.Text);
      spacingBox.Leave += (e, obj) => Spacing(spacingBox.Text);

      positionBtn.CheckedChanged += (e, obj) => Alignment = GridAlignment.Position;
      locationBtn.CheckedChanged += (e, obj) => Alignment = GridAlignment.Location;
      leftToRightBtn.CheckedChanged += (e, obj) => Arrangement = GridArrangement.LeftToRight;
      rightToLeftBtn.CheckedChanged += (e, obj) => Arrangement = GridArrangement.RightToLeft;

      sorting.DataSource = Enum.GetNames(typeof(GridSorting));
      sorting.SelectedIndexChanged += (s, e) => { Sorting = (GridSorting)Enum.Parse(typeof(GridSorting), (string)sorting.SelectedItem); };

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

      // copy properties into GridLayout
      WrappingColumn(columnBox.Text);
      WrappingWidth(widthBox.Text);
      CellSize(cellSizeBox.Text);
      Spacing(spacingBox.Text);
      lay.Alignment = Alignment;
      lay.Arrangement = Arrangement;
      lay.Sorting = Sorting;

      myDiagram.CommitTransaction("change layout");
    }

    private void WrappingColumn(string _WrappingColumn) {
      double i = 10;
      if (_WrappingColumn.Trim().ToLower().Equals("nan")) _WrappingColumn = int.MaxValue.ToString();
      else if (!double.TryParse(_WrappingColumn.Trim(), out i)) return;
      var lay = diagramControl1.Diagram.Layout as GridLayout;
      lay.WrappingColumn = Convert.ToInt32(_WrappingColumn);
    }

    private void WrappingWidth(string _WrappingWidth) {
      double i = 0;
      if (_WrappingWidth.Trim().ToLower().Equals("nan")) _WrappingWidth = "1000";
      else if (!double.TryParse(_WrappingWidth.Trim(), out i)) return;
      var lay = diagramControl1.Diagram.Layout as GridLayout;
      lay.WrappingWidth = Convert.ToDouble(_WrappingWidth);
    }

    private void CellSize(string _CellSize) {
      var lay = diagramControl1.Diagram.Layout as GridLayout;
      string trimmed = _CellSize.Trim();
      
      // Case: one double
      if (trimmed.IndexOf(" ") == -1) {
        if (double.TryParse(trimmed, out double i)) {
          lay.CellSize = Northwoods.Go.Size.Parse(i + " 0");
        }
        // Case: NaN
        if (trimmed.ToLower().Equals("nannan")) {
          lay.CellSize = Northwoods.Go.Size.Parse("NaN, NaN");
        }
        else if (trimmed.ToLower().Equals("nan")) {
          lay.CellSize = Northwoods.Go.Size.Parse("NaN, NaN");
        }
      }
      // Case: two doubles
      else {
        string xSize = trimmed.Substring(0, trimmed.IndexOf(" "));
        string ySize = trimmed.Substring(trimmed.LastIndexOf(" ") + 1);
        if (xSize.ToLower().Equals("nan")) xSize = "NaN";
        if (ySize.ToLower().Equals("nan")) ySize = "NaN";
        if ((xSize.Equals("NaN") || double.TryParse(xSize, out double x)) && (ySize.Equals("NaN") || double.TryParse(ySize, out double y))) {
          lay.CellSize = Northwoods.Go.Size.Parse(xSize + " " + ySize);
        }
      }
    }

    private void Spacing(string _Spacing) {
      var lay = diagramControl1.Diagram.Layout as GridLayout;
      string trimmed = _Spacing.Trim();

      // Case: one double
      if (trimmed.IndexOf(" ") == -1) {
        if (double.TryParse(trimmed, out double i)) {
          lay.Spacing = Northwoods.Go.Size.Parse(i + " 0");
        }
        // Case: NaN
        if (trimmed.ToLower().Equals("nannan")) {
          lay.Spacing = Northwoods.Go.Size.Parse("NaN, NaN");
        } else if (trimmed.ToLower() == "nan") {
          lay.Spacing = Northwoods.Go.Size.Parse("NaN, NaN");
        }
      }
      // Case: two doubles
      else {
        string xSize = trimmed.Substring(0, trimmed.IndexOf(" ") - 0);
        string ySize = trimmed.Substring(trimmed.LastIndexOf(" ") + 1);
        if (xSize.ToLower().Equals("nan")) xSize = "NaN";
        if (ySize.ToLower().Equals("nan")) ySize = "NaN";
        if ((xSize.Equals("NaN") || double.TryParse(xSize, out double x)) && (ySize.Equals("NaN") || double.TryParse(ySize, out double y))) {
          lay.Spacing = Northwoods.Go.Size.Parse(xSize + " " + ySize);
        }
      }
    }
    

    // Get or set the alignment type (position or location) for each node in the layout.
    public GridAlignment Alignment {
      get {
        return _Alignment;
      }
      set {
        if (_Alignment != value) {
          _Alignment = value;
          _RedoLayout();
        }
      }
    }

    // Get or set the arrangement (left->right or right->left) of the Grid nodes.
    public GridArrangement Arrangement {
      get {
        return _Arrangement;
      }
      set {
        if (_Arrangement != value) {
          _Arrangement = value;
          _RedoLayout();
        }
      }
    }

    // Get or set the sorting that should be used by the GridLayout.
    public GridSorting Sorting {
      get {
        return _Sorting;
      }
      set {
        if (_Sorting != value) {
          _Sorting = value;
          _RedoLayout();
        }
      }
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
