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
    public partial class Tri3060Tess : Tessellation
    {
        /// <summary>
        /// Customize the child
        /// </summary>
        public Tri3060Tess()
        {
            InitializeComponent();
            int baseScale = 30;
            double heightOnSide = Math.Sin(Math.PI / 3d) * baseScale;
            double segmentHeight = Math.Tan(Math.PI / 6d) * baseScale;
            double triH = Math.Sin(Math.PI / 6d) * baseScale;
            double height = Math.Sqrt(900 + (segmentHeight * segmentHeight));
            double totalHeight = height + segmentHeight;
            Pattern pat = new Pattern(baseScale*2, totalHeight, baseScale);
            List<DoublePoint> tri1 = new List<DoublePoint>();
            List<DoublePoint> tri2 = new List<DoublePoint>();
            List<DoublePoint> tri3 = new List<DoublePoint>();
            List<DoublePoint> tri4 = new List<DoublePoint>();
            List<DoublePoint> tri5 = new List<DoublePoint>();
            List<DoublePoint> tri6 = new List<DoublePoint>();
            List<DoublePoint> tri7 = new List<DoublePoint>();
            List<DoublePoint> tri8 = new List<DoublePoint>();
            List<DoublePoint> tri9 = new List<DoublePoint>();
            List<DoublePoint> tri10 = new List<DoublePoint>();
            List<DoublePoint> tri11 = new List<DoublePoint>();
            List<DoublePoint> tri12 = new List<DoublePoint>();
            addStartZone(new Point(baseScale, 0), new Point(baseScale * 2, 0));
            addReflectedStartZone(new Point(0, 0), new Point(baseScale, 0));
            setShapeHeight(heightOnSide);

            tri1.Add(new DoublePoint(baseScale, 0));
            tri1.Add(new DoublePoint(0, 0));
            tri1.Add(new DoublePoint(0, segmentHeight));

            tri2.Add(new DoublePoint(baseScale, 0));
            tri2.Add(new DoublePoint(0, segmentHeight));
            tri2.Add(new DoublePoint(baseScale - triH, heightOnSide));

            tri3.Add(new DoublePoint(baseScale, 0));
            tri3.Add(new DoublePoint(baseScale, height));
            tri3.Add(new DoublePoint(baseScale - triH, heightOnSide));

            tri4.Add(new DoublePoint(baseScale, 0));
            tri4.Add(new DoublePoint(baseScale, height));
            tri4.Add(new DoublePoint(baseScale + triH, heightOnSide));

            tri5.Add(new DoublePoint(baseScale, 0));
            tri5.Add(new DoublePoint(baseScale + triH, heightOnSide));
            tri5.Add(new DoublePoint(baseScale*2, segmentHeight));

            tri6.Add(new DoublePoint(baseScale, 0));
            tri6.Add(new DoublePoint(baseScale * 2,  0));
            tri6.Add(new DoublePoint(baseScale * 2, segmentHeight));

            tri7.Add(new DoublePoint(baseScale + baseScale, totalHeight));
            tri7.Add(new DoublePoint(baseScale, totalHeight));
            tri7.Add(new DoublePoint(baseScale, totalHeight - segmentHeight));

            tri8.Add(new DoublePoint(baseScale + baseScale, totalHeight));
            tri8.Add(new DoublePoint(baseScale, totalHeight - segmentHeight));
            tri8.Add(new DoublePoint(baseScale + baseScale - triH, totalHeight - heightOnSide));

            tri9.Add(new DoublePoint(baseScale + baseScale, totalHeight));
            tri9.Add(new DoublePoint(baseScale + baseScale, totalHeight - height));
            tri9.Add(new DoublePoint(baseScale + baseScale - triH, totalHeight - heightOnSide));

            tri10.Add(new DoublePoint(baseScale + baseScale, totalHeight));
            tri10.Add(new DoublePoint(baseScale + baseScale, totalHeight - height));
            tri10.Add(new DoublePoint(baseScale + baseScale + triH, totalHeight - heightOnSide));

            tri11.Add(new DoublePoint(baseScale + baseScale, totalHeight));
            tri11.Add(new DoublePoint(baseScale + baseScale + triH, totalHeight - heightOnSide));
            tri11.Add(new DoublePoint(baseScale + baseScale * 2, totalHeight - segmentHeight));

            tri12.Add(new DoublePoint(baseScale + baseScale, totalHeight));
            tri12.Add(new DoublePoint(baseScale + baseScale * 2, totalHeight));
            tri12.Add(new DoublePoint(baseScale + baseScale * 2, totalHeight - segmentHeight));

            pat.addPattern(tri1);
            pat.addPattern(tri2);
            pat.addPattern(tri3);
            pat.addPattern(tri4);
            pat.addPattern(tri5);
            pat.addPattern(tri6);
            pat.addPattern(tri7);
            pat.addPattern(tri8);
            pat.addPattern(tri9);
            pat.addPattern(tri10);
            pat.addPattern(tri11);
            pat.addPattern(tri12);
            setPattern(pat);
        }
    }
}
