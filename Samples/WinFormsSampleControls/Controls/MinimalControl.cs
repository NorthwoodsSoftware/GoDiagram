using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.Minimal {
  [ToolboxItem(false)]
  public partial class MinimalControl : UserControl {
    private Diagram _Diagram;

    public MinimalControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;
      goWebBrowser1.Html = @"
        <p>
          This isn't a truly <i>minimal</i> demonstration of <b>GoDiagram</b>,
          because we do specify a custom Node template, but it's pretty simple.
          The whole source for the sample is shown below if you click on the link.
        </p>
        <p>
          This sample sets the <a>Diagram.NodeTemplate</a>, with a <a>Node</a> template that data binds both the text string and the shape's fill color.
          For an overview of building your own templates and model data, see the <a href=""learn/index.html"">Getting Started tutorial.</a>
        </p>
        <p>
          Using the mouse and common keyboard commands, you can pan, select, move, copy, delete, and undo/redo.
          On touch devices, use your finger to act as the mouse, and hold your finger stationary to bring up a context menu.
          The default context menu supports most of the standard commands that
          are enabled at that time for the selected object.
        </p>
        <p>
          For a more elaborate and capable sample, see the <a href=""Basic"">Basic</a> sample.
        </p>
      ";
    }

    private void Setup() {
      _Diagram = diagramControl1.Diagram;

      // diagram properties
      _Diagram.UndoManager.IsEnabled = true;  // enable undo & redo

      // define a simple Node template
      _Diagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance)  // the Shape will go around the TextBlock
          .Add(
            new Shape("RoundedRectangle") { StrokeWidth = 0 }
              .Bind("Fill", "Color"),
            new TextBlock {
                Font = new Font("Segoe UI", 14, FontWeight.Bold), Stroke = "#333", Margin = 8, // Specify a margin to add some room around the text
                Editable = true
              }
              .Bind("Text")
          );

      // but use the default Link template, by not setting Diagram.LinkTemplate

      // create the model data that will be represented by Nodes and Links
      _Diagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "n0", Text = "Alpha", Color = "lightblue" },
          new NodeData { Key = "n1", Text = "Beta", Color = "orange" },
          new NodeData { Key = "n2", Text = "Gamma", Color = "lightgreen" },
          new NodeData { Key = "n3", Text = "Delta", Color = "pink" }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = "n0", To = "n1" },
          new LinkData { From = "n0", To = "n2" },
          new LinkData { From = "n1", To = "n1" },
          new LinkData { From = "n2", To = "n3" },
          new LinkData { From = "n3", To = "n0" }
        }
      };
    }
  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Color { get; set; }
  }

  public class LinkData : Model.LinkData { }
}
