This sample is a simplified version of the [Draggable Link](DraggableLink) sample.
Links are not draggable, there are no custom [Adornment]s, nodes are not rotatable, and links
do not have text labels.

Its purpose is to demonstrate the [SnapLinkReshapingTool] (defined in
[SnapLinkReshapingTool.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/SnapLinkReshaping/SnapLinkReshapingTool.cs)),
an extension of [LinkReshapingTool] that snaps each dragged reshape handle of selected Links to
the nearest grid point. If the [SnapLinkReshapingTool.AvoidsNodes] option is true,
as it is by default, then the reshaping is limited to points where the adjacent segments would not
be crossing over any avoidable nodes.

Note how the "LinkReshaped" DiagramEvent listener changes the [Link.Routing] of the reshaped Link,
so that it is no longer AvoidsNodes routing but simple Orthogonal routing.
This combined with [Link.Adjusting] being End permits the middle points of the link route to be
retained even after the user moves or resizes nodes.
Furthermore there is a TwoWay [Binding] on [Link.Routing], so that the model remembers
whether the link route had ever been reshaped manually.