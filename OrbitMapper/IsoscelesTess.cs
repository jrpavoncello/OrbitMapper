using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrbitMapper
{
    public partial class IsoscelesTess : Tessellation
    {
        public IsoscelesTess()
        {
            InitializeComponent();
            double temp = Math.Tan(Math.PI / 6d) * 20;
            Pattern pat = new Pattern(40, temp * 3d, 20);
            List<doublePoint> tri1 = new List<doublePoint>();
            List<doublePoint> tri2 = new List<doublePoint>();
            addStartZone(new Point(0, 0), new Point(40, 0));
            setShapeHeight(temp);

            tri1.Add(new doublePoint(0, 0));
            tri1.Add(new doublePoint(20, temp * 3d));
            tri1.Add(new doublePoint(20, temp));
            tri1.Add(new doublePoint(0, 0));
            tri1.Add(new doublePoint(40, 0));
            tri1.Add(new doublePoint(20, temp * 3d));
            tri1.Add(new doublePoint(20, temp));
            tri1.Add(new doublePoint(40, 0));

            tri2.Add(new doublePoint(20, temp * 3d));
            tri2.Add(new doublePoint(40, 0));
            tri2.Add(new doublePoint(40, temp * 2d));
            tri2.Add(new doublePoint(20, temp * 3d));
            tri2.Add(new doublePoint(60, temp * 3d));
            tri2.Add(new doublePoint(40, 0));
            tri2.Add(new doublePoint(40, temp * 2d));
            tri2.Add(new doublePoint(60, temp * 3d));

            pat.addPattern(tri1);
            pat.addPattern(tri2);
            setPattern(pat);
        }
    }
}
