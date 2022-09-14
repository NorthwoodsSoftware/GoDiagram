This sample demonstrates the ability for the user to drag around a Link as if it were a Node.
When either end of the link passes over a valid port, the port is highlighted.

The link-dragging functionality is enabled by setting some or all of the following properties:
[DraggingTool.DragsLink], [LinkingTool.IsUnconnectedLinkValid], and
[RelinkingTool.IsUnconnectedLinkValid].

Note that a Link is present in the [Palette] so that it too can be dragged out and onto
the main Diagram.  Because links are not automatically routed when either end is not connected
with a Node, the route is provided explicitly when that Palette item is defined.

This also demonstrates several custom Adornments:
[Part.SelectionAdornmentTemplate], [Part.ResizeAdornmentTemplate], and
[Part.RotateAdornmentTemplate].

Finally this sample demonstrates saving and restoring the [Diagram.Position] as a property
on the [Model.SharedData] object that is automatically saved and restored when calling [Model.ToJson]
and [Model.FromJson].