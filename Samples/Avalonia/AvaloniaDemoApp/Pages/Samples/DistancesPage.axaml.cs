/* Copyright 1998-2023 by Northwoods Software Corporation. */

using Avalonia;
using System.Collections.ObjectModel;

namespace Demo.Samples.Distances {
  public partial class Distances : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    public static readonly StyledProperty<ObservableCollection<string>> PathsProperty =
      AvaloniaProperty.Register<Distances, ObservableCollection<string>>(nameof(Paths));

    public ObservableCollection<string> Paths {
      get { return GetValue(PathsProperty); }
      set {
        SetValue(PathsProperty, value);
      }
    }

    private void _RebuildList() {
      Paths = myPaths;
    }
  }
}
