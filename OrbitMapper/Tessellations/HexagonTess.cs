using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace OrbitMapper
{
    public partial class HexagonTess : Tessellation
    {
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
            List<doublePoint> tri1 = new List<doublePoint>();

            tri1.Add(new doublePoint(xSmallSegment + xLargeSegment, 0));
            tri1.Add(new doublePoint(xSmallSegment, 0));
            tri1.Add(new doublePoint(0, ySegment));
            tri1.Add(new doublePoint(xSmallSegment, height));
            tri1.Add(new doublePoint(xSmallSegment + xLargeSegment, height));
            tri1.Add(new doublePoint(width, ySegment));
            tri1.Add(new doublePoint(xLargeSegment + width, ySegment));
            tri1.Add(new doublePoint(xSmallSegment + xLargeSegment + width, height));
            tri1.Add(new doublePoint(xLargeSegment + width, ySegment + height));
            tri1.Add(new doublePoint(width, ySegment + height));
            tri1.Add(new doublePoint(xSmallSegment + xLargeSegment, height));
            tri1.Add(new doublePoint(width, ySegment));

            pat.addPattern(tri1);
            setPattern(pat);
        }
    }
}
