using System;
using System.Windows;
using System.Windows.Controls;

namespace LandmarkDevs.UI.Material.Helpers
{
    /// <summary>
    /// Class StateManager.
    /// This was taken from https://tdanemar.wordpress.com/2009/11/15/using-the-visualstatemanager-with-the-model-view-viewmodel-pattern-in-wpf-or-silverlight/
    /// </summary>
    /// <seealso cref="System.Windows.DependencyObject" />
    public class StateManager : DependencyObject
    {
        /// <summary>
        /// The visual state property
        /// </summary>
        public static readonly DependencyProperty VisualStateProperty = DependencyProperty.RegisterAttached(
            "VisualState", typeof(string), typeof(StateManager), new PropertyMetadata((s, e) =>
            {
                var stateName = (string) e.NewValue;
                var control = s as Control;
                if(control == null)
                    throw new InvalidOperationException("This attached property only supports types derived from Control.");
                VisualStateManager.GoToState(control, stateName, true);
            }));

        /// <summary>
        /// Sets the visual state.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetVisualState(DependencyObject element, string value)
        {
            element.SetValue(VisualStateProperty, value);
        }
        /// <summary>
        /// Gets the visual state.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>System.String.</returns>
        public static string GetVisualState(DependencyObject element)
        {
            return (string) element.GetValue(VisualStateProperty);
        }
    }
}
