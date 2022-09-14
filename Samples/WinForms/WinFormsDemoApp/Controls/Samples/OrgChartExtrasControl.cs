/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace Demo.Samples.OrgChartExtras {
  [ToolboxItem(false)]
  public partial class OrgChartExtrasControl : DemoControl {
    private Diagram myDiagram;

    public OrgChartExtrasControl() {
      InitializeComponent();

      nameBox.Leave += (e, obj) => UpdateData(nameBox.Text, "name");
      titleBox.Leave += (e, obj) => UpdateData(titleBox.Text, "title");
      commentBox.Leave += (e, obj) => UpdateData(commentBox.Text, "comments");

      modelJson1.SaveClick = SaveModel;
      modelJson1.LoadClick = LoadModel;
      modelJson1.JsonText = @"{
        ""NodeDataSource"": [
          {""Key"":""1"", ""Name"":""Corrado 'Junior' Soprano"", ""Title"":""The Boss""},
          {""Key"":""2"", ""Name"":""Tony Soprano"", ""Title"":""Underboss""},
          {""Key"":""3"", ""Name"":""Herman 'Hesh' Rabkin"", ""Title"":""Advisor""},
          {""Key"":""4"", ""Name"":""Paulie Walnuts"", ""Title"":""Capo""},
          {""Key"":""5"", ""Name"":""Ralph Cifaretto"", ""Title"":""Capo MIA""},
          {""Key"":""6"", ""Name"":""Silvio Dante"", ""Title"":""Consigliere""},
          {""Key"":""7"", ""Name"":""Bobby Baccilien"", ""Title"":""Capo""},
          {""Key"":""8"", ""Name"":""Sal Bonpensiero"", ""Title"":""MIA""},
          {""Key"":""9"", ""Name"":""Christopher Moltisanti"", ""Title"":""Made Man""},
          {""Key"":""10"", ""Name"":""Furio Giunta"", ""Title"":""Muscle""},
          {""Key"":""11"", ""Name"":""Patsy Parisi"", ""Title"":""Accountant""}
        ],
        ""LinkDataSource"": [
          {""From"":""1"", ""To"":""2""},
          {""From"":""1"", ""To"":""3""},
          {""From"":""2"", ""To"":""4""},
          {""From"":""2"", ""To"":""5""},
          {""From"":""2"", ""To"":""6""},
          {""From"":""2"", ""To"":""7""},
          {""From"":""4"", ""To"":""8""},
          {""From"":""4"", ""To"":""9""},
          {""From"":""4"", ""To"":""10""},
          {""From"":""4"", ""To"":""11""},
          {""From"":""11"", ""To"":""6"", ""Category"":""Support"", ""Text"":""50% support""},
          {""From"":""9"", ""To"":""7"", ""Category"":""Motion"", ""Text"":""Will change here in 2 months""}
        ]
      }";

      goWebBrowser1.Html = @"
  <p>
    Double click on a node in order to add a person.
    Drag a node onto another in order to change relationships.
    You can also draw a link from a node's background to other nodes that have no ""boss"".
  </p>
  <p>
    This is the <a href=""OrgChartEditor"">Org Chart Editor</a> sample,
    but each node includes a TreeExpanderButton,
    and there are additional non-tree links connecting some of the nodes.
  </p>
";

      Setup();
    }

    private void Setup() {

      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.ValidCycle = CycleMode.DestinationTree; // trees only
      myDiagram.MaxSelectionCount = 1; // can select only one part at a time
      myDiagram.Layout = new TreeLayout {
        TreeStyle = TreeStyle.LastParents,
        Arrangement = TreeArrangement.Horizontal,
        // properties for most of the tree:
        Angle = 90,
        LayerSpacing = 35,
        // properties for the "last parents":
        AlternateAngle = 0,
        AlternateAlignment = TreeAlignment.Start,
        AlternateNodeIndent = 10,
        AlternateNodeIndentPastParent = 1.0,
        AlternateNodeSpacing = 10,
        AlternateLayerSpacing = 30,
        AlternateLayerSpacingParentOverlap = 1.0,
        AlternatePortSpot = new Spot(0.01, 1, 10, 0),
        AlternateChildPortSpot = Spot.Left
      };
      myDiagram.ChangedSelection += OnSelectionChanged; // support editing properties in HTML
      myDiagram.TextEdited += OnTextEdited;
      myDiagram.UndoManager.IsEnabled = true;

      // linear gradient brush
      var graygrad = new Brush(new LinearGradientPaint(new Dictionary<float, string> {
          { 0, "rgb(125, 125, 125)" },
          { 0.5f, "rgb(86, 86, 86)" },
          { 1, "rgb(86, 86, 86)" }
        }
      ));

      // when a node is double-clicked, add a child to it
      void NodeDoubleClick(InputEvent e, GraphObject obj) {
        var clicked = obj.Part;
        if (clicked != null) {
          var thisemp = clicked.Data as NodeData;
          myDiagram.StartTransaction("add employee");
          var nextkey = (myDiagram.Model.NodeDataSource.Count() + 1).ToString();
          var newemp = new NodeData {
            Key = nextkey,
            Name = "(new person)",
            Title = ""
          };
          myDiagram.Model.AddNodeData(newemp);
          (myDiagram.Model as Model).AddLinkData(new LinkData {
            From = thisemp.Key,
            To = nextkey
          });
          myDiagram.CommitTransaction("add employee");
        }
      }

      // this is used to determine feedback during drags
      bool MayWorkFor(GraphObject node1, GraphObject node2) {
        if (!(node1 is Node)) return false;  // must be a Node
        if (node1 == node2) return false;  // cannot work for yourself
        if ((node2 as Node).IsInTreeOf(node1 as Node)) return false;  // cannot work for someone who works for you
        return true;
      }

      // This function provides a common style for most of the TextBlocks.
      // Some of these values may be overridden in a particular TextBlock.
      object TextStyle() {
        return new { Font = new Font("Segoe UI", 9), Stroke = "white" };
      }

      // define the Node template
      myDiagram.NodeTemplate =
        new Node(PanelType.Auto) {
          DoubleClick = NodeDoubleClick,
          // handle dragging a Node onto a Node to (maybe) change the reporting relationship
          MouseDragEnter = (e, nodeAsObj, prev) => {
            var node = nodeAsObj as Node;
            var diagram = node.Diagram;
            var selnode = diagram.Selection.Count > 0 ? diagram.Selection.First() : null;
            if (!MayWorkFor(selnode, node)) return;
            var shape = node.FindElement("SHAPE") as Shape;
            if (shape != null) shape.Fill = "darkred";
          },
          MouseDragLeave = (e, nodeAsObj, next) => {
            var node = nodeAsObj as Node;
            var shape = node.FindElement("SHAPE") as Shape;
            if (shape != null) shape.Fill = graygrad;
          },
          MouseDrop = (e, nodeAsObj) => {
            var node = nodeAsObj as Node;
            var diagram = node.Diagram;
            var selnode = diagram.Selection.Count > 0 ? diagram.Selection.First() : null;  // assume just one Node in selection
            if (MayWorkFor(selnode, node)) {
              // find any existing link into the selected node
              var link = (selnode as Node).FindTreeParentLink();
              if (link != null) {  // reconnect any existing link
                link.FromNode = node;
              } else {  // else create a new link
                diagram.ToolManager.LinkingTool.InsertLink(node, node.Port, selnode as Node, (selnode as Node).Port);
              }
            }
          }
        }.Bind(
          // for sorting, have the Node.Text be the data.Name
          new Binding("Text", "Name"),
          // bind the Part.LayerName to control the Node's layer depending on whether it isSelected
          new Binding("LayerName", "IsSelected", (sel, _) => {
            return (sel as bool? ?? false) ? "Foreground" : "";
          }).OfElement()
        ).Add(
          // define the node's outer shape
          new Shape {
            Figure = "RoundedRectangle",
            Name = "SHAPE",
            Fill = graygrad,
            Stroke = "black",
            PortId = "",
            FromLinkable = true,
            ToLinkable = true,
            Cursor = "pointer"
          },
          // define the panel where the text will appear
          new Panel(PanelType.Table) {
            MaxSize = new Size(150, 999),
            Margin = new Margin(3, 3, 0, 3),
            DefaultAlignment = Spot.Left
          }.Add(
            new ColumnDefinition {
              Column = 2,
              Width = 4
            }
          ).Add(
            new TextBlock  // the name
              {
              Row = 0,
              Column = 0,
              ColumnSpan = 5,
              Font = new Font("Segoe UI", 9, FontWeight.Bold),
              Editable = true,
              IsMultiline = false,
              Stroke = "white",
              MinSize = new Size(10, 14),
              Name = "name"
            }.Bind(
              new Binding("Text", "Name").MakeTwoWay()
            ),
            new TextBlock {
              Text = "Title: ",
              Row = 1,
              Column = 0
            }.Set(TextStyle()),
            new TextBlock {
              Row = 1,
              Column = 1,
              ColumnSpan = 4,
              Editable = true,
              IsMultiline = false,
              MinSize = new Size(10, 14),
              Margin = new Margin(0, 0, 0, 3),
              Name = "title"
            }.Set(TextStyle()).Bind(
              new Binding("Text", "Title").MakeTwoWay()
            ),
            new TextBlock { // the ID and the boss
              Text = "ID: ",
              Row = 2,
              Column = 0
            }.Set(TextStyle()),
            new TextBlock {
              Row = 2,
              Column = 1
            }.Set(TextStyle()).Bind(
              new Binding("Text", "Key")
            ),
            new TextBlock {
              Text = "Boss: ",
              Row = 3,
              Column = 0
            }.Set(TextStyle()),
            new TextBlock {
              Row = 3,
              Column = 1,
              Stroke = "white",
              Font = new Font("Segoe UI", 9)
            }.Bind(
              new Binding("Text", "", (nodeAsObj, _) => { // get parent name
                var node = nodeAsObj as Node;
                if (node == null) return "";
                var linksInto = node.FindLinksInto();
                if (linksInto.Count() <= 0) return "";
                return (linksInto.First().FromNode.Data as NodeData).Name;
              }).OfElement()
            ),
            new TextBlock { // the comments
              Row = 4,
              Column = 0,
              ColumnSpan = 5,
              Font = new Font("Segoe UI", 9, FontWeight.Bold),
              Wrap = Wrap.Fit,
              Editable = true,  // by default newlines are allowed
              Stroke = "white",
              MinSize = new Size(10, 14),
              Name = "comments"
            }.Bind(
              new Binding("Text", "Comments").MakeTwoWay()
            ),
            Builder.Make<Panel>("TreeExpanderButton").Set(
              new { Row = 4, ColumnSpan = 99, Alignment = Spot.Center }
            )
          )  // end Table Panel
        );  // end Node

      // define the Link template
      myDiagram.LinkTemplate =
        new Link {
          Routing = LinkRouting.Orthogonal,
          Corner = 5,
          RelinkableFrom = true,
          RelinkableTo = true
        }.Add(
          new Shape { // the link shape
            StrokeWidth = 2
          }
        );

      myDiagram.LinkTemplateMap.Add("Support",
        new Link {
          Curve = LinkCurve.Bezier,
          IsLayoutPositioned = false,
          IsTreeLink = false,
          Curviness = -50,
          RelinkableFrom = true,
          RelinkableTo = true
        }.Add(
          new Shape {
            Stroke = "green",
            StrokeWidth = 2
          },
          new Shape {
            ToArrow = "OpenTriangle",
            Stroke = "green",
            StrokeWidth = 2
          },
          new TextBlock {
            Stroke = "green",
            Background = "rgba(255,255,255,0.75)",
            MaxSize = new Size(80, double.NaN)
          }.Bind(
            new Binding("Text", "Text")
          )
        )
      );

      myDiagram.LinkTemplateMap.Add("Motion",
        new Link {
          Curve = LinkCurve.Bezier,
          IsLayoutPositioned = false,
          IsTreeLink = false,
          Curviness = -50,
          RelinkableFrom = true,
          RelinkableTo = true
        }.Add(
          new Shape {
            Stroke = "orange",
            StrokeWidth = 2
          },
          new Shape {
            ToArrow = "OpenTriangle",
            Stroke = "orange",
            StrokeWidth = 2
          },
          new TextBlock {
            Stroke = "orange",
            Background = "rgba(255,255,255,0.75)",
            MaxSize = new Size(80, double.NaN)
          }.Bind(
            new Binding("Text", "Text")
          )
        )
      );

      // read in the JSON-format data from the "mySavedModel" element
      LoadModel();
    }

    private void OnSelectionChanged(object _, DiagramEvent e) {
      //tableLayoutPanel3.Visible = true;
      var sel = e.Diagram.Selection;
      Node node = null;
      if (sel.Count >= 1) node = sel.First() as Node;
      if (node != null) {
        UpdateProperties(node.Data as NodeData);
      } else {
        UpdateProperties(null);
      }
    }

    private void UpdateProperties(NodeData data) {
      if (data == null) {
        propertyTable.Visible = false;
        nameBox.Text = "";
        titleBox.Text = "";
        commentBox.Text = "";
      } else {
        propertyTable.Visible = true;
        nameBox.Text = data.Name ?? "";
        titleBox.Text = data.Title ?? "";
        commentBox.Text = data.Comments ?? "";
      }
    }

    // This is called when the user has finished inline text-editing
    private void OnTextEdited(object _, DiagramEvent e) {
      var tb = e.Subject as TextBlock;
      if (tb == null || tb.Name != null) return;
      var node = tb.Part;
      if (node is Node) {
        UpdateData(tb.Text, tb.Name);
        UpdateProperties(node.Data as NodeData);
      }
    }

    // Update the data fields when the text is changed
    private void UpdateData(string text, string field) {
      var node = myDiagram.Selection.First();
      // maxSelectionCount = 1, so there can only be one Part in this collection
      var data = node.Data as NodeData;
      if (node is Node && data != null) {
        var model = myDiagram.Model;
        model.StartTransaction("modified " + field);
        if (field == "name") {
          model.Set(data, "Name", text);
        } else if (field == "title") {
          model.Set(data, "Title", text);
        } else if (field == "comments") {
          model.Set(data, "Comments", text);
        }
        model.CommitTransaction("modified " + field);
      }
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
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Name { get; set; }
    public string Title { get; set; }
    public string Comments { get; set; }
  }

  public class LinkData : Model.LinkData { }

}
