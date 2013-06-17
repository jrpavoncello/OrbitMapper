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
    public partial class IsosTri120Tess : Tessellation
    {
        /// <summary>
        /// Customize this child
        /// </summary>
        public IsosTri120Tess()
        {
            InitializeComponent();
            double temp = Math.Tan(Math.PI / 6d) * 20;
            double height = temp * 3d;
            Pattern pat = new Pattern(40, height, 20);
            List<DoublePoint> tri1 = new List<DoublePoint>();
            List<DoublePoint> tri2 = new List<DoublePoint>();
            List<DoublePoint> tri3 = new List<DoublePoint>();
            List<DoublePoint> tri4 = new List<DoublePoint>();
            List<DoublePoint> tri5 = new List<DoublePoint>();
            List<DoublePoint> tri6 = new List<DoublePoint>();
            addStartZone(new Point(0, 0), new Point(40, 0));
            setShapeHeight(temp);

            tri1.Add(new DoublePoint(0, 0));
            tri1.Add(new DoublePoint(20, height));
            tri1.Add(new DoublePoint(20, temp));

            tri2.Add(new DoublePoint(0, 0));
            tri2.Add(new DoublePoint(40, 0));
            tri2.Add(new DoublePoint(20, temp));

            tri3.Add(new DoublePoint(20, height));
            tri3.Add(new DoublePoint(40, 0));
            tri3.Add(new DoublePoint(20, temp));

            tri4.Add(new DoublePoint(20, height));
            tri4.Add(new DoublePoint(40, 0));
            tri4.Add(new DoublePoint(40, height - temp));

            tri5.Add(new DoublePoint(20, height));
            tri5.Add(new DoublePoint(60, height));
            tri5.Add(new DoublePoint(40, height - temp));

            tri6.Add(new DoublePoint(60, height));
            tri6.Add(new DoublePoint(40, 0));
            tri6.Add(new DoublePoint(40, height - temp));

            pat.addPattern(tri1);
            pat.addPattern(tri2);
            pat.addPattern(tri3);
            pat.addPattern(tri4);
            pat.addPattern(tri5);
            pat.addPattern(tri6);
            setPattern(pat);
        }
    }
}
