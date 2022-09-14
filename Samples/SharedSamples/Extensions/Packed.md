This sample demonstrates a custom Layout, [PackedLayout], which attempts to pack nodes as close together as possible without overlap.
Each node is assumed to be either rectangular or circular (dictated by the 'HasCircularNodes' property). This layout supports packing
nodes into either a rectangle or an ellipse, with the shape determined by the PackShape and the aspect ratio determined by either the
AspectRatio property, or the specified width and height (depending on the PackMode).

The layout is defined in its own file, as [PackedLayout.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Layouts/Packed/PackedLayout.cs),
with an additional dependency on [Quadtree.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Layouts/Packed/Quadtree.cs).