/* Copyright 1998-2023 by Northwoods Software Corporation. */

using Avalonia.Controls;

namespace Demo.Samples.Planogram {
  public partial class Planogram : DemoControl {
    // See the SharedSamples project for sample implementation.

    private void _InitExpanders() {
      smallExpander.PropertyChanged += (s, e) => {
        if (e.Property == Expander.IsExpandedProperty && (bool)e.NewValue) {
          tallExpander.IsExpanded = false;
          wideExpander.IsExpanded = false;
          bigExpander.IsExpanded = false;
        }
      };
      tallExpander.PropertyChanged += (s, e) => {
        if (e.Property == Expander.IsExpandedProperty && (bool)e.NewValue) {
          smallExpander.IsExpanded = false;
          wideExpander.IsExpanded = false;
          bigExpander.IsExpanded = false;
        }
      };
      wideExpander.PropertyChanged += (s, e) => {
        if (e.Property == Expander.IsExpandedProperty && (bool)e.NewValue) {
          smallExpander.IsExpanded = false;
          tallExpander.IsExpanded = false;
          bigExpander.IsExpanded = false;
        }
      };
      bigExpander.PropertyChanged += (s, e) => {
        if (e.Property == Expander.IsExpandedProperty && (bool)e.NewValue) {
          smallExpander.IsExpanded = false;
          tallExpander.IsExpanded = false;
          wideExpander.IsExpanded = false;
        }
      };
    }
  }
}
