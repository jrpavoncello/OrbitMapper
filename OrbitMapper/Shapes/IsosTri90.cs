using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace OrbitMapper.Shapes
{
    public class IsosTri90 : Shape
    {
        /// <summary>
        /// Use the base class to configure this child specifically for a 90 isosceles triangle
        /// </summary>
        public IsosTri90()
        {
            base.Text = "IsosTri90";
            base.Name = "IsosTri90" + (base.getShapeCount() - 1);
            base.addVertex(0, 0, 0);
            double temp = Math.Tan(Math.PI / 4) * (512d);
            base.addVertex(512d, temp, 45d);
            base.addVertex(1024d, 0, 135d);
            base.setStartArea(0d, 1024);
        }
    }
}