using System;
using System.Collections.Generic;
using System.Text;
using g3;

namespace FiniteElementSurfaceUnfold
{
    public class UnfoldElement
    {
        public UnfoldElement(Element unfolded, Element orig)
        {
            Unfolded = unfolded;
            Original = orig;
        }

        public Element Unfolded;
        public Element Original;

        public float Angle { get => Vector2f.AngleD(Unfolded.vx0, Unfolded.vy0); }
    }
}
