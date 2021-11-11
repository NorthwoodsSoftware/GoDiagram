/*
*  Copyright (C) 1998-2020 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main GoJS library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in the GoJS kit under the extensions or extensionsTS folders.
* See the Extensions intro page (https://gojs.net/latest/intro/extensions.html) for more information.
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Northwoods.Go.Layouts.Extensions {
  [Undocumented]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
  public interface ITreeMapData {
    int Total { get; set; }
    int Size { get; set; }
  }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

  /// <summary>
  /// A custom <see cref="Layout"/> that lays out hierarchical data using nested rectangles.
  /// </summary>
  public class TreeMapLayout : Layout {
    private bool _IsTopLayerHorizontal = false;

    /// <summary>
    /// Create a TreeMap layout.
    /// </summary>
    public TreeMapLayout() : base() { }

    /// <summary>
    /// Copies properties to a cloned Layout.
    /// </summary>
    /// <param name="c"></param>
    [Undocumented]
    protected override void CloneProtected(Layout c) {
      if (c == null) return;

      base.CloneProtected(c);
      var copy = (TreeMapLayout)c;
      copy._IsTopLayerHorizontal = _IsTopLayerHorizontal;
    }

    /// <summary>
    /// Gets or sets whether top level Parts are laid out horizontally.
    /// </summary>
    public bool IsTopLayerHorizontal {
      get {
        return _IsTopLayerHorizontal;
      }
      set {
        _IsTopLayerHorizontal = value;
        InvalidateLayout();
      }
    }

    /// <summary>
    /// This method positions all of the nodes by determining total area and then recursively tiling nodes from the top-level down.
    /// </summary>
    /// <param name="coll"></param>
    public override void DoLayout(IEnumerable<Part> coll = null) {
      var diagram = Diagram;
      if (diagram == null) throw new Exception("TreeMapLayout only works as the Diagram.Layout");
      foreach (var n in diagram.Nodes) {
        if (n is Node && !(n.Data is ITreeMapData)) {
          throw new Exception("All node data objects in a TreeMapLayout must implement TreeMapLayout.ITreeMapData.");
        }
      }

      ComputeTotals(diagram);
      // make sure data.total has been computed for every node
      // figure out how large an area to cover;
      // perhaps this should be a property that could be set, rather than depending on the current viewport
      ArrangementOrigin = InitialOrigin(ArrangementOrigin);
      var x = ArrangementOrigin.X;
      var y = ArrangementOrigin.Y;
      var w = diagram.ViewportBounds.Width;
      var h = diagram.ViewportBounds.Height;
      if (double.IsNaN(w)) w = 1000;
      if (double.IsNaN(h)) h = 1000;
      // collect all top-level nodes, and sum their totals
      var tops = new HashSet<Node>();
      var total = 0.0;

      foreach (var n in Diagram.Nodes) {
        if (n.IsTopLevel) {
          tops.Add(n);
          total += (n.Data as ITreeMapData).Total;
        }
      }
      var horiz = _IsTopLayerHorizontal;
      var gx = x;
      var gy = y;
      var lay = this;
      foreach (var n in tops) {
        var tot = (n.Data as ITreeMapData).Total;
        if (horiz) {
          var pw = w * tot / total;
          lay.LayoutNode(!horiz, n, gx, gy, pw, h);
          gx += pw;
        } else {
          var ph = h * tot / total;
          lay.LayoutNode(!horiz, n, gx, gy, w, ph);
          gy += ph;
        }
      }
    }

    private void LayoutNode(bool horiz, Panel n, double x, double y, double w, double h) {
      n.Position = new Point(x, y);
      n.DesiredSize = new Size(w, h);

      if (n is Group) {
        var g = n;
        var total = (g.Data as ITreeMapData).Total;
        var gx = x;
        var gy = y;
        var lay = this;
        foreach (var p in (g as Group).MemberParts) {
          if (p is Link) continue;
          var tot = (p.Data as ITreeMapData).Total;
          if (horiz) {
            var pw = w * tot / total;
            lay.LayoutNode(!horiz, p, gx, gy, pw, h);
            gx += pw;
          } else {
            var ph = h * tot / total;
            lay.LayoutNode(!horiz, p, gx, gy, w, ph);
            gy += ph;
          }
        }
      }
    }

    /// <summary>
    /// Compute the _total for each node in the Diagram, with a <see cref="Group"/>'s being a sum of its members.
    /// </summary>
    /// <param name="diagram"></param>
    private void ComputeTotals(Diagram diagram) {
      if (!diagram.Nodes.All(g => { return !(g is Group) || (g.Data as ITreeMapData).Total >= 0; })) {
        var groups = new HashSet<Group>();
        foreach (var n in diagram.Nodes) {
          if (n is Group) {
            groups.Add(n as Group);
          } else {
            (n.Data as ITreeMapData).Total = (n.Data as ITreeMapData).Size;
          }
        }

        // keep looking for groups whose total can be computed, until all groups have been processed
        while (groups.Count > 0) {
          var grps = new HashSet<Group>();

          foreach (var g in groups) {
            // for a group all of whose member nodes have an initialized total
            if (g.MemberParts.All(m => { return !(m is Group) || (m.Data as ITreeMapData).Total >= 0; })) {
              // compute the group's total as the sum of the sizes of all of the member nodes
              (g.Data as ITreeMapData).Total = 0;
              foreach (var m in g.MemberParts) {
                if (m is Node) {
                  (g.Data as ITreeMapData).Total += (m.Data as ITreeMapData).Total;
                }
              }
            } else {
              grps.Add(g);
            }
          }
          groups = grps;
        }
      }
    }
  }
}
