This sample demonstrates the [PolygonDrawingTool], a custom [Tool] added to the Diagram's MouseDownTools.
It is defined in its own file, as [PolygonDrawingTool.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/PolygonDrawing/PolygonDrawingTool.cs).
It also demonstrates the GeometryReshapingTool, another custom tool,
defined in [GeometryReshapingTool.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/GeometryReshaping/GeometryReshapingTool.cs).

These extensions serve as examples of features that can be added to GoDiagram by writing new classes.
With the PolygonDrawingTool, a new mode is supported that allows the user to draw custom shapes.
With the GeometryReshapingTool, users can change the geometry (i.e. the "shape") of a [Shape]s in a selected [Node].

Click a "Draw" button and then click in the diagram to place a new point in a polygon or polyline shape.
Right-click, double-click, or Enter to finish.  Press **Escape** to cancel, or **Z** to remove the last point.
Click the "Select" button to switch back to the normal selection behavior, so that you can select, resize, and rotate the shapes.
The checkboxes control whether you can resize, reshape, and /or rotate selected shapes.