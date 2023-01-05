/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace Demo.Samples.SwimBands {
  public partial class SwimBands : DemoControl {
    private Diagram _Diagram;

    public SwimBands() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      desc1.MdText = DescriptionReader.Read("Samples.SwimBands.md");
    }

    private void Setup() {
      _Diagram.Layout = new BandedTreeLayout { // custom layout defined above
        Angle = 0,
        Arrangement = TreeArrangement.Vertical
      };
      _Diagram.UndoManager.IsEnabled = true;

      _Diagram.NodeTemplate =
        new Node("Auto")
        .Add(
          new Shape { Figure = "Rectangle", Fill = "white" },
          new TextBlock { Margin = 5 }
            .Bind("Text", "Key")
        );

      // There should be at most a single object of this category.
      // This Part will be modified by BandedTreeLayout.commitLayers to display visual "bands"
      // where each "layer" is a layer of the tree.
      // This template is parameterized at load time by the HORIZONTAL parameter.
      // You also have the option of showing rectangles for the layer bands or
      // of showing separator lines between the layers, but not both at the same time,
      // by commenting in/out the indicated code.
      _Diagram.NodeTemplateMap.Add("Bands",
        new Part(PanelType.Position) {
          IsLayoutPositioned = false, // but still in document bounds
          LocationSpot = new Spot(0, 0, 0, 16),
          LayerName = "Background",
          Pickable = false,
          Selectable = false,
          ItemTemplate =
            new Panel(PanelType.Vertical)
            .Bind(new Binding("Position", "Bounds", (b) => { return ((Rect)b).Position; }))
            .Add(
              new TextBlock {
                Angle = 0,
                TextAlign = TextAlign.Center,
                Wrap = Wrap.None,
                Font = new Font("Segoe UI", 15, Northwoods.Go.FontWeight.Bold),
                Background = "aqua"
              }.Bind("Text").Bind("Width", "Bounds", (r) => ((Rect)r).Width),
              // option 1: rectangular bands
              new Shape {
                Stroke = null, StrokeWidth = 0
              }.Bind(
                new Binding("DesiredSize", "Bounds", (r) => ((Rect)r).Size),
                new Binding("Fill", "ItemIndex", (i) => ((int)i % 2 == 0) ? "whitesmoke" : Brush.Darken("whitesmoke")).OfElement()
              )
            // option 2: separator lines
            //new Shape {
            //  Figure = "LineV",
            //  Stroke = "gray",
            //  Alignment = Spot.TopLeft,
            //  Width = 1
            //}.Put(
            //  new Binding("Height", "Bounds", (r) => ((Rect)r).Height),
            //  new Binding("Visible", "ItemIndex", (i) => (int)i > 0).OfElement()
            //)
            )
        }.Bind("ItemList")
      );

      // simple black line, no arrowhead needed
      _Diagram.LinkTemplate = new Link().Add(new Shape());

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

      _Diagram.Model = new Model {
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
        bands.Location = new Point(ArrangementOrigin.X, ArrangementOrigin.Y).Add(offset);

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
