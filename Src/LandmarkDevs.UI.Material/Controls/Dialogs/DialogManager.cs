#region
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LandmarkDevs.UI.Material.Controls.Windows;
#endregion

namespace LandmarkDevs.UI.Material.Controls.Dialogs
{
    /// <summary>
    ///     Class DialogManager.
    /// </summary>
    public static class DialogManager
    {
        #region Show Dialog Methods
        /// <summary>
        ///     Shows the dialog asynchronously.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;DialogResult&gt;.</returns>
        public static Task<DialogResult> ShowDialogAsync(this MaterialDesignWindow window, string title, string message)
        {
            return ShowDialogAsync(window, title, message, null);
        }

        /// <summary>
        ///     Shows the dialog asynchronously.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="style">The style.</param>
        /// <returns>Task&lt;DialogResult&gt;.</returns>
        public static Task<DialogResult> ShowDialogAsync(this MaterialDesignWindow window, string title, string message,
                                                         DialogStyle style)
        {
            return ShowDialogAsync(window, title, message, style, null);
        }

        /// <summary>
        ///     Shows the dialog asynchronously.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>Task&lt;DialogResult&gt;.</returns>
        public static Task<DialogResult> ShowDialogAsync(this MaterialDesignWindow window, string title, string message,
                                                         DialogSettings settings)
        {
            return ShowDialogAsync(window, title, message, DialogStyle.Ok, settings);
        }

        /// <summary>
        ///     Shows the dialog asynchronously.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="style">The style.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>Task&lt;DialogResult&gt;.</returns>
        public static Task<DialogResult> ShowDialogAsync(this MaterialDesignWindow window, string title, string message,
                                                         DialogStyle style, DialogSettings settings)
        {
            // Verify UI Access.
            window.Dispatcher.VerifyAccess();
            // Show the window shade
            return ShowWindowShade(window).ContinueWith(z =>
            {
                // Return the dialog result
                return window.Dispatcher.Invoke(() =>
                {
                    // Create the dialog
                    var settings1 = settings;
                    if (settings == null)
                        settings1 = window.DialogOptions;
                    var dialog = new AppDialog(window, settings1)
                    {
                        Title = title,
                        Message = message,
                        ButtonStyle = style
                    };
                    var sizeHandler = SetupAndOpenDialog(window, dialog);
                    dialog.SizeChangedHandler = sizeHandler;
                    // Load the dialog
                    return dialog.WaitForLoadAsync().ContinueWith(x =>
                    {
                        if (DialogOpened != null)
                            window.Dispatcher.BeginInvoke(
                                new Action(() => DialogOpened(window, new DialogStateChangedEventArgs())));
                        // Wait for the user to press ok.
                        return dialog.WaitForButtonPressAsync().ContinueWith(y =>
                        {
                            dialog.OnClose();
                            if (DialogClosed != null)
                                window.Dispatcher.BeginInvoke(
                                    new Action(() => DialogClosed(window, new DialogStateChangedEventArgs())));
                            var closingTask = window.Dispatcher.Invoke(() => dialog.WaitForCloseAsync());
                            return closingTask.ContinueWith(a =>
                            {
                                return window.Dispatcher.Invoke(() =>
                                {
                                    window.SizeChanged -= sizeHandler;
                                    window.RemoveDialog(dialog);
                                    return HideWindowShade(window);
                                }).ContinueWith(b => y).Unwrap();
                            });
                        }).Unwrap();
                    }).Unwrap().Unwrap();
                });
            }).Unwrap();
        }

        /// <summary>
        ///     Shows the dialog.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="style">The style.</param>
        /// <param name="settings">The settings.</param>
        public static void ShowDialog(this MaterialDesignWindow window, string title, string message,
                                      DialogStyle style, DialogSettings settings = null)
        {
            window.Dispatcher.VerifyAccess();

            var task = new Task(async () =>
            {
                await ShowWindowShadeNonAsync(window).ContinueWith(z =>
                {
                    // Return the dialog result
                    return window.Dispatcher.Invoke(() =>
                    {
                        // Create the dialog
                        if (settings == null)
                            settings = window.DialogOptions;
                        var dialog = new AppDialog(window, settings)
                        {
                            Title = title,
                            Message = message,
                            ButtonStyle = style
                        };
                        var sizeHandler = SetupAndOpenDialog(window, dialog);
                        dialog.SizeChangedHandler = sizeHandler;
                        // Load the dialog
                        return dialog.WaitForLoadAsync().ContinueWith(x =>
                        {
                            if (DialogOpened != null)
                                window.Dispatcher.BeginInvoke(
                                    new Action(() => DialogOpened(window, new DialogStateChangedEventArgs())));
                            // Wait for the user to press ok.
                            return dialog.WaitForButtonPressAsync().ContinueWith(y =>
                            {
                                dialog.OnClose();
                                if (DialogClosed != null)
                                    window.Dispatcher.BeginInvoke(
                                        new Action(() => DialogClosed(window, new DialogStateChangedEventArgs())));
                                var closingTask = window.Dispatcher.Invoke(() => dialog.WaitForCloseAsync());
                                return closingTask.ContinueWith(a =>
                                {
                                    return window.Dispatcher.Invoke(() =>
                                    {
                                        window.SizeChanged -= sizeHandler;
                                        window.RemoveDialog(dialog);
                                        return HideWindowShade(window);
                                    }).ContinueWith(b => y).Unwrap();
                                });
                            }).Unwrap();
                        }).Unwrap().Unwrap();
                    });
                }).Unwrap();
            });
            task.Start();
        }
        #endregion

        #region Applies to all dialogs
        private static async Task ShowWindowShadeNonAsync(MaterialDesignWindow window)
        {
            await ShowWindowShade(window);
        }

        private static Task ShowWindowShade(MaterialDesignWindow window)
        {
            return
                Task.Factory.StartNew(() => window.Dispatcher.Invoke(window.ShowWindowShade))
                    .ContinueWith(c => window.Dispatcher.Invoke(window.ShowDialogContainer));
        }

        private static Task HideWindowShade(MaterialDesignWindow window)
        {
            return
                Task.Factory.StartNew(() => window.Dispatcher.Invoke(window.HideWindowShade))
                    .ContinueWith(c => window.Dispatcher.Invoke(window.HideDialogContainer));
        }

        /// <summary>
        ///     Gets the current dialog asynchronously.
        /// </summary>
        /// <typeparam name="TDialog">The type of the t dialog.</typeparam>
        /// <param name="window">The window.</param>
        /// <returns>Task&lt;TDialog&gt;.</returns>
        public static Task<TDialog> GetCurrentDialogAsync<TDialog>(this MaterialDesignWindow window)
            where TDialog : DialogBase
        {
            window.Dispatcher.VerifyAccess();
            var t = new TaskCompletionSource<TDialog>();
            window.Dispatcher.Invoke(() =>
            {
                TDialog dialog = window.ActiveDialogContainer?.Children.OfType<TDialog>().LastOrDefault();
                t.TrySetResult(dialog);
            });
            return t.Task;
        }

        private static SizeChangedEventHandler SetupAndOpenDialog(MaterialDesignWindow window, DialogBase dialog)
        {
            dialog.SetValue(Panel.ZIndexProperty, 20);
            dialog.MinHeight = window.ActualHeight / 4.0;
            dialog.MaxHeight = window.ActualHeight;
            SizeChangedEventHandler sizeHandler = (sender, args) =>
            {
                dialog.MinHeight = window.ActualHeight / 4.0;
                dialog.MaxHeight = window.ActualHeight;
            };
            window.SizeChanged += sizeHandler;
            window.AddDialog(dialog);
            dialog.OnShown();
            return sizeHandler;
        }

        private static SizeChangedEventHandler SetupAndOpenDialog(MaterialDesignWindow window, AppDialog dialog)
        {
            dialog.SetValue(Panel.ZIndexProperty, 20);
            dialog.MinHeight = window.ActualHeight / 4.0;
            dialog.MaxHeight = window.ActualHeight;
            SizeChangedEventHandler sizeHandler = (sender, args) =>
            {
                dialog.MinHeight = window.ActualHeight / 4.0;
                dialog.MaxHeight = window.ActualHeight;
            };
            window.SizeChanged += sizeHandler;
            window.AddDialog(dialog);
            dialog.OnShown();
            return sizeHandler;
        }

        private static void AddDialog(this MaterialDesignWindow window, AppDialog dialog)
        {
            var activeDialog = window.ActiveDialogContainer.Children.Cast<UIElement>().SingleOrDefault();
            if (activeDialog != null)
            {
                window.ActiveDialogContainer.Children.Remove(activeDialog);
                window.InactiveDialogContainer.Children.Add(activeDialog);
            }
            window.ActiveDialogContainer.Children.Add(dialog);
        }

        private static void RemoveDialog(this MaterialDesignWindow window, AppDialog dialog)
        {
            if (window.ActiveDialogContainer.Children.Contains(dialog))
            {
                window.ActiveDialogContainer.Children.Remove(dialog);
                var dlg = window.InactiveDialogContainer.Children.Cast<UIElement>().LastOrDefault();
                if (dlg != null)
                {
                    window.InactiveDialogContainer.Children.Remove(dlg);
                    window.ActiveDialogContainer.Children.Add(dlg);
                }
            }
            else
            {
                window.InactiveDialogContainer.Children.Remove(dialog);
            }
        }

        private static void AddDialog(this MaterialDesignWindow window, DialogBase dialog)
        {
            var activeDialog = window.ActiveDialogContainer.Children.Cast<UIElement>().SingleOrDefault();
            if (activeDialog != null)
            {
                window.ActiveDialogContainer.Children.Remove(activeDialog);
                window.InactiveDialogContainer.Children.Add(activeDialog);
            }
            window.ActiveDialogContainer.Children.Add(dialog);
        }

        private static void RemoveDialog(this MaterialDesignWindow window, DialogBase dialog)
        {
            if (window.ActiveDialogContainer.Children.Contains(dialog))
            {
                window.ActiveDialogContainer.Children.Remove(dialog);
                var dlg = window.InactiveDialogContainer.Children.Cast<UIElement>().LastOrDefault();
                if (dlg != null)
                {
                    window.InactiveDialogContainer.Children.Remove(dlg);
                    window.ActiveDialogContainer.Children.Add(dlg);
                }
            }
            else
            {
                window.InactiveDialogContainer.Children.Remove(dialog);
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