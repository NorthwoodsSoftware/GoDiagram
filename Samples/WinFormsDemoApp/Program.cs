using System;
using System.Windows.Forms;

using Northwoods.Go;

namespace WinFormsDemoApp {
  internal static class Program {
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args) {
      var openTo = (DemoType.Sample, "OrgChartStatic");  // open to Org Chart Static by default
      // read only the first arg, which will direct to a particular sample
      if (args.Length > 0) {
        openTo = ProcessInput(args[0]);
      }

      Diagram.ResourceManager = WinFormsSampleControls.Properties.Resources.ResourceManager;
      Diagram.LicenseKey = "eolaYRvpVthgXeXXAdW7IEDU+nB4nyBMz/z//CSrBv1A/Ac9fjdl2YyAwe6PFLBvcg3XIrf1om5+L4OQnP7JAGQUlQJw/dcyNz8irG8jnIAM2TtDLGbPRnH7MKPdl0UW";

#if NET5_0_OR_GREATER
      Application.SetHighDpiMode(HighDpiMode.SystemAware);
#endif
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new App(openTo));
    }

    static (DemoType, string) ProcessInput(string s) {
      s = s.Substring(s.IndexOf(':') + 1);
      if (DemoIndex.Samples.ContainsKey(s)) {
        return (DemoType.Sample, s);
      } else if (DemoIndex.Extensions.ContainsKey(s)) {
        return (DemoType.Extension, s);
      }
      return (DemoType.Sample, "OrgChartStatic");  // unknown input argument? use OrgChartStatic sample
    }
  }
}
