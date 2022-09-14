/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace Demo.Samples.DynamicPorts {
  public partial class DynamicPorts : DemoControl {
    private Diagram _Diagram;

    public DynamicPorts() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      topBtn.Click += (e, obj) => AddPort("Top");
      bottomBtn.Click += (e, obj) => AddPort("Bottom");
      leftBtn.Click += (e, obj) => AddPort("Left");
      rightBtn.Click += (e, obj) => AddPort("Right");

      modelJson1.SaveClick = SaveModel;
      modelJson1.LoadClick = LoadModel;

      desc1.MdText = DescriptionReader.Read("Samples.DynamicPorts.md");

      modelJson1.JsonText = @"{
  ""LinkFromPortIdProperty"": ""FromPort"",
  ""LinkToPortIdProperty"": ""ToPort"",
  ""NodeDataSource"": [
    {
      ""Key"": 1, ""Name"": ""Unit One"", ""Loc"": ""101 204"",
      ""LeftList"": [ { ""PortColor"": ""#fae3d7"", ""PortId"": ""Left0"" } ],
      ""TopList"": [ { ""PortColor"": ""#d6effc"", ""PortId"": ""Top0"" } ],
      ""BottomList"": [ { ""PortColor"": ""#ebe3fc"", ""PortId"": ""Bottom0"" } ],
      ""RightList"": [ { ""PortColor"": ""#eaeef8"", ""PortId"": ""Right0"" }, { ""PortColor"": ""#fadfe5"", ""PortId"": ""Right1"" } ]
    },
    {
      ""Key"": 2, ""Name"": ""Unit Two"", ""Loc"": ""320 152"",
      ""LeftList"": [ { ""PortColor"": ""#6cafdb"", ""PortId"": ""Left0"" }, { ""PortColor"": ""#66d6d1"", ""PortId"": ""Left1"" }, { ""PortColor"": ""#fae3d7"", ""PortId"": ""Left2"" } ],
      ""TopList"": [ { ""PortColor"": ""#d6effc"", ""PortId"": ""Top0"" } ],
      ""BottomList"": [ { ""PortColor"": ""#eaeef8"", ""PortId"": ""Bottom0"" }, { ""PortColor"": ""#eaeef8"", ""PortId"": ""Bottom1"" }, { ""PortColor"": ""#6cafdb"", ""PortId"": ""Bottom2"" } ],
      ""RightList"": [ ]
    },
    {
      ""Key"": 3, ""Name"": ""Unit Three"", ""Loc"": ""384 319"",
      ""LeftList"": [ { ""PortColor"": ""#66d6d1"", ""PortId"": ""Left0"" }, { ""PortColor"": ""#fadfe5"", ""PortId"": ""Left1"" }, { ""PortColor"": ""#6cafdb"", ""PortId"": ""Left2"" } ],
      ""TopList"": [ { ""PortColor"": ""#66d6d1"", ""PortId"": ""Top0"" } ],
      ""BottomList"": [ { ""PortColor"": ""#6cafdb"", ""PortId"": ""Bottom0"" } ],
      ""RightList"": [ ]
    },
    {
      ""Key"": 4, ""Name"": ""Unit Four"", ""Loc"": ""138 351"",
      ""LeftList"": [ { ""PortColor"": ""#fae3d7"", ""PortId"": ""Left0"" } ],
      ""TopList"": [ { ""PortColor"": ""#6cafdb"", ""PortId"": ""Top0"" } ],
      ""BottomList"": [ { ""PortColor"": ""#6cafdb"", ""PortId"": ""Bottom0"" } ],
      ""RightList"": [ { ""PortColor"": ""#6cafdb"", ""PortId"": ""Right0"" }, { ""PortColor"": ""#66d6d1"", ""PortId"": ""Right1"" } ]
    }
  ],
  ""LinkDataSource"": [
    { ""From"": 4, ""To"": 2, ""FromPort"": ""Top0"", ""ToPort"": ""Bottom0"" },
    { ""From"": 4, ""To"": 2, ""FromPort"": ""Top0"", ""ToPort"": ""Bottom0"" },
    { ""From"": 3, ""To"": 2, ""FromPort"": ""Top0"", ""ToPort"": ""Bottom1"" },
    { ""From"": 4, ""To"": 3, ""FromPort"": ""Right0"", ""ToPort"": ""Left0"" },
    { ""From"": 4, ""To"": 3, ""FromPort"": ""Right1"", ""ToPort"": ""Left2"" },
    { ""From"": 1, ""To"": 2, ""FromPort"": ""Right0"", ""ToPort"": ""Left1"" },
    { ""From"": 1, ""To"": 2, ""FromPort"": ""Right1"", ""ToPort"": ""Left2"" }
  ]
}";

      Setup();
    }

    private void Setup() {
      _Diagram.UndoManager.IsEnabled = true;

      // To simplify this code we define a function for creating a context menu button:
      Panel makeButton(string text, Action<InputEvent, GraphObject> action, Func<GraphObject, bool> visiblePredicate = null) {
        object converter(object obj) {
          var elt = obj as GraphObject;
          return elt.Diagram != null && visiblePredicate(elt);
        }

        return Builder.Make<Panel>("ContextMenuButton")
          .Add(new TextBlock(text))
          .Set(new { Click = action })
          .Bind(visiblePredicate != null ? new Binding("Visible", "", converter).OfElement() : null);
      }

      var nodeMenu =  // context menu for each node
        Builder.Make<Adornment>("ContextMenu")
          .Add(
            makeButton("Copy",
              (e, obj) => { e.Diagram.CommandHandler.CopySelection(); },
              (o) => { return o.Diagram.CommandHandler.CanCopySelection(); }),
            makeButton("Delete",
              (e, obj) => { e.Diagram.CommandHandler.DeleteSelection(); },
              (o) => { return o.Diagram.CommandHandler.CanDeleteSelection(); }),
            new Shape("LineH") { StrokeWidth = 2, Height = 1, Stretch = Stretch.Horizontal },
            makeButton("Add top port",
              (e, obj) => { AddPort("Top"); }),
            makeButton("Add left port",
              (e, obj) => { AddPort("Left"); }),
            makeButton("Add right port",
              (e, obj) => { AddPort("Right"); }),
            makeButton("Add bottom port",
              (e, obj) => { AddPort("Bottom"); })
          );

      var portSize = new Size(8, 8);

      var portMenu = // context menu for each port
        Builder.Make<Adornment>("ContextMenu")
          .Add(
            makeButton("Swap order",
              (e, obj) => { SwapOrder((obj.Part as Adornment).AdornedElement); }),
            makeButton("Remove port",
              // in the click event handler, the obj.part is the Adornment;
              // its AdornedElement is the port
              (e, obj) => { RemovePort((obj.Part as Adornment).AdornedElement); }),
            makeButton("Change color",
              (e, obj) => { ChangeColor((obj.Part as Adornment).AdornedElement); }),
            makeButton("Remove side ports",
              (e, obj) => { RemoveAll((obj.Part as Adornment).AdornedElement); })
          );

      // the panels holding the port elements, which are themselves Panels,
      // created for each item in the ItemList, bound to Data.Left/Right/Top/BottomList
      Panel makePortPanel(string side, Spot spot, int row, int col) {
        var type = "Horizontal";
        var margin = new Margin(0, 1);
        if (side == "Left" || side == "Right") {
          type = "Vertical";
          margin = new Margin(1, 0);
        }

        return new Panel(type) {
          Row = row, Column = col,
          ItemTemplate =
              new Panel {
                FromSpot = spot, ToSpot = spot,
                FromLinkable = true, ToLinkable = true, Cursor = "pointer",
                ContextMenu = portMenu
              }
                .Set(new { _Side = side })  // internal property to make it easier to tell which side it's on
                .Bind("PortId")
                .Add(
                   new Shape("Rectangle") {
                     Stroke = null, StrokeWidth = 0,
                     DesiredSize = portSize,
                     Margin = margin
                   }
                     .Bind("Fill", "PortColor")
                )
        }
          .Bind("ItemList", side + "List");
      }

      // the node template
      // includes a panel on each side with an ItemList of panels containing ports
      _Diagram.NodeTemplate =
        new Node("Table") {
          LocationElementName = "BODY",
          LocationSpot = Spot.Center,
          SelectionElementName = "BODY",
          ContextMenu = nodeMenu
        }
          .Bind("Location", "Loc", Point.Parse, Point.Stringify)
          .Add(
            // the body
            new Panel("Auto") {
              Row = 1, Column = 1, Name = "BODY",
              Stretch = Stretch.Fill
            }
              .Add(
                new Shape("Rectangle") {
                  Fill = "#dbf6cb", Stroke = null, StrokeWidth = 0,
                  MinSize = new Size(65, 65)
                },
                new TextBlock {
                  Margin = 10, TextAlign = TextAlign.Center, Font = new Font("Segoe UI", 14, Northwoods.Go.FontWeight.Bold), Stroke = "#484848", Editable = true
                }
                  .Bind(new Binding("Text", "Name").MakeTwoWay())
              ),  // end panel body

            // left panel
            makePortPanel("Left", Spot.Left, 1, 0),
            // top panel
            makePortPanel("Top", Spot.Top, 0, 1),
            // right panel
            makePortPanel("Right", Spot.Right, 1, 2),
            // bottom panel
            makePortPanel("Bottom", Spot.Bottom, 2, 1)
          );

      // an orthogonal link template, reshapable and relinkable
      _Diagram.LinkTemplate =
        new CustomLink {
          Routing = LinkRouting.AvoidsNodes,
          Corner = 4,
          Curve = LinkCurve.JumpGap,
          Reshapable = true,
          Resegmentable = true,
          RelinkableFrom = true,
          RelinkableTo = true
        }
          .Bind(new Binding("Points").MakeTwoWay())
          .Add(new Shape { Stroke = "#2F4F4F", StrokeWidth = 2 });

      // support double-clicking in the background to add a copy of this data as a node
      _Diagram.ToolManager.ClickCreatingTool.ArchetypeNodeData =
        new NodeData {
          Name = "Unit",
          LeftList = new List<PortData>(),
          RightList = new List<PortData>(),
          TopList = new List<PortData>(),
          BottomList = new List<PortData>(),
        };

      _Diagram.ContextMenu =
        Builder.Make<Adornment>("ContextMenu")
          .Add(
            makeButton("Paste",
              (e, obj) => { e.Diagram.CommandHandler.PasteSelection(e.Diagram.ToolManager.ContextMenuTool.MouseDownPoint); },
              (o) => { return o.Diagram.CommandHandler.CanPasteSelection(o.Diagram.ToolManager.ContextMenuTool.MouseDownPoint); }),
            makeButton("Undo",
              (e, obj) => { e.Diagram.CommandHandler.Undo(); },
              (o) => { return o.Diagram.CommandHandler.CanUndo(); }),
            makeButton("Redo",
              (e, obj) => { e.Diagram.CommandHandler.Redo(); },
              (o) => { return o.Diagram.CommandHandler.CanRedo(); })
          );

      // load the diagram from JSON data
      LoadModel();
    }

    // Add a port to the specified side of the selected nodes.
    private void AddPort(string side) {
      _Diagram.StartTransaction("addPort");
      foreach (var n in _Diagram.Selection) {
        // skip any selected Links
        if (n is not Node node) continue;
        // compute the next available index number for the side
        var i = 0;
        while (node.FindPort(side + i.ToString()) != node) i++;
        // now this new port name is unique within the whole Node because of the side prefix
        var name = side + i.ToString();
        // get the list of port data to be modified
        List<PortData> arr = null;
        var nodedata = node.Data as NodeData;
        switch (side) {
          case "Left": arr = nodedata.LeftList; break;
          case "Right": arr = nodedata.RightList; break;
          case "Top": arr = nodedata.TopList; break;
          case "Bottom": arr = nodedata.BottomList; break;
        }
        if (arr != null) {
          // create a new port data object
          var newportdata = new PortData {
            PortId = name,
            PortColor = GetPortColor()
          };
          // and add it to the list of port data
          _Diagram.Model.InsertListItem(arr, -1, newportdata);
        }
      }
      _Diagram.CommitTransaction("addPort");
    }

    // Exchange the position/order of the given port with the next one.
    // If it's the last one, swap with the previous one.
    private void SwapOrder(GraphObject port) {
      var arr = port.Panel.ItemList as List<PortData>;
      if (arr.Count >= 2) {  // only if there are at least two ports!
        for (var i = 0; i < arr.Count; i++) {
          if (arr[i].PortId == port.PortId) {
            _Diagram.StartTransaction("swap ports");
            if (i >= arr.Count - 1) i--;  // now can swap I and I+1, even if it's the last port
            var newarr = new List<PortData>(arr);  // copy list
            // swap items
            newarr[i] = arr[i + 1];
            newarr[i + 1] = arr[i];
            // remember the new list in the model
            _Diagram.Model.Set(port.Part.Data, port["_Side"] + "List", newarr);
            foreach (var l in ((Node)port.Part).FindLinksConnected(newarr[i].PortId)) l.InvalidateRoute();
            foreach (var l in ((Node)port.Part).FindLinksConnected(newarr[i + 1].PortId)) l.InvalidateRoute();
            _Diagram.CommitTransaction("swap ports");
            break;
          }
        }
      }
    }

    // Remove the clicked port from the node.
    // Links to the port will be redrawn to the node's shape.
    private void RemovePort(GraphObject port) {
      _Diagram.StartTransaction("removePort");
      var pid = port.PortId;
      var arr = port.Panel.ItemList as List<PortData>;
      for (var i = 0; i < arr.Count; i++) {
        if (arr[i].PortId == pid) {
          _Diagram.Model.RemoveListItem(arr, i);
          break;
        }
      }
      _Diagram.CommitTransaction("removePort");
    }

    // Remove all ports from the same side of the node as the clicked port.
    private void RemoveAll(GraphObject port) {
      _Diagram.StartTransaction("removePorts");
      var nodedata = port.Part.Data;
      var side = port["_Side"];  // there are four property names, all ending in "List"
      _Diagram.Model.Set(nodedata, side + "List", new List<PortData>());  // an empty list
      _Diagram.CommitTransaction("removePorts");
    }

    // Change the color of the clicked port.
    private void ChangeColor(GraphObject port) {
      _Diagram.StartTransaction("colorPort");
      var data = (port as Panel).Data;
      _Diagram.Model.Set(data, "PortColor", GetPortColor());
      _Diagram.CommitTransaction("colorPort");
    }

    // Use some pastel colors for ports
    private string GetPortColor() {
      var rand = new Random();
      var portColors = new string[] { "#fae3d7", "#d6effc", "#ebe3fc", "#eaeef8", "#fadfe5", "#6cafdb", "#66d6d1" };
      return portColors[rand.Next(portColors.Length)];
    }

    // Show the diagram's model in JSON format that the user may edit
    private void SaveModel() {
      if (_Diagram == null) return;
      modelJson1.JsonText = _Diagram.Model.ToJson();
    }

    private void LoadModel() {
      if (_Diagram == null) return;
      _Diagram.Model = Model.FromJson<Model>(modelJson1.JsonText);
    }

    // define the model data
    public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }

    public class NodeData : Model.NodeData {
      public string Loc { get; set; }
      public string Name { get; set; }
      public List<PortData> LeftList { get; set; }
      public List<PortData> RightList { get; set; }
      public List<PortData> TopList { get; set; }
      public List<PortData> BottomList { get; set; }

      // override clone to deep clone the above Lists
      public override object Clone() {
        var newdata = base.Clone() as NodeData;

        // deep copy lists
        var leftList = new List<PortData>();
        foreach (var pd in LeftList) {
          leftList.Add(pd.Clone() as PortData);
        }
        newdata.LeftList = leftList;
        var rightList = new List<PortData>();
        foreach (var pd in RightList) {
          rightList.Add(pd.Clone() as PortData);
        }
        newdata.RightList = rightList;
        var topList = new List<PortData>();
        foreach (var pd in TopList) {
          topList.Add(pd.Clone() as PortData);
        }
        newdata.TopList = topList;
        var bottomList = new List<PortData>();
        foreach (var pd in BottomList) {
          bottomList.Add(pd.Clone() as PortData);
        }
        newdata.BottomList = bottomList;

        return newdata;
      }
    }

    public class LinkData : Model.LinkData {
      public List<Point> Points { get; set; }
    }

    public class PortData : ICloneable {
      public string PortColor { get; set; }
      public string PortId { get; set; }

      public object Clone() => MemberwiseClone();
    }

    // This custom-routing Link class tries to separate parallel links from each other.
    // This assumes that ports are lined up in a row/column on a side of the node.
    public class CustomLink : Link {
      private (int Idx, int Count) FindSidePortIndexAndCount(Node node, Panel port) {
        var nodedata = node.Data as NodeData;
        if (nodedata != null) {
          var portdata = port.Data as PortData;
          var side = port["_Side"];
          List<PortData> arr;
          switch (side) {
            case "Left": arr = nodedata.LeftList; break;
            case "Right": arr = nodedata.RightList; break;
            case "Top": arr = nodedata.TopList; break;
            case "Bottom": arr = nodedata.BottomList; break;
            default: return (-1, -1);
          }
          var len = arr.Count;
          for (var i = 0; i < len; i++) {
            var d = arr[i];
            if (d == portdata) return (i, len);
          }
        }
        return (-1, -1);
      }

      public override double ComputeEndSegmentLength(Node node, GraphObject port, Spot spot, bool from) {
        var esl = base.ComputeEndSegmentLength(node, port, spot, from);
        var other = GetOtherPort(port);
        if (port != null && other != null) {
          var thispt = port.GetDocumentPoint(ComputeSpot(from));
          var otherpt = other.GetDocumentPoint(ComputeSpot(!from));
          if (Math.Abs(thispt.X - otherpt.X) > 20 || Math.Abs(thispt.Y - otherpt.Y) > 20) {
            var (idx, count) = FindSidePortIndexAndCount(node, port as Panel);
            if (port["_Side"] as string == "Top" || port["_Side"] as string == "Bottom") {
              if (otherpt.X < thispt.X) {
                return esl + 4 + idx * 8;
              } else {
                return esl + (count - idx - 1) * 8;
              }
            } else {  // left or right
              if (otherpt.Y < thispt.Y) {
                return esl + 4 + idx * 8;
              } else {
                return esl + (count - idx - 1) * 8;
              }
            }
          }
        }
        return esl;
      }

      public override bool HasCurviness() {
        if (double.IsNaN(Curviness)) return true;
        return base.HasCurviness();
      }

      public override double ComputeCurviness() {
        if (double.IsNaN(Curviness)) {
          var fromnode = FromNode;
          var fromport = FromPort;
          var fromspot = ComputeSpot(true);
          var frompt = fromport.GetDocumentPoint(fromspot);
          var tonode = ToNode;
          var toport = ToPort;
          var tospot = ComputeSpot(false);
          var topt = toport.GetDocumentPoint(tospot);
          if (Math.Abs(frompt.X - topt.X) > 20 || Math.Abs(frompt.Y - topt.Y) > 20) {
            if ((fromspot.Equals(Spot.Left) || fromspot.Equals(Spot.Right)) &&
                (tospot.Equals(Spot.Left) || tospot.Equals(Spot.Right))) {
              var fromseglen = ComputeEndSegmentLength(fromnode, fromport, fromspot, true);
              var toseglen = ComputeEndSegmentLength(tonode, toport, tospot, false);
              var c = (fromseglen - toseglen) / 2;
              if (frompt.X + fromseglen >= topt.X - toseglen) {
                if (frompt.Y < topt.Y) return c;
                if (frompt.Y > topt.Y) return -c;
              }
            } else if ((fromspot.Equals(Spot.Top) || fromspot.Equals(Spot.Bottom)) &&
                       (tospot.Equals(Spot.Top) || tospot.Equals(Spot.Bottom))) {
              var fromseglen = ComputeEndSegmentLength(fromnode, fromport, fromspot, true);
              var toseglen = ComputeEndSegmentLength(tonode, toport, tospot, false);
              var c = (fromseglen - toseglen) / 2;
              if (frompt.X + fromseglen >= topt.X - toseglen) {
                if (frompt.Y < topt.Y) return c;
                if (frompt.Y > topt.Y) return -c;
              }
            }
          }
        }
        return base.ComputeCurviness();
      }
    }
  }
}
