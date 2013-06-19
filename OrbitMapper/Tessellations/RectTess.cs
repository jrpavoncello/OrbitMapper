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
    public partial class RectTess : Tessellation
    {
        /// <summary>
        /// Customize this child
        /// </summary>
        public RectTess(double ratio)
        {
            InitializeComponent();

            double patBase = 30;
            // ratioOver1 is used to scale down the rectangle
            double ratioOver1 = ratio - 1;
            if (ratioOver1 > 0)
            {
                // If the ratioOver1 is over 2, the scale down will be much to small, so we will make 2 the max.
                if (ratioOver1 > 2)
                    ratioOver1 = 2;
                patBase /= ratioOver1 + 1;
            }
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
