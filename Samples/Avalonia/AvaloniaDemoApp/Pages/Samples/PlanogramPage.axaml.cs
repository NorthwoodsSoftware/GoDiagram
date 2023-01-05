/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using Avalonia;

namespace Demo.Samples.Planogram {
  public partial class Planogram : DemoControl {
    // See the SharedSamples project for sample implementation.

    private void _InitExpanders() {
      smallExpander.GetPropertyChangedObservable(Avalonia.Controls.Expander.IsExpandedProperty).Subscribe(_OnSmallExpanded);
      tallExpander.GetPropertyChangedObservable(Avalonia.Controls.Expander.IsExpandedProperty).Subscribe(_OnTallExpanded);
      wideExpander.GetPropertyChangedObservable(Avalonia.Controls.Expander.IsExpandedProperty).Subscribe(_OnWideExpanded);
      bigExpander.GetPropertyChangedObservable(Avalonia.Controls.Expander.IsExpandedProperty).Subscribe(_OnBigExpanded);
    }

    private void _OnSmallExpanded(AvaloniaPropertyChangedEventArgs obj) {
      if ((bool)obj.NewValue) {
        tallExpander.IsExpanded = false;
        wideExpander.IsExpanded = false;
        bigExpander.IsExpanded = false;
      }
    }

    private void _OnTallExpanded(AvaloniaPropertyChangedEventArgs obj) {
      if ((bool)obj.NewValue) {
        smallExpander.IsExpanded = false;
        wideExpander.IsExpanded = false;
        bigExpander.IsExpanded = false;
      }
    }

    private void _OnWideExpanded(AvaloniaPropertyChangedEventArgs obj) {
      if ((bool)obj.NewValue) {
        smallExpander.IsExpanded = false;
        tallExpander.IsExpanded = false;
        bigExpander.IsExpanded = false;
      }
    }

    private void _OnBigExpanded(AvaloniaPropertyChangedEventArgs obj) {
      if ((bool)obj.NewValue) {
        smallExpander.IsExpanded = false;
        tallExpander.IsExpanded = false;
        wideExpander.IsExpanded = false;
      }
    }
  }
}
