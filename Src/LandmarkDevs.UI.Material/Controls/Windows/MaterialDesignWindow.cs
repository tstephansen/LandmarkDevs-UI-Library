#region
using LandmarkDevs.UI.Material.Controls.Dialogs;
using LandmarkDevs.UI.Material.Controls.Panels;
using LandmarkDevs.UI.Material.Helpers;
using LandmarkDevs.UI.Material.Models;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Path = System.IO.Path;
using WinInterop = System.Windows.Interop;
#endregion

// ReSharper disable InconsistentNaming
namespace LandmarkDevs.UI.Material.Controls.Windows
{
    /// <summary>
    ///     Class MaterialDesignWindow.
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    [TemplatePart(Name = PART_WindowBorder, Type = typeof(Border))]
    [TemplatePart(Name = PART_LayoutRoot, Type = typeof(Grid))]
    [TemplatePart(Name = PART_WindowTitleGrid, Type = typeof(Grid))]
    [TemplatePart(Name = PART_NavigationDrawerToggleButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_WindowShadeContentControl, Type = typeof(ContentControl))]
    [TemplatePart(Name = PART_WindowShade, Type = typeof(Rectangle))]
    [TemplatePart(Name = PART_MainContentPresenter, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PART_StatusBarGrid, Type = typeof(Grid))]
    [TemplatePart(Name = PART_StatusBar, Type = typeof(MaterialStatusBar))]
    [TemplatePart(Name = PART_NavigationDrawer, Type = typeof(NavigationDrawer))]
    [TemplatePart(Name = PART_TitleRectangle, Type = typeof(Rectangle))]
    [TemplatePart(Name = PART_DialogsLayoutRoot, Type = typeof(Grid))]
    [TemplatePart(Name = PART_InactiveDialogContainer, Type = typeof(Grid))]
    [TemplatePart(Name = PART_ActiveDialogContainer, Type = typeof(Grid))]
    [TemplatePart(Name = PART_CloseThemeButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_ColorSelectorContentControl, Type = typeof(ContentControl))]
    [TemplatePart(Name = PART_PalletButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_MaterialColorWheel, Type = typeof(MaterialColorWheel))]
    [TemplatePart(Name = PART_CancelThemeChangeButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_SettingsPanelContentControl, Type = typeof(ContentControl))]
    [TemplatePart(Name = PART_SettingsPanel, Type = typeof(MaterialSettingsPanel))]
    //[TemplatePart(Name = PART_CloseSettingsPanelButton, Type = typeof(Button))]
    public class MaterialDesignWindow : Window
    {
        /// <summary>
        ///     Initializes static members of the <see cref="MaterialDesignWindow" /> class.
        /// </summary>
        static MaterialDesignWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialDesignWindow),
                                                     new FrameworkPropertyMetadata(typeof(MaterialDesignWindow)));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MaterialDesignWindow" /> class.
        /// </summary>
        public MaterialDesignWindow()
        {
            Loaded += MaterialDesignWindow_Loaded;
            SourceInitialized += MaterialDesignWindow_SourceInitialized;
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, OnCloseWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, OnMaximizeWindow,
                                                   OnCanResizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, OnMinimizeWindow,
                                                   OnCanMinimizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, OnRestoreWindow,
                                                   OnCanResizeWindow));
        }

        /// <summary>
        ///     When overridden in a derived class, is invoked whenever application code or internal processes call
        ///     <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _windowBorder = GetTemplateChild(PART_WindowBorder) as Border;
            _layoutRoot = GetTemplateChild(PART_LayoutRoot) as Grid;
            _windowTitleGrid = GetTemplateChild(PART_WindowTitleGrid) as Grid;
            _windowShadeContentControl = GetTemplateChild(PART_WindowShadeContentControl) as ContentControl;
            WindowShade = GetTemplateChild(PART_WindowShade) as Rectangle;
            _mainContentPresenter = GetTemplateChild(PART_MainContentPresenter) as ContentPresenter;
            _navigationDrawerToggleButton = GetTemplateChild(PART_NavigationDrawerToggleButton) as Button;
            NavigationDrawer = GetTemplateChild(PART_NavigationDrawer) as NavigationDrawer;
            _statusBarGrid = GetTemplateChild(PART_StatusBarGrid) as Grid;
            _statusBar = GetTemplateChild(PART_StatusBar) as MaterialStatusBar;
            _titleRectangle = GetTemplateChild(PART_TitleRectangle) as Rectangle;
            _dialogsLayoutRoot = GetTemplateChild(PART_DialogsLayoutRoot) as Grid;
            InactiveDialogContainer = GetTemplateChild(PART_InactiveDialogContainer) as Grid;
            ActiveDialogContainer = GetTemplateChild(PART_ActiveDialogContainer) as Grid;
            _closeSchemeButton = GetTemplateChild(PART_CloseThemeButton) as Button;
            _colorSelectorContentControl = GetTemplateChild(PART_ColorSelectorContentControl) as ContentControl;
            _palletButton = GetTemplateChild(PART_PalletButton) as Button;
            _cancelChangeThemeButton = GetTemplateChild(PART_CancelThemeChangeButton) as Button;
            _materialColorWheel = GetTemplateChild(PART_MaterialColorWheel) as MaterialColorWheel;
            _settingsPanelContentControl = GetTemplateChild(PART_SettingsPanelContentControl) as ContentControl;
            _settingsPanel = GetTemplateChild(PART_SettingsPanel) as MaterialSettingsPanel;
            SetThemeSettings();
            SetButtonSettings();
        }

        private void SetThemeSettings()
        {
            _currentPrimary = "Blue";
            _currentAccent = "Yellow";
            _isDark = false;
            var themeSettings = LoadThemeSettings();
            if (themeSettings != null)
            {
                if (themeSettings.PrimaryName != null)
                    new PaletteHelper().ReplacePrimaryColor(themeSettings.PrimaryName);
                if (themeSettings.AccentName != null)
                    new PaletteHelper().ReplaceAccentColor(themeSettings.AccentName);
                _isDark = themeSettings.IsDark;
                new PaletteHelper().SetLightDark(themeSettings.IsDark);
                _currentPrimary = themeSettings.PrimaryName;
                _currentAccent = themeSettings.AccentName;
                if (_isDark)
                {
                    _materialColorWheel.CenterButtonText = "Dark";
                    MaterialColorWheel.ReplaceEntry("MaterialDesignUserControlBackground", new SolidColorBrush(Color.FromArgb(255, 55, 71, 79)));
                }
            }
        }

        private void SetButtonSettings()
        {
            if (_palletButton != null)
                _palletButton.Click += (s, e) =>
                {
                    if (ThemeSelectionVisibility == Visibility.Collapsed)
                    {
                        ThemeSelectionVisibility = Visibility.Visible;
                        Panel.SetZIndex(_colorSelectorContentControl, 2);
                    }
                    else
                    {
                        new PaletteHelper().ReplacePrimaryColor(_currentPrimary);
                        new PaletteHelper().ReplaceAccentColor(_currentAccent);
                        ThemeSelectionVisibility = Visibility.Collapsed;
                        Panel.SetZIndex(_colorSelectorContentControl, -1);
                    }
                };
            if (_navigationDrawerToggleButton != null)
                _navigationDrawerToggleButton.Click += NavigationDrawerToggleButton_MouseUp;
            if (_windowShadeContentControl != null)
                _windowShadeContentControl.MouseUp += WindowShadeContentControl_MouseUp;
            if (_closeSchemeButton != null)
                _closeSchemeButton.Click += (s, e) =>
                {
                    ThemeSelectionVisibility = Visibility.Collapsed;
                    Panel.SetZIndex(_colorSelectorContentControl, -1);
                    SaveThemeSettings();
                };
            if (_cancelChangeThemeButton != null)
                _cancelChangeThemeButton.Click += (s, e) =>
                {
                    new PaletteHelper().ReplacePrimaryColor(_currentPrimary);
                    new PaletteHelper().ReplaceAccentColor(_currentAccent);
                    new PaletteHelper().SetLightDark(_isDark);
                    ThemeSelectionVisibility = Visibility.Collapsed;
                    Panel.SetZIndex(_colorSelectorContentControl, -1);
                };
        }

        #region Internal Settings
        /// <summary>
        ///     Saves the theme settings.
        /// </summary>
        public void SaveThemeSettings()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (directoryName == null)
                return;
            var fileName = Path.Combine(directoryName, "Theme.config");
            if (File.Exists(fileName))
                File.Delete(fileName);
            var themeSettings = new ThemeSettingsModel
            {
                PrimaryName = _materialColorWheel.PrimaryName,
                AccentName = _materialColorWheel.AccentName,
                IsDark = _materialColorWheel.IsDark
            };
            JsonActions.SaveToJson(fileName, themeSettings);
        }

        /// <summary>
        ///     Loads the theme settings.
        /// </summary>
        /// <returns>ThemeSettingsModel.</returns>
        public static ThemeSettingsModel LoadThemeSettings()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (directoryName == null)
                return null;
            var fileName = Path.Combine(directoryName, "Theme.config");
            if (!File.Exists(fileName))
                return null;
            return JsonActions.ReadJson<ThemeSettingsModel>(fileName);
        }

        #endregion

        #region Template Properties
        // ReSharper disable InconsistentNaming
        private const string PART_WindowBorder = "PART_WindowBorder";

        private const string PART_LayoutRoot = "PART_LayoutRoot";
        private const string PART_WindowTitleGrid = "PART_WindowTitleGrid";
        private const string PART_WindowShadeContentControl = "PART_WindowShadeContentControl";
        private const string PART_WindowShade = "PART_WindowShade";
        private const string PART_MainContentPresenter = "PART_MainContentPresenter";
        private const string PART_NavigationDrawerToggleButton = "PART_NavigationDrawerToggleButton";
        private const string PART_NavigationDrawer = "PART_NavigationDrawer";
        private const string PART_StatusBarGrid = "PART_StatusBarGrid";
        private const string PART_StatusBar = "PART_StatusBar";
        private const string PART_TitleRectangle = "PART_TitleRectangle";
        private const string PART_DialogsLayoutRoot = "PART_DialogsLayoutRoot";
        private const string PART_InactiveDialogContainer = "PART_InactiveDialogContainer";
        private const string PART_ActiveDialogContainer = "PART_ActiveDialogContainer";
        private const string PART_ColorSelectorContentControl = "PART_ColorSelectorContentControl";
        private const string PART_CloseThemeButton = "PART_CloseThemeButton";
        private const string PART_PalletButton = "PART_PalletButton";
        private const string PART_MaterialColorWheel = "PART_MaterialColorWheel";
        private const string PART_CancelThemeChangeButton = "PART_CancelThemeChangeButton";
        private const string PART_SettingsPanelContentControl = "PART_SettingsPanelContentControl";
        private const string PART_SettingsPanel = "PART_SettingsPanel";
        // ReSharper restore InconsistentNaming
        private Border _windowBorder;

        private Grid _layoutRoot;
        private Grid _windowTitleGrid;
        private ContentControl _windowShadeContentControl;
        private Rectangle WindowShade;
        private ContentPresenter _mainContentPresenter;
        private Button _navigationDrawerToggleButton;
        internal NavigationDrawer NavigationDrawer;
        private Grid _statusBarGrid;
        private MaterialStatusBar _statusBar;
        private Rectangle _titleRectangle;
        private Grid _dialogsLayoutRoot;
        internal Grid InactiveDialogContainer;
        internal Grid ActiveDialogContainer;
        private Button _closeSchemeButton;
        private ContentControl _colorSelectorContentControl;
        private Button _palletButton;
        private Storyboard _windowShadeStoryboard;
        private MaterialColorWheel _materialColorWheel;
        private Button _cancelChangeThemeButton;
        private ContentControl _settingsPanelContentControl;
        private MaterialSettingsPanel _settingsPanel;
        private Storyboard _settingsPanelStoryboard;
        private string _currentPrimary;
        private string _currentAccent;
        private bool _isDark;
        #endregion

        #region Themes
        /// <summary>
        ///     The theme selection visibility property
        /// </summary>
        public static readonly DependencyProperty ThemeSelectionVisibilityProperty = DependencyProperty.Register(
            "ThemeSelectionVisibility", typeof(Visibility), typeof(MaterialDesignWindow),
            new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnThemeSelectionVisibilityChanged));

        /// <summary>
        ///     Gets or sets the theme selection visibility.
        /// </summary>
        /// <value>The theme selection visibility.</value>
        public Visibility ThemeSelectionVisibility
        {
            get { return (Visibility)GetValue(ThemeSelectionVisibilityProperty); }
            set { SetValue(ThemeSelectionVisibilityProperty, value); }
        }

        private static void OnThemeSelectionVisibilityChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue)
                return;
            var host = sender as MaterialDesignWindow;
            if (host == null)
                throw new InvalidOperationException();
            var vis = (Visibility)e.NewValue;
            host.ThemeSelectionVisibility = vis;
            if (vis == Visibility.Visible)
            {
                if (host.NavigationDrawerVisible)
                    host.NavigationDrawerVisible = false;
                Panel.SetZIndex(host._colorSelectorContentControl, 2);
            }
            else
            {
                if (host.NavigationDrawerVisible)
                    host.NavigationDrawerVisible = false;
                Panel.SetZIndex(host._colorSelectorContentControl, -1);
            }
            host.NavigationDrawer.IsExpanded = host.NavigationDrawerVisible;
            if (host.NavigationDrawerVisible)
                host.ShowWindowShadeAsync();
            else
                host.HideWindowShadeAsync();
        }

        /// <summary>
        ///     The show pallet button property
        /// </summary>
        public static readonly DependencyProperty ShowPalletButtonProperty = DependencyProperty.Register(
            "ShowPalletButton", typeof(bool), typeof(MaterialDesignWindow),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnShowPalletButtonChanged));

        /// <summary>
        ///     Gets or sets a value indicating whether [show pallet button].
        /// </summary>
        /// <value><c>true</c> if [show pallet button]; otherwise, <c>false</c>.</value>
        public bool ShowPalletButton
        {
            get { return (bool)GetValue(ShowPalletButtonProperty); }
            set { SetValue(ShowPalletButtonProperty, value); }
        }

        /// <summary>
        ///     Handles the <see cref="E:ShowPalletButtonChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnShowPalletButtonChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue)
                return;
            var host = sender as MaterialDesignWindow;
            if (host == null)
                throw new InvalidOperationException();
            var show = (bool)e.NewValue;
            host.ShowPalletButton = show;
            host.PalletButtonVisibility = show ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        ///     The pallet button visibility property
        /// </summary>
        public static readonly DependencyProperty PalletButtonVisibilityProperty = DependencyProperty.Register(
            "PalletButtonVisibility", typeof(Visibility), typeof(MaterialDesignWindow),
            new FrameworkPropertyMetadata(Visibility.Visible, OnPalletButtonVisibilityChanged));

        /// <summary>
        ///     Gets or sets the pallet button visibility.
        /// </summary>
        /// <value>The pallet button visibility.</value>
        public Visibility PalletButtonVisibility
        {
            get { return (Visibility)GetValue(PalletButtonVisibilityProperty); }
            set { SetValue(PalletButtonVisibilityProperty, value); }
        }

        /// <summary>
        ///     Handles the <see cref="E:PalletButtonVisibilityChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        /// <exception cref="InvalidOperationException"></exception>
        private static void OnPalletButtonVisibilityChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue)
                return;
            var host = sender as MaterialDesignWindow;
            if (host == null)
                throw new InvalidOperationException();
            var vis = (Visibility)e.NewValue;
            host.PalletButtonVisibility = vis;
        }

        internal static readonly Dictionary<int, string> PrimaryColors = new Dictionary<int, string>
        {
            {0, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Amber.Primary.xaml"},
            {1, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Blue.Primary.xaml"},
            {2, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.BlueGrey.Primary.xaml"},
            {3, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Brown.Primary.xaml"},
            {4, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Cyan.Primary.xaml"},
            {5, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.DeepOrange.Primary.xaml"},
            {6, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.DeepPurple.Primary.xaml"},
            {7, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Green.Primary.xaml"},
            {8, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Grey.Primary.xaml"},
            {9, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Indigo.Primary.xaml"},
            {10, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.LightBlue.Primary.xaml"},
            {11, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.LightGreen.Primary.xaml"},
            {12, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Lime.Primary.xaml"},
            {13, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Orange.Primary.xaml"},
            {14, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Pink.Primary.xaml"},
            {15, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Purple.Primary.xaml"},
            {16, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Red.Primary.xaml"},
            {17, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Teal.Primary.xaml"},
            {18, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Yellow.Primary.xaml"}
        };

        internal static readonly Dictionary<int, string> AccentColors = new Dictionary<int, string>
        {
            {0, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Amber.Accent.xaml"},
            {1, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Blue.Accent.xaml"},
            {2, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Cyan.Accent.xaml"},
            {3, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.DeepOrange.Accent.xaml"},
            {4, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.DeepPurple.Accent.xaml"},
            {5, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Green.Accent.xaml"},
            {6, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Indigo.Accent.xaml"},
            {7, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.LightBlue.Accent.xaml"},
            {8, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.LightGreen.Accent.xaml"},
            {9, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Lime.Accent.xaml"},
            {10, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Orange.Accent.xaml"},
            {11, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Pink.Accent.xaml"},
            {12, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Purple.Accent.xaml"},
            {13, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Red.Accent.xaml"},
            {14, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Teal.Accent.xaml"},
            {15, "pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Yellow.Accent.xaml"}
        };

        internal static readonly Dictionary<int, string> RecommendedPrimaryColors = new Dictionary<int, string>
        {
            {0, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Amber.xaml"},
            {1, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Blue.xaml"},
            {2, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.BlueGrey.xaml"},
            {3, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Brown.xaml"},
            {4, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Cyan.xaml"},
            {5, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.DeepOrange.xaml"},
            {6, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.DeepPurple.xaml"},
            {7, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Green.xaml"},
            {8, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Grey.xaml"},
            {9, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Indigo.xaml"},
            {10, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.LightBlue.xaml"},
            {11, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.LightGreen.xaml"},
            {12, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Lime.xaml"},
            {13, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Orange.xaml"},
            {14, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Pink.xaml"},
            {15, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Purple.xaml"},
            {16, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Red.xaml"},
            {17, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Teal.xaml"},
            {18, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Yellow.xaml"}
        };

        internal static readonly Dictionary<int, string> RecommendedAccentColors = new Dictionary<int, string>
        {
            {0, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Amber.xaml"},
            {1, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Blue.xaml"},
            {2, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Cyan.xaml"},
            {3, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.DeepOrange.xaml"},
            {4, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.DeepPurple.xaml"},
            {5, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Green.xaml"},
            {6, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Indigo.xaml"},
            {7, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.LightBlue.xaml"},
            {8, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.LightGreen.xaml"},
            {9, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Lime.xaml"},
            {10, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Orange.xaml"},
            {11, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Pink.xaml"},
            {12, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Purple.xaml"},
            {13, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Red.xaml"},
            {14, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Teal.xaml"},
            {15, "pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Yellow.xaml"}
        };

        #endregion

        #region Dialogs
        /// <summary>
        ///     Shows the dialog container.
        /// </summary>
        public void ShowDialogContainer()
        {
            _dialogsLayoutRoot.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Hides the dialog container.
        /// </summary>
        public void HideDialogContainer()
        {
            _dialogsLayoutRoot.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        ///     The dialog content property
        /// </summary>
        public static readonly DependencyProperty DialogContentProperty = DependencyProperty.Register(
            "DialogContent", typeof(ContentControl), typeof(MaterialDesignWindow),
            new PropertyMetadata(default(ContentControl)));

        /// <summary>
        ///     Gets or sets the content of the dialog.
        /// </summary>
        /// <value>The content of the dialog.</value>
        public ContentControl DialogContent
        {
            get { return (ContentControl)GetValue(DialogContentProperty); }
            set { SetValue(DialogContentProperty, value); }
        }

        /// <summary>
        ///     The dialog options property
        /// </summary>
        public static readonly DependencyProperty DialogOptionsProperty = DependencyProperty.Register("DialogOptions",
                                                                                                      typeof(DialogSettings), typeof(MaterialDesignWindow),
                                                                                                      new PropertyMetadata(new DialogSettings()));

        /// <summary>
        ///     Gets or sets the dialog options.
        /// </summary>
        /// <value>The dialog options.</value>
        public DialogSettings DialogOptions
        {
            get { return (DialogSettings)GetValue(DialogOptionsProperty); }
            set { SetValue(DialogOptionsProperty, value); }
        }

        /// <summary>
        ///     The is dialog visible property
        /// </summary>
        public static readonly DependencyProperty IsDialogVisibleProperty = DependencyProperty.Register(
            "IsDialogVisible", typeof(bool), typeof(MaterialDesignWindow), new PropertyMetadata(default(bool)));

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is dialog visible.
        /// </summary>
        /// <value><c>true</c> if this instance is dialog visible; otherwise, <c>false</c>.</value>
        public bool IsDialogVisible
        {
            get { return (bool)GetValue(IsDialogVisibleProperty); }
            set { SetValue(IsDialogVisibleProperty, value); }
        }

        #endregion

        #region Status Bar
        /// <summary>
        ///     The status bar items property
        /// </summary>
        public static readonly DependencyProperty StatusBarItemsProperty = DependencyProperty.Register(
            "StatusBarItems", typeof(MaterialStatusBar), typeof(MaterialDesignWindow),
            new PropertyMetadata(default(MaterialStatusBar)));

        /// <summary>
        ///     Gets or sets the status bar items.
        /// </summary>
        /// <value>The status bar items.</value>
        public MaterialStatusBar StatusBarItems
        {
            get { return (MaterialStatusBar)GetValue(StatusBarItemsProperty); }
            set { SetValue(StatusBarItemsProperty, value); }
        }

        #endregion

        #region Navigation Drawer
        /// <summary>
        ///     The navigation drawer button visibility property
        /// </summary>
        public static readonly DependencyProperty NavigationDrawerButtonVisibilityProperty = DependencyProperty.Register
        (
            "NavigationDrawerButtonVisibility", typeof(Visibility), typeof(MaterialDesignWindow),
            new FrameworkPropertyMetadata(default(Visibility),
                                          FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnNavigationDrawerButtonVisibilityPropertyChanged));

        /// <summary>
        ///     Gets or sets the navigation drawer button visibility.
        /// </summary>
        /// <value>The navigation drawer button visibility.</value>
        public Visibility NavigationDrawerButtonVisibility
        {
            get { return (Visibility)GetValue(NavigationDrawerButtonVisibilityProperty); }
            set { SetValue(NavigationDrawerButtonVisibilityProperty, value); }
        }

        private static void OnNavigationDrawerButtonVisibilityPropertyChanged(DependencyObject d,
                                                                              DependencyPropertyChangedEventArgs e)
        {
            var source = (MaterialDesignWindow)d;
            if (e.OldValue == e.NewValue)
                return;
            source.NavigationDrawerButtonVisibility = (Visibility)e.NewValue;
        }

        /// <summary>
        ///     The navigation drawer visibility property
        /// </summary>
        public static readonly DependencyProperty NavigationDrawerVisibilityProperty = DependencyProperty.Register(
            "NavigationDrawerVisibility", typeof(bool), typeof(MaterialDesignWindow),
            new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                          OnNavigationDrawerVisibilityPropertyChanged));

        /// <summary>
        ///     Gets or sets a value indicating whether [navigation drawer visible].
        /// </summary>
        /// <value><c>true</c> if [navigation drawer visible]; otherwise, <c>false</c>.</value>
        public bool NavigationDrawerVisible
        {
            get { return (bool)GetValue(NavigationDrawerVisibilityProperty); }
            set { SetValue(NavigationDrawerVisibilityProperty, value); }
        }

        private static void OnNavigationDrawerVisibilityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = (MaterialDesignWindow)d;
            if (e.OldValue == e.NewValue)
                return;
            source.NavigationDrawerVisible = (bool)e.NewValue;
            source.NavigationDrawer.IsExpanded = source.NavigationDrawerVisible;
        }

        /// <summary>
        ///     The navigation drawer header text property
        /// </summary>
        public static readonly DependencyProperty NavigationDrawerHeaderTextProperty = DependencyProperty.Register(
            "NavigationDrawerHeaderText", typeof(string), typeof(MaterialDesignWindow), new PropertyMetadata(default(string)));

        /// <summary>
        ///     Gets or sets the navigation drawer header text.
        /// </summary>
        /// <value>The navigation drawer header text.</value>
        public string NavigationDrawerHeaderText
        {
            get { return (string)GetValue(NavigationDrawerHeaderTextProperty); }
            set { SetValue(NavigationDrawerHeaderTextProperty, value); }
        }

        /// <summary>
        ///     The navigation drawer header text font color property
        /// </summary>
        public static readonly DependencyProperty NavigationDrawerHeaderTextFontColorProperty = DependencyProperty
            .Register(
                "NavigationDrawerHeaderTextFontColor", typeof(SolidColorBrush), typeof(MaterialDesignWindow),
                new PropertyMetadata(new SolidColorBrush { Color = Color.FromRgb(255, 255, 255) }));

        /// <summary>
        ///     Gets or sets the color of the navigation drawer header text font.
        /// </summary>
        /// <value>The color of the navigation drawer header text font.</value>
        public SolidColorBrush NavigationDrawerHeaderTextFontColor
        {
            get { return (SolidColorBrush)GetValue(NavigationDrawerHeaderTextFontColorProperty); }
            set { SetValue(NavigationDrawerHeaderTextFontColorProperty, value); }
        }

        /// <summary>
        ///     The navigation drawer header content property
        /// </summary>
        public static readonly DependencyProperty NavigationDrawerHeaderContentProperty = DependencyProperty.Register(
            "NavigationDrawerHeaderContent", typeof(ContentControl), typeof(MaterialDesignWindow),
            new PropertyMetadata(default(ContentControl)));

        /// <summary>
        ///     Gets or sets the content of the navigation drawer header.
        /// </summary>
        /// <value>The content of the navigation drawer header.</value>
        public ContentControl NavigationDrawerHeaderContent
        {
            get { return (ContentControl)GetValue(NavigationDrawerHeaderContentProperty); }
            set { SetValue(NavigationDrawerHeaderContentProperty, value); }
        }

        /// <summary>
        ///     The navigation drawer content property
        /// </summary>
        public static readonly DependencyProperty NavigationDrawerContentProperty = DependencyProperty.Register(
            "NavigationDrawerContent", typeof(FrameworkElement), typeof(MaterialDesignWindow),
            new PropertyMetadata(default(FrameworkElement)));

        /// <summary>
        ///     Gets or sets the content of the navigation drawer.
        /// </summary>
        /// <value>The content of the navigation drawer.</value>
        public FrameworkElement NavigationDrawerContent
        {
            get { return (FrameworkElement)GetValue(NavigationDrawerContentProperty); }
            set { SetValue(NavigationDrawerContentProperty, value); }
        }

        /// <summary>
        ///     The navigation drawer width property
        /// </summary>
        public static readonly DependencyProperty NavigationDrawerWidthProperty =
            DependencyProperty.Register("NavigationDrawerWidth", typeof(double), typeof(MaterialDesignWindow),
                                        new FrameworkPropertyMetadata(0.0));

        /// <summary>
        ///     Gets or sets the width of the navigation drawer.
        /// </summary>
        /// <value>The width of the navigation drawer.</value>
        public double NavigationDrawerWidth
        {
            get { return (double)GetValue(NavigationDrawerWidthProperty); }
            set { SetValue(NavigationDrawerWidthProperty, value); }
        }

        /// <summary>
        ///     The navigation drawer header text font size property
        /// </summary>
        public static readonly DependencyProperty NavigationDrawerHeaderTextFontSizeProperty = DependencyProperty
            .Register(
                "NavigationDrawerHeaderTextFontSize", typeof(double), typeof(MaterialDesignWindow), new PropertyMetadata(24.0));

        /// <summary>
        ///     Gets or sets the size of the navigation drawer header text font.
        /// </summary>
        /// <value>The size of the navigation drawer header text font.</value>
        public double NavigationDrawerHeaderTextFontSize
        {
            get { return (double)GetValue(NavigationDrawerHeaderTextFontSizeProperty); }
            set { SetValue(NavigationDrawerHeaderTextFontSizeProperty, value); }
        }

        /// <summary>
        ///     The navigation drawer header text visibility property
        /// </summary>
        public static readonly DependencyProperty NavigationDrawerHeaderTextVisibilityProperty = DependencyProperty
            .Register(
                "NavigationDrawerHeaderTextVisibility", typeof(Visibility), typeof(MaterialDesignWindow),
                new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        ///     Gets or sets the navigation drawer header text visibility.
        /// </summary>
        /// <value>The navigation drawer header text visibility.</value>
        public Visibility NavigationDrawerHeaderTextVisibility
        {
            get { return (Visibility)GetValue(NavigationDrawerHeaderTextVisibilityProperty); }
            set { SetValue(NavigationDrawerHeaderTextVisibilityProperty, value); }
        }

        private void NavigationDrawerToggleButton_MouseUp(object sender, RoutedEventArgs e)
        {
            NavigationDrawerVisible = !NavigationDrawerVisible;
            NavigationDrawer.IsExpanded = NavigationDrawerVisible;
            if (NavigationDrawerVisible)
                ShowWindowShadeAsync();
            else
                HideWindowShadeAsync();
        }

        #endregion

        #region Window Shade
        /// <summary>
        ///     Handles the MouseUp event of the WindowShadeContentControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void WindowShadeContentControl_MouseUp(object sender, RoutedEventArgs e)
        {
            if (IsDialogVisible)
                return;
            NavigationDrawerVisible = !NavigationDrawerVisible;
            NavigationDrawer.IsExpanded = NavigationDrawerVisible;
            if (NavigationDrawerVisible)
                ShowWindowShadeAsync();
            else
                HideWindowShadeAsync();
        }

        /// <summary>
        ///     Determines whether [is window shade visible].
        /// </summary>
        /// <returns><c>true</c> if [is window shade visible]; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.InvalidOperationException">Window Shade Missing!</exception>
        public bool IsWindowShadeVisible()
        {
            if (_windowShadeContentControl == null)
                throw new InvalidOperationException("Window Shade Missing!");
            return _windowShadeContentControl.Visibility == Visibility.Visible &&
                   _windowShadeContentControl.Opacity >= 0.7;
        }

        /// <summary>
        ///     Shows the window shade.
        /// </summary>
        public void ShowWindowShade()
        {
            _windowShadeContentControl.Visibility = Visibility.Visible;
            _windowShadeContentControl.SetCurrentValue(OpacityProperty, 0.7);
        }

        /// <summary>
        ///     Hides the window shade.
        /// </summary>
        public void HideWindowShade()
        {
            _windowShadeContentControl.SetCurrentValue(OpacityProperty, 0.0);
            _windowShadeContentControl.Visibility = Visibility.Hidden;
        }

        /// <summary>
        ///     Shows the window shade asynchronously.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exception cref="System.InvalidOperationException">Window Shade Missing!</exception>
        public Task ShowWindowShadeAsync()
        {
            if (_windowShadeContentControl == null)
                throw new InvalidOperationException("Window Shade Missing!");
            if (IsWindowShadeVisible() && _windowShadeStoryboard == null)
                return new Task(() => { });
            Dispatcher.VerifyAccess();
            _windowShadeContentControl.Visibility = Visibility.Visible;
            var taskCompletionSource = new TaskCompletionSource<object>();
            var sb = (Storyboard)Template.Resources["ShowWindowShadeStoryboard"];
            sb = sb.Clone();
            EventHandler completionHandler = null;
            completionHandler = (sender, args) =>
            {
                sb.Completed -= completionHandler;

                // ReSharper disable once PossibleUnintendedReferenceComparison
                if (_windowShadeStoryboard == sb)
                    _windowShadeStoryboard = null;

                taskCompletionSource.TrySetResult(null);
            };
            sb.Completed += completionHandler;
            _windowShadeContentControl.BeginStoryboard(sb);
            _windowShadeStoryboard = sb;
            return taskCompletionSource.Task;
        }

        /// <summary>
        ///     Hides the window shade asynchronously.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exception cref="System.InvalidOperationException">Window Shade Missing!</exception>
        public Task HideWindowShadeAsync()
        {
            if (_windowShadeContentControl == null)
                throw new InvalidOperationException("Window Shade Missing!");
            if (_windowShadeContentControl.Visibility == Visibility.Visible &&
                _windowShadeContentControl.Opacity.Equals(0.0))
                return new Task(() => { });
            Dispatcher.VerifyAccess();
            var taskCompletionSource = new TaskCompletionSource<object>();
            var sb = (Storyboard)Template.Resources["HideWindowShadeStoryboard"];
            sb = sb.Clone();
            EventHandler completionHandler = null;
            completionHandler = (sender, args) =>
            {
                sb.Completed -= completionHandler;
                // ReSharper disable once PossibleUnintendedReferenceComparison
                if (_windowShadeStoryboard == sb)
                {
                    _windowShadeContentControl.Visibility = Visibility.Hidden;
                    _windowShadeStoryboard = null;
                }
                taskCompletionSource.TrySetResult(null);
            };
            sb.Completed += completionHandler;
            _windowShadeContentControl.BeginStoryboard(sb);
            _windowShadeStoryboard = sb;
            return taskCompletionSource.Task;
        }

        #endregion

        #region Window Commands
        /// <summary>
        ///     Handles the <see cref="E:CanResizeWindow" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CanExecuteRoutedEventArgs" /> instance containing the event data.</param>
        private void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        /// <summary>
        ///     Handles the <see cref="E:CanMinimizeWindow" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CanExecuteRoutedEventArgs" /> instance containing the event data.</param>
        private void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode != ResizeMode.NoResize;
        }

        /// <summary>
        ///     Handles the <see cref="E:CloseWindow" /> event.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs" /> instance containing the event data.</param>
        private void OnCloseWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        /// <summary>
        ///     Handles the <see cref="E:MaximizeWindow" /> event.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs" /> instance containing the event data.</param>
        private void OnMaximizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        /// <summary>
        ///     Handles the <see cref="E:MinimizeWindow" /> event.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs" /> instance containing the event data.</param>
        private void OnMinimizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        /// <summary>
        ///     Handles the <see cref="E:RestoreWindow" /> event.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs" /> instance containing the event data.</param>
        private void OnRestoreWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        #endregion

        #region Window Sizing Fix
        /// <summary>
        ///     The center on primary screen property
        /// </summary>
        public static readonly DependencyProperty CenterOnPrimaryScreenProperty = DependencyProperty.Register(
            "CenterOnPrimaryScreen", typeof(bool), typeof(MaterialDesignWindow), new PropertyMetadata(default(bool)));

        /// <summary>
        ///     Gets or sets a value indicating whether [center on primary screen].
        /// </summary>
        /// <value><c>true</c> if [center on primary screen]; otherwise, <c>false</c>.</value>
        public bool CenterOnPrimaryScreen
        {
            get { return (bool)GetValue(CenterOnPrimaryScreenProperty); }
            set { SetValue(CenterOnPrimaryScreenProperty, value); }
        }

        private static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024: /* WM_GETMINMAXINFO */
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }
            return (IntPtr)0;
        }

        private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            var mmi = (MinMaxInfo)Marshal.PtrToStructure(lParam, typeof(MinMaxInfo));
            // Adjust the maximized size and position to fit the work area of the correct monitor
            var MONITOR_DEFAULTTONEAREST = 0x00000002;
            var monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            if (monitor != IntPtr.Zero)
            {
                var monitorInfo = new MonitorInfo();
                GetMonitorInfo(monitor, monitorInfo);
                var rcWorkArea = monitorInfo.rcWork;
                var rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.Left - rcMonitorArea.Left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.Top - rcMonitorArea.Top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.Right - rcWorkArea.Left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.Bottom - rcWorkArea.Top);
            }
            Marshal.StructureToPtr(mmi, lParam, true);
        }

        void MaterialDesignWindow_SourceInitialized(object sender, EventArgs e)
        {
            var handle = (new WinInterop.WindowInteropHelper(this)).Handle;
            WinInterop.HwndSource.FromHwnd(handle).AddHook(new WinInterop.HwndSourceHook(WindowProc));
        }

        /// <summary>
        ///     POINT aka POINTAPI
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            /// <summary>
            ///     x coordinate of point.
            /// </summary>
            public int x;

            /// <summary>
            ///     y coordinate of point.
            /// </summary>
            public int y;

            /// <summary>
            ///     Construct a point of coordinates (x,y).
            /// </summary>
            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        /// <summary>
        ///     Struct MINMAXINFO
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MinMaxInfo
        {
            /// <summary>
            ///     The pt reserved
            /// </summary>
            public Point ptReserved;

            /// <summary>
            ///     The pt maximum size
            /// </summary>
            public Point ptMaxSize;

            /// <summary>
            ///     The pt maximum position
            /// </summary>
            public Point ptMaxPosition;

            /// <summary>
            ///     The pt minimum track size
            /// </summary>
            public Point ptMinTrackSize;

            /// <summary>
            ///     The pt maximum track size
            /// </summary>
            public Point ptMaxTrackSize;
        };

        void MaterialDesignWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!CenterOnPrimaryScreen)
                return;
            WindowState = WindowState.Normal;
            var setWindowHeight = (SystemParameters.PrimaryScreenHeight * 0.80);
            var setWindowWidth = (SystemParameters.PrimaryScreenWidth * 0.80);
            if (setWindowHeight >= 500)
                Height = (SystemParameters.PrimaryScreenHeight * 0.80);
            if (setWindowWidth >= 1100)
                Width = (SystemParameters.PrimaryScreenWidth * 0.80);
            var topDifference = (SystemParameters.PrimaryScreenHeight - setWindowHeight) / 2;
            var leftDifference = (SystemParameters.PrimaryScreenWidth - setWindowWidth) / 2;
            Top = topDifference;
            Left = leftDifference;
        }

        /// <summary>
        /// The monitor information.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MonitorInfo
        {
            /// <summary>The size. </summary>
            public int cbSize = Marshal.SizeOf(typeof(MonitorInfo));

            /// <summary>The rectangle monitor. </summary>
            public Rect rcMonitor = new Rect();

            /// <summary>The rectangle work. </summary>
            public Rect rcWork = new Rect();

            /// <summary>The flags. </summary>
            public int dwFlags = 0;
        }

        /// <summary>
        /// The rectangle.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct Rect
        {
            /// <summary> Win32 </summary>
            public readonly int Left;

            /// <summary> Win32 </summary>
            public readonly int Top;

            /// <summary> Win32 </summary>
            public readonly int Right;

            /// <summary> Win32 </summary>
            public readonly int Bottom;

            /// <summary> Win32 </summary>
            public static readonly Rect Empty = new Rect();

            /// <summary> Win32 </summary>
            public int Width => Math.Abs(Right - Left);

            /// <summary> Win32 </summary>
            public int Height => Bottom - Top;

            /// <summary> Win32 </summary>
            public Rect(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            /// <summary> Win32 </summary>
            public Rect(Rect rcSrc)
            {
                Left = rcSrc.Left;
                Top = rcSrc.Top;
                Right = rcSrc.Right;
                Bottom = rcSrc.Bottom;
            }

            /// <summary>
            ///     Win32
            /// </summary>
            public bool IsEmpty => Left >= Right || Top >= Bottom;

            /// <summary>
            ///     Return a user friendly representation of this struct
            /// </summary>
            public override string ToString()
            {
                if (this == Empty)
                {
                    return "RECT {Empty}";
                }
                return "RECT { left : " + Left + " / top : " + Top + " / right : " + Right + " / bottom : " + Bottom + " }";
            }

            /// <summary>Tests if this object is considered equal to another. </summary>
            /// <param name="obj">The object to compare to this object. </param>
            /// <returns>True if the objects are considered equal, false if they are not. </returns>
            /// <seealso cref="M:System.ValueType.Equals(object)"/>
            public override bool Equals(object obj)
            {
                if (!(obj is System.Windows.Rect))
                {
                    return false;
                }
                return this == (Rect)obj;
            }

            /// <summary>
            ///     Return the HashCode for this struct (not garanteed to be unique)
            /// </summary>
            public override int GetHashCode()
            {
                return Left.GetHashCode() + Top.GetHashCode() + Right.GetHashCode() + Bottom.GetHashCode();
            }

            /// <summary>
            ///     Determine if 2 RECT are equal (deep compare)
            /// </summary>
            public static bool operator ==(Rect rect1, Rect rect2)
            {
                return (rect1.Left == rect2.Left && rect1.Top == rect2.Top && rect1.Right == rect2.Right &&
                        rect1.Bottom == rect2.Bottom);
            }

            /// <summary>
            ///     Determine if 2 RECT are different(deep compare)
            /// </summary>
            public static bool operator !=(Rect rect1, Rect rect2)
            {
                return !(rect1 == rect2);
            }
        }

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MonitorInfo lpmi);

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        #endregion

        #region Settings
        /// <summary>
        /// The settings panel visibility property
        /// </summary>
        public static readonly DependencyProperty SettingsPanelVisibilityProperty = DependencyProperty.Register(
            "SettingsPanelVisibility", typeof(Visibility), typeof(MaterialDesignWindow), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSettingsPanelVisibilityChanged));

        /// <summary>
        /// Gets or sets the settings panel visibility.
        /// </summary>
        /// <value>The settings panel visibility.</value>
        public Visibility SettingsPanelVisibility
        {
            get { return (Visibility)GetValue(SettingsPanelVisibilityProperty); }
            set { SetValue(SettingsPanelVisibilityProperty, value); }
        }

        private static void OnSettingsPanelVisibilityChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;
            var host = obj as MaterialDesignWindow;
            if (host == null)
                return;
            var vis = (Visibility)e.NewValue;
            host.ShowOrHideSettingsPanel(vis);
        }

        private void ShowOrHideSettingsPanel(Visibility vis)
        {
            if (vis == Visibility.Collapsed)
            {
                HideSettingsPanel();
                HideWindowShadeAsync();
            }
            else
            {
                ShowWindowShadeAsync();
                ShowSettingsPanel();
            }
        }

        private void HideSettingsPanel()
        {
            if (_settingsPanel == null)
                throw new InvalidOperationException("SettingsPanel Missing!");
            if (_settingsPanel.Visibility == Visibility.Visible && _settingsPanel.Opacity.Equals(0.0))
                return;
            Dispatcher.VerifyAccess();
            var taskCompletionSource = new TaskCompletionSource<object>();
            var sb = (Storyboard)Template.Resources["HideSettingsPanelStoryboard"];
            sb = sb.Clone();
            EventHandler completionHandler = null;
            completionHandler = (sender, args) =>
            {
                sb.Completed -= completionHandler;
                // ReSharper disable once PossibleUnintendedReferenceComparison
                if (_settingsPanelStoryboard == sb)
                {
                    _settingsPanel.Visibility = Visibility.Collapsed;
                    _settingsPanelStoryboard = null;
                }
                taskCompletionSource.TrySetResult(null);
            };
            sb.Completed += completionHandler;
            _settingsPanel.BeginStoryboard(sb);
            _settingsPanelStoryboard = sb;
        }

        private void ShowSettingsPanel()
        {
            if (_settingsPanel == null)
                throw new InvalidOperationException("SettingsPanel Missing!");
            if (_settingsPanel.Visibility == Visibility.Collapsed &&
                _settingsPanel.Opacity.Equals(0.0))
                return;
            Dispatcher.VerifyAccess();
            var taskCompletionSource = new TaskCompletionSource<object>();
            var sb = (Storyboard)Template.Resources["ShowSettingsPanelStoryboard"];
            sb = sb.Clone();
            EventHandler completionHandler = null;
            completionHandler = (sender, args) =>
            {
                sb.Completed -= completionHandler;
                // ReSharper disable once PossibleUnintendedReferenceComparison
                if (_settingsPanelStoryboard == sb)
                {
                    _settingsPanel.Visibility = Visibility.Visible;
                    _settingsPanelStoryboard = null;
                }
                taskCompletionSource.TrySetResult(null);
            };
            sb.Completed += completionHandler;
            _settingsPanel.BeginStoryboard(sb);
            _settingsPanelStoryboard = sb;
            return;
        }

        /// <summary>
        /// The settings panel width property
        /// </summary>
        public static readonly DependencyProperty SettingsPanelWidthProperty = DependencyProperty.Register(
            "SettingsPanelWidth", typeof(double), typeof(MaterialDesignWindow), new PropertyMetadata(default(double)));

        /// <summary>
        /// Gets or sets the width of the settings panel.
        /// </summary>
        /// <value>The width of the settings panel.</value>
        public double SettingsPanelWidth
        {
            get { return (double)GetValue(SettingsPanelWidthProperty); }
            set { SetValue(SettingsPanelWidthProperty, value); }
        }

        /// <summary>
        /// The settings panel height property
        /// </summary>
        public static readonly DependencyProperty SettingsPanelHeightProperty = DependencyProperty.Register(
            "SettingsPanelHeight", typeof(double), typeof(MaterialDesignWindow), new PropertyMetadata(default(double)));

        /// <summary>
        /// Gets or sets the height of the settings panel.
        /// </summary>
        /// <value>The height of the settings panel.</value>
        public double SettingsPanelHeight
        {
            get { return (double)GetValue(SettingsPanelHeightProperty); }
            set { SetValue(SettingsPanelHeightProperty, value); }
        }

        /// <summary>
        /// The settings panel content property
        /// </summary>
        public static readonly DependencyProperty SettingsPanelContentProperty = DependencyProperty.Register(
            "SettingsPanelContent", typeof(MaterialSettingsPanel), typeof(MaterialDesignWindow),
            new PropertyMetadata(default(MaterialSettingsPanel)));

        /// <summary>
        /// Gets or sets the content of the settings panel.
        /// </summary>
        /// <value>The content of the settings panel.</value>
        public MaterialSettingsPanel SettingsPanelContent
        {
            get { return (MaterialSettingsPanel)GetValue(SettingsPanelContentProperty); }
            set { SetValue(SettingsPanelContentProperty, value); }
        }

        #endregion
    }
}