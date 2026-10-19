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

using System.IO;
using System.Runtime.CompilerServices;
using SkiaSharp;
using Svg.Skia;

namespace Northwoods.Go.Extensions {
  /// <summary>
  /// Lets a <see cref="Picture"/> display SVG images by parsing them with the Svg.Skia library.
  /// </summary>
  /// <remarks>
  /// GoDiagram does not include an SVG parser, so a Picture whose <see cref="Picture.Source"/> ends in ".svg"
  /// fails to load unless <see cref="ImageElement.SvgLoader"/> is set.
  /// Call <see cref="Enable"/> once at startup, before any such Picture is created.
  ///
  /// This extension needs a reference to the Svg.Skia NuGet package.
  /// </remarks>
  public static class SvgPictures {
    // Svg.Skia's SKSvg owns the SKPicture it produces, so keep each one alive as long as its picture is in use.
    private static readonly ConditionalWeakTable<SKPicture, SKSvg> _Documents = new();

    /// <summary>
    /// Sets <see cref="ImageElement.SvgLoader"/> to <see cref="Load"/>, so that Pictures can display SVG images.
    /// </summary>
    public static void Enable() {
      ImageElement.SvgLoader = Load;
    }

    /// <summary>
    /// Parses SVG data with Svg.Skia.
    /// </summary>
    /// <param name="stream">a stream of SVG data</param>
    /// <returns>the parsed picture, or null if the data could not be parsed</returns>
    public static SKPicture Load(Stream stream) {
      var svg = new SKSvg();
      var picture = svg.Load(stream);
      if (picture == null) {
        svg.Dispose();
        return null;
      }
      _Documents.Add(picture, svg);
      return picture;
    }
  }
}
