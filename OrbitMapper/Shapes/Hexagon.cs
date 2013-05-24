using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbitMapper
{
    class Hexagon : Shape
    {
        public Hexagon(){
            base.Text = "Hexagon";
            base.Name = "Hexagon" + (base.getShapeCount() - 1);
            base.setTabCount(base.getTabCount() + 1);
            double xSmallSegment = 256d;
            double xLargeSegment = 512d;
            double ySegment = Math.Cos(Math.PI / 6)*512d;
            double height = ySegment*2;
            addVertex(xSmallSegment, 0, 0);
            addVertex(0, ySegment, 120d);
            addVertex(xSmallSegment, height, 60);
            addVertex(xSmallSegment + xLargeSegment, height, 0d);
            addVertex(1024d, ySegment, 120d);
            addVertex(xSmallSegment + xLargeSegment, 0, 60d);
            setScale(1024d, height);
            setStartArea(xSmallSegment, xSmallSegment + xLargeSegment);
        }
    }
}
