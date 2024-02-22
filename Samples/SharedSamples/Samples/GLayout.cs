/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;

namespace Demo.Samples.GLayout {
  public partial class GLayout : DemoControl {
    private Diagram _Diagram;

    public GLayout() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      _InitControls();

      desc1.MdText = DescriptionReader.Read("Samples.GLayout.md");

      // Ensure page is loaded before we start updating the layout from the UI
      AfterLoad(Setup);
    }

    private void Setup() {
      var rand = new Random();

      _Diagram.Layout = new GridLayout() {
        Comparer = GridLayout.SmartComparer  // have the comparer sort by numbers as well as letters
      }; // other properties are set by the layout function, defined below

      _Diagram.NodeTemplate =
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
      _Diagram.Model = new Model {
        NodeDataSource = nodeDataSource
      };

      _Layout();
    }

    private void _Layout() {
      if (_Diagram.Layout is not GridLayout lay) return;
      _Diagram.StartTransaction("change layout");

      if (!int.TryParse(wrapColTb.Text.Trim(), out var c)) c = 0;
      lay.WrappingColumn = c;
      if (double.TryParse(wrapWidthTb.Text.Trim(), out var w)) w = double.NaN;
      lay.WrappingWidth = w;
      lay.CellSize = Northwoods.Go.Size.Parse(cellSizeTb.Text);
      lay.Spacing = Northwoods.Go.Size.Parse(spacingTb.Text);
      lay.Alignment = _GetChecked(alignPosRb) ? GridAlignment.Position : GridAlignment.Location;
      lay.Arrangement = _GetChecked(arrangeLTRRb) ? GridArrangement.LeftToRight : GridArrangement.RightToLeft;
      lay.Sorting = (GridSorting)Enum.Parse(typeof(GridSorting), (string)sortingCb.SelectedItem);

      _Diagram.CommitTransaction("change layout");
    }

    // Custom Model for the sample. Note that no link structure is needed, so the base Model class can be used.
    public class Model : Model<NodeData, int, object> { }

    public class NodeData : Model.NodeData {
      public Brush Fill { get; set; }
      public Size Size { get; set; }
    }
  }
}
