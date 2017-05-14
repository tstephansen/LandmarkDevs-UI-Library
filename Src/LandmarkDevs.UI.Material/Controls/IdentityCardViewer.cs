#region
using System.Windows;
using System.Windows.Controls;
#endregion

namespace LandmarkDevs.UI.Material.Controls
{
    /// <summary>
    /// Class IdentityCardViewer.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ListView" />
    public class IdentityCardViewer : ListView
    {
        /// <summary>
        /// Initializes static members of the <see cref="IdentityCardViewer"/> class.
        /// </summary>
        static IdentityCardViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IdentityCardViewer), new FrameworkPropertyMetadata(typeof(IdentityCardViewer)));
        }
    }
}