using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace OrbitMapper
{
    public class Equilateral : Shape
    {
        /// <summary>
        /// Use the base class to configure this child specifically for an equilateral triangle.
        /// </summary>
        public Equilateral()
        {
            base.Text = "Equilateral";
            base.Name = "Equilateral" + (base.getShapeCount() - 1);
            addVertex(0, 0, 0);
            double temp = Math.Tan(Math.PI / 3) * (256d);
            addVertex(256d, temp, 60d);
            addVertex(512d, 0, 120d);
            setStartArea(0d, 512d);
        }
    }
}
