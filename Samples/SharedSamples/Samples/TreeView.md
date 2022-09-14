This shows how to create a traditional "TreeView" in a **GoDiagram** diagram.
There are 500 nodes in the tree.

The node template makes use of a "TreeExpanderButton" panel to implement the expand/collapse button.
It also implements a custom DoubleClick function to allow nodes to be expanded/collapsed on double-click.
Lastly, the source of the picture on each node is bound to two different properties, [Node.IsTreeLeaf] and
[Node.IsTreeExpanded]; the **imageConverter** function is used to select the correct image
based on these properties.

There are two link templates in the source code, one which uses no lines, and one which connects the items with dotted lines.

See the [Intro page on Buttons](intro/buttons.html) for more GoDiagram button information.
The [Tri-state CheckBox Tree](demo/TriStateCheckBoxTree) sample demonstrates a "tree view" where each item
has a three - state checkbox. The [Tree Mapper](demo/TreeMapper) sample demonstrates how to map (draw associations) between items in two trees.
The [Update Demo](demo/UpdateDemo) sample also uses a "tree view" for its own purposes.

The icons in this sample are from [icons8.com](https://icons8.com).