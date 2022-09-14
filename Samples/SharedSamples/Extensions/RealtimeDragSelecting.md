This sample demonstrates the [RealtimeDragSelectingTool], which replaces the standard [DragSelectingTool].
Press in the background, wait briefly, and then drag to start selecting Nodes or Links that intersect with the box.
You can press or release Control or Shift while dragging to see how the selection changes.

Load it in your own app by including [RealtimeDragSelectingTool.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/RealtimeDragSelecting/RealtimeDragSelectingTool.cs).
Initialize your Diagram by setting [ToolManager.DragSelectingTool] to a new instance of this tool.
For example:

```cs
myDiagram.ToolManager.DragSelectingTool = new RealtimeDragSelectingTool();
```