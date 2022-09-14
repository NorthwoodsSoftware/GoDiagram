Nodes in this sample use [Shape.GeometryString] to determine their shape.
You can see more custom geometry examples and read about GeometryString
on the [Geometry Path Strings Introduction page](intro/geometry.html).

As a part's unconnected port (shown by an X) comes close to a stationary port
with which it is compatible, the dragged selection snaps so that those ports coincide.
A custom [DraggingTool], called **SnappingTool**, is used to check compatibility.

Dragging automatically drags all connected parts.
Hold down the Shift key before dragging in order to detach a part from the parts it is connected with.
These functionalities are also controlled by the custom SnappingTool.

Use the [GraphObject.ContextMenu] to rotate, detach, or delete a node.
If it is connected with other parts, the whole collection rotates.