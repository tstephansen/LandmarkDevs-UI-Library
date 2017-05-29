#region

using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

#endregion

namespace LandmarkDevs.UI.Material.Converters
{
    /// <summary>
    ///     Class ViewModelBaseTitleConverter.
    ///     Gets the title of the tab item from the ViewModel.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class ViewModelBaseTitleConverter : IValueConverter
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
            var item = value as TabItem;
            if (item == null)
                return null;
            return item.Header;
        }

        /// <summary>
        ///     Converts the back.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotImplementedException();
        }
    }
}