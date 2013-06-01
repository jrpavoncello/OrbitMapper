using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrbitMapper
{
    public class DoublePoint
    {

        public double x1;
        public double x2;
        public DoublePoint()
        {
            x1 = 0;
            x2 = 0;
        }
        public DoublePoint(double x1, double x2)
        {
            this.x1 = x1;
            this.x2 = x2;
        }
    }
}
