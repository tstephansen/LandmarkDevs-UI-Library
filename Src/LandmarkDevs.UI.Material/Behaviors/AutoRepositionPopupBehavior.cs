using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LandmarkDevs.UI.Material.Behaviors
{
    /// <summary>
    /// Repositions the popup when teh window is moved.
    /// From http://putridparrot.com/blog/automatically-update-a-wpf-popup-position/
    /// </summary>
    public class AutoRepositionPopupBehavior : Behavior<Popup>
    {
        private const int WM_MOVING = 0x0216;

        // should be moved to a helper class
        private DependencyObject GetTopmostParent(DependencyObject element)
        {
            var current = element;
            var result = element;

            while (current != null)
            {
                result = current;
                current = (current is Visual || current is Visual3D) ?
                   VisualTreeHelper.GetParent(current) :
                   LogicalTreeHelper.GetParent(current);
            }
            return result;
        }

        /// <summary>
        /// Called when [attached].
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += (sender, e) =>
            {
                var root = GetTopmostParent(AssociatedObject.PlacementTarget) as Window;
                if (root != null)
                {
                    var helper = new WindowInteropHelper(root);
                    var hwndSource = HwndSource.FromHwnd(helper.Handle);
                    if (hwndSource != null)
                    {
                        hwndSource.AddHook(HwndMessageHook);
                    }
                }
            };

            AssociatedObject.LayoutUpdated += (sender, e) => Update();
        }

        private IntPtr HwndMessageHook(IntPtr hWnd,
                int msg, IntPtr wParam,
                IntPtr lParam, ref bool bHandled)
        {
            if (msg == WM_MOVING)
            {
                Update();
            }
            return IntPtr.Zero;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            // force the popup to update it's position
            var mode = AssociatedObject.Placement;
            AssociatedObject.Placement = PlacementMode.Relative;
            AssociatedObject.Placement = mode;
        }
    }
}