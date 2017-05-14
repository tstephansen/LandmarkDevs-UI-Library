#region
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using LandmarkDevs.UI.Material.Controls.Windows;
#endregion

namespace LandmarkDevs.UI.Material.Controls.Dialogs
{
    /// <summary>
    ///     Class DialogBase.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ContentControl" />
    public class DialogBase : ContentControl
    {
        static DialogBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogBase),
                new FrameworkPropertyMetadata(typeof(DialogBase)));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DialogBase" /> class.
        /// </summary>
        /// <param name="owningWindow">The owning window.</param>
        /// <param name="settings">The settings.</param>
        public DialogBase(MaterialDesignWindow owningWindow, DialogSettings settings)
        {
            DialogSettings = settings ?? owningWindow.DialogOptions;
            OwningWindow = owningWindow;
            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DialogBase" /> class.
        /// </summary>
        /// <param name="owningWindow">The owning window.</param>
        /// <param name="style">The style.</param>
        protected DialogBase(MaterialDesignWindow owningWindow, DialogStyle style)
        {
            DialogSettings = new DialogSettings {DialogStyle = style};
            OwningWindow = owningWindow;
            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DialogBase" /> class.
        /// </summary>
        protected DialogBase()
        {
            DialogSettings = new DialogSettings();
            Initialize();
        }

        /// <summary>
        ///     Gets or sets the size changed handler.
        /// </summary>
        /// <value>The size changed handler.</value>
        internal SizeChangedEventHandler SizeChangedHandler { get; set; }

        /// <summary>
        ///     Gets the dialog settings.
        /// </summary>
        /// <value>The dialog settings.</value>
        public DialogSettings DialogSettings { get; private set; }

        /// <summary>
        ///     Gets the parent dialog window.
        /// </summary>
        /// <value>The parent dialog window.</value>
        protected internal Window ParentDialogWindow { get; internal set; }

        /// <summary>
        ///     Gets the owning window.
        /// </summary>
        /// <value>The owning window.</value>
        protected internal Window OwningWindow { get; internal set; }

        private void Initialize()
        {
            Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/LandmarkDevs.UI.Material;component/Styles/ApplicationStyles.xaml")
            });
            Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/LandmarkDevs.UI.Material;component/Themes/Dialogs/DialogBase.xaml")
            });
            if (DialogSettings?.CustomResourceDictionary != null)
                Resources.MergedDictionaries.Add(DialogSettings.CustomResourceDictionary);
            Unloaded += DialogBase_Unloaded;
        }

        private void DialogBase_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= DialogBase_Unloaded;
        }

        /// <summary>
        ///     Called when [loaded].
        /// </summary>
        protected virtual void OnLoaded()
        {
            // nothing here
        }

        /// <summary>
        ///     Waits for load asynchronously.
        /// </summary>
        /// <returns>Task.</returns>
        public Task WaitForLoadAsync()
        {
            Dispatcher.VerifyAccess();
            if (IsLoaded)
                return new Task(() => { });
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
        ///     Called when [shown].
        /// </summary>
        protected internal virtual void OnShown()
        {
        }

        /// <summary>
        ///     Raises the Close event.
        /// </summary>
        protected internal virtual void OnClose()
        {
            ParentDialogWindow?.Close();
        }

        /// <summary>
        ///     Called when [request close].
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected internal virtual bool OnRequestClose()
        {
            return true;
        }

        /// <summary>
        ///     Waits the until unloaded asynchronously.
        /// </summary>
        /// <returns>Task.</returns>
        public Task WaitUntilUnloadedAsync()
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            Unloaded += (s, e) => { tcs.TrySetResult(null); };
            return tcs.Task;
        }

        /// <summary>
        ///     Waits for close asynchronously.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exception cref="System.InvalidOperationException">
        ///     Unable to find the dialog closing storyboard. Did you forget to add
        ///     BaseMetroDialog.xaml to your merged dictionaries?
        /// </exception>
        public Task WaitForCloseAsync()
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            Storyboard closingStoryboard = Resources["DialogCloseStoryboard"] as Storyboard;
            if (closingStoryboard == null)
                throw new InvalidOperationException(
                    "Unable to find the dialog closing storyboard. Did you forget to add BaseMetroDialog.xaml to your merged dictionaries?");
            EventHandler handler = null;
            handler = (sender, args) =>
            {
                // ReSharper disable once AccessToModifiedClosure
                closingStoryboard.Completed -= handler;
                tcs.TrySetResult(null);
            };
            closingStoryboard = closingStoryboard.Clone();
            closingStoryboard.Completed += handler;
            closingStoryboard.Begin(this);
            return tcs.Task;
        }

        #region Dependency Properties
        /// <summary>
        ///     The title property
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(DialogBase), new PropertyMetadata(default(string)));

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return (string) GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        ///     The dialog top property
        /// </summary>
        public static readonly DependencyProperty DialogTopProperty = DependencyProperty.Register(
            "DialogTop", typeof(object), typeof(DialogBase), new PropertyMetadata(null));

        /// <summary>
        ///     Gets or sets the dialog top.
        /// </summary>
        /// <value>The dialog top.</value>
        public object DialogTop
        {
            get { return GetValue(DialogTopProperty); }
            set { SetValue(DialogTopProperty, value); }
        }

        /// <summary>
        ///     The dialog bottom property
        /// </summary>
        public static readonly DependencyProperty DialogBottomProperty = DependencyProperty.Register(
            "DialogBottom", typeof(object), typeof(DialogBase), new PropertyMetadata(null));

        /// <summary>
        ///     Gets or sets the dialog bottom.
        /// </summary>
        /// <value>The dialog bottom.</value>
        public object DialogBottom
        {
            get { return GetValue(DialogBottomProperty); }
            set { SetValue(DialogBottomProperty, value); }
        }
        #endregion
    }

    /// <summary>
    ///     Enum DialogResult
    /// </summary>
    public enum DialogResult
    {
        /// <summary>
        ///     No
        /// </summary>
        No = 0,

        /// <summary>
        ///     Ok
        /// </summary>
        Ok = 1,

        /// <summary>
        ///     Cancel
        /// </summary>
        Cancel,

        /// <summary>
        ///     Extra
        /// </summary>
        Extra
    }

    /// <summary>
    ///     An enum representing the different button states for a Message Dialog.
    /// </summary>
    public enum DialogStyle
    {
        /// <summary>
        ///     Just "OK"
        /// </summary>
        Ok = 0,

        /// <summary>
        ///     "OK" and "Cancel"
        /// </summary>
        OkNo = 1,

        /// <summary>
        ///     Ok, No, and Cancel
        /// </summary>
        OkNoCancel = 2,

        /// <summary>
        ///     Ok, No, Cancel, and Extra
        /// </summary>
        OkNoCancelExtra = 3
    }
}