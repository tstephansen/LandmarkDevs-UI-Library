#region
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LandmarkDevs.UI.Material.Controls.Panels;
using LandmarkDevs.UI.Material.Models;
#endregion

namespace LandmarkDevs.UI.Material.Controls
{
    /// <summary>
    ///     Class IdentityCard.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ContentControl" />
    [TemplatePart(Name = PART_CardLayoutRoot, Type = typeof(Grid))]
    [TemplatePart(Name = PART_EditContentControl, Type = typeof(ContentControl))]
    [TemplatePart(Name = PART_EditGrid, Type = typeof(Grid))]
    [TemplatePart(Name = PART_EditCard, Type = typeof(Card))]
    [TemplatePart(Name = PART_ViewContentControl, Type = typeof(ContentControl))]
    [TemplatePart(Name = PART_ViewGrid, Type = typeof(Grid))]
    [TemplatePart(Name = PART_ViewCard, Type = typeof(Card))]
    [TemplatePart(Name = PART_SaveButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_CancelButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_EditButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_OkButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_FirstNameTextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = PART_LastNameTextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = PART_UsernameTextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = PART_UserThumbnail, Type = typeof(Image))]
    [TemplatePart(Name = PART_FirstNameTextBlock, Type = typeof(TextBlock))]
    [TemplatePart(Name = PART_LastNameTextBlock, Type = typeof(TextBlock))]
    [TemplatePart(Name = PART_UsernameTextBlock, Type = typeof(TextBlock))]
    [TemplatePart(Name = PART_FullNameTextBlock, Type = typeof(TextBlock))]
    [TemplatePart(Name = PART_RoleItems, Type = typeof(ItemsControl))]
    public class IdentityCard : Control
    {
        #region Dependency Properties
        /// <summary>
        ///     The first name property  
        /// </summary>
        public static readonly DependencyProperty FirstNameProperty = DependencyProperty.Register(
            "FirstName", typeof(string), typeof(IdentityCard),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnFirstNameChanged));

        /// <summary>
        ///     The last name property
        /// </summary>
        public static readonly DependencyProperty LastNameProperty = DependencyProperty.Register(
            "LastName", typeof(string), typeof(IdentityCard),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnLastNameChanged));

        /// <summary>
        ///     The user image property
        /// </summary>
        public static readonly DependencyProperty UserImageProperty = DependencyProperty.Register(
            "UserImage", typeof(ImageSource), typeof(IdentityCard), new PropertyMetadata(default(ImageSource)));

        /// <summary>
        ///     The username property
        /// </summary>
        public static readonly DependencyProperty UsernameProperty = DependencyProperty.Register(
            "Username", typeof(string), typeof(IdentityCard), new PropertyMetadata(default(string)));
        
        /// <summary>
        ///     The full name property
        /// </summary>
        public static readonly DependencyProperty FullNameProperty = DependencyProperty.Register(
            "FullName", typeof(string), typeof(IdentityCard),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnFullNameChanged));

        /// <summary>
        ///     Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName
        {
            get { return (string) GetValue(FirstNameProperty); }
            set { SetValue(FirstNameProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public string LastName
        {
            get { return (string) GetValue(LastNameProperty); }
            set { SetValue(LastNameProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the user image.
        /// </summary>
        /// <value>The user image.</value>
        public ImageSource UserImage
        {
            get { return (ImageSource) GetValue(UserImageProperty); }
            set { SetValue(UserImageProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        public string Username
        {
            get { return (string) GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName
        {
            get { return (string) GetValue(FullNameProperty); }
            set { SetValue(FullNameProperty, value); }
        }

        /// <summary>
        ///     The orientation property
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation", typeof(IdentityCardOrientation), typeof(IdentityCard), new PropertyMetadata(IdentityCardOrientation.Vertical));

        /// <summary>
        ///     Gets or sets the orientation.
        /// </summary>
        /// <value>The orientation.</value>
        public IdentityCardOrientation Orientation
        {
            get { return (IdentityCardOrientation) GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// The roles property
        /// </summary>
        public static readonly DependencyProperty RolesProperty = DependencyProperty.Register("Roles", typeof(IList), typeof(IdentityCard));

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>The roles.</value>
        public IList Roles
        {
            get { return (IList) GetValue(RolesProperty); }
            set { SetValue(RolesProperty, value); }
        }
        #endregion

        #region PropertyChanges                
        /// <summary>
        ///     Handles the <see cref="E:FirstNameChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnFirstNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;
            var host = (IdentityCard) sender;
            host.FirstName = (string) e.NewValue;
            host.FullName = $"{host.FirstName} {host.LastName}";
        }

        /// <summary>
        ///     Handles the <see cref="E:LastNameChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnLastNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;
            var host = (IdentityCard) sender;
            host.LastName = (string) e.NewValue;
            host.FullName = $"{host.FirstName} {host.LastName}";
        }

        /// <summary>
        ///     Handles the <see cref="E:FullNameChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnFullNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;
            var host = (IdentityCard) sender;
            host.FullName = (string) e.NewValue;
        }
        #endregion

        #region Class Initialization        
        /// <summary>
        ///     Initializes static members of the <see cref="IdentityCard" /> class.
        /// </summary>
        static IdentityCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IdentityCard), new FrameworkPropertyMetadata(typeof(IdentityCard)));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="IdentityCard" /> class.
        /// </summary>
        public IdentityCard()
        {
            CommandBindings.Add(new CommandBinding(ClickOkCommand, ClickOk, CanClickOk));
            CommandBindings.Add(new CommandBinding(EditUserCommand, EditUser, CanEditUser));
        }

        /// <summary>
        ///     When overridden in a derived class, is invoked whenever application code or internal processes call
        ///     <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            CardLayoutRoot = GetTemplateChild(PART_CardLayoutRoot) as Grid;
            EditContentControl = GetTemplateChild(PART_EditContentControl) as ContentControl;
            EditCard = GetTemplateChild(PART_EditCard) as Card;
            EditGrid = GetTemplateChild(PART_EditGrid) as Grid;
            ViewContentControl = GetTemplateChild(PART_ViewContentControl) as ContentControl;
            ViewCard = GetTemplateChild(PART_ViewCard) as Card;
            ViewGrid = GetTemplateChild(PART_ViewGrid) as Grid;
            SaveButton = GetTemplateChild(PART_SaveButton) as Button;
            CancelButton = GetTemplateChild(PART_CancelButton) as Button;
            EditButton = GetTemplateChild(PART_EditButton) as Button;
            OkButton = GetTemplateChild(PART_OkButton) as Button;
            FirstNameTextBox = GetTemplateChild(PART_FirstNameTextBox) as TextBox;
            LastNameTextBox = GetTemplateChild(PART_LastNameTextBox) as TextBox;
            UsernameTextBox = GetTemplateChild(PART_UsernameTextBox) as TextBox;
            UserThumbnail = GetTemplateChild(PART_UserThumbnail) as Image;
            FirstNameTextBlock = GetTemplateChild(PART_FirstNameTextBlock) as TextBlock;
            LastNameTextBlock = GetTemplateChild(PART_LastNameTextBlock) as TextBlock;
            UsernameTextBlock = GetTemplateChild(PART_UsernameTextBlock) as TextBlock;
            FullNameTextBlock = GetTemplateChild(PART_FullNameTextBlock) as TextBlock;
            _roleItems = GetTemplateChild(PART_RoleItems) as ItemsControl;
            if (_roleItems != null)
                _roleItems.ItemsSource = Roles;
            if (EditButton != null)
                EditButton.Command = EditUserCommand;
            if (SaveButton != null)
                SaveButton.Command = ClickOkCommand;
            if (CancelButton != null)
                CancelButton.Command = ClickOkCommand;
            if (OkButton != null)
                OkButton.Command = ClickOkCommand;
            GotoVisualState(ViewCardState, false);
        }

        private void GotoVisualState(string stateName, bool useTransitions = true)
        {
            var thisFe = this as FrameworkElement;
            VisualStateManager.GoToState(thisFe, stateName, useTransitions);
        }
        #endregion

        #region Template Properties
        internal Grid CardLayoutRoot;
        internal Grid EditGrid;
        internal Grid ViewGrid;
        internal Card EditCard;
        internal Card ViewCard;
        internal ContentControl EditContentControl;
        internal ContentControl ViewContentControl;
        internal Button SaveButton;
        internal Button CancelButton;
        internal Button EditButton;
        internal Button OkButton;
        internal TextBox FirstNameTextBox;
        internal TextBox LastNameTextBox;
        internal TextBox UsernameTextBox;
        internal Image UserThumbnail;
        internal TextBlock FirstNameTextBlock;
        internal TextBlock LastNameTextBlock;
        internal TextBlock UsernameTextBlock;
        internal TextBlock FullNameTextBlock;
        private ItemsControl _roleItems;
        // ReSharper disable InconsistentNaming
        private const string PART_CardLayoutRoot = "PART_CardLayoutRoot";
        private const string PART_EditContentControl = "PART_EditContentControl";
        private const string PART_EditCard = "PART_EditCard";
        private const string PART_EditGrid = "PART_EditGrid";
        private const string PART_CancelButton = "PART_CancelButton";
        private const string PART_SaveButton = "PART_SaveButton";
        private const string PART_EditButton = "PART_EditButton";
        private const string PART_OkButton = "PART_OkButton";
        private const string PART_ViewContentControl = "PART_ViewContentControl";
        private const string PART_ViewCard = "PART_ViewCard";
        private const string PART_ViewGrid = "PART_ViewGrid";
        private const string PART_FirstNameTextBox = "PART_FirstNameTextBox";
        private const string PART_LastNameTextBox = "PART_LastNameTextBox";
        private const string PART_UsernameTextBox = "PART_UsernameTextBox";
        private const string PART_UserThumbnail = "PART_UserThumbnail";
        private const string PART_FirstNameTextBlock = "PART_FirstNameTextBlock";
        private const string PART_LastNameTextBlock = "PART_LastNameTextBlock";
        private const string PART_UsernameTextBlock = "PART_UsernameTextBlock";
        private const string PART_FullNameTextBlock = "PART_FullNameTextBlock";
        private const string ViewCardState = "ViewCardState";
        private const string EditCardState = "EditCardState";
        private const string PART_RoleItems = "PART_RoleItems";
        // ReSharper restore InconsistentNaming
        #endregion

        #region Commands        
        /// <summary>
        ///     Gets the click ok command.
        /// </summary>
        /// <value>The click ok command.</value>
        public static RoutedCommand ClickOkCommand { get; } = new RoutedCommand();

        /// <summary>
        ///     Clicks the ok.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs" /> instance containing the event data.</param>
        private static void ClickOk(object sender, ExecutedRoutedEventArgs e)
        {
            var host = (IdentityCard) sender;
            host.GotoVisualState(ViewCardState);
        }

        /// <summary>
        ///     Determines whether this instance [can click ok] the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CanExecuteRoutedEventArgs" /> instance containing the event data.</param>
        private static void CanClickOk(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        ///     Gets the edit user command.
        /// </summary>
        /// <value>The edit user command.</value>
        public static RoutedCommand EditUserCommand { get; } = new RoutedCommand();

        /// <summary>
        ///     Edits the user.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs" /> instance containing the event data.</param>
        private static void EditUser(object sender, ExecutedRoutedEventArgs e)
        {
            var host = (IdentityCard) sender;
            host.GotoVisualState(EditCardState);
        }

        /// <summary>
        ///     Determines whether this instance [can edit user] the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CanExecuteRoutedEventArgs" /> instance containing the event data.</param>
        private static void CanEditUser(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion
    }

    /// <summary>
    ///     Enum IdentityCardOrientation
    /// </summary>
    public enum IdentityCardOrientation
    {
        /// <summary>
        ///     The horizontal layout
        /// </summary>
        Horizontal,

        /// <summary>
        ///     The vertical layout
        /// </summary>
        Vertical
    }
}