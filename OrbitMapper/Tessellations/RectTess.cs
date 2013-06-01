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
    public partial class RectTess : Tessellation
    {
        public RectTess(double ratio)
        {
            InitializeComponent();

            double patBase = 30;
            double heightRatio = ratio;
            double patHeight = patBase * heightRatio;
            double patWidth = patBase;
            addStartZone(new Point(0, 0), new Point((int)patWidth, 0));
            setShapeHeight(patHeight);

            Pattern pat = new Pattern(patWidth, patHeight, 0d);
            List<DoublePoint> tri1 = new List<DoublePoint>();

            tri1.Add(new DoublePoint(0, 0));
            tri1.Add(new DoublePoint(0, patHeight));
            tri1.Add(new DoublePoint(patWidth, patHeight));
            tri1.Add(new DoublePoint(patWidth, 0));

            pat.addPattern(tri1);
            setPattern(pat);
        }
    }
}
