using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandmarkDevs.UI.Material.Dialogs
{
    /// <summary>
    ///     Interface IDialogCoordinator
    /// </summary>
    public interface IDialogCoordinator
    {
        /// <summary>
        ///     Shows the dialog asynchronously.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="style">The style.</param>
        /// <returns>Task&lt;DialogResult&gt;.</returns>
        Task<DialogResult> ShowDialogAsync(object context, string title, string message,
            DialogStyle style);

        /// <summary>
        ///     Shows the dialog asynchronously.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>Task&lt;DialogResult&gt;.</returns>
        Task<DialogResult> ShowDialogAsync(object context, string title, string message, DialogSettings settings);

        /// <summary>
        ///     Shows the dialog asynchronously.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;DialogResult&gt;.</returns>
        Task<DialogResult> ShowDialogAsync(object context, string title, string message);

        /// <summary>
        ///     Shows the dialog.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        void ShowDialog(object context, string title, string message);
    }
}
