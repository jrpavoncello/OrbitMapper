using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrbitMapper
{
    /// <summary>
    /// Only designed to be used just like a Point, but with more precision.
    /// </summary>
    public class DoublePoint
    {
        public double x1;
        public double x2;
        public DoublePoint(double x1 = 0, double x2 = 0)
        {
            this.x1 = x1;
            this.x2 = x2;
        }
    }
}
