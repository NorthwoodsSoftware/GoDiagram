/* Copyright (c) Northwoods Software Corporation. */

using System.Collections.Generic;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Extensions;
using Northwoods.Go.Models;
using Northwoods.Go.PanelLayouts;
using Northwoods.Go.Tools;
using Northwoods.Go.Tools.Extensions;

namespace Demo.Samples.SystemDynamics; 
public partial class SystemDynamics : DemoControl {
  private Diagram _Diagram;
  public static AppStatusData _Status = new AppStatusData();

  public SystemDynamics() {
    InitializeComponent();
    _Diagram = diagramControl1.Diagram;

    pointerBtn.Click += (e, obj) => SetMode("pointer", "pointer");
    stockBtn.Click += (e, obj) => SetMode("node", "stock");
    cloudBtn.Click += (e, obj) => SetMode("node", "cloud");
    variableBtn.Click += (e, obj) => SetMode("node", "variable");
    flowBtn.Click += (e, obj) => SetMode("link", "flow");
    influenceBtn.Click += (e, obj) => SetMode("link", "influence");

    modelJson1.SaveClick = SaveModel;
    modelJson1.LoadClick = LoadModel;

    desc1.MdText = DescriptionReader.Read("Samples.SystemDynamics.md");

    modelJson1.JsonText = @"{
  ""linkLabelKeysProperty"": ""LabelKeys"",
  ""nodeDataSource"": [
    { ""key"": ""grass"", ""category"": ""stock"", ""label"": ""Grass"", ""loc"": ""30 220"", ""labelOffset"": ""0.5 0.5 0 30"" },
    { ""key"": ""cloud1"", ""category"": ""cloud"", ""loc"": ""200 220"" },
    { ""key"": ""sheep"", ""category"": ""stock"", ""label"": ""Sheep"", ""loc"": ""30 20"", ""labelOffset"": ""0.5 0.5 0 -30"" },
    { ""key"": ""cloud2"", ""category"": ""cloud"", ""loc"": ""200 20"" },
    { ""key"": ""cloud3"", ""category"": ""cloud"", ""loc"": ""-150 220"" },
    { ""key"": ""grass_loss"", ""category"": ""valve"", ""label"": ""grass_loss"", ""labelOffset"": ""0.5 0.5 0 20""  },
    { ""key"": ""grazing"", ""category"": ""valve"", ""label"": ""grazing"", ""labelOffset"": ""0.5 0.5 45 0""  },
    { ""key"": ""growth"", ""category"": ""valve"", ""label"": ""growth"", ""labelOffset"": ""0.5 0.5 0 20""  },
    { ""key"": ""sheep_loss"", ""category"": ""valve"", ""label"": ""sheep_loss"", ""labelOffset"": ""0.5 0.5 0 20""  },
    { ""key"": ""k1"", ""category"": ""variable"", ""label"": ""good weather"", ""loc"": ""-80 100"" },
    { ""key"": ""k2"", ""category"": ""variable"", ""label"": ""bad weather"", ""loc"": ""100 150"" },
    { ""key"": ""k3"", ""category"": ""variable"", ""label"": ""wolves"", ""loc"": ""150 -40"" }

  ],
  ""linkDataSource"": [
    { ""from"": ""grass"", ""to"": ""cloud1"", ""category"": ""flow"", ""labelKeys"": [ ""grass_loss"" ]},
    { ""from"": ""sheep"", ""to"": ""cloud2"", ""category"": ""flow"", ""labelKeys"": [ ""sheep_loss"" ]},
    { ""from"": ""grass"", ""to"": ""sheep"", ""category"": ""flow"", ""labelKeys"": [ ""grazing"" ]},
    { ""from"": ""cloud3"", ""to"": ""grass"", ""category"": ""flow"", ""labelKeys"": [ ""growth"" ]},
    { ""from"": ""grass"", ""to"": ""grass_loss"", ""category"": ""influence""},
    { ""from"": ""sheep"", ""to"": ""sheep_loss"", ""category"": ""influence""},
    { ""from"": ""grass"", ""to"": ""growth"", ""category"": ""influence""},
    { ""from"": ""grass"", ""to"": ""grazing"", ""category"": ""influence""},
    { ""from"": ""sheep"", ""to"": ""grazing"", ""category"": ""influence""},
    { ""from"": ""k1"", ""to"": ""growth"", ""category"": ""influence""},
    { ""from"": ""k1"", ""to"": ""grazing"", ""category"": ""influence""},
    { ""from"": ""k2"", ""to"": ""grass_loss"", ""category"": ""influence""},
    { ""from"": ""k3"", ""to"": ""sheep_loss"", ""category"": ""influence""}
  ]
}";

    Setup();
  }

  private void Setup() {
    // load extra figures
    Figures.DefineExtraFigures();

    _Diagram.UndoManager.IsEnabled = true;
    _Diagram.AllowLink = false; // linking is only started via buttons, not modelessly
    _Diagram.AnimationManager.IsEnabled = false;

    _Diagram.ToolManager.LinkingTool = new CustomLinkingTool {
      PortGravity = 0 // no snapping while drawing new links
    };

    _Diagram.ToolManager.ClickCreatingTool = new CustomClickCreatingTool {
      ArchetypeNodeData = new NodeData(), // enable ClickCreatingTool
      IsDoubleClick = false // operates on a single click in the background
      // but only in "node" creation mode
    };

    // install the NodeLabelDraggingTool as a "mouse move" tool
    _Diagram.ToolManager.MouseMoveTools.Insert(0, new NodeLabelDraggingTool());

    // generate unique label from valve on newly-created flow link
    _Diagram.LinkDrawn += (s, e) => {
      var link = e.Subject as Link;
      if (link.Category == "flow") {
        _Diagram.StartTransaction("UpdateNode");
        _Status.NodeCounter["valve"] = _Status.NodeCounter["valve"] + 1;
        var newNodeId = "flow" + _Status.NodeCounter["valve"];
        var labelNode = link.LabelNodes.First();
        _Diagram.Model.Set(labelNode.Data, "Label", newNodeId);
        _Diagram.CommitTransaction("UpdateNode");
      }
    };

    BuildTemplates();

    LoadModel();
  }

  private void BuildTemplates() {
    // helpers for the templates
    void nodeStyle(Node node) {
      node.Type = PanelLayoutSpot.Instance;
      node.LayerName = "Background";
      node.LocationElementName = "SHAPE";
      node.SelectionElementName = "SHAPE";
      node.LocationSpot = Spot.Center;
      node.BindTwoWay("Location", "Loc", Point.Parse, Point.Stringify);
    }

    var shapeStyle = new {
      Name = "SHAPE",
      Stroke = "black",
      Fill = "#f0f0f0",
      PortId = "",  // So a link can be dragged from the Node
      FromLinkable = true,
      ToLinkable = true
    };

    var textStyle = new {
      Font = new Font("Arial", 11, Northwoods.Go.FontWeight.Bold),
      Margin = 2,
      Editable = true
    };
    void labelBind(TextBlock tb) {
      tb.BindTwoWay("Text", "Label");
    }

    // node templates
    _Diagram.NodeTemplateMap.Add("stock",
      new Node()
        .Apply(nodeStyle)
        .Add(
          new Shape { DesiredSize = new Size(50, 30) }
            .Set(shapeStyle),
          new TextBlock {
            Alignment = new Spot(0.5, 0.5, 0, 30)  // initial value
          }
            .Set(textStyle)
            .Set(new { _IsNodeLabel = true })  // declare draggable by NodeLabelDraggingTool
            .Apply(labelBind)
            .BindTwoWay("Alignment", "LabelOffset", Spot.Parse, Spot.Stringify)
        )
    );

    _Diagram.NodeTemplateMap.Add("cloud",
      new Node()
        .Apply(nodeStyle)
        .Add(
          new Shape("Cloud") { DesiredSize = new Size(35, 35) }
            .Set(shapeStyle)
        )
    );

    _Diagram.NodeTemplateMap.Add("valve",
      new Node() {
          Movable = false,
          AlignmentFocus = Spot.None
        }
        .Apply(nodeStyle)
        .Set(new { LayerName = "Foreground" })
        .Add(
          new Shape("Ellipse") { DesiredSize = new Size(20, 20) }
            .Set(shapeStyle),
          new TextBlock {
            Alignment = new Spot(0.5, 0.5, 0, 20)  // initial value
          }
            .Set(textStyle)
            .Set(new { _IsNodeLabel = true })  // declare draggable by NodeLabelDraggingTool
            .Apply(labelBind)
            .BindTwoWay("Alignment", "LabelOffset", Spot.Parse, Spot.Stringify)
        )

    );

    _Diagram.NodeTemplateMap.Add("variable",
      new Node()
       .Apply(nodeStyle)
       .Set(new { Type = PanelLayoutAuto.Instance })
       .Add(
          new TextBlock { IsMultiline = false }
            .Set(textStyle)
            .Apply(labelBind),
          new Shape { IsPanelMain = true }
            .Set(shapeStyle)
            // the port is in front and transparent, even though it goes around the text
            // in "link" mode will support drawing a new link
            .Set(new { Stroke = (Brush)null, Fill = "transparent" })
       )
    );

    // link templates
    _Diagram.LinkTemplateMap.Add("flow",
      new Link { ToShortLength = 8 }
        .Add(
          new Shape { Stroke = "blue", StrokeWidth = 5 },
          new Shape {
            Fill = "blue",
            Stroke = null,
            ToArrow = "Standard",
            Scale = 2.5
          }
        )
    );

    _Diagram.LinkTemplateMap.Add("influence",
      new Link { Curve = LinkCurve.Bezier, ToShortLength = 8 }
        .Add(
          new Shape { Stroke = "green", StrokeWidth = 1.5 },
          new Shape {
            Fill = "green",
            Stroke = null,
            ToArrow = "Standard",
            Scale = 1.5
          }
        )
    );
  }

  private void SetMode(string mode, string itemType) {
    _Diagram.StartTransaction("Mode Changed");
    _Status.Mode = mode;
    _Status.ItemType = itemType;
    if (mode == "pointer" || mode == "node") {
      _Diagram.AllowLink = false;
      foreach (var n in _Diagram.Nodes) {
        n.Port.Cursor = "";
      }
    } else if (mode == "link") {
      _Diagram.AllowLink = true;
      foreach (var n in _Diagram.Nodes) {
        n.Port.Cursor = "pointer";
      }
    }
    _Diagram.CommitTransaction("Mode Changed");
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
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Label { get; set; }
    public string Loc { get; set; }
    public string LabelOffset { get; set; }
  }
  public class LinkData : Model.LinkData {
    public List<string> LabelKeys { get; set; }
  }

  // override ClickCreatingTool
  public class CustomClickCreatingTool : ClickCreatingTool {
    public override bool CanStart() {
      var mode = _Status.Mode;
      return (mode == "node" && base.CanStart());
    }
    public override Part InsertPart(Point loc) {
      var itemType = _Status.ItemType;
      var nodeCounter = _Status.NodeCounter;
      nodeCounter[itemType] = nodeCounter[itemType] + 1;
      var newNodeId = itemType + nodeCounter[itemType];
      ArchetypeNodeData = new NodeData {
        Key = newNodeId,
        Category = itemType,
        Label = newNodeId
      };
      return base.InsertPart(loc);
    }
  }

  // override LinkingTool
  public class CustomLinkingTool : LinkingTool {
    public override void DoActivate() {
      var itemType = _Status.ItemType;
      TemporaryLink.Curve = (itemType == "flow" ? LinkCurve.None : LinkCurve.Bezier);
      TemporaryLink.Path.Stroke = (itemType == "flow" ? "blue" : "green");
      TemporaryLink.Path.StrokeWidth = (itemType == "flow" ? 5 : 1);
      base.DoActivate();
    }
    public override Link InsertLink(Node fromnode, GraphObject fromport, Node tonode, GraphObject toport) {
      var itemType = _Status.ItemType;
      // to control what kind of Link is created,
      // change the LinkingTool.ArchetypeLinkData's category
      ArchetypeLinkData = (itemType == "flow" ? new LinkData { Category = "flow" } : new LinkData { Category = "influence", LabelKeys = null });
      // Whenever a new Link is drawing by the LinkingTool, it also adds a node data object
      // that acts as the label node for the link, to allow links to be drawn to/from the link
      ArchetypeLabelNodeData = (itemType == "flow" ? new NodeData { Category = "valve" } : null);
      return base.InsertLink(fromnode, fromport, tonode, toport);
    }
  }

  public class AppStatusData {
    public string Mode = "pointer";  // "pointer", "node", "link" for
                                     // interacting, adding a new node or a new link respectively
    public string ItemType = "pointer";  // set when user clicks on a node or link button
    public Dictionary<string, int> NodeCounter = new() {
      { "stock", 0 },
      { "cloud", 0 },
      { "variable", 0 },
      { "valve", 0 }
    };
  }
}
