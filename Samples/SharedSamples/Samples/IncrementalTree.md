This diagram demonstrates the expansion of a tree where nodes are only created "on-demand",
when the user clicks on the "Expand" Button.
As you expand the tree, it automatically performs a force-directed layout,
which will make some room for the additional nodes.

The data for each node is implemented by an object held by the Diagram's model.
Each node data has an **EverExpanded** property that indicates whether we have already
created all of its "child" data and added them to the model.
The **EverExpanded** property defaults to false,
to match the initial value of [Node.IsTreeExpanded] in the node template,
which specifies that the nodes start collapsed.

When the user clicks on the "Expand" Button, the button click event handler finds the corresponding
data object by going up the visual tree to find the Node via the [GraphObject.Part] property
and then getting the node data that the Node is bound to via [Part.Data].
If **EverExpanded** is false, the code creates a random number of
child data for that node.
Then the button click event handler changes the value of **Node.IsExpandedTree**,
thereby expanding or collapsing the node.

Some initial node expansions result in there being no children at all.
In this case the Button is made invisible.

All changes are performed within a transaction.
You should always surround your code with calls to [Model.Commit],
or the same method on [Diagram].

The diagram's [Diagram.Layout] is an instance of [ForceDirectedLayout].
The standard conditions under which the layout will be performed include
when nodes or links or group-memberships are added or removed from the model,
or when they change visibility.
In this case that means that when the user expands or collapses a node,
another force-directed layout occurs again, even upon repeated expansions or collapses.