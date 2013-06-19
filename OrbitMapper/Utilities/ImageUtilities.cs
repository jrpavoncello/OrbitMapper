using OrbitMapper.Shapes;
using OrbitMapper.Tessellations;
using OrbitMapper.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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


        /// <summary>
        /// Creates the tessellation.
        /// </summary>
        /// <param name="tess">The tess.</param>
        /// <returns></returns>
        public static Bitmap createTessellation(Tessellation tess)
        {
            int tessWidth = Math.Abs(tess.getBaseClick().X - tess.getEndClick().X);
            int tessHeight = Math.Abs(tess.getBaseClick().Y - tess.getEndClick().Y);

            // Get the patterns to tile and draw
            Point[][] patterns = tess.getPattern().getPatterns();
            // Determine the number of iterations in the x direction and the number of iterations in the y direction
            // that need to be used in order to cover the entire tessellation area.
            // 2 was added so that I can draw it overlapping the User Controls draw area so that there is no whitespace near edges.

            int iterX = (tess.getBaseClick().Y - tess.getEndClick().Y) / tess.getPattern().iWidth + 6;
            int iterY = (tess.getBaseClick().Y - tess.getEndClick().Y) / tess.getPattern().iHeight + 6;
            tess.getUnfoldingPath().Clear();
            // What divPatWidth and divPatHeight are used for is to determine the area of iterations that this pattern should run through while drawing in order
            // for the pattern to not end while using your mouse to scroll the map beyond the bounds of the picturebox.
            // This method to create a dynamic maps such as this is a necessary so that you are not trying to draw things that are not even visible to the container.
            // Although, there is a bit of an overlap purposely on the left, right and top because many patterns would simply end while scrolling because they are so large jagged.
            int divPatWidth = (-tess.getOffset().X) / tess.getPattern().iWidth;
            int divPatHeight = (-tess.getOffset().Y) / tess.getPattern().iHeight;
            if (divPatWidth > 0)
                divPatWidth = 0;
            if (divPatHeight > 0)
                divPatHeight = 0;
            // Run through the iterations in both directions (it draws the Y direction for each X first).
            for (int i = divPatWidth; i < iterX + divPatWidth; i++)
            {
                int iMultGetPatWidth = i * tess.getPattern().iWidth;
                for (int j = divPatHeight; j < iterY + divPatHeight; j++)
                {
                    int jMultGetPatHeight = j * tess.getPattern().iHeight;
                    for (int k = 0; k < patterns.Count(); k++)
                    {
                        // Draw each pattern
                        Point[] poly = new Point[patterns.ElementAt<Point[]>(k).Count()];
                        bool isShapeCollided = false;
                        for (int l = 0; l < patterns.ElementAt<Point[]>(k).Count(); l++)
                        {
                            Point temp = new Point(iMultGetPatWidth + patterns[k][l].X, tess.getPictureBox().Height - 5 - jMultGetPatHeight - patterns[k][l].Y);
                            // This is applied every other row because some patterns do not populate the map perfectly straight up.
                            // For example, the equilateral pattern must be applied at a slight horizontal offset otherwise the pattern would not match up as we draw
                            if (j % 2 == 1)
                                temp.X -= tess.getPattern().iOffset;
                            // This is applied at every iteration, we will subtract 3 times the width to not mess up the iOffset and give it some overlap to the left
                            temp.X -= tess.getPattern().iWidth * 3 - tess.getOffset().X;
                            temp.Y -= tess.getOffset().Y;
                            poly[l] = temp;

                            // Algorithm for highlighting the shapes that have a collision
                            int tempMod = (int)MathUtilities.mod(l - 1, patterns.ElementAt<Point[]>(k).Count()); // Use tempMod to find the correct vertex to determine a line for the current wall
                            if (!isShapeCollided) // Check if this polygon has been determined as part of the unfolding
                            {
                                if (l > 0)
                                {
                                    // Check the collisions between the vertices of the wall we're examining currently and the vertices of the base and end clicks
                                    if (MathUtilities.isValidIntersect((double)poly[tempMod].X, (double)poly[tempMod].Y, (double)poly[l].X, (double)poly[l].Y, (double)tess.getBaseClick().X + tess.getOffset().X, (double)tess.getBaseClick().Y - tess.getOffset().Y, (double)tess.getEndClick().X + tess.getOffset().X, (double)tess.getEndClick().Y - tess.getOffset().Y))
                                    {
                                        isShapeCollided = true;
                                    }
                                }
                                // Because we didn't check when l is 0, we must come back around after it has been set (on the last iteration) to check that wall we skipped
                                if (l == patterns.ElementAt<Point[]>(k).Count() - 1)
                                {
                                    if (MathUtilities.isValidIntersect((double)poly[l].X, (double)poly[l].Y, (double)poly[0].X, (double)poly[0].Y, (double)tess.getBaseClick().X + tess.getOffset().X, (double)tess.getBaseClick().Y - tess.getOffset().Y, (double)tess.getEndClick().X + tess.getOffset().X, (double)tess.getEndClick().Y - tess.getOffset().Y))
                                    {
                                        isShapeCollided = true;
                                    }
                                }
                            }
                        }
                        if(isShapeCollided)
                            tess.getUnfoldingPath().Add(poly);
                    }
                }
            }

            Bitmap myBitmap = new Bitmap(tessWidth + tess.getPattern().iWidth*2, tessHeight + tess.getPattern().iHeight*2);
            using (Graphics g = Graphics.FromImage(myBitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.White);

                for (int i = 0; i < tess.getUnfoldingPath().Count; i++)
                {
                    Point[] poly = tess.getUnfoldingPath()[i];
                    for (int j = 0; j < poly.Length; j++)
                    {
                        poly[j].X -= tess.getBaseClick().X > tess.getEndClick().X ? tess.getEndClick().X : tess.getBaseClick().X;
                        poly[j].Y -= tess.getBaseClick().Y > tess.getEndClick().Y ? tess.getEndClick().Y : tess.getBaseClick().Y;
                        poly[j].X += tess.getPattern().iWidth - tess.getOffset().X;
                        poly[j].Y += tess.getPattern().iHeight + tess.getOffset().Y;
                    }
                    g.DrawPolygon(System.Drawing.Pens.Black, poly);
                }
                Point newBaseClick = new Point();
                Point newEndClick = new Point();
                int xOffset = tess.getBaseClick().X > tess.getEndClick().X ? tess.getEndClick().X : tess.getBaseClick().X;
                int yOffset = tess.getBaseClick().Y > tess.getEndClick().Y ? tess.getEndClick().Y : tess.getBaseClick().Y;
                xOffset -= tess.getPattern().iWidth;
                yOffset -= tess.getPattern().iHeight;
                newBaseClick.X = tess.getBaseClick().X - xOffset;
                newBaseClick.Y = tess.getBaseClick().Y - yOffset;
                newEndClick.X = tess.getEndClick().X - xOffset;
                newEndClick.Y = tess.getEndClick().Y - yOffset;
                System.Drawing.Pen myPen = new Pen(System.Drawing.Brushes.DarkBlue, 2);
                g.DrawLine(System.Drawing.Pens.Blue, newBaseClick, newEndClick);

                g.Save();
                return myBitmap;
            }
        }

        /// <summary>
        /// Creates the bitmap preview.
        /// </summary>
        /// <param name="src">The source bitmap to resize for a preview.</param>
        /// <returns></returns>
        public static Bitmap createBitmapPreview(Control src)
        {
            using (var bmp = new Bitmap(src.Width, src.Height))
            {
                src.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                return ResizeImage(bmp, new Size(src.Width, src.Height));
            }
        }

        /// <summary>
        /// Creates the bitmap preview. Will distort BMP
        /// </summary>
        /// <param name="src">The shape to scale bitmap to.</param>
        /// <param name="width">The width to scale down to.</param>
        /// <param name="height">The height to scale down to.</param>
        /// <returns></returns>
        public static Bitmap createBitmapPreview(Control src, int width, int height)
        {
            using (var bmp = new Bitmap(src.Width, src.Height))
            {
                src.DrawToBitmap(bmp, new Rectangle(0, 0, src.Width, src.Height));
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
                Graphics g = Graphics.FromImage((System.Drawing.Image)b);
                // High quality BMP
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
                return b;
            }
            catch
            {
                return null;
            }
        }
    }
}
