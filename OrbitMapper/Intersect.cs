using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrbitMapper
{
    /// <summary>
    /// Quick and easy class for storing Intersection data, could have been a struct too
    /// </summary>
    public class Intersect
    {
        /// <summary>
        /// X
        /// </summary>
        public double x1;
        /// <summary>
        /// Y
        /// </summary>
        public double x2;
        /// <summary>
        /// Angle in degrees
        /// </summary>
        public double angle;
        /// <summary>
        /// Distance in pixels
        /// </summary>
        public double distance;
        /// <summary>
        /// A wall or face the the intersect occured on
        /// </summary>
        public int wall;
        /// <summary>
        /// Initialize all values to zero
        /// </summary>
        public Intersect()
        {
            x1 = 0;
            x2 = 0;
            angle = 0;
            distance = 0;
            wall = 0;
        }
    }
}