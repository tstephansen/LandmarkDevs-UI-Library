#region
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

#endregion

namespace LandmarkDevs.UI.Common.Converters
{
    /// <summary>
    ///     Class BrushToColorConverter.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class BrushToColorConverter : IValueConverter
    {
        /// <summary>
        ///     Converts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns>System.Object.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            object solidColorBrush;
            Color color;
            SolidColorBrush solidColorBrush1 = value as SolidColorBrush;
            if (!(solidColorBrush1 != null || parameter == null || parameter.ToString() != "AccentBrushnull"))
            {
                solidColorBrush = new SolidColorBrush(Colors.Transparent);
            }
            else if (!(solidColorBrush1 != null || parameter == null || parameter.ToString() != "ContentBrushnull"))
            {
                solidColorBrush = Colors.Black;
            }
            else if (!(solidColorBrush1 == null || parameter == null || parameter.ToString() != "ContentBrushnull"))
            {
                solidColorBrush = solidColorBrush1.Color;
            }
            else if (!(solidColorBrush1 == null || parameter == null || parameter.ToString() != "AccentBrushnull"))
            {
                byte a = solidColorBrush1.Color.A;
                byte r = solidColorBrush1.Color.R;
                byte g = solidColorBrush1.Color.G;
                color = solidColorBrush1.Color;
                Color color1 = Color.FromArgb(a, r, g, color.B);
                solidColorBrush = new SolidColorBrush(color1);
            }
            else if (solidColorBrush1 != null)
            {
                byte num = solidColorBrush1.Color.A;
                byte r1 = solidColorBrush1.Color.R;
                byte g1 = solidColorBrush1.Color.G;
                color = solidColorBrush1.Color;
                solidColorBrush = Color.FromArgb(num, r1, g1, color.B);
            }
            else
            {
                solidColorBrush = Colors.Transparent;
            }
            return solidColorBrush;
        }

        /// <summary>
        ///     Converts the back.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns>System.Object.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            return null;
        }
    }
}