using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace OrbitMapper
{
    public class IsosTri120 : Shape
    {
        /// <summary>
        /// Use the base class to configure this child specifically for a 120 isosceles triangle
        /// </summary>
        public IsosTri120(){
            base.Text = "IsosTri120";
            base.Name = "IsosTri120" + (base.getShapeCount() - 1);
            addVertex(0, 0, 0);
            double temp = Math.Tan(Math.PI / 6) * (512d);
            addVertex(512d, temp, 30d);
            addVertex(1024d, 0, 150d);
            setScale(1024d, temp);
            setStartArea(0d, 1024);
        }
    }
}
