using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OrbitMapper
{
    public class Pattern
    {
        private List<List<doublePoint>> vertices;
        private double width = 0;
        private double height = 0;
        private double offset = 0;

        public Pattern(double width, double height, double offset)
        {
            vertices = new List<List<doublePoint>>();
            this.width = width;
            this.height = height;
            this.offset = offset;
        }
        public Pattern()
        {
            vertices = new List<List<doublePoint>>();
        }

        public double getOffset()
        {
            return offset;
        }

        public void setOffset(double offset)
        {
            this.offset = offset;
        }

        public double getWidth(){
            return width;
        }

        public double getHeight(){
            return height;
        }

        public void setWidth(double width)
        {
            this.width = width;
        }

        public void setHeight(double height)
        {
            this.height = height;
        }

        public void addVertex(double x1, double x2, int patternNum){
            vertices.ElementAt<List<doublePoint>>(patternNum).Add(new doublePoint(x1, x2));
        }

        public void addPattern(List<doublePoint> pattern){
            vertices.Add(pattern);
        }

        public Point[][] getPatterns(){
            int len1 = vertices.Count;
            Point[][] temp = new Point[len1][];
            for(int i = 0; i < len1; i++){
                int len2 = vertices.ElementAt<List<doublePoint>>(i).Count();
                temp[i] = new Point[len2];
                for(int j = 0; j < vertices.ElementAt<List<doublePoint>>(i).Count(); j++){
                    Point item = new Point((int)vertices.ElementAt<List<doublePoint>>(i).ElementAt<doublePoint>(j).x1, (int)vertices.ElementAt<List<doublePoint>>(i).ElementAt<doublePoint>(j).x2);
                    temp[i][j] = item;
                }
            }
            return temp;
        }
    }
}
