using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace OrbitMapper
{
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
            addVertex(0, 0, 0);
            addVertex(1024d, temp, 30d);
            addVertex(1024d, 0, 90d);
            setScale(1024d, temp);
            setStartArea(0d, 1024);
        }
    }
}