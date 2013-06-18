using OrbitMapper;
using OrbitMapper.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace OrbitMapper.Tessellations
{
    /// <summary>
    /// Tessellation is meant to be used exactly the same as a button, grid, slider, tab page.
    /// Inheriting from UserControl allows you to have this functionality as well as have this and its children 
    /// show up in the add controls toolbox for C# Forms Designer ;)
    /// </summary>
    public partial class Tessellation : UserControl
    {
        private Pattern pattern = null;
        private Point baseClick = new Point(0, 0);
        private Point endClick = new Point(0, 0);
        private Point lastPictureBoxState = new Point();
        private List<Point[]> startZones = new List<Point[]>();
        private List<Point[]> reflectedStartZones = new List<Point[]>();
        /// <summary>
        /// Field used to determine whether the last valid algorithm determined that the base was valid.
        /// </summary>
        public bool baseIsGood = true;
        private bool hasReflectedZones = false;
        private double startingPoint;
        private double startingAngle;
        private double distance;
        private int lastScrollValue = -1;
        private double shapeHeight;
        /// <summary>
        /// Field used to determine when to use this tessellation for drawing the collisions
        /// </summary>
        public bool populateByTess = false;
        /// <summary>
        /// Field used when the splitter is being used in tandem with the hide button and tab switching to help determine the proper sizing we need.
        /// </summary>
        public static int lastWidth = 0;
        private Point lastClick = new Point(0, 0);
        private Point offset = new Point(0, 0);
        private bool inRegularZone = true;

        /// <summary>
        /// TODO
        /// </summary>
        public Tessellation()
        {
            InitializeComponent();
            this.TabStop = false;
            // Fill out completely wherever you put this control
            this.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public double getStartingPoint()
        {
            return startingPoint;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public double getStartingAngle()
        {
            return startingAngle;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public double getDistance()
        {
            return distance;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public PictureBox getPictureBox()
        {
            return pictureBox1;
        }

        public Point getBaseClick()
        {
            return this.baseClick;
        }

        public Point getEndClick()
        {
            return this.endClick;
        }

        public Point getOffset()
        {
            return this.offset;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public void addStartZone(Point p1, Point p2)
        {
            startZones.Add(new Point[] { p1, p2 });
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="pos"></param>
        public void setBasePos(Point pos)
        {
            baseClick.X = pos.X;
            baseClick.Y = pos.Y;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="pos"></param>
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

        /// <summary>
        /// Sets the shapes height
        /// </summary>
        /// <param name="height"></param>
        public void setShapeHeight(double height)
        {
            shapeHeight = height;
        }

        /// <summary>
        /// Gets the shapes height
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public Pattern getPattern()
        {
            return pattern;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="pat"></param>
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

            bool isReadyForTest = (baseClick.X != 0 || baseClick.Y != 0) && (endClick.X != 0 || endClick.Y != 0);
            // If the baseClick or endClick has been specified...
            if (isReadyForTest)
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

            int iterX = this.Width / getPattern().iWidth + 6;
            int iterY = this.Height / getPattern().iHeight + 2;
            // What divPatWidth and divPatHeight are used for is to determine the area of iterations that this pattern should run through while drawing in order
            // for the pattern to not end while using your mouse to scroll the map beyond the bounds of the picturebox.
            // This method to create a dynamic maps such as this is a necessary so that you are not trying to draw things that are not even visible to the container.
            // Although, there is a bit of an overlap purposely on the left, right and top because many patterns would simply end while scrolling because they are so large jagged.
            int divPatWidth = (-offset.X)/getPattern().iWidth;
            int divPatHeight = (-offset.Y)/getPattern().iHeight;
            bool baseIsCorrect = false;
            // Run through the iterations in both directions (it draws the Y direction for each X first).
            for (int i = divPatWidth; i < iterX + divPatWidth; i++)
            {
                int iMultGetPatWidth = i * getPattern().iWidth;
                for (int j = divPatHeight; j < iterY + divPatHeight; j++)
                {
                    int jMultGetPatHeight = j * getPattern().iHeight;
                    for (int k = 0; k < patterns.Count(); k++)
                    {
                        // Draw each pattern
                        Point[] poly = new Point[patterns.ElementAt<Point[]>(k).Count()];
                        bool isShapeCollided = false;
                        for (int l = 0; l < patterns.ElementAt<Point[]>(k).Count(); l++)
                        {
                            Point temp = new Point(iMultGetPatWidth + patterns[k][l].X, getPictureBox().Height - 5 - jMultGetPatHeight - patterns[k][l].Y);
                            // This is applied every other row because some patterns do not populate the map perfectly straight up.
                            // For example, the equilateral pattern must be applied at a slight horizontal offset otherwise the pattern would not match up as we draw
                            if (j % 2 == 1)
                                temp.X -= getPattern().iOffset;
                            // This is applied at every iteration, we will subtract 3 times the width to not mess up the iOffset and give it some overlap to the left
                            temp.X -= getPattern().iWidth * 3 - offset.X;
                            temp.Y -= offset.Y;
                            poly[l] = temp;

                            // Algorithm for highlighting the shapes that have a collision
                            int tempMod = (int)MathUtilities.mod(l - 1, patterns.ElementAt<Point[]>(k).Count()); // Use tempMod to find the correct vertex to determine a line for the current wall

                            Intersect tempIntersect = new Intersect();
                            if (!isShapeCollided)
                            {
                                if (l > 0)
                                {
                                    if (MathUtilities.isValidIntersect((double)poly[tempMod].X, (double)poly[tempMod].Y, (double)poly[l].X, (double)poly[l].Y, (double)baseClick.X + offset.X, (double)baseClick.Y - offset.Y, (double)endClick.X + offset.X, (double)endClick.Y - offset.Y))
                                    {
                                        isShapeCollided = true;
                                    }
                                }
                            }
                        }
                        if (!isShapeCollided || !isReadyForTest)
                            g.DrawPolygon(System.Drawing.Pens.Black, poly);
                        else if(isReadyForTest)
                        {
                            g.DrawPolygon(new System.Drawing.Pen(System.Drawing.Brushes.Red, 3), poly);
                        }
                    }
                    #region Logic for getting data from mouse click
                    // I preface this logic by saying that I am now looking back at it 6+ months later to document it, I'll try my best.
                    // Make sure both the base and end clicks are set by the user
                    // If so, then check to see if it is between a small offset of +- 5 pixels in order to anchor it to a baseline.
                    // The pitfall with this is that if the pattern has multiple shapes draw vertically, it can only anchor it to the top of the entire pattern itself.
                    // This will not be accurate if the baseClick is not currently in view (including the overlap), we will account for this later.
                    if (((baseClick.X != 0 || baseClick.Y != 0) && (endClick.X != 0 || endClick.Y != 0)) &&
                    (endClick.Y >= getPictureBox().Height - 5 - jMultGetPatHeight - 5 &&
                    endClick.Y <= getPictureBox().Height - 5 - jMultGetPatHeight + 5))
                    {
                        endClick.Y = getPictureBox().Height - 5 - jMultGetPatHeight;
                    }
                    // Do the same for the baseline
                    if (((baseClick.X != 0 || baseClick.Y != 0) && (endClick.X != 0 || endClick.Y != 0)) &&
                    (baseClick.Y >= getPictureBox().Height - 5 - jMultGetPatHeight - 5 &&
                    baseClick.Y <= getPictureBox().Height - 5 - jMultGetPatHeight + 5))
                    {
                        baseClick.Y = getPictureBox().Height - 5 - jMultGetPatHeight;
                        #region Determine the starting areas for the mouse click and weather it's in a reflected or regular
                        // If the current click is between any starting area at all, reflected or not...
                        if (betweenStartZones(baseClick.X - iMultGetPatWidth))
                        {
                            // zone will be used to determine which starting area it's in, in order to get the correct X position data for that pattern's start
                            int zone;
                            // Set zone equal to whatever reflectedstartzone(the current X position in relation to the entire pattern) returns
                            // If it is non-negative, it means that it is between a reflected start area
                            if ((zone = betweenReflectedStartZones(baseClick.X - iMultGetPatWidth)) > -1)
                            {
                                startingPoint = (double)(reflectedStartZones.ElementAt<Point[]>(zone)[1].X - (baseClick.X - iMultGetPatWidth))/*The X position of the point along the zones width*/ / (double)(reflectedStartZones.ElementAt<Point[]>(zone)[1].X - reflectedStartZones.ElementAt<Point[]>(zone)[0].X)/*The zones entire width*/;
                                // Use ArcTan to find the and using the baseclick's x and y and the endclick's x and y, then mod with 180 degrees because it is a reflected area
                                startingAngle = Math.Atan((double)(endClick.Y - baseClick.Y) / (double)(endClick.X - baseClick.X)) * 180d / Math.PI;
                                startingAngle = MathUtilities.mod(startingAngle, 180);
                                distance = Math.Sqrt(Math.Pow(endClick.Y - baseClick.Y, 2) + Math.Pow(endClick.X - baseClick.X, 2));
                                inRegularZone = false;
                            }
                            // The baseclick must then be between a regular start zone
                            else
                            {
                                startingPoint = (double)(baseClick.X - iMultGetPatWidth - startZones.ElementAt<Point[]>(0)[0].X) / (double)(startZones.ElementAt<Point[]>(0)[1].X - startZones.ElementAt<Point[]>(0)[0].X);
                                startingAngle = Math.Atan((double)(endClick.Y - baseClick.Y) / (double)(endClick.X - baseClick.X)) * 180d / Math.PI;
                                if (baseClick.X > endClick.X)
                                    startingAngle = 180 - startingAngle;
                                startingAngle = Math.Abs(startingAngle);
                                distance = Math.Sqrt(Math.Pow(endClick.Y - baseClick.Y, 2) + Math.Pow(endClick.X - baseClick.X, 2));
                                inRegularZone = true;
                            }
                            // Since the baseClick was between one of the starting areas, we'll set this true. Used when reporting back to the MainForm
                            baseIsCorrect = true;
                        }
                        #endregion
                    }
                    #endregion
                }
            }
            // Set the field to what we determined during the algorithm only if we know that the results above are valid 
            // (they will not be valid if the current offset makes the baseclick not visible or in the slight overlap)
            if (baseClick.X + offset.X >= 0 && baseClick.X + offset.X <= getPictureBox().Width && offset.Y >= 0 && offset.Y <= getPictureBox().Height)
                baseIsGood = baseIsCorrect;
            else
            {
                // If the last valid algorithm determined that the base is valid, we know it hasn't changed (because the base is out of view, including the overlap)
                if (baseIsGood)
                {
                    // Set all the properties EXCEPT that starting position (since it couldn't have changed) according to whether the last known base was in a regular or reflected zone
                    if (inRegularZone)
                    {
                        startingAngle = Math.Atan((double)(endClick.Y - baseClick.Y) / (double)(endClick.X - baseClick.X)) * 180d / Math.PI;
                        if (baseClick.X > endClick.X)
                            startingAngle = 180 - startingAngle;
                        startingAngle = Math.Abs(startingAngle);
                        distance = Math.Sqrt(Math.Pow(endClick.Y - baseClick.Y, 2) + Math.Pow(endClick.X - baseClick.X, 2));
                    }
                    else
                    {
                        // Use ArcTan to find the and using the baseclick's x and y and the endclick's x and y, then mod with 180 degrees because it is a reflected area
                        startingAngle = Math.Atan((double)(endClick.Y - baseClick.Y) / (double)(endClick.X - baseClick.X)) * 180d / Math.PI;
                        startingAngle = MathUtilities.mod(startingAngle, 180);
                        distance = Math.Sqrt(Math.Pow(endClick.Y - baseClick.Y, 2) + Math.Pow(endClick.X - baseClick.X, 2));
                    }
                }
            }

            // If the baseClick or endClick has been specified...
            if (isReadyForTest)
            {
                System.Drawing.Pen myPen = new Pen(System.Drawing.Brushes.DarkBlue, 2);
                //In order to draw the base click and end click in their correct positions, we must add the offset when drawing them
                g.DrawLine(myPen, new Point(baseClick.X + offset.X, baseClick.Y - offset.Y), new Point(endClick.X + offset.X, endClick.Y - offset.Y));
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
        /// When the user resizes the windows trigger a redraw.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
            this.Invalidate();
        }

        /// <summary>
        /// Sets the respective baseclick or endclick depending on where the user clicks.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // If the user clicked near the bottom, it was a baseclick
                if (e.Y >= pictureBox1.Height - 10)
                {
                    //Add the current offsets to get the click's absolute position
                    baseClick.X = e.Location.X - offset.X;
                    baseClick.Y = e.Location.Y + offset.Y;
                }
                // Otherwise, it was an endclick.
                else
                {
                    //Add the current offsets to get the click's absolute position
                    if (e.Y < pictureBox1.Height - 10)
                    {
                        endClick.X = e.Location.X - offset.X;
                        endClick.Y = e.Location.Y + offset.Y;
                    }
                }
                // If the user clicked in here, then they must want us to use this data to find the collisions
                populateByTess = true;
                pictureBox1.Invalidate();
            }
        }

        /// <summary>
        /// Start tracking the mouse movement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //This is called once per mouse click, set lastclick to the current location so that it is know before we start triggering the mousemove event from old data.
            if (e.Button == MouseButtons.Right)
            {
                lastClick = e.Location;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //Calculate the offset from the mouse's current position and the mouse's last known position
                offset.X += e.Location.X - lastClick.X;
                offset.Y -= e.Location.Y - lastClick.Y;
                lastClick = e.Location;

                if (offset.Y > 0)
                    offset.Y = 0;

                pictureBox1.Invalidate();
            }
        }

        /// <summary>
        /// Shortcut for moving the user back to the origin in the tessellation map.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            //Return the offsets back to the origin and redraw
            offset.X = 0;
            offset.Y = 0;
            pictureBox1.Invalidate();
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
                //Return the offsets back to the origin and redraw
                offset.X = 0;
                offset.Y = 0;
                pictureBox1.Invalidate();
            }
        }
    }
}
