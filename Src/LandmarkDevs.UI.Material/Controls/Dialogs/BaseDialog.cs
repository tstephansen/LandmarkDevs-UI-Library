using System;
using System.Windows;
using System.Windows.Controls;
using LandmarkDevs.UI.Models.Dialogs;

// ReSharper disable InheritdocConsiderUsage

namespace LandmarkDevs.UI.Material.Controls.Dialogs
{
    /// <summary>
    ///     Class BaseDialog.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ContentControl" />
    public class BaseDialog : ContentControl
    {
        static BaseDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseDialog),
                new FrameworkPropertyMetadata(typeof(BaseDialog)));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseDialog" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="settings">The settings.</param>
        public BaseDialog(DialogHost host, DialogSettings settings)
        {
            DialogSettings = settings ?? host.DialogOptions;
            OwningControl = host;
            InitializeBaseDialog();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseDialog" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="style">The style.</param>
        protected BaseDialog(DialogHost host, DialogStyle style)
        {
            DialogSettings = new DialogSettings { DialogStyle = style };
            OwningControl = host;
            InitializeBaseDialog();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseDialog" /> class.
        /// </summary>
        protected BaseDialog()
        {
            DialogSettings = new DialogSettings();
            InitializeBaseDialog();
        }

        /// <summary>
        ///     Gets the dialog settings.
        /// </summary>
        /// <value>The dialog settings.</value>
        public DialogSettings DialogSettings { get; set; }

        /// <summary>
        ///     Gets the parent dialog window.
        /// </summary>
        /// <value>The parent dialog window.</value>
        protected internal Window ParentDialogWindow { get; internal set; }

        /// <summary>
        ///     Gets the owning window.
        /// </summary>
        /// <value>The owning window.</value>
        protected internal ContentControl OwningControl { get; internal set; }

        private void InitializeBaseDialog()
        {
            Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/LandmarkDevs.UI.Material;component/Styles/ApplicationStyles.xaml")
            });
            Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/LandmarkDevs.UI.Material;component/Themes/BaseDialog.xaml")
            });
            Unloaded += BaseDialog_Unloaded;
        }

        private void BaseDialog_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= BaseDialog_Unloaded;
        }

        /// <summary>
        ///     Called when [shown].
        /// </summary>
        protected internal virtual void OnShown()
        {
        }

        internal SizeChangedEventHandler SizeChangedHandler { get; set; }

        #region Dependency Properties
        /// <summary>
        ///     The title property
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(BaseDialog), new PropertyMetadata(default(string)));

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        /// <summary>
        ///     The dialog top property
        /// </summary>
        public static readonly DependencyProperty DialogTopProperty = DependencyProperty.Register(
            "DialogTop", typeof(object), typeof(BaseDialog), new PropertyMetadata(null));

        /// <summary>
        ///     Gets or sets the dialog top.
        /// </summary>
        /// <value>The dialog top.</value>
        public object DialogTop
        {
            get => GetValue(DialogTopProperty);
            set => SetValue(DialogTopProperty, value);
        }

        /// <summary>
        ///     The dialog bottom property
        /// </summary>
        public static readonly DependencyProperty DialogBottomProperty = DependencyProperty.Register(
            "DialogBottom", typeof(object), typeof(BaseDialog), new PropertyMetadata(null));

        /// <summary>
        ///     Gets or sets the dialog bottom.
        /// </summary>
        /// <value>The dialog bottom.</value>
        public object DialogBottom
        {
            get => GetValue(DialogBottomProperty);
            set => SetValue(DialogBottomProperty, value);
        }

        public static readonly DependencyProperty DialogMessageFontSizeProperty = DependencyProperty.Register("DialogMessageFontSize", typeof(double), typeof(BaseDialog), new PropertyMetadata(26D));
        
        public double DialogMessageFontSize
        {
            get => (double)GetValue(DialogMessageFontSizeProperty);
            set => SetValue(DialogMessageFontSizeProperty, value);
        }

        public static readonly DependencyProperty DialogTitleFontSizeProperty = DependencyProperty.Register("DialogTitleFontSize", typeof(double), typeof(BaseDialog), new PropertyMetadata(15D));

        public double DialogTitleFontSize
        {
            get => (double)GetValue(DialogTitleFontSizeProperty);
            set => SetValue(DialogTitleFontSizeProperty, value);
        }
        #endregion
    }
}
