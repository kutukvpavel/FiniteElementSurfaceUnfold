using System;
using System.Collections.Generic;
using System.Text;
using g3;

namespace FiniteElementSurfaceUnfold
{
    public static class GeometryExtensions
    {
        public static Vector2d Rotate(this Vector2d v, double rad)
        {
            var m = new Matrix2d(rad);
            return m * v;
        }

        public static Vector2d Rotate(this Vector2f v, double rad)
        {
            return Rotate((Vector2d)v, rad);
        }

        public static Vector2d RotateAdjoint(this Vector2d v, double rad)
        {
            var m = new Matrix2d(Math.PI - rad);
            return m * v;
        }

        public static Vector2d RotateAdjoint(this Vector2f v, double rad)
        {
            return RotateAdjoint((Vector2d)v, rad);
        }
    }
}
