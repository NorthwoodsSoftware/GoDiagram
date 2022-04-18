/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.TriStateCheckBoxTree {
  [ToolboxItem(false)]
  public partial class TriStateCheckBoxTreeControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public TriStateCheckBoxTreeControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"
        <p>
      This sample is derived from the <a href=""TreeView"">Tree View</a> sample.
      It adds the definition of the ""TriStateCheckBoxButton"" which is defined only to be used in a tree.
        </p>
";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.AllowMove = false;
      myDiagram.AllowCopy = false;
      myDiagram.AllowDelete = false;
      myDiagram.AllowHorizontalScroll = false;
      myDiagram.Layout = new TreeLayout {
        Alignment = TreeAlignment.Start,
        Angle = 0,
        Compaction = TreeCompaction.None,
        LayerSpacing = 16,
        LayerSpacingParentOverlap = 1,
        NodeIndentPastParent = 1.0,
        NodeSpacing = 0,
        SetsPortSpot = false,
        SetsChildPortSpot = false
      };

      // This button assumes data binding to the "checked" property.
      Builder.DefineBuilder("TriStateCheckBoxButton", (args) => {
        var button = /** @type {Panel} */ (
          Builder.Make<Panel>("Button").Set(
            new {
              Width = 14,
              Height = 14
            }
          ).Add(
            new Shape {
              Name = "ButtonIcon",
              GeometryString = "M0 0 M0 8.85 L4.9 13.75 16.2 2.45 M16.2 16.2",  // a 'check' mark
              StrokeWidth = 2,
              Stretch = Stretch.Fill,  // this Shape expands to fill the Button
              GeometryStretch = GeometryStretch.Uniform,  // the check mark fills the Shape without distortion
              Background = (Brush)null,
              Visible = false  // visible set to False = not checked, unless data.Checked is true
            }.Bind(
              new Binding("Visible", "Checked", (p, _) => { return (CheckState)p == CheckState.Checked || (CheckState)p == CheckState.Indeterminate; }),
              new Binding("Stroke", "Checked", (p, _) => { return (CheckState)p == CheckState.Indeterminate ? (Brush)null : "black"; }),
              new Binding("Background", "Checked", (p, _) => { return (CheckState)p == CheckState.Indeterminate ? "gray" : "transparent"; })
            )
          )
        );
        (button.FindElement("ButtonBorder") as Shape).Fill = "white";

        void UpdateCheckBoxesDown(Node node, CheckState val) {
          node.Diagram.Model.Set(node.Data, "Checked", val);
          foreach (var child in node.FindTreeChildrenNodes()) {
            UpdateCheckBoxesDown(child, val);
          }
        }

        void UpdateCheckBoxesUp(Node node) {
          var parent = node.FindTreeParentNode();
          if (parent != null) {
            var anychecked = parent.FindTreeChildrenNodes().Any((n) => {
              var c = (n.Data as NodeData).Checked;
              return c == CheckState.Checked || c == CheckState.Indeterminate;
            });
            var allchecked = parent.FindTreeChildrenNodes().All((n) => {
              return (n.Data as NodeData).Checked == CheckState.Checked;
            });
            node.Diagram.Model.Set(parent.Data, "Checked", allchecked ? CheckState.Checked : (anychecked ? CheckState.Indeterminate : CheckState.Unchecked));
            UpdateCheckBoxesUp(parent);
          }
        }

        button.Click = (e, buttonAsObj) => {
          var button = buttonAsObj as Panel;
          if (!button.IsEnabledElement()) return;
          var diagram = e.Diagram;
          if (diagram == null || diagram.IsReadOnly) return;
          if (diagram.Model.IsReadOnly) return;
          e.Handled = true;
          var shape = button.FindElement("ButtonIcon") as Shape;
          diagram.StartTransaction("checkbox");
          // Assume the name of the data property is "checked".
          var node = button.Part as Node;
          var oldval = (node.Data as NodeData).Checked;
          var newval = oldval != CheckState.Checked ? CheckState.Checked : CheckState.Unchecked;
          // Set this data.Checked property and those of all its children to the same value
          UpdateCheckBoxesDown(node, newval);
          // Walk up the tree and update all of their checkboxes
          UpdateCheckBoxesUp(node);
          // support extra side-effects without clobbering the click event handler:
          if (button["_DoClick"] is Action<InputEvent, GraphObject> action) action(e, button);
          diagram.CommitTransaction("checkbox");
        };

        return button;
      });


      var treeExpanderButton = Builder.Make<Panel>("TreeExpanderButton");
      treeExpanderButton.Width = 14;
      var buttonBorder = treeExpanderButton.FindElement("ButtonBorder") as Shape;
      buttonBorder.Fill = "whitesmoke";
      buttonBorder.Stroke = "lightgray";
      treeExpanderButton["_ButtonFillOver"] = "rgba(0,128,255,0.25)";
      treeExpanderButton["_buttonStrokeOver"] = null;
      treeExpanderButton["_buttonFillPressed"] = "rgba(0,128,255,0.4)";

      // node template
      myDiagram.NodeTemplate =
        new Node { // no Adornment = instead change panel background color by binding to Node.IsSelected
          SelectionAdorned = false,
          // a custom function to allow expanding/collapsing on double-click
          // this uses similar logic to a TreeExpanderButton
          DoubleClick = (e, nodeIn) => {
            var node = nodeIn as Node;
            var cmd = myDiagram.CommandHandler;
            if (node.IsTreeExpanded) {
              if (!cmd.CanCollapseTree(node)) return;
            } else {
              if (!cmd.CanExpandTree(node)) return;
            }
            e.Handled = true;
            if (node.IsTreeExpanded) {
              cmd.CollapseTree(node);
            } else {
              cmd.ExpandTree(node);
            }
          }
        }.Add(
          treeExpanderButton,
          new Panel(PanelLayoutHorizontal.Instance) {
            Position = new Point(16, 0),
            Margin = new Margin(0, 2, 0, 0),
            DefaultAlignment = Spot.Center
          }.Bind(
            new Binding("Background", "IsSelected", (s, _) => {
              return ((s as bool? ?? false) ? "lightblue" : "white");
            }).OfElement()
          ).Add(
            Builder.Make<Panel>("TriStateCheckBoxButton"),
            new TextBlock {
              Font = new Font("Verdana", 9),
              Margin = new Margin(0, 0, 0, 2)
            }.Bind(
              new Binding("Text", "Key", (s, _) => { return "item " + ((int)s - 1).ToString(); })
            )
          )  // end Horizontal Panel
        );  // end Node

      // without lines
      //myDiagram.LinkTemplate = new Link();

      // with lines
      myDiagram.LinkTemplate =
        new Link {
          Selectable = false,
          Routing = LinkRouting.Orthogonal,
          FromEndSegmentLength = 4,
          ToEndSegmentLength = 4,
          FromSpot = new Spot(0.001, 1, 7, 0),
          ToSpot = Spot.Left
        }.Add(
          new Shape {
            Stroke = "gray",
            StrokeDashArray = new float[] { 1, 2 }
          }
        );

      // RNG to be captured
      var rand = new Random();
      int MakeTree(int level, int count, int max, List<NodeData> nodeDataSource, NodeData parentdata) {
        var numchildren = Math.Floor(rand.NextDouble() * 10);
        for (var i = 0; i < numchildren; i++) {
          if (count >= max) return count;
          count++;
          var childdata = new NodeData { Key = count, Parent = parentdata.Key, Checked = CheckState.Unchecked };
          nodeDataSource.Add(childdata);
          if (level > 0 && rand.NextDouble() > 0.5) {
            count = MakeTree(level - 1, count, max, nodeDataSource, childdata);
          }
        }
        return count;
      }

      // create a random tree
      var nodeDataSource = new List<NodeData> {
        new NodeData {
          Key = 1,
          Checked = CheckState.Unchecked
        }
      };
      var max = 26;
      var count = 1;
      while (count < max) {
        count = MakeTree(3, count, max, nodeDataSource, nodeDataSource[0]);
      }
      myDiagram.Model = new Model {
        NodeDataSource = nodeDataSource
      };
    }
  }

  // just like System.Windows.Forms.CheckState
  public enum CheckState {
    Checked,
    Indeterminate,
    Unchecked
  }

  // define the model data
  public class Model : TreeModel<NodeData, int, object> { }
  public class NodeData : Model.NodeData {
    public CheckState Checked { get; set; }
  }

}
