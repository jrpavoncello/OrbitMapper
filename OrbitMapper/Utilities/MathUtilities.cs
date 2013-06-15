using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OrbitMapper;

namespace OrbitMapper.Utilities
{
    /// <summary>
    /// Contains solution wide methods to perform mostly common algebra
    /// </summary>
    public static class MathUtilities
    {
        /// <summary>
        /// Gets the intersection between the lines (x1, y1), (x2, y2) and (x3, y3), (x4, y4)
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="x3"></param>
        /// <param name="y3"></param>
        /// <param name="x4"></param>
        /// <param name="y4"></param>
        /// <returns></returns>
        public static DoublePoint getIntersect(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
        {
            DoublePoint ret = new DoublePoint();
            // Special case for when the first line is vertical
            if (x2 - x1 == 0)
            {
                double m2 = (y4 - y3) / (x4 - x3);
                double b2 = y3 - (m2 * x3);
                ret.x1 = x1;
                ret.x2 = (m2 * x1) + b2;
                return ret;
            }
            // Special case for when the first line is vertical
            else if (x4 - x3 == 0)
            {
                double m1 = (y2 - y1) / (x2 - x1);
                double b1 = y1 - (m1 * x1);
                ret.x1 = x3;
                ret.x2 = m1 * x3 + b1;
                return ret;
            }
            else
            {
                double m1 = (y2 - y1) / (x2 - x1); // If not those two, get the slope of both lines
                double m2 = (y4 - y3) / (x4 - x3);
                if (m1 != m2) // If they're not parallel
                {
                    double b1 = y1 - (m1 * x1); // Get their y-intercepts
                    double b2 = y3 - (m2 * x3); // Get their y-intercepts
                    ret.x1 = (b2 - b1) / (m1 - m2); // Solve for x of the interception
                    ret.x2 = m2 * ret.x1 + b2; // Solve for y of the interception
                    return ret;
                }
                return null;
            }
        }

        public static bool isValidIntersect(List<DoublePoint> wallVertices, int iVertexCollided, Intersect intersect)
        {
            int tempMod = (int)MathUtilities.mod(iVertexCollided - 1, wallVertices.Count); // Use tempMod to find the correct vertex to determine a line for the current wall

            // Run through the current walls' vertices and determine the min and max for the x position and min and max for the y position
            DoublePoint minXPoint = new DoublePoint();
            DoublePoint maxXPoint = new DoublePoint();
            DoublePoint minYPoint = new DoublePoint();
            DoublePoint maxYPoint = new DoublePoint();
            if (wallVertices[tempMod].x1 > wallVertices[iVertexCollided].x1)
            {
                minXPoint.x1 = wallVertices[iVertexCollided].x1;
                minXPoint.x2 = wallVertices[iVertexCollided].x2;
                maxXPoint.x1 = wallVertices[tempMod].x1;
                maxXPoint.x2 = wallVertices[tempMod].x2;
            }
            else
            {
                minXPoint.x1 = wallVertices[tempMod].x1;
                minXPoint.x2 = wallVertices[tempMod].x2;
                maxXPoint.x1 = wallVertices[iVertexCollided].x1;
                maxXPoint.x2 = wallVertices[iVertexCollided].x2;
            }

            if (wallVertices[tempMod].x2 > wallVertices[iVertexCollided].x2)
            {
                minYPoint.x1 = wallVertices[iVertexCollided].x1;
                minYPoint.x2 = wallVertices[iVertexCollided].x2;
                maxYPoint.x1 = wallVertices[tempMod].x1;
                maxYPoint.x2 = wallVertices[tempMod].x2;
            }
            else
            {
                minYPoint.x1 = wallVertices[tempMod].x1;
                minYPoint.x2 = wallVertices[tempMod].x2;
                maxYPoint.x1 = wallVertices[iVertexCollided].x1;
                maxYPoint.x2 = wallVertices[iVertexCollided].x2;
            }
            // Use this data to determine whether the collision we found is actually a valid collision (inside the shape)
            return (intersect.x1 >= minXPoint.x1 && intersect.x1 <= maxXPoint.x1 &&
                    intersect.x2 >= minYPoint.x2 && intersect.x2 <= maxYPoint.x2);
        }

        /// <summary>
        /// Given a point, a distance for the project, and an angle, find (x3, x4) from the projection.
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="distance"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static DoublePoint findProjection(double x1, double x2, double distance, double angle)
        {
            DoublePoint ret = new DoublePoint();

            // Special case if the angle is 0
            if (angle == 0)
            {
                ret.x1 = x1 + distance;
                ret.x2 = x2;
                return ret;
            } // or 180
            else if (angle == 180)
            {
                ret.x1 = x1 - distance;
                ret.x2 = x2;
                return ret;
            } // or 90
            else if (angle == 90)
            {
                ret.x1 = x1;
                ret.x2 = x2 + distance;
                return ret;
            } // or 270
            else if (angle == 270)
            {
                ret.x1 = x1;
                ret.x2 = x2 - distance;
                return ret;
            }
            // IF none of those, calculate x3 and x4
            double x3 = Math.Abs(Math.Cos(angle * Math.PI / 180d) * distance);
            double x4 = Math.Abs(Math.Sin(angle * Math.PI / 180d) * distance);
            // If the angle was in quadrant 1, add x1 and x3, add x2 and x4
            if (angle > 0 && angle < 90)
            {
                ret.x1 = x1 + x3;
                ret.x2 = x2 + x4;
            }
            // If the angle was in quadrant 2, subtract x3 from x1, add x2 and x4
            else if (angle > 90 && angle < 180)
            {
                ret.x1 = x1 - x3;
                ret.x2 = x2 + x4;
            } // If the angle was in quadrant 3, subtract x3 from x1, subtract x4 from x2
            else if (angle > 180 && angle < 270)
            {
                ret.x1 = x1 - x3;
                ret.x2 = x2 - x4;
            } // If the angle was in quadrant 4, add x1 and x3, subtract x4 from x2
            else
            {
                ret.x1 = x1 + x3;
                ret.x2 = x2 - x4;
            }

            return ret;
        }

        /// <summary>
        /// This finds the angle of reflection given the angle of project and the angle of the wall
        /// </summary>
        /// <param name="projection"></param>
        /// <param name="wallAngle"></param>
        /// <returns></returns>
        public static double findReflection(double projection, double wallAngle)
        {
            double temp = 0;
            // Special cases where either is horizontal
            if (wallAngle == 180 || wallAngle == 0)
            {
                temp = 360 - projection;
            }
            // Special cases where either is vertical
            else if (wallAngle == 90 || wallAngle == 270)
            {
                temp = mod(180 - projection, 360);
            }
            // Else formula I solved to find the angle of reflection
            else
            {
                temp = mod((2 * wallAngle) - projection, 360);
            }
            return temp;
        }

        /// <summary>
        /// Custom mod formula for double because the regular modulo truncates double precision numbers to integers before performing the operation.
        /// This is much more expensive to perform than regualar Math.mod.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static double mod(double x, double m)
        {
            return (x % m + m) % m;
        }

        /// <summary>
        /// Finds the angle given two points (x1, x2) and (x3, x4)
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="x3"></param>
        /// <param name="x4"></param>
        /// <returns></returns>
        public static double findAngle(double x1, double x2, double x3, double x4)
        {
            double x13 = x3 - x1;
            double x24 = x4 - x2;
            // Find their angle using ArcTan
            double angle = Math.Abs(Math.Atan(x24 / x13) * 180 / Math.PI);

            // Special cases for if they are horizontal or vertical
            if (x13 == 0 && x24 >= 0)
            {
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

            // In quadrant 1
            // if(x13 >= 0 && x24 >= 0)
            // angle = angle;

            // In quadrant 2
            else if (x13 <= 0 && x24 >= 0)
                angle = 180 - angle;

            // In quadrant 4
            else if (x13 >= 0 && x24 <= 0)
                angle = 360 - angle;

            // In quadrant 3
            else
                angle = angle + 180;

            return angle;
        }
    }
}
