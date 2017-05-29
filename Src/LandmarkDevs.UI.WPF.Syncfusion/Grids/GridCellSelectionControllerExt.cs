using System.Windows.Input;
using Syncfusion.UI.Xaml.Grid;

namespace LandmarkDevs.UI.WPF.Syncfusion.Grids
{
    /// <summary>
    /// Class GridCellSelectionControllerExt.
    /// </summary>
    /// <seealso cref="GridCellSelectionController" />
    public class GridCellSelectionControllerExt : GridCellSelectionController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridCellSelectionControllerExt"/> class.
        /// </summary>
        /// <param name="dataGrid">The data grid.</param>
        public GridCellSelectionControllerExt(SfDataGrid dataGrid)
            : base(dataGrid)
        {

        }

        /// <summary>
        /// Processes the key down.
        /// </summary>
        /// <param name="args">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        protected override void ProcessKeyDown(KeyEventArgs args)
        {
            if (args.Key == Key.Enter)
            {
                args.Handled = false;
                return;
            }
            base.ProcessKeyDown(args);
        }
    }
}
