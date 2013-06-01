using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using OrbitMapper;

namespace OrbitMapper.Tessellations
{
    /// <summary>
    /// Tessellation is meant to be used exactly the same as a button, grid, slider, tab page.
    /// Inheriting from UserControl allows you to have this functionality as well as have this and its children 
    /// show up in the add controls toolbox for C# Forms Designer ;)
    /// </summary>
    public partial class Tessellation : UserControl
    {
        private Pattern pattern = new Pattern();
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

        public Tessellation()
        {
            InitializeComponent();
            this.TabStop = false;
            // Fill out completely wherever you put this control
            this.Dock = DockStyle.Fill;
        }

        public double getStartingPoint()
        {
            return startingPoint;
        }

        public double getStartingAngle()
        {
            return startingAngle;
        }

        public double getDistance()
        {
            return distance;
        }

        public PictureBox getPictureBox()
        {
            return pictureBox1;
        }

        public void addStartZone(Point p1, Point p2)
        {
            startZones.Add(new Point[] { p1, p2 });
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

        /// <summary>
        /// Set the starting point for the tessellation. If this is set, then the user will not want to run the simulation from the text boxes
        /// so set the populateByTess field accordingly. Invalidate the picture to do a redraw.
        /// </summary>
        /// <param name="pos"></param>
        public void setBaseClick(Point pos)
        {
            baseClick.X = pos.X;
            baseClick.Y = pos.Y;
            populateByTess = true;
            pictureBox1.Invalidate();
            pictureBox1.Update();
        }

        /// <summary>
        /// Set the end point for the tessellation. If this is set, then the user will not want to run the simulation from the text boxes
        /// so set the populateByTess field accordingly. Invalidate the picture box to do a redraw.
        /// </summary>
        /// <param name="pos"></param>
        public void setEndClick(Point pos)
        {
            populateByTess = true;
            endClick.X = pos.X;
            endClick.Y = pos.Y;
            pictureBox1.Invalidate();
            pictureBox1.Update();
        }

        /// <summary>
        /// Iterate through all of the start zones, if the integer given is between either reflectedstartzones or regular startzones, return true.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Add the reflected startzone and set the field to make use of it
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public void addReflectedStartZone(Point p1, Point p2)
        {
            reflectedStartZones.Add(new Point[] { p1, p2 });
            hasReflectedZones = true;
        }

        public void setShapeHeight(double height)
        {
            shapeHeight = height;
        }

        public double getShapeHeight()
        {
            return shapeHeight;
        }

        /// <summary>
        /// Iterate through all of the reflected startzones only and check if x is between them.
        /// </summary>
        /// <param name="x"></param>
        /// <returns>-1: If there are no reflected start zones. -2: If it's not between any of the existing ones. Nonnegative: The index of the start zone it is between.</returns>
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

        public Pattern getPattern()
        {
            return pattern;
        }

        public void setPattern(Pattern pat)
        {
            pattern = pat;
        }

        /// <summary>
        /// Draws the pattern in a tile-like fashion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(SystemColors.Control);

            // If the baseClick or endClick has been specified...
            if ((baseClick.X != 0 || baseClick.Y != 0) && (endClick.X != 0 || endClick.Y != 0))
            {
                // If the last state has not yet been specified, specify it
                if (lastPictureBoxState.Y == 0)
                {
                    lastPictureBoxState = new Point(pictureBox1.Width, pictureBox1.Height);
                }
                // Otherwise set the offset to draw the tessellations from so they anchor at the bottom and expand upward.
                else
                {
                    int offsetY = pictureBox1.Height - lastPictureBoxState.Y;
                    baseClick.Y += offsetY;
                    endClick.Y += offsetY;
                    lastPictureBoxState = new Point(pictureBox1.Width, pictureBox1.Height);
                }
            }

            // Get the patterns to tile and draw
            Point[][] patterns = getPattern().getPatterns();
            // Determine the number of iterations in the x direction and the number of iterations in the y direction
            // that need to be used in order to cover the entire tessellation area.
            // 2 was added so that I can draw it overlapping the User Controls draw area so that there is no whitespace near edges.
            int iterX = (int)(this.Width / getPattern().getWidth()) + 2;
            int iterY = (int)(this.Height / getPattern().getHeight()) + 2;
            bool baseIsCorrect = false;
            // Run through the iterations in both directions (it draws the Y direction for each X first).
            for (int i = 0; i < iterX; i++)
            {
                for (int j = 0; j < iterY; j++)
                {
                    for (int k = 0; k < patterns.Count(); k++)
                    {
                        // Draw each pattern
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
                    // I preface this logic by saying that I am now looking back at it 6+ months later to document it, I'll try my best.
                    // Make sure both the base and end clicks are set by the user
                    // If so, then check to see if it is between a small offset of +- 5 pixels in order to anchor it to a baseline.
                    // The pitfall with this is that if the pattern has multiple shapes draw vertically, it can only anchor it to the top of the entire pattern itself.
                    if (((baseClick.X != 0 || baseClick.Y != 0) && (endClick.X != 0 || endClick.Y != 0)) &&
                    (endClick.Y >= getPictureBox().Height - 5 - (int)(j * getPattern().getHeight()) - 5 &&
                    endClick.Y <= getPictureBox().Height - 5 - (int)(j * getPattern().getHeight()) + 5))
                    {
                        endClick.Y = getPictureBox().Height - 5 - (int)(j * getPattern().getHeight());
                    }
                    // Do the same for the baseline
                    if (((baseClick.X != 0 || baseClick.Y != 0) && (endClick.X != 0 || endClick.Y != 0)) &&
                    (baseClick.Y >= getPictureBox().Height - 5 - (int)(j * getPattern().getHeight()) - 5 &&
                    baseClick.Y <= getPictureBox().Height - 5 - (int)(j * getPattern().getHeight()) + 5))
                    {
                        baseClick.Y = getPictureBox().Height - 5 - (int)(j * getPattern().getHeight());
                        #region Determine the starting areas for the mouse click and weather it's in a reflected or regular
                        // If the current click is between any starting area at all, reflected or not...
                        if (betweenStartZones(baseClick.X - (int)(i * getPattern().getWidth())))
                        {
                            // zone will be used to determine which starting area it's in, in order to get the correct X position data for that pattern's start
                            int zone;
                            // Set zone equal to whatever reflectedstartzone(the current X position in relation to the entire pattern) returns
                            // If it is non-negative, it means that it is between a reflected start area
                            if ((zone = betweenReflectedStartZones(baseClick.X - (int)(i * getPattern().getWidth()))) > -1)
                            {
                                startingPoint = (double)(reflectedStartZones.ElementAt<Point[]>(zone)[1].X - (baseClick.X - (i * getPattern().getWidth())))/*The X position of the point along the zones width*/ / (double)(reflectedStartZones.ElementAt<Point[]>(zone)[1].X - reflectedStartZones.ElementAt<Point[]>(zone)[0].X)/*The zones entire width*/;
                                // Use ArcTan to find the and using the baseclick's x and y and the endclick's x and y, then mod with 180 degrees because it is a reflected area
                                startingAngle = Math.Atan((double)(endClick.Y - baseClick.Y) / (double)(endClick.X - baseClick.X)) * 180d / Math.PI;
                                startingAngle = mod(startingAngle, 180);
                                distance = Math.Sqrt(Math.Pow(endClick.Y - baseClick.Y, 2) + Math.Pow(endClick.X - baseClick.X, 2));
                            }
                            // The baseclick must then be between a regular start zone
                            else
                            {
                                startingPoint = (double)(baseClick.X - (i * getPattern().getWidth()) - startZones.ElementAt<Point[]>(0)[0].X) / (double)(startZones.ElementAt<Point[]>(0)[1].X - startZones.ElementAt<Point[]>(0)[0].X);
                                startingAngle = Math.Atan((double)(endClick.Y - baseClick.Y) / (double)(endClick.X - baseClick.X)) * 180d / Math.PI;
                                if (baseClick.X > endClick.X)
                                    startingAngle = 180 - startingAngle;
                                startingAngle = Math.Abs(startingAngle);
                                distance = Math.Sqrt(Math.Pow(endClick.Y - baseClick.Y, 2) + Math.Pow(endClick.X - baseClick.X, 2));
                            }
                            // Since the baseClick was between one of the starting areas, we'll set this true. Used when reporting back to the MainForm
                            baseIsCorrect = true;
                        }
                        #endregion
                    }
                    #endregion
                }
            }
            // Set the instances field to what we determined during the algorithm
            baseIsGood = baseIsCorrect;

            // If the baseClick or endClick has been specified...
            if ((baseClick.X != 0 || baseClick.Y != 0) && (endClick.X != 0 || endClick.Y != 0))
            {
                System.Drawing.Pen myPen = new Pen(System.Drawing.Brushes.DarkBlue, 2);
                g.DrawLine(myPen, baseClick, endClick);
                EventSource.updateTess(this);
            }
        }

        /// <summary>
        /// This is called by the MainForm when gathering data in order to save the Tessellation data with the corresponding Shape data to a file.
        /// </summary>
        /// <returns></returns>
        public string getTessData()
        {
            string ret = "";
            ret += "<baseclick_x>" + baseClick.X + "</baseclick_x>";
            ret += "<baseclick_y>" + (this.Height - baseClick.Y) + "</baseclick_y>";
            ret += "<endclick_x>" + endClick.X + "</endclick_x>";
            ret += "<endclick_y>" + (this.Height - endClick.Y) + "</endclick_y>";
            ret += "<populateByTess>" + populateByTess.ToString() + "</populateByTess>";
            return ret;
        }

        /// <summary>
        /// Custom mod formula for double because the regular modulo truncates double precision numbers to integers before performing the operation.
        /// This is much more expensive to perform than regualar Math.mod.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        private double mod(double x, double m)
        {
            return (x % m + m) % m;
        }

        /// <summary>
        /// When the user resizes the windows trigger a redraw.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
            pictureBox1.Update();
            this.Invalidate();
            this.Update();
        }

        /// <summary>
        /// Sets the respective baseclick or endclick depending on where the user clicks.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the user clicked near the bottom, it was a baseclick
            if (e.Y >= pictureBox1.Height - 10)
            {
                baseClick = e.Location;
            }
            // Otherwise, it was an endclick.
            else
            {
                if (e.Y < pictureBox1.Height - 10)
                {
                    endClick = e.Location;
                }
            }
            // If the user clicked in here, then they must want us to use this data to find the collisions
            populateByTess = true;
            pictureBox1.Invalidate();
            pictureBox1.Update();
        }

        /// <summary>
        /// The trackbar for the tessellation control shifts the start and the end point only in the X direction.
        /// If it is shifted before the redraw can be completed, the user will not see any redraw as they shift it until they either
        /// shift it more slowly or stop the trackbar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            populateByTess = true;
            if (lastScrollValue == -1)
            {
                lastScrollValue = trackBar1.Value;
            }
            else
            {
                if (lastScrollValue != trackBar1.Value)
                {
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
