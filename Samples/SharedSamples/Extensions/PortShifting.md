This is exactly like the [Logic Circuit sample](demo/LogicCircuit)
but also makes use of the [PortShiftingTool],
which is defined in [PortShiftingTool.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/PortShifting/PortShiftingTool.cs)

When the user wants to shift the position of a port on a node,
the user can hold down the Shift key during a mouse - down on a port element.
Dragging then will move the port within the node.

Note how the relative position of the port within the node is maintained as you resize the node.

If you want to persist the port's spot, you should add a TwoWay Binding of the [GraphObject.Alignment]
property with a property that you define on the node data for each port.

This sample does not constrain the position of the port within the node,
but you could adapt the [PortShiftingTool.UpdateAlignment] method to do so.
For example if you wanted, you could keep a port stuck along one edge of the node.