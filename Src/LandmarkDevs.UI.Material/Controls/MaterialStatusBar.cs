#region
using System.Windows;
using System.Windows.Controls;

#endregion

namespace LandmarkDevs.UI.Material.Controls
{
    /// <summary>
    ///     Class MaterialStatusBar.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ContentControl" />
    public class MaterialStatusBar : ContentControl
    {
        static MaterialStatusBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialStatusBar),
                new FrameworkPropertyMetadata(typeof(MaterialStatusBar)));
        }
    }
}