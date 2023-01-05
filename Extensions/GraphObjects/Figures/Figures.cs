/*
*  Copyright (C) 1998-2023 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main GoDiagram library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in the GoDiagram repository (https://github.com/NorthwoodsSoftware/GoDiagram/tree/main/Extensions).
* See the Extensions intro page (https://godiagram.com/intro/extensions.html) for more information.
*/

using System;
using System.Collections.Generic;
using System.Linq;

// This file holds definitions of all standard shape figures -- string values for Shape.Figure.
// You do not need to load this file in order to use named Shape figure.

// The following figures are built-in to the library and thus do not need to be redefined:
//   Rectangle, Square, RoundedRectangle, Border, Ellipse, Circle,
//   TriangleRight, TriangleDown, TriangleLeft, TriangleUp, Triangle,
//   LineH, LineV, None, BarH, BarV, MinusLine, PlusLine, XLine
// If you need any of the other figures that are defined in this file, we suggest that you copy
// just those definitions into your own code.  Do not load this file unless you really want to
// define a lot of code that your app does not use and will not get garbage-collected.

namespace Northwoods.Go.Extensions {
  // The following functions and variables are used throughout this file:
  /// <summary>
  /// This FigureParameter class describes various properties each parameter uses in figures.
  /// </summary>
  internal class FigureParameter {
    public static Dictionary<string, List<FigureParameter>> DefinedParameters = new(StringComparer.OrdinalIgnoreCase);
    private string _Name;
    private double _DefaultValue;
    private double _Minimum;
    private double _Maximum;

    public FigureParameter(string name, double def, double min = 0.0, double max = double.PositiveInfinity) {
      _Name = name;
      _DefaultValue = def;
      _Minimum = min;
      _Maximum = max;
    }

    /// <summary>
    /// Gets or sets the name of the figure.
    /// </summary>
    public string Name {
      get {
        return _Name;
      }
      set {
        if (value == null || value == "") throw new ArgumentException("Shape name must be a valid string.");
        _Name = value;
      }
    }

    /// <summary>
    /// Gets or sets the default value for the parameter.
    /// </summary>
    public double DefaultValue {
      get {
        return _DefaultValue;
      }
      set {
        if (double.IsNaN(value)) throw new ArgumentException("The default value must be a real number, not: " + value);
        _DefaultValue = value;
      }
    }

    /// <summary>
    /// Gets or sets the minimum value allowed for the figure parameter.
    /// </summary>
    public double Minimum {
      get {
        return _Minimum;
      }
      set {
        if (double.IsNaN(value)) throw new ArgumentException("Minimum must be a real number, not: " + value);
        _Minimum = value;
      }
    }

    /// <summary>
    /// Gets or sets the maximum value allowed for the figure parameter.
    /// </summary>
    public double Maximum {
      get {
        return _Maximum;
      }
      set {
        if (double.IsNaN(value)) throw new Exception("Maximum must be a real number, not: " + value);
        _Maximum = value;
      }
    }

    /// <summary>
    /// This static function gets a FigureParameter for a particular figure name.
    /// </summary>
    /// <param name="figurename"></param>
    /// <param name="index">currently must be either 0 or 1</param>
    public static FigureParameter GetFigureParameter(string figurename, int index) {
      var arr = DefinedParameters[figurename];
      if (arr == null) return null;
      return arr[index];
    }

    /// <summary>
    /// This static function sets a FigureParameter for a particular figure name.
    /// </summary>
    /// <param name="figurename"></param>
    /// <param name="index">currently must be either 0 or 1</param>
    /// <param name="figparam"></param>
    public static void SetFigureParameter(string figurename, int index, FigureParameter figparam) {
      if (figparam == null) throw new Exception("Third argument to FigureParameter.SetFigureParameter is not FigureParameter: " + figparam);
      if (figparam.DefaultValue < figparam.Minimum || figparam.DefaultValue > figparam.Maximum) {
        throw new Exception("DefaultValue must be between minimum and maximum, not: " + figparam.DefaultValue);
      }
      List<FigureParameter> arr;
      if (DefinedParameters.ContainsKey(figurename)) {
        arr = DefinedParameters[figurename];
      } else {
        arr = new List<FigureParameter>();
        DefinedParameters.Add(figurename, arr);
      }
      arr.Insert(index, figparam);
    }
  }

  /// <summary>
  /// This class contains static methods pertaining to <see cref="Shape"/> figures.
  /// </summary>
  public class Figures {
    private static Point GetIntersection(double p1x, double p1y, double p2x, double p2y, double q1x, double q1y, double q2x, double q2y, out Point result) {
      var dx1 = p1x - p2x;
      var dx2 = q1x - q2x;
      double x;
      double y;

      if (dx1 == 0 || dx2 == 0) {
        if (dx1 == 0) {
          var m2 = (q1y - q2y) / dx2;
          var b2 = q1y - m2 * q1x;
          x = p1x;
          y = m2 * x + b2;
        } else {
          var m1 = (p1y - p2y) / dx1;
          var b1 = p1y - m1 * p1x;
          x = q1x;
          y = m1 * x + b1;
        }
      } else {
        var m1 = (p1y - p2y) / dx1;
        var m2 = (q1y - q2y) / dx2;
        var b1 = p1y - m1 * p1x;
        var b2 = q1y - m2 * q1x;

        x = (b2 - b1) / (m1 - m2);
        y = m1 * x + b1;
      }

      result = new Point(x, y);
      return result;
    }

    private static void BreakUpBezier(double startx, double starty, double c1x, double c1y, double c2x, double c2y, double endx, double endy, double fraction,
                                      out Point curve1cp1, out Point curve1cp2, out Point midpoint, out Point curve2cp1, out Point curve2cp2) {
      var fo = 1 - fraction;
      var so = fraction;
      var m1x = (startx * fo + c1x * so);
      var m1y = (starty * fo + c1y * so);
      var m2x = (c1x * fo + c2x * so);
      var m2y = (c1y * fo + c2y * so);
      var m3x = (c2x * fo + endx * so);
      var m3y = (c2y * fo + endy * so);
      var m12x = (m1x * fo + m2x * so);
      var m12y = (m1y * fo + m2y * so);
      var m23x = (m2x * fo + m3x * so);
      var m23y = (m2y * fo + m3y * so);
      var m123x = (m12x * fo + m23x * so);
      var m123y = (m12y * fo + m23y * so);

      curve1cp1 = new Point(m1x, m1y);

      curve1cp2 = new Point(m12x, m12y);

      midpoint = new Point(m123x, m123y);

      curve2cp1 = new Point(m23x, m23y);

      curve2cp2 = new Point(m3x, m3y);
    }

    /// <summary>
    /// Define extra figures to be used by <see cref="Shape"/>s.
    /// </summary>
    public static void DefineExtraFigures() {
      var GeneratorEllipseSpot1 = new Spot(0.156, 0.156);
      var GeneratorEllipseSpot2 = new Spot(0.844, 0.844);
      var KAPPA = 4 * ((Math.Sqrt(2) - 1) / 3);

      // OPTIONAL figures, not predefined in the library:
      Shape.DefineFigureGenerator("AsteriskLine", (shape, w, h) => {
        var offset = .2 / Math.Sqrt(2);
        return new Geometry()
          .Add(new PathFigure(offset * w, (1 - offset) * h, false)
            .Add(new PathSegment(SegmentType.Line, (1 - offset) * w, offset * h))
            .Add(new PathSegment(SegmentType.Move, offset * w, offset * h))
            .Add(new PathSegment(SegmentType.Line, (1 - offset) * w, (1 - offset) * h))
            .Add(new PathSegment(SegmentType.Move, 0, h / 2))
            .Add(new PathSegment(SegmentType.Line, w, h / 2))
            .Add(new PathSegment(SegmentType.Move, w / 2, 0))
            .Add(new PathSegment(SegmentType.Line, w / 2, h)));
      });

      Shape.DefineFigureGenerator("CircleLine", (shape, w, h) => {
        var rad = w / 2;
        var geo = new Geometry()
          .Add(new PathFigure(w, w / 2, false)  // clockwise
            .Add(new PathSegment(SegmentType.Arc, 0, 360, rad, rad, rad, rad).Close()));
        geo.Spot1 = GeneratorEllipseSpot1;
        geo.Spot2 = GeneratorEllipseSpot2;
        geo.DefaultStretch = GeometryStretch.Uniform;
        return geo;
      });

      Shape.DefineFigureGenerator("Line1", (shape, w, h) => {
        var geo = new Geometry(GeometryType.Line) {
          StartX = 0,
          StartY = 0,
          EndX = w,
          EndY = h
        };
        return geo;
      });

      Shape.DefineFigureGenerator("Line2", (shape, w, h) => {
        var geo = new Geometry(GeometryType.Line) {
          StartX = w,
          StartY = 0,
          EndX = 0,
          EndY = h
        };
        return geo;
      });

      Shape.DefineFigureGenerator("Curve1", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(0, 0, false)
            .Add(new PathSegment(SegmentType.Bezier, w, h, KAPPA * w, 0, w, (1 - KAPPA) * h)));
      });

      Shape.DefineFigureGenerator("Curve2", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(0, 0, false)
            .Add(new PathSegment(SegmentType.Bezier, w, h, 0, KAPPA * h, (1 - KAPPA) * w, h)));
      });

      Shape.DefineFigureGenerator("Curve3", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(w, 0, false)
            .Add(new PathSegment(SegmentType.Bezier, 0, h, w, KAPPA * h, KAPPA * w, h)));
      });

      Shape.DefineFigureGenerator("Curve4", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(w, 0, false)
            .Add(new PathSegment(SegmentType.Bezier, 0, h, (1 - KAPPA) * w, 0, 0, (1 - KAPPA) * h)));
      });

      Shape.DefineFigureGenerator("TriangleDownLeft", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(0, 0, true)
            .Add(new PathSegment(SegmentType.Line, w, h))
            .Add(new PathSegment(SegmentType.Line, 0, h).Close()))
          .SetSpots(0, 0.5, 0.5, 1);
      });

      Shape.DefineFigureGenerator("TriangleDownRight", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(w, 0, true)
            .Add(new PathSegment(SegmentType.Line, w, h))
            .Add(new PathSegment(SegmentType.Line, 0, h).Close()))
          .SetSpots(0.5, 0.5, 1, 1);
      });

      Shape.DefineFigureGenerator("TriangleUpLeft", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(0, 0, true)
            .Add(new PathSegment(SegmentType.Line, w, 0))
            .Add(new PathSegment(SegmentType.Line, 0, h).Close()))
          .SetSpots(0, 0, 0.5, 0.5);
      });

      Shape.DefineFigureGenerator("TriangleUpRight", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(0, 0, true)
            .Add(new PathSegment(SegmentType.Line, w, 0))
            .Add(new PathSegment(SegmentType.Line, w, h).Close()))
          .SetSpots(0.5, 0, 1, 0.5);
      });

      Shape.DefineFigureGenerator("RightTriangle", "TriangleDownLeft");

      FigureParameter.SetFigureParameter("Parallelogram1", 0, new FigureParameter("Indent", .1, -.99, .99));
      Shape.DefineFigureGenerator("Parallelogram1", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN; // indent's percent distance
        if (double.IsNaN(param1)) param1 = 0.1;
        else if (param1 < -1) param1 = -1;
        else if (param1 > 1) param1 = 1;
        var indent = Math.Abs(param1) * w;

        if (param1 == 0) {
          var geo = new Geometry(GeometryType.Rectangle) {
            StartX = 0,
            StartY = 0,
            EndX = w,
            EndY = h
          };
          return geo;
        } else {
          var geo = new Geometry();
          if (param1 > 0) {
            geo.Add(new PathFigure(indent, 0)
              .Add(new PathSegment(SegmentType.Line, w, 0))
              .Add(new PathSegment(SegmentType.Line, w - indent, h))
              .Add(new PathSegment(SegmentType.Line, 0, h).Close()));
          } else {  // param1 < 0
            geo.Add(new PathFigure(0, 0)
              .Add(new PathSegment(SegmentType.Line, w - indent, 0))
              .Add(new PathSegment(SegmentType.Line, w, h))
              .Add(new PathSegment(SegmentType.Line, indent, h).Close()));
          }
          if (indent < w / 2) {
            geo.SetSpots(indent / w, 0, (w - indent) / w, 1);
          }
          return geo;
        }
      });
      Shape.DefineFigureGenerator("Parallelogram", "Parallelogram1"); // alias

      // Parallelogram with absolutes instead of scaling
      FigureParameter.SetFigureParameter("Parallelogram2", 0, new FigureParameter("Indent", 10, -double.PositiveInfinity, double.PositiveInfinity));
      Shape.DefineFigureGenerator("Parallelogram2", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN; // indent's x distance
        if (double.IsNaN(param1)) param1 = 10;
        else if (param1 < -1) param1 = -w;
        else if (param1 > 1) param1 = w;
        var indent = Math.Abs(param1);

        if (param1 == 0) {
          var geo = new Geometry(GeometryType.Rectangle) {
            StartX = 0,
            StartY = 0,
            EndX = w,
            EndY = h
          };
          return geo;
        } else {
          var geo = new Geometry();
          if (param1 > 0) {
            geo.Add(new PathFigure(indent, 0)
              .Add(new PathSegment(SegmentType.Line, w, 0))
              .Add(new PathSegment(SegmentType.Line, w - indent, h))
              .Add(new PathSegment(SegmentType.Line, 0, h).Close()));
          } else {  // param1 < 0
            geo.Add(new PathFigure(0, 0)
              .Add(new PathSegment(SegmentType.Line, w - indent, 0))
              .Add(new PathSegment(SegmentType.Line, w, h))
              .Add(new PathSegment(SegmentType.Line, indent, h).Close()));
          }
          if (indent < w / 2) {
            geo.SetSpots(indent / w, 0, (w - indent) / w, 1);
          }
          return geo;
        }
      });

      FigureParameter.SetFigureParameter("Trapezoid1", 0, new FigureParameter("Indent", .2, -.99, .99));
      Shape.DefineFigureGenerator("Trapezoid1", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN; // indent's percent distance
        if (double.IsNaN(param1)) param1 = 0.2;
        else if (param1 < 0.5) param1 = -0.5;
        else if (param1 > 0.5) param1 = 0.5;
        var indent = Math.Abs(param1) * w;

        if (param1 == 0) {
          var geo = new Geometry(GeometryType.Rectangle) {
            StartX = 0,
            StartY = 0,
            EndX = w,
            EndY = h
          };
          return geo;
        } else {
          var geo = new Geometry();
          if (param1 > 0) {
            geo.Add(new PathFigure(indent, 0)
              .Add(new PathSegment(SegmentType.Line, w - indent, 0))
              .Add(new PathSegment(SegmentType.Line, w, h))
              .Add(new PathSegment(SegmentType.Line, 0, h).Close()));
          } else {  // param1 < 0
            geo.Add(new PathFigure(0, 0)
              .Add(new PathSegment(SegmentType.Line, w, 0))
              .Add(new PathSegment(SegmentType.Line, w - indent, h))
              .Add(new PathSegment(SegmentType.Line, indent, h).Close()));
          }
          if (indent < w / 2) {
            geo.SetSpots(indent / w, 0, (w - indent) / w, 1);
          }
          return geo;
        }
      });
      Shape.DefineFigureGenerator("Trapezoid", "Trapezoid1"); // alias

      // Trapezoid with absolutes instead of scaling
      FigureParameter.SetFigureParameter("Trapezoid2", 0, new FigureParameter("Indent", 20, -double.PositiveInfinity, double.PositiveInfinity));
      Shape.DefineFigureGenerator("Trapezoid2", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN; // indent's x distance
        if (double.IsNaN(param1)) param1 = 20; // default value
        else if (param1 < -w) param1 = -w / 2;
        else if (param1 > w) param1 = w / 2;
        var indent = Math.Abs(param1);

        if (param1 == 0) {
          var geo = new Geometry(GeometryType.Rectangle) {
            StartX = 0,
            StartY = 0,
            EndX = w,
            EndY = h
          };
          return geo;
        } else {
          var geo = new Geometry();
          if (param1 > 0) {
            geo.Add(new PathFigure(indent, 0)
              .Add(new PathSegment(SegmentType.Line, w - indent, 0))
              .Add(new PathSegment(SegmentType.Line, w, h))
              .Add(new PathSegment(SegmentType.Line, 0, h).Close()));
          } else {  // param1 < 0
            geo.Add(new PathFigure(0, 0)
              .Add(new PathSegment(SegmentType.Line, w, 0))
              .Add(new PathSegment(SegmentType.Line, w - indent, h))
              .Add(new PathSegment(SegmentType.Line, indent, h).Close()));
          }
          if (indent < w / 2) {
            geo.SetSpots(indent / w, 0, (w - indent) / w, 1);
          }
          return geo;
        }
      });

      FigureParameter.SetFigureParameter("ManualOperation", 0, new FigureParameter("Indent", 10, -double.PositiveInfinity, double.PositiveInfinity));
      Shape.DefineFigureGenerator("ManualOperation", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        // Distance from topleft of bounding rectangle,
        // in % of the total width, of the topleft corner
        if (double.IsNaN(param1)) param1 = 10; // default value
        else if (param1 < -w) param1 = -w / 2;
        else if (param1 > w) param1 = w / 2;
        var indent = Math.Abs(param1);

        if (param1 == 0) {
          var geo = new Geometry(GeometryType.Rectangle) {
            StartX = 0,
            StartY = 0,
            EndX = w,
            EndY = h
          };
          return geo;
        } else {
          var geo = new Geometry();
          if (param1 > 0) {
            geo.Add(new PathFigure(0, 0)
              .Add(new PathSegment(SegmentType.Line, w, 0))
              .Add(new PathSegment(SegmentType.Line, w - indent, h))
              .Add(new PathSegment(SegmentType.Line, indent, h).Close()));
          } else {  // param1 < 0
            geo.Add(new PathFigure(indent, 0)
              .Add(new PathSegment(SegmentType.Line, w - indent, 0))
              .Add(new PathSegment(SegmentType.Line, w, h))
              .Add(new PathSegment(SegmentType.Line, 0, h).Close()));
          }
          if (indent < w / 2) {
            geo.SetSpots(indent / w, 0, (w - indent) / w, 1);
          }
          return geo;
        }
      });

      // The following functions are used by a group of regular figures that are defined below:

      // This allocates a temporary Array that should be freed by the caller.
      Point[] createPolygon(int sides) {
        var points = new Point[sides + 1];
        var radius = .5;
        var center = .5;
        var offsetAngle = Math.PI * 1.5;
        var angle = 0d;

        // Loop through each side of the polygon
        for (var i = 0; i < sides; i++) {
          angle = 2 * Math.PI / sides * i + offsetAngle;
          points[i] = new Point((center + radius * Math.Cos(angle)), (center + radius * Math.Sin(angle)));
        }

        // Add the last line
        points[points.Length - 1] = points[0];
        return points;
      }

      // This allocates a temporary Array that should be freed by the caller.
      Point[] createBurst(int points) {
        var star = createStar(points);
        var pts = new Point[points * 3 + 1];

        pts[0] = star[0];
        for (int i = 1, count = 1; i < star.Length; i += 2, count += 3) {
          pts[count] = star[i];
          pts[count + 1] = star[i];
          pts[count + 2] = star[i + 1];
        }

        return pts;
      }

      // This allocates a temporary Array that should be freed by the caller.
      Point[] createStar(int points) {
        // First, create a regular polygon
        var polygon = createPolygon(points);
        // Calculate the points inbetween
        var pts = new Point[points * 2 + 1];

        var half = (int)Math.Floor(polygon.Length / 2d);
        var count = polygon.Length - 1;
        var offset = (points % 2 == 0) ? 2 : 1;

        for (var i = 0; i < count; i++) {
          // Get the intersection of two lines
          var p0 = polygon[i];
          var p1 = polygon[i + 1];
          var q21 = polygon[(half + i - 1) % count];
          var q2off = polygon[(half + i + offset) % count];
          pts[i * 2] = p0;
          var tempPoint = new Point();
          pts[i * 2 + 1] = GetIntersection(p0.X, p0.Y,
            q21.X, q21.Y,
            p1.X, p1.Y,
            q2off.X, q2off.Y, out tempPoint);  // ?? not currently managed
        }

        pts[pts.Length - 1] = pts[0];
        return pts;
      }


      Shape.DefineFigureGenerator("Pentagon", (shape, w, h) => {
        var points = createPolygon(5);
        var geo = new Geometry();
        var fig = new PathFigure(points[0].X * w, points[0].Y * h, true);
        geo.Add(fig);

        for (var i = 1; i < 5; i++) {
          fig.Add(new PathSegment(SegmentType.Line, points[i].X * w, points[i].Y * h));
        }
        fig.Add(new PathSegment(SegmentType.Line, points[0].X * w, points[0].Y * h).Close());
        geo.Spot1 = new Spot(.2, .22);
        geo.Spot2 = new Spot(.8, .9);
        return geo;
      });

      Shape.DefineFigureGenerator("Hexagon", (shape, w, h) => {
        var points = createPolygon(6);
        var geo = new Geometry();
        var fig = new PathFigure(points[0].X * w, points[0].Y * h, true);
        geo.Add(fig);

        for (var i = 1; i < 6; i++) {
          fig.Add(new PathSegment(SegmentType.Line, points[i].X * w, points[i].Y * h));
        }
        fig.Add(new PathSegment(SegmentType.Line, points[0].X * w, points[0].Y * h).Close());
        geo.Spot1 = new Spot(.07, .25);
        geo.Spot2 = new Spot(.93, .75);
        return geo;
      });

      Shape.DefineFigureGenerator("Heptagon", (shape, w, h) => {
        var points = createPolygon(7);
        var geo = new Geometry();
        var fig = new PathFigure(points[0].X * w, points[0].Y * h, true);
        geo.Add(fig);

        for (var i = 1; i < 7; i++) {
          fig.Add(new PathSegment(SegmentType.Line, points[i].X * w, points[i].Y * h));
        }
        fig.Add(new PathSegment(SegmentType.Line, points[0].X * w, points[0].Y * h).Close());
        geo.Spot1 = new Spot(.2, .15);
        geo.Spot2 = new Spot(.8, .85);
        return geo;
      });

      Shape.DefineFigureGenerator("Octagon", (shape, w, h) => {
        var points = createPolygon(8);
        var geo = new Geometry();
        var fig = new PathFigure(points[0].X * w, points[0].Y * h, true);
        geo.Add(fig);

        for (var i = 1; i < 8; i++) {
          fig.Add(new PathSegment(SegmentType.Line, points[i].X * w, points[i].Y * h));
        }
        fig.Add(new PathSegment(SegmentType.Line, points[0].X * w, points[0].Y * h).Close());
        geo.Spot1 = new Spot(.15, .15);
        geo.Spot2 = new Spot(.85, .85);
        return geo;
      });

      Shape.DefineFigureGenerator("Nonagon", (shape, w, h) => {
        var points = createPolygon(9);
        var geo = new Geometry();
        var fig = new PathFigure(points[0].X * w, points[0].Y * h, true);
        geo.Add(fig);

        for (var i = 1; i < 9; i++) {
          fig.Add(new PathSegment(SegmentType.Line, points[i].X * w, points[i].Y * h));
        }
        fig.Add(new PathSegment(SegmentType.Line, points[0].X * w, points[0].Y * h).Close());
        geo.Spot1 = new Spot(.17, .13);
        geo.Spot2 = new Spot(.82, .82);
        return geo;
      });

      Shape.DefineFigureGenerator("Decagon", (shape, w, h) => {
        var points = createPolygon(10);
        var geo = new Geometry();
        var fig = new PathFigure(points[0].X * w, points[0].Y * h, true);
        geo.Add(fig);

        for (var i = 1; i < 10; i++) {
          fig.Add(new PathSegment(SegmentType.Line, points[i].X * w, points[i].Y * h));
        }
        fig.Add(new PathSegment(SegmentType.Line, points[0].X * w, points[0].Y * h).Close());
        geo.Spot1 = new Spot(.16, .16);
        geo.Spot2 = new Spot(.84, .84);
        return geo;
      });

      Shape.DefineFigureGenerator("Dodecagon", (shape, w, h) => {
        var points = createPolygon(12);
        var geo = new Geometry();
        var fig = new PathFigure(points[0].X * w, points[0].Y * h, true);
        geo.Add(fig);

        for (var i = 1; i < 12; i++) {
          fig.Add(new PathSegment(SegmentType.Line, points[i].X * w, points[i].Y * h));
        }
        fig.Add(new PathSegment(SegmentType.Line, points[0].X * w, points[0].Y * h).Close());
        geo.Spot1 = new Spot(.16, .16);
        geo.Spot2 = new Spot(.84, .84);
        return geo;
      });

      Shape.DefineFigureGenerator("FivePointedStar", (shape, w, h) => {
        var starPoints = createStar(5);
        var geo = new Geometry();
        var fig = new PathFigure(starPoints[0].X * w, starPoints[0].Y * h, true);
        geo.Add(fig);

        for (var i = 1; i < 10; i++) {
          fig.Add(new PathSegment(SegmentType.Line, starPoints[i].X * w, starPoints[i].Y * h));
        }
        fig.Add(new PathSegment(SegmentType.Line, starPoints[0].X * w, starPoints[0].Y * h).Close());
        geo.Spot1 = new Spot(.266, .333);
        geo.Spot2 = new Spot(.733, .733);
        return geo;
      });

      Shape.DefineFigureGenerator("SixPointedStar", (shape, w, h) => {
        var starPoints = createStar(6);
        var geo = new Geometry();
        var fig = new PathFigure(starPoints[0].X * w, starPoints[0].Y * h, true);
        geo.Add(fig);

        for (var i = 1; i < 12; i++) {
          fig.Add(new PathSegment(SegmentType.Line, starPoints[i].X * w, starPoints[i].Y * h));
        }
        fig.Add(new PathSegment(SegmentType.Line, starPoints[0].X * w, starPoints[0].Y * h).Close());
        geo.Spot1 = new Spot(.17, .25);
        geo.Spot2 = new Spot(.83, .75);
        return geo;
      });

      Shape.DefineFigureGenerator("SevenPointedStar", (shape, w, h) => {
        var starPoints = createStar(7);
        var geo = new Geometry();
        var fig = new PathFigure(starPoints[0].X * w, starPoints[0].Y * h, true);
        geo.Add(fig);

        for (var i = 1; i < 14; i++) {
          fig.Add(new PathSegment(SegmentType.Line, starPoints[i].X * w, starPoints[i].Y * h));
        }
        fig.Add(new PathSegment(SegmentType.Line, starPoints[0].X * w, starPoints[0].Y * h).Close());
        geo.Spot1 = new Spot(.222, .277);
        geo.Spot2 = new Spot(.777, .666);
        return geo;
      });

      Shape.DefineFigureGenerator("EightPointedStar", (shape, w, h) => {
        var starPoints = createStar(8);
        var geo = new Geometry();
        var fig = new PathFigure(starPoints[0].X * w, starPoints[0].Y * h, true);
        geo.Add(fig);

        for (var i = 1; i < 16; i++) {
          fig.Add(new PathSegment(SegmentType.Line, starPoints[i].X * w, starPoints[i].Y * h));
        }
        fig.Add(new PathSegment(SegmentType.Line, starPoints[0].X * w, starPoints[0].Y * h).Close());
        geo.Spot1 = new Spot(.25, .25);
        geo.Spot2 = new Spot(.75, .75);
        return geo;
      });

      Shape.DefineFigureGenerator("NinePointedStar", (shape, w, h) => {
        var starPoints = createStar(9);
        var geo = new Geometry();
        var fig = new PathFigure(starPoints[0].X * w, starPoints[0].Y * h, true);
        geo.Add(fig);

        for (var i = 1; i < 18; i++) {
          fig.Add(new PathSegment(SegmentType.Line, starPoints[i].X * w, starPoints[i].Y * h));
        }
        fig.Add(new PathSegment(SegmentType.Line, starPoints[0].X * w, starPoints[0].Y * h).Close());
        geo.Spot1 = new Spot(.222, .277);
        geo.Spot2 = new Spot(.777, .666);
        return geo;
      });

      Shape.DefineFigureGenerator("TenPointedStar", (shape, w, h) => {
        var starPoints = createStar(10);
        var geo = new Geometry();
        var fig = new PathFigure(starPoints[0].X * w, starPoints[0].Y * h, true);
        geo.Add(fig);

        for (var i = 1; i < 20; i++) {
          fig.Add(new PathSegment(SegmentType.Line, starPoints[i].X * w, starPoints[i].Y * h));
        }
        fig.Add(new PathSegment(SegmentType.Line, starPoints[0].X * w, starPoints[0].Y * h).Close());
        geo.Spot1 = new Spot(.281, .261);
        geo.Spot2 = new Spot(.723, .748);
        return geo;
      });

      Shape.DefineFigureGenerator("FivePointedBurst", (shape, w, h) => {
        var burstPoints = createBurst(5);
        var geo = new Geometry();
        var fig = new PathFigure(burstPoints[0].X * w, burstPoints[0].Y * h, true);
        geo.Add(fig);

        for (var i = 1; i < burstPoints.Length; i += 3) {
          fig.Add(new PathSegment(SegmentType.Bezier, burstPoints[i + 2].X * w,
            burstPoints[i + 2].Y * h, burstPoints[i].X * w,
            burstPoints[i].Y * h, burstPoints[i + 1].X * w,
            burstPoints[i + 1].Y * h));
        }
        var lst = fig.Segments.Last();
        if (lst != null) lst.Close();
        geo.Spot1 = new Spot(.222, .277);
        geo.Spot2 = new Spot(.777, .777);
        return geo;
      });

      Shape.DefineFigureGenerator("SixPointedBurst", (shape, w, h) => {
        var burstPoints = createBurst(6);
        var geo = new Geometry();
        var fig = new PathFigure(burstPoints[0].X * w, burstPoints[0].Y * h, true);
        geo.Add(fig);

        for (var i = 1; i < burstPoints.Length; i += 3) {
          fig.Add(new PathSegment(SegmentType.Bezier, burstPoints[i + 2].X * w,
            burstPoints[i + 2].Y * h, burstPoints[i].X * w,
            burstPoints[i].Y * h, burstPoints[i + 1].X * w,
            burstPoints[i + 1].Y * h));
        }
        var lst = fig.Segments.Last();
        if (lst != null) lst.Close();
        geo.Spot1 = new Spot(.170, .222);
        geo.Spot2 = new Spot(.833, .777);
        return geo;
      });

      Shape.DefineFigureGenerator("SevenPointedBurst", (shape, w, h) => {
        var burstPoints = createBurst(7);
        var geo = new Geometry();
        var fig = new PathFigure(burstPoints[0].X * w, burstPoints[0].Y * h, true);
        geo.Add(fig);

        for (var i = 1; i < burstPoints.Length; i += 3) {
          fig.Add(new PathSegment(SegmentType.Bezier, burstPoints[i + 2].X * w,
            burstPoints[i + 2].Y * h, burstPoints[i].X * w,
            burstPoints[i].Y * h, burstPoints[i + 1].X * w,
            burstPoints[i + 1].Y * h));
        }
        var lst = fig.Segments.Last();
        if (lst != null) lst.Close();
        geo.Spot1 = new Spot(.222, .222);
        geo.Spot2 = new Spot(.777, .777);
        return geo;
      });

      Shape.DefineFigureGenerator("EightPointedBurst", (shape, w, h) => {
        var burstPoints = createBurst(8);
        var geo = new Geometry();
        var fig = new PathFigure(burstPoints[0].X * w, burstPoints[0].Y * h, true);
        geo.Add(fig);

        for (var i = 1; i < burstPoints.Length; i += 3) {
          fig.Add(new PathSegment(SegmentType.Bezier, burstPoints[i + 2].X * w,
            burstPoints[i + 2].Y * h, burstPoints[i].X * w,
            burstPoints[i].Y * h, burstPoints[i + 1].X * w,
            burstPoints[i + 1].Y * h));
        }
        var lst = fig.Segments.Last();
        if (lst != null) lst.Close();
        geo.Spot1 = new Spot(.222, .222);
        geo.Spot2 = new Spot(.777, .777);
        return geo;
      });

      Shape.DefineFigureGenerator("NinePointedBurst", (shape, w, h) => {
        var burstPoints = createBurst(9);
        var geo = new Geometry();
        var fig = new PathFigure(burstPoints[0].X * w, burstPoints[0].Y * h, true);
        geo.Add(fig);

        for (var i = 1; i < burstPoints.Length; i += 3) {
          fig.Add(new PathSegment(SegmentType.Bezier, burstPoints[i + 2].X * w,
            burstPoints[i + 2].Y * h, burstPoints[i].X * w,
            burstPoints[i].Y * h, burstPoints[i + 1].X * w,
            burstPoints[i + 1].Y * h));
        }
        var lst = fig.Segments.Last();
        if (lst != null) lst.Close();
        geo.Spot1 = new Spot(.222, .222);
        geo.Spot2 = new Spot(.777, .777);
        return geo;
      });

      Shape.DefineFigureGenerator("TenPointedBurst", (shape, w, h) => {
        var burstPoints = createBurst(10);
        var geo = new Geometry();
        var fig = new PathFigure(burstPoints[0].X * w, burstPoints[0].Y * h, true);
        geo.Add(fig);

        for (var i = 1; i < burstPoints.Length; i += 3) {
          fig.Add(new PathSegment(SegmentType.Bezier, burstPoints[i + 2].X * w,
            burstPoints[i + 2].Y * h, burstPoints[i].X * w,
            burstPoints[i].Y * h, burstPoints[i + 1].X * w,
            burstPoints[i + 1].Y * h));
        }
        var lst = fig.Segments.Last();
        if (lst != null) lst.Close();
        geo.Spot1 = new Spot(.222, .222);
        geo.Spot2 = new Spot(.777, .777);
        return geo;
      });

      Shape.DefineFigureGenerator("RoundedTopRectangle", (shape, w, h) => {
        // this figure takes one parameter, the size of the corner
        var p1 = 5.0;  // default corner size
        if (shape != null) {
          var param1 = shape.Parameter1;
          if (!double.IsNaN(param1) && param1 >= 0) p1 = param1;  // can't be negative or double.NaN
        }
        p1 = Math.Min(p1, w / 3);  // limit by width & height
        p1 = Math.Min(p1, h);
        var geo = new Geometry();
        // a single figure consisting of straight lines and quarter-circle arcs
        geo.Add(new PathFigure(0, p1)
          .Add(new PathSegment(SegmentType.Arc, 180, 90, p1, p1, p1, p1))
          .Add(new PathSegment(SegmentType.Line, w - p1, 0))
          .Add(new PathSegment(SegmentType.Arc, 270, 90, w - p1, p1, p1, p1))
          .Add(new PathSegment(SegmentType.Line, w, h))
          .Add(new PathSegment(SegmentType.Line, 0, h).Close()));
        // don't intersect with two top corners when used in an "Auto" Panel
        geo.Spot1 = new Spot(0, 0, 0.3 * p1, 0.3 * p1);
        geo.Spot2 = new Spot(1, 1, -0.3 * p1, 0);
        return geo;
      });

      Shape.DefineFigureGenerator("RoundedBottomRectangle", (shape, w, h) => {
        // this figure takes one parameter, the size of the corner
        var p1 = 5.0;  // default corner size
        if (shape != null) {
          var param1 = shape.Parameter1;
          if (!double.IsNaN(param1) && param1 >= 0) p1 = param1;  // can't be negative or double.NaN
        }
        p1 = Math.Min(p1, w / 3);  // limit by width & height
        p1 = Math.Min(p1, h);
        var geo = new Geometry();
        // a single figure consisting of straight lines and quarter-circle arcs
        geo.Add(new PathFigure(0, 0)
          .Add(new PathSegment(SegmentType.Line, w, 0))
          .Add(new PathSegment(SegmentType.Line, w, h - p1))
          .Add(new PathSegment(SegmentType.Arc, 0, 90, w - p1, h - p1, p1, p1))
          .Add(new PathSegment(SegmentType.Line, p1, h))
          .Add(new PathSegment(SegmentType.Arc, 90, 90, p1, h - p1, p1, p1).Close()));
        // don't intersect with two bottom corners when used in an "Auto" Panel
        geo.Spot1 = new Spot(0, 0, 0.3 * p1, 0);
        geo.Spot2 = new Spot(1, 1, -0.3 * p1, -0.3 * p1);
        return geo;
      });

      Shape.DefineFigureGenerator("RoundedLeftRectangle", (shape, w, h) => {
        // this figure takes one parameter, the size of the corner
        var p1 = 5.0;  // default corner size
        if (shape != null) {
          var param1 = shape.Parameter1;
          if (!double.IsNaN(param1) && param1 >= 0) p1 = param1;  // can't be negative or double.NaN
        }
        p1 = Math.Min(p1, w);  // limit by width & height
        p1 = Math.Min(p1, h / 3);
        var geo = new Geometry();
        // a single figure consisting of straight lines and quarter-circle arcs
        geo.Add(new PathFigure(w, 0)
          .Add(new PathSegment(SegmentType.Line, w, h))
          .Add(new PathSegment(SegmentType.Line, p1, h))
          .Add(new PathSegment(SegmentType.Arc, 90, 90, p1, h - p1, p1, p1))
          .Add(new PathSegment(SegmentType.Line, 0, p1))
          .Add(new PathSegment(SegmentType.Arc, 180, 90, p1, p1, p1, p1).Close()));
        // don't intersect with two top corners when used in an "Auto" Panel
        geo.Spot1 = new Spot(0, 0, 0.3 * p1, 0.3 * p1);
        geo.Spot2 = new Spot(1, 1, -0.3 * p1, 0);
        return geo;
      });

      Shape.DefineFigureGenerator("RoundedRightRectangle", (shape, w, h) => {
        // this figure takes one parameter, the size of the corner
        var p1 = 5.0;  // default corner size
        if (shape != null) {
          var param1 = shape.Parameter1;
          if (!double.IsNaN(param1) && param1 >= 0) p1 = param1;  // can't be negative or double.NaN
        }
        p1 = Math.Min(p1, w);  // limit by width & height
        p1 = Math.Min(p1, h / 3);
        var geo = new Geometry();
        // a single figure consisting of straight lines and quarter-circle arcs
        geo.Add(new PathFigure(0, 0)
          .Add(new PathSegment(SegmentType.Line, w - p1, 0))
          .Add(new PathSegment(SegmentType.Arc, 270, 90, w - p1, p1, p1, p1))
          .Add(new PathSegment(SegmentType.Line, w, h - p1))
          .Add(new PathSegment(SegmentType.Arc, 0, 90, w - p1, h - p1, p1, p1))
          .Add(new PathSegment(SegmentType.Line, 0, h).Close()));
        // don't intersect with two bottom corners when used in an "Auto" Panel
        geo.Spot1 = new Spot(0, 0, 0.3 * p1, 0);
        geo.Spot2 = new Spot(1, 1, -0.3 * p1, -0.3 * p1);
        return geo;
      });

      // these two figures have rounded ends
      Shape.DefineFigureGenerator("CapsuleH", (shape, w, h) => {
        var geo = new Geometry();
        if (w < h) {
          var fig = new PathFigure(w / 2, 0, true);
          fig.Add(new PathSegment(SegmentType.Bezier, w / 2, h, w, 0, w, h));
          fig.Add(new PathSegment(SegmentType.Bezier, w / 2, 0, 0, h, 0, 0));
          geo.Add(fig);
          return geo;
        } else {
          var fig = new PathFigure(h / 2, 0, true);
          geo.Add(fig);
          // Outline
          fig.Add(new PathSegment(SegmentType.Line, w - h / 2, 0));
          fig.Add(new PathSegment(SegmentType.Arc, 270, 180, w - h / 2, h / 2, h / 2, h / 2));
          fig.Add(new PathSegment(SegmentType.Line, w - h / 2, h));
          fig.Add(new PathSegment(SegmentType.Arc, 90, 180, h / 2, h / 2, h / 2, h / 2));
          return geo;
        }
      });
      Shape.DefineFigureGenerator("Capsule", "CapsuleH"); // synonym

      Shape.DefineFigureGenerator("CapsuleV", (shape, w, h) => {
        var geo = new Geometry();
        if (h < w) {
          var fig = new PathFigure(0, h / 2, true);
          fig.Add(new PathSegment(SegmentType.Bezier, w, h / 2, 0, h, w, h));
          fig.Add(new PathSegment(SegmentType.Bezier, 0, h / 2, w, 0, 0, 0));
          geo.Add(fig);
          return geo;
        } else {
          var fig = new PathFigure(0, w / 2, true);
          geo.Add(fig);
          // Outline
          fig.Add(new PathSegment(SegmentType.Arc, 180, 180, w / 2, w / 2, w / 2, w / 2));
          fig.Add(new PathSegment(SegmentType.Line, w, h - w / 2));
          fig.Add(new PathSegment(SegmentType.Arc, 0, 180, w / 2, h - w / 2, w / 2, w / 2));
          fig.Add(new PathSegment(SegmentType.Line, 0, w / 2));
          return geo;
        }
      });

      FigureParameter.SetFigureParameter("FramedRectangle", 0, new FigureParameter("ThicknessX", 8));
      FigureParameter.SetFigureParameter("FramedRectangle", 1, new FigureParameter("ThicknessY", 8));
      Shape.DefineFigureGenerator("FramedRectangle", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        var param2 = (shape != null) ? shape.Parameter2 : double.NaN;
        if (double.IsNaN(param1)) param1 = 8;  // default values PARAMETER 1 is for WIDTH
        if (double.IsNaN(param2)) param2 = 8;  // default values PARAMETER 2 is for HEIGHT

        var geo = new Geometry();
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);
        // outer rectangle, clockwise
        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h).Close());
        if (param1 < w / 2 && param2 < h / 2) {
          // inner rectangle, counter-clockwise
          fig.Add(new PathSegment(SegmentType.Move, param1, param2));  // subpath
          fig.Add(new PathSegment(SegmentType.Line, param1, h - param2));
          fig.Add(new PathSegment(SegmentType.Line, w - param1, h - param2));
          fig.Add(new PathSegment(SegmentType.Line, w - param1, param2).Close());
        }
        geo.SetSpots(0, 0, 1, 1, param1, param2, -param1, -param2);
        return geo;
      });

      FigureParameter.SetFigureParameter("Ring", 0, new FigureParameter("Thickness", 8));
      Shape.DefineFigureGenerator("Ring", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        if (double.IsNaN(param1) || param1 < 0) param1 = 8;

        var rad = w / 2;
        var geo = new Geometry();
        var fig = new PathFigure(w, w / 2, true);  // clockwise
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Arc, 0, 360, rad, rad, rad, rad).Close());

        var rad2 = Math.Max(rad - param1, 0);
        if (rad2 > 0) {  // counter-clockwise
          fig.Add(new PathSegment(SegmentType.Move, w / 2 + rad2, w / 2));
          fig.Add(new PathSegment(SegmentType.Arc, 0, -360, rad, rad, rad2, rad2).Close());
        }
        geo.Spot1 = GeneratorEllipseSpot1;
        geo.Spot2 = GeneratorEllipseSpot2;
        geo.DefaultStretch = GeometryStretch.Uniform;
        return geo;
      });

      Shape.DefineFigureGenerator("Cloud", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(.08034461 * w, .1944299 * h, true)
            .Add(new PathSegment(SegmentType.Bezier,
              .2008615 * w, .05349299 * h, -.09239631 * w, .07836421 * h, .1406031 * w, -.0542823 * h))
            .Add(new PathSegment(SegmentType.Bezier,
              .4338609 * w, .074219 * h, .2450511 * w, -.00697547 * h, .3776197 * w, -.01112067 * h))
            .Add(new PathSegment(SegmentType.Bezier,
              .6558228 * w, .07004196 * h, .4539471 * w, 0, .6066018 * w, -.02526587 * h))
            .Add(new PathSegment(SegmentType.Bezier,
              .8921095 * w, .08370865 * h, .6914277 * w, -.01904177 * h, .8921095 * w, -.01220843 * h))
            .Add(new PathSegment(SegmentType.Bezier,
              .9147671 * w, .3194596 * h, 1.036446 * w, .04105738 * h, 1.020377 * w, .3022052 * h))
            .Add(new PathSegment(SegmentType.Bezier,
              .9082935 * w, .562044 * h, 1.04448 * w, .360238 * h, .992256 * w, .5219009 * h))
            .Add(new PathSegment(SegmentType.Bezier,
              .9212406 * w, .8217117 * h, 1.032337 * w, .5771781 * h, 1.018411 * w, .8120651 * h))
            .Add(new PathSegment(SegmentType.Bezier,
              .7592566 * w, .9156953 * h, 1.028411 * w, .9571472 * h, .8556702 * w, 1.052487 * h))
            .Add(new PathSegment(SegmentType.Bezier,
              .5101666 * w, .9310455 * h, .7431877 * w, 1.009325 * h, .5624123 * w, 1.021761 * h))
            .Add(new PathSegment(SegmentType.Bezier,
              .2609328 * w, .9344623 * h, .4820677 * w, 1.031761 * h, .3030112 * w, 1.002796 * h))
            .Add(new PathSegment(SegmentType.Bezier,
              .08034461 * w, .870098 * h, .2329994 * w, 1.01518 * h, .03213784 * w, 1.01518 * h))
            .Add(new PathSegment(SegmentType.Bezier,
              .06829292 * w, .6545475 * h, -.02812061 * w, .9032597 * h, -.01205169 * w, .6835638 * h))
            .Add(new PathSegment(SegmentType.Bezier,
              .06427569 * w, .4265613 * h, -.01812061 * w, .6089503 * h, -.00606892 * w, .4555777 * h))
            .Add(new PathSegment(SegmentType.Bezier,
              .08034461 * w, .1944299 * h, -.01606892 * w, .3892545 * h, -.01205169 * w, .1944299 * h)))
          .SetSpots(.1, .1, .9, .9);
      });

      Shape.DefineFigureGenerator("StopSign", (shape, w, h) => {
        var part = 1 / (Math.Sqrt(2) + 2);
        return new Geometry()
          .Add(new PathFigure(part * w, 0, true)
            .Add(new PathSegment(SegmentType.Line, (1 - part) * w, 0))
            .Add(new PathSegment(SegmentType.Line, w, part * h))
            .Add(new PathSegment(SegmentType.Line, w, (1 - part) * h))
            .Add(new PathSegment(SegmentType.Line, (1 - part) * w, h))
            .Add(new PathSegment(SegmentType.Line, part * w, h))
            .Add(new PathSegment(SegmentType.Line, 0, (1 - part) * h))
            .Add(new PathSegment(SegmentType.Line, 0, part * h).Close()))
          .SetSpots(part / 2, part / 2, 1 - part / 2, 1 - part / 2);
      });

      FigureParameter.SetFigureParameter("Pie", 0, new FigureParameter("Start", 0, -360, 360));
      FigureParameter.SetFigureParameter("Pie", 1, new FigureParameter("Sweep", 315, -360, 360));
      Shape.DefineFigureGenerator("Pie", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        var param2 = (shape != null) ? shape.Parameter2 : double.NaN;
        if (double.IsNaN(param1)) param1 = 0;  // default values PARAMETER 1 is for Start Angle
        if (double.IsNaN(param2)) param2 = 315;  // default values PARAMETER 2 is for Sweep Angle

        var start = param1 % 360;
        if (start < 0) start += 360;
        var sweep = param2 % 360;
        var rad = Math.Min(w, h) / 2;

        return new Geometry()
          .Add(new PathFigure(rad, rad)  // start point
            .Add(new PathSegment(SegmentType.Arc,
              start, sweep,  // angles
              rad, rad,  // center
              rad, rad)  // radius
              .Close()));
      });

      Shape.DefineFigureGenerator("PiePiece", (shape, w, h) => {
        var factor = KAPPA / Math.Sqrt(2) * .5;
        var x1 = Math.Sqrt(2) / 2;
        var y1 = 1 - Math.Sqrt(2) / 2;
        return new Geometry()
          .Add(new PathFigure(w, h, true)
            .Add(new PathSegment(SegmentType.Bezier, x1 * w, y1 * h, w, (1 - factor) * h, (x1 + factor) * w, (y1 + factor) * h))
            .Add(new PathSegment(SegmentType.Line, 0, h).Close()));
      });

      FigureParameter.SetFigureParameter("ThickCross", 0, new FigureParameter("Thickness", 30));
      Shape.DefineFigureGenerator("ThickCross", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        if (double.IsNaN(param1) || param1 < 0) param1 = 30;

        var t = Math.Min(param1, w) / 2;
        var mx = w / 2;
        var my = h / 2;

        return new Geometry()
          .Add(new PathFigure(mx - t, 0, true)
            .Add(new PathSegment(SegmentType.Line, mx + t, 0))
            .Add(new PathSegment(SegmentType.Line, mx + t, my - t))

            .Add(new PathSegment(SegmentType.Line, w, my - t))
            .Add(new PathSegment(SegmentType.Line, w, my + t))
            .Add(new PathSegment(SegmentType.Line, mx + t, my + t))

            .Add(new PathSegment(SegmentType.Line, mx + t, h))
            .Add(new PathSegment(SegmentType.Line, mx - t, h))
            .Add(new PathSegment(SegmentType.Line, mx - t, my + t))

            .Add(new PathSegment(SegmentType.Line, 0, my + t))
            .Add(new PathSegment(SegmentType.Line, 0, my - t))
            .Add(new PathSegment(SegmentType.Line, mx - t, my - t).Close()));
      });

      FigureParameter.SetFigureParameter("ThinCross", 0, new FigureParameter("Thickness", 10));
      Shape.DefineFigureGenerator("ThinCross", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        if (double.IsNaN(param1) || param1 < 0) param1 = 10;

        var t = Math.Min(param1, w) / 2;
        var mx = w / 2;
        var my = h / 2;

        return new Geometry()
          .Add(new PathFigure(mx - t, 0, true)
            .Add(new PathSegment(SegmentType.Line, mx + t, 0))
            .Add(new PathSegment(SegmentType.Line, mx + t, my - t))

            .Add(new PathSegment(SegmentType.Line, w, my - t))
            .Add(new PathSegment(SegmentType.Line, w, my + t))
            .Add(new PathSegment(SegmentType.Line, mx + t, my + t))

            .Add(new PathSegment(SegmentType.Line, mx + t, h))
            .Add(new PathSegment(SegmentType.Line, mx - t, h))
            .Add(new PathSegment(SegmentType.Line, mx - t, my + t))

            .Add(new PathSegment(SegmentType.Line, 0, my + t))
            .Add(new PathSegment(SegmentType.Line, 0, my - t))
            .Add(new PathSegment(SegmentType.Line, mx - t, my - t).Close()));
      });


      FigureParameter.SetFigureParameter("ThickX", 0, new FigureParameter("Thickness", 30));
      Shape.DefineFigureGenerator("ThickX", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        if (double.IsNaN(param1) || param1 < 0) param1 = 30;
        if (w == 0 || h == 0) {
          var geo = new Geometry(GeometryType.Rectangle) {
            StartX = 0,
            StartY = 0,
            EndX = w,
            EndY = h
          };
          return geo;
        } else {
          var w2 = w / 2;
          var h2 = h / 2;
          var a2 = Math.Atan2(h, w);
          var dx = param1 - Math.Min(Math.Cos(a2) * param1 / 2, w2);
          var dy = param1 - Math.Min(Math.Sin(a2) * param1 / 2, h2);

          var geo = new Geometry();
          var fig = new PathFigure(dx, 0, true);
          geo.Add(fig);
          fig.Add(new PathSegment(SegmentType.Line, w2, .2 * h));
          fig.Add(new PathSegment(SegmentType.Line, w - dx, 0));
          fig.Add(new PathSegment(SegmentType.Line, w, dy));
          fig.Add(new PathSegment(SegmentType.Line, .8 * w, h2));
          fig.Add(new PathSegment(SegmentType.Line, w, h - dy));
          fig.Add(new PathSegment(SegmentType.Line, w - dx, h));
          fig.Add(new PathSegment(SegmentType.Line, w2, .8 * h));
          fig.Add(new PathSegment(SegmentType.Line, dx, h));
          fig.Add(new PathSegment(SegmentType.Line, 0, h - dy));
          fig.Add(new PathSegment(SegmentType.Line, .2 * w, h2));
          fig.Add(new PathSegment(SegmentType.Line, 0, dy).Close());
          return geo;
        }
      });

      FigureParameter.SetFigureParameter("ThinX", 0, new FigureParameter("Thickness", 10));
      Shape.DefineFigureGenerator("ThinX", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        if (double.IsNaN(param1) || param1 < 0) param1 = 10;

        var geo = new Geometry();
        var fig = new PathFigure(.1 * w, 0, true);
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Line, .5 * w, .4 * h));
        fig.Add(new PathSegment(SegmentType.Line, .9 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, .1 * h));
        fig.Add(new PathSegment(SegmentType.Line, .6 * w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, .9 * h));
        fig.Add(new PathSegment(SegmentType.Line, .9 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, .5 * w, .6 * h));
        fig.Add(new PathSegment(SegmentType.Line, .1 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, .9 * h));
        fig.Add(new PathSegment(SegmentType.Line, .4 * w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, .1 * h).Close());
        return geo;
      });

      // adjust the width of the vertical beam
      FigureParameter.SetFigureParameter("SquareIBeam", 0, new FigureParameter("BeamWidth", 0.2, 0.1, 0.9));
      Shape.DefineFigureGenerator("SquareIBeam", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;  // width of the ibeam in % of the total width
        if (double.IsNaN(param1)) param1 = .2;

        var geo = new Geometry();
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, param1 * h));
        fig.Add(new PathSegment(SegmentType.Line, (.5 + param1 / 2) * w, param1 * h));
        fig.Add(new PathSegment(SegmentType.Line, (.5 + param1 / 2) * w, (1 - param1) * h));
        fig.Add(new PathSegment(SegmentType.Line, w, (1 - param1) * h));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, (1 - param1) * h));
        fig.Add(new PathSegment(SegmentType.Line, (.5 - param1 / 2) * w, (1 - param1) * h));
        fig.Add(new PathSegment(SegmentType.Line, (.5 - param1 / 2) * w, param1 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, param1 * h).Close());
        return geo;
      });

      // parameter allows it easy to adjust the roundness of the curves that cut inward
      FigureParameter.SetFigureParameter("RoundedIBeam", 0, new FigureParameter("Curviness", .5, .05, .65));
      Shape.DefineFigureGenerator("RoundedIBeam", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;  // curviness of the ibeam relative to total width
        if (double.IsNaN(param1)) param1 = .5;

        var geo = new Geometry();
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Bezier, w, h, Math.Abs((1 - param1)) * w, .25 * h, Math.Abs((1 - param1)) * w, .75 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, 0, param1 * w, .75 * h,
        param1 * w, .25 * h).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("HalfEllipse", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(0, 0, true)
            .Add(new PathSegment(SegmentType.Bezier, w, .5 * h, KAPPA * w, 0, w, (.5 - KAPPA / 2) * h))
            .Add(new PathSegment(SegmentType.Bezier, 0, h, w, (.5 + KAPPA / 2) * h, KAPPA * w, h).Close()))
          .SetSpots(0, 0.156, 0.844, 0.844);
      });

      Shape.DefineFigureGenerator("Crescent", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(0, 0, true)
            .Add(new PathSegment(SegmentType.Bezier,
              0, h, w, 0, w, h))
            .Add(new PathSegment(SegmentType.Bezier,
              0, 0, 0.5 * w, 0.75 * h, 0.5 * w, 0.25 * h).Close()))
          .SetSpots(.311, 0.266, 0.744, 0.744);
      });

      Shape.DefineFigureGenerator("Heart", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(.5 * w, h, true)
            .Add(new PathSegment(SegmentType.Bezier, 0, .3 * h, .1 * w, .8 * h, 0, .5 * h))
            .Add(new PathSegment(SegmentType.Bezier, .5 * w, .3 * h, 0, 0, .45 * w, 0))
            .Add(new PathSegment(SegmentType.Bezier, w, .3 * h, .55 * w, 0, w, 0))
            .Add(new PathSegment(SegmentType.Bezier, .5 * w, h, w, .5 * h, .9 * w, .8 * h).Close()))
          .SetSpots(.14, .29, .86, .78);
      });

      Shape.DefineFigureGenerator("Spade", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(.5 * w, 0, true)
            .Add(new PathSegment(SegmentType.Line, .51 * w, .01 * h))
            .Add(new PathSegment(SegmentType.Bezier, w, .5 * h, .6 * w, .2 * h, w, .25 * h))
            .Add(new PathSegment(SegmentType.Bezier, .55 * w, .7 * h, w, .8 * h, .6 * w, .8 * h))
            .Add(new PathSegment(SegmentType.Bezier, .75 * w, h, .5 * w, .75 * h, .55 * w, .95 * h))
            .Add(new PathSegment(SegmentType.Line, .25 * w, h))
            .Add(new PathSegment(SegmentType.Bezier, .45 * w, .7 * h, .45 * w, .95 * h, .5 * w, .75 * h))
            .Add(new PathSegment(SegmentType.Bezier, 0, .5 * h, .4 * w, .8 * h, 0, .8 * h))
            .Add(new PathSegment(SegmentType.Bezier, .49 * w, .01 * h, 0, .25 * h, .4 * w, .2 * h).Close()))
          .SetSpots(.14, .26, .86, .78);
      });

      Shape.DefineFigureGenerator("Club", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(.4 * w, .6 * h, true);
        geo.Add(fig);
        // Start the base
        fig.Add(new PathSegment(SegmentType.Bezier, .15 * w, h, .5 * w, .75 * h, .45 * w, .95 * h));
        fig.Add(new PathSegment(SegmentType.Line, .85 * w, h));
        fig.Add(new PathSegment(SegmentType.Bezier, .6 * w, .6 * h, .55 * w, .95 * h, .5 * w, .75 * h));
        // First circle:
        var r = .2;  // radius
        var cx = .3;  // offset from Center x
        var cy = 0d;  // offset from Center y
        var d = r * KAPPA;
        fig.Add(new PathSegment(SegmentType.Bezier, (.5 + cx) * w, (.5 + r + cy) * h,
          (.5 - r + cx) * w, (.5 + d + cy) * h,
          (.5 - d + cx) * w, (.5 + r + cy) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (1 - .5 + r + cx) * w, (.5 + cy) * h,
          (.5 + d + cx) * w, (.5 + r + cy) * h,
          (.5 + r + cx) * w, (.5 + d + cy) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (.5 + cx) * w, (.5 - r + cy) * h,
          (1 - .5 + r + cx) * w, (.5 - d + cy) * h,
          (.5 + d + cx) * w, (.5 - r + cy) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (.65) * w, (0.36771243) * h,
          (.5 - d + cx) * w, (.5 - r + cy) * h,
          (.5 - r + cx + .05) * w, (.5 - d + cy - .02) * h));
        r = .2;  // radius
        cx = 0;  // offset from Center x
        cy = -.3;  // offset from Center y
        d = r * KAPPA;
        fig.Add(new PathSegment(SegmentType.Bezier, (1 - .5 + r + cx) * w, (.5 + cy) * h,
          (.5 + d + cx) * w, (.5 + r + cy) * h,
          (.5 + r + cx) * w, (.5 + d + cy) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (.5 + cx) * w, (.5 - r + cy) * h,
          (1 - .5 + r + cx) * w, (.5 - d + cy) * h,
          (.5 + d + cx) * w, (.5 - r + cy) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (.5 - r + cx) * w, (.5 + cy) * h,
          (.5 - d + cx) * w, (.5 - r + cy) * h,
          (.5 - r + cx) * w, (.5 - d + cy) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (.5 - d + cx) * w, (.5 + r + cy) * h,
          (.5 - r + cx) * w, (.5 + d + cy) * h,
          (.5 - d + cx) * w, (.5 + r + cy) * h));
        r = .2;  // radius
        cx = -.3;  // offset from Center x
        cy = 0;  // offset from Center y
        d = r * KAPPA;
        fig.Add(new PathSegment(SegmentType.Bezier, (.5 + cx) * w, (.5 - r + cy) * h,
          (1 - .5 + r + cx - .05) * w, (.5 - d + cy - .02) * h,
          (.5 + d + cx) * w, (.5 - r + cy) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (.5 - r + cx) * w, (.5 + cy) * h,
          (.5 - d + cx) * w, (.5 - r + cy) * h,
          (.5 - r + cx) * w, (.5 - d + cy) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (.5 + cx) * w, (.5 + r + cy) * h,
          (.5 - r + cx) * w, (.5 + d + cy) * h,
          (.5 - d + cx) * w, (.5 + r + cy) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, .4 * w, .6 * h,
          (.5 + d + cx) * w, (.5 + r + cy) * h,
          (.5 + r + cx) * w, (.5 + d + cy) * h).Close());
        geo.SetSpots(.06, .33, .93, .68);
        return geo;
      });

      Shape.DefineFigureGenerator("YinYang", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(w * 0.5, 0, true);
        geo.Add(fig);
        // Right semi-circle
        fig.Add(new PathSegment(SegmentType.Arc, 270, 180, w * 0.5, w * 0.5, w * 0.5, w * 0.5));
        // bottom semi-circle
        fig.Add(new PathSegment(SegmentType.Arc, 90, -180, w * 0.5, w * 0.75, w * 0.25, w * 0.25));
        // top semi-circle
        fig.Add(new PathSegment(SegmentType.Arc, 90, 180, w * 0.5, w * 0.25, w * 0.25, w * 0.25));
        var radius = .1;  // of the small circles
        var centerx = .5;
        var centery = .25;
        // Top small circle, goes counter-clockwise
        fig.Add(new PathSegment(SegmentType.Move, (centerx + radius) * w, (centery) * h));
        fig.Add(new PathSegment(SegmentType.Arc, 0, -360, w * centerx, h * centery, radius * w, radius * w).Close());  // Right semi-circle
        // Left semi-circle
        fig = new PathFigure(w * 0.5, 0, false);
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Arc, 270, -180, w * 0.5, w * 0.5, w * 0.5, w * 0.5));
        centery = .75;
        // Bottom small circle
        fig = new PathFigure((centerx + radius) * w, (centery) * h, true);  // Not a subpath
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Arc, 0, 360, w * centerx, h * centery, radius * w, radius * w).Close());  // Right semi-circle
        geo.DefaultStretch = GeometryStretch.Uniform;
        return geo;
      });

      Shape.DefineFigureGenerator("Peace", (shape, w, h) => {
        var a = 1.0 - 0.1464466094067262;  // at 45 degrees
        var w2 = 0.5 * w;
        var h2 = 0.5 * h;
        return new Geometry()
          .Add(new PathFigure(w2, 0, false)
            .Add(new PathSegment(SegmentType.Arc, 270, 360, w2, h2, w2, h2))
            .Add(new PathSegment(SegmentType.Line, w2, h))
            .Add(new PathSegment(SegmentType.Move, w2, h2))
            .Add(new PathSegment(SegmentType.Line, (1.0 - a) * w, a * h))
            .Add(new PathSegment(SegmentType.Move, w2, h2))
            .Add(new PathSegment(SegmentType.Line, a * w, a * h)));
      });

      Shape.DefineFigureGenerator("NotAllowed", (shape, w, h) => {
        var geo = new Geometry();
        var cpOffset = KAPPA * .5;
        var radius = .5;
        var centerx = .5;
        var centery = .5;
        var fig = new PathFigure(centerx * w, (centery - radius) * h);
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w, centery * h, (centerx - cpOffset) * w, (centery - radius) * h,
          (centerx - radius) * w, (centery - cpOffset) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radius) * h, (centerx - radius) * w, (centery + cpOffset) * h,
          (centerx - cpOffset) * w, (centery + radius) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, centery * h, (centerx + cpOffset) * w, (centery + radius) * h,
          (centerx + radius) * w, (centery + cpOffset) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - radius) * h, (centerx + radius) * w, (centery - cpOffset) * h,
          (centerx + cpOffset) * w, (centery - radius) * h));
        // Inner circle, composed of two parts, separated by
        // a beam across going from top-right to bottom-left.
        radius = .40;
        cpOffset = KAPPA * .40;
        // First we cut up the top right 90 degree curve into two smaller
        // curves.
        // Since its clockwise, StartOfArrow is the first of the two points
        // on the circle. EndOfArrow is the other one.
        var startOfArrowc1 = new Point();
        var startOfArrowc2 = new Point();
        var startOfArrow = new Point();
        var unused = new Point();
        BreakUpBezier(centerx, centery - radius,
          centerx + cpOffset, centery - radius,
          centerx + radius, centery - cpOffset,
          centerx + radius, centery, .42, out startOfArrowc1,
          out startOfArrowc2, out startOfArrow, out unused, out unused);
        var endOfArrowc1 = new Point();
        var endOfArrowc2 = new Point();
        var endOfArrow = new Point();
        BreakUpBezier(centerx, centery - radius,
          centerx + cpOffset, centery - radius,
          centerx + radius, centery - cpOffset,
          centerx + radius, centery, .58, out unused,
          out unused, out endOfArrow, out endOfArrowc1, out endOfArrowc2);
        // Cut up the bottom left 90 degree curve into two smaller curves.
        var startOfArrow2c1 = new Point();
        var startOfArrow2c2 = new Point();
        var startOfArrow2 = new Point();
        BreakUpBezier(centerx, centery + radius,
          centerx - cpOffset, centery + radius,
          centerx - radius, centery + cpOffset,
          centerx - radius, centery, .42, out startOfArrow2c1,
          out startOfArrow2c2, out startOfArrow2, out unused, out unused);
        var endOfArrow2c1 = new Point();
        var endOfArrow2c2 = new Point();
        var endOfArrow2 = new Point();
        BreakUpBezier(centerx, centery + radius,
          centerx - cpOffset, centery + radius,
          centerx - radius, centery + cpOffset,
          centerx - radius, centery, .58, out unused,
          out unused, out endOfArrow2, out endOfArrow2c1, out endOfArrow2c2);
        fig.Add(new PathSegment(SegmentType.Move, endOfArrow2.X * w, endOfArrow2.Y * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w, centery * h, endOfArrow2c1.X * w, endOfArrow2c1.Y * h,
          endOfArrow2c2.X * w, endOfArrow2c2.Y * h));
        fig.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - radius) * h, (centerx - radius) * w, (centery - cpOffset) * h,
          (centerx - cpOffset) * w, (centery - radius) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, startOfArrow.X * w, startOfArrow.Y * h, startOfArrowc1.X * w, startOfArrowc1.Y * h,
          startOfArrowc2.X * w, startOfArrowc2.Y * h));
        fig.Add(new PathSegment(SegmentType.Line, endOfArrow2.X * w, endOfArrow2.Y * h).Close());
        fig.Add(new PathSegment(SegmentType.Move, startOfArrow2.X * w, startOfArrow2.Y * h));
        fig.Add(new PathSegment(SegmentType.Line, endOfArrow.X * w, endOfArrow.Y * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, centery * h, endOfArrowc1.X * w, endOfArrowc1.Y * h,
          endOfArrowc2.X * w, endOfArrowc2.Y * h));
        fig.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radius) * h, (centerx + radius) * w, (centery + cpOffset) * h,
          (centerx + cpOffset) * w, (centery + radius) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, startOfArrow2.X * w, startOfArrow2.Y * h, startOfArrow2c1.X * w, startOfArrow2c1.Y * h,
          startOfArrow2c2.X * w, startOfArrow2c2.Y * h).Close());
        geo.DefaultStretch = GeometryStretch.Uniform;
        return geo;
      });

      Shape.DefineFigureGenerator("Fragile", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(0, 0, true)
            .Add(new PathSegment(SegmentType.Line, .25 * w, 0))
            .Add(new PathSegment(SegmentType.Line, .2 * w, .15 * h))
            .Add(new PathSegment(SegmentType.Line, .3 * w, .25 * h))
            .Add(new PathSegment(SegmentType.Line, .29 * w, .33 * h))
            .Add(new PathSegment(SegmentType.Line, .35 * w, .25 * h))
            .Add(new PathSegment(SegmentType.Line, .3 * w, .15 * h))
            .Add(new PathSegment(SegmentType.Line, .4 * w, 0))
            .Add(new PathSegment(SegmentType.Line, w, 0))
            // Left Side
            .Add(new PathSegment(SegmentType.Bezier, .55 * w, .5 * h, w, .25 * h, .75 * w, .5 * h))
            .Add(new PathSegment(SegmentType.Line, .55 * w, .9 * h))
            // The base
            .Add(new PathSegment(SegmentType.Line, .7 * w, .9 * h))
            .Add(new PathSegment(SegmentType.Line, .7 * w, h))
            .Add(new PathSegment(SegmentType.Line, .3 * w, h))
            .Add(new PathSegment(SegmentType.Line, .3 * w, .9 * h))
            // Right side
            .Add(new PathSegment(SegmentType.Line, .45 * w, .9 * h))
            .Add(new PathSegment(SegmentType.Line, .45 * w, .5 * h))
            .Add(new PathSegment(SegmentType.Bezier, 0, 0, .25 * w, .5 * h, 0, .25 * h).Close()));
      });

      FigureParameter.SetFigureParameter("HourGlass", 0, new FigureParameter("Thickness", 30));
      Shape.DefineFigureGenerator("HourGlass", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;  // width at middle of hourglass
        if (double.IsNaN(param1) || param1 < 0) param1 = 30;
        if (param1 > w) param1 = w;
        var x1 = (w - param1) / 2;
        var x2 = x1 + param1;
        return new Geometry()
               .Add(new PathFigure(x2, 0.5 * h)
                    .Add(new PathSegment(SegmentType.Line, w, h))
                    .Add(new PathSegment(SegmentType.Line, 0, h))
                    .Add(new PathSegment(SegmentType.Line, x1, 0.5 * h))
                    .Add(new PathSegment(SegmentType.Line, 0, 0))
                    .Add(new PathSegment(SegmentType.Line, w, 0).Close()));
      });

      Shape.DefineFigureGenerator("Lightning", (shape, w, h) => {
        return new Geometry()
               .Add(new PathFigure(0, 0.55 * h)
                    .Add(new PathSegment(SegmentType.Line, 0.6 * w, 0))
                    .Add(new PathSegment(SegmentType.Line, 0.3 * w, 0.45 * h))
                    .Add(new PathSegment(SegmentType.Line, w, 0.45 * h))
                    .Add(new PathSegment(SegmentType.Line, 0.4 * w, h))
                    .Add(new PathSegment(SegmentType.Line, 0.7 * w, 0.55 * h).Close()));
      });

      Shape.DefineFigureGenerator("GenderMale", (shape, w, h) => {
        var geo = new Geometry();
        var cpOffset = KAPPA * .4;
        var radius = .4;
        var centerx = .5;
        var centery = .5;
        var unused = new Point();
        var mid = new Point();
        var c1 = new Point();
        var c2 = new Point();
        var fig = new PathFigure((centerx - radius) * w, centery * h, false);
        geo.Add(fig);

        // Outer circle
        fig.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - radius) * h, (centerx - radius) * w, (centery - cpOffset) * h,
          (centerx - cpOffset) * w, (centery - radius) * h));
        BreakUpBezier(centerx, centery - radius,
          centerx + cpOffset, centery - radius,
          centerx + radius, centery - cpOffset,
          centerx + radius, centery, .44, out c1,
          out c2, out mid, out unused, out unused);
        fig.Add(new PathSegment(SegmentType.Bezier, mid.X * w, mid.Y * h, c1.X * w, c1.Y * h, c2.X * w, c2.Y * h));
        var startOfArrow = new Point(mid.X, mid.Y);
        BreakUpBezier(centerx, centery - radius,
          centerx + cpOffset, centery - radius,
          centerx + radius, centery - cpOffset,
          centerx + radius, centery, .56, out unused,
          out unused, out mid, out c1, out c2);
        var endOfArrow = new Point(mid.X, mid.Y);
        fig.Add(new PathSegment(SegmentType.Line, (startOfArrow.X * .1 + .95 * .9) * w,
          (startOfArrow.Y * .1) * h));
        fig.Add(new PathSegment(SegmentType.Line, .85 * w, (startOfArrow.Y * .1) * h));
        fig.Add(new PathSegment(SegmentType.Line, .85 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, .15 * h));
        fig.Add(new PathSegment(SegmentType.Line, (endOfArrow.X * .1 + .9) * w, .15 * h));
        fig.Add(new PathSegment(SegmentType.Line, (endOfArrow.X * .1 + .9) * w,
          (endOfArrow.Y * .1 + .05 * .9) * h));
        fig.Add(new PathSegment(SegmentType.Line, endOfArrow.X * w, endOfArrow.Y * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, centery * h, c1.X * w, c1.Y * h, c2.X * w, c2.Y * h));
        fig.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radius) * h, (centerx + radius) * w, (centery + cpOffset) * h,
          (centerx + cpOffset) * w, (centery + radius) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w, centery * h, (centerx - cpOffset) * w, (centery + radius) * h,
          (centerx - radius) * w, (centery + cpOffset) * h));
        // Inner circle
        radius = .35;
        cpOffset = KAPPA * .35;
        var fig2 = new PathFigure(centerx * w, (centery - radius) * h, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w, centery * h, (centerx - cpOffset) * w, (centery - radius) * h,
          (centerx - radius) * w, (centery - cpOffset) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radius) * h, (centerx - radius) * w, (centery + cpOffset) * h,
          (centerx - cpOffset) * w, (centery + radius) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, centery * h, (centerx + cpOffset) * w, (centery + radius) * h,
          (centerx + radius) * w, (centery + cpOffset) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - radius) * h, (centerx + radius) * w, (centery - cpOffset) * h,
          (centerx + cpOffset) * w, (centery - radius) * h));
        var fig3 = new PathFigure((centerx - radius) * w, centery * h, false);
        geo.Add(fig3);
        geo.Spot1 = new Spot(.202, .257);
        geo.Spot2 = new Spot(.792, .739);
        geo.DefaultStretch = GeometryStretch.Uniform;
        return geo;
      });

      Shape.DefineFigureGenerator("GenderFemale", (shape, w, h) => {
        var geo = new Geometry();
        // Outer Circle
        var r = .375;  // radius
        var cx = 0;  // offset from Center x
        var cy = -.125;  // offset from Center y
        var d = r * KAPPA;
        var fig = new PathFigure((.525 + cx) * w, (.5 + r + cy) * h, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Bezier, (1 - .5 + r + cx) * w, (.5 + cy) * h, (.5 + d + cx) * w, (.5 + r + cy) * h,
          (.5 + r + cx) * w, (.5 + d + cy) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (.5 + cx) * w, (.5 - r + cy) * h, (1 - .5 + r + cx) * w, (.5 - d + cy) * h,
          (.5 + d + cx) * w, (.5 - r + cy) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (.5 - r + cx) * w, (.5 + cy) * h, (.5 - d + cx) * w, (.5 - r + cy) * h,
          (.5 - r + cx) * w, (.5 - d + cy) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (.475 + cx) * w, (.5 + r + cy) * h, (.5 - r + cx) * w, (.5 + d + cy) * h,
          (.5 - d + cx) * w, (.5 + r + cy) * h));
        // Legs
        fig.Add(new PathSegment(SegmentType.Line, .475 * w, .85 * h));
        fig.Add(new PathSegment(SegmentType.Line, .425 * w, .85 * h));
        fig.Add(new PathSegment(SegmentType.Line, .425 * w, .9 * h));
        fig.Add(new PathSegment(SegmentType.Line, .475 * w, .9 * h));
        fig.Add(new PathSegment(SegmentType.Line, .475 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, .525 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, .525 * w, .9 * h));
        fig.Add(new PathSegment(SegmentType.Line, .575 * w, .9 * h));
        fig.Add(new PathSegment(SegmentType.Line, .575 * w, .85 * h));
        fig.Add(new PathSegment(SegmentType.Line, .525 * w, .85 * h).Close());
        // Inner circle
        r = .325;  // radius
        cx = 0;  // offset from Center x
        cy = -.125;  // offset from Center y
        d = r * KAPPA;
        fig = new PathFigure((1 - .5 + r + cx) * w, (.5 + cy) * h, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Bezier, (.5 + cx) * w, (.5 + r + cy) * h, (.5 + r + cx) * w, (.5 + d + cy) * h,
          (.5 + d + cx) * w, (.5 + r + cy) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (.5 - r + cx) * w, (.5 + cy) * h, (.5 - d + cx) * w, (.5 + r + cy) * h,
          (.5 - r + cx) * w, (.5 + d + cy) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (.5 + cx) * w, (.5 - r + cy) * h, (.5 - r + cx) * w, (.5 - d + cy) * h,
          (.5 - d + cx) * w, (.5 - r + cy) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (1 - .5 + r + cx) * w, (.5 + cy) * h, (.5 + d + cx) * w, (.5 - r + cy) * h,
          (1 - .5 + r + cx) * w, (.5 - d + cy) * h));
        fig = new PathFigure((.525 + cx) * w, (.5 + r + cy) * h, false);
        geo.Add(fig);
        geo.Spot1 = new Spot(.232, .136);
        geo.Spot2 = new Spot(.682, .611);
        geo.DefaultStretch = GeometryStretch.Uniform;
        return geo;
      });

      Shape.DefineFigureGenerator("LogicImplies", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        if (double.IsNaN(param1)) param1 = .2;  // Distance the arrow folds from the right
        return new Geometry()
          .Add(new PathFigure((1 - param1) * w, 0, false)
            .Add(new PathSegment(SegmentType.Line, w, .5 * h))
            .Add(new PathSegment(SegmentType.Line, (1 - param1) * w, h))
            .Add(new PathSegment(SegmentType.Move, 0, .5 * h))
            .Add(new PathSegment(SegmentType.Line, w, .5 * h)))
          .SetSpots(0, 0, 0.8, 0.5);
      });

      Shape.DefineFigureGenerator("LogicIff", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        if (double.IsNaN(param1)) param1 = .2;  // Distance the arrow folds from the right
        return new Geometry()
          .Add(new PathFigure((1 - param1) * w, 0, false)
            .Add(new PathSegment(SegmentType.Line, w, .5 * h))
            .Add(new PathSegment(SegmentType.Line, (1 - param1) * w, h))
            .Add(new PathSegment(SegmentType.Move, 0, .5 * h))
            .Add(new PathSegment(SegmentType.Line, w, .5 * h))
            .Add(new PathSegment(SegmentType.Move, param1 * w, 0))
            .Add(new PathSegment(SegmentType.Line, 0, .5 * h))
            .Add(new PathSegment(SegmentType.Line, param1 * w, h)))
          .SetSpots(0.2, 0, 0.8, 0.5);
      });

      Shape.DefineFigureGenerator("LogicNot", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(0, 0, false)
            .Add(new PathSegment(SegmentType.Line, w, 0))
            .Add(new PathSegment(SegmentType.Line, w, h)));
      });

      Shape.DefineFigureGenerator("LogicAnd", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(0, h, false)
            .Add(new PathSegment(SegmentType.Line, .5 * w, 0))
            .Add(new PathSegment(SegmentType.Line, w, h)))
          .SetSpots(0.25, 0.5, 0.75, 1);
      });

      Shape.DefineFigureGenerator("LogicOr", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(0, 0, false)
            .Add(new PathSegment(SegmentType.Line, .5 * w, h))
            .Add(new PathSegment(SegmentType.Line, w, 0)))
          .SetSpots(0.219, 0, 0.78, 0.409);
      });

      Shape.DefineFigureGenerator("LogicXor", (shape, w, h) => {
        var geo = new Geometry()
          .Add(new PathFigure(.5 * w, 0, false)
            .Add(new PathSegment(SegmentType.Line, .5 * w, h))
            .Add(new PathSegment(SegmentType.Move, 0, .5 * h))
            .Add(new PathSegment(SegmentType.Line, w, .5 * h))
            .Add(new PathSegment(SegmentType.Arc, 0, 360, .5 * w, .5 * h, .5 * w, .5 * h)));
        geo.DefaultStretch = GeometryStretch.Uniform;
        return geo;
      });

      Shape.DefineFigureGenerator("LogicTruth", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(0, 0, false)
            .Add(new PathSegment(SegmentType.Line, w, 0))
            .Add(new PathSegment(SegmentType.Move, .5 * w, 0))
            .Add(new PathSegment(SegmentType.Line, .5 * w, h)));
      });

      Shape.DefineFigureGenerator("LogicFalsity", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(0, h, false)
            .Add(new PathSegment(SegmentType.Line, w, h))
            .Add(new PathSegment(SegmentType.Move, .5 * w, h))
            .Add(new PathSegment(SegmentType.Line, .5 * w, 0)));
      });

      Shape.DefineFigureGenerator("LogicThereExists", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(0, 0, false)
            .Add(new PathSegment(SegmentType.Line, w, 0))
            .Add(new PathSegment(SegmentType.Line, w, .5 * h))
            .Add(new PathSegment(SegmentType.Line, 0, .5 * h))
            .Add(new PathSegment(SegmentType.Move, w, .5 * h))
            .Add(new PathSegment(SegmentType.Line, w, h))
            .Add(new PathSegment(SegmentType.Line, 0, h)));
      });

      Shape.DefineFigureGenerator("LogicForAll", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(0, 0, false)
            .Add(new PathSegment(SegmentType.Line, .5 * w, h))
            .Add(new PathSegment(SegmentType.Line, w, 0))
            .Add(new PathSegment(SegmentType.Move, .25 * w, .5 * h))
            .Add(new PathSegment(SegmentType.Line, .75 * w, .5 * h)))
          .SetSpots(0.25, 0, 0.75, 0.5);
      });

      Shape.DefineFigureGenerator("LogicIsDefinedAs", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(0, 0, false)
            .Add(new PathSegment(SegmentType.Line, w, 0))
            .Add(new PathSegment(SegmentType.Move, 0, .5 * h))
            .Add(new PathSegment(SegmentType.Line, w, .5 * h))
            .Add(new PathSegment(SegmentType.Move, 0, h))
            .Add(new PathSegment(SegmentType.Line, w, h)))
          .SetSpots(0.01, 0.01, 0.99, 0.49);
      });

      Shape.DefineFigureGenerator("LogicIntersect", (shape, w, h) => {
        var radius = 0.5;
        return new Geometry()
          .Add(new PathFigure(0, h, false)
            .Add(new PathSegment(SegmentType.Line, 0, radius * h))
            .Add(new PathSegment(SegmentType.Arc, 180, 180, radius * w, radius * h, radius * w, radius * h))
            .Add(new PathSegment(SegmentType.Line, w, h)))
          .SetSpots(0, 0.5, 1, 1);
      });

      Shape.DefineFigureGenerator("LogicUnion", (shape, w, h) => {
        var radius = 0.5;
        return new Geometry()
          .Add(new PathFigure(w, 0, false)
            .Add(new PathSegment(SegmentType.Line, w, radius * h))
            .Add(new PathSegment(SegmentType.Arc, 0, 180, radius * w, radius * h, radius * w, radius * h))
            .Add(new PathSegment(SegmentType.Line, 0, 0)))
          .SetSpots(0, 0, 1, 0.5);
      });

      FigureParameter.SetFigureParameter("Arrow", 0, new FigureParameter("ArrowheadWidth", .3, .01, .99));
      FigureParameter.SetFigureParameter("Arrow", 1, new FigureParameter("TailHeight", .3, .01, .99));
      Shape.DefineFigureGenerator("Arrow", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;  // % width of arrowhead
        if (double.IsNaN(param1)) param1 = .3;
        var param2 = (shape != null) ? shape.Parameter2 : double.NaN;  // % height of tail
        if (double.IsNaN(param2)) param2 = .3;

        var x = (1 - param1) * w;
        var y1 = (.5 - param2 / 2) * h;
        var y2 = (.5 + param2 / 2) * h;

        var geo = new Geometry();
        var fig = new PathFigure(0, y1, true);
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Line, x, y1));
        fig.Add(new PathSegment(SegmentType.Line, x, 0));
        fig.Add(new PathSegment(SegmentType.Line, x, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, x, h));
        fig.Add(new PathSegment(SegmentType.Line, x, y2));
        fig.Add(new PathSegment(SegmentType.Line, 0, y2).Close());
        geo.Spot1 = new Spot(0, y1 / h);
        var tempPoint = new Point();
        var temp = GetIntersection(0, y2 / h,
            1, y2 / h,
            x / w, 1,
            1, .5,
            out tempPoint);
        geo.Spot2 = new Spot(temp.X, temp.Y);
        return geo;
      });

      // Arrow with absolutes instead of scaling
      FigureParameter.SetFigureParameter("Arrow2", 0, new FigureParameter("ArrowheadWidth", 30));
      FigureParameter.SetFigureParameter("Arrow2", 1, new FigureParameter("TailHeight", 30));
      Shape.DefineFigureGenerator("Arrow2", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;  // width of arrowhead
        if (double.IsNaN(param1)) param1 = 30;
        if (param1 > w) param1 = w;
        var param2 = (shape != null) ? shape.Parameter2 : double.NaN;  // height of tail
        if (double.IsNaN(param2)) param2 = 30;
        param2 = Math.Min(param2, h / 2);

        var x = w - param1;
        var y1 = (h - param2) / 2;
        var y2 = y1 + param2;

        var geo = new Geometry();
        var fig = new PathFigure(0, y1, true);
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Line, x, y1));
        fig.Add(new PathSegment(SegmentType.Line, x, 0));
        fig.Add(new PathSegment(SegmentType.Line, x, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, x, h));
        fig.Add(new PathSegment(SegmentType.Line, x, y2));
        fig.Add(new PathSegment(SegmentType.Line, 0, y2).Close());
        geo.Spot1 = new Spot(0, y1 / h);
        var tempPoint = new Point();
        var temp = GetIntersection(0, y2 / h,
            1, y2 / h,
            x / w, 1,
            1, .5,
            out tempPoint);
        geo.Spot2 = new Spot(temp.X, temp.Y);
        return geo;
      });

      Shape.DefineFigureGenerator("Chevron", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, .5 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, .5 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Line, .5 * w, .5 * h).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("DoubleArrow", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, .3 * w, 0.214 * h));
        fig.Add(new PathSegment(SegmentType.Line, .3 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, 1.0 * w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, .3 * w, 1.0 * h));
        fig.Add(new PathSegment(SegmentType.Line, .3 * w, 0.786 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, 1.0 * h).Close());
        return geo;
      });

      FigureParameter.SetFigureParameter("DoubleEndArrow", 0, new FigureParameter("ConnecterHeight", .3, .01, .99));
      Shape.DefineFigureGenerator("DoubleEndArrow", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;  // height of midsection
        if (double.IsNaN(param1)) param1 = .3;

        var y1 = (.5 - param1 / 2) * h;
        var y2 = (.5 + param1 / 2) * h;

        var geo = new Geometry();
        var fig = new PathFigure(w, .5 * h, true);
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, y2));
        fig.Add(new PathSegment(SegmentType.Line, .3 * w, y2));
        fig.Add(new PathSegment(SegmentType.Line, .3 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, .3 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, .3 * w, y1));
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, y1));
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, 0).Close());
        var tempPoint = new Point();
        var temp = GetIntersection(0, .5,
          .3, 0,
          0, y1 / h,
          .1, y1 / h,
          out tempPoint);
        geo.Spot1 = new Spot(temp.X, temp.Y);
        temp = GetIntersection(.7, 1,
          1, .5,
          0, y2 / h,
          1, y2 / h,
          out temp);
        geo.Spot2 = new Spot(temp.X, temp.Y);
        return geo;
      });

      // DoubleEndArrow with absolutes instead of scaling
      FigureParameter.SetFigureParameter("DoubleEndArrow2", 0, new FigureParameter("ConnecterHeight", 40));
      FigureParameter.SetFigureParameter("DoubleEndArrow2", 1, new FigureParameter("ArrowHeight", 100));
      Shape.DefineFigureGenerator("DoubleEndArrow2", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;  // height of midsection
        if (double.IsNaN(param1)) param1 = 40;
        var param2 = (shape != null) ? shape.Parameter2 : double.NaN;  // height of arrows
        if (double.IsNaN(param2)) param2 = 100;

        /*
          y1outer
            /|     |\
           / |     | \
          /  y1----   \
         /             \
         \             /
          \  y2----   /
           \ |     | /
            \|     |/
          y2outer
        */
        var y1 = (h - param1) / 2;
        var y2 = y1 + param1;
        var y1outer = (h - param2) / 2;
        var y2outer = y1outer + param2;
        if (param1 > h || param2 > h) {
          if (param2 > param1) {
            param1 = param1 * h / param2;  // use similar ratio
            y1 = (h - param1) / 2;
            y2 = y1 + param1;
            y1outer = 0;
            y2outer = h;
          } else {
            y1 = 0;
            y2 = h;
            y1outer = 0;
            y2outer = h;
          }
        }
        var geo = new Geometry();
        var fig = new PathFigure(w, .5 * h, true);
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, y2outer));
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, y2));
        fig.Add(new PathSegment(SegmentType.Line, .3 * w, y2));
        fig.Add(new PathSegment(SegmentType.Line, .3 * w, y2outer));
        fig.Add(new PathSegment(SegmentType.Line, 0, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, .3 * w, y1outer));
        fig.Add(new PathSegment(SegmentType.Line, .3 * w, y1));
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, y1));
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, y1outer).Close());
        var tempPoint = new Point();
        var temp = GetIntersection(0, .5,
          .3, y1outer / h,
          0, y1 / h,
          1, y1 / h,
          out tempPoint);
        geo.Spot1 = new Spot(temp.X, temp.Y);
        temp = GetIntersection(.7, y2outer / h,
          1, .5,
          0, y2 / h,
          1, y2 / h,
          out temp);
        geo.Spot2 = new Spot(temp.X, temp.Y);

        return geo;
      });

      FigureParameter.SetFigureParameter("IBeamArrow", 0, new FigureParameter("ConnectorHeight", .3, .01, .99));
      Shape.DefineFigureGenerator("IBeamArrow", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;  // height of midsection
        if (double.IsNaN(param1)) param1 = .3;

        var y1 = (.5 - param1 / 2) * h;
        var y2 = (.5 + param1 / 2) * h;

        var geo = new Geometry();
        var fig = new PathFigure(w, .5 * h, true);
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, y2));
        fig.Add(new PathSegment(SegmentType.Line, .2 * w, y2));
        fig.Add(new PathSegment(SegmentType.Line, .2 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, 0));
        fig.Add(new PathSegment(SegmentType.Line, .2 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, .2 * w, y1));
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, y1));
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, 0).Close());
        geo.Spot1 = new Spot(0, y1 / h);
        var tempPoint = new Point();
        var temp = GetIntersection(.7, 1,
          1, .5,
          0, y2 / h,
          1, y2 / h,
          out tempPoint);
        geo.Spot2 = new Spot(temp.X, temp.Y);
        return geo;
      });

      // IBeamArrow with absolutes instead of scaling
      FigureParameter.SetFigureParameter("IBeamArrow2", 0, new FigureParameter("ConnectorHeight", 40));
      FigureParameter.SetFigureParameter("IBeamArrow2", 1, new FigureParameter("BeamArrowHeight", 100));
      Shape.DefineFigureGenerator("IBeamArrow2", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;  // height of midsection
        if (double.IsNaN(param1)) param1 = 40;
        var param2 = (shape != null) ? shape.Parameter2 : double.NaN;  // height of beam and arrow
        if (double.IsNaN(param2)) param2 = 100;

        var y1 = (h - param1) / 2;
        var y2 = y1 + param1;
        var y1outer = (h - param2) / 2;
        var y2outer = y1outer + param2;
        if (param1 > h || param2 > h) {
          if (param2 > param1) {
            param1 = param1 * h / param2;  // use similar ratio
            y1 = (h - param1) / 2;
            y2 = y1 + param1;
            y1outer = 0;
            y2outer = h;
          } else {
            y1 = 0;
            y2 = h;
            y1outer = 0;
            y2outer = h;
          }
        }
        var geo = new Geometry();
        var fig = new PathFigure(w, .5 * h, true);
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, y2outer));
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, y2));
        fig.Add(new PathSegment(SegmentType.Line, .2 * w, y2));
        fig.Add(new PathSegment(SegmentType.Line, .2 * w, y2outer));
        fig.Add(new PathSegment(SegmentType.Line, 0, y2outer));
        fig.Add(new PathSegment(SegmentType.Line, 0, y1outer));
        fig.Add(new PathSegment(SegmentType.Line, .2 * w, y1outer));
        fig.Add(new PathSegment(SegmentType.Line, .2 * w, y1));
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, y1));
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, y1outer).Close());
        geo.Spot1 = new Spot(0, y1 / h);
        var tempPoint = new Point();
        var temp = GetIntersection(.7, y2outer / h,
          1, .5,
          0, y2 / h,
          1, y2 / h,
          out tempPoint);
        geo.Spot2 = new Spot(temp.X, temp.Y);
        return geo;
      });

      FigureParameter.SetFigureParameter("Pointer", 0, new FigureParameter("BackPoint", .1, 0, .2));
      Shape.DefineFigureGenerator("Pointer", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;  // how much the back of the pointer comes in
        if (double.IsNaN(param1)) param1 = .1;

        var geo = new Geometry();
        var fig = new PathFigure(w, .5 * h, true);
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Line, param1 * w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, 0).Close());
        geo.Spot1 = new Spot(param1, .35);
        var tempPoint = new Point();
        var temp = GetIntersection(0, .65, 1, .65, 0, 1, 1, .5, out tempPoint);  // ?? constant
        geo.Spot2 = new Spot(temp.X, temp.Y);
        return geo;
      });

      FigureParameter.SetFigureParameter("RoundedPointer", 0, new FigureParameter("RoundedEdged", .3, 0, .5));
      Shape.DefineFigureGenerator("RoundedPointer", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;  // how much the curved back of the pointer comes in
        if (double.IsNaN(param1)) param1 = .3;

        var geo = new Geometry();
        var fig = new PathFigure(w, .5 * h, true);
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, 0, param1 * w, .75 * h,
            param1 * w, .25 * h).Close());
        geo.Spot1 = new Spot(param1, .35);
        var tempPoint = new Point();
        var temp = GetIntersection(0, .65, 1, .65, 0, 1, 1, .5, out tempPoint);  // ?? constant
        geo.Spot2 = new Spot(temp.X, temp.Y);
        return geo;
      });

      FigureParameter.SetFigureParameter("SplitEndArrow", 0, new FigureParameter("TailHeight", 0.4, 0.01, .99));
      Shape.DefineFigureGenerator("SplitEndArrow", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;  // % height of arrow tail
        if (double.IsNaN(param1)) param1 = .4;

        var y1 = (.5 - param1 / 2) * h;
        var y2 = (.5 + param1 / 2) * h;

        var geo = new Geometry();
        var fig = new PathFigure(w, .5 * h, true);
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, y2));
        fig.Add(new PathSegment(SegmentType.Line, 0, y2));
        fig.Add(new PathSegment(SegmentType.Line, .2 * w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, y1));
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, y1));
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, 0).Close());
        geo.Spot1 = new Spot(.2, y1 / h);
        var tempPoint = new Point();
        var temp = GetIntersection(.7, 1,
          1, .5,
          0, y2 / h,
          1, y2 / h,
          out tempPoint);
        geo.Spot2 = new Spot(temp.X, temp.Y);
        return geo;
      });

      // SplitEndArrow with absolutes instead of scaling
      FigureParameter.SetFigureParameter("SplitEndArrow2", 0, new FigureParameter("TailHeight", 40));
      Shape.DefineFigureGenerator("SplitEndArrow2", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;  // height of arrow tail
        if (double.IsNaN(param1)) param1 = 50;

        var y1 = (h - param1) / 2;
        var y2 = y1 + param1;
        if (param1 > h) {
          y1 = 0;
          y2 = h;
        }
        var geo = new Geometry();
        var fig = new PathFigure(w, .5 * h, true);
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, y2));
        fig.Add(new PathSegment(SegmentType.Line, 0, y2));
        fig.Add(new PathSegment(SegmentType.Line, .2 * w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, y1));
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, y1));
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, 0).Close());
        geo.Spot1 = new Spot(.2, y1 / h);
        var tempPoint = new Point();
        var temp = GetIntersection(.7, 1,
          1, .5,
          0, y2 / h,
          1, y2 / h,
          out tempPoint);
        geo.Spot2 = new Spot(temp.X, temp.Y);
        return geo;
      });

      FigureParameter.SetFigureParameter("SquareArrow", 0, new FigureParameter("ArrowPoint", .7, .2, .9));
      Shape.DefineFigureGenerator("SquareArrow", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;  // pointiness of arrow, lower is more pointy
        if (double.IsNaN(param1)) param1 = .7;

        var geo = new Geometry();
        var fig = new PathFigure(w, .5 * h, true);
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Line, param1 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, 0));
        fig.Add(new PathSegment(SegmentType.Line, param1 * w, 0).Close());
        geo.Spot1 = Spot.TopLeft;
        geo.Spot2 = new Spot(param1, 1);
        return geo;
      });

      Shape.DefineFigureGenerator("Cone1", (shape, w, h) => {
        var geo = new Geometry();
        var cpxOffset = KAPPA * .5;
        var cpyOffset = KAPPA * .1;
        var fig = new PathFigure(0, .9 * h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, .5 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, .9 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, .5 * w, h, w, (.9 + cpyOffset) * h,
          (.5 + cpxOffset) * w, h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, .9 * h, (.5 - cpxOffset) * w, h,
          0, (.9 + cpyOffset) * h).Close());
        geo.Spot1 = new Spot(.25, .5);
        geo.Spot2 = new Spot(.75, .97);
        return geo;
      });

      Shape.DefineFigureGenerator("Cone2", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, .9 * h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Bezier, w, .9 * h, (1 - .85 / .9) * w, h,
          (.85 / .9) * w, h));
        fig.Add(new PathSegment(SegmentType.Line, .5 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, 0, .9 * h).Close());
        var fig2 = new PathFigure(0, .9 * h, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Bezier, w, .9 * h, (1 - .85 / .9) * w, .8 * h,
          (.85 / .9) * w, .8 * h));
        geo.Spot1 = new Spot(.25, .5);
        geo.Spot2 = new Spot(.75, .82);
        return geo;
      });

      Shape.DefineFigureGenerator("Cube1", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(.5 * w, h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w, .85 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, .15 * h));
        fig.Add(new PathSegment(SegmentType.Line, .5 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, 0, .15 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, .85 * h).Close());
        var fig2 = new PathFigure(.5 * w, h, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, .5 * w, .3 * h));
        fig2.Add(new PathSegment(SegmentType.Line, 0, .15 * h));
        fig2.Add(new PathSegment(SegmentType.Move, .5 * w, .3 * h));
        fig2.Add(new PathSegment(SegmentType.Line, w, .15 * h));
        geo.Spot1 = new Spot(0, .3);
        geo.Spot2 = new Spot(.5, .85);
        return geo;
      });

      Shape.DefineFigureGenerator("Cube2", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, .3 * h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, w, .7 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, .3 * w, 0).Close());
        var fig2 = new PathFigure(0, .3 * h, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, .7 * w, .3 * h));
        fig2.Add(new PathSegment(SegmentType.Line, w, 0));
        fig2.Add(new PathSegment(SegmentType.Move, .7 * w, .3 * h));
        fig2.Add(new PathSegment(SegmentType.Line, .7 * w, h));
        geo.Spot1 = new Spot(0, .3);
        geo.Spot2 = new Spot(.7, 1);
        return geo;
      });

      Shape.DefineFigureGenerator("Cylinder1", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;  // half the height of the ellipse
        if (double.IsNaN(param1)) param1 = 5;  // default value
        param1 = Math.Min(param1, h / 3);

        var geo = new Geometry();
        var cpxOffset = KAPPA * .5;
        var fig = new PathFigure(0, param1, true);
        geo.Add(fig);
        // The base (top)
        fig.Add(new PathSegment(SegmentType.Bezier, .5 * w, 0, 0, KAPPA * param1,
          (.5 - cpxOffset) * w, 0));
        fig.Add(new PathSegment(SegmentType.Bezier, 1.0 * w, param1, (.5 + cpxOffset) * w, 0,
          1.0 * w, KAPPA * param1));
        fig.Add(new PathSegment(SegmentType.Line, w, h - param1));
        // Bottom curve
        fig.Add(new PathSegment(SegmentType.Bezier, .5 * w, 1.0 * h, 1.0 * w, h - KAPPA * param1,
          (.5 + cpxOffset) * w, 1.0 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, h - param1, (.5 - cpxOffset) * w, 1.0 * h,
          0, h - KAPPA * param1));
        fig.Add(new PathSegment(SegmentType.Line, 0, param1));

        var fig2 = new PathFigure(w, param1, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Bezier, .5 * w, 2 * param1, 1.0 * w, 2 * param1 - KAPPA * param1,
          (.5 + cpxOffset) * w, 2 * param1));
        fig2.Add(new PathSegment(SegmentType.Bezier, 0, param1, (.5 - cpxOffset) * w, 2 * param1,
          0, 2 * param1 - KAPPA * param1));

        geo.Spot1 = new Spot(0, 0, 0, 2 * param1);
        geo.Spot2 = new Spot(1, 1);
        return geo;
      });

      Shape.DefineFigureGenerator("Cylinder2", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;  // half the height of the ellipse
        if (double.IsNaN(param1)) param1 = 5;  // default value
        param1 = Math.Min(param1, h / 3);

        var geo = new Geometry();
        var cpxOffset = KAPPA * .5;
        var fig = new PathFigure(0, h - param1, true);
        geo.Add(fig);
        // The body, starting and ending bottom left
        fig.Add(new PathSegment(SegmentType.Line, 0, param1));
        fig.Add(new PathSegment(SegmentType.Bezier, .5 * w, 0, 0, KAPPA * param1,
          (.5 - cpxOffset) * w, 0));
        fig.Add(new PathSegment(SegmentType.Bezier, w, param1, (.5 + cpxOffset) * w, 0,
          w, KAPPA * param1));
        fig.Add(new PathSegment(SegmentType.Line, w, h - param1));
        fig.Add(new PathSegment(SegmentType.Bezier, .5 * w, h, w, h - KAPPA * param1,
          (.5 + cpxOffset) * w, h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, h - param1, (.5 - cpxOffset) * w, h,
          0, h - KAPPA * param1));

        var fig2 = new PathFigure(0, h - param1, false);
        geo.Add(fig2);
        // The base (bottom)
        fig2.Add(new PathSegment(SegmentType.Bezier, .5 * w, h - 2 * param1, 0, h - param1 - KAPPA * param1,
          (.5 - cpxOffset) * w, h - 2 * param1));
        fig2.Add(new PathSegment(SegmentType.Bezier, w, h - param1, (.5 + cpxOffset) * w, h - 2 * param1,
          w, h - param1 - KAPPA * param1));

        geo.Spot1 = new Spot(0, 0);
        geo.Spot2 = new Spot(1, 1, 0, -2 * param1);
        return geo;
      });

      Shape.DefineFigureGenerator("Cylinder3", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;  // half the width of the ellipse
        if (double.IsNaN(param1)) param1 = 5;  // default value
        param1 = Math.Min(param1, w / 3);

        var geo = new Geometry();
        var cpyOffset = KAPPA * .5;
        var fig = new PathFigure(param1, 0, true);
        geo.Add(fig);
        // The body, starting and ending top left
        fig.Add(new PathSegment(SegmentType.Line, w - param1, 0));
        fig.Add(new PathSegment(SegmentType.Bezier, w, .5 * h, w - KAPPA * param1, 0,
          w, (.5 - cpyOffset) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, w - param1, h, w, (.5 + cpyOffset) * h,
          w - KAPPA * param1, h));
        fig.Add(new PathSegment(SegmentType.Line, param1, h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, .5 * h, KAPPA * param1, h,
          0, (.5 + cpyOffset) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, param1, 0, 0, (.5 - cpyOffset) * h,
          KAPPA * param1, 0));

        var fig2 = new PathFigure(param1, 0, false);
        geo.Add(fig2);
        // Cylinder line (left)
        fig2.Add(new PathSegment(SegmentType.Bezier, 2 * param1, .5 * h, param1 + KAPPA * param1, 0,
          2 * param1, (.5 - cpyOffset) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, param1, h, 2 * param1, (.5 + cpyOffset) * h,
          param1 + KAPPA * param1, h));

        geo.Spot1 = new Spot(0, 0, 2 * param1, 0);
        geo.Spot2 = new Spot(1, 1);
        return geo;
      });

      Shape.DefineFigureGenerator("Cylinder4", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;  // half the width of the ellipse
        if (double.IsNaN(param1)) param1 = 5;  // default value
        param1 = Math.Min(param1, w / 3);

        var geo = new Geometry();
        var cpyOffset = KAPPA * .5;
        var fig = new PathFigure(w - param1, 0, true);
        geo.Add(fig);
        // The body, starting and ending top right
        fig.Add(new PathSegment(SegmentType.Bezier, w, .5 * h, w - KAPPA * param1, 0,
          w, (.5 - cpyOffset) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, w - param1, h, w, (.5 + cpyOffset) * h,
          w - KAPPA * param1, h));
        fig.Add(new PathSegment(SegmentType.Line, param1, h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, .5 * h, KAPPA * param1, h,
          0, (.5 + cpyOffset) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, param1, 0, 0, (.5 - cpyOffset) * h,
          KAPPA * param1, 0));
        fig.Add(new PathSegment(SegmentType.Line, w - param1, 0));

        var fig2 = new PathFigure(w - param1, 0, false);
        geo.Add(fig2);
        // Cylinder line (right)
        fig2.Add(new PathSegment(SegmentType.Bezier, w - 2 * param1, .5 * h, w - param1 - KAPPA * param1, 0,
          w - 2 * param1, (.5 - cpyOffset) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, w - param1, h, w - 2 * param1, (.5 + cpyOffset) * h,
          w - param1 - KAPPA * param1, h));

        geo.Spot1 = new Spot(0, 0);
        geo.Spot2 = new Spot(1, 1, -2 * param1, 0);
        return geo;
      });

      Shape.DefineFigureGenerator("Prism1", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(.25 * w, .25 * h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, .75 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, .5 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h).Close());
        var fig2 = new PathFigure(.25 * w, .25 * h, false);
        geo.Add(fig2);
        // Inner prism line
        fig2.Add(new PathSegment(SegmentType.Line, .5 * w, h));
        geo.Spot1 = new Spot(.408, .172);
        geo.Spot2 = new Spot(.833, .662);
        return geo;
      });

      Shape.DefineFigureGenerator("Prism2", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, .25 * h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, .75 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, .25 * h));
        fig.Add(new PathSegment(SegmentType.Line, .75 * w, .75 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h).Close());
        var fig2 = new PathFigure(0, h, false);
        geo.Add(fig2);
        // Inner prism lines
        fig2.Add(new PathSegment(SegmentType.Line, .25 * w, .5 * h));
        fig2.Add(new PathSegment(SegmentType.Line, w, .25 * h));
        fig2.Add(new PathSegment(SegmentType.Move, 0, .25 * h));
        fig2.Add(new PathSegment(SegmentType.Line, .25 * w, .5 * h));
        geo.Spot1 = new Spot(.25, .5);
        geo.Spot2 = new Spot(.75, .75);
        return geo;
      });

      Shape.DefineFigureGenerator("Pyramid1", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(.5 * w, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w, .75 * h));
        fig.Add(new PathSegment(SegmentType.Line, .5 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, .75 * h).Close());
        var fig2 = new PathFigure(.5 * w, 0, false);
        geo.Add(fig2);
        // Inner pyramind line
        fig2.Add(new PathSegment(SegmentType.Line, .5 * w, h));
        geo.Spot1 = new Spot(.25, .367);
        geo.Spot2 = new Spot(.75, .875);
        return geo;
      });

      Shape.DefineFigureGenerator("Pyramid2", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(.5 * w, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w, .85 * h));
        fig.Add(new PathSegment(SegmentType.Line, .5 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, .85 * h).Close());
        var fig2 = new PathFigure(.5 * w, 0, false);
        geo.Add(fig2);
        // Inner pyramid lines
        fig2.Add(new PathSegment(SegmentType.Line, .5 * w, .7 * h));
        fig2.Add(new PathSegment(SegmentType.Line, 0, .85 * h));
        fig2.Add(new PathSegment(SegmentType.Move, .5 * w, .7 * h));
        fig2.Add(new PathSegment(SegmentType.Line, w, .85 * h));
        geo.Spot1 = new Spot(.25, .367);
        geo.Spot2 = new Spot(.75, .875);
        return geo;
      });

      Shape.DefineFigureGenerator("Actor", (shape, w, h) => {
        var geo = new Geometry();
        var radiusw = .2;
        var radiush = .1;
        var offsetw = KAPPA * radiusw;
        var offseth = KAPPA * radiush;
        var centerx = .5;
        var centery = .1;
        var fig = new PathFigure(centerx * w, (centery + radiush) * h, true);
        geo.Add(fig);

        // Head
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx - radiusw) * w, centery * h, (centerx - offsetw) * w, (centery + radiush) * h,
          (centerx - radiusw) * w, (centery + offseth) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - radiush) * h, (centerx - radiusw) * w, (centery - offseth) * h,
          (centerx - offsetw) * w, (centery - radiush) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx + radiusw) * w, centery * h, (centerx + offsetw) * w, (centery - radiush) * h,
          (centerx + radiusw) * w, (centery - offseth) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radiush) * h, (centerx + radiusw) * w, (centery + offseth) * h,
          (centerx + offsetw) * w, (centery + radiush) * h));
        var r = .05;
        var cpOffset = KAPPA * r;
        centerx = .05;
        centery = .25;
        var fig2 = new PathFigure(.5 * w, .2 * h, true);
        geo.Add(fig2);
        // Body
        fig2.Add(new PathSegment(SegmentType.Line, .95 * w, .2 * h));
        centerx = .95;
        centery = .25;
        // Right arm
        fig2.Add(new PathSegment(SegmentType.Bezier, (centerx + r) * w, centery * h, (centerx + cpOffset) * w, (centery - r) * h,
          (centerx + r) * w, (centery - cpOffset) * h));
        fig2.Add(new PathSegment(SegmentType.Line, w, .6 * h));
        fig2.Add(new PathSegment(SegmentType.Line, .85 * w, .6 * h));
        fig2.Add(new PathSegment(SegmentType.Line, .85 * w, .35 * h));
        r = .025;
        cpOffset = KAPPA * r;
        centerx = .825;
        centery = .35;
        // Right under arm
        fig2.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - r) * h, (centerx + r) * w, (centery - cpOffset) * h,
          (centerx + cpOffset) * w, (centery - r) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, (centerx - r) * w, centery * h, (centerx - cpOffset) * w, (centery - r) * h,
          (centerx - r) * w, (centery - cpOffset) * h));
        // Right side/leg
        fig2.Add(new PathSegment(SegmentType.Line, .8 * w, h));
        fig2.Add(new PathSegment(SegmentType.Line, .55 * w, h));
        fig2.Add(new PathSegment(SegmentType.Line, .55 * w, .7 * h));
        // Right in between
        r = .05;
        cpOffset = KAPPA * r;
        centerx = .5;
        centery = .7;
        fig2.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - r) * h, (centerx + r) * w, (centery - cpOffset) * h,
          (centerx + cpOffset) * w, (centery - r) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, (centerx - r) * w, centery * h, (centerx - cpOffset) * w, (centery - r) * h,
          (centerx - r) * w, (centery - cpOffset) * h));
        // Left side/leg
        fig2.Add(new PathSegment(SegmentType.Line, .45 * w, h));
        fig2.Add(new PathSegment(SegmentType.Line, .2 * w, h));
        fig2.Add(new PathSegment(SegmentType.Line, .2 * w, .35 * h));
        r = .025;
        cpOffset = KAPPA * r;
        centerx = .175;
        centery = .35;
        // Left under arm
        fig2.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - r) * h, (centerx + r) * w, (centery - cpOffset) * h,
          (centerx + cpOffset) * w, (centery - r) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, (centerx - r) * w, centery * h, (centerx - cpOffset) * w, (centery - r) * h,
          (centerx - r) * w, (centery - cpOffset) * h));
        // Left arm
        fig2.Add(new PathSegment(SegmentType.Line, .15 * w, .6 * h));
        fig2.Add(new PathSegment(SegmentType.Line, 0, .6 * h));
        fig2.Add(new PathSegment(SegmentType.Line, 0, .25 * h));
        r = .05;
        cpOffset = KAPPA * r;
        centerx = .05;
        centery = .25;
        // Left shoulder
        fig2.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - r) * h, (centerx - r) * w, (centery - cpOffset) * h,
          (centerx - cpOffset) * w, (centery - r) * h));
        fig2.Add(new PathSegment(SegmentType.Line, .5 * w, .2 * h));
        geo.Spot1 = new Spot(.2, .2);
        geo.Spot2 = new Spot(.8, .65);
        return geo;
      });

      FigureParameter.SetFigureParameter("Card", 0, new FigureParameter("CornerCutoutSize", .2, .1, .9));
      Shape.DefineFigureGenerator("Card", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;  // size of corner cutout
        if (double.IsNaN(param1)) param1 = .2;

        var geo = new Geometry();
        var fig = new PathFigure(w, 0, true);
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, param1 * h));
        fig.Add(new PathSegment(SegmentType.Line, param1 * w, 0).Close());
        geo.Spot1 = new Spot(0, param1);
        geo.Spot2 = Spot.BottomRight;
        return geo;
      });

      Shape.DefineFigureGenerator("Collate", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(.5 * w, .5 * h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, .5 * w, .5 * h));
        var fig2 = new PathFigure(.5 * w, .5 * h, true);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, w, h));
        fig2.Add(new PathSegment(SegmentType.Line, 0, h));
        fig2.Add(new PathSegment(SegmentType.Line, .5 * w, .5 * h));
        geo.Spot1 = new Spot(.25, 0);
        geo.Spot2 = new Spot(.75, .25);
        return geo;
      });

      Shape.DefineFigureGenerator("CreateRequest", (shape, w, h) => {
        var geo = new Geometry();
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        if (double.IsNaN(param1)) param1 = .1;
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        // Body
        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h).Close());
        var fig2 = new PathFigure(0, param1 * h, false);
        geo.Add(fig2);
        // Inside lines
        fig2.Add(new PathSegment(SegmentType.Line, w, param1 * h));
        fig2.Add(new PathSegment(SegmentType.Move, 0, (1 - param1) * h));
        fig2.Add(new PathSegment(SegmentType.Line, w, (1 - param1) * h));
        // ??? geo.Spot1 = new Spot(0, param1);
        // ??? geo.Spot2 = new Spot(1, 1 - param1);
        return geo;
      });

      Shape.DefineFigureGenerator("Database", (shape, w, h) => {
        var geo = new Geometry();
        var cpxOffset = KAPPA * .5;
        var cpyOffset = KAPPA * .1;
        var fig = new PathFigure(w, .1 * h, true);
        geo.Add(fig);

        // Body
        fig.Add(new PathSegment(SegmentType.Line, w, .9 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, .5 * w, h, w, (.9 + cpyOffset) * h,
          (.5 + cpxOffset) * w, h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, .9 * h, (.5 - cpxOffset) * w, h,
          0, (.9 + cpyOffset) * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, .1 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, .5 * w, 0, 0, (.1 - cpyOffset) * h,
          (.5 - cpxOffset) * w, 0));
        fig.Add(new PathSegment(SegmentType.Bezier, w, .1 * h, (.5 + cpxOffset) * w, 0,
          w, (.1 - cpyOffset) * h));
        var fig2 = new PathFigure(w, .1 * h, false);
        geo.Add(fig2);
        // Rings
        fig2.Add(new PathSegment(SegmentType.Bezier, .5 * w, .2 * h, w, (.1 + cpyOffset) * h,
          (.5 + cpxOffset) * w, .2 * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, 0, .1 * h, (.5 - cpxOffset) * w, .2 * h,
          0, (.1 + cpyOffset) * h));
        fig2.Add(new PathSegment(SegmentType.Move, w, .2 * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, .5 * w, .3 * h, w, (.2 + cpyOffset) * h,
          (.5 + cpxOffset) * w, .3 * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, 0, .2 * h, (.5 - cpxOffset) * w, .3 * h,
          0, (.2 + cpyOffset) * h));
        fig2.Add(new PathSegment(SegmentType.Move, w, .3 * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, .5 * w, .4 * h, w, (.3 + cpyOffset) * h,
          (.5 + cpxOffset) * w, .4 * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, 0, .3 * h, (.5 - cpxOffset) * w, .4 * h,
          0, (.3 + cpyOffset) * h));
        geo.Spot1 = new Spot(0, .4);
        geo.Spot2 = new Spot(1, .9);
        return geo;
      });

      Shape.DefineFigureGenerator("DataStorage", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, .75 * w, 0));
        fig.Add(new PathSegment(SegmentType.Bezier, .75 * w, h, w, 0, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, 0, .25 * w, .9 * h, .25 * w, .1 * h).Close());
        geo.Spot1 = new Spot(.226, 0);
        geo.Spot2 = new Spot(.81, 1);
        return geo;
      });

      Shape.DefineFigureGenerator("DiskStorage", (shape, w, h) => {
        var geo = new Geometry();
        var cpxOffset = KAPPA * .5;
        var cpyOffset = KAPPA * .1;
        var fig = new PathFigure(w, .1 * h, true);
        geo.Add(fig);

        // Body
        fig.Add(new PathSegment(SegmentType.Line, w, .9 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, .5 * w, h, w, (.9 + cpyOffset) * h,
          (.5 + cpxOffset) * w, h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, .9 * h, (.5 - cpxOffset) * w, h,
          0, (.9 + cpyOffset) * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, .1 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, .5 * w, 0, 0, (.1 - cpyOffset) * h,
          (.5 - cpxOffset) * w, 0));
        fig.Add(new PathSegment(SegmentType.Bezier, w, .1 * h, (.5 + cpxOffset) * w, 0,
          w, (.1 - cpyOffset) * h));
        var fig2 = new PathFigure(w, .1 * h, false);
        geo.Add(fig2);
        // Rings
        fig2.Add(new PathSegment(SegmentType.Bezier, .5 * w, .2 * h, w, (.1 + cpyOffset) * h,
          (.5 + cpxOffset) * w, .2 * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, 0, .1 * h, (.5 - cpxOffset) * w, .2 * h,
          0, (.1 + cpyOffset) * h));
        fig2.Add(new PathSegment(SegmentType.Move, w, .2 * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, .5 * w, .3 * h, w, (.2 + cpyOffset) * h,
          (.5 + cpxOffset) * w, .3 * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, 0, .2 * h, (.5 - cpxOffset) * w, .3 * h,
          0, (.2 + cpyOffset) * h));
        geo.Spot1 = new Spot(0, .3);
        geo.Spot2 = new Spot(1, .9);
        return geo;
      });

      Shape.DefineFigureGenerator("Display", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(.25 * w, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, .75 * w, 0));
        fig.Add(new PathSegment(SegmentType.Bezier, .75 * w, h, w, 0, w, h));
        fig.Add(new PathSegment(SegmentType.Line, .25 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, .5 * h).Close());
        geo.Spot1 = new Spot(.25, 0);
        geo.Spot2 = new Spot(.75, 1);
        return geo;
      });

      Shape.DefineFigureGenerator("DividedEvent", (shape, w, h) => {
        var geo = new Geometry();
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        if (double.IsNaN(param1)) param1 = .2;
        else if (param1 < .15) param1 = .15;  // Minimum
        var cpOffset = KAPPA * .2;
        var fig = new PathFigure(0, .2 * h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Bezier, .2 * w, 0, 0, (.2 - cpOffset) * h,
          (.2 - cpOffset) * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, .8 * w, 0));
        fig.Add(new PathSegment(SegmentType.Bezier, w, .2 * h, (.8 + cpOffset) * w, 0,
          w, (.2 - cpOffset) * h));
        fig.Add(new PathSegment(SegmentType.Line, w, .8 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, .8 * w, h, w, (.8 + cpOffset) * h,
          (.8 + cpOffset) * w, h));
        fig.Add(new PathSegment(SegmentType.Line, .2 * w, h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, .8 * h, (.2 - cpOffset) * w, h,
          0, (.8 + cpOffset) * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, .2 * h));
        var fig2 = new PathFigure(0, param1 * h, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, w, param1 * h));
        // ??? geo.Spot1 = new Spot(0, param1);
        // ??? geo.Spot2 = new Spot(1, 1 - param1);
        return geo;
      });

      Shape.DefineFigureGenerator("DividedProcess", (shape, w, h) => {
        var geo = new Geometry();
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        if (double.IsNaN(param1) || param1 < .1) param1 = .1;  // Minimum
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h).Close());
        var fig2 = new PathFigure(0, param1 * h, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, w, param1 * h));
        // ??? geo.Spot1 = new Spot(0, param1);
        // ??? geo.Spot2 = Spot.BottomRight;
        return geo;
      });

      Shape.DefineFigureGenerator("Document", (shape, w, h) => {
        var geo = new Geometry();
        h = h / .8;
        var fig = new PathFigure(0, .7 * h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, .7 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, .7 * h, .5 * w, .4 * h, .5 * w, h).Close());
        geo.Spot1 = Spot.TopLeft;
        geo.Spot2 = new Spot(1, .6);
        return geo;
      });

      Shape.DefineFigureGenerator("ExternalOrganization", (shape, w, h) => {
        var geo = new Geometry();
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        if (double.IsNaN(param1) || param1 < .2) param1 = .2;  // Minimum
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        // Body
        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h).Close());
        var fig2 = new PathFigure(param1 * w, 0, false);
        geo.Add(fig2);
        // Top left triangle
        fig2.Add(new PathSegment(SegmentType.Line, 0, param1 * h));
        // Top right triangle
        fig2.Add(new PathSegment(SegmentType.Move, w, param1 * h));
        fig2.Add(new PathSegment(SegmentType.Line, (1 - param1) * w, 0));
        // Bottom left triangle
        fig2.Add(new PathSegment(SegmentType.Move, 0, (1 - param1) * h));
        fig2.Add(new PathSegment(SegmentType.Line, param1 * w, h));
        // Bottom right triangle
        fig2.Add(new PathSegment(SegmentType.Move, (1 - param1) * w, h));
        fig2.Add(new PathSegment(SegmentType.Line, w, (1 - param1) * h));
        // ??? geo.Spot1 = new Spot(param1 / 2, param1 / 2);
        // ??? geo.Spot2 = new Spot(1 - param1 / 2, 1 - param1 / 2);
        return geo;
      });

      Shape.DefineFigureGenerator("ExternalProcess", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(.5 * w, 0, true);
        geo.Add(fig);

        // Body
        fig.Add(new PathSegment(SegmentType.Line, w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, .5 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, .5 * h).Close());
        var fig2 = new PathFigure(.1 * w, .4 * h, false);
        geo.Add(fig2);
        // Top left triangle
        fig2.Add(new PathSegment(SegmentType.Line, .1 * w, .6 * h));
        // Top right triangle
        fig2.Add(new PathSegment(SegmentType.Move, .9 * w, .6 * h));
        fig2.Add(new PathSegment(SegmentType.Line, .9 * w, .4 * h));
        // Bottom left triangle
        fig2.Add(new PathSegment(SegmentType.Move, .6 * w, .1 * h));
        fig2.Add(new PathSegment(SegmentType.Line, .4 * w, .1 * h));
        // Bottom right triangle
        fig2.Add(new PathSegment(SegmentType.Move, .4 * w, .9 * h));
        fig2.Add(new PathSegment(SegmentType.Line, .6 * w, .9 * h));
        geo.Spot1 = new Spot(.25, .25);
        geo.Spot2 = new Spot(.75, .75);
        return geo;
      });

      Shape.DefineFigureGenerator("File", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0, true);  // starting point
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Line, .75 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, .25 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h).Close());
        var fig2 = new PathFigure(.75 * w, 0, false);
        geo.Add(fig2);
        // The Fold
        fig2.Add(new PathSegment(SegmentType.Line, .75 * w, .25 * h));
        fig2.Add(new PathSegment(SegmentType.Line, w, .25 * h));
        geo.Spot1 = new Spot(0, .25);
        geo.Spot2 = Spot.BottomRight;
        return geo;
      });

      Shape.DefineFigureGenerator("Interrupt", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(w, .5 * h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, .5 * h));
        var fig2 = new PathFigure(w, .5 * h, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, w, h));
        var fig3 = new PathFigure(w, .5 * h, false);
        geo.Add(fig3);
        fig3.Add(new PathSegment(SegmentType.Line, w, 0));
        geo.Spot1 = new Spot(0, .25);
        geo.Spot2 = new Spot(.5, .75);
        return geo;
      });

      Shape.DefineFigureGenerator("InternalStorage", (shape, w, h) => {
        var geo = new Geometry();
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        var param2 = (shape != null) ? shape.Parameter2 : double.NaN;
        if (double.IsNaN(param1)) param1 = .1;  // Distance from left
        if (double.IsNaN(param2)) param2 = .1;  // Distance from top
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        // The main body
        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h).Close());
        var fig2 = new PathFigure(param1 * w, 0, false);
        geo.Add(fig2);
        // Two lines
        fig2.Add(new PathSegment(SegmentType.Line, param1 * w, h));
        fig2.Add(new PathSegment(SegmentType.Move, 0, param2 * h));
        fig2.Add(new PathSegment(SegmentType.Line, w, param2 * h));
        // ??? geo.Spot1 = new Spot(param1, param2);
        // ??? geo.Spot2 = Spot.BottomRight;
        return geo;
      });

      Shape.DefineFigureGenerator("Junction", (shape, w, h) => {
        var geo = new Geometry();
        var dist = (1 / Math.Sqrt(2));
        var small = ((1 - 1 / Math.Sqrt(2)) / 2);
        var cpOffset = KAPPA * .5;
        var radius = .5;
        var fig = new PathFigure(w, radius * h, true);
        geo.Add(fig);

        // Circle
        fig.Add(new PathSegment(SegmentType.Bezier, radius * w, h, w, (radius + cpOffset) * h,
          (radius + cpOffset) * w, h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, radius * h, (radius - cpOffset) * w, h,
          0, (radius + cpOffset) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, radius * w, 0, 0, (radius - cpOffset) * h,
          (radius - cpOffset) * w, 0));
        fig.Add(new PathSegment(SegmentType.Bezier, w, radius * h, (radius + cpOffset) * w, 0,
          w, (radius - cpOffset) * h));
        var fig2 = new PathFigure((small + dist) * w, (small + dist) * h, false);
        geo.Add(fig2);
        // X
        fig2.Add(new PathSegment(SegmentType.Line, small * w, small * h));
        fig2.Add(new PathSegment(SegmentType.Move, small * w, (small + dist) * h));
        fig2.Add(new PathSegment(SegmentType.Line, (small + dist) * w, small * h));
        return geo;
      });

      Shape.DefineFigureGenerator("LinedDocument", (shape, w, h) => {
        var geo = new Geometry();
        h = h / .8;
        var fig = new PathFigure(0, .7 * h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, .7 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, .7 * h, .5 * w, .4 * h, .5 * w, h).Close());
        var fig2 = new PathFigure(.1 * w, 0, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, .1 * w, .75 * h));
        geo.Spot1 = new Spot(.1, 0);
        geo.Spot2 = new Spot(1, .6);
        return geo;
      });

      Shape.DefineFigureGenerator("LoopLimit", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0, .25 * h));
        fig.Add(new PathSegment(SegmentType.Line, .25 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, .75 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, .25 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, h).Close());
        geo.Spot1 = new Spot(0, .25);
        geo.Spot2 = Spot.BottomRight;
        return geo;
      });

      Shape.DefineFigureGenerator("MagneticTape", (shape, w, h) => {
        var geo = new Geometry();
        var cpOffset = KAPPA * .5;
        var radius = .5;
        var fig = new PathFigure(.5 * w, h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Bezier, 0, radius * h, (radius - cpOffset) * w, h,
          0, (radius + cpOffset) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, radius * w, 0, 0, (radius - cpOffset) * h,
          (radius - cpOffset) * w, 0));
        fig.Add(new PathSegment(SegmentType.Bezier, w, radius * h, (radius + cpOffset) * w, 0,
          w, (radius - cpOffset) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (radius + .1) * w, .9 * h, w, (radius + cpOffset) * h,
          (radius + cpOffset) * w, .9 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, .9 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, .5 * w, h));
        geo.Spot1 = new Spot(.15, .15);
        geo.Spot2 = new Spot(.85, .8);
        return geo;
      });

      Shape.DefineFigureGenerator("ManualInput", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(w, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, .25 * h).Close());
        geo.Spot1 = new Spot(0, .25);
        geo.Spot2 = Spot.BottomRight;
        return geo;
      });

      Shape.DefineFigureGenerator("MessageFromUser", (shape, w, h) => {
        var geo = new Geometry();
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        if (double.IsNaN(param1)) param1 = .7;  // How far from the right the point is
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, param1 * w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h).Close());
        geo.Spot1 = Spot.TopLeft;
        // ??? geo.Spot2 = new Spot(param1, 1);
        return geo;
      });

      Shape.DefineFigureGenerator("MicroformProcessing", (shape, w, h) => {
        var geo = new Geometry();
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        if (double.IsNaN(param1)) param1 = .25;  // How far from the top/bottom the points are
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, .5 * w, param1 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, .5 * w, (1 - param1) * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h).Close());
        // ??? geo.Spot1 = new Spot(0, param1);
        // ??? geo.Spot2 = new Spot(1, 1 - param1);
        return geo;
      });

      Shape.DefineFigureGenerator("MicroformRecording", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, .75 * w, .25 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, .15 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, .85 * h));
        fig.Add(new PathSegment(SegmentType.Line, .75 * w, .75 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h).Close());
        geo.Spot1 = new Spot(0, .25);
        geo.Spot2 = new Spot(1, .75);
        return geo;
      });

      Shape.DefineFigureGenerator("MultiDocument", (shape, w, h) => {
        var geo = new Geometry();
        h = h / .8;
        var fig = new PathFigure(w, 0, true);
        geo.Add(fig);

        // Outline
        fig.Add(new PathSegment(SegmentType.Line, w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, .9 * w, .44 * h, .96 * w, .47 * h, .93 * w, .45 * h));
        fig.Add(new PathSegment(SegmentType.Line, .9 * w, .6 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, .8 * w, .54 * h, .86 * w, .57 * h, .83 * w, .55 * h));
        fig.Add(new PathSegment(SegmentType.Line, .8 * w, .7 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, .7 * h, .4 * w, .4 * h, .4 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, .2 * h));
        fig.Add(new PathSegment(SegmentType.Line, .1 * w, .2 * h));
        fig.Add(new PathSegment(SegmentType.Line, .1 * w, .1 * h));
        fig.Add(new PathSegment(SegmentType.Line, .2 * w, .1 * h));
        fig.Add(new PathSegment(SegmentType.Line, .2 * w, 0).Close());
        var fig2 = new PathFigure(.1 * w, .2 * h, false);
        geo.Add(fig2);
        // Inside lines
        fig2.Add(new PathSegment(SegmentType.Line, .8 * w, .2 * h));
        fig2.Add(new PathSegment(SegmentType.Line, .8 * w, .54 * h));
        fig2.Add(new PathSegment(SegmentType.Move, .2 * w, .1 * h));
        fig2.Add(new PathSegment(SegmentType.Line, .9 * w, .1 * h));
        fig2.Add(new PathSegment(SegmentType.Line, .9 * w, .44 * h));
        geo.Spot1 = new Spot(0, .25);
        geo.Spot2 = new Spot(.8, .77);
        return geo;
      });

      Shape.DefineFigureGenerator("MultiProcess", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(.1 * w, .1 * h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, .2 * w, .1 * h));
        fig.Add(new PathSegment(SegmentType.Line, .2 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, .8 * h));
        fig.Add(new PathSegment(SegmentType.Line, .9 * w, .8 * h));
        fig.Add(new PathSegment(SegmentType.Line, .9 * w, .9 * h));
        fig.Add(new PathSegment(SegmentType.Line, .8 * w, .9 * h));
        fig.Add(new PathSegment(SegmentType.Line, .8 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, .2 * h));
        fig.Add(new PathSegment(SegmentType.Line, .1 * w, .2 * h).Close());
        var fig2 = new PathFigure(.2 * w, .1 * h, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, .9 * w, .1 * h));
        fig2.Add(new PathSegment(SegmentType.Line, .9 * w, .8 * h));
        fig2.Add(new PathSegment(SegmentType.Move, .1 * w, .2 * h));
        fig2.Add(new PathSegment(SegmentType.Line, .8 * w, .2 * h));
        fig2.Add(new PathSegment(SegmentType.Line, .8 * w, .9 * h));
        geo.Spot1 = new Spot(0, .2);
        geo.Spot2 = new Spot(.8, 1);
        return geo;
      });

      Shape.DefineFigureGenerator("OfflineStorage", (shape, w, h) => {
        var geo = new Geometry();
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        if (double.IsNaN(param1)) param1 = .1;  // Distance between 2 top lines
        var l = 1 - param1;  // Length of the top line
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, .5 * w, h).Close());
        var fig2 = new PathFigure(.5 * param1 * w, param1 * h, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, (1 - .5 * param1) * w, param1 * h));
        // ??? geo.Spot1 = new Spot(l / 4 + .5 * param1, param1);
        // ??? geo.Spot2 = new Spot(3 * l / 4 + .5 * param1, param1 + .5 * l);
        return geo;
      });

      Shape.DefineFigureGenerator("OffPageConnector", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, .75 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, .75 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h).Close());
        geo.Spot1 = Spot.TopLeft;
        geo.Spot2 = new Spot(.75, 1);
        return geo;
      });

      Shape.DefineFigureGenerator("Or", (shape, w, h) => {
        var geo = new Geometry();
        var cpOffset = KAPPA * .5;
        var radius = .5;
        var fig = new PathFigure(w, radius * h, true);
        geo.Add(fig);

        // Circle
        fig.Add(new PathSegment(SegmentType.Bezier, radius * w, h, w, (radius + cpOffset) * h,
          (radius + cpOffset) * w, h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, radius * h, (radius - cpOffset) * w, h,
          0, (radius + cpOffset) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, radius * w, 0, 0, (radius - cpOffset) * h,
          (radius - cpOffset) * w, 0));
        fig.Add(new PathSegment(SegmentType.Bezier, w, radius * h, (radius + cpOffset) * w, 0,
          w, (radius - cpOffset) * h));
        var fig2 = new PathFigure(w, .5 * h, false);
        geo.Add(fig2);
        // +
        fig2.Add(new PathSegment(SegmentType.Line, 0, .5 * h));
        fig2.Add(new PathSegment(SegmentType.Move, .5 * w, h));
        fig2.Add(new PathSegment(SegmentType.Line, .5 * w, 0));
        return geo;
      });

      Shape.DefineFigureGenerator("PaperTape", (shape, w, h) => {
        var geo = new Geometry();
        h = h / .8;
        var fig = new PathFigure(0, .7 * h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0, .3 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, w, .3 * h, .5 * w, .6 * h,
          .5 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, .7 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, .7 * h, .5 * w, .4 * h,
          .5 * w, h).Close());
        geo.Spot1 = new Spot(0, .49);
        geo.Spot2 = new Spot(1, .75);
        return geo;
      });

      Shape.DefineFigureGenerator("PrimitiveFromCall", (shape, w, h) => {
        var geo = new Geometry();
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        var param2 = (shape != null) ? shape.Parameter2 : double.NaN;
        if (double.IsNaN(param1)) param1 = .1;  // Distance of left line from left
        if (double.IsNaN(param2)) param2 = .3;  // Distance of point from right
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, (1 - param2) * w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h).Close());
        // ??? geo.Spot1 = new Spot(param1, 0);
        // ??? geo.Spot2 = new Spot(1 - param2, 1);
        return geo;
      });

      Shape.DefineFigureGenerator("PrimitiveToCall", (shape, w, h) => {
        var geo = new Geometry();
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        var param2 = (shape != null) ? shape.Parameter2 : double.NaN;
        if (double.IsNaN(param1)) param1 = .1;  // Distance of left line from left
        if (double.IsNaN(param2)) param2 = .3;  // Distance of top and bottom right corners from right
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, (1 - param2) * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, (1 - param2) * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h).Close());
        // ??? geo.Spot1 = new Spot(param1, 0);
        // ??? geo.Spot2 = new Spot(1 - param2, 1);
        return geo;
      });

      Shape.DefineFigureGenerator("Procedure", (shape, w, h) => {
        var geo = new Geometry();
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        // Distance of left  and right lines from edge
        if (double.IsNaN(param1)) param1 = .1;
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h).Close());
        var fig2 = new PathFigure((1 - param1) * w, 0, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, (1 - param1) * w, h));
        fig2.Add(new PathSegment(SegmentType.Move, param1 * w, 0));
        fig2.Add(new PathSegment(SegmentType.Line, param1 * w, h));
        // ??? geo.Spot1 = new Spot(param1, 0);
        // ??? geo.Spot2 = new Spot(1 - param1, 1);
        return geo;
      });

      Shape.DefineFigureGenerator("Process", (shape, w, h) => {
        var geo = new Geometry();
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        if (double.IsNaN(param1)) param1 = .1;  // Distance of left  line from left edge
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h).Close());
        var fig2 = new PathFigure(param1 * w, 0, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, param1 * w, h));
        // ??? geo.Spot1 = new Spot(param1, 0);
        geo.Spot2 = Spot.BottomRight;
        return geo;
      });

      Shape.DefineFigureGenerator("Sort", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(.5 * w, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, .5 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, .5 * h).Close());
        var fig2 = new PathFigure(0, .5 * h, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, w, .5 * h));
        geo.Spot1 = new Spot(.25, .25);
        geo.Spot2 = new Spot(.75, .5);
        return geo;
      });

      Shape.DefineFigureGenerator("Start", (shape, w, h) => {
        var geo = new Geometry();
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        if (double.IsNaN(param1)) param1 = 0.25;
        var fig = new PathFigure(param1 * w, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Arc, 270, 180, .75 * w, 0.5 * h, .25 * w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Arc, 90, 180, .25 * w, 0.5 * h, .25 * w, .5 * h));
        var fig2 = new PathFigure(param1 * w, 0, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, param1 * w, h));
        fig2.Add(new PathSegment(SegmentType.Move, (1 - param1) * w, 0));
        fig2.Add(new PathSegment(SegmentType.Line, (1 - param1) * w, h));
        geo.Spot1 = new Spot(param1, 0);
        geo.Spot2 = new Spot((1 - param1), 1);
        return geo;
      });

      Shape.DefineFigureGenerator("Terminator", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(.25 * w, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Arc, 270, 180, .75 * w, 0.5 * h, .25 * w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Arc, 90, 180, .25 * w, 0.5 * h, .25 * w, .5 * h));
        geo.Spot1 = new Spot(.23, 0);
        geo.Spot2 = new Spot(.77, 1);
        return geo;
      });

      Shape.DefineFigureGenerator("TransmittalTape", (shape, w, h) => {
        var geo = new Geometry();
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        if (double.IsNaN(param1)) param1 = .1;  // Bottom line's distance from the point on the triangle
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, .75 * w, (1 - param1) * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, (1 - param1) * h).Close());
        geo.Spot1 = Spot.TopLeft;
        // ??? geo.Spot2 = new Spot(1, 1 - param1);
        return geo;
      });

      Shape.DefineFigureGenerator("AndGate", (shape, w, h) => {
        var geo = new Geometry();
        var cpOffset = KAPPA * .5;
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        // The gate body
        fig.Add(new PathSegment(SegmentType.Line, .5 * w, 0));
        fig.Add(new PathSegment(SegmentType.Bezier, w, .5 * h, (.5 + cpOffset) * w, 0,
          w, (.5 - cpOffset) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, .5 * w, h, w, (.5 + cpOffset) * h,
          (.5 + cpOffset) * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h).Close());
        geo.Spot1 = Spot.TopLeft;
        geo.Spot2 = new Spot(.55, 1);
        return geo;
      });

      Shape.DefineFigureGenerator("Buffer", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h).Close());
        geo.Spot1 = new Spot(0, .25);
        geo.Spot2 = new Spot(.5, .75);
        return geo;
      });

      Shape.DefineFigureGenerator("Clock", (shape, w, h) => {
        var geo = new Geometry();
        var cpOffset = KAPPA * .5;
        var radius = .5;
        var fig = new PathFigure(w, radius * h, true);
        geo.Add(fig);

        // Ellipse
        fig.Add(new PathSegment(SegmentType.Bezier, radius * w, h, w, (radius + cpOffset) * h,
          (radius + cpOffset) * w, h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, radius * h, (radius - cpOffset) * w, h,
          0, (radius + cpOffset) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, radius * w, 0, 0, (radius - cpOffset) * h,
          (radius - cpOffset) * w, 0));
        fig.Add(new PathSegment(SegmentType.Bezier, w, radius * h, (radius + cpOffset) * w, 0,
          w, (radius - cpOffset) * h));
        var fig2 = new PathFigure(w, radius * h, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, w, radius * h));
        var fig3 = new PathFigure(.8 * w, .75 * h, false);
        geo.Add(fig3);
        // Inside clock
        // This first line solves a GDI+ graphical error with
        // more complex gradient brushes
        fig3.Add(new PathSegment(SegmentType.Line, .8 * w, .25 * h));
        fig3.Add(new PathSegment(SegmentType.Line, .6 * w, .25 * h));
        fig3.Add(new PathSegment(SegmentType.Line, .6 * w, .75 * h));
        fig3.Add(new PathSegment(SegmentType.Line, .4 * w, .75 * h));
        fig3.Add(new PathSegment(SegmentType.Line, .4 * w, .25 * h));
        fig3.Add(new PathSegment(SegmentType.Line, .2 * w, .25 * h));
        fig3.Add(new PathSegment(SegmentType.Line, .2 * w, .75 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("Ground", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(.5 * w, 0, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, .5 * w, .4 * h));
        fig.Add(new PathSegment(SegmentType.Move, .2 * w, .6 * h));
        fig.Add(new PathSegment(SegmentType.Line, .8 * w, .6 * h));
        fig.Add(new PathSegment(SegmentType.Move, .3 * w, .8 * h));
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, .8 * h));
        fig.Add(new PathSegment(SegmentType.Move, .4 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, .6 * w, h));
        return geo;
      });

      Shape.DefineFigureGenerator("Inverter", (shape, w, h) => {
        var geo = new Geometry();
        var cpOffset = KAPPA * .1;
        var radius = .1;
        var centerx = .9;
        var centery = .5;
        var fig = new PathFigure(.8 * w, .5 * h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, 0));
        fig.Add(new PathSegment(SegmentType.Line, .8 * w, .5 * h));
        var fig2 = new PathFigure((centerx + radius) * w, centery * h, true);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radius) * h, (centerx + radius) * w, (centery + cpOffset) * h,
          (centerx + cpOffset) * w, (centery + radius) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w, centery * h, (centerx - cpOffset) * w, (centery + radius) * h,
          (centerx - radius) * w, (centery + cpOffset) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - radius) * h, (centerx - radius) * w, (centery - cpOffset) * h,
          (centerx - cpOffset) * w, (centery - radius) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, centery * h, (centerx + cpOffset) * w, (centery - radius) * h,
          (centerx + radius) * w, (centery - cpOffset) * h));
        geo.Spot1 = new Spot(0, .25);
        geo.Spot2 = new Spot(.4, .75);
        return geo;
      });

      Shape.DefineFigureGenerator("NandGate", (shape, w, h) => {
        var geo = new Geometry();
        var cpxOffset = KAPPA * .5;
        var cpyOffset = KAPPA * .4;
        var cpOffset = KAPPA * .1;
        var radius = .1;
        var centerx = .9;
        var centery = .5;
        var fig = new PathFigure(.8 * w, .5 * h, true);
        geo.Add(fig);

        // The gate body
        fig.Add(new PathSegment(SegmentType.Bezier, .4 * w, h, .8 * w, (.5 + cpyOffset) * h,
          (.4 + cpxOffset) * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, 0));
        fig.Add(new PathSegment(SegmentType.Line, .4 * w, 0));
        fig.Add(new PathSegment(SegmentType.Bezier, .8 * w, .5 * h, (.4 + cpxOffset) * w, 0,
          .8 * w, (.5 - cpyOffset) * h));
        var fig2 = new PathFigure((centerx + radius) * w, centery * h, true);
        geo.Add(fig2);
        // Inversion
        fig2.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radius) * h, (centerx + radius) * w, (centery + cpOffset) * h,
          (centerx + cpOffset) * w, (centery + radius) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w, centery * h, (centerx - cpOffset) * w, (centery + radius) * h,
          (centerx - radius) * w, (centery + cpOffset) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - radius) * h, (centerx - radius) * w, (centery - cpOffset) * h,
          (centerx - cpOffset) * w, (centery - radius) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, (centery) * h, (centerx + cpOffset) * w, (centery - radius) * h,
          (centerx + radius) * w, (centery - cpOffset) * h));
        geo.Spot1 = new Spot(0, .05);
        geo.Spot2 = new Spot(.55, .95);
        return geo;
      });

      Shape.DefineFigureGenerator("NorGate", (shape, w, h) => {
        var geo = new Geometry();
        var radius = .5;
        var cpOffset = KAPPA * radius;
        var centerx = 0d;
        var centery = .5;
        var fig = new PathFigure(.8 * w, .5 * h, true);
        geo.Add(fig);

        // Normal
        fig.Add(new PathSegment(SegmentType.Bezier, 0, h, .7 * w, (centery + cpOffset) * h,
          (centerx + cpOffset) * w, (centery + radius) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, 0, .25 * w, .75 * h,
          .25 * w, .25 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, .8 * w, .5 * h, (centerx + cpOffset) * w, (centery - radius) * h,
          .7 * w, (centery - cpOffset) * h));
        radius = .1;
        cpOffset = KAPPA * .1;
        centerx = .9;
        centery = .5;
        var fig2 = new PathFigure((centerx - radius) * w, centery * h, true);
        geo.Add(fig2);
        // Inversion
        fig2.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - radius) * h, (centerx - radius) * w, (centery - cpOffset) * h,
          (centerx - cpOffset) * w, (centery - radius) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, centery * h, (centerx + cpOffset) * w, (centery - radius) * h,
          (centerx + radius) * w, (centery - cpOffset) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radius) * h, (centerx + radius) * w, (centery + cpOffset) * h,
          (centerx + cpOffset) * w, (centery + radius) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w, centery * h, (centerx - cpOffset) * w, (centery + radius) * h,
          (centerx - radius) * w, (centery + cpOffset) * h));
        geo.Spot1 = new Spot(.2, .25);
        geo.Spot2 = new Spot(.6, .75);
        return geo;
      });

      Shape.DefineFigureGenerator("OrGate", (shape, w, h) => {
        var geo = new Geometry();
        var radius = .5;
        var cpOffset = KAPPA * radius;
        var centerx = 0;
        var centery = .5;
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Bezier, w, .5 * h, (centerx + cpOffset + cpOffset) * w, (centery - radius) * h,
          .8 * w, (centery - cpOffset) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, h, .8 * w, (centery + cpOffset) * h,
          (centerx + cpOffset + cpOffset) * w, (centery + radius) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, 0, .25 * w, .75 * h, .25 * w, .25 * h).Close());
        geo.Spot1 = new Spot(.2, .25);
        geo.Spot2 = new Spot(.75, .75);
        return geo;
      });

      Shape.DefineFigureGenerator("XnorGate", (shape, w, h) => {
        var geo = new Geometry();
        var radius = .5;
        var cpOffset = KAPPA * radius;
        var centerx = .2;
        var centery = .5;
        var fig = new PathFigure(.1 * w, 0, false);
        geo.Add(fig);

        // Normal
        fig.Add(new PathSegment(SegmentType.Bezier, .1 * w, h, .35 * w, .25 * h, .35 * w, .75 * h));
        var fig2 = new PathFigure(.8 * w, .5 * h, true);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Bezier, .2 * w, h, .7 * w, (centery + cpOffset) * h,
          (centerx + cpOffset) * w, (centery + radius) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, .2 * w, 0, .45 * w, .75 * h, .45 * w, .25 * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, .8 * w, .5 * h, (centerx + cpOffset) * w, (centery - radius) * h,
          .7 * w, (centery - cpOffset) * h));
        radius = .1;
        cpOffset = KAPPA * .1;
        centerx = .9;
        centery = .5;
        var fig3 = new PathFigure((centerx - radius) * w, centery * h, true);
        geo.Add(fig3);
        // Inversion
        fig3.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - radius) * h, (centerx - radius) * w, (centery - cpOffset) * h,
          (centerx - cpOffset) * w, (centery - radius) * h));
        fig3.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, centery * h, (centerx + cpOffset) * w, (centery - radius) * h,
          (centerx + radius) * w, (centery - cpOffset) * h));
        fig3.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radius) * h, (centerx + radius) * w, (centery + cpOffset) * h,
          (centerx + cpOffset) * w, (centery + radius) * h));
        fig3.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w, centery * h, (centerx - cpOffset) * w, (centery + radius) * h,
          (centerx - radius) * w, (centery + cpOffset) * h));
        geo.Spot1 = new Spot(.4, .25);
        geo.Spot2 = new Spot(.65, .75);
        return geo;
      });

      Shape.DefineFigureGenerator("XorGate", (shape, w, h) => {
        var geo = new Geometry();
        var radius = .5;
        var cpOffset = KAPPA * radius;
        var centerx = .2;
        var centery = .5;
        var fig = new PathFigure(.1 * w, 0, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Bezier, .1 * w, h, .35 * w, .25 * h, .35 * w, .75 * h));
        var fig2 = new PathFigure(.2 * w, 0, true);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Bezier, w, .5 * h, (centerx + cpOffset) * w, (centery - radius) * h,
          .9 * w, (centery - cpOffset) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, .2 * w, h, .9 * w, (centery + cpOffset) * h,
          (centerx + cpOffset) * w, (centery + radius) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, .2 * w, 0, .45 * w, .75 * h, .45 * w, .25 * h).Close());
        geo.Spot1 = new Spot(.4, .25);
        geo.Spot2 = new Spot(.8, .75);
        return geo;
      });

      Shape.DefineFigureGenerator("Capacitor", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0, false);
        geo.Add(fig);

        // Two vertical lines
        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Move, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        return geo;
      });

      Shape.DefineFigureGenerator("Resistor", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, .5 * h, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, .1 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, .2 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, .3 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, .4 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, .5 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, .6 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, .5 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("Inductor", (shape, w, h) => {
        var geo = new Geometry();
        var cpOffset = KAPPA * .1;
        var radius = .1;
        var centerx = .1;
        // Up
        var fig = new PathFigure((centerx - cpOffset * .5) * w, h, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, 0, (centerx - cpOffset) * w, h, (centerx - radius) * w, 0));
        // Down up
        centerx = .3;
        fig.Add(new PathSegment(SegmentType.Bezier, centerx * w, h, (centerx + radius) * w, 0, (centerx + cpOffset) * w, h));
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, 0, (centerx - cpOffset) * w, h, (centerx - radius) * w, 0));
        // Down up
        centerx = .5;
        fig.Add(new PathSegment(SegmentType.Bezier, centerx * w, h, (centerx + radius) * w, 0, (centerx + cpOffset) * w, h));
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, 0, (centerx - cpOffset) * w, h, (centerx - radius) * w, 0));
        // Down up
        centerx = .7;
        fig.Add(new PathSegment(SegmentType.Bezier, centerx * w, h, (centerx + radius) * w, 0, (centerx + cpOffset) * w, h));
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, 0, (centerx - cpOffset) * w, h, (centerx - radius) * w, 0));
        // Down up
        centerx = .9;
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx + cpOffset * .5) * w, h, (centerx + radius) * w, 0, (centerx + cpOffset) * w, h));
        return geo;
      });

      Shape.DefineFigureGenerator("ACvoltageSource", (shape, w, h) => {
        var geo = new Geometry();
        var cpOffset = KAPPA * .5;
        var radius = .5;
        var centerx = .5;
        var centery = .5;
        var fig = new PathFigure((centerx - radius) * w, centery * h, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - radius) * h, (centerx - radius) * w, (centery - cpOffset) * h,
          (centerx - cpOffset) * w, (centery - radius) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, centery * h, (centerx + cpOffset) * w, (centery - radius) * h,
          (centerx + radius) * w, (centery - cpOffset) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radius) * h, (centerx + radius) * w, (centery + cpOffset) * h,
          (centerx + cpOffset) * w, (centery + radius) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w, centery * h, (centerx - cpOffset) * w, (centery + radius) * h,
          (centerx - radius) * w, (centery + cpOffset) * h));
        fig.Add(new PathSegment(SegmentType.Move, (centerx - radius + .1) * w, centery * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx + radius - .1) * w, centery * h, centerx * w, (centery - radius) * h,
          centerx * w, (centery + radius) * h));
        return geo;
      });

      Shape.DefineFigureGenerator("DCvoltageSource", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, .75 * h, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0, .25 * h));
        fig.Add(new PathSegment(SegmentType.Move, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        return geo;
      });

      Shape.DefineFigureGenerator("Diode", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(w, 0, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        geo.Spot1 = new Spot(0, .25);
        geo.Spot2 = new Spot(.5, .75);
        return geo;
      });

      Shape.DefineFigureGenerator("Wifi", (shape, w, h) => {
        var geo = new Geometry();
        var origw = w;
        var origh = h;
        w = w * .38;
        h = h * .6;
        var cpOffset = KAPPA * .8;
        var radius = .8;
        var centerx = 0d;
        var centery = .5;
        var xOffset = (origw - w) / 2;
        var yOffset = (origh - h) / 2;
        var fig = new PathFigure(centerx * w + xOffset, (centery + radius) * h + yOffset, true);
        geo.Add(fig);

        // Left curves
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w + xOffset,
          centery * h + yOffset, (centerx - cpOffset) * w + xOffset,
          (centery + radius) * h + yOffset,
          (centerx - radius) * w + xOffset,
          (centery + cpOffset) * h + yOffset));
        fig.Add(new PathSegment(SegmentType.Bezier, centerx * w + xOffset,
          (centery - radius) * h + yOffset, (centerx - radius) * w + xOffset,
          (centery - cpOffset) * h + yOffset,
          (centerx - cpOffset) * w + xOffset,
          (centery - radius) * h + yOffset));
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx - radius + cpOffset * .5) * w + xOffset,
          centery * h + yOffset, centerx * w + xOffset,
          (centery - radius) * h + yOffset,
          (centerx - radius + cpOffset * .5) * w + xOffset,
          (centery - cpOffset) * h + yOffset));
        fig.Add(new PathSegment(SegmentType.Bezier, centerx * w + xOffset,
          (centery + radius) * h + yOffset, (centerx - radius + cpOffset * .5) * w + xOffset,
          (centery + cpOffset) * h + yOffset,
          centerx * w + xOffset,
          (centery + radius) * h + yOffset).Close());
        cpOffset = KAPPA * .4;
        radius = .4;
        centerx = .2;
        centery = .5;
        var fig2 = new PathFigure(centerx * w + xOffset, (centery + radius) * h + yOffset, true);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w + xOffset,
          centery * h + yOffset, (centerx - cpOffset) * w + xOffset,
          (centery + radius) * h + yOffset,
          (centerx - radius) * w + xOffset,
          (centery + cpOffset) * h + yOffset));
        fig2.Add(new PathSegment(SegmentType.Bezier, centerx * w + xOffset,
          (centery - radius) * h + yOffset, (centerx - radius) * w + xOffset,
          (centery - cpOffset) * h + yOffset,
          (centerx - cpOffset) * w + xOffset,
          (centery - radius) * h + yOffset));
        fig2.Add(new PathSegment(SegmentType.Bezier, (centerx - radius + cpOffset * .5) * w + xOffset,
          centery * h + yOffset, centerx * w + xOffset,
          (centery - radius) * h + yOffset,
          (centerx - radius + cpOffset * .5) * w + xOffset,
          (centery - cpOffset) * h + yOffset));
        fig2.Add(new PathSegment(SegmentType.Bezier, centerx * w + xOffset,
          (centery + radius) * h + yOffset, (centerx - radius + cpOffset * .5) * w + xOffset,
          (centery + cpOffset) * h + yOffset,
          centerx * w + xOffset,
          (centery + radius) * h + yOffset).Close());
        cpOffset = KAPPA * .2;
        radius = .2;
        centerx = .5;
        centery = .5;
        var fig3 = new PathFigure((centerx - radius) * w + xOffset, centery * h + yOffset, true);
        geo.Add(fig3);
        // Center circle
        fig3.Add(new PathSegment(SegmentType.Bezier, centerx * w + xOffset,
          (centery - radius) * h + yOffset, (centerx - radius) * w + xOffset,
          (centery - cpOffset) * h + yOffset,
          (centerx - cpOffset) * w + xOffset,
          (centery - radius) * h + yOffset));
        fig3.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w + xOffset,
          centery * h + yOffset, (centerx + cpOffset) * w + xOffset,
          (centery - radius) * h + yOffset,
          (centerx + radius) * w + xOffset,
          (centery - cpOffset) * h + yOffset));
        fig3.Add(new PathSegment(SegmentType.Bezier, centerx * w + xOffset,
          (centery + radius) * h + yOffset, (centerx + radius) * w + xOffset,
          (centery + cpOffset) * h + yOffset,
          (centerx + cpOffset) * w + xOffset,
          (centery + radius) * h + yOffset));
        fig3.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w + xOffset,
          centery * h + yOffset, (centerx - cpOffset) * w + xOffset,
          (centery + radius) * h + yOffset,
          (centerx - radius) * w + xOffset,
          (centery + cpOffset) * h + yOffset));
        cpOffset = KAPPA * .4;
        radius = .4;
        centerx = .8;
        centery = .5;
        var fig4 = new PathFigure(centerx * w + xOffset, (centery - radius) * h + yOffset, true);
        geo.Add(fig4);
        // Right curves
        fig4.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w + xOffset,
          centery * h + yOffset, (centerx + cpOffset) * w + xOffset,
          (centery - radius) * h + yOffset,
          (centerx + radius) * w + xOffset,
          (centery - cpOffset) * h + yOffset));
        fig4.Add(new PathSegment(SegmentType.Bezier, centerx * w + xOffset,
          (centery + radius) * h + yOffset, (centerx + radius) * w + xOffset,
          (centery + cpOffset) * h + yOffset,
          (centerx + cpOffset) * w + xOffset,
          (centery + radius) * h + yOffset));
        fig4.Add(new PathSegment(SegmentType.Bezier, (centerx + radius - cpOffset * .5) * w + xOffset,
          centery * h + yOffset, centerx * w + xOffset,
          (centery + radius) * h + yOffset,
          (centerx + radius - cpOffset * .5) * w + xOffset,
          (centery + cpOffset) * h + yOffset));
        fig4.Add(new PathSegment(SegmentType.Bezier, centerx * w + xOffset,
          (centery - radius) * h + yOffset, (centerx + radius - cpOffset * .5) * w + xOffset,
          (centery - cpOffset) * h + yOffset,
          centerx * w + xOffset,
          (centery - radius) * h + yOffset).Close());
        cpOffset = KAPPA * .8;
        radius = .8;
        centerx = 1;
        centery = .5;
        var fig5 = new PathFigure(centerx * w + xOffset, (centery - radius) * h + yOffset, true);
        geo.Add(fig5);
        fig5.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w + xOffset,
          centery * h + yOffset, (centerx + cpOffset) * w + xOffset,
          (centery - radius) * h + yOffset,
          (centerx + radius) * w + xOffset,
          (centery - cpOffset) * h + yOffset));
        fig5.Add(new PathSegment(SegmentType.Bezier, centerx * w + xOffset,
          (centery + radius) * h + yOffset, (centerx + radius) * w + xOffset,
          (centery + cpOffset) * h + yOffset,
          (centerx + cpOffset) * w + xOffset,
          (centery + radius) * h + yOffset));
        fig5.Add(new PathSegment(SegmentType.Bezier, (centerx + radius - cpOffset * .5) * w + xOffset,
          centery * h + yOffset, centerx * w + xOffset,
          (centery + radius) * h + yOffset,
          (centerx + radius - cpOffset * .5) * w + xOffset,
          (centery + cpOffset) * h + yOffset));
        fig5.Add(new PathSegment(SegmentType.Bezier, centerx * w + xOffset,
          (centery - radius) * h + yOffset, (centerx + radius - cpOffset * .5) * w + xOffset,
          (centery - cpOffset) * h + yOffset,
          centerx * w + xOffset,
          (centery - radius) * h + yOffset).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("Email", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, 0).Close());
        var fig2 = new PathFigure(0, 0, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, .5 * w, .6 * h));
        fig2.Add(new PathSegment(SegmentType.Line, w, 0));
        fig2.Add(new PathSegment(SegmentType.Move, 0, h));
        fig2.Add(new PathSegment(SegmentType.Line, .45 * w, .54 * h));
        fig2.Add(new PathSegment(SegmentType.Move, w, h));
        fig2.Add(new PathSegment(SegmentType.Line, .55 * w, .54 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("Ethernet", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(.35 * w, 0, true);
        geo.Add(fig);
        // Boxes above the wire
        fig.Add(new PathSegment(SegmentType.Line, .65 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, .65 * w, .4 * h));
        fig.Add(new PathSegment(SegmentType.Line, .35 * w, .4 * h));
        fig.Add(new PathSegment(SegmentType.Line, .35 * w, 0).Close());
        var fig2 = new PathFigure(.10 * w, h, true, true);
        geo.Add(fig2);
        // Boxes under the wire
        fig2.Add(new PathSegment(SegmentType.Line, .40 * w, h));
        fig2.Add(new PathSegment(SegmentType.Line, .40 * w, .6 * h));
        fig2.Add(new PathSegment(SegmentType.Line, .10 * w, .6 * h));
        fig2.Add(new PathSegment(SegmentType.Line, .10 * w, h).Close());
        var fig3 = new PathFigure(.60 * w, h, true, true);
        geo.Add(fig3);
        fig3.Add(new PathSegment(SegmentType.Line, .90 * w, h));
        fig3.Add(new PathSegment(SegmentType.Line, .90 * w, .6 * h));
        fig3.Add(new PathSegment(SegmentType.Line, .60 * w, .6 * h));
        fig3.Add(new PathSegment(SegmentType.Line, .60 * w, h).Close());
        var fig4 = new PathFigure(0, .5 * h, false);
        geo.Add(fig4);
        // Wire
        fig4.Add(new PathSegment(SegmentType.Line, w, .5 * h));
        fig4.Add(new PathSegment(SegmentType.Move, .5 * w, .5 * h));
        fig4.Add(new PathSegment(SegmentType.Line, .5 * w, .4 * h));
        fig4.Add(new PathSegment(SegmentType.Move, .75 * w, .5 * h));
        fig4.Add(new PathSegment(SegmentType.Line, .75 * w, .6 * h));
        fig4.Add(new PathSegment(SegmentType.Move, .25 * w, .5 * h));
        fig4.Add(new PathSegment(SegmentType.Line, .25 * w, .6 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("Power", (shape, w, h) => {
        var geo = new Geometry();
        var cpOffset = KAPPA * .4;
        var radius = .4;
        var centerx = .5;
        var centery = .5;
        var unused = new Point();
        var mid = new Point();
        var c1 = new Point();
        var c2 = new Point();
        // Find the 45 degree midpoint for the first bezier
        BreakUpBezier(centerx, centery - radius,
          centerx + cpOffset, centery - radius,
          centerx + radius, centery - cpOffset,
          centerx + radius, centery, .5, out unused,
          out unused, out mid, out c1, out c2);
        var start = new Point(mid.X, mid.Y);
        var fig = new PathFigure(mid.X * w, mid.Y * h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, centery * h, c1.X * w, c1.Y * h, c2.X * w, c2.Y * h));
        fig.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radius) * h, (centerx + radius) * w, (centery + cpOffset) * h,
          (centerx + cpOffset) * w, (centery + radius) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w, centery * h, (centerx - cpOffset) * w, (centery + radius) * h,
          (centerx - radius) * w, (centery + cpOffset) * h));
        // Find the 45 degree midpoint of for the fourth bezier
        BreakUpBezier(centerx - radius, centery,
          centerx - radius, centery - cpOffset,
          centerx - cpOffset, centery - radius,
          centerx, centery - radius, .5, out c1,
          out c2, out mid, out unused, out unused);
        fig.Add(new PathSegment(SegmentType.Bezier, mid.X * w, mid.Y * h, c1.X * w, c1.Y * h,
          c2.X * w, c2.Y * h));
        // now make a smaller circle
        cpOffset = KAPPA * .3;
        radius = .3;
        // Find the 45 degree midpoint for the first bezier
        BreakUpBezier(centerx - radius, centery,
          centerx - radius, centery - cpOffset,
          centerx - cpOffset, centery - radius,
          centerx, centery - radius, .5, out c1,
          out c2, out mid, out unused, out unused);
        fig.Add(new PathSegment(SegmentType.Line, mid.X * w, mid.Y * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w, centery * h, c2.X * w, c2.Y * h, c1.X * w, c1.Y * h));
        fig.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radius) * h, (centerx - radius) * w, (centery + cpOffset) * h,
          (centerx - cpOffset) * w, (centery + radius) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, centery * h, (centerx + cpOffset) * w, (centery + radius) * h,
          (centerx + radius) * w, (centery + cpOffset) * h));
        // Find the 45 degree midpoint for the fourth bezier
        BreakUpBezier(centerx, centery - radius,
          centerx + cpOffset, centery - radius,
          centerx + radius, centery - cpOffset,
          centerx + radius, centery, .5, out unused,
          out unused, out mid, out c1, out c2);
        fig.Add(new PathSegment(SegmentType.Bezier, mid.X * w, mid.Y * h, c2.X * w, c2.Y * h, c1.X * w, c1.Y * h).Close());
        fig = new PathFigure(.45 * w, 0, true);
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Line, .45 * w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, .55 * w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, .55 * w, 0).Close());
        geo.Spot1 = new Spot(.25, .45);
        geo.Spot2 = new Spot(.75, .8);
        return geo;
      });

      Shape.DefineFigureGenerator("Fallout", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, h / 2, true, true);
        geo.Add(fig);

        // Containing circle
        fig.Add(new PathSegment(SegmentType.Arc, 180, 360, w / 2, h / 2, w / 2, h / 2, false));

        void DrawTriangle(PathFigure f, double offsetx, double offsety) {
          f.Add(new PathSegment(SegmentType.Move, (.3 + offsetx) * w, (.8 + offsety) * h));
          f.Add(new PathSegment(SegmentType.Line, (.5 + offsetx) * w, (.5 + offsety) * h));
          f.Add(new PathSegment(SegmentType.Line, (.1 + offsetx) * w, (.5 + offsety) * h));
          f.Add(new PathSegment(SegmentType.Line, (.3 + offsetx) * w, (.8 + offsety) * h).Close());
        }

        // Triangles
        DrawTriangle(fig, 0, 0);
        DrawTriangle(fig, 0.4, 0);
        DrawTriangle(fig, 0.2, -0.3);
        return geo;
      });

      Shape.DefineFigureGenerator("IrritationHazard", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(.2 * w, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, .5 * w, .3 * h));
        fig.Add(new PathSegment(SegmentType.Line, .8 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, .2 * h));
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, .8 * h));
        fig.Add(new PathSegment(SegmentType.Line, .8 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, .5 * w, .7 * h));
        fig.Add(new PathSegment(SegmentType.Line, .2 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, .8 * h));
        fig.Add(new PathSegment(SegmentType.Line, .3 * w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, .2 * h).Close());
        geo.Spot1 = new Spot(.3, .3);
        geo.Spot2 = new Spot(.7, .7);
        return geo;
      });

      Shape.DefineFigureGenerator("ElectricalHazard", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(.37 * w, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, .5 * w, .11 * h));
        fig.Add(new PathSegment(SegmentType.Line, .77 * w, .04 * h));
        fig.Add(new PathSegment(SegmentType.Line, .33 * w, .49 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, .37 * h));
        fig.Add(new PathSegment(SegmentType.Line, .63 * w, .86 * h));
        fig.Add(new PathSegment(SegmentType.Line, .77 * w, .91 * h));
        fig.Add(new PathSegment(SegmentType.Line, .34 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, .34 * w, .78 * h));
        fig.Add(new PathSegment(SegmentType.Line, .44 * w, .8 * h));
        fig.Add(new PathSegment(SegmentType.Line, .65 * w, .56 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, .68 * h).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("FireHazard", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(.1 * w, h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Bezier, .29 * w, 0, -.25 * w, .63 * h,
          .45 * w, .44 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, .51 * w, .42 * h, .48 * w, .17 * h,
          .54 * w, .35 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, .59 * w, .18 * h, .59 * w, .29 * h,
          .58 * w, .28 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, .75 * w, .6 * h, .8 * w, .34 * h,
          .88 * w, .43 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, .88 * w, .31 * h, .87 * w, .48 * h,
          .88 * w, .43 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, .9 * w, h, 1.17 * w, .76 * h,
          .82 * w, .8 * h).Close());
        geo.Spot1 = new Spot(.07, .445);
        geo.Spot2 = new Spot(.884, .958);
        return geo;
      });

      Shape.DefineFigureGenerator("BpmnActivityLoop", (shape, w, h) => {
        var geo = new Geometry();
        var r = .5;
        var cx = 0;  // offset from Center x
        var cy = 0;  // offset from Center y
        var d = r * KAPPA;
        var mx1 = (.4 * Math.Sqrt(2) / 2 + .5);
        var my1 = (.5 - .5 * Math.Sqrt(2) / 2);
        var x1 = 1;
        var y1 = .5;
        var x2 = .5;
        var y2 = 0;
        var fig = new PathFigure(mx1 * w, (1 - my1) * h, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Bezier, x1 * w, y1 * h, x1 * w, .7 * h,
          x1 * w, y1 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (x2 + cx) * w, (y2 + cx) * h, (.5 + r + cx) * w, (.5 - d + cx) * h,
          (.5 + d + cx) * w, (.5 - r + cx) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (.5 - r + cx) * w, (.5 + cy) * h, (.5 - d + cx) * w, (.5 - r + cy) * h,
          (.5 - r + cx) * w, (.5 - d + cy) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (.35 + cx) * w, .9 * h, (.5 - r + cx) * w, (.5 + d + cy) * h,
          (.5 - d + cx) * w, .9 * h));
        // Arrowhead
        fig.Add(new PathSegment(SegmentType.Move, (.25 + cx) * w, 0.8 * h));
        fig.Add(new PathSegment(SegmentType.Line, (.35 + cx) * w, 0.9 * h));
        fig.Add(new PathSegment(SegmentType.Line, (.20 + cx) * w, 0.95 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("BpmnActivityParallel", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Move, .5 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, .5 * w, h));
        fig.Add(new PathSegment(SegmentType.Move, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        return geo;
      });

      Shape.DefineFigureGenerator("BpmnActivitySequential", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Move, 0, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Move, 0, h));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        return geo;
      });

      Shape.DefineFigureGenerator("BpmnActivityAdHoc", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0, false);
        geo.Add(fig);

        var fig2 = new PathFigure(w, h, false);
        geo.Add(fig2);
        var fig3 = new PathFigure(0, .5 * h, false);
        geo.Add(fig3);
        fig3.Add(new PathSegment(SegmentType.Bezier, .5 * w, .5 * h, .2 * w, .35 * h,
          .3 * w, .35 * h));
        fig3.Add(new PathSegment(SegmentType.Bezier, w, .5 * h, .7 * w, .65 * h,
          .8 * w, .65 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("BpmnActivityCompensation", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, .5 * h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, .5 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, .5 * w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, .5 * w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, .5 * w, h).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("BpmnTaskMessage", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, .2 * h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w, .2 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, .8 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, .8 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, .8 * h).Close());
        fig = new PathFigure(0, .2 * h, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, .5 * w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, .2 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("BpmnTaskScript", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(.7 * w, h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, .3 * w, h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.3 * w, 0, .6 * w, .5 * h,
          0, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, .7 * w, 0));
        fig.Add(new PathSegment(SegmentType.Bezier, .7 * w, h, .4 * w, .5 * h,
          w, .5 * h).Close());
        var fig2 = new PathFigure(.45 * w, .73 * h, false);
        geo.Add(fig2);
        // Lines on script
        fig2.Add(new PathSegment(SegmentType.Line, .7 * w, .73 * h));
        fig2.Add(new PathSegment(SegmentType.Move, .38 * w, .5 * h));
        fig2.Add(new PathSegment(SegmentType.Line, .63 * w, .5 * h));
        fig2.Add(new PathSegment(SegmentType.Move, .31 * w, .27 * h));
        fig2.Add(new PathSegment(SegmentType.Line, .56 * w, .27 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("BpmnTaskUser", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0, false);
        geo.Add(fig);

        var fig2 = new PathFigure(.335 * w, (1 - .555) * h, true);
        geo.Add(fig2);
        // Shirt
        fig2.Add(new PathSegment(SegmentType.Line, .335 * w, (1 - .405) * h));
        fig2.Add(new PathSegment(SegmentType.Line, (1 - .335) * w, (1 - .405) * h));
        fig2.Add(new PathSegment(SegmentType.Line, (1 - .335) * w, (1 - .555) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, w, .68 * h, (1 - .12) * w, .46 * h,
          (1 - .02) * w, .54 * h));
        fig2.Add(new PathSegment(SegmentType.Line, w, h));
        fig2.Add(new PathSegment(SegmentType.Line, 0, h));
        fig2.Add(new PathSegment(SegmentType.Line, 0, .68 * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, .335 * w, (1 - .555) * h, .02 * w, .54 * h,
          .12 * w, .46 * h));
        // Start of neck
        fig2.Add(new PathSegment(SegmentType.Line, .365 * w, (1 - .595) * h));
        var radiushead = .5 - .285;
        var centerx = .5;
        var centery = radiushead;
        var alpha2 = Math.PI / 4;
        var KAPPA2 = ((4 * (1 - Math.Cos(alpha2))) / (3 * Math.Sin(alpha2)));
        var cpOffset = KAPPA2 * .5;
        var radiusw = radiushead;
        var radiush = radiushead;
        var offsetw = KAPPA2 * radiusw;
        var offseth = KAPPA2 * radiush;
        // Circle (head)
        fig2.Add(new PathSegment(SegmentType.Bezier, (centerx - radiusw) * w, centery * h, (centerx - ((offsetw + radiusw) / 2)) * w, (centery + ((radiush + offseth) / 2)) * h,
          (centerx - radiusw) * w, (centery + offseth) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - radiush) * h, (centerx - radiusw) * w, (centery - offseth) * h,
          (centerx - offsetw) * w, (centery - radiush) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, (centerx + radiusw) * w, centery * h, (centerx + offsetw) * w, (centery - radiush) * h,
          (centerx + radiusw) * w, (centery - offseth) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, (1 - .365) * w, (1 - .595) * h, (centerx + radiusw) * w, (centery + offseth) * h,
          (centerx + ((offsetw + radiusw) / 2)) * w, (centery + ((radiush + offseth) / 2)) * h));
        fig2.Add(new PathSegment(SegmentType.Line, (1 - .365) * w, (1 - .595) * h));
        // Neckline
        fig2.Add(new PathSegment(SegmentType.Line, (1 - .335) * w, (1 - .555) * h));
        fig2.Add(new PathSegment(SegmentType.Line, (1 - .335) * w, (1 - .405) * h));
        fig2.Add(new PathSegment(SegmentType.Line, .335 * w, (1 - .405) * h));
        var fig3 = new PathFigure(.2 * w, h, false);
        geo.Add(fig3);
        // Arm lines
        fig3.Add(new PathSegment(SegmentType.Line, .2 * w, .8 * h));
        var fig4 = new PathFigure(.8 * w, h, false);
        geo.Add(fig4);
        fig4.Add(new PathSegment(SegmentType.Line, .8 * w, .8 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("BpmnEventConditional", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(.1 * w, 0, true);
        geo.Add(fig);

        // Body
        fig.Add(new PathSegment(SegmentType.Line, .9 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, .9 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, .1 * w, h).Close());
        var fig2 = new PathFigure(.2 * w, .2 * h, false);
        geo.Add(fig2);
        // Inside lines
        fig2.Add(new PathSegment(SegmentType.Line, .8 * w, .2 * h));
        fig2.Add(new PathSegment(SegmentType.Move, .2 * w, .4 * h));
        fig2.Add(new PathSegment(SegmentType.Line, .8 * w, .4 * h));
        fig2.Add(new PathSegment(SegmentType.Move, .2 * w, .6 * h));
        fig2.Add(new PathSegment(SegmentType.Line, .8 * w, .6 * h));
        fig2.Add(new PathSegment(SegmentType.Move, .2 * w, .8 * h));
        fig2.Add(new PathSegment(SegmentType.Line, .8 * w, .8 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("BpmnEventError", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, .33 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, .66 * w, .50 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, .66 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, .33 * w, .50 * h).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("BpmnEventEscalation", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0, false);
        geo.Add(fig);
        // Set dimensions
        var fig2 = new PathFigure(w, h, false);
        geo.Add(fig2);
        var fig3 = new PathFigure(.1 * w, h, true);
        geo.Add(fig3);
        fig3.Add(new PathSegment(SegmentType.Line, .5 * w, 0));
        fig3.Add(new PathSegment(SegmentType.Line, .9 * w, h));
        fig3.Add(new PathSegment(SegmentType.Line, .5 * w, .5 * h).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("Caution", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0.05 * w, h, true);
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Bezier, 0.1 * w, .8 * h, 0, h, 0, h));
        fig.Add(new PathSegment(SegmentType.Line, 0.45 * w, .1 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.55 * w, .1 * h, 0.5 * w, 0, 0.5 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, 0.95 * w, 0.9 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.9 * w, h, w, h, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0.05 * w, h));
        var radius = 0.05;
        // Bottom circle of exclamation point
        fig.Add(new PathSegment(SegmentType.Move, (0.5 - radius) * w, 0.875 * h));
        fig.Add(new PathSegment(SegmentType.Arc, 180, -360, 0.5 * w, 0.875 * h, radius * w, radius * h));
        // Upper rectangle of exclamation point
        fig.Add(new PathSegment(SegmentType.Move, 0.5 * w, 0.75 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.5 * w, 0.325 * h, 0.575 * w, 0.725 * h, 0.625 * w, 0.375 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.5 * w, 0.75 * h, 0.375 * w, 0.375 * h, 0.425 * w, 0.725 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("Recycle", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0.45 * w, 0.95 * h, false);
        geo.Add(fig);

        // Bottom left arrow
        fig.Add(new PathSegment(SegmentType.Line, 0.2 * w, 0.95 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.185 * w, 0.85 * h, 0.17 * w, 0.95 * h, 0.15 * w, 0.9 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.235 * w, 0.75 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.30 * w, 0.625 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.35 * w, 0.65 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.275 * w, 0.45 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.05 * w, 0.45 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.1 * w, 0.5 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.05 * w, 0.575 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.1875 * w, 0.95 * h, 0, 0.675 * h, 0, 0.7 * h));
        fig.Add(new PathSegment(SegmentType.Move, 0.45 * w, 0.95 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.45 * w, 0.775 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.22 * w, 0.775 * h));
        var fig2 = new PathFigure(0.475 * w, 0.2 * h, false);
        geo.Add(fig2);
        // Top arrow
        fig2.Add(new PathSegment(SegmentType.Line, 0.4 * w, 0.4 * h));
        fig2.Add(new PathSegment(SegmentType.Line, 0.225 * w, 0.3 * h));
        fig2.Add(new PathSegment(SegmentType.Line, 0.275 * w, 0.175 * h));
        fig2.Add(new PathSegment(SegmentType.Line, 0.325 * w, 0.05 * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, 0.4 * w, 0.05 * h, 0.35 * w, 0, 0.375 * w, 0));
        fig2.Add(new PathSegment(SegmentType.Line, 0.575 * w, 0.375 * h));
        fig2.Add(new PathSegment(SegmentType.Line, 0.525 * w, 0.4 * h));
        fig2.Add(new PathSegment(SegmentType.Line, 0.75 * w, 0.475 * h));
        fig2.Add(new PathSegment(SegmentType.Line, 0.85 * w, 0.315 * h));
        fig2.Add(new PathSegment(SegmentType.Line, 0.8 * w, 0.32 * h));
        fig2.Add(new PathSegment(SegmentType.Line, 0.65 * w, 0.05 * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, 0.575 * w, 0, 0.65 * w, 0.05 * h, 0.625 * w, 0));
        fig2.Add(new PathSegment(SegmentType.Line, 0.38 * w, 0.0105 * h));
        var fig3 = new PathFigure(0.675 * w, 0.575 * h, false);
        geo.Add(fig3);
        // Bottom right arrow
        fig3.Add(new PathSegment(SegmentType.Line, 0.875 * w, 0.525 * h));
        fig3.Add(new PathSegment(SegmentType.Line, w, 0.775 * h));
        fig3.Add(new PathSegment(SegmentType.Bezier, 0.85 * w, 0.95 * h, w, 0.8 * h, w, 0.85 * h));
        fig3.Add(new PathSegment(SegmentType.Line, 0.65 * w, 0.95 * h));
        fig3.Add(new PathSegment(SegmentType.Line, 0.65 * w, h));
        fig3.Add(new PathSegment(SegmentType.Line, 0.55 * w, 0.85 * h));
        fig3.Add(new PathSegment(SegmentType.Line, 0.65 * w, 0.725 * h));
        fig3.Add(new PathSegment(SegmentType.Line, 0.65 * w, 0.775 * h));
        fig3.Add(new PathSegment(SegmentType.Line, 0.7 * w, 0.775 * h));
        fig3.Add(new PathSegment(SegmentType.Line, w, 0.775 * h));
        fig3.Add(new PathSegment(SegmentType.Move, 0.675 * w, 0.575 * h));
        fig3.Add(new PathSegment(SegmentType.Line, 0.775 * w, 0.775 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("BpmnEventTimer", (shape, w, h) => {
        var geo = new Geometry();
        var radius = .5;
        var cpOffset = KAPPA * .5;
        var fig = new PathFigure(w, radius * h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Bezier, radius * w, h, w, (radius + cpOffset) * h,
          (radius + cpOffset) * w, h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, radius * h, (radius - cpOffset) * w, h,
          0, (radius + cpOffset) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, radius * w, 0, 0, (radius - cpOffset) * h,
          (radius - cpOffset) * w, 0));
        fig.Add(new PathSegment(SegmentType.Bezier, w, radius * h, (radius + cpOffset) * w, 0,
          w, (radius - cpOffset) * h));
        var fig2 = new PathFigure(radius * w, 0, false);
        geo.Add(fig2);
        // Hour lines
        fig2.Add(new PathSegment(SegmentType.Line, radius * w, .15 * h));
        fig2.Add(new PathSegment(SegmentType.Move, radius * w, h));
        fig2.Add(new PathSegment(SegmentType.Line, radius * w, .85 * h));
        fig2.Add(new PathSegment(SegmentType.Move, 0, radius * h));
        fig2.Add(new PathSegment(SegmentType.Line, .15 * w, radius * h));
        fig2.Add(new PathSegment(SegmentType.Move, w, radius * h));
        fig2.Add(new PathSegment(SegmentType.Line, .85 * w, radius * h));
        // Clock hands
        fig2.Add(new PathSegment(SegmentType.Move, radius * w, radius * h));
        fig2.Add(new PathSegment(SegmentType.Line, .58 * w, 0.1 * h));
        fig2.Add(new PathSegment(SegmentType.Move, radius * w, radius * h));
        fig2.Add(new PathSegment(SegmentType.Line, .78 * w, .54 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("Package", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0.15 * h, true);
        geo.Add(fig);

        // Package bottom rectangle
        fig.Add(new PathSegment(SegmentType.Line, w, 0.15 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h).Close());
        var fig2 = new PathFigure(0, 0.15 * h, true);
        geo.Add(fig2);
        // Package top flap
        fig2.Add(new PathSegment(SegmentType.Line, 0, 0));
        fig2.Add(new PathSegment(SegmentType.Line, 0.6 * w, 0));
        fig2.Add(new PathSegment(SegmentType.Line, 0.65 * w, 0.15 * h).Close());
        geo.Spot1 = new Spot(0, 0.1);
        geo.Spot2 = new Spot(1, 1);
        return geo;
      });

      Shape.DefineFigureGenerator("Class", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        // Class box
        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, 0).Close());
        var fig2 = new PathFigure(0, 0.2 * h, false);
        geo.Add(fig2);
        // Top box separater
        fig2.Add(new PathSegment(SegmentType.Line, w, 0.2 * h).Close());
        var fig3 = new PathFigure(0, 0.5 * h, false);
        geo.Add(fig3);
        // Middle box separater
        fig3.Add(new PathSegment(SegmentType.Line, w, 0.5 * h).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("Component", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(w, h, true);
        geo.Add(fig);

        // Component Box
        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, 0.15 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, 0.15 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, w, h).Close());
        var fig2 = new PathFigure(0, 0.2 * h, true);
        geo.Add(fig2);
        // Component top sub box
        fig2.Add(new PathSegment(SegmentType.Line, 0.45 * w, 0.2 * h));
        fig2.Add(new PathSegment(SegmentType.Line, 0.45 * w, 0.4 * h));
        fig2.Add(new PathSegment(SegmentType.Line, 0, 0.4 * h));
        fig2.Add(new PathSegment(SegmentType.Line, 0, 0.2 * h).Close());
        var fig3 = new PathFigure(0, 0.6 * h, true);
        geo.Add(fig3);
        // Component bottom sub box
        fig3.Add(new PathSegment(SegmentType.Line, 0.45 * w, 0.6 * h));
        fig3.Add(new PathSegment(SegmentType.Line, 0.45 * w, 0.8 * h));
        fig3.Add(new PathSegment(SegmentType.Line, 0, 0.8 * h));
        fig3.Add(new PathSegment(SegmentType.Line, 0, 0.6 * h).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("Boat Shipment", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0.15 * w, 0.6 * h, true);
        geo.Add(fig);

        // Boat shipment flag
        fig.Add(new PathSegment(SegmentType.Line, 0.15 * w, 0.6 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, 0.6 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.15 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0.85 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, w, 0.6 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.85 * w, 0.6 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.5 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, 0.15 * w, 0.6 * h));
        var fig2 = new PathFigure(0.15 * w, 0.6 * h, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, 0.85 * w, 0.6 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("Customer/Supplier", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(w, h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, 0.66 * w, 0.33 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.66 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, 0.33 * w, 0.33 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.33 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, 0, 0.33 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Line, w, h).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("Workcell", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0.65 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0.65 * w, 0.4 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.35 * w, 0.4 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.35 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("Supermarket", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, 0.33 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, 0.33 * h));
        fig.Add(new PathSegment(SegmentType.Move, w, 0.33 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, 0.66 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, 0.66 * h));
        fig.Add(new PathSegment(SegmentType.Move, w, 0.66 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        return geo;
      });

      Shape.DefineFigureGenerator("TruckShipment", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        // Left rectangle
        fig.Add(new PathSegment(SegmentType.Line, 0.6 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, 0.6 * w, 0.8 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, 0.8 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, 0).Close());
        var fig2 = new PathFigure(w, 0.8 * h, true);
        geo.Add(fig2);
        // Right rectangle
        fig2.Add(new PathSegment(SegmentType.Line, w, 0.4 * h));
        fig2.Add(new PathSegment(SegmentType.Line, 0.6 * w, 0.4 * h));
        fig2.Add(new PathSegment(SegmentType.Line, 0.6 * w, 0.8 * h));
        fig2.Add(new PathSegment(SegmentType.Line, w, 0.8 * h).Close());
        var radius = .1;
        var cpOffset = KAPPA * .1;
        var centerx = .2;
        var centery = .9;
        var fig3 = new PathFigure((centerx - radius) * w, centery * h, true);
        geo.Add(fig3);
        // Left wheel
        fig3.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - radius) * h, (centerx - radius) * w, (centery - cpOffset) * h,
          (centerx - cpOffset) * w, (centery - radius) * h));
        fig3.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, centery * h, (centerx + cpOffset) * w, (centery - radius) * h,
          (centerx + radius) * w, (centery - cpOffset) * h));
        fig3.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radius) * h, (centerx + radius) * w, (centery + cpOffset) * h,
          (centerx + cpOffset) * w, (centery + radius) * h));
        fig3.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w, centery * h, (centerx - cpOffset) * w, (centery + radius) * h,
          (centerx - radius) * w, (centery + cpOffset) * h).Close());
        radius = .1;
        cpOffset = KAPPA * .1;
        centerx = .8;
        centery = .9;
        var fig4 = new PathFigure((centerx - radius) * w, centery * h, true);
        geo.Add(fig4);
        // Right wheel
        fig4.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - radius) * h, (centerx - radius) * w, (centery - cpOffset) * h,
          (centerx - cpOffset) * w, (centery - radius) * h));
        fig4.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, centery * h, (centerx + cpOffset) * w, (centery - radius) * h,
          (centerx + radius) * w, (centery - cpOffset) * h));
        fig4.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radius) * h, (centerx + radius) * w, (centery + cpOffset) * h,
          (centerx + cpOffset) * w, (centery + radius) * h));
        fig4.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w, centery * h, (centerx - cpOffset) * w, (centery + radius) * h,
          (centerx - radius) * w, (centery + cpOffset) * h).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("KanbanPost", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0.2 * w, 0, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0.2 * w, .5 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.8 * w, 0.5 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.8 * w, 0));
        fig.Add(new PathSegment(SegmentType.Move, 0.5 * w, 0.5 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.5 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0.2 * w, h));
        fig.Add(new PathSegment(SegmentType.Move, 0.5 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0.8 * w, h));
        return geo;
      });

      Shape.DefineFigureGenerator("Forklift", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0, 0.5 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.5 * w, 0.5 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.4 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, 0, 0));
        var fig2 = new PathFigure(0, 0.5 * h, true);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, 0, 0.8 * h));
        fig2.Add(new PathSegment(SegmentType.Line, 0.5 * w, 0.8 * h));
        fig2.Add(new PathSegment(SegmentType.Line, 0.5 * w, 0.5 * h));
        fig2.Add(new PathSegment(SegmentType.Line, 0, 0.5 * h));
        var fig3 = new PathFigure(0.50 * w, 0.8 * h, true);
        geo.Add(fig3);
        fig3.Add(new PathSegment(SegmentType.Line, 0.50 * w, 0.1 * h));
        fig3.Add(new PathSegment(SegmentType.Line, 0.55 * w, 0.1 * h));
        fig3.Add(new PathSegment(SegmentType.Line, 0.55 * w, 0.8 * h));
        fig3.Add(new PathSegment(SegmentType.Line, 0.50 * w, 0.8 * h));
        var fig4 = new PathFigure(0.5 * w, 0.7 * h, false);
        geo.Add(fig4);
        fig4.Add(new PathSegment(SegmentType.Line, w, 0.7 * h));
        var radius = .1;
        var cpOffset = KAPPA * .1;
        var centerx = .1;
        var centery = .9;
        var fig5 = new PathFigure((centerx - radius) * w, centery * h, true);
        geo.Add(fig5);
        fig5.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - radius) * h, (centerx - radius) * w, (centery - cpOffset) * h,
          (centerx - cpOffset) * w, (centery - radius) * h));
        fig5.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, centery * h, (centerx + cpOffset) * w, (centery - radius) * h,
          (centerx + radius) * w, (centery - cpOffset) * h));
        fig5.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radius) * h, (centerx + radius) * w, (centery + cpOffset) * h,
          (centerx + cpOffset) * w, (centery + radius) * h));
        fig5.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w, centery * h, (centerx - cpOffset) * w, (centery + radius) * h,
          (centerx - radius) * w, (centery + cpOffset) * h));
        radius = .1;
        cpOffset = KAPPA * .1;
        centerx = .4;
        centery = .9;
        var fig6 = new PathFigure((centerx - radius) * w, centery * h, true);
        geo.Add(fig6);
        fig6.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - radius) * h, (centerx - radius) * w, (centery - cpOffset) * h,
          (centerx - cpOffset) * w, (centery - radius) * h));
        fig6.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, centery * h, (centerx + cpOffset) * w, (centery - radius) * h,
          (centerx + radius) * w, (centery - cpOffset) * h));
        fig6.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radius) * h, (centerx + radius) * w, (centery + cpOffset) * h,
          (centerx + cpOffset) * w, (centery + radius) * h));
        fig6.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w, centery * h, (centerx - cpOffset) * w, (centery + radius) * h,
          (centerx - radius) * w, (centery + cpOffset) * h));
        return geo;
      });

      Shape.DefineFigureGenerator("RailShipment", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0.1 * w, 0.4 * h, true);
        geo.Add(fig);

        // Left cart
        fig.Add(new PathSegment(SegmentType.Line, 0.45 * w, 0.4 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.45 * w, 0.9 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.1 * w, 0.9 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.1 * w, 0.4 * h).Close());
        var fig2 = new PathFigure(0.45 * w, 0.7 * h, false);
        geo.Add(fig2);
        // Line connecting carts
        fig2.Add(new PathSegment(SegmentType.Line, 0.55 * w, 0.7 * h));
        var fig3 = new PathFigure(0.55 * w, 0.4 * h, true);
        geo.Add(fig3);
        // Right cart
        fig3.Add(new PathSegment(SegmentType.Line, 0.9 * w, 0.4 * h));
        fig3.Add(new PathSegment(SegmentType.Line, 0.9 * w, 0.9 * h));
        fig3.Add(new PathSegment(SegmentType.Line, 0.55 * w, 0.9 * h));
        fig3.Add(new PathSegment(SegmentType.Line, 0.55 * w, 0.4 * h).Close());
        var radius = .05;
        var cpOffset = KAPPA * .05;
        var centerx = .175;
        var centery = .95;
        var fig4 = new PathFigure((centerx - radius) * w, centery * h, true);
        geo.Add(fig4);
        // Wheels
        fig4.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - radius) * h, (centerx - radius) * w, (centery - cpOffset) * h,
          (centerx - cpOffset) * w, (centery - radius) * h));
        fig4.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, centery * h, (centerx + cpOffset) * w, (centery - radius) * h,
          (centerx + radius) * w, (centery - cpOffset) * h));
        fig4.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radius) * h, (centerx + radius) * w, (centery + cpOffset) * h,
          (centerx + cpOffset) * w, (centery + radius) * h));
        fig4.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w, centery * h, (centerx - cpOffset) * w, (centery + radius) * h,
          (centerx - radius) * w, (centery + cpOffset) * h));
        radius = .05;
        cpOffset = KAPPA * .05;
        centerx = .375;
        centery = .95;
        var fig5 = new PathFigure((centerx - radius) * w, centery * h, true);
        geo.Add(fig5);
        fig5.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - radius) * h, (centerx - radius) * w, (centery - cpOffset) * h,
          (centerx - cpOffset) * w, (centery - radius) * h));
        fig5.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, centery * h, (centerx + cpOffset) * w, (centery - radius) * h,
          (centerx + radius) * w, (centery - cpOffset) * h));
        fig5.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radius) * h, (centerx + radius) * w, (centery + cpOffset) * h,
          (centerx + cpOffset) * w, (centery + radius) * h));
        fig5.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w, centery * h, (centerx - cpOffset) * w, (centery + radius) * h,
          (centerx - radius) * w, (centery + cpOffset) * h));
        radius = .05;
        cpOffset = KAPPA * .05;
        centerx = .625;
        centery = .95;
        var fig6 = new PathFigure((centerx - radius) * w, centery * h, true);
        geo.Add(fig6);
        fig6.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - radius) * h, (centerx - radius) * w, (centery - cpOffset) * h,
          (centerx - cpOffset) * w, (centery - radius) * h));
        fig6.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, centery * h, (centerx + cpOffset) * w, (centery - radius) * h,
          (centerx + radius) * w, (centery - cpOffset) * h));
        fig6.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radius) * h, (centerx + radius) * w, (centery + cpOffset) * h,
          (centerx + cpOffset) * w, (centery + radius) * h));
        fig6.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w, centery * h, (centerx - cpOffset) * w, (centery + radius) * h,
          (centerx - radius) * w, (centery + cpOffset) * h));
        radius = .05;
        cpOffset = KAPPA * .05;
        centerx = .825;
        centery = .95;
        var fig7 = new PathFigure((centerx - radius) * w, centery * h, true);
        geo.Add(fig7);
        fig7.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - radius) * h, (centerx - radius) * w, (centery - cpOffset) * h,
          (centerx - cpOffset) * w, (centery - radius) * h));
        fig7.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, centery * h, (centerx + cpOffset) * w, (centery - radius) * h,
          (centerx + radius) * w, (centery - cpOffset) * h));
        fig7.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radius) * h, (centerx + radius) * w, (centery + cpOffset) * h,
          (centerx + cpOffset) * w, (centery + radius) * h));
        fig7.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w, centery * h, (centerx - cpOffset) * w, (centery + radius) * h,
          (centerx - radius) * w, (centery + cpOffset) * h).Close());
        var fig8 = new PathFigure(0, h, false);
        geo.Add(fig8);
        fig8.Add(new PathSegment(SegmentType.Line, w, h).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("Warehouse", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, 0).Close());
        var fig2 = new PathFigure(0, 0.2 * h, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, w, 0.2 * h).Close());
        var fig3 = new PathFigure(0.15 * w, h, true);
        geo.Add(fig3);
        fig3.Add(new PathSegment(SegmentType.Line, 0.15 * w, 0.5 * h));
        fig3.Add(new PathSegment(SegmentType.Line, 0.40 * w, 0.5 * h));
        fig3.Add(new PathSegment(SegmentType.Line, 0.40 * w, h));
        fig3.Add(new PathSegment(SegmentType.Line, 0.15 * w, h).Close());
        var radius = .05;
        var cpOffset = KAPPA * .05;
        var centerx = .35;
        var centery = .775;
        var fig4 = new PathFigure((centerx - radius) * w, centery * h, true);
        geo.Add(fig4);
        // Door handle
        fig4.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - radius) * h, (centerx - radius) * w, (centery - cpOffset) * h,
          (centerx - cpOffset) * w, (centery - radius) * h));
        fig4.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, centery * h, (centerx + cpOffset) * w, (centery - radius) * h,
          (centerx + radius) * w, (centery - cpOffset) * h));
        fig4.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radius) * h, (centerx + radius) * w, (centery + cpOffset) * h,
          (centerx + cpOffset) * w, (centery + radius) * h));
        fig4.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w, centery * h, (centerx - cpOffset) * w, (centery + radius) * h,
          (centerx - radius) * w, (centery + cpOffset) * h).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("ControlCenter", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0, 0.8 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.1 * w, 0.8 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.1 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, 0.9 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, 0.9 * w, 0.8 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, 0.8 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Move, 0.1 * w, 0.8 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.9 * w, 0.8 * h).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("Bluetooth", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0.75 * h, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0, 0.75 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, 0.25 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.5 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, 0.5 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, w, 0.75 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, 0.25 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("Bookmark", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Line, 0.5 * w, 0.6 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, 0, 0));
        fig.Add(new PathSegment(SegmentType.Move, 0.2 * w, 0.2 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.8 * w, 0.2 * h));
        fig.Add(new PathSegment(SegmentType.Move, 0.2 * w, 0.4 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.8 * w, 0.4 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("Bookmark", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Line, 0.5 * w, 0.6 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        fig.Add(new PathSegment(SegmentType.Line, 0, 0));
        fig.Add(new PathSegment(SegmentType.Move, 0.2 * w, 0.2 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.8 * w, 0.2 * h));
        fig.Add(new PathSegment(SegmentType.Move, 0.2 * w, 0.4 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.8 * w, 0.4 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("Globe", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0.5 * w, 0, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0.5 * w, h));
        fig.Add(new PathSegment(SegmentType.Move, 0, 0.5 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, 0.5 * h));
        fig.Add(new PathSegment(SegmentType.Move, 0.5 * w, 0));
        fig.Add(new PathSegment(SegmentType.Bezier, w, 0.5 * h, 0.75 * w, 0, w, 0.25 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.5 * w, h, w, 0.75 * h, 0.75 * w, h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, 0.5 * h, 0.25 * w, h, 0, 0.75 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.5 * w, 0, 0, 0.25 * h, 0.25 * w, 0));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.5 * w, h, 0.15 * w, 0.25 * h, 0.15 * w, 0.75 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.5 * w, 0, 0.85 * w, 0.75 * h, 0.85 * w, 0.25 * h));
        fig.Add(new PathSegment(SegmentType.Move, 0.1675 * w, 0.15 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.8325 * w, 0.15 * h, 0.35 * w, 0.3 * h, 0.65 * w, 0.3 * h));
        fig.Add(new PathSegment(SegmentType.Move, 0.1675 * w, 0.85 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.8325 * w, 0.85 * h, 0.35 * w, 0.7 * h, 0.65 * w, 0.7 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("Wave", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0.25 * h, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Bezier, 0.3 * w, 0.25 * h, 0.10 * w, 0, 0.2 * w, 0));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.7 * w, 0.25 * h, 0.425 * w, 0.5 * h, 0.575 * w, 0.5 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, w, 0.25 * h, 0.8 * w, 0, 0.9 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, 0.75 * h));
        fig.Add(new PathSegment(SegmentType.Move, 0, 0.25 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, 0.75 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.3 * w, 0.75 * h, 0.10 * w, 0.5 * h, 0.2 * w, 0.5 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.7 * w, 0.75 * h, 0.425 * w, h, 0.575 * w, h));
        fig.Add(new PathSegment(SegmentType.Bezier, w, 0.75 * h, 0.8 * w, 0.5 * h, 0.9 * w, 0.5 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("Operator", (shape, w, h) => {
        var geo = new Geometry();
        var radius = .3;
        var cpOffset = KAPPA * .3;
        var centerx = .5;
        var centery = .7;
        var fig = new PathFigure((centerx - radius) * w, centery * h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - radius) * h, (centerx - radius) * w, (centery - cpOffset) * h,
          (centerx - cpOffset) * w, (centery - radius) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, centery * h, (centerx + cpOffset) * w, (centery - radius) * h,
          (centerx + radius) * w, (centery - cpOffset) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radius) * h, (centerx + radius) * w, (centery + cpOffset) * h,
          (centerx + cpOffset) * w, (centery + radius) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w, centery * h, (centerx - cpOffset) * w, (centery + radius) * h,
          (centerx - radius) * w, (centery + cpOffset) * h));
        var fig2 = new PathFigure(0, 0.7 * h, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Bezier, w, 0.7 * h, 0, 0, w, 0));
        return geo;
      });

      Shape.DefineFigureGenerator("TripleFanBlades", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0.5 * w, 0, true);
        geo.Add(fig);

        // Top blade
        fig.Add(new PathSegment(SegmentType.Bezier, 0.5 * w, 0.65 * h, 0.65 * w, 0.3 * h, 0.65 * w, 0.5 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.5 * w, 0, 0.35 * w, 0.5 * h, 0.35 * w, 0.3 * h));
        // Bottom left blade
        fig.Add(new PathSegment(SegmentType.Move, 0.5 * w, 0.65 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, h, 0.3 * w, 0.6 * h, 0.1 * w, 0.8 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.5 * w, 0.65 * h, 0.2 * w, h, 0.35 * w, 0.95 * h));
        // Bottom right blade
        fig.Add(new PathSegment(SegmentType.Move, 0.5 * w, 0.65 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, w, h, 0.7 * w, 0.6 * h, 0.9 * w, 0.8 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.5 * w, 0.65 * h, 0.8 * w, h, 0.65 * w, 0.95 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("CentrifugalPump", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(w, 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0.4 * w, 0));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, 0.5 * h, 0, 0.075 * h, 0, 0.5 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.4 * w, h, 0, h, 0.4 * w, h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.8 * w, 0.4 * h, 0.8 * w, h, 0.85 * w, 0.6 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, 0.4 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, 0));
        return geo;
      });

      Shape.DefineFigureGenerator("Battery", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0, 0.1 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, 0.1 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Move, 0.4 * w, 0.1 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.4 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, 0.6 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, 0.6 * w, 0.1 * h));
        var fig2 = new PathFigure(0, 0.6 * h, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Move, 0, 0.4 * h));
        fig2.Add(new PathSegment(SegmentType.Line, w, 0.4 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("Delete", (shape, w, h) => {
        var geo = new Geometry();
        var radius = .5;
        var cpOffset = KAPPA * .5;
        var centerx = .5;
        var centery = .5;
        var fig = new PathFigure((centerx - radius) * w, centery * h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - radius) * h, (centerx - radius) * w, (centery - cpOffset) * h,
          (centerx - cpOffset) * w, (centery - radius) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, centery * h, (centerx + cpOffset) * w, (centery - radius) * h,
          (centerx + radius) * w, (centery - cpOffset) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radius) * h, (centerx + radius) * w, (centery + cpOffset) * h,
          (centerx + cpOffset) * w, (centery + radius) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w, centery * h, (centerx - cpOffset) * w, (centery + radius) * h,
          (centerx - radius) * w, (centery + cpOffset) * h));
        var fig2 = new PathFigure(0.15 * w, 0.5 * h, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, 0.85 * w, 0.5 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("Flag", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0.1 * h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Move, 0, 0.1 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.5 * w, 0.1 * h, 0.15 * w, 0, 0.35 * w, 0));
        fig.Add(new PathSegment(SegmentType.Bezier, w, 0.1 * h, 0.65 * w, 0.2 * h, 0.85 * w, 0.2 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, 0.5 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.5 * w, 0.5 * h, 0.85 * w, 0.6 * h, 0.65 * w, 0.6 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, 0.5 * h, 0.35 * w, 0.4 * h, 0.15 * w, 0.4 * h).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("Help", (shape, w, h) => {
        var geo = new Geometry();
        var radius = .5;
        var cpOffset = KAPPA * .5;
        var centerx = .5;
        var centery = .5;
        var fig = new PathFigure((centerx - radius) * w, centery * h, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - radius) * h, (centerx - radius) * w, (centery - cpOffset) * h,
          (centerx - cpOffset) * w, (centery - radius) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, centery * h, (centerx + cpOffset) * w, (centery - radius) * h,
          (centerx + radius) * w, (centery - cpOffset) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radius) * h, (centerx + radius) * w, (centery + cpOffset) * h,
          (centerx + cpOffset) * w, (centery + radius) * h));
        fig.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w, centery * h, (centerx - cpOffset) * w, (centery + radius) * h,
          (centerx - radius) * w, (centery + cpOffset) * h).Close());
        radius = .05;
        cpOffset = KAPPA * .05;
        centerx = .5;
        centery = .8;
        var fig2 = new PathFigure((centerx - radius) * w, centery * h, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery - radius) * h, (centerx - radius) * w, (centery - cpOffset) * h,
          (centerx - cpOffset) * w, (centery - radius) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, (centerx + radius) * w, centery * h, (centerx + cpOffset) * w, (centery - radius) * h,
          (centerx + radius) * w, (centery - cpOffset) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, centerx * w, (centery + radius) * h, (centerx + radius) * w, (centery + cpOffset) * h,
          (centerx + cpOffset) * w, (centery + radius) * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, (centerx - radius) * w, centery * h, (centerx - cpOffset) * w, (centery + radius) * h,
          (centerx - radius) * w, (centery + cpOffset) * h).Close());
        fig2.Add(new PathSegment(SegmentType.Move, 0.5 * w, 0.7 * h));
        fig2.Add(new PathSegment(SegmentType.Line, 0.5 * w, 0.5 * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, 0.5 * w, 0.2 * h, 0.75 * w, 0.475 * h, 0.75 * w, 0.225 * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, 0.3 * w, 0.35 * h, 0.4 * w, 0.2 * h, 0.3 * w, 0.25 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("Location", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(0.5 * w, h, true)
            .Add(new PathSegment(SegmentType.Line, 0.75 * w, 0.5 * h))
            .Add(new PathSegment(SegmentType.Bezier, 0.5 * w, 0, .975 * w, 0.025 * h, 0.5 * w, 0))
            .Add(new PathSegment(SegmentType.Bezier, 0.25 * w, 0.5 * h, 0.5 * w, 0, 0.025 * w, 0.025 * h).Close())
            .Add(new PathSegment(SegmentType.Move, 0.5 * w, 0.2 * h))
            .Add(new PathSegment(SegmentType.Arc, 270, 360, 0.5 * w, 0.3 * h, 0.1 * w, 0.1 * h).Close()));
      });

      Shape.DefineFigureGenerator("Lock", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0.5 * h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, w, 0.5 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, 0.5 * h));
        var fig2 = new PathFigure(0.2 * w, 0.5 * h, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Move, 0.2 * w, 0.5 * h));
        fig2.Add(new PathSegment(SegmentType.Line, 0.2 * w, 0.3 * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, 0.8 * w, 0.3 * h, 0.25 * w, 0, 0.75 * w, 0));
        fig2.Add(new PathSegment(SegmentType.Line, 0.8 * w, 0.5 * h));
        fig2.Add(new PathSegment(SegmentType.Line, 0.8 * w, 0.3 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("Unlocked", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0.5 * h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0, h));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, w, 0.5 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, 0.5 * h));
        var fig2 = new PathFigure(0.2 * w, 0.5 * h, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Move, 0.2 * w, 0.5 * h));
        fig2.Add(new PathSegment(SegmentType.Line, 0.2 * w, 0.3 * h));
        fig2.Add(new PathSegment(SegmentType.Bezier, 0.8 * w, 0.3 * h, 0.25 * w, 0, 0.75 * w, 0));
        fig2.Add(new PathSegment(SegmentType.Line, 0.8 * w, 0.35 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("Gear", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(0.9375 * w, 0.56246875 * h, true)
            .Add(new PathSegment(SegmentType.Line, 0.9375 * w, 0.4375 * h))
            .Add(new PathSegment(SegmentType.Line, 0.80621875 * w, 0.4375 * h))
            .Add(new PathSegment(SegmentType.Bezier, 0.763 * w, 0.3316875 * h, 0.79840625 * w, 0.39915625 * h, 0.7834375 * w, 0.3635 * h))
            .Add(new PathSegment(SegmentType.Line, 0.8566875 * w, 0.23796875 * h))
            .Add(new PathSegment(SegmentType.Line, 0.76825 * w, 0.14959375 * h))
            .Add(new PathSegment(SegmentType.Line, 0.67596875 * w, 0.24184375 * h))
            .Add(new PathSegment(SegmentType.Bezier, 0.5625 * w, 0.19378125 * h, 0.64228125 * w, 0.2188125 * h, 0.603875 * w, 0.2022875 * h))
            .Add(new PathSegment(SegmentType.Line, 0.5625 * w, 0.0625 * h))
            .Add(new PathSegment(SegmentType.Line, 0.4375 * w, 0.0625 * h))
            .Add(new PathSegment(SegmentType.Line, 0.4375 * w, 0.19378125 * h))
            .Add(new PathSegment(SegmentType.Bezier, 0.32775 * w, 0.239375 * h, 0.39759375 * w, 0.20190625 * h, 0.36053125 * w, 0.2176875 * h))
            .Add(new PathSegment(SegmentType.Line, 0.2379375 * w, 0.14959375 * h))
            .Add(new PathSegment(SegmentType.Line, 0.14953125 * w, 0.2379375 * h))
            .Add(new PathSegment(SegmentType.Line, 0.23934375 * w, 0.3278125 * h))
            .Add(new PathSegment(SegmentType.Bezier, 0.19378125 * w, 0.4375 * h, 0.21765625 * w, 0.36059375 * h, 0.201875 * w, 0.397625 * h))
            .Add(new PathSegment(SegmentType.Line, 0.0625 * w, 0.4375 * h))
            .Add(new PathSegment(SegmentType.Line, 0.0625 * w, 0.5625 * h))
            .Add(new PathSegment(SegmentType.Line, 0.1938125 * w, 0.5625 * h))
            .Add(new PathSegment(SegmentType.Bezier, 0.241875 * w, 0.67596875 * h, 0.20221875 * w, 0.603875 * h, 0.21884375 * w, 0.64228125 * h))
            .Add(new PathSegment(SegmentType.Line, 0.1495625 * w, 0.76825 * h))
            .Add(new PathSegment(SegmentType.Line, 0.238 * w, 0.8566875 * h))
            .Add(new PathSegment(SegmentType.Line, 0.3316875 * w, 0.76296875 * h))
            .Add(new PathSegment(SegmentType.Bezier, 0.43753125 * w, 0.80621875 * h, 0.36353125 * w, 0.78340625 * h, 0.3991875 * w, 0.79840625 * h))
            .Add(new PathSegment(SegmentType.Line, 0.43753125 * w, 0.9375 * h))
            .Add(new PathSegment(SegmentType.Line, 0.5625 * w, 0.9375 * h))
            .Add(new PathSegment(SegmentType.Line, 0.5625 * w, 0.80621875 * h))
            .Add(new PathSegment(SegmentType.Bezier, 0.67225 * w, 0.760625 * h, 0.602375 * w, 0.79809375 * h, 0.63946875 * w, 0.78234375 * h))
            .Add(new PathSegment(SegmentType.Line, 0.76828125 * w, 0.8566875 * h))
            .Add(new PathSegment(SegmentType.Line, 0.85671875 * w, 0.76825 * h))
            .Add(new PathSegment(SegmentType.Line, 0.76065625 * w, 0.67221875 * h))
            .Add(new PathSegment(SegmentType.Bezier, 0.80621875 * w, 0.56246875 * h, 0.78234375 * w, 0.63940625 * h, 0.798125 * w, 0.602375 * h))
            .Add(new PathSegment(SegmentType.Line, 0.9375 * w, 0.56246875 * h).Close())

            .Add(new PathSegment(SegmentType.Move, 0.5 * w, 0.6 * h))
            .Add(new PathSegment(SegmentType.Arc, 90, 360, 0.5 * w, 0.5 * h, 0.1 * w, 0.1 * h).Close()));
      });

      Shape.DefineFigureGenerator("Hand", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0.5 * h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Bezier, 0.1 * w, 0.3 * h, 0, 0.375 * h, 0.05 * w, 0.325 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.45 * w, 0.075 * h, 0.3 * w, 0.225 * h, 0.4 * w, 0.175 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.525 * w, 0.075 * h, 0.46 * w, 0.05 * h, 0.525 * w, 0.05 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.3 * w, 0.4 * h, 0.525 * w, 0.275 * h, 0.475 * w, 0.325 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.9 * w, 0.4 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.9 * w, 0.55 * h, w, 0.4 * h, w, 0.55 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.425 * w, 0.55 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.6 * w, 0.55 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.6 * w, 0.7 * h, 0.675 * w, 0.55 * h, 0.675 * w, 0.7 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.4 * w, 0.7 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.575 * w, 0.7 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.575 * w, 0.85 * h, 0.65 * w, 0.7 * h, 0.65 * w, 0.85 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.4 * w, 0.85 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.525 * w, 0.85 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.535 * w, h, 0.61 * w, 0.85 * h, 0.61 * w, h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, 0.9 * h, 0.435 * w, h, 0, h).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("Map", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0.2 * h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0.25 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, 0.5 * w, 0.2 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.75 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, w, 0.2 * h));
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0.75 * w, 0.8 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.5 * w, h));
        fig.Add(new PathSegment(SegmentType.Line, 0.25 * w, 0.8 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, h).Close());
        fig.Add(new PathSegment(SegmentType.Move, 0.25 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, 0.25 * w, 0.8 * h));
        fig.Add(new PathSegment(SegmentType.Move, 0.5 * w, 0.2 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.5 * w, h));
        fig.Add(new PathSegment(SegmentType.Move, 0.75 * w, 0));
        fig.Add(new PathSegment(SegmentType.Line, 0.75 * w, 0.8 * h));
        return geo;
      });

      Shape.DefineFigureGenerator("Eject", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, h, true);
        geo.Add(fig);

        // bottom rectangle section
        fig.Add(new PathSegment(SegmentType.Line, w, h));
        fig.Add(new PathSegment(SegmentType.Line, w, h * .7));
        fig.Add(new PathSegment(SegmentType.Line, 0, h * .7).Close());
        var fig2 = new PathFigure(0, (h * .6), true);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, w, (.6 * h)));
        fig2.Add(new PathSegment(SegmentType.Line, .5 * w, 0).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("Pencil", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(0, 0, true)
            .Add(new PathSegment(SegmentType.Line, 0.2 * w, 0.1 * h))
            .Add(new PathSegment(SegmentType.Line, w, 0.9 * h))
            .Add(new PathSegment(SegmentType.Line, 0.9 * w, h))
            .Add(new PathSegment(SegmentType.Line, 0.1 * w, 0.2 * h).Close()));
      });

      Shape.DefineFigureGenerator("Building", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(w * 1, h * 1, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0, h * 1));  // bottom part
        fig.Add(new PathSegment(SegmentType.Line, 0, h * .85));
        fig.Add(new PathSegment(SegmentType.Line, .046 * w, h * .85));
        fig.Add(new PathSegment(SegmentType.Line, .046 * w, h * .45));
        fig.Add(new PathSegment(SegmentType.Line, 0, h * .45));
        fig.Add(new PathSegment(SegmentType.Line, 0, h * .30));
        fig.Add(new PathSegment(SegmentType.Line, .046 * w, h * .30));
        fig.Add(new PathSegment(SegmentType.Line, .5 * w, h * 0));
        fig.Add(new PathSegment(SegmentType.Line, (1 - .046) * w, h * .30));
        fig.Add(new PathSegment(SegmentType.Line, w, h * .30));
        fig.Add(new PathSegment(SegmentType.Line, w, h * .45));
        fig.Add(new PathSegment(SegmentType.Line, (1 - .046) * w, h * .45));
        fig.Add(new PathSegment(SegmentType.Line, (1 - .046) * w, h * .85));
        fig.Add(new PathSegment(SegmentType.Line, w, h * .85).Close());
        var fig2 = new PathFigure(.126 * w, .85 * h, false);  // is filled in our not
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, .126 * w, .45 * h));
        fig2.Add(new PathSegment(SegmentType.Line, .322 * w, .45 * h));
        fig2.Add(new PathSegment(SegmentType.Line, .322 * w, .85 * h).Close());
        var fig3 = new PathFigure(.402 * w, .85 * h, false);  // is filled in our not
        geo.Add(fig3);
        fig3.Add(new PathSegment(SegmentType.Line, .402 * w, .45 * h));
        fig3.Add(new PathSegment(SegmentType.Line, .598 * w, .45 * h));
        fig3.Add(new PathSegment(SegmentType.Line, .598 * w, .85 * h).Close());
        var fig4 = new PathFigure(.678 * w, .85 * h, false);  // is filled in our not
        geo.Add(fig4);
        fig4.Add(new PathSegment(SegmentType.Line, .678 * w, .45 * h));
        fig4.Add(new PathSegment(SegmentType.Line, .874 * w, .45 * h));
        fig4.Add(new PathSegment(SegmentType.Line, .874 * w, .85 * h).Close());
        // the top inner triangle
        var fig5 = new PathFigure(.5 * w, .1 * h, false);  // is filled in our not
        geo.Add(fig5);
        fig5.Add(new PathSegment(SegmentType.Line, (.046 + .15) * w, .30 * h));
        fig5.Add(new PathSegment(SegmentType.Line, (1 - (.046 + .15)) * w, .30 * h).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("Staircase", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, h * 1, true);
        geo.Add(fig);

        // Bottom part
        fig.Add(new PathSegment(SegmentType.Line, w * .20, h * 1));  // bottom left part
        fig.Add(new PathSegment(SegmentType.Line, w * .20, h * .80));
        fig.Add(new PathSegment(SegmentType.Line, w * .40, h * .80));
        fig.Add(new PathSegment(SegmentType.Line, w * .40, h * .60));
        fig.Add(new PathSegment(SegmentType.Line, w * .60, h * .60));
        fig.Add(new PathSegment(SegmentType.Line, w * .60, h * .40));
        fig.Add(new PathSegment(SegmentType.Line, w * .80, h * .40));
        fig.Add(new PathSegment(SegmentType.Line, w * .80, h * .20));
        fig.Add(new PathSegment(SegmentType.Line, w * 1, h * .20));
        fig.Add(new PathSegment(SegmentType.Line, w * 1, h * .15));
        fig.Add(new PathSegment(SegmentType.Line, w * .75, h * .15));
        fig.Add(new PathSegment(SegmentType.Line, w * .75, h * .35));
        fig.Add(new PathSegment(SegmentType.Line, w * .55, h * .35));
        fig.Add(new PathSegment(SegmentType.Line, w * .55, h * .55));
        fig.Add(new PathSegment(SegmentType.Line, w * .35, h * .55));
        fig.Add(new PathSegment(SegmentType.Line, w * .35, h * .75));
        fig.Add(new PathSegment(SegmentType.Line, w * .15, h * .75));
        fig.Add(new PathSegment(SegmentType.Line, w * .15, h * .95));
        fig.Add(new PathSegment(SegmentType.Line, 0, h * .95).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("5Bars", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, h * 1, true);  // bottom left
        geo.Add(fig);

        // Width of each bar is .184
        // space in between each bar is .2
        fig.Add(new PathSegment(SegmentType.Line, w * .184, h * 1));  // bottom left part
        fig.Add(new PathSegment(SegmentType.Line, w * .184, h * (1 - .184)).Close());
        var fig3 = new PathFigure(w * .204, h, true);  // is filled in our not
        geo.Add(fig3);
        fig3.Add(new PathSegment(SegmentType.Line, w * .204, h * (1 - .184)));
        fig3.Add(new PathSegment(SegmentType.Line, w * .388, h * (1 - (.184 * 2))));
        fig3.Add(new PathSegment(SegmentType.Line, w * .388, h * 1).Close());
        var fig4 = new PathFigure(w * .408, h, true);  // is filled in our not
        geo.Add(fig4);
        fig4.Add(new PathSegment(SegmentType.Line, w * .408, h * (1 - (.184 * 2))));
        fig4.Add(new PathSegment(SegmentType.Line, w * .592, h * (1 - (.184 * 3))));
        fig4.Add(new PathSegment(SegmentType.Line, w * .592, h * 1).Close());
        var fig5 = new PathFigure(w * .612, h, true);  // is filled in our not
        geo.Add(fig5);
        fig5.Add(new PathSegment(SegmentType.Line, w * .612, h * (1 - (.184 * 3))));
        fig5.Add(new PathSegment(SegmentType.Line, w * .796, h * (1 - (.184 * 4))));
        fig5.Add(new PathSegment(SegmentType.Line, w * .796, h * 1).Close());
        var fig6 = new PathFigure(w * .816, h, true);  // is filled in our not
        geo.Add(fig6);
        fig6.Add(new PathSegment(SegmentType.Line, w * .816, h * (1 - (.184 * 4))));
        fig6.Add(new PathSegment(SegmentType.Line, w * 1, h * (1 - (.184 * 5))));
        fig6.Add(new PathSegment(SegmentType.Line, w * 1, h * 1).Close());
        return geo;
      });

      // desktop
      Shape.DefineFigureGenerator("PC", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0, true);  // top right
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0, h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * .3, h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * .3, 0).Close());
        // Drive looking rectangle 1
        var fig2 = new PathFigure(w * .055, .07 * h, true);  // is filled in our not
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, w * .245, h * .07));
        fig2.Add(new PathSegment(SegmentType.Line, w * .245, h * .1));
        fig2.Add(new PathSegment(SegmentType.Line, w * .055, h * .1).Close());
        // Drive looking rectangle 2
        var fig3 = new PathFigure(w * .055, .13 * h, true);  // is filled in our not
        geo.Add(fig3);
        fig3.Add(new PathSegment(SegmentType.Line, w * .245, h * .13));
        fig3.Add(new PathSegment(SegmentType.Line, w * .245, h * .16));
        fig3.Add(new PathSegment(SegmentType.Line, w * .055, h * .16).Close());
        // Drive/cd rom looking rectangle 3
        var fig4 = new PathFigure(w * .055, .18 * h, true);  // is filled in our not
        geo.Add(fig4);
        fig4.Add(new PathSegment(SegmentType.Line, w * .245, h * .18));
        fig4.Add(new PathSegment(SegmentType.Line, w * .245, h * .21));
        fig4.Add(new PathSegment(SegmentType.Line, w * .055, h * .21).Close());
        var fig5 = new PathFigure(w * 1, 0, true);  // is filled in our not
        geo.Add(fig5);
        fig5.Add(new PathSegment(SegmentType.Line, w * .4, 0));
        fig5.Add(new PathSegment(SegmentType.Line, w * .4, h * .65));
        fig5.Add(new PathSegment(SegmentType.Line, w * 1, h * .65).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("Plane", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0.55 * w, h, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, 0.5 * w, 0.6 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.4 * w, 0.7 * h));
        fig.Add(new PathSegment(SegmentType.Line, .1 * w, 0.475 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.35 * w, 0.525 * h, 0, 0.4 * h, 0.225 * w, 0.45 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.4 * w, 0.475 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.15 * w, 0.35 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.2 * w, 0.325 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.6 * w, 0.325 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.85 * w, 0.1 * h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0.9 * w, 0.2 * h, 0.975 * w, 0, w, .08 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.7 * w, 0.45 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.6 * w, 0.95 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0.55 * w, h).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("Key", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(w * 1, h * .5, true);
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Line, w * .90, .40 * h));
        fig.Add(new PathSegment(SegmentType.Line, w * .50, .40 * h));
        fig.Add(new PathSegment(SegmentType.Line, w * .50, .35 * h));
        fig.Add(new PathSegment(SegmentType.Line, w * .45, .35 * h));
        fig.Add(new PathSegment(SegmentType.Line, w * .30, .20 * h));
        fig.Add(new PathSegment(SegmentType.Line, w * .15, .20 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, .35 * h));
        fig.Add(new PathSegment(SegmentType.Line, 0, .65 * h));
        fig.Add(new PathSegment(SegmentType.Line, w * .15, .80 * h));
        fig.Add(new PathSegment(SegmentType.Line, w * .30, .80 * h));
        fig.Add(new PathSegment(SegmentType.Line, w * .45, .65 * h));
        fig.Add(new PathSegment(SegmentType.Line, w * .50, .65 * h));
        fig.Add(new PathSegment(SegmentType.Line, w * .50, .6 * h));
        fig.Add(new PathSegment(SegmentType.Line, w * .60, .6 * h));
        fig.Add(new PathSegment(SegmentType.Line, w * .65, .55 * h));
        fig.Add(new PathSegment(SegmentType.Line, w * .70, .6 * h));
        fig.Add(new PathSegment(SegmentType.Line, w * .75, .55 * h));
        fig.Add(new PathSegment(SegmentType.Line, w * .80, .6 * h));
        fig.Add(new PathSegment(SegmentType.Line, w * .85, .575 * h));
        fig.Add(new PathSegment(SegmentType.Line, w * .9, 0.60 * h).Close());
        fig.Add(new PathSegment(SegmentType.Move, 0.17 * w, 0.425 * h));
        fig.Add(new PathSegment(SegmentType.Arc, 270, 360, 0.17 * w, 0.5 * h, 0.075 * w, 0.075 * h).Close());
        return geo;
      });

      // movie like logo
      Shape.DefineFigureGenerator("FilmTape", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(0, 0, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Arc, 270, 180, w * 0, w * 0.3, w * 0.055));  // left semi-circle
        fig.Add(new PathSegment(SegmentType.Line, 0, h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * .08, h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * .08, h * .95));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 1), h * .95));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 1), h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 2), h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 2), h * .95));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 3), h * .95));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 3), h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 4), h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 4), h * .95));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 5), h * .95));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 5), h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 6), h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 6), h * .95));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 7), h * .95));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 7), h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 8), h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 8), h * .95));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 9), h * .95));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 9), h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 10), h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 10), h * .95));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 11), h * .95));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 11), h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 12), h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 12), h * .95));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 13), h * .95));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 13), h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 14), h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 14), h * .95));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 15), h * .95));
        fig.Add(new PathSegment(SegmentType.Line, w * (.08 + .056 * 15), h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * 1, h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * 1, h * 1));
        var fig2 = new PathFigure(0, 0, false);  // is filled in our not
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, w * 1, h * 0));
        fig2.Add(new PathSegment(SegmentType.Arc, 270, -180, w * 1, w * 0.3, w * 0.055));  // right semi circle
        fig2.Add(new PathSegment(SegmentType.Line, w * 1, h * 1));
        // Each of the little square boxes on the tape
        var fig3 = new PathFigure(w * .11, h * .1, false);  // is filled in our not
        geo.Add(fig3);
        fig3.Add(new PathSegment(SegmentType.Line, w * (.11 + (.24133333 * 1) + (.028 * 0)), h * .1));
        fig3.Add(new PathSegment(SegmentType.Line, w * (.11 + (.24133333 * 1) + (.028 * 0)), h * .8));
        fig3.Add(new PathSegment(SegmentType.Line, w * .11, h * .8).Close());
        var fig4 = new PathFigure(w * (.11 + (.24133333 * 1) + (.028 * 1)), h * .1, false);  // is filled in our not
        geo.Add(fig4);
        fig4.Add(new PathSegment(SegmentType.Line, w * (.11 + (.24133333 * 2) + (.028 * 1)), h * .1));
        fig4.Add(new PathSegment(SegmentType.Line, w * (.11 + (.24133333 * 2) + (.028 * 1)), h * .8));
        fig4.Add(new PathSegment(SegmentType.Line, w * (.11 + (.24133333 * 1) + (.028 * 1)), h * .8).Close());
        var fig5 = new PathFigure(w * (.11 + (.24133333 * 2) + (.028 * 2)), h * .1, false);  // is filled in our not
        geo.Add(fig5);
        fig5.Add(new PathSegment(SegmentType.Line, w * (.11 + (.24133333 * 3) + (.028 * 2)), h * .1));
        fig5.Add(new PathSegment(SegmentType.Line, w * (.11 + (.24133333 * 3) + (.028 * 2)), h * .8));
        fig5.Add(new PathSegment(SegmentType.Line, w * (.11 + (.24133333 * 2) + (.028 * 2)), h * .8).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("FloppyDisk", (shape, w, h) => {
        var geo = new Geometry();
        var roundValue = 8;
        var cpOffset = roundValue * KAPPA;
        var fig = new PathFigure(roundValue, 0, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w * .86, 0));
        fig.Add(new PathSegment(SegmentType.Line, w * 1, h * .14));
        fig.Add(new PathSegment(SegmentType.Line, w, h - roundValue));
        fig.Add(new PathSegment(SegmentType.Bezier, w - roundValue, h, w, h - cpOffset, w - cpOffset, h));
        fig.Add(new PathSegment(SegmentType.Line, roundValue, h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, h - roundValue, cpOffset, h, 0, h - cpOffset));
        fig.Add(new PathSegment(SegmentType.Line, 0, roundValue));
        fig.Add(new PathSegment(SegmentType.Bezier, roundValue, 0, 0, cpOffset, cpOffset, 0).Close());
        // interior slightly  rectangle
        var fig2 = new PathFigure(w * .83, 0, false);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, w * .83, h * .3));
        fig2.Add(new PathSegment(SegmentType.Line, w * .17, h * .3));
        fig2.Add(new PathSegment(SegmentType.Line, w * .17, h * 0).Close());
        var fig3 = new PathFigure(w * .83, h * 1, false);
        geo.Add(fig3);
        fig3.Add(new PathSegment(SegmentType.Line, w * .83, h * .5));
        fig3.Add(new PathSegment(SegmentType.Line, w * .17, h * .5));
        fig3.Add(new PathSegment(SegmentType.Line, w * .17, h * 1).Close());
        var fig4 = new PathFigure(w * .78, h * .05, false);
        geo.Add(fig4);
        fig4.Add(new PathSegment(SegmentType.Line, w * .66, h * .05));
        fig4.Add(new PathSegment(SegmentType.Line, w * .66, h * .25));
        fig4.Add(new PathSegment(SegmentType.Line, w * .78, h * .25).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("SpeechBubble", (shape, w, h) => {
        var param1 = (shape != null) ? shape.Parameter1 : double.NaN;
        if (double.IsNaN(param1) || param1 < 0) param1 = 15;  // default corner
        param1 = Math.Min(param1, w / 3);
        param1 = Math.Min(param1, h / 3);

        var cpOffset = param1 * KAPPA;
        var bubbleH = h * .8;  // leave some room at bottom for pointer

        var geo = new Geometry();
        var fig = new PathFigure(param1, 0, true);
        geo.Add(fig);
        fig.Add(new PathSegment(SegmentType.Line, w - param1, 0));
        fig.Add(new PathSegment(SegmentType.Bezier, w, param1, w - cpOffset, 0, w, cpOffset));
        fig.Add(new PathSegment(SegmentType.Line, w, bubbleH - param1));
        fig.Add(new PathSegment(SegmentType.Bezier, w - param1, bubbleH, w, bubbleH - cpOffset, w - cpOffset, bubbleH));
        fig.Add(new PathSegment(SegmentType.Line, w * .70, bubbleH));
        fig.Add(new PathSegment(SegmentType.Line, w * .70, h));
        fig.Add(new PathSegment(SegmentType.Line, w * .55, bubbleH));
        fig.Add(new PathSegment(SegmentType.Line, param1, bubbleH));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, bubbleH - param1, cpOffset, bubbleH, 0, bubbleH - cpOffset));
        fig.Add(new PathSegment(SegmentType.Line, 0, param1));
        fig.Add(new PathSegment(SegmentType.Bezier, param1, 0, 0, cpOffset, cpOffset, 0).Close());
        if (cpOffset > 1) {
          geo.Spot1 = new Spot(0, 0, cpOffset, cpOffset);
          geo.Spot2 = new Spot(1, .8, -cpOffset, -cpOffset);
        } else {
          geo.Spot1 = Spot.TopLeft;
          geo.Spot2 = new Spot(1, .8);
        }
        return geo;
      });

      Shape.DefineFigureGenerator("Repeat", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(w * 0, h * .45, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w * .25, h * 0));
        fig.Add(new PathSegment(SegmentType.Line, w * .50, h * .45));
        fig.Add(new PathSegment(SegmentType.Line, w * .30, h * .45));
        fig.Add(new PathSegment(SegmentType.Line, w * .30, h * .90));
        fig.Add(new PathSegment(SegmentType.Line, w * .60, h * .90));
        fig.Add(new PathSegment(SegmentType.Line, w * .65, h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * .20, h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * .20, h * .45).Close());
        var fig2 = new PathFigure(w * 1, h * .55, true);  // is filled in our not
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, w * .75, h * 1));
        fig2.Add(new PathSegment(SegmentType.Line, w * .50, h * .55));
        fig2.Add(new PathSegment(SegmentType.Line, w * .70, h * .55));
        fig2.Add(new PathSegment(SegmentType.Line, w * .70, h * .10));
        fig2.Add(new PathSegment(SegmentType.Line, w * .40, h * .10));
        fig2.Add(new PathSegment(SegmentType.Line, w * .35, h * 0));
        fig2.Add(new PathSegment(SegmentType.Line, w * .80, h * 0));
        fig2.Add(new PathSegment(SegmentType.Line, w * .80, h * .55).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("Windows", (shape, w, h) => {
        return new Geometry()
          .Add(new PathFigure(0, 0, true)
            .Add(new PathSegment(SegmentType.Line, w, 0))
            .Add(new PathSegment(SegmentType.Line, w, h))
            .Add(new PathSegment(SegmentType.Line, 0, h).Close())
            .Add(new PathSegment(SegmentType.Move, 0.4 * w, 0.4 * h))
            .Add(new PathSegment(SegmentType.Line, 0.4 * w, 0.8 * h))
            .Add(new PathSegment(SegmentType.Line, 0.9 * w, 0.8 * h))
            .Add(new PathSegment(SegmentType.Line, 0.9 * w, 0.4 * h).Close())
            .Add(new PathSegment(SegmentType.Move, 0.2 * w, 0.1 * h))
            .Add(new PathSegment(SegmentType.Line, 0.2 * w, 0.6 * h))
            .Add(new PathSegment(SegmentType.Line, 0.7 * w, 0.6 * h))
            .Add(new PathSegment(SegmentType.Line, 0.7 * w, 0.1 * h).Close())
            .Add(new PathSegment(SegmentType.Move, 0.1 * w, 0.6 * h))
            .Add(new PathSegment(SegmentType.Line, 0.1 * w, 0.9 * h))
            .Add(new PathSegment(SegmentType.Line, 0.5 * w, 0.9 * h))
            .Add(new PathSegment(SegmentType.Line, 0.5 * w, 0.6 * h).Close()));
      });

      Shape.DefineFigureGenerator("Terminal", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(w * 0, h * .10, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w * 1, h * .10));
        fig.Add(new PathSegment(SegmentType.Line, w * 1, h * .90));
        fig.Add(new PathSegment(SegmentType.Line, w * 0, h * .90).Close());
        var fig2 = new PathFigure(w * .10, h * .20, true);  // is filled in our not
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, w * .10, h * .25));
        fig2.Add(new PathSegment(SegmentType.Line, w * .22, h * .285));  // midpoint
        fig2.Add(new PathSegment(SegmentType.Line, w * .10, h * .32));
        fig2.Add(new PathSegment(SegmentType.Line, w * .10, h * .37));
        fig2.Add(new PathSegment(SegmentType.Line, w * .275, h * .32));
        fig2.Add(new PathSegment(SegmentType.Line, w * .275, h * .25).Close());
        var fig3 = new PathFigure(w * .28, h * .37, true);  // is filled in our not
        geo.Add(fig3);
        fig3.Add(new PathSegment(SegmentType.Line, w * .45, h * .37));
        fig3.Add(new PathSegment(SegmentType.Line, w * .45, h * .41));
        fig3.Add(new PathSegment(SegmentType.Line, w * .28, h * .41).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("Beaker", (shape, w, h) => {
        var geo = new Geometry();
        var param1 = 15;
        var cpOffset = param1 * KAPPA;
        var fig = new PathFigure(w * .62, h * .475, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w, h - param1));
        fig.Add(new PathSegment(SegmentType.Bezier, w - param1, h, w, h - cpOffset, w - cpOffset, h));
        fig.Add(new PathSegment(SegmentType.Line, param1, h));
        fig.Add(new PathSegment(SegmentType.Bezier, 0, h - param1, cpOffset, h, 0, h - cpOffset));
        fig.Add(new PathSegment(SegmentType.Line, w * .38, h * .475));
        fig.Add(new PathSegment(SegmentType.Line, w * .38, h * .03));
        fig.Add(new PathSegment(SegmentType.Line, w * .36, h * 0));
        fig.Add(new PathSegment(SegmentType.Line, w * .64, h * 0));
        fig.Add(new PathSegment(SegmentType.Line, w * .62, h * .03).Close());
        if (cpOffset > 1) {
          geo.Spot1 = new Spot(0, 0, cpOffset, cpOffset);
          geo.Spot2 = new Spot(1, 1, -cpOffset, -cpOffset);
        } else {
          geo.Spot1 = Spot.TopLeft;
          geo.Spot2 = Spot.BottomRight;
        }
        return geo;
      });

      Shape.DefineFigureGenerator("Download", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(w * 0, h * 1, true);
        geo.Add(fig);

        var third = .1 / .3;  // just to keep values consistent
        // outer frame
        // starts bottom left
        fig.Add(new PathSegment(SegmentType.Line, w * 1, h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * 1, h * (1 - third)));
        fig.Add(new PathSegment(SegmentType.Line, w * .8, h * 0));
        fig.Add(new PathSegment(SegmentType.Line, w * .66, h * 0));
        fig.Add(new PathSegment(SegmentType.Line, w * .66, h * .055));
        fig.Add(new PathSegment(SegmentType.Line, w * .755, h * .055));
        fig.Add(new PathSegment(SegmentType.Line, w * .93, h * (1 - third)));
        fig.Add(new PathSegment(SegmentType.Line, w * .64, h * (1 - third)));
        fig.Add(new PathSegment(SegmentType.Line, w * .61, h * .75));
        fig.Add(new PathSegment(SegmentType.Line, w * .5, h * .75));
        fig.Add(new PathSegment(SegmentType.Line, w * .39, h * .75));
        fig.Add(new PathSegment(SegmentType.Line, w * .36, h * (1 - third)));
        fig.Add(new PathSegment(SegmentType.Line, w * .07, h * (1 - third)));
        fig.Add(new PathSegment(SegmentType.Line, w * (1 - .755), h * (.055)));
        fig.Add(new PathSegment(SegmentType.Line, w * (1 - .66), h * (.055)));
        fig.Add(new PathSegment(SegmentType.Line, w * (1 - .66), h * (0)));
        fig.Add(new PathSegment(SegmentType.Line, w * (1 - .8), h * (0)));
        fig.Add(new PathSegment(SegmentType.Line, w * 0, h * (1 - third)).Close());
        // arrow pointing down
        var fig2 = new PathFigure(w * .40, h * 0, true);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, w * .40, h * .44));
        fig2.Add(new PathSegment(SegmentType.Line, w * .26, h * .44));
        fig2.Add(new PathSegment(SegmentType.Line, w * .5, h * .66));
        fig2.Add(new PathSegment(SegmentType.Line, w * (1 - .26), h * .44));
        fig2.Add(new PathSegment(SegmentType.Line, w * .60, h * .44));
        fig2.Add(new PathSegment(SegmentType.Line, w * .60, h * 0).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("Bin", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(w * 0, h * 1, true);
        geo.Add(fig);

        var third = .1 / .3;  // just to keep values consistent
        // outer frame
        // starts bottom left
        fig.Add(new PathSegment(SegmentType.Line, w * 1, h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * 1, h * (1 - third)));
        fig.Add(new PathSegment(SegmentType.Line, w * .8, h * 0));
        fig.Add(new PathSegment(SegmentType.Line, w * .66, h * 0));
        fig.Add(new PathSegment(SegmentType.Line, w * .66, h * .055));
        fig.Add(new PathSegment(SegmentType.Line, w * .755, h * .055));
        fig.Add(new PathSegment(SegmentType.Line, w * .93, h * (1 - third)));
        fig.Add(new PathSegment(SegmentType.Line, w * .64, h * (1 - third)));
        fig.Add(new PathSegment(SegmentType.Line, w * .61, h * .75));
        fig.Add(new PathSegment(SegmentType.Line, w * .5, h * .75));
        fig.Add(new PathSegment(SegmentType.Line, w * .39, h * .75));
        fig.Add(new PathSegment(SegmentType.Line, w * .36, h * (1 - third)));
        fig.Add(new PathSegment(SegmentType.Line, w * .07, h * (1 - third)));
        fig.Add(new PathSegment(SegmentType.Line, w * (1 - .755), h * (.055)));
        fig.Add(new PathSegment(SegmentType.Line, w * (1 - .66), h * (.055)));
        fig.Add(new PathSegment(SegmentType.Line, w * (1 - .66), h * (0)));
        fig.Add(new PathSegment(SegmentType.Line, w * (1 - .8), h * (0)));
        fig.Add(new PathSegment(SegmentType.Line, w * 0, h * (1 - third)).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("Upload", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(w * 0, h * 1, true);
        geo.Add(fig);

        var third = .1 / .3;  // just to keep values consistent
        // outer frame
        // starts bottom left
        fig.Add(new PathSegment(SegmentType.Line, w * 1, h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * 1, h * (1 - third)));
        fig.Add(new PathSegment(SegmentType.Line, w * .8, h * 0));
        fig.Add(new PathSegment(SegmentType.Line, w * .66, h * 0));
        fig.Add(new PathSegment(SegmentType.Line, w * .66, h * .055));
        fig.Add(new PathSegment(SegmentType.Line, w * .755, h * .055));
        fig.Add(new PathSegment(SegmentType.Line, w * .93, h * (1 - third)));
        fig.Add(new PathSegment(SegmentType.Line, w * .64, h * (1 - third)));
        fig.Add(new PathSegment(SegmentType.Line, w * .61, h * .75));
        fig.Add(new PathSegment(SegmentType.Line, w * .5, h * .75));
        fig.Add(new PathSegment(SegmentType.Line, w * .39, h * .75));
        fig.Add(new PathSegment(SegmentType.Line, w * .36, h * (1 - third)));
        fig.Add(new PathSegment(SegmentType.Line, w * .07, h * (1 - third)));
        fig.Add(new PathSegment(SegmentType.Line, w * (1 - .755), h * (.055)));
        fig.Add(new PathSegment(SegmentType.Line, w * (1 - .66), h * (.055)));
        fig.Add(new PathSegment(SegmentType.Line, w * (1 - .66), h * (0)));
        fig.Add(new PathSegment(SegmentType.Line, w * (1 - .8), h * (0)));
        fig.Add(new PathSegment(SegmentType.Line, w * 0, h * (1 - third)).Close());
        var fig2 = new PathFigure(w * .5, h * 0, true);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, w * .26, h * .25));
        fig2.Add(new PathSegment(SegmentType.Line, w * .40, h * .25));
        fig2.Add(new PathSegment(SegmentType.Line, w * .40, h * .63));
        fig2.Add(new PathSegment(SegmentType.Line, w * .60, h * .63));
        fig2.Add(new PathSegment(SegmentType.Line, w * .60, h * .25));
        fig2.Add(new PathSegment(SegmentType.Line, w * .74, h * .25).Close());

        return geo;
      });

      Shape.DefineFigureGenerator("EmptyDrink", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(w * .15, h * 0, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w * .85, h * 0));
        fig.Add(new PathSegment(SegmentType.Line, w * .70, h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * .30, h * 1).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("Drink", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(w * .15, h * 0, false);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w * .85, h * 0));
        fig.Add(new PathSegment(SegmentType.Line, w * .70, h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * .30, h * 1).Close());
        var fig2 = new PathFigure(w * .235, h * .28, true);
        geo.Add(fig2);
        fig2.Add(new PathSegment(SegmentType.Line, w * .765, h * .28));
        fig2.Add(new PathSegment(SegmentType.Line, w * .655, h * .97));
        fig2.Add(new PathSegment(SegmentType.Line, w * .345, h * .97).Close());

        return geo;
      });

      Shape.DefineFigureGenerator("4Arrows", (shape, w, h) => {
        var geo = new Geometry();
        var fig = new PathFigure(w * .5, h * 0, true);
        geo.Add(fig);

        fig.Add(new PathSegment(SegmentType.Line, w * .65, h * .25));
        fig.Add(new PathSegment(SegmentType.Line, w * .55, h * .25));
        fig.Add(new PathSegment(SegmentType.Line, w * .55, h * .45));
        fig.Add(new PathSegment(SegmentType.Line, w * .75, h * .45));
        fig.Add(new PathSegment(SegmentType.Line, w * .75, h * .35));
        fig.Add(new PathSegment(SegmentType.Line, w * 1, h * .5));
        fig.Add(new PathSegment(SegmentType.Line, w * .75, h * .65));
        fig.Add(new PathSegment(SegmentType.Line, w * .75, h * .55));
        fig.Add(new PathSegment(SegmentType.Line, w * .55, h * .55));
        fig.Add(new PathSegment(SegmentType.Line, w * .55, h * .75));
        fig.Add(new PathSegment(SegmentType.Line, w * .65, h * .75));
        fig.Add(new PathSegment(SegmentType.Line, w * .5, h * 1));
        fig.Add(new PathSegment(SegmentType.Line, w * .35, h * .75));
        fig.Add(new PathSegment(SegmentType.Line, w * .45, h * .75));
        fig.Add(new PathSegment(SegmentType.Line, w * .45, h * .55));
        fig.Add(new PathSegment(SegmentType.Line, w * .25, h * .55));
        fig.Add(new PathSegment(SegmentType.Line, w * .25, h * .65));
        fig.Add(new PathSegment(SegmentType.Line, w * 0, h * .5));
        fig.Add(new PathSegment(SegmentType.Line, w * .25, h * .35));
        fig.Add(new PathSegment(SegmentType.Line, w * .25, h * .45));
        fig.Add(new PathSegment(SegmentType.Line, w * .45, h * .45));
        fig.Add(new PathSegment(SegmentType.Line, w * .45, h * .25));
        fig.Add(new PathSegment(SegmentType.Line, w * .35, h * .25).Close());
        return geo;
      });

      Shape.DefineFigureGenerator("Connector", "Ellipse");
      Shape.DefineFigureGenerator("Alternative", "TriangleUp");
      Shape.DefineFigureGenerator("Merge", "TriangleUp");
      Shape.DefineFigureGenerator("Decision", "Diamond");
      Shape.DefineFigureGenerator("DataTransmissions", "Hexagon");
      Shape.DefineFigureGenerator("Gate", "Crescent");
      Shape.DefineFigureGenerator("Delay", "HalfEllipse");
      Shape.DefineFigureGenerator("Input", "Parallelogram1");
      Shape.DefineFigureGenerator("ManualLoop", "ManualOperation");
      Shape.DefineFigureGenerator("ISOProcess", "Chevron");
      Shape.DefineFigureGenerator("MessageToUser", "SquareArrow");
      Shape.DefineFigureGenerator("MagneticData", "Cylinder1");
      Shape.DefineFigureGenerator("DirectData", "Cylinder4");
      Shape.DefineFigureGenerator("StoredData", "DataStorage");
      Shape.DefineFigureGenerator("SequentialData", "MagneticTape");
      Shape.DefineFigureGenerator("Subroutine", "Procedure");
    }
  }
}
