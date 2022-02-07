/*
*  Copyright (C) 1998-2022 by Northwoods Software Corporation. All Rights Reserved.
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
using System.Linq;

namespace Northwoods.Go.Layouts.Extensions {
  [Undocumented]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
  public class DefaultDict<TKey, TValue> : Dictionary<TKey, TValue> where TValue : class {
    public new TValue this[TKey key] {
      get {
        return TryGetValue(key, out var value) ? value : null;
      }
      set {
        base[key] = value;
      }
    }
  }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

  /// <summary>
  /// This <see cref="Layout"/> positions non-Link Parts into a table according to the values of
  /// <see cref="GraphObject.Row"/>, <see cref="GraphObject.Column"/>, <see cref="GraphObject.RowSpan"/>, <see cref="GraphObject.ColumnSpan"/>,
  /// <see cref="GraphObject.Alignment"/>, <see cref="GraphObject.Stretch"/>.
  /// </summary>
  /// <remarks>
  /// If the value of GraphObject.Stretch is not <see cref="Stretch.None"/>, the Part will be sized
  /// according to the available space in the cell(s).
  ///
  /// You can specify constraints for whole rows or columns by calling
  /// <see cref="GetRowDefinition(int)"/> or <see cref="GetColumnDefinition(int)"/> and setting one of the following properties:
  /// <see cref="RowDefinition.Alignment"/>, <see cref="RowDefinition.Height"/>,
  /// <see cref="RowDefinition.MaxHeight"/>, <see cref="RowDefinition.MinHeight"/> (or their column equivalents), <see cref="RowDefinition.Stretch"/>,
  /// or in bulk by calling <see cref="Add(RowDefinition[])"/>.
  ///
  /// The <see cref="DefaultAlignment"/> and <see cref="DefaultStretch"/> properties apply to all parts if not specified
  /// on the individual Part or in the corresponding row or column definition.
  ///
  /// At the current time, there is no support for separator lines
  /// (<see cref="RowDefinition.SeparatorStroke"/>, <see cref="RowDefinition.SeparatorStrokeWidth"/>,
  /// and <see cref="RowDefinition.SeparatorDashArray"/> properties)
  /// nor background (<see cref="RowDefinition.Background"/> and <see cref="RowDefinition.CoversSeparators"/> properties).
  /// There is no support for <see cref="RowDefinition.Sizing"/>, either.
  ///
  /// If you want to experiment with this extension, try the <a href="../../extensions/Table.html">Table Layout</a> sample.
  /// </remarks>
  /// @category Layout Extension
  public class TableLayout : Layout {
    private Spot _DefaultAlignment = Spot.Default;
    private Stretch _DefaultStretch = Stretch.Default;

    private readonly DefaultDict<int, RowDefinition> _RowDefs = new DefaultDict<int, RowDefinition>();
    private readonly DefaultDict<int, ColumnDefinition> _ColDefs = new DefaultDict<int, ColumnDefinition>();

    /// <summary>
    /// Create a Table layout.
    /// </summary>
    public TableLayout() : base() { }

    /// <summary>
    /// Copies properties to a cloned Layout.
    /// </summary>
    [Undocumented]
    protected override void CloneProtected(Layout c) {
      if (c == null) return;

      base.CloneProtected(c);
      var copy = (TableLayout)c;
      copy._DefaultAlignment = _DefaultAlignment;
      copy._DefaultStretch = _DefaultStretch;

      foreach (var def in _RowDefs) {
        copy._RowDefs.Add(def.Key, def.Value?.Copy());
      }
      foreach (var def in _ColDefs) {
        copy._ColDefs.Add(def.Key, def.Value?.Copy());
      }
    }

    /// <summary>
    /// Adds a number of RowDefinitions to this TableLayout.
    /// </summary>
    public TableLayout Add(params RowDefinition[] rows) {
      if (rows == null) Trace.Error("Can't add null to a TableLayout");
      foreach (var r in rows) {
        var def = GetRowDefinition(r.Row);
        if (def != null) {
          def.CopyFrom(r);
        }
      }
      return this;
    }

    /// <summary>
    /// Adds a number of RowDefinitions to this TableLayout.
    /// </summary>
    public TableLayout Add(IEnumerable<RowDefinition> rows) {
      if (rows == null) Trace.Error("Can't add null to a TableLayout");
      foreach (var r in rows) {
        var def = GetRowDefinition(r.Row);
        if (def != null) {
          def.CopyFrom(r);
        }
      }
      return this;
    }

    /// <summary>
    /// Adds a number of ColumnDefinitions to this TableLayout.
    /// </summary>
    public TableLayout Add(params ColumnDefinition[] cols) {
      if (cols == null) Trace.Error("Can't add null to a TableLayout");
      foreach (var c in cols) {
        var def = GetColumnDefinition(c.Column);
        if (def != null) {
          def.CopyFrom(c);
        }
      }
      return this;
    }

    /// <summary>
    /// Adds a number of ColumnDefinitions to this TableLayout.
    /// </summary>
    public TableLayout Add(IEnumerable<ColumnDefinition> cols) {
      if (cols == null) Trace.Error("Can't add null to a TableLayout");
      foreach (var c in cols) {
        var def = GetColumnDefinition(c.Column);
        if (def != null) {
          def.CopyFrom(c);
        }
      }
      return this;
    }

    /// <summary>
    /// Gets or sets the alignment to use by default for Parts in rows (vertically) and in columns (horizontally).
    /// </summary>
    /// <remarks>
    /// The default value is <see cref="Spot.Default"/>.
    /// Setting this property does not raise any events.
    /// </remarks>
    public Spot DefaultAlignment {
      get {
        return _DefaultAlignment;
      }
      set {
        if (_DefaultAlignment != value) {
          _DefaultAlignment = value;
        }
      }
    }

    /// <summary>
    /// Gets or sets whether Parts should be stretched in rows (vertically) and in columns (horizontally).
    /// </summary>
    /// <remarks>
    /// The default value is <see cref="Stretch.Default"/>.
    /// Setting this property does not raise any events.
    /// </remarks>
    public Stretch DefaultStretch {
      get {
        return _DefaultStretch;
      }
      set {
        if (_DefaultStretch != value) {
          _DefaultStretch = value;
        }
      }
    }

    /// <summary>
    /// This read-only property returns the number of rows in this TableLayout.
    /// </summary>
    /// <remarks>
    /// This value is only valid after the layout has been performed.
    /// </remarks>
    public int RowCount {
      get {
        return 1 + _RowDefs.Keys.Max();
      }
    }

    /// <summary>
    /// This read-only property returns the number of columns in this TableLayout.
    /// </summary>
    /// <remarks>
    /// This value is only valid after the layout has been performed.
    /// </remarks>
    public int ColumnCount {
      get {
        return 1 + _ColDefs.Keys.Max();
      }
    }

    /// <summary>
    /// Gets the <see cref="RowDefinition"/> for a particular row in this TableLayout.
    /// </summary>
    /// <remarks>
    /// If you ask for the definition of a row at or beyond the <see cref="RowCount" />,
    /// it will automatically create one and return it.
    /// </remarks>
    /// <param name="idx">The non-negative zero-based integer row index.</param>
    /// <returns><see cref="RowDefinition"/></returns>
    public RowDefinition GetRowDefinition(int idx) {
      if (idx < 0) throw new Exception("Row index must be non-negative, not: " + idx);
      var defs = _RowDefs;

      if (!defs.TryGetValue(idx, out var rcd)) {
        rcd = new RowDefinition {
          Row = idx
          // .Panel remains null
        };
        defs[idx] = rcd;
      }
      return rcd;
    }

    /// <summary>
    /// Gets the <see cref="ColumnDefinition"/> for a particular column in this TableLayout.
    /// </summary>
    /// <remarks>
    /// If you ask for the definition of a column at or beyond the <see cref="ColumnCount" />,
    /// it will automatically create one and return it.
    /// </remarks>
    /// <param name="idx">The non-negative zero-based integer column index.</param>
    /// <returns><see cref="ColumnDefinition"/></returns>
    public ColumnDefinition GetColumnDefinition(int idx) {
      if (idx < 0) throw new Exception("Column index must be non-negative, not: " + idx);

      var defs = _ColDefs;

      if (!defs.TryGetValue(idx, out var rcd)) {
        rcd = new ColumnDefinition {
          Column = idx
          // .Panel remains null
        };
        defs[idx] = rcd;
      }
      return rcd;
    }

    /// <summary>
    /// Returns the row at a given y-coordinate in document coordinates.
    /// </summary>
    /// <remarks>
    /// This information is only valid when this layout has been performed and <see cref="Layout.IsValidLayout"/> is true.
    ///
    /// If the point is above row 0, this method returns -1.
    /// If the point is below the last row, this returns the last row + 1.
    /// </remarks>
    /// <param name="y"></param>
    /// <returns>a zero-based integer</returns>
    /// <seealso cref="FindColumnForDocumentX" />
    public int FindRowForDocumentY(double y) {
      y -= ArrangementOrigin.Y;
      if (y < 0) return -1;

      var total = 0.0;

      foreach (var rowdef in _RowDefs) {
        if (rowdef.Value == null) continue;
        total += rowdef.Value.TotalHeight;
        if (y < total) return rowdef.Key;
      }
      return 1 + _RowDefs.Keys.Max();
    }

    /// <summary>
    /// Returns the column at a given x-coordinate in document coordinates.
    /// </summary>
    /// <remarks>
    /// This information is only valid when this layout has been performed and <see cref="Layout.IsValidLayout"/> is true.
    ///
    /// If the point is to the left of column 0, this method returns -1.
    /// If the point is to the right of the last column, this returns the last column + 1.
    /// </remarks>
    /// <param name="x"></param>
    /// <returns>a zero-based integer</returns>
    /// <seealso cref="FindRowForDocumentY" />
    public int FindColumnForDocumentX(double x) {
      x -= ArrangementOrigin.X;
      if (x < 0) return -1;

      var total = 0.0;

      foreach (var coldef in _ColDefs) {
        if (coldef.Value == null) continue;
        total += coldef.Value.TotalWidth;
        if (x < total) return coldef.Key;
      }
      return 1 + _ColDefs.Keys.Max();
    }

    /// <summary>
    /// Assign the positions of the parts.
    /// </summary>
    public override void DoLayout(IEnumerable<Part> coll = null) {
      var diagram = Diagram;
      HashSet<Part> allparts;
      if (coll != null) {
        allparts = CollectParts(coll);
      } else if (Group != null) {
        allparts = CollectParts(Group);
      } else if (diagram != null) {
        allparts = CollectParts(diagram);
      } else {
        return; // Nothing to layout!
      }
      ArrangementOrigin = InitialOrigin(ArrangementOrigin);


      // put all eligible Parts that are not Links into an Array
      var parts = new List<Part>();
      foreach (var part in allparts) {
        if (!part.IsLinkLike()) parts.Add(part);
      }

      if (Diagram != null) {
        Diagram.StartTransaction("Table Layout");
        var union = new Size();
        // this calls .BeforeMeasure(parts, rowcol)
        var rowcol = MeasureTable(int.MaxValue, int.MaxValue, parts, union, 0, 0);

        ArrangeTable(parts, union, rowcol);
        AfterArrange(parts, rowcol);

        Diagram.CommitTransaction("Table Layout");
      }
    }

    /// <summary>
    /// Override this method in order to perform some operations before measuring.
    /// By default this method does nothing.
    /// </summary>
    protected virtual void BeforeMeasure(List<Part> parts, DefaultDict<int, DefaultDict<int, List<Part>>> rowcol) { }

    /// <summary>
    /// Override this method in order to perform some operations after arranging.
    /// By default this method does nothing.
    /// </summary>
    protected virtual void AfterArrange(List<Part> parts, DefaultDict<int, DefaultDict<int, List<Part>>> rowcol) { }

    private DefaultDict<int, DefaultDict<int, List<Part>>> MeasureTable(int width, int height, List<Part> children, Size union, int minw, int minh) {
      var l = children.Count();
      var rowcol = new DefaultDict<int, DefaultDict<int, List<Part>>>();

      for (var c = 0; c < l; c++) {
        var child = children[c];

        if (rowcol[child.Row] == null) {
          rowcol[child.Row] = new DefaultDict<int, List<Part>>();
        }

        if (rowcol[child.Row][child.Column] == null) {
          rowcol[child.Row][child.Column] = new List<Part>();
        }

        rowcol[child.Row][child.Column].Add(child);
      }

      BeforeMeasure(children, rowcol);

      // spanners/nosize/cols/rows here
      var spanners = new List<Part>();
      var nosize = new List<Part>();

      double colleft = width;
      double rowleft = height;

      var rdefs = _RowDefs;
      foreach (var def in rdefs.Values) {
        if (def != null) def.ActualHeight = 0;
      }

      var cdefs = _ColDefs;
      foreach (var def in cdefs.Values) {
        if (def != null) def.ActualWidth = 0;
      }

      var lcol = 0;
      foreach (var row in rowcol.Values) {
        if (row != null) lcol = Math.Max(lcol, 1 + row.Keys.Max());  // column length in this row
      }

      var nosizeCols = new double[lcol];
      var nosizeRows = new double[1 + rowcol.Keys.Max()];

      // Reset the row/col definitions because the ones from last measure are irrelevant
      var resetCols = new bool[lcol];

      var nosizeColsCount = 0;
      var nosizeRowsCount = 0;

      var amt = 0.0;
      foreach (var rowpair in rowcol) {
        var rowIdx = rowpair.Key;
        var row = rowpair.Value;
        if (row == null) continue;

        lcol = 1 + row.Keys.Max();
        var rowHerald = GetRowDefinition(rowIdx);
        rowHerald.ActualHeight = 0;  // Reset rows (only on first pass)

        foreach (var colpair in row) {
          var colIdx = colpair.Key;
          var col = colpair.Value;
          if (col == null) continue;

          var colHerald = GetColumnDefinition(colIdx);

          if (!resetCols[colIdx]) {
            colHerald.ActualWidth = 0;
            resetCols[colIdx] = true;
          }

          var cell = rowcol[rowIdx][colIdx];
          // foreach element in cell, measure
          foreach (var child in cell) {
            // Skip children that span multiple rows/columns or have no set size
            var spanner = false;
            if (child.RowSpan > 1 || child.ColumnSpan > 1) {
              spanner = true;
              spanners.Add(child);
              // We used to not measure spanners twice, but now we do
              // The reason is that there may be a row whose size
              // is dictated by an object with columnSpan 2+ and vice versa

              // continue;
            }

            var marg = child.Margin;
            var margw = marg.Right + marg.Left;
            var margh = marg.Top + marg.Bottom;

            var stretch = GetEffectiveTableStretch(child, rowHerald, colHerald);
            var dsize = child.ResizeElement.DesiredSize;
            var realwidth = !(double.IsNaN(dsize.Width));
            var realheight = !(double.IsNaN(dsize.Height));
            var realsize = realwidth && realheight;

            if (!spanner && stretch != Stretch.None && !realsize) {
              if (nosizeCols[colIdx] == 0 && (stretch == Stretch.Fill || stretch == Stretch.Horizontal)) {
                nosizeCols[colIdx] = -1;
                nosizeColsCount++;
              }
              if (nosizeRows[rowIdx] == 0 && (stretch == Stretch.Fill || stretch == Stretch.Vertical)) {
                nosizeRows[rowIdx] = -1;
                nosizeRowsCount++;
              }
              nosize.Add(child);
            }

            if (stretch != Stretch.None) {
              var unrestrictedsize = new Size(double.NaN, double.NaN);

              // ??? allow resizing during measure phase
              child.ResizeElement.DesiredSize = unrestrictedsize;
              child.EnsureBounds();
            }

            var m = GetLayoutBounds(child);
            var mwidth = Math.Max(m.Width + margw, 0);
            var mheight = Math.Max(m.Height + margh, 0);

            // Make sure the heralds have the right layout size
            // the row/column should use the largest measured size of any
            // GraphObject contained, constrained by mins and maxes
            if (child.RowSpan == 1 && (realheight || stretch == Stretch.None || stretch == Stretch.Horizontal)) {
              var def = GetRowDefinition(rowIdx);
              amt = Math.Max(mheight - def.ActualHeight, 0);
              if (amt > rowleft) amt = rowleft;
              def.ActualHeight += amt;
              rowleft = Math.Max(rowleft - amt, 0);
            }

            if (child.ColumnSpan == 1 && (realheight || stretch == Stretch.None || stretch == Stretch.Vertical)) {
              var def = GetColumnDefinition(colIdx);
              amt = Math.Max(mwidth - def.ActualWidth, 0);
              if (amt > colleft) amt = colleft;
              def.ActualWidth += amt;
              colleft = Math.Max(colleft - amt, 0);
            }
          } // end cell
        } // end col
      } // end row

      // For objects of no desired size we allocate what is left as we go,
      // or else what is already in the column
      var totalColWidth = 0.0;
      var totalRowHeight = 0.0;

      foreach (var coldef in _ColDefs) {
        if (coldef.Value != null) totalColWidth += GetColumnDefinition(coldef.Key).ActualWidth;
      }

      foreach (var rowdef in _RowDefs) {
        if (rowdef.Value != null) totalRowHeight += GetRowDefinition(rowdef.Key).ActualHeight;
      }

      colleft = Math.Max(width - totalColWidth, 0);
      rowleft = Math.Max(height - totalRowHeight, 0);

      var originalrowleft = rowleft;
      var originalcolleft = colleft;

      foreach (var child in nosize) {
        var rowHerald = GetRowDefinition(child.Row);
        var colHerald = GetColumnDefinition(child.Column);

        var mb = GetLayoutBounds(child);
        var marg = child.Margin;
        var margw = marg.Right + marg.Left;
        var margh = marg.Top + marg.Bottom;

        if (colHerald.ActualWidth == 0 && nosizeCols[child.Column] != 0) {
          nosizeCols[child.Column] = Math.Max(mb.Width + margw, nosizeCols[child.Column]);
        } else {
          nosizeCols[child.Column] = 0; // obey the column herald
        }

        if (rowHerald.ActualHeight == 0 && nosizeRows[child.Row] != 0) {
          nosizeRows[child.Row] = Math.Max(mb.Height + margh, nosizeRows[child.Row]);
        } else {
          nosizeRows[child.Row] = 0; // obey the row herald
        }
      }

      // we now have the size that all these columns prefer to be
      // we also have the amount left over
      var desiredRowTotal = nosizeRows.Sum();
      var desiredColTotal = nosizeCols.Sum();

      var allowedSize = new Size();

      // Deal with objects that have a stretch
      foreach (var child in nosize) {
        var rowHerald = GetRowDefinition(child.Row);
        var colHerald = GetColumnDefinition(child.Column);

        var w = 0.0;
        if (Util.IsFinite(colHerald.Width)) {
          w = colHerald.Width;
        } else {
          if (Util.IsFinite(colleft) && nosizeCols[child.Column] != 0) {
            if (desiredColTotal == 0) w = colHerald.ActualWidth + colleft;
            else w = ((nosizeCols[child.Column] / desiredColTotal) * originalcolleft);
          } else {
            // Only use colHerald.Actual if it was nonzero before this loop
            if (nosizeCols[child.Column] != 0) w = colleft;
            else w = (colHerald.ActualWidth == 0) ? colleft : colHerald.ActualWidth;
          }
          w = Math.Max(0, w - colHerald.ComputeEffectiveSpacing());
        }

        var h = 0.0;
        if (Util.IsFinite(rowHerald.Height)) {
          w = rowHerald.Height;
        } else {
          if (Util.IsFinite(rowleft) && nosizeRows[child.Row] != 0) {
            if (desiredRowTotal == 0) h = rowHerald.ActualHeight + rowleft;
            else h = ((nosizeRows[child.Row] / desiredRowTotal) * originalrowleft);
          } else {
            // Only use colHerald.Actual if it was nonzero before this loop
            if (nosizeRows[child.Row] != 0) h = rowleft;
            else h = (rowHerald.ActualHeight == 0) ? rowleft : rowHerald.ActualHeight;
          }
          h = Math.Max(0, h - rowHerald.ComputeEffectiveSpacing());
        }

        allowedSize.Width = Math.Max(colHerald.MinWidth, Math.Min(w, colHerald.MaxWidth));
        allowedSize.Height = Math.Max(rowHerald.MinHeight, Math.Min(h, rowHerald.MaxHeight));

        var stretch = GetEffectiveTableStretch(child, rowHerald, colHerald);

        switch (stretch) {
          case Stretch.Horizontal:
            allowedSize.Height = Math.Max(allowedSize.Height, rowHerald.ActualHeight + rowleft);
            break;
          case Stretch.Vertical:
            allowedSize.Width = Math.Max(allowedSize.Width, colHerald.ActualWidth + colleft);
            break;
        }

        var marg = child.Margin;
        var margw = marg.Right + marg.Left;
        var margh = marg.Top + marg.Bottom;

        var m = GetLayoutBounds(child);
        var mwidth = Math.Max(m.Width + margw, 0);
        var mheight = Math.Max(m.Height + margh, 0);
        if (Util.IsFinite(colleft)) mwidth = Math.Min(mwidth, allowedSize.Width);
        if (Util.IsFinite(rowleft)) mheight = Math.Min(mheight, allowedSize.Height);

        var oldAmount = 0.0;

        oldAmount = rowHerald.ActualHeight;
        rowHerald.ActualHeight = Math.Max(rowHerald.ActualHeight, mheight);
        amt = rowHerald.ActualHeight - oldAmount;
        rowleft = Math.Max(rowleft - amt, 0);

        oldAmount = colHerald.ActualWidth;
        colHerald.ActualWidth = Math.Max(colHerald.ActualWidth, mwidth);
        amt = colHerald.ActualWidth - oldAmount;
        colleft = Math.Max(colleft - amt, 0);
      }  // end no fixed size objects

      foreach (var row in rowcol.Values) {
        if (row == null) continue;
        lcol = Math.Max(lcol, 1 + row.Keys.Max());  // column length in this row
      }

      // Go through each object that spans multiple rows or columns
      var additionalSpan = new Size();
      var actualSizeRows = new double[1 + rowcol.Keys.Max()];
      var actualSizeColumns = new double[lcol];
      l = spanners.Count();
      if (l != 0) {
        // record the actual sizes of every row/column before measuring spanners
        // because they will change during the loop and we want to use their 'before' values
        foreach (var rowpair in rowcol) {
          var ix = rowpair.Key;
          var row = rowpair.Value;
          if (row == null) continue;

          lcol = 1 + row.Keys.Max();  // column length in this row
          var rowHerald = GetRowDefinition(ix);
          actualSizeRows[ix] = rowHerald.ActualHeight;
          for (var j = 0; j < lcol; j++) {
            // foreach column j in row i...
            if (rowcol[ix][j] == null) continue;
            var colHerald = GetColumnDefinition(j);
            actualSizeColumns[j] = colHerald.ActualWidth;
          }
        }
      }

      foreach (var child in spanners) {
        var rowHerald = GetRowDefinition(child.Row);
        var colHerald = GetColumnDefinition(child.Column);

        // If there's a set column width/height we don't care about the given width/height
        allowedSize.Width = Math.Max(colHerald.MinWidth, Math.Min(width, colHerald.MaxWidth));
        allowedSize.Height = Math.Max(rowHerald.MinHeight, Math.Min(height, rowHerald.MaxHeight));

        // If it is a spanner and has a fill:
        var stretch = GetEffectiveTableStretch(child, rowHerald, colHerald);
        switch (stretch) {
          case Stretch.Fill:
            if (actualSizeColumns[colHerald.Column] != 0) allowedSize.Width = Math.Min(allowedSize.Width, actualSizeColumns[colHerald.Column]);
            if (actualSizeRows[rowHerald.Row] != 0) allowedSize.Height = Math.Min(allowedSize.Height, actualSizeRows[rowHerald.Row]);
            break;
          case Stretch.Horizontal:
            if (actualSizeColumns[colHerald.Column] != 0) allowedSize.Width = Math.Min(allowedSize.Width, actualSizeColumns[colHerald.Column]);
            break;
          case Stretch.Vertical:
            if (actualSizeRows[rowHerald.Row] != 0) allowedSize.Height = Math.Min(allowedSize.Height, actualSizeRows[rowHerald.Row]);
            break;
        }

        if (Util.IsFinite(colHerald.Width)) allowedSize.Width = colHerald.Width;
        if (Util.IsFinite(rowHerald.Height)) allowedSize.Height = rowHerald.Height;

        var rdefspan = GetRowDefinition(child.Row);
        additionalSpan.Height = 0;

        for (var n = 1; n < child.RowSpan; n++) {
          if (child.Row + n >= RowCount) break;  // if the row exists at all
          rdefspan = GetRowDefinition(child.Row + n);
          amt = 0;
          if (stretch == Stretch.Fill || stretch == Stretch.Vertical) {
            amt = Math.Max(rdefspan.MinHeight, (actualSizeRows[child.Row + n] == 0) ? rdefspan.MaxHeight : Math.Min(actualSizeRows[child.Row + n], rdefspan.MaxHeight));
          } else {
            amt = Math.Max(rdefspan.MinHeight, double.IsNaN(rdefspan.Height) ? rdefspan.MaxHeight : Math.Min(rdefspan.Height, rdefspan.MaxHeight));
          }
          additionalSpan.Height += amt;
        }

        var cdefspan = GetColumnDefinition(child.Column);
        additionalSpan.Width = 0;

        for (var n = 1; n < child.ColumnSpan; n++) {
          if (child.Column + n >= ColumnCount) break;  // if the column exists at all
          cdefspan = GetColumnDefinition(child.Column + n);
          amt = 0;
          if (stretch == Stretch.Fill || stretch == Stretch.Horizontal) {
            amt = Math.Max(cdefspan.MinWidth, (actualSizeColumns[child.Column + n] == 0) ? cdefspan.MaxWidth : Math.Min(actualSizeColumns[child.Column + n], cdefspan.MaxWidth));
          } else {
            amt = Math.Max(cdefspan.MinWidth, double.IsNaN(cdefspan.Width) ? cdefspan.MaxWidth : Math.Min(cdefspan.Width, cdefspan.MaxWidth));
          }
          additionalSpan.Width += amt;
        }

        allowedSize.Width += additionalSpan.Width;
        allowedSize.Height += additionalSpan.Height;

        var marg = child.Margin;
        var margw = marg.Right + marg.Left;
        var margh = marg.Top + marg.Bottom;
        var m = GetLayoutBounds(child);
        var mwidth = Math.Max(m.Width + margw, 0);
        var mheight = Math.Max(m.Height + margh, 0);

        var rdeftot = GetRowDefinition(child.Row);
        var totalRow = 0.0;
        for (var n = 0; n < child.RowSpan; n++) {
          if (child.Row + n >= RowCount) break;  // if the row exists at all
          rdeftot = GetRowDefinition(child.Row + n);
          totalRow += rdeftot.TotalHeight;
        }
        // def is the last row definition
        if (totalRow < mheight) {
          var roomLeft = mheight - totalRow;
          while (roomLeft > 0) {  // Add the extra to the first row that allows us to
            var act = rdeftot.ActualHeight;
            if (double.IsNaN(rdeftot.Height) && rdeftot.MaxHeight > act) {
              rdeftot.ActualHeight = Math.Min(rdeftot.MaxHeight, act + roomLeft);
              if (rdeftot.ActualHeight != act) roomLeft -= rdeftot.ActualHeight - act;
            }
            if (rdeftot.Row - 1 == -1) break;
            rdeftot = GetRowDefinition(rdeftot.Row - 1);
          }
        }

        var cdeftot = GetColumnDefinition(child.Column);
        var totalCol = 0.0;
        for (var n = 0; n < child.ColumnSpan; n++) {
          if (child.Column + n >= ColumnCount) break;  // if the col exists at all
          cdeftot = GetColumnDefinition(child.Column + n);
          totalCol += cdeftot.TotalWidth;
        }
        // def is the last col definition
        if (totalCol < mwidth) {
          var roomLeft = mwidth - totalCol;
          while (roomLeft > 0) {  // Add the extra to the first column that allows us to
            var act = cdeftot.ActualWidth;
            if (double.IsNaN(cdeftot.Width) && cdeftot.MaxWidth > act) {
              cdeftot.ActualWidth = Math.Min(cdeftot.MaxWidth, act + roomLeft);
              if (cdeftot.ActualWidth != act) roomLeft -= cdeftot.ActualWidth - act;
            }
            if (cdeftot.Column - 1 == -1) break;
            cdeftot = GetColumnDefinition(cdeftot.Column - 1);
          }
        }
      }  // end spanning objects

      l = ColumnCount;
      for (var j = 0; j < l; j++) {
        if (_ColDefs[j] == null) continue;
        var def = GetColumnDefinition(j);
        def.ActualX = union.Width;
        if (def.ActualWidth != 0) {
          union.Width += def.ActualWidth;
          union.Width += def.ComputeEffectiveSpacing();
        }
      }

      l = RowCount;
      for (var j = 0; j < l; j++) {
        if (_RowDefs[j] == null) continue;
        var def = GetRowDefinition(j);
        def.ActualY = union.Height;
        if (def.ActualHeight != 0) {
          union.Height += def.ActualHeight;
          union.Height += def.ComputeEffectiveSpacing();
        }
      }

      return rowcol;
    }

    /*
     * Only ever called from TableLayout's measure and arrange
     */
    private Stretch GetEffectiveTableStretch(Part child, RowDefinition rowHerald, ColumnDefinition colHerald) {
      var effectivestretch = child.Stretch;
      if (effectivestretch != Stretch.Default) return effectivestretch;

      //which directions are we stretching? false = default
      var horizontal = false;
      var vertical = false;
      switch (rowHerald.Stretch) {
        case Stretch.Default:
        case Stretch.Horizontal: break;
        case Stretch.Vertical: vertical = true; break;
        case Stretch.Fill: vertical = true; break;
      }
      switch (colHerald.Stretch) {
        case Stretch.Default:
        case Stretch.Vertical: break;
        case Stretch.Horizontal: horizontal = true; break;
        case Stretch.Fill: horizontal = true; break;
      }

      var str = DefaultStretch;
      if (horizontal == false && (str == Stretch.Horizontal || str == Stretch.Fill)) {
        horizontal = true;
      }
      if (vertical == false && (str == Stretch.Vertical || str == Stretch.Fill)) {
        vertical = true;
      }

      if (horizontal && vertical) return Stretch.Fill;
      if (horizontal) return Stretch.Horizontal;
      if (vertical) return Stretch.Vertical;
      return Stretch.None;  // Everything else is None by default
    }

    private void ArrangeTable(List<Part> children, Size union, DefaultDict<int, DefaultDict<int, List<Part>>> rowcol) {
      var l = children.Count();
      var origin = ArrangementOrigin;
      var x = 0.0;
      var y = 0.0;

      var lcol = 0;
      foreach (var row in rowcol.Values) {
        if (row == null) continue;
        lcol = Math.Max(lcol, 1 + row.Keys.Max());  // column length in this row
      }

      var additionalSpan = new Size();
      var i = -1;
      // Find cell space and arrange objects:
      foreach (var rowpair in rowcol) {
        i = rowpair.Key;
        var row = rowpair.Value;
        if (row == null) continue;
        lcol = 1 + row.Keys.Max();
        var rowHerald = GetRowDefinition(i);
        y = origin.Y + rowHerald.ActualY + rowHerald.ComputeEffectiveSpacingTop();

        var j = -1;
        foreach (var colpair in row) {
          j = colpair.Key;
          var col = colpair.Value;
          if (col == null) continue;
          var colHerald = GetColumnDefinition(j);
          x = origin.X + colHerald.ActualX + colHerald.ComputeEffectiveSpacingTop();

          var cell = rowcol[i][j];
          foreach (var child in cell) {
            // add to layoutWidth/Height any additional span
            additionalSpan.Width = 0;
            additionalSpan.Height = 0;

            for (var n = 1; n < child.RowSpan; n++) {
              //if the row exists at all
              if (i + n >= RowCount) break;
              additionalSpan.Height += GetRowDefinition(i + n).TotalHeight;
            }

            for (var n = 1; n < child.ColumnSpan; n++) {
              //if the column exists at all
              if (j + n >= ColumnCount) break;
              additionalSpan.Width += GetColumnDefinition(j + n).TotalWidth;
            }

            // Construct containing rect (cell):

            // total width and height of the cell that an object could possibly be created in
            var colwidth = colHerald.ActualWidth + additionalSpan.Width;
            var rowheight = rowHerald.ActualHeight + additionalSpan.Height;

            // construct a rect that represents the total cell size allowed for this object
            var ar = new Rect {
              X = x,
              Y = y,
              Width = colwidth,
              Height = rowheight
            };

            // Also keep them for clip values
            var cellx = x;
            var celly = y;
            var cellw = colwidth;
            var cellh = rowheight;
            // Ending rows/col might have actual spaces that are larger than the remaining space
            // Modify them for clipping regions
            if (x + colwidth > union.Width) cellw = Math.Max(union.Width - x, 0);
            if (y + rowheight > union.Height) cellh = Math.Max(union.Height - y, 0);

            // Construct alignment:
            var align = child.Alignment;
            var alignx = 0.0;
            var aligny = 0.0;
            var alignoffsetX = 0.0;
            var alignoffsetY = 0.0;
            if (align.IsDefault()) {
              align = DefaultAlignment;
              if (!align.IsSpot()) align = Spot.Center;
              alignx = align.X;
              aligny = align.Y;
              alignoffsetX = align.OffsetX;
              alignoffsetY = align.OffsetY;
              var ca = colHerald.Alignment;
              var ra = rowHerald.Alignment;
              if (ca.IsSpot()) {
                alignx = ca.X;
                alignoffsetX = ca.OffsetX;
              }
              if (ra.IsSpot()) {
                aligny = ra.Y;
                alignoffsetY = ra.OffsetY;
              }
            } else {
              alignx = align.X;
              aligny = align.Y;
              alignoffsetX = align.OffsetX;
              alignoffsetY = align.OffsetY;
            }

            // same as if (!align.IsSpot()) align = Spot.Center
            if (double.IsNaN(alignx) || double.IsNaN(aligny)) {
              alignx = 0.5;
              aligny = 0.5;
              alignoffsetX = 0;
              alignoffsetY = 0;
            }

            var width = 0.0;
            var height = 0.0;

            var marg = child.Margin;
            var margw = marg.Left + marg.Right;
            var margh = marg.Top + marg.Bottom;
            var stretch = GetEffectiveTableStretch(child, rowHerald, colHerald);
            if (stretch == Stretch.Fill || stretch == Stretch.Horizontal) {
              width = Math.Max(colwidth - margw, 0);
            } else {
              width = GetLayoutBounds(child).Width;
            }
            if (stretch == Stretch.Fill || stretch == Stretch.Vertical) {
              height = Math.Max(rowheight - margh, 0);
            } else {
              height = GetLayoutBounds(child).Height;
            }

            // min and max override any stretch values
            width = Math.Min(child.MaxSize.Width, width);
            height = Math.Min(child.MaxSize.Height, height);
            width = Math.Max(child.MinSize.Width, width);
            height = Math.Max(child.MinSize.Height, height);

            var widthmarg = width + margw;
            var heightmarg = height + margh;

            ar.X += (ar.Width * alignx) - (widthmarg * alignx) + alignoffsetX + marg.Left;
            ar.Y += (ar.Height * aligny) - (heightmarg * aligny) + alignoffsetY + marg.Top;

            child.MoveTo(ar.X, ar.Y);
            if (stretch != Stretch.None) {
              child.ResizeElement.DesiredSize = new Size(width, height);
            }
          } // end cell
        } // end col
      } // end row
    } // end arrangeTable
  } // end TableLayout class
} // end namespace declaration
