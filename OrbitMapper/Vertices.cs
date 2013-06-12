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
        /// <summary>
        /// Initilize empty lists
        /// </summary>
        public Vertices()
        {
            x1 = new List<double>();
            x2 = new List<double>();
        }
        /// <summary>
        /// Call the default and add the two parameters
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        public Vertices(double x1, double x2) : this()
        {
            this.x1.Add(x1);
            this.x2.Add(x2);
        }
        /// <summary>
        /// Add a vertex only
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
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
        /// <summary>
        /// return the size of this list of vertices
        /// </summary>
        /// <returns></returns>
        public int size()
        {
            return x1.Count;
        }
    }
}