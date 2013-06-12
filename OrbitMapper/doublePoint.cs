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
        /// <summary>
        /// Typically used as a double x coordinate
        /// </summary>
        public double x1;
        /// <summary>
        /// Typically used as a double y coordinate
        /// </summary>
        public double x2;
        /// <summary>
        /// Create new DoublePoint
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        public DoublePoint(double x1 = 0, double x2 = 0)
        {
            this.x1 = x1;
            this.x2 = x2;
        }
    }
}
