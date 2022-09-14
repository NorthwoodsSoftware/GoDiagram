This sample demonstrates a custom TreeLayout, [ParallelLayout],
which assumes that there is a single "Split" node that is the root of a tree,
other than links that connect with a single "Merge" node.
The layout is defined in its own file, as [ParallelLayout.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Layouts/Parallel/ParallelLayout.cs).

Both the [Diagram.Layout] and the [Group.Layout] are instances of ParallelLayout,
allowing for nested layouts that appear in parallel.