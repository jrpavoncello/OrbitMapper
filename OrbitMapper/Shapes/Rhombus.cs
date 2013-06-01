using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace OrbitMapper
{
    public class Rhombus : Shape
    {
        /// <summary>
        /// Use the base class to configure this child specifically for a 60-120 Rhombus
        /// </summary>
        public Rhombus()
        {
            base.Text = "Rhombus";
            base.Name = "Rhombus" + (base.getShapeCount() - 1);
            addVertex(0, 0, 0);
            double temp = Math.Tan(Math.PI / 3) * (256d);
            addVertex(256d, temp, 60d);
            addVertex(768d, temp, 0d);
            addVertex(512d, 0, 60d);
            setStartArea(0d, 512d);
        }
    }
}
