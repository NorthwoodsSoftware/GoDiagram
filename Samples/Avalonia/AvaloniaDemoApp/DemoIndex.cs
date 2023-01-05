/* Copyright 1998-2023 by Northwoods Software Corporation. */

using System;
using System.Collections.Generic;

namespace AvaloniaDemoApp {
  public enum DemoType {
    Sample,
    Extension
  }

  static class DemoIndex {
    // Map short names to NavItems
    public static Dictionary<string, NavItem> Samples = new(StringComparer.InvariantCultureIgnoreCase) {
      // Samples on short List corresponding to GoJS order
      { "OrgChartStatic", new NavItem("Org Chart (Static)", typeof(Demo.Samples.OrgChartStatic.OrgChartStatic)) },
      { "OrgChartEditor", new NavItem("Org Chart Editor", typeof(Demo.Samples.OrgChartEditor.OrgChartEditor)) },
      { "FamilyTree", new NavItem("Family Tree", typeof(Demo.Samples.FamilyTree.FamilyTree)) },
      { "Genogram", new NavItem("Genogram", typeof(Demo.Samples.Genogram.Genogram)) },
      { "DoubleTree", new NavItem("Double Tree", typeof(Demo.Samples.DoubleTree.DoubleTree)) },
      { "MindMap", new NavItem("Mind Map", typeof(Demo.Samples.MindMap.MindMap)) },
      { "DecisionTree", new NavItem("Decision Tree", typeof(Demo.Samples.DecisionTree.DecisionTree)) },
      { "IvrTree", new NavItem("IVR Tree", typeof(Demo.Samples.IvrTree.IvrTree)) },
      { "IncrementalTree", new NavItem("Incremental Tree", typeof(Demo.Samples.IncrementalTree.IncrementalTree)) },
      { "ParseTree", new NavItem("Parse Tree", typeof(Demo.Samples.ParseTree.ParseTree)) },
      { "TreeView", new NavItem("Tree View", typeof(Demo.Samples.TreeView.TreeView)) },
      { "Tournament", new NavItem("Tournament", typeof(Demo.Samples.Tournament.Tournament)) },
      { "LocalView", new NavItem("Local View", typeof(Demo.Samples.LocalView.LocalView)) },
      { "Flowchart", new NavItem("Flowchart", typeof(Demo.Samples.Flowchart.Flowchart)) },
      { "BlockEditor", new NavItem("Block Editor", typeof(Demo.Samples.BlockEditor.BlockEditor)) },
      { "PageFlow", new NavItem("Page Flow", typeof(Demo.Samples.PageFlow.PageFlow)) },
      { "ProcessFlow", new NavItem("Process Flow", typeof(Demo.Samples.ProcessFlow.ProcessFlow)) },
      { "SystemDynamics", new NavItem("System Dynamics", typeof(Demo.Samples.SystemDynamics.SystemDynamics)) },
      { "StateChart", new NavItem("State Chart", typeof(Demo.Samples.StateChart.StateChart)) },
      { "Kanban", new NavItem("Kanban Board", typeof(Demo.Samples.Kanban.Kanban)) },
      { "SequentialFunction", new NavItem("Sequential Function", typeof(Demo.Samples.SequentialFunction.SequentialFunction)) },
      { "Grafcet", new NavItem("Grafcet Diagrams", typeof(Demo.Samples.Grafcet.Grafcet)) },
      { "SequenceDiagram", new NavItem("Sequence Diagram", typeof(Demo.Samples.SequenceDiagram.SequenceDiagram)) },
      { "LogicCircuit", new NavItem("Logic Circuit", typeof(Demo.Samples.LogicCircuit.LogicCircuit)) },
      { "Records", new NavItem("Record Mapper", typeof(Demo.Samples.Records.Records)) },
      { "DataFlow", new NavItem("Data Flow", typeof(Demo.Samples.DataFlow.DataFlow)) },
      { "DynamicPorts", new NavItem("Dynamic Ports", typeof(Demo.Samples.DynamicPorts.DynamicPorts)) },
      { "Planogram", new NavItem("Planogram", typeof(Demo.Samples.Planogram.Planogram)) },
      { "SeatingChart", new NavItem("Seating Chart", typeof(Demo.Samples.SeatingChart.SeatingChart)) },
      { "Regrouping", new NavItem("Regrouping", typeof(Demo.Samples.Regrouping.Regrouping)) },
      { "Pipes", new NavItem("Pipes", typeof(Demo.Samples.Pipes.Pipes)) },
      { "DraggableLink", new NavItem("Draggable Links", typeof(Demo.Samples.DraggableLink.DraggableLink)) },
      { "LinkToLinks", new NavItem("Links To Links", typeof(Demo.Samples.LinksToLinks.LinksToLinks)) },
      { "BeatPaths", new NavItem("Beat Paths", typeof(Demo.Samples.BeatPaths.BeatPaths)) },
      { "ConceptMap", new NavItem("Concept Map", typeof(Demo.Samples.ConceptMap.ConceptMap)) },
      { "Euler", new NavItem("Euler Diagram", typeof(Demo.Samples.Euler.Euler)) },
      { "EntityRelationship", new NavItem("Entity Relationship", typeof(Demo.Samples.EntityRelationship.EntityRelationship)) },
      { "FriendWheel", new NavItem("Friend Wheel", typeof(Demo.Samples.FriendWheel.FriendWheel)) },
      { "Radial", new NavItem("Recentering Radial", typeof(Demo.Samples.Radial.Radial)) }, // Moved to samples from extensions
      { "RadialPartition", new NavItem("Radial Partition", typeof(Demo.Samples.RadialPartition.RadialPartition)) },
      { "Distances", new NavItem("Distances and Paths", typeof(Demo.Samples.Distances.Distances)) },
      { "Sankey", new NavItem("Sankey Diagram", typeof(Demo.Samples.Sankey.Sankey)) },
      { "PERT", new NavItem("PERT", typeof(Demo.Samples.Pert.Pert)) },
      { "Gantt", new NavItem("Gantt", typeof(Demo.Samples.Gantt.Gantt)) },
      { "ShopFloorMonitor", new NavItem("Shop Floor Monitor", typeof(Demo.Samples.ShopFloorMonitor.ShopFloorMonitor)) },
      { "KittenMonitor", new NavItem("Kitten Monitor", typeof(Demo.Samples.KittenMonitor.KittenMonitor)) },
      { "Grouping", new NavItem("Grouping", typeof(Demo.Samples.Grouping.Grouping)) },
      { "SwimBands", new NavItem("Layer Bands", typeof(Demo.Samples.SwimBands.SwimBands)) },
      { "SwimLanes", new NavItem("Swim Lanes (Horizontal)", typeof(Demo.Samples.SwimLanes.SwimLanes)) },
      { "UmlClass", new NavItem("UML Class", typeof(Demo.Samples.UmlClass.UmlClass)) },
      { "Minimal", new NavItem("Minimal", typeof(Demo.Samples.Minimal.Minimal)) },
      { "Basic", new NavItem("Basic", typeof(Demo.Samples.Basic.Basic)) },
      { "ClassHierarchy", new NavItem("Class Hierarchy", typeof(Demo.Samples.ClassHierarchy.ClassHierarchy)) },
      { "VisualTree", new NavItem("Visual Tree", typeof(Demo.Samples.VisualTree.VisualTree)) },
      { "Shapes", new NavItem("Shape Figures", typeof(Demo.Samples.Shapes.Shapes)) },
      { "Icons", new NavItem("SVG Icons", typeof(Demo.Samples.Icons.Icons)) },
      { "Arrowheads", new NavItem("Arrowheads", typeof(Demo.Samples.Arrowheads.Arrowheads)) },
      { "Navigation", new NavItem("Navigation", typeof(Demo.Samples.Navigation.Navigation)) },
      { "UpdateDemo", new NavItem("Update Demo", typeof(Demo.Samples.UpdateDemo.UpdateDemo)) },
      { "ContentAlign", new NavItem("Content Alignment", typeof(Demo.Samples.ContentAlign.ContentAlign)) },
      { "Comments", new NavItem("Comments", typeof(Demo.Samples.Comments.Comments)) },
      { "GLayout", new NavItem("Grid Layout", typeof(Demo.Samples.GLayout.GLayout)) },
      { "TLayout", new NavItem("Tree Layout", typeof(Demo.Samples.TLayout.TLayout)) },
      { "FDLayout", new NavItem("Force Directed", typeof(Demo.Samples.FDLayout.FDLayout)) },
      { "LDLayout", new NavItem("Layered Digraph", typeof(Demo.Samples.LDLayout.LDLayout)) },
      { "CLayout", new NavItem("Circular Layout", typeof(Demo.Samples.CLayout.CLayout)) },
      { "InteractiveForce", new NavItem("Interactive Force", typeof(Demo.Samples.InteractiveForce.InteractiveForce)) },
    };

    public static Dictionary<string, NavItem> Extensions = new(StringComparer.InvariantCultureIgnoreCase) {
      /****** LAYOUTS ******/
      { "Arranging", new NavItem("Arranging Layout", typeof(Demo.Extensions.Arranging.Arranging)) },
      { "Fishbone", new NavItem("Fishbone Layout", typeof(Demo.Extensions.Fishbone.Fishbone)) },
      { "PackedLayout", new NavItem("Packed Layout", typeof(Demo.Extensions.Packed.Packed)) },
      { "Parellel", new NavItem("Parallel Layout", typeof(Demo.Extensions.Parallel.Parallel)) },
      { "Serpentine", new NavItem("Serpentine Layout", typeof(Demo.Extensions.Serpentine.Serpentine)) },
      { "Spiral", new NavItem("Spiral Layout", typeof(Demo.Extensions.Spiral.Spiral)) },
      { "SwimLaneLayout", new NavItem("Swim Lane Layout", typeof(Demo.Extensions.SwimLane.SwimLane)) },
      { "Table", new NavItem("Table Layout", typeof(Demo.Extensions.Table.Table)) },
      { "TreeMap", new NavItem("Tree Map Layout", typeof(Demo.Extensions.TreeMap.TreeMap)) },

      /****** TOOLS ******/
      { "RealtimeSelecting", new NavItem("Realtime Selecting", typeof(Demo.Extensions.RealtimeDragSelecting.RealtimeDragSelecting)) },
      { "DragCreating", new NavItem("Drag Creating", typeof(Demo.Extensions.DragCreating.DragCreating)) },
      { "DragZooming", new NavItem("Drag Zooming", typeof(Demo.Extensions.DragZooming.DragZooming)) },
      { "ResizeMultiple", new NavItem("Resize Multiple", typeof(Demo.Extensions.ResizeMultiple.ResizeMultiple)) },
      { "RotateMultiple", new NavItem("Rotate Multiple", typeof(Demo.Extensions.RotateMultiple.RotateMultiple)) },
      // spot rotating
      { "Rescaling", new NavItem("Rescaling", typeof(Demo.Extensions.Rescaling.Rescaling)) },
      { "CurvedLinkReshaping", new NavItem("Bez. Link Reshaping", typeof(Demo.Extensions.CurvedLinkReshaping.CurvedLinkReshaping)) },
      { "OrthogonalLinkReshaping", new NavItem("Orth. Link Reshaping", typeof(Demo.Extensions.OrthogonalLinkReshaping.OrthogonalLinkReshaping)) },
      { "SnapLinkReshaping", new NavItem("Snap Link Reshaping", typeof(Demo.Extensions.SnapLinkReshaping.SnapLinkReshaping)) },
      { "GeometryReshaping", new NavItem("Geometry Reshaping", typeof(Demo.Extensions.GeometryReshaping.GeometryReshaping)) },
      { "SectorReshaping", new NavItem("Sector Reshaping", typeof(Demo.Extensions.SectorReshaping.SectorReshaping)) },
      { "FreehandDrawing", new NavItem("Freehand Drawing", typeof(Demo.Extensions.FreehandDrawing.FreehandDrawing)) },
      { "PolygonDrawing", new NavItem("Polygon Drawing", typeof(Demo.Extensions.PolygonDrawing.PolygonDrawing)) },
      { "PolylineLinking", new NavItem("Polyline Linking", typeof(Demo.Extensions.PolylineLinking.PolylineLinking)) },
      { "LinkShifting", new NavItem("Link Shifting", typeof(Demo.Extensions.LinkShifting.LinkShifting)) },
      { "LinkLabelDragging", new NavItem("Link Label Dragging", typeof(Demo.Extensions.LinkLabelDragging.LinkLabelDragging)) },
      { "NodeLabelDragging", new NavItem("Node Label Dragging", typeof(Demo.Extensions.NodeLabelDragging.NodeLabelDragging)) },
      { "LinkLabelOnPathDragging", new NavItem("Label On Path Dragging", typeof(Demo.Extensions.LinkLabelOnPathDragging.LinkLabelOnPathDragging)) },
      { "GuidedDragging", new NavItem("Guided Dragging", typeof(Demo.Extensions.GuidedDragging.GuidedDragging)) },
      { "NonRealtimeDragging", new NavItem("Non-Realtime Dragging", typeof(Demo.Extensions.NonRealtimeDragging.NonRealtimeDragging)) },
      { "PortShifting", new NavItem("Port Shifting", typeof(Demo.Extensions.PortShifting.PortShifting)) },
      { "ColumnResizing", new NavItem("Column Resizing", typeof(Demo.Extensions.ColumnResizing.ColumnResizing)) },
      { "OverviewResizing", new NavItem("Overview Resizing", typeof(Demo.Extensions.OverviewResizing.OverviewResizing)) },

      /****** GRAPH OBJECTS ******/
      // scrolling table
      { "BalloonLink", new NavItem("Balloon Link", typeof(Demo.Extensions.BalloonLink.BalloonLink)) },
      { "ParallelRoute", new NavItem("Parallel Route Links", typeof(Demo.Extensions.ParallelRoute.ParallelRoute)) },
      { "Dimensioning", new NavItem("Dimensioning Links", typeof(Demo.Extensions.Dimensioning.Dimensioning)) },
      { "DrawCommandHandler", new NavItem("Drawing Commands", typeof(Demo.Extensions.DrawCommandHandler.DrawCommandHandler)) },
      // local storage?
      { "Robot", new NavItem("Simulating Input", typeof(Demo.Extensions.Robot.Robot)) },
      // data inspector

      /****** BIG SAMPLES ******/
      // bpmn
      // floor planner

      /****** OTHER ******/
      { "Hyperlink", new NavItem("Hyperlinks", typeof(Demo.Extensions.Hyperlink.Hyperlink)) },
      { "ZoomSlider", new NavItem("Zoom Slider", typeof(Demo.Extensions.ZoomSlider.ZoomSlider)) },
      { "VirtualizedPacked", new NavItem("Virtualized Packed", typeof(Demo.Extensions.VirtualizedPacked.VirtualizedPacked)) }
    };
  }

  public class NavItem {
    public NavItem(string name, Type type) {
      Name = name;
      ControlType = type;
    }
    public string Name { get; }
    public Type ControlType { get; }

    public override string ToString() => Name;
  }
}
