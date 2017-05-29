#region

using System.Windows;
using System.Windows.Input;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Helpers;
using Syncfusion.UI.Xaml.ScrollAxis;

#endregion

namespace LandmarkDevs.UI.WPF.Syncfusion.Grids
{
    /// <summary>
    ///     Class SfDataGridExt.
    /// </summary>
    /// <seealso cref="SfDataGrid" />
    public class SfDataGridExt : SfDataGrid
    {
        /// <summary>
        ///     Builds the visual tree for the SfDataGrid when a new template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            MouseDoubleClick += SfDataGridExt_MouseDoubleClick;
        }

        private void SfDataGridExt_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vc = this.GetVisualContainer();
            MousePositionPoint = vc.PointToCellRowColumnIndex(e.GetPosition(vc));
        }

        /// <summary>
        ///     The mouse position point property
        /// </summary>
        public static readonly DependencyProperty MousePositionPointProperty = DependencyProperty.Register(
            "MousePositionPoint", typeof(RowColumnIndex), typeof(SfDataGridExt),
            new PropertyMetadata(default(RowColumnIndex)));

        /// <summary>
        ///     Gets or sets the mouse position point.
        /// </summary>
        /// <value>The mouse position point.</value>
        public RowColumnIndex MousePositionPoint
        {
            get { return (RowColumnIndex) GetValue(MousePositionPointProperty); }
            set { SetValue(MousePositionPointProperty, value); }
        }
    }
}