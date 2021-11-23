﻿/*
*  Copyright (C) 1998-2020 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main GoJS library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in the GoJS kit under the extensions or extensionsTS folders.
* See the Extensions intro page (https://gojs.Net/latest/intro/extensions.Html) for more information.
*/

using System;

namespace Northwoods.Go.Tools.Extensions {

  /// <summary>
  /// The ResizeMultipleTool class lets the user resize multiple objects at once.
  ///
  /// If you want to experiment with this extension, try the <a href="../../extensionsTS/ResizeMultiple.Html">Resize Multiple</a> sample.
  /// </summary>
  /// @category Tool Extension
  public class ResizeMultipleTool : ResizingTool {
    /// <summary>
    /// Constructs a ResizeMultipleTool and sets the name for the tool.
    /// </summary>
    public ResizeMultipleTool() : base() {
      Name = "ResizeMultiple";
    }

    /// <summary>
    /// Overrides <see cref="ResizingTool.Resize"/> to resize all selected objects to the same size.
    /// </summary>
    /// <param name="newr"></param> the intended new rectangular bounds for each Part"s <see cref="Part.ResizeElement"/>.
    public override void Resize(Rect newr) {
      var diagram = Diagram;
      foreach (var part in diagram.Selection) {
        if (part is Link || part is Group) continue; // only Nodes and simple Parts
        var obj = part.ResizeElement;

        // calculate new location
        var pos = part.Position;
        var angle = obj.GetDocumentAngle();
        var sc = obj.GetDocumentScale();

        var radAngle = Math.PI * angle / 180;
        var angleCos = Math.Cos(radAngle);
        var angleSin = Math.Sin(radAngle);

        var deltaWidth = newr.Width - obj.NaturalBounds.Width;
        var deltaHeight = newr.Height - obj.NaturalBounds.Height;

        var angleRight = (angle > 270 || angle < 90) ? 1 : 0;
        var angleBottom = (angle > 0 && angle < 180) ? 1 : 0;
        var angleLeft = (angle > 90 && angle < 270) ? 1 : 0;
        var angleTop = (angle > 180 && angle < 360) ? 1 : 0;

        pos.X += sc * ((newr.X + deltaWidth * angleLeft) * angleCos - (newr.Y + deltaHeight * angleBottom) * angleSin);
        pos.Y += sc * ((newr.X + deltaWidth * angleTop) * angleSin + (newr.Y + deltaHeight * angleLeft) * angleCos);

        obj.DesiredSize = newr.Size;
        part.Position = pos;
      }
    }

  }
}