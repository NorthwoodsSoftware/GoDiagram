/*
*  Copyright (C) 1998-2024 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main Go library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in the GoDiagram repository (https://github.com/NorthwoodsSoftware/GoDiagram/tree/main/Extensions).
* See the Extensions intro page (https://godiagram.com/intro/extensions.html) for more information.
*/

using System;

/// These are the definitions for all the predefined arrowheads.
/// You do not need to load this file in order to use these arrowheads.
///
/// Typical custom definition:
/// Shape.DefineArrowheadGeometry("Zigzag", "M0,4 L1,8 3,0 5,8 7,0 8,4");
///
/// Typical usage in a link template:
/// myDiagram.LinkTemplate =
///   new Link().Add(
///     new Shape(),
///     new Shape { ToArrow = "Zigzag" }
///   );

namespace Northwoods.Go.Extensions {
  public class Arrowheads {
    public static void DefineArrowheads() {
      Shape.DefineArrowheadGeometry("Standard", "F1 m 0,0 l 8,4 -8,4 2,-4 z");
      Shape.DefineArrowheadGeometry("Backward", "F1 m 8,0 l -2,4 2,4 -8,-4 z");
      Shape.DefineArrowheadGeometry("Triangle", "F1 m 0,0 l 8,4.62 -8,4.62 z");
      Shape.DefineArrowheadGeometry("BackwardTriangle", "F1 m 8,4 l 0,4 -8,-4 8,-4 0,4 z");
      Shape.DefineArrowheadGeometry("Boomerang", "F1 m 0,0 l 8,4 -8,4 4,-4 -4,-4 z");
      Shape.DefineArrowheadGeometry("BackwardBoomerang", "F1 m 8,0 l -8,4 8,4 -4,-4 4,-4 z");
      Shape.DefineArrowheadGeometry("SidewaysV", "m 0,0 l 8,4 -8,4 0,-1 6,-3 -6,-3 0,-1 z");
      Shape.DefineArrowheadGeometry("BackwardV", "m 8,0 l -8,4 8,4 0,-1 -6,-3 6,-3 0,-1 z");

      Shape.DefineArrowheadGeometry("OpenTriangle", "m 0,0 l 8,4 -8,4");
      Shape.DefineArrowheadGeometry("BackwardOpenTriangle", "m 8,0 l -8,4 8,4");
      Shape.DefineArrowheadGeometry("OpenTriangleLine", "m 0,0 l 8,4 -8,4 m 8.5,0 l 0,-8");
      Shape.DefineArrowheadGeometry("BackwardOpenTriangleLine", "m 8,0 l  -8,4 8,4 m -8.5,0 l 0,-8");

      Shape.DefineArrowheadGeometry("OpenTriangleTop", "m 0,0 l 8,4 m 0,4");
      Shape.DefineArrowheadGeometry("BackwardOpenTriangleTop", "m 8,0 l -8,4 m 0,4");
      Shape.DefineArrowheadGeometry("OpenTriangleBottom", "m 0,8 l 8,-4");
      Shape.DefineArrowheadGeometry("BackwardOpenTriangleBottom", "m 0,4 l 8,4");

      Shape.DefineArrowheadGeometry("HalfTriangleTop", "F1 m 0,0 l 0,4 8,0 z m 0,8");
      Shape.DefineArrowheadGeometry("BackwardHalfTriangleTop", "F1 m 8,0 l 0,4 -8,0 z m 0,8");
      Shape.DefineArrowheadGeometry("HalfTriangleBottom", "F1 m 0,4 l 0,4 8,-4 z");
      Shape.DefineArrowheadGeometry("BackwardHalfTriangleBottom", "F1 m 8,4 l 0,4 -8,-4 z");

      Shape.DefineArrowheadGeometry("ForwardSemiCircle", "m 4,0 b 270 180 0 4 4");
      Shape.DefineArrowheadGeometry("BackwardSemiCircle", "m 4,8 b 90 180 0 -4 4");

      Shape.DefineArrowheadGeometry("Feather", "m 0,0 l 3,4 -3,4");
      Shape.DefineArrowheadGeometry("BackwardFeather", "m 3,0 l -3,4 3,4");
      Shape.DefineArrowheadGeometry("DoubleFeathers", "m 0,0 l 3,4 -3,4 m 3,-8 l 3,4 -3,4");
      Shape.DefineArrowheadGeometry("BackwardDoubleFeathers", "m 3,0 l -3,4 3,4 m 3,-8 l -3,4 3,4");
      Shape.DefineArrowheadGeometry("TripleFeathers", "m 0,0 l 3,4 -3,4 m 3,-8 l 3,4 -3,4 m 3,-8 l 3,4 -3,4");
      Shape.DefineArrowheadGeometry("BackwardTripleFeathers", "m 3,0 l -3,4 3,4 m 3,-8 l -3,4 3,4 m 3,-8 l -3,4 3,4");

      Shape.DefineArrowheadGeometry("ForwardSlash", "m 0,8 l 5,-8");
      Shape.DefineArrowheadGeometry("BackSlash", "m 0,0 l 5,8");
      Shape.DefineArrowheadGeometry("DoubleForwardSlash", "m 0,8 l 4,-8 m -2,8 l 4,-8");
      Shape.DefineArrowheadGeometry("DoubleBackSlash", "m 0,0 l 4,8 m -2,-8 l 4,8");
      Shape.DefineArrowheadGeometry("TripleForwardSlash", "m 0,8 l 4,-8 m -2,8 l 4,-8 m -2,8 l 4,-8");
      Shape.DefineArrowheadGeometry("TripleBackSlash", "m 0,0 l 4,8 m -2,-8 l 4,8 m -2,-8 l 4,8");

      Shape.DefineArrowheadGeometry("Fork", "m 0,4 l 8,0 m -8,0 l 8,-4 m -8,4 l 8,4");
      Shape.DefineArrowheadGeometry("BackwardFork", "m 8,4 l -8,0 m 8,0 l -8,-4 m 8,4 l -8,4");
      Shape.DefineArrowheadGeometry("LineFork", "m 0,0 l 0,8 m 0,-4 l 8,0 m -8,0 l 8,-4 m -8,4 l 8,4");
      Shape.DefineArrowheadGeometry("BackwardLineFork", "m 8,4 l -8,0 m 8,0 l -8,-4 m 8,4 l -8,4 m 8,-8 l 0,8");
      Shape.DefineArrowheadGeometry("CircleFork", "F1 m 6,4 b 0 360 -3 0 3 z m 0,0 l 6,0 m -6,0 l 6,-4 m -6,4 l 6,4");
      Shape.DefineArrowheadGeometry("BackwardCircleFork", "F1 m 0,4 l 6,0 m -6,-4 l 6,4 m -6,4 l 6,-4 m 6,0 b 0 360 -3 0 3");
      Shape.DefineArrowheadGeometry("CircleLineFork", "F1 m 6,4 b 0 360 -3 0 3 z m 1,-4 l 0,8 m 0,-4 l 6,0 m -6,0 l 6,-4 m -6,4 l 6,4");
      Shape.DefineArrowheadGeometry("BackwardCircleLineFork", "F1 m 0,4 l 6,0 m -6,-4 l 6,4 m -6,4 l 6,-4 m 0,-4 l 0,8 m 7,-4 b 0 360 -3 0 3");

      Shape.DefineArrowheadGeometry("Circle", "F1 m 8,4 b 0 360 -4 0 4 z");
      Shape.DefineArrowheadGeometry("Block", "F1 m 0,0 l 0,8 8,0 0,-8 z");
      Shape.DefineArrowheadGeometry("StretchedDiamond", "F1 m 0,3 l 5,-3 5,3 -5,3 -5,-3 z");
      Shape.DefineArrowheadGeometry("Diamond", "F1 m 0,4 l 4,-4 4,4 -4,4 -4,-4 z");
      Shape.DefineArrowheadGeometry("Chevron", "F1 m 0,0 l 5,0 3,4 -3,4 -5,0 3,-4 -3,-4 z");
      Shape.DefineArrowheadGeometry("StretchedChevron", "F1 m 0,0 l 8,0 3,4 -3,4 -8,0 3,-4 -3,-4 z");

      Shape.DefineArrowheadGeometry("NormalArrow", "F1 m 0,2 l 4,0 0,-2 4,4 -4,4 0,-2 -4,0 z");
      Shape.DefineArrowheadGeometry("X", "m 0,0 l 8,8 m 0,-8 l -8,8");
      Shape.DefineArrowheadGeometry("TailedNormalArrow", "F1 m 0,0 l 2,0 1,2 3,0 0,-2 2,4 -2,4 0,-2 -3,0 -1,2 -2,0 1,-4 -1,-4 z");
      Shape.DefineArrowheadGeometry("DoubleTriangle", "F1 m 0,0 l 4,4 -4,4 0,-8 z  m 4,0 l 4,4 -4,4 0,-8 z");
      Shape.DefineArrowheadGeometry("BigEndArrow", "F1 m 0,0 l 5,2 0,-2 3,4 -3,4 0,-2 -5,2 0,-8 z");
      Shape.DefineArrowheadGeometry("ConcaveTailArrow", "F1 m 0,2 h 4 v -2 l 4,4 -4,4 v -2 h -4 l 2,-2 -2,-2 z");
      Shape.DefineArrowheadGeometry("RoundedTriangle", "F1 m 0,1 a 1,1 0 0 1 1,-1 l 7,3 a 0.5,1 0 0 1 0,2 l -7,3 a 1,1 0 0 1 -1,-1 l 0,-6 z");
      Shape.DefineArrowheadGeometry("SimpleArrow", "F1 m 1,2 l -1,-2 2,0 1,2 -1,2 -2,0 1,-2 5,0 0,-2 2,2 -2,2 0,-2 z");
      Shape.DefineArrowheadGeometry("AccelerationArrow", "F1 m 0,0 l 0,8 0.2,0 0,-8 -0.2,0 z m 2,0 l 0,8 1,0 0,-8 -1,0 z m 3,0 l 2,0 2,4 -2,4 -2,0 0,-8 z");
      Shape.DefineArrowheadGeometry("BoxArrow", "F1 m 0,0 l 4,0 0,2 2,0 0,-2 2,4 -2,4 0,-2 -2,0 0,2 -4,0 0,-8 z");
      Shape.DefineArrowheadGeometry("TriangleLine", "F1 m 8,4 l -8,-4 0,8 8,-4 z m 0.5,4 l 0,-8");

      Shape.DefineArrowheadGeometry("CircleEndedArrow", "F1 m 10,4 l -2,-3 0,2 -2,0 0,2 2,0 0,2 2,-3 z m -4,0 b 0 360 -3 0 3 z");

      Shape.DefineArrowheadGeometry("DynamicWidthArrow", "F1 m 0,3 l 2,0 2,-1 2,-2 2,4 -2,4 -2,-2 -2,-1 -2,0 0,-2 z");
      Shape.DefineArrowheadGeometry("EquilibriumArrow", "m 0,3 l 8,0 -3,-3 m 3,5 l -8,0 3,3");
      Shape.DefineArrowheadGeometry("FastForward", "F1 m 0,0 l 3.5,4 0,-4 3.5,4 0,-4 1,0 0,8 -1,0 0,-4 -3.5,4 0,-4 -3.5,4 0,-8 z");
      Shape.DefineArrowheadGeometry("Kite", "F1 m 0,4 l 2,-4 6,4 -6,4 -2,-4 z");
      Shape.DefineArrowheadGeometry("HalfArrowTop", "F1 m 0,0 l 4,4 4,0 -8,-4 z m 0,8");
      Shape.DefineArrowheadGeometry("HalfArrowBottom", "F1 m 0,8 l 4,-4 4,0 -8,4 z");
      Shape.DefineArrowheadGeometry("OpposingDirectionDoubleArrow", "F1 m 0,4 l 2,-4 0,2 4,0 0,-2 2,4 -2,4 0,-2 -4,0 0,2 -2,-4 z");
      Shape.DefineArrowheadGeometry("PartialDoubleTriangle", "F1 m 0,0 4,3 0,-3 4,4 -4,4 0,-3 -4,3 0,-8 z");
      Shape.DefineArrowheadGeometry("LineCircle", "F1 m 0,0 l 0,8 m 7 -4 b 0 360 -3 0 3 z");
      Shape.DefineArrowheadGeometry("DoubleLineCircle", "F1 m 0,0 l 0,8 m 2,-8 l 0,8 m 7 -4 b 0 360 -3 0 3 z");
      Shape.DefineArrowheadGeometry("TripleLineCircle", "F1 m 0,0 l 0,8 m 2,-8 l 0,8 m 2,-8 l 0,8 m 7 -4 b 0 360 -3 0 3 z");
      Shape.DefineArrowheadGeometry("CircleLine", "F1 m 6 4 b 0 360 -3 0 3 z m 1,-4 l 0,8");
      Shape.DefineArrowheadGeometry("DiamondCircle", "F1 m 8,4 l -4,4 -4,-4 4,-4 4,4 m 8,0 b 0 360 -4 0 4 z");
      Shape.DefineArrowheadGeometry("PlusCircle", "F1 m 8,4 b 0 360 -4 0 4 l -8 0 z m -4 -4 l 0 8");
      Shape.DefineArrowheadGeometry("OpenRightTriangleTop", "m 8,0 l 0,4 -8,0 m 0,4");
      Shape.DefineArrowheadGeometry("OpenRightTriangleBottom", "m 8,8 l 0,-4 -8,0");
      Shape.DefineArrowheadGeometry("Line", "m 0,0 l 0,8");
      Shape.DefineArrowheadGeometry("DoubleLine", "m 0,0 l 0,8 m 2,0 l 0,-8");
      Shape.DefineArrowheadGeometry("TripleLine", "m 0,0 l 0,8 m 2,0 l 0,-8 m 2,0 l 0,8");
      Shape.DefineArrowheadGeometry("PentagonArrow", "F1 m 8,4 l -4,-4 -4,0 0,8 4,0 4,-4 z");
    }
  }
}
