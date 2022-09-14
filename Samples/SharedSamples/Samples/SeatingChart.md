This sample demonstrates how a "Person" node can be dropped onto a "Table" node,
causing the person to be assigned a position at the closest empty seat at that table.
When a person is dropped into the background of the diagram, that person is no longer assigned a seat.
People dragged between diagrams are automatically removed from the diagram they came from.

"Table" nodes are defined by separate templates, to permit maximum customization of the shapes and
sizes and positions of the tables and chairs.

"Person" nodes in the `_Guests` diagram can also represent a group of people,
for example a named person plus one whose name might not be known.
When such a person is dropped onto a table, additional nodes are created in `_Diagram`.
Those people are seated at the table if there is room.

Tables can be moved or rotated. Moving or rotating a table automatically repositions the people seated at that table.

The [UndoManager] is shared between the two Diagrams, so that one can undo/redo in either diagram
and have it automatically handle drags between diagrams, as well as the usual changes within the diagram.