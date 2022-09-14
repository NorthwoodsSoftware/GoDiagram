This sample is a modification of the [State Chart](demo/StateChart) sample
that makes use of the [CurvedLinkReshapingTool] that is defined in its own file,
as [CurvedLinkReshapingTool.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Tools/CurvedLinkReshaping/CurvedLinkReshapingTool.cs).

Note that unlike the standard case of a Bezier - curved Link that is [Part.Reshapable], there is only one reshape handle
When the user drags that handle, the value of [Link.Curviness] is modified, causing the link to be curved differently.
This sample also defines a TwoWay [Binding] on that property, thereby saving the curviness to the model data.
Unlike the regular State Chart sample, there is no Binding on [Link.Points], which is no longer needed when the curviness is the only modified property.