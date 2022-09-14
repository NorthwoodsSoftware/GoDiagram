/* Copyright 1998-2022 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Northwoods.Go;
using Northwoods.Go.Models;

namespace Demo.Samples.GameOfLife {
  [ToolboxItem(false)]
  public partial class GameOfLifeControl : DemoControl {
    private Diagram myDiagram;

    private string samplePatterns = "Symmetry";

    // instance vars for go
    private Part[,] goLGrid;
    private bool initializing = true;
    private bool enabled = false; // flag to turn the simulatin on or off

    // consts
    private const int rows = 40;
    private const int cols = 40;
    private const int interval = 15; // interval between steps in ms when the sim is enabled
    private const int nodeSize = 25;
    // stroke color and width of the grid lines
    private const string gridStroke = "#A2A2A2";
    private const int gridStrokeWidth = 1;

    public GameOfLifeControl() {
      InitializeComponent();

      Setup();

      btnStart.Click += (e, obj) => ToggleSimulation();
      btnStep.Click += (e, obj) => StepOnclick();
      btnClear.Click += (e, obj) => GoLClear();

      patterns.SelectedIndex = 0;
      patterns.SelectedIndexChanged += (s, e) => {
        LoadSample((string)patterns.SelectedItem);
      };

      goWebBrowser1.Html = @"

  <p>
      This sample shows an implementation of <a href=""https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life"">Conway's Game of Life</a> in GoDiagram.
      Conway's Game of Life is a simple cellular automaton devised by British mathematician John Horton Conway in 1970. To start or advance the simulation,
      use the controls above.
    </p>
    <p>
      Whether or not a given cell lives, dies, or is born in a step is determined by the number of live cells in the 8 squares adjacent to it. For a cell <i>x</i> with <i>n</i> adjacent live cells:
    </p>
    <ul>
      <li>If <i>n</i> <= 1, cell <i>x</i> dies or stays dead (from underpopulation).</li>
      <li>If <i>n</i> > 3, cell <i>x</i> dies or stays dead (from overpopulation).</li>
      <li>If <i>n</i> = 3, then <i>x</i> is born or stays alive.</li>
      <li>If <i>n</i> = 2, then <i>x</i> maintains its status.</li>
    </ul>
    <p>
      Though the rules are simple, they can produce complex patterns, some of which are shown in the dropdown above.
      To create your own patterns, click or drag anywhere on the grid when the simulation is not running.
    </p>
    <p>
      Each cell is implemented by a simple <a>Part</a> holding a small square <a>Shape</a>.

     </p>

";

    }

    // accessors for razor bindings
    public string SamplePatterns {
      get {
        return samplePatterns;
      }
      set {
        samplePatterns = value;
        LoadSample(samplePatterns);
      }
    }

    private void Setup() {
      // initialize grid
      goLGrid = new Part[rows, cols];

      myDiagram = diagramControl1.Diagram;

      // diagram properties
      myDiagram.AnimationManager.IsEnabled = false;
      myDiagram.ToolManager.PanningTool.IsEnabled = false;
      myDiagram.IsReadOnly = true;
      myDiagram.AllowZoom = false;
      myDiagram.AllowSelect = false;
      myDiagram.HasHorizontalScrollbar = false;
      myDiagram.HasVerticalScrollbar = false;
      myDiagram.InitialAutoScale = AutoScale.Uniform;

      var nodeDataSource = new List<NodeData>();
      // populate data array by initializing Nodes in a grid and setting their "isAlive" state to false
      for (var i = 0; i < rows; i++) {
        for (var j = 0; j < cols; j++) {
          var data = new NodeData {
            Location = new Point(j * nodeSize, i * nodeSize),
            Row = i,
            Col = j,
            IsAlive = false
          };
          nodeDataSource.Add(data);
        }
      }

      // define the node template, which also includes interactive functionality, such as clicking to toggle a node's state
      myDiagram.NodeTemplate =
        new Part {
          IsLayoutPositioned = false,
          MouseEnter = (e, partAsObj, prev) => {  // set mouseover border
            if (!initializing && !enabled) {
              var part = partAsObj as Part;
              var shape = part.Elt(0) as Shape;
              if (shape != null) {
                shape.Stroke = "steelblue";
                shape.StrokeWidth = 3;
              }
              part.ZOrder = 2; // ensure that the selection border is in front of all other Nodes by increasing its zOrder

              // drag with a button down to add or erase cells
              if ((e.Buttons == 1 && !(part.Data as NodeData).IsAlive) || (e.Buttons == 2 && (part.Data as NodeData).IsAlive)) {
                Select(part);
              }
            }
          },
          MouseLeave = (e, partAsObj, next) => {  // restore to original borders
            var part = partAsObj as Part;
            var shape = part.Elt(0) as Shape;
            if (shape != null) {
              shape.Stroke = gridStroke;
              shape.StrokeWidth = gridStrokeWidth;
            }
            part.ZOrder = 1;
          },
          Click = (e, partAsObj) => { // left click to toggle cell
            var part = partAsObj as Part;
            initializing = false;
            if (!enabled) {
              Select(part);
            }
          },
          ContextClick = (e, partAsObj) => { // right click to clear cell
            var part = partAsObj as Part;
            initializing = false;
            if (!enabled && (part.Data as NodeData).IsAlive) {
              Select(part);
            }
          },
          ZOrder = 1
        }.Add(
          new Shape {
            Figure = "Rectangle",
            Fill = "white",
            Stroke = gridStroke,
            StrokeWidth = gridStrokeWidth,
            Width = nodeSize,
            Height = nodeSize
          }
        ).Bind(
          new Binding("Location")
        );

      // simple model
      myDiagram.Model = new Model {
        NodeDataSource = nodeDataSource
      };

      // myDiagram.Parts is populated after a model is assigned;
      // populate the internal gamestate array with the newly created nodes
      var it = myDiagram.Parts.GetEnumerator();
      while (it.MoveNext()) {
        var part = it.Current;
        var data = part.Data as NodeData;
        goLGrid[data.Row, data.Col] = part;
      }

      // update razor then load sample
      LoadSample(SamplePatterns);
    }

    private void Select(Part part) {
      var shape = part.Elt(0) as Shape;
      if (shape != null) {
        if (shape.Fill == "white") {
          shape.Fill = "steelblue";
        } else {
          shape.Fill = "white";
        }
      };
      (part.Data as NodeData).IsAlive = !(part.Data as NodeData).IsAlive;
    }

    // toggles the state of the simulation, changing the button text from "Start" to "Pause" or back again
    private void ToggleSimulation() {
      initializing = false;
      if (!enabled) {
        btnStart.Text = "Pause";
        enabled = true;
        GoLStep();
      } else {
        btnStart.Text = "Start";
        enabled = false;
      }
    }

    // the callback for the step button, only steps forward if the simulation is currently stopped
    private void StepOnclick() {
      initializing = false;
      if (!enabled) {
        GoLStep(true);
      }
    }

    // performs a single step in the Game of Life
    private void GoLStep(bool isManualStep = false) {
      if (goLGrid == null || goLGrid.Length == 0) {
        return; // don't do anything if things aren't initialized yet
      }

      var isAlive = false;
      var toSelect = new List<Part>();
      var liveCellCount = 0; // count the number of live cells to determine if there are no more left
      for (var i = 0; i < rows; i++) {
        for (var j = 0; j < cols; j++) {
          if (isAlive) {
            liveCellCount++;
          }

          // count the number of cells in the 8 squares adjacent to this one
          var total = 0;

          var above = i > 0 ? i - 1 : rows - 1;
          var below = i + 1 < rows ? i + 1 : 0;
          var left = j > 0 ? j - 1 : cols - 1;
          var right = j + 1 < cols ? j + 1 : 0;

          total += (goLGrid[above, left].Data as NodeData).IsAlive ? 1 : 0;
          total += (goLGrid[above, j].Data as NodeData).IsAlive ? 1 : 0;
          total += (goLGrid[above, right].Data as NodeData).IsAlive ? 1 : 0;
          total += (goLGrid[i, left].Data as NodeData).IsAlive ? 1 : 0;
          total += (goLGrid[i, right].Data as NodeData).IsAlive ? 1 : 0;
          total += (goLGrid[below, left].Data as NodeData).IsAlive ? 1 : 0;
          total += (goLGrid[below, j].Data as NodeData).IsAlive ? 1 : 0;
          total += (goLGrid[below, right].Data as NodeData).IsAlive ? 1 : 0;

          // toggle the cell if necessary according to the three rules
          var part = goLGrid[i, j];
          isAlive = (part.Data as NodeData).IsAlive;
          if ((total <= 1 && isAlive)
              || (total > 3 && isAlive)
              || (total == 3 && !isAlive)) {
            if (!isAlive) {
              liveCellCount++;
            } else {
              liveCellCount--;
            }
            toSelect.Add(part); // don't actually toggle the cell yet, this happens all at once after everything is done
          }
        }
      }

      // change the board state according the earlier loop
      if (enabled || isManualStep) {
        for (var i = 0; i < toSelect.Count; i++) {
          Select(toSelect[i]);
        }
      }

      if (enabled) {
        if (liveCellCount == 0) { // stop the simulation if there are no more live cells
          ToggleSimulation();
        } else { // queue another step if the simuation is still enabled
          Task.Delay(interval).ContinueWith((t) => {
            GoLStep();
          });
        }
      }
    }

    // clear the board of all live cells, stopping the simulation if it's currently enabled
    void GoLClear() {
      if (enabled) {
        ToggleSimulation();
      }
      for (var i = 0; i < rows; i++) {
        for (var j = 0; j < cols; j++) {
          var part = goLGrid[i, j];
          if ((part.Data as NodeData).IsAlive) {
            Select(part);
          }
        }
      }
    }

    // this function contains all of the data for the four included sample patterns as well as the logic for drawing them
    void LoadSample(string value) {
      GoLClear(); // clear the board first, stopping the simulation if it's enabled
      // select the correct sample data based on the option value passed to the function
      int[,] sampleData;
      switch (value) {
        case "Symmetry":
        default:
          sampleData = new int[,] {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
          };
          break;
        case "Pulsar":
          sampleData = new int[,] {
            { 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0},
            { 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1},
            { 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1},
            { 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1},
            { 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0},
            { 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1},
            { 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1},
            { 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1},
            { 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0}
          };
          break;
        case "Spaceships":
          sampleData = new int[,] {
            { 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
            { 0, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0},
            { 1, 1, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0},
            { 1, 1, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0}
          };
          break;
        case "Big gliders":
          sampleData = new int[,] {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 1, 0, 1, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 1, 1, 1, 1},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 0, 1, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
          };
          break;
      }

      // draw the sample pattern in the middle of the board
      var startRow = Convert.ToInt32(Math.Floor((rows / 2.0) - (sampleData.GetLength(0) / 2.0)));
      var startCol = Convert.ToInt32(Math.Floor((cols / 2.0) - (sampleData.GetLength(1) / 2.0)));
      for (var i = startRow; i < startRow + sampleData.GetLength(0); i++) {
        for (var j = startCol; j < startCol + sampleData.GetLength(1); j++) {
          if (sampleData[i - startRow, j - startCol] == 1) {
            Select(goLGrid[i, j]);
          }
        }
      }
    }

  }

  // define the model data
  public class Model : Model<NodeData, string, object> { }
  public class NodeData : Model.NodeData {
    public int Row { get; set; }
    public int Col { get; set; }
    public bool IsAlive { get; set; }
  }

}
