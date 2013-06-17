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
    public partial class HexagonTess : Tessellation
    {
        /// <summary>
        /// Customize this child
        /// </summary>
        public HexagonTess()
        {
            InitializeComponent();

            double xSmallSegment = 10d;
            double xLargeSegment = 20d;
            double ySegment = Math.Cos(Math.PI / 6) * 20d;
            double height = ySegment * 2;
            double width = 40d;

            addStartZone(new Point((int)xSmallSegment, 0), new Point((int)(xSmallSegment + xLargeSegment), 0));
            setShapeHeight(height);

            Pattern pat = new Pattern(60d, height, 0d);
            List<DoublePoint> shape1 = new List<DoublePoint>();
            List<DoublePoint> shape2 = new List<DoublePoint>();

            shape1.Add(new DoublePoint(xSmallSegment + xLargeSegment, 0));
            shape1.Add(new DoublePoint(xSmallSegment, 0));
            shape1.Add(new DoublePoint(0, ySegment));
            shape1.Add(new DoublePoint(xSmallSegment, height));
            shape1.Add(new DoublePoint(xSmallSegment + xLargeSegment, height));
            shape1.Add(new DoublePoint(width, ySegment));

            shape2.Add(new DoublePoint(xLargeSegment + xLargeSegment + xSmallSegment + xSmallSegment, ySegment));
            shape2.Add(new DoublePoint(xLargeSegment + xSmallSegment + xSmallSegment, ySegment));
            shape2.Add(new DoublePoint(xLargeSegment + xSmallSegment + 0, ySegment + ySegment));
            shape2.Add(new DoublePoint(xLargeSegment + xSmallSegment + xSmallSegment, ySegment + height));
            shape2.Add(new DoublePoint(xLargeSegment + xSmallSegment + xSmallSegment + xLargeSegment, ySegment + height));
            shape2.Add(new DoublePoint(xLargeSegment + xSmallSegment + width, ySegment + ySegment));

            pat.addPattern(shape1);
            pat.addPattern(shape2);
            setPattern(pat);
        }
    }
}
