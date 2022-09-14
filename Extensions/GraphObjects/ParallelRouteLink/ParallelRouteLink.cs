/*
*  Copyright (C) 1998-2022 by Northwoods Software Corporation. All Rights Reserved.
*/

/*
* This is an extension and not part of the main GoDiagram library.
* Note that the API for this class may change with any version, even point releases.
* If you intend to use an extension in production, you should copy the code to your own source directory.
* Extensions can be found in the GoDiagram repository (https://github.com/NorthwoodsSoftware/GoDiagram/tree/main/Extensions).
* See the Extensions intro page (https://godiagram.com/intro/extensions.html) for more information.
*/

using System;

namespace Northwoods.Go.Extensions {
  /// <summary>
  /// This custom <see cref="Link"/> class customizes its route to go parallel to other links
  /// conntecting the same ports, if the link is not orthogonal and is not Bezier curved.
  /// </summary>
  public class ParallelRouteLink : Link {
    /// <summary>
    /// Constructs the link's route by modifying points.
    /// </summary>
    /// <returns>true if it computed a route of points</returns>
    public override bool ComputePoints() {
      var result = base.ComputePoints();

      if (!IsOrthogonal && Curve != LinkCurve.Bezier && HasCurviness()) {
        var curv = ComputeCurviness();
        if (curv != 0) {
          var num = PointsCount;
          var pidx = 0;
          var qidx = num - 1;
          if (num >= 4) {
            pidx++;
            qidx--;
          }

          var frompt = GetPoint(pidx);
          var topt = GetPoint(qidx);
          var dx = topt.X - frompt.X;
          var dy = topt.Y - frompt.Y;

          var mx = frompt.X + dx / 8;
          var my = frompt.Y + dy / 8;
          var px = mx;
          var py = my;
          if (-0.01 < dy && dy < 0.01) {
            if (dx > 0) py -= curv; else py += curv;
          } else {
            var slope = -dx / dy;
            var e = Math.Sqrt(curv * curv / (slope * slope + 1));

            if (curv < 0) e = -e;
            px = (dy < 0 ? -1 : 1) * e + mx;
            py = slope * (px - mx) + my;
          }

          mx = frompt.X + dx * 7 / 8;
          my = frompt.Y + dy * 7 / 8;
          var qx = mx;
          var qy = my;
          if (-0.01 < dy && dy < 0.01) {
            if (dx > 0) qy -= curv; else qy += curv;
          } else {
            var slope = -dx / dy;
            var e = Math.Sqrt(curv * curv / (slope * slope + 1));
            if (curv < 0) e = -e;
            qx = (dy < 0 ? -1 : 1) * e + mx;
            qy = slope * (qx - mx) + my;
          }

          InsertPoint(pidx + 1, px, py);
          InsertPoint(qidx + 1, qx, qy);
        }
      }

      return result;
    }
  }
}
