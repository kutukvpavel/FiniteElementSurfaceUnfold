using System;
using System.Collections.Generic;
using System.Text;

namespace FiniteElementSurfaceUnfold
{
    public interface ICaseInfo
    {
        public double GetAngle(SurfaceParameters p, double delta, double z);
        public double GetDeltaLength(SurfaceParameters p, double delta, double z);
        public double GetZLength(SurfaceParameters p, double delta, double z);
        public double GetULength(SurfaceParameters p, double delta, double z);
        public double GetVLength(SurfaceParameters p, double delta, double z);
    }
}
