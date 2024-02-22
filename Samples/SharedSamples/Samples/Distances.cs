/* Copyright 1998-2024 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;
using Northwoods.Go;
using Northwoods.Go.Models;
using Northwoods.Go.Layouts;
using Northwoods.Go.Tools;
using System.Linq;
using System.Collections.ObjectModel;

namespace Demo.Samples.Distances {
  public partial class Distances : DemoControl {
    private Diagram _Diagram;
    public ObservableCollection<string> myPaths = new ObservableCollection<string>();
    //private int? selectedPath;
    private List<List<Node>> paths;

    public Distances() {
      InitializeComponent();
      _Diagram = diagramControl1.Diagram;

      Setup();

      randomNodesBtn.Click += (e, obj) => ChooseTwoNodes();

      desc1.MdText = DescriptionReader.Read("Samples.Distances.md");
    }

    private void Setup() {
      _Diagram.InitialAutoScale = AutoScale.Uniform;
      _Diagram.ContentAlignment = Spot.Center;
      _Diagram.Layout = new ForceDirectedLayout {
        DefaultSpringLength = 10, MaxIterations = 300
      };
      _Diagram.MaxSelectionCount = 2;
      _Diagram.ToolManager.ClickSelectingTool = new DistancesClickSelectingTool();

      // define the Node template
      _Diagram.NodeTemplate =
        new Node(PanelType.Horizontal) {
            LocationSpot = Spot.Center,  // Node.Location is the center of the Shape
            LocationElementName = "SHAPE",
            SelectionAdorned = false,
            SelectionChanged = NodeSelectionChanged
          }
          .Add(
            new Panel(PanelType.Spot)
              .Add(
                new Shape("Circle") {
                    Name = "SHAPE",
                    Fill = "lightgray",  // default value, but also data-bound
                    StrokeWidth = 0,
                    DesiredSize = new Size(30, 30),
                    PortId = ""  // so links will go to the shape, not the whole node
                  }
                  .Bind(
                    new Binding("Fill", "IsSelected", (s, obj) => {
                      return (bool)s ? "red" : ((obj as GraphObject).Part.Data as NodeData).Color;
                    }).OfElement()
                  ),
                new TextBlock()
                  .Bind("Text", "Distance", (d) => {
                    if ((int)d == -1) return "INF";
                    return d.ToString();
                  })
              ),
            new TextBlock().Bind("Text")
          );

      // define the Link template
      _Diagram.LinkTemplate =
        new Link {
            Selectable = false,      // links cannot be selected by the user
            Curve = LinkCurve.Bezier,
            LayerName = "Background"  // don't cross in front of any nodes
          }
          .Add(
            new Shape { // this shape only shows when it IsHighlighted
                IsPanelMain = true, Stroke = null, StrokeWidth = 5
              }
              .Bind(
                new Binding("Stroke", "IsHighlighted", (h) => {
                  return (bool)h ? "red" : null;
                }).OfElement()
              ),
            new Shape {
                // mark each Shape to get the link geometry with IsPanelMain = true
                IsPanelMain = true, Stroke = "black", StrokeWidth = 1
              }
              .Bind("Stroke", "Color"),
            new Shape { ToArrow = "Standard" }
          );

      // generate model data
      GenerateGraph();

      ChooseTwoNodes();
    }

    private void GenerateGraph() {
      var names = new string[] {
        "Joshua", "Kathryn", "Robert", "Jason", "Scott", "Betsy", "John",
        "Walter", "Gabriel", "Simon", "Emily", "Tina", "Elena", "Samuel",
        "Jacob", "Michael", "Juliana", "Natalie", "Grace", "Ashley", "Dylan"
      };

      var num = names.Length;
      var nodeDataSource = new NodeData[num];
      for (var i = 0; i < num; i++) {
        nodeDataSource[i] = new NodeData { Key = i + 1, Text = names[i], Color = Brush.RandomColor(128, 240) };
      }

      var rand = new Random();
      var linkDataSource = new LinkData[num * 2];
      for (var i = 0; i < num * 2; i++) {
        var a = (int)Math.Floor(i / 2f);
        var b = rand.Next(num / 4) + 1;
        linkDataSource[i] = new LinkData { Key = -1 - i, From = a + 1, To = (a + b) % num + 1, Color = Brush.RandomColor(0, 127) };
      }

      _Diagram.Model = new Model {
        NodeDataSource = nodeDataSource,
        LinkDataSource = linkDataSource
      };
    }

    private void ChooseTwoNodes() {
      _Diagram.ClearSelection();
      var rand = new Random();
      var num = _Diagram.Model.NodeDataSource.Count();
      for (var i = rand.Next(num); i < num * 2; i++) {
        var node1 = _Diagram.FindNodeForKey(i % num + 1);
        var distances = FindDistances(node1);
        for (var j = rand.Next(num); j < num * 2; j++) {
          var node2 = _Diagram.FindNodeForKey(j % num + 1);
          var dist = distances[node2];
          if (dist > 1 && dist < int.MaxValue) {
            node1.IsSelected = true;
            node2.IsSelected = true;
            break;
          }
        }
        if (_Diagram.Selection.Count() > 0) break;
      }
    }

    // There are three bits of functionality here:
    // 1 = FindDistances(Node) computes the distance of each Node from the given Node.
    //    This function is used by ShowDistances to update the model data.
    // 2 = FindShortestPath(Node, Node) finds a shortest path from one Node to another.
    //    This uses FindDistances. This is used by HighlightShortestPath.
    // 3 = CollectAllPaths(Node, Node) produces a collection of all paths from one Node to another.
    //    This is used by ListAllPaths. The result is remembered in a property
    //    which is used by HighlightSelectedPath. This does not depend on FindDistances.

    // Returns a Map of Nodes with distance values from the given source Node.
    // Assumes all links are unidirectional.
    private Dictionary<Node, int> FindDistances(Node source) {
      var diagram = source.Diagram;
      // keep track of distances from the source node
      var distances = new Dictionary<Node, int>();
      // all nodes start with distance Infinity
      var nit = diagram.Nodes.GetEnumerator();
      while (nit.MoveNext()) {
        var n = nit.Current;
        distances.Add(n, -1);
      }
      // the source node starts with distance 0
      distances[source] = 0;
      // keep track of nodes for which we have set a non-Infinity distance,
      // but which we have not yet finished examining
      var seen = new List<Node> {
        source
      };

      // keep track of nodes we have finished examining;
      // this avoids unnecessary traversals and helps keep the SEEN collection small
      var finished = new List<Node>();
      while (seen.Count > 0) {
        // look at the unfinished node with the shortest distance so far
        var least = LeastNode(seen, distances);
        int leastdist;

        if (least == null) {
          leastdist = int.MaxValue;
          return distances;
        }

        leastdist = distances[least];
        // by the end of this loop we will have finished examining this LEAST node
        seen.Remove(least);
        finished.Add(least);
        // look at all Links connected with this node
        var it = least.FindLinksOutOf().GetEnumerator();
        while (it.MoveNext()) {
          var link = it.Current;
          var neighbor = link.GetOtherNode(least);
          // skip nodes that we have finished
          if (finished.Contains(neighbor)) continue;
          var neighbordist = distances[neighbor];
          // assume "distance" along a link is unitary, but could be any non-negative number.
          var dist = leastdist + 1;  //Math.Sqrt(least.Location.DistanceSquared(neighbor.Location));
          if (dist < neighbordist || (neighbordist == -1 && dist != -1)) {
            // if haven't seen that node before, add it to the SEEN collection
            if (neighbordist == -1) {
              seen.Add(neighbor);
            }
            // record the new best distance so far to that node
            distances[neighbor] = dist;
          }
        }
      }
      return distances;
    }

    // This helper function finds a Node in the given collection that has the smallest distance.
    private Node LeastNode(List<Node> coll, Dictionary<Node, int> distances) {
      var bestdist = int.MaxValue;
      Node bestnode = null;
      var it = coll.GetEnumerator();
      while (it.MoveNext()) {
        var n = it.Current;
        var dist = distances[n];
        if ((dist < bestdist) && (bestdist != -1 && dist != -1)) {
          bestdist = dist;
          bestnode = n;
        }
      }
      return bestnode;
    }

    // Find a path that is shortest from the BEGIN node to the END node.
    // (There might be more than one, and there might be none.)
    private List<Node> _FindShortestPath(Node begin, Node end) {
      // compute and remember the distance of each node from the BEGIN node
      var distances = FindDistances(begin);

      // now find a path from END to BEGIN, always choosing the adjacent Node with the lowest distance
      var path = new List<Node>();
      path.Add(end);
      while (end != null) {
        var next = LeastNode(end.FindNodesInto().ToList(), distances);
        if (next != null) {
          var dnext = distances[next];
          var dend = distances[end];
          if ((dnext < dend) || (dend == -1 && dnext != -1)) {
            path.Add(next);  // making progress towards the beginning
          } else {
            next = null;  // nothing better found -- stop looking
          }
        }
        end = next;
      }
      // reverse the list to start at the node closest to BEGIN that is on the path to END
      // NOTE = if there's no path from BEGIN to END, the first node won't be BEGIN!
      path.Reverse();
      return path;
    }

    // Recursively walk the graph starting from the BEGIN node;
    // when reaching the END node remember the list of nodes along the current path.
    // Finally return the collection of paths, which may be empty.
    // This assumes all links are unidirectional.
    private List<List<Node>> CollectAllPaths(Node begin, Node end) {
      var stack = new List<Node>();
      var coll = new List<List<Node>>();

      void find(Node source, Node end) {
        foreach (var n in source.FindNodesOutOf()) {
          if (n == source) return;  // ignore reflexive links
          if (n == end) {  // success
            var path = stack.ToList();
            path.Add(end);  // finish the path at the end node
            coll.Add(path);  // remember the whole path
          } else if (!stack.Contains(n)) {  // inefficient way to check having visited
            stack.Add(n);  // remember that we've been here for this path (but not forever)
            find(n, end);
            stack.RemoveAt(stack.Count - 1);
          }  // else might be a cycle
        }
      }

      stack.Add(begin);  // start the path at the begin node
      find(begin, end);
      return coll;
    }

    // Return a string representation of a path for humans to read.
    private string PathToString(List<Node> path) {
      var s = path.Count + ": ";
      for (var i = 0; i < path.Count; i++) {
        if (i > 0) s += " -- ";
        s += (path.ElementAt(i).Data as NodeData).Text;
      }
      return s;
    }

    // When a node is selected show distances from the first selected node.
    // When a second node is selected, highlight the shortest path between two selected nodes.
    // If a node is deselected, clear all highlights.
    private void NodeSelectionChanged(Part nodeAsPart) {
      var node = nodeAsPart as Node;
      var diagram = node.Diagram;
      if (diagram == null) return;
      diagram.ClearHighlighteds();
      if (node.IsSelected) {
        // when there is a selection made, always clear out the list of all paths
        myPaths.Clear();
        // show the distance for each node from the selected node
        var begin = diagram.Selection.First() as Node;
        ShowDistances(begin);

        if (diagram.Selection.Count == 2) {
          var end = node;  // just became selected

          // highlight the shortest path
          HighlightShortestPath(begin, end);

          // list all paths
          ListAllPaths(begin, end);
        }
      }
      myPaths.Sort(new SmartStringComparer());
    }

    // Have each node show how far it is from the BEGIN node.
    private void ShowDistances(Node begin) {
      // compute and remember the distance of each node from the BEGIN node
      var distances = FindDistances(begin);

      // show the distance on each node
      var it = distances.GetEnumerator();
      while (it.MoveNext()) {
        var cur = it.Current;
        var n = cur.Key;
        var dist = cur.Value;
        _Diagram.Model.Set(n.Data, "Distance", dist);
      }
    }

    // Highlight links along one of the shortest paths between the BEGIN and the END nodes.
    // Assume links are unidirectional.
    private void HighlightShortestPath(Node begin, Node end) {
      HighlightPath(_FindShortestPath(begin, end));
    }

    // List all paths from BEGIN to END
    private void ListAllPaths(Node begin, Node end) { // void --> List<string>
      // compute and remember all paths from BEGIN to END = Lists of Nodes
      paths = CollectAllPaths(begin, end);

      // update the Selection element with a bunch of Option elements, one per path
      myPaths.Clear();
      foreach (var p in paths) {
        myPaths.Add(PathToString(p));
      }
      myPaths.Sort(new SmartStringComparer());
      _RebuildList();
    }

    // When the selected item changes in the Selection element,
    // highlight the corresponding path of nodes.
    //private void HighlightSelectedPath() {
    //  if (selectedPath != null) {
    //    HighlightPath(paths.ElementAt((int)selectedPath));
    //  }
    //}

    // Highlight a particular path, a List of Nodes.
    private void HighlightPath(List<Node> path) {
      _Diagram.ClearHighlighteds();
      for (var i = 0; i < path.Count - 1; i++) {
        var f = path.ElementAt(i);
        var t = path.ElementAt(i + 1);
        foreach (var l in f.FindLinksTo(t)) {
          l.IsHighlighted = true;
        }
      }
    }
  }

  public class Model : GraphLinksModel<NodeData, int, object, LinkData, int, string> { }
  public class NodeData : Model.NodeData {
    public string Color { get; set; }
    public int? Distance { get; set; }
  }

  public class LinkData : Model.LinkData {
    public string Color { get; set; }
  }

  // Override the clickSelectingTool's standardMouseSelect
  // If less than 2 nodes are selected, always add to the selection
  public class DistancesClickSelectingTool : ClickSelectingTool {
    public override void StandardMouseSelect() {
      var diagram = Diagram;
      if (diagram == null || !diagram.AllowSelect) return;
      var e = diagram.LastInput;
      var count = diagram.Selection.Count;
      var curobj = diagram.FindPartAt(e.DocumentPoint, false);
      if (curobj != null) {
        if (count < 2) {  // add the part to the selection
          if (!curobj.IsSelected) {
            var part = curobj;
            if (part != null) part.IsSelected = true;
          }
        } else {
          if (!curobj.IsSelected) {
            var part = curobj;
            if (part != null) diagram.Select(part);
          }
        }
      } else if (e.Left && !(e.Control || e.Meta) && !e.Shift) {
        // left click on background with no Modifier = clear selection
        diagram.ClearSelection();
      }
    }
  }
}
