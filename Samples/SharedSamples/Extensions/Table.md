This sample demonstrates a custom Layout, [TableLayout], that is very much like a simplified "Table" Panel layout,
but working on non-Link Parts in a Diagram or a Group rather than on GraphObjects in a Panel.
The layout is defined in its own file, as [TableLayout.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Layouts/Table/TableLayout.cs).

You can drag-and-drop nodes from the Palette into any Group.
Dragging into a Group highlights the Group.
Drops must occur inside Groups; otherwise the action is cancelled.

Each row and each column is [Part.Resizable] and has a custom [Part.ResizeAdornmentTemplate]
showing a single resize handle on the right side or on the bottom.
There is a custom LaneResizingTool to provide a minimum width or height based on the contents of all of the
groups (cells) in that column or row.

This example assumes the Groups are predefined and exist in each cell at a particular row/column,
but this sample could be extended to allow adding and removing rows and/or columns.