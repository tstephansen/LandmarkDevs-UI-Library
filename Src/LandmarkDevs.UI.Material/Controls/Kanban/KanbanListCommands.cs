using System.Windows.Controls;
using System.Windows.Input;

namespace LandmarkDevs.UI.Material.Controls.Kanban
{
    /// <summary>
    ///     Class KanbanListCommands.
    /// </summary>
    public static class KanbanListCommands
    {
        /// <summary>
        ///     Gets the close edit kanban item command.
        /// </summary>
        /// <value>The close edit kanban item command.</value>
        public static RoutedCommand CloseEditKanbanItemCommand { get; } = new RoutedCommand();

        /// <summary>
        ///     Gets or sets the edit item.
        /// </summary>
        /// <value>The edit item.</value>
        public static KanbanItem EditItem { get; set; }

        /// <summary>
        ///     Gets or sets the kanban grid.
        /// </summary>
        /// <value>The kanban grid.</value>
        public static Grid KanbanGrid { get; set; }
    }
}