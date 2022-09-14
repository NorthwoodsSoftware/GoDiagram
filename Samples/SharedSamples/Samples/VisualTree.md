This sample shows the actual visual tree of a running Diagram.
The Diagram that we inspect is named "_Diagram" and initially contains two simple Nodes and two Links.
The Diagram below it is named "_VisualTree" and shows the visual tree of "_Diagram".

You can also try selecting, copying, and deleting parts in **_Diagram** and then click on "Draw Visual Tree" again to see how the visual tree in **_Diagram** changes.

The **TraverseVisualTree** method is what walks the visual tree of "_Diagram" and constructs the corresponding Nodes and Links used in "_VisualTree".
The text for each Node in "_VisualTree" is data-bound to the actual Diagram/Layer/Part/GraphObject object. That object is converted to a text string by using the ToString method.