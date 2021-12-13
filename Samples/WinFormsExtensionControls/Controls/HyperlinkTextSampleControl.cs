using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Extensions;
using Northwoods.Go.Models;

namespace WinFormsExtensionControls.HyperlinkTextSample {
  [ToolboxItem(false)]
  public partial class HyperlinkTextSampleControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    
    public HyperlinkTextSampleControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      goWebBrowser1.Html = @"
  <p>
    This uses the ""HyperlinkText"" builder defined in <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/GraphObjects/HyperlinkText/HyperlinkTextWinForms/HyperlinkText.cs"">HyperlinkText.cs</a>.
  </p>
  <p>
    Click on the text to open a window to a computed URL.
    A mouse-over on the text will underline the text.
    Hover on the text and you will see a tooltip showing the destination URL.
  </p>
";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      HyperlinkText.DefineBuilders();

      Func<object, string> func1 = (node) => {
        return "https://gojs.net/" + ((node as Node).Data as NodeData).Version;
      };
      Func<object, string> func2 = (node) => {
        return "Visit GoJS " + ((node as Node).Data as NodeData).Version;
      };
      TextBlock hyperlinkObj =
        Builder.Make<TextBlock>("HyperlinkText", new object[] {
          func1,
          func2
        });
      hyperlinkObj.Margin = 10;


      // node template
      myDiagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance).Add(
          new Shape {
            Figure = "Ellipse", Fill = "lightskyblue"
          },
          hyperlinkObj
        );

      // model data
      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData {Key = 1, Version = "beta"},
          new NodeData {Key = 2, Version = "latest"}
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = 1, To = 2}
        }
      };
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Version { get; set; }
  }

  public class LinkData : Model.LinkData { }

}
