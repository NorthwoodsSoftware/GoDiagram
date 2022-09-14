This sample is a modification of the [State Chart](demo/StateChart) sample
that makes use of the [NodeLabelDraggingTool] that is defined in its own file,
as [NodeLabelDraggingTool.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/NodeLabelDragging/NodeLabelDraggingTool.cs).

Note that after dragging a node label you can move that node and the label maintains the same position relative to the node.
That relative position is specified by the [GraphObject.Alignment] property, used by the "Spot" [Panel].
This sample also saves any changes to that property by means of a TwoWay [Binding].