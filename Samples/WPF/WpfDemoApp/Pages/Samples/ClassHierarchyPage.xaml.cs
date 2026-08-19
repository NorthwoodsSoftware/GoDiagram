/* Copyright (c) Northwoods Software Corporation. */

using Northwoods.Go;

namespace Demo.Samples.ClassHierarchy; 
public partial class ClassHierarchy : DemoControl {
  // This is a stub for the designer.
  // See the SharedSamples project for sample implementation.

  private static Dictionary<string, string> _ApiMap = WpfDemoApp.Views.DescriptionView.ApiMap;

  private string linkfunc(object node) {
    var data = (node as Node).Data as NodeData;
    if (_ApiMap != null && _ApiMap.TryGetValue(data.Key, out var url))
      return "https://godiagram.com/wpf/latest/" + url;
    return "https://godiagram.com/wpf/latest/api/";
  }
}
