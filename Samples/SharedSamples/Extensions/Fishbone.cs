/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Layouts.Extensions;
using Northwoods.Go.Models;

namespace Demo.Extensions.Fishbone {
  public partial class Fishbone : DemoControl {
    private Diagram _Diagram;

    public Fishbone() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      fishboneBtn.Click += (s, e) => LayoutFishbone();
      branchingBtn.Click += (s, e) => LayoutBranching();
      normalBtn.Click += (s, e) => LayoutNormal();

      desc1.MdText = DescriptionReader.Read("Extensions.Fishbone.md");
    }

    private void Setup() {
      _Diagram.IsReadOnly = true;

      _Diagram.NodeTemplate =
        new Node()
          .Add(
            new TextBlock()
              .Bind("Text", "Key")
              .Bind("Font")
          );

      // This demo switches the Diagram.linkTemplate between the "normal" and the "fishbone" templates.
      // If you are only doing a FishboneLayout, you could just set Diagram.linkTemplate
      // to the template named "fishbone" here, and not switch templates dynamically.

      // define the non-fishbone link template
      _Diagram.LinkTemplateMap.Add("normal",
        new Link {
          Routing = LinkRouting.Orthogonal,
          Corner = 4
        }.Add(new Shape())
      );

      _Diagram.LinkTemplateMap.Add("fishbone",
        new FishboneLink().Add(new Shape())
      );

      var rootFont = new Font("Microsoft Sans Serif", 18, Northwoods.Go.FontWeight.Bold);
      var hdrFont = new Font("Microsoft Sans Serif", 14, Northwoods.Go.FontWeight.Bold);
      var hdrFontSm = new Font("Microsoft Sans Serif", 13, Northwoods.Go.FontWeight.Bold);
      var nodeFont = new Font("Microsoft Sans Serif", 13);

      _Diagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = "Incorrect Deliveries", Font = rootFont },

          new NodeData { Key = "Skills", Font = hdrFont, Parent = "Incorrect Deliveries" },
          new NodeData { Key = "knowledge", Font = nodeFont, Parent = "Skills" },
          new NodeData { Key = "literacy", Font = hdrFontSm, Parent = "Skills" },
          new NodeData { Key = "documentation", Font = nodeFont, Parent = "knowledge" },
          new NodeData { Key = "products", Font = nodeFont, Parent = "knowledge" },

          new NodeData { Key = "Procedures", Font = hdrFont, Parent = "Incorrect Deliveries" },
          new NodeData { Key = "manual", Font = hdrFontSm, Parent = "Procedures" },
          new NodeData { Key = "consistency", Font = nodeFont, Parent = "manual" },
          new NodeData { Key = "automated", Font = hdrFontSm, Parent = "Procedures" },
          new NodeData { Key = "correctness", Font = nodeFont, Parent = "automated" },
          new NodeData { Key = "reliability", Font = nodeFont, Parent = "automated" },

          new NodeData { Key = "Communication", Font = hdrFont, Parent = "Incorrect Deliveries" },
          new NodeData { Key = "ambiguity", Font = hdrFontSm, Parent = "Communication" },
          new NodeData { Key = "sales staff", Font = hdrFontSm, Parent = "Communication" },
          new NodeData { Key = "telephone orders", Font = hdrFontSm, Parent = "Communication" },
          new NodeData { Key = "picking slips", Font = hdrFontSm, Parent = "Communication" },
          new NodeData { Key = "order details", Font = nodeFont, Parent = "sales staff" },
          new NodeData { Key = "lack of knowledge", Font = nodeFont, Parent = "order details" },
          new NodeData { Key = "lack of information", Font = nodeFont, Parent = "telephone orders" },
          new NodeData { Key = "details", Font = nodeFont, Parent = "picking slips" },
          new NodeData { Key = "legibility", Font = nodeFont, Parent = "picking slips" },

          new NodeData { Key = "Transport", Font = hdrFont, Parent = "Incorrect Deliveries" },
          new NodeData { Key = "information", Font = hdrFontSm, Parent = "Transport" },
          new NodeData { Key = "carriers", Font = hdrFontSm, Parent = "Transport" },
          new NodeData { Key = "incorrect person", Font = nodeFont, Parent = "information" },
          new NodeData { Key = "incorrect addresses", Font = nodeFont, Parent = "information" },
          new NodeData { Key = "customer data base", Font = nodeFont, Parent = "incorrect addresses" },
          new NodeData { Key = "incorrect dept", Font = nodeFont, Parent = "information" },
          new NodeData { Key = "efficiency", Font = nodeFont, Parent = "carriers" },
          new NodeData { Key = "methods", Font = nodeFont, Parent = "carriers" },
          new NodeData { Key = "not up-to-date", Font = nodeFont, Parent = "customer data base" },
          new NodeData { Key = "incorrect program", Font = nodeFont, Parent = "customer data base" }
        }
      };

      _Diagram.LinkTemplate = _Diagram.LinkTemplateMap["fishbone"];
      _Diagram.Layout = new FishboneLayout() {
        Angle = 180,
        LayerSpacing = 10,
        NodeSpacing = 20,
        RowSpacing = 10
      };
    }

    private void LayoutFishbone() {
      if (_Diagram == null) return;

      _Diagram.StartTransaction("fishbone layout");
      _Diagram.LinkTemplate = _Diagram.LinkTemplateMap["fishbone"];
      _Diagram.Layout = new FishboneLayout {
        Angle = 180,
        LayerSpacing = 10,
        NodeSpacing = 20,
        RowSpacing = 10
      };
      _Diagram.CommitTransaction("fishbone layout");
    }

    private void LayoutNormal() {
      if (_Diagram == null) return;

      _Diagram.StartTransaction("tree layout");
      _Diagram.LinkTemplate = _Diagram.LinkTemplateMap["normal"];
      _Diagram.Layout = new TreeLayout() {
        Angle = 180,
        BreadthLimit = 1000,
        Alignment = TreeAlignment.Start
      };
      _Diagram.CommitTransaction("tree layout");
    }

    private void LayoutBranching() {
      if (_Diagram == null) return;

      _Diagram.StartTransaction("branching layout");
      _Diagram.LinkTemplate = _Diagram.LinkTemplateMap["normal"];
      _Diagram.Layout = new TreeLayout() {
        Angle = 180,
        LayerSpacing = 20,
        Alignment = TreeAlignment.BusBranching
      };
      _Diagram.CommitTransaction("branching layout");
    }
  }

  public class Model : TreeModel<NodeData, string, object> { }

  public class NodeData : Model.NodeData {
    public Font Font { get; set; } = new();
  }
}
