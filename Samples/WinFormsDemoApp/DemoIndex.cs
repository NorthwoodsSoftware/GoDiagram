using System;
using System.Collections.Generic;

using WinFormsSharedControls;

namespace WinFormsDemoApp {
  public enum DemoType {
    Sample,
    Extension
  }

  static class DemoIndex {
    // Map short names to NavItems
    public static Dictionary<string, NavItem> Samples = new(StringComparer.InvariantCultureIgnoreCase) {
      // Samples on short List corresponding to GoJS order
      { "OrgChartEditor", new NavItem("OrgChart Editor", typeof(WinFormsSampleControls.OrgChartEditor.OrgChartEditorControl)) },
      { "OrgChartStatic", new NavItem("Org Chart (Static)", typeof(WinFormsSampleControls.OrgChartStatic.OrgChartStaticControl)) },
      { "FamilyTree", new NavItem("Family Tree", typeof(WinFormsSampleControls.FamilyTree.FamilyTreeControl)) },
      { "Genogram", new NavItem("Genogram", typeof(WinFormsSampleControls.Genogram.GenogramControl)) },
      { "DoubleTree", new NavItem("Double Tree", typeof(WinFormsSampleControls.DoubleTree.DoubleTreeControl)) },
      { "MindMap", new NavItem("Mind Map", typeof(WinFormsSampleControls.MindMap.MindMapControl)) },
      { "DecisionTree", new NavItem("Decision Tree", typeof(WinFormsSampleControls.DecisionTree.DecisionTreeControl)) },
      { "IVRtree", new NavItem("IVR", typeof(WinFormsSampleControls.IVRTree.IVRTreeControl)) },
      { "IncrementalTree", new NavItem("Incremental Tree", typeof(WinFormsSampleControls.IncrementalTree.IncrementalTreeControl)) },
      { "ParseTree", new NavItem("Parse Tree", typeof(WinFormsSampleControls.ParseTree.ParseTreeControl)) },
      { "TreeView", new NavItem("Tree View", typeof(WinFormsSampleControls.TreeView.TreeViewControl)) },
      { "Tournament", new NavItem("Tournament", typeof(WinFormsSampleControls.Tournament.TournamentControl)) },
      { "LocalView", new NavItem("Local View", typeof(WinFormsSampleControls.LocalView.LocalViewControl)) },
      { "Flowchart", new NavItem("Flowchart", typeof(WinFormsSampleControls.Flowchart.FlowchartControl)) },
      { "BlockEditor", new NavItem("Block Editor", typeof(WinFormsSampleControls.BlockEditor.BlockEditorControl)) },
      { "PageFlow", new NavItem("Page Flow", typeof(WinFormsSampleControls.PageFlow.PageFlowControl)) },
      { "ProcessFlow", new NavItem("Process Flow", typeof(WinFormsSampleControls.ProcessFlow.ProcessFlowControl)) },
      { "SystemDynamics", new NavItem("System Dynamics", typeof(WinFormsSampleControls.SystemDynamics.SystemDynamicsControl)) },
      { "StateChart", new NavItem("State Chart", typeof(WinFormsSampleControls.StateChart.StateChartControl)) },
      { "Kanban", new NavItem("Kanban Board", typeof(WinFormsSampleControls.Kanban.KanbanControl)) },
      { "SequentialFunction", new NavItem("Sequential Function", typeof(WinFormsSampleControls.SequentialFunction.SequentialFunctionControl)) },
      { "Grafcet", new NavItem("Grafcet", typeof(WinFormsSampleControls.Grafcet.GrafcetControl)) },
      { "SequenceDiagram", new NavItem("Sequence Diagram", typeof(WinFormsSampleControls.SequenceDiagram.SequenceDiagramControl)) },
      { "LogicCircuit", new NavItem("Logic Circuit", typeof(WinFormsSampleControls.LogicCircuit.LogicCircuitControl)) },
      { "Records", new NavItem("Record Mapper", typeof(WinFormsSampleControls.Records.RecordsControl)) },
      { "DataFlow", new NavItem("Data Flow (Horizontal)", typeof(WinFormsSampleControls.DataFlow.DataFlowControl)) },
      { "DynamicPorts", new NavItem("Dynamic Ports", typeof(WinFormsSampleControls.DynamicPorts.DynamicPortsControl)) },
      { "Planogram", new NavItem("Planogram", typeof(WinFormsSampleControls.Planogram.PlanogramControl)) },
      { "SeatingChart", new NavItem("Seating Chart", typeof(WinFormsSampleControls.SeatingChart.SeatingChartControl)) },
      { "Regrouping", new NavItem("Regrouping", typeof(WinFormsSampleControls.Regrouping.RegroupingControl)) },
      { "Pipes", new NavItem("Pipes", typeof(WinFormsSampleControls.Pipes.PipesControl)) },
      { "DraggableLink", new NavItem("Draggable Links", typeof(WinFormsSampleControls.DraggableLink.DraggableLinkControl)) },
      { "LinkToLinks", new NavItem("Links To Links", typeof(WinFormsSampleControls.LinksToLinks.LinksToLinksControl)) },
      { "BeatPaths", new NavItem("Beat Paths", typeof(WinFormsSampleControls.BeatPaths.BeatPathsControl)) },
      { "ConceptMap", new NavItem("Concept Map", typeof(WinFormsSampleControls.ConceptMap.ConceptMapControl)) },
      { "Euler", new NavItem("Euler Diagram", typeof(WinFormsSampleControls.Euler.EulerControl)) },
      { "EntityRelationship", new NavItem("Entity Relationship", typeof(WinFormsSampleControls.EntityRelationship.EntityRelationshipControl)) },
      { "FriendWheel", new NavItem("Friend Wheel", typeof(WinFormsSampleControls.FriendWheel.FriendWheelControl)) },
      { "Radial", new NavItem("Recentering Radial", typeof(WinFormsSampleControls.Radial.RadialControl)) }, // Moved to samples from extensions
      { "RadialPartition", new NavItem("Radial Partition", typeof(WinFormsSampleControls.RadialPartition.RadialPartitionControl)) },
      { "Distances", new NavItem("Distances and Paths", typeof(WinFormsSampleControls.Distances.DistancesControl)) },
      { "Sankey", new NavItem("Sankey Diagram", typeof(WinFormsSampleControls.Sankey.SankeyControl)) },
      { "PERT", new NavItem("PERT", typeof(WinFormsSampleControls.PERT.PERTControl)) },
      { "Gantt", new NavItem("Gantt", typeof(WinFormsSampleControls.Gantt.GanttControl)) },
      { "ShopFloorMonitor", new NavItem("Shop Floor Monitor", typeof(WinFormsSampleControls.ShopFloorMonitor.ShopFloorMonitorControl)) },
      { "KittenMonitor", new NavItem("Kitten Monitor", typeof(WinFormsSampleControls.KittenMonitor.KittenMonitorControl)) },
      { "Grouping", new NavItem("Grouping", typeof(WinFormsSampleControls.Grouping.GroupingControl)) },
      { "SwimBands", new NavItem("Layer Bands", typeof(WinFormsSampleControls.SwimBands.SwimBandsControl)) },
      { "SwimLanes", new NavItem("Swim Lanes (Horizontal)", typeof(WinFormsSampleControls.SwimLanes.SwimLanesControl)) },
      { "UmlClass", new NavItem("UML Class", typeof(WinFormsSampleControls.UMLClass.UMLClassControl)) },
      { "Minimal", new NavItem("Minimal", typeof(WinFormsSampleControls.Minimal.MinimalControl)) },
      { "Basic", new NavItem("Basic", typeof(WinFormsSampleControls.Basic.BasicControl)) },
      { "ClassHierarchy", new NavItem("Class Hierarchy", typeof(WinFormsSampleControls.ClassHierarchy.ClassHierarchyControl)) },
      { "VisualTree", new NavItem("Visual Tree", typeof(WinFormsSampleControls.VisualTree.VisualTreeControl)) },
      { "Shapes", new NavItem("Shape Figures", typeof(WinFormsSampleControls.Shapes.ShapesControl)) },
      { "Icons", new NavItem("SVG Icons", typeof(WinFormsSampleControls.Icons.IconsControl)) },
      { "Arrowheads", new NavItem("Arrowheads", typeof(WinFormsSampleControls.Arrowheads.ArrowheadsControl)) },
      { "Navigation", new NavItem("Navigation", typeof(WinFormsSampleControls.Navigation.NavigationControl)) },
      { "UpdateDemo", new NavItem("Update Demo", typeof(WinFormsSampleControls.UpdateDemo.UpdateDemoControl)) },
      { "ContentAlign", new NavItem("Content Alignment", typeof(WinFormsSampleControls.ContentAlign.ContentAlignControl)) },
      { "Comments", new NavItem("Comments", typeof(WinFormsSampleControls.Comments.CommentsControl)) },
      { "GLayout", new NavItem("Grid Layout", typeof(WinFormsSampleControls.GLayout.GLayoutControl)) },
      { "TLayout", new NavItem("Tree Layout", typeof(WinFormsSampleControls.TLayout.TLayoutControl)) },
      { "FDLayout", new NavItem("ForceDirected Layout", typeof(WinFormsSampleControls.FDLayout.FDLayoutControl)) },
      { "LDLayout", new NavItem("Layered Digraph", typeof(WinFormsSampleControls.LDLayout.LDLayoutControl)) },
      { "CLayout", new NavItem("Circular Layout", typeof(WinFormsSampleControls.CLayout.CLayoutControl)) },
      { "InteractiveForce", new NavItem("Interactive Force", typeof(WinFormsSampleControls.InteractiveForce.InteractiveForceControl)) },

      // Samples on Complete list but not on short list
      { "Absolute", new NavItem("Absolute", typeof(WinFormsSampleControls.Absolute.AbsoluteControl)) },
      { "AddRemoveColumns", new NavItem("Add & Remove Columns", typeof(WinFormsSampleControls.AddRemoveColumns.AddRemoveColumnsControl)) },
      { "AddToPalette", new NavItem("Add To Palette", typeof(WinFormsSampleControls.AddToPalette.AddToPaletteControl)) },
      { "AdornmentButtons", new NavItem("Adornment Buttons", typeof(WinFormsSampleControls.AdornmentButtons.AdornmentButtonsControl)) },
      { "BarCharts", new NavItem("Bar Charts", typeof(WinFormsSampleControls.BarCharts.BarChartsControl)) },
      //{ "", new NavItem("Canvases", typeof(WinFormsSampleControls.Canvases.CanvasesControl)) },
      { "CandlestickCharts", new NavItem("Candle-Stick Charts", typeof(WinFormsSampleControls.CandlestickCharts.CandlestickChartsControl)) },
      { "ConstantSize", new NavItem("Constant Size", typeof(WinFormsSampleControls.ConstantSize.ConstantSizeControl)) },
      { "ControlGauges", new NavItem("Control Gauges", typeof(WinFormsSampleControls.ControlGauges.ControlGaugesControl)) },
      { "Curviness", new NavItem("Curviness", typeof(WinFormsSampleControls.Curviness.CurvinessControl)) },
      { "CustomExpandCollapse", new NavItem("Custom Expand Collapse", typeof(WinFormsSampleControls.CustomExpandCollapse.CustomExpandCollapseControl)) },
      { "DataFlowVertical", new NavItem("Data Flow (Vertical)", typeof(WinFormsSampleControls.DataFlowVertical.DataFlowVerticalControl)) },
      { "DonutCharts", new NavItem("Donut Charts", typeof(WinFormsSampleControls.DonutCharts.DonutChartsControl)) },
      { "DoubleCircle", new NavItem("Double Circle", typeof(WinFormsSampleControls.DoubleCircle.DoubleCircleControl)) },
      { "DraggablePorts", new NavItem("Draggable Ports", typeof(WinFormsSampleControls.DraggablePorts.DraggablePortsControl)) },
      { "DragUnoccupied", new NavItem("Drag Unoccupied", typeof(WinFormsSampleControls.DragUnoccupied.DragUnoccupiedControl)) },
      { "DynamicPieChart", new NavItem("Dynamic Pie Chart", typeof(WinFormsSampleControls.DynamicPieChart.DynamicPieChartControl)) },
      { "FamilyTreeJP", new NavItem("Family Tree (Japanese)", typeof(WinFormsSampleControls.FamilyTreeJP.FamilyTreeJPControl)) },
      { "FaultTree", new NavItem("Fault Tree", typeof(WinFormsSampleControls.FaultTree.FaultTreeControl)) },
      { "FlowBuilder", new NavItem("Flow Builder", typeof(WinFormsSampleControls.FlowBuilder.FlowBuilderControl)) },
      { "Flowgrammer", new NavItem("Flowgrammer", typeof(WinFormsSampleControls.Flowgrammer.FlowgrammerControl)) },
      { "GameOfLife", new NavItem("Game Of Life", typeof(WinFormsSampleControls.GameOfLife.GameOfLifeControl)) },
      { "HoverButtons", new NavItem("Hover Buttons", typeof(WinFormsSampleControls.HoverButtons.HoverButtonsControl)) },
      { "InstrumentGauge", new NavItem("Instrument Gauge", typeof(WinFormsSampleControls.InstrumentGauge.InstrumentGaugeControl)) },
      { "Macros", new NavItem("Macros", typeof(WinFormsSampleControls.Macros.MacrosControl)) },
      { "MultiNodePathLinks", new NavItem("Multi-Node Path Links", typeof(WinFormsSampleControls.MultiNodePathLinks.MultiNodePathLinksControl)) },
      { "MultiArrow", new NavItem("Multiple Arrowheads", typeof(WinFormsSampleControls.MultiArrow.MultiArrowControl)) },
      { "Network", new NavItem("Network", typeof(WinFormsSampleControls.Network.NetworkControl)) },
      { "OrgChartAssistants", new NavItem("OrgChart Assistants", typeof(WinFormsSampleControls.OrgChartAssistants.OrgChartAssistantsControl)) },
      { "OrgChartExtras", new NavItem("OrgChart Extras", typeof(WinFormsSampleControls.OrgChartExtras.OrgChartExtrasControl)) },
      { "PackedHierarchy", new NavItem("Packed Hierarchy", typeof(WinFormsSampleControls.PackedHierarchy.PackedHierarchyControl)) },
      { "PanelLayout", new NavItem("Panel Layout", typeof(WinFormsSampleControls.PanelLayout.PanelLayoutControl)) },
      { "PieCharts", new NavItem("Pie Charts", typeof(WinFormsSampleControls.PieCharts.PieChartsControl)) },
      { "PipeTree", new NavItem("Pipe Tree", typeof(WinFormsSampleControls.PipeTree.PipeTreeControl)) },
      { "ProductionEditor", new NavItem("Production Editor", typeof(WinFormsSampleControls.ProductionEditor.ProductionEditorControl)) },
      { "ProductionProcess", new NavItem("Production Process", typeof(WinFormsSampleControls.ProductionProcess.ProductionProcessControl)) },
      { "RadialAdornment", new NavItem("Radial Adornment", typeof(WinFormsSampleControls.RadialAdornment.RadialAdornmentControl)) },
      { "RegroupingTreeView", new NavItem("Regrouping Tree View", typeof(WinFormsSampleControls.RegroupingTreeView.RegroupingTreeViewControl)) },
      { "Relationships", new NavItem("Relationships", typeof(WinFormsSampleControls.Relationships.RelationshipsControl)) },
      { "RoundedGroups", new NavItem("Rounded Groups", typeof(WinFormsSampleControls.RoundedGroups.RoundedGroupsControl)) },
      { "RuleredDiagram", new NavItem("Rulered Diagram", typeof(WinFormsSampleControls.RuleredDiagram.RuleredDiagramControl)) },
      { "ScrollModes", new NavItem("Scroll Modes", typeof(WinFormsSampleControls.ScrollModes.ScrollModesControl)) },
      { "SelectableFields", new NavItem("Selectable Fields", typeof(WinFormsSampleControls.SelectableFields.SelectableFieldsControl)) },
      { "SelectablePorts", new NavItem("Selectable Ports", typeof(WinFormsSampleControls.SelectablePorts.SelectablePortsControl)) },
      { "SharedStates", new NavItem("Shared States", typeof(WinFormsSampleControls.SharedStates.SharedStatesControl)) },
      { "SinglePage", new NavItem("Single Page", typeof(WinFormsSampleControls.SinglePage.SinglePageControl)) },
      { "SpacingZoom", new NavItem("Spacing Zoom", typeof(WinFormsSampleControls.SpacingZoom.SpacingZoomControl)) },
      { "SparklineGraphs", new NavItem("Sparklines", typeof(WinFormsSampleControls.SparklineGraphs.SparklineGraphsControl)) },
      { "Spreadsheet", new NavItem("Spreadsheet", typeof(WinFormsSampleControls.Spreadsheet.SpreadsheetControl)) },
      { "StateChartIncremental", new NavItem("State Chart Incremental", typeof(WinFormsSampleControls.StateChartIncremental.StateChartIncrementalControl)) },
      { "SwimLanesVertical", new NavItem("Swim Lanes (Vertical)", typeof(WinFormsSampleControls.SwimLanesVertical.SwimLanesVerticalControl)) },
      { "TaperedLinks", new NavItem("Tapered Links", typeof(WinFormsSampleControls.TaperedLinks.TaperedLinksControl)) },
      { "Thermometer", new NavItem("Thermometer", typeof(WinFormsSampleControls.Thermometer.ThermometerControl)) },
      { "Timeline", new NavItem("Timeline", typeof(WinFormsSampleControls.Timeline.TimelineControl)) },
      { "TreeMapper", new NavItem("Tree Mapper", typeof(WinFormsSampleControls.TreeMapper.TreeMapperControl)) },
      { "TriStateCheckBoxTree", new NavItem("Tri-State CheckBox Tree", typeof(WinFormsSampleControls.TriStateCheckBoxTree.TriStateCheckBoxTreeControl)) },
      { "TwoHalves", new NavItem("Two Halves", typeof(WinFormsSampleControls.TwoHalves.TwoHalvesControl)) },
      { "Virtualized", new NavItem("Virtualized", typeof(WinFormsSampleControls.Virtualized.VirtualizedControl)) },
      { "Wordcloud", new NavItem("Word Cloud", typeof(WinFormsSampleControls.Wordcloud.WordcloudControl)) }
    };

    public static Dictionary<string, NavItem> Extensions = new(StringComparer.InvariantCultureIgnoreCase) {
      /****** LAYOUTS ******/
      { "Arranging", new NavItem("Arranging Layout", typeof(WinFormsExtensionControls.Arranging.ArrangingControl)) },
      { "Fishbone", new NavItem("Fishbone Layout", typeof(WinFormsExtensionControls.Fishbone.FishboneControl)) },
      { "PackedLayout", new NavItem("Packed Layout", typeof(WinFormsExtensionControls.PackedLayout.PackedLayoutControl)) },
      { "Parellel", new NavItem("Parallel Layout", typeof(WinFormsExtensionControls.Parallel.ParallelControl)) },
      { "Serpentine", new NavItem("Serpentine Layout", typeof(WinFormsExtensionControls.Serpentine.SerpentineControl)) },
      { "Spiral", new NavItem("Spiral Layout", typeof(WinFormsExtensionControls.Spiral.SpiralControl)) },
      { "SwimLaneLayout", new NavItem("Swim Lane Layout", typeof(WinFormsExtensionControls.SwimLaneLayout.SwimLaneLayoutControl)) },
      { "Table", new NavItem("Table Layout", typeof(WinFormsExtensionControls.Table.TableControl)) },
      { "TreeMap", new NavItem("Tree Map Layout", typeof(WinFormsExtensionControls.TreeMap.TreeMapControl)) },

      /****** TOOLS ******/
      { "RealtimeSelecting", new NavItem("Realtime Selecting", typeof(WinFormsExtensionControls.RealtimmeDragSelecting.RealtimeDragSelectingControl)) },
      { "DragCreating", new NavItem("Drag Creating", typeof(WinFormsExtensionControls.DragCreating.DragCreatingControl)) },
      { "DragZooming", new NavItem("Drag Zooming", typeof(WinFormsExtensionControls.DragZooming.DragZoomingControl)) },
      { "ResizeMultiple", new NavItem("Resize Multiple", typeof(WinFormsExtensionControls.ResizeMultiple.ResizeMultipleControl)) },
      { "RotateMultiple", new NavItem("Rotate Multiple", typeof(WinFormsExtensionControls.RotateMultiple.RotateMultipleControl)) },
      // spot rotating
      { "Rescaling", new NavItem("Rescaling", typeof(WinFormsExtensionControls.Rescaling.RescalingControl)) },
      { "CurvedLinkReshaping", new NavItem("Bez. Link Reshaping", typeof(WinFormsExtensionControls.CurvedLinkReshaping.CurvedLinkReshapingControl)) },
      { "OrthogonalLinkReshaping", new NavItem("Orth. Link Reshaping", typeof(WinFormsExtensionControls.OrthogonalLinkReshaping.OrthogonalLinkReshapingControl)) },
      { "SnapLinkReshaping", new NavItem("Snap Link Reshaping", typeof(WinFormsExtensionControls.SnapLinkReshaping.SnapLinkReshapingControl)) },
      { "GeometryReshaping", new NavItem("Geometry Reshaping", typeof(WinFormsExtensionControls.GeometryReshaping.GeometryReshapingControl)) },
      { "SectorReshaping", new NavItem("Sector Reshaping", typeof(WinFormsExtensionControls.SectorReshaping.SectorReshapingControl)) },
      { "FreehandDrawing", new NavItem("Freehand Drawing", typeof(WinFormsExtensionControls.FreehandDrawing.FreehandDrawingControl)) },
      { "PolygonDrawing", new NavItem("Polygon Drawing", typeof(WinFormsExtensionControls.PolygonDrawing.PolygonDrawingControl)) },
      { "PolylineLinking", new NavItem("Polyline Linking", typeof(WinFormsExtensionControls.PolylineLinking.PolylineLinkingControl)) },
      { "LinkShifting", new NavItem("Link Shifting", typeof(WinFormsExtensionControls.LinkShifting.LinkShiftingControl)) },
      { "LinkLabelDragging", new NavItem("Link Label Dragging", typeof(WinFormsExtensionControls.LinkLabelDragging.LinkLabelDraggingControl)) },
      { "NodeLabelDragging", new NavItem("Node Label Dragging", typeof(WinFormsExtensionControls.NodeLabelDragging.NodeLabelDraggingControl)) },
      { "LinkLabelOnPathDragging", new NavItem("Label On Path Dragging", typeof(WinFormsExtensionControls.LinkLabelOnPathDragging.LinkLabelOnPathDraggingControl)) },
      { "GuidedDragging", new NavItem("Guided Dragging", typeof(WinFormsExtensionControls.GuidedDragging.GuidedDraggingControl)) },
      { "NonRealtimeDragging", new NavItem("Non-Realtime Dragging", typeof(WinFormsExtensionControls.NonRealtimeDragging.NonRealtimeDraggingControl)) },
      { "PortShifting", new NavItem("Port Shifting", typeof(WinFormsExtensionControls.PortShifting.PortShiftingControl)) },
      { "ColumnResizing", new NavItem("Column Resizing", typeof(WinFormsExtensionControls.ColumnResizing.ColumnResizingControl)) },
      // NYI, needs overview fixes: { "OverviewResizing", new NavItem("Overview Resizing", typeof(WinFormsExtensionControls.OverviewResizing.OverviewResizingControl)) },

      /****** GRAPH OBJECTS ******/
      // scrolling table
      { "BalloonLink", new NavItem("Balloon Link", typeof(WinFormsExtensionControls.BalloonLink.BalloonLinkControl)) },
      { "ParallelRoute", new NavItem("Parallel Route Links", typeof(WinFormsExtensionControls.ParallelRoute.ParallelRouteControl)) },
      { "Dimensioning", new NavItem("Dimensioning Links", typeof(WinFormsExtensionControls.Dimensioning.DimensioningControl)) },
      { "DrawCommandHandler", new NavItem("Drawing Commands", typeof(WinFormsExtensionControls.DrawCommandHandler.DrawCommandHandlerControl)) },
      // local storage?
      // simulating input (robot)
      // data inspector

      /****** BIG SAMPLES ******/
      // bpmn
      // floor planner

      /****** OTHER ******/
      { "Hyperlink", new NavItem("Hyperlinks", typeof(WinFormsExtensionControls.Hyperlink.HyperlinkControl)) },
      { "ZoomSlider", new NavItem("Zoom Slider", typeof(WinFormsExtensionControls.ZoomSlider.ZoomSliderControl)) },
      { "VirtualizedPacked", new NavItem("Virtualized Packed", typeof(WinFormsExtensionControls.VirtualizedPacked.VirtualizedPackedControl)) }
    };
  }
}
