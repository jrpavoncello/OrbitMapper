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
        public Kite()
        {
            /// <summary>
            /// Use the base class to configure this child specifically for a kite laying on one side spanned horizontally.
            /// The leftmost angle is 60, the two identical angles are 90 and the remaining is 120
            /// </summary>
            base.Text = "Kite";
            base.Name = "Kite" + (base.getShapeCount() - 1);
            addVertex(0, 0, 0);
            addVertex(Math.Cos(Math.PI / 3) * 1024, Math.Sin(Math.PI / 3) * 1024, 60d);
            addVertex(1024d, Math.Tan(Math.PI / 6) * 1024, 330d);
            addVertex(1024d, 0d, 90d);
        }
    }
}
