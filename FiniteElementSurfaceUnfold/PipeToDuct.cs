using System;
using System.Collections.Generic;
using System.Text;

namespace FiniteElementSurfaceUnfold
{
    public class PipeToDuct : ICaseInfo
    {
        public class MySurfaceParams : SurfaceParameters
        {
            public MySurfaceParams(double h, double w, double zTotal, double d)
            {
                hh = h / 2;
                ww = w / 2;
                zz = zTotal;
                R0 = d / 2;
            }

            public double hh;
            public double ww;
            public double zz;
            public double R0;
        }

        public double GetAngle(SurfaceParameters p, double delta, double z)
        {
            var pp = (MySurfaceParams)p;
            if (z >= pp.zz) z = 0.999999 * pp.zz;
            return Math.Acos(((2 * pp.hh * pp.R0 * Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2)) * (z - pp.zz) + 2 * Math.Pow(pp.hh, 2) * pp.R0 * (-z + pp.zz) + Math.Pow(pp.ww, 2) * (-2 * pp.R0 * z + Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2)) * z + 2 * pp.R0 * pp.zz)) * (-delta + pp.ww - pp.R0 * Math.Cos((delta * Math.Acos(pp.hh / Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2)))) / pp.ww + Math.Atan(pp.hh / pp.ww)))) / (pp.R0 * (-pp.hh + Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2))) * (z - pp.zz) * Math.Sqrt((Math.Pow(pp.zz, 2) + Math.Pow(delta - pp.ww + pp.R0 * Math.Cos((delta * Math.Acos(pp.hh / Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2)))) / pp.ww + Math.Atan(pp.hh / pp.ww)), 2) + Math.Pow(pp.hh - pp.R0 * Math.Sin((delta * Math.Acos(pp.hh / Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2)))) / pp.ww + Math.Atan(pp.hh / pp.ww)), 2)) * (4 * Math.Pow(delta - pp.ww + pp.R0 * Math.Cos((delta * Math.Acos(pp.hh / Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2)))) / pp.ww + Math.Atan(pp.hh / pp.ww)), 2) + Math.Pow(Math.Pow(pp.ww, 2) * (-2 * pp.R0 * z + Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2)) * z + 2 * pp.R0 * pp.zz) + 2 * Math.Pow(pp.R0, 2) * (-pp.hh + Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2))) * (z - pp.zz) * Math.Sin((delta * Math.Acos(pp.hh / Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2)))) / pp.ww + Math.Atan(pp.hh / pp.ww)), 2) / (Math.Pow(pp.R0, 2) * Math.Pow(pp.hh - Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2)), 2) * Math.Pow(z - pp.zz, 2))))));
        }

        public double GetDeltaLength(SurfaceParameters p, double delta, double z)
        {
            var pp = (MySurfaceParams)p;
            return pp.ww;
        }
        public double GetZLength(SurfaceParameters p, double delta, double z)
        {
            var pp = (MySurfaceParams)p;
            return pp.zz;
        }
        public double GetULength(SurfaceParameters p, double delta, double z)
        {
            var pp = (MySurfaceParams)p;
            if (z >= pp.zz) return pp.ww;
            return -(Math.Abs(Math.Pow(pp.ww, 2) * Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2)) * Math.Pow(z, 2) - 2 * pp.hh * Math.Pow(pp.R0, 2) * Math.Pow(z - pp.zz, 2) + 2 * Math.Pow(pp.R0, 2) * Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2)) * Math.Pow(z - pp.zz, 2) + 2 * pp.R0 * Math.Pow(pp.ww, 2) * z * (-z + pp.zz)) * Math.Acos(((Math.Pow(pp.ww, 2) * Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2)) * Math.Pow(z, 2) - 2 * pp.hh * Math.Pow(pp.R0, 2) * Math.Pow(z - pp.zz, 2) + 2 * Math.Pow(pp.R0, 2) * Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2)) * Math.Pow(z - pp.zz, 2) + 2 * pp.R0 * Math.Pow(pp.ww, 2) * z * (-z + pp.zz)) * (Math.Pow(pp.hh, 2) * (Math.Pow(pp.ww, 2) * Math.Pow(z, 2) - 2 * Math.Pow(pp.R0, 2) * Math.Pow(z - pp.zz, 2)) + 2 * pp.hh * Math.Pow(pp.R0, 2) * Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2)) * Math.Pow(z - pp.zz, 2) + Math.Pow(pp.ww, 2) * z * (Math.Pow(pp.ww, 2) * z + 2 * pp.R0 * Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2)) * (-z + pp.zz)))) / (Math.Sqrt((Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2)) * (Math.Pow(pp.hh, 2) * (Math.Pow(pp.ww, 4) * Math.Pow(z, 4) + 4 * Math.Pow(pp.R0, 2) * Math.Pow(pp.ww, 2) * Math.Pow(z, 2) * Math.Pow(z - pp.zz, 2) + 8 * Math.Pow(pp.R0, 4) * Math.Pow(z - pp.zz, 4)) - 4 * pp.hh * Math.Pow(pp.R0, 2) * Math.Pow(z - pp.zz, 2) * (Math.Pow(pp.ww, 2) * Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2)) * Math.Pow(z, 2) + 2 * Math.Pow(pp.R0, 2) * Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2)) * Math.Pow(z - pp.zz, 2) + 2 * pp.R0 * Math.Pow(pp.ww, 2) * z * (-z + pp.zz)) + Math.Pow(pp.ww, 2) * (Math.Pow(pp.ww, 4) * Math.Pow(z, 4) + 8 * Math.Pow(pp.R0, 2) * Math.Pow(pp.ww, 2) * Math.Pow(z, 2) * Math.Pow(z - pp.zz, 2) - 8 * Math.Pow(pp.R0, 3) * Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2)) * z * Math.Pow(z - pp.zz, 3) + 4 * Math.Pow(pp.R0, 4) * Math.Pow(z - pp.zz, 4) + 4 * pp.R0 * Math.Pow(pp.ww, 2) * Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2)) * Math.Pow(z, 3) * (-z + pp.zz)))) * Math.Abs(Math.Pow(pp.ww, 2) * Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2)) * Math.Pow(z, 2) - 2 * pp.hh * Math.Pow(pp.R0, 2) * Math.Pow(z - pp.zz, 2) + 2 * Math.Pow(pp.R0, 2) * Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2)) * Math.Pow(z - pp.zz, 2) + 2 * pp.R0 * Math.Pow(pp.ww, 2) * z * (-z + pp.zz))))) / (2 * pp.R0 * (-pp.hh + Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2))) * (z - pp.zz) * pp.zz);
        }
        public double GetVLength(SurfaceParameters p, double delta, double z)
        {
            var pp = (MySurfaceParams)p;
            return Math.Sqrt(Math.Pow(pp.zz, 2) + Math.Pow(delta - pp.ww + pp.R0 * Math.Cos((delta * Math.Acos(pp.hh / Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2)))) / pp.ww + Math.Atan(pp.hh / pp.ww)), 2) + Math.Pow(pp.hh - pp.R0 * Math.Sin((delta * Math.Acos(pp.hh / Math.Sqrt(Math.Pow(pp.hh, 2) + Math.Pow(pp.ww, 2)))) / pp.ww + Math.Atan(pp.hh / pp.ww)), 2));
        }
    }
}
