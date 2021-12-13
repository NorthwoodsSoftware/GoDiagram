using System;
using System.Linq;
using System.Windows.Forms;

using Northwoods.Go;

namespace WinFormsDemoApp {
  internal static class Program {
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args) {
      var openTo = (DemoType.Sample, "OrgChartEditor");  // open to Org Chart Editor by default
      // read only the first arg, which will direct to a particular sample
      if (args.Length > 0) {
        openTo = ProcessInput(args[0]);
      }

      Diagram.ResourceManager = WinFormsSampleControls.Properties.Resources.ResourceManager;

#if NET5_0_OR_GREATER
      Application.SetHighDpiMode(HighDpiMode.SystemAware);
#endif
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new App(openTo));
    }

    public static (DemoType, string) ProcessInput(string s) {
      s = s.Substring(s.IndexOf(':') + 1);
      // unfortunately, original keys must be iterated to match given string
      // https://stackoverflow.com/questions/1619090/getting-a-keyvaluepair-directly-from-a-dictionary
      if (DemoIndex.Samples.ContainsKey(s)) {
        s = DemoIndex.Samples.First(kvp => DemoIndex.Samples.Comparer.Equals(kvp.Key, s)).Key;
        return (DemoType.Sample, s);
      } else if (DemoIndex.Extensions.ContainsKey(s)) {
        s = DemoIndex.Extensions.First(kvp => DemoIndex.Extensions.Comparer.Equals(kvp.Key, s)).Key;
        return (DemoType.Extension, s);
      }
      return (DemoType.Sample, "OrgChartEditor");  // unknown input argument? use OrgChartEditor sample
    }
  }
}
