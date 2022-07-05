/*
*  Copyright (C) 1998-2022 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main Go library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in the GoDiagram repository (https://github.com/NorthwoodsSoftware/GoDiagram/tree/main/Extensions).
* See the Extensions intro page (https://godiagram.com/intro/extensions.html) for more information.
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Northwoods.Go.Layouts.Extensions {
  /// <summary>
  /// A custom LayeredDigraphLayout that knows about "lanes"
  /// and that positions each node in its respective lane.
  /// </summary>
  /// <remarks>
  /// This assumes that each Node.Data.Lane property is a string that names the lane the node should be in.
  /// You can set the <see cref="LaneProperty"/> property to use a different data property name.
  /// It is commonplace to set this property to be the same as the
  /// <see cref="Models.GraphLinksModel{TNodeData, TNodeKey, TSharedData, TLinkData, TLinkKey, TPort}.NodeGroupKeyProperty"/>,
  /// so that the one property indicates that a particular node data is a member of a particular group
  /// and thus that that group represents a lane.
  ///
  /// The lanes can be sorted by specifying the <see cref="LaneComparer"/> function.
  /// 
  /// You can add extra space between the lanes by increasing <see cref="LaneSpacing"/> from its default of zero.
  /// That number's unit is columns, <see cref="LayeredDigraphLayout.ColumnSpacing"/>, not in document coordinates.
  /// </remarks>
  /// @category Layout Extension
  public class SwimLaneLayout : LayeredDigraphLayout {
    // settable properties
    private string _LaneProperty = "Lane";  // how to get lane identifier string from node data
    private List<string> _LaneNames;  // lane names, may be sorted using this.LaneComparer
    private StringComparer _LaneComparer = null;
    private int _LaneSpacing = 0;  // in columns
    private dynamic _Router = new { LinkSpacing = 4 };
    private dynamic _Reducer = null;
    // computed, read-only state
    private readonly Dictionary<string, int> _LanePositions = new();  // lane names --> start columns, left to right
    private readonly Dictionary<string, int> _LaneBreadths = new();  // lane names --> needed width in columns
    // internal state
    private readonly List<List<LayeredDigraphVertex>> _Layers = new();
    private List<double> _NeededSpaces = new();

    /// <summary>
    /// Constructs a SwimLaneLayout.
    /// </summary>
    public SwimLaneLayout() : base() { }

    /// <summary>
    /// Gets or sets the name of the data property that holds the string which is the name of the lane that the node should be in.
    /// </summary>
    /// <remarks>
    /// The default value is "Lane".
    /// </remarks>
    public string LaneProperty {
      get {
        return _LaneProperty;
      }
      set {
        if (_LaneProperty != value) {
          _LaneProperty = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Gets or sets a list of lane names.
    /// </summary>
    /// <remarks>
    /// If you set this before a layout happens, it will use those lanes in that order.
    /// Any additional lane names that it discovers will be added to the end of this list.
    /// 
    /// This property is reset to an empty list at the end of each layout.
    /// The default value is an empty list.
    /// </remarks>
    public List<string> LaneNames {
      get {
        return _LaneNames;
      }
      set {
        if (_LaneNames != value) {
          _LaneNames = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Gets or sets a comparer by which to compare lane names, for ordering the lanes within the <see cref="LaneNames"/> list.
    /// </summary>
    /// <remarks>
    /// By default the function is null -- the lanes are not sorted.
    /// </remarks>
    public StringComparer LaneComparer {
      get {
        return _LaneComparer;
      }
      set {
        if (_LaneComparer != value) {
          _LaneComparer = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Gets or sets the amount of additional space it allocates between the lanes.
    /// </summary>
    /// <remarks>
    /// This number specifies the number of columns, with the same meaning as <see cref="LayeredDigraphLayout.ColumnSpacing"/>.
    /// The number unit is not in document coordinate or pixels.
    /// The default value is zero columns.
    /// </remarks>
    public int LaneSpacing {
      get {
        return _LaneSpacing;
      }
      set {
        if (_LaneSpacing != value) {
          _LaneSpacing = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Undocumented.
    /// </summary>
    [Undocumented]
    public dynamic Router {
      get {
        return _Router;
      }
      set {
        if (_Router != value) {
          _Router = value;
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// Undocumented.
    /// </summary>
    [Undocumented]
    public dynamic Reducer {
      get {
        return _Reducer;
      }
      set {
        if (_Reducer != value) {
          _Reducer = value;
          if (value != null) {
            var lay = this;
            value.FindLane = new Func<LayeredDigraphVertex, string>(v => lay.GetLane(v));
            value.GetIndex = new Func<LayeredDigraphVertex, int>(v => v.Index);
            value.GetBary = new Func<LayeredDigraphVertex, float>(v => (float)v["_Bary"]);
            value.SetBary = new Action<LayeredDigraphVertex, float>((v, f) => v["_Bary"] = f);
            value.GetConnectedNodesIterator = new Func<LayeredDigraphVertex, IReadOnlyCollection<LayeredDigraphVertex>>(v => v.Vertexes);
          }
          InvalidateLayout();
        }
      }
    }

    /// <summary>
    /// The computed positions of each lane,
    /// in the form of a dictionary mapping lane names (strings) to numbers.
    /// </summary>
    protected Dictionary<string, int> LanePositions {
      get {
        return _LanePositions;
      }
    }

    /// <summary>
    /// The computed breadths (widths or heights depending on the direction) of each lane,
    /// in the form of a dictionary mapping lane names (strings) to numbers.
    /// </summary>
    protected Dictionary<string, int> LaneBreadths {
      get {
        return _LaneBreadths;
      }
    }

    /// <summary>
    /// Undocumented.
    /// </summary>
    /// <param name="coll"></param>
    [Undocumented]
    public override void DoLayout(IEnumerable<Part> coll = null) {
      _LanePositions.Clear();  // lane names --> start columns, left to right
      _LaneBreadths.Clear();  // lane names --> needed width in columns
      _Layers.Clear();
      _NeededSpaces.Clear();
      base.DoLayout(coll);
      _LanePositions.Clear();
      _LaneBreadths.Clear();
      _Layers.Clear();
      _NeededSpaces.Clear();
    }

    /// <inheritdoc/>
    [Undocumented]
    protected override double NodeMinLayerSpace(LayeredDigraphVertex v, bool topleft) {
      if (_NeededSpaces == null) _NeededSpaces = _ComputeNeededLayerSpaces(Network);
      if (v.Node == null) return 0;
      var lay = v.Layer;
      if (!topleft) {
        if (lay > 0) lay--;
      }
      var overlaps = _NeededSpaces.ElementAtOrDefault(lay) / 2;
      var edges = _CountEdgesForDirection(v, (Direction > 135) ? !topleft : topleft);
      var needed = Math.Max(overlaps, edges) * Router.LinkSpacing * 1.5;
      if (Direction == 90 || Direction == 270) {
        if (topleft) {
          return v.Focus.Y + 10 + needed;
        } else {
          return v.Bounds.Height - v.Focus.Y + 10 + needed;
        }
      } else {
        if (topleft) {
          return v.Focus.X + 10 + needed;
        } else {
          return v.Bounds.Width - v.Focus.X + 10 + needed;
        }
      }
    }

    private static int _CountEdgesForDirection(LayeredDigraphVertex vertex, bool topleft) {
      var c = 0;
      var lay = vertex.Layer;
      foreach (var e in vertex.Edges) {
        if (topleft) {
          if (e.GetOtherVertex(vertex).Layer >= lay) c++;
        } else {
          if (e.GetOtherVertex(vertex).Layer <= lay) c++;
        }
      }
      return c;
    }

    private static List<double> _ComputeNeededLayerSpaces(LayeredDigraphNetwork net) {
      // group all edges by their connected vertexes' least layer
      var layerMinEdges = new List<List<LayeredDigraphEdge>>();
      foreach (var e in net.Edges) {
        // consider all edges, including dummy ones!
        var f = e.FromVertex;
        var t = e.ToVertex;
        if (f.Column == t.Column) continue;  // skip edges that don't go between columns
        if (Math.Abs(f.Layer - t.Layer) > 1) continue;  // skip edges that don't go between adjacent layers
        var lay = Math.Min(f.Layer, t.Layer);
        var arr = layerMinEdges.ElementAtOrDefault(lay);
        if (arr == null) arr = layerMinEdges[lay] = new List<LayeredDigraphEdge>();
        arr.Add(e);
      }
      // sort each array of edges by their lowest connected vertex column
      // for edges with the same minimum column, sort by their maximum column
      var layerMaxEdges = new List<List<LayeredDigraphEdge>>();  // same as layerMinEdges, but sorted by maximum column
      for (var lay = 0; lay < layerMinEdges.Count; lay++) {
        var arr = layerMinEdges[lay];
        if (arr == null) continue;
        arr.Sort(delegate (LayeredDigraphEdge e1, LayeredDigraphEdge e2) {
          var f1c = e1.FromVertex.Column;
          var t1c = e1.ToVertex.Column;
          var f2c = e2.FromVertex.Column;
          var t2c = e2.ToVertex.Column;
          var e1mincol = Math.Min(f1c, t1c);
          var e2mincol = Math.Min(f2c, t2c);
          if (e1mincol > e2mincol) return 1;
          if (e1mincol < e2mincol) return -1;
          var e1maxcol = Math.Max(f1c, t1c);
          var e2maxcol = Math.Max(f2c, t2c);
          if (e1maxcol > e2maxcol) return 1;
          if (e1maxcol < e2maxcol) return -1;
          return 0;
        });
        layerMaxEdges[lay] = new List<LayeredDigraphEdge>(arr);
        layerMaxEdges[lay].Sort(delegate (LayeredDigraphEdge e1, LayeredDigraphEdge e2) {
          var f1c = e1.FromVertex.Column;
          var t1c = e1.ToVertex.Column;
          var f2c = e2.FromVertex.Column;
          var t2c = e2.ToVertex.Column;
          var e1maxcol = Math.Max(f1c, t1c);
          var e2maxcol = Math.Max(f2c, t2c);
          if (e1maxcol > e2maxcol) return 1;
          if (e1maxcol < e2maxcol) return -1;
          var e1mincol = Math.Min(f1c, t1c);
          var e2mincol = Math.Min(f2c, t2c);
          if (e1mincol > e2mincol) return 1;
          if (e1mincol < e2mincol) return -1;
          return 0;
        });
      }

      // run through each array of edges to count how many overlaps there might be
      var layerOverlaps = new List<double>();
      for (var lay = 0; lay < layerMinEdges.Count; lay++) {
        var arr = layerMinEdges[lay];
        var mins = arr;  // sorted by min column
        var maxs = layerMaxEdges[lay];  // sorted by max column
        var maxoverlap = 0;  // maximum count for this layer
        if (mins != null && maxs != null && mins.Count > 1 && maxs.Count > 1) {
          var mini = 0;
          LayeredDigraphEdge min = null;
          var maxi = 0;
          LayeredDigraphEdge max = null;
          while (mini < mins.Count || maxi < maxs.Count) {
            if (mini < mins.Count) min = mins[mini];
            var mincol = (min != null) ? Math.Min(min.FromVertex.Column, min.ToVertex.Column) : 0;
            if (maxi < maxs.Count) max = maxs[maxi];
            var maxcol = (max != null) ? Math.Max(max.FromVertex.Column, max.ToVertex.Column) : int.MaxValue;
            maxoverlap = Math.Max(maxoverlap, Math.Abs(mini - maxi));
            if (mincol <= maxcol && mini < mins.Count) {
              mini++;
            } else if (maxi < maxs.Count) {
              maxi++;
            }
          }
        }
        layerOverlaps[lay] = maxoverlap * 1.5;  // # of parallel links
      }
      return layerOverlaps;
    }

    private void _SetupLanes() {
      // set up some data structures
      var layout = this;
      var laneNameSet = new HashSet<string>();
      laneNameSet.UnionWith(LaneNames);
      var laneIndexes = new Dictionary<string, int>();  // lane names --> index when sorted

      foreach (var v in Network.Vertexes) {
        var lane = GetLane(v);  // cannot call FindLane yet
        if (lane != null && !laneNameSet.Contains(lane)) {
          laneNameSet.Add(lane);
          LaneNames.Add(lane);
        }

        var layer = v.Layer;
        if (layer >= 0) {
          var arr = _Layers.ElementAtOrDefault(layer);
          if (arr == null) {
            while (_Layers.Count <= layer) _Layers.Add(null);
            _Layers[layer] = new List<LayeredDigraphVertex>() { v };
          } else {
            arr.Add(v);
          }
        }
      }

      // sort laneNames and initialize laneIndexes
      if (LaneComparer != null) LaneNames.Sort(LaneComparer);
      for (var i = 0; i < LaneNames.Count; i++) {
        laneIndexes[LaneNames[i]] = i;
      }
      // now OK to call findLane

      // sort vertexes so that vertexes are grouped by lane
      for (var i = 0; i <= MaxLayer; i++) {
        _Layers[i].Sort((LayeredDigraphVertex a, LayeredDigraphVertex b) => { return _CompareVertexes(a, b); });
      }
    }

    /// Replace the standard ReduceCrossings behavior so that it respects lanes.
    /// <summary>
    /// Undocumented.
    /// </summary>
    [Undocumented]
    protected override void ReduceCrossings() {
      _SetupLanes();

      // this just cares about the .Index and ignores .Column
      var layers = _Layers;
      var red = Reducer;
      if (red != null) {
        for (var i = 0; i < layers.Count - 1; i++) {
          red.ReduceCrossings(layers[i], layers[i + 1]);
          for (var j = 0; j < layers[i].Count; j++) {
            var v = layers[i][j];
            v.Index = j;
          }
        }
        for (var i = layers.Count - 1; i > 0; i--) {
          red.ReduceCrossings(layers[i], layers[i - 1]);
          for (var j = 0; j < layers[i].Count; j++) {
            var v = layers[i][j];
            v.Index = j;
          }
        }
      }

      _ComputeLanes(); // and recompute all vertex.Column values
    }

    private void _ComputeLanes() {
      // compute needed width for each lane, in columns
      foreach (var lane in LaneNames) {
        LaneBreadths.Add(lane, ComputeMinLaneWidth(lane));
      }
      var lwidths = new Dictionary<string, int>(); // reused for each layer
      for (var i = 0; i <= MaxLayer; i++) {
        var arr = _Layers[i];
        if (arr != null) {
          var layout = this;
          // now run through Array finding width (in columns) of each lane
          // and max with LaneBreaths[lane]
          foreach (var v in arr) {
            var w = NodeMinColumnSpace(v, true) + 1 + NodeMinColumnSpace(v, false);
            var ln = FindLane(v) ?? "";
            if (!lwidths.TryGetValue(ln, out var totw)) {
              lwidths[ln] = w;
            } else {
              lwidths[ln] = totw + w;
            }
          }
          foreach (var kvp in lwidths) {
            var lane = kvp.Key;
            var colsInLayer = kvp.Value;
            layout.LaneBreadths.TryGetValue(lane, out var colsMax);
            if (colsInLayer > colsMax) layout.LaneBreadths[lane] = colsInLayer;

          }
          lwidths.Clear();
        }
      }

      // compute starting positions for each line
      var x = 0;
      foreach (var lane in LaneNames) {
        LanePositions[lane] = x;
        LaneBreadths.TryGetValue(lane, out var w);
        x += w + LaneSpacing;
      }

      _RenormalizeColumns();
    }

    private void _RenormalizeColumns() {
      // set new column and index on each vertex
      foreach (var arr in _Layers) {
        string prevlane = null;
        var c = 0;
        for (var j = 0; j < arr.Count; j++) {
          var v = arr[j];
          v.Index = j;

          var l = FindLane(v);
          if (l != null && prevlane != l) {
            LanePositions.TryGetValue(l, out c);
            LaneBreadths.TryGetValue(l, out var w);
            // compute needed breadth within lane, in columns
            var z = NodeMinColumnSpace(v, true) + 1 + NodeMinColumnSpace(v, false);
            var k = j + 1;
            while (k < arr.Count && FindLane(arr[k]) == l) {
              var vz = arr[k];
              z += NodeMinColumnSpace(vz, true) + 1 + NodeMinColumnSpace(vz, false);
              k++;
            }
            // if there is extra space, shift the vertexes to the middle of the lane
            if (z < w) {
              c += (w - z) / 2;
            }
          }

          c += NodeMinColumnSpace(v, true);
          v.Column = (int)c;
          c += 1;
          c += NodeMinColumnSpace(v, false);
          prevlane = l;
        }
      }
    }

    /// <summary>
    /// Return the minimum lane width, in columns.
    /// </summary>
    public virtual int ComputeMinLaneWidth(string lane) {
      return 0;
    }

    /// Disable normal StraightenAndPack behavior, which would mess up the columns.
    /// <summary>
    /// Undocumented.
    /// </summary>
    [Undocumented]
    protected override void StraightenAndPack() { }

    /// <summary>
    /// Given a vertex, get the lane (name) that its node belongs in.
    /// </summary>
    /// <remarks>
    /// If the lane appears to be undefined, this returns the empty string.
    /// For dummy vertexes (with no node) this will return null.
    /// </remarks>
    protected string GetLane(LayeredDigraphVertex v) {
      if (v == null) return null;
      var node = v.Node;
      if (node != null) {
        var data = node.Data;
        if (data != null) {
          string lane = null;
          if (LaneProperty != null) {
            lane = (string)data.GetType().GetProperty(LaneProperty).GetValue(data);
          }
          if (lane != null) return lane;
          return "";
        }
      }
      return null;
    }

    /// <summary>
    /// This is just like <see cref="GetLane(LayeredDigraphVertex)"/> but handles dummy vertexes
    /// for which the <see cref="GetLane(LayeredDigraphVertex)"/> returns null by returning the
    /// lane of the edge's source or destination vertex.
    /// </summary>
    /// <remarks>
    /// This can only be called after the lanes have been set up internally.
    /// </remarks>
    protected string FindLane(LayeredDigraphVertex v) {
      if (v != null) {
        var lane = GetLane(v);
        if (lane != null) {
          return lane;
        } else {
          var srcv = _FindRealSource(v.SourceEdges.First());
          var dstv = _FindRealDestination(v.DestinationEdges.First());
          var srcLane = GetLane(srcv);
          var dstLane = GetLane(dstv);
          if (srcLane != null || dstLane != null) {
            if (srcLane == dstLane) return srcLane;
            if (srcLane != null) return srcLane;
            if (dstLane != null) return dstLane;
          }
        }
      }
      return null;
    }

    private LayeredDigraphVertex _FindRealSource(LayeredDigraphEdge e) {
      if (e == null) return null;
      var fv = e.FromVertex;
      if (fv != null && fv.Node != null) return fv;
      return _FindRealSource(fv.SourceEdges.First());
    }

    private LayeredDigraphVertex _FindRealDestination(LayeredDigraphEdge e) {
      if (e == null) return null;
      var tv = e.ToVertex;
      if (tv.Node != null) {
        return tv;
      }
      return _FindRealDestination(tv.DestinationEdges.First());
    }

    private int _CompareVertexes(LayeredDigraphVertex v, LayeredDigraphVertex w) {
      var laneV = FindLane(v);
      if (laneV == null) laneV = "";
      var laneW = FindLane(w);
      if (laneW == null) laneW = "";
      var ret =  laneV.CompareTo(laneW);
      if (ret != 0) return ret;
      return v.Column.CompareTo(w.Column);
    }
  }
}

