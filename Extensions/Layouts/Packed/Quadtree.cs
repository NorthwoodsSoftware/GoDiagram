/*
*  Copyright (C) 1998-2023 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main GoDiagram library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in the GoDiagram repository (https://github.com/NorthwoodsSoftware/GoDiagram/tree/main/Extensions).
* See the Extensions intro page (https://godiagram.com/intro/extensions.html) for more information.
*/

using System;
using System.Collections.Generic;

namespace Northwoods.Go.Layouts.Extensions {
  [Undocumented]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
  public class QuadNode<T> {
    public Rect Bounds;
    public QuadNode<T> Parent;
    public int Level;
    public List<T> Objects = new();
    public List<QuadObj<T>> TreeObjects = new();
    public int TotalObjects = 0; // total in this node + in all children (recursively)
    public QuadNode<T>[] Nodes = new QuadNode<T>[4];

    public QuadNode(Rect bounds, QuadNode<T> parent, int level) {
      Bounds = bounds;
      Parent = parent;
      Level = level;
    }

    public void Split() {
      var w2 = Bounds.Width / 2;
      var h2 = Bounds.Height / 2;
      var x = Bounds.X;
      var y = Bounds.Y;

      Nodes[0] = new QuadNode<T>(new Rect(x + w2, y, w2, h2), this, Level + 1);
      Nodes[1] = new QuadNode<T>(new Rect(x, y, w2, h2), this, Level + 1);
      Nodes[2] = new QuadNode<T>(new Rect(x, y + h2, w2, h2), this, Level + 1);
      Nodes[3] = new QuadNode<T>(new Rect(x + w2, y + h2, w2, h2), this, Level + 1);
    }

    public void Clear() {
      TreeObjects = new List<QuadObj<T>>();
      Objects = new List<T>();
      TotalObjects = 0;
      for (var i = 0; i < Nodes.Length; i++) {
        var n = Nodes[i];
        if (n != null) {
          n.Clear();
          Nodes[i] = null;
        }
      }
    }
  }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

  /// <summary>
  /// Object to be contained by the <see cref="Quadtree{T}"/> class. This object needs
  /// to have rectangular bounds (described by an <see cref="Rect"/> object), as well
  /// as something (of any type) associated with it.
  /// </summary>
  [Undocumented]
  public class QuadObj<T> {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public Rect Bounds;
    public T Obj;

    public QuadObj(Rect bounds, T obj) {
      Bounds = bounds;
      Obj = obj;
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
  }

  /// <summary>
  /// Implementation of the quadtree data structure using the <see cref="Rect"/> class.
  /// </summary>
  /// <remarks>
  /// Each Quadtree has defined bounds found at <see cref="Bounds"/>, an array
  /// of member rectangles, and an array of child nodes
  /// (Quadtrees themselves). If the Quadtree has no
  /// children, the nodes array will have four nulls. To construct a Quadtree, you
  /// can call its constructor with no arguments. Then, to insert a rectangle, call
  /// <see cref="Add(T, Rect)"/>. This tree supports adding points (rectangles with 0
  /// width and height), segments (rectangles with either 0 width or 0 height), and
  /// rectangles with nonzero widths and heights.
  ///
  /// Quadtrees can be used to calculate intersections extremely quickly between a
  /// given rectangle and all of the rectangles in the quadtree. Use of this data
  /// structure prevents having to do precise intersection calculations for every
  /// rectangle in the tree. To calculate all of the rectangular intersections for
  /// a given rectangle, use <see cref="Intersecting(Rect)"/>.
  ///
  /// Other common operations are detailed below.
  /// </remarks>
  /// @category Layout Extension
  public class Quadtree<T> {
    private QuadNode<T> _Root;

    private readonly int _NodeCapacity = 1;
    private readonly int _MaxLevels = int.MaxValue;
    private readonly Dictionary<T, QuadObj<T>> _TreeObjectMap = new();

    // we can avoid unnecessary work when adding objects if there are no objects with 0 width or height.
    // Note that after being set to true, these flags are not ever set again to false, even if all objects
    // with zero width/height are removed (assumption was made that this should almost never matter)
    private bool _HasZeroWidthObject = false;
    private bool _HasZeroHeightObject = false;

    /// <summary>
    /// Gets the node capacity of this quadtree. This is the number of objects a node can contain before it splits.
    /// </summary>
    public double NodeCapacity {
      get {
        return _NodeCapacity;
      }
    }
    /// <summary>
    /// Gets the maximum depth the Quadtree will allow before it will no longer split.
    /// </summary>
    public double MaxLevels {
      get {
        return _MaxLevels;
      }
    }
    /// <summary>
    /// Gets the boundaries of the node. All nodes should be square.
    /// </summary>
    public Rect Bounds {
      get {
        return _Root.Bounds;
      }
    }
    /// <summary>
    /// Gets the root node of the tree.
    /// </summary>
    public QuadNode<T> Root {
      get {
        return _Root;
      }
    }


    /// <summary>
    /// In most cases, simply calling this constructor with no arguments will produce the desired behaviour.
    /// </summary>
    /// <param name="nodeCapacity">The node capacity of this quadtree. This is the double of objects a node can contain before it splits. Defaults to 1.</param>
    /// <param name="maxLevel">The maximum depth the Quadtree will allow before it will no longer split. Defaults to double.PositiveInfinity (no maximum depth).</param>
    /// <param name="bounds">The bounding box surrounding the entire Quadtree. If the bounds are unset or a node is inserted outside of the bounds, the tree will automatically grow.</param>
    public Quadtree(int nodeCapacity = 1, int maxLevel = int.MaxValue, Rect bounds = new Rect()) {
      _NodeCapacity = nodeCapacity;
      _MaxLevels = maxLevel;

      _Root = new QuadNode<T>(bounds, null, 0);
    }

    /// <summary>
    /// Clears the Quadtree, removing all objects and children nodes. Keeps the current bounds of the root node.
    /// </summary>
    public void Clear() {
      _Root.Clear();
      _TreeObjectMap.Clear();
    }

    private static bool IsNullArea(Rect rect, double error = 1e-7) {
      return Math.Abs(rect.Width * rect.Height) < error;
    }

    /// <summary>
    /// Returns a list of possible quadrants that the given rect could be in
    /// </summary>
    /// <param name="rect">the rectangle to test</param>
    /// <param name="node"></param>
    private static List<int> _GetQuadrants(Rect rect, QuadNode<T> node) {
      var quadrants = new List<int>();
      var horizontalMidpoint = node.Bounds.X + (node.Bounds.Width / 2);
      var verticalMidpoint = node.Bounds.Y + (node.Bounds.Height / 2);

      var topQuadrant = rect.Y <= verticalMidpoint;
      var bottomQuadrant = rect.Y + rect.Height >= verticalMidpoint;

      if (rect.X <= horizontalMidpoint) {
        if (topQuadrant) {
          quadrants.Add(1);
        }
        if (bottomQuadrant) {
          quadrants.Add(2);
        }
      }
      if (rect.X + rect.Width >= horizontalMidpoint) {
        if (topQuadrant) {
          quadrants.Add(0);
        }
        if (bottomQuadrant) {
          quadrants.Add(3);
        }
      }
      return quadrants;
    }

    /// <summary>
    /// Determine which node the rect belongs to. -1 means rect
    /// cannot completely fit within a child node and is part of
    /// the parent node. This function avoids some additional
    /// calculations by assuming that the rect is contained entirely
    /// within the parent node's bounds.
    /// </summary>
    /// <param name="rect">the rect to test</param>
    /// <param name="node"></param>
    private static int _GetIndex(Rect rect, QuadNode<T> node) {
      var index = -1;
      if (node.Bounds.IsEmpty()) { // the quadtree is empty (empty Bounds)
        return index;
      }

      var horizontalMidpoint = node.Bounds.X + (node.Bounds.Width / 2);
      var verticalMidpoint = node.Bounds.Y + (node.Bounds.Height / 2);

      var topQuadrant = rect.Y <= verticalMidpoint && rect.Y + rect.Height <= verticalMidpoint;
      var bottomQuadrant = rect.Y >= verticalMidpoint;

      if (rect.X + rect.Width <= horizontalMidpoint) {
        if (topQuadrant) {
          index = 1;
        } else if (bottomQuadrant) {
          index = 2;
        }
      } else if (rect.X >= horizontalMidpoint) {
        if (topQuadrant) {
          index = 0;
        } else if (bottomQuadrant) {
          index = 3;
        }
      }

      return index;
    }

    /// <summary>
    /// Insert the object into the quadtree. If the node
    /// exceeds the capacity, it will split and add all
    /// objects to their corresponding nodes. If the object is
    /// outside the bounds of the tree's root node, the tree
    /// will grow to accomodate it. Possibly restructures the
    /// tree if a more efficient configuration can be found with
    /// the new dimensions.
    /// </summary>
    /// <param name="obj">the object to insert</param>
    /// <param name="bounds">The Rect bounds of the object</param>
    public void Add(T obj, Rect bounds) {
      _Add(obj, new Rect(bounds.X, bounds.Y, bounds.Width, bounds.Height));
    }

    /// <summary>
    /// Insert the object into the quadtree. If the node
    /// exceeds the capacity, it will split and add all
    /// objects to their corresponding nodes. If the object is
    /// outside the bounds of the tree's root node, the tree
    /// will grow to accomodate it. Possibly restructures the
    /// tree if a more efficient configuration can be found with
    /// the new dimensions.
    /// </summary>
    /// <param name="obj">the object to insert</param>
    /// <param name="x">The x value.</param>
    /// <param name="y">The y value.</param>
    /// <param name="w">The width, must be non-negative.</param>
    /// <param name="h">The height, must be non-negative</param>
    public void Add(T obj, double x, double y, double w, double h) {
      _Add(obj, new Rect(x, y, w, h));
    }

    /// <summary>
    /// Insert the object into the quadtree. If the node
    /// exceeds the capacity, it will split and add all
    /// objects to their corresponding nodes. If the object is
    /// outside the bounds of the tree's root node, the tree
    /// will grow to accomodate it. Possibly restructures the
    /// tree if a more efficient configuration can be found with
    /// the new dimensions.
    /// </summary>
    /// <param name="obj">the object to insert</param>
    public void Add(QuadObj<T> obj) {
      Add(obj.Obj, obj.Bounds);
    }

    /// <summary>
    /// Insert the object into the quadtree. If the node
    /// exceeds the capacity, it will split and add all
    /// objects to their corresponding nodes. If the object is
    /// outside the bounds of the tree's root node, the tree
    /// will grow to accomodate it. Possibly restructures the
    /// tree if a more efficient configuration can be found with
    /// the new dimensions. Bounds can be given either as a
    /// single <see cref="Rect"/> or as any combination of arguments
    /// which is valid for the <see cref="Rect"/> constructor.
    /// </summary>
    /// <param name="obj">the object to insert</param>
    /// <param name="bounds">The Rect bounds of the object</param>
    private void _Add(T obj, Rect bounds) {
      var treeObj = new QuadObj<T>(bounds, obj);

      if (double.IsNaN(bounds.X) || bounds.X == double.PositiveInfinity ||
        double.IsNaN(bounds.Y) || bounds.Y == double.PositiveInfinity ||
        double.IsNaN(bounds.Width) || bounds.Width == double.PositiveInfinity ||
        double.IsNaN(bounds.Height) || bounds.Height == double.PositiveInfinity) {
        throw new Exception("Invalid rectangle, contains NaN or double.PositiveInfinity properties");
      }

      _HasZeroWidthObject = _HasZeroWidthObject || bounds.Width == 0;
      _HasZeroHeightObject = _HasZeroHeightObject || bounds.Height == 0;

      // initialize bounds of tree as the max width or height of the first object added
      if (_Root.Bounds.Width == 0 || _Root.Bounds.Height == 0) {
        var len = Math.Max(bounds.Width, bounds.Height);
        _Root.Bounds = new Rect(bounds.X, bounds.Y, len, len);
      }

      // fixes quadtree having a width and height of 0 if the first object added is a point
      // this will only be called after a second object is added, the new width/height is the maximum distance between them
      if (!IsNullArea(_Root.Bounds) && (_Root.Bounds.Width == 0 || _Root.Bounds.Height == 0)) {
        var len = Math.Max(Math.Abs(bounds.X - _Root.Bounds.X), Math.Abs(bounds.Y - _Root.Bounds.Y));
        _Root.Bounds = new Rect(Math.Min(_Root.Bounds.X, bounds.X), Math.Min(_Root.Bounds.Y, bounds.Y), len, len);
      }

      // map the object to its corresponding QuadObj (so that the bounds of this object can be retrieved later)
      _TreeObjectMap.Add(obj, treeObj);

      // grow as many times as necessary to fit the new object
      while (!_Root.Bounds.Contains(bounds)) {
        var old = _Root;
        Walk(n => n.Level++, old);

        var intersectsTopBound = bounds.Y < _Root.Bounds.Y;
        var intersectsBottomBound = bounds.Y + bounds.Height > _Root.Bounds.Y + _Root.Bounds.Height;
        var intersectsRightBound = bounds.X + bounds.Width > _Root.Bounds.X + _Root.Bounds.Width;
        var intersectsLeftBound = bounds.X < _Root.Bounds.X;

        if ((intersectsTopBound && intersectsRightBound) || (intersectsTopBound && !intersectsLeftBound)) {
          /*  _______
           * | 1 | 0 |
           * |___|___|
           * |old| 3 |
           * |___|___|
           */
          var newBounds = new Rect(_Root.Bounds.X,
                                    _Root.Bounds.Y - _Root.Bounds.Height,
                                    _Root.Bounds.Width * 2,
                                    _Root.Bounds.Height * 2);
          _Root = new QuadNode<T>(newBounds, null, 0);
          _Root.Split();
          _Root.Nodes[2] = old;
          _Root.TotalObjects = old.TotalObjects;
          old.Parent = _Root;
          _Restructure(old);
          _RestructureLevels(old);
          if (_HasZeroHeightObject) {
            _FixTopObjectPlacement(old);
          }
        } else if (intersectsTopBound && intersectsLeftBound) {
          /*  _______
           * | 1 | 0 |
           * |___|___|
           * | 2 |old|
           * |___|___|
           */
          var newBounds = new Rect(_Root.Bounds.X - _Root.Bounds.Width,
                                    _Root.Bounds.Y - _Root.Bounds.Height,
                                    _Root.Bounds.Width * 2,
                                    _Root.Bounds.Height * 2);
          _Root = new QuadNode<T>(newBounds, null, 0);
          _Root.Split();
          _Root.Nodes[3] = old;
          _Root.TotalObjects = old.TotalObjects;
          old.Parent = _Root;
          _Restructure(old);
          _RestructureLevels(old);
          if (_HasZeroWidthObject) {
            _FixLeftObjectPlacement(old);
          }
          if (_HasZeroHeightObject) {
            _FixTopObjectPlacement(old);
          }
        } else if ((intersectsBottomBound && intersectsRightBound) || ((intersectsRightBound || intersectsBottomBound) && !intersectsLeftBound)) {
          /*  _______
           * |old| 0 |
           * |___|___|
           * | 2 | 3 |
           * |___|___|
           */
          var newBounds = new Rect(_Root.Bounds.X,
                                    _Root.Bounds.Y,
                                    _Root.Bounds.Width * 2,
                                    _Root.Bounds.Height * 2);
          _Root = new QuadNode<T>(newBounds, null, 0);
          _Root.Split();
          _Root.Nodes[1] = old;
          _Root.TotalObjects = old.TotalObjects;
          old.Parent = _Root;
          _Restructure(old);
          _RestructureLevels(old);
        } else if ((intersectsBottomBound && intersectsLeftBound) || intersectsLeftBound) {
          /*  _______
           * | 1 |old|
           * |___|___|
           * | 2 | 3 |
           * |___|___|
           */
          var newBounds = new Rect(_Root.Bounds.X - _Root.Bounds.Width,
                                    _Root.Bounds.Y,
                                    _Root.Bounds.Width * 2,
                                    _Root.Bounds.Height * 2);
          _Root = new QuadNode<T>(newBounds, null, 0);
          _Root.Split();
          _Root.Nodes[0] = old;
          _Root.TotalObjects = old.TotalObjects;
          old.Parent = _Root;
          _Restructure(old);
          _RestructureLevels(old);
          if (_HasZeroWidthObject) {
            _FixLeftObjectPlacement(old);
          }
        }
      }

      // add the object to the tree
      _AddHelper(_Root, treeObj);
    }

    /// <summary>
    /// Helper function to recursively perform the add operation
    /// on the tree.
    /// </summary>
    /// <param name="root">the current node being operated on</param>
    /// <param name="treeObj">the object being added</param>
    /// <returns></returns>
    private void _AddHelper(QuadNode<T> root, QuadObj<T> treeObj) {
      root.TotalObjects++;

      if (root.Nodes[0] != null) {
        var index = _GetIndex(treeObj.Bounds, root);
        if (index != -1) {
          var selected = root.Nodes[index];
          if (selected != null) {
            _AddHelper(selected, treeObj);
            return;
          }
        }
      }

      root.TreeObjects.Add(treeObj);
      root.Objects.Add(treeObj.Obj);
      if (root.TreeObjects.Count > _NodeCapacity && root.Level < _MaxLevels) {
        if (root.Nodes[0] == null) {
          root.Split();
        }

        var i = 0;
        while (i < root.TreeObjects.Count) {
          var index = _GetIndex(root.TreeObjects[i].Bounds, root);
          if (index != -1 && !(root.TreeObjects[i].Bounds.Width == 0 || root.TreeObjects[i].Bounds.Height == 0)) {
            root.Objects.RemoveAt(i);
            var selected = root.Nodes[index];
            if (selected != null) {
              var treeObjArg = root.TreeObjects[i];
              root.TreeObjects.RemoveAt(i);
              _AddHelper(selected, treeObjArg);
            }
          } else {
            i++;
          }
        }
      }
    }

    /// <summary>
    /// Recursively moves objects placed on the right side of a vertical border
    /// between two nodes to the left side of the vertical border. This allows
    /// them to be located by <see cref="_GetIndex"/>. This function is called
    /// after an <see cref="Add(T, Rect)"/> call grows the Quadtree, but only if there
    /// are 0 width objects in the tree.
    /// </summary>
    /// <param name="root">the current root node being operated on</param>
    private void _FixLeftObjectPlacement(QuadNode<T> root) {
      var nw = root.Nodes[1];
      if (nw != null) { // if root is split
        _FixLeftObjectPlacement(nw); // NW
        var sw = root.Nodes[2];
        if (sw != null) {
          _FixLeftObjectPlacement(sw); // SW
        }
      }

      var toRemove = new List<int>();
      var toAdd = new List<QuadObj<T>>();
      for (var i = 0; i < root.Objects.Count; i++) {
        var obj = root.TreeObjects[i];
        if (obj.Bounds.Width == 0 && obj.Bounds.X == root.Bounds.X) {
          toRemove.Add(i);
          toAdd.Add(obj);
        }
      }
      _RemoveFromOwner(root, toRemove);
      foreach (var obj in toAdd) {
        Add(obj.Obj, obj.Bounds);
      }
    }

    /// <summary>
    /// Recursively moves objects placed on the bottom side of a horizontal border
    /// between two nodes to the top side of the vertical border. This allows
    /// them to be located by <see cref="_GetIndex"/>. This function is called
    /// after an <see cref="Add(T, Rect)"/> call grows the Quadtree, but only if there
    /// are 0 height objects in the tree.
    /// </summary>
    /// <param name="root">the current root node being operated on</param>
    private void _FixTopObjectPlacement(QuadNode<T> root) {
      var ne = root.Nodes[0];
      if (ne != null) { // if root is split
        _FixTopObjectPlacement(ne); // NE
        var nw = root.Nodes[1];
        if (nw != null) {
          _FixTopObjectPlacement(nw); // NW
        }
      }

      var toRemove = new List<int>();
      var toAdd = new List<QuadObj<T>>();
      for (var i = 0; i < root.Objects.Count; i++) {
        var obj = root.TreeObjects[i];
        if (obj.Bounds.Height == 0 && obj.Bounds.Y == root.Bounds.Y) {
          toRemove.Add(i);
          toAdd.Add(obj);
        }
      }
      _RemoveFromOwner(root, toRemove);
      foreach (var obj in toAdd) {
        Add(obj);
      }
    }

    /// <summary>
    /// Moves all objects from a leaf node to its parent and unsplits.
    /// Used after growing the tree when level>max level.
    /// </summary>
    /// <param name="node">the leaf node to restructure</param>
    private void _RestructureLevels(QuadNode<T> node) {
      if (node != null && _MaxLevels < int.MaxValue && node.Nodes[0] != null) {
        if (node.Level >= _MaxLevels) {
          for (var i = 0; i < node.Nodes.Length; i++) {
            var selected = node.Nodes[i];
            if (selected != null) {
              node.Objects.AddRange(selected.Objects);
              node.TreeObjects.AddRange(selected.TreeObjects);
              selected.Clear();
              node.Nodes[i] = null;
            }
          }
        } else {
          for (var i = 0; i < node.Nodes.Length; i++) {
            var selected = node.Nodes[i];
            if (selected != null) {
              _RestructureLevels(selected);
            }
          }
        }
      }
    }

    /// <summary>
    /// Return the node that contains the given object.
    /// </summary>
    /// <param name="obj">the object to find</param>
    /// <returns>the node containing the given object, null if the object is not found</returns>
    public QuadNode<T> Find(T obj) {
      if (_TreeObjectMap.TryGetValue(obj, out var treeObj)) {
        return _FindHelper(_Root, treeObj);
      }

      return null;
    }

    private QuadNode<T> _FindHelper(QuadNode<T> root, QuadObj<T> treeObj) {
      foreach (var obj in root.TreeObjects) {
        if (obj == treeObj) {
          return root;
        }
      }

      var index = _GetIndex(treeObj.Bounds, root);
      var selected = index == -1 ? null : root.Nodes[index];
      if (selected != null) {
        var result = _FindHelper(selected, treeObj);
        if (result != null) {
          return result;
        }
      }

      return null;
    }

    /// <summary>
    /// Convenience method, calls <see cref="Find"/> and returns a bool
    /// indicating whether or not the tree contains the given object.
    /// </summary>
    /// <param name="obj">the object to check for</param>
    /// <returns>whether or not the given object is present in the tree</returns>
    public bool Has(T obj) {
      return Find(obj) != null;
    }

    /// <summary>
    /// Checks if any of the objects in the tree have the given boundaries.
    /// </summary>
    /// <param name="bounds">the rectangle to check for</param>
    /// <returns>the actual bounds object stored in the tree</returns>
    public Rect? FindBounds(Rect bounds) {
      return _FindBoundsHelper(_Root, bounds);
    }

    private Rect? _FindBoundsHelper(QuadNode<T> root, Rect bounds) {
      foreach (var obj in root.TreeObjects) {
        if (bounds.EqualsApprox(obj.Bounds)) {
          return bounds;
        }
      }

      var index = _GetIndex(bounds, root);
      var selected = index == -1 ? null : root.Nodes[index];
      if (selected != null) {
        return _FindBoundsHelper(selected, bounds);
      }

      return null;
    }

    /// <summary>
    /// Remove the given object from the tree, restructuring to
    /// get rid of empty nodes that are unneeded.
    /// </summary>
    /// <param name="obj">the object to remove</param>
    /// <returns>whether or not the deletion was successful. False when the object is not in the tree.</returns>
    public bool Remove(T obj) {
      if (_TreeObjectMap.TryGetValue(obj, out var treeObj)) {
        var owner = _FindHelper(_Root, treeObj);

        if (owner != null) {
          owner.TreeObjects.Remove(treeObj);
          owner.Objects.Remove(obj);
          owner.TotalObjects--;
          _TreeObjectMap.Remove(obj);
          var parent = owner.Parent;
          while (parent != null) {
            parent.TotalObjects--;
            parent = parent.Parent;
          }
          if (owner.Nodes[0] != null && owner.TotalObjects <= _NodeCapacity) {
            _AddChildObjectsToNode(owner, owner);
            for (var i = 0; i < owner.Nodes.Length; i++) {
              var selected = owner.Nodes[i];
              if (selected != null) {
                selected.Clear();
              }
              owner.Nodes[i] = null;
            }
          }
          _Restructure(owner);
          return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Removes multiple objects at the given indices from the given owner. Similar
    /// to the normal remove function, but much faster when the owner and indices are
    /// already known.
    /// </summary>
    /// <param name="owner">the node to remove objects from</param>
    /// <param name="indexes">the indices to remove. Should be given in ascending order.</param>
    private void _RemoveFromOwner(QuadNode<T> owner, List<int> indexes) {
      if (indexes.Count == 0) {
        return;
      }

      for (var i = indexes.Count - 1; i >= 0; i--) {
        _TreeObjectMap.Remove(owner.Objects[indexes[i]]);
        owner.TreeObjects.RemoveAt(indexes[i]);
        owner.Objects.RemoveAt(indexes[i]);
      }

      owner.TotalObjects -= indexes.Count;
      var parent = owner.Parent;
      while (parent != null) {
        parent.TotalObjects -= indexes.Count;
        parent = parent.Parent;
      }
      if (owner.Nodes[0] != null && owner.TotalObjects <= _NodeCapacity) {
        _AddChildObjectsToNode(owner, owner);
        for (var i = 0; i < owner.Nodes.Length; i++) {
          var selected = owner.Nodes[i];
          if (selected != null) {
            selected.Clear();
          }
          owner.Nodes[i] = null;
        }
      }
      _Restructure(owner);
    }

    /// <summary>
    /// Recursively adds all objects from children of the given
    /// root tree to the given owner tree.
    /// Used internally by <see cref="Remove"/>.
    /// </summary>
    /// <param name="owner">the tree to add objects to</param>
    /// <param name="root"></param>
    private void _AddChildObjectsToNode(QuadNode<T> owner, QuadNode<T> root) {
      foreach (var node in root.Nodes) {
        if (node != null) {
          owner.TreeObjects.AddRange(node.TreeObjects);
          owner.Objects.AddRange(node.Objects);
          _AddChildObjectsToNode(owner, node);
        }
      }
    }

    /// <summary>
    /// Recursively combines parent nodes that should be split, all the way
    /// up the tree. Starts from the given node.
    /// </summary>
    private void _Restructure(QuadNode<T> root) {
      var parent = root.Parent;
      if (parent != null) {
        // if none of the child nodes have any objects, the parent should not be split
        var childrenHaveNoObjects = true;
        foreach (var node in parent.Nodes) {
          if (node != null && node.TotalObjects > 0) {
            childrenHaveNoObjects = false;
            break;
          }
        }

        // unsplit children and move nodes to parent
        if (parent.TotalObjects <= _NodeCapacity || childrenHaveNoObjects) {
          for (var i = 0; i < parent.Nodes.Length; i++) {
            var selected = parent.Nodes[i];
            if (selected != null) {
              parent.Objects.AddRange(selected.Objects);
              parent.TreeObjects.AddRange(selected.TreeObjects);
              selected.Clear();
              parent.Nodes[i] = null;
            }
          }
          _Restructure(parent);
        }

      }
    }

    /// <summary>
    /// Translate the given object to a given <see cref="Point"/>.
    /// </summary>
    /// <param name="obj">the object to move</param>
    /// <param name="p">the Point to move the object to</param>
    /// <returns>whether or not the move was successful. False if the object was not in the tree.</returns>
    public bool Move(T obj, Point p) {
      return Move(obj, p.X, p.Y);
    }

    /// <summary>
    /// Translate the given object to given x and y coordinates.
    /// </summary>
    /// <param name="obj">the object to move</param>
    /// <param name="x">the x coordinate to move the object to</param>
    /// <param name="y">the y coordinate to move the object to</param>
    /// <returns>whether or not the move was successful. False if the object was not in the tree.</returns>
    public bool Move(T obj, double x, double y) {
      if (_TreeObjectMap.TryGetValue(obj, out var treeObj) && Remove(obj)) {
        treeObj.Bounds.X = x;
        treeObj.Bounds.Y = y;
        Add(treeObj);
        return true;
      }
      return false;
    }

    /// <summary>
    /// Resize the given object to a given <see cref="Size"/>.
    /// </summary>
    /// <param name="obj">the object to resize</param>
    /// <param name="size">the Size to resize the object to</param>
    /// <returns>whether or not the resize was successful. False if the object was not in the tree.</returns>
    public bool Resize(T obj, Size size) {
      return Resize(obj, size.Width, size.Height);
    }

    /// <summary>
    /// Resize the given object to given width and height.
    /// </summary>
    /// <param name="obj">the object to resize</param>
    /// <param name="width">the width to resize the object to</param>
    /// <param name="height">the height to resize the object to</param>
    /// <returns>whether or not the resize was successful. False if the object was not in the tree.</returns>
    public bool Resize(T obj, double width, double height) {
      if (_TreeObjectMap.TryGetValue(obj, out var treeObj) && Remove(obj)) {
        treeObj.Bounds.Width = width;
        treeObj.Bounds.Height = height;
        Add(treeObj);
        return true;
      }
      return false;
    }

    /// <summary>
    /// Updates the given object to have the bounds given, provided as a <see cref="Rect"/>.
    /// </summary>
    /// <param name="obj">the object to change the bounds of</param>
    /// <param name="rect">the Rect to set the object to</param>
    public bool SetTo(T obj, Rect rect) {
      return SetTo(obj, rect.X, rect.Y, rect.Width, rect.Height);
    }

    /// <summary>
    /// Updates the given object to have the bounds given, provided as
    /// x, y, width, and height.
    /// </summary>
    /// <param name="obj">the object to change the bounds of</param>
    /// <param name="x">the x-coordinate to set the object to</param>
    /// <param name="y">the y-coordinate to set the object to</param>
    /// <param name="width">the width to set the object to</param>
    /// <param name="height">the height to set the object to</param>
    public bool SetTo(T obj, double x, double y, double width, double height) {
      if (_TreeObjectMap.TryGetValue(obj, out var treeObj) && Remove(obj)) {
        treeObj.Bounds.X = x;
        treeObj.Bounds.Y = y;
        treeObj.Bounds.Width = width;
        treeObj.Bounds.Height = height;
        Add(treeObj);
        return true;
      }
      return false;
    }

    /// <summary>
    /// Return all objects that intersect (wholly or partially) with
    /// the given <see cref="Point"/>. Touching edges and
    /// objects overlapping by 1e-7 or less (to account for floating
    /// point error) are both not considered intersections.
    /// </summary>
    /// <param name="point">the Point to check intersections for. A Rect with size (0, 0) is created for intersection calculations.</param>
    /// <returns>list containing all intersecting objects</returns>
    public List<T> Intersecting(Point point) {
      return Intersecting(new Rect(point.X, point.Y, 0, 0));
    }

    /// <summary>
    /// Return all objects that intersect (wholly or partially) with
    /// the given <see cref="Rect"/>. Touching edges and
    /// objects overlapping by 1e-7 or less (to account for floating
    /// point error) are both not considered intersections.
    /// </summary>
    /// <param name="rect">the Rect to check intersections for.</param>
    /// <returns>list containing all intersecting objects</returns>
    public List<T> Intersecting(Rect rect) {
      var returnObjects = new List<T>();
      _IntersectingHelper(rect, _Root, returnObjects);
      return returnObjects;
    }

    private void _IntersectingHelper(Rect rect, QuadNode<T> root, List<T> returnObjects) {
      var index = _GetIndex(rect, root);
      var selected = index == -1 ? null : root.Nodes[index];
      if (selected != null) {
        _IntersectingHelper(rect, selected, returnObjects);
      } else if (root.Nodes[0] != null) {
        var quadrants = _GetQuadrants(rect, root);
        foreach (var quadrant in quadrants) {
          var node = root.Nodes[quadrant];
          if (node != null) {
            _IntersectingHelper(rect, node, returnObjects);
          }
        }
      }

      foreach (var obj in root.TreeObjects) {
        if (_RectsIntersect(obj.Bounds, rect)) {
          returnObjects.Add(obj.Obj);
        }
      }
    }

    /// <summary>
    /// Similar as <see cref="Rect.Intersects(Rect)"/>, but doesn't count edges as intersections.
    /// Also accounts for floating error (by returning false more often) up to an error of 1e-7.
    /// Used by <see cref="Intersecting(Rect)"/>.
    /// </summary>
    /// <param name="r1">first rectangle</param>
    /// <param name="r2">second rectangle</param>
    /// <returns>whether or not the two rectangles intersect</returns>
    private static bool _RectsIntersect(Rect r1, Rect r2) {
      return !(r2.Left + 1e-7 >= r1.Right || r2.Right - 1e-7 <= r1.Left || r2.Top + 1e-7 >= r1.Bottom || r2.Bottom - 1e-7 <= r1.Top);
    }

    /// <summary>
    /// Return all objects that fully contain the given <see cref="Point"/>.
    /// </summary>
    /// <param name="point">the Point to check containing for. A Rect with size (0, 0) is created for containment calculations.</param>
    /// <returns>list containing all containing objects</returns>
    public List<T> Containing(Point point) {
      return Containing(new Rect(point.X, point.Y, 0, 0));
    }

    /// <summary>
    /// Return all objects that fully contain the given <see cref="Rect"/>.
    /// </summary>
    /// <param name="rect">the Rect to check containing for.</param>
    /// <returns>list containing all containing objects</returns>
    public List<T> Containing(Rect rect) {
      var returnObjects = new List<T>();
      _ContainingHelper(rect, _Root, returnObjects);
      return returnObjects;
    }

    private void _ContainingHelper(Rect rect, QuadNode<T> root, List<T> returnObjects) {
      var index = _GetIndex(rect, root);
      var selected = index == -1 ? null : root.Nodes[index];
      if (selected != null) {
        _ContainingHelper(rect, selected, returnObjects);
      } else if (root.Nodes[0] != null) {
        var quadrants = _GetQuadrants(rect, root);
        foreach (var quadrant in quadrants) {
          var node = root.Nodes[quadrant];
          if (node != null) {
            _ContainingHelper(rect, node, returnObjects);
          }
        }
      }

      foreach (var obj in root.TreeObjects) {
        if (obj.Bounds.Contains(rect)) {
          returnObjects.Add(obj.Obj);
        }
      }
    }

    /// <summary>
    /// Returns the square of the distance from the centers of the given objects.
    /// </summary>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    /// <returns>square of the distance between the centers of obj1 and obj2</returns>
    public double DistanceSquared(T obj1, T obj2) {
      var owner1 = Find(obj1);
      var owner2 = Find(obj2);
      if (owner1 != null && owner2 != null) {
        if (_TreeObjectMap.TryGetValue(obj1, out var treeObj1) && _TreeObjectMap.TryGetValue(obj2, out var treeObj2)) {
          return treeObj1.Bounds.Center.DistanceSquared(treeObj2.Bounds.Center);
        }
      }
      return -1;
    }

    /// <summary>
    /// Recursively traverses the tree (depth first) and executes the
    /// given callback on each node.
    /// </summary>
    /// <param name="callback">the callback to execute on each node. Takes the form of (n: Quadtree) => void</param>
    /// <param name="root">whether or not to execute the callback on the root node as well. Defaults to true</param>
    public void Walk(Action<QuadNode<T>> callback, bool root = true) {
      Walk(callback, _Root, root);
    }

    /// <summary>
    /// Recursively traverses the tree (depth first) and executes the
    /// given callback on each node.
    /// </summary>
    /// <param name="callback">the callback to execute on each node. Takes the form of (n: Quadtree) => void</param>
    /// <param name="node"></param>
    /// <param name="root">whether or not to execute the callback on the root node as well. Defaults to true</param>
    public void Walk(Action<QuadNode<T>> callback, QuadNode<T> node, bool root = true) {
      if (root) {
        root = false;
        callback(node);
      }
      foreach (var n in node.Nodes) {
        if (n != null) {
          callback(n);
          Walk(callback, n, root);
        }
      }
    }

    /// <summary>
    /// Visits every object stored in the tree (depth first).
    /// </summary>
    /// <param name="callback">the callback to execute on each object.</param>
    public void ForEach(Action<T> callback) {
      Walk((n) => {
        foreach (var obj in n.Objects) {
          callback(obj);
        }
      });
    }

    /// <summary>
    /// Finds the furthest object in each direction stored in the tree.
    /// Bounds are tested using the center x and y coordinate.
    /// </summary>
    /// <returns>maximum and minimum objects in the tree, in the format [min x, max x, min y, max y].</returns>
    public (T, T, T, T) FindExtremeObjects() {
      (var extremes0, var extremes1, var extremes2, var extremes3) = _FindExtremeObjectsHelper(_Root);
      return (
        extremes0 != null ? extremes0.Obj : default,
        extremes1 != null ? extremes1.Obj : default,
        extremes2 != null ? extremes2.Obj : default,
        extremes3 != null ? extremes3.Obj : default
      );
    }

    /// <summary>
    /// Recursive helper function for <see cref="FindExtremeObjects"/>
    /// </summary>
    /// <param name="root">the current root node being searched</param>
    /// <returns>maximum and minimum objects in the tree, in the format [min x, max x, min y, max y].</returns>
    private (QuadObj<T>, QuadObj<T>, QuadObj<T>, QuadObj<T>) _FindExtremeObjectsHelper(QuadNode<T> root) {
      QuadObj<T> minX = null;
      QuadObj<T> maxX = null;
      QuadObj<T> minY = null;
      QuadObj<T> maxY = null;
      if (root.Nodes[0] != null) { // if root is split
        foreach (var node in root.Nodes) {
          if (node != null) {
            (var extremes0, var extremes1, var extremes2, var extremes3) = _FindExtremeObjectsHelper(node);
            if (minX == null || (extremes0 != null && extremes0.Bounds.CenterX < minX.Bounds.CenterX)) {
              minX = extremes0;
            }
            if (maxX == null || (extremes1 != null && extremes1.Bounds.CenterX > maxX.Bounds.CenterX)) {
              maxX = extremes1;
            }
            if (minY == null || (extremes2 != null && extremes2.Bounds.CenterY < minY.Bounds.CenterY)) {
              minY = extremes2;
            }
            if (maxY == null || (extremes3 != null && extremes3.Bounds.CenterY > maxY.Bounds.CenterY)) {
              maxY = extremes3;
            }
          }
        }
      }

      foreach (var obj in root.TreeObjects) {
        if (minX == null || obj.Bounds.CenterX < minX.Bounds.CenterX) {
          minX = obj;
        }
        if (maxX == null || obj.Bounds.CenterX > maxX.Bounds.CenterX) {
          maxX = obj;
        }
        if (minY == null || obj.Bounds.CenterY < minY.Bounds.CenterY) {
          minY = obj;
        }
        if (maxY == null || obj.Bounds.CenterY > maxY.Bounds.CenterY) {
          maxY = obj;
        }
      }

      return (minX, maxX, minY, maxY);
    }

  }
}
