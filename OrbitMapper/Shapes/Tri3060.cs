using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace OrbitMapper.Shapes
{
    /// <summary>
    /// Extend from Shape for a custom Tri3060
    /// </summary>
    public class Tri3060 : Shape
    {
        /// <summary>
        /// Use the base class to configure this child specifically for a 30-60-90 triangle
        /// </summary>
        public Tri3060()
        {
            base.Text = "Tri3060";
            base.Name = "Tri3060" + (base.getShapeCount() - 1);
            double temp = Math.Tan(Math.PI / 6) * (1024d);
            base.addVertex(0, 0, 0);
            base.addVertex(1024d, temp, 30d);
            base.addVertex(1024d, 0, 90d);
            base.setStartArea(0d, 1024);
        }
    }
}