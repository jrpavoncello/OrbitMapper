using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OrbitMapper.Tessellations
{
    /// <summary>
    /// Extend from base 
    /// </summary>
    public partial class IsosTri90Tess : Tessellation
    {
        /// <summary>
        /// Customize this child
        /// </summary>
        public IsosTri90Tess()
        {
            InitializeComponent();
            double size = 40d;
            double temp = Math.Tan(Math.PI / 4d) * (size / 2);
            Pattern pat = new Pattern(size, temp * 2d, 0);
            List<DoublePoint> tri1 = new List<DoublePoint>();
            List<DoublePoint> tri2 = new List<DoublePoint>();
            List<DoublePoint> tri3 = new List<DoublePoint>();
            List<DoublePoint> tri4 = new List<DoublePoint>();
            addStartZone(new Point(0, 0), new Point((int)size, 0));
            setShapeHeight(temp);

            tri1.Add(new DoublePoint(0, 0));
            tri1.Add(new DoublePoint(size / 2, temp));
            tri1.Add(new DoublePoint(0, temp*2));

            tri2.Add(new DoublePoint(0, 0));
            tri2.Add(new DoublePoint(size, 0));
            tri2.Add(new DoublePoint(size / 2, temp));

            tri3.Add(new DoublePoint(size, temp * 2));
            tri3.Add(new DoublePoint(0, temp * 2));
            tri3.Add(new DoublePoint(size / 2, temp));

            tri4.Add(new DoublePoint(size, temp * 2));
            tri4.Add(new DoublePoint(size, 0));
            tri4.Add(new DoublePoint(size / 2, temp));

            pat.addPattern(tri1);
            pat.addPattern(tri2);
            pat.addPattern(tri3);
            pat.addPattern(tri4);
            setPattern(pat);
        }
    }
}
