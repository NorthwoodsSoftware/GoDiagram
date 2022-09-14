This "friend wheel" demonstrates the use of [CircularLayout].
The layout has been customized to make sure each node is considered to have a fixed diameter,
ignoring the size of any [TextBlock].

The custom layout also rotates each [Node] according to the actual angle at which the node was positioned.
This information is available on the [CircularVertex] used by the [LayoutNetwork] that
the [CircularLayout] constructs from the nodes and links of the diagram.
Furthermore, when laying out the nodes it also flips the angle of the [TextBlock] so that the
text is not upside-down.

[GraphObject.MouseEnter] and [GraphObject.MouseLeave] event handlers on the [Node] template
highlight both the Node and all of the Links that connect with the Node.
The same event handlers on the [Links] highlight that Link and both connected Nodes.

Changes made in these event handlers automatically are not recorded in the [UndoManager],
although this sample does not enable the UndoManager anyway.