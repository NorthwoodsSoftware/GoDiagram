Click on a Node to center it and show its relationships.

The [RadialLayout] class is an extension defined at [RadialLayout.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Layouts/Radial/RadialLayout.cs).
The override of the [RadialLayout.RotateNode] sets the `Angle`,
`Sweep`, and `Radius` data properties.
Bindings in the node template use those properties to produce the appropriate [Shape.Geometry]
and the [GraphObject.Alignment] and [GraphObject.Angle] for each [TextBlock].