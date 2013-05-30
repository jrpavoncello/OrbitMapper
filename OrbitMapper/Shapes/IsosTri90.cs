using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace OrbitMapper
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
            addVertex(0, 0, 0);
            double temp = Math.Tan(Math.PI / 4) * (512d);
            addVertex(512d, temp, 45d);
            addVertex(1024d, 0, 135d);
            setScale(1024d, temp);
            setStartArea(0d, 1024);
        }
    }
}