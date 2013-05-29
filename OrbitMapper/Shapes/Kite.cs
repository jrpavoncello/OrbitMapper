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
            base.Text = "Kite";
            base.Name = "Kite" + (base.getShapeCount() - 1);
            addVertex(0, 0, 0);
            addVertex(Math.Cos(Math.PI / 3) * 1024, Math.Sin(Math.PI / 3) * 1024, 60d);
            addVertex(1024d, Math.Tan(Math.PI / 6) * 1024, 330d);
            addVertex(1024d, 0d, 90d);
            setScale(1024d, Math.Sin(Math.PI / 3) * 1024);
            setStartArea(0d, 1024d);
        }
    }
}
