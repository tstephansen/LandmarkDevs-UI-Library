using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using LandmarkDevs.UI.Models.Dialogs;
// ReSharper disable InconsistentNaming
#pragma warning disable CC0022

namespace LandmarkDevs.UI.Material.Controls.Dialogs
{
    [TemplatePart(Name = PART_WindowShadeContentControl, Type = typeof(ContentControl))]
    [TemplatePart(Name = PART_WindowShade, Type = typeof(Rectangle))]
    [TemplatePart(Name = PART_DialogsLayoutRoot, Type = typeof(Grid))]
    [TemplatePart(Name = PART_InactiveDialogContainer, Type = typeof(Grid))]
    [TemplatePart(Name = PART_ActiveDialogContainer, Type = typeof(Grid))]
    public class DialogHost : ContentControl
    {

        /// <summary>
        ///     Initializes static members of the <see cref="DialogHost" /> class.
        /// </summary>
        static DialogHost()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogHost), new FrameworkPropertyMetadata(typeof(DialogHost)));
        }

        /// <summary>
        ///     When overridden in a derived class, is invoked whenever application code or internal processes call
        ///     <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            windowShadeContentControl = GetTemplateChild(PART_WindowShadeContentControl) as ContentControl;
            dialogsLayoutRoot = GetTemplateChild(PART_DialogsLayoutRoot) as Grid;
            InactiveDialogContainer = GetTemplateChild(PART_InactiveDialogContainer) as Grid;
            ActiveDialogContainer = GetTemplateChild(PART_ActiveDialogContainer) as Grid;
        }

        #region Template Properties
        private const string PART_DialogsLayoutRoot = "PART_DialogsLayoutRoot";
        private const string PART_InactiveDialogContainer = "PART_InactiveDialogContainer";
        private const string PART_ActiveDialogContainer = "PART_ActiveDialogContainer";
        private const string PART_WindowShadeContentControl = "PART_WindowShadeContentControl";
        private const string PART_WindowShade = "PART_WindowShade";
        private Grid dialogsLayoutRoot;
        internal Grid InactiveDialogContainer;
        internal Grid ActiveDialogContainer;
        private ContentControl windowShadeContentControl;
        #endregion

        /// <summary>
        ///     Shows the dialog container.
        /// </summary>
        public void ShowDialogContainer()
        {
            dialogsLayoutRoot.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Hides the dialog container.
        /// </summary>
        public void HideDialogContainer()
        {
            dialogsLayoutRoot.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        ///     The dialog content property
        /// </summary>
        public static readonly DependencyProperty DialogContentProperty = DependencyProperty.Register(
            nameof(DialogContent), typeof(ContentControl), typeof(DialogHost),
            new PropertyMetadata(default(ContentControl)));

        /// <summary>
        ///     Gets or sets the content of the dialog.
        /// </summary>
        /// <value>The content of the dialog.</value>
        public ContentControl DialogContent
        {
            get => (ContentControl)GetValue(DialogContentProperty);
            set => SetValue(DialogContentProperty, value);
        }

        /// <summary>
        ///     The dialog options property
        /// </summary>
        public static readonly DependencyProperty DialogOptionsProperty = DependencyProperty.Register(nameof(DialogOptions),
                                                                                                      typeof(DialogSettings), typeof(DialogHost),
                                                                                                      new PropertyMetadata(new DialogSettings()));

        /// <summary>
        ///     Gets or sets the dialog options.
        /// </summary>
        /// <value>The dialog options.</value>
        public DialogSettings DialogOptions
        {
            get => (DialogSettings)GetValue(DialogOptionsProperty);
            set => SetValue(DialogOptionsProperty, value);
        }

        /// <summary>
        ///     The is dialog visible property
        /// </summary>
        public static readonly DependencyProperty IsDialogVisibleProperty = DependencyProperty.Register(
            nameof(IsDialogVisible), typeof(bool), typeof(DialogHost), new PropertyMetadata(default(bool)));

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is dialog visible.
        /// </summary>
        /// <value><c>true</c> if this instance is dialog visible; otherwise, <c>false</c>.</value>
        public bool IsDialogVisible
        {
            get => (bool)GetValue(IsDialogVisibleProperty);
            set => SetValue(IsDialogVisibleProperty, value);
        }

        /// <summary>
        ///     Shows the window shade.
        /// </summary>
        public void ShowWindowShade()
        {
            windowShadeContentControl.Visibility = Visibility.Visible;
            windowShadeContentControl.SetCurrentValue(OpacityProperty, 0.7);
        }

        /// <summary>
        ///     Hides the window shade.
        /// </summary>
        public void HideWindowShade()
        {
            windowShadeContentControl.SetCurrentValue(OpacityProperty, 0.0);
            windowShadeContentControl.Visibility = Visibility.Hidden;
        }

        #region Dialog Activation Methods
        /// <summary>
        ///     Shows the dialog asynchronously.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;DialogResult&gt;.</returns>
        public Task<DialogResult> ShowDialogAsync(string title, string message) => ShowDialogAsync(title, message, null);

        /// <summary>
        ///     Shows the dialog asynchronously.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="style">The style.</param>
        /// <returns>Task&lt;DialogResult&gt;.</returns>
        public Task<DialogResult> ShowDialogAsync(string title, string message,
            DialogStyle style) => ShowDialogAsync(title, message, style, null);

        /// <summary>
        ///     Shows the dialog asynchronously.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>Task&lt;DialogResult&gt;.</returns>
        public Task<DialogResult> ShowDialogAsync(string title, string message,
            DialogSettings settings) => ShowDialogAsync(title, message, DialogStyle.Ok, settings);

        /// <summary>
        ///     Shows the dialog asynchronously.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="style">The style.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>Task&lt;DialogResult&gt;.</returns>
        public Task<DialogResult> ShowDialogAsync(string title, string message,
                                                         DialogStyle style, DialogSettings settings)
        {
            // Verify UI Access.
            Dispatcher.VerifyAccess();
            // Show the window shade
            return ShowWindowShadeAsync().ContinueWith(z =>
            {
                // Return the dialog result
                return Dispatcher.Invoke(() =>
                {
                    // Create the dialog
                    var settings1 = settings;
                    if (settings == null)
                        settings1 = DialogOptions;
                    var dialog = new ButtonDialog(this, settings1)
                    {
                        Title = title,
                        Message = message,
                        ButtonStyle = style
                    };
                    var sizeHandler = SetupAndOpenDialog(dialog);
                    dialog.SizeChangedHandler = sizeHandler;
                    // Load the dialog
                    return dialog.WaitForLoadAsync().ContinueWith(x =>
                    {
                        if (DialogOpened != null)
                            Dispatcher.BeginInvoke(
                                new Action(() => DialogOpened(this, new DialogStateChangedEventArgs())));
                        // Wait for the user to press a button.
                        return dialog.WaitForButtonPressAsync().ContinueWith(y =>
                        {
                            // Start closing the dialog.
                            dialog.OnClose();
                            if (DialogClosed != null)
                                Dispatcher.BeginInvoke(
                                    new Action(() => DialogClosed(this, new DialogStateChangedEventArgs())));
                            var closingTask = Dispatcher.Invoke(() => dialog.WaitForCloseAsync());
                            return closingTask.ContinueWith(a =>
                            {
                                return Dispatcher.Invoke(() =>
                                {
                                    SizeChanged -= sizeHandler;
                                    RemoveDialog(dialog);
                                    return HideWindowShadeAsync();
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
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="settings">The settings.</param>
        public void ShowDialog(string title, string message, DialogSettings settings = null)
        {
            Dispatcher.VerifyAccess();
            var task = new Task(async () =>
            {
                await ShowWindowShadeNonAsync().ContinueWith(z =>
                {
                    // Return the dialog result
                    return Dispatcher.Invoke(() =>
                    {
                        // Create the dialog
                        if (settings == null)
                            settings = DialogOptions;
                        var dialog = new ButtonDialog(this, settings)
                        {
                            Title = title,
                            Message = message,
                            ButtonStyle = DialogStyle.Ok
                        };
                        var sizeHandler = SetupAndOpenDialog(dialog);
                        dialog.SizeChangedHandler = sizeHandler;
                        // Load the dialog
                        return dialog.WaitForLoadAsync().ContinueWith(x =>
                        {
                            if (DialogOpened != null)
                                Dispatcher.BeginInvoke(
                                    new Action(() => DialogOpened(this, new DialogStateChangedEventArgs())));
                            // Wait for the user to press ok.
                            return dialog.WaitForButtonPressAsync().ContinueWith(y =>
                            {
                                dialog.OnClose();
                                if (DialogClosed != null)
                                    Dispatcher.BeginInvoke(
                                        new Action(() => DialogClosed(this, new DialogStateChangedEventArgs())));
                                var closingTask = Dispatcher.Invoke(() => dialog.WaitForCloseAsync());
                                return closingTask.ContinueWith(a =>
                                {
                                    return Dispatcher.Invoke(() =>
                                    {
                                        SizeChanged -= sizeHandler;
                                        RemoveDialog(dialog);
                                        return HideWindowShadeAsync();
                                    }).ContinueWith(b => y).Unwrap();
                                });
                            }).Unwrap();
                        }).Unwrap().Unwrap();
                    });
                }).Unwrap();
            });
            task.Start();
        }

        private async Task ShowWindowShadeNonAsync() => await ShowWindowShadeAsync();

        private Task ShowWindowShadeAsync() => Task.Factory.StartNew(() => Dispatcher.Invoke(ShowWindowShade))
            .ContinueWith(c => Dispatcher.Invoke(ShowDialogContainer));

        private Task HideWindowShadeAsync() => Task.Factory.StartNew(() => Dispatcher.Invoke(HideWindowShade))
            .ContinueWith(c => Dispatcher.Invoke(HideDialogContainer));

        private SizeChangedEventHandler SetupAndOpenDialog(ButtonDialog dialog)
        {
            dialog.SetValue(Panel.ZIndexProperty, 20);
            dialog.MinHeight = ActualHeight / 4.0;
            dialog.MaxHeight = ActualHeight;
            void SizeHandler(object sender, SizeChangedEventArgs args)
            {
                dialog.MinHeight = ActualHeight / 4.0;
                dialog.MaxHeight = ActualHeight;
            }
            SizeChanged += SizeHandler;
            AddDialog(dialog);
            dialog.OnShown();
            return SizeHandler;
        }

        private void AddDialog(ButtonDialog dialog)
        {
            var activeDialog = ActiveDialogContainer.Children.Cast<UIElement>().SingleOrDefault();
            if (activeDialog != null)
            {
                ActiveDialogContainer.Children.Remove(activeDialog);
                InactiveDialogContainer.Children.Add(activeDialog);
            }
            ActiveDialogContainer.Children.Add(dialog);
        }

        private void RemoveDialog(ButtonDialog dialog)
        {
            if (ActiveDialogContainer.Children.Contains(dialog))
            {
                ActiveDialogContainer.Children.Remove(dialog);
                var dlg = InactiveDialogContainer.Children.Cast<UIElement>().LastOrDefault();
                if (dlg == null) return;
                InactiveDialogContainer.Children.Remove(dlg);
                ActiveDialogContainer.Children.Add(dlg);
            }
            else
            {
                InactiveDialogContainer.Children.Remove(dialog);
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
