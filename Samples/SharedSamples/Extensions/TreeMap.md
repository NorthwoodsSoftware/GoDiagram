This sample demonstrates a custom Layout, [TreeMapLayout], which assumes that the diagram consists of nested Groups and simple Nodes.
Each node is positioned and sized to fill an area of the viewport proportionate to its "size", as determined by its Node.Data.Size property.
Each Group gets a size that is the sum of all of its member Nodes.

The layout is defined in its own file, as [TreeMapLayout.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Layouts/TreeMap/TreeMapLayout.cs).

Clicking repeatedly at the same point will initially select the Node at that point, and then its containing Group, and so on up the chain of containers.