This sample demonstrates a custom Tool, [LinkLabelOnPathDraggingTool], that allows the user to drag the label of a Link,
but that keeps the label exactly on the path of the link.
The tool is defined at [LinkLabelOnPathDraggingTool.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/LinkLabelOnPathDragging/LinkLabelOnPathDraggingTool.cs).

The label on the link can be any arbitrarily complex object.
It is positioned by the [GraphObject.SegmentIndex] and [GraphObject.SegmentFraction] properties.
The SegmentIndex is set to NaN such that the whole link path acts as the segment, and the SegmentFraction is set by the LinkLabelOnPathDraggingTool.
A two-way data binding on SegmentFraction automatically remembers any modified value on the link data object in the model.

The tool is derived from a similar tool, [LinkLabelDraggingTool.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/LinkLabelDragging/LinkLabelDraggingTool.cs),
that allows the user to drag the label in any direction from the mid-point of the Link path.