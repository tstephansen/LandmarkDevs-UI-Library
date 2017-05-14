#region
using System.Windows;
using System.Windows.Interactivity;
#endregion

namespace LandmarkDevs.UI.Common.Behaviors
{
    /// <summary>
    ///     Class HeightWidthBehavior.
    /// </summary>
    /// <seealso cref="System.Windows.Interactivity.Behavior{FrameworkElement}" />
    public class HeightWidthBehavior : Behavior<FrameworkElement>
    {
        /// <summary>
        ///     The set height to actual property
        /// </summary>
        public static readonly DependencyProperty SetHeightToActualProperty =
            DependencyProperty.RegisterAttached(
                "SetHeightToActual",
                typeof(bool),
                typeof(HeightWidthBehavior), new FrameworkPropertyMetadata(false, OnSetHeightToActualChanged));

        /// <summary>
        ///     Gets the set height to actual.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <returns><see langword="true" /> if XXXX, <see langword="false" /> otherwise.</returns>
        public static bool GetSetHeightToActual(DependencyObject d)
        {
            return (bool) d.GetValue(SetHeightToActualProperty);
        }

        /// <summary>
        ///     Sets the set height to actual.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="value">if set to <see langword="true" /> [value].</param>
        public static void SetSetHeightToActual(DependencyObject d, bool value)
        {
            d.SetValue(SetHeightToActualProperty, value);
        }

        /// <summary>
        ///     Handles the <see cref="E:SetHeightToActualChanged" /> event.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnSetHeightToActualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool) e.NewValue)
            {
                var element = (FrameworkElement) d;
                RoutedEventHandler handler = null;
                handler = delegate
                {
                    element.Height = element.ActualHeight;
                    element.Loaded -= handler;
                };
                element.Loaded += handler;
            }
        }
    }
}