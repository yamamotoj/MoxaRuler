using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Windows;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MoxaRuler
{
    class DeskTopCapture
    {
        private static BitmapSource toBitmapSource(System.Drawing.Bitmap source)
        {
            var hBitmap = source.GetHbitmap();

            try
            {
                var ret =  System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
                return ret;
            }
            catch (Win32Exception)
            {
                return null;
            }
            finally
            {
                DeleteObject(hBitmap);
            }
        }

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr hObject);


        static public BitmapSource CaptureScreen(double x, double y, double width, double height)
        {
            int ix, iy, iw, ih;
            ix = Convert.ToInt32(x);
            iy = Convert.ToInt32(y);
            iw = Convert.ToInt32(width);
            ih = Convert.ToInt32(height);
            using (var image = new Bitmap(iw, ih, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {

                Graphics g = Graphics.FromImage(image);
                g.CopyFromScreen(ix, iy, 0, 0,
                         new System.Drawing.Size(iw, ih),
                         CopyPixelOperation.SourceCopy);

                var bms = toBitmapSource(image);
                return bms;
            }
        }


        static public System.Windows.Media.Color GetColor(double x, double y)
        {
            int ix, iy;
            ix = Convert.ToInt32(x);
            iy = Convert.ToInt32(y);
            using (var image = new Bitmap(100, 100, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {

                Graphics g = Graphics.FromImage(image);
                g.CopyFromScreen(ix, iy, 0, 0,
                         new System.Drawing.Size(1, 1),
                         CopyPixelOperation.SourceCopy);
                var pixel = image.GetPixel(0, 0);
                return System.Windows.Media.Color.FromRgb(pixel.R, pixel.G, pixel.B);
            }
        }

    }
}

