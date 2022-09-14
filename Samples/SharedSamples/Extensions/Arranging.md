This sample demonstrates a custom Layout, [ArrangingLayout], that provides layouts of layouts.
It assumes the graph should be split up and laid out by potentially three separate Layouts.

The first step of ArrangingLayout is that all unconnected nodes are separated out to be laid out later by
the [ArrangingLayout.SideLayout], which by default is a [GridLayout].

The remaining nodes and links are partitioned into separate subgraphs with no links between subgraphs.
The [ArrangingLayout.PrimaryLayout] is performed on each subgraph.

If there is more than one subgraph, those subgraphs are treated as if they were individual nodes and are
laid out by the [ArrangingLayout.ArrangeLayout].

Finally the unconnected nodes are laid out by [ArrangingLayout.SideLayout] and they are all positioned
at the [ArrangingLayout.Side] Spot relative to the main body of nodes and links.

This extension layout is defined in its own file, as [ArrangingLayout.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Layouts/Arranging/ArrangingLayout.cs).