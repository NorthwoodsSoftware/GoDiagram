using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.AddRemoveColumns {
  [ToolboxItem(false)]
  public partial class AddRemoveColumnsControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public AddRemoveColumnsControl() {
      InitializeComponent();

      Setup();


      btnInsertIntoArray.Click += (e, obj) => InsertIntoArray();
      btnRemoveFromArray.Click += (e, obj) => RemoveFromArray();
      btnAddColumn.Click += (e, obj) => AddColumn("");
      btnRemoveColumn.Click += (e, obj) => RemoveColumn();
      btnSwapColumns.Click += (e, obj) => SwapTwoColumns();

      goWebBrowser1.Html = @"

   <p>Add a row or Remove the second row of the table held by the selected node:</p>

";

      goWebBrowser2.Html = @"

   <p>Add a column or remove the fourth column from the table of the selected node:</p>

";

      goWebBrowser3.Html = @"

   <p>Swap the ""phone"" and ""office"" columns for each selected node:</p>
";

      goWebBrowser4.Html = @"

   <p>See also the <a href=""ColumnResizing"">Column and Row Resizing Tools</a></p>
";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.UndoManager.IsEnabled = true;

      // node template
      myDiagram.NodeTemplate =
        new Node(PanelLayoutAuto.Instance).Add(
          new Shape {
            Fill = "white"
          },
          new Panel(PanelLayoutTable.Instance) {
            // the rows for the people
            DefaultAlignment = Spot.Left,
            DefaultColumnSeparatorStroke = "black",
            ItemTemplate =  // bound to a person/row data object
              new Panel(PanelLayoutTableRow.Instance) {
                ItemTemplate =  // bound to a cell object
                  new Panel { // each of which as "attr" and "text" properties
                    Stretch = Stretch.Fill,
                    Alignment = Spot.TopLeft
                  }.Bind(
                    new Binding("Column", "Attr", (a, elt) => {  // ELT is this bound item/cell Panel
                      // elt.Data will be the cell object
                      // elt.Panel.Data will be the person/row data object
                      // elt.Part.Data will be the node data object
                      // "columnDefinitions" is on the node data object, so:
                      var cd = FindColumnDefinitionForName((elt as Panel).Part.Data as NodeData, a as string);
                      if (cd != null) {
                        return cd.Column;
                      }
                      // else
                      throw new Exception("unknown column Name = " + (a as string));
                    }))
                  .Add(
                    // you could also have other Bindings here for this cell
                    new TextBlock {
                      Editable = true,
                      Margin = new Margin(2, 2, 0, 2),
                      Wrap = Wrap.None
                    }.Bind(
                      new Binding("Text").MakeTwoWay()
                    )
                  )
              }.Bind(
                // which in turn consists of a collection of cell objects,
                // held by the "columns" property in an Array
                new Binding("ItemList", "Columns")
              )
            // you could also have other Bindings here for the whole row
          }.Bind(new Binding("ItemList", "People"))
          .Add(
            new RowDefinition {
              Row = 0,
              Background = "lightgray"
            },
            new RowDefinition {
              Row = 1,
              SeparatorStroke = "black"
            })
          .Add(
            // the table headers -- remains even if ItemList is empty
            new Panel(PanelLayoutTableRow.Instance) {
              IsPanelMain = true,
              ItemTemplate =
                new Panel()
                .Bind(
                  new Binding("Column", "Column", (col, _) => {
                    var colAsInt = (col as int? ?? 0);
                    return colAsInt;
                  }))
                .Add(
                  new TextBlock {
                    Margin = new Margin(2, 2, 0, 2),
                    Font = new Font("Segoe UI", 13, FontWeight.Bold)
                  }.Bind(
                    new Binding("Text")
                  )
                )
            }.Bind(
              new Binding("ItemList", "ColumnDefinitions")
            )
          )
        );

      var model = new Model {
        CopyNodeDataFunction = (data, model) => {
          return DeepCopyNodeData(data);
        },
        NodeDataSource = new List<NodeData> {
          new NodeData { // first node
            Key = "1",
            ColumnDefinitions = new List<ColDefData> {
              // each column definition needs to specify the column used
              new ColDefData { Attr = "name", Text = "Name", Column = 0 },
              new ColDefData { Attr = "phone", Text = "Phone #", Column = 1 },
              new ColDefData { Attr = "office", Text = "Office", Column = 2 }
            },
            People = new List<FieldData> {  // the table of people
              // each row is a person with an Array of Objects associating a column name with a text value
              new FieldData { Columns = new List<ColDefData> {new ColDefData { Attr = "name", Text = "Alice" }, new ColDefData { Attr = "phone", Text = "2345" }, new ColDefData { Attr = "office", Text = "C4-E18" }} },
              new FieldData { Columns = new List<ColDefData> {new ColDefData { Attr = "name", Text = "Bob" }, new ColDefData { Attr = "phone", Text = "9876" }, new ColDefData { Attr = "office", Text = "E1-B34" }} },
              new FieldData { Columns = new List<ColDefData> {new ColDefData { Attr = "name", Text = "Carol" }, new ColDefData { Attr = "phone", Text = "1111" }, new ColDefData { Attr = "office", Text = "C4-E23" }} },
              new FieldData { Columns = new List<ColDefData> {new ColDefData { Attr = "name", Text = "Ted" }, new ColDefData { Attr = "phone", Text = "2222" }, new ColDefData { Attr = "office", Text = "C4-E197" }} }
            }
          },
          new NodeData { // second node
            Key = "2",
            ColumnDefinitions = new List<ColDefData> {
              new ColDefData {
                Attr = "name", Text = "Name", Column = 0
              },
              new ColDefData {
                Attr = "phone", Text = "Phone #", Column = 2
              },  // note the different order of columns
              new ColDefData {
                Attr = "office", Text = "Office", Column = 1
              }
            },
            People = new List<FieldData> {
              new FieldData {
                Columns = new List<ColDefData> {
                  new ColDefData {
                    Attr = "name", Text = "Robert"
                  },
                  new ColDefData {
                    Attr = "phone", Text = "5656"
                  },
                  new ColDefData {
                    Attr = "office", Text = "B1-A27"
                  }
                }
              },
              new FieldData {
                Columns = new List<ColDefData> {
                  new ColDefData {
                    Attr = "name", Text = "Natalie"
                  }, new ColDefData {
                    Attr = "phone", Text = "5698"
                  }, new ColDefData {
                    Attr = "office", Text = "B1-B6"
                  }
                }
              }
            }
          }
        },
        LinkDataSource = new List<LinkData> {
          new LinkData { From = "1", To = "2" }
        }
      };

      myDiagram.Model = model;

      NodeData DeepCopyNodeData(NodeData dIn) {
        return new NodeData {
          Key = dIn.Key,
          ColumnDefinitions = dIn.ColumnDefinitions.ConvertAll(DeepCopyColDefData),
          People = dIn.People.ConvertAll(DeepCopyFieldData)
        };
      }

      ColDefData DeepCopyColDefData(object dIn) {
        var d = dIn as ColDefData;
        return new ColDefData {
          Attr = d.Attr,
          Text = d.Text,
          Column = d.Column
        };
      }

      FieldData DeepCopyFieldData(object dIn) {
        var d = dIn as FieldData;
        return new FieldData {
          Columns = d.Columns.ConvertAll(DeepCopyColDefData)
        };
      }
    }

    private void InsertIntoArray() {
      var n = myDiagram.Selection.FirstOrDefault();
      if (n == null) return;
      var d = n.Data as NodeData;
      myDiagram.StartTransaction("insertIntoTable");
      // add item as second in the list, at index #1
      // of course this new data could be more realistic:
      myDiagram.Model.InsertListItem(d.People, 1,
        new FieldData {
          Columns = new List<ColDefData> {
            new ColDefData { Attr = "name", Text = "Elena" },
            new ColDefData { Attr = "phone", Text = "456" },
            new ColDefData { Attr = "office", Text = "LA" }
          }
        }
      );
      myDiagram.CommitTransaction("insertIntoTable");
    }

    private void RemoveFromArray() {
      var n = myDiagram.Selection.FirstOrDefault();
      if (n == null) return;
      var d = n.Data as NodeData;
      if (d.People.Count <= 1) return;
      myDiagram.StartTransaction("removeFromTable");
      // remove second item of list, at index #1
      myDiagram.Model.RemoveListItem(d.People, 1);
      myDiagram.CommitTransaction("removeFromTable");
    }

    // add or remove a column from the selected node's table of people

    private ColDefData FindColumnDefinitionForName(NodeData nodedata, string attrname) {
      var columns = nodedata.ColumnDefinitions;
      for (var i = 0; i < columns.Count; i++) {
        var col = columns[i] as ColDefData;
        if (col.Attr == attrname) return col;
      }
      return null;
    }

    private ColDefData FindColumnDefinitionForColumn(NodeData nodedata, int idx) {
      var columns = nodedata.ColumnDefinitions;
      for (var i = 0; i < columns.Count; i++) {
        var col = columns[i] as ColDefData;
        if (col.Column == idx) return col;
      }
      return null;
    }

    private void AddColumn(string attrname) {
      var n = myDiagram.Selection.FirstOrDefault();
      if (n == null) return;
      var d = n.Data as NodeData;
      // if name is not given, find an unused column name
      if (attrname == null || attrname == "") {
        attrname = "new";
        var count = 1;
        while (FindColumnDefinitionForName(d, attrname) != null) {
          attrname = "new" + (count++).ToString();
        }
      }
      // find an unused column #
      var col = 3;
      while (FindColumnDefinitionForColumn(d, col) != null) {
        col++;
      }
      myDiagram.StartTransaction("addColumn");
      var model = myDiagram.Model as Model;
      // add a column definition for the node's whole table
      model.AddListItem(d.ColumnDefinitions,
        new ColDefData {
          Attr = attrname,
          Text = attrname,
          Column = col
        }
      );
      // add cell to each person in the node's table of people
      var people = d.People;
      var rand = new Random();
      for (var j = 0; j < people.Count; j++) {
        var person = people[j] as FieldData;
        model.AddListItem(person.Columns,
          new ColDefData {
            Attr = attrname,
            Text = Convert.ToInt32(Math.Floor(rand.NextDouble() * 1000)).ToString()
          });
      }
      myDiagram.CommitTransaction("addColumn");
    }

    private void RemoveColumn() {
      var n = myDiagram.Selection.FirstOrDefault();
      if (n == null) return;
      var d = n.Data as NodeData;
      ColDefData coldef = null;
      if (d.ColumnDefinitions.Count > 3) {
        coldef = d.ColumnDefinitions[3] as ColDefData;  // get the fourth column
      }
      if (coldef == null) return;
      var attrname = coldef.Attr;
      myDiagram.StartTransaction("removeColumn");
      var model = myDiagram.Model;
      model.RemoveListItem(d.ColumnDefinitions, 3);
      // update columns for each person in this table
      var people = d.People;
      for (var j = 0; j < people.Count; j++) {
        var person = people[j] as FieldData;
        var columns = person.Columns;
        for (var k = 0; k < columns.Count; k++) {
          var cell = columns[k] as ColDefData;
          if (cell.Attr == attrname) {
            // get rid of this attribute cell from the person.Columns Array
            model.RemoveListItem(columns, k);
            break;
          }
        }
      }
      myDiagram.CommitTransaction("removeColumn");
    }

    private void SwapTwoColumns() {
      myDiagram.StartTransaction("swapColumns");
      foreach (var n in myDiagram.Selection) {
        var model = myDiagram.Model as Model;
        if (!(n is Node)) return;
        var d = n.Data as NodeData;
        var phonedef = FindColumnDefinitionForName(d, "phone");
        if (phonedef == null) return;
        var phonecolumn = phonedef.Column;  // remember the column number
        var officedef = FindColumnDefinitionForName(d, "office");
        if (officedef == null) return;
        var officecolumn = officedef.Column;  // and this one too
        model.Set(phonedef, "Column", officecolumn);
        model.Set(officedef, "Column", phonecolumn);
        model.UpdateTargetBindings(d);  // update all bindings, to get the cells right
      }
      myDiagram.CommitTransaction("swapColumns");
    }

  }

  // define the model data
  public class Model : GraphLinksModel<NodeData, string, object, LinkData, string, string> { }
  public class NodeData : Model.NodeData {
    public List<ColDefData> ColumnDefinitions { get; set; }
    public List<FieldData> People { get; set; }
  }

  public class LinkData : Model.LinkData { }

  public class ColDefData {
    public string Attr { get; set; }
    public string Text { get; set; }
    public int? Column { get; set; }
  }

  public class FieldData {
    public List<ColDefData> Columns { get; set; }
  }

}
