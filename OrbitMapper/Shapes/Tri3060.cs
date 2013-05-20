using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace OrbitMapper
{
    public class Tri3060 : Shape
    {
        public Tri3060()
        {
            base.Text = "Tri3060";
            base.Name = "Tri3060" + base.getTabCount();
            base.setTabCount(base.getTabCount() + 1);
            double leftWallLength = 1024d * Math.Cos(Math.PI / 3);
            double height = Math.Sin(Math.PI / 3) * leftWallLength;
            double smallBottomWallLength = leftWallLength * Math.Sin(Math.PI / 3);
            addVertex(0, 0, 0);
            addVertex(smallBottomWallLength, height, 60d);
            addVertex(1024d, 0, 150d);
            setScale(1024d, height);
            setStartArea(0d, 1024);
        }
    }
}