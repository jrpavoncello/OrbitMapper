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
