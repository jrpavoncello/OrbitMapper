using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace OrbitMapper
{
    public class Rhombus : Shape
    {
        public Rhombus()
        {
            base.Text = "Rhombus";
            base.Name = "Rhombus" + base.getTabCount();
            base.setTabCount(base.getTabCount() + 1);
            addVertex(0, 0, 0);
            double temp = Math.Tan(Math.PI / 3) * (256d);
            addVertex(256d, temp, 60d);
            addVertex(768d, temp, 0d);
            addVertex(512d, 0, 60d);
            setScale(768d, temp);
            setStartArea(0d, 512d);
        }
    }
}
