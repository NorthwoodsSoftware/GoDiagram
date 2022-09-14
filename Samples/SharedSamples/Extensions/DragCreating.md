This sample demonstrates the [DragCreatingTool], which replaces the standard DragSelectingTool.
It is defined in its own file, as [DragCreatingTool.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/DragCreating/DragCreatingTool.cs).

Press in the background and then drag to show the area to be occupied by the new node.
The mouse-up event will add a copy of the [DragCreatingTool.ArchetypeNodeData] object, causing a new node to be created.
The tool will assign its [GraphObject.Position] and [GraphObject.DesiredSize].