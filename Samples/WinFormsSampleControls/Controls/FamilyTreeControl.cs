using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace WinFormsSampleControls.FamilyTree {
  [ToolboxItem(false)]
  public partial class FamilyTreeControl : System.Windows.Forms.UserControl {
    private Diagram myDiagram;

    public FamilyTreeControl() {
      InitializeComponent();

      Setup();

      btnZoomToFit.Click += (e, obj) => ZoomToFit();
      btnCenterOnRoot.Click += (e, obj) => CenterRoot();

      goWebBrowser1.Html = @"
        <p>This family tree diagram shows several generations of British royalty beginning with George V (1865-1936).</p>
        <p><a>Node</a> data contains information about gender, and a data binding assigns a corresponding color. Additional data is displayed with a tooltip. A key is placed on the diagram using a <a>PanelLayoutTable</a>.</p>
        <p>For a variation of this tree, see the <a href=""FamilyTreeJP"">Japanese family tree sample</a>.</p>
        <p>For a more complex family tree see the <a href=""Genogram"">genogram sample</a>.</p>
      ";
    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      myDiagram.AnimationManager.IsEnabled = true;
      myDiagram.ToolManager.HoverDelay = 100; // 100 ms instead of the default 850
      myDiagram.AllowCopy = false;
      myDiagram.Layout = new TreeLayout {
        Angle = 90,
        NodeSpacing = 10,
        LayerSpacing = 40,
        LayerStyle = TreeLayerStyle.Uniform
      };

      var bluegrad = "#90CAF9";
      var pinkgrad = "#F48FB1";

      // Set up a Part as a legend, and place it directly on the diagram
      myDiagram.Add(
        new Part(PanelLayoutTable.Instance) {
          Position = new Point(300, 10),
          Selectable = false
        }.Add(
          new TextBlock("Key") {
            Row = 0, Font = new Font("Segoe UI", 14, FontWeight.Bold)
          }, // end row 0
          new Panel(PanelLayoutHorizontal.Instance) {
            Row = 1,
            Alignment = Spot.Left
          }.Add(
            new Shape("Rectangle") {
              DesiredSize = new Size(30, 30), Fill = bluegrad, Margin = 5
            },
            new TextBlock("Males") { Font = new Font("Segoe UI", 13, FontWeight.Bold) }
          ), // end row 1
          new Panel(PanelLayoutHorizontal.Instance) {
            Row = 2,
            Alignment = Spot.Left
          }.Add(
            new Shape("Rectangle") {
              DesiredSize = new Size(30, 30), Fill = pinkgrad, Margin = 5
            },
            new TextBlock("Females") { Font = new Font("Segoe UI", 13, FontWeight.Bold) }
          ) // end row 2
        )
      );

      object tooltipTextConverter(object person, object _) {
        if (person is NodeData data) {
          var str = "";
          str += "Born: " + data.BirthYear;
          if (data.DeathYear != null) str += "\nDied: " + data.DeathYear;
          if (data.Reign != null) str += "\nReign = " + data.Reign;
          return str;
        }
        return null; // should never occur
      }

      var tooltipTemplate = Builder.Make<Adornment>("ToolTip").Add(
        new TextBlock {
          Font = new Font("Arial", 8, FontWeight.Bold, FontUnit.Point),
          Wrap = Wrap.Fit,
          Margin = 5
        }.Bind("Text", "", tooltipTextConverter)
      );

      // define Converters to be used for bindings
      object genderBrushConverter(object gender, object _) {
        if ((string)gender == "M") return bluegrad;
        if ((string)gender == "F") return pinkgrad;
        return "orange";
      }

      myDiagram.NodeTemplate = new Node(PanelLayoutAuto.Instance) {
        Deletable = false,
        ToolTip = tooltipTemplate
      }.Bind("Text", "Name").Add(
        new Shape("Rectangle") {
          Fill = "lightgray",
          Stroke = null, StrokeWidth = 0,
          Stretch = Stretch.Fill,
          Alignment = Spot.Center
        }.Bind("Fill", "Gender", genderBrushConverter),
        new TextBlock {
          Font = new Font("Segoe UI", 12, FontWeight.Bold),
          TextAlign = TextAlign.Center,
          Margin = 10, MaxSize = new Size(80, double.NaN)
        }.Bind("Text", "Name")
      );

      myDiagram.LinkTemplate = new Link {
        Routing = LinkRouting.Orthogonal,
        Corner = 5,
        Selectable = false
      }.Add(
        new Shape {
          StrokeWidth = 3,
          Stroke = "#424242"
        }
      );

      // here's the family data
      var nodeDataSource = new List<NodeData> {
       new NodeData { Key = -1, Name = "George V", Gender = "M", BirthYear = "1865", DeathYear = "1936", Reign = "1910-1936" },
       new NodeData { Key = 1, Parent = -1, Name = "Edward VIII", Gender = "M", BirthYear = "1894", DeathYear = "1972", Reign = "1936" },
       new NodeData { Key = 2, Parent = -1, Name = "George VI", Gender = "M", BirthYear = "1895", DeathYear = "1952", Reign = "1936-1952" },
       new NodeData { Key = 7, Parent = 2, Name = "Elizabeth II", Gender = "F", BirthYear = "1926", Reign = "1952-" },
       new NodeData { Key = 16, Parent = 7, Name = "Charles, Prince of Wales", Gender = "M", BirthYear = "1948" },
       new NodeData { Key = 38, Parent = 16, Name = "Prince William", Gender = "M", BirthYear = "1982" },
       new NodeData { Key = 39, Parent = 16, Name = "Prince Harry of Wales", Gender = "M", BirthYear = "1984" },
       new NodeData { Key = 17, Parent = 7, Name = "Anne, Princess Royal", Gender = "F", BirthYear = "1950" },
       new NodeData { Key = 40, Parent = 17, Name = "Peter Phillips", Gender = "M", BirthYear = "1977" },
       new NodeData { Key = 82, Parent = 40, Name = "Savannah Phillips", Gender = "F", BirthYear = "2010" },
       new NodeData { Key = 41, Parent = 17, Name = "Zara Phillips", Gender = "F", BirthYear = "1981" },
       new NodeData { Key = 18, Parent = 7, Name = "Prince Andrew", Gender = "M", BirthYear = "1960" },
       new NodeData { Key = 42, Parent = 18, Name = "Princess Beatrice of York", Gender = "F", BirthYear = "1988" },
       new NodeData { Key = 43, Parent = 18, Name = "Princess Eugenie of York", Gender = "F", BirthYear = "1990" },
       new NodeData { Key = 19, Parent = 7, Name = "Prince Edward", Gender = "M", BirthYear = "1964" },
       new NodeData { Key = 44, Parent = 19, Name = "Lady Louise Windsor", Gender = "F", BirthYear = "2003" },
       new NodeData { Key = 45, Parent = 19, Name = "James, Viscount Severn", Gender = "M", BirthYear = "2007" },
       new NodeData { Key = 8, Parent = 2, Name = "Princess Margaret", Gender = "F", BirthYear = "1930", DeathYear = "2002" },
       new NodeData { Key = 20, Parent = 8, Name = "David Armstrong-Jones", Gender = "M", BirthYear = "1961" },
       new NodeData { Key = 21, Parent = 8, Name = "Lady Sarah Chatto", Gender = "F", BirthYear = "1964" },
       new NodeData { Key = 46, Parent = 21, Name = "Samuel Chatto", Gender = "M", BirthYear = "1996" },
       new NodeData { Key = 47, Parent = 21, Name = "Arthur Chatto", Gender = "M", BirthYear = "1999" },
       new NodeData { Key = 3, Parent = -1, Name = "Mary, Princess Royal", Gender = "F", BirthYear = "1897", DeathYear = "1965" },
       new NodeData { Key = 9, Parent = 3, Name = "George Lascelles", Gender = "M", BirthYear = "1923", DeathYear = "2011" },
       new NodeData { Key = 22, Parent = 9, Name = "David Lascelles", Gender = "M", BirthYear = "1950" },
       new NodeData { Key = 48, Parent = 22, Name = "Emily Shard", Gender = "F", BirthYear = "1975" },
       new NodeData { Key = 49, Parent = 22, Name = "Benjamin Lascelles", Gender = "M", BirthYear = "1978" },
       new NodeData { Key = 50, Parent = 22, Name = "Alexander Lascelles", Gender = "M", BirthYear = "1980" },
       new NodeData { Key = 51, Parent = 22, Name = "Edward Lascelles", Gender = "M", BirthYear = "1982" },
       new NodeData { Key = 23, Parent = 9, Name = "James Lascelles", Gender = "M", BirthYear = "1953" },
       new NodeData { Key = 52, Parent = 23, Name = "Sophie Lascelles", Gender = "F", BirthYear = "1973" },
       new NodeData { Key = 53, Parent = 23, Name = "Rowan Lascelles", Gender = "M", BirthYear = "1977" },
       new NodeData { Key = 54, Parent = 23, Name = "Tanit Lascelles", Gender = "F", BirthYear = "1981" },
       new NodeData { Key = 55, Parent = 23, Name = "Tewa Lascelles", Gender = "M", BirthYear = "1985" },
       new NodeData { Key = 24, Parent = 9, Name = "Jeremy Lascelles", Gender = "M", BirthYear = "1955" },
       new NodeData { Key = 56, Parent = 24, Name = "Thomas Lascelles", Gender = "M", BirthYear = "1982" },
       new NodeData { Key = 57, Parent = 24, Name = "Ellen Lascelles", Gender = "F", BirthYear = "1984" },
       new NodeData { Key = 58, Parent = 24, Name = "Amy Lascelles", Gender = "F", BirthYear = "1986" },
       new NodeData { Key = 59, Parent = 24, Name = "Tallulah Lascelles", Gender = "F", BirthYear = "2005" },
       new NodeData { Key = 25, Parent = 9, Name = "Mark Lascelles", Gender = "M", BirthYear = "1964" },
       new NodeData { Key = 60, Parent = 25, Name = "Charlotte Lascelles", Gender = "F", BirthYear = "1996" },
       new NodeData { Key = 61, Parent = 25, Name = "Imogen Lascelles", Gender = "F", BirthYear = "1998" },
       new NodeData { Key = 62, Parent = 25, Name = "Miranda Lascelles", Gender = "F", BirthYear = "2000" },
       new NodeData { Key = 10, Parent = 3, Name = "Gerald Lascelles", Gender = "M", BirthYear = "1924", DeathYear = "1998" },
       new NodeData { Key = 26, Parent = 10, Name = "Henry Lascelles", Gender = "M", BirthYear = "1953" },
       new NodeData { Key = 63, Parent = 26, Name = "Maximilian Lascelles", Gender = "M", BirthYear = "1991" },
       new NodeData { Key = 27, Parent = 10, Name = "Martin David Lascelles", Gender = "M", BirthYear = "1962" },
       new NodeData { Key = 64, Parent = 27, Name = "Alexander Lascelles", Gender = "M", BirthYear = "2002" },
       new NodeData { Key = 4, Parent = -1, Name = "Prince Henry", Gender = "M", BirthYear = "1900", DeathYear = "1974" },
       new NodeData { Key = 11, Parent = 4, Name = "Prince William of Gloucester", Gender = "M", BirthYear = "1941", DeathYear = "1972" },
       new NodeData { Key = 12, Parent = 4, Name = "Prince Richard", Gender = "M", BirthYear = "1944" },
       new NodeData { Key = 28, Parent = 12, Name = "Alexander Windsor", Gender = "M", BirthYear = "1974" },
       new NodeData { Key = 65, Parent = 28, Name = "Xan Windsor", Gender = "M", BirthYear = "2007" },
       new NodeData { Key = 66, Parent = 28, Name = "Lady Cosima Windsor", Gender = "F", BirthYear = "2010" },
       new NodeData { Key = 29, Parent = 12, Name = "Lady Davina Lewis", Gender = "F", BirthYear = "1977" },
       new NodeData { Key = 67, Parent = 29, Name = "Senna Lewis", Gender = "F", BirthYear = "2010" },
       new NodeData { Key = 30, Parent = 12, Name = "Lady Rose Gilman", Gender = "F", BirthYear = "1980" },
       new NodeData { Key = 68, Parent = 30, Name = "Lyla Gilman", Gender = "F", BirthYear = "2010" },
       new NodeData { Key = 5, Parent = -1, Name = "Prince George", Gender = "M", BirthYear = "1902", DeathYear = "1942" },
       new NodeData { Key = 13, Parent = 5, Name = "Prince Edward", Gender = "M", BirthYear = "1935" },
       new NodeData { Key = 31, Parent = 13, Name = "George Windsor", Gender = "M", BirthYear = "1962" },
       new NodeData { Key = 69, Parent = 31, Name = "Edward Windsor", Gender = "M", BirthYear = "1988" },
       new NodeData { Key = 70, Parent = 31, Name = "Lady Marina-Charlotte Windsor", Gender = "F", BirthYear = "1992" },
       new NodeData { Key = 71, Parent = 31, Name = "Lady Amelia Windsor", Gender = "F", BirthYear = "1995" },
       new NodeData { Key = 32, Parent = 13, Name = "Lady Helen Taylor", Gender = "F", BirthYear = "1964" },
       new NodeData { Key = 72, Parent = 32, Name = "Columbus Taylor", Gender = "M", BirthYear = "1994" },
       new NodeData { Key = 73, Parent = 32, Name = "Cassius Taylor", Gender = "M", BirthYear = "1996" },
       new NodeData { Key = 74, Parent = 32, Name = "Eloise Taylor", Gender = "F", BirthYear = "2003" },
       new NodeData { Key = 75, Parent = 32, Name = "Estella Taylor", Gender = "F", BirthYear = "2004" },
       new NodeData { Key = 33, Parent = 13, Name = "Lord Nicholas Windsor", Gender = "M", BirthYear = "1970" },
       new NodeData { Key = 76, Parent = 33, Name = "Albert Windsor", Gender = "M", BirthYear = "2007" },
       new NodeData { Key = 77, Parent = 33, Name = "Leopold Windsor", Gender = "M", BirthYear = "2009" },
       new NodeData { Key = 14, Parent = 5, Name = "Princess Alexandra", Gender = "F", BirthYear = "1936" },
       new NodeData { Key = 34, Parent = 14, Name = "James Ogilvy", Gender = "M", BirthYear = "1964" },
       new NodeData { Key = 78, Parent = 34, Name = "Flora Ogilvy", Gender = "F", BirthYear = "1994" },
       new NodeData { Key = 79, Parent = 34, Name = "Alexander Ogilvy", Gender = "M", BirthYear = "1996" },
       new NodeData { Key = 35, Parent = 14, Name = "Marina Ogilvy", Gender = "F", BirthYear = "1966" },
       new NodeData { Key = 80, Parent = 35, Name = "Zenouska Mowatt", Gender = "F", BirthYear = "1990" },
       new NodeData { Key = 81, Parent = 35, Name = "Christian Mowatt", Gender = "M", BirthYear = "1993" },
       new NodeData { Key = 15, Parent = 5, Name = "Prince Michael of Kent", Gender = "M", BirthYear = "1942" },
       new NodeData { Key = 36, Parent = 15, Name = "Lord Frederick Windsor", Gender = "M", BirthYear = "1979" },
       new NodeData { Key = 37, Parent = 15, Name = "Lady Gabriella Windsor", Gender = "F", BirthYear = "1981" },
       new NodeData { Key = 6, Parent = -1, Name = "Prince John", Gender = "M", BirthYear = "1905", DeathYear = "1919" }
      };

      myDiagram.Model = new Model {
        NodeDataSource = nodeDataSource
      };
    }

    private void ZoomToFit() {
      myDiagram.CommandHandler.ZoomToFit();
    }

    private void CenterRoot() {
      myDiagram.Scale = 1;
      myDiagram.CommandHandler.ScrollToPart(myDiagram.FindNodeForKey(-1));
    }


  }

  // Define the model data
  public class Model : TreeModel<NodeData, int, object> { }

  public class NodeData : Model.NodeData {
    public string Name { get; set; }
    public string Gender { get; set; }
    public string BirthYear { get; set; }
    public string DeathYear { get; set; }
    public string Reign { get; set; }
  }

}
