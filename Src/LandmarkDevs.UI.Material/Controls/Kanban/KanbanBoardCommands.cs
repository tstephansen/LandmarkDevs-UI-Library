#region
using System.Windows.Input;

#endregion

namespace LandmarkDevs.UI.Material.Controls.Kanban
{
    /// <summary>
    ///     Class KanbanBoardCommands.
    /// </summary>
    public static class KanbanBoardCommands
    {
        /// <summary>
        ///     Gets the cancel new kanban item command.
        /// </summary>
        /// <value>The cancel new kanban item command.</value>
        public static RoutedCommand CancelNewKanbanItemCommand { get; } = new RoutedCommand();

        /// <summary>
        ///     Gets the save new kanban item command.
        /// </summary>
        /// <value>The save new kanban item command.</value>
        public static RoutedCommand SaveNewKanbanItemCommand { get; } = new RoutedCommand();

        /// <summary>
        ///     Gets the add new kanban item command.
        /// </summary>
        /// <value>The add new kanban item command.</value>
        public static RoutedCommand AddNewKanbanItemCommand { get; } = new RoutedCommand();

        /// <summary>
        ///     Gets the close kanban item command.
        /// </summary>
        /// <value>The close kanban item command.</value>
        public static RoutedCommand CloseKanbanItemCommand { get; } = new RoutedCommand();

        /// <summary>
        ///     Gets the edit kanban item command.
        /// </summary>
        /// <value>The edit kanban item command.</value>
        public static RoutedCommand EditKanbanItemCommand { get; } = new RoutedCommand();
    }
}