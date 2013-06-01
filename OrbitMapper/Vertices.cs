using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OrbitMapper
{
    /// <summary>
    /// This is used as a precise and easy way to store and return vertices, whether used for a shape or for the collisions.
    /// </summary>
    public class Vertices
    {
        private List<double> x1;
        private List<double> x2;
        public Vertices()
        {
            x1 = new List<double>();
            x2 = new List<double>();
        }
        public Vertices(double x1, double x2)
        {
            this.x1 = new List<double>();
            this.x2 = new List<double>();
            this.x1.Add(x1);
            this.x2.Add(x2);
        }
        public void addVertex(double x1, double x2)
        {
            this.x1.Add(x1);
            this.x2.Add(x2);
        }
        /// <summary>
        /// Get a specific vertex
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DoublePoint pointAt(int index)
        {
            return new DoublePoint(x1.ElementAt<double>(index), x2.ElementAt<double>(index));
        }
        public int size()
        {
            return x1.Count;
        }
    }
}