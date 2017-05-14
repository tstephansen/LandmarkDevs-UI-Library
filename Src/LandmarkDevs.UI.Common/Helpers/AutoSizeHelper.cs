#region
using System.Windows;

#endregion

namespace LandmarkDevs.UI.Common.Helpers
{
    /// <summary>
    ///     Class AutoSizeHelper.
    /// </summary>
    public static class AutoSizeHelper
    {
        /// <summary>
        ///     The set height to actual property
        /// </summary>
        public static readonly DependencyProperty SetHeightToActualProperty =
            DependencyProperty.RegisterAttached("SetHeightToActual", typeof(bool), typeof(AutoSizeHelper),
                new FrameworkPropertyMetadata(false, (s, e) =>
                {
                    if ((bool) e.NewValue)
                    {
                        var element = (FrameworkElement) s;
                        RoutedEventHandler handler = null;
                        handler = delegate
                        {
                            element.Height = element.ActualHeight;
                            element.Loaded -= handler;
                        };
                        element.Loaded += handler;
                    }
                }));

        /// <summary>
        ///     Gets the set height to actual.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool GetSetHeightToActual(DependencyObject obj)
        {
            return (bool) obj.GetValue(SetHeightToActualProperty);
        }

        /// <summary>
        ///     Sets the set height to actual.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetSetHeightToActual(DependencyObject obj, bool value)
        {
            obj.SetValue(SetHeightToActualProperty, value);
        }

        /// <summary>
        ///     Handles the <see cref="E:SetHeightToActualChanged" /> event.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnSetHeightToActualChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if ((bool) e.NewValue)
            {
                var element = (FrameworkElement) obj;
                RoutedEventHandler handler = null;
                handler = delegate
                {
                    element.Height = element.ActualHeight;
                    element.Loaded -= handler;
                };
                element.Loaded += handler;
            }
        }

        /// <summary>
        ///     The set width to actual property
        /// </summary>
        public static readonly DependencyProperty SetWidthToActualProperty =
            DependencyProperty.RegisterAttached("SetWidthToActual", typeof(bool), typeof(AutoSizeHelper),
                new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnSetWidthToActualChanged)));

        /// <summary>
        ///     Gets the set width to actual.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool GetSetWidthToActual(DependencyObject obj)
        {
            return (bool) obj.GetValue(SetWidthToActualProperty);
        }

        /// <summary>
        ///     Sets the set width to actual.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetSetWidthToActual(DependencyObject obj, bool value)
        {
            obj.SetValue(SetWidthToActualProperty, value);
        }

        /// <summary>
        ///     Handles the <see cref="E:SetWidthToActualChanged" /> event.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnSetWidthToActualChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if ((bool) e.NewValue)
            {
                var element = (FrameworkElement) obj;
                RoutedEventHandler handler = null;
                handler = delegate
                {
                    element.Width = element.ActualWidth;
                    element.Loaded -= handler;
                };
                element.Loaded += handler;
            }
        }
    }
}