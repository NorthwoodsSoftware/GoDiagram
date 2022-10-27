/*
*  Copyright (C) 1998-2022 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main GoDiagram library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in the GoDiagram repository (https://github.com/NorthwoodsSoftware/GoDiagram/tree/main/Extensions).
* See the Extensions intro page (https://godiagram.com/intro/extensions.html) for more information.
*/

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Northwoods.Go.Extensions {
  public partial class HyperlinkText {
    /// <summary>
    /// Defines the platform-specific click handler for hyperlinks.
    /// </summary>
    public static void Click(InputEvent e, GraphObject obj) {
      var u = obj["_Url"];
      if (u is Func<object, string>) u = (u as Func<object, string>).Invoke(obj.FindBindingPanel());
      if (u is string uri) {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
          Process.Start(new ProcessStartInfo(uri) {
            UseShellExecute = true,
            Verb = "open"
          });
        } else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
          Process.Start("xdg-open", uri);
        } else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
          Process.Start("open", uri);
        }
      }
    }
  }
}
