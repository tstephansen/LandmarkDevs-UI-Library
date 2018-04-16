using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LandmarkDevs.UI.Material.Dialogs
{
    public static class DialogManager
    {
        /// <summary>
        ///     Shows the dialog asynchronously.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;DialogResult&gt;.</returns>
        public static Task<DialogResult> ShowDialogAsync(this DialogHost host, string title, string message) => ShowDialogAsync(host, title, message, null);

        /// <summary>
        ///     Shows the dialog asynchronously.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="style">The style.</param>
        /// <returns>Task&lt;DialogResult&gt;.</returns>
        public static Task<DialogResult> ShowDialogAsync(this DialogHost host, string title, string message,
            DialogStyle style) => ShowDialogAsync(host, title, message, style, null);

        /// <summary>
        ///     Shows the dialog asynchronously.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>Task&lt;DialogResult&gt;.</returns>
        public static Task<DialogResult> ShowDialogAsync(this DialogHost host, string title, string message,
            DialogSettings settings) => ShowDialogAsync(host, title, message, DialogStyle.Ok, settings);

        /// <summary>
        ///     Shows the dialog asynchronously.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="style">The style.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>Task&lt;DialogResult&gt;.</returns>
        public static Task<DialogResult> ShowDialogAsync(this DialogHost host, string title, string message,
                                                         DialogStyle style, DialogSettings settings)
        {
            // Verify UI Access.
            host.Dispatcher.VerifyAccess();
            // Show the window shade
            return ShowWindowShade(host).ContinueWith(z =>
            {
                // Return the dialog result
                return host.Dispatcher.Invoke(() =>
                {
                    // Create the dialog
                    var settings1 = settings;
                    if (settings == null)
                        settings1 = host.DialogOptions;
                    var dialog = new ButtonDialog(host, settings1)
                    {
                        Title = title,
                        Message = message,
                        ButtonStyle = style
                    };
                    var sizeHandler = SetupAndOpenDialog(host, dialog);
                    dialog.SizeChangedHandler = sizeHandler;
                    // Load the dialog
                    return dialog.WaitForLoadAsync().ContinueWith(x =>
                    {
                        if (DialogOpened != null)
                            host.Dispatcher.BeginInvoke(
                                new Action(() => DialogOpened(host, new DialogStateChangedEventArgs())));
                        // Wait for the user to press ok.
                        return dialog.WaitForButtonPressAsync().ContinueWith(y =>
                        {
                            dialog.OnClose();
                            if (DialogClosed != null)
                                host.Dispatcher.BeginInvoke(
                                    new Action(() => DialogClosed(host, new DialogStateChangedEventArgs())));
                            var closingTask = host.Dispatcher.Invoke(() => dialog.WaitForCloseAsync());
                            return closingTask.ContinueWith(a =>
                            {
                                return host.Dispatcher.Invoke(() =>
                                {
                                    host.SizeChanged -= sizeHandler;
                                    host.RemoveDialog(dialog);
                                    return HideWindowShade(host);
                                }).ContinueWith(b => y).Unwrap();
                            });
                        }).Unwrap();
                    }).Unwrap().Unwrap();
                });
            }).Unwrap();
        }

        #region Applies to all dialogs
        private static async Task ShowWindowShadeNonAsync(DialogHost host) => await ShowWindowShade(host);

        private static Task ShowWindowShade(DialogHost host)
        {
            return
                Task.Factory.StartNew(() => host.Dispatcher.Invoke(host.ShowWindowShade))
                    .ContinueWith(c => host.Dispatcher.Invoke(host.ShowDialogContainer));
        }

        private static Task HideWindowShade(DialogHost host)
        {
            return
                Task.Factory.StartNew(() => host.Dispatcher.Invoke(host.HideWindowShade))
                    .ContinueWith(c => host.Dispatcher.Invoke(host.HideDialogContainer));
        }

        /// <summary>
        ///     Gets the current dialog asynchronously.
        /// </summary>
        /// <typeparam name="TDialog">The type of the t dialog.</typeparam>
        /// <param name="host">The host.</param>
        /// <returns>Task&lt;TDialog&gt;.</returns>
        public static Task<TDialog> GetCurrentDialogAsync<TDialog>(this DialogHost host)
            where TDialog : BaseDialog
        {
            host.Dispatcher.VerifyAccess();
            var t = new TaskCompletionSource<TDialog>();
            host.Dispatcher.Invoke(() =>
            {
                TDialog dialog = host.ActiveDialogContainer?.Children.OfType<TDialog>().LastOrDefault();
                t.TrySetResult(dialog);
            });
            return t.Task;
        }

        private static SizeChangedEventHandler SetupAndOpenDialog(DialogHost host, BaseDialog dialog)
        {
            dialog.SetValue(Panel.ZIndexProperty, 20);
            dialog.MinHeight = host.ActualHeight / 4.0;
            dialog.MaxHeight = host.ActualHeight;
            void SizeHandler(object sender, SizeChangedEventArgs args)
            {
                dialog.MinHeight = host.ActualHeight / 4.0;
                dialog.MaxHeight = host.ActualHeight;
            }
            host.SizeChanged += SizeHandler;
            host.AddDialog(dialog);
            dialog.OnShown();
            return SizeHandler;
        }

        private static SizeChangedEventHandler SetupAndOpenDialog(DialogHost host, ButtonDialog dialog)
        {
            dialog.SetValue(Panel.ZIndexProperty, 20);
            dialog.MinHeight = host.ActualHeight / 4.0;
            dialog.MaxHeight = host.ActualHeight;
            void SizeHandler(object sender, SizeChangedEventArgs args)
            {
                dialog.MinHeight = host.ActualHeight / 4.0;
                dialog.MaxHeight = host.ActualHeight;
            }
            host.SizeChanged += SizeHandler;
            host.AddDialog(dialog);
            dialog.OnShown();
            return SizeHandler;
        }

        private static void AddDialog(this DialogHost host, ButtonDialog dialog)
        {
            var activeDialog = host.ActiveDialogContainer.Children.Cast<UIElement>().SingleOrDefault();
            if (activeDialog != null)
            {
                host.ActiveDialogContainer.Children.Remove(activeDialog);
                host.InactiveDialogContainer.Children.Add(activeDialog);
            }
            host.ActiveDialogContainer.Children.Add(dialog);
        }

        private static void RemoveDialog(this DialogHost host, ButtonDialog dialog)
        {
            if (host.ActiveDialogContainer.Children.Contains(dialog))
            {
                host.ActiveDialogContainer.Children.Remove(dialog);
                var dlg = host.InactiveDialogContainer.Children.Cast<UIElement>().LastOrDefault();
                if (dlg != null)
                {
                    host.InactiveDialogContainer.Children.Remove(dlg);
                    host.ActiveDialogContainer.Children.Add(dlg);
                }
            }
            else
            {
                host.InactiveDialogContainer.Children.Remove(dialog);
            }
        }

        private static void AddDialog(this DialogHost host, BaseDialog dialog)
        {
            var activeDialog = host.ActiveDialogContainer.Children.Cast<UIElement>().SingleOrDefault();
            if (activeDialog != null)
            {
                host.ActiveDialogContainer.Children.Remove(activeDialog);
                host.InactiveDialogContainer.Children.Add(activeDialog);
            }
            host.ActiveDialogContainer.Children.Add(dialog);
        }

        private static void RemoveDialog(this DialogHost host, BaseDialog dialog)
        {
            if (host.ActiveDialogContainer.Children.Contains(dialog))
            {
                host.ActiveDialogContainer.Children.Remove(dialog);
                var dlg = host.InactiveDialogContainer.Children.Cast<UIElement>().LastOrDefault();
                if (dlg != null)
                {
                    host.InactiveDialogContainer.Children.Remove(dlg);
                    host.ActiveDialogContainer.Children.Add(dlg);
                }
            }
            else
            {
                host.InactiveDialogContainer.Children.Remove(dialog);
            }
        }

        /// <summary>
        ///     Occurs when the dialog is opened.
        /// </summary>
        public static event EventHandler<DialogStateChangedEventArgs> DialogOpened;

        /// <summary>
        ///     Occurs when the dialog is closed.
        /// </summary>
        public static event EventHandler<DialogStateChangedEventArgs> DialogClosed;
        #endregion
    }
}
