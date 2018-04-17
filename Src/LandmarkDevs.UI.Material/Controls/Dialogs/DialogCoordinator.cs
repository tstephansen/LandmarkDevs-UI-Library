using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LandmarkDevs.UI.Material.Dialogs
{
    /// <summary>
    ///     Class DialogCoordinator.
    /// </summary>
    /// <seealso cref="LandmarkDevs.UI.Material.Controls.Dialogs.IDialogCoordinator" />
    public class DialogCoordinator : IDialogCoordinator
    {
        /// <summary>
        ///     The instance
        /// </summary>
        public static readonly DialogCoordinator Instance = new DialogCoordinator();

        /// <summary>
        ///     Shows the dialog asynchronously.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;DialogResult&gt;.</returns>
        public Task<DialogResult> ShowDialogAsync(object context, string title, string message)
        {
            var host = GetHost(context);
            return host.ShowDialogAsync(title, message);
        }

        /// <summary>
        ///     Shows the dialog asynchronously.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>Task&lt;DialogResult&gt;.</returns>
        public Task<DialogResult> ShowDialogAsync(object context, string title, string message, DialogSettings settings)
        {
            var host = GetHost(context);
            return host.ShowDialogAsync(title, message, DialogStyle.Ok, settings);
        }

        /// <summary>
        ///     Shows the dialog asynchronously.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="style">The style.</param>
        /// <returns>Task&lt;DialogResult&gt;.</returns>
        public Task<DialogResult> ShowDialogAsync(object context, string title, string message, DialogStyle style)
        {
            var host = GetHost(context);
            return host.ShowDialogAsync(title, message, style);
        }

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        public void ShowDialog(object context, string title, string message)
        {
            var host = GetHost(context);
            host.ShowDialog(title, message, DialogStyle.Ok);
        }

        /// <summary>
        ///     Shows the dialog asynchronously.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="style">The style.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>Task&lt;DialogResult&gt;.</returns>
        public Task<DialogResult> ShowDialogAsync(object context, string title, string message,
                                                  DialogStyle style, DialogSettings settings)
        {
            var host = GetHost(context);
            return host.ShowDialogAsync(title, message, style, settings);
        }

        //private static DialogHost GetHost(object context)
        //{
        //    if (context == null)
        //        throw new ArgumentNullException(nameof(context));
        //    //if (!DialogParticipation.IsRegistered(context))
        //    //    throw new InvalidOperationException(
        //    //        "Context is not registered. Consider using DialogParticipation.Register in XAML to bind in the DataContext.");

        //    var association = DialogParticipation.GetAssociation(context);
        //    var control = Window.GetWindow(association) as Window;
        //    //var responsiveWindow = Window.GetWindow(association) as MaterialDesignWindow;

        //    if (control == null)
        //        throw new InvalidOperationException("Control is not inside a ResponsiveWindow.");
        //    //return control;
        //}
    }
}
