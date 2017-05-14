#region
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
#endregion

namespace LandmarkDevs.UI.Common.Converters
{
    /// <summary>
    ///     Class BooleanToVisibilityConverter.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class BooleanToVisibilityConverter : IValueConverter
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
            return (!(bool) value ? Visibility.Collapsed : Visibility.Visible);
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
            return ((Visibility) value != Visibility.Collapsed ? true : false);
        }
    }
}