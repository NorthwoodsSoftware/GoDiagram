This sample is a modification of the [State Chart](demo/StateChart) sample
that makes use of the [LinkLabelDraggingTool] that is defined in its own file,
as [LinkLabelDraggingTool.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/LinkLabelDragging/LinkLabelDraggingTool.cs).

Note that after dragging a link label you can move a node connected by that link and the label maintains the same position relative to the link route.
That relative position is specified by the [GraphObject.SegmentOffset] property.
This sample also saves any changes to that property by means of a TwoWay [Binding].

See also the similar [Link Label On Path Dragging sample](demo/LinkLabelOnPathDragging),
where the label is constrained to remain on the path of the link.