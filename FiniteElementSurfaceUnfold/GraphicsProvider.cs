using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using g3;
using System.Linq;
using SixLabors.ImageSharp.Drawing;
using System.Numerics;
using MoreLinq;

namespace FiniteElementSurfaceUnfold
{
    public static class GraphicsProvider
    {
        public static int IsolineCount { get; set; } = 10;

        private static PointF ToPointF(this Vector2f v)
        {
            return new PointF(v.x, v.y);
        }
        private static IEnumerable<PointF> ToPointF(this IEnumerable<Vector2d> v)
        {
            return v.Select(x => new PointF((float)x.x, (float)x.y));
        }
        private static IEnumerable<PointF> ToPointF(this IEnumerable<Vector2f> v)
        {
            return v.Select(x => x.ToPointF());
        }

        private static Image Engine(IEnumerable<Vector2f> points, Action<PathBuilder, Polygon2d, List<IPath>> a)
        {
            var c = new ConvexHull2(points.Select(x => (Vector2d)x).ToList(), float.Epsilon, QueryNumberType.QT_DOUBLE);
            var p = c.GetHullPolygon();
            var res = new Image<Rgb24>((int)Math.Ceiling(p.Bounds.Width * 1.1), (int)Math.Ceiling(p.Bounds.Height * 1.1));
            PathBuilder b = new PathBuilder(Matrix3x2.CreateTranslation(
                (float)(p.Bounds.Width * 0.05 - p.Bounds.Min.x), (float)(p.Bounds.Height * 0.05 - p.Bounds.Min.y)));
            List<IPath> paths = new List<IPath>();
            a(b, p, paths);
            foreach (var item in paths)
            {
                res.Mutate(x => x.Draw(Pens.Solid(Color.Green, 2), item));
            }
            res.Mutate(x => x.Flip(FlipMode.Vertical));
            return res;
        }

        public static Image DrawHull(IEnumerable<Vector2f> points)
        {
            return Engine(points, (b, p, paths) => { 
                b.AddLines(p.VerticesItr(true).ToPointF());
                paths.Add(b.Build());
            });
        }

        public static Image DrawAll(UnfoldElement[][] elements)
        {
            return Engine(elements.Flatten().Cast<UnfoldElement>().Select(x => x.Unfolded.r0), (b, p, paths) => {
                var div = elements.Length / IsolineCount;
                bool dir = true;
                for (int i = 0; i < elements.Length; i++)
                {
                    if ((i % div != 0) && (i != elements.Length - 1)) continue;
                    var pts = elements[i].Select(x => x.Unfolded.r0).ToPointF();
                    b.AddLines(dir ? pts : pts.Reverse());
                    dir = !dir;
                }
                paths.Add(b.Build());
                b.Reset();
                int l = elements.First().Length;
                div = l / IsolineCount;
                dir = true;
                for (int i = 0; i < l; i++)
                {
                    if ((i % div != 0) && (i != l - 1)) continue;
                    for (int j = dir ? 1 : elements.Length - 1; dir ? (j < elements.Length) : (j > 0); )
                    {
                        var v1 = elements[j - 1][i].Unfolded.r0;
                        var v2 = elements[j][i].Unfolded.r0;
                        b.AddLine(v1.ToPointF(), v2.ToPointF());
                        if (dir) j++; else j--;
                    }
                    dir = !dir;
                }
                paths.Add(b.Build());
            });
        }
    }
}
