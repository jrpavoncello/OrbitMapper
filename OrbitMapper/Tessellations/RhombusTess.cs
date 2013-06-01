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
    public partial class RhombusTess : Tessellation
    {
        public RhombusTess()
        {
            InitializeComponent();

            double smallSegment = Math.Cos(Math.PI / 3d) * 30d;
            double height = Math.Sin(Math.PI / 3d) * 30d;
            double lengthLong = Math.Cos(Math.PI / 6d) * 60d;
            double lengthShort = Math.Sin(Math.PI / 6d) * 60d;
            addStartZone(new Point((int)smallSegment + 30, 0), new Point((int)smallSegment + 60, 0));
            addReflectedStartZone(new Point((int)smallSegment, 0), new Point((int)smallSegment + 30, 0));
            setShapeHeight(height);

            Pattern pat = new Pattern(60d + lengthShort, lengthLong, 0d);
            List<DoublePoint> tri1 = new List<DoublePoint>();
            List<DoublePoint> tri2 = new List<DoublePoint>();
            List<DoublePoint> tri3 = new List<DoublePoint>();
            List<DoublePoint> tri4 = new List<DoublePoint>();
            List<DoublePoint> tri5 = new List<DoublePoint>();
            List<DoublePoint> tri6 = new List<DoublePoint>();

            tri1.Add(new DoublePoint(smallSegment, 0));
            tri1.Add(new DoublePoint(0, height));
            tri1.Add(new DoublePoint(30, height));
            tri1.Add(new DoublePoint(30 + smallSegment, 0));

            tri2.Add(new DoublePoint(0, height));
            tri2.Add(new DoublePoint(smallSegment, lengthLong));
            tri2.Add(new DoublePoint(30 + smallSegment, lengthLong));
            tri2.Add(new DoublePoint(30, height));

            tri3.Add(new DoublePoint(30 + smallSegment, 0));
            tri3.Add(new DoublePoint(30, height));
            tri3.Add(new DoublePoint(30 + smallSegment, lengthLong));
            tri3.Add(new DoublePoint(30 + lengthShort, height));

            tri4.Add(new DoublePoint(30 + smallSegment, 0));
            tri4.Add(new DoublePoint(30 + lengthShort, height));
            tri4.Add(new DoublePoint(60 + lengthShort, height));
            tri4.Add(new DoublePoint(60 + smallSegment, 0));

            tri5.Add(new DoublePoint(30 + lengthShort, height));
            tri5.Add(new DoublePoint(30 + smallSegment, lengthLong));
            tri5.Add(new DoublePoint(60 + smallSegment, lengthLong));
            tri5.Add(new DoublePoint(60 + lengthShort, height));

            tri6.Add(new DoublePoint(60 + lengthShort, height));
            tri6.Add(new DoublePoint(60 + smallSegment, lengthLong));
            tri6.Add(new DoublePoint(60 + lengthShort, lengthLong * 1.5));
            tri6.Add(new DoublePoint(60 + smallSegment + lengthShort, lengthLong));

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
