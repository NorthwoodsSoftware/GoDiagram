A **system dynamics diagram** shows the storages (stocks) and flows of material in some system,
and the factors that influence the rates of flow.
It is usually a cosmetic interface for building mathematical models --
you provide values and equations for the stocks and flows,
and appropriate software can then simulate the system's behaiour.

The diagram has two types of link: flow links and influence links.
In additon to the node attached to each flow, there are 3 types of node:
  + **stocks**, the amount of some substance
  + **clouds**, like stocks, but outside the system of interest
  + **variables**, either numeric constants or calculated from other elements

The conventional user interface for building system dynamics diagrams is modal --
you select a tool in the toolbar, then either click in an empty part of the diagram to add a node
or drag from one node to another to add a link.
That is the approach used in this example, accomplished with the [ClickCreatingTool] and [LinkingTool].
Note that you need to click on the Pointer tool to revert to the normal mode.

In addition to the above, the diagram also installs the [NodeLabelDraggingTool](demo/NodeLabelDragging)
extension into [ToolManager.MouseMoveTools].

This sample is based on a prototype developed by Robert Muetzelfeldt.