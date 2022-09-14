**GoDiagram** supports the notion of "Comment"s.
A "Comment" is a node that is linked with another node but is positioned by some layouts to go along with that other node,
rather than be laid out like a regular node and link.

In this sample there are three "Comment" nodes, connected with regular nodes by three "Comment" links.
Node and link data are marked as "Comment"s by specifying "Comment" as the category.
But the "Comment" nodes and links have a different default template, and thus a different appearance, than regular nodes and links.
You can specify your own templates for "Comment" nodes and "Comment" links.
The "Comment" link template defined here uses the `BalloonLink` class defined in
[BalloonLink.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/GraphObjects/BalloonLink/BalloonLink.cs) in the Extensions directory.