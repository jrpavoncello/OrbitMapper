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

            Control surface = new Control();

            Graphics g = surface.CreateGraphics();
            g.Flush(FlushIntention.Flush);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            // Get the patterns to tile and draw
            Point[][] patterns = tess.getPattern().getPatterns();
            // Determine the number of iterations in the x direction and the number of iterations in the y direction
            // that need to be used in order to cover the entire tessellation area.
            // 2 was added so that I can draw it overlapping the User Controls draw area so that there is no whitespace near edges.

            int iterX = tess.Width / tess.getPattern().iWidth + 1;
            int iterY = tess.Height / tess.getPattern().iHeight + 1;

            // Run through the iterations in both directions (it draws the Y direction for each X first).
            for (int i = 0; i < iterX; i++)
            {
                int iMultGetPatWidth = i * tess.getPattern().iWidth;
                for (int j = 0; j < iterY; j++)
                {
                    int jMultGetPatHeight = j * tess.getPattern().iHeight;
                    for (int k = 0; k < patterns.Count(); k++)
                    {
                        // Draw each pattern
                        Point[] poly = new Point[patterns.ElementAt<Point[]>(k).Count()];
                        for (int l = 0; l < patterns.ElementAt<Point[]>(k).Count(); l++)
                        {
                            int tempMod = (int)MathUtilities.mod(l - 1, patterns.ElementAt<Point[]>(k).Count()); // Use tempMod to find the correct vertex to determine a line for the current wall
                            Point temp = new Point(iMultGetPatWidth + patterns[k][l].X, tess.getPictureBox().Height - 5 - jMultGetPatHeight - patterns[k][l].Y);
                            int lastElement = patterns.ElementAt<Point[]>(k).Count() - 1;
                            // This is applied every other row because some patterns do not populate the map perfectly straight up.
                            // For example, the equilateral pattern must be applied at a slight horizontal offset otherwise the pattern would not match up as we draw
                            if (j % 2 == 1)
                                temp.X -= tess.getPattern().iOffset;
                            // This is applied at every iteration, we will subtract 3 times the width to not mess up the iOffset and give it some overlap to the left
                            temp.X -= tess.getPattern().iWidth * 3 - tess.getOffset().X;
                            temp.Y -= tess.getOffset().Y;
                            DoublePoint intersect = MathUtilities.getIntersect(poly[tempMod].X, poly[tempMod].Y, temp.X, temp.Y, tess.getBaseClick().X, tess.getBaseClick().Y, tess.getEndClick().X, tess.getEndClick().Y);
                            if (intersect != null)
                            {
                                Intersect tempIntersect = new Intersect();
                                tempIntersect.x1 = intersect.x1;
                                tempIntersect.x2 = intersect.x2;
                                if (MathUtilities.isValidIntersect(new List<DoublePoint> { new DoublePoint(poly[tempMod].X, poly[tempMod].Y), new DoublePoint(temp.X, temp.Y) }, tempIntersect))
                                {
                                    poly[lastElement] = temp;
                                }
                            }
                        }
                        g.DrawPolygon(System.Drawing.Pens.Black, poly);
                    }
                }
            }
            g.Flush(FlushIntention.Flush);
            using (Bitmap tempBMP = new Bitmap(tessWidth + tess.getPattern().iWidth, tessHeight + tess.getPattern().iHeight))
            {
                surface.DrawToBitmap(tempBMP, new Rectangle(0, 0, tempBMP.Width, tempBMP.Height));
                return tempBMP;
            }
        }

        /// <summary>
        /// Creates the bitmap preview.
        /// </summary>
        /// <param name="shape">The shape to draw to a BMP.</param>
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
