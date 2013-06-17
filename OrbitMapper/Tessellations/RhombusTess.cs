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
    public partial class RhombusTess : Tessellation
    {
        /// <summary>
        /// Customize this child
        /// </summary>
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
            List<DoublePoint> shape1 = new List<DoublePoint>();
            List<DoublePoint> shape2 = new List<DoublePoint>();
            List<DoublePoint> shape3 = new List<DoublePoint>();
            List<DoublePoint> shape4 = new List<DoublePoint>();
            List<DoublePoint> shape5 = new List<DoublePoint>();
            List<DoublePoint> shape6 = new List<DoublePoint>();

            shape1.Add(new DoublePoint(smallSegment, 0));
            shape1.Add(new DoublePoint(0, height));
            shape1.Add(new DoublePoint(30, height));
            shape1.Add(new DoublePoint(30 + smallSegment, 0));

            shape2.Add(new DoublePoint(0, height));
            shape2.Add(new DoublePoint(smallSegment, lengthLong));
            shape2.Add(new DoublePoint(30 + smallSegment, lengthLong));
            shape2.Add(new DoublePoint(30, height));

            shape3.Add(new DoublePoint(30 + smallSegment, 0));
            shape3.Add(new DoublePoint(30, height));
            shape3.Add(new DoublePoint(30 + smallSegment, lengthLong));
            shape3.Add(new DoublePoint(30 + lengthShort, height));

            shape4.Add(new DoublePoint(30 + smallSegment, 0));
            shape4.Add(new DoublePoint(30 + lengthShort, height));
            shape4.Add(new DoublePoint(60 + lengthShort, height));
            shape4.Add(new DoublePoint(60 + smallSegment, 0));

            shape5.Add(new DoublePoint(30 + lengthShort, height));
            shape5.Add(new DoublePoint(30 + smallSegment, lengthLong));
            shape5.Add(new DoublePoint(60 + smallSegment, lengthLong));
            shape5.Add(new DoublePoint(60 + lengthShort, height));

            shape6.Add(new DoublePoint(60 + lengthShort, height));
            shape6.Add(new DoublePoint(60 + smallSegment, lengthLong));
            shape6.Add(new DoublePoint(60 + lengthShort, lengthLong * 1.5));
            shape6.Add(new DoublePoint(60 + smallSegment + lengthShort, lengthLong));

            pat.addPattern(shape1);
            pat.addPattern(shape2);
            pat.addPattern(shape3);
            pat.addPattern(shape4);
            pat.addPattern(shape5);
            pat.addPattern(shape6);
            setPattern(pat);
        }
    }
}
