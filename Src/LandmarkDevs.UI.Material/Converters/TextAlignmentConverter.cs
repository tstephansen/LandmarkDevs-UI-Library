#region

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#endregion

namespace LandmarkDevs.UI.Material.Converters
{
    /// <summary>
    ///     Class TextAlignmentConverter.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class TextAlignmentConverter : IValueConverter
    {
        /// <summary>
        ///     Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TextAlignment textAlignment;
            switch ((HorizontalAlignment) value)
            {
                case HorizontalAlignment.Center:
                    textAlignment = TextAlignment.Center;
                    break;
                case HorizontalAlignment.Right:
                    textAlignment = TextAlignment.Right;
                    break;
                case HorizontalAlignment.Stretch:
                    textAlignment = TextAlignment.Justify;
                    break;
                default:
                    textAlignment = TextAlignment.Left;
                    break;
            }
            return (object) textAlignment;
        }

        /// <summary>
        ///     Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}