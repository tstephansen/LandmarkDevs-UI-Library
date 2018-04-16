using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
// ReSharper disable InconsistentNaming

namespace LandmarkDevs.UI.Material.Dialogs
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

        #region Dialogs
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
        #endregion

        #region Window Shade
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
        #endregion
    }
}
