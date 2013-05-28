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
    public partial class Tessellation : UserControl
    {
        private Pattern pattern = new Pattern();
        private bool buttonPressed = true;
        private Point baseClick = new Point(0, 0);
        private Point endClick = new Point(0, 0);
        private Point lastPictureBoxState = new Point();
        private List<Point[]> startZones = new List<Point[]>();
        private List<Point[]> reflectedStartZones = new List<Point[]>();
        public bool baseIsGood = true;
        private bool hasReflectedZones = false;
        private double startingPoint;
        private double startingAngle;
        private double distance;
        private int lastScrollValue = -1;
        private double shapeHeight;
        public bool populateByTess = false;
        public static int lastWidth = 0;
        private static int tabCount = 0;
        public bool isInReflectedZone = false;

        public Tessellation()
        {
            InitializeComponent();
            tabCount++;
            this.TabStop = false;
        }

        public double getStartingPoint(){
            return startingPoint;
        }

        public double getStartingAngle(){
            return startingAngle;
        }

        public double getDistance(){
            return distance;
        }

        public PictureBox getPictureBox(){
            return pictureBox1;
        }

        public void addStartZone(Point p1, Point p2){
            startZones.Add(new Point[]{p1, p2});
        }

        public void decTabCount()
        {
            tabCount--;
        }

        public void setBasePos(Point pos)
        {
            baseClick.X = pos.X;
            baseClick.Y = pos.Y;
        }

        public void setEndPos(Point pos)
        {
            endClick.X = pos.X;
            endClick.Y = pos.Y;
        }

        public void setBaseClick(Point pos){
            baseClick.X = pos.X;
            baseClick.Y = pos.Y;
            populateByTess = true;
            pictureBox1.Invalidate();
            pictureBox1.Update();
        }

        public void setEndClick(Point pos)
        {
            populateByTess = true;
            endClick.X = pos.X;
            endClick.Y = pos.Y;
            pictureBox1.Invalidate();
            pictureBox1.Update();
        }

        private bool betweenStartZones(int x)
        {
            bool isBetween = false;
            for (int i = 0; i < startZones.Count(); i++)
            {
                if (x > startZones.ElementAt<Point[]>(i)[0].X && x < startZones.ElementAt<Point[]>(i)[1].X)
                    isBetween = true;
            }
            for (int i = 0; i < reflectedStartZones.Count(); i++)
            {
                if (x > reflectedStartZones.ElementAt<Point[]>(i)[0].X && x < reflectedStartZones.ElementAt<Point[]>(i)[1].X)
                    isBetween = true;
            }
            return isBetween;
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
            else
            {
                double m1 = (x4 - x2) / (x3 - x1);
                double m2 = (x8 - x6) / (x7 - x5);
                if (m1 != m2)
                {
                    double b1 = x2 - (m1 * x1);
                    double b2 = x6 - (m2 * x5);
                    ret.x1 = (b2 - b1) / (m1 - m2);
                    ret.x2 = m2 * ret.x1 + b2;
                    return ret;
                }
                return null;
            }
        }

        public void addReflectedStartZone(Point p1, Point p2)
        {
            reflectedStartZones.Add(new Point[] { p1, p2 });
            hasReflectedZones = true;
        }

        public void setShapeHeight(double height){
            shapeHeight = height;
        }

        public double getShapeHeight()
        {
            return shapeHeight;
        }

        private int betweenReflectedStartZones(int x)
        {
            if (!hasReflectedZones)
                return -1;
            for (int i = 0; i < reflectedStartZones.Count(); i++)
            {
                if (x > reflectedStartZones.ElementAt<Point[]>(i)[0].X && x < reflectedStartZones.ElementAt<Point[]>(i)[1].X)
                    return i;
            }
            return -2;
        }

        public void toggleButton()
        {
            if (buttonPressed)
                buttonPressed = false;
            else
                buttonPressed = true;
        }

        public Pattern getPattern()
        {
            return pattern;
        }

        public void setPattern(Pattern pat)
        {
            pattern = pat;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(SystemColors.Control);

            if ((baseClick.X != 0 || baseClick.Y != 0) && (endClick.X != 0 || endClick.Y != 0))
            {
                if (lastPictureBoxState.Y == 0)
                {
                    lastPictureBoxState = new Point(pictureBox1.Width, pictureBox1.Height);
                }
                else{
                    int offsetY = pictureBox1.Height - lastPictureBoxState.Y;
                    baseClick.Y += offsetY;
                    endClick.Y += offsetY;
                    lastPictureBoxState = new Point(pictureBox1.Width, pictureBox1.Height);
                }
            }

            Point[][] patterns = getPattern().getPatterns();
            int iterX = (int)(this.Width / getPattern().getWidth()) + 2;
            int iterY = (int)(this.Height / getPattern().getHeight()) + 2;
            bool baseIsCorrect = false;
            for (int i = 0; i < iterX; i++)
            {
                for (int j = 0; j < iterY; j++)
                {
                    for (int k = 0; k < patterns.Count(); k++)
                    {
                        Point[] poly = new Point[patterns.ElementAt<Point[]>(k).Count()];
                        for (int l = 0; l < patterns.ElementAt<Point[]>(k).Count(); l++)
                        {
                            Point temp = new Point((int)(i * getPattern().getWidth()) + patterns[k][l].X, getPictureBox().Height - 5 - (int)(j * getPattern().getHeight()) - patterns[k][l].Y);
                            if (j % 2 == 1)
                                temp.X -= (int)getPattern().getOffset();
                            temp.X -= (int)getPattern().getWidth();
                            poly[l] = temp;
                        }
                        g.DrawPolygon(System.Drawing.Pens.Black, poly);
                    }
                    #region Logic for getting data from mouse click
                    if (((baseClick.X != 0 || baseClick.Y != 0) && (endClick.X != 0 || endClick.Y != 0)) && 
                    (endClick.Y >= getPictureBox().Height - 5 - (int)(j * getPattern().getHeight()) - 5 && 
                    endClick.Y <= getPictureBox().Height - 5 - (int)(j * getPattern().getHeight()) + 5))
                    {
                        endClick.Y = getPictureBox().Height - 5 - (int)(j * getPattern().getHeight());
                    }
                    if (((baseClick.X != 0 || baseClick.Y != 0) && (endClick.X != 0 || endClick.Y != 0)) &&
                    (baseClick.Y >= getPictureBox().Height - 5 - (int)(j * getPattern().getHeight()) - 5 &&
                    baseClick.Y <= getPictureBox().Height - 5 - (int)(j * getPattern().getHeight()) + 5))
                    {
                        baseClick.Y = getPictureBox().Height - 5 - (int)(j * getPattern().getHeight());
                        if (betweenStartZones(baseClick.X - (int)(i * getPattern().getWidth())))
                        {
                            int zone;
                            if ((zone = betweenReflectedStartZones(baseClick.X - (int)(i * getPattern().getWidth()))) > -1)
                            {
                                startingPoint = (double)(reflectedStartZones.ElementAt<Point[]>(zone)[1].X - (baseClick.X - (i * getPattern().getWidth()))) / (double)(reflectedStartZones.ElementAt<Point[]>(zone)[1].X - reflectedStartZones.ElementAt<Point[]>(zone)[0].X);
                                startingAngle = Math.Atan((double)(endClick.Y - baseClick.Y) / (double)(endClick.X - baseClick.X)) * 180d / Math.PI;
                                startingAngle = mod(startingAngle, 180);
                                distance = Math.Sqrt(Math.Pow(endClick.Y - baseClick.Y, 2) + Math.Pow(endClick.X - baseClick.X, 2));
                                this.isInReflectedZone = true;
                            }
                            else
                            {
                                try
                                {
                                    startingPoint = (double)(baseClick.X - (i * getPattern().getWidth()) - startZones.ElementAt<Point[]>(0)[0].X) / (double)(startZones.ElementAt<Point[]>(0)[1].X - startZones.ElementAt<Point[]>(0)[0].X);
                                    startingAngle = Math.Atan((double)(endClick.Y - baseClick.Y) / (double)(endClick.X - baseClick.X)) * 180d / Math.PI;
                                    if (baseClick.X > endClick.X)
                                        startingAngle = 180 - startingAngle;
                                    startingAngle = Math.Abs(startingAngle);
                                    this.isInReflectedZone = false;
                                }
                                catch (Exception ex)
                                {
                                    Console.Out.WriteLine(ex.GetBaseException().StackTrace);
                                }
                                distance = Math.Sqrt(Math.Pow(endClick.Y - baseClick.Y, 2) + Math.Pow(endClick.X - baseClick.X, 2));
                            }
                            baseIsCorrect = true;
                        }
                    }
                    #endregion
                }
            }
            baseIsGood = baseIsCorrect;
                
            if ((baseClick.X != 0 || baseClick.Y != 0) && (endClick.X != 0 || endClick.Y != 0))
            {
                System.Drawing.Pen myPen = new Pen(System.Drawing.Brushes.DarkBlue, 2);
                g.DrawLine(myPen, baseClick, endClick);
                EventSource.updateTess(this);
            }
        }

        public string getTessData(){
            string ret = "";
            ret += "<baseclick_x>" + baseClick.X + "</baseclick_x>";
            ret += "<baseclick_y>" + (this.Height - baseClick.Y) + "</baseclick_y>";
            ret += "<endclick_x>" + endClick.X + "</endclick_x>";
            ret += "<endclick_y>" + (this.Height - endClick.Y) + "</endclick_y>";
            ret += "<populateByTess>" + populateByTess.ToString() + "</populateByTess>";
            return ret;
        }

        private double mod(double x, double m)
        {
            return (x % m + m) % m;
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
            pictureBox1.Update();
            this.Invalidate();
            this.Update();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Y >= pictureBox1.Height - 10){
                baseClick = e.Location;
            }
            else{
                if (e.Y < pictureBox1.Height - 10)
                {
                    endClick = e.Location;
                }
            }
            populateByTess = true;
            pictureBox1.Invalidate();
            pictureBox1.Update();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            populateByTess = true;
            if(lastScrollValue == -1){
                lastScrollValue = trackBar1.Value;
            }
            else{
                if(lastScrollValue != trackBar1.Value){
                    baseClick.X += trackBar1.Value - lastScrollValue;
                    endClick.X += trackBar1.Value - lastScrollValue;
                    lastScrollValue = trackBar1.Value;
                }
                pictureBox1.Invalidate();
                pictureBox1.Update();
            }
        }
    }
}
