/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Layouts.Extensions;
using Northwoods.Go.Models;

namespace Demo.Extensions.Packed {
  public partial class Packed : DemoControl {
    private Diagram _Diagram;

    public Packed() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      _InitControls();
      randomizeBtn.Click += (s, e) => _RebuildGraph();

      desc1.MdText = DescriptionReader.Read("Extensions.Packed.md");

      // Ensure page is loaded before we start updating the layout from the UI
      AfterLoad(Setup);
    }

    private void Setup() {
      _Diagram.Scale = 0.75;
      _Diagram.IsReadOnly = true;
      _Diagram.Layout = new PackedLayout { ArrangesToOrigin = false };

      _Diagram.NodeTemplate =
        new Node { Background = "transparent" }
          .Bind("Visible")
          .Add(
            new Shape { StrokeWidth = 0 }
              .Bind("Figure")
              .Bind("Width")
              .Bind("Height")
              .Bind("Fill")
          );

      _Diagram.Model = new Model();

      _RebuildGraph();
    }

    private void _RebuildGraph() {
      _ValidateInput();
      _GenerateNodeData((int)numNodes.Value, _GetChecked(rectangleRb), (int)nodeMinSide.Value, (int)nodeMaxSide.Value, _GetChecked(sameSides));
      _Layout();
    }

    private void _ValidateInput() {
      if (aspectRatio.Value <= 0) aspectRatio.Value = ToNumericUpDownValue(0.1);
      if (width.Value <= 0) width.Value = 1;
      if (height.Value <= 0) height.Value = 1;
      if (numNodes.Value < 1) numNodes.Value = 1;
      if (nodeMinSide.Value < 1) nodeMinSide.Value = 1;
      if (nodeMaxSide.Value < 1) nodeMaxSide.Value = 1;
      if (nodeMaxSide.Value < nodeMinSide.Value) nodeMaxSide.Value = nodeMinSide.Value;
      _DisableInputs();
    }

    private void _GenerateNodeData(int numNodes, bool isRect, int minSide, int maxSide, bool sameSide) {
      var rand = new Random();
      var fig = isRect ? "Rectangle" : "Ellipse";

      var arr = new List<NodeData>();
      for (var i = 1; i <= numNodes; i++) {
        var width = rand.Next(minSide, maxSide + 1);
        var height = sameSide ? width : rand.Next(minSide, maxSide + 1);
        arr.Add(new NodeData {
          Key = i, Width = width, Height = height, Fill = Brush.RandomColor(128, 235), Figure = fig
        });
      }

      _Diagram.Model = new Model {
        NodeDataSource = arr
      };
    }

    private void _Layout() {
      _ValidateInput();
      if (_Diagram.Layout is not PackedLayout lay) return;
      _Diagram.StartTransaction("change layout");
      lay.PackShape = (PackShape)Enum.Parse(typeof(PackShape), (string)packShape.SelectedItem);
      lay.PackMode = (PackMode)Enum.Parse(typeof(PackMode), (string)packMode.SelectedItem);
      lay.AspectRatio = (double)aspectRatio.Value;
      lay.Size = new Size((double)width.Value, (double)height.Value);
      lay.Spacing = (double)spacing.Value;
      lay.SortOrder = (SortOrder)Enum.Parse(typeof(SortOrder), (string)sortOrder.SelectedItem);
      lay.SortMode = (SortMode)Enum.Parse(typeof(SortMode), (string)sortMode.SelectedItem);
      lay.HasCircularNodes = _GetChecked(hasCircularNodes);
      _Diagram.CommitTransaction("change layout");
    }

    private bool? _HasCircularNodesSavedState = null;
    private bool? _SameSidesSavedState = null;
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
