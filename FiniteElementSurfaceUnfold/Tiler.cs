using System;
using System.Collections.Generic;
using System.Text;
using g3;

namespace FiniteElementSurfaceUnfold
{
    public class Tiler
    {
        public Tiler(FemData config)
        {
            Configuration = config;
        }

        public event EventHandler<float> ProgressChanged;

        public FemData Configuration { get; set; }
        public UnfoldElement[][] Rows { get; private set; }
        public List<Vector2f> Outline { get; private set; }

        /*
         * Go in rows along X (Delta), this is U direction, align the first X vector with startX 
         */
        public void Calculate(SurfaceParameters p, Vector2d startX, int rowLimit)
        {
            if (rowLimit > Configuration.nZ) rowLimit = Configuration.nZ;
            Rows = new UnfoldElement[rowLimit][];
            Outline = new List<Vector2f>(Configuration.OutlinePointsCount);
            for (int i = 0; i < rowLimit; i++)
            {
                var currentRow = new UnfoldElement[Configuration.nDelta];
                var currentElement = i == 0 ? 
                    CreateFirstElement(p, startX) : 
                    CreateNextRowElement(p, Rows[i - 1][0]);
                currentRow[0] = currentElement;
                Outline.Add(currentElement.Unfolded.r0);
                for (int j = 1; j < Configuration.nDelta; j++)
                {
                    currentElement = CreateNextColumnElement(p, currentElement);
                    currentRow[j] = currentElement;
                    if (!currentElement.Unfolded.r0.IsFinite) throw new Exception("NaN!!!");
                    if (i == 0) Outline.Add(currentElement.Unfolded.r0);
                }
                Outline.Add(currentElement.Unfolded.r0);
                Rows[i] = currentRow;
                ProgressChanged?.Invoke(this, 100f * i / rowLimit);
            }
        }

        private UnfoldElement CreateFirstElement(SurfaceParameters p, Vector2d start)
        {
            start = start.Normalized;
            var res = new UnfoldElement(new Element(), new Element());
            //Original
            res.Original.r0 = Vector2f.Zero;
            res.Original.vx0 = (Vector2f)(start * Configuration.DeltaStep(p, 0, 0));
            var angle00 = start.Rotate(Configuration.GetAngle(p, 0, 0));
            res.Original.vy0 = (Vector2f)(angle00 * Configuration.ZStep(p, 0, 0));
            var ld = res.Original.vx0.Length;
            var angle01 = start.Rotate(Configuration.GetAngle(p, ld, 0));
            res.Original.vxy = (Vector2f)(res.Original.vx0 + angle01 * Configuration.ZStep(p, ld, 0));
            //Unfold
            res.Unfolded.r0 = res.Original.r0;
            res.Unfolded.vx0 = (Vector2f)(start * Configuration.UStep(p, 0, 0));
            res.Unfolded.vy0 = (Vector2f)(angle00 * Configuration.VStep(p, 0, 0));
            res.Unfolded.vxy = (Vector2f)(res.Unfolded.vx0 + angle01 * Configuration.VStep(p, res.Original.rx0));
            return res;
        }

        private UnfoldElement CreateNextRowElement(SurfaceParameters p, UnfoldElement last)
        {
            var res = new UnfoldElement(new Element(), new Element());
            //Original
            res.Original.r0 = last.Original.ry0;
            res.Original.vx0 = last.Original.vx1;
            var lv = Configuration.ZStep(p, res.Original.r0);
            var angle0 = Configuration.AngleAt(p, res.Original.r0);
            res.Original.vy0 = (Vector2f)(res.Original.vx0.Normalized.Rotate(angle0) * lv);
            var angle1 = Configuration.AngleAt(p, res.Original.rx0);
            res.Original.vxy = (Vector2f)(res.Original.vx0 + res.Original.vx0.Normalized.Rotate(angle1)
                * Configuration.ZStep(p, res.Original.rx0));
            //Unfold
            res.Unfolded.r0 = last.Unfolded.ry0;
            res.Unfolded.vx0 = last.Unfolded.vx1;
            lv = Configuration.VStep(p, res.Original.r0);
            res.Unfolded.vy0 = (Vector2f)(res.Unfolded.vx0.Normalized.Rotate(angle0) * lv);
            res.Unfolded.vxy = (Vector2f)(res.Unfolded.vx0 + res.Unfolded.vx0.Normalized.Rotate(angle1)
                * Configuration.VStep(p, res.Original.rx0));
            return res;
        }
        private UnfoldElement CreateNextColumnElement(SurfaceParameters p, UnfoldElement last)
        {
            var res = new UnfoldElement(new Element(), new Element());
            //Original
            res.Original.r0 = last.Original.rx0;
            res.Original.vy0 = last.Original.vy1;
            var ld = Configuration.DeltaStep(p, res.Original.r0);
            var angle0 = Configuration.AngleAt(p, res.Original.r0);
            res.Original.vx0 = (Vector2f)(last.Original.vy0.Normalized.Rotate(-angle0) * ld);
            var angle1 = Configuration.AngleAt(p, res.Original.rx0);
            res.Original.vxy = (Vector2f)(res.Original.vx0 + res.Original.vx0.Normalized.Rotate(angle1) 
                * Configuration.ZStep(p, res.Original.rx0));
            if (!res.Original.vxy.IsFinite) throw new Exception("NaN!!!");
            //Unfold
            res.Unfolded.r0 = last.Unfolded.rx0;
            ld = Configuration.UStep(p, res.Unfolded.r0);
            res.Unfolded.vy0 = last.Unfolded.vy0;
            res.Unfolded.vx0 = (Vector2f)(res.Unfolded.vy0.Normalized.Rotate(-angle0) * ld);
            res.Unfolded.vxy = (Vector2f)(res.Unfolded.vx0 + res.Unfolded.vx0.Normalized.Rotate(angle1)
                * Configuration.VStep(p, res.Original.rx0));
            return res;
        }
    }
}
