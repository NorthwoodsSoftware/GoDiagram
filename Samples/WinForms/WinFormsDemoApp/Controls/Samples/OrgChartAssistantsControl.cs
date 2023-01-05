/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;

namespace Demo.Samples.OrgChartAssistants {
  [ToolboxItem(false)]
  public partial class OrgChartAssistantsControl : DemoControl {
    private Diagram myDiagram;

    public OrgChartAssistantsControl() {
      InitializeComponent();
      myDiagram = diagramControl1.Diagram;

      modelJson1.SaveClick = SaveModel;
      modelJson1.LoadClick = LoadModel;

      goWebBrowser1.Html = @"
  <p>
    This editable organizational chart sample was derived from <a href=""demo/OrgChartEditor"">Org Chart Editor</a>
    and adds support for ""assistant"" nodes that are laid out by a custom <a>TreeLayout</a> below the side
    of the parent node and above the regular child nodes.
  </p>
  <p>
    Whether or not a node is considered to be an ""assistant"" node is determined by the <code>IsAssistant</code> property of the node data.
    The user can modify this data property via an additional context menu command.
  </p>
  <p>
    Assistant employees may themselves be bosses of their own employees.
    All of a boss's reports may be assistants.
  </p>
";

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
    {""Key"":16, ""Name"":""Lotta B. Essen"", ""Title"":""Sales Rep"", ""Pic"": ""16.jpg"", ""Parent"":3},
    {""Key"":17, ""Name"":""Joaquin Closet"", ""Title"":""Wardrobe Assistant"", ""IsAssistant"":true, ""Parent"":1},
    {""Key"":18, ""Name"":""Hannah Twomey"", ""Title"":""Engineering Assistant"", ""Parent"":10, ""IsAssistant"":true}
  ]
}";

      Setup();
    }

    private void Setup() {
      myDiagram.AllowCopy = false;
      myDiagram.AllowDelete = false;
      myDiagram.InitialAutoScale = AutoScale.Uniform;
      myDiagram.MaxSelectionCount = 1;  // users can select only one part at a time
      myDiagram.ValidCycle = CycleMode.DestinationTree;  // make sure users can only create trees
      // custom click creating tool
      myDiagram.ToolManager.ClickCreatingTool = new OrgChartAssistantsClickCreatingTool {
        ArchetypeNodeData = new NodeData {  // allow double-click in background to create a new node
          Name = "(new person)",
          Title = "",
          Comments = ""
        }
      };
      // layout
      myDiagram.Layout = new SideTreeLayout {
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
      myDiagram.UndoManager.IsEnabled = true; // enable undo and redo

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

      // define the Node template
      myDiagram.NodeTemplate =
        new Node(PanelType.Auto) {
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
        }.Bind(
          // for sorting, have the Node.Text be the data.Name
          new Binding("Text", "Name"),
          // bind the Part.LayerName to control the Node's layer depending on whether it isSelected
          new Binding("LayerName", "IsSelected", (sel) => { return (bool)sel ? "Foreground" : ""; }).OfElement())
        .Add(
          // define the node's outer shape
          new Shape("Rectangle") {
            Name = "SHAPE", Fill = "white", Stroke = null,
          },
          new Panel(PanelType.Horizontal).Add(
            new Picture {
              Name = "Picture",
              DesiredSize = new Size(39, 50),
              Margin = new Margin(6, 8, 6, 10),
              Source = "https://nwoods.com/go/images/samples/HSnopic.png"  // the default image
            }.Bind(
              new Binding("Source", "Pic", FindHeadShot)),
            // define the panel where the text will appear
            new Panel(PanelType.Table) {
              MaxSize = new Size(150, 999),
              Margin = new Margin(6, 10, 0, 3),
              DefaultAlignment = Spot.Left
            }.Add(new ColumnDefinition { Column = 2, Width = 4 })
            .Add(
              new TextBlock { // the name
                Name = "NAMETB",
                Stroke = "white",
                Font = new Font("Segoe UI", 12, FontUnit.Point),
                Row = 0, Column = 0, ColumnSpan = 5,
                Editable = true, IsMultiline = false,
                MinSize = new Size(10, 16)
              }.Bind(new Binding("Text", "Name").MakeTwoWay()),
              new TextBlock("Title: ") { Row = 1, Column = 0 }
                .Set(textStyle),
              new TextBlock {
                Row = 1, Column = 1, ColumnSpan = 4,
                Editable = true, IsMultiline = false,
                MinSize = new Size(10, 14),
                Margin = new Margin(0, 0, 0, 3)
              }.Set(textStyle).Bind(new Binding("Text", "Title").MakeTwoWay()),
              new TextBlock {
                Row = 2, Column = 0
              }.Set(textStyle).Bind( new Binding("Text", "Key", (v) => { return "ID: " + v.ToString(); })),
              new TextBlock {
                Name = "boss",  // we include a name so we can access this TextBlock when deleting Nodes/Links
                Row = 2, Column = 3
              }.Set(textStyle).Bind(new Binding("Text", "Parent", (v) => { return (int)v == 0 ? "" : "Boss: " + v.ToString(); })),
              new TextBlock {  // the comments
                Row = 3, Column = 0, ColumnSpan = 5,
                Stroke = "white",
                Font = new Font("Segoe UI", 9, Northwoods.Go.FontStyle.Italic, FontUnit.Point),
                Wrap = Wrap.Fit,
                Editable = true,  // by default newlines are allowed
                MinSize = new Size(10, 14),
              }.Bind( new Binding("Text", "Comments").MakeTwoWay())
            )  // end Table Panel
          ) // end Horizontal Panel
        );  // end Node

      // the context menu allows users to add an employee, make a position vacant,
      // remove a role and reassign the subtree, or remove a department
      myDiagram.NodeTemplate.ContextMenu =
        Builder.Make<Adornment>("ContextMenu").Add(
          Builder.Make<Panel>("ContextMenuButton")
            .Add(new TextBlock("Add Employee"))
            .Set(new {
              Click = new Action<InputEvent, GraphObject>((e, button) => {
                if ((button.Part as Adornment).AdornedPart is Node node) {
                  var thisemp = node.Data as NodeData;
                  myDiagram.Commit(d => {
                    var newemp = new NodeData { Name = "(new person)", Title = "", Comments = "", Parent = thisemp.Key };
                    d.Model.AddNodeData(newemp);
                    var newnode = d.FindNodeForData(newemp);
                    if (newnode != null) newnode.Location = node.Location;
                  }, "add employee");
                }
              })
            }),
          Builder.Make<Panel>("ContextMenuButton")
            .Add(new TextBlock("Vacate Position"))
            .Set(new {
              Click = new Action<InputEvent, GraphObject>((e, button) => {
                if ((button.Part as Adornment).AdornedPart is Node node) {
                  var thisemp = node.Data as NodeData;
                  myDiagram.Commit(d => {
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
                  myDiagram.Commit(d => {
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
                  myDiagram.Commit(d => {
                    d.RemoveParts(node.FindTreeParts());
                  }, "remove dept");
                }
              })
            }
          ),
          Builder.Make<Panel>("ContextMenuButton")
            .Add(new TextBlock("Toggle Assistant"))
            .Set(new {
              Click = new Action<InputEvent, GraphObject>((e, button) => {
                // remove the whole subtree, including the node itself
                if ((button.Part as Adornment).AdornedPart is Node node) {
                  myDiagram.Commit(d => {
                    myDiagram.Model.Set(node.Data, "IsAssistant", !(node.Data as NodeData).IsAssistant);
                    myDiagram.Layout.InvalidateLayout();
                  }, "toggle assistant");
                }
              })
            }
          )
        );

      // define the Link template
      myDiagram.LinkTemplate =
        new Link { Routing = LinkRouting.Orthogonal, Corner = 5 }
          .Add(new Shape { StrokeWidth = 4, Stroke = "#00a4a4" });  // the link shape

      LoadModel();
    }

    private void SaveModel() {
      if (myDiagram == null) return;
      modelJson1.JsonText = myDiagram.Model.ToJson();
    }

    private void LoadModel() {
      if (myDiagram == null) return;
      myDiagram.Model = Model.FromJson<Model>(modelJson1.JsonText);
      myDiagram.Model.UndoManager.IsEnabled = true;
    }

  }

  // define the model data
  public class Model : TreeModel<NodeData, int, object> { }
  public class NodeData : Model.NodeData {
    public string Name { get; set; }
    public string Title { get; set; }
    public string Comments { get; set; } = "";
    public bool IsAssistant { get; set; }
    public string Pic { get; set; }
  }

  // override TreeLayout.CommitNodes to also modify the background brush based on the tree depth level
  public class SideTreeLayout : TreeLayout {
    private static string[] levelColors = new string[] {
        "#AC193D", "#2672EC", "#8C0095", "#5133AB",
        "#008299", "#D24726", "#008A00", "#094AB2"
      };

    private static bool IsAssistant(Node n) {
      if (n == null) return false;
      return (n.Data as NodeData).IsAssistant;
    }

    protected override void CommitNodes() {
      base.CommitNodes(); // standard behavior
      // then go through all of the vertexes and set their corresponding node's Shape.Fill
      // to a brush dependent on the TreeVertex.Level value
      foreach (var v in (Diagram.Layout as TreeLayout).Network.Vertexes) {
        if (v.Node != null) {
          var level = v.Level % (levelColors.Length);
          var color = levelColors[level];
          if (v.Node.FindElement("SHAPE") is Shape shape) {
            shape.Fill = new Brush(new LinearGradientPaint(
              new Dictionary<float, string> {
                { 0, color },
                { 1, Brush.Lighten(color, 0.05) }
              }, Spot.Left, Spot.Right));
          }
        }
      }
    }

    // This is a custom TreeLayout that knows about "assistants".
    // A Node for which IsAssistant(n) is true will be placed at the side below the parent node
    // but above all of the other child nodes.
    // An assistant node may be the root of its own subtree.
    // An assistant node may have its own assistant nodes.
    public override TreeNetwork MakeNetwork(IEnumerable<Part> coll = null) {
      var net = base.MakeNetwork(coll);
      // copy the collection of TreeVertexes, because we will modify the network
      var vertexcoll = new HashSet<TreeVertex>(net.Vertexes);
      foreach (var parent in vertexcoll) {
        // count the number of assistants
        var acount = 0;
        foreach (var asst in parent.DestinationVertexes) {
          if (IsAssistant(asst.Node)) acount++;
        }
        // if a vertex has some number of children that should be assistants
        if (acount > 0) {
          // remember the assistant edges and the regular child edges
          var asstedges = new HashSet<TreeEdge>();
          var childedges = new HashSet<TreeEdge>();
          //var eit = parent.DestinationEdges.GetEnumerator();
          foreach (var e in parent.DestinationEdges) {
            if (IsAssistant(e.ToVertex.Node)) {
              asstedges.Add(e);
            } else {
              childedges.Add(e);
            }
          }
          // first remove all edges from PARENT
          foreach (var ae in asstedges) {
            parent.DeleteDestinationEdge(ae);
          }
          foreach (var ce in childedges) {
            parent.DeleteDestinationEdge(ce);
          }
          // if the number of assistants is odd, add a dummy assistant, to make the count even
          if (acount % 2 == 1) {
            var dummy = net.CreateVertex();
            net.AddVertex(dummy);
            net.LinkVertexes(parent, dummy, asstedges.First().Link);
          }
          // now PARENT should get all of the assistant children
          foreach (var e in asstedges) {
            parent.AddDestinationEdge(e);
          }
          // create substitute vertex to be new parent of all regular children
          var subst = net.CreateVertex();
          net.AddVertex(subst);
          // reparent regular children to the new substitute vertex
          foreach (var ce in childedges) {
            ce.FromVertex = subst;
            subst.AddDestinationEdge(ce);
          }
          // finally can add substitute vertex as the final odd child,
          // to be positioned at the end of the PARENT's immediate subtree.
          var newedge = net.LinkVertexes(parent, subst, null);
        }
      }
      return net;
    }

    protected override void AssignTreeVertexValues(TreeVertex v) {
      // if a vertex has any assistants, use Bus alignment
      if (v.Children.Any(c => IsAssistant(c.Node))) {
        // this is the parent for the assistant(s)
        v.Alignment = TreeAlignment.Bus;  // this is required
        v.NodeSpacing = 50; // control the distance of the assistants from the parent's main links
      } else if (v.Node == null && v.ChildrenCount > 0) {
        // found the substitute parent for non-assistant children
        //v.Alignment = TreeLayout.AlignmentCenterChildren;
        //v.BreadthLimit = 3000;
        v.LayerSpacing = 0;
      }
    }

    protected override void CommitLinks() {
      base.CommitLinks();
      // make sure the middle segment of an orthogonal link does not cross over the assistant subtree
      foreach (var e in Network.Edges) {
        if (e.Link == null) continue;
        var r = e.Link;
        // does this edge come from a substitute parent vertex?
        var subst = e.FromVertex;
        if (subst.Node == null && r.Routing == LinkRouting.Orthogonal) {
          r.UpdateRoute();
          r.StartRoute();
          // middle segment goes from point 2 to point 3
          var p1 = new Point(subst.CenterX, subst.CenterY);  // assume artificial vertex has zero size
          var p2 = r.GetPoint(2);
          var p3 = r.GetPoint(3);
          var p5 = r.GetPoint(r.PointsCount - 1);
          var dist = 10;
          if (subst.Angle == 270 || subst.Angle == 180) dist = -20;
          if (subst.Angle == 90 || subst.Angle == 270) {
            p2.Y = p5.Y - dist; // (p1.Y+p5.Y)/2;
            p3.Y = p5.Y - dist; // (p1.Y+p5.Y)/2;
          } else {
            p2.X = p5.X - dist; // (p1.X+p5.X)/2;
            p3.X = p5.X - dist; // (p1.X+p5.X)/2;
          }
          r.SetPoint(2, p2);
          r.SetPoint(3, p3);
          r.CommitRoute();
        }
      }
    }
  }

  // extend click creating tool
  public class OrgChartAssistantsClickCreatingTool : ClickCreatingTool {
    public override Part InsertPart(Point loc) {  // override to scroll to the new node
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
