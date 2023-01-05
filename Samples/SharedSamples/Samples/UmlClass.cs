/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace Demo.Samples.UmlClass {
  public partial class UmlClass : DemoControl {
    private Diagram _Diagram;

    public UmlClass() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      desc1.MdText = DescriptionReader.Read("Samples.UmlClass.md");
    }

    private void Setup() {
      _Diagram.UndoManager.IsEnabled = true;
      _Diagram.Layout = new TreeLayout {
        Angle = 90,
        Path = TreePath.Source,  // links go from child to parent
        SetsPortSpot = false,  // keep Spot.AllSides for link connection spot
        SetsChildPortSpot = false,  // keep Spot.AllSides
        // nodes not connected by "generalization" links are laid out horizontally
        Arrangement = TreeArrangement.Horizontal
      };

      // show visibility or access as a single character at the beginning of each property or method
      string ConvertVisibility(object v) {
        switch (v as string) {
          case "public": return "+";
          case "private": return "-";
          case "protected": return "#";
          case "package":
            return "~";
          default: return v as string;
        }
      }

      // the item template for properties
      var propertyTemplate =
        new Panel(PanelType.Horizontal).Add(
          // property visibility/access
          new TextBlock {
            IsMultiline = false,
            Editable = false,
            Width = 12
          }.Bind(
            new Binding("Text", "Visibility", ConvertVisibility)
          ),
          new TextBlock {
            IsMultiline = false,
            Editable = true
          }.Bind(
            new Binding("Text", "Name").MakeTwoWay()
          ),
          // property type, if known
          new TextBlock().Bind(
            new Binding("Text", "Type", (t) => { return (t as string != null ? ": " : ""); })),
          new TextBlock {
            IsMultiline = false,
            Editable = true
          }.Bind(
            new Binding("Text", "Type").MakeTwoWay()
          ),
          // property default value, if any
          new TextBlock {
            IsMultiline = false,
            Editable = false
          }.Bind(
            new Binding("Text", "Default", (s) => {
              return s as string != null ? " = " + s : "";
            })
          )
        );

      // the item template for methods
      var methodTemplate =
        new Panel(PanelType.Horizontal).Add(
          // method visibility/access
          new TextBlock {
            IsMultiline = false,
            Editable = false,
            Width = 12
          }.Bind(
            new Binding("Text", "Visibility", ConvertVisibility)
          ),
          // method name, underlined if scope=="class" to indicate static method
          new TextBlock {
            IsMultiline = false,
            Editable = true
          }.Bind(
            new Binding("Text", "Name").MakeTwoWay()
          ),
          // method parameters
          new TextBlock {
            Text = "()"
          }.Bind(
            // this does not permit adding/editing/removing of parameters via inplace edits
            new Binding("Text", "Parameters", (parrAsObj) => {
              var parr = parrAsObj as List<ClassParam>;
              var s = "(";
              for (var i = 0; i < parr.Count; i++) {
                var param = parr[i];
                if (i > 0) s += ", ";
                s += param.Name + ": " + param.Type;
              }
              return s + ")";
            })
          ),
          // method return type, if any
          new TextBlock {
            Text = ""
          }.Bind(
            new Binding("Text", "Type", (t) => {
              return (t as string != null ? ": " : "");
            })
          ),
          new TextBlock {
            IsMultiline = false,
            Editable = true
          }.Bind(
            new Binding("Text", "Type").MakeTwoWay()
          )
        );

      // this simple template does not have any buttons to permit adding or
      // removing properties or methods, but it could!
      _Diagram.NodeTemplate =
        new Node(PanelType.Auto) {
          LocationSpot = Spot.Center,
          FromSpot = Spot.AllSides,
          ToSpot = Spot.AllSides
        }.Add(
          new Shape {
            Fill = "lightyellow"
          },
          new Panel(PanelType.Table) {
            DefaultRowSeparatorStroke = "black"
          }.Add(
            // header
            new TextBlock {
              Row = 0,
              ColumnSpan = 2,
              Margin = 3,
              Alignment = Spot.Center,
              Font = new Font("Segoe UI", 12, Northwoods.Go.FontWeight.Bold),
              IsMultiline = false,
              Editable = true
            }.Bind(
              new Binding("Text", "Name").MakeTwoWay()
            ),
            // properties
            new TextBlock {
              Text = "Properties",
              Row = 1,
              Font = new Font("Segoe UI", 10, Northwoods.Go.FontStyle.Italic)
            }.Bind(
              new Binding("Visible", "Visible", (v) => {
                return !(v as bool? ?? false);
              }).OfElement("PROPERTIES")),
            new Panel(PanelType.Vertical) {
              Name = "PROPERTIES",
              Row = 1,
              Margin = 3,
              Stretch = Stretch.Fill,
              DefaultAlignment = Spot.Left,
              Background = "lightyellow",
              ItemTemplate = propertyTemplate
            }.Bind(
              new Binding("ItemList", "Properties")
            ),
            Builder.Make<Panel>("PanelExpanderButton", "PROPERTIES").Set(
              new {
                Row = 1,
                Column = 1,
                Alignment = Spot.TopRight,
                Visible = false
              }
            ).Bind(
              new Binding("Visible", "Properties", (arr) => {
                return (arr as List<ClassProperty>).Count > 0;
              })
            ),
            // methods
            new TextBlock {
              Text = "Methods",
              Row = 2,
              Font = new Font("Segoe UI", 10, Northwoods.Go.FontStyle.Italic)
            }.Bind(
              new Binding("Visible", "Visible", (v) => {
                return !(v as bool? ?? false);
              }).OfElement("METHODS")
            ),
            new Panel(PanelType.Vertical) {
              Name = "METHODS",
              Row = 2,
              Margin = 3,
              Stretch = Stretch.Fill,
              DefaultAlignment = Spot.Left,
              Background = "lightyellow",
              ItemTemplate = methodTemplate
            }.Bind(
              new Binding("ItemList", "Methods")
            ),
            Builder.Make<Panel>("PanelExpanderButton", "METHODS").Set(
              new {
                Row = 2,
                Column = 1,
                Alignment = Spot.TopRight,
                Visible = false
              }
            ).Bind(
              new Binding("Visible", "Methods", (arr) => {
                return (arr as List<ClassMethod>).Count > 0;
              })
            )
          )
        );

      object ConvertIsTreeLink(object r) {
        return r as string == "generalization";
      }

      object ConvertFromArrow(object r) {
        switch (r as string) {
          case "generalization": return "";
          default: return "";
        }
      }

      object ConvertToArrow(object r) {
        switch (r as string) {
          case "generalization": return "Triangle";
          case "aggregation": return "StretchedDiamond";
          default: return "";
        }
      }

      // link template
      _Diagram.LinkTemplate =
        new Link {
          Routing = LinkRouting.Orthogonal
        }.Bind(
          new Binding("IsLayoutPositioned", "Relationship", ConvertIsTreeLink)
        ).Add(
          new Shape(),
          new Shape {
            Scale = 1.3,
            Fill = "white"
          }.Bind(
            new Binding("FromArrow", "Relationship", ConvertFromArrow)
          ),
          new Shape {
            Scale = 1.3,
            Fill = "white"
          }.Bind(
            new Binding("ToArrow", "Relationship", ConvertToArrow)
          )
        );

      var nodedata = new List<NodeData> {
        new NodeData {
          Key = 1,
          Name = "BankAccount",
          Properties = new List<ClassProperty> {
            new ClassProperty { Name = "owner", Type = "String", Visibility = "public" },
            new ClassProperty { Name = "balance", Type = "Currency", Visibility = "public", Default = "0" }
          },
          Methods = new List<ClassMethod> {
            new ClassMethod { Name = "deposit", Parameters = new List<ClassParam> { new ClassParam { Name = "amount", Type = "Currency" } }, Visibility = "public" },
            new ClassMethod { Name = "withdraw", Parameters = new List<ClassParam> { new ClassParam { Name = "amount", Type = "Currency" } }, Visibility = "public" }
          }
        },
        new NodeData {
          Key = 11,
          Name = "Person",
          Properties = new List<ClassProperty> {
            new ClassProperty { Name = "name", Type = "String", Visibility = "public" },
            new ClassProperty { Name = "birth", Type = "Date", Visibility = "protected" }
          },
          Methods = new List<ClassMethod> {
            new ClassMethod { Name = "getCurrentAge", Type = "int", Visibility = "public" }
          }
        },
        new NodeData {
          Key = 12,
          Name = "Student",
          Properties = new List<ClassProperty> {
            new ClassProperty { Name = "classes", Type = "List<Course>", Visibility = "public" }
          },
          Methods = new List<ClassMethod> {
            new ClassMethod { Name = "attend", Parameters = new List<ClassParam> { new ClassParam { Name = "class", Type = "Course" } }, Visibility = "private" },
            new ClassMethod { Name = "sleep", Visibility = "private" }
          }
        },
        new NodeData {
          Key = 13,
          Name = "Professor",
          Properties = new List<ClassProperty> {
            new ClassProperty { Name = "classes", Type = "List<Course>", Visibility = "public" }
          },
          Methods = new List<ClassMethod> {
            new ClassMethod { Name = "teach", Parameters = new List<ClassParam> { new ClassParam { Name = "class", Type = "Course" } }, Visibility = "private" }
          }
        },
        new NodeData {
          Key = 14,
          Name = "Course",
          Properties = new List<ClassProperty> {
            new ClassProperty { Name = "name", Type = "String", Visibility = "public" },
            new ClassProperty { Name = "description", Type = "String", Visibility = "public" },
            new ClassProperty { Name = "professor", Type = "Professor", Visibility = "public" },
            new ClassProperty { Name = "location", Type = "String", Visibility = "public" },
            new ClassProperty { Name = "times", Type = "List<Time>", Visibility = "public" },
            new ClassProperty { Name = "prerequisites", Type = "List<Course>", Visibility = "public" },
            new ClassProperty { Name = "students", Type = "List<Student>", Visibility = "public" }
          }
        }
      };
      var linkdata = new List<LinkData> {
        new LinkData { From = 12, To = 11, Relationship = "generalization" },
        new LinkData { From = 13, To = 11, Relationship = "generalization" },
        new LinkData { From = 14, To = 13, Relationship = "aggregation" }
      };

      _Diagram.Model = new Model {
        NodeDataSource = nodedata,
        LinkDataSource = linkdata
      };
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, int, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public string Name { get; set; }
    public List<ClassProperty> Properties { get; set; }
    public List<ClassMethod> Methods { get; set; }

    public override object Clone() {
      return new NodeData {
        Name = Name,
        Properties = Properties.ConvertAll((o) => {
          return o.Clone();
        }),
        Methods = Methods.ConvertAll((o) => {
          return o.Clone();
        })
      };
    }
  }
  public class LinkData : Model.LinkData {
    public string Relationship { get; set; }
  }

  public class ClassParam {
    public string Name { get; set; }
    public string Type { get; set; }

    public ClassParam Clone() {
      return new ClassParam {
        Name = Name,
        Type = Type
      };
    }
  }

  public class ClassProperty {
    public string Name { get; set; }
    public string Type { get; set; }
    public string Visibility { get; set; }
    public string Default { get; set; }

    public ClassProperty Clone() {
      return new ClassProperty {
        Name = Name,
        Type = Type,
        Visibility = Visibility,
        Default = Default
      };
    }
  }

  public class ClassMethod {
    public string Name { get; set; }
    public List<ClassParam> Parameters { get; set; }
    public string Visibility { get; set; }
    public string Type { get; set; }

    public ClassMethod Clone() {
      return new ClassMethod {
        Name = Name,
        Parameters = Parameters.ConvertAll((o) => {
          return o.Clone();
        }),
        Visibility = Visibility,
        Type = Type
      };
    }
  }
}
