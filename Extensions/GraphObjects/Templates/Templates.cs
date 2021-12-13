/*
*  Copyright (C) 1998-2021 by Northwoods Software Corporation. All Rights Reserved.
*/

using System;
using System.Collections.Generic;
using Northwoods.Go.Models;
using Northwoods.Go.Tools;

/// These are the definitions for all of the predefined templates and tool archetypes.
/// You do not need to load this file in order to use the default templates and archetypes.
///
/// Although we have tried to provide definitions here that are faithful to how they
/// are actually implemented, there may be some differences from what is in the library.
///
/// Caution: these may change in a future version.

namespace Northwoods.Go.Extensions {
  public class Templates {
    /// Set up the default templates that each Diagram starts off with.
    public static void DefineDiagramTemplates(Diagram diagram) {
      // Node Templates
      var NodeTemplateMap = new Dictionary<string, Part>();

      // create the default Node template
      var archnode = new Node();
      var nodet = new TextBlock();
      nodet.Bind(new Binding("Text", "", ToString));
      archnode.Add(nodet);
      NodeTemplateMap[""] = archnode;

      // create the default Comment Node template
      var archcmnt = new Node();
      var nodec = new TextBlock();
      nodec.Stroke = "brown";
      nodec.Bind(new Binding("Text", "", ToString));
      archcmnt.Add(nodec);
      NodeTemplateMap["Comment"] = archcmnt;

      // create the default Link Label Node template
      var archllab = new Node();
      archllab.Selectable = false;
      archllab.Avoidable = false;
      var nodel = new Shape();
      nodel.Figure = "Ellipse";
      nodel.Fill = "black";
      nodel.Stroke = null;
      nodel.DesiredSize = new Size(3, 3);
      archllab.Add(nodel);
      NodeTemplateMap["LinkLabel"] = archllab;

      diagram.NodeTemplateMap = NodeTemplateMap;

      // Group Templates
      var GroupTemplateMap = new Dictionary<string, Group>();

      // create the default Group template
      var archgrp = new Group();
      archgrp.SelectionElementName = "GROUPPANEL";
      archgrp.Type = PanelLayoutVertical.Instance;
      var grpt = new TextBlock();
      grpt.Font = "bold 12pt sans-serif";
      grpt.Bind(new Binding("Text", "", ToString));
      archgrp.Add(grpt);
      var grppan = new Panel(PanelLayoutAuto.Instance);
      grppan.Name = "GROUPPANEL";
      var grpbord = new Shape();
      grpbord.Figure = "Rectangle";
      grpbord.Fill = "rgba(128,128,128,0.2)";
      grpbord.Stroke = "black";
      grppan.Add(grpbord);
      var phold = new Placeholder();
      phold.Padding = new Margin(5, 5, 5, 5);
      grppan.Add(phold);
      archgrp.Add(grppan);
      GroupTemplateMap[""] = archgrp;

      diagram.GroupTemplateMap = GroupTemplateMap;

      // Link Templates
      var LinkTemplateMap = new Dictionary<string, Link>();

      // create the default Link template
      var archlink = new Link();
      var archpath = new Shape();
      archpath.IsPanelMain = true;
      archlink.Add(archpath);
      var archarrow = new Shape();
      archarrow.ToArrow = "Standard";
      archarrow.Fill = "black";
      archarrow.Stroke = null;
      archarrow.StrokeWidth = 0;
      archlink.Add(archarrow);
      LinkTemplateMap[""] = archlink;

      // create the default Comment Link template
      var archcmntlink = new Link();
      var archcmntpath = new Shape();
      archcmntpath.IsPanelMain = true;
      archcmntpath.Stroke = "brown";
      archcmntlink.Add(archcmntpath);
      LinkTemplateMap["Comment"] = archcmntlink;

      diagram.LinkTemplateMap = LinkTemplateMap;
    }

    /// Set up the default Panel.ItemTemplate.
    public static void DefineDefaultItemTemplate(Panel panel) {
      var architem = new Panel();
      var itemtxt = new TextBlock();
      itemtxt.Bind(new Binding("Text", "", ToString));
      architem.Add(itemtxt);
      panel.ItemTemplate = architem;
    }

    /// Set up the diagram's selection Adornments
    public static void DefineSelectionAdornments(Diagram diagram) {
      // create the default Adornment for selection
      var selad = new Adornment();
      selad.Type = PanelLayoutAuto.Instance;
      var seladhandle = new Shape();
      seladhandle.Fill = null;
      seladhandle.Stroke = "dodgerblue";
      seladhandle.StrokeWidth = 3;
      selad.Add(seladhandle);
      var selplace = new Placeholder();
      selplace.Margin = new Margin(1.5, 1.5, 1.5, 1.5);
      selad.Add(selplace);
      diagram.NodeSelectionAdornmentTemplate = selad;

      // reuse the default Node Adornment for selection
      diagram.GroupSelectionAdornmentTemplate = selad;

      // create the default Link Adornment for selection
      selad = new Adornment();
      selad.Type = PanelLayoutLink.Instance;
      seladhandle = new Shape();
      seladhandle.IsPanelMain = true;
      seladhandle.Fill = null;
      seladhandle.Stroke = "dodgerblue";
      seladhandle.StrokeWidth = 3;  // ?? zero to use selection object's strokeWidth is often not wide enough
      selad.Add(seladhandle);
      diagram.LinkSelectionAdornmentTemplate = selad;
    }

    public static void DefineDefaultBackgroundGrid(Diagram diagram) {
      // make the background Grid Panel
      var Grid = new Panel(PanelLayoutGrid.Instance);
      Grid.Name = "GRID";

      var hlines = new Shape();
      hlines.Figure = "LineH";
      hlines.Stroke = "lightgray";
      hlines.StrokeWidth = 0.5;
      hlines.Interval = 1;
      Grid.Add(hlines);

      hlines = new Shape();
      hlines.Figure = "LineH";
      hlines.Stroke = "gray";
      hlines.StrokeWidth = 0.5;
      hlines.Interval = 5;
      Grid.Add(hlines);

      hlines = new Shape();
      hlines.Figure = "LineH";
      hlines.Stroke = "gray";
      hlines.StrokeWidth = 1;
      hlines.Interval = 10;
      Grid.Add(hlines);

      var vlines = new Shape();
      vlines.Figure = "LineV";
      vlines.Stroke = "lightgray";
      vlines.StrokeWidth = 0.5;
      vlines.Interval = 1;
      Grid.Add(vlines);

      vlines = new Shape();
      vlines.Figure = "LineV";
      vlines.Stroke = "gray";
      vlines.StrokeWidth = 0.5;
      vlines.Interval = 5;
      Grid.Add(vlines);

      vlines = new Shape();
      vlines.Figure = "LineV";
      vlines.Stroke = "gray";
      vlines.StrokeWidth = 1;
      vlines.Interval = 10;
      Grid.Add(vlines);

      Grid.Visible = false;  // grid is by default not visible

      // create the Part holding the Grid Panel
      //var gridpart = new Part();
      //gridpart.Add(Grid);
      //gridpart.LayerName = "Grid";  // goes in the "Grid" layer
      //gridpart.ZOrder = 0;  // to make it easier for other background parts to be behind the grid
      //gridpart.IsInDocumentBounds = false;  // never part of the document bounds
      //gridpart.IsAnimated = false;
      //gridpart.Pickable = false;
      //gridpart.LocationElementName = "GRID";
      //diagram.Add(gridpart);  // add this Part to the Diagram

      // BUT, the gridpart is not actually in the Diagram.parts collection,
      // and that Part cannot be replaced; so the above code is commented out.
      // Instead, this works in an existing environment:
      diagram.Grid = Grid;
    }

    // overview box?

    /// Set up LinkingBaseTool's default temporary nodes and link.
    public static void DefineLinkingToolTemporaryNodesAndLink(LinkingBaseTool tool) {
      // LinkingTool.temporaryLink
      var link = new Link();
      var path = new Shape();
      path.IsPanelMain = true;
      path.Stroke = "blue";
      link.Add(path);
      var arrow = new Shape();
      arrow.ToArrow = "Standard";
      arrow.Fill = "blue";
      arrow.Stroke = "blue";
      link.Add(arrow);
      link.LayerName = "Tool";

      tool.TemporaryLink = link;

      // LinkingTool.TemporaryFromNode and .TemporaryFromPort
      var fromNode = new Node();
      var fromPort = new Shape();
      fromPort.PortId = "";
      fromPort.Figure = "Rectangle";
      fromPort.Fill = null;
      fromPort.Stroke = "magenta";
      fromPort.StrokeWidth = 2;
      fromPort.DesiredSize = new Size(1, 1);
      fromNode.Add(fromPort);
      fromNode.Selectable = false;
      fromNode.LayerName = "Tool";

      tool.TemporaryFromNode = fromNode;
      tool.TemporaryFromPort = fromPort;

      // LinkingTool.TemporaryToNode and .TemporaryToPort
      var toNode = new Node();
      var toPort = new Shape();
      toPort.PortId = "";
      toPort.Figure = "Rectangle";
      toPort.Fill = null;
      toPort.Stroke = "magenta";
      toPort.StrokeWidth = 2;
      toPort.DesiredSize = new Size(1, 1);
      toNode.Add(toPort);
      toNode.Selectable = false;
      toNode.LayerName = "Tool";

      tool.TemporaryToNode = toNode;
      tool.TemporaryToPort = toPort;
    }

    /// Set up RelinkingTool's default handle archetypes
    public static void DefineRelinkingToolHandles(RelinkingTool tool) {
      var h = new Shape();
      h.Figure = "Diamond";
      h.DesiredSize = new Size(8, 8);
      h.Fill = "lightblue";
      h.Stroke = "dodgerblue";
      h.Cursor = "pointer";
      h.SegmentIndex = 0;

      tool.FromHandleArchetype = h;

      h = new Shape();
      h.Figure = "Diamond";
      h.DesiredSize = new Size(8, 8);
      h.Fill = "lightblue";
      h.Stroke = "dodgerblue";
      h.Cursor = "pointer";
      h.SegmentIndex = -1;

      tool.ToHandleArchetype = h;
    }

    /// Set up LinkReshapingTool's default handle archetypes
    public static void DefineLinkReshapingToolHandles(LinkReshapingTool tool) {
      var h = new Shape();
      h.Figure = "Rectangle";
      h.DesiredSize = new Size(6, 6);
      h.Fill = "lightblue";
      h.Stroke = "dodgerblue";

      tool.HandleArchetype = h;

      h = new Shape();
      h.Figure = "Diamond";
      h.DesiredSize = new Size(8, 8);
      h.Fill = "lightblue";
      h.Stroke = "dodgerblue";
      h.Cursor = "move";

      tool.MidHandleArchetype = h;
    }

    /// Set up ResizingTool's default handle archetype
    public static void DefineResizingToolHandles(ResizingTool tool) {
      var h = new Shape();
      h.AlignmentFocus = Spot.Center;
      h.Figure = "Rectangle";
      h.DesiredSize = new Size(6, 6);
      h.Fill = "lightblue";
      h.Stroke = "dodgerblue";
      h.StrokeWidth = 1;
      h.Cursor = "pointer";

      tool.HandleArchetype = h;
    }

    /// Set up RotatingTool's default handle archetype
    public static void DefineRotatingToolHandles(RotatingTool tool) {
      var h = new Shape();
      h.Figure = "Ellipse";
      h.DesiredSize = new Size(8, 8);
      h.Fill = "lightblue";
      h.Stroke = "dodgerblue";
      h.StrokeWidth = 1;
      h.Cursor = "pointer";

      tool.HandleArchetype = h;
    }

    /// Set up DragSelectingTool's default box
    public static void DefineDragSelectingToolBox(DragSelectingTool tool) {
      var b = new Part();
      b.LayerName = "Tool";
      b.Selectable = false;
      var r = new Shape();
      r.Name = "SHAPE";
      r.Figure = "Rectangle";
      r.Fill = null;
      r.Stroke = "magenta";
      b.Add(r);

      tool.Box = b;
    }

    /// This static function can be used to convert an object to a string,
    /// looking for commonly defined data properties, such as "Text", "Name", "Key", or "Id".
    public static string ToString(object val) {
      var v = val;
      if (!Util.IsSimpleType(val)) {
        v = SafePropertyValue(val, "Text", ErrorMode.Off);
        if (v == null) v = SafePropertyValue(val, "Name", ErrorMode.Off);
        if (v == null) v = SafePropertyValue(val, "Key", ErrorMode.Off);
        if (v == null) v = SafePropertyValue(val, "Id", ErrorMode.Off);
        if (v == null) v = SafePropertyValue(val, "ID", ErrorMode.Off);
      }
      if (v == null) return "null";
      return v.ToString();
    }

    /// This is a simplified version of the normal property getter.
    public static object GetProp(object obj, string prop) {
      return obj.GetType().GetProperty(prop).GetValue(obj);
    }
  }
}
