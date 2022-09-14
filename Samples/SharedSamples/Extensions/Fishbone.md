This sample shows a "fishbone" layout of a tree model of cause-and-effect relationships.
This type of layout is often seen in root cause analysis, or RCA.
The layout is defined in its own file, as [FishboneLayout.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Layouts/Fishbone/FishboneLayout.cs).
When using [FishboneLayout] the diagram uses [FishboneLink] in order to get custom routing for the links.

The buttons each set the [Diagram.Layout] within a transaction.