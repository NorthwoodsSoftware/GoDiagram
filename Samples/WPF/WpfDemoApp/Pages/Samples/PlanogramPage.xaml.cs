/* Copyright (c) Northwoods Software Corporation. */

namespace Demo.Samples.Planogram; 
public partial class Planogram : DemoControl {
  // See the SharedSamples project for sample implementation.

  private void _InitExpanders() {
    smallExpander.Expanded += (s, e) => {
      tallExpander.IsExpanded = false;
      wideExpander.IsExpanded = false;
      bigExpander.IsExpanded = false;
    };
    tallExpander.Expanded += (s, e) => {
      smallExpander.IsExpanded = false;
      wideExpander.IsExpanded = false;
      bigExpander.IsExpanded = false;
    };
    wideExpander.Expanded += (s, e) => {
      smallExpander.IsExpanded = false;
      tallExpander.IsExpanded = false;
      bigExpander.IsExpanded = false;
    };
    bigExpander.Expanded += (s, e) => {
      smallExpander.IsExpanded = false;
      tallExpander.IsExpanded = false;
      wideExpander.IsExpanded = false;
    };
  }
}
