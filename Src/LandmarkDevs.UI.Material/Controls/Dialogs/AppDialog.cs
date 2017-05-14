using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using LandmarkDevs.UI.Material.Controls.Windows;

namespace LandmarkDevs.UI.Material.Controls.Dialogs
{
    /// <summary>
    /// Class AppDialog.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ContentControl" />
    [TemplatePart(Name = PART_OkButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_CancelButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_NoButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_ExtraButton, Type = typeof(Button))]
    public class AppDialog : ContentControl
    {
        /// <summary>
        /// Gets or sets the size changed handler.
        /// </summary>
        /// <value>The size changed handler.</value>
        internal SizeChangedEventHandler SizeChangedHandler { get; set; }


        /// <summary>
        /// Initializes static members of the <see cref="AppDialog"/> class.
        /// </summary>
        static AppDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AppDialog), new FrameworkPropertyMetadata(typeof(AppDialog)));
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            OkButton = GetTemplateChild(PART_OkButton) as Button;
            CancelButton = GetTemplateChild(PART_CancelButton) as Button;
            NoButton = GetTemplateChild(PART_NoButton) as Button;
            ExtraButton = GetTemplateChild(PART_ExtraButton) as Button;
        }

        /// <summary>
        /// The ok button
        /// </summary>
        internal Button OkButton;
        /// <summary>
        /// The cancel button
        /// </summary>
        internal Button CancelButton;
        /// <summary>
        /// The no button
        /// </summary>
        internal Button NoButton;
        /// <summary>
        /// The extra button
        /// </summary>
        internal Button ExtraButton;
        /// <summary>
        /// The part ok button
        /// </summary>
        private const string PART_OkButton = "PART_OkButton";
        /// <summary>
        /// The part cancel button
        /// </summary>
        private const string PART_CancelButton = "PART_CancelButton";
        /// <summary>
        /// The part no button
        /// </summary>
        private const string PART_NoButton = "PART_NoButton";
        /// <summary>
        /// The part extra button
        /// </summary>
        private const string PART_ExtraButton = "PART_ExtraButton";

        /// <summary>
        /// Gets the dialog settings.
        /// </summary>
        /// <value>The dialog settings.</value>
        public DialogSettings DialogSettings { get; private set; }

        #region Dependency Properties
        /// <summary>
        /// The title property
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(AppDialog), new PropertyMetadata(default(string)));

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// The dialog top property
        /// </summary>
        public static readonly DependencyProperty DialogTopProperty = DependencyProperty.Register(
            "DialogTop", typeof(object), typeof(AppDialog), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the dialog top.
        /// </summary>
        /// <value>The dialog top.</value>
        public object DialogTop
        {
            get { return GetValue(DialogTopProperty); }
            set { SetValue(DialogTopProperty, value); }
        }

        /// <summary>
        /// The dialog bottom property
        /// </summary>
        public static readonly DependencyProperty DialogBottomProperty = DependencyProperty.Register(
            "DialogBottom", typeof(object), typeof(AppDialog), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the dialog bottom.
        /// </summary>
        /// <value>The dialog bottom.</value>
        public object DialogBottom
        {
            get { return GetValue(DialogBottomProperty); }
            set { SetValue(DialogBottomProperty, value); }
        }

        /// <summary>
        /// The message property
        /// </summary>
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(AppDialog), new PropertyMetadata(default(string)));

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        /// <summary>
        /// The ok button text property
        /// </summary>
        public static readonly DependencyProperty OkButtonTextProperty = DependencyProperty.Register("OkButtonText", typeof(string), typeof(AppDialog), new PropertyMetadata("OK"));

        /// <summary>
        /// Gets or sets the ok button text.
        /// </summary>
        /// <value>The ok button text.</value>
        public string OkButtonText
        {
            get { return (string)GetValue(OkButtonTextProperty); }
            set { SetValue(OkButtonTextProperty, value); }
        }

        /// <summary>
        /// The no button text property
        /// </summary>
        public static readonly DependencyProperty NoButtonTextProperty = DependencyProperty.Register("NoButtonText", typeof(string), typeof(AppDialog), new PropertyMetadata("No"));

        /// <summary>
        /// Gets or sets the no button text.
        /// </summary>
        /// <value>The no button text.</value>
        public string NoButtonText
        {
            get { return (string)GetValue(NoButtonTextProperty); }
            set { SetValue(NoButtonTextProperty, value); }
        }

        /// <summary>
        /// The cancel button text property
        /// </summary>
        public static readonly DependencyProperty CancelButtonTextProperty = DependencyProperty.Register("CancelButtonText", typeof(string), typeof(AppDialog), new PropertyMetadata("Cancel"));

        /// <summary>
        /// Gets or sets the cancel button text.
        /// </summary>
        /// <value>The cancel button text.</value>
        public string CancelButtonText
        {
            get { return (string)GetValue(CancelButtonTextProperty); }
            set { SetValue(CancelButtonTextProperty, value); }
        }

        /// <summary>
        /// The extra button text property
        /// </summary>
        public static readonly DependencyProperty ExtraButtonTextProperty = DependencyProperty.Register("ExtraButtonText", typeof(string), typeof(AppDialog), new PropertyMetadata("Yes"));

        /// <summary>
        /// Gets or sets the extra button text.
        /// </summary>
        /// <value>The extra button text.</value>
        public string ExtraButtonText
        {
            get { return (string)GetValue(ExtraButtonTextProperty); }
            set { SetValue(ExtraButtonTextProperty, value); }
        }

        /// <summary>
        /// The button style property
        /// </summary>
        public static readonly DependencyProperty ButtonStyleProperty =
            DependencyProperty.Register("ButtonStyle", typeof(DialogStyle), typeof(AppDialog), new PropertyMetadata(DialogStyle.Ok, (s, e) =>
            {
                var md = (AppDialog)s;
                SetButtonState(md);
            }));

        /// <summary>
        /// Gets or sets the button style.
        /// </summary>
        /// <value>The button style.</value>
        public DialogStyle ButtonStyle
        {
            get { return (DialogStyle)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }

        /// <summary>
        /// Sets the state of the button.
        /// </summary>
        /// <param name="sd">The sd.</param>
        private static void SetButtonState(AppDialog sd)
        {
            if (sd.OkButton == null)
                return;

            switch (sd.ButtonStyle)
            {
                default:
                    {
                        sd.OkButton.Visibility = Visibility.Visible;
                        sd.NoButton.Visibility = Visibility.Collapsed;
                        sd.CancelButton.Visibility = Visibility.Collapsed;
                        sd.ExtraButton.Visibility = Visibility.Collapsed;
                    }
                    break;
                case DialogStyle.OkNoCancel:
                case DialogStyle.OkNoCancelExtra:
                case DialogStyle.OkNo:
                    {
                        sd.OkButton.Visibility = Visibility.Visible;
                        sd.NoButton.Visibility = Visibility.Visible;

                        if (sd.ButtonStyle == DialogStyle.OkNoCancel || sd.ButtonStyle == DialogStyle.OkNoCancelExtra)
                        {
                            sd.CancelButton.Visibility = Visibility.Visible;
                        }

                        if (sd.ButtonStyle == DialogStyle.OkNoCancelExtra)
                        {
                            sd.ExtraButton.Visibility = Visibility.Visible;
                        }
                    }
                    break;
            }
            sd.OkButtonText = sd.DialogSettings.OkButtonText;
            sd.NoButtonText = sd.DialogSettings.NoButtonText;
            sd.CancelButtonText = sd.DialogSettings.CancelButtonText;
            sd.ExtraButtonText = sd.DialogSettings.ExtraButtonText;
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDialog" /> class.
        /// </summary>
        /// <param name="owningWindow">The owning window.</param>
        /// <param name="settings">The settings.</param>
        public AppDialog(MaterialDesignWindow owningWindow, DialogSettings settings)
        {
            DialogSettings = settings ?? owningWindow.DialogOptions;
            OwningWindow = owningWindow;
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDialog" /> class.
        /// </summary>
        /// <param name="owningWindow">The owning window.</param>
        /// <param name="style">The style.</param>
        public AppDialog(MaterialDesignWindow owningWindow, DialogStyle style)
        {
            DialogSettings = new DialogSettings { DialogStyle = style };
            OwningWindow = owningWindow;
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDialog" /> class.
        /// </summary>
        public AppDialog()
        {
            DialogSettings = new DialogSettings();
            Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/LandmarkDevs.UI.Material;component/Themes/AppDialog.xaml") });
            if (DialogSettings?.CustomResourceDictionary != null)
            {
                Resources.MergedDictionaries.Add(DialogSettings.CustomResourceDictionary);
            }
            Loaded += (sender, args) =>
            {
                OnLoaded();
            };
            Unloaded += AppDialog_Unloaded;
        }

        /// <summary>
        /// Handles the Unloaded event of the AppDialog control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AppDialog_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= AppDialog_Unloaded;
        }

        /// <summary>
        /// Called when [loaded].
        /// </summary>
        protected virtual void OnLoaded()
        {
            // nothing here
            SetButtonState(this);
        }

        /// <summary>
        /// Waits for load asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        public Task WaitForLoadAsync()
        {
            Dispatcher.VerifyAccess();
            if (IsLoaded) return new Task(() => { });
            var tcs = new TaskCompletionSource<object>();
            RoutedEventHandler handler = null;
            handler = (sender, args) =>
            {
                Loaded -= handler;
                Focus();
                tcs.TrySetResult(null);
            };
            Loaded += handler;
            return tcs.Task;
        }


        /// <summary>
        /// Called when [shown].
        /// </summary>
        protected internal virtual void OnShown() { }
        /// <summary>
        /// Raises the Close event.
        /// </summary>
        protected internal virtual void OnClose()
        {
            ParentDialogWindow?.Close();
        }

        /// <summary>
        /// Called when [request close].
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected internal virtual bool OnRequestClose()
        {
            return true;
        }

        /// <summary>
        /// Gets the parent dialog window.
        /// </summary>
        /// <value>The parent dialog window.</value>
        protected internal Window ParentDialogWindow { get; internal set; }
        /// <summary>
        /// Gets the owning window.
        /// </summary>
        /// <value>The owning window.</value>
        protected internal Window OwningWindow { get; internal set; }

        /// <summary>
        /// Waits the until unloaded asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        public Task WaitUntilUnloadedAsync()
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            Unloaded += (s, e) =>
            {
                tcs.TrySetResult(null);
            };
            return tcs.Task;
        }


        /// <summary>
        /// Waits for close asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exception cref="System.InvalidOperationException">Unable to find the dialog closing storyboard. Did you forget to add BaseMetroDialog.xaml to your merged dictionaries?</exception>
        public Task WaitForCloseAsync()
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            Storyboard closingStoryboard = this.Resources["DialogCloseStoryboard"] as Storyboard;
            if (closingStoryboard == null)
                throw new InvalidOperationException("Unable to find the dialog closing storyboard. Did you forget to add BaseMetroDialog.xaml to your merged dictionaries?");
            EventHandler handler = null;
            handler = (sender, args) =>
            {
                closingStoryboard.Completed -= handler;
                tcs.TrySetResult(null);
            };
            closingStoryboard = closingStoryboard.Clone();
            closingStoryboard.Completed += handler;
            closingStoryboard.Begin(this);
            return tcs.Task;
        }

        /// <summary>
        /// Waits for button press asynchronous.
        /// </summary>
        /// <returns>Task&lt;DialogResult&gt;.</returns>
        internal Task<DialogResult> WaitForButtonPressAsync()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Focus();
                switch (ButtonStyle)
                {
                    default:
                        OkButton.Focus();
                        break;
                    case DialogStyle.OkNo:
                    case DialogStyle.OkNoCancel:
                    case DialogStyle.OkNoCancelExtra:
                        NoButton.Focus();
                        break;
                }
            }));
            TaskCompletionSource<DialogResult> tcs = new TaskCompletionSource<DialogResult>();
            Action cleanUpHandlers = null;
            RoutedEventHandler okHandler = null;
            RoutedEventHandler cancelHandler = null;
            RoutedEventHandler extraHandler = null;
            RoutedEventHandler noHandler = null;
            KeyEventHandler okKeyHandler = null;
            KeyEventHandler cancelKeyHandler = null;
            KeyEventHandler extraKeyHandler = null;
            KeyEventHandler noKeyHandler = null;

            var cancellationTokenRegistration = DialogSettings.CancellationToken.Register(() =>
            {
                cleanUpHandlers?.Invoke();
                tcs.TrySetResult(ButtonStyle == DialogStyle.Ok ? DialogResult.Ok : DialogResult.No);
            });

            cleanUpHandlers = () =>
            {
                OkButton.Click -= okHandler;
                OkButton.KeyDown -= okKeyHandler;
                CancelButton.Click -= cancelHandler;
                CancelButton.KeyDown -= cancelKeyHandler;
                ExtraButton.Click -= extraHandler;
                ExtraButton.KeyDown -= extraKeyHandler;
                NoButton.Click -= noHandler;
                NoButton.KeyDown -= noKeyHandler;
                cancellationTokenRegistration.Dispose();
            };

            okKeyHandler = (sender, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    cleanUpHandlers();
                    tcs.TrySetResult(DialogResult.Ok);
                }
            };

            cancelKeyHandler = (sender, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    cleanUpHandlers();
                    tcs.TrySetResult(DialogResult.Cancel);
                }
            };

            extraKeyHandler = (sender, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    cleanUpHandlers();
                    tcs.TrySetResult(DialogResult.Extra);
                }
            };

            noKeyHandler = (sender, e) => {
                if (e.Key == Key.Enter)
                {
                    cleanUpHandlers();
                    tcs.TrySetResult(DialogResult.No);
                }
            };

            okHandler = (sender, e) =>
            {
                cleanUpHandlers();
                tcs.TrySetResult(DialogResult.Ok);
                e.Handled = true;
            };

            cancelHandler = (sender, e) =>
            {
                cleanUpHandlers();
                tcs.TrySetResult(DialogResult.Cancel);
                e.Handled = true;
            };

            extraHandler = (sender, e) =>
            {
                cleanUpHandlers();
                tcs.TrySetResult(DialogResult.Extra);
                e.Handled = true;
            };

            noHandler = (sender, e) =>
            {
                cleanUpHandlers();
                tcs.TrySetResult(DialogResult.No);
                e.Handled = true;
            };

            OkButton.KeyDown += okKeyHandler;
            CancelButton.KeyDown += cancelKeyHandler;
            ExtraButton.KeyDown += extraKeyHandler;
            NoButton.KeyDown += noKeyHandler;
            OkButton.Click += okHandler;
            CancelButton.Click += cancelHandler;
            ExtraButton.Click += extraHandler;
            NoButton.Click += noHandler;
            return tcs.Task;
        }
    }
}
