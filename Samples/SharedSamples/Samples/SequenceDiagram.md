A **sequence diagram** is an interaction diagram that shows how entities operate with one another and in what order.
In this sample, we show the interaction between different people in a restaurant.

The diagram uses the [Diagram.GroupTemplate] for "lifelines,"
[Diagram.NodeTemplate] for "activities," and [Diagram.LinkTemplate] for "messages" between the entities.

Also featured are a custom Link class and custom [LinkingTool] to draw links
between lifelines and create activities at the end of the new link. Nodes use a binding function on the location
property to ensure they are anchored to their lifeline.