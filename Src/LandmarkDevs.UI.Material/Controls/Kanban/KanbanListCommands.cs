#region File Header

// ***********************************************************************
// Solution           : TDLS
// Assembly           : LandmarkDevs.UI.Material
// File               : KanbanListCommands.cs
// Created            : 01-04-2017  6:36 PM
// 
// Last Modified By   : Tim Stephansen
// Last Modified Date : 01-04-2017  6:37 PM
// ***********************************************************************
// <copyright file="KanbanListCommands.cs" company="LandmarkDevs">
//     Copyright © LandmarkDevs 2016
// </copyright>
// ***********************************************************************

#endregion

#region

using System.Windows.Controls;
using System.Windows.Input;

#endregion

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