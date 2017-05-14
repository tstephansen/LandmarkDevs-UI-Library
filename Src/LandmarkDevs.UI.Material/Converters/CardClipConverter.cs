#region
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
#endregion

namespace LandmarkDevs.UI.Material.Converters
{
    /// <summary>
    ///     This was taken directly from MaterialDesignThemes.Wpf
    ///     All credit for this goes to James.
    /// </summary>
    public class CardClipConverter : IMultiValueConverter
    {
        /// <summary>
        ///     1 - Content presenter render size,
        ///     2 - Clipping border padding (main control padding)
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2 || !(values[0] is Size) || !(values[1] is Thickness))
                return Binding.DoNothing;

            var size = (Size)values[0];
            var farPoint = new Point(
                Math.Max(0, size.Width),
                Math.Max(0, size.Height));
            var padding = (Thickness)values[1];
            farPoint.Offset(padding.Left + padding.Right, padding.Top + padding.Bottom);

            return new Rect(
                new Point(),
                new Point(farPoint.X, farPoint.Y));
        }

        /// <summary>
        ///     Converts a binding target value to the source binding values.
        /// </summary>
        /// <param name="value">The value that the binding target produces.</param>
        /// <param name="targetTypes">
        ///     The array of types to convert to. The array length indicates the number and types of values
        ///     that are suggested for the method to return.
        /// </param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>An array of values that have been converted from the target value back to the source values.</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}