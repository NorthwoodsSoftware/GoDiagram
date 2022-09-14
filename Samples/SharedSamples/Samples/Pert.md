This sample demonstrates how to create a simple PERT chart. A PERT chart is a project management tool used to schedule and coordinate tasks within a project.
        
Each node represents an activity and displays several pieces of information about each one.
The node template is basically a [Panel] of type [PanelLayoutTable] holding several [TextBlock]s
that are data-bound to properties of the Activity, all surrounded by a rectangular border.
The lines separating the text are implemented by setting the [RowDefinition.SeparatorStroke]/[ColumnDefinition.SeparatorStroke]
for two columns and two rows. The separators are not seen in the middle because the middle row
of each node has its [RowDefinition.Background] set to white,
and [RowDefinition.CoversSeparators] set to true.
        
The "Critical" property on the activity data object controls whether the node is drawn with a red brush or a blue one.
There is a special converter that is used to determine the brush used by the links.
        
The light blue legend is implemented by a separate Part implemented in a manner similar to the Node template.
However it is not bound to data -- there is no object in the model representing the legend.