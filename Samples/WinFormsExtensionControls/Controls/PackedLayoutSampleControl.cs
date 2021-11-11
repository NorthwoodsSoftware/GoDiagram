using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Layouts.Extensions;
using Northwoods.Go.Models;

namespace WinFormsExtensionControls.PackedLayoutSample {
  [ToolboxItem(false)]
  public partial class PackedLayoutSampleControl : System.Windows.Forms.UserControl {
    private Diagram MyDiagram;
    
    public PackedLayoutSampleControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      radBtnEllipticalPackShape.CheckedChanged += (e, obj) => ChangePackShape();

      radBtnAspectRatio.CheckedChanged += (e, obj) => ChangePackMode();
      radBtnExpandToFit.CheckedChanged += (e, obj) => ChangePackMode();
      radBtnFit.CheckedChanged += (e, obj) => ChangePackMode();
      
      radBtnDescending.CheckedChanged += (e, obj) => ChangeSortOrder();

      radBtnNone.CheckedChanged += (e, obj) => ChangeSortMode();
      radBtnMaxSideLength.CheckedChanged += (e, obj) => ChangeSortMode();
      radBtnArea.CheckedChanged += (e, obj) => ChangeSortMode();
      
      radBtnRectNodeShape.CheckedChanged += (e, obj) => ChangeShapeToPack();


      txtAspectRatio.Leave += (obj, e) => {
        double i = AspectRatio;
        if (double.TryParse(txtAspectRatio.Text, out i) && Convert.ToDouble(txtAspectRatio.Text) >= 0) {
          AspectRatio = Convert.ToDouble(txtAspectRatio.Text);
          RebuildGraph();
        }
      };

      txtLayoutWidth.Leave += (obj, e) => {
        int i = LayoutWidth;
        if (int.TryParse(txtLayoutWidth.Text, out i)) {
          LayoutWidth = int.Parse(txtLayoutWidth.Text);
          RebuildGraph();
        }
      };

      txtLayoutHeight.Leave += (obj, e) => {
        int i = LayoutHeight;
        if (int.TryParse(txtLayoutHeight.Text, out i)) {
          LayoutHeight = int.Parse(txtLayoutHeight.Text);
          RebuildGraph();
        }
      };

      txtSpacing.Leave += (obj, e) => {
        int i = NodeSpacing;
        if (int.TryParse(txtSpacing.Text, out i)) {
          NodeSpacing = int.Parse(txtSpacing.Text);
          RebuildGraph();
        }
      };

      txtNumNodes.Leave += (obj, e) => {
        int i = NumNodes;
        if (int.TryParse(txtNumNodes.Text, out i)) {
          NumNodes = int.Parse(txtNumNodes.Text);
          RebuildGraph();
        }
      };

      txtMaxSideLength.Leave += (obj, e) => {
        int i = NodeMaxSide;
        if (int.TryParse(txtMaxSideLength.Text, out i) && int.Parse(txtMaxSideLength.Text) >= int.Parse(txtMinSideLength.Text)) {
          NodeMaxSide = int.Parse(txtMaxSideLength.Text);
          RebuildGraph();
        }
      };

      txtMinSideLength.Leave += (obj, e) => {
        int i = NodeMinSide;
        if (int.TryParse(txtMinSideLength.Text, out i) && int.Parse(txtMinSideLength.Text) <= int.Parse(txtMaxSideLength.Text)) {
          NodeMinSide = int.Parse(txtMinSideLength.Text);
          RebuildGraph();
        }
      };

      checkBxCircular.CheckedChanged += (e, obj) => ChangeCircularNodes();
      checkBxSpiral.CheckedChanged += (e, obj) => ChangeSpiralPacked();
      
      checkBxSameWidthHeight.CheckedChanged += (e, obj) => ChangeSameWidthHeight();

      btnRandomize.Click += (e, obj) => Randomize();

      goWebBrowser1.Html = @"
   <p>
    This sample demonstrates a custom Layout, PackedLayout, which attempts to pack nodes as close together as possible without overlap.
    Each node is assumed to be either rectangular or circular (dictated by the 'nodesAreCircles' property). This layout supports packing
    nodes into either a rectangle or an ellipse, with the shape determined by the PackShape and the aspect ratio determined by either the
    aspectRatio property, or the specified width and height (depending on the PackMode).
  </p>
  <p>
    This extension's code is TypeScript-only and the source files can be found in the <code>extensionsTS</code> directory.
    The layout is defined in its own file, as <a href="".. / extensionsTS / PackedLayout.ts"">extensionsTS/PackedLayout.ts</a>, with an additional dependency on <a href="".. / extensionsTS / Quadtree.ts"">extensionsTS/Quadtree.ts</a>.
  </p>
";

    }

    private bool _SameSides = false;
    private bool _HasCircularNodes = false;

    public int NumNodes { get; set; } = 200;
    public double AspectRatio { get; set; } = 1;
    public int LayoutWidth { get; set; } = 600;
    public int LayoutHeight { get; set; } = 600;
    public int NodeSpacing { get; set; } = 0;
    
    public bool IsSpiralPacked { get; set; } = false;
    public int NodeMinSide { get; set; } = 30;
    public int NodeMaxSide { get; set; } = 50;
    
    public int _PackShape { get; set; } = 0;
    public int _PackMode { get; set; } = 0;
    public int _SortOrder { get; set; } = 0;
    public int _SortMode { get; set; } = 2;
    public int ShapeToPack { get; set; } = 0;

    private bool _SameSidesPrevious;
    private int _MinSidePrevious;
    private int _MaxSidePrevious;

    private void Setup() {

      MyDiagram = diagramControl1.Diagram;
      MyDiagram.Scale = 0.75;
      MyDiagram.IsReadOnly = true;

      MyDiagram.NodeTemplate = new Node(PanelLayoutAuto.Instance) { Background = "transparent" }.Bind("Visible").Add(
        new Shape() {
          StrokeWidth = 1,
          Stroke = "black",
        }.Bind("Figure").Bind("Width").Bind("Height").Bind("Fill")
      );
      MyDiagram.Model = new Model();
      RebuildGraph();
    }

    private void ValidateInput() {
      if (AspectRatio <= 0) AspectRatio = 0.1;
      if (LayoutWidth <= 0) LayoutWidth = 1;
      if (LayoutHeight <= 0) LayoutHeight = 1;
      if (NumNodes < 1) NumNodes = 1;
      if (NodeMinSide < 1) NodeMinSide = 1;
      if (NodeMaxSide < 1) NodeMaxSide = 1;
    }

    private void RebuildGraph() {
      var packShape = PackShape.Elliptical;
      ValidateInput();
      switch (_PackShape) {
        case 0: // 'Elliptical':
          packShape = PackShape.Elliptical;
          break;
        case 1: // 'Rectangular':
          packShape = PackShape.Rectangular;
          break;
        case 2: // 'Spiral':
          packShape = PackShape.Spiral;
          break;
      }
      var packMode = PackMode.AspectOnly;
      switch (_PackMode) {
        case 0: // 'AspectOnly':
          packMode = PackMode.AspectOnly;
          break;
        case 1: // 'ExpandToFit':
          packMode = PackMode.ExpandToFit;
          break;
        case 2: // 'Fit':
          packMode = PackMode.Fit;
          break;
      }
      var sortMode = SortMode.None;
      switch (_SortMode) {
        case 0: // 'None':
          sortMode = SortMode.None;
          break;
        case 1: // 'MaxSide':
          sortMode = SortMode.MaxSide;
          break;
        case 2: // 'Area':
          sortMode = SortMode.Area;
          break;
      }
      var sortOrder = SortOrder.Descending;
      switch (_SortOrder) {
        case 0: // 'Descending':
          sortOrder = SortOrder.Descending;
          break;
        case 1: // 'Ascending':
          sortOrder = SortOrder.Ascending;
          break;
      }
      if (_SameSides != _SameSidesPrevious
        || NodeMinSide != _MinSidePrevious
        || NodeMaxSide != _MaxSidePrevious) {
        _SameSidesPrevious = _SameSides;
        _MinSidePrevious = NodeMinSide;
        _MaxSidePrevious = NodeMaxSide;
        Randomize();
        return;
      }
      diagramControl1.Diagram.StartTransaction("packed layout");
      GenerateNodeData();
      diagramControl1.Diagram.Layout = new PackedLayout {
        PackMode = packMode,
        PackShape = packShape,
        SortMode = sortMode,
        SortOrder = sortOrder,
        AspectRatio = AspectRatio,
        Size = new Size(LayoutWidth, LayoutHeight),
        Spacing = NodeSpacing,
        HasCircularNodes = _HasCircularNodes,
        ArrangesToOrigin = false
      };
      diagramControl1.Diagram.CommitTransaction("packed layout");
    }

    private void Randomize() {
      diagramControl1.Diagram.Model = new Model();
      RebuildGraph();
    }

    private void GenerateNodeData() {
      var rand = new Random();

      var MyDiagram = diagramControl1.Diagram;
      var nodeDataSource = MyDiagram.Model.NodeDataSource as List<NodeData>;
      var count = NumNodes;
      var min = NodeMinSide;
      var max = NodeMaxSide;

      Func<int, string> asFigure = s => {
        switch (s) {
          case 0: return "Rectangle";
          case 1: return "Ellipse";
          default: return "Rectangle";
        }
      };

      if (count > nodeDataSource.Count) {
        var arr = new List<NodeData>();
        for (var i = nodeDataSource.Count; i < count; i++) {
          var width = rand.Next(min, 1 + max);
          var height = _SameSides ? width : rand.Next(min, 1 + max);
          var color = Brush.RandomColor(128, 235);
          arr.Add(new NodeData {
            Key = i, Width = width, Height = height, Fill = color, Figure = asFigure(ShapeToPack)
          });
        }
        foreach (var d in arr) MyDiagram.Model.AddNodeData(d);
      } else if (count < nodeDataSource.Count) {
        while (count < nodeDataSource.Count) {
          MyDiagram.Model.RemoveNodeData(MyDiagram.Model.NodeDataSource.ElementAt(MyDiagram.Model.NodeDataSource.Count() - 1));
        }
      } else {
        var _nodeDataSource_1 = nodeDataSource;
        for (var _i = 0; _i < _nodeDataSource_1.Count; _i++) {
          var data = _nodeDataSource_1[_i];
          MyDiagram.Model.Set(data, "Figure", asFigure(ShapeToPack));
        }
      }

    }

    // Functions to call for RadioButton events
    private void ChangePackShape() {
      if (radBtnEllipticalPackShape.Checked) {
        _PackShape = 0;
      } else {
        _PackShape = 1;
      }
      RebuildGraph();
    }

    private void ChangePackMode() {
      if (radBtnAspectRatio.Checked) {
        _PackMode = 0;
        txtAspectRatio.Enabled = true;
        txtLayoutHeight.Enabled = false;
        txtLayoutWidth.Enabled = false;
      }
      else if (radBtnExpandToFit.Checked) {
        _PackMode = 1;
        txtAspectRatio.Enabled = false;
        txtLayoutHeight.Enabled = true;
        txtLayoutWidth.Enabled = true;
      }
      else if (radBtnFit.Checked) {
        _PackMode = 2;
        txtAspectRatio.Enabled = false;
        txtLayoutHeight.Enabled = true;
        txtLayoutWidth.Enabled = true;
      }
      RebuildGraph();
    }

    private void ChangeSortOrder() {
      if (radBtnDescending.Checked) {
        _SortOrder = 0;
      }
      else {
        _SortOrder = 1;
      }
      RebuildGraph();
    }

    private void ChangeSortMode() {
      if (radBtnNone.Checked) {
        _SortMode = 0;
      }
      else if (radBtnMaxSideLength.Checked) {
        _SortMode = 1;
      }
      else if (radBtnArea.Checked) {
        _SortMode = 2;
      }
      RebuildGraph();
    }

    private void ChangeShapeToPack() {
      if (radBtnRectNodeShape.Checked) {
        ShapeToPack = 0;
      }
      else {
        ShapeToPack = 1;
      }
      RebuildGraph();
    }

    // Functions to call for Checkbox events
    private void ChangeCircularNodes() {
      _HasCircularNodes = !_HasCircularNodes;
      RebuildGraph();
    }

    private void ChangeSpiralPacked() {
      IsSpiralPacked = !IsSpiralPacked;
      RebuildGraph();
    }

    private void ChangeSameWidthHeight() {
      _SameSides = !_SameSides;
      RebuildGraph();
    }


  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }

  public class NodeData : Model.NodeData {
    public int Width { get; set; }
    public int Height { get; set; }
    public Brush Fill { get; set; }
    public string Figure { get; set; }
  }

  public class LinkData : Model.LinkData { }

}
