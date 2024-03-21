# GoDiagram — .NET Library for Interactive Diagrams

[GoDiagram](https://godiagram.com) is a .NET library for creating interactive diagrams, charts, and graphs. It is based on the [GoJS](https://gojs.net) JavaScript diagramming library, also from Northwoods Software.

[![open issues](https://img.shields.io/github/issues-raw/NorthwoodsSoftware/GoDiagram.svg)](https://github.com/NorthwoodsSoftware/GoDiagram/issues)
[![Twitter Follow](https://img.shields.io/twitter/follow/NorthwoodsGo.svg?style=social&label=Follow)](https://twitter.com/NorthwoodsGo)

[Get Started with GoDiagram](https://godiagram.com/winforms/latest/learn)

GoDiagram is a flexible library that can be used to create a number of different kinds of interactive diagrams, including data visualizations, drawing tools, and graph editors. GoDiagram includes a number of built in layouts including tree layout, force directed, radial, and layered digraph layout, and a number of custom layout examples.

Read more about GoDiagram at [godiagram.com](https://godiagram.com).

[This repository](https://github.com/NorthwoodsSoftware/GoDiagram) contains the sources for all samples and extensions.
You can use the GitHub repository to quickly [search through the sample sources](https://github.com/NorthwoodsSoftware/GoDiagram/search?q=FindNodeDataForKey&type=Code).

In your project, we recommend referencing the library via NuGet as it will more reliably add toolbox items and necessary references:

- [WinForms](https://www.nuget.org/packages/Northwoods.GoDiagram.WinForms)
- [Avalonia](https://www.nuget.org/packages/Northwoods.GoDiagram.Avalonia)

## Minimal Sample

Graphs are constructed by creating one or more templates, with desired properties data-bound, and adding model data.

```cs
  ...

  private void Setup() {
    _Diagram = diagramControl1.Diagram;

    _Diagram.UndoManager.IsEnabled = true;  // enable undo & redo

    // define a simple Node template
    _Diagram.NodeTemplate =
      new Node("Auto")  // the Shape will go around the TextBlock
        .Add(
          new Shape("RoundedRectangle") {
              StrokeWidth = 0,  // no border
              Fill = "white"  // default fill is white
            }
            // Shape.Fill is bound to Node.Data.Color
            .Bind("Fill", "Color"),
          new TextBlock {
              Margin = 8, // some room around the text
              Font = new Font("Segoe UI", 14, FontWeight.Bold),
              Stroke = "#333"
            }
            // TextBlock.Text is bound to Node.Data.Key
            .Bind("Text", "Key")
        );

    // but use the default Link template, by not setting Diagram.LinkTemplate

    // create the model data that will be represented by Nodes and Links
    _Diagram.Model = new Model {
      NodeDataSource = new List<NodeData> {
        new NodeData { Key = "Alpha", Color = "lightblue" },
        new NodeData { Key = "Beta", Color = "orange" },
        new NodeData { Key = "Gamma", Color = "lightgreen" },
        new NodeData { Key = "Delta", Color = "pink" }
      },
      LinkDataSource = new List<LinkData> {
        new LinkData { From = "Alpha", To = "Beta" },
        new LinkData { From = "Alpha", To = "Gamma" },
        new LinkData { From = "Beta", To = "Beta" },
        new LinkData { From = "Gamma", To = "Delta" },
        new LinkData { From = "Delta", To = "Alpha" }
      }
    };
  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Color { get; set; }
  }
  public class LinkData : Model.LinkData { }

  ...
```

The above diagram and model code creates the following graph.

![minimal](https://github.com/NorthwoodsSoftware/GoDiagram/assets/1202288/6182994a-15e7-4075-822f-15b561e6ad58)

## License

The GoDiagram [software license](https://godiagram.com/license.html).

Copyright (c) 2024 Northwoods Software Corporation
