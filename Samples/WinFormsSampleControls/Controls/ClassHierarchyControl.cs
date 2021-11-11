using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Extensions;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.ClassHierarchy {
  [ToolboxItem(false)]
  public partial class ClassHierarchyControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;
    private Diagram myDiagram2;

    public ClassHierarchyControl() {
      InitializeComponent();

      diagramControl1.AfterRender = Setup;

      goWebBrowser1.Html = @"

 <p>The JavaScript class hierarchy defined by the GoDiagram library, laid out by a <a>TreeLayout</a>.
     Classes that do not have any inheritance relationship are shown at the right.</p>
  <p>Because the node template uses a ""HyperlinkText"", clicking on a node will open the API reference for that class in a new window.</p>
  <p> For more uses of the Tree Layout, see the <a href=""DOMTree.html""> DOM Tree </a> and <a href = ""visualTree.html""> Visual Tree </a> samples.</p>
";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;
      myDiagram2 = diagramControl2.Diagram;

      // add hyperlink text extension
      HyperlinkText.DefineBuilders();

      // diagram properties
      // Automatically lay out the diagram as a tree;
      // separate trees are arranged vertically above each other.
      myDiagram.Layout = new TreeLayout {
        NodeSpacing = 3
      };


      // Define a node template showing class names.
      // Clicking on the node opens up the documentation for that class.

      // functions for hyperlink textblock
      Func<object, string> linkfunc = (object nodeIn) => {
        var node = nodeIn as Node;
        var data = node.Data as NodeData;
        return "https://gojs.net/latest/api/symbols/" + data.Key + ".Html";
      };

      // hyperlinkText
      var hyperlinkText =
        Builder.Make<Panel>("HyperlinkText",
          linkfunc,
          // define the visual tree of the textblock
          new Panel(PanelLayoutAuto.Instance).Add(
            new Shape { Fill = "#1F4963", Stroke = (Brush)null },
            new TextBlock {
              Text = "Text",
              Font = "Segoe UI, 13px, style=bold",
              Stroke = "white",
              Margin = 3
            }.Bind(
              new Binding("Text", "Key")
            )
          )
        );

      // node template
      myDiagram.NodeTemplate =
        new Node().Add(
          hyperlinkText
        );

      // Define a trivial link template with no arrowhead
      myDiagram.LinkTemplate =
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
        // find base class constructor
        var parent = c.BaseType;
        if (parent == null || parent.Name == null ||
            parent.FullName == "System.Object" ||
            parent.FullName == "System.ValueType" ||
            parent.FullName == "System.MulticastDelegate" ||
            parent.FullName == "System.Windows.Forms.Control") {  // "root" node?
          nodeDataSource.Add(new NodeData { Key = c.Name });
        } else {
          nodeDataSource.Add(new NodeData { Key = c.Name, Parent = parent.Name });
        }
      }

      // model data
      myDiagram.Model = new Model {
        NodeDataSource = nodeDataSource
      };

      // Now collect all node data that are singletons
      var singlesArray = new List<NodeData>();
      foreach (var node in myDiagram.Nodes) {
        if (!node.LinksConnected.Any()) {
          singlesArray.Add(node.Data as NodeData);
        }
      }

      // Remove the unconnected class nodes from the main Diagram
      (myDiagram.Model as Model).RemoveNodeDataCollection(singlesArray);

      // Display the unconnected classes in a separate diagram
      myDiagram2.NodeTemplate = myDiagram.NodeTemplate;
      myDiagram2.Layout = new GridLayout {
        WrappingColumn = 1, // put the unconnected nodes in a column
        Spacing = new Size(3, 3)
      };
      myDiagram2.Model = new Model<NodeData, string, object>(singlesArray);
    }

  }

  // define the model data
  public class Model : TreeModel<NodeData, string, object> { }
  public class NodeData : Model.NodeData {
  }

}
