This sample demonstrates tooltips and context menus for all parts and for the diagram background,
as well as several other powerful [Diagram] editing abilities.

Unlike the [Minimal](demo/Minimal) sample, this sample has templates for Links and for Groups,
plus tooltips and context menus for Nodes, for Links, for Groups, and for the Diagram.

This sample has all of the functionality of the Minimal sample, but additionally allows the user to:
  + create new nodes: double-click in the background of the diagram
  + edit text: select the node and then click on the text, or select the node and press F2
  + draw new links: drag from the inner edge of the node's or the group's shape
  + reconnect existing links: select the link and then drag the diamond-shaped handle at either end of the link
  + group nodes and links: select some nodes and links and then type Ctrl-G (or invoke via context menu)
  + ungroup an existing group: select a group and then type Ctrl-Shift-G (or invoke via context menu)

GoDiagram contains many other possible commands, which can be invoked by either mouse/keyboard/touch or programatically.
[See an overview of possible commands here](intro/commands.html).
On a Mac, use CMD instead of Ctrl.

On touch devices, hold your finger stationary to bring up a context menu.
The default context menu supports most of the standard commands that are enabled at that time for that object.
