This sample creates a state chart to story-board an online shopping experience.

The text is editable for both the nodes and the links.
The user can draw as many links from one node to another node as desired,
and the links can be reshaped or deleted when selected.
Double-clicking in the background of the diagram creates a new node.
The mouse wheel is set to zoom in and out instead of scroll.

This sample customizes the [Part.SelectionAdornmentTemplate]
of the node to a template that contains a button
The button is positioned to be at the Top-Right corner of the node by
being in a Spot Panel with its [GraphObject.Alignment] property set to Spot.TopRight.

The Button's [GraphObject.Click] method creates a new node data,
adds it to the model, and creates a link from the original node data to the new node data.
All of this is done inside a transaction, so that it can be undone by the user
(Ctrl+Z and Ctrl+Y will undo and redo transactions). After the node is created,
[Diagram.ScrollToRect] is called in case it is off-screen.