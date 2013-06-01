using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace OrbitMapper.Shapes
{
    public class IsosTri120 : Shape
    {
        /// <summary>
        /// Use the base class to configure this child specifically for a 120 isosceles triangle
        /// </summary>
        public IsosTri120(){
            base.Text = "IsosTri120";
            base.Name = "IsosTri120" + (base.getShapeCount() - 1);
            base.addVertex(0, 0, 0);
            double temp = Math.Tan(Math.PI / 6) * (512d);
            base.addVertex(512d, temp, 30d);
            base.addVertex(1024d, 0, 150d);
            base.setStartArea(0d, 1024);
        }
    }
}
