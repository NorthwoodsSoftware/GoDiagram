using System;
using System.Collections.Generic;
using System.ComponentModel;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;

namespace WinFormsSampleControls.Tournament {
  [ToolboxItem(false)]
  public partial class TournamentControl : System.Windows.Forms.UserControl {
    private Diagram MyDiagram;

    public TournamentControl() {
      InitializeComponent();

      //When page loads
      Setup();
      goWebBrowser1.Html = @"
        <p>
        Click on the empty score boxes next to names to fill in scores for each player.
        The scores must be non-negative numbers with at most 3 digits. Scores are validated using a <a>TextEditingTool.TextValidation</a> function.
        When two players in a ""game"" have a score, one of them will automatically advance to the next round of the bracket.
        </p>
      ";
    }

    private void Setup() {

      MyDiagram = diagramControl1.Diagram;

      MyDiagram.UndoManager.IsEnabled = true;
      MyDiagram.Layout = new TreeLayout { Angle = 180 };
      MyDiagram.ToolManager.TextEditingTool.Starting = TextEditingStarting.SingleClick;
      MyDiagram.ToolManager.TextEditingTool.TextValidation = (textblock, oldstr, newstr) => {
        if (newstr == "") return true;
        if (int.TryParse(newstr, out var n)) {
          return n >= 0 && n < 1000;
        }
        return false;
      };

      MyDiagram.NodeTemplate = new Node(PanelLayoutAuto.Instance) { Selectable = false }
      .Add(
        new Shape("Rectangle") { Fill = "#8C8C8C", Stroke = null }
          .Bind("Fill", "Color"),
        new Panel(PanelLayoutTable.Instance)
        .Add(
          new ColumnDefinition { Column = 0, SeparatorStroke = "black" },
          new ColumnDefinition { Column = 1, SeparatorStroke = "black", Background = "#BABABA" })
        .Add(
          new RowDefinition { Row = 0, SeparatorStroke = "black" },
          new RowDefinition { Row = 1, SeparatorStroke = "black" })
        .Add(
          new TextBlock {
            Row = 0,
            Wrap = Wrap.None, Margin = 5, Width = 90,
            IsMultiline = false, TextAlign = TextAlign.Left,
            Font = new Font("Segoe UI", 10, FontWeight.Bold), Stroke = "white"
          }.Bind(new Binding("Text", "Player1").MakeTwoWay()),
          new TextBlock {
            Row = 1,
            Wrap = Wrap.None, Margin = 5, Width = 90,
            IsMultiline = false, TextAlign = TextAlign.Left,
            Font = new Font("Segoe UI", 10, FontWeight.Bold), Stroke = "white"
          }.Bind(new Binding("Text", "Player2").MakeTwoWay()),
          new TextBlock {
            Column = 1, Row = 0,
            Wrap = Wrap.None, Margin = 2, Width = 25,
            IsMultiline = false, Editable = true, TextAlign = TextAlign.Center,
            Font = new Font("Segoe UI", 10, FontWeight.Bold), Stroke = "black"
          }.Bind(new Binding("Text", "Score1").MakeTwoWay()),
          new TextBlock {
            Column = 1, Row = 1,
            Wrap = Wrap.None, Margin = 2, Width = 25,
            IsMultiline = false, Editable = true, TextAlign = TextAlign.Center,
            Font = new Font("Segoe UI", 10, FontWeight.Bold), Stroke = "black"
          }.Bind(new Binding("Text", "Score2").MakeTwoWay())
      ));

      MyDiagram.LinkTemplate = new Link {
        Routing = LinkRouting.Orthogonal,
        Selectable = false
      }.Add(
        new Shape {
          StrokeWidth = 2,
          Stroke = "darkgray"
        }
      );

      MyDiagram.Model = _MakeModel(new List<string> {
        "Adler",
        "Pollock",
        "Montgomery",
        "Lestrade",
        "Wilson",
        "Moran",
        "Bardle",
        "Edwards"
      });
    }

    private List<NodeData> _CreatePairs(List<string> players) {
      if (players.Count % 2 != 0) players.Add("(empty)");
      var startingGroups = players.Count / 2;
      var currentLevel = Math.Ceiling(Math.Log(startingGroups) / Math.Log(2));
      var levelGroups = new List<string>();
      for (var i = 0; i < startingGroups; i++) {
        levelGroups.Add(currentLevel + "-" + i);
      }
      var totalGroups = new List<NodeData>();
      _MakeLevel(levelGroups, (int)currentLevel, totalGroups, players);
      return totalGroups;
    }

    private void _MakeLevel(List<string> levelGroups, int currentLevel, List<NodeData> totalGroups, List<string> players) {
      currentLevel--;
      var len = levelGroups.Count;
      var parentKeys = new List<string>();
      var parentNumber = 0;
      var p = "";
      for (var i = 0; i < len; i++) {
        if (parentNumber == 0) {
          p = currentLevel + "-" + parentKeys.Count;
          parentKeys.Add(p);
        }

        if (players != null) {
          var p1 = players[i * 2];
          var p2 = players[i * 2 + 1];
          totalGroups.Add(
            new NodeData { Key = levelGroups[i], Parent = p, Player1 = p1, Player2 = p2, ParentNumber = parentNumber }
          );
        } else {
          totalGroups.Add(
            new NodeData { Key = levelGroups[i], Parent = p, ParentNumber = parentNumber }
          );
        }

        parentNumber++;
        if (parentNumber > 1) parentNumber = 0;
      }

      // after the first created level there are no player names
      if (currentLevel >= 0) _MakeLevel(parentKeys, currentLevel, totalGroups, null);

    }

    private void _UpdateModel(object sender, NodeData data, Model model) {
      if (data.Score1 == null || data.Score2 == null) return;

      // TODO: What happens if score1 and score2 are the same number?

      // both score1 and score2 are numbers,
      // set the name of the higher-score'd player in the advancing (parent) node
      // if the data.ParentNumber is 0, then we set player1 on the parent
      // if the data.ParentNumber is 1, then we set player2
      var parent = model.FindNodeDataForKey(data.Parent);
      if (parent == null) return;

      var playerName = double.Parse(data.Score1) > double.Parse(data.Score2) ? data.Player1 : data.Player2;
      if (Math.Abs(double.Parse(data.Score1) - double.Parse(data.Score2)) < 1e-5) playerName = "";

      model.Set(parent, data.ParentNumber == 0 ? "Player1" : "Player2", playerName);
    }

    private Model _MakeModel(List<string> players) {
      var rand = new Random();

      var model = new Model {
        NodeDataSource = _CreatePairs(players)
      };

      model.Changed += (sender, e) => {
        if (e.PropertyName == "Score1" || e.PropertyName == "Score2") {
          _UpdateModel(sender, e.Object as NodeData, model);
        }
      };

      var arr = model.NodeDataSource as List<NodeData>;
      for (var i = 0; i < Math.Min(3, arr.Count); i++) {
        var d = arr[i];
        if (d.Player1 != null && d.Player2 != null) {
          // TODO: doesnt prevent tie scores
          model.Set(d, "Score1", rand.Next(100).ToString());
          model.Set(d, "Score2", rand.Next(100).ToString());
        }
      }

      return model;
    }


  }

  public class Model : TreeModel<NodeData, string, object> { }

  public class NodeData : Model.NodeData {
    public string Player1 { get; set; }
    public string Player2 { get; set; }
    public int ParentNumber { get; set; }
    public string Score1 { get; set; }
    public string Score2 { get; set; }
  }

}
