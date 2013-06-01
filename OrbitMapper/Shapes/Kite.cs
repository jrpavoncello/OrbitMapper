using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace OrbitMapper
{
    public class Kite : Shape
    {
        /// <summary>
        /// Use the base class to configure this child specifically for a kite laying on one side spanned horizontally.
        /// The leftmost angle is 60, the two identical angles are 90 and the remaining is 120
        /// </summary>
        public Kite()
        {
            base.Text = "Kite";
            base.Name = "Kite" + (base.getShapeCount() - 1);
            base.addVertex(0, 0, 0);
            base.addVertex(Math.Cos(Math.PI / 3) * 1024, Math.Sin(Math.PI / 3) * 1024, 60d);
            base.addVertex(1024d, Math.Tan(Math.PI / 6) * 1024, 330d);
            base.addVertex(1024d, 0d, 90d);
            base.setStartArea(0, 1024d);
        }
    }
}
