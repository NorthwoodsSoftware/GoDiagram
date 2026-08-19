/* Copyright (c) Northwoods Software Corporation. */

using System.Windows.Controls;
using Northwoods.Go.Layouts.Extensions;

namespace Demo.Extensions.Packed; 
public partial class Packed : DemoControl {
  // This is a stub for the designer.
  // See the SharedSamples project for sample implementation.

  private bool _GetChecked(CheckBox cb) {
    return cb.IsChecked == true;
  }

  private bool _GetChecked(RadioButton rb) {
    return rb.IsChecked == true;
  }

  private void _InitControls() {
    // ComboBoxes
    packShape.ItemsSource = Enum.GetNames<PackShape>();
    packMode.ItemsSource = Enum.GetNames<PackMode>();
    sortOrder.ItemsSource = Enum.GetNames<SortOrder>();
    sortMode.ItemsSource = Enum.GetNames<SortMode>();

    packShape.SelectedItem = "Elliptical";
    packMode.SelectedItem = "AspectOnly";
    sortOrder.SelectedItem = "Descending";
    sortMode.SelectedItem = "Area";

    packShape.SelectionChanged += (s, e) => _Layout();
    packMode.SelectionChanged += (s, e) => _Layout();
    sortOrder.SelectionChanged += (s, e) => _Layout();
    sortMode.SelectionChanged += (s, e) => _Layout();

    // TextBoxes
    aspectRatio.LostFocus += (s, e) => _Layout();
    width.LostFocus += (s, e) => _Layout();
    height.LostFocus += (s, e) => _Layout();
    spacing.LostFocus += (s, e) => _Layout();

    // CheckBoxes
    hasCircularNodes.Checked += (s, e) => _RebuildGraph();
    hasCircularNodes.Unchecked += (s, e) => _RebuildGraph();
  }

  private void _DisableInputs() {
    var myPackShape = (PackShape)Enum.Parse(typeof(PackShape), (string)packShape.SelectedItem);
    var myPackMode = (PackMode)Enum.Parse(typeof(PackMode), (string)packMode.SelectedItem);
    var myHasCircularNodes = hasCircularNodes.IsChecked == true;

    packMode.IsEnabled = myPackShape != PackShape.Spiral;
    aspectRatio.IsEnabled = myPackMode == PackMode.AspectOnly && myPackShape != PackShape.Spiral;
    width.IsEnabled = myPackMode != PackMode.AspectOnly && myPackShape != PackShape.Spiral;
    height.IsEnabled = myPackMode != PackMode.AspectOnly && myPackShape != PackShape.Spiral;
    spacing.IsEnabled = myPackMode != PackMode.ExpandToFit;
    hasCircularNodes.IsEnabled = myPackShape != PackShape.Spiral;

    if (myPackShape == PackShape.Spiral) {
      if (_HasCircularNodesSavedState == null) _HasCircularNodesSavedState = hasCircularNodes.IsChecked;
      hasCircularNodes.IsChecked = myHasCircularNodes = true;
    } else if (_HasCircularNodesSavedState != null) {
      hasCircularNodes.IsChecked = _HasCircularNodesSavedState.Value;
      myHasCircularNodes = false;
      _HasCircularNodesSavedState = null;
    }

    sameSides.IsEnabled = !myHasCircularNodes;
    if (myHasCircularNodes) {
      if (_SameSidesSavedState == null) _SameSidesSavedState = sameSides.IsChecked;
      sameSides.IsChecked = true;
    } else if (_SameSidesSavedState != null) {
      sameSides.IsChecked = _SameSidesSavedState.Value;
      _SameSidesSavedState = null;
    }
  }
}
