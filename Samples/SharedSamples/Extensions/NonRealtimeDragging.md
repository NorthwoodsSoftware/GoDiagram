This custom [DraggingTool] class causes the user to drag around a translucent image of the Nodes and Links being moved,
leaving the selected Parts in place, rather than actually moving those Nodes and Links in realtime.
Only when the mouse up occurs does the move happen.

This tool is defined in its own file, as [NonRealtimeDraggingTool.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/NonRealtimeDragging/NonRealtimeDraggingTool.cs)