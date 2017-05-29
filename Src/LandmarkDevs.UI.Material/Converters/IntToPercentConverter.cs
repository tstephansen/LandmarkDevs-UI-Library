using System;
using System.Globalization;
using System.Windows.Data;

namespace LandmarkDevs.UI.Material.Converters
{
    /// <summary>
    /// Class IntToPercentConverter.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class IntToPercentConverter : IValueConverter
    {
        /// <summary>
        /// Converts an integer to a string percentage.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? $"{value}%" : string.Empty;
        }

        /// <summary>
        /// Converts a string percentage to an integer.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var percentString = value.ToString().TrimEnd('%');
            if (string.IsNullOrEmpty(percentString)) return 0;
            int percent;
            int.TryParse(percentString, out percent);
            return percent;
        }
    }
}
