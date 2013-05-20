using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace OrbitMapper
{
    public class IsosTri120 : Shape
    {
        public IsosTri120(){
            base.Text = "IsosTri120";
            base.Name = "IsosTri120" + base.getTabCount();
            base.setTabCount(base.getTabCount() + 1);
            addVertex(0, 0, 0);
            double temp = Math.Tan(Math.PI / 6) * (512d);
            addVertex(512d, temp, 30d);
            addVertex(1024d, 0, 150d);
            setScale(1024d, temp);
            setStartArea(0d, 1024);
        }
    }
}
