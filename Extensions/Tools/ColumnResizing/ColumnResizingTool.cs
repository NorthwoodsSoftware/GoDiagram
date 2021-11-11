using System;

/*
*  Copyright (C) 1998-2020 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main GoJS library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in the GoJS kit under the extensions or extensionsTS folders.
* See the Extensions intro page (https://gojs.Net/latest/intro/extensions.Html) for more information.
*/

namespace Northwoods.Go.Tools.Extensions {

  /// <summary>
  /// The ColumnResizingTool class lets the user resize each column of a named Table Panel in a selected Part.
  ///
  /// If you want to experiment with this extension, try the <a href="../../extensionsTS/ColumnResizing.Html">Column Resizing</a> sample.
  /// </summary>
  /// @category Tool Extension
  public class ColumnResizingTool : Tool {
    private Shape _HandleArchetype;
    private string _TableName = "TABLE";

    // internal state
    private GraphObject _Handle = null;
    private Panel _AdornedTable = null;

    /// <summary>
    /// Constructs a ColumnResizingTool and sets the handle and name of the tool.
    /// </summary>
    public ColumnResizingTool() : base() {
      var h = new Shape();
      h.GeometryString = "M0 0 V14 M2 0 V14";
      h.DesiredSize = new Size(2, 14);
      h.Cursor = "col-resize";
      h.GeometryStretch = GeometryStretch.None;
      h.Background = "rgba(255,255,255,0.5)";
      h.Stroke = "rgba(30,144,255,0.5)";
      _HandleArchetype = h;
      Name = "ColumnResizing";
    }

    /// <summary>
    /// Gets or sets small GraphObject that is copied as a resize handle for each column.
    /// This tool expects that this object"s <see cref="GraphObject.DesiredSize"/> (a.K.a width and height) has been set to real numbers.
    ///
    /// The default value is a <see cref="Shape"/> that is a narrow rectangle.
    /// </summary>
    public Shape HandleArchetype {
      get {
        return _HandleArchetype;
      }
      set {
        _HandleArchetype = value;
      }
    }

    /// <summary>
    /// Gets or sets the name of the Table Panel to be resized.
    ///
    /// The default value is the name "TABLE".
    /// </summary>
    public string TableName {
      get {
        return _TableName;
      }
      set {
        _TableName = value;
      }
    }

    /// <summary>
    /// This read-only property returns the <see cref="GraphObject"/> that is the tool handle being dragged by the user.
    /// This will be contained by an <see cref="Adornment"/> whose category is "ColumnResizing".
    /// Its <see cref="Adornment.AdornedElement"/> is the same as the <see cref="AdornedTable"/>.
    /// </summary>
    public GraphObject Handle {
      get {
        return _Handle;
      }
    }

    /// <summary>
    /// This read-only property returns the <see cref="Panel"/> of type <see cref="PanelLayoutTable.Instance"/> whose columns are being resized.
    /// This must be contained within the selected <see cref="Part"/>.
    /// </summary>
    public Panel AdornedTable {
      get {
        return _AdornedTable;
      }
    }

    /// <summary>
    /// Show an <see cref="Adornment"/> with a resize handle at each column.
    /// Don"t show anything if <see cref="TableName"/> doesn"t identify a <see cref="Panel"/>
    /// that has a <see cref="Panel.Type"/> of type <see cref="PanelLayoutTable.Instance"/>.
    /// </summary>
    public override void UpdateAdornments(Part part) {
      if ((part == null) || (part is Link)) return;  // this tool never applies to Links
      if (part.IsSelected && !Diagram.IsReadOnly) {
        var selelt = part.FindElement(TableName);
        if (selelt is Panel && selelt.ActualBounds.IsReal() && selelt.IsVisibleElement() &&
          part.ActualBounds.IsReal() && part.IsVisible() &&
          ((Panel)selelt).Type == PanelLayoutTable.Instance) {
          var table = selelt as Panel;
          var adornment = part.FindAdornment(Name);
          if (adornment == null) {
            adornment = MakeAdornment(table);
            part.AddAdornment(Name, adornment);
          }
          if (adornment != null) {
            var pad = (Margin)table.Padding;
            var numcols = table.ColumnCount;
            // update the position/alignment of each handle
            foreach (var h in adornment.Elements) {
              if (!h.Pickable) continue;
              var coldef = table.GetColumnDefinition(h.Column);
              var wid = coldef.ActualWidth;
              if (wid > 0) wid = coldef.TotalWidth;
              var sep = 0d;
              // find next non-zero-width column"s separatorStrokeWidth
              var idx = h.Column + 1;
              while ((idx < numcols) && (table.GetColumnDefinition(idx).ActualWidth == 0)) idx++;
              if (idx < numcols) {
                sep = table.GetColumnDefinition(idx).SeparatorStrokeWidth;
                if (double.IsNaN(sep)) sep = table.DefaultColumnSeparatorStrokeWidth;
              }
              h.Alignment = new Spot(0, 0, pad.Left + coldef.ActualX + wid + (sep / 2), pad.Top + (h.Height / 2));
            }
            adornment.LocationElement.DesiredSize = table.ActualBounds.Size;
            adornment.Location = table.GetDocumentPoint(adornment.LocationSpot);
            adornment.Angle = table.GetDocumentAngle();
            return;
          }
        }
      }
      part.RemoveAdornment(Name);
    }

    /// <summary>
    /// Undocumented.
    /// </summary>
    [Undocumented]
    public virtual Adornment MakeAdornment(Panel table) {
      // the Adornment is a Spot Panel holding resize handles
      var adornment = new Adornment();
      adornment.Category = Name;

      adornment.AdornedElement = table;

      adornment.Type = PanelLayoutSpot.Instance;

      adornment.LocationElementName = "BLOCK";
      // create the "main" element of the Spot Panel
      var block = new TextBlock();  // doesn"t matter much what this is

      block.Name = "BLOCK";

      block.Pickable = false;  // it"s transparent and not pickable

      adornment.Add(block);
      // now add resize handles for each column
      for (var i = 0; i < table.ColumnCount; i++) {
        var coldef = table.GetColumnDefinition(i);
        var h = MakeHandle(table, coldef);
        if (h != null) adornment.Add(h);
      }
      return adornment;
    }

    /// <summary>
    /// Undocumented.
    /// </summary>
    [Undocumented]
    public virtual GraphObject MakeHandle(Panel table, ColumnDefinition coldef) {
      var h = HandleArchetype;
      if (h == null) return null;
      var c = h.Copy();
      c.Column = coldef.Column;
      return c;
    }


    /// <summary>
    /// This tool may run when there is a mouse-down event on a "ColumnResizing" handle,
    /// the diagram is not read-only, the left mouse button is being used,
    /// and this tool"s adornment"s resize handle is at the current mouse point.
    /// </summary>
    public override bool CanStart() {
      if (!IsEnabled) return false;

      var diagram = Diagram;
      if (diagram.IsReadOnly) return false;
      if (!diagram.LastInput.Left) return false;
      var h = FindToolHandleAt(diagram.FirstInput.DocumentPoint, Name);
      return (h != null);
    }

    /// <summary>
    /// Find the <see cref="Handle"/>, ensure type <see cref="PanelLayoutTable.Instance"/>, capture the mouse, and start a transaction.
    ///
    /// If the call to <see cref="Tool.FindToolHandleAt"/> finds no "ColumnResizing" tool handle, this method returns without activating this tool.
    /// </summary>
    public override void DoActivate() {
      var diagram = Diagram;
      _Handle = FindToolHandleAt(diagram.FirstInput.DocumentPoint, Name);
      if (Handle == null) return;
      var panel = ((Handle.Part as Adornment).AdornedElement) as Panel;
      if ((panel == null) || (panel.Type != PanelLayoutTable.Instance)) return;
      _AdornedTable = panel;
      diagram.IsMouseCaptured = true;
      StartTransaction(Name);
      IsActive = true;
    }

    /// <summary>
    /// Stop the current transaction and release the mouse.
    /// </summary>
    public override void DoDeactivate() {
      StopTransaction();
      _Handle = null;
      _AdornedTable = null;
      var diagram = Diagram;
      diagram.IsMouseCaptured = false;
      IsActive = false;
    }

    /// <summary>
    /// Call <see cref="Resize"/> with a new size determined by the current mouse point.
    /// This determines the new bounds by calling <see cref="ComputeResize"/>.
    /// </summary>
    public override void DoMouseMove() {
      var diagram = Diagram;
      if (IsActive) {
        var newpt = ComputeResize(diagram.LastInput.DocumentPoint);
        Resize(newpt);
      }
    }

    /// <summary>
    /// Call <see cref="Resize"/> with the final bounds based on the most recent mouse point, and commit the transaction.
    /// This determines the new bounds by calling <see cref="ComputeResize"/>.
    /// </summary>
    public override void DoMouseUp() {
      var diagram = Diagram;
      if (IsActive) {
        var newpt = ComputeResize(diagram.LastInput.DocumentPoint);
        Resize(newpt);
        TransactionResult = Name;  // success
      }
      StopTool();
    }

    /// <summary>
    /// Change the <see cref="ColumnDefinition.Width"/> of the column being resized
    /// to a value corresponding to the given mouse point.
    /// </summary>
    /// <param name="newPoint">the value returned by the call to <see cref="ComputeResize"/></param>
    public void Resize(Point newPoint) {
      var table = AdornedTable;
      if (table == null) return;
      var h = Handle;
      if (h == null) return;
      var pad = (Margin)table.Padding;
      var numcols = table.ColumnCount;
      var locpt = table.GetLocalPoint(newPoint);
      var coldef = table.GetColumnDefinition(h.Column);
      var sep = 0d;
      var idx = h.Column + 1;
      while (idx < numcols && table.GetColumnDefinition(idx).ActualWidth == 0) idx++;
      if (idx < numcols) {
        sep = table.GetColumnDefinition(idx).SeparatorStrokeWidth;
        if (double.IsNaN(sep)) sep = table.DefaultColumnSeparatorStrokeWidth;
      }
      coldef.Width = Math.Max(0, locpt.X - pad.Left - coldef.ActualX - (coldef.TotalWidth - coldef.ActualWidth) - sep / 2);
    }


    /// <summary>
    /// This can be overridden in order to customize the resizing process.
    /// </summary>
    /// @expose
    /// <param name="p">the point where the handle is being dragged</param>
    /// <returns></returns>
    public Point ComputeResize(Point p) {
      return p;
    }

    /// <summary>
    /// Pressing the Delete key removes any column width setting and stops this tool.
    /// </summary>
    public override void DoKeyDown() {
      if (!IsActive) return;
      var e = Diagram.LastInput;
      if (e.Key == "DELETE") {  // remove width setting
        if ((AdornedTable != null) && (Handle != null)) {
          var coldef = AdornedTable.GetColumnDefinition(Handle.Column);
          coldef.Width = double.NaN;
          TransactionResult = Name;  // success
          StopTool();
          return;
        }
      }
      base.DoKeyDown();
    }
  }
}
