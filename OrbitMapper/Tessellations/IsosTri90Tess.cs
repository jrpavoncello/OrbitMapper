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
    public partial class IsosTri90Tess : Tessellation
    {
        public IsosTri90Tess()
        {
            InitializeComponent();
            double temp = Math.Tan(Math.PI / 4d) * 20;
            Pattern pat = new Pattern(40, temp * 2d, 0);
            List<doublePoint> tri1 = new List<doublePoint>();
            List<doublePoint> tri2 = new List<doublePoint>();
            addStartZone(new Point(0, 0), new Point(40, 0));
            setShapeHeight(temp);

            tri1.Add(new doublePoint(0, 0));
            tri1.Add(new doublePoint(20, temp));
            tri1.Add(new doublePoint(0, temp*2));
            tri1.Add(new doublePoint(0, 0));
            tri1.Add(new doublePoint(40, 0));
            tri1.Add(new doublePoint(20, temp));
            tri1.Add(new doublePoint(40, temp*2));
            tri1.Add(new doublePoint(0, temp*2));

            tri2.Add(new doublePoint(40, temp*2));
            tri2.Add(new doublePoint(40, 0));

            pat.addPattern(tri1);
            pat.addPattern(tri2);
            setPattern(pat);
        }
    }
}
