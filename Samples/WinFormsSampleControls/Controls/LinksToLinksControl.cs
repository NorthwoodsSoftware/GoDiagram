using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.LinksToLinks {
  [ToolboxItem(false)]
  public partial class LinksToLinksControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    public LinksToLinksControl() {
      InitializeComponent();

      saveLoadModel1.SaveClick += (e, obj) => SaveModel();
      saveLoadModel1.LoadClick += (e, obj) => LoadModel();

      goWebBrowser1.Html = @"
    <p>
      This demonstrates the ability for a <a>Link</a> to appear to connect with another Link.
      Regular links are blue.  Link-connecting links are green.
      Try moving a node around to see how the links adapt.
      Initially the ""Alpha"" node connects with the link between Gamma and Delta.
      There is also a link between the two horizontal links.
    </p>
    <p>
      This effect is achieved by using ""label nodes"" that belong to links.
      Such ""label nodes"" are real <a>Node</a>s that are referenced from their owning <a>Link</a>.
      This sample customizes the ""Link Label"" Node template to allow the user to draw new links to/from such label nodes.
    </p>
    <p>
      Newly drawn links automatically get a label node by the <a>LinkingTool</a> because this sample initializes
      the <a>LinkingTool.ArchetypeLabelNodeData</a> property of the <a>ToolManager.LinkingTool</a>.
      The category (i.e. template) of each link is determined by what kinds of nodes the link is connected with.
    </p>
";
      saveLoadModel1.ModelJson = @"{
  ""LinkLabelKeysProperty"": ""LabelKeys"",
  ""NodeDataSource"": [
{ ""Key"": ""Alpha"", ""Color"": ""lightblue"", ""Loc"": ""29 14"" },
{ ""Key"": ""Beta"", ""Color"": ""orange"", ""Loc"": ""129 14"" },
{ ""Key"": ""Gamma"", ""Color"": ""lightgreen"", ""Loc"": ""29 106"" },
{ ""Key"": ""Delta"", ""Color"": ""pink"", ""Loc"": ""129 106"" },
{ ""Key"": ""A-B"", ""Category"": ""LinkLabel"" },
{ ""Key"": ""G-D"", ""Category"": ""LinkLabel"" },
{ ""Key"": ""A-G"", ""Category"": ""LinkLabel"" },
{ ""Key"": ""A-G-D"", ""Category"": ""LinkLabel"" },
{ ""Key"": ""A-B-G-D"", ""Category"": ""LinkLabel"" }
 ],
  ""LinkDataSource"": [
{ ""From"": ""Alpha"", ""To"": ""Beta"", ""LabelKeys"": [ ""A-B"" ] },
{ ""From"": ""Gamma"", ""To"": ""Delta"", ""LabelKeys"": [ ""G-D"" ] },
{ ""From"": ""Alpha"", ""To"": ""Gamma"", ""LabelKeys"": [ ""A-G"" ] },
{ ""From"": ""Alpha"", ""To"": ""G-D"", ""LabelKeys"": [ ""A-G-D"" ], ""Category"": ""linkToLink"" },
{ ""From"": ""A-B"", ""To"": ""G-D"", ""LabelKeys"": [ ""A-B-G-D"" ], ""Category"": ""linkToLink"" }
 ]}";

      Setup();
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.LinkDrawn += MaybeChangeLinkCategory;
      myDiagram.LinkRelinked += MaybeChangeLinkCategory;
      myDiagram.UndoManager.IsEnabled = true;

      // the regular node template
      myDiagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance) {
          LocationSpot = Spot.Center,
          LayerName = "Background"  // always have regular nodes behing links
        }.Bind(new Binding("Location", "Loc", Point.Parse).MakeTwoWay(Point.Stringify))
        .Add(
          new Shape {
            Figure = "RoundedRectangle",
            Fill = "white", Stroke = (Brush)null,
            PortId = "", FromLinkable = true, ToLinkable = true, Cursor = "pointer"
          }.Bind(
            new Binding("Fill", "Color")
          ),
          new TextBlock {
            Margin = 8 // make some extra space for the space around the text
          }.Bind(
            new Binding("Text", "Key") // the label shows the node data's key
          )
        );

      // this is the template for a label node on a link: just an Ellipse
      // this node supports user-drawn links to and from the label node
      if (myDiagram.NodeTemplateMap.ContainsKey("LinkLabel")) {
        myDiagram.NodeTemplateMap.Remove("LinkLabel");
      }
      myDiagram.NodeTemplateMap.Add("LinkLabel",
        new Node {
          Selectable = false, Avoidable = false,
          LayerName = "Foreground"
        }.Add(
          new Shape {
            Figure = "Ellipse",
            Width = 5, Height = 5, Stroke = (Brush)null,
            PortId = "", FromLinkable = true, ToLinkable = true, Cursor = "pointer"
          }
        )
      );

      // the regular link template, a straight blue arrow
      myDiagram.LinkTemplate =
        new Link {
          RelinkableFrom = true,
          RelinkableTo = true,
          ToShortLength = 2
        }.Add(
          new Shape {
            Stroke = "#2E68CC", StrokeWidth = 2
          },
          new Shape {
            Fill = "#2E68CC", Stroke = (Brush)null, ToArrow = "Standard"
          }
        );

      myDiagram.LinkTemplateMap.Add("linkToLink",
        new Link {
          RelinkableFrom = true, RelinkableTo = true
        }.Add(
          new Shape {
            Stroke = "#2D9945", StrokeWidth = 2
          }
        )
      );

      LoadModel();

      void MaybeChangeLinkCategory(object s, DiagramEvent e) {
        var link = e.Subject as Link;
        var linktolink = (link.FromNode.IsLinkLabel || link.ToNode.IsLinkLabel);
        (e.Diagram.Model as Model).SetCategoryForLinkData(link.Data as LinkData, (linktolink ? "linkToLink" : ""));
      }
    }

    private void LoadModel() {
      if (myDiagram == null) return;
      myDiagram.Model = Model.FromJson<Model>(saveLoadModel1.ModelJson);
      myDiagram.Model.UndoManager.IsEnabled = true;
    }

    private void SaveModel() {
      if (myDiagram == null) return;
      saveLoadModel1.ModelJson = myDiagram.Model.ToJson();
    }

    // define the model data
    public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
    public class NodeData : Model.NodeData {
      public string Color { get; set; }
      public string Loc { get; set; }
    }

    public class LinkData : Model.LinkData {
      public List<string> LabelKeys { get; set; }
    }
  }
}
