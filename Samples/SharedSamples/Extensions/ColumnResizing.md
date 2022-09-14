This makes use of two tools, defined in their own files:
[ColumnResizingTool.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/ColumnResizing/ColumnResizingTool.cs) and
[RowResizingTool.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/RowResizing/RowResizingTool.cs).
Each tool adds an [Adornment] to a selected node that has a resize handle for each column or each row of a "Table" [Panel].
While resizing, you can press the Tab or the Delete key in order to stop the tool and restore the column or row to its natural size.

This sample also adds TwoWay Bindings to the [ColumnDefinition.Width] property for the columns.
Each column width is stored in the corresponding index of the node data's "Widths" property, which must be an Array of numbers.
The default value is NaN, allowing the column to occupy its natural width.
Note that there are **no** Bindings for the row heights.

See also the [Add & Remove Rows & Columns](demo/AddRemoveColumns) sample.