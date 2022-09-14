This sample demonstrates the [PolylineLinkingTool], which replaces the standard LinkingTool.
The tool is defined in its own file, as [PolylineLinkingTool.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/PolylineLinking/PolylineLinkingTool.cs).

The user starts drawing a new link from a node in the normal manner, by dragging from a port,
which for feedback purposes has a "pointer" cursor.
Normally the user would have to release the mouse near the target port/node.
However with the PolylineLinkingTool the user may click at various points to cause the new link
to be routed along those points.
Clicking on the target port completes the new link.
Press **Escape** to cancel, or **Z** to remove the last point.

Furthermore, because [Link.Resegmentable] is true, the user can easily add or remove segments
from the route of a selected link. To insert a segment, the user can start dragging the small
diamond resegmenting handle. To remove a segment, the user needs to move a regular reshaping handle
to cause the adjacent two segments to be in a straight line.

The PolylineLinkingTool also works with orthogonally routed links.
To demonstrate this, uncomment the two lines that initialize [Link.Routing] to be [LinkRouting.Orthogonal].