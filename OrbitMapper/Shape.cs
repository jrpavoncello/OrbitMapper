using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace OrbitMapper
{
    public class Shape : TabPage
    {
        private Vertices vertices = new Vertices();
        private static int tabCount = 0;
        private List<Intersect> collisions;
        private int numWalls;
        private List<doublePoint> walls;
        private double width = 0;
        private double height = 0;
        private double scaleX;
        private double scaleY;
        private double startPoint = 0;
        private double startAngle = 0;
        private int bounces = 0;
        private doublePoint startArea = new doublePoint();
        private int tabNum;
        private bool fromTessellation = false;
        private double tessShapeHeight;
        private double distance = 0;
        private bool isNotScaled = true;
        public bool undefCollision = false;
        public ContextMenu cm;
        //isResizable is reserved for only the rectangle to be true
        public bool isResizable = false;

        public Shape(){
            collisions = new List<Intersect>();
            walls = new List<doublePoint>();
            this.CausesValidation = true;
            tabNum = tabCount;
            cm = new ContextMenu();
            cm.MenuItems.Add("Remove", new EventHandler(removeThisTab));
        }

        public string getShapeData(){
            string data = "";
            if (startPoint <= 0 || startPoint >= 1 || startAngle <= 0 || startAngle >= 180)
                return null;
            data += "<startpoint>" + startPoint + "</startpoint>";
            data += "<startangle>" + startAngle + "</startangle>";
            data += "<bounces>" + bounces + "</bounces>";
            return data;
        }

        public void removeThisTab(Object sender, EventArgs e)
        {
            EventSource.removeTab(this.Name);
        }

        public doublePoint getStartArea(){
            return startArea;
        }

        public void decTabCount(){
            tabCount--;
        }

        public void setStartArea(double x1, double x2){
            startArea.x1 = x1;
            startArea.x2 = x2;
        }

        public void setFromTessellation(bool isFromTess){
            fromTessellation = isFromTess;
        }

        public void setStartPoint(double startPoint){
            this.startPoint = startPoint;
        }

        public void setTessShapeHeight(double h){
            tessShapeHeight = h;
        }

        public void setDistance(double d){
            distance = d;
            isNotScaled = true;
        }

        public double getStartPoint()
        {
            return startPoint;
        }

        public void setStartAngle(double startAngle)
        {
            this.startAngle = startAngle;
        }

        public double getStartAngle()
        {
            return startAngle;
        }

        public void setBounces(int bounces)
        {
            this.bounces = bounces;
        }

        public int getBounces()
        {
            return bounces;
        }
        
        public void addVertex(double x1, double x2, double angle)
        {
            numWalls++;
            this.walls.Add(new doublePoint(numWalls - 1, angle));
            vertices.addVertex(x1, x2);
            if(x1 > width)
                width = x1;
            if(x2 > height)
                height = x2;
        }

        private double offsetX1{
            set{ offsetX1 = value;}
            get{ return offsetX1; }
        }

        private double offsetX2
        {
            set { offsetX2 = value; }
            get { return offsetX2; }
        }

        public void setScale(double scaleX, double scaleY)
        {
            this.scaleX = scaleX;
            this.scaleY = scaleY;
        }

        public int getTabCount(){
            return tabCount;
        }

        public void setTabCount(int value){
            tabCount = value;
        }

        public int getTabNum(){
            return tabNum;
        }

        private int getVerticesCount(){
            return vertices.size();
        }

        private doublePoint vertexAt(int index){
            return vertices.pointAt(index);
        }

        private doublePoint getIntersect(double x1, double x2, double x3, double x4, double x5, double x6, double x7, double x8)
        {
            doublePoint ret = new doublePoint();
            if (x3 - x1 == 0)
            {
                double m2 = (x8 - x6) / (x7 - x5);
                double b2 = x6 - (m2 * x5);
                ret.x1 = x1;
                ret.x2 = (m2 * x1) + b2;
                return ret;
            }
            else if (x7 - x5 == 0)
            {
                double m1 = (x4 - x2) / (x3 - x1);
                double b1 = x2 - (m1 * x1);
                ret.x1 = x5;
                ret.x2 = m1 * x5 + b1;
                return ret;
            }
            else{
                double m1 = (x4 - x2) / (x3 - x1);
                double m2 = (x8 - x6) / (x7 - x5);
                if (m1 != m2)
                {
                    double b1 = x2 - (m1 * x1);
                    double b2 = x6 - (m2 * x5);
                    ret.x1 = (b2 - b1) / (m1 - m2);
                    ret.x2 = m2*ret.x1 + b2;
                    return ret;
                }
                return null;
            }
        }

        private doublePoint findProjection(double x1, double x2, double distance, double angle)
        {
            doublePoint ret = new doublePoint();

            if(angle == 0){
                ret.x1 = x1 + distance;
                ret.x2 = x2;
                return ret;
            }
            else if (angle == 180)
            {
                ret.x1 = x1 - distance;
                ret.x2 = x2;
                return ret;
            }
            else if (angle == 90)
            {
                ret.x1 = x1;
                ret.x2 = x2 + distance;
                return ret;
            }
            else if (angle == 270)
            {
                ret.x1 = x1;
                ret.x2 = x2 - distance;
                return ret;
            }

            double x3 = Math.Abs(Math.Cos(angle*Math.PI/180d) * distance);
            double x4 = Math.Abs(Math.Sin(angle*Math.PI/180d) * distance);
            if (angle > 0 && angle < 90)
            {
                ret.x1 = x1 + x3;
                ret.x2 = x2 + x4;
            }
            else if(angle > 90 && angle < 180){
                ret.x1 = x1 - x3;
                ret.x2 = x2 + x4;
            }
            else if (angle > 180 && angle < 270)
            {
                ret.x1 = x1 - x3;
                ret.x2 = x2 - x4;
            }
            else{
                ret.x1 = x1 + x3;
                ret.x2 = x2 - x4;
            }

            return ret;
        }

        private void findIntersectionsFromTess()
        {
        try{

            if (startPoint <= 0 || startPoint >= 1 || startAngle <= 0 || startAngle >= 180)
                return;

            collisions.Clear();

            double startingPoint = startArea.x1 + (startPoint * Math.Abs(startArea.x1 - startArea.x2));

            List<Intersect> intersects = new List<Intersect>();
            double totalDistance = 0;
            if(isNotScaled){
                distance = (this.height / tessShapeHeight) * distance;
                isNotScaled = false;
            }
            int i = 0;
            bounces = 0;
            intersects.Add(new Intersect());
            intersects[0].x1 = startingPoint;
            intersects[0].x2 = 0d;
            intersects[0].wall = 0;
            intersects[0].angle = startAngle;
            //Perform the loop for each of the bounces
            //At the end of the loop, set the values of intersect[] for i + 1
            while(totalDistance <= distance - 0.000001d)
            {
                Intersect[] tempIntersects = new Intersect[numWalls];
                for (int j = 0; j < tempIntersects.Length; j++)
                {
                    tempIntersects[j] = new Intersect();
                }
                int leastDistance = -1;
                doublePoint tempProjection = findProjection(intersects[i].x1, intersects[i].x2, width, intersects[i].angle);
                double x3 = tempProjection.x1;
                double x4 = tempProjection.x2;
                for (int j = 0; j < numWalls; j++)
                {
                    if (intersects[i].wall != j)
                    {
                        int tempMod = (int)mod(j - 1, numWalls);
                        doublePoint temp = getIntersect(intersects[i].x1, intersects[i].x2, x3, x4, vertexAt(tempMod).x1, vertexAt(tempMod).x2, vertexAt(j).x1, vertexAt(j).x2);
                        if (temp == null)
                            continue;
                        tempIntersects[j].x1 = temp.x1;
                        tempIntersects[j].x2 = temp.x2;
                        tempIntersects[j].wall = j;
                        tempIntersects[j].distance = Math.Sqrt(Math.Pow(intersects[i].x1 - tempIntersects[j].x1, 2) + Math.Pow(intersects[i].x2 - tempIntersects[j].x2, 2));

                        doublePoint minXPoint = new doublePoint();
                        doublePoint maxXPoint = new doublePoint();
                        doublePoint minYPoint = new doublePoint();
                        doublePoint maxYPoint = new doublePoint();
                        if (vertexAt(tempMod).x1 > vertexAt(j).x1)
                        {
                            minXPoint.x1 = vertexAt(j).x1;
                            minXPoint.x2 = vertexAt(j).x2;
                            maxXPoint.x1 = vertexAt(tempMod).x1;
                            maxXPoint.x2 = vertexAt(tempMod).x2;
                        }
                        else
                        {
                            minXPoint.x1 = vertexAt(tempMod).x1;
                            minXPoint.x2 = vertexAt(tempMod).x2;
                            maxXPoint.x1 = vertexAt(j).x1;
                            maxXPoint.x2 = vertexAt(j).x2;
                        }

                        if (vertexAt(tempMod).x2 > vertexAt(j).x2)
                        {
                            minYPoint.x1 = vertexAt(j).x1;
                            minYPoint.x2 = vertexAt(j).x2;
                            maxYPoint.x1 = vertexAt(tempMod).x1;
                            maxYPoint.x2 = vertexAt(tempMod).x2;
                        }
                        else
                        {
                            minYPoint.x1 = vertexAt(tempMod).x1;
                            minYPoint.x2 = vertexAt(tempMod).x2;
                            maxYPoint.x1 = vertexAt(j).x1;
                            maxYPoint.x2 = vertexAt(j).x2;
                        }

                        if (tempIntersects[j].distance > 0 &&
                            tempIntersects[j].x1 >= minXPoint.x1 && tempIntersects[j].x1 <= maxXPoint.x1 &&
                            tempIntersects[j].x2 >= minYPoint.x2 && tempIntersects[j].x2 <= maxYPoint.x2)
                        {
                            if (leastDistance == -1)
                            {
                                leastDistance = j;
                            }
                            else
                            {
                                if (tempIntersects[leastDistance].distance > tempIntersects[j].distance)
                                {
                                    leastDistance = j;
                                }
                            }
                        }
                    }
                }
                for (int j = 0; j < numWalls; j++)
                {
                    if (tempIntersects[leastDistance].x1 == vertexAt(j).x1 && tempIntersects[leastDistance].x2 == vertexAt(j).x2)
                        undefCollision = true;
                }
                double projection = findAngle(intersects[i].x1, intersects[i].x2, tempIntersects[leastDistance].x1, tempIntersects[leastDistance].x2);
                tempIntersects[leastDistance].angle = findReflection(projection, walls[leastDistance].x2);
                tempIntersects[leastDistance].wall = leastDistance;
                intersects.Add(new Intersect());
                intersects[i + 1] = tempIntersects[leastDistance];
                totalDistance += tempIntersects[leastDistance].distance;
                bounces++;
                i++;
            }
            for (int j = 0; j < bounces + 1; j++)
            {
                collisions.Add(intersects[j]);
            }
            EventSource.finishedDraw(bounces);
            }
            catch(Exception e){
                Console.Out.WriteLine(e.StackTrace);
            }
        }

        private void findIntersections()
        {

            if (fromTessellation){
                findIntersectionsFromTess();
                return;
            }

            if (startPoint <= 0 || startPoint >= 1 || startAngle <= 0 || startAngle >= 180 || bounces <= 0)
                return;

            collisions.Clear();

            double startingPoint = startArea.x1 + (startPoint * Math.Abs(startArea.x1 - startArea.x2));

            Intersect[] intersects = new Intersect[bounces + 1];
            for(int i = 0; i < intersects.Length; i++){
                intersects[i] = new Intersect();
            }
            intersects[0].x1 = startingPoint;
            intersects[0].x2 = 0d;
            intersects[0].wall = 0;
            intersects[0].angle = startAngle;
            //Perform the loop for each of the bounces
            //At the end of the loop, set the values of intersect[] for i + 1
            for(int i = 0; i < bounces; i++){

                Intersect[] tempIntersects = new Intersect[numWalls];
                for (int j = 0; j < tempIntersects.Length; j++)
                {
                    tempIntersects[j] = new Intersect();
                }
                int leastDistance = -1;
                doublePoint tempProjection = findProjection(intersects[i].x1, intersects[i].x2, width, intersects[i].angle);
                double x3 = tempProjection.x1;
                double x4 = tempProjection.x2;
                for (int j = 0; j < numWalls; j++){
                    if(intersects[i].wall != j){
                        int tempMod = (int)mod(j - 1, numWalls);
                        doublePoint temp = getIntersect(intersects[i].x1, intersects[i].x2, x3, x4, vertexAt(tempMod).x1, vertexAt(tempMod).x2, vertexAt(j).x1, vertexAt(j).x2);
                        if(temp == null)
                            continue;
                        tempIntersects[j].x1 = temp.x1;
                        tempIntersects[j].x2 = temp.x2;
                        tempIntersects[j].wall = j;
                        tempIntersects[j].distance = Math.Sqrt(Math.Pow(intersects[i].x1 - tempIntersects[j].x1, 2) + Math.Pow(intersects[i].x2 - tempIntersects[j].x2, 2));
                        
                        doublePoint minXPoint = new doublePoint();
                        doublePoint maxXPoint = new doublePoint();
                        doublePoint minYPoint = new doublePoint();
                        doublePoint maxYPoint = new doublePoint();
                        if (vertexAt(tempMod).x1 > vertexAt(j).x1)
                        {
                            minXPoint.x1 = vertexAt(j).x1;
                            minXPoint.x2 = vertexAt(j).x2;
                            maxXPoint.x1 = vertexAt(tempMod).x1;
                            maxXPoint.x2 = vertexAt(tempMod).x2;
                        }
                        else
                        {
                            minXPoint.x1 = vertexAt(tempMod).x1;
                            minXPoint.x2 = vertexAt(tempMod).x2;
                            maxXPoint.x1 = vertexAt(j).x1;
                            maxXPoint.x2 = vertexAt(j).x2;
                        }

                        if (vertexAt(tempMod).x2 > vertexAt(j).x2)
                        {
                            minYPoint.x1 = vertexAt(j).x1;
                            minYPoint.x2 = vertexAt(j).x2;
                            maxYPoint.x1 = vertexAt(tempMod).x1;
                            maxYPoint.x2 = vertexAt(tempMod).x2;
                        }
                        else
                        {
                            minYPoint.x1 = vertexAt(tempMod).x1;
                            minYPoint.x2 = vertexAt(tempMod).x2;
                            maxYPoint.x1 = vertexAt(j).x1;
                            maxYPoint.x2 = vertexAt(j).x2;
                        }
                        
                        if(tempIntersects[j].distance > 0 &&
                            tempIntersects[j].x1 >= minXPoint.x1 && tempIntersects[j].x1 <= maxXPoint.x1 &&
                            tempIntersects[j].x2 >= minYPoint.x2 && tempIntersects[j].x2 <= maxYPoint.x2)
                        {
                            if(leastDistance == -1){
                                leastDistance = j;
                            }
                            else{
                                if(tempIntersects[leastDistance].distance > tempIntersects[j].distance){
                                    leastDistance = j;
                                }
                            }
                        }
                    }
                }
                for (int j = 0; j < numWalls; j++)
                {
                    if (tempIntersects[leastDistance].x1 == vertexAt(j).x1 && tempIntersects[leastDistance].x2 == vertexAt(j).x2)
                        undefCollision = true;
                }
                double projection = findAngle(intersects[i].x1, intersects[i].x2, tempIntersects[leastDistance].x1, tempIntersects[leastDistance].x2);
                tempIntersects[leastDistance].angle = findReflection(projection, walls[leastDistance].x2);
                tempIntersects[leastDistance].wall = leastDistance;
                intersects[i + 1] = tempIntersects[leastDistance];
            }
            for(int i = 0; i < bounces+1; i++){
                collisions.Add(intersects[i]);
            }
        }

        private double findReflection(double projection, double wallAngle){
            double temp = 0;
            if (wallAngle == 180 || wallAngle == 0)
            {
                temp = 360 - projection;
            }
            else if(wallAngle == 90 || wallAngle == 270)
            {
                temp = mod(180 - projection, 360);
            }
            else{
                temp = mod(2 * wallAngle - projection, 360);
            }
            return temp;
        }

        private double mod(double x, double m)
        {
            return (x % m + m) % m;
        }

        private double findAngle(double x1, double x2, double x3, double x4)
        {
            double x13 = x3 - x1;
            double x24 = x4 - x2;
            double angle = Math.Abs(Math.Atan(x24/x13)*180/Math.PI);

            if(x13 == 0 && x24 >= 0){
                angle = 90;
            }
            else if (x13 == 0 && x24 <= 0)
            {
                angle = 270;
            }
            else if (x13 >= 0 && x24 == 0)
            {
                angle = 0;
            }
            else if (x13 <= 0 && x24 == 0)
            {
                angle = 180;
            }

            if(x13 >= 0 && x24 >= 0)
                angle = angle;
                
            else if(x13 <= 0 && x24 >= 0)
                angle = 180 - angle;

            else if (x13 >= 0 && x24 <= 0)
                angle = 360 - angle;

            else
                angle = angle + 180;

            return angle;
        }

        private int scale(double x)
        {
            double tabWidth = base.Width - 10;
            double tabHeight = base.Height - 10;
            double scaledX = tabWidth / scaleX;
            double scaledY = tabHeight / scaleY;
            if(scaledX < scaledY){
                return (int)(scaledX * x);
            }
            else
            {
                return (int)(scaledY * x);
            }
        }

        public Point[] getCollisions()
        {
            int count = collisions.Count;
            if (count == 0)
                return null;
            Point[] temp = new Point[count];
            EventSource.output(count + " collisions.");
            for (int i = 0; i < count; i++)
            {
                temp[i] = new Point((int)((base.Width/2) - (scale(this.width)/2) + scale(collisions.ElementAt<Intersect>(i).x1)), base.Height - 1 - scale(collisions.ElementAt<Intersect>(i).x2));
            }
            return temp;
        }

        public Point[] getVertices()
        {
            List<Point> temp = new List<Point>();
            EventSource.output(vertices.size() + " vertices.");
            for (int i = 0; i < vertices.size(); i++)
            {
                temp.Add(new Point((base.Width / 2) - (scale(this.width) / 2) + scale(vertices.pointAt(i).x1), base.Height - 1 - scale(vertices.pointAt(i).x2)));
            }
            return temp.ToArray<Point>();
        }

        public int getTabHeight()
        {
            return base.Height;
        }

        public int getTabWidth()
        {
            return base.Width;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (getVerticesCount() != 0)
            {
                Graphics g = e.Graphics;
                g.Clear(SystemColors.Control);

                Point[] shape = getVertices();
                findIntersections();
                Point[] collisions = getCollisions();
                EventSource.output("Drawing " + shape.Length + " vertices.");
                g.DrawPolygon(System.Drawing.Pens.Black, shape);
                if (collisions != null && !undefCollision)
                {
                    EventSource.output("Intersections detected: " + collisions.Length);
                    for (int i = 0; i < collisions.Length - 1; i++)
                    {
                        if (i == 0)
                            g.FillEllipse(new SolidBrush(Color.SteelBlue), collisions[i].X - 5, collisions[i].Y - 6, 10, 10);
                        else
                            if (i == collisions.Length - 2)
                                g.FillEllipse(new SolidBrush(Color.Tomato), collisions[i + 1].X - 4, collisions[i + 1].Y - 5, 8, 8);
                        g.DrawLine(System.Drawing.Pens.Red, collisions[i], collisions[i + 1]);
                    }
                }
            }
        }
    }
}