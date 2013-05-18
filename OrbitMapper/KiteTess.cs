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
    public partial class KiteTess : Tessellation
    {
        public KiteTess()
        {
            InitializeComponent();
            double heightOnSide = Math.Sin(Math.PI / 3d) * 30d;
            double segmentHeight = Math.Tan(Math.PI / 6d) * 30d;
            double triH = Math.Sin(Math.PI / 6d) * 30d;
            double height = Math.Sqrt(900 + (segmentHeight*segmentHeight));
            double totalHeight = height + segmentHeight;
            Pattern pat = new Pattern(60, totalHeight, 30);
            List<doublePoint> tri1 = new List<doublePoint>();
            List<doublePoint> tri2 = new List<doublePoint>();
            addStartZone(new Point(30, 0), new Point(60, 0));
            addReflectedStartZone(new Point(0, 0), new Point(30, 0));
            setShapeHeight(heightOnSide);

            tri1.Add(new doublePoint(30 - (Math.Cos(Math.PI / 3d) * 30d), heightOnSide));
            tri1.Add(new doublePoint(0, segmentHeight));
            tri1.Add(new doublePoint(0, 0));
            tri1.Add(new doublePoint(30, 0));
            tri1.Add(new doublePoint(30 - (Math.Cos(Math.PI / 3d) * 30d), heightOnSide));
            tri1.Add(new doublePoint(30, height));
            tri1.Add(new doublePoint(30 + triH, heightOnSide));
            tri1.Add(new doublePoint(30, 0));
            tri1.Add(new doublePoint(60, 0));
            tri1.Add(new doublePoint(60, segmentHeight));
            tri1.Add(new doublePoint(30 + triH, heightOnSide));
            tri1.Add(new doublePoint(30, 0));

            tri2.Add(new doublePoint(60 - (Math.Cos(Math.PI / 3d) * 30d), totalHeight - heightOnSide));
            tri2.Add(new doublePoint(30, totalHeight - segmentHeight));
            tri2.Add(new doublePoint(30, totalHeight));
            tri2.Add(new doublePoint(60, totalHeight));
            tri2.Add(new doublePoint(60 - (Math.Cos(Math.PI / 3d) * 30d), totalHeight - heightOnSide));
            tri2.Add(new doublePoint(60, totalHeight - height));
            tri2.Add(new doublePoint(60 + triH, totalHeight - heightOnSide));
            tri2.Add(new doublePoint(60, totalHeight));
            tri2.Add(new doublePoint(90, totalHeight));
            tri2.Add(new doublePoint(90, totalHeight - segmentHeight));
            tri2.Add(new doublePoint(60 + triH, totalHeight - heightOnSide));
            tri2.Add(new doublePoint(60, totalHeight));


            pat.addPattern(tri1);
            pat.addPattern(tri2);
            setPattern(pat);
        }
    }
}
