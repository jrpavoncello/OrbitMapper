using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;

namespace OrbitMapper
{
    public class Rect : Shape
    {
        public Rect(double ratio)
        {
            base.Text = "Rectangle";
            base.Name = "Rectangle" + (base.getShapeCount() - 1);
            base.setTabCount(base.getTabCount() + 1);
            base.setRatio(ratio);
            double heightRatio = ratio;
            double widthRatio = 1 / ratio;
            double height = 1024 * heightRatio;
            double width = 1024 * widthRatio;
            addVertex(0, 0, 0);
            addVertex(0, height, 90);
            addVertex(width, height, 0);
            addVertex(width, 0, 90);
            setScale(width, height);
            setStartArea(0, width);
        }
    }
}
