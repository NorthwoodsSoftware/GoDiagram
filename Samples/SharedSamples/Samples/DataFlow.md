This sample demonstrates a data flow or workflow graph with labeled ports on nodes.
A real application would provide the ability to edit the details (properties)
of each node so that the actual database operation could be executed.

The ports are set up as panels, created within the **makePort** function.
This function sets various properties of the [Shape] and
[TextBlock] that make up the panel, and properties of the panel itself. Most notable are
[GraphObject.PortId] to declare the shape as a port, and [GraphObject.FromLinkable] and
[GraphObject.ToLinkable] to set the way the ports can be linked.

The diagram also uses the **makeTemplate** function to create the node templates with shared features.
This function takes a type, an image, a background color, and arrays of ports to create the node
to be added to the [Diagram.NodeTemplateMap].

For the same data model rendered somewhat differently, see the [Data Flow (vertical)](demo/DataFlowVertical) sample.