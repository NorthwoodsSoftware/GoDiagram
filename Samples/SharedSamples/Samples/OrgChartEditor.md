This editable organizational chart sample color-codes the Nodes according to the tree level in the hierarchy.

Double click on a node in order to add a person or the diagram background to add a new boss. Double clicking the diagram uses the [ClickCreatingTool]
with a custom [ClickCreatingTool.InsertPart] to scroll to the new node and start editing the [TextBlock] for its name .

Drag a node onto another in order to change relationships.
You can also draw a link from a node's background to other nodes that have no "boss". Links can also be relinked to change relationships.
Right-click or tap-hold a Node to bring up a context menu which allows you to:
  + Vacate Position - remove the information specfic to the current person in that role
  + Remove Role - removes the role entirely and reparents any children
  + Remove Department - removes the role and the whole subtree

Deleting a Node or Link will orphan the child Nodes and generate a new tree. A custom SelectionDeleting [DiagramEvent] listener will clear out the boss info
when the parent is removed.

Select a node to edit/update node data values. This sample uses the [DataInspector](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Input/DataInspector/DataInspector.cs) extension to display and modify Part data.

To learn how to build an org chart from scratch with GoDiagram, see the [Getting Started tutorial](learn/index.html).

If you want to have some "assistant" nodes on the side, above the regular reports,
see the [Org Chart Assistants](demo/OrgChartAssistants) sample, which is a copy of this sample
that uses a custom [TreeLayout] to position "assistants" that way.