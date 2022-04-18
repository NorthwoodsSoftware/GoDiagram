/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;

namespace WinFormsSampleControls.OrgChartAssistants {
  [ToolboxItem(false)]
  public partial class OrgChartAssistantsControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public OrgChartAssistantsControl() {
      InitializeComponent();

      saveLoadModel1.SaveClick += (e, obj) => SaveModel();
      saveLoadModel1.LoadClick += (e, obj) => LoadModel();

      goWebBrowser1.Html = @"
  <p>
    This editable organizational chart sample was derived from <a href=""OrgChartEditor"">Org Chart Editor</a>
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
    {""Key"":16, ""Name"":""Lotta B. Essen"", ""Title"":""Sales Rep"", ""Parent"":3},
    {""Key"":17, ""Name"":""Joaquin Closet"", ""Title"":""Wardrobe Assistant"", ""IsAssistant"":true, ""Parent"":1},
    {""Key"":18, ""Name"":""Hannah Twomey"", ""Title"":""Engineering Assistant"", ""Parent"":10, ""IsAssistant"":true}
  ]
}";

      Setup();
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      myDiagram.InitialAutoScale = AutoScale.Uniform;
      myDiagram.MaxSelectionCount = 1;
      myDiagram.ValidCycle = CycleMode.DestinationTree;
      // custom click creating tool
      myDiagram.ToolManager.ClickCreatingTool = new OrgChartAssistantsClickCreatingTool {
        ArchetypeNodeData = new NodeData {
          Name = "(new person)",
          Title = "",
          Comments = ""
        }
      };
      // layout
      myDiagram.Layout = new OrgChartAssistantsTreeLayout {
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


      // manage boss info manually when a node or link is deleted from the diagram
      myDiagram.SelectionDeleting += (obj, e) => {
        var part = (e.Subject as HashSet<Part>).First(); // e.Subject is the myDiagram.Selection collection,
        // so we'll get the first since we know we only have one selection
        myDiagram.StartTransaction("clear boss");
        if (part is Node) {
          var it = (part as Node).FindTreeChildrenNodes().GetEnumerator(); // find all child nodes
          while (it.MoveNext()) { // now iterate through them and clear out the boss information
            var child = it.Current;
            var bossText = child.FindElement("boss") as TextBlock; // since the boss TextBlock is named, we can access it by name
            if (bossText == null) return;
            bossText.Text = "";
          }
        } else if (part is Link) {
          var child = (part as Link).ToNode;
          var bossText = child.FindElement("boss") as TextBlock; // since the boss TextBlock is named, we can access it by name
          if (bossText == null) return;
          bossText.Text = "";
        }
        myDiagram.CommitTransaction("clear boss");
      };

      void NodeDoubleClick(InputEvent e, GraphObject obj) {
        var clicked = obj.Part;
        if (clicked != null) {
          var thisemp = clicked.Data as NodeData;
          myDiagram.StartTransaction("add employee");
          var newemp = new NodeData {
            Name = "(new person)",
            Title = "",
            Comments = "",
            Parent = thisemp.Key
          };
          myDiagram.Model.AddNodeData(newemp);
          myDiagram.CommitTransaction("add employee");
        }
      }

      // this is used to determine feedback during drags
      bool MayWorkFor(Node node1, Node node2) {
        if (!(node1 is Node)) return false;  // must be a Node
        if (node1 == node2) return false;  // cannot work for yourself
        if (node2.IsInTreeOf(node1)) return false;  // cannot work for someone who works for you
        return true;
      }

      // This converter is used by the Picture.
      string FindHeadShot(object keyAsObj, object _) {
        var key = (keyAsObj as int? ?? int.MinValue);
        if (key < 0 || key > 16) return "HSnopic"; // There are only 16 images
        return "hs" + key;
      }

      // define the Node template
      myDiagram.NodeTemplate =
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
            if (MayWorkFor(selnode as Node, node)) {
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
          new Binding("LayerName", "IsSelected", (sel, _) => { return (sel as bool? ?? false) ? "Foreground" : ""; }).OfElement())
        .Add(
          // define the node's outer shape
          new Shape {
            Figure = "Rectangle",
            Name = "SHAPE",
            Fill = "white",
            Stroke = (Brush)null,
            // set the port properties:
            PortId = "",
            FromLinkable = true,
            ToLinkable = true,
            Cursor = "pointer"
          },
          new Panel(PanelLayoutHorizontal.Instance).Add(
            new Picture {
              Name = "Picture",
              Margin = new Margin(6, 8, 6, 10),
              DesiredSize = new Size(39, 50)
            }.Bind(
              new Binding("Source", "Key", FindHeadShot)),
            // define the panel where the text will appear
            new Panel(PanelLayoutTable.Instance) {
              MaxSize = new Size(150, 999),
              Margin = new Margin(6, 10, 0, 3),
              DefaultAlignment = Spot.Left
            }.Add(new ColumnDefinition { Column = 2, Width = 4 })
            .Add(
              new TextBlock { // the name
                Row = 0,
                Column = 0,
                ColumnSpan = 5,
                Font = new Font("Segoe UI", 12, FontUnit.Point),
                Editable = true,
                IsMultiline = false,
                MinSize = new Size(10, 16)
              }.Bind(
                new Binding("Text", "Name").MakeTwoWay()),
              new TextBlock {
                Row = 1,
                Column = 0,
                Text = "Title: ",
                Font = new Font("Segoe UI", 9, FontUnit.Point),
              },
              new TextBlock {
                Row = 1,
                Column = 1,
                ColumnSpan = 4,
                Editable = true,
                IsMultiline = false,
                MinSize = new Size(10, 14),
                Margin = new Margin(0, 0, 0, 3),
                Font = new Font("Segoe UI", 9, FontUnit.Point),
              }.Bind(
                new Binding("Text", "Title").MakeTwoWay()),
              new TextBlock {
                Row = 2,
                Column = 0,
                Font = new Font("Segoe UI", 9, FontUnit.Point),
              }.Bind(
                new Binding("Text", "Key", (v, _) => { return "ID: " + v.ToString(); })),
              new TextBlock {
                Name = "boss",
                Row = 2,
                Column = 3,
                Font = new Font("Segoe UI", 9, FontUnit.Point),
              }.Bind( // we include a name so we can access this TextBlock when deleting Nodes/Links
                new Binding("Text", "Parent", (v, _) => { return (int)v == 0 ? "" : "Boss: " + v.ToString(); })),
              new TextBlock  // the comments
                {
                Row = 3,
                Column = 0,
                ColumnSpan = 5,
                Font = new Font("Segoe UI", 9, FontStyle.Italic, FontUnit.Point),
                Wrap = Wrap.Fit,
                Editable = true,  // by default newlines are allowed
                MinSize = new Size(10, 14),
              }.Bind(
                new Binding("Text", "Comments").MakeTwoWay())
            )  // end Table Panel
          ) // end Horizontal Panel
        );  // end Node

      // the context menu allows users to make a position vacant,
      // remove a role and reassign the subtree, or remove a department
      myDiagram.NodeTemplate.ContextMenu =
        Builder.Make<Adornment>("ContextMenu").Add(
          Builder.Make<Panel>("ContextMenuButton").Add(
            new TextBlock {
              Text = "Vacate Position",
              Click = (e, obj) => {
                var node = (obj.Part as Adornment).AdornedPart as Node;
                if (node != null) {
                  var thisemp = node.Data as NodeData;
                  myDiagram.StartTransaction("vacate");
                  // update the key, name, and comments
                  myDiagram.Model.Set(thisemp, "Name", "(Vacant)");
                  myDiagram.Model.Set(thisemp, "Comments", "");
                  myDiagram.CommitTransaction("vacate");
                }
              }
            }
          ),
          Builder.Make<Panel>("ContextMenuButton").Add(
            new TextBlock {
              Text = "Remove Role",
              Click = (e, obj) => {
                // reparent the subtree to this node's boss, then remove the node
                var node = (obj.Part as Adornment).AdornedPart as Node;
                if (node != null) {
                  myDiagram.StartTransaction("reparent remove");
                  var chl = node.FindTreeChildrenNodes().GetEnumerator();
                  // iterate through the children and set their parent key to our selected node's parent key
                  while (chl.MoveNext()) {
                    var emp = chl.Current;
                    var data = emp.Data as NodeData;
                    var pdata = node.FindTreeParentNode().Data as NodeData;
                    (myDiagram.Model as Model).SetParentKeyForNodeData(data, pdata.Key);
                  }
                  // and now remove the selected node itself
                  myDiagram.Model.RemoveNodeData(node.Data);
                  myDiagram.CommitTransaction("reparent remove");
                }
              }
            }
          ),
          Builder.Make<Panel>("ContextMenuButton").Add(
            new TextBlock {
              Text = "Remove Department",
              Click = (e, obj) => {
                // remove the whole subtree, including the node itself
                var node = (obj.Part as Adornment).AdornedPart as Node;
                if (node != null) {
                  myDiagram.StartTransaction("remove dept");
                  myDiagram.RemoveParts(node.FindTreeParts(), true);
                  myDiagram.CommitTransaction("remove dept");
                }
              }
            }
          ),
          Builder.Make<Panel>("ContextMenuButton").Add(
            new TextBlock {
              Text = "Toggle Assistant",
              Click = (e, obj) => {
                // remove the whole subtree, including the node itself
                var node = (obj.Part as Adornment).AdornedPart as Node;
                if (node != null) {
                  myDiagram.StartTransaction("toggle assistant");
                  myDiagram.Model.Set(node.Data, "IsAssistant", !(node.Data as NodeData).IsAssistant);
                  myDiagram.Layout.InvalidateLayout();
                  myDiagram.CommitTransaction("toggle assistant");
                }
              }
            }
          )
        );

      // define the Link template
      myDiagram.LinkTemplate =
        new Link {
          Routing = LinkRouting.Orthogonal,
          Corner = 5,
          RelinkableFrom = true,
          RelinkableTo = true
        }.Add(
          new Shape {
            StrokeWidth = 4,
            Stroke = "#00a4a4"
          });  // the link shape

      myDiagram.Model = new Model();
      LoadModel();

      // TODO data inspector
    }

    private void SaveModel() {
      if (myDiagram == null) return;
      saveLoadModel1.ModelJson = myDiagram.Model.ToJson();
    }

    private void LoadModel() {
      if (myDiagram == null) return;
      myDiagram.Model = Model.FromJson<Model>(saveLoadModel1.ModelJson);
      myDiagram.Model.UndoManager.IsEnabled = true;
    }

  }

  // define the model data
  public class Model : TreeModel<NodeData, int, object> { }
  public class NodeData : Model.NodeData {
    public string Name { get; set; }
    public string Title { get; set; }
    public string Comments { get; set; }
    public bool IsAssistant { get; set; }
  }

  // override TreeLayout.CommitNodes to also modify the background brush based on the tree depth level
  public class OrgChartAssistantsTreeLayout : TreeLayout {
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
            shape.Fill = new Brush(new LinearGradientPaint(
              new Dictionary<float, string> {
                { 0, color },
                { 1, Brush.LightenBy(color, 0.05) }
              }, Spot.Left, Spot.Right));
          }
        }
      }
    }


    private bool IsAssistant(Node n) {
      if (n == null) return false;
      return (n.Data as NodeData).IsAssistant;
    }

    protected override void CommitLinks() {
      base.CommitLinks();
      // make sure the middle segment of an orthogonal link does not cross over the assistant subtree
      var eit = Network.Edges.GetEnumerator();
      while (eit.MoveNext()) {
        var e = eit.Current;
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

    public override TreeNetwork MakeNetwork(IEnumerable<Part> coll = null) {
      var net = base.MakeNetwork(Diagram.Nodes.Concat<Part>(Diagram.Links));
      // copy the collection of TreeVertexes, because we will modify the network
      var vertexcoll = new HashSet<TreeVertex>(net.Vertexes);
      var it = vertexcoll.GetEnumerator();
      while (it.MoveNext()) {
        var parent = it.Current;
        // count the number of assistants
        var acount = 0;
        var ait = parent.DestinationVertexes.GetEnumerator();
        while (ait.MoveNext()) {
          if (IsAssistant(ait.Current.Node)) acount++;
        }
        // if a vertex has some number of children that should be assistants
        if (acount > 0) {
          // remember the assistant edges and the regular child edges
          var asstedges = new HashSet<TreeEdge>();
          var childedges = new HashSet<TreeEdge>();
          var eit = parent.DestinationEdges.GetEnumerator();
          while (eit.MoveNext()) {
            var e = eit.Current;
            if (IsAssistant(e.ToVertex.Node)) {
              asstedges.Add(e);
            } else {
              childedges.Add(e);
            }
          }
          // first remove all edges from PARENT
          eit = asstedges.GetEnumerator();
          while (eit.MoveNext()) { parent.DeleteDestinationEdge(eit.Current); }
          eit = childedges.GetEnumerator();
          while (eit.MoveNext()) { parent.DeleteDestinationEdge(eit.Current); }
          // if the number of assistants is odd, add a dummy assistant, to make the count even
          if (acount % 2 == 1) {
            var dummy = net.CreateVertex();
            net.AddVertex(dummy);
            net.LinkVertexes(parent, dummy, asstedges.First().Link);
          }
          // now PARENT should get all of the assistant children
          eit = asstedges.GetEnumerator();
          while (eit.MoveNext()) {
            parent.AddDestinationEdge(eit.Current);
          }
          // create substitute vertex to be new parent of all regular children
          var subst = net.CreateVertex();
          net.AddVertex(subst);
          // reparent regular children to the new substitute vertex
          eit = childedges.GetEnumerator();
          while (eit.MoveNext()) {
            var ce = eit.Current;
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
      var any = false;
      var children = v.Children;
      for (var i = 0; i < children.Length; i++) {
        var c = children[i];
        if (IsAssistant(c.Node)) {
          any = true;
          break;
        }
      }
      if (any) {
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
  }

  // extend click creating tool
  public class OrgChartAssistantsClickCreatingTool : ClickCreatingTool {
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
