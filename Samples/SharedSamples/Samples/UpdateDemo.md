This sample has two Diagrams, named "blueDiagram" and "greenDiagram", that display the same Model.
Each diagram uses its own templates for its nodes and links, causing the appearance of each diagram to be different.
However making a change in one diagram that changes the model causes those model changes to be reflected in the other diagram.

This sample also shows, next to the blue diagram, almost all of the [ChangedEvent]s that the shared model undergoes.
(For clarity it leaves out some of the Transaction - oriented events.)
The model Changed listener adds a line for each ChangedEvent to the log.
Transaction notification events start with an asterisk "*",
while property changes and collection insertions and removals start with an exclamation mark "!".

Next to the green diagram there is a tree view display of the [UndoManager]'s history.
The [UndoManager.History] is a List of [Transaction]s,
where each [Transaction.Changes] property holds all of the ChangedEvents that occurred due to some command or tool operation.
These ChangedEvents are reflective of both changes to the model (prefixed with "!m") and to the diagram (prefixed with "!d").
You will note that there are often several diagram changes for each model change.

This demo is different from the [Two Diagrams](TwoDiagrams) sample, which is an example of two Diagrams,
each sharing/showing a different [Model], but sharing the same [UndoManager].

Many of the other samples demonstrate saving the whole model by calling [Model.ToJson].
If you want to save incrementally, you should do so at the end of each transaction, when [ChangedEvent.IsTransactionFinished].
The [ChangedEvent.Object] may be a [Transaction].
Look through the [Transaction.Changes] list for the model changes that you want to save.