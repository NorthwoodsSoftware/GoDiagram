/* Copyright (c) Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;

namespace Demo.Samples.SharedStates {
  [ToolboxItem(false)]
  public partial class SharedStatesControl : DemoControl {
    private Diagram MyDiagram;

    public SharedStatesControl() {
      InitializeComponent();

      modelJson1.SaveClick = _Save;
      modelJson1.LoadClick = _Load;

      goWebBrowser1.Html = @"
        <p>
          This demonstrates the ability to simulate having nodes be members of multiple ""groups"".
          Regular <a>Group</a> s only support a single <a>Part.ContainingGroup</a> for nodes.
          This sample does not make use of <a>Group</a>s at all, but simulates one possible ""grouping"" relationship
          using a custom <a>Layout</a> and a custom <a>DraggingTool</a>.
        </p>
        <p>
          The CustomLayout assumes regular nodes already have real locations, and only assigns <a>Part.Location</a> to ""Super"" nodes.
          It also sets the <a>GraphObject.DesiredSize</a> on the ""BODY"" element of each ""Super"" node, based on the area occupied by all of its member nodes.
          The CustomDraggingTool overrides the <a>DraggingTool.ComputeEffectiveCollection</a> method to determine what nodes to drag around.
        </p>
        <p>
          This sample does not support dynamic restructuring of the relationships in the graph.
        </p>
";

      modelJson1.JsonText = @"
      {
        ""nodeDataSource"": [
          { ""key"": -1, ""text"": ""Operating"", ""category"": ""Super"" },
          { ""key"": -2, ""text"": ""Drying"", ""category"": ""Super"", ""supers"": [-1]},
          { ""key"": -3, ""text"": ""Non Drying"", ""category"": ""Super"" },
          { ""key"": 1, ""loc"": ""100 100"", ""text"": ""Starting.End"", ""supers"": [-2] },
          { ""key"": 2, ""loc"": ""250 100"", ""text"": ""Running"", ""supers"": [-2] },
          { ""key"": 3, ""loc"": ""100 200"", ""text"": ""Starting.Init"", ""supers"": [-1, -3] },
          { ""key"": 4, ""loc"": ""250 200"", ""text"": ""Washing"", ""supers"": [-1, -3] },
          { ""key"": 5, ""loc"": ""100 300"", ""text"": ""Stopped"", ""supers"": [-3] },
          { ""key"": 6, ""loc"": ""250 300"", ""text"": ""Stopping"", ""supers"": [-3] }
        ],
        ""linkDataSource"": [
          { ""from"":  1, ""to"": 2 },
          { ""from"":  3, ""to"": 1 },
          { ""from"":  4, ""to"": 2 },
          { ""from"": -2, ""to"": 4 },
          { ""from"":  5, ""to"": 3 },
          { ""from"":  6, ""to"": 5 },
          { ""from"": -1, ""to"": 5 }
        ]
       }";

      Setup();
    }

    private void _Save() {
      if (MyDiagram == null) return;
      modelJson1.JsonText = MyDiagram.Model.ToJson();
      MyDiagram.IsModified = false;
    }

    public void _Load() {
      if (MyDiagram == null) return;
      MyDiagram.Model = Model.FromJson<Model>(modelJson1.JsonText);
      // make sure all data have up-to-date "members" collections
      // this does not prevent any "cycles" of membership, which would result in undefined behavior

      var arr = MyDiagram.Model.NodeDataSource;
      foreach (NodeData data in arr) {
        var supers = data.Supers;
        if (supers != null) {
          foreach (var super in supers) {
            if (MyDiagram.Model.FindNodeDataForKey(super) is NodeData sdata) {
              // update Members to be an array of references to node data
              if (sdata.Members == null) {
                sdata.Members = new List<NodeData> { data };
              } else {
                sdata.Members.Add(data);
              }
            }
          }
        }
      }

    }

    private void Setup() {

      MyDiagram = diagramControl1.Diagram;

      MyDiagram.AllowCopy = false;
      MyDiagram.AllowDelete = false;
      MyDiagram.ToolManager.DraggingTool = new CustomDraggingTool();
      MyDiagram.Layout = new CustomLayout();
      MyDiagram.UndoManager.IsEnabled = true;

      MyDiagram.NodeTemplate = new Node(PanelType.Auto)
        .BindTwoWay("Location", "Loc", Point.Parse, Point.Stringify)
        .Add(
          // define the node's outer shape, which will surround the TextBlock
          new Shape {
            Figure = "RoundedRectangle",
            Fill = "rgb(254, 201, 0)", Stroke = "black", Parameter1 = 20, // the corner has a large radius
            PortId = "", FromSpot = Spot.AllSides, ToSpot = Spot.AllSides
          },
          new TextBlock().BindTwoWay("Text")
        );

      MyDiagram.NodeTemplateMap.Add("Super",
        new Node(PanelType.Auto) {
          LocationElementName = "BODY"
        }.Add(
          new Shape {
            Figure = "RoundedRectangle",
            Fill = "rgba(128, 128, 64, 0.5)",
            StrokeWidth = 1.5,
            Parameter1 = 20,
            Spot1 = Spot.TopLeft, Spot2 = Spot.BottomRight, MinSize = new Size(30, 30)
          },
          new Panel(PanelType.Vertical) {
            Margin = 10
          }.Add(
            new TextBlock {
              Font = new Font("Segoe UI", 13, FontWeight.Bold), Margin = new Margin(0, 0, 5, 0)
            }.Bind("Text"),
            new Shape {
              Name = "BODY", Opacity = 0
            }
          )
        )
      );

      // replace the default Link template in the LinkTemplateMap
      MyDiagram.LinkTemplate = new Link { // the whole link panel
        Routing = LinkRouting.Orthogonal,
        Corner = 10
      }.Add(
        new Shape { // the link shape
          StrokeWidth = 1.5
        },
        new Shape { // the arrowhead
          ToArrow = "Standard", Stroke = null
        }
      );

      // read in the JSON-format data from the MySavedModel element
      _Load();
    }
  }


  public class CustomDraggingTool : DraggingTool {
    public override void MoveParts(IDictionary<Part, DraggingInfo> parts, Point offset, bool check) {
      base.MoveParts(parts, offset, check);
      Diagram.LayoutDiagram(true);
    }

    public override IDictionary<Part, DraggingInfo> ComputeEffectiveCollection(IEnumerable<Part> parts, DraggingOptions options) {
      var coll = new HashSet<Part>();
      foreach (var part in parts) coll.Add(part);
      foreach (var p in parts) {
        _WalkSubTree(p, coll);
      }
      return base.ComputeEffectiveCollection(parts, options);
    }

    private void _WalkSubTree(Part sup, HashSet<Part> coll) {
      if (sup == null) return;
      coll.Add(sup);
      if (sup.Category != "Super") return;
      // recuse through this super state's members
      var model = Diagram.Model;
      var mems = (sup.Data as NodeData).Members;
      if (mems != null) {
        foreach (var mdata in mems) {
          _WalkSubTree(Diagram.FindNodeForData(mdata), coll);
        }
      }
    }
  }

  public class CustomLayout : Layout {
    public override void DoLayout(IEnumerable<Part> coll = null) {
      HashSet<Part> allparts = null;
      var diagram = Diagram;
      if (coll != null) {
        allparts = CollectParts(coll);
      } else if (Group != null) {
        allparts = CollectParts(Group);
      } else if (diagram != null) {
        allparts = CollectParts(diagram);
      } else {
        return; // Nothing to layout!
      }

      var supers = new HashSet<Node>();
      foreach (var p in allparts) {
        if (p is Node n && p.Category == "Super") supers.Add(n);
      }

      HashSet<Part> membersOf(Node sup, Diagram diag) {
        var set = new HashSet<Part>();
        var arr = (sup.Data as NodeData).Members;
        foreach (var d in arr) {
          set.Add(diag.FindNodeForData(d));
        }
        return set;
      }

      bool isReady(Node sup, IEnumerable<Node> supers) {
        var arr = (sup.Data as NodeData).Members;
        foreach (var d in arr) {
          if (d.Category != "Super") continue;
          var n = Diagram.FindNodeForData(d);
          if (supers.Contains(n)) return false;
        }
        return true;
      }

      // implementations of DoLayout that do not make use of a LayoutNetwork
      // need to perform their own transactions
      Diagram.StartTransaction("Custom Layout");

      while (supers.Count > 0) {
        Node ready = null;
        var it = supers;
        foreach (var value in it) {
          if (isReady(value, supers)) {
            ready = value;
            break;
          }
        }
        supers.Remove(ready);
        var b = Diagram.ComputePartsBounds(membersOf(ready, Diagram), true);
        ready.Location = b.Position;
        var body = ready.FindElement("BODY");
        if (body != null) body.DesiredSize = b.Size;
      }

      Diagram.CommitTransaction("Custom Layout");
    }
  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }

  public class NodeData : Model.NodeData {
    public string Loc { get; set; }
    public List<int> Supers { get; set; } = null; // these collections represent the group structure of this SharedStatesModel.
    [JsonIgnore]
    public List<NodeData> Members { get; set; } = null;  // this should not be serialized during save
  }

  public class LinkData : Model.LinkData { }

}




