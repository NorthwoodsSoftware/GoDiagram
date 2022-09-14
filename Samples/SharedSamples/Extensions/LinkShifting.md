This sample demonstrates the [LinkShiftingTool], which is an extra tool
that can be installed in the ToolManager to allow users to shift the end
point of the link to be anywhere along the sides of the port with which it
remains connected.

This only looks good for ports that occupy the whole of a rectangular node.
If you want to restrict the user's permitted sides, you can adapt the
[LinkShiftingTool.DoReshape] method to do what you want.