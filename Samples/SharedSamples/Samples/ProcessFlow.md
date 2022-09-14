A **process flow diagram** is commonly used in chemical and process engineering to indicate the general flow of plant processes and equipment.
A simple SCADA diagram, with animation of the flow along the pipes, is implemented here.

The diagram displays the background grid layer by setting **Grid.Visible** to true,
and also allows snapping to the grid using [DraggingTool.IsGridSnapEnabled],
[ResizingTool.IsGridSnapEnabled], and [RotatingTool.SnapAngleMultiple] alongside [RotatingTool.SnapAngleEpsilon].

There is also a custom animation that modifies the [Shape.StrokeDashOffset] for each link.