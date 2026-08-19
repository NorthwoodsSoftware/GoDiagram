/* Copyright (c) Northwoods Software Corporation. */

using System.Collections.ObjectModel;
using System.Windows;

namespace Demo.Samples.Distances; 
public partial class Distances : DemoControl {
  // This is a stub for the designer.
  // See the SharedSamples project for sample implementation.

  public static readonly DependencyProperty PathsProperty =
    DependencyProperty.Register(nameof(Paths), typeof(ObservableCollection<string>), typeof(Distances));

  public ObservableCollection<string> Paths {
    get { return (ObservableCollection<string>)GetValue(PathsProperty); }
    set { SetValue(PathsProperty, value); }
  }

  private void _RebuildList() {
    Paths = myPaths;
  }
}
