using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace LandmarkDevs.UI.Common.Converters
{
    /// <summary>
    /// Class TextForegroundColorConverter.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class TextForegroundColorConverter : IValueConverter
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
            var brownColor = Color.FromRgb(121, 85, 72);
            var deepPurpleColor = Color.FromRgb(103, 58, 183);
            var indigoColor = Color.FromRgb(63, 81, 181);
            var pinkColor = Color.FromRgb(233, 30, 99);
            var purpleColor = Color.FromRgb(156, 39, 176);
            var solidColorBrush = (SolidColorBrush)value;
            if (solidColorBrush == null) return null;
            var color = solidColorBrush.Color;
            if (color == brownColor || color == deepPurpleColor || color == indigoColor || color == pinkColor || color == purpleColor)
                return new SolidColorBrush(Color.FromRgb(255, 255, 255));
            else
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
            var brownColor = Color.FromRgb(121, 85, 72);
            var deepPurpleColor = Color.FromRgb(103, 58, 183);
            var indigoColor = Color.FromRgb(63, 81, 181);
            var pinkColor = Color.FromRgb(233, 30, 99);
            var purpleColor = Color.FromRgb(156, 39, 176);
            var solidColorBrush = (SolidColorBrush)value;
            if (solidColorBrush == null) return null;
            var color = solidColorBrush.Color;
            if (color == brownColor || color == deepPurpleColor || color == indigoColor || color == pinkColor || color == purpleColor)
                return solidColorBrush;
            else
                return new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
    }
}
