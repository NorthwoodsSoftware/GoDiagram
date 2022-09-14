This sample demonstrates subgraphs that are created only as groups are expanded.
  
The model is initially a random number of nodes, including some groups, in a tree layout.
When a group is expanded, the [Group.SubGraphExpandedChanged] event handler calls a function to generate a random number of nodes
in a tree layout inside the group if it did not contain none any.
Each non-group node added has a unique random color, and links are added by giving each node one link to another node.
  
The addition of nodes and links is performed within a transaction to ensure that the diagram updates itself properly.
The diagram's tree layout and the tree layouts within each group are performed again when a sub-graph is expanded or collapsed.