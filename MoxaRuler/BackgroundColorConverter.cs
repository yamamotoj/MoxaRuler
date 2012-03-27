using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Runtime.InteropServices;

namespace MoxaRuler
{
    class BackgroundColorConverter :IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(Color)) { return value; }
            if(! (value  is Color)) { return value; }
            var color = (Color)value;
            var c = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
            var h = c.GetHue();
            var b = c.GetBrightness();
            var s = c.GetSaturation();

            var c2 = ColorUtil.ColorFromAhsb(200, h, s /2f, 0.75f);
            return Color.FromArgb(c2.A, c2.R, c2.G, c2.B);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
