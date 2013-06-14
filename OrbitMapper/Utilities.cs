using OrbitMapper.Shapes;
using OrbitMapper.Tessellations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OrbitMapper.Utilities
{
    /// <summary>
    /// Contains shared methods for working with images in OrbitMapper.
    /// </summary>
    public static class ImageUtilities
    {

        /// <summary>
        /// Saves the JPEG.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="quality">The quality.</param>
        public static void SaveJpeg(Image img, string filePath, long quality)
        {
            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            img.Save(filePath, GetEncoder(ImageFormat.Jpeg), encoderParameters);
        }

        /// <summary>
        /// Saves the PNG.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="quality">The quality.</param>
        public static void SavePng(Image img, string filePath, long quality)
        {
            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            img.Save(filePath, GetEncoder(ImageFormat.Png), encoderParameters);
        }

        /// <summary>
        /// Saves the tiff.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="quality">The quality.</param>
        public static void SaveTiff(Image img, string filePath, long quality)
        {
            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            img.Save(filePath, GetEncoder(ImageFormat.Tiff), encoderParameters);
        }

        /// <summary>
        /// Gets the encoder.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.Single(codec => codec.FormatID == format.Guid);
        }

        
        public static Bitmap createTessellation(Tessellation tess)
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

            int iterX = this.Width / getPattern().iWidth + 6;
            int iterY = this.Height / getPattern().iHeight + 2;
            // What divPatWidth and divPatHeight are used for is to determine the area of iterations that this pattern should run through while drawing in order
            // for the pattern to not end while using your mouse to scroll the map beyond the bounds of the picturebox.
            // This method to create a dynamic maps such as this is a necessary so that you are not trying to draw things that are not even visible to the container.
            // Although, there is a bit of an overlap purposely on the left, right and top because many patterns would simply end while scrolling because they are so large jagged.
            int divPatWidth = (-offset.X) / getPattern().iWidth;
            int divPatHeight = (-offset.Y) / getPattern().iHeight;
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
                        }
                        g.DrawPolygon(System.Drawing.Pens.Black, poly);
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
                                startingAngle = mod(startingAngle, 180);
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
        }

        /// <summary>
        /// Creates the bitmap preview.
        /// </summary>
        /// <param name="shape">The shape to draw to a BMP.</param>
        /// <returns></returns>
        public static Bitmap createBitmapPreview(Shape shape)
        {
            using (var bmp = new Bitmap(shape.Width, shape.Height))
            {
                shape.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                return ResizeImage(bmp, new Size(shape.Width, shape.Height));
            }
        }

        /// <summary>
        /// Creates the bitmap preview. Will distort BMP
        /// </summary>
        /// <param name="shape">The shape to scale bitmap to.</param>
        /// <param name="width">The width to scale down to.</param>
        /// <param name="height">The height to scale down to.</param>
        /// <returns></returns>
        public static Bitmap createBitmapPreview(Shape shape, int width, int height)
        {
            using (var bmp = new Bitmap(shape.Width, shape.Height))
            {
                shape.DrawToBitmap(bmp, new Rectangle(0, 0, shape.Width, shape.Height));
                return ResizeImage(bmp, new Size(width, height));
            }
        }

        /// <summary>
        /// Resizes the image.
        /// </summary>
        /// <param name="imgToResize">The img to resize.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static Bitmap ResizeImage(Bitmap imgToResize, Size size)
        {
            try
            {
                Bitmap b = new Bitmap(size.Width, size.Height);
                using (Graphics g = Graphics.FromImage((System.Drawing.Image)b))
                {
                    // High quality BMP
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
                }
                return b;
            }
            catch
            {
                return null;
            }
        }
    }
}
