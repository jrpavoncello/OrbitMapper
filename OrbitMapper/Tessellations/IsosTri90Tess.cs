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
            double temp = Math.Tan(Math.PI / 4d) * 20;
            Pattern pat = new Pattern(40, temp * 2d, 0);
            List<DoublePoint> tri1 = new List<DoublePoint>();
            List<DoublePoint> tri2 = new List<DoublePoint>();
            addStartZone(new Point(0, 0), new Point(40, 0));
            setShapeHeight(temp);

            tri1.Add(new DoublePoint(0, 0));
            tri1.Add(new DoublePoint(20, temp));
            tri1.Add(new DoublePoint(0, temp*2));
            tri1.Add(new DoublePoint(0, 0));
            tri1.Add(new DoublePoint(40, 0));
            tri1.Add(new DoublePoint(20, temp));
            tri1.Add(new DoublePoint(40, temp*2));
            tri1.Add(new DoublePoint(0, temp*2));

            tri2.Add(new DoublePoint(40, temp*2));
            tri2.Add(new DoublePoint(40, 0));

            pat.addPattern(tri1);
            pat.addPattern(tri2);
            setPattern(pat);
        }
    }
}
