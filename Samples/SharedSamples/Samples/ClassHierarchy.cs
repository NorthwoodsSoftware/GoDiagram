/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Northwoods.Go;
using Northwoods.Go.Extensions;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace Demo.Samples.ClassHierarchy {
  public partial class ClassHierarchy : DemoControl {
    private Diagram _Diagram;
    private Diagram _Singletons;

    public ClassHierarchy() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;
      _Singletons = diagramControl2.Diagram;

      Setup();

      desc1.MdText = DescriptionReader.Read("Samples.ClassHierarchy.md");
    }

    private void Setup() {
      // add hyperlink text extension
      HyperlinkText.DefineBuilders();

      // diagram properties
      // Automatically lay out the diagram as a tree;
      // separate trees are arranged vertically above each other.
      _Diagram.Layout = new TreeLayout {
        NodeSpacing = 3
      };


      // Define a node template showing class names.
      // Clicking on the node opens up the documentation for that class.

      // hyperlinkText
      var hyperlinkText =
        Builder.Make<Panel>("HyperlinkText",
          new Func<object, string>(linkfunc),
          // define the visual tree of the textblock
          new Panel(PanelType.Auto).Add(
            new Shape { Fill = "#1F4963", Stroke = (Brush)null },
            new TextBlock {
              Text = "Text",
              Font = new Font("Segoe UI", 13, Northwoods.Go.FontWeight.Bold),
              Stroke = "white",
              Margin = 3
            }.Bind(
              new Binding("Text", "Key")
            )
          )
        );

      // node template
      _Diagram.NodeTemplate =
        new Node().Add(
          hyperlinkText
        );

      // Define a trivial link template with no arrowhead
      _Diagram.LinkTemplate =
        new Link {
          Curve = LinkCurve.Bezier,
          ToEndSegmentLength = 30,
          FromEndSegmentLength = 30
        }.Add(
          new Shape { // the link shape, with the default black stroke
            StrokeWidth = 1.5
          }
        );

      // Collect all of the data for the model of the class hierarchy
      var nodeDataSource = new List<NodeData>();

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

      foreach(var c in classlist) {
        if (c.BaseType?.Name == c.Name) continue;  // don't repeat derived types that share name with parent

        // some names need to be adjusted
        string getKey(Type t) {
          var key = t.Name;
          // generics have '`', remove
          var idx = key.IndexOf('`');
          if (idx >= 0) key = key.Substring(0, idx);
          // nested classes should include their declaring class
          if (t.IsNested) {
            key = $"{getKey(t.DeclaringType)}.{key}";
          }
          return key;
        }

        // find base class constructor
        var parent = c.BaseType;
        var key = getKey(c);
        if (parent == null || parent.Name == null ||
            parent.FullName == "System.Object" ||
            parent.FullName == "System.ValueType" ||
            parent.FullName == "System.MulticastDelegate" ||
            parent.FullName == "System.Windows.Forms.Control" ||
            parent.FullName == "Avalonia.Controls.Primitives.TemplatedControl") {  // "root" node?
          nodeDataSource.Add(new NodeData { Key = key });
        } else {
          nodeDataSource.Add(new NodeData { Key = key, Parent = getKey(parent) });
        }
      }

      // model data
      _Diagram.Model = new Model {
        NodeDataSource = nodeDataSource
      };

      // Now collect all node data that are singletons
      var singlesArray = new List<NodeData>();
      foreach (var node in _Diagram.Nodes) {
        if (!node.LinksConnected.Any()) {
          singlesArray.Add(node.Data as NodeData);
        }
      }

      // Remove the unconnected class nodes from the main Diagram
      (_Diagram.Model as Model).RemoveNodeData(singlesArray);

      // Display the unconnected classes in a separate diagram
      _Singletons.NodeTemplate = _Diagram.NodeTemplate;
      _Singletons.Layout = new GridLayout {
        WrappingColumn = 1, // put the unconnected nodes in a column
        Spacing = new Size(3, 3)
      };
      _Singletons.Model = new Model<NodeData, string, object>(singlesArray);
    }
  }

  // define the model data
  public class Model : TreeModel<NodeData, string, object> { }
  public class NodeData : Model.NodeData { }
}
