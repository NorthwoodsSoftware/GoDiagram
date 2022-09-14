Click on a Node to center it and show its relationships.
It is also easy to add more information to each node, including pictures,
or to put such information into [Tooltips](intro/toolTips.html).

The `RadialLayout` class is an extension defined at [RadialLayout.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Layouts/Radial/RadialLayout.cs).
You can control how many layers to show, whether to draw the circles, and whether to rotate the text, by modifying
RadialLayout properties or changing overrides of `RadialLayout.RotateNode` and/or `RadialLayout.CommitLayers`.