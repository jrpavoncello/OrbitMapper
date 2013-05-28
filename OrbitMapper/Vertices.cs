using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OrbitMapper
{
    public class Vertices
    {
        private List<double> x1;
        private List<double> x2;
        private List<doublePoint> vertices;
        public Vertices()
        {
            x1 = new List<double>();
            x2 = new List<double>();
            vertices = new List<doublePoint>();
        }
        public Vertices(double x1, double x2)
        {
            this.x1 = new List<double>();
            this.x2 = new List<double>();
            this.x1.Add(x1);
            this.x2.Add(x2);
            vertices = new List<doublePoint>();
            this.vertices.Add(new doublePoint(x1, x2));
        }
        public void addVertex(double x1, double x2)
        {
            this.x1.Add(x1);
            this.x2.Add(x2);
            this.vertices.Add(new doublePoint(x1, x2));
        }
        public doublePoint[] getVertices()
        {
            return vertices.ToArray<doublePoint>();
        }
        public doublePoint pointAt(int index)
        {
            return new doublePoint(x1.ElementAt<double>(index), x2.ElementAt<double>(index));
        }
        public int size()
        {
            return x1.Count;
        }
    }
}
