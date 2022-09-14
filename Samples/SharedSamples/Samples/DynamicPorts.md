Double-click in the diagram background in order to add a new node there.
In this sample you can add ports to a selected node by clicking the above buttons or by using the context menu.
Draw links between ports by dragging between ports.
If you select a link you can relink or reshape it.
Right-click or touch-hold on a port to bring up a context menu that allows you to remove it or change its color.

The diagram also uses a custom link to allow for special routing to help parallel links avoid each other
using overridden [Link.ComputeEndSegmentLength], [Link.HasCurviness], and [Link.ComputeCurviness]
functions.

See the [Ports Intro page](intro/ports.html) for an explanation of GoDiagram ports.