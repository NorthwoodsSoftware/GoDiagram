This example demonstrates a custom [CommandHandler].
It allows the user to position selected Parts in a diagram relative to each other,
overrides [CommandHandler.DoKeyDown] to allow handling the arrow keys in additional manners,
and uses a "paste offset" so that pasting objects will cascade them rather than place them on top of one another.
It is defined in its own file, as [DrawCommandHandler.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/Input/DrawCommandHandler/DrawCommandHandler.cs).

The above buttons can be used to align Parts, rotate Parts, or change the behavior of the arrow keys.