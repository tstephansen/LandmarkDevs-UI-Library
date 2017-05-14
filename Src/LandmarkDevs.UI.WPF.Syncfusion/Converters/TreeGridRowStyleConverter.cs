#region
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Syncfusion.UI.Xaml.TreeGrid;

#endregion

namespace LandmarkDevs.UI.WPF.Syncfusion.Converters
{
    /// <summary>
    ///     Class TreeGridRowStyleConverter.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class TreeGridRowStyleConverter : IValueConverter
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
            if ((value as TreeNode).Level == 0)
                return new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            else if ((value as TreeNode).Level == 1)
                return new SolidColorBrush(Color.FromArgb(255, 249, 249, 249));
            else if ((value as TreeNode).Level == 2)
                return new SolidColorBrush(Color.FromArgb(255, 243, 243, 243));
            else if ((value as TreeNode).Level == 3)
                return new SolidColorBrush(Color.FromArgb(255, 237, 237, 237));
            else if ((value as TreeNode).Level == 4)
                return new SolidColorBrush(Color.FromArgb(255, 231, 231, 231));
            return new SolidColorBrush();
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