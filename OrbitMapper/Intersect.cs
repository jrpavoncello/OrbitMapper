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
        public double x1;
        public double x2;
        public double angle;
        public double distance;
        public int wall;
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