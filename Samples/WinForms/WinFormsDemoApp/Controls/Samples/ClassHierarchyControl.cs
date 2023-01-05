/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;

namespace Demo.Samples.ClassHierarchy {
  [ToolboxItem(false)]
  public partial class ClassHierarchy : DemoControl {
    // This is a stub for the designer.
    // See the SharedSamples project for sample implementation.

    private static Dictionary<string, string> _ApiMap = WinFormsDemoApp.GoWebBrowser.ApiMap;

    private string linkfunc(object node) {
      var data = (node as Node).Data as NodeData;
      if (_ApiMap != null && _ApiMap.TryGetValue(data.Key, out var url))
        return "https://godiagram.com/winforms/latest/" + url;
      return "https://godiagram.com/winforms/latest/api/";
    }
  }
}
