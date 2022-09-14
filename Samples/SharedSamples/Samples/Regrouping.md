This sample allows the user to drag nodes, including groups, into and out of groups,
both from the Palette as well as from within the Diagram.
See the [Groups Intro page](intro/groups.html) for an explanation of GoDiagram Groups.

Highlighting to show feedback about potential addition to a group during a drag is implemented
using [GraphObject.MouseDragEnter] and [GraphObject.MouseDragLeave] event handlers.
Because [Group.ComputesBoundsAfterDrag] is true, the Group's [Placeholder]'s bounds are
not computed until the drop happens, so the group does not continuously expand as the user drags
a member of a group.

When a drop occurs on a Group or a regular Node, the [GraphObject.MouseDrop] event handler
adds the selection (the dragged Nodes) to the Group as a new member.
The [Diagram.MouseDrop] event handler changes the dragged selected Nodes to be top-level,
rather than members of whatever Groups they had been in.

The slider controls how many nested levels of Groups are expanded.