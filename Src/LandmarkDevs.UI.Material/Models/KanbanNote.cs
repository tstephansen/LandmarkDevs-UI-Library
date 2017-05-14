#region
using System;

#endregion

namespace LandmarkDevs.UI.Material.Models
{
    /// <summary>
    ///     Class KanbanNote.
    /// </summary>
    public class KanbanNote
    {
        /// <summary>
        ///     Gets or sets the note.
        /// </summary>
        /// <value>The note.</value>
        public string Note { get; set; }

        /// <summary>
        ///     Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public string CreatedBy { get; set; }

        /// <summary>
        ///     Gets or sets the time.
        /// </summary>
        /// <value>The time.</value>
        public DateTime Time { get; set; }
    }
}