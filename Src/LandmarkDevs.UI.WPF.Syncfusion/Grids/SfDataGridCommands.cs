#region

using System.Windows.Input;

#endregion

namespace LandmarkDevs.UI.WPF.Syncfusion.Grids
{
    /// <summary>
    ///     Class SfDataGridCommands.
    /// </summary>
    public static class SfDataGridCommands
    {
        /// <summary>
        ///     Initializes static members of the <see cref="SfDataGridCommands" /> class.
        /// </summary>
        static SfDataGridCommands()
        {
            CopyDownCommand.InputGestures.Add(new KeyGesture(Key.D, ModifierKeys.Control));
        }

        /// <summary>
        ///     The copy down command
        /// </summary>
        public static readonly RoutedCommand CopyDownCommand = new RoutedCommand();
    }
}