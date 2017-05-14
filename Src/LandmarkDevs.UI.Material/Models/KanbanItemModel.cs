#region
using System;
using System.Collections.ObjectModel;

#endregion

namespace LandmarkDevs.UI.Material.Models
{
    /// <summary>
    ///     Class KanbanItemModel.
    /// </summary>
    public class KanbanItemModel
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="KanbanItemModel" /> class.
        /// </summary>
        public KanbanItemModel()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="KanbanItemModel" /> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public KanbanItemModel(KanbanItemModel model)
        {
            Title = model.Title;
            Summary = model.Summary;
            CreatedBy = model.CreatedBy;
            Created = model.Created;
            KanbanPriority = model.KanbanPriority;
            IsComplete = model.IsComplete;
            Completed = model.Completed;
            Updated = model.Updated;
            Notes = model.Notes;
        }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        ///     Gets or sets the summary.
        /// </summary>
        /// <value>The summary.</value>
        public string Summary { get; set; }

        /// <summary>
        ///     Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public string CreatedBy { get; set; }

        /// <summary>
        ///     Gets or sets the created.
        /// </summary>
        /// <value>The created.</value>
        public DateTime Created { get; set; }

        /// <summary>
        ///     Gets or sets the kanban priority.
        /// </summary>
        /// <value>The kanban priority.</value>
        public KanbanPriority KanbanPriority { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is complete.
        /// </summary>
        /// <value><c>true</c> if this instance is complete; otherwise, <c>false</c>.</value>
        public bool IsComplete { get; set; }

        /// <summary>
        ///     Gets or sets the completed.
        /// </summary>
        /// <value>The completed.</value>
        public DateTime? Completed { get; set; }

        /// <summary>
        ///     Gets or sets the updated.
        /// </summary>
        /// <value>The updated.</value>
        public DateTime Updated { get; set; }

        /// <summary>
        ///     Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public ObservableCollection<KanbanNote> Notes { get; set; }
    }
}