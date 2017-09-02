#region
using System.Windows;
using System.Windows.Controls;
using LandmarkDevs.UI.Material.Controls.Windows;
#endregion

namespace LandmarkDevs.UI.Material.Controls
{
    /// <summary>
    ///     Class NavigationDrawerButton.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.Button" />
    public class NavigationDrawerButton : Button
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationDrawerButton"/> class.
        /// </summary>
        public NavigationDrawerButton()
        {
            Click += NavigationDrawerButton_Click;
        }

        private void NavigationDrawerButton_Click(object sender, RoutedEventArgs e)
        {
            var mw = Application.Current.MainWindow as MaterialDesignWindow;
            mw.NavigationDrawer.IsExpanded = false;
            mw.NavigationDrawerVisible = false;
            mw.HideWindowShade();
        }

        /// <summary>
        ///     Initializes static members of the <see cref="NavigationDrawerButton" /> class.
        /// </summary>
        static NavigationDrawerButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationDrawerButton),
                new FrameworkPropertyMetadata(typeof(NavigationDrawerButton)));
        }

        /// <summary>
        ///     The image content control property
        /// </summary>
        public static readonly DependencyProperty ImageContentControlProperty = DependencyProperty.Register(
            "ImageContentControl", typeof(FrameworkElement), typeof(NavigationDrawerButton),
            new PropertyMetadata(default(FrameworkElement)));

        /// <summary>
        ///     Gets or sets the image content control.
        /// </summary>
        /// <value>The image content control.</value>
        public FrameworkElement ImageContentControl
        {
            get { return (FrameworkElement) GetValue(ImageContentControlProperty); }
            set { SetValue(ImageContentControlProperty, value); }
        }

        /// <summary>
        ///     The text property
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(NavigationDrawerButton), new PropertyMetadata(default(string)));

        /// <summary>
        ///     Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}