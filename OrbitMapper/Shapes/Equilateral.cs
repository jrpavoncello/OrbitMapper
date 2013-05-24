using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace OrbitMapper
{
    public class Equilateral : Shape
    {
        public Equilateral()
        {
            base.Text = "Equilateral";
            base.Name = "Equilateral" + (base.getShapeCount() - 1);
            base.setTabCount(base.getTabCount() + 1);
            addVertex(0, 0, 0);
            double temp = Math.Tan(Math.PI / 3) * (256d);
            addVertex(256d, temp, 60d);
            addVertex(512d, 0, 120d);
            setScale(512d, temp);
            setStartArea(0d, 512d);
        }
    }
}
