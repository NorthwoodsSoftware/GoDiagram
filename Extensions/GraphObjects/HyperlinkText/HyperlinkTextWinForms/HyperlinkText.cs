/*
*  Copyright (C) 1998-2021 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main GoDiagram library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in the GoDiagram repository (https://github.com/NorthwoodsSoftware/GoDiagram/tree/main/Extensions).
* See the Extensions intro page (https://godiagram.com/intro/extensions.html) for more information.
*/

using System;
using System.Diagnostics;
using System.Linq;
using Northwoods.Go.Models;

namespace Northwoods.Go.Extensions {
  /// <summary>
  /// A "HyperlinkText" is either a TextBlock or a Panel containing a TextBlock that when clicked
  /// opens a new browser window with a given or computed URL.
  /// </summary>
  /// <remarks>
  /// When the user's mouse passes over a "HyperlinkText", the text is underlined.
  /// When the mouse hovers over a "HyperlinkText", it shows a tooltip that displays the URL.
  ///
  /// This "HyperlinkText" builder is not pre-defined in the GoDiagram library, so you will need to load this definition.
  ///
  /// Typical usages:
  /// <code language="cs">
  ///   Builder.Make&lt;GraphObject&gt;("HyperlinkText", "https://godiagram.com", "Visit GoDiagram");
  ///
  ///   Builder.Make&lt;GraphObject&gt;("HyperlinkText",
  ///     (n) =&gt; "https://godiagram.com/" + n.data.version,
  ///     new Panel("Auto")
  ///       .Add(
  ///         new Shape { ... },
  ///         new TextBlock { ... }
  ///       )
  ///   );
  ///
  ///   Builder.Make&lt;GraphObject&gt;("HyperlinkText",
  ///     (n) =&gt; "https://godiagram.com/" + n.data.version,
  ///     (n) =&gt; "Visit GoDiagram version " + n.data.version
  ///   );
  /// </code>
  ///
  /// The first argument to the "HyperlinkText" builder should be either the URL string or a function
  /// that takes the data-bound Panel and returns the URL string.
  /// If the URL string is empty or if the function returns an empty string,
  /// the text will not be underlined on a mouse-over and a click has no effect.
  ///
  /// The second argument to the "HyperlinkText" builder may be either a string to display in a TextBlock,
  /// or a function that takes the data-bound Panel and returns the string to display in a TextBlock.
  /// If no text string or function is provided, it assumes all of the arguments are used to
  /// define the visual tree for the "HyperlinkText", in the normal fashion for a Panel.
  ///
  /// The result is either a TextBlock or a Panel.
  /// </remarks>
  public class HyperlinkText {

    /// <summary>
    /// Adds the "HyperlinkText" builder to the <see cref="Builder"/>.
    /// </summary>
    public static void DefineBuilders() {
      Builder.DefineBuilder("HyperlinkText", (args) => {
        // the URL is required as the first argument, either a string or a side-effect-free function returning a string
        var url = Builder.TakeBuilderArgument(ref args, null, (x) => { return (x is string) || (x is Func<object, string>); });
        // the text for the HyperlinkText is the optional second argument, either a string or a side-effect-free function returning a string
        var text = Builder.TakeBuilderArgument(ref args, null, (x) => { return (x is string) || (x is Func<object, string>); });
        // see if the visual tree is supplied in the arguments to the "HyperlinkText"
        var anyGraphObjects = false;
        for (var i = 0; i < args.Length; i++) {
          var a = args[i];
          if (a != null && a is GraphObject) anyGraphObjects = true;
        }

        // define the click behavior
        Action<InputEvent, GraphObject> click = (e, obj) => {
          var u = obj["_Url"];
          if (u is Func<object, string>) u = (u as Func<object, string>).Invoke(obj.FindBindingPanel());
          var uri = u as string;
          if (uri != null) {
            var psi = new ProcessStartInfo { FileName = uri, UseShellExecute = true };
            Process.Start(psi);
          }
        };

        // define the tooltip
        var tooltip =
          Builder.Make<Adornment>("ToolTip").Add(
            new TextBlock { Name = "TB", Margin = 4 }.Bind(
              new Binding("Text", "", (data, obj) => {
                // here OBJ will be in the Adornment, need to get the HyperlinkText/TextBlock
                var tempobj = ((data as GraphObject).Part as Adornment).AdornedElement;
                var u = tempobj["_Url"];
                if (u is Func<object, string>) u = (u as Func<object, string>).Invoke(tempobj.FindBindingPanel());
                return u;
              }).OfElement()
            )).Bind(new Binding("Visible", "Text", (t, obj) => {
              return (t != null && (t as string) != "");
            }).OfElement("TB"));

        // if the text is provided, use a new TextBlock; otherwise assume the TextBlock is provided
        if (text is string || text is Func<object, string> || !anyGraphObjects) {
          if (text == null && url is string) text = url;
          var tb = new TextBlock {
            Cursor = "pointer",
            MouseEnter = (e, obj, targObj) => {
              var u = (obj as GraphObject)["_Url"];
              if (u is Func<object, string>) u = (u as Func<object, string>).Invoke(obj.FindBindingPanel());
              if (u != null && obj is TextBlock) (obj as TextBlock).IsUnderline = true;
            },
            MouseLeave = (e, obj, targObj) => { if (obj is TextBlock) (obj as TextBlock).IsUnderline = false; },
            Click = click,  // defined above
            ToolTip = tooltip // shared by all HyperlinkText textblocks
          };
          tb["_Url"] = url;


          if (text is string) {
            tb.Text = text as string;
          } else if (text is Func<object, string>) {
            tb.Bind(new Binding("Text", "", (data, obj) => { return (text as Func<object, string>).Invoke(data); }).OfElement());
          } else if (url is Func<object, string>) {
            tb.Bind(new Binding("Text", "", (data, obj) => { return (url as Func<object, string>).Invoke(data); }).OfElement());
          }
          return tb;
        } else {
          // recursive delegate
          Func<GraphObject, TextBlock> findTextBlock = null;
          findTextBlock = (obj) => {
            if (obj is TextBlock) return obj as TextBlock;
            if (obj is Panel) {
              var it = (obj as Panel).Elements.GetEnumerator();
              while (it.MoveNext()) {
                var result = findTextBlock(it.Current);
                if (result != null) return result;
              }
            }
            return null;
          };
          var visualTree = args.OfType<GraphObject>();  // pull GraphObjects from args array
          var retPanel = new Panel {
            Cursor = "pointer",
            MouseEnter = (e, panel, obj) => {
              var tb = findTextBlock(panel);
              var u = (panel as GraphObject)["_Url"];
              if (u is Func<object, string>) u = (u as Func<object, string>).Invoke(panel.FindBindingPanel());
              if (tb != null && u != null) tb.IsUnderline = true;
            },
            MouseLeave = (e, panel, obj) => {
              var tb = findTextBlock(panel);
              if (tb != null) tb.IsUnderline = false;
            },
            Click = click,  // defined above
            ToolTip = tooltip  // shared by all HyperlinkText panels
          }.Add(visualTree);
          retPanel["_Url"] = url;
          return retPanel;
        }
      });
    }
  }
}
