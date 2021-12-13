using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.LayerBands {
  [ToolboxItem(false)]
  public partial class LayerBandsControl : System.Windows.Forms.UserControl {
    private Diagram MyDiagram;

    public LayerBandsControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      goWebBrowser1.Html = @"

 <p>
    Unlike swim lane diagrams where the nodes are supposed to stay in their lanes,
    layer bands run perpendicular to the growth direction of the layout.
  </p>
  <p>
    This sample uses a custom <a>TreeLayout</a> that overrides the <a>TreeLayout.CommitLayers</a> method
    in order to specify the position and size of each ""band"" that surrounds a layer of the tree.
    The ""bands"" are held in a single Part that is bound to a particular node data object whose key is ""_BANDS"".
    The headers, and potentially any other information that you might want to display in the headers,
    are stored in this ""_BANDS"" object in a List.
  </p>
  <p>
    This sample can be adapted to use a <a>GraphLinksModel</a> instead of a <a>TreeModel</a>
     and a <a>LayeredDigraphLayout</a> instead of a <a>TreeLayout</a>.
    </p>
";

    }

    private void Setup() {

      MyDiagram = diagramControl1.Diagram;

      MyDiagram.Layout = new BandedTreeLayout { // custom layout defined above
        Angle = 0,
        Arrangement = TreeArrangement.Vertical
      };
      MyDiagram.UndoManager.IsEnabled = true;

      MyDiagram.NodeTemplate = new Node(PanelLayoutAuto.Instance).Add(
        new Shape {
          Figure = "Rectangle",
          Fill = "white"
        },
        new TextBlock {
          Margin = 5
        }.Bind("Text", "Key")
      );

      // There should be at most a single object of this category.
      // This Part will be modified by BandedTreeLayout.commitLayers to display visual "bands"
      // where each "layer" is a layer of the tree.
      // This template is parameterized at load time by the HORIZONTAL parameter.
      // You also have the option of showing rectangles for the layer bands or
      // of showing separator lines between the layers, but not both at the same time,
      // by commenting in/out the indicated code.
      MyDiagram.NodeTemplateMap.Add("Bands",
        new Part(PanelLayoutPosition.Instance) {
          IsLayoutPositioned = false, // but still in document bounds
          LocationSpot = new Spot(0, 0, 0, 16),
          LayerName = "Background",
          Pickable = false,
          Selectable = false,
          ItemTemplate =
            new Panel(PanelLayoutVertical.Instance)
            .Bind(new Binding("Position", "Bounds", (b, _) => { return ((Rect)b).Position; }))
            .Add(
              new TextBlock {
                Angle = 0,
                TextAlign = TextAlign.Center,
                Wrap = Wrap.None,
                Font = "bold 11pt sans-serif",
                Background = "aqua"
              }.Bind("Text").Bind("Width", "Bounds", (r, _) => ((Rect)r).Width),
              // option 1: rectangular bands
              new Shape {
                Stroke = null, StrokeWidth = 0
              }.Bind(
                new Binding("DesiredSize", "Bounds", (r, _) => ((Rect)r).Size),
                new Binding("Fill", "ItemIndex", (i, _) => ((int)i % 2 == 0) ? "whitesmoke" : Brush.Darken("whitesmoke")).OfElement()
              )
            // option 2: separator lines
            //new Shape {
            //  Figure = "LineV",
            //  Stroke = "gray",
            //  Alignment = Spot.TopLeft,
            //  Width = 1
            //}.Put(
            //  new Binding("Height", "Bounds", (r, _) => ((Rect)r).Height),
            //  new Binding("Visible", "ItemIndex", (i, _) => (int)i > 0).OfElement()
            //)
            )
        }.Bind("ItemList")
      );

      // simple black line, no arrowhead needed
      MyDiagram.LinkTemplate = new Link().Add(new Shape());

      var nodearray = new List<NodeData> {
        // this is the information needed for the headers of the bands
        new NodeData { Key = "_BANDS", Category = "Bands",
          ItemList = new List<ItemData> {
            new ItemData { Text = "Zero" },
            new ItemData { Text = "One" },
            new ItemData { Text = "Two" },
            new ItemData { Text = "Three" },
            new ItemData { Text = "Four" },
            new ItemData { Text = "Five" }
          }
        },
        // these are the regular nodes in the TreeModel
        new NodeData { Key = "root" },
        new NodeData { Key = "oneB", Parent = "root" },
        new NodeData { Key = "twoA", Parent = "oneB" },
        new NodeData { Key = "twoC", Parent = "root" },
        new NodeData { Key = "threeC", Parent = "twoC" },
        new NodeData { Key = "threeD", Parent = "twoC" },
        new NodeData { Key = "fourB", Parent = "threeD" },
        new NodeData { Key = "fourC", Parent = "twoC" },
        new NodeData { Key = "fourD", Parent = "fourB" },
        new NodeData { Key = "twoD", Parent = "root" }
      };

      MyDiagram.Model = new Model {
        NodeDataSource = nodearray
      };
    }

  }

  public class Model : TreeModel<NodeData, string, object> { }

  public class NodeData : Model.NodeData {
    public List<ItemData> ItemList { get; set; } = null; // stores information needed for band headers
  }

  public class ItemData {
    public Rect Bounds { get; set; }
    public string Text { get; set; }
    public int ItemIndex { get; set; }
  }

  public class BandedTreeLayout : TreeLayout {
    public BandedTreeLayout() : base() {
      LayerStyle = TreeLayerStyle.Uniform; // needed for straight layers
    }

    protected override void CommitLayers(List<Rect> layerRects, Point offset) {
      // update the background object holding the visual bands
      var bands = Diagram.FindPartForKey("_BANDS");
      if (bands != null) {
        var model = Diagram.Model;
        bands.Location = new Northwoods.Go.Point(ArrangementOrigin.X, ArrangementOrigin.Y).Add(offset);

        // make each band visible or not, depending on whether there is a layer for it
        for (var idx = 0; idx < bands.Elements.Count(); idx++) {
          var elt = bands.Elements.ElementAt(idx); // the item panel representing a band
          elt.Visible = idx < layerRects.Count;
        }

        // set the bounds of each band via data binding of the "Bounds" property
        var arr = (bands.Data as NodeData).ItemList;
        if (arr == null) {
          Console.WriteLine("warning: _BANDS part doesn't have an ItemList property.");
        } else {
          for (var i = 0; i < layerRects.Count; i++) {
            var itemdata = arr[i];
            if (itemdata != null) {
              model.Set(itemdata, "Bounds", layerRects[i]);
            }
          }
        }
      }
    } // end CommitLayers
  } // end BandedTreeLayout

}
