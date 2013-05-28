using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrbitMapper
{
    public partial class RhombusTess : Tessellation
    {
        public RhombusTess(double ratio)
        {
            InitializeComponent();

            addStartZone(new Point((int)smallSegment + 30, 0), new Point((int)smallSegment + 60, 0));
            addReflectedStartZone(new Point((int)smallSegment, 0), new Point((int)smallSegment + 30, 0));
            setShapeHeight(height);

            Pattern pat = new Pattern(60d + lengthShort, lengthLong, 0d);
            List<doublePoint> tri1 = new List<doublePoint>();

            tri1.Add(new doublePoint(smallSegment, 0));
            tri1.Add(new doublePoint(0, height));
            tri1.Add(new doublePoint(30, height));
            tri1.Add(new doublePoint(30 + smallSegment, 0));

            pat.addPattern(tri1);
            setPattern(pat);
        }
    }
}
