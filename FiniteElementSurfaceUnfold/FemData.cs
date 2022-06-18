using System;
using System.Collections.Generic;
using System.Text;
using g3;

namespace FiniteElementSurfaceUnfold
{
    public class FemData
    {
        public FemData(int nd, int nz, ICaseInfo info)
        {
            nDelta = nd;
            nZ = nz;
            GetDeltaLength = info.GetDeltaLength;
            GetZLength = info.GetZLength;
            GetAngle = info.GetAngle;
            GetULength = info.GetULength;
            GetVLength = info.GetVLength;
        }

        // unfolded coords (X,Y) ~ original coords (delta, Z)
        public int nDelta; // X ~ delta
        public int nZ; // Y ~ Z

        public int TotalElements { get => nDelta * nZ; }
        public int OutlinePointsCount { get => (nDelta + nZ) * 2; }

        // f(Delta, Z)
        public Func<SurfaceParameters, double, double, double> GetDeltaLength; //Full length along Delta in folded space
        public Func<SurfaceParameters, double, double, double> GetZLength; //Full length along Z in folded space
        public Func<SurfaceParameters, double, double, double> GetAngle; //Angle between the isolines to be preserved
        public Func<SurfaceParameters, double, double, double> GetULength; //Full length along U in unfolded space
        public Func<SurfaceParameters, double, double, double> GetVLength; //Full length along V in unfolded space

        //Finite step along Delta in folded space
        public double DeltaStep(SurfaceParameters p, Vector2d v)
        {
            return DeltaStep(p, v.x, v.y);
        }
        public double DeltaStep(SurfaceParameters p, double delta, double z)
        {
            return GetDeltaLength(p, delta, z) / nDelta;
        }
        //Finite step along Z in folded space
        public double ZStep(SurfaceParameters p, Vector2d v)
        {
            return ZStep(p, v.x, v.y);
        }
        public double ZStep(SurfaceParameters p, double delta, double z)
        {
            return GetZLength(p, delta, z) / nZ;
        }
        //Finite step along U in unfolded space
        public double UStep(SurfaceParameters p, Vector2d v)
        {
            return UStep(p, v.x, v.y);
        }
        public double UStep(SurfaceParameters p, double delta, double z)
        {
            return GetULength(p, delta, z) / nDelta;
        }
        //Finite step along V in unfolded space
        public double VStep(SurfaceParameters p, Vector2d v)
        {
            return VStep(p, v.x, v.y);
        }
        public double VStep(SurfaceParameters p, double delta, double z)
        {
            return GetVLength(p, delta, z) / nZ;
        }

        public double AngleAt(SurfaceParameters p, Vector2d r)
        {
            return GetAngle(p, r.x, r.y);
        }
    }
}
