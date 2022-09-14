In this design each swimlane is implemented by a [Group], and all lanes are inside a "Pool" Group.
Each lane Group has its own [Group.Layout], which in this case is a [LayeredDigraphLayout].
Each pool Group has its own custom [GridLayout] that arranges all of its lanes in a vertical stack.
That custom layout makes sure all of the pool's lanes have the same length.
If you don't want each lane/group to have its own layout,
you could use set the lane group's [Group.Layout] to null and set the pool group's
[Group.Layout] to an instance of [SwimLaneLayout], shown at [Swim Lane Layout](demo/SwimLaneLayout).
         
When dragging nodes note that the nodes are limited to stay within the lanes.
This is implemented by a custom [Part.DragComputation] function, here named **stayInGroup**.
Hold down the Shift key while dragging simple nodes to move the selection to another lane.
Lane groups cannot be moved between pool groups.
        
A Group (i.e. swimlane) is movable but not copyable.
When the user moves a lane up or down the lanes automatically re-order.
You can prevent lanes from being moved and thus re-ordered by setting Group.Movable to false.
        
Each Group is collapsible.
The previous breadth of that lane is saved in the SavedBreadth property, to be restored when expanded.
        
When a Group/lane is selected, its custom [Part.ResizeAdornmentTemplate]
gives it a broad resize handle at the bottom of the Group
and a broad resize handle at the right side of the Group.
This allows the user to resize the "breadth" of the selected lane
as well as the "length" of all of the lanes.
However, the custom [ResizingTool] prevents the lane from being too narrow
to hold the [Group.Placeholder] that represents the subgraph,
and it prevents the lane from being too short to hold any of the contents of the lanes.
Each Group/lane is also has a [GraphObject.MinSize] to keep it from
being too narrow even if there are no member [Part]s at all.
        
A different sample has its swim lanes vertically oriented: [Swim Lanes (vertical)](demo/SwimLanesVertical).