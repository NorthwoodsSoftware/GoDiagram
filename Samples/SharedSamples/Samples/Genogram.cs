/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;

namespace Demo.Samples.Genogram {
  public partial class Genogram : DemoControl {
    private Diagram _Diagram;

    public Genogram() {

      InitializeComponent();

      _Diagram = diagramControl1.Diagram;
      Setup();
      desc1.MdText = DescriptionReader.Read("Samples.Genogram.md");

      // Display example code in textbox
      textBox1.Text = @"[
        { Key: -1, n: ""Aaron"", s: ""M"", m: -10, f: -11, ux: 1, a:[""C"", ""F"", ""K""] },
        { Key: 1, n: ""Alice"", s: ""F"", m: -12, f: -13, a:[""B"", ""H"", ""K""] },
        { Key: 2, n: ""Bob"", s: ""M"", m: 1, f: -1, ux: 3, a:[""C"", ""H"", ""L""] },
        { Key: 3, n: ""Barbara"", s: ""F"", a:[""C""] },
        { Key: 4, n: ""Bill"", s: ""M"", m: 1, f: -1, ux: 5, a:[""E"", ""H""] },
        { Key: 5, n: ""Brooke"", s: ""F"", a:[""B"", ""H"", ""L""] },
        { Key: 6, n: ""Claire"", s: ""F"", m: 1, f: -1, a:[""C""] },
        { Key: 7, n: ""Carol"", s: ""F"", m: 1, f: -1, a:[""C"", ""I""] },
        { Key: 8, n: ""Chloe"", s: ""F"", m: 1, f: -1, vir: 9, a:[""E""] },
        { Key: 9, n: ""Chris"", s: ""M"", a:[""B"", ""H""] },
        { Key: 10, n: ""Ellie"", s: ""F"", m: 3, f: 2, a:[""E"", ""G""] },
        { Key: 11, n: ""Dan"", s: ""M"", m: 3, f: 2, a:[""B"", ""J""] },
        { Key: 12, n: ""Elizabeth"", s: ""F"", vir: 13, a:[""J""] },
        { Key: 13, n: ""David"", s: ""M"", m: 5, f: 4, a:[""B"", ""H""] },
        { Key: 14, n: ""Emma"", s: ""F"", m: 5, f: 4, a:[""E"", ""G""] },
        { Key: 15, n: ""Evan"", s: ""M"", m: 8, f: 9, a:[""F"", ""H""] },
        { Key: 16, n: ""Ethan"", s: ""M"", m: 8, f: 9, a:[""D"", ""K"", ""S""] },
        { Key: 17, n: ""Eve"", s: ""F"", vir: 16, a:[""B"", ""F"", ""L"", ""S""] },
        { Key: 18, n: ""Emily"", s: ""F"", m: 8, f: 9 },
        { Key: 19, n: ""Fred"", s: ""M"", m: 17, f: 16, a:[""B""] },
        { Key: 20, n: ""Faith"", s: ""F"", m: 17, f: 16, a:[""L""] },
        { Key: 21, n: ""Felicia"", s: ""F"", m: 12, f: 13, a:[""H""] },
        { Key: 22, n: ""Frank"", s: ""M"", m: 12, f: 13, a:[""B"", ""H""] },

        // ""Aaron""'s ancestors
        { Key: -10, n: ""Paternal Grandfather"", s: ""M"", m: -33, f: -32, ux: -11, a:[""A""] },
        { Key: -11, n: ""Paternal Grandmother"", s: ""F"", a:[""E""] },
        { Key: -32, n: ""Paternal Great"", s: ""M"", ux: -33, a:[""F"", ""H""] },
        { Key: -33, n: ""Paternal Great"", s: ""F"" },
        { Key: -40, n: ""Great Uncle"", s: ""M"", m: -33, f: -32, a:[""F"", ""H""] },
        { Key: -41, n: ""Great Aunt"", s: ""F"", m: -33, f: -32, a:[""B"", ""I""] },
        { Key: -20, n: ""Uncle"", s: ""M"", m: -11, f: -10, a:[""A""] },

        // ""Alice""'s ancestors
        { Key: -12, n: ""Maternal Grandfather"", s: ""M"", ux: -13, a:[""D"", ""L"", ""S""] },
        { Key: -13, n: ""Maternal Grandmother"", s: ""F"", m: -31, f: -30, a:[""H"", ""S""] },
        { Key: -21, n: ""Aunt"", s: ""F"", m: -13, f: -12, a:[""C"", ""I""] },
        { Key: -22, n: ""Uncle"", s: ""M"", ux: -21 },
        { Key: -23, n: ""Cousin"", s: ""M"", m: -21, f: -22 },
        { Key: -30, n: ""Maternal Great"", s: ""M"", ux: -31, a:[""D"", ""J"", ""S""] },
        { Key: -31, n: ""Maternal Great"", s: ""F"", m: -50, f: -51, a:[""B"", ""H"", ""L"", ""S""] },
        { Key: -42, n: ""Great Uncle"", s: ""M"", m: -30, f: -31, a:[""C"", ""J"", ""S""] },
        { Key: -43, n: ""Great Aunt"", s: ""F"", m: -30, f: -31, a:[""E"", ""G"", ""S""] },
        { Key: -50, n: ""Maternal Great Great"", s: ""F"", vir: -51, a:[""D"", ""I"", ""S""] },
        { Key: -51, n: ""Maternal Great Great"", s: ""M"", a:[""B"", ""H"", ""S""] }
      ]";
    }

    private void Setup() {
      _Diagram.InitialAutoScale = AutoScale.Uniform;
      // when a node is selected, draw a big yellow circle behind it
      _Diagram.NodeSelectionAdornmentTemplate = new Adornment(PanelType.Auto) {
        LayerName = "Grid"
      }.Add(
        new Shape("Circle") {
          Fill = "#C1CEE3", Stroke = null
        },
        new Placeholder {
          Margin = 2
        }
      );

      // use a custom layout, defined below
      _Diagram.Layout = new GenogramLayout {
        Direction = 90,
        LayerSpacing = 30,
        ColumnSpacing = 30
      };

      // determine the color for each attribute shape
      Brush attrFill(string a) {
        switch (a) {
          case "A": return "#00af54"; // green
          case "B": return "#f27935"; // orange
          case "C": return "#d4071c"; // red
          case "D": return "#70bdc2"; // cyan
          case "E": return "#fcf384"; // gold
          case "F": return "#e69aaf"; // pink
          case "G": return "#08488f"; // blue
          case "H": return "#866310"; // brown
          case "I": return "#9270c2"; // purple
          case "J": return "#a3cf62"; // chartreuse
          case "K": return "#91a4c2"; // lightgray bluish
          case "L": return "#af70c2"; // magenta
          case "S": return "#d4071c"; // red
          default: return "transparent";
        }
      }

      // determine the geometry for each attribute shape in a male;
      // except for the slash these are all squares at each of the four corners of the overall square
      var tlsq = Geometry.Parse("F M1 1 l19 0 0 19 -19 0z");
      var trsq = Geometry.Parse("F M20 1 l19 0 0 19 -19 0z");
      var brsq = Geometry.Parse("F M20 20 l19 0 0 19 -19 0z");
      var blsq = Geometry.Parse("F M1 20 l19 0 0 19 -19 0z");
      var slash = Geometry.Parse("F M38 0 L40 0 40 2 2 40 0 40 0 38z");
      Geometry maleGeometry(string a) {
        switch (a) {
          case "A": return tlsq;
          case "B": return tlsq;
          case "C": return tlsq;
          case "D": return trsq;
          case "E": return trsq;
          case "F": return trsq;
          case "G": return brsq;
          case "H": return brsq;
          case "I": return brsq;
          case "J": return blsq;
          case "K": return blsq;
          case "L": return blsq;
          case "S": return slash;
          default: return tlsq;
        }
      }

      // determine the geometry for each attribute shape in a female;
      // except for the slash these are all pie shapes at each of the four quadrants of the overall circle
      var tlarc = Geometry.Parse("F M20 20 B 180 90 20 20 19 19 z");
      var trarc = Geometry.Parse("F M20 20 B 270 90 20 20 19 19 z");
      var brarc = Geometry.Parse("F M20 20 B 0 90 20 20 19 19 z");
      var blarc = Geometry.Parse("F M20 20 B 90 90 20 20 19 19 z");
      Geometry femaleGeometry(string a) {
        switch (a) {
          case "A": return tlarc;
          case "B": return tlarc;
          case "C": return tlarc;
          case "D": return trarc;
          case "E": return trarc;
          case "F": return trarc;
          case "G": return brarc;
          case "H": return brarc;
          case "I": return brarc;
          case "J": return blarc;
          case "K": return blarc;
          case "L": return blarc;
          case "S": return slash;
          default: return tlarc;
        }
      }

      // two different node templates, one for each sex,
      // named by the category value in the node data object
      _Diagram.NodeTemplateMap.Add("M", // male
        new Node(PanelType.Vertical) {
          LocationSpot = Spot.Center,
          LocationElementName = "ICON",
          SelectionElementName = "ICON"
        }.Add(
          new Panel { Name = "ICON" }.Add(
            new Shape("Square") {
              Width = 40,
              Height = 40,
              StrokeWidth = 2,
              Fill = "white",
              Stroke = "#919191",
              PortId = ""
            },
            new Panel { // for each attribute show a Shape at a particular place in the overall square
              Margin = 1,
              ItemTemplate = new Panel().Add(
                new Shape {
                  Stroke = null,
                  StrokeWidth = 0
                }.Bind("Fill", "", (s, _) => attrFill(s as string))
                  .Bind("Geometry", "", (s, _) => maleGeometry(s as string))
              )
            }.Bind("ItemList", "a")
          ),
          new TextBlock {
            TextAlign = TextAlign.Center,
            MaxSize = new Size(80, double.NaN)
          }.Bind("Text", "n")
      ));

      _Diagram.NodeTemplateMap.Add("F", // female
        new Node(PanelType.Vertical) {
          LocationSpot = Spot.Center,
          LocationElementName = "ICON",
          SelectionElementName = "ICON"
        }.Add(
          new Panel { Name = "ICON" }.Add(
            new Shape("Circle") {
              Width = 40,
              Height = 40,
              StrokeWidth = 2,
              Fill = "white",
              Stroke = "#a1a1a1",
              PortId = ""
            },
            new Panel { // for each attribute show a Shape at a particular place in the overall circle
              Margin = 1,
              ItemTemplate = new Panel().Add(
                  new Shape {
                    Stroke = null, StrokeWidth = 0
                  }.Bind("Fill", "", (s, _) => attrFill(s as string))
                   .Bind("Geometry", "", (s, _) => femaleGeometry(s as string))
                )
            }.Bind("ItemList", "a")
          ),
          new TextBlock {
            TextAlign = TextAlign.Center, MaxSize = new Size(80, double.NaN)
          }.Bind("Text", "n")
        )
      );

      // the representation of each label node -- nothing shows on a marriage Link
      _Diagram.NodeTemplateMap["LinkLabel"] =
        new Node {
          Selectable = false,
          Width = 1,
          Height = 1,
          FromEndSegmentLength = 20
        };

      _Diagram.LinkTemplate = new Link { // for Parent-child relationships
        Routing = LinkRouting.Orthogonal, Corner = 5,
        LayerName = "Background", Selectable = false,
        FromSpot = Spot.Bottom, ToSpot = Spot.Top
      }.Add(new Shape {
        Stroke = "#424242", StrokeWidth = 2
      });

      _Diagram.LinkTemplateMap.Add("Marriage", // for marriage relationships
        new Link {
          Selectable = false
        }.Add(new Shape {
          StrokeWidth = 2.5, Stroke = "#5d8cc1" /* blue */
        })
      );

      _SetupDiagram(new List<NodeData> {
        new NodeData { Key = -1, n = "Aaron", s = "M", m = -10, f = -11, ux = 1, a = new List<string> {"C", "F", "K"} }, // don't use key 0 since it's default
        new NodeData { Key = 1, n = "Alice", s = "F", m = -12, f = -13, a = new List<string> {"B", "H", "K"} },
        new NodeData { Key = 2, n = "Bob", s = "M", m = 1, f = -1, ux = 3, a = new List<string> {"C", "H", "L"} },
        new NodeData { Key = 3, n = "Barbara", s = "F", a = new List<string> {"C"} },
        new NodeData { Key = 4, n = "Bill", s = "M", m = 1, f = -1, ux = 5, a = new List<string> {"E", "H"} },
        new NodeData { Key = 5, n = "Brooke", s = "F", a = new List<string> {"B", "H", "L"} },
        new NodeData { Key = 6, n = "Claire", s = "F", m = 1, f = -1, a = new List<string> {"C"} },
        new NodeData { Key = 7, n = "Carol", s = "F", m = 1, f = -1, a = new List<string> {"C", "I"} },
        new NodeData { Key = 8, n = "Chloe", s = "F", m = 1, f = -1, vir = 9, a = new List<string> {"E"} },
        new NodeData { Key = 9, n = "Chris", s = "M", a = new List<string> {"B", "H"} },
        new NodeData { Key = 10, n = "Ellie", s = "F", m = 3, f = 2, a = new List<string> {"E", "G"} },
        new NodeData { Key = 11, n = "Dan", s = "M", m = 3, f = 2, a = new List<string> {"B", "J"} },
        new NodeData { Key = 12, n = "Elizabeth", s = "F", vir = 13, a = new List<string> {"J"} },
        new NodeData { Key = 13, n = "David", s = "M", m = 5, f = 4, a = new List<string> {"B", "H"} },
        new NodeData { Key = 14, n = "Emma", s = "F", m = 5, f = 4, a = new List<string> {"E", "G"} },
        new NodeData { Key = 15, n = "Evan", s = "M", m = 8, f = 9, a = new List<string> {"F", "H"} },
        new NodeData { Key = 16, n = "Ethan", s = "M", m = 8, f = 9, a = new List<string> {"D", "K"} },
        new NodeData { Key = 17, n = "Eve", s = "F", vir = 16, a = new List<string> {"B", "F", "L"} },
        new NodeData { Key = 18, n = "Emily", s = "F", m = 8, f = 9 },
        new NodeData { Key = 19, n = "Fred", s = "M", m = 17, f = 16, a = new List<string> {"B"} },
        new NodeData { Key = 20, n = "Faith", s = "F", m = 17, f = 16, a = new List<string> {"L"} },
        new NodeData { Key = 21, n = "Felicia", s = "F", m = 12, f = 13, a = new List<string> {"H"} },
        new NodeData { Key = 22, n = "Frank", s = "M", m = 12, f = 13, a = new List<string> {"B", "H"} },

        // "Aaron"'s ancestors
        new NodeData { Key = -10, n = "Paternal Grandfather", s = "M", m = -33, f = -32, ux = -11, a = new List<string> {"A", "S"} },
        new NodeData { Key = -11, n = "Paternal Grandmother", s = "F", a = new List<string> {"E", "S"} },
        new NodeData { Key = -32, n = "Paternal Great", s = "M", ux = -33, a = new List<string> {"F", "H", "S"} },
        new NodeData { Key = -33, n = "Paternal Great", s = "F", a = new List<string> {"S"} },
        new NodeData { Key = -40, n = "Great Uncle", s = "M", m = -33, f = -32, a = new List<string> {"F", "H", "S"} },
        new NodeData { Key = -41, n = "Great Aunt", s = "F", m = -33, f = -32, a = new List<string> {"B", "I", "S"} },
        new NodeData { Key = -20, n = "Uncle", s = "M", m = -11, f = -10, a = new List<string> {"A", "S"} },

        // "Alice"'s ancestors
        new NodeData { Key = -12, n = "Maternal Grandfather", s = "M", ux = -13, a = new List<string> {"D", "L", "S"} },
        new NodeData { Key = -13, n = "Maternal Grandmother", s = "F", m = -31, f = -30, a = new List<string> {"H", "S"} },
        new NodeData { Key = -21, n = "Aunt", s = "F", m = -13, f = -12, a = new List<string> {"C", "I"} },
        new NodeData { Key = -22, n = "Uncle", s = "M", ux = -21 },
        new NodeData { Key = -23, n = "Cousin", s = "M", m = -21, f = -22 },
        new NodeData { Key = -30, n = "Maternal Great", s = "M", ux = -31, a = new List<string> {"D", "J", "S"} },
        new NodeData { Key = -31, n = "Maternal Great", s = "F", m = -50, f = -51, a = new List<string> {"B", "H", "L", "S"} },
        new NodeData { Key = -42, n = "Great Uncle", s = "M", m = -30, f = -31, a = new List<string> {"C", "J", "S"} },
        new NodeData { Key = -43, n = "Great Aunt", s = "F", m = -30, f = -31, a = new List<string> {"E", "G", "S"} },
        new NodeData { Key = -50, n = "Maternal Great Great", s = "F", vir = -51, a = new List<string> {"D", "I", "S"} },
        new NodeData { Key = -51, n = "Maternal Great Great", s = "M", a = new List<string> {"B", "H", "S"} }
      }, 4);
    }

    // create and initialize the Diagram.Model given an array of node data representing people
    private void _SetupDiagram(List<NodeData> array, int focusId) {
      _Diagram.Model = new Model {
        // declare support for link label nodes
        LinkLabelKeysProperty = "LabelKeys",
        // this property determines which template is used
        NodeCategoryProperty = "s",
        // if a node data object is copied, copy its data.A array
        //CopiesArrays = true,
        // create all of the nodes for people
        NodeDataSource = array
      };

      _SetupMarriages();
      _SetupParents();

      var node = _Diagram.FindNodeForKey(focusId);
      if (node != null) {
        _Diagram.Select(node);
      }
    }

    private Link _FindMarriage(int a, int b) { // A and B are node keys
      var nodeA = _Diagram.FindNodeForKey(a);
      var nodeB = _Diagram.FindNodeForKey(b);
      if (nodeA != null && nodeB != null) {
        var it = nodeA.FindLinksBetween(nodeB); // in either direction
        foreach (var link in it) {
          // Link.data.category == "Marriage" means its a marriage relationship
          if (link.Data != null && (link.Data as LinkData).Category == "Marriage") return link;
        }
      }
      return null;
    }

    // now process the node data to determine marriages
    private void _SetupMarriages() {
      int idx = 10000; // a key that will never be used by another Node
      var model = _Diagram.Model;
      var nodeDataSource = model.NodeDataSource as List<NodeData>;
      for (var i = 0; i < nodeDataSource.Count; i++) {
        var data = nodeDataSource[i];
        var key = data.Key;
        var _uxs = data.ux;
        if (_uxs != null) {
          var uxs = _uxs.Value;
          var l = new List<int> { uxs };
          for (var j = 0; j < l.Count; j++) {
            var wife = l[j];
            if (key == wife || model.FindNodeDataForKey(wife) is not NodeData wdata || wdata.s != "F") {
              System.Diagnostics.Trace.TraceWarning($"cannot create Marriage relationship with self or unknown person {wife}");
              continue;
            }
            var link = _FindMarriage(key, wife);
            if (link == null) {
              // add a label node for the marriage link
              var mlab = new NodeData { s = "LinkLabel", Key = idx++ };
              model.AddNodeData(mlab);
              // add the marriage link itself, also referring to the label node
              var mdata = new LinkData { From = key, To = wife, LabelKeys = new List<int> { mlab.Key }, Category = "Marriage" };
              (model as Model).AddLinkData(mdata);
            }
          }
        }
        var _virs = data.vir;
        if (_virs != null) {
          var virs = _virs.Value;
          var l = new List<int> { virs };
          for (var j = 0; j < l.Count; j++) {
            var husband = l[j];
            if (key == husband || model.FindNodeDataForKey(husband) is not NodeData hdata || hdata.s != "M") {
              System.Diagnostics.Trace.TraceWarning($"cannot create Marriage relationship with self or unknown person {husband}");
              continue;
            }
            var link = _FindMarriage(key, husband);
            if (link == null) {
              // add a label node for the marriage link
              var mlab = new NodeData { s = "LinkLabel", Key = idx++ };
              model.AddNodeData(mlab);
              // add the marriage link itself, also referring to the label node
              var mdata = new LinkData { From = key, To = husband, LabelKeys = new List<int> { mlab.Key }, Category = "Marriage" };
              (model as Model).AddLinkData(mdata);
            }
          }
        }
      }
    }

    // process parent-child relationships once all marriage are known
    private void _SetupParents() {
      var model = _Diagram.Model;
      var nodeDataSource = model.NodeDataSource as List<NodeData>;
      for (var i = 0; i < nodeDataSource.Count; i++) {
        var data = nodeDataSource[i];
        var key = data.Key;
        var mother = data.m;
        var father = data.f;
        if (mother != null && father != null) {
          var link = _FindMarriage(mother.Value, father.Value);
          if (link == null) {
            // or warn no known mother or no known father or no known marriage between them
            System.Diagnostics.Trace.TraceWarning($"unknown marriage: {mother.Value} & {father.Value}");
            continue;
          }
          var mdata = link.Data as LinkData;
          if (mdata.LabelKeys == null) continue;
          var mlabkey = mdata.LabelKeys[0];
          var cdata = new LinkData { From = mlabkey, To = key };
          (model as Model).AddLinkData(cdata);
        }
      }
    }



  }

  public class GenogramLayout : LayeredDigraphLayout {
    // minimum space between spouses
    public double SpouseSpacing { get; set; }

    public GenogramLayout() : base() {
      InitializeOption = LayeredDigraphInit.DepthFirstIn;
      SpouseSpacing = 30;
    }

    public override LayeredDigraphNetwork MakeNetwork(IEnumerable<Part> coll = null) {
      var net = CreateNetwork();
      if (coll != null) {
        _Add(net, coll, false);
      } else if (Group != null) {
        _Add(net, Group.MemberParts, false);
      } else if (Diagram != null) {
        _Add(net, Diagram.Nodes, true);
        _Add(net, Diagram.Links, true);
      }
      return net;
    }

    // internal method for creating LayeredDigraphNetwork where husband/wife pairs are represented
    // by a single Vertex corresponding to the label node on the marriage Link
    private void _Add(LayeredDigraphNetwork net, IEnumerable<Part> coll, bool nonmemberonly) {
      var multiSpousePeople = new HashSet<Node>();
      // consider all Nodes in the given collection
      foreach (var part in coll) {
        if (!(part is Node node)) continue;
        if (!node.IsLayoutPositioned || !node.IsVisible()) continue;
        if (nonmemberonly && node.ContainingGroup != null) continue;
        // if it's an unmarried Node, or if it's a Link Label Node, create a LayoutVertex for it
        if (node.IsLinkLabel) {
          // get Marriage Link
          var link = node.LabeledLink;
          var spouseA = link.FromNode;
          var spouseB = link.ToNode;
          // create vertex representing both husband and wife
          var vertex = net.AddNode(node);
          // now define the vertex size to be big enough to hold both spouses
          vertex.Width = spouseA.ActualBounds.Width + SpouseSpacing + spouseB.ActualBounds.Width;
          vertex.Height = Math.Max(spouseA.ActualBounds.Height, spouseB.ActualBounds.Height);
          vertex.Focus = new Point(spouseA.ActualBounds.Width + SpouseSpacing / 2, vertex.Height / 2);
        } else {
          // don't add a vertex for any married person!
          // intead, code above adds label node for marriage Link
          // assume a marriage Link has a label Node
          var marriages = 0;
          foreach (var l in node.LinksConnected) { if (l.IsLabeledLink) marriages++; }
          if (marriages == 0) {
            var vertex = net.AddNode(node);
          } else if (marriages > 1) {
            multiSpousePeople.Add(node);
          }
        }
      }
      // now do all Links
      foreach (var part in coll) {
        if (!(part is Link link)) continue;
        if (!link.IsLayoutPositioned || !link.IsVisible()) continue;
        if (nonmemberonly && link.ContainingGroup != null) continue;
        // if it's a parent-child link, add a LayoutEdge for it
        if (!link.IsLabeledLink) {
          var parent = net.FindVertex(link.FromNode); // should be a label node
          var child = net.FindVertex(link.ToNode);
          if (child != null) { // an unmarried child
            net.LinkVertexes(parent, child, link);
          } else { // a married child
            foreach (var l in link.ToNode.LinksConnected) {
              if (!l.IsLabeledLink) continue; // if it has no label node, it's a parent-child link
              // found the Marriage Link, now get its label node
              var mlab = l.LabelNodes.First();
              // parent child link should connect with the label node,
              // so the LayoutEdge should connect with the LayoutVertex representing the label node
              var mlabvert = net.FindVertex(mlab);
              if (mlabvert != null) {
                net.LinkVertexes(parent, mlabvert, link);
              }
            }
          }
        }
      }

      while (multiSpousePeople.Count > 0) {
        // find all collections of people that are indirectly married to each other
        var node = multiSpousePeople.First();
        var cohort = new HashSet<Node>();
        _ExtendCohort(cohort, node);
        // then encourage them all to be the same generation by connceting them all with a common vertex
        var dummyvert = net.CreateVertex();
        net.AddVertex(dummyvert);
        var marriages = new HashSet<Link>();
        foreach (var n in cohort) {
          foreach (var l in n.LinksConnected) {
            marriages.Add(l);
          }
        }
        foreach (var link in marriages) {
          // find the vertex for the marriage link (i.e. for the label node)
          var mlab = link.LabelNodes.First();
          var v = net.FindVertex(mlab);
          if (v != null) {
            net.LinkVertexes(dummyvert, v, null);
          }
        }
        // done with these people, now see if there are any other multiple-married people
        multiSpousePeople.RemoveWhere(node => cohort.Contains(node));
      }
    }

    // collect all of the people indirectly married with a person
    private void _ExtendCohort(HashSet<Node> coll, Node node) {
      if (coll.Contains(node)) return;
      coll.Add(node);
      var lay = this;
      foreach (var l in node.LinksConnected) {
        if (l.IsLabeledLink) { // if it's a marriage link, continue with both spouses
          lay._ExtendCohort(coll, l.FromNode);
          lay._ExtendCohort(coll, l.ToNode);
        }
      }
    }

    protected override void AssignLayers() {
      base.AssignLayers();
      var horiz = Math.Abs(Direction) < 0.001 || Math.Abs(Direction - 180) < 0.001;
      // for every vertex record the maximum vertex width or height for the vertex's layer
      var maxsizes = new Dictionary<int, double>();
      foreach (var v in Network.Vertexes) {
        var lay = v.Layer;
        var max = 0.0;
        if (maxsizes.ContainsKey(lay)) max = maxsizes[lay];
        var sz = horiz ? v.Width : v.Height;
        if (sz > max) maxsizes[lay] = sz;
      }

      // now make sure every vertex has the maximum width or height according to which layer it is in,
      // and aligned on the left (if horizontal) or the top (if vertical)
      foreach (var v in Network.Vertexes) {
        var lay = v.Layer;
        var max = maxsizes[lay];
        if (horiz) {
          v.Focus = new Point(0, v.Height / 2);
          v.Width = max;
        } else {
          v.Focus = new Point(v.Width / 2, 0);
          v.Height = max;
        }
      }
      // from now on, the LayeredDigraphLayout will think that the node is bigger than it really is
      // (other than the ones that are the widest and tallest in their respective layer).
    }

    protected override void CommitNodes() {
      base.CommitNodes();
      // position regular nodes
      foreach (var v in Network.Vertexes) {
        if (v.Node != null && !v.Node.IsLinkLabel) {
          v.Node.Move(v.X, v.Y);
        }
      }
      // position the spouses of each marriage vertex
      var layout = this;
      foreach (var v in Network.Vertexes) {
        if (v.Node == null) continue;
        if (!v.Node.IsLinkLabel) continue;
        var labnode = v.Node;
        var lablink = labnode.LabeledLink;
        // In case the spouses are not actually moved, we need to have the marriage link
        // position the label node, because LayoutVertex.Commit was called above on these vertexes.
        // Alternatively we could override LayoutVertex.Commit to be a noop for label node vertexes.
        lablink.InvalidateRoute();
        var spouseA = lablink.FromNode;
        var spouseB = lablink.ToNode;
        // prefer father on the left, mothers on the right
        if ((spouseA.Data as NodeData).s == "F") { // sex is female
          var temp = spouseA;
          spouseA = spouseB;
          spouseB = temp;
        }
        // see if the parents are on the desired sides, to avoid a link crossing
        var aParentsNode = layout.FindParentsMarriageLabelNode(spouseA);
        var bParentsNode = layout.FindParentsMarriageLabelNode(spouseB);
        if (aParentsNode != null && bParentsNode != null && aParentsNode.Position.X > bParentsNode.Position.X) {
          // swap the spouses again
          var temp = spouseA;
          spouseA = spouseB;
          spouseB = temp;
        }
        spouseA.Move(v.X, v.Y);
        spouseB.Move(v.X + spouseA.ActualBounds.Width + layout.SpouseSpacing, v.Y);
        if (spouseA.Opacity == 0) {
          var pos = new Point(v.CenterX - spouseA.ActualBounds.Width / 2, v.Y);
          spouseA.Move(pos);
          spouseB.Move(pos);
        } else if (spouseB.Opacity == 0) {
          var pos = new Point(v.CenterX - spouseB.ActualBounds.Width / 2, v.Y);
          spouseA.Move(pos);
          spouseB.Move(pos);
        }
      }
      // position only-child nodes to be under the marriage label node
      foreach (var v in Network.Vertexes) {
        if (v.Node == null || v.Node.LinksConnected.Count() > 1) continue;
        var mnode = layout.FindParentsMarriageLabelNode(v.Node);
        if (mnode != null && mnode.LinksConnected.Count() == 1) { // if only one child
          var mvert = layout.Network.FindVertex(mnode);
          var newbnds = new Rect(v.Node.ActualBounds.X, v.Node.ActualBounds.Y, v.Node.ActualBounds.Width, v.Node.ActualBounds.Height);
          newbnds.X = mvert.CenterX - v.Node.ActualBounds.Width / 2;
          // see if there's any empty space at the horizontal mid-point in that layer
          var overlaps = new List<Part>();
          layout.Diagram.FindElementsIn(newbnds, x => x.Part, p => p != v.Node, true, overlaps);
          if (overlaps.Count == 0) {
            v.Node.Move(newbnds.Position);
          }
        }
      }
    }

    private Node FindParentsMarriageLabelNode(Node parent) {
      foreach (var n in parent.FindNodesInto()) {
        if (n.IsLinkLabel) return n;
      }
      return null;
    }
  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }

  // n: name, s: sex, m: mother, f: father, ux: wife, vir: husband, a: attributes/markers
  public class NodeData : Model.NodeData {
    public string n { get; set; }
    public string s { get; set; }
    public int? m { get; set; }
    public int? f { get; set; }
    public int? ux { get; set; }
    public int? vir { get; set; }
    public List<string> a { get; set; }
  }

  public class LinkData : Model.LinkData {
    public List<int> LabelKeys { get; set; }
  }

}
