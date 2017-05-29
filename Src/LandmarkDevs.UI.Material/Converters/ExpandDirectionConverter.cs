#region

using System;
using System.Globalization;
using System.Windows.Data;

#endregion

namespace LandmarkDevs.UI.Material.Converters
{
    /// <summary>
    ///     Class ExpandDirectionConverter.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class ExpandDirectionConverter : IValueConverter
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
            var exp = (ExpandFrom) value;
            switch (exp)
            {
                case ExpandFrom.Bottom:
                    return "Bottom";
                case ExpandFrom.BottomLeft:
                    return "BottomLeft";
                case ExpandFrom.BottomRight:
                    return "BottomRight";
                case ExpandFrom.Left:
                    return "Left";
                case ExpandFrom.Right:
                    return "Right";
                case ExpandFrom.Top:
                    return "Top";
                case ExpandFrom.TopLeft:
                    return "TopLeft";
                case ExpandFrom.TopRight:
                    return "TopRight";
                default:
                    return null;
            }
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

    /// <summary>
    ///     Enum ExpandFrom
    /// </summary>
    public enum ExpandFrom
    {
        /// <summary>
        ///     The top
        /// </summary>
        Top,

        /// <summary>
        ///     The top right
        /// </summary>
        TopRight,

        /// <summary>
        ///     The right
        /// </summary>
        Right,

        /// <summary>
        ///     The bottom right
        /// </summary>
        BottomRight,

        /// <summary>
        ///     The bottom
        /// </summary>
        Bottom,

        /// <summary>
        ///     The bottom left
        /// </summary>
        BottomLeft,

        /// <summary>
        ///     The left
        /// </summary>
        Left,

        /// <summary>
        ///     The top left
        /// </summary>
        TopLeft
    }
}