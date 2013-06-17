using OrbitMapper.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Drawing.Drawing2D;

namespace OrbitMapper.Shapes
{
    /// <summary>
    /// Base class for shape, not designed to be used directly as a TabPage. Create a custom class, extend this Shape base class, and configure it according to the shape you would like to add.
    /// </summary>
    public class Shape : TabPage
    {

        #region Initialization of member fields
        private List<DoublePoint> vertices = new List<DoublePoint>();
        private static int shapeCount = 0;
        private List<Intersect> collisions;
        private int numWalls;
        private List<DoublePoint> walls;
        private double width = 0;
        private double height = 0;
        private double scaleX;
        private double scaleY;
        private double startPoint = 0;
        private double startAngle = 0;
        private int bounces = 0;
        private DoublePoint startArea = new DoublePoint();
        private bool fromTessellation = false;
        private double tessShapeHeight;
        private double distance = 0;
        private bool isNotScaled = true;
        /// <summary>
        /// Used to determine whether undefined behavior has been detected
        /// </summary>
        public bool undefCollision = false;
        private double ratio = 0;
        private ContextMenu cm;
        #endregion

        /// <summary>
        /// Initialize some objects and add the context menu for when you right click a tab
        /// </summary>
        public Shape()
        {
            collisions = new List<Intersect>();
            walls = new List<DoublePoint>();
            this.CausesValidation = true;
            // Set the Context Menu of this instance referenced by the TabControl this will be placed ino to be the context menu created below.
            cm = new ContextMenu();
            cm.MenuItems.Add("Remove", new EventHandler(removeThisTab));
            this.ContextMenu = cm;
            shapeCount++;
            base.BackColor = Color.White;
        }

        /// <summary>
        /// The ratio is used for scaling the shape.
        /// </summary>
        /// <param name="ratio"></param>
        public void setRatio(double ratio)
        {
            this.ratio = ratio;
        }

        /// <summary>
        /// Gets the width of the shape, this will be larger than the tabs width, it is the actual size we do calculations on
        /// </summary>
        /// <returns></returns>
        public int getShapeWidth()
        {
            return (int)this.width;
        }

        /// <summary>
        /// Gets the height of the shape, this will be larger than the tabs height, it is the actual size we do calculations on
        /// </summary>
        /// <returns></returns>
        public int getShapeHeight()
        {
            return (int)this.height;
        }

        /// <summary>
        /// Get shape data is called by MainForm when it getting saved.
        /// </summary>
        /// <returns></returns>
        public string getShapeData()
        {
            string data = "";
            if (startPoint <= 0 || startPoint >= 1 || startAngle <= 0 || startAngle >= 180)
                return null;
            data += "<startpoint>" + startPoint + "</startpoint>";
            data += "<startangle>" + startAngle + "</startangle>";
            data += "<bounces>" + bounces + "</bounces>";
            data += "<ratio>" + this.ratio + "</ratio>";
            return data;
        }

        /// <summary>
        /// Method that triggers the event that is handled by MainForm to remove the tab from the TabControl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void removeThisTab(Object sender, EventArgs e)
        {
            EventSource.removeTab(this.Name);
        }

        /// <summary>
        /// Get the whole start area
        /// </summary>
        /// <returns></returns>
        public DoublePoint getStartArea()
        {
            return startArea;
        }

        /// <summary>
        /// Get the Shape count (
        /// </summary>
        /// <returns></returns>
        public int getShapeCount()
        {
            return shapeCount;
        }

        /// <summary>
        /// This is the area on the shape along the base that the user can start a projection from.
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        public void setStartArea(double x1, double x2)
        {
            startArea.x1 = x1;
            startArea.x2 = x2;
        }

        /// <summary>
        /// The is the Shape's way of keeping track of whether the collision is going to use tessellation data to start the collision simulation
        /// or whether it's going to used the pulled information from the text boxes.
        /// </summary>
        /// <param name="isFromTess"></param>
        public void setFromTessellation(bool isFromTess)
        {
            fromTessellation = isFromTess;
        }

        /// <summary>
        /// Set the starting point for the shape, this is the area along the base that a user would be able to start a projection from
        /// </summary>
        /// <param name="startPoint"></param>
        public void setStartPoint(double startPoint)
        {
            this.startPoint = startPoint;
        }

        /// <summary>
        /// Set the tessellations shape height (used to determined where to start repeating vertically
        /// </summary>
        /// <param name="h"></param>
        public void setTessShapeHeight(double h)
        {
            tessShapeHeight = h;
        }

        /// <summary>
        /// Used to set the distance pulled from the baseClick and the endClick. It sets isNotScaled to true so that the we know that it still has yet to be scaled
        /// to the distance we will used here for calculating the collisions, scaled up for the size of the shape.
        /// </summary>
        /// <param name="d"></param>
        public void setDistance(double d)
        {
            distance = d;
            isNotScaled = true;
        }

        /// <summary>
        /// Get the starting point
        /// </summary>
        /// <returns></returns>
        public double getStartPoint()
        {
            return startPoint;
        }

        /// <summary>
        /// Set the starting angle of the projection in degrees
        /// </summary>
        /// <returns></returns>
        public void setStartAngle(double startAngle)
        {
            this.startAngle = startAngle;
        }

        /// <summary>
        /// Get the starting angle of the projection in degrees
        /// </summary>
        /// <returns></returns>
        public double getStartAngle()
        {
            return startAngle;
        }

        /// <summary>
        /// Set the number of bounces that you would like to simulation
        /// </summary>
        /// <returns></returns>
        public void setBounces(int bounces)
        {
            this.bounces = bounces;
        }

        /// <summary>
        /// Get the number of bounces
        /// </summary>
        /// <returns></returns>
        public int getBounces()
        {
            return bounces;
        }

        /// <summary>
        /// This vertex is used for drawing the collision to the TabPage and for detecting collisions.
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="angle"></param>
        public void addVertex(double x1, double x2, double angle)
        {
            // Add to the numWalls (used for iterating over them without having to use Count.
            numWalls++;
            this.walls.Add(new DoublePoint(numWalls - 1, angle)); // Vertices are stored as Double for accuracy
            vertices.Add(new DoublePoint(x1, x2));
            if (x1 > this.width)
            { // If the X or Y position is greater than the width or height, increase the width or height for the user automatically
                this.width = x1;
                this.scaleX = x1;
            }
            if (x2 > this.height)
            {
                this.height = x2;
                this.scaleY = x2;
            }
        }

        private int getVerticesCount()
        {
            return vertices.Count();
        }

        private DoublePoint vertexAt(int index)
        {
            return vertices.ElementAt<DoublePoint>(index);
        }

        /// <summary>
        /// This uses this distance to determine when to stop the collision simulation as oppose to the bounces, then calculates the bounces.
        /// </summary>
        private void findIntersectionsFromTess()
        {
            // If the starting point or angle entered was not right, don't continue
            if (startPoint <= 0 || startPoint >= 1 || startAngle <= 0 || startAngle >= 180)
                return;

            // Clear the last simulation done from the collisions List
            collisions.Clear();
            
            // Find the total starting width, multiply it by the ratio entered (0,1) and add it to the startareas x1 position
            // If you're confused, this is for shapes like the hexagon where the starting area is only a small portion of the shapes width
            double startingPoint = startArea.x1 + (startPoint * Math.Abs(startArea.x1 - startArea.x2));

            // Reset any member fields
            this.undefCollision = false;
            double totalDistance = 0;
            // If the distance from the mouse clicks has not yet been scaled up, scale it up
            if (isNotScaled)
            {
                distance = (this.height / tessShapeHeight) * distance;
                isNotScaled = false;
            }
            DoublePoint tempProjection = null;
            int i = 0;
            bounces = 0;
            // Set up the first intersect and data before running through the loop
            Intersect tempInt = new Intersect();
            tempInt.x1 = startingPoint;
            tempInt.x2 = 0d;
            tempInt.wall = 0;
            tempInt.angle = startAngle;
            collisions.Add(tempInt);
            // Perform the loop for each of the bounces
            // At the end of the loop, set the values of intersect[] for i + 1
            // Determine the distance traveled and if it not yet more than the scaled up distance, keep looping and getting more collisions
            while (totalDistance <= distance - 0.000001d)
            {
                Intersect curIntersect = collisions.ElementAt<Intersect>(i);
                // Temporary variable for finding the collisions with EACH of the walls (to determine which is the least distance
                Intersect[] tempIntersects = new Intersect[numWalls];
                for (int j = 0; j < tempIntersects.Length; j++)
                {
                    tempIntersects[j] = new Intersect();
                }
                // leastDistance is to choose the index of tempIntersects which has the right wall
                int leastDistance = -1;
                // Find the project from the current point, longest possible distance, angle of reflection.
                tempProjection = MathUtilities.findProjection(curIntersect.x1, curIntersect.x2, width, curIntersect.angle);
                double x3 = tempProjection.x1;
                double x4 = tempProjection.x2;
                // For each of the walls
                for (int j = 0; j < numWalls; j++)
                {
                    // If the wall is not the wall that the last collision took place
                    if (curIntersect.wall != j)
                    {
                        int tempMod = (int)MathUtilities.mod(j - 1, numWalls); // Use tempMod to find the correct vertex to determine a line for the current wall
                        // Find the intersect with the current wall
                        DoublePoint temp = MathUtilities.getIntersect(curIntersect.x1, curIntersect.x2, x3, x4, vertexAt(tempMod).x1, vertexAt(tempMod).x2, vertexAt(j).x1, vertexAt(j).x2);
                        if (temp == null)
                            continue;

                        tempIntersects[j].x1 = temp.x1;
                        tempIntersects[j].x2 = temp.x2;
                        tempIntersects[j].wall = j;
                        tempIntersects[j].distance = Math.Sqrt(Math.Pow(curIntersect.x1 - tempIntersects[j].x1, 2) + Math.Pow(curIntersect.x2 - tempIntersects[j].x2, 2));

                        if(MathUtilities.isValidIntersect(vertices, tempIntersects[j], j))
                        {   // If it is valid and leastDistance has not yet been set, set it to this index
                            if (leastDistance == -1)
                            {
                                leastDistance = j;
                            }
                            else
                            {   // If it valid and leastDistance has been set, check to see that the new distance is more than the distance at the index marked
                                // by leastDistance, if it is, set leastDistance equal to this index
                                if (tempIntersects[leastDistance].distance > tempIntersects[j].distance)
                                {
                                    leastDistance = j;
                                }
                            }
                        }
                    }
                }
                // After leastDistance has been determined for the current iteration of the algorithm, test to make sure it's not equal to any
                // of the current shapes vertices (which would be illegal)
                for (int j = 0; j < numWalls; j++)
                {
                    if (tempIntersects[leastDistance].x1 == vertexAt(j).x1 && tempIntersects[leastDistance].x2 == vertexAt(j).x2)
                        this.undefCollision = true;
                }
                // If so, call the event the triggers this and return from this algorithm. Going further could crash this draw function rendering the shape useless
                // .NET quarantines the draw function when it throws an exception for this shape, only way to get around it is to delete the tab containing the shape or close the program
                if (this.undefCollision)
                {
                    EventSource.finishedDrawTess(bounces);
                    EventSource.finishedDrawShape();
                    return;
                }
                // With the current collision, find the angle from the last intersect to this one
                double projection = MathUtilities.findAngle(curIntersect.x1, curIntersect.x2, tempIntersects[leastDistance].x1, tempIntersects[leastDistance].x2);
                tempIntersects[leastDistance].angle = MathUtilities.findReflection(projection, walls[leastDistance].x2);
                tempIntersects[leastDistance].wall = leastDistance;
                // Add the tempIntersect to the collisions container
                collisions.Add(tempIntersects[leastDistance]);
                totalDistance += tempIntersects[leastDistance].distance;
                bounces++;
                i++;
            }
            EventSource.finishedDrawTess(bounces);
            EventSource.finishedDrawShape();
        }

        /// <summary>
        /// This uses the bounces to calculate when to stop the collision simulation.
        /// </summary>
        private void findIntersections()
        {

            // If the user wants to generate the collisions from the tessellation, branch to the method that does it, then return
            if (fromTessellation)
            {
                findIntersectionsFromTess();
                return;
            }

            // If the starting point or angle entered was not right, don't continue
            if (startPoint <= 0 || startPoint >= 1 || startAngle <= 0 || startAngle >= 180 || bounces <= 0)
                return;

            // Clear the last simulation done from the collisions List
            collisions.Clear();

            // Find the total starting width, multiply it by the ratio entered (0,1) and add it to the startareas x1 position
            // If you're confused, this is for shapes like the hexagon where the starting area is only a small portion of the shapes width
            double startingPoint = startArea.x1 + (startPoint * Math.Abs(startArea.x1 - startArea.x2));

            DoublePoint tempProjection = null;
            // Reset any member fields
            this.undefCollision = false;
            // Set up the first intersect and data before running through the loop
            Intersect tempInt = new Intersect();
            tempInt.x1 = startingPoint;
            tempInt.x2 = 0d;
            tempInt.wall = 0;
            tempInt.angle = startAngle;
            collisions.Add(tempInt);
            // Perform the loop for each of the bounces
            // At the end of the loop, set the values of intersect[] for i + 1
            for (int i = 0; i < bounces; i++)
            {
                Intersect curIntersect = collisions.ElementAt<Intersect>(i);
                // Temporary variable for finding the collisions with EACH of the walls (to determine which is the least distance
                Intersect[] tempIntersects = new Intersect[numWalls];
                for (int j = 0; j < tempIntersects.Length; j++)
                {
                    tempIntersects[j] = new Intersect();
                }
                // leastDistance is to choose the index of tempIntersects which has the right wall
                int leastDistance = -1;
                // Find the project from the current point, longest possible distance, angle of reflection.
                tempProjection = MathUtilities.findProjection(curIntersect.x1, curIntersect.x2, width, curIntersect.angle);
                double x3 = tempProjection.x1;
                double x4 = tempProjection.x2;
                // For each of the walls
                for (int j = 0; j < numWalls; j++)
                {
                    // If the wall is not the wall that the last collision took place
                    if (curIntersect.wall != j)
                    {
                        int tempMod = (int)MathUtilities.mod(j - 1, numWalls); // Use tempMod to find the correct vertex to determine a line for the current wall
                        // Find the intersect with the current wall
                        DoublePoint temp = MathUtilities.getIntersect(curIntersect.x1, curIntersect.x2, x3, x4, vertexAt(tempMod).x1, vertexAt(tempMod).x2, vertexAt(j).x1, vertexAt(j).x2);
                        if (temp == null)
                            continue;
                        tempIntersects[j].x1 = temp.x1;
                        tempIntersects[j].x2 = temp.x2;
                        tempIntersects[j].wall = j;
                        tempIntersects[j].distance = Math.Sqrt(Math.Pow(curIntersect.x1 - tempIntersects[j].x1, 2) + Math.Pow(curIntersect.x2 - tempIntersects[j].x2, 2));

                        // Run through the current walls' vertices and determine the min and max for the x position and min and max for the y position
                        DoublePoint minXPoint = new DoublePoint();
                        DoublePoint maxXPoint = new DoublePoint();
                        DoublePoint minYPoint = new DoublePoint();
                        DoublePoint maxYPoint = new DoublePoint();
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
                        // Use this data to determine whether the collision we found is actually a valid collision (inside the shape)
                        if (tempIntersects[j].distance > 0 &&
                            tempIntersects[j].x1 >= minXPoint.x1 && tempIntersects[j].x1 <= maxXPoint.x1 &&
                            tempIntersects[j].x2 >= minYPoint.x2 && tempIntersects[j].x2 <= maxYPoint.x2)
                        {   // If it is valid and leastDistance has not yet been set, set it to this index
                            if (leastDistance == -1)
                            {
                                leastDistance = j;
                            }
                            else
                            {   // If it valid and leastDistance has been set, check to see that the new distance is more than the distance at the index marked
                                // by leastDistance, if it is, set leastDistance equal to this index
                                if (tempIntersects[leastDistance].distance > tempIntersects[j].distance)
                                {
                                    leastDistance = j;
                                }
                            }
                        }
                    }
                }
                // After leastDistance has been determined for the current iteration of the algorithm, test to make sure it's not equal to any
                // of the current shapes vertices (which would be illegal)
                for (int j = 0; j < numWalls; j++)
                {
                    if (tempIntersects[leastDistance].x1 == vertexAt(j).x1 && tempIntersects[leastDistance].x2 == vertexAt(j).x2)
                        this.undefCollision = true;
                }
                // If there was an undefined collision found, return and trigger the event that will fill in the boxes with Undef if it detects it
                if (this.undefCollision)
                {
                    EventSource.finishedDrawShape();
                    return;
                }
                // Set fields for the tempIntersect and add it to the current collisions
                double projection = MathUtilities.findAngle(curIntersect.x1, curIntersect.x2, tempIntersects[leastDistance].x1, tempIntersects[leastDistance].x2);
                tempIntersects[leastDistance].angle = MathUtilities.findReflection(projection, walls[leastDistance].x2);
                tempIntersects[leastDistance].wall = leastDistance;
                collisions.Add(tempIntersects[leastDistance]);
            }
            EventSource.finishedDrawShape();
        }

        /// <summary>
        /// Scale the shape down to match the tabPage size
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private int scale(double x)
        {
            double tabWidth = base.Width - 10;
            double tabHeight = base.Height - 10;
            double scaledX = tabWidth / scaleX;
            double scaledY = tabHeight / scaleY;
            // If x direction needs more scaling down, use that
            if (scaledX < scaledY)
            {
                return (int)(scaledX * x);
            }
            // Otherwise, use the y direction's scale
            else
            {
                return (int)(scaledY * x);
            }
        }

        /// <summary>
        /// Return all of the collisions detect in a friendly format to iterate through and use a Graphics.drawLine method
        /// </summary>
        /// <returns></returns>
        public Point[] getCollisions()
        {
            int count = collisions.Count;
            if (count == 0)
                return null;
            Point[] temp = new Point[count];
            EventSource.output(count + " collisions.");
            for (int i = 0; i < count; i++)
            {
                temp[i] = new Point((int)((base.Width / 2) - (scale(this.width) / 2) + scale(collisions.ElementAt<Intersect>(i).x1)), base.Height - 1 - scale(collisions.ElementAt<Intersect>(i).x2));
            }
            return temp;
        }

        /// <summary>
        /// Return the vetices for the shape in a friendly format to insert into a Graphics.drawPoly method
        /// </summary>
        /// <returns></returns>
        public Point[] getVertices()
        {
            List<Point> temp = new List<Point>();
            EventSource.output(vertices.Count() + " vertices.");
            for (int i = 0; i < vertices.Count(); i++)
            {
                temp.Add(new Point((base.Width / 2) - (scale(this.width) / 2) + scale(vertices.ElementAt<DoublePoint>(i).x1), base.Height - 1 - scale(vertices.ElementAt<DoublePoint>(i).x2)));
            }
            return temp.ToArray<Point>();
        }

        /// <summary>
        /// Get the current tab height (used for scaling)
        /// </summary>
        /// <returns></returns>
        public int getTabHeight()
        {
            return base.Height;
        }

        /// <summary>
        /// Get the current tab width (used for scaling)
        /// </summary>
        /// <returns></returns>
        public int getTabWidth()
        {
            return base.Width;
        }

        /// <summary>
        /// Paint method to custom draw the shape and the intersections on a tab page
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (getVerticesCount() != 0)
            {
                // Create the graphics context and clear it of the last draw
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.White);

                // Get the shape, intiate the collision simulation, and get the collisions in a Point array
                Point[] shape = getVertices();
                findIntersections();
                Point[] collisions = getCollisions();
                EventSource.output("Drawing " + shape.Length + " vertices.");
                g.DrawPolygon(System.Drawing.Pens.Black, shape);
                if (collisions != null && !this.undefCollision) // If there are no collisions or there was an undefined collision we don't want to draw
                {
                    EventSource.output("Intersections detected: " + collisions.Length);
                    for (int i = 0; i < collisions.Length - 1; i++)
                    {
                        if (i == 0) // Mark the start point with a blue dot
                            g.FillEllipse(new SolidBrush(Color.SteelBlue), collisions[i].X - 5, collisions[i].Y - 6, 10, 10);
                        else
                            if (i == collisions.Length - 2) // Mark the end point with a red dot
                                g.FillEllipse(new SolidBrush(Color.Tomato), collisions[i + 1].X - 4, collisions[i + 1].Y - 5, 8, 8);
                        g.DrawLine(System.Drawing.Pens.Red, collisions[i], collisions[i + 1]);
                    }
                }
            }
        }
    }
}