/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;

namespace Demo.Samples.FamilyTreeJP {
  [ToolboxItem(false)]
  public partial class FamilyTreeJPControl : DemoControl {
    private Diagram myDiagram;

    public FamilyTreeJPControl() {
      InitializeComponent();

      Setup();

      goWebBrowser1.Html = @"

  <p>For a variation of this tree, see the <a href=""demo/FamilyTree"">British family tree sample</a>.</p>
  <p>For a more complex family tree see the <a href=""demo/Genogram"">genogram sample</a>.</p>
";

    }

    private void Setup() {
      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.AllowCopy = false;
      myDiagram.Layout = new TreeLayout {
        Angle = 90,
        NodeSpacing = 5
      };

      var bluegrad = new Brush(new LinearGradientPaint(new Dictionary<float, string> {
          { 0, "rgb(60, 204, 254)" },
          { 1, "rgb(70, 172, 254)" }
        }
      ));
      var pinkgrad = new Brush(new LinearGradientPaint(new Dictionary<float, string> {
          { 0, "rgb(255, 192, 203)" },
          { 1, "rgb(255, 142, 203)" }
        }
      ));

      // Set up a Part as a legend, and place it directly on the diagram
      myDiagram.Add(
        new Part(PanelType.Table) {
          Position = new Point(10, 10),
          Selectable = false
        }.Add(
          new TextBlock {
            Text = "Key",
            Row = 0,
            Font = new Font("Segoe UI", 10, FontWeight.Bold)
          },  // end row 0
          new Panel(PanelType.Horizontal) {
            Row = 1,
            Alignment = Spot.Left
          }.Add(
            new Shape {
              Figure = "Rectangle",
              DesiredSize = new Size(30, 30),
              Fill = bluegrad,
              Margin = 5
            },
            new TextBlock {
              Text = "Males",
              Font = new Font("Segoe UI", 8, FontWeight.Bold)
            }
          ),  // end row 1
          new Panel(PanelType.Horizontal) {
            Row = 2,
            Alignment = Spot.Left
          }.Add(
            new Shape {
              Figure = "Rectangle",
              DesiredSize = new Size(30, 30),
              Fill = pinkgrad,
              Margin = 5
            },
            new TextBlock {
              Text = "Females",
              Font = new Font("Segoe UI", 8, FontWeight.Bold)
            }
          )  // end row 2
        )
      );

      // get tooltip text from the object's data
      string TooltipTextConverter(object personAsObj, object _) {
        var person = personAsObj as NodeData;
        var str = "";
        str += "Born: " + person.BirthYear;
        if (person.DeathYear != null) str += "\nDied: " + person.DeathYear;
        if (person.Reign != null) str += "\nReign: " + person.Reign;
        return str;
      }

      // define tooltips for nodes
      var tooltiptemplate =
        Builder.Make<Adornment>("ToolTip").Add(
          new TextBlock {
            Font = new Font("Segoe UI", 8, FontWeight.Bold),
            Wrap = Wrap.Fit,
            Margin = 5
          }.Bind(
            new Binding("Text", "", TooltipTextConverter)
          )
        );
      var tooltipborder = tooltiptemplate.FindElement("Border") as Shape;
      tooltipborder.Fill = "whitesmoke";
      tooltipborder.Stroke = "black";

      // define Converters to be used for Bindings
      object GenderBrushConverter(object genderAsObj, object _) {
        var gender = genderAsObj as string;
        if (gender[0] == 'M') {
          return bluegrad;
        }
        if (gender[0] == 'F') {
          return pinkgrad;
        }
        return "orange";
      }

      // replace the default Node template in the nodeTemplateMap
      myDiagram.NodeTemplate =
        new Node(PanelType.Auto) {
          Deletable = false,
          ToolTip = tooltiptemplate
        }.Bind(
          new Binding("Text", "Name")
        ).Add(
          new Shape {
            Figure = "Rectangle",
            Fill = "orange",
            Stroke = "black",
            Stretch = Stretch.Fill,
            Alignment = Spot.Center
          }.Bind(
            new Binding("Fill", "Gender", GenderBrushConverter)
          ),
          new Panel(PanelType.Vertical).Add(
            new TextBlock {
              Font = new Font("Segoe UI", 8, FontWeight.Bold),
              Alignment = Spot.Center,
              Margin = 6
            }.Bind(
              new Binding("Text", "Name")
            ),
            new TextBlock { Font = new Font("MS UI Gothic", 8) }.Bind(
              new Binding("Text", "KanjiName")
            )
          )
        );

      // define the Link template
      myDiagram.LinkTemplate =
        new Link { // the whole link panel
          Routing = LinkRouting.Orthogonal,
          Corner = 5,
          Selectable = false
        }.Add(
          new Shape()
        );  // the default black link shape

      // model data
      myDiagram.Model = new Model {
        NodeDataSource = new List<NodeData> {
        new NodeData { Key = -1, Name = "Osahito", Gender = "M", FullTitle = "Emperor Kōmei", KanjiName = "統仁 孝明天皇", PosthumousName = "Komei", BirthYear = "1831", DeathYear = "1867" },
        new NodeData { Key = 1, Parent = -1, Name = "Matsuhito", Gender = "M", FullTitle = "Emperor Meiji", KanjiName = "睦仁 明治天皇", PosthumousName = "Meiji", BirthYear = "1852", DeathYear = "1912" },
        new NodeData { Key = 2, Parent = 1, Name = "Toshiko", Gender = "F", FullTitle = "Princess Yasu-no-Miya Toshiko", BirthYear = "1896", DeathYear = "1978", StatusChange = "In 1947, lost imperial family status due to American abrogation of Japanese nobility" },
        new NodeData { Key = 3, Parent = 2, Name = "Higashikuni Morihiro", Gender = "M", FullTitle = "Prince Higashikuni Morihiro", KanjiName = "東久邇宮 盛厚王", BirthYear = "1916", DeathYear = "1969", StatusChange = "In 1947, lost imperial family status due to American abrogation of Japanese nobility" },
        new NodeData { Key = 4, Parent = 3, Name = "See spouse for descendants" },
        new NodeData { Key = 5, Parent = 2, Name = "Moromasa", Gender = "M", FullTitle = "Prince Moromasa", KanjiName = "師正王", BirthYear = "1917", DeathYear = "1923" },
        new NodeData { Key = 6, Parent = 2, Name = "Akitsune", Gender = "M", FullTitle = "Prince Akitsune", KanjiName = "彰常王", BirthYear = "1920", DeathYear = "2006", StatusChange = "In 1947, lost imperial family status due to American abrogation of Japanese nobility" },
        new NodeData { Key = 7, Parent = 2, Name = "Toshihiko", Gender = "M", FullTitle = "Prince Toshihiko", KanjiName = "俊彦王", BirthYear = "1929", StatusChange = "In 1947, lost imperial family status due to American abrogation of Japanese nobility" },
        new NodeData { Key = 8, Parent = 1, Name = "Yoshihito", Gender = "M", FullTitle = "Emperor Taishō", KanjiName = "嘉仁 大正天皇,", PosthumousName = "Taisho", BirthYear = "1879", DeathYear = "1926" },
        new NodeData { Key = 9, Parent = 8, Name = "Hirohito", Gender = "M", FullTitle = "Emperor Showa", KanjiName = "裕仁 昭和天皇", PosthumousName = "Showa", BirthYear = "1901", DeathYear = "1989" },
        new NodeData { Key = 10, Parent = 9, Name = "Higashikuni Shigeko", Gender = "F", Spouse = "Higashikuni Morihiro", SpouseKanji = "東久邇宮 盛厚王", FullTitle = "Princess Shigeko Higashikuni", KanjiName = "東久邇成子", BirthYear = "1925", DeathYear = "1961", StatusChange = "In 1947, lost imperial family status due to American abrogation of Japanese nobility" },
        new NodeData { Key = 11, Parent = 10, Name = "Higashikuni Nobuhiko", Gender = "M", FullTitle = "Prince Higashikuni Nobuhiko", KanjiName = "東久邇宮 信彦王", BirthYear = "1945", StatusChange = "In 1947, lost imperial family status due to American abrogation of Japanese nobility" },
        new NodeData { Key = 12, Parent = 11, Name = "Higashikuni Yukihiko", Gender = "M", FullTitle = "No Title", BirthYear = "1974" },
        new NodeData { Key = 13, Parent = 10, Name = "Higashikuni Fumiko", Gender = "F", FullTitle = "Princess Higashikuni Fumiko", KanjiName = "文子女王", BirthYear = "1946", StatusChange = "In 1947, lost imperial family status due to American abrogation of Japanese nobility" },
        new NodeData { Key = 14, Parent = 10, Name = "Higashikuni Naohiko", Gender = "M", FullTitle = "No Title", KanjiName = "東久邇真彦", BirthYear = "1948" },
        new NodeData { Key = 15, Parent = 14, Name = "Higashikuni Teruhiko", Gender = "M", FullTitle = "No Title" },
        new NodeData { Key = 16, Parent = 14, Name = "Higashikuni Matsuhiko", Gender = "M", FullTitle = "No Title" },
        new NodeData { Key = 17, Parent = 10, Name = "Higashikuni Hidehiko", Gender = "M", FullTitle = "No Title", KanjiName = "東久邇基博", BirthYear = "1949" },
        new NodeData { Key = 18, Parent = 10, Name = "Higashikuni Yuko", Gender = "F", FullTitle = "No Title", KanjiName = "東久邇優子", BirthYear = "1950" },
        new NodeData { Key = 19, Parent = 9, Name = "Sachiko", Gender = "F", FullTitle = "Princess Sachiko", KanjiName = "久宮祐子", BirthYear = "1927", DeathYear = "1928" },
        new NodeData { Key = 20, Parent = 9, Name = "Kazuko Takatsukasa", Gender = "F", FullTitle = "Kazuko, Princess Taka", KanjiName = "鷹司 和子", BirthYear = "1929", DeathYear = "1989", StatusChange = "In 1950, lost imperial family status by marrying a commoner" },
        new NodeData { Key = 21, Parent = 9, Name = "Atsuko Ikeda", Gender = "F", FullTitle = "Atsuko, Princess Yori", KanjiName = "池田厚子", BirthYear = "1931", StatusChange = "In 1952, lost imperial family status by marrying a commoner" },
        new NodeData { Key = 22, Parent = 9, Name = "Akihito", Gender = "M", FullTitle = "Reigning Emperor of Japan; Tennō", KanjiName = "明仁 今上天皇", PosthumousName = "Heisei", BirthYear = "1933" },
        new NodeData { Key = 23, Parent = 22, Name = "Naruhito", Gender = "M", FullTitle = "Naruhito, Crown Prince of Japan", KanjiName = "皇太子徳仁親王", OrderInSuccession = "1", BirthYear = "1960" },
        new NodeData { Key = 24, Parent = 23, Name = "Aiko", Gender = "F", FullTitle = "Aiko, Princess Toshi", KanjiName = "敬宮愛子内親王", BirthYear = "2001" },
        new NodeData { Key = 25, Parent = 22, Name = "Fumihito", Gender = "M", FullTitle = "Fumihito, Prince Akishino", KanjiName = "秋篠宮文仁親王", OrderInSuccession = "2", BirthYear = "1965" },
        new NodeData { Key = 26, Parent = 25, Name = "Mako", Gender = "F", FullTitle = "Princess Mako of Akishino", KanjiName = "眞子内親王", BirthYear = "1991" },
        new NodeData { Key = 27, Parent = 25, Name = "Kako", Gender = "F", FullTitle = "Princess Kako of Akishino", KanjiName = "佳子内親王", BirthYear = "1994" },
        new NodeData { Key = 28, Parent = 25, Name = "Hisahito", Gender = "M", FullTitle = "Prince Hisahito of Akishino", KanjiName = "悠仁親王", OrderInSuccession = "3", BirthYear = "2006" },
        new NodeData { Key = 29, Parent = 22, Name = "Sayako Kuroda", Gender = "F", FullTitle = "Princess Sayako of Japan", KanjiName = "黒田清子", BirthYear = "1969", StatusChange = "In 2005, lost imperial family status by marrying a commoner" },
        new NodeData { Key = 30, Parent = 9, Name = "Masahito", Gender = "M", FullTitle = "Masahito, Prince Hitachi", KanjiName = "常陸宮正仁親王", OrderInSuccession = "4", BirthYear = "1935" },
        new NodeData { Key = 31, Parent = 9, Name = "Takako Shimazu", Gender = "F", FullTitle = "Princess Takako", KanjiName = "島津貴子", BirthYear = "1939", StatusChange = "In 1960, lost imperial family status by marrying a commoner" },
        new NodeData { Key = 32, Parent = 31, Name = "Yorihisa Shimazu", Gender = "M", FullTitle = "No Title", BirthYear = "1962" },
        new NodeData { Key = 33, Parent = 8, Name = "Yasuhito", Gender = "M", FullTitle = "Yasuhito, Prince Chichibu of Japan", KanjiName = "秩父宮 雍仁", BirthYear = "1902", DeathYear = "1953" },
        new NodeData { Key = 34, Parent = 8, Name = "Nobuhito", Gender = "M", FullTitle = "Nobuhito, Prince Takamatsu", KanjiName = "高松宮宣仁親王", BirthYear = "1905", DeathYear = "1987" },
        new NodeData { Key = 35, Parent = 8, Name = "Takahito", Gender = "M", FullTitle = "Takahito, Prince Mikasa", KanjiName = "三笠宮崇仁親王", OrderInSuccession = "5", BirthYear = "1915" },
        new NodeData { Key = 36, Parent = 35, Name = "Yasuko Konoe", Gender = "F", FullTitle = "Princess Yasuko of Mikasa", KanjiName = "甯子内親王", BirthYear = "1944", StatusChange = "In 1966, lost imperial family stutus by marrying a commoner" },
        new NodeData { Key = 37, Parent = 36, Name = "Tadahiro", Gender = "M", FullTitle = "None" },
        new NodeData { Key = 38, Parent = 35, Name = "Tomihito", Gender = "M", FullTitle = "Prince Tomohito of Mikasa", KanjiName = "三笠宮寬仁", OrderInSuccession = "6", BirthYear = "1946" },
        new NodeData { Key = 39, Parent = 38, Name = "Akiko", Gender = "F", FullTitle = "Princess Akiko of Mikasa", KanjiName = "彬子女王", BirthYear = "1981" },
        new NodeData { Key = 40, Parent = 38, Name = "Yoko", Gender = "F", FullTitle = "Princess Yoko of Mikasa", KanjiName = "瑶子女王", BirthYear = "1983" },
        new NodeData { Key = 41, Parent = 35, Name = "Yoshihito", Gender = "M", FullTitle = "Yoshihito, Prince Katsura", KanjiName = "桂宮 宜仁親王", OrderInSuccession = "7", BirthYear = "1948" },
        new NodeData { Key = 42, Parent = 35, Name = "Masako Sen", Gender = "F", FullTitle = "Princess Masako of Mikasa", KanjiName = "容子内親王", BirthYear = "1951", StatusChange = "In 1983, lost imperial family status by marrying a commoner" },
        new NodeData { Key = 43, Parent = 42, Name = "Akifumi", Gender = "M", FullTitle = "No Title" },
        new NodeData { Key = 44, Parent = 42, Name = "Takafumi", Gender = "M", FullTitle = "No Title" },
        new NodeData { Key = 45, Parent = 42, Name = "Makiko", Gender = "F", FullTitle = "No Title" },
        new NodeData { Key = 46, Parent = 35, Name = "Norihito", Gender = "M", FullTitle = "Norihito, Prince Takamado", KanjiName = "高円宮憲仁親王", BirthYear = "1954", DeathYear = "2002" },
        new NodeData { Key = 47, Parent = 46, Name = "Tsuguko", Gender = "F", FullTitle = "Princess Tsuguko of Takamado", KanjiName = "承子女王", BirthYear = "1986" },
        new NodeData { Key = 48, Parent = 46, Name = "Noriko", Gender = "F", FullTitle = "Princess Noriko of Takamado", KanjiName = "典子女王", BirthYear = "1988" },
        new NodeData { Key = 49, Parent = 46, Name = "Ayako", Gender = "F", FullTitle = "Princess Ayako of Takamado", KanjiName = "絢子女王", BirthYear = "1990" },
        new NodeData { Key = 50, Parent = 1, Name = "Masako", Gender = "F", FullTitle = "Princess Masako of Tsune", BirthYear = "1888", DeathYear = "1940" },
        new NodeData { Key = 51, Parent = 50, Name = "Takeda Tsuneyoshi", Gender = "M", FullTitle = "Prince Takeda Tsunehisa", KanjiName = "竹田宮恒徳王", BirthYear = "1909", DeathYear = "1992", StatusChange = "In 1947, lost imperial family status due to American abrogation of Japanese nobility" },
        new NodeData { Key = 52, Parent = 51, Name = "Takeda Tsunetada", Gender = "M", FullTitle = "Prince Takeda Tsunetada", KanjiName = "竹田恒正王", BirthYear = "1940", StatusChange = "In 1947, lost imperial family status due to American abrogation of Japanese nobility" },
        new NodeData { Key = 53, Parent = 52, Name = "Takeda Tsunetaka", Gender = "M", FullTitle = "No Title", BirthYear = "1967" },
        new NodeData { Key = 54, Parent = 52, Name = "Takeda Hiroko", Gender = "M", FullTitle = "No Title", BirthYear = "1971" },
        new NodeData { Key = 55, Parent = 51, Name = "Takeda Motoko", Gender = "F", FullTitle = "Princess Takeda Motoko", KanjiName = "素子女王", BirthYear = "1942", StatusChange = "In 1947, lost imperial family status due to American abrogation of Japanese nobility" },
        new NodeData { Key = 56, Parent = 51, Name = "Takeda Tsunekazu", Gender = "M", FullTitle = "No Title", KanjiName = "竹田恒和王", BirthYear = "1944", StatusChange = "In 1947, lost imperial family status due to American abrogation of Japanese nobility" },
        new NodeData { Key = 57, Parent = 51, Name = "Takeda Noriko", Gender = "F", FullTitle = "Princess Takeda Noriko", KanjiName = "紀子女王", BirthYear = "1943", StatusChange = "In 1947, lost imperial family status due to American abrogation of Japanese nobility" },
        new NodeData { Key = 58, Parent = 51, Name = "Tsuneharu Takeda", Gender = "M", FullTitle = "Prince Tsuneharu Takeda", KanjiName = "竹田恒治王", BirthYear = "1945", StatusChange = "In 1947, lost imperial family status due to American abrogation of Japanese nobility" },
        new NodeData { Key = 59, Parent = 50, Name = "Takeda Ayako", Gender = "F", FullTitle = "Princess Tsune-no-Miya Takeda Ayako", KanjiName = "禮子女王", BirthYear = "1911", StatusChange = "In 1947, lost imperial family status due to American abrogation of Japanese nobility" },
        new NodeData { Key = 60, Parent = 1, Name = "Fusako", Gender = "F", FullTitle = "Princess Fusako of Kane", BirthYear = "1890", DeathYear = "1974" },
        new NodeData { Key = 61, Parent = 60, Name = "Kitashirakawa Nagahisa", Gender = "M", FullTitle = "Prince Kitashirakawa Nagahisa", KanjiName = "北白川宮永久王", BirthYear = "1910", DeathYear = "1940" },
        new NodeData { Key = 62, Parent = 61, Name = "Kitashirakawa Michihisa", Gender = "M", FullTitle = "Prince Kitashirakawa Michihisa", BirthYear = "1937", StatusChange = "In 1947, lost imperial family status due to American abrogation of Japanese nobility" },
        new NodeData { Key = 63, Parent = 62, Name = "Kitashirakawa Naoko", Gender = "F", FullTitle = "No Title", BirthYear = "1969" },
        new NodeData { Key = 64, Parent = 62, Name = "Kitashirakawa Nobuko", Gender = "F", FullTitle = "No Title", BirthYear = "1971" },
        new NodeData { Key = 65, Parent = 62, Name = "Kitashirakawa Akiko", Gender = "F", FullTitle = "No Title", BirthYear = "1973" },
        new NodeData { Key = 66, Parent = 61, Name = "Hatsuko", Gender = "F", FullTitle = "Princess Hatsuko", BirthYear = "1939", StatusChange = "In 1947, lost imperial family status due to American abrogation of Japanese nobility" },
        new NodeData { Key = 67, Parent = 60, Name = "Kitashirakawa Mineko", Gender = "F", FullTitle = "Princess Kitashirakawa Mineko", KanjiName = "美年子女王", BirthYear = "1910", DeathYear = "1970", StatusChange = "In 1947, lost imperial family status due to American abrogation of Japanese nobility" },
        new NodeData { Key = 68, Parent = 60, Name = "Kitashirakawa Sawako", Gender = "F", FullTitle = "Princess Kitashirakawa Sawako", KanjiName = "佐和子女王", BirthYear = "1913", DeathYear = "2001", StatusChange = "In 1947, lost imperial family status due to American abrogation of Japanese nobility" },
        new NodeData { Key = 69, Parent = 60, Name = "Kitashirakawa Taeko", Gender = "F", FullTitle = "Princess Kitashirakawa Taeko", KanjiName = "多惠子女王", BirthYear = "1920", DeathYear = "1954", StatusChange = "In 1947, lost imperial family status due to American abrogation of Japanese nobility" },
        new NodeData { Key = 70, Parent = 1, Name = "Nobuko", Gender = "F", FullTitle = "Princess Fumi-no-Miya Nobuko", BirthYear = "1891", DeathYear = "1933" },
        new NodeData { Key = 71, Parent = 70, Name = "Asaka Kikuko", Gender = "F", FullTitle = "Princess Asaka Kikuko", KanjiName = "紀久子", BirthYear = "1911", DeathYear = "1989", StatusChange = "In 1947, lost imperial family status due to American abrogation of Japanese nobility" },
        new NodeData { Key = 72, Parent = 70, Name = "Asaka Takahiko", Gender = "M", FullTitle = "Prince Asaka Takahiko", KanjiName = "朝香 孚彦", BirthYear = "1913", DeathYear = "1994", StatusChange = "In 1947, lost imperial family status due to American abrogation of Japanese nobility" },
        new NodeData { Key = 73, Parent = 72, Name = "Fukuko", Gender = "F", FullTitle = "No Title" },
        new NodeData { Key = 74, Parent = 72, Name = "Minoko", Gender = "F", FullTitle = "No Title" },
        new NodeData { Key = 75, Parent = 72, Name = "Tomohiko", Gender = "M", FullTitle = "No Title" },
        new NodeData { Key = 76, Parent = 70, Name = "Asaka Tadahito", Gender = "M", FullTitle = "Prince Asaka Tadahito", KanjiName = "朝香正彦", BirthYear = "1914", DeathYear = "1944" },
        new NodeData { Key = 77, Parent = 70, Name = "Asaka Kiyoko", Gender = "F", FullTitle = "Princess Asaka Kiyoko", KanjiName = "湛子", BirthYear = "1919", StatusChange = "In 1947, lost imperial family status due to American abrogation of Japanese nobility" },
        new NodeData { Key = 78, Parent = 1, Name = "Ten Other Children Not Surviving Infancy" },
        new NodeData { Key = 79, Parent = -1, Name = "Five Other Children Not Surviving Infancy" }
        }
      };
    }

  }

  // define the model data
  public class Model : TreeModel<NodeData, int, object> { }
  public class NodeData : Model.NodeData {
    public string Name { get; set; }
    public string Gender { get; set; }
    public string FullTitle { get; set; }
    public string KanjiName { get; set; }
    public string BirthYear { get; set; }
    public string DeathYear { get; set; }
    public string StatusChange { get; set; }
    public string OrderInSuccession { get; set; }
    public string PosthumousName { get; set; }
    public string Spouse { get; set; }
    public string SpouseKanji { get; set; }
    public string Reign { get; set; }
  }

}
