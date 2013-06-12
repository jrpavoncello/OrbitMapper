using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OrbitMapper
{
    /// <summary>
    /// The Pattern class is specifically used to contain a structured way to house the vertices that are used to draw the tessellation patterns.
    /// The assumption with Patterns is that each Pattern instance contains the necessary vertices in order necessary to draw a shape in constant iterations.
    /// <seealso cref="Pattern.getPatterns"/> <seealso cref="Pattern.addPattern"/>
    /// </summary>
    public class Pattern
    {
        private List<List<DoublePoint>> vertices;
        private double width = 0;
        private double height = 0;
        private double offset = 0;
        public int iWidth { get; set; }
        public int iHeight { get; set; }
        public int iOffset { get; set; }
        public int fullWidth { get; set; }
        public int fullHeight { get; set; }

        public Pattern(double width, double height, double offset)
        {
            vertices = new List<List<DoublePoint>>();
            this.width = width;
            this.height = height;
            this.offset = offset;
            this.iWidth = (int)width;
            this.iHeight = (int)height;
            this.iOffset = (int)offset;
        }

        public Pattern()
        {
            vertices = new List<List<DoublePoint>>();
        }


        public void addVertex(double x1, double x2, int patternNum){
            vertices.ElementAt<List<DoublePoint>>(patternNum).Add(new DoublePoint(x1, x2));
        }

        /// <summary>
        /// Add pattern assumes that the List pattern is in the correct order to be able to be directly fed to a Graphics.DrawPoly() method for drawing.
        /// I try to keep the number of patterns down and reduce the amount of pathing that requires a double-back to draw shapes, but is not necessary
        /// </summary>
        /// <param name="pattern"></param>
        public void addPattern(List<DoublePoint> pattern){
            vertices.Add(pattern);
        }

        /// <summary>
        /// This returns a jagged array (array of pointers to instances of Point[]). It is designed to be iterated over each Point[i] and fed to a Graphics.DrawPoly().
        /// The way I use it I never have more than a single pattern anyways, although there is support for using multiple patterns instead of just adding a ton of 
        /// vertices to a single pattern.
        /// </summary>
        /// <returns></returns>
        public Point[][] getPatterns(){
            int len1 = vertices.Count; //Get the number of patterns added
            Point[][] temp = new Point[len1][]; //Tell the Point[][] jagged array instance how many pointer to expect in the first index
            for(int i = 0; i < len1; i++){
                int len2 = vertices.ElementAt<List<DoublePoint>>(i).Count(); //Get how many vertices are at the current pattern
                temp[i] = new Point[len2]; //Instantiate the Point array to fill it
                for(int j = 0; j < vertices.ElementAt<List<DoublePoint>>(i).Count(); j++){ // Iterate over each vertex
                    temp[i][j] = new Point((int)vertices.ElementAt<List<DoublePoint>>(i).ElementAt<DoublePoint>(j).x1, (int)vertices.ElementAt<List<DoublePoint>>(i).ElementAt<DoublePoint>(j).x2); // Add each vertex of that pattern to that Poitn array
                }
            }
            return temp;
        }
    }
}
