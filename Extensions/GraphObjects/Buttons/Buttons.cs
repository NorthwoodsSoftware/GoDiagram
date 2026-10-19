/*
*  Copyright (c) Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main GoDiagram library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in the GoDiagram repository (https://github.com/NorthwoodsSoftware/GoDiagram/tree/main/Extensions).
* See the Extensions intro page (https://godiagram.com/intro/extensions.html) for more information.
*/

using System;
using Northwoods.Go.Models;
using Northwoods.Go.PanelLayouts;

/// These are the definitions for all of the predefined buttons.
/// You do not need to load this file in order to use buttons.
///
/// A 'Button' is a Panel that has a Shape surrounding some content
/// and that has MouseEnter/MouseLeave behavior to highlight the button.
/// The content of the button, whether a TextBlock or a Picture or a complicated Panel,
/// must be supplied by the caller.
/// The caller must also provide a click event handler.
///
/// Typical usage:
///   Builder.Make<Panel>("Button").Add(
///     new TextBlock("Click me!")  // the content is just the text label
///   ).Set(new {
///     Click = new Action<InputEvent, GraphObject>((e, obj) => {
///       Console.WriteLine("I was clicked");
///     })
///   });

namespace Northwoods.Go.Extensions {
  public class Buttons {
    public static void DefineButtons() {
      Builder.DefineBuilder("Button", (args) => {
        // default colors for 'Button' shape
        var buttonFillNormal = "#f5f5f5";
        var buttonStrokeNormal = "#737373";
        var buttonFillOver = "#d4d4d4";
        var buttonStrokeOver = "#737373";
        var buttonFillDisabled = "#a3a3a3";

        // padding inside the ButtonBorder to match sizing from previous versions
        var paddingHorizontal = 2.76142374915397;
        var paddingVertical = 2.761423749153969;

        var button =
          new Panel(PanelLayoutAuto.Instance) {
            IsActionable = true,
            EnabledChanged = new Action<GraphObject, bool>((btn, enabled) => {
              if (btn is Panel bpan) {
                if (bpan.FindElement("ButtonBorder") is Shape shape) {
                  saveNormal(bpan, shape);
                  if (enabled) {
                    // if the mouse is over the button, show the "over" fill
                    GraphObject fnd = null;
                    var diagram = bpan.Diagram;
                    if (bpan.Layer != null && diagram != null && bpan.IsVisibleElement() && diagram.LastInput.DocumentPoint.IsReal()) {
                      fnd = bpan.Layer.FindElementAt<GraphObject>(diagram.LastInput.DocumentPoint);
                    }
                    if (fnd != null && (fnd == bpan || fnd.IsContainedBy(bpan))) {
                      shape.Fill = asBrush(bpan["_ButtonFillOver"]);
                    } else {
                      shape.Fill = asBrush(bpan["_ButtonFillNormal"]);
                    }
                  } else {
                    shape.Fill = asBrush(bpan["_ButtonFillDisabled"]);
                  }
                }
              }
            }),
            Cursor = "Pointer"
          }.Set(new {
            // save these values for the MouseEnter and MouseLeave event handlers;
            // _ButtonFillNormal and _ButtonStrokeNormal are taken from the ButtonBorder when first needed,
            // so that any customization of the ButtonBorder's Fill and Stroke is respected
            _ButtonFillOver = buttonFillOver,
            _ButtonStrokeOver = buttonStrokeOver,
            _ButtonFillDisabled = buttonFillDisabled
          }).Add(
            new Shape("RoundedRectangle") {
              Name = "ButtonBorder",
              Spot1 = new Spot(0, 0, paddingHorizontal, paddingVertical),
              Spot2 = new Spot(1, 1, -paddingHorizontal, -paddingVertical),
              Parameter1 = 2,
              Fill = buttonFillNormal,
              Stroke = buttonStrokeNormal
            }
          );

        // remember the normal Fill and Stroke of the ButtonBorder, unless already known
        void saveNormal(Panel btn, Shape bb) {
          if (btn["_ButtonFillNormal"] == null) btn["_ButtonFillNormal"] = bb.Fill;
          if (btn["_ButtonStrokeNormal"] == null) btn["_ButtonStrokeNormal"] = bb.Stroke;
        }

        Brush asBrush(object b) {
          if (b is Brush br) return br;
          if (b is string s) return s;
          return default;
        }

        button.MouseEnter = (e, obj, prev) => {
          if (!obj.IsEnabledElement()) return;
          if (!(obj is Panel btn)) return;
          if (btn.FindElement("ButtonBorder") is Shape bb) {  // the border Shape
            saveNormal(btn, bb);
            bb.Fill = asBrush(btn["_ButtonFillOver"]);
            bb.Stroke = asBrush(btn["_ButtonStrokeOver"]);
          }
        };

        button.MouseLeave = (e, obj, prev) => {
          if (!obj.IsEnabledElement()) return;
          if (!(obj is Panel btn)) return;
          if (btn.FindElement("ButtonBorder") is Shape bb) {  // the border Shape
            // restore the normal fill and stroke
            if (btn["_ButtonFillNormal"] != null) bb.Fill = asBrush(btn["_ButtonFillNormal"]);
            if (btn["_ButtonStrokeNormal"] != null) bb.Stroke = asBrush(btn["_ButtonStrokeNormal"]);
          }
        };

        return button;
      });

      // This is a complete Button that you can have in a Node template
      // to allow the user to collapse/expand the subtree beginning at that Node.
      //
      // Typical usage within a Node template:
      //   Builder.Make<Panel>("TreeExpanderButton")
      Builder.DefineBuilder("TreeExpanderButton", (args) => {
        var button = Builder.Make<Panel>("Button").Set(new {
          // set these values for the IsTreeExpanded binding conversion
          _TreeExpandedFigure = "MinusLine",
          _TreeCollapsedFigure = "PlusLine",
          // assume initially not visible because there are no links coming out
          Visible = false
        }).Add(
          new Shape("MinusLine") {  // default value for IsTreeExpanded is true
            Name = "ButtonIcon",
            Stroke = "#0a0a0a",
            StrokeWidth = 2,
            DesiredSize = new Size(8, 8)
          }
          // bind the Shape.Figure to the Node.IsTreeExpanded value using this converter:
          .Bind(new Binding("Figure", "IsTreeExpanded", (exp, shape) => {
            var but = ((Shape)shape).Panel;
            return (bool)exp ? but["_TreeExpandedFigure"] : but["_TreeCollapsedFigure"];
          }).OfElement())
        )
        // bind the button visibility to whether it's not a leaf node
        .Bind(new Binding("Visible", "IsTreeLeaf", (leaf, obj) => !(bool)leaf).OfElement());

        // tree expand/collapse behavior
        button.Click = (e, btn) => {
          var part = btn.Part;
          if (part is Adornment ad) part = ad.AdornedPart;
          if (!(part is Node node)) return;
          var diagram = node.Diagram;
          if (diagram == null) return;
          var cmd = diagram.CommandHandler;
          if (node.IsTreeExpanded) {
            if (!cmd.CanCollapseTree(node)) return;
          } else {
            if (!cmd.CanExpandTree(node)) return;
          }
          e.Handled = true;
          if (node.IsTreeExpanded) {
            cmd.CollapseTree(node);
          } else {
            cmd.ExpandTree(node);
          }
        };

        return button;
      });

      // This is a complete Button that you can have in a Group template
      // to allow the user to collapse/expand the subgraph that the Group holds.
      //
      // Typical usage within a Group template:
      //   Builder.Make<Panel>("SubGraphExpanderButton")
      Builder.DefineBuilder("SubGraphExpanderButton", (args) => {
        var button = Builder.Make<Panel>("Button").Set(new {
          // set these values for the IsSubGraphExpanded binding conversion
          _SubGraphExpandedFigure = "MinusLine",
          _SubGraphCollapsedFigure = "PlusLine"
        }).Add(
          new Shape("MinusLine") {  // default value for IsSubGraphExpanded is true
            Name = "ButtonIcon",
            Stroke = "#0a0a0a",
            StrokeWidth = 2,
            DesiredSize = new Size(8, 8)
          }
          // bind the Shape.Figure to the Group.isSubGraphExpanded value using this converter:
          .Bind(new Binding("Figure", "IsSubGraphExpanded", (exp, shape) => {
            var but = ((Shape)shape).Panel;
            return (bool)exp ? but["_SubGraphExpandedFigure"] : but["_SubGraphCollapsedFigure"];
          }).OfElement())
        );

        // subgraph expand/collapse behavior
        button.Click = (e, btn) => {
          var part = btn.Part;
          if (part is Adornment ad) part = ad.AdornedPart;
          if (!(part is Group group)) return;
          var diagram = group.Diagram;
          if (diagram == null) return;
          var cmd = diagram.CommandHandler;
          if (group.IsSubGraphExpanded) {
            if (!cmd.CanCollapseSubGraph(group)) return;
          } else {
            if (!cmd.CanExpandSubGraph(group)) return;
          }
          e.Handled = true;
          if (group.IsSubGraphExpanded) {
            cmd.CollapseSubGraph(group);
          } else {
            cmd.ExpandSubGraph(group);
          }
        };

        return button;
      });

      // This is just an "Auto" Adornment that can hold some contents within a light gray, shadowed box.
      //
      // Typical usage:
      //   ToolTip =
      //     Builder.Make<Adornment>("ToolTip").Add(
      //       new TextBlock { ... }
      //     )
      Builder.DefineBuilder("ToolTip", (args) => {
        var ad = new Adornment(PanelLayoutAuto.Instance) {
          IsShadowed = true,
          ShadowColor = "rgba(0, 0, 0, .4)",
          ShadowOffset = new Point(0, 2),
          // keep the tooltip showing while the mouse is over it
          MouseOver = (e, obj) => {
            var mgr = e.Diagram.ToolManager;
            mgr.ExtendToolTip(mgr.ToolTipDuration);
          }
        }.Add(
          new Shape("RoundedRectangle") {
            Name = "Border",
            Parameter1 = 1,
            Fill = "#f5f5f5",
            StrokeWidth = 0,
            Spot1 = new Spot(0, 0, 4, 6),
            Spot2 = new Spot(1, 1, -4, -4)
          }
        );
        return ad;
      });

      // This is just a "Vertical" Adornment that can hold some "ContextMenuButton"s.
      //
      // Typical usage:
      //   ContextMenu =
      //     Builder.Make<Adornment>("ContextMenu").Add(
      //       Builder.Make<Panel>("ContextMenuButton").Add(
      //         new TextBlock { ... }
      //       ).Set(new {
      //           Click = ...
      //       }),
      //       Builder.Make<Panel>("ContextMenuButton") ...
      //     )
      Builder.DefineBuilder("ContextMenu", (args) => {
        var ad = new Adornment(PanelLayoutVertical.Instance) {
          Background = "#f5f5f5",
          IsShadowed = true,
          ShadowColor = "rgba(0, 0, 0, .4)",
          ShadowOffset = new Point(0, 2)
        }
        // don't set the background if the ContextMenu is adorning something and there's a Placeholder
        .Bind(new Binding("Background", "", (obj, _) => {
          if (!(obj is Adornment adorn)) return null;
          var part = adorn.AdornedPart;
          if (part != null && adorn.HasPlaceholder()) return null;
          return "#f5f5f5";
        }).OfElement());
        return ad;
      });

      // This just holds the 'ButtonBorder' Shape that acts as the border
      // around the button contents, which must be supplied by the caller.
      // The button contents are usually a TextBlock or Panel consisting of a Shape and a TextBlock.
      //
      // Typical usage within an Adornment that is either a GraphObject.ContextMenu or a Diagram.ContextMenu:
      //   Builder.Make<Panel>("ContextMenuButton").Add(
      //     new TextBlock { ... }
      //   ).Set(new {
      //     Click = new Action<InputEvent, GraphObject>((e, obj) => {
      //       Console.WriteLine("Command for " + (obj.Part as Adornment).AdornedPart);
      //     })
      //   }).Bind("Visible", "", (data, _) => { return ...OK to perform Command...; })
      Builder.DefineBuilder("ContextMenuButton", (args) => {
        var button = Builder.Make<Panel>("Button");
        button.Stretch = Stretch.Horizontal;
        var border = button.FindElement("ButtonBorder");
        if (border is Shape shp) {
          shp.Figure = "Rectangle";
          shp.StrokeWidth = 0;
          shp.Spot1 = new Spot(0, 0, 4, 6);
          shp.Spot2 = new Spot(1, 1, -4, -4);
        }

        return button;
      });

      // This button is used to toggle the visibility of a GraphObject named
      // by the second argument to Builder.Make.  If the second argument is not present
      // or if it is not a string, this assumes that the element name is 'COLLAPSIBLE'.
      // You can only control the visibility of one element in a Part at a time,
      // although that element might be an arbitrarily complex Panel.
      //
      // Typical usage:
      //   new Panel { ... }.Add(
      //     Builder.Make<Panel>("PanelExpanderButton", "COLLAPSIBLE"),
      //     ...,
      //     new Panel { ..., Name = "COLLAPSIBLE" }.Add(
      //       ...stuff to be hidden or shown as the PanelExpanderButton is clicked...
      //     )
      //   )
      Builder.DefineBuilder("PanelExpanderButton", (args) => {
        var eltname = Builder.TakeBuilderArgument(ref args, "COLLAPSIBLE");

        var button = Builder.Make<Panel>("Button").Set(new {
          _ButtonExpandedFigure = "M0 0 M0 6 L4 2 8 6 M8 8",
          _ButtonCollapsedFigure = "M0 0 M0 2 L4 6 8 2 M8 8",
          _ButtonFillNormal = "rgba(0, 0, 0, 0)",
          _ButtonStrokeNormal = (Brush)null,
          _ButtonFillOver = "rgba(0, 0, 0, .2)",
          _ButtonStrokeOver = (Brush)null,
        }).Add(
          new Shape() {
            Name = "ButtonIcon",
            StrokeWidth = 2,
            GeometryString = "M0 0 M0 6 L4 2 8 6 M8 8"
          }.Bind(new Binding("GeometryString", "Visible", (vis, obj) => {
            var but = ((Shape)obj).Panel;  // obj is this shape, obj.Panel is the PanelExpanderButton
            return (bool)vis ? but["_ButtonExpandedFigure"] : but["_ButtonCollapsedFigure"];
          }).OfElement(eltname))
        );

        var border = button.FindElement("ButtonBorder");
        if (border is Shape shp) {
          shp.Stroke = null;
          shp.Fill = "rgba(0, 0, 0, 0)";
        }

        button.Click = (e, btn) => {
          var diagram = btn.Diagram;
          if (diagram == null) return;
          if (diagram.IsReadOnly) return;
          var elt = btn.FindBindingPanel();
          if (elt == null) elt = btn.Part;
          if (elt != null) {
            if (elt.FindElement(eltname) is GraphObject pan) {
              e.Handled = true;
              diagram.StartTransaction("Collapse/Expand Panel");
              pan.Visible = !pan.Visible;
              diagram.CommitTransaction("Collapse/Expand Panel");
            }
          }
        };

        return button;
      });

      // Define a common checkbox button; the first argument is the name of the data property
      // to which the state of this checkbox is data bound.  If the first argument is not a string,
      // it raises an error.  If no data binding of the checked state is desired,
      // pass an empty string as the first argument.
      //
      // Examples:
      //   Builder.Make<Panel>("CheckBoxButton", "DataPropertyName").Set(...)
      // or:
      //   Builder.Make<Panel>("CheckBoxButton", "").Set(new {
      //     _DoClick = new Action<InputEvent, GraphObject>((e, obj) => {
      //       Console.WriteLine("Clicked!");
      //     })
      //   })
      Builder.DefineBuilder("CheckBoxButton", (args) => {
        // process the one required string argument for this kind of button
        var propname = Builder.TakeBuilderArgument(ref args);

        var button = Builder.Make<Panel>("Button").Set(new {
          DesiredSize = new Size(14, 14)
        }).Add(
          new Shape() {
            Name = "ButtonIcon",
            GeometryString = "M0 0 M0 8.85 L4.9 13.75 16.2 2.45 M16.2 16.2",  // a "check" mark
            StrokeWidth = 2,
            Stretch = Stretch.Fill,  // this Shape expands to fill the Button
            GeometryStretch = GeometryStretch.Uniform,  // the check mark fills the Shape without distortion
            Visible = false  // visible set to false: not checked, unless data.PROPNAME is true
          }
          // create a data Binding only if PROPNAME is supplied and not the empty string
          .Bind(propname != "" ? new Binding("Visible", propname).MakeTwoWay() : null)
        );

        if (button.FindElement("ButtonBorder") is Shape cbborder) {  // a smaller inset, so the check mark is bigger
          cbborder.Spot1 = new Spot(0, 0, 1, 1);
          cbborder.Spot2 = new Spot(1, 1, -1, -1);
        }

        button.Click = (e, obj) => {
          if (!(obj is Panel btn)) return;
          var diagram = e.Diagram;
          if (diagram == null || diagram.IsReadOnly) return;
          if (propname != "" && diagram.Model.IsReadOnly) return;
          e.Handled = true;
          var shape = btn.FindElement("ButtonIcon");
          diagram.StartTransaction("checkbox");
          if (shape != null) shape.Visible = !shape.Visible;  // this toggles data.checked due to TwoWay Binding
          // support extra side-effects without clobbering the click event handler:
          if (btn["_DoClick"] is Action<InputEvent, GraphObject> fn) fn(e, btn);
          diagram.CommitTransaction("checkbox");
        };

        return button;
      });

      // This defines a whole check-box -- including both a 'CheckBoxButton' and whatever you want as the check box label.
      // Note that MouseEnter/MouseLeave/Click events apply to everything in the panel, not just in the 'CheckBoxButton'.
      //
      // Examples:
      //   Builder.Make<Panel>("CheckBox", "ABooleanDataProperty").Add(
      //     new TextBlock("the checkbox label")
      //   )
      // or
      //   Builder.Make<Panel>("CheckBox", "SomeProperty").Set(new {
      //     _DoClick = new Action<InputEvent, GraphObject>((e, obj) => {
      //       ...perform extra side-effects...
      //     })
      //   }).Add(
      //     new TextBlock("A Choice")
      //   )
      Builder.DefineBuilder("CheckBox", (args) => {
        // process the one required string argument for this kind of button
        var propname = Builder.TakeBuilderArgument(ref args);

        var button = Builder.Make<Panel>("CheckBoxButton", propname).Set(
          new {
            Name = "Button",
            IsActionable = false,  // actionable is set on the whole horizontal panel
            Margin = new Margin(0, 1, 0, 0)
          }
        );

        var box = new Panel(PanelLayoutHorizontal.Instance) {
          IsActionable = true,
          Cursor = button.Cursor,
          Margin = 1,
          MouseEnter = button.MouseEnter,
          MouseLeave = button.MouseLeave,
          Click = button.Click
        }.Set(new {
          // transfer CheckBoxButton properties over to this new CheckBox panel
          _ButtonFillNormal = button["_ButtonFillNormal"],
          _ButtonStrokeNormal = button["_ButtonStrokeNormal"],
          _ButtonFillOver = button["_ButtonFillOver"],
          _ButtonStrokeOver = button["_ButtonStrokeOver"],
          _ButtonFillDisabled = button["_ButtonFillDisabled"],
          // also save original Button behavior, for potential use in a Panel.Click event handler
          _ButtonClick = button.Click
        }).Add(button);

        // avoid potentially conflicting event handlers on the "CheckBoxButton"
        button.MouseEnter = null;
        button.MouseLeave = null;
        button.Click = null;

        return box;
      });
    }
  }
}
