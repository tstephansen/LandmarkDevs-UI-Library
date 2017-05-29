#region

using System.Windows;
using System.Windows.Input;

#endregion

namespace LandmarkDevs.UI.Material.Helpers
{
    /// <summary>
    ///     Class KeyStateHelper.
    /// </summary>
    public static class KeyStateHelper
    {
        /// <summary>
        ///     The caps lock on property
        /// </summary>
        public static readonly DependencyProperty CapsLockOnProperty = DependencyProperty.RegisterAttached("CapsLockOn",
            typeof(bool), typeof(KeyStateHelper), new PropertyMetadata());

        /// <summary>
        ///     Gets the caps lock on.
        /// </summary>
        /// <returns><c>true</c> if caps lock is on, <c>false</c> otherwise.</returns>
        public static bool GetCapsLockOn()
        {
            return Keyboard.GetKeyStates(Key.CapsLock) == KeyStates.Toggled;
        }
    }
}