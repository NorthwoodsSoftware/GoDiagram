This demonstrates the ability for a [Link] to appear to connect with another Link.
Regular links are blue. Link-connecting links are green.
Try moving a node around to see how the links adapt.
Initially the "Alpha" node connects with the link between Gamma and Delta.
There is also a link between the two horizontal links.

This effect is achieved by using "label nodes" that belong to links.
Such "label nodes" are real [Node]s that are referenced from their owning [Link].
This sample customizes the "Link Label" Node template to allow the user to draw new links to/from such label nodes.

Newly drawn links automatically get a label node by the [LinkingTool] because this sample initializes
the [LinkingTool.ArchetypeLabelNodeData] property of the [ToolManager.LinkingTool].
The category (i.e. template) of each link is determined by what kinds of nodes the link is connected with.