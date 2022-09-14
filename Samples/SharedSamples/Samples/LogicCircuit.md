The Logic Circuit sample allows the user to make circuits using gates and wires,
which are updated whenever a Link is modified and at intervals by a looping function.

The **UpdateStates** function calls a function to update each node according to type,
which uses the color of the links into the node to determine the color of those exiting it.
Red means zero or false; green means one or true. Double-clicking an input node will toggle true/false.

Mouse over a node to see its category, displayed using a shared [Adornment] set as the tooltip.
A [Palette] to the left of the main diagram allows the user to drag and drop new nodes.
These nodes can then be linked using ports which are defined on the various node templates.
Each input port can only have one input link, while output ports can have many output links.
This is controlled by the [GraphObject.ToMaxLinks] property.