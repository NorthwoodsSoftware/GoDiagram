/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Layouts.Extensions;
using Northwoods.Go.Models;

namespace WinFormsExtensionControls.Packed {
  [ToolboxItem(false)]
  public partial class PackedControl : System.Windows.Forms.UserControl {
    private Diagram _Diagram;

    public PackedControl() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      packShape.DataSource = Enum.GetNames(typeof(PackShape));
      packMode.DataSource = Enum.GetNames(typeof(PackMode));
      sortOrder.DataSource = Enum.GetNames(typeof(SortOrder));
      sortMode.DataSource = Enum.GetNames(typeof(SortMode));

      packShape.SelectedItem = "Elliptical";
      packMode.SelectedItem = "AspectOnly";
      sortOrder.SelectedItem = "Descending";
      sortMode.SelectedItem = "Area";

      packShape.SelectedIndexChanged += _PropertyChanged;
      packMode.SelectedIndexChanged += _PropertyChanged;
      sortOrder.SelectedIndexChanged += _PropertyChanged;
      sortMode.SelectedIndexChanged += _PropertyChanged;

      hasCircularNodes.CheckedChanged += (s, e) => _RebuildGraph();

      Setup();

      goWebBrowser1.Html = @"
   <p>
    This sample demonstrates a custom Layout, PackedLayout, which attempts to pack nodes as close together as possible without overlap.
    Each node is assumed to be either rectangular or circular (dictated by the 'HasCircularNodes' property). This layout supports packing
    nodes into either a rectangle or an ellipse, with the shape determined by the PackShape and the aspect ratio determined by either the
    AspectRatio property, or the specified width and height (depending on the PackMode).
  </p>
  <p>
    The layout is defined in its own file, as <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Layouts/Packed/PackedLayout.cs"">PackedLayout.cs</a>,
    with an additional dependency on <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Layouts/Packed/Quadtree.cs"">Quadtree.cs</a>.
  </p>
";

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

      _Diagram.DelayInitialization((d) => {
        _RebuildGraph();
      });
    }

    private void _RebuildGraph() {
      _ValidateInput();
      _Diagram.DelayInitialization((d) => {
        d.StartTransaction("randomize graph");
        _GenerateNodeData((int)numNodes.Value, rectangleRb.Checked, (int)nodeMinSide.Value, (int)nodeMaxSide.Value, sameSides.Checked);
        _Layout();
        d.CommitTransaction("randomize graph");
      });
    }

    private void _ValidateInput() {
      if (aspectRatio.Value <= 0) aspectRatio.Value = 0.1M;
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
      _Diagram.Model.NodeDataSource = arr;
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
      lay.HasCircularNodes = hasCircularNodes.Checked;
      _Diagram.CommitTransaction("change layout");
    }

    private void randomizeBtn_Click(object sender, EventArgs e) {
      _RebuildGraph();
    }

    private void _PropertyChanged(object sender, EventArgs e) {
      _Layout();
    }

    private bool? _HasCircularNodesSavedState = null;
    private bool? _SameSidesSavedState = null;
    private void _DisableInputs() {
      var myPackShape = (PackShape)Enum.Parse(typeof(PackShape), (string)packShape.SelectedItem);
      var myPackMode = (PackMode)Enum.Parse(typeof(PackMode), (string)packMode.SelectedItem);
      var myHasCircularNodes = hasCircularNodes.Checked;

      packMode.Enabled = myPackShape != PackShape.Spiral;
      aspectRatio.Enabled = myPackMode == PackMode.AspectOnly && myPackShape != PackShape.Spiral;
      width.Enabled = myPackMode != PackMode.AspectOnly && myPackShape != PackShape.Spiral;
      height.Enabled = myPackMode != PackMode.AspectOnly && myPackShape != PackShape.Spiral;
      spacing.Enabled = myPackMode != PackMode.ExpandToFit;
      hasCircularNodes.Enabled = myPackShape != PackShape.Spiral;

      if (myPackShape == PackShape.Spiral) {
        if (_HasCircularNodesSavedState == null) _HasCircularNodesSavedState = hasCircularNodes.Checked;
        hasCircularNodes.Checked = myHasCircularNodes = true;
      } else if (_HasCircularNodesSavedState != null) {
        hasCircularNodes.Checked = _HasCircularNodesSavedState.Value;
        myHasCircularNodes = false;
        _HasCircularNodesSavedState = null;
      }

      sameSides.Enabled = !myHasCircularNodes;
      if (myHasCircularNodes) {
        if (_SameSidesSavedState == null) _SameSidesSavedState = sameSides.Checked;
        sameSides.Checked = true;
      } else if (_SameSidesSavedState != null) {
        sameSides.Checked = _SameSidesSavedState.Value;
        _SameSidesSavedState = null;
      }
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
