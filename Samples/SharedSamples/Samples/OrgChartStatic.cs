/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.Linq;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace Demo.Samples.OrgChartStatic {
  public partial class OrgChartStatic : DemoControl {
    private Diagram myDiagram;
    private Overview myOverview;

    public OrgChartStatic() {
      InitializeComponent();

      myDiagram = diagramControl1.Diagram;
      myOverview = overviewControl1.Diagram as Overview;

      Setup();
      SetupOverview();

      searchBtn.Click += (sender, e) => SearchDiagram();
      desc1.MdText = DescriptionReader.Read("Samples.OrgChartStatic.md");
    }

    private void Setup() {
      // some constants that will be reused within templates
      var mt8 = new Margin(8, 0, 0, 0);
      var mr8 = new Margin(0, 8, 0, 0);
      var ml8 = new Margin(0, 0, 0, 8);
      object roundedRectangleParams = new {
        Parameter1 = 2,  // set rounded corner
        Spot1 = Spot.TopLeft, Spot2 = Spot.BottomRight  // make content go all the way to inside edges of rounded corners
      };

      // Put the diagram contents at the top center of the viewport
      myDiagram.InitialDocumentSpot = Spot.Top;
      myDiagram.InitialViewportSpot = Spot.Top;
      myDiagram.Layout = new TreeLayout { // use a TreeLayout to position all of the nodes
        IsOngoing = false,  // don't relayout when expanding/collapsing panels
        TreeStyle = TreeStyle.LastParents,
        // properties for most of the tree:
        Angle = 90,
        LayerSpacing = 80,
        // properties for the "last parents":
        AlternateAngle = 0,
        AlternateAlignment = TreeAlignment.Start,
        AlternateNodeIndent = 15,
        AlternateNodeIndentPastParent = 1,
        AlternateNodeSpacing = 15,
        AlternateLayerSpacing = 40,
        AlternateLayerSpacingParentOverlap = 1,
        AlternatePortSpot = new Spot(0.001, 1, 20, 0),
        AlternateChildPortSpot = Spot.Left
      };

      // Provide a common style for most of the TextBlocks.
      var textStyle =
        new {
          Font = new Font("Malgun Gothic", 12), Stroke = "rgba(0, 0, 0, .60)",
          Visible = false  // only show textblocks when there is corresponding data for them
        };

      Binding textVisible(string field) {
        return new Binding("Visible", field, (val, obj) => {
          if (val is int i) {  // Boss property
            return i != 0;
          } else {  // HeadOf property
            return val != null;
          }
        });
      }

      // define Converters to be used for Bindings
      string flagConverter(object obj, object _) {
        var nation = obj as string;
        var str = "https://www.nwoods.com/images/emojiflags/" + nation + ".png";
        return str;
      }

      // define the Node template
      myDiagram.NodeTemplate =
        new Node(PanelType.Auto) {
          LocationSpot = Spot.Top,
          IsShadowed = true, ShadowBlur = 1,
          ShadowOffset = new Point(0, 1),
          ShadowColor = "rgba(0, 0, 0, .14)",
          SelectionAdornmentTemplate =  // selection adornment to match shape of nodes
              new Adornment(PanelType.Auto)
                .Add(
                  new Shape("RoundedRectangle") { Fill = null, Stroke = "#7986cb", StrokeWidth = 3 }.Set(roundedRectangleParams),
                  new Placeholder()
                )  // end Adornment
        }
          .Add(
            new Shape("RoundedRectangle") { Name = "SHAPE", Fill = "#ffffff", StrokeWidth = 0 }
              .Set(roundedRectangleParams)
              // bluish if highlighted, white otherwise
              .Bind(new Binding("Fill", "IsHighlighted", (h, obj) => { return (h as bool? ?? false) ? "#e8eaf6" : "#ffffff"; }).OfElement()),
            new Panel(PanelType.Vertical)
              .Add(
                new Panel(PanelType.Horizontal) { Margin = 8 }
                  .Add(
                    new Picture  // flag image, only visible if a nation is specified
                      { Margin = mr8, DesiredSize = new Size(50, 50), Visible = false }
                      .Bind(
                        new Binding("Source", "Nation", flagConverter),
                        new Binding("Visible", "Nation", (nat, obj) => {
                          return nat != null;
                        })
                      ),
                    new Panel(PanelType.Table)
                      .Add(
                        new TextBlock {
                          Row = 0, Alignment = Spot.Left,
                          Font = new Font("Malgun Gothic", 16),
                          Stroke = "rgba(0, 0, 0, .87)",
                          MaxSize = new Size(160, double.NaN)
                        }
                          .Bind("Text", "Name"),
                        new TextBlock {
                          Row = 1, Alignment = Spot.Left,
                          MaxSize = new Size(160, double.NaN)
                        }
                          .Set(textStyle)
                          .Bind(textVisible("Title"))
                          .Bind("Text", "Title"),
                        Builder.Make<Panel>("PanelExpanderButton", "INFO")
                          .Set(new {
                            Row = 0, Column = 1, RowSpan = 2, Margin = ml8
                          })
                      )  // end table panel
                  ),  // end horizontal panel
                new Shape {
                  Figure = "LineH",
                  Stroke = "rgba(0, 0, 0, .60)", StrokeWidth = 1,
                  Height = 1, Stretch = Stretch.Horizontal
                }
                  .Bind(new Binding("Visible").OfElement("INFO")),  // only visible when info is expanded
                new Panel(PanelType.Vertical) {
                  Name = "INFO",  // identify to the PanelExpanderButton
                  Stretch = Stretch.Horizontal,  // take up whole available width
                  Margin = 8,
                  DefaultAlignment = Spot.Left,  // thus no need to specify alignment on each element
                }
                  .Add(
                    new TextBlock()
                      .Set(textStyle)
                      .Bind(textVisible("HeadOf"))
                      .Bind("Text", "HeadOf", (head, obj) => { return "Head Of: " + head; }),
                    new TextBlock()
                      .Set(textStyle)
                      .Bind(textVisible("Boss"))
                      .Bind(
                        new Binding("Margin", "HeadOf", (head, obj) => { return mt8; }), // some space above if there is also a HeadOf value
                        new Binding("Text", "Boss", (data, obj) => {
                          var boss = myDiagram.Model.FindNodeDataForKey(data);
                          if (boss != null) {
                            return "Reporting To: " + (boss as NodeData).Name;
                          }
                          return "";
                        })
                      )
                  )  // end INFO panel
              )  // end vertical panel
          );  // end node

      // define the Link template, a simple orthogonal line
      myDiagram.LinkTemplate =
        new Link { Routing = LinkRouting.Orthogonal, Corner = 5, Selectable = false }
          .Add(new Shape { StrokeWidth = 3, Stroke = "#424242" }); // dark gray, rounded corner links

      myDiagram.Model = new Model {
        NodeParentKeyProperty = "Boss",
        NodeDataSource = new List<NodeData> {
          new NodeData { Key = -1, Name = "Ban Ki-moon 반기문", Nation = "SouthKorea", Title = "Secretary-General of the United Nations", HeadOf = "Secretariat" },
          new NodeData { Key = 1, Boss = -1, Name = "Patricia O'Brien", Nation = "Ireland", Title = "Under-Secretary-General for Legal Affairs and United Nations Legal Counsel", HeadOf = "Office of Legal Affairs" },
          new NodeData { Key = 3, Boss = 1, Name = "Peter Taksøe-Jensen", Nation = "Denmark", Title = "Assistant Secretary-General for Legal Affairs" },
          new NodeData { Key = 9, Boss = 3, Name = "Other Employees" },
          new NodeData { Key = 4, Boss = 1, Name = "Maria R. Vicien - Milburn", Nation = "Argentina", Title = "General Legal Division Director", HeadOf = "General Legal Division" },
          new NodeData { Key = 10, Boss = 4, Name = "Other Employees" },
          new NodeData { Key = 5, Boss = 1, Name = "Václav Mikulka", Nation = "CzechRepublic", Title = "Codification Division Director", HeadOf = "Codification Division" },
          new NodeData { Key = 11, Boss = 5, Name = "Other Employees" },
          new NodeData { Key = 6, Boss = 1, Name = "Sergei Tarassenko", Nation = "Russia", Title = "Division for Ocean Affairs and the Law of the Sea Director", HeadOf = "Division for Ocean Affairs and the Law of the Sea" },
          new NodeData { Key = 12, Boss = 6, Name = "Alexandre Tagore Medeiros de Albuquerque", Nation = "Brazil", Title = "Chairman of the Commission on the Limits of the Continental Shelf", HeadOf = "The Commission on the Limits of the Continental Shelf" },
          new NodeData { Key = 17, Boss = 12, Name = "Peter F. Croker", Nation = "Ireland", Title = "Chairman of the Committee on Confidentiality", HeadOf = "The Committee on Confidentiality" },
          new NodeData { Key = 31, Boss = 17, Name = "Michael Anselme Marc Rosette", Nation = "Seychelles", Title = "Vice Chairman of the Committee on Confidentiality" },
          new NodeData { Key = 32, Boss = 17, Name = "Kensaku Tamaki", Nation = "Japan", Title = "Vice Chairman of the Committee on Confidentiality" },
          new NodeData { Key = 33, Boss = 17, Name = "Osvaldo Pedro Astiz", Nation = "Argentina", Title = "Member of the Committee on Confidentiality" },
          new NodeData { Key = 34, Boss = 17, Name = "Yuri Borisovitch Kazmin", Nation = "Russia", Title = "Member of the Committee on Confidentiality" },
          new NodeData { Key = 18, Boss = 12, Name = "Philip Alexander Symonds", Nation = "Australia", Title = "Chairman of the Committee on provision of scientific and technical advice to coastal States", HeadOf = "Committee on provision of scientific and technical advice to coastal States" },
          new NodeData { Key = 35, Boss = 18, Name = "Emmanuel Kalngui", Nation = "Cameroon", Title = "Vice Chairman of the Committee on provision of scientific and technical advice to coastal States" },
          new NodeData { Key = 36, Boss = 18, Name = "Sivaramakrishnan Rajan", Nation = "India", Title = "Vice Chairman of the Committee on provision of scientific and technical advice to coastal States" },
          new NodeData { Key = 37, Boss = 18, Name = "Francis L. Charles", Nation = "TrinidadAndTobago", Title = "Member of the Committee on provision of scientific and technical advice to costal States" },
          new NodeData { Key = 38, Boss = 18, Name = "Mihai Silviu German", Nation = "Romania", Title = "Member of the Committee on provision of scientific and technical advice to costal States" },
          new NodeData { Key = 19, Boss = 12, Name = "Lawrence Folajimi Awosika", Nation = "Nigeria", Title = "Vice Chairman of the Commission on the Limits of the Continental Shelf" },
          new NodeData { Key = 20, Boss = 12, Name = "Harald Brekke", Nation = "Norway", Title = "Vice Chairman of the Commission on the Limits of the Continental Shelf" },
          new NodeData { Key = 21, Boss = 12, Name = "Yong-Ahn Park", Nation = "SouthKorea", Title = "Vice Chairman of the Commission on the Limits of the Continental Shelf" },
          new NodeData { Key = 22, Boss = 12, Name = "Abu Bakar Jaafar", Nation = "Malaysia", Title = "Chairman of the Editorial Committee", HeadOf = "Editorial Committee" },
          new NodeData { Key = 23, Boss = 12, Name = "Galo Carrera Hurtado", Nation = "Mexico", Title = "Chairman of the Training Committee", HeadOf = "Training Committee" },
          new NodeData { Key = 24, Boss = 12, Name = "Indurlall Fagoonee", Nation = "Mauritius", Title = "Member of the Commission on the Limits of the Continental Shelf" },
          new NodeData { Key = 25, Boss = 12, Name = "George Jaoshvili", Nation = "Georgia", Title = "Member of the Commission on the Limits of the Continental Shelf" },
          new NodeData { Key = 26, Boss = 12, Name = "Wenzhang Lu", Nation = "China", Title = "Member of the Commission on the Limits of the Continental Shelf" },
          new NodeData { Key = 27, Boss = 12, Name = "Isaac Owusu Orudo", Nation = "Ghana", Title = "Member of the Commission on the Limits of the Continental Shelf" },
          new NodeData { Key = 28, Boss = 12, Name = "Fernando Manuel Maia Pimentel", Nation = "Portugal", Title = "Member of the Commission on the Limits of the Continental Shelf" },
          new NodeData { Key = 7, Boss = 1, Name = "Renaud Sorieul", Nation = "France", Title = "International Trade Law Division Director", HeadOf = "International Trade Law Division" },
          new NodeData { Key = 13, Boss = 7, Name = "Other Employees" },
          new NodeData { Key = 8, Boss = 1, Name = "Annebeth Rosenboom", Nation = "Netherlands", Title = "Treaty Section Chief", HeadOf = "Treaty Section" },
          new NodeData { Key = 14, Boss = 8, Name = "Bradford Smith", Nation = "UnitedStates", Title = "Substantive Legal Issues Head", HeadOf = "Substantive Legal Issues" },
          new NodeData { Key = 29, Boss = 14, Name = "Other Employees" },
          new NodeData { Key = 15, Boss = 8, Name = "Andrei Kolomoets", Nation = "Russia", Title = "Technical/Legal Issues Head", HeadOf = "Technical/Legal Issues" },
          new NodeData { Key = 30, Boss = 15, Name = "Other Employees" },
          new NodeData { Key = 16, Boss = 8, Name = "Other Employees" },
          new NodeData { Key = 2, Boss = -1, Name = "Heads of Other Offices/Departments" }
        }
      };
    }

    private void SetupOverview() {
      myOverview.Observed = myDiagram;
      myOverview.ContentAlignment = Spot.Center;
    }

    // the Search functionality highlights all of the nodes that have at least one data property containing an input string
    private void SearchDiagram() {  // called by button
      var input = textBox1.Text;
      if ((input == "") || (input == null)) return;
      myDiagram.Focus();

      myDiagram.StartTransaction("highlight search");

      if (input != "") {
        // search four different data properties for the string, any of which may match for success
        var data = myDiagram.Model.NodeDataSource as IEnumerable<NodeData>;
        var results = new HashSet<Node>();
        foreach (var nd in data) {
          var found = nd.Name?.Contains(input, StringComparison.OrdinalIgnoreCase);
          if (found == null) found = nd.Nation?.Contains(input, StringComparison.OrdinalIgnoreCase);
          if (found == null) found = nd.Title?.Contains(input, StringComparison.OrdinalIgnoreCase);
          if (found == null) found = nd.HeadOf?.Contains(input, StringComparison.OrdinalIgnoreCase);
          if (found == true) {
            results.Add(myDiagram.FindNodeForData(nd));
          }
        }
        myDiagram.Highlight(results);
        // try to center the diagram at the first node that was found
        if (results.Count > 0) myDiagram.CenterRect(results.FirstOrDefault().ActualBounds);
      } else {  // empty string only clears highlighteds collection
        myDiagram.ClearHighlighteds();
      }
      myDiagram.CommitTransaction("highlight search");
    }
  }

  // define the model data
  public class Model : TreeModel<NodeData, int, object> { }
  public class NodeData : Model.NodeData {
    public int Boss { get; set; }
    public string Name { get; set; }
    public string Nation { get; set; }
    public string Title { get; set; }
    public string HeadOf { get; set; }
  }
}
