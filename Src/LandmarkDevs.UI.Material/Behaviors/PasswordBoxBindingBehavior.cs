using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Threading;

namespace LandmarkDevs.UI.Material.Behaviors
{
    /// <summary>
    /// Class PasswordBoxBindingBehavior.
    /// </summary>
    /// <seealso cref="System.Windows.Interactivity.Behavior" />
    /// <seealso cref="System.Windows.Controls.PasswordBox"/>
    public class PasswordBoxBindingBehavior : Behavior<PasswordBox>
    {
        /// <summary>
        ///     Gets or sets the bindable Password property on the PasswordBox control. This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty PasswordProperty
            = DependencyProperty.RegisterAttached("Password", typeof(string),
                                                  typeof(PasswordBoxBindingBehavior),
                                                  new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(OnPasswordPropertyChanged)));

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <param name="dpo">The dpo.</param>
        /// <returns>System.String.</returns>
        public static string GetPassword(DependencyObject dpo)
        {
            return (string)dpo.GetValue(PasswordProperty);
        }

        /// <summary>
        /// Sets the password.
        /// </summary>
        /// <param name="dpo">The dpo.</param>
        /// <param name="value">The value.</param>
        public static void SetPassword(DependencyObject dpo, string value)
        {
            dpo.SetValue(PasswordProperty, value);
        }

        /// <summary>
        ///     Handles changes to the 'Password' attached property.
        /// </summary>
        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var targetPasswordBox = sender as PasswordBox;
            if (targetPasswordBox != null)
            {
                targetPasswordBox.PasswordChanged -= PasswordBoxPasswordChanged;
                if (!GetIsChanging(targetPasswordBox))
                {
                    targetPasswordBox.Password = (string)e.NewValue;
                }
                targetPasswordBox.PasswordChanged += PasswordBoxPasswordChanged;
            }
        }

        /// <summary>
        ///     Handle the 'PasswordChanged'-event on the PasswordBox
        /// </summary>
        private static void PasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            SetIsChanging(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsChanging(passwordBox, false);
        }

        /// <summary>
        ///     Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        ///     Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.PasswordChanged += PasswordBoxPasswordChanged;
            this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded,
                                        new Action(() => {
                                            if (this.AssociatedObject != null)
                                            {
                                                SetPassword(this.AssociatedObject, this.AssociatedObject.Password);
                                            }
                                        }));
        }

        /// <summary>
        ///     Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>
        ///     Override this to unhook functionality from the AssociatedObject.
        /// </remarks>
        protected override void OnDetaching()
        {
            // it seems, it was already detached, or never attached
            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.PasswordChanged -= PasswordBoxPasswordChanged;
            }
            base.OnDetaching();
        }

        private static readonly DependencyProperty IsChangingProperty
            = DependencyProperty.RegisterAttached("IsChanging",
                                                  typeof(bool),
                                                  typeof(PasswordBoxBindingBehavior),
                                                  new UIPropertyMetadata(false));

        private static bool GetIsChanging(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsChangingProperty);
        }

        private static void SetIsChanging(DependencyObject obj, bool value)
        {
            obj.SetValue(IsChangingProperty, value);
        }
    }
}
