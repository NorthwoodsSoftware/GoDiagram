/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;
using Northwoods.Go.Extensions;

namespace Demo.Samples.OrgChartEditor {
  public partial class OrgChartEditor : DemoControl {
    private Diagram _Diagram;
    private Inspector _Inspector;


    public OrgChartEditor() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      zoomFitBtn.Click += (e, obj) => ZoomToFit();
      centerRootBtn.Click += (e, obj) => CenterRoot();
      modelJson1.SaveClick = SaveModel;
      modelJson1.LoadClick = LoadModel;

      modelJson1.JsonText = @"{
  ""NodeDataSource"": [
    {""Key"":1, ""Name"":""Stella Payne Diaz"", ""Title"":""CEO"", ""Pic"": ""1.jpg"" },
    {""Key"":2, ""Name"":""Luke Warm"", ""Title"":""VP Marketing/Sales"", ""Pic"": ""2.jpg"", ""Parent"":1},
    {""Key"":3, ""Name"":""Meg Meehan Hoffa"", ""Title"":""Sales"", ""Pic"": ""3.jpg"", ""Parent"":2},
    {""Key"":4, ""Name"":""Peggy Flaming"", ""Title"":""VP Engineering"", ""Pic"": ""4.jpg"", ""Parent"":1},
    {""Key"":5, ""Name"":""Saul Wellingood"", ""Title"":""Manufacturing"", ""Pic"": ""5.jpg"", ""Parent"":4},
    {""Key"":6, ""Name"":""Al Ligori"", ""Title"":""Marketing"", ""Pic"": ""6.jpg"", ""Parent"":2},
    {""Key"":7, ""Name"":""Dot Stubadd"", ""Title"":""Sales Rep"", ""Pic"": ""7.jpg"", ""Parent"":3},
    {""Key"":8, ""Name"":""Les Ismore"", ""Title"":""Project Mgr"", ""Pic"": ""8.jpg"", ""Parent"":5},
    {""Key"":9, ""Name"":""April Lynn Parris"", ""Title"":""Events Mgr"", ""Pic"": ""9.jpg"", ""Parent"":6},
    {""Key"":10, ""Name"":""Xavier Breath"", ""Title"":""Engineering"", ""Pic"": ""10.jpg"", ""Parent"":4},
    {""Key"":11, ""Name"":""Anita Hammer"", ""Title"":""Process"", ""Pic"": ""11.jpg"", ""Parent"":5},
    {""Key"":12, ""Name"":""Billy Aiken"", ""Title"":""Software"", ""Pic"": ""12.jpg"", ""Parent"":10},
    {""Key"":13, ""Name"":""Stan Wellback"", ""Title"":""Testing"", ""Pic"": ""13.jpg"", ""Parent"":10},
    {""Key"":14, ""Name"":""Marge Innovera"", ""Title"":""Hardware"", ""Pic"": ""14.jpg"", ""Parent"":10},
    {""Key"":15, ""Name"":""Evan Elpus"", ""Title"":""Quality"", ""Pic"": ""15.jpg"", ""Parent"":5},
    {""Key"":16, ""Name"":""Lotta B. Essen"", ""Title"":""Sales Rep"", ""Pic"": ""16.jpg"", ""Parent"":3}
  ]
}";
      desc1.MdText = DescriptionReader.Read("Samples.OrgChartEditor.md");

      Setup();
    }

    private void Setup() {
      _Diagram.AllowCopy = false;
      _Diagram.AllowDelete = false;
      _Diagram.MaxSelectionCount = 1;  // users can select only one part at a time
      _Diagram.ValidCycle = CycleMode.DestinationTree;  // make sure users can only create trees
      // custom click creating tool
      _Diagram.ToolManager.ClickCreatingTool = new OrgChartEditorClickCreatingTool {
        ArchetypeNodeData = new NodeData {  // allow double-click in background to create a new node
          Name = "(new person)",
          Title = "",
          Comments = ""
        }
      };
      // layout
      _Diagram.Layout = new SideTreeLayout {
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

      // this is used to determine feedback during drags
      bool MayWorkFor(Node node1, Node node2) {
        if (!(node1 is Node)) return false;  // must be a Node
        if (node1 == node2) return false;  // cannot work for yourself
        if (node2.IsInTreeOf(node1)) return false;  // cannot work for someone who works for you
        return true;
      }

      // Provides a common style for most of the TextBlocks.
      var textStyle = new {
        Font = new Font("Segoe UI", 9, FontUnit.Point), Stroke = "white"
      };

      // This converter is used by the Picture.
      string FindHeadShot(object obj) {
        if (obj is not string pic || pic == "") return "https://nwoods.com/go/images/samples/HSnopic.png"; // There are only 16 images on the server
        return $"https://nwoods.com/go/images/samples/hs{pic}";
      }

      void AddEmployee(Node node) {
        if (node == null) return;
        var thisemp = node.Data as NodeData;
        _Diagram.Commit(d => {
          var newemp = new NodeData { Name = "(new person)", Title = "(title)", Comments = "", Parent = thisemp.Key };
          d.Model.AddNodeData(newemp);
          var newnode = d.FindNodeForData(newemp);
          if (newnode != null) newnode.Location = node.Location;
        }, "add employee");
      }

      // define the Node template
      _Diagram.NodeTemplate =
        new Node(PanelType.Spot) {
            SelectionElementName = "BODY",
            MouseEnter = (e, obj, prev) => {
              var node = obj as Node;
              node.FindElement("BUTTON").Opacity = node.FindElement("BUTTONX").Opacity = 1;
            },
            MouseLeave = (e, obj, prev) => {
              var node = obj as Node;
              node.FindElement("BUTTON").Opacity = node.FindElement("BUTTONX").Opacity = 0;
            },
            // handle dragging a Node onto a Node to (maybe) change the reporting relationship
            MouseDragEnter = (e, obj, prev) => {
              var node = obj as Node;
              var diagram = node.Diagram;
              var selnode = diagram.Selection.First() as Node;
              if (!MayWorkFor(selnode, node)) return;
              if (node.FindElement("SHAPE") is Shape shape) {
                shape["_PrevFill"] = shape.Fill;  // remember the original brush
                shape.Fill = "darkred";
              }
            },
            MouseDragLeave = (e, obj, next) => {
              var node = obj as Node;
              if (node.FindElement("SHAPE") is Shape shape && shape["_PrevFill"] != null) {
                shape.Fill = (Brush)shape["_PrevFill"];  // restore the original brush
              }
            },
            MouseDrop = (e, obj) => {
              var node = obj as Node;
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
            new Binding("LayerName", "IsSelected", (sel) => { return (bool)sel ? "Foreground" : ""; }).OfElement(),
            new Binding("IsTreeExpanded").MakeTwoWay()
          )
          .Add(
            new Panel(PanelType.Auto) { Name = "BODY" }
              .Add(
                // define the node's outer shape
                new Shape("Rectangle") {
                    Name = "SHAPE", Fill = "#333333", Stroke = "white", StrokeWidth = 3.5, PortId = ""
                  },
                new Panel(PanelType.Horizontal)
                  .Add(
                    new Picture {
                        Name = "Picture",
                        DesiredSize = new Size(80, 80),
                        Margin = 1.5,
                        Source = "https://nwoods.com/go/images/samples/HSnopic.png"  // the default image
                      }
                      .Bind("Source", "Pic", FindHeadShot),
                    // define the panel where the text will appear
                    new Panel(PanelType.Table) {
                        MinSize = new Size(130, double.NaN),
                        MaxSize = new Size(150, double.NaN),
                        Margin = new Margin(6, 10, 0, 6),
                        DefaultAlignment = Spot.Left
                      }
                      .Add(new ColumnDefinition { Column = 2, Width = 4 })
                      .Add(
                        new TextBlock { // the name
                            Name = "NAMETB",
                            Row = 0, Column = 0, ColumnSpan = 5,
                            Stroke = "white",
                            Font = new Font("Segoe UI", 12, FontUnit.Point),
                            Editable = true, IsMultiline = false,
                            MinSize = new Size(50, 16)
                          }
                          .Bind(new Binding("Text", "Name").MakeTwoWay()),
                        new TextBlock("Title: ") { Row = 1, Column = 0 }
                          .Set(textStyle),
                        new TextBlock {
                            Row = 1, Column = 1, ColumnSpan = 4,
                            Editable = true, IsMultiline = false,
                            MinSize = new Size(50, 14),
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
                            Stroke = "white",
                            Font = new Font("Segoe UI", 12, Northwoods.Go.FontStyle.Italic),
                            Wrap = Wrap.Fit,
                            Editable = true,  // by default newlines are allowed
                            MinSize = new Size(100, 14)
                          }
                          .Bind(new Binding("Text", "Comments").MakeTwoWay())
                      )  // end Table Panel
                  )  // end Horizontal Panel
              ),  // end Auto Panel
            Builder.Make<Panel>("Button")
              .Add(new Shape("PlusLine") { Width = 10, Height = 10 })
              .Set(new {
                Name = "BUTTON", Alignment = Spot.Right, Opacity = 0,  // initially not visible
                Click = new Action<InputEvent, GraphObject>((e, button) => {
                  if (button.Part is Node node) {
                    AddEmployee(node);
                  }
                })
              })
              // button is visible either when node is selected or on mouse-over
              .Bind(new Binding("Opacity", "IsSelected", s => (bool)s ? 1 : 0).OfElement()),
            Builder.Make<Panel>("TreeExpanderButton")
              .Set(new {
                Name = "BUTTONX", Alignment = Spot.Bottom, Opacity = 0,  // initially not visible
                _TreeExpandedFigure = "LineUp",
                _TreeCollapsedFigure = "LineDown"
              })
              // button is visible either when node is selected or on mouse-over
              .Bind(new Binding("Opacity", "IsSelected", s => (bool)s ? 1 : 0).OfElement())
          );  // end Node



      // the context menu allows users to add an employee, make a position vacant,
      // remove a role and reassign the subtree, or remove a department
      _Diagram.NodeTemplate.ContextMenu =
        Builder.Make<Adornment>("ContextMenu").Add(
          Builder.Make<Panel>("ContextMenuButton")
            .Add(new TextBlock("Add Employee"))
            .Set(new {
              Click = new Action<InputEvent, GraphObject>((e, button) => {
                if ((button.Part as Adornment).AdornedPart is Node node) {
                  AddEmployee(node);
                }
              })
            }),
          Builder.Make<Panel>("ContextMenuButton")
            .Add(new TextBlock("Vacate Position"))
            .Set(new {
              Click = new Action<InputEvent, GraphObject>((e, button) => {
                if ((button.Part as Adornment).AdornedPart is Node node) {
                  var thisemp = node.Data as NodeData;
                  _Diagram.Commit(d => {
                    // update the key, name, picture, and comments, but leave the title
                    d.Model.Set(thisemp, "Name", "(Vacant)");
                    d.Model.Set(thisemp, "Pic", "");
                    d.Model.Set(thisemp, "Comments", "");
                  }, "vacate");
                }
              })
            }
          ),
          Builder.Make<Panel>("ContextMenuButton")
            .Add(new TextBlock("Remove Role"))
            .Set(new {
              Click = new Action<InputEvent, GraphObject>((e, button) => {
                // reparent the subtree to this node's boss, then remove the node
                if ((button.Part as Adornment).AdornedPart is Node node) {
                  _Diagram.Commit(d => {
                    var chl = node.FindTreeChildrenNodes();
                    foreach (var emp in chl) {
                      var data = emp.Data as NodeData;
                      var pdata = node.FindTreeParentNode().Data as NodeData;
                      (d.Model as Model).SetParentKeyForNodeData(data, pdata.Key);
                    }
                    // and now remove the selected node itself
                    d.Model.RemoveNodeData(node.Data);
                  }, "reparent remove");
                }
              })
            }
          ),
          Builder.Make<Panel>("ContextMenuButton")
            .Add(new TextBlock("Remove Department"))
            .Set(new {
              Click = new Action<InputEvent, GraphObject>((e, button) => {
                // remove the whole subtree, including the node itself
                if ((button.Part as Adornment).AdornedPart is Node node) {
                  _Diagram.Commit(d => {
                    d.RemoveParts(node.FindTreeParts());
                  }, "remove dept");
                }
              })
            }
          )
        );

      // define the Link template
      _Diagram.LinkTemplate =
        new Link { Routing = LinkRouting.Orthogonal, LayerName = "Background", Corner = 5 }
          .Add(new Shape { StrokeWidth = 1.5, Stroke = "#F5F5F5" });  // the link shape

      LoadModel();

      _Inspector = new Inspector(inspector1, _Diagram, new Inspector.Options {
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
      modelJson1.JsonText = _Diagram.Model.ToJson();
    }

    private void LoadModel() {
      if (_Diagram == null) return;
      var model = Model.FromJson<Model>(modelJson1.JsonText);
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
    public string Pic { get; set; }
    public bool IsTreeExpanded { get; set; } = true;
  }

  // override TreeLayout.CommitNodes to also modify the background brush based on the tree depth level
  public class SideTreeLayout : TreeLayout {
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
                { 1, Brush.Lighten(color, 0.05) }
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
