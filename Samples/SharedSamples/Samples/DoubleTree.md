This sample displays a bow-tie diagram of two trees sharing a single root node growing in opposite directions.
The immediate child data of the root node have a "Dir" property
that describes the direction that subtree should grow.

The [Diagram.Layout] is an instance of the [DoubleTreeLayout] extension layout,
defined in [DoubleTreeLayout.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Layouts/DoubleTree/DoubleTreeLayout.cs).
The layout requires a [DoubleTreeLayout.DirectionFunction] predicate to decide for a child node
of the root node which way the subtree should grow.