using System;
using System.Collections.Generic;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;
using System.ComponentModel;
using Northwoods.Go.Extensions;

namespace WinFormsSampleControls.OrgChartEditor {
  [ToolboxItem(false)]
  public partial class OrgChartEditorControl : System.Windows.Forms.UserControl {
    private Diagram _Diagram;
    private Inspector _Inspector;


    public OrgChartEditorControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      zoomFitBtn.Click += (e, obj) => ZoomToFit();
      centerRootBtn.Click += (e, obj) => CenterRoot();
      saveLoadModel1.SaveClick += (e, obj) => SaveModel();
      saveLoadModel1.LoadClick += (e, obj) => LoadModel();

      saveLoadModel1.ModelJson = @"{
  ""NodeDataSource"": [
    {""Key"":1, ""Name"":""Stella Payne Diaz"", ""Title"":""CEO""},
    {""Key"":2, ""Name"":""Luke Warm"", ""Title"":""VP Marketing/Sales"", ""Parent"":1},
    {""Key"":3, ""Name"":""Meg Meehan Hoffa"", ""Title"":""Sales"", ""Parent"":2},
    {""Key"":4, ""Name"":""Peggy Flaming"", ""Title"":""VP Engineering"", ""Parent"":1},
    {""Key"":5, ""Name"":""Saul Wellingood"", ""Title"":""Manufacturing"", ""Parent"":4},
    {""Key"":6, ""Name"":""Al Ligori"", ""Title"":""Marketing"", ""Parent"":2},
    {""Key"":7, ""Name"":""Dot Stubadd"", ""Title"":""Sales Rep"", ""Parent"":3},
    {""Key"":8, ""Name"":""Les Ismore"", ""Title"":""Project Mgr"", ""Parent"":5},
    {""Key"":9, ""Name"":""April Lynn Parris"", ""Title"":""Events Mgr"", ""Parent"":6},
    {""Key"":10, ""Name"":""Xavier Breath"", ""Title"":""Engineering"", ""Parent"":4},
    {""Key"":11, ""Name"":""Anita Hammer"", ""Title"":""Process"", ""Parent"":5},
    {""Key"":12, ""Name"":""Billy Aiken"", ""Title"":""Software"", ""Parent"":10},
    {""Key"":13, ""Name"":""Stan Wellback"", ""Title"":""Testing"", ""Parent"":10},
    {""Key"":14, ""Name"":""Marge Innovera"", ""Title"":""Hardware"", ""Parent"":10},
    {""Key"":15, ""Name"":""Evan Elpus"", ""Title"":""Quality"", ""Parent"":5},
    {""Key"":16, ""Name"":""Lotta B. Essen"", ""Title"":""Sales Rep"", ""Parent"":3}
  ]
}";

      goWebBrowser1.Html = @"
        <p>
          This editable organizational chart sample color-codes the Nodes according to the tree level in the hierarchy.
        </p>
        <p>
          Double click on a node in order to add a person or the diagram background to add a new boss. Double clicking the diagram uses the <a>ClickCreatingTool</a>
          with a custom <a>ClickCreatingTool.InsertPart</a> to scroll to the new node and start editing the <a>TextBlock</a> for its name .
        </p>
        <p>
          Drag a node onto another in order to change relationships.
          You can also draw a link from a node's background to other nodes that have no ""boss"". Links can also be relinked to change relationships.
          Right-click or tap-hold a Node to bring up a context menu which allows you to:
          <ul>
              <li>Vacate Position - remove the information specfic to the current person in that role</li>
              <li>Remove Role - removes the role entirely and reparents any children</li>
              <li>Remove Department - removes the role and the whole subtree</li>
          </ul>
          Deleting a Node or Link will orphan the child Nodes and generate a new tree. A custom SelectionDeleting <a>DiagramEvent</a> listener will clear out the boss info
          when the parent is removed.
        </p>
        <p>
          Select a node to edit/update node data values. This sample uses the <a href=""https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Input/DataInspector/DataInspector.cs"">Data Inspector</a> extension to display and modify Part data.
        </p>
        <p>
          To learn how to build an org chart from scratch with GoDiagram, see the <a href=""learn/index.html"">Getting Started tutorial</a>.
        </p>
        <p>
          If you want to have some ""assistant"" nodes on the side, above the regular reports,
          see the <a href=""OrgChartAssistants"">Org Chart Assistants</a> sample, which is a copy of this sample
          that uses a custom <a>TreeLayout</a> to position ""assistants"" that way.
        </p>
      ";
    }

    private void Setup() {
      _Diagram = diagramControl1.Diagram;

      _Diagram.MaxSelectionCount = 1;
      _Diagram.ValidCycle = CycleMode.DestinationTree;
      // custom click creating tool
      _Diagram.ToolManager.ClickCreatingTool = new OrgChartEditorClickCreatingTool {
        ArchetypeNodeData = new NodeData {
          Name = "(new person)",
          Title = "",
          Comments = ""
        }
      };
      // layout
      _Diagram.Layout = new OrgChartEditorTreeLayout {
        TreeStyle = TreeStyle.LastParents,
        Arrangement = TreeArrangement.Horizontal,
        // properties for most of the tree:
        Angle = 90,
        LayerSpacing = 35,
        // properties for the "last parents":
        AlternateAngle = 90,
        AlternateLayerSpacing = 35,
        AlternateAlignment = TreeAlignment.Bus,
        AlternateNodeSpacing = 20
      };
      _Diagram.UndoManager.IsEnabled = true; // enable undo and redo


      // manage boss info manually when a node or link is deleted from the diagram
      _Diagram.SelectionDeleting += (obj, e) => {
        var part = (e.Subject as HashSet<Part>).First();  // e.Subject is the _Diagram.Selection collection,
        // so we'll get the first since we know we only have one selection
        _Diagram.StartTransaction("clear boss");
        if (part is Node n) {
          var children = n.FindTreeChildrenNodes();  // find all child nodes
          foreach (var child in children) {  // now iterate through them and clear out the boss information
            var bossText = child.FindElement("boss") as TextBlock; // since the boss TextBlock is named, we can access it by name
            if (bossText == null) return;
            bossText.Text = "";
          }
        } else if (part is Link l) {
          var child = l.ToNode;
          var bossText = child.FindElement("boss") as TextBlock; // since the boss TextBlock is named, we can access it by name
          if (bossText == null) return;
          bossText.Text = "";
        }
        _Diagram.CommitTransaction("clear boss");
      };

      void NodeDoubleClick(InputEvent e, GraphObject obj) {
        var clicked = obj.Part;
        if (clicked != null) {
          var thisemp = clicked.Data as NodeData;
          _Diagram.StartTransaction("add employee");
          var newemp = new NodeData {
            Name = "(new person)",
            Title = "",
            Comments = "",
            Parent = thisemp.Key
          };
          _Diagram.Model.AddNodeData(newemp);
          _Diagram.CommitTransaction("add employee");
        }
      }

      // this is used to determine feedback during drags
      bool MayWorkFor(Node node1, Node node2) {
        if (!(node1 is Node)) return false;  // must be a Node
        if (node1 == node2) return false;  // cannot work for yourself
        if (node2.IsInTreeOf(node1)) return false;  // cannot work for someone who works for you
        return true;
      }

      // Provides a common style for most of the TextBlocks.
      var textStyle = new {
        Font = new Font("Segoe UI", 12), Stroke = "white"
      };

      // This converter is used by the Picture.
      string FindHeadShot(object keyAsObj, object _) {
        var key = (keyAsObj as int? ?? int.MinValue);
        if (key < 0 || key > 16) return "HSnopic"; // There are only 16 images
        return "hs" + key;
      }

      // define the Node template
      _Diagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance) {
          DoubleClick = NodeDoubleClick,
          // handle dragging a Node onto a Node to (maybe) change the reporting relationship
          MouseDragEnter = (e, nodeAsObj, prev) => {
            var node = nodeAsObj as Node;
            var diagram = node.Diagram;
            var selnode = diagram.Selection.First() as Node;
            if (!MayWorkFor(selnode, node)) return;
            var shape = node.FindElement("SHAPE") as Shape;
            if (shape != null) {
              shape["_PrevFill"] = shape.Fill;  // remember the original brush
              shape.Fill = "darkred";
            }
          },
          MouseDragLeave = (e, nodeAsObj, next) => {
            var node = nodeAsObj as Node;
            var shape = node.FindElement("SHAPE") as Shape;
            if (shape != null && shape["_PrevFill"] != null) {
              shape.Fill = (Brush)shape["_PrevFill"];  // restore the original brush
            }
          },
          MouseDrop = (e, nodeAsObj) => {
            var node = nodeAsObj as Node;
            var diagram = node.Diagram;
            var selnode = diagram.Selection.First() as Node;  // assume just one Node in selection
            if (MayWorkFor(selnode, node)) {
              // find any existing link into the selected node
              var link = selnode.FindTreeParentLink();
              if (link != null) {  // reconnect any existing link
                link.FromNode = node;
              } else {  // else create a new link
                diagram.ToolManager.LinkingTool.InsertLink(node, node.Port, selnode, selnode.Port);
              }
            }
          }
        }
          .Bind(
            // for sorting, have the Node.Text be the data.Name
            new Binding("Text", "Name"),
            // bind the Part.LayerName to control the Node's layer depending on whether it isSelected
            new Binding("LayerName", "IsSelected", (sel, _) => { return (sel as bool? ?? false) ? "Foreground" : ""; }).OfElement()
          )
          .Add(
            // define the node's outer shape
            new Shape("Rectangle") {
              Name = "SHAPE", Fill = "#333333", Stroke = "white", StrokeWidth = 3.5,
              // set the port properties:
              PortId = "", FromLinkable = true, ToLinkable = true, Cursor = "pointer"
            },
            new Panel(PanelLayoutHorizontal.Instance)
              .Add(
                new Picture {
                  Name = "Picture",
                  DesiredSize = new Size(80, 80),
                  Margin = 1.5
                }
                  .Bind("Source", "Key", FindHeadShot),
                // define the panel where the text will appear
                new Panel(PanelLayoutTable.Instance) {
                  MinSize = new Size(130, double.NaN),
                  MaxSize = new Size(150, double.NaN),
                  Margin = new Margin(6, 10, 0, 6),
                  DefaultAlignment = Spot.Left
                }
                  .Add(new ColumnDefinition { Column = 2, Width = 4 })
                  .Add(
                    new TextBlock { // the name
                      Row = 0, Column = 0, ColumnSpan = 5,
                      Font = new Font("Segoe UI", 16),
                      Stroke = "white",
                      Editable = true, IsMultiline = false,
                      MinSize = new Size(10, 16)
                    }
                      .Bind(new Binding("Text", "Name").MakeTwoWay()),
                    new TextBlock("Title: ") { Row = 1, Column = 0 }
                      .Set(textStyle),
                    new TextBlock {
                      Row = 1, Column = 1, ColumnSpan = 4,
                      Editable = true, IsMultiline = false,
                      MinSize = new Size(10, 14),
                      Margin = new Margin(0, 0, 0, 3)
                    }
                      .Set(textStyle)
                      .Bind(new Binding("Text", "Title").MakeTwoWay()),
                    new TextBlock { Row = 2, Column = 0 }
                      .Set(textStyle)
                      .Bind("Text", "Key", (v, _) => { return "ID: " + v.ToString(); }),
                    new TextBlock { Name = "boss", Row = 2, Column = 3 }  // we include a name so we can access this TextBlock when deleting Nodes/Links
                      .Set(textStyle)
                      .Bind("Text", "Parent", (v, _) => { return (int)v == 0 ? "" : "Boss: " + v.ToString(); }),
                    new TextBlock {  // the comments
                      Row = 3, Column = 0, ColumnSpan = 5,
                      Font = new Font("Segoe UI", 12, FontStyle.Italic),
                      Stroke = "white",
                      Wrap = Wrap.Fit,
                      Editable = true,  // by default newlines are allowed
                      MinSize = new Size(10, 14)
                    }
                      .Bind(new Binding("Text", "Comments").MakeTwoWay())
                  )  // end Table Panel
              ) // end Horizontal Panel
          );  // end Node

      // the context menu allows users to make a position vacant,
      // remove a role and reassign the subtree, or remove a department
      _Diagram.NodeTemplate.ContextMenu =
        Builder.Make<Adornment>("ContextMenu")
          .Add(
            Builder.Make<Panel>("ContextMenuButton")
              .Add(new TextBlock("Vacate Position"))
              .Set(
                new {
                  Click = new Action<InputEvent, GraphObject>((e, obj) => {
                    var node = (obj.Part as Adornment).AdornedPart as Node;
                    if (node != null) {
                      var thisemp = node.Data as NodeData;
                      _Diagram.StartTransaction("vacate");
                      // update the key, name, and comments
                      _Diagram.Model.Set(thisemp, "Name", "(Vacant)");
                      _Diagram.Model.Set(thisemp, "Comments", "");
                      _Diagram.CommitTransaction("vacate");
                    }
                  })
                }
              ),
            Builder.Make<Panel>("ContextMenuButton")
              .Add(new TextBlock("Remove Role"))
              .Set(
                new {
                  Click = new Action<InputEvent, GraphObject>((e, obj) => {
                    // reparent the subtree to this node's boss, then remove the node
                    var node = (obj.Part as Adornment).AdornedPart as Node;
                    if (node != null) {
                      _Diagram.StartTransaction("reparent remove");
                      var chl = node.FindTreeChildrenNodes();
                      // iterate through the children and set their parent key to our selected node's parent key
                      foreach (var emp in chl) {
                        var data = emp.Data as NodeData;
                        var pdata = node.FindTreeParentNode().Data as NodeData;
                        (_Diagram.Model as Model).SetParentKeyForNodeData(data, pdata.Key);
                      }
                      // and now remove the selected node itself
                      _Diagram.Model.RemoveNodeData(node.Data);
                      _Diagram.CommitTransaction("reparent remove");
                    }
                  })
                }
              ),
            Builder.Make<Panel>("ContextMenuButton")
              .Add(new TextBlock("Remove Department"))
              .Set(
                new {
                  Click = new Action<InputEvent, GraphObject>((e, obj) => {
                    // remove the whole subtree, including the node itself
                    var node = (obj.Part as Adornment).AdornedPart as Node;
                    if (node != null) {
                      _Diagram.StartTransaction("remove dept");
                      _Diagram.RemoveParts(node.FindTreeParts(), true);
                      _Diagram.CommitTransaction("remove dept");
                    }
                  })
                }
              )
          );

      // define the Link template
      _Diagram.LinkTemplate =
        new Link { Routing = LinkRouting.Orthogonal, Corner = 5, RelinkableFrom = true, RelinkableTo = true }
          .Add(new Shape { StrokeWidth = 1.5, Stroke = "#F5F5F5" });  // the link shape

      LoadModel();

      _Inspector = new Inspector(inspectorControl1, _Diagram, new Inspector.Options {
        IncludesOwnProperties = false,
        Properties = new Dictionary<string, Inspector.PropertyOptions> {
          { "Key", new Inspector.PropertyOptions { ReadOnly = true } },
          { "Name", new Inspector.PropertyOptions { Show = Inspector.ShowIfNode } },
          { "Title", new Inspector.PropertyOptions { Show = Inspector.ShowIfNode } },
          { "Parent", new Inspector.PropertyOptions { Show = Inspector.ShowIfNode } },
          { "Comments", new Inspector.PropertyOptions { Show = Inspector.ShowIfNode } },
        }
      });
    }

    private void ZoomToFit() {
      _Diagram.CommandHandler.ZoomToFit();
    }

    private void CenterRoot() {
      _Diagram.Scale = 1;
      _Diagram.CommandHandler.ScrollToPart(_Diagram.FindNodeForKey(1));
    }

    private void SaveModel() {
      if (_Diagram == null) return;
      saveLoadModel1.ModelJson = _Diagram.Model.ToJson();
    }

    private void LoadModel() {
      if (_Diagram == null) return;
      var model = Model.FromJson<Model>(saveLoadModel1.ModelJson);
      var lastkey = 1;
      model.MakeUniqueKeyFunction = (model, data) => {
        var k = data.Key;
        if (k == 0) k = lastkey;  // no 0 keys
        while (model.FindNodeDataForKey(k) != null) k++;
        data.Key = lastkey = k;
        return k;
      };
      _Diagram.Model = model;
    }
  }

  // define the model data
  public class Model : TreeModel<NodeData, int, object> { }
  public class NodeData : Model.NodeData {
    public string Name { get; set; }
    public string Title { get; set; }
    public string Comments { get; set; } = "";
  }

  // override TreeLayout.CommitNodes to also modify the background brush based on the tree depth level
  public class OrgChartEditorTreeLayout : TreeLayout {
    private static string[] levelColors = new string[] {
        "#AC193D", "#2672EC", "#8C0095", "#5133AB",
        "#008299", "#D24726", "#008A00", "#094AB2"
      };
    protected override void CommitNodes() {
      base.CommitNodes(); // standard behavior
      // then go through all of the vertexes and set their corresponding node's Shape.Fill
      // to a brush dependent on the TreeVertex.Level value
      foreach (var v in (Diagram.Layout as TreeLayout).Network.Vertexes) {
        if (v.Node != null) {
          var level = v.Level % (levelColors.Length);
          var color = levelColors[level];
          var shape = v.Node.FindElement("SHAPE") as Shape;
          if (shape != null) {
            shape.Stroke = new Brush(new LinearGradientPaint(
              new Dictionary<float, string> {
                { 0, color },
                { 1, Brush.LightenBy(color, 0.05) }
              }, Spot.Left, Spot.Right));
          }
        }
      }
    }
  }

  // extend click creating tool
  public class OrgChartEditorClickCreatingTool : ClickCreatingTool {
    public override Part InsertPart(Point loc) {
      var node = base.InsertPart(loc);
      if (node != null) {
        Diagram.Select(node);
        Diagram.CommandHandler.ScrollToPart(node);
        Diagram.CommandHandler.EditTextBlock(node.FindElement("NAMETB") as TextBlock);
      }
      return node;
    }
  }
}
