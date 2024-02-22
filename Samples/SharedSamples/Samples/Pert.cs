/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace Demo.Samples.Pert {
  public partial class Pert : DemoControl {
    private Diagram _Diagram;

    public Pert() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      desc1.MdText = DescriptionReader.Read("Samples.Pert.md");
    }

    private void Setup() {
      // colors used, named for easier identification
      var blue = "#0288D1";
      var pink = "#B71C1C";
      var pinkfill = "#F8BBD0";
      var bluefill = "#B3E5FC";

      _Diagram.InitialAutoScale = AutoScale.Uniform;
      _Diagram.Layout = new LayeredDigraphLayout();

      // The node template shows the activity name in the middle as well as
      // various statistics about the activity, all surrounded by a border.
      // The border's color is determined by the node data's ".Critical" property.
      // Some information is not available as properties on the node data,
      // but must be computed -- we use converter functions for that.
      _Diagram.NodeTemplate = new Node(PanelType.Auto).Add(
        new Shape { // the border
          Figure = "Rectangle",
          Fill = "white",
          StrokeWidth = 2
        }.Bind(
          new Binding("Fill", "Critical", (b, _) => (bool)b ? pinkfill : bluefill),
          new Binding("Stroke", "Critical", (b, _) => (bool)b ? pink : blue)
        ),
        new Panel(PanelType.Table) {
          Padding = 0.5
        }
        .Add(
          new ColumnDefinition { Column = 1, SeparatorStroke = "black" },
          new ColumnDefinition { Column = 2, SeparatorStroke = "black" })
        .Add(
          new RowDefinition { Row = 1, SeparatorStroke = "black", Background = "white", CoversSeparators = true },
          new RowDefinition { Row = 2, SeparatorStroke = "black" })
        .Add(
          new TextBlock { // earlyStart
            Row = 0, Column = 0, Margin = 5, TextAlign = TextAlign.Center
          }.Bind("Text", "EarlyStart", (s, _) => ((double)s).ToString("0.##")),
          new TextBlock {
            Row = 0, Column = 1, Margin = 5, TextAlign = TextAlign.Center
          }.Bind("Text", "Length", (s, _) => ((double)s).ToString("0.##")),
          new TextBlock {
            Row = 0, Column = 2, Margin = 5, TextAlign = TextAlign.Center
          }.Bind("Text", "", (d, _) => ((d as NodeData).EarlyStart + (d as NodeData).Length).ToString("0.##")),
          new TextBlock {
            Row = 1, Column = 0, ColumnSpan = 3, Margin = 5,
            TextAlign = TextAlign.Center, Font = new Font("Segoe UI", 14, Northwoods.Go.FontWeight.Bold)
          }.Bind("Text", "Text"),
          new TextBlock { // late start
            Row = 2, Column = 0, Margin = 5, TextAlign = TextAlign.Center
          }.Bind("Text", "", (d, _) => ((d as NodeData).LateFinish - (d as NodeData).Length).ToString("0.##")),
          new TextBlock { // slack
            Row = 2, Column = 1, Margin = 5, TextAlign = TextAlign.Center
          }.Bind("Text", "", (d, _) => ((d as NodeData).LateFinish - ((d as NodeData).EarlyStart + (d as NodeData).Length)).ToString("0.##")),
          new TextBlock { // late finish
            Row = 2, Column = 2, Margin = 5, TextAlign = TextAlign.Center
          }.Bind("Text", "LateFinish", (s, _) => ((double)s).ToString("0.##"))
        )
      );

      // The link data object does not have direct access to both nodes
      // (although it does have references to their keys: .from and .to).
      // This conversion function gets the GraphObject that was data-bound as the second argument.
      // From that we can get the containing Link, and then the Link.fromNode or .toNode,
      // and then its node data, which has the ".critical" property we need.
      //
      // But note that if we were to dynamically change the ".critical" property on a node data,
      // calling myDiagram.model.updateTargetBindings(nodedata) would only update the color
      // of the nodes.  It would be insufficient to change the appearance of any Links.
      string linkColorConverter(object linkdata, object elt) {
        var link = (elt as Shape).Part;
        if (link == null) return blue;
        var f = (link as Link).FromNode;
        if (f == null || f.Data == null || !(f.Data as NodeData).Critical) return blue;
        var t = (link as Link).ToNode;
        if (t == null || t.Data == null | !(t.Data as NodeData).Critical) return blue;
        return pink; // when both Link.FromNode.Data.Critical and Link.ToNode.Data.Critical
      }

      // The color of a link (including its arrowhead) is red only when both
      // connected nodes have data that is ".Critical"; otherwise it is blue.
      // This is computed by the binding converter function.
      _Diagram.LinkTemplate = new Link {
        ToShortLength = 6,
        ToEndSegmentLength = 20
      }.Add(
        new Shape {
          StrokeWidth = 4
        }.Bind("Stroke", "", linkColorConverter),
        new Shape { // arrowhead
          ToArrow = "Triangle", Stroke = null, Scale = 1.5
        }.Bind("Fill", "", linkColorConverter)
      );

      _Diagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = 1, Text = "Start", Length = 0, EarlyStart = 0, LateFinish =  0, Critical = true },
          new NodeData { Key = 2, Text = "a", Length = 4, EarlyStart = 0, LateFinish = 4, Critical = true },
          new NodeData { Key = 3, Text = "b", Length = 5.33, EarlyStart = 0, LateFinish = 9.17, Critical = false },
          new NodeData { Key = 4, Text = "c", Length = 5.17, EarlyStart = 4, LateFinish = 9.17, Critical = true },
          new NodeData { Key = 5, Text = "d", Length = 6.33, EarlyStart = 4, LateFinish = 15.01, Critical = false },
          new NodeData { Key = 6, Text = "e", Length = 5.17, EarlyStart = 9.17, LateFinish = 14.34, Critical = true },
          new NodeData { Key = 7, Text = "f", Length = 4.5, EarlyStart = 10.33, LateFinish = 19.51, Critical = false },
          new NodeData { Key = 8, Text = "g", Length = 5.17, EarlyStart = 14.34, LateFinish = 19.51, Critical = true },
          new NodeData { Key = 9, Text = "Finish", Length = 0, EarlyStart = 19.51, LateFinish = 19.51, Critical = true }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = 1, To = 2 },
          new LinkData { From = 1, To = 3 },
          new LinkData { From = 2, To = 4 },
          new LinkData { From = 2, To = 5 },
          new LinkData { From = 3, To = 6 },
          new LinkData { From = 4, To = 6 },
          new LinkData { From = 5, To = 7 },
          new LinkData { From = 6, To = 8 },
          new LinkData { From = 7, To = 9 },
          new LinkData { From = 8, To = 9 }
        }
      };

      // create an unbound Part that acts as a legend for the diagram
      _Diagram.Add(new Node(PanelType.Auto).Add(
        new Shape { // the border
          Figure = "Rectangle",
          Fill = bluefill
        },
        new Panel(PanelType.Table)
        .Add(
          new ColumnDefinition { Column = 1, SeparatorStroke = "black" },
          new ColumnDefinition { Column = 2, SeparatorStroke = "black" })
        .Add(
          new RowDefinition { Row = 1, SeparatorStroke = "black", Background = bluefill, CoversSeparators = true },
          new RowDefinition { Row = 2, SeparatorStroke = "black" })
        .Add(
          new TextBlock {
            Text = "Early Start",
            Row = 0, Column = 0, Margin = 5, TextAlign = TextAlign.Center
          },
          new TextBlock {
            Text = "Length",
            Row = 0, Column = 1, Margin = 5, TextAlign = TextAlign.Center
          },
          new TextBlock {
            Text = "Early Finish",
            Row = 0, Column = 2, Margin = 5, TextAlign = TextAlign.Center
          },
          new TextBlock {
            Text = "Activity Name",
            Row = 1, Column = 0, ColumnSpan = 3, Margin = 5,
            TextAlign = TextAlign.Center, Font = new Font("Segoe UI", 14, Northwoods.Go.FontWeight.Bold)
          },
          new TextBlock {
            Text = "Late Start",
            Row = 2, Column = 0, Margin = 5, TextAlign = TextAlign.Center
          },
          new TextBlock {
            Text = "Slack",
            Row = 2, Column = 1, Margin = 5, TextAlign = TextAlign.Center
          },
          new TextBlock {
            Text = "Late Finish",
            Row = 2, Column = 2, Margin = 5, TextAlign = TextAlign.Center
          }
        )
      ));
    }
  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }
  public class NodeData : Model.NodeData {
    public double EarlyStart { get; set; }
    public double LateFinish { get; set; }
    public double Length { get; set; }
    public bool Critical { get; set; }
  }
  public class LinkData : Model.LinkData { }
}
