A mind map is a kind of spider diagram that organizes information around a central concept, with connecting branches.

The layout is controlled by moving the nodes closest to the tree's root node.
When one of these nodes is moved horizontally to the other side of the root,
all of its children will be sent to [Layout.DoLayout] with a new direction,
causing text to always be moved outwards from the root. The **spotConverter** function is used to manage
[GraphObject.FromSpot] and [GraphObject.ToSpot] for nodes manually, so the [TreeLayout.SetsPortSpot] and [TreeLayout.SetsChildPortSpot]
properties are set to false so that laying out the diagram will not overwrite the values.

When a node is deleted the [CommandHandler.DeletesTree] property ensures that
all of its children are deleted with it. When a node is dragged the [DraggingTool.DragsTree]
property ensures that all its children are dragged with it.
Both of these are set during the the Diagram's initalization.

Node templates also have a [Part.SelectionAdornmentTemplate] defined to allow for new nodes to be created and a [GraphObject.ContextMenu] with additional commands.