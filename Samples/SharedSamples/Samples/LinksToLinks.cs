/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace Demo.Samples.LinksToLinks {
  public partial class LinksToLinks : DemoControl {
    private Diagram _Diagram;

    public LinksToLinks() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      modelJson1.SaveClick = SaveModel;
      modelJson1.LoadClick = LoadModel;

      modelJson1.JsonText = @"{
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

      desc1.MdText = DescriptionReader.Read("Samples.LinksToLinks.md");

      Setup();
    }

    private void Setup() {
      _Diagram.LinkDrawn += MaybeChangeLinkCategory;
      _Diagram.LinkRelinked += MaybeChangeLinkCategory;
      _Diagram.UndoManager.IsEnabled = true;

      // the regular node template
      _Diagram.NodeTemplate =
        new Node(PanelType.Auto) {
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
      if (_Diagram.NodeTemplateMap.ContainsKey("LinkLabel")) {
        _Diagram.NodeTemplateMap.Remove("LinkLabel");
      }
      _Diagram.NodeTemplateMap.Add("LinkLabel",
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
      _Diagram.LinkTemplate =
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

      _Diagram.LinkTemplateMap.Add("linkToLink",
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
      if (_Diagram == null) return;
      _Diagram.Model = Model.FromJson<Model>(modelJson1.JsonText);
      _Diagram.Model.UndoManager.IsEnabled = true;
    }

    private void SaveModel() {
      if (_Diagram == null) return;
      modelJson1.JsonText = _Diagram.Model.ToJson();
    }
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
