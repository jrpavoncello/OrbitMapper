using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace OrbitMapper.Shapes
{
    public class Rect : Shape
    {
        /// <summary>
        /// Use the base class to configure this child specifically for a rectangle (could also be a square).
        /// </summary>
        public Rect(double ratio)
        {
            base.Text = "Rectangle";
            base.Name = "Rectangle" + (base.getShapeCount() - 1);
            base.setRatio(ratio);
            double heightRatio = ratio;
            double height = 1024 * heightRatio;
            double width = 1024;
            base.addVertex(0, 0, 0);
            base.addVertex(0, height, 90);
            base.addVertex(width, height, 0);
            base.addVertex(width, 0, 90);
            base.setStartArea(0, width);
        }
    }
}
