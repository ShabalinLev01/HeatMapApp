using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace HeatMapApp.ViewModel
{
    /// <summary>
    /// Service for generation HeatMap
    /// </summary>
    public class HeatMapService
    {
        /// <summary>
        /// The method takes the path of a file and, based on the data in it, generates an image
        /// </summary>
        /// <param name="fileName">File path</param>
        /// <returns>Returns the finished image</returns>
        public BitmapSource GenerateHeatMap(string fileName)
        {
            var lines = File.ReadLines(fileName);
            int width = lines.FirstOrDefault().Count(x => x == Convert.ToChar(","))+1;
            int height = lines.Count();
            Bitmap bitmapImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            string[] arrayLines = lines.Select(i => i.ToString()).ToArray();
            DrawingPixelByValue(bitmapImage, arrayLines);

            return ConvertToBitmapSource(bitmapImage);
        }

        private void DrawingPixelByValue(Bitmap processedBitmap, string[] arrayLines)
        {
            BitmapData bitmapData = processedBitmap.LockBits(new Rectangle(0, 0, processedBitmap.Width, processedBitmap.Height), 
                ImageLockMode.ReadWrite, processedBitmap.PixelFormat);

            int bytesPerPixel = Bitmap.GetPixelFormatSize(processedBitmap.PixelFormat) / 8;
            int byteCount = bitmapData.Stride * processedBitmap.Height;

            byte[] pixels = new byte[byteCount];
            IntPtr ptrFirstPixel = bitmapData.Scan0;

            Marshal.Copy(ptrFirstPixel, pixels, 0, pixels.Length);

            int heightInPixels = bitmapData.Height;
            int widthInBytes = bitmapData.Width * bytesPerPixel;
            Parallel.For(0, heightInPixels, y =>
            {
                string[] array = arrayLines[y].Split(",");
                int currentLine = y * bitmapData.Stride;
                int numOfArray = 0;
                for (int x = 0; x < widthInBytes; x += bytesPerPixel)
                {
                    var color = HeatMapColor(decimal.Parse(array[numOfArray], CultureInfo.InvariantCulture), 30, 180);
                    pixels[currentLine + x] = color.B;
                    pixels[currentLine + x + 1] = color.G;
                    pixels[currentLine + x + 2] = color.R;
                    pixels[currentLine + x + 3] = color.A;
                    numOfArray++;
                }
            });

            // copy modified bytes back
            Marshal.Copy(pixels, 0, ptrFirstPixel, pixels.Length);
            processedBitmap.UnlockBits(bitmapData);
        }

        private BitmapSource ConvertToBitmapSource(Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height,
                bitmap.HorizontalResolution, bitmap.VerticalResolution,
                PixelFormats.Bgra32, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }

        private Color HeatMapColor(decimal value, decimal min, decimal max)
        {
            decimal val = 2 * (value - min) / (max - min);
            int r = Convert.ToInt32(Distance(val, 2));
            int g = Convert.ToInt32(Distance(val, 1));
            int b = Convert.ToInt32(Distance(val, 0));
            int a = 255;

            return Color.FromArgb(a,r,g,b);
        }

        private decimal Distance(decimal value, decimal color)
        {
            var distance = Math.Abs(value - color);

            var colorStrength = 1 - distance;

            if (colorStrength < 0)
                colorStrength = 0;

            return (decimal)Math.Round(colorStrength * 255);
        }
    }
}
