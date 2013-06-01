using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrbitMapper.Shapes
{
    class Hexagon : Shape
    {
        /// <summary>
        /// Use the base class to configure this child specifically for a 120 Hexagon
        /// </summary>
        public Hexagon(){
            base.Text = "Hexagon";
            base.Name = "Hexagon" + (base.getShapeCount() - 1);
            double xSmallSegment = 256d;
            double xLargeSegment = 512d;
            double ySegment = Math.Cos(Math.PI / 6)*512d;
            double height = ySegment*2;
            base.addVertex(xSmallSegment, 0, 0);
            base.addVertex(0, ySegment, 120d);
            base.addVertex(xSmallSegment, height, 60);
            base.addVertex(xSmallSegment + xLargeSegment, height, 0d);
            base.addVertex(1024d, ySegment, 120d);
            base.addVertex(xSmallSegment + xLargeSegment, 0, 60d);
            base.setStartArea(xSmallSegment, xSmallSegment + xLargeSegment);
        }
    }
}
