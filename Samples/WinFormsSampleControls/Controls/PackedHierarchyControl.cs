/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Layouts.Extensions;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.PackedHierarchy {
  [ToolboxItem(false)]
  public partial class PackedHierarchyControl : System.Windows.Forms.UserControl {
    private Diagram MyDiagram;
    public static Dictionary<string, NodeData> _KeyToNodeDataMap = new();

    public PackedHierarchyControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"
        <p>
      Circle packing can be a useful way to visualize hierarchical data, as demonstrated here
      with a visualization of the class hierarchy of the GoDiagram library. This layout is performed
      automatically by the <a href=""PackedLayout"">PackedLayout</a> extension. Nodes
      are sized according to how many properties their corresponding class has, or has inherited.
      As a result, larger nodes generally represent more complex classes.Mouse over nodes to see
      their full name and the number of properties on their corresponding class.
        </p>
        <p>
      This sample is very similar to the<a href=""ClassHierarchy""> Class Hierarchy</a> sample,
      except that instead of showing the class hierarchy as a tree, it is displayed using nested circles.
      Opening the API page is achieved by double-clicking on a node, rather than using a ""HyperlinkText"".
        </p>
";

    }

    private void Setup() {

      MyDiagram = diagramControl1.Diagram;

      MyDiagram.Layout = new HierarchyLayout();
      MyDiagram.AnimationManager.IsEnabled = false;
      MyDiagram.IsReadOnly = true;
      MyDiagram.InitialAutoScale = AutoScale.Uniform;

      // common definition for both Nodes and Groups
      var toolTipTemplate =
        new Adornment("Auto").Add(
          new Shape {
            Fill = "white"
          },
          new TextBlock {
            Margin = 4
          }.Bind(new Binding("Text", "ToolTip"))
        );

      var selectionAdornmentTemplate =
        new Adornment("Auto").Add(
          new Shape {
            Figure = "Circle",
            Fill = null,
            Stroke = "dodgerblue",
            StrokeWidth = 3,
            Spot1 = Spot.TopLeft,
            Spot2 = Spot.BottomRight
          },
          new Placeholder()
        );

      var apiMap = WinFormsSharedControls.GoWebBrowser.ApiMap;
      var commonStyle = new {
        ToolTip = toolTipTemplate,
        SelectionAdornmentTemplate = selectionAdornmentTemplate,
        DoubleClick = new Action<InputEvent, GraphObject>((e, obj) => {
          var node = obj as Node;
          var data = node.Data as NodeData;
          if (apiMap != null && apiMap.TryGetValue(data.Key, out var url))
            Process.Start("explorer.exe", "https://godiagram.com/winforms/latest/" + url);
          else
            Process.Start("explorer.exe", "https://godiagram.com/winforms/latest/");
        })
      };

      MyDiagram.NodeTemplate =
        new Node("Auto")
          .Set(commonStyle)
          .Bind(
            new Binding("Width", "Diameter"),
            new Binding("Height", "Diameter")
          )
          .Add(
            new Shape {
                Figure = "Circle",
                Fill = "#1F4963",
                StrokeWidth = 0,
                Spot1 = Spot.TopLeft,
                Spot2 = Spot.BottomRight
              }
              .Bind("Fill"),
            new TextBlock {
                Font = new Font("Arial", 12),
                Stroke = "white",
                MaxLines = 1
              }
              .Bind(
                new Binding("Text"),
                new Binding("Font"),
                new Binding("Stroke", "Fill", (f, _) => ((Brush)f).IsDark() ? "white" : "black")
              )
          );

      MyDiagram.GroupTemplate = new Group() {
          Layout = new HierarchyLayout()
        }
        .Set(commonStyle)
        .Add(
          new Shape {
              Figure = "Circle",
              Fill = "rgba(128,128,128,0.33)"
            }
            .Bind(
              new Binding("DesiredSize", "Size"),
              new Binding("Position")
            ),
          new Placeholder()  // represents area for all member parts
        );

      // Collect all of the data for the model of the class hierarchy
      var nodeDataSource = new List<NodeData> {
        new NodeData {
          Key = "GoDiagram",
          Text = "GoDiagram",
          Children = new List<NodeData>()
        },
        // large label to be placed at the very end
        new NodeData {
          Key  = "GoDiagram label",
          Group = "GoDiagram",
          Text = "GoDiagram",
          ToolTip = "GoDiagram",
          Fill = null,
          Font = new Font("Arial", 64, FontWeight.Bold),
          IsLabel = true,
          Children = new List<NodeData>()
        }
      };

      _KeyToNodeDataMap.Clear();
      _KeyToNodeDataMap.Add("GoDiagram", nodeDataSource[0]);

      static bool includeType(Type t) {
        return t.Namespace != null &&
          t.Namespace.Contains("Northwoods.Go") &&
          t.IsVisible &&
          !t.IsEnum &&
          !Attribute.IsDefined(t, typeof(UndocumentedAttribute)) &&
          !t.Name.Contains("<>");
      };

      // iterate over all the classes in Go namespace, including layouts
      var asm = Assembly.GetAssembly(typeof(Diagram));
      var classlist = asm.GetTypes().Where(includeType).ToList();
      asm = Assembly.GetAssembly(typeof(CircularLayout));
      classlist.AddRange(asm.GetTypes().Where(includeType));
      asm = Assembly.GetAssembly(typeof(ForceDirectedLayout));
      classlist.AddRange(asm.GetTypes().Where(includeType));
      asm = Assembly.GetAssembly(typeof(LayeredDigraphLayout));
      classlist.AddRange(asm.GetTypes().Where(includeType));
      asm = Assembly.GetAssembly(typeof(TreeLayout));
      classlist.AddRange(asm.GetTypes().Where(includeType));

      foreach (var c in classlist) {
        if (c.BaseType?.Name == c.Name) continue;  // don't repeat derived types that share name with parent

        var propCount = 0; // count number of properties on the class
        foreach (var prop in c.GetProperties()) {
          if (c.GetProperty(prop.Name) != null) {
            propCount++;
          }
        }

        var data = new NodeData {
          Key = c.Name,
          Text = c.Name,
          PropCount = propCount,
          Children = new List<NodeData>()
        };

        if (_KeyToNodeDataMap.TryGetValue(c.Name, out var nd)) {
          data.Children = nd.Children;
        }
        _KeyToNodeDataMap[c.Name] = data;

        // find base class constructor
        var parent = c.BaseType;
        if (parent == null || parent.Name == null ||
            parent.FullName == "System.Object" ||
            parent.FullName == "System.ValueType" ||
            parent.FullName == "System.MulticastDelegate" ||
            parent.FullName == "System.Windows.Forms.Control") {  // "root" node?
          data.Group = "GoDiagram";
          _KeyToNodeDataMap["GoDiagram"].Children.Add(data);
          nodeDataSource.Add(data);
        } else {
          // add a node for this class and set its group to its parent
          data.Group = parent.Name;
          if (_KeyToNodeDataMap.TryGetValue(parent.Name, out var pd)) {
            pd.Children.Add(data);
          } else {
            _KeyToNodeDataMap.Add(parent.Name, new NodeData {
              Children = new List<NodeData> { data }
            });
          }
          nodeDataSource.Add(data);
        }
      }

      // create groups and add labels to groups with only 1 child
      for (var i = nodeDataSource.Count - 1; i >= 0; i--) {
        var n = nodeDataSource[i];
        if (n.Children.Count > 0) {
          n.IsGroup = true;
        }

        // add tooltip and/or size node using the total number of properties it has and has inherited
        var totalCount = n.PropCount;
        var parentKey = n.Group;
        NodeData parentData = null;
        while (parentKey != null && _KeyToNodeDataMap.TryGetValue(parentKey, out parentData) && parentData.Key != "GoDiagram") {
          totalCount += parentData.PropCount;
          parentKey = parentData.Group;
        }

        if (n.Group == null) {  // applies to the root GoDiagram group only
          n.ToolTip = n.Text;
        } else {
          n.ToolTip = n.Text + ": " + totalCount;  // add tooltip
        }
        if (!n.IsGroup && !n.IsLabel) { // only set size of node if it is not a group
          // calculate size by scaling totalCount logarithmically to produce more visually appealing results
          n.Diameter = 20 * Math.Log(0.5 * (totalCount + 5.5));
        }

        // add label to groups that only have one child
        if (n.Children.Count == 1) {
          nodeDataSource.Add(new NodeData {
            Text = n.Text,
            ToolTip = n.ToolTip,
            Group = n.Key,
            Fill = null,
            Font = new Font("Arial", 20, FontWeight.Bold)
          });
        }

        n.Children = null;
      }

      MyDiagram.Model = new Model {
        NodeDataSource = nodeDataSource
      };
    }
  }

  public class Model : GraphLinksModel<NodeData, string, object, LinkData, int, string> { }

  public class NodeData : Model.NodeData {
    public double Diameter { get; set; } = double.NaN;
    public List<NodeData> Children { get; set; }
    public bool IsLabel { get; set; }
    public Size Size { get; set; }
    public Point Position { get; set; }
    public string ToolTip { get; set; }
    public Brush Fill { get; set; } = "#1F4963";
    public Font Font { get; set; } = new();
    public int PropCount { get; set; }
  }

  public class LinkData : Model.LinkData { }

  public class HierarchyLayout : PackedLayout {
    // subclass PackedLayout to change default properties and override CommitLayout function
    public HierarchyLayout() : base() {
      PackShape = PackShape.Spiral;
      HasCircularNodes = true;
      SortMode = SortMode.Area;
      Comparer = (na, nb) => {
        // ensure label is placed last
        if ((na.Data as NodeData).IsLabel) {
          return 1;
        }
        if ((nb.Data as NodeData).IsLabel) {
          return -1;
        }
        // otherwise sort in ascending order by size (all nodes are circular, so using width or height here doesn't matter)
        var diff = na.ActualBounds.Width - nb.ActualBounds.Width;
        if (diff > 0) return 1;
        if (diff < 0) return -1;
        return 0;
      };
    }

    // after each group has had its layout applied, size and position it according
    // to the smallest enclosing circle which goes around all of its nodes
    public override void CommitLayout() {
      if (Group != null) {
        var groupData = PackedHierarchyControl._KeyToNodeDataMap[(string)Group.Key];

        var enclosingCircle = EnclosingCircle;
        var actualBounds = ActualBounds;

        var size = new Size(enclosingCircle.Width, enclosingCircle.Width);
        Diagram.Model.Set(groupData, "Size", size);

        var dx = EnclosingCircle.CenterX - ActualBounds.CenterX;
        var dy = EnclosingCircle.CenterY - ActualBounds.CenterY;

        var position = new Point((ActualBounds.Width - EnclosingCircle.Width) / 2 + dx, (ActualBounds.Height - EnclosingCircle.Height) / 2 + dy);
        Diagram.Model.Set(groupData, "Position", position);
      }
    }
  }

}
