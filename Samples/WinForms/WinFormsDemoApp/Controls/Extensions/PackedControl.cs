/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using System.ComponentModel;
using Northwoods.Go.Layouts.Extensions;

namespace Demo.Extensions.Packed {
  [ToolboxItem(false)]
  public partial class Packed : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private bool _GetChecked(System.Windows.Forms.CheckBox cb) {
      return cb.Checked;
    }

    private bool _GetChecked(System.Windows.Forms.RadioButton rb) {
      return rb.Checked;
    }

    private void _InitControls() {
      // ComboBoxes
      packShape.DataSource = Enum.GetNames(typeof(PackShape));
      packMode.DataSource = Enum.GetNames(typeof(PackMode));
      sortOrder.DataSource = Enum.GetNames(typeof(SortOrder));
      sortMode.DataSource = Enum.GetNames(typeof(SortMode));

      packShape.SelectedItem = "Elliptical";
      packMode.SelectedItem = "AspectOnly";
      sortOrder.SelectedItem = "Descending";
      sortMode.SelectedItem = "Area";

      packShape.SelectedIndexChanged += (s, e) => _Layout();
      packMode.SelectedIndexChanged += (s, e) => _Layout();
      sortOrder.SelectedIndexChanged += (s, e) => _Layout();
      sortMode.SelectedIndexChanged += (s, e) => _Layout();

      // NumericUpDowns
      aspectRatio.ValueChanged += (s, e) => _Layout();
      width.ValueChanged += (s, e) => _Layout();
      height.ValueChanged += (s, e) => _Layout();
      spacing.ValueChanged += (s, e) => _Layout();

      // CheckBoxes
      hasCircularNodes.CheckedChanged += (s, e) => _RebuildGraph();
    }

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
}
