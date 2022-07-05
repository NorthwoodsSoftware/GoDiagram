/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Extensions;
using Northwoods.Go.Tools;
using System.Linq;
using Northwoods.Go.Tools.Extensions;

namespace WinFormsSampleControls.SystemDynamics {
  [ToolboxItem(false)]
  public partial class SystemDynamicsControl : System.Windows.Forms.UserControl {

    private Diagram myDiagram;

    public static AppStatusData SD = new AppStatusData();


    public SystemDynamicsControl() {
      InitializeComponent();

      pointerBtn.Click += (e, obj) => SetMode("pointer", "pointer");
      stockBtn.Click += (e, obj) => SetMode("node", "stock");
      cloudBtn.Click += (e, obj) => SetMode("node", "cloud");
      variableBtn.Click += (e, obj) => SetMode("node", "variable");
      flowBtn.Click += (e, obj) => SetMode("link", "flow");
      influenceBtn.Click += (e, obj) => SetMode("link", "influence");

      modelJson1.SaveClick += (e, obj) => SaveModel();
      modelJson1.LoadClick += (e, obj) => LoadModel();

      goWebBrowser1.Html = @"
        <p>
      A <em>system dynamics diagram</em> shows the storages (stocks) and flows of material in some system,
      and the factors that influence the rates of flow.
      It is usually a cosmetic interface for building mathematical models --
      you provide values and equations for the stocks and flows,
      and appropriate software can then simulate the system's behaiour.
       </p>
        <p>
      The diagram has two types of link: flow links and influence links.
      In additon to the node attached to each flow, there are 3 types of node:
          <ul>
        <li><b>stocks</b>, the amount of some substance</li>
        <li><b>clouds</b>, like stocks, but outside the system of interest</li>
        <li><b>variables</b>, either numeric constants or calculated from other elements</li>
          </ul>
        </p>
        <p>
      The conventional user interface for building system dynamics diagrams is modal --
      you select a tool in the toolbar, then either click in an empty part of the diagram to add a node
      or drag from one node to another to add a link.
      That is the approach used in this example, accomplished with the <a>ClickCreatingTool</a> and <a>LinkingTool</a>.
      Note that you need to click on the Pointer tool to revert to the normal mode.
        </p>
        <p>
      In addition to the above, the diagram also installs the <a href=""NodeLabelDragging"">NodeLabelDraggingTool</a>
      extension into <a>ToolManager.MouseMoveTools</a>.
        </p>
        <p>
      This sample is based on a prototype developed by Robert Muetzelfeldt.
        </p>
";

      modelJson1.JsonText = @"{
  ""LinkLabelKeysProperty"": ""LabelKeys"",
  ""NodeDataSource"": [
    { ""Key"": ""grass"", ""Category"": ""stock"", ""Label"": ""Grass"", ""Loc"": ""30 220"", ""LabelOffset"": ""0.5 0.5 0 30"" },
    { ""Key"": ""cloud1"", ""Category"": ""cloud"", ""Loc"": ""200 220"" },
    { ""Key"": ""sheep"", ""Category"": ""stock"", ""Label"": ""Sheep"", ""Loc"": ""30 20"", ""LabelOffset"": ""0.5 0.5 0 -30"" },
    { ""Key"": ""cloud2"", ""Category"": ""cloud"", ""Loc"": ""200 20"" },
    { ""Key"": ""cloud3"", ""Category"": ""cloud"", ""Loc"": ""-150 220"" },
    { ""Key"": ""grass_loss"", ""Category"": ""valve"", ""Label"": ""grass_loss"", ""LabelOffset"": ""0.5 0.5 0 20""  },
    { ""Key"": ""grazing"", ""Category"": ""valve"", ""Label"": ""grazing"", ""LabelOffset"": ""0.5 0.5 45 0""  },
    { ""Key"": ""growth"", ""Category"": ""valve"", ""Label"": ""growth"", ""LabelOffset"": ""0.5 0.5 0 20""  },
    { ""Key"": ""sheep_loss"", ""Category"": ""valve"", ""Label"": ""sheep_loss"", ""LabelOffset"": ""0.5 0.5 0 20""  },
    { ""Key"": ""k1"", ""Category"": ""variable"", ""Label"": ""good weather"", ""Loc"": ""-80 100"" },
    { ""Key"": ""k2"", ""Category"": ""variable"", ""Label"": ""bad weather"", ""Loc"": ""100 150"" },
    { ""Key"": ""k3"", ""Category"": ""variable"", ""Label"": ""wolves"", ""Loc"": ""150 -40"" }

  ],
  ""LinkDataSource"": [
    { ""From"": ""grass"", ""To"": ""cloud1"", ""Category"": ""flow"", ""LabelKeys"": [ ""grass_loss"" ]},
    { ""From"": ""sheep"", ""To"": ""cloud2"", ""Category"": ""flow"", ""LabelKeys"": [ ""sheep_loss"" ]},
    { ""From"": ""grass"", ""To"": ""sheep"", ""Category"": ""flow"", ""LabelKeys"": [ ""grazing"" ]},
    { ""From"": ""cloud3"", ""To"": ""grass"", ""Category"": ""flow"", ""LabelKeys"": [ ""growth"" ]},
    { ""From"": ""grass"", ""To"": ""grass_loss"", ""Category"": ""influence""},
    { ""From"": ""sheep"", ""To"": ""sheep_loss"", ""Category"": ""influence""},
    { ""From"": ""grass"", ""To"": ""growth"", ""Category"": ""influence""},
    { ""From"": ""grass"", ""To"": ""grazing"", ""Category"": ""influence""},
    { ""From"": ""sheep"", ""To"": ""grazing"", ""Category"": ""influence""},
    { ""From"": ""k1"", ""To"": ""growth"", ""Category"": ""influence""},
    { ""From"": ""k1"", ""To"": ""grazing"", ""Category"": ""influence""},
    { ""From"": ""k2"", ""To"": ""grass_loss"", ""Category"": ""influence""},
    { ""From"": ""k3"", ""To"": ""sheep_loss"", ""Category"": ""influence""}
  ]
}";

      Setup();
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // load extra figures
      Figures.DefineExtraFigures();

      // diagram properties
      myDiagram.UndoManager.IsEnabled = true;
      myDiagram.AllowLink = false; // linking is only started via buttons, not modelessly
      myDiagram.AnimationManager.IsEnabled = false;

      myDiagram.ToolManager.LinkingTool = new CustomLinkingTool {
        PortGravity = 0 // no snapping while drawing new links
      };

      myDiagram.ToolManager.ClickCreatingTool = new CustomClickCreatingTool {
        ArchetypeNodeData = new NodeData(), // enable ClickCreatingTool
        IsDoubleClick = false // operates on a single click in the background
        // but only in "node" creation mode
      };

      // install the NodeLabelDraggingTool as a "mouse move" tool
      myDiagram.ToolManager.MouseMoveTools.Insert(0, new NodeLabelDraggingTool());

      // generate unique label from valve on newly-created flow link
      myDiagram.LinkDrawn += (s, e) => {
        var link = e.Subject as Link;
        if (link.Category == "flow") {
          myDiagram.StartTransaction("UpdateNode");
          SD.NodeCounter["valve"] = SD.NodeCounter["valve"] + 1;
          var newNodeId = "flow" + SD.NodeCounter["valve"];
          var labelNode = link.LabelNodes.First();
          myDiagram.Model.Set(labelNode.Data, "Label", newNodeId);
          myDiagram.CommitTransaction("UpdateNode");
        }
      };

      BuildTemplates();

      LoadModel();
    }

    private void BuildTemplates() {
      // helpers for the templates
      var nodeStyle = new {
        Type = PanelLayoutSpot.Instance,
        LayerName = "Background",
        LocationElementName = "SHAPE",
        SelectionElementName = "SHAPE",
        LocationSpot = Spot.Center
      };
      Binding locBind() {
        return new Binding("Location", "Loc", Point.Parse, Point.Stringify);
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
        Font = new Font("Arial", 11, FontWeight.Bold),
        Margin = 2,
        Editable = true
      };
      Binding labelBind() {
        return new Binding("Text", "Label").MakeTwoWay();
      }

      // node templates
      myDiagram.NodeTemplateMap.Add("stock",
        new Node()
          .Set(nodeStyle)
          .Bind(locBind())
          .Add(
            new Shape { DesiredSize = new Size(50, 30) }
              .Set(shapeStyle),
            new TextBlock {
              Alignment = new Spot(0.5, 0.5, 0, 30)  // initial value
            }
              .Set(textStyle)
              .Set(new { _IsNodeLabel = true })  // declare draggable by NodeLabelDraggingTool
              .Bind(labelBind())
              .Bind("Alignment", "LabelOffset", Spot.Parse, Spot.Stringify)
          )
      );

      myDiagram.NodeTemplateMap.Add("cloud",
        new Node()
          .Set(nodeStyle)
          .Bind(locBind())
          .Add(
            new Shape("Cloud") { DesiredSize = new Size(35, 35) }
              .Set(shapeStyle)
          )
      );

      myDiagram.NodeTemplateMap.Add("valve",
        new Node() {
          Movable = false,
          AlignmentFocus = Spot.None
        }
          .Set(nodeStyle)
          .Set(new { LayerName = "Foreground" })
          .Bind(locBind())
          .Add(
            new Shape("Ellipse") { DesiredSize = new Size(20, 20) }
              .Set(shapeStyle),
            new TextBlock {
              Alignment = new Spot(0.5, 0.5, 0, 20)  // initial value
            }
              .Set(textStyle)
              .Set(new { _IsNodeLabel = true })  // declare draggable by NodeLabelDraggingTool
              .Bind(labelBind())
              .Bind("Alignment", "LabelOffset", Spot.Parse, Spot.Stringify)
          )

      );

      myDiagram.NodeTemplateMap.Add("variable",
        new Node()
         .Set(nodeStyle)
         .Set(new { Type = PanelLayoutAuto.Instance })
         .Bind(locBind())
         .Add(
            new TextBlock { IsMultiline = false }
              .Set(textStyle)
              .Bind(labelBind()),
            new Shape { IsPanelMain = true }
              .Set(shapeStyle)
              // the port is in front and transparent, even though it goes around the text
              // in "link" mode will support drawing a new link
              .Set(new { Stroke = (Brush)null, Fill = "transparent" })
         )
      );

      // link templates
      myDiagram.LinkTemplateMap.Add("flow",
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

      myDiagram.LinkTemplateMap.Add("influence",
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
      myDiagram.StartTransaction("Mode Changed");
      SystemDynamicsControl.SD.Mode = mode;
      SystemDynamicsControl.SD.ItemType = itemType;
      if (mode == "pointer" || mode == "node") {
        myDiagram.AllowLink = false;
        foreach (var n in myDiagram.Nodes) {
          n.Port.Cursor = "";
        }
      } else if (mode == "link") {
        myDiagram.AllowLink = true;
        foreach (var n in myDiagram.Nodes) {
          n.Port.Cursor = "pointer";
        }
      }
      myDiagram.CommitTransaction("Mode Changed");
    }

    // Show the diagram's model in JSON format that the user may edit
    private void SaveModel() {
      if (myDiagram == null) return;
      modelJson1.JsonText = myDiagram.Model.ToJson();
    }

    private void LoadModel() {
      if (myDiagram == null) return;
      myDiagram.Model = Model.FromJson<Model>(modelJson1.JsonText);
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
        var mode = SystemDynamicsControl.SD.Mode;
        return (mode == "node" && base.CanStart());
      }
      public override Part InsertPart(Point loc) {
        var itemType = SystemDynamicsControl.SD.ItemType;
        var nodeCounter = SystemDynamicsControl.SD.NodeCounter;
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
        var itemType = SystemDynamicsControl.SD.ItemType;
        TemporaryLink.Curve = (itemType == "flow" ? LinkCurve.None : LinkCurve.Bezier);
        TemporaryLink.Path.Stroke = (itemType == "flow" ? "blue" : "green");
        TemporaryLink.Path.StrokeWidth = (itemType == "flow" ? 5 : 1);
        base.DoActivate();
      }
      public override Link InsertLink(Node fromnode, GraphObject fromport, Node tonode, GraphObject toport) {
        var itemType = SystemDynamicsControl.SD.ItemType;
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
}
