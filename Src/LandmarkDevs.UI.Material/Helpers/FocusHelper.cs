#region

using System.Windows;
using System.Windows.Controls;

#endregion

namespace LandmarkDevs.UI.Material.Helpers
{
    /// <summary>
    ///     Class FocusHelper.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.Control" />
    public class FocusHelper : Control
    {
        /// <summary>
        ///     The target element property
        /// </summary>
        public static readonly DependencyProperty TargetElementProperty =
            DependencyProperty.Register(
                "TargetElement",
                typeof(Control),
                typeof(FocusHelper),
                null);

        /// <summary>
        ///     Gets or sets the target element.
        /// </summary>
        /// <value>The target element.</value>
        public Control TargetElement
        {
            get { return (Control) GetValue(TargetElementProperty); }
            set { SetValue(TargetElementProperty, value); }
        }

        /// <summary>
        ///     The set focus property
        /// </summary>
        public static readonly DependencyProperty SetFocusProperty = DependencyProperty.Register(
            "SetFocus",
            typeof(bool),
            typeof(FocusHelper),
            new PropertyMetadata(false, FocusChanged));

        /// <summary>
        ///     Gets or sets a value indicating whether [set focus].
        /// </summary>
        /// <value><c>true</c> if [set focus]; otherwise, <c>false</c>.</value>
        public bool SetFocus
        {
            get { return (bool) GetValue(SetFocusProperty); }
            set { SetValue(SetFocusProperty, value); }
        }

        /// <summary>
        ///     Focuses the changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void FocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var targetElement = d.GetValue(TargetElementProperty) as Control;
            if (targetElement == null || e.NewValue == null || (!((bool) e.NewValue)))
            {
                return;
            }
            targetElement.Focus();
            d.SetValue(SetFocusProperty, false);
        }
    }
}