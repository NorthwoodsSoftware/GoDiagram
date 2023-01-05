/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Extensions;
using Northwoods.Go.Models;

namespace Demo.Extensions.Hyperlink {
  public partial class Hyperlink : DemoControl {
    private Diagram _Diagram;

    public Hyperlink() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      desc1.MdText = DescriptionReader.Read("Extensions.Hyperlink.md");
    }

    private void Setup() {
      HyperlinkText.DefineBuilders();

      Func<object, string> func1 = (node) => {
        return "https://godiagram.com/winforms/" + ((node as Node).Data as NodeData).Version;
      };
      Func<object, string> func2 = (node) => {
        return "Visit GoDiagram " + ((node as Node).Data as NodeData).Version;
      };
      TextBlock hyperlinkObj =
        Builder.Make<TextBlock>("HyperlinkText", new object[] {
          func1,
          func2
        });
      hyperlinkObj.Margin = 10;

      // node template
      _Diagram.NodeTemplate =
        new Node(PanelType.Auto).Add(
          new Shape {
            Figure = "Ellipse", Fill = "lightskyblue"
          },
          hyperlinkObj
        );

      // model data
      _Diagram.Model = new Model {
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
