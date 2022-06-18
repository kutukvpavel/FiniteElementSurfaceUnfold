using System;
using System.Collections.Generic;
using System.Text;
using g3;

namespace FiniteElementSurfaceUnfold
{
    public class Element
    {
        public Vector2f r0;
        public Vector2f vx0;
        public Vector2f vy0;
        public Vector2f vxy;

        public Vector2f r1 { get => r0 + vxy; }
        public Vector2f rx0 { get => r0 + vx0; }
        public Vector2f ry0 { get => r0 + vy0; }
        public Vector2f vy1 { get => vxy - vx0; }
        public Vector2f vx1 { get => vxy - vy0; }
    }
}
