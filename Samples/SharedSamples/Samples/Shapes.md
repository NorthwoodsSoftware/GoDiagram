This sample showcases all predefined **GoDiagram** figures.
This sample also makes use of [GoDiagram Highlighting](intro/highlighting.html) data bindings: Mouse-hover over a shape to see its name.
             
You can specify a predefined geometry for a [Shape] by setting its [Shape.Figure].

In order to reduce the size of the GoDiagram library, most predefined figures are in the
[Figures.cs](https://github.com/NorthwoodsSoftware/GoDiagram/blob/main/Extensions/GraphObjects/Figures/Figures.cs) file.
You can load this file or simply load only those figures that you want to use by copying their definitions into your code.

A number of very common figures are predefined in GoDiagram: `"Rectangle", "Square", "RoundedRectangle", "Border", "Ellipse", "Circle", "TriangleRight",
"TriangleDown", "TriangleLeft", "TriangleUp", "Triangle", "Diamond", "LineH", "LineV", "BarH", "BarV", "MinusLine", "PlusLine", "XLine"`.
These figures are filled green above, instead of pink.

With GoDiagram you can also define your own custom shapes with SVG-like path syntax, see the [SVG Icons](demo/Icons)
sample for examples or the [Geometry Path Strings intro page](intro/geometry.html) to learn more.

For predefined arrowheads, see the [Arrowheads](demo/Arrowheads) sample.